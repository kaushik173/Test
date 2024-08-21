using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Calendar;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Person;
using LALoDep.Core.Custom.Extensions;
using Jcats.SD.UI.ViewModels;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Inquiry;
using LALoDep.Custom;
using LALoDep.Domain.pd_Leave;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.rpt_Print;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace LALoDep.Controllers
{
    public partial class InquiryController : Controller
    {

        // GET: MyCalendar
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.ViewMyCalendar, CustomSecurityItemIds = PageLevelSecurityItemIds.ViewMyCalendarPage)]
        public virtual ActionResult MyCalendar(string p)
        {
            var model = new MyCalendarModel
            {
                AttorneyPersonId = UserManager.UserExtended.PersonID,
                AttorneyPersonName = UserManager.UserExtended.FullName
            };

            var spParams = new pd_CodeGetByTypeIDAndUserID_spParams
            {
                AgencyID = UserManager.UserExtended.AgencyID,
                CodeTypeID = 10,

                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
            };

            #region Person Get
            var personId = UserManager.UserExtended.PersonID;
            if (string.IsNullOrEmpty(p))
            {
                personId = UserManager.UserExtended.PersonID;
                model.AttorneyPersonId = personId;
            }
            else
            {
                personId = p.ToDecrypt().ToInt();
                if (personId != 0)
                {
                    model.AttorneyPersonId = personId;

                    var personParams = new pd_PersonGet_spParams
                    {
                        PersonID = personId,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),

                    };
                    var personResult =
                        UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>(
                            "pd_PersonGet_sp", personParams).FirstOrDefault();
                    if (personResult != null)
                    {
                        model.AttorneyPersonName = personResult.FirstName + " " + personResult.LastName;
                    }

                }
            }
            TempData["AttorneyPersonName"] = model.AttorneyPersonName;
            #endregion

            model.HearingType =
                UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>(
                    "pd_CodeGetByTypeIDAndUserID_sp", spParams)
                    .Select(o => new HearingTypeViewModel() { HearingTypeId = o.CodeID, HearingTypeName = o.CodeValue })
                    .ToList();
            model.PendingHearingsOnly = false;

            // Need to remove this session variable
            //if (Session["CalendarStartDate"] != null)
            //{
            //    model.StartDate = Session["CalendarStartDate"].ToString();
            //}
            //else
            //{
            //    Session["CalendarStartDate"] = model.StartDate = DateTime.Now.ToString("MM/dd/yyyy");
            //}
            //if (Session["CalendarEndDate"] != null)
            //{
            //    model.EndDate = Session["CalendarEndDate"].ToString();
            //}
            //else
            //{
            //    Session["CalendarEndDate"] = model.EndDate = DateTime.Now.AddDays(7).ToString("MM/dd/yyyy");
            //}

            if (TempData["StartDate"] != null)
                model.StartDate = TempData["StartDate"].ToString();
            else
                model.StartDate = DateTime.Now.ToString("MM/dd/yyyy");

            if (TempData["EndDate"] != null)
                model.EndDate = TempData["EndDate"].ToString();
            else
                model.EndDate = DateTime.Now.AddDays(7).ToString("MM/dd/yyyy");

            TempData["PersonID"] = (p ?? string.Empty);

            //TempData["StartDate"] = model.StartDate;
            //TempData["EndDate"] = model.EndDate;

            return View(model);
        }

        [HttpPost]
        public virtual JsonResult GetHearingDataForTableView(MyCalendarModel model)
        {
            // Need to remove this Session variables
            // Session["CalendarStartDate"] = model.StartDate;
            //Session["CalendarEndDate"] = model.EndDate;

            #region Hearing Data

            var spParams = new pd_IndividualCalendar_spParams()
            {
                PersonID = model.AttorneyPersonId,
                PendingOnlyFlag = model.PendingHearingsOnly ? 1 : 0,
                StartDate = DateTime.Parse(model.StartDate),
                EndDate = DateTime.Parse(model.EndDate),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),

                HearingTypeCodeID = model.HearingTypeId

            };

            var hearingData =
                UtilityService.ExecStoredProcedureWithResults<pd_IndividualCalendar_spResult>(
                    "pd_IndividualCalendar_sp", spParams).ToList();

            var hearingDataList = hearingData.Select(o => new HearingListViewModel()
            {
                EventDate = o.EventDate,
                EventTime = o.EventTime,
                Result = string.IsNullOrEmpty(o.Result) ? "" : o.Result,
                Clients = o.Clients,
                Department = o.Department,
                HearingType = o.HearingType,
                Petitions = o.Petitions,
                HearingID = o.HearingID,
                EncryptedCaseID = o.CaseID.ToEncrypt(),
                EncryptedHearingID = o.HearingID.ToEncrypt(),
                QHE = "QHE",
                hearingIDList = string.Join(",", hearingData.Select(x => x.HearingID)),
                FillInFor=o.FillInFor
            }).ToList();

            //var time = string.Empty;
            //var date = string.Empty;
            //var dept = string.Empty;
            //var type = string.Empty;

            int? previosHearingId = 0;
            foreach (var item in hearingDataList)
            {
                //bool isSame = date.Equals(item.EventDate, StringComparison.Ordinal);
                //if (isSame)
                //{
                //    //time = string.Empty;
                //}
                //else
                //{
                //    //time = string.Empty;
                //    //dept = string.Empty;
                //    //type = string.Empty;
                //    date = item.EventDate;
                //}

                if (previosHearingId != item.HearingID)
                {
                    //time = item.EventTime;
                    //dept = item.Department;
                    //type = item.HearingType;                    
                    //item.Result = string.IsNullOrEmpty(item.Result) ? "No" : item.Result;
                    previosHearingId = item.HearingID;
                }
                else
                {
                    item.EventTime = string.Empty;
                    item.Department = string.Empty;
                    item.HearingType = string.Empty;
                    item.Result = string.Empty;
                    item.QHE = string.Empty;
                    item.FillInFor = string.Empty;
                }
            }


            #endregion



            #region Leave Data

            var spLeaveParams = new pd_GetLeaveForMyCalendar_spParams()
            {
                PersonID = model.AttorneyPersonId,

                StartDate = DateTime.Parse(model.StartDate),
                EndDate = DateTime.Parse(model.EndDate),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),

            };
            var leaveData =
               UtilityService.ExecStoredProcedureWithResults<pd_GetLeaveForMyCalendar_spResult>(
                    "pd_GetLeaveForMyCalendar_sp", spLeaveParams).ToList();



            #endregion

            var dt = new List<DataTablesResponse> { new DataTablesResponse(0, hearingDataList, hearingDataList.Count, hearingDataList.Count),
            new DataTablesResponse(2, leaveData, leaveData.Count, leaveData.Count) };

            TempData["StartDate"] = model.StartDate;
            TempData["EndDate"] = model.EndDate;

            return Json(dt);


        }

        [HttpPost]
        public virtual ActionResult PrintMyCalendar(MyCalendarModel model)
        {
            var reportParams = new rpt_IndividualCalendar_spParams()
            {
                PersonID = model.AttorneyPersonId,
                StartDate = model.StartDate.ToDateTimeValue(),
                EndDate = model.EndDate.ToDateTimeValue(),
                HearingTypeID = model.HearingTypeId ?? 0,
                PendingOnlyFlag = model.PendingHearingsOnly ? 1 : 0,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

  string filename = "MyCalendar" + UserManager.UserExtended.UserID.ToEncrypt() + ".pdf";


            ReportClass rpt = new ReportClass();
            try
            {
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/IndividualCalendar.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("rpt_IndividualCalendar_sp", reportParams);
                rpt.SetDataSource(table);

              
                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + filename);


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






            return Download(filename);


        }

        #region Leave
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.ViewLeave, CustomSecurityItemIds = PageLevelSecurityItemIds.LeavePage)]
        public virtual ActionResult Leave(string id, string p)
        {
            var viewModel = new LeaveViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                var leaveGetParams = new pd_LeaveGet_spParams
                {
                    LeaveID = id.ToDecrypt().ToInt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };

                var leave = UtilityService.ExecStoredProcedureWithResults<pd_LeaveGet_spResult>("pd_LeaveGet_sp", leaveGetParams).FirstOrDefault();
                if (leave != null)
                {
                    viewModel.LeaveID = leave.LeaveID;

                    var startDate = leave.LeaveStartDateTime.ToDateTimeValue();
                    if (startDate.HasValue)
                    {
                        viewModel.StartDate = startDate.ToDefaultFormat();
                        viewModel.StartTime = startDate.Value.ToShortTimeString();
                    }

                    var endDate = leave.LeaveEndDateTime.ToDateTimeValue();
                    if (endDate.HasValue)
                    {
                        viewModel.EndDate = endDate.ToDefaultFormat();
                        viewModel.EndTime = endDate.Value.ToShortTimeString();
                    }

                    viewModel.LeaveTypeCodeID = leave.LeaveTypeCodeID;

                }
            }

            viewModel.PersonID = p.ToDecrypt().ToInt(); ;
            if (viewModel.PersonID == 0)
                viewModel.PersonID = UserManager.UserExtended.PersonID;
            var personParams = new pd_PersonGet_spParams
            {
                PersonID = viewModel.PersonID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var personResult = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", personParams).FirstOrDefault();
            if (personResult != null)
            {
                viewModel.PersonName = personResult.FirstName + " " + personResult.LastName;
            }

            viewModel.LeaveTypes = UtilityFunctions.CodeGetByTypeIdAndUserId(45, includeCodeId: viewModel.LeaveTypeCodeID ?? 0);
            viewModel.RecordTypeList = new List<SelectListItem>() { new SelectListItem { Text = "Block", Value = "Block" }, new SelectListItem { Text = "Individual", Value = "Individual" } };
            viewModel.LeaveList = UtilityService.ExecStoredProcedureWithResults<pd_LeaveGetByPersonID_spResult>("pd_LeaveGetByPersonID_sp",
                                    new pd_LeaveGetByPersonID_spParams { PersonID = viewModel.PersonID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                        .Select(x => new LeaveListViewModel
                                        {
                                            LeaveID = x.LeaveID,
                                            LeaveDateTimeDisplay = x.LeaveDateTimeDisplay,
                                            LeaveType = x.LeaveType
                                        }).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult Leave(LeaveViewModel viewModel)
        {
            if (viewModel.LeaveID.HasValue)
            {
                UtilityService.ExecStoredProcedureScalar("pd_LeaveUpdate_sp", new pd_LeaveUpdate_spParams
                {
                    LeaveID = viewModel.LeaveID,
                    PersonID = viewModel.PersonID,
                    LeaveStartDateTime = (viewModel.StartDate + " " + viewModel.StartTime).ToDateTimeValue(),
                    LeaveEndDateTime = (viewModel.EndDate + " " + viewModel.EndTime).ToDateTimeValue(),
                    LeaveTypeCodeID = viewModel.LeaveTypeCodeID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });
            }
            else // Insert
            {
                UtilityService.ExecStoredProcedureScalar("pd_LeaveInsert_sp", new pd_LeaveInsert_spParams
                {
                    PersonID = viewModel.PersonID,
                    LeaveStartDateTime = (viewModel.StartDate + " " + viewModel.StartTime).ToDateTimeValue(),
                    LeaveEndDateTime = (viewModel.EndDate + " " + viewModel.EndTime).ToDateTimeValue(),
                    LeaveTypeCodeID = viewModel.LeaveTypeCodeID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });

            }

            return Json(new { isSuccess = true });
        }

        public virtual ActionResult DeleteLeave(string id)
        {
            UtilityService.ExecStoredProcedureScalar("pd_LeaveDelete_sp", new pd_LeaveDelete_spParams
            {
                ID = id.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            });

            return Json(new { isSuccess = true });
        }
        #endregion

        #region Transfer Judge
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.TransferHearingOfficer)]
        public virtual ActionResult HearingOfficerTransfer(string p)
        {
            var viewModel = new JudgeTransferViewModel();
            if (string.IsNullOrEmpty(p))
                viewModel.PersonID = UserManager.UserExtended.PersonID;
            else
                viewModel.PersonID = p.ToDecrypt().ToInt();


            if (TempData["StartDate"] != null)
                viewModel.StartDate = TempData["StartDate"].ToString();
            if (TempData["EndDate"] != null)
                viewModel.EndDate = TempData["EndDate"].ToString();

            viewModel.StartTime = "08:00 AM";
            viewModel.EndTime = "05:00 PM";

            if (TempData["AttorneyPersonName"] != null)
                viewModel.PersonName = TempData["AttorneyPersonName"].ToString();

            viewModel.HearingOfficerList = UtilityService.ExecStoredProcedureWithResults<pd_HearingOfficerGet_spResults>("pd_HearingOfficerGet_sp",
                                            new pd_HearingOfficerGet_spParams
                                            {
                                                UserID = UserManager.UserExtended.UserID,
                                                AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                                BatchLogJobID = Guid.NewGuid(),
                                                CaseID = UserManager.UserExtended.CaseID
                                            }).Select(o => new SelectListItem() { Value = o.PersonID.ToString(), Text = o.PersonNameLast + ", " + o.PersonNameFirst }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult HearingOfficerTransfer(JudgeTransferViewModel viewModel)
        {

            UtilityService.ExecStoredProcedureScalar("pd_HearingOfficerTransfer_sp",
                                        new pd_HearingOfficerTransfer_spParams
                                        {
                                            StartDateTime = (viewModel.StartDate + " " + viewModel.StartTime).ToDateTimeValue(),
                                            EndDateTime = (viewModel.EndDate + " " + viewModel.EndDate).ToDateTimeValue(),
                                            FromPersonID = viewModel.TransferFromPersonID,
                                            ToPersonID = viewModel.TransferToPersonID,
                                            PersonID = viewModel.PersonID,
                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid()
                                        });

            return Json(new { isSuccess = true });
        }

        #endregion
    }
}