using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using LALoDep.Domain.com_Report;
using LALoDep.Domain.pd_CodeTables;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.PD_PDAction;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.CaseOpening;
using pd_CodeGetByTypeIDAndUserID_spParams = LALoDep.Domain.pd_Code.pd_CodeGetByTypeIDAndUserID_spParams;
using System.IO;
using LALoDep.Domain.pd_Profile;

namespace LALoDep.Controllers.CaseOpening
{
    public partial class CaseOpeningController : Controller
    {
        #region ActionRequest

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningActionRequest, IsCasePage = true,
            PageSecurityItemID = SecurityToken.ViewActionRequest)]
        public virtual ActionResult ActionRequest()
        {
            var model = new ActionRequestModel
            {
                ActionRequestList =
                    UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGetByCaseID_spResults>(
                        "pd_HearingReportFilingDueGetByCaseID_sp",
                        new pd_HearingReportFilingDueGetByCaseID_spParams()
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        }).ToList()


            };
            if (Request.QueryString["dataentry"] != null)
            {
                foreach (var item in model.ActionRequestList)
                {
                    if (item.HearingReportFilingDueID != null)
                    {
                        var profile = UtilityService.ExecStoredProcedureWithResults<pd_ProfileGetList_spResult>("pd_ProfileGetList_sp",
                            new pd_ProfileGetList_spParams
                            {
                                CaseID = UserManager.UserExtended.CaseID,
                                UserID = UserManager.UserExtended.UserID,
                                RFDID = item.HearingReportFilingDueID.Value,
                                BatchLogJobID = Guid.NewGuid()
                            }).ToList();
                        model.ProfileList.AddRange(profile);

                    }
                }
            }


            return View(model);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningActionRequest, IsCasePage = true)]
        [HttpPost]

        public virtual ActionResult ActionRequestDelete(int id)
        {
            if (!UserManager.IsUserAccessTo(SecurityToken.DeleteActionRequest))
            {
                return Json(new { Status = "Fail", URL = MVC.Home.Name + "/" + MVC.Home.ActionNames.AccessDenied });
            }



            UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingReportFilingDueDelete_sp",
                new pd_HearingReportFilingDueDelete_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    ID = id,

                    RecordTimeStamp = null
                });



            return Json(new { Status = "Done" });
        }

        [HttpPost]

        public virtual ActionResult ActionRequestPrint(string id)
        {
            var comReportSourceGetByReportIdSpParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 74,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };

            #region Delete Report Parameter

            var dictionaryParam = new Dictionary<string, object>()
            {
                {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
            };

            /*Delete Existing Report Parameters saved for this User*/
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueDelete_sp", dictionaryParam);
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterHeaderDelete_sp", dictionaryParam);

            #endregion


            #region Insert Report Parameter
            /*Insert New Report Parameters saved for this User*/

            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueInsert_sp",
                new Dictionary<string, object>()
                {
                    {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                    {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                    {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
                    ,
                    {"@ReportParameterValueID", null},
                    {"@Sequence", 1}
                    ,
                    {"@Value", id.ToDecrypt().ToInt()}
                });


            #endregion
            var spResult =
              UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>(
                  "com_ReportSourceGetByReportID_sp", comReportSourceGetByReportIdSpParams).ToList();


            var rpt = new ReportClass
            {
                FileName = HttpContext.Server.MapPath("~/Reports/" + spResult[0].ReportSourceDocumentName)
            };
            try
            {
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable(spResult[0].ReportSourceStoredProcedureName.Replace(".dbo", ""),
                    comReportSourceGetByReportIdSpParams);
                rpt.SetDataSource(table);

                foreach (var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != spResult[0].ReportSourceDocumentName))
                {
                    rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(
                        UtilityService.ExecStoredProcedureForDataTable(subRptDt.ReportSourceStoredProcedureName.Replace(".dbo", ""),
                            comReportSourceGetByReportIdSpParams));
                }

                var filename = spResult[0].ReportDisplayName + comReportSourceGetByReportIdSpParams.ReportID.ToEncrypt() + ".pdf";

                //if file already exists then delete it

                if (UserManager.UserExtended.PrintDocumentOn == "NewWindow")
                {


                    var filePath = UtilityFunctions.GetDocumentDownloadFolderPath() + filename;
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);



                    rpt.Close();
                    rpt.Dispose();
                    GC.Collect();

                    return RedirectToAction("Preview", "Home", new { path = Utility.Encrypt(UtilityFunctions.GetDocumentDownloadFolderRelativePath() + filename) });
                }


                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
                return File(stream, "application/pdf", filename);

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
        #endregion

        #region ActionRequestAdd

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningActionRequestAdd, IsCasePage = true,
            PageSecurityItemID = SecurityToken.AddActionRequest)]
        public virtual ActionResult ActionRequestAdd()
        {
            var model = new ActionRequestAddModel
            {
                RequestDate = DateTime.Now.ToString("d"),
                HearingList =
                    UtilityService.ExecStoredProcedureWithResults<pd_HearingGetByCaseIDRFD_spResult>(
                        "pd_HearingGetByCaseIDRFD_sp",
                        new pd_HearingGetByCaseIDRFD_spParams()
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        }).Select(o => new SelectListItem() { Text = o.HearingDateTime.ToString() + " " + o.Type, Value = o.HearingID.ToString() }).ToList()
                        ,
                RequestTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResult>("pd_CodeGetByTypeIDAndUserID_sp", new pd_CodeGetByTypeIDAndUserID_spParams
                {

                    CodeTypeID = 21,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID


                }).ToList(),
                RequestByList = UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGetRequestedBy_spResult>(
                        "pd_HearingReportFilingDueGetRequestedBy_sp",
                        new pd_HearingReportFilingDueGetRequestedBy_spParams()
                        {
                            CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        }).Select(o => new SelectListItem() { Text = o.PersonNameDisplay, Value = o.PersonID.ToString(), Selected = o.Current == 1 }).ToList(),
                RequestForList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetARRequestFor_spResult>(
                    "pd_RoleGetARRequestFor_sp",
                    new pd_RoleGetARRequestFor_spParams()
                    {
                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),

                    }).Select(o => new SelectListItem() { Text = o.DisplayName, Value = o.PersonID.ToString() }).ToList()
                        ,
                LegalResearchTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(63, userShortValue: true, agencyId: UserManager.UserExtended.CaseNumberAgencyID),
                RequestList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResult>("pd_CodeGetByTypeIDAndUserID_sp", new pd_CodeGetByTypeIDAndUserID_spParams
                {

                    CodeTypeID = 64,

                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID


                }).ToList(),
                ClientAddressList = UtilityService.ExecStoredProcedureForDataTable("pd_RFDRoleGetByReportFilingDueID_sp", new pd_RFDRoleGetByReportFilingDueID_spParams
                {
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    CaseID = UserManager.UserExtended.CaseID

                })
            };
            model.ControlType = UtilityFunctions.GetNoteControlType("CaseOpening/ActionRequestAdd");
            var current = model.RequestByList.FirstOrDefault(x => x.Selected == true);
            if (current != null)
                model.RequestByID = current.Value.ToInt();

            foreach (var item in model.RequestTypeList)
            {
                if (item.CodeEnumName == "21_RPT/INV")
                {
                    model.RequestTypeID = item.CodeID;
                    break;
                }
            }

            return View(model);
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePageOnlyCheck = true, ReturnJsonResponseOnRequestFaild = true)]
        public virtual ActionResult ActionRequestAdd(ActionRequestAddModel model)
        {

            if (!model.ForceCreateARAnyway)
            {
                var arAssignmentCheckResult = UtilityService.ExecStoredProcedureWithResults<sup_ARAssignmentConflicts_spResult>(
                              "sup_ARAssignmentConflicts_sp",
                              new sup_ARAssignmentConflicts_spParams()
                              {
                                  CaseID = UserManager.UserExtended.CaseID,
                                  PersonID = model.RequestForID,
                                  UserID = UserManager.UserExtended.UserID,
                              }).FirstOrDefault();
                if (arAssignmentCheckResult != null)
                {
                    return Json(new { Status = "AssignmentFail", ErrorMessage = arAssignmentCheckResult.Comment, DialogType = arAssignmentCheckResult.DialogType });
                }

            }
          

            var insertedId = UtilityService.ExecStoredProcedureWithResults<int>("pd_HearingReportFilingDueInsert_sp",
                    new pd_HearingReportFilingDueInsert_spParams()
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        HearingReportFilingDueDate = model.DueDate.ToDateTime(),
                        HearingReportFilingDueOrderDate = model.RequestDate.ToDateTime(),
                        HearingReportFilingDueLegalResearchTypeCodeID = model.LegalResearchTypeID,
                        HearingReportFilingDueTypeCodeID = model.RequestTypeID,
                        RequestedByPersonID = model.RequestByID,
                        RequestedForPersonID = model.RequestForID,
                        RecordStateID = 1,
                        HearingID = model.HearingID > 0 ? model.HearingID : (int?)null

                    }).FirstOrDefault();

            if (!string.IsNullOrEmpty(model.ClientAddressIds))
            {
                var ids = model.ClientAddressIds.Split(',');
                foreach (var id in ids)
                {
                    var rfRoleid = UtilityService.ExecStoredProcedureScalar("pd_RFDRoleInsert_sp",
                         new pd_RFDRoleInsert_spParams()
                         {
                             BatchLogJobID = Guid.NewGuid(),

                             UserID = UserManager.UserExtended.UserID,

                             RecordStateID = 1,
                             ReportFilingDueID = insertedId,
                             RoleID = id.ToInt()

                         });


                    if (!string.IsNullOrEmpty(model.RequestItemIds))
                    {
                        var requestItemIds = model.RequestItemIds.Split(',');
                        foreach (var requestItemId in requestItemIds)
                        {


                            var contactid = UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_RFDRoleContactInsert_sp",
                                    new pd_RFDRoleContactInsert_spParams()
                                    {
                                        BatchLogJobID = Guid.NewGuid(),

                                        UserID = UserManager.UserExtended.UserID,

                                        RecordStateID = 1,
                                        RFDRoleID = (int)rfRoleid,
                                        RFDRoleContactTypeCodeID = requestItemId.ToInt(),

                                    }).FirstOrDefault();

                        }
                    }


                }


            }
            if (!string.IsNullOrEmpty(model.RequestNote))
            {
                UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_NoteInsert_sp", new pd_NoteInsert_spParams()
                {
                    NoteEntitySystemValueTypeID = 117,
                    NoteEntityTypeSystemValueTypeID = 123,
                    EntityPrimaryKeyID = insertedId,
                    NoteTypeCodeID = 2449,
                    NoteEntry = model.RequestNote,
                    CaseID = UserManager.UserExtended.CaseID,

                    RecordStateID = 1,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();

            }


            Session["PrintAR"] = null;//for print and submit
            return Json(new { Status = "Done", insertedId = insertedId.ToEncrypt() });
        }


        #endregion
    }
}