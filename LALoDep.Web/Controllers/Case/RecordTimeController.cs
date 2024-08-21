using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Case;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using LALoDep.Domain.pd_Role;
using DataTables.Mvc;
using LALoDep.Domain.pd_Code;
using LALoDep.Core.Enums;
using LALoDep.Models;
using LALoDep.Custom;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Work;
using LALoDep.Domain.pd_wt;
using LALoDep.Domain.pd_Note;
using Omu.ValueInjecter;
using LALoDep.Domain.pd_Conflict;
using LALoDep.Domain.rpt_Print;


namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        #region RecordTime List

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeview)]
        public virtual ActionResult RecordTime()
        {
            //If User Coming RFD Wizard  AR Record Time then we need to maintain the wizard steps and its information 
            if (!string.IsNullOrEmpty(Request.QueryString["arId"]))
            {

                var reportFillingDueGet = UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGet_spResult>(
                "pd_HearingReportFilingDueGet_sp", new pd_HearingReportFilingDueGet_spParams
                {
                    BatchLogJobID = Guid.NewGuid(),
                    HearingReportFilingDueID = Request.QueryString["arId"].ToDecrypt().ToInt(),
                    UserID = UserManager.UserExtended.UserID

                }).FirstOrDefault();

                if (reportFillingDueGet != null)
                {

                    ViewBag.Id = Request.QueryString["arId"];
                    ViewBag.HearingReportFilingDue = reportFillingDueGet;
                }
            }

            var viewModel = new RecordTimeViewModel();
            var pd_RoleGetByCaseIDClientCriteria_spParams = new pd_RoleGetByCaseIDClientCriteria_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            viewModel.Workers = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDWorkerCriteria_spResult>("pd_RoleGetByCaseIDWorkerCriteria_sp", pd_RoleGetByCaseIDClientCriteria_spParams).ToList();
            viewModel.Clients = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDClientCriteria_spResult>("pd_RoleGetByCaseIDClientCriteria_sp", pd_RoleGetByCaseIDClientCriteria_spParams).ToList();

            //var pd_HearingReportFilingDueGet_spParams = new pd_HearingReportFilingDueGet_spParams()
            //{
            //    UserID = UserManager.UserExtended.UserID,
            //    BatchLogJobID = Guid.NewGuid()
            //};
            //viewModel.Clients = UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGet_spResult>("pd_HearingReportFilingDueGet_sp", pd_HearingReportFilingDueGet_spParams).ToList();

            return View(viewModel);


        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeview)]
        [HttpPost]
        public virtual PartialViewResult RecordTimeList(RecordTimeViewModel viewModel)
        {
            var pd_WorkGetByCaseID_spParams = new pd_WorkGetByCaseID_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                WorkerPersonID = viewModel.WorkerPersonID,
                ClientPersonID = viewModel.ClientPersonID,
                BatchLogJobID = Guid.NewGuid(),
            };
            var data = UtilityService.ExecStoredProcedureWithResults<pd_WorkGetByCaseID_spResult>("pd_WorkGetByCaseID_sp", pd_WorkGetByCaseID_spParams).ToList();
            //if user come from RFD Wizard-> AR Record Time Page 
            ViewBag.ARID = Request.Form["ARID"];

            return PartialView("_partialRecordTimeList", data);
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeDelete)]
        public virtual JsonResult RecordTimeDelete(string id)
        {
            var pd_WorkDelete_spParams = new pd_WorkDelete_spParams()
            {
                ID = id.ToDecrypt().ToInt(),
                RecordStateID = 10,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "Work",
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_WorkDelete_sp", pd_WorkDelete_spParams).ToList();
            return Json(new { isSuccess = true });
        }


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeview)]
        [HttpPost]
        public virtual ActionResult RecordTimeListPrint(string id)
        {
            var rpt_RecordTimePrintableVersion_spParams = new rpt_RecordTimePrintableVersion_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                WorkIDList = id,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = new Guid()
            };

            ReportClass rpt = new ReportClass();
            try
            {
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/CaseActivityLog.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("dbo.rpt_RecordTimePrintableVersion_sp", rpt_RecordTimePrintableVersion_spParams);
                rpt.SetDataSource(table);
                string fileName = "RecordTimeList_" + UserManager.UserExtended.CaseID + ".pdf";
                if (UserManager.UserExtended.PrintDocumentOn == "NewWindow")
                {


                    var filePath = UtilityFunctions.GetDocumentDownloadFolderPath() + fileName;
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);



                    rpt.Close();
                    rpt.Dispose();
                    GC.Collect();


                    return RedirectToAction("Preview", "Home", new { path = Utility.Encrypt(UtilityFunctions.GetDocumentDownloadFolderRelativePath() + fileName) });
                }
                Stream stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
                rpt.Close();
                rpt.Dispose();
                GC.Collect();

                return File(stream, "application/pdf", fileName);
            }
            catch (Exception ex)
            {



            }
            finally
            {
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
            }
            return Content("Report not generating");
        }

        #endregion RecordTime List

        #region RecordTime Add

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeAdd)]
        public virtual ActionResult RecordTimeAdd()
        {
            if (UserManager.UserExtended.CaseID > 0)
            {
                //If User Coming RFD Wizard  AR Record Time then we need to maintain the wizard steps and its information 
                if (!string.IsNullOrEmpty(Request.QueryString["arId"]))
                {

                    var reportFillingDueGet = UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGet_spResult>(
                    "pd_HearingReportFilingDueGet_sp", new pd_HearingReportFilingDueGet_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        HearingReportFilingDueID = Request.QueryString["arId"].ToDecrypt().ToInt(),
                        UserID = UserManager.UserExtended.UserID

                    }).FirstOrDefault();

                    if (reportFillingDueGet != null)
                    {

                        ViewBag.Id = Request.QueryString["arId"];
                        ViewBag.HearingReportFilingDue = reportFillingDueGet;
                    }
                }

                var viewModel = new RecordTimeAddViewModel();
                if (Request.QueryString["startDate"] != null)
                    viewModel.StartDate = Request.QueryString["startDate"];
              
                viewModel.WorkStartTime = viewModel.WorkEndTime = "08:30 AM";
                var wtRecordTimeGetCases_spParams = new wtRecordTimeGetCases_spParams()
                {
                    ReloadFlag = 1,
                    CaseID = UserManager.UserExtended.CaseID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                viewModel.CaseList = UtilityService.ExecStoredProcedureWithResults<wtRecordTimeGetCases_spResult>("wtRecordTimeGetCases_sp", wtRecordTimeGetCases_spParams).ToList();

                viewModel.Descriptions = UtilityFunctions.CodeGetWorkDescription(agencyId: UserManager.UserExtended.CaseNumberAgencyID,referralId: Request.QueryString["ReferralID"].ToDecrypt().ToInt());
                viewModel.IVeEligibleList = UtilityFunctions.CodeGetWorkIVeEligible(agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                viewModel.Phases = UtilityFunctions.CodeGetByTypeIdAndUserId(600, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                viewModel.ControlType = UtilityFunctions.GetNoteControlType("Case/RecordTimeAdd");
                var defaultPhase = UtilityService.ExecStoredProcedureWithResults<Default_RecordTime_spResult>("Default_RecordTime_sp", new Default_RecordTime_spParams()
                {

                    CaseID = UserManager.UserExtended.CaseID,

                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()

                }).FirstOrDefault();
                viewModel.HoursLabel = "Hours (use tenths for partial hrs)";
                if (defaultPhase != null && defaultPhase.WorkPhaseCodeID.HasValue)
                {
                  
                    if (Request.QueryString["startDate"] == null)
                        viewModel.StartDate = defaultPhase.WorkStartDate;
                    viewModel.EndDate = defaultPhase.WorkEndDate;
                    viewModel.WorkPhaseCodeID = defaultPhase.WorkPhaseCodeID.Value;
                    viewModel.WorkHoursRequiredFlag = defaultPhase.WorkHoursRequiredFlag.HasValue ? defaultPhase.WorkHoursRequiredFlag.Value : 0;
                    viewModel.WorkPhaseRequiredFlag = defaultPhase.WorkPhaseRequiredFlag.HasValue ? defaultPhase.WorkPhaseRequiredFlag.Value : 0;
                    viewModel.RecordTimeNoteSubjectFlag = defaultPhase.RecordTimeNoteSubjectFlag.HasValue ? defaultPhase.RecordTimeNoteSubjectFlag.Value : 0;
                    viewModel.HoursLabel = string.IsNullOrEmpty(defaultPhase.HoursLabel) ? "Hours (use tenths for partial hrs)" : defaultPhase.HoursLabel;
                    viewModel.UseWorkHoursForActivityLog = defaultPhase.UseWorkHoursForActivityLog.Value;
                }
                var pd_RoleGetByCaseIDClient_spParams = new pd_RoleGetByCaseIDClient_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    ReferralID = Request.QueryString["ReferralID"].ToDecrypt().ToInt()
                };
                viewModel.WorkForList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDClient_spResult>("pd_RoleGetByCaseIDClient_sp", pd_RoleGetByCaseIDClient_spParams).ToList();

                var pd_RoleGetByCaseIDBillingWorker_spParams = new pd_RoleGetByCaseIDBillingWorker_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),

                };
                viewModel.StaffOnCaseList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDBillingWorker_spResult>("pd_RoleGetByCaseIDBillingWorker_sp", pd_RoleGetByCaseIDBillingWorker_spParams).ToList();
                viewModel.StaffNotOnCaseList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDBillingWorker_spResult>("pd_RoleGetByCaseIDNewWorkers_sp", pd_RoleGetByCaseIDBillingWorker_spParams).ToList();

                if (viewModel.StaffOnCaseList.Any(x => x.PersonID == UserManager.UserExtended.PersonID))
                {
                    viewModel.StaffOnPersonID = UserManager.UserExtended.PersonID;
                }
                else if (viewModel.StaffOnCaseList.Any(x => x.IsCurrentUserFlag == 1))
                {
                    viewModel.StaffNotOnPersonID = viewModel.StaffOnCaseList.FirstOrDefault(x => x.IsCurrentUserFlag == 1).PersonID;
                }
                if (viewModel.RecordTimeNoteSubjectFlag == 0)
                {
                    viewModel.NoteSubject = "Record Time Note";
                }
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }


        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeAdd)]
        public virtual JsonResult RecordTimeAddSave(RecordTimeAddViewModel viewModel)
        {


            #region

            var personId = 0;
            if (viewModel.StaffOnPersonID.HasValue)
            {
                personId = viewModel.StaffOnPersonID.Value;
            }
            if (viewModel.StaffNotOnPersonID.HasValue)
            {
                personId = viewModel.StaffNotOnPersonID.Value;
            }

            if (!viewModel.WorkStartTime.IsNullOrEmpty() || !viewModel.WorkEndTime.IsNullOrEmpty())
            {


                var oValidation = UtilityService.ExecStoredProcedureWithResults<WorkTimeValidate_spResult>("WorkTimeValidate_sp", new WorkTimeValidate_spParams()
                {

                    PersonID = personId,
                    StartDateTime = viewModel.WorkStartTime.ToDateTime(),
                    EndDateTime = viewModel.WorkEndTime.ToDateTime(),

                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),

                }).FirstOrDefault();
                if (oValidation != null)
                {
                    if (!oValidation.ValidationMessage.IsNullOrEmpty())
                    {
                        return Json(new { isSuccess = false, Message = oValidation.ValidationMessage });
                    }
                }
            }
            if (viewModel.StaffNotOnPersonID.HasValue)
            {
                var pd_RoleInsert_spParams = new pd_RoleInsert_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    PersonID = (viewModel.StaffOnPersonID.HasValue) ? viewModel.StaffOnPersonID.Value : viewModel.StaffNotOnPersonID.Value,
                    RoleTypeCodeID = viewModel.RoleTypeCodeID,
                    RoleClient = 0,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                };
                if (!string.IsNullOrEmpty(viewModel.StartDate))
                    pd_RoleInsert_spParams.RoleStartDate = viewModel.StartDate.ToDateTime();
                else
                    pd_RoleInsert_spParams.RoleStartDate = DateTime.Now;
                if (!string.IsNullOrEmpty(viewModel.EndDate))
                    pd_RoleInsert_spParams.RoleEndDate = viewModel.EndDate.ToDateTime();
                else
                    pd_RoleInsert_spParams.RoleEndDate = DateTime.Now;
                var roleId = UtilityService.ExecStoredProcedureWithResults<int>("pd_RoleInsert_sp", pd_RoleInsert_spParams).FirstOrDefault();
            }



            var pd_WorkInsert1_spParams = new pd_WorkInsert1_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                PersonID = personId,
                WorkHours = viewModel.WorkHours,
                WorkMileage = viewModel.WorkMileage,
                WorkStartDate = viewModel.StartDate.ToDateTime(),
                WorkDescriptionCodeID = viewModel.WorkDescriptionCodeID,
                WorkPhaseCodeID = viewModel.WorkPhaseCodeID,
                RecordStateID = 1,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                WorkIVeEligibleCodeID = viewModel.WorkIVeEligibleCodeID,
            };
            if (!string.IsNullOrEmpty(viewModel.StartDate))
                pd_WorkInsert1_spParams.WorkStartDate = viewModel.StartDate.ToDateTime();
            else
                pd_WorkInsert1_spParams.WorkStartDate = DateTime.Now;
            if (!string.IsNullOrEmpty(viewModel.EndDate))
                pd_WorkInsert1_spParams.WorkEndDate = viewModel.EndDate.ToDateTime();
            else
                pd_WorkInsert1_spParams.WorkEndDate = pd_WorkInsert1_spParams.WorkStartDate;

            var workID = UtilityService.ExecStoredProcedureWithResults<decimal>("pd_WorkInsert1_sp", pd_WorkInsert1_spParams).FirstOrDefault();
            if (!string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var noteID = UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", new pd_NoteInsert_spParams
                {
                    NoteEntitySystemValueTypeID = 152,
                    NoteEntityTypeSystemValueTypeID = 123,
                    EntityPrimaryKeyID = (int)workID,
                    NoteTypeCodeID = 16,
                    NoteSubject = viewModel.NoteSubject,
                    NoteEntry = viewModel.NoteEntry,
                    CaseID = UserManager.UserExtended.CaseID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                }).FirstOrDefault();
            }
            #endregion
            foreach (var item in viewModel.WorkForList)
            {

                var workRoleID = UtilityService.ExecStoredProcedureWithResults<object>("pd_WorkRoleInsert_sp", new pd_WorkRoleInsert_spParams
                {
                    WorkID = (int)workID,
                    RoleID = item.RoleID.Value,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                }).FirstOrDefault();

            }
            
            if (viewModel.WorkTimeVisibleFlag == 1 && !viewModel.WorkStartTime.IsNullOrEmpty() && !viewModel.WorkEndTime.IsNullOrEmpty())
            {
                UtilityService.ExecStoredProcedureForDataTableADO("WorkTimeIUD_sp", new WorkTimeIUD_spParams()
                {

                    IUD = "INSERT",
                    WorkTimeStart = viewModel.WorkStartTime.ToDateTime(),
                    WorkTimeEnd = viewModel.WorkEndTime.ToDateTime(),
                    WorkID = (int)workID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),

                });
            }
            if (!viewModel.FromZipCode.IsNullOrEmpty() && !viewModel.ToZipCode.IsNullOrEmpty())
            {


                UtilityService.ExecStoredProcedureForDataTableADO("WorkZipCodeIUD_sp", new WorkZipCodeIUD_spParams()
                {

                    IUD = "INSERT",
                    WorkZipCodeFrom = viewModel.FromZipCode,
                    WorkZipCodeTo = viewModel.ToZipCode,
                    WorkID = (int)workID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),

                });
            }
            var arIdQueryString = "";
            if (Request.QueryString["arId"] != null)
            {
                arIdQueryString = "?arId=" + Request.QueryString["arId"];
                if (!string.IsNullOrEmpty(viewModel.StartDate) && viewModel.ButtonID == 3)
                {
                    arIdQueryString += "&startDate=" + viewModel.StartDate;
                }
            }
            else if (!string.IsNullOrEmpty(viewModel.StartDate) && viewModel.ButtonID == 3)
            {
                arIdQueryString = "?startDate=" + viewModel.StartDate;
            }


            string URL = string.Empty;
            if (!viewModel.NextCase.IsNullOrEmpty() || viewModel.NextCaseID > 0)
            {
                if (viewModel.NextCaseID > 0)
                {
                    UserManager.UpdateCaseStatusBar(viewModel.NextCaseID);
                    URL = MVC.Case.Name + "/" + MVC.Case.ActionNames.RecordTimeAdd+ arIdQueryString;
                }
                else if (!viewModel.NextCase.IsNullOrEmpty())
                {

                    var oCase = UtilityService.ExecStoredProcedureWithResults<LALoDep.Domain.pd_CaseSearch_sp_Result>("pd_CaseSearch_sp", new pd_CaseSearch_spParams
                    {
                        DocketNumber = viewModel.NextCase,
                        Range = 100,
                        StartRecord = 1,
                        Appointment = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                    }).FirstOrDefault();
                    if (oCase != null)
                    {
                        UserManager.UpdateCaseStatusBar(oCase.CaseID);
                        URL = MVC.Case.Name + "/" + MVC.Case.ActionNames.RecordTimeAdd+ arIdQueryString;
                    }
                }
            }



            if (URL.IsNullOrEmpty())
            {
                switch (viewModel.ButtonID)
                {
                    case 1:
                        URL = MVC.Case.Name + "/" + MVC.Case.ActionNames.Main;
                        break;
                    case 2:
                        URL = MVC.Case.Name + "/" + MVC.Case.ActionNames.RecordTime + arIdQueryString;
                        break;
                    case 3:
                        URL = MVC.Case.Name + "/" + MVC.Case.ActionNames.RecordTimeAdd + arIdQueryString;
                        break;
                    default:
                        break;
                }
            }

            return Json(new { isSuccess = true, URL = URL });
        }

        #endregion RecordTime Add

        #region RecordTime Edit
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeEdit)]
        public virtual ActionResult RecordTimeEdit(string id)
        {
            if (Request.QueryString["CaseID"] != null)
            {
                Custom.UserEnvironment.UserManager.UpdateCaseStatusBar(Request.QueryString["CaseID"].ToDecrypt().ToInt());
            }
            int workId = id.ToDecrypt().ToInt();
            if (UserManager.UserExtended.CaseID > 0 && workId > 0)
            {
                //If User Coming RFD Wizard  AR Record Time then we need to maintain the wizard steps and its information 
                if (!string.IsNullOrEmpty(Request.QueryString["arId"]))
                {
                    var reportFillingDueGet = UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGet_spResult>("pd_HearingReportFilingDueGet_sp", new pd_HearingReportFilingDueGet_spParams { BatchLogJobID = Guid.NewGuid(), HearingReportFilingDueID = Request.QueryString["arId"].ToDecrypt().ToInt(), UserID = UserManager.UserExtended.UserID }).FirstOrDefault();
                    if (reportFillingDueGet != null)
                    {
                        ViewBag.Id = Request.QueryString["arId"];
                        ViewBag.HearingReportFilingDue = reportFillingDueGet;
                    }
                }

                var viewModel = new RecordTimeEditViewModel();
                var pd_WorkGet_spParams = new pd_WorkGet_spParams
                {
                    WorkID = workId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };

                var work = UtilityService.ExecStoredProcedureWithResults<pd_WorkGet_spResult>("pd_WorkGet_sp", pd_WorkGet_spParams).FirstOrDefault();
                if (work != null)
                {
                    viewModel.InjectFrom(work);
                    viewModel.WorkStartDate = work.WorkStartDate.HasValue ? work.WorkStartDate.ToShortDateString() : string.Empty;
                    viewModel.WorkEndDate = work.WorkEndDate.HasValue ? work.WorkEndDate.ToShortDateString() : string.Empty;
                    viewModel.WorkStartTime = work.WorkTimeStart.HasValue ? work.WorkTimeStart.ToString() : "08:30 AM";
                    viewModel.WorkEndTime = work.WorkTimeEnd.HasValue ? work.WorkTimeEnd.ToString() : "08:30 AM";
                    viewModel.WorkTimeID = work.WorkTimeID.ToInt();
                    viewModel.Phases = UtilityFunctions.CodeGetByTypeIdAndUserId(600, includeCodeId: viewModel.WorkPhaseCodeID ?? 0, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                    viewModel.Descriptions = UtilityFunctions.CodeGetWorkDescription(workDescriptionCodeId: viewModel.WorkDescriptionCodeID, workId: work.WorkID, agencyId: work.AgencyID ?? UserManager.UserExtended.CaseNumberAgencyID);
                    viewModel.IVeEligibleList = UtilityFunctions.CodeGetWorkIVeEligible(workIVeEligibleCodeId: viewModel.WorkIVeEligibleCodeID, workId: work.WorkID, agencyId: work.AgencyID ?? UserManager.UserExtended.CaseNumberAgencyID);
                    viewModel.WorkZipCodeID = work.WorkZipCodeID;
                    viewModel.FromZipCode = work.WorkZipCodeFrom;
                    viewModel.ToZipCode = work.WorkZipCodeTo;
                    // Worker list
                    var pd_RoleGetByCaseIDBillingWorker_spParams = new pd_RoleGetByCaseIDBillingWorker_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    viewModel.WorkerList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDBillingWorker_spResult>("pd_RoleGetByCaseIDBillingWorker_sp", pd_RoleGetByCaseIDBillingWorker_spParams).ToList();

                    var workNoteParams = new pd_NoteGetByEntity_spParams
                    {
                        EntityPrimaryKeyID = workId,
                        EntityCodeSystemValueTypeID = 152,
                        EntityCodeTypeSystemValueTypeID = 123,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };

                    // Note
                    var workNoteResult = UtilityService.ExecStoredProcedureWithResults<pd_NoteGetByEntity_spResult>("pd_NoteGetByEntity_sp", workNoteParams).FirstOrDefault();
                    if (workNoteResult != null)
                    {
                        viewModel.NoteEntry = workNoteResult.NoteEntry;
                        viewModel.NoteID = workNoteResult.NoteID ?? 0;
                        viewModel.NoteAgencyID = workNoteResult.AgencyID;
                        viewModel.NoteEntityCodeID = workNoteResult.NoteEntityCodeID;
                        viewModel.NoteEntityTypeCodeID = workNoteResult.NoteEntityTypeCodeID;
                        viewModel.NoteEntityPrimaryKeyID = workNoteResult.EntityPrimaryKeyID;
                        viewModel.NoteTypeCodeID = workNoteResult.NoteTypeCodeID;
                        viewModel.NoteSubject = workNoteResult.NoteSubject;
                        viewModel.NoteCaseID = workNoteResult.CaseID;
                        viewModel.NotePetitionID = workNoteResult.PetitionID;
                        viewModel.NoteHearingID = workNoteResult.HearingID;
                        viewModel.NoteRecordStateID = workNoteResult.RecordStateID;
                    }
                    viewModel.ControlType = UtilityFunctions.GetNoteControlType("Case/RecordTimeEdit", noteId: viewModel.NoteID);

                    // Work for
                    viewModel.WorkForList = UtilityService.ExecStoredProcedureWithResults<pd_WorkRoleGetByWorkID_spResult>("pd_WorkRoleGetByWorkID_sp", new pd_WorkRoleGetByWorkID_spParams
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        WorkID = workId,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).ToList();
                    var defaultPhase = UtilityService.ExecStoredProcedureWithResults<Default_RecordTime_spResult>("Default_RecordTime_sp", new Default_RecordTime_spParams()
                    {

                        CaseID = UserManager.UserExtended.CaseID,

                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()

                    }).FirstOrDefault();
                    if (defaultPhase != null && defaultPhase.WorkPhaseCodeID.HasValue)
                    {
                        viewModel.RecordTimeNoteSubjectFlag = defaultPhase.RecordTimeNoteSubjectFlag.HasValue ? defaultPhase.RecordTimeNoteSubjectFlag.Value : 0;
                        viewModel.UseWorkHoursForActivityLog = defaultPhase.UseWorkHoursForActivityLog.Value;
                        viewModel.WorkPhaseRequiredFlag = defaultPhase.WorkPhaseRequiredFlag.HasValue ? defaultPhase.WorkPhaseRequiredFlag.Value : 0;

                        viewModel.WorkHoursRequiredFlag = defaultPhase.WorkHoursRequiredFlag.HasValue ? defaultPhase.WorkHoursRequiredFlag.Value : 0;
                    }
                    if (viewModel.RecordTimeNoteSubjectFlag == 0)
                    {
                        if (viewModel.NoteSubject.IsNullOrEmpty())
                            viewModel.NoteSubject = "Record Time Note";
                    }
                    return View(viewModel);
                }
            }

            return RedirectToAction("AccessDenied", "Home");

        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.RecordTimePage, PageSecurityItemID = SecurityToken.RecordTimeEdit)]
        [HttpPost]
        public virtual JsonResult RecordTimeEditSave(RecordTimeEditViewModel viewModel, RecordTimeEditViewModel oldViewModel)
        {
            if (!viewModel.WorkStartTime.IsNullOrEmpty() || !viewModel.WorkEndTime.IsNullOrEmpty())
            {


                var oValidation = UtilityService.ExecStoredProcedureWithResults<WorkTimeValidate_spResult>("WorkTimeValidate_sp", new WorkTimeValidate_spParams()
                {

                    PersonID = viewModel.PersonID,
                    StartDateTime = viewModel.WorkStartTime.ToDateTime(),
                    EndDateTime = viewModel.WorkEndTime.ToDateTime(),
                    WorkID = viewModel.WorkID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),

                }).FirstOrDefault();
                if (oValidation != null)
                {
                    if (!oValidation.ValidationMessage.IsNullOrEmpty())
                    {
                        return Json(new { isSuccess = false, Message = oValidation.ValidationMessage });
                    }
                }
            }
            if (!viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var noteID = UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", new pd_NoteInsert_spParams
                {
                    NoteEntitySystemValueTypeID = 152,
                    NoteEntityTypeSystemValueTypeID = 123,
                    EntityPrimaryKeyID = viewModel.WorkID,
                    NoteTypeCodeID = 16,
                    NoteSubject = viewModel.NoteSubject,
                    NoteEntry = viewModel.NoteEntry,
                    CaseID = UserManager.UserExtended.CaseID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                }).FirstOrDefault();

            }
            else if (viewModel.NoteID.HasValue && ((!string.IsNullOrEmpty(viewModel.NoteEntry) && viewModel.NoteEntry != oldViewModel.NoteEntry) || viewModel.NoteSubject != oldViewModel.NoteSubject))
            {

                // Update Note
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteUpdate_sp", new pd_NoteUpdate_spParams
                {
                    NoteID = viewModel.NoteID,
                    AgencyID = viewModel.NoteAgencyID,
                    NoteEntityCodeID = viewModel.NoteEntityCodeID,
                    NoteEntityTypeCodeID = viewModel.NoteEntityTypeCodeID,
                    EntityPrimaryKeyID = viewModel.NoteEntityPrimaryKeyID,
                    NoteTypeCodeID = viewModel.NoteTypeCodeID,
                    NoteSubject = viewModel.NoteSubject,
                    NoteEntry = viewModel.NoteEntry,
                    CaseID = UserManager.UserExtended.CaseID,
                    PetitionID = viewModel.NotePetitionID,
                    HearingID = viewModel.NoteHearingID,
                    RecordStateID = viewModel.NoteRecordStateID,
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();
            }
            else if (viewModel.NoteID.HasValue && string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var pd_NoteDelete_spParams = new pd_NoteDelete_spParams()
                {
                    RecordStateID = 10,
                    ID = viewModel.NoteID.Value,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    LoadOption = "Note"
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteDelete_sp", pd_NoteDelete_spParams).ToList();
            }
            //if any data changed then update.
            if (viewModel.PersonID != oldViewModel.PersonID || viewModel.WorkHours != oldViewModel.WorkHours
                || viewModel.WorkHoursOverTime != oldViewModel.WorkHoursOverTime || viewModel.WorkMileage != oldViewModel.WorkMileage
                || viewModel.WorkDescriptionCodeID != oldViewModel.WorkDescriptionCodeID || viewModel.WorkPhaseCodeID != oldViewModel.WorkPhaseCodeID
                || viewModel.WorkStartDate != oldViewModel.WorkStartDate || viewModel.WorkEndDate != oldViewModel.WorkEndDate || viewModel.WorkIVeEligibleCodeID != oldViewModel.WorkIVeEligibleCodeID)
            {
                // Update Work time
                var pd_WorkUpdate1_spParams = new pd_WorkUpdate1_spParams
                {
                    WorkID = viewModel.WorkID,
                    AgencyID = viewModel.AgencyID,
                    CaseID = UserManager.UserExtended.CaseID,
                    PersonID = viewModel.PersonID,
                    WorkHours = viewModel.WorkHours,
                    WorkHoursOverTime = viewModel.WorkHoursOverTime,
                    WorkMileage = viewModel.WorkMileage,
                    WorkDescriptionCodeID = viewModel.WorkDescriptionCodeID,
                    RecordStateID = viewModel.RecordStateID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    WorkPhaseCodeID = viewModel.WorkPhaseCodeID,
                    HearingID = viewModel.HearingID.ToInt() <= 0 ? -1 : viewModel.HearingID.ToInt(),
                    WorkIVeEligibleCodeID = viewModel.WorkIVeEligibleCodeID
                };

                if (!string.IsNullOrEmpty(viewModel.WorkStartDate))
                    pd_WorkUpdate1_spParams.WorkStartDate = Convert.ToDateTime(viewModel.WorkStartDate);

                if (!string.IsNullOrEmpty(viewModel.WorkEndDate))
                    pd_WorkUpdate1_spParams.WorkEndDate = Convert.ToDateTime(viewModel.WorkEndDate);

                UtilityService.ExecStoredProcedureWithResults<object>("pd_WorkUpdate1_sp", pd_WorkUpdate1_spParams).FirstOrDefault();
            }
            foreach (var item in viewModel.WorkForList)
            {
                if (item.WorkRoleID.ToInt() <= 0)
                {
                    var workRoleID = UtilityService.ExecStoredProcedureWithResults<object>("pd_WorkRoleInsert_sp", new pd_WorkRoleInsert_spParams
                    {
                        WorkID = viewModel.WorkID,
                        RoleID = item.RoleID.Value,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                    }).FirstOrDefault();
                }




            }
            foreach (var item in viewModel.DeleteWorkForList)
            {
                if (item.WorkRoleID.ToInt() > 0)
                {
                    UtilityService.ExecStoredProcedureWithoutResults("pd_WorkRoleDelete_sp", new pd_Delete_spParams
                    {
                        ID = item.WorkRoleID.Value,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                    });
                }

            }
            if (!viewModel.WorkStartTime.IsNullOrEmpty() && !viewModel.WorkEndTime.IsNullOrEmpty())
            {
                if (viewModel.WorkTimeID > 0)
                {
                    UtilityService.ExecStoredProcedureForDataTableADO("WorkTimeIUD_sp", new WorkTimeIUD_spParams()
                    {

                        IUD = "UPDATE",
                        WorkTimeStart = viewModel.WorkStartTime.ToDateTime(),
                        WorkTimeEnd = viewModel.WorkEndTime.ToDateTime(),
                        WorkID = viewModel.WorkID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        WorkTimeID = viewModel.WorkTimeID
                    });
                }
                else
                {
                    UtilityService.ExecStoredProcedureForDataTableADO("WorkTimeIUD_sp", new WorkTimeIUD_spParams()
                    {

                        IUD = "INSERT",
                        WorkTimeStart = viewModel.WorkStartTime.ToDateTime(),
                        WorkTimeEnd = viewModel.WorkEndTime.ToDateTime(),
                        WorkID = viewModel.WorkID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        WorkTimeID = viewModel.WorkTimeID
                    });
                }

            }
            else if (viewModel.WorkTimeID > 0)
            {
                UtilityService.ExecStoredProcedureForDataTableADO("WorkTimeIUD_sp", new WorkTimeIUD_spParams()
                {

                    IUD = "DELETE",

                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    WorkTimeID = viewModel.WorkTimeID
                });
            }


            if (!viewModel.FromZipCode.IsNullOrEmpty() && !viewModel.ToZipCode.IsNullOrEmpty())
            {
                if (viewModel.WorkZipCodeID > 0)
                {
                    UtilityService.ExecStoredProcedureForDataTableADO("WorkZipCodeIUD_sp", new WorkZipCodeIUD_spParams()
                    {
                        WorkZipCodeID = viewModel.WorkZipCodeID,
                        IUD = "UPDATE",
                        WorkZipCodeFrom = viewModel.FromZipCode,
                        WorkZipCodeTo = viewModel.ToZipCode,
                        WorkID = viewModel.WorkID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),

                    });
                }
                else
                {
                    UtilityService.ExecStoredProcedureForDataTableADO("WorkZipCodeIUD_sp", new WorkZipCodeIUD_spParams()
                    {

                        IUD = "INSERT",
                        WorkZipCodeFrom = viewModel.FromZipCode,
                        WorkZipCodeTo = viewModel.ToZipCode,
                        WorkID = viewModel.WorkID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),

                    });
                }


            }
            else if (viewModel.WorkZipCodeID > 0)
            {
                UtilityService.ExecStoredProcedureForDataTableADO("WorkZipCodeIUD_sp", new WorkZipCodeIUD_spParams()
                {
                    WorkZipCodeID = viewModel.WorkZipCodeID,
                    IUD = "DELETE",
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                });
            }


            var arIdQueryString = "";
            if (Request.QueryString["arId"] != null)
            {
                arIdQueryString = "?arId=" + Request.QueryString["arId"];
            }
            string URL = Url.Action(MVC.Case.RecordTime()) + arIdQueryString;
            return Json(new { isSuccess = true, URL = URL });
        }
        #endregion RecordTime Edit

    }
}