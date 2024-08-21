using System;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.IVEActvityLog;
using LALoDep.Domain.IVeActivity;
using Newtonsoft.Json;
using System.Collections.Generic;
using LALoDep.Domain.TitleIVe;
using LALoDep.Core.Custom.Extensions;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.IVEActvityLogPage, PageSecurityItemID = SecurityToken.IVEActvityLog)]
        public virtual ActionResult IVEActvityLog()
        {
            
            var viewModel = new IVEActvityLogViewModel() { PersonName = UserManager.UserExtended.FullName };
            viewModel.PersonID = UserManager.UserExtended.PersonID;
            if (Request.QueryString["personId"] != null)
            {
                viewModel.PersonID = Request.QueryString["personId"].ToDecrypt().ToInt();

            }
            var header = UtilityService.ExecStoredProcedureWithResults<TitleIVeActivityLogGet_spResult>("TitleIVeActivityLogGet_sp", new TitleIVeActivityLogGet_spParams
            {
                AgencyID = UserManager.UserExtended.AgencyID,
                //AgencyCountyID = 2
                ActivityYear = DateTime.Today.Year,
                ActivityMonth = DateTime.Today.Month,
                PersonID = viewModel.PersonID,
                UserID = UserManager.UserExtended.UserID,
            }).FirstOrDefault();

            if (header != null)
            {
                viewModel.AgencyCountyID = header?.AgencyCountyID;
                viewModel.ActivityLogID = header?.ActivityLogID;
                viewModel.Title = header.Title;
                viewModel.ActivityMonth = new DateTime(header.ActivityYear ?? DateTime.Today.Year, header.ActivityMonth ?? DateTime.Today.Month, 1);
                viewModel.AgencyCountyID = header.AgencyCountyID;
                viewModel.DateSignedEmployee = header.DateSignedEmployee?.ToString("MM/dd/yyyy");
                viewModel.DateSignedSupervisor = header.DateSignedSupervisor?.ToString("MM/dd/yyyy");

                viewModel.SupervisorSignedName = header.SupervisorSignedName;
                viewModel.PersonName = header.EmployeeName;
                viewModel.EmployeeName = header.EmployeeSignedPersonName;
                viewModel.UseWorkHoursForActivityLog = header.UseWorkHoursForActivityLog;
                viewModel.OKToSwitchToDifferentEmployee = header.OKToSwitchToDifferentEmployee;
                viewModel.SaveSignatureNotAllowedMessage = header.SaveSignatureNotAllowedMessage;
            }else
            {
                viewModel.ActivityMonth = new DateTime(  DateTime.Today.Year,   DateTime.Today.Month, 1);
                viewModel.ActivityLogID = 0;
            }

            viewModel.AgencyCountyList = UtilityService.ExecStoredProcedureWithResults<TitleIVeCountyDropDown_spResult>("TitleIVeCountyDropDown_sp", new TitleIVeCountyDropDown_spParams { UserID = UserManager.UserExtended.UserID })
                                                        .Select(x => new SelectListItem() { Text = x.AgencyCounty, Value = x.AgencyCountyID.ToString() });
            viewModel.OKToSwitchToDifferentEmployee = 0;
            if (UserManager.IsUserAccessTo(SecurityToken.IVEChangePersonOnActivityLog))
            {
                viewModel.OKToSwitchToDifferentEmployee = 1;
            }
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult GetActivityHeader(int? countyId, int year, int month, int personId)
        {
            var header = UtilityService.ExecStoredProcedureWithResults<TitleIVeActivityLogGet_spResult>("TitleIVeActivityLogGet_sp", new TitleIVeActivityLogGet_spParams
            {
                AgencyID = UserManager.UserExtended.AgencyID,
                AgencyCountyID = countyId,
                ActivityYear = year,
                ActivityMonth = month,
                PersonID = personId,
                UserID = UserManager.UserExtended.UserID,
            }).FirstOrDefault();

            return Json(new
            {
                isSuccess = true,
                data = new
                {
                    header.Title,
                    header.AgencyCountyID,
                    header.ActivityLogID,
                    header.ActivityYear,
                    header.ActivityMonth,
                    DateSignedEmployee = header.DateSignedEmployee?.ToString("MM/dd/yyyy") ?? "",
                    DateSignedSupervisor = header.DateSignedSupervisor?.ToString("MM/dd/yyyy") ?? "",
                    EmployeeName=  header.EmployeeSignedPersonName,
                    header.SupervisorSignedName,
                    ReadOnly = header.ReadOnly.ToInt() == 1 ? "readonly" : "",
                    header.UseWorkHoursForActivityLog,
                    SaveSignatureNotAllowedMessage = header.SaveSignatureNotAllowedMessage,
                }
            });
        }

        [HttpPost]
        public virtual ActionResult GetActivity(int activityLogId, int year, int month, int? parentLogDetailId = null, int? useWorkHoursForActivityLog = null)
        {
            var viewModel = new IVEActvitySheetViewModel()
            {
                ActivityLogID = activityLogId,
                ActivityMonth = new DateTime(year, month, 1),
                ParentActivityLogDetailID = parentLogDetailId,
                UseWorkHoursForActivityLog = useWorkHoursForActivityLog
            };

            var jsonData = UtilityService.ExecStoredProcedureWithJsonResult("TitleIVeActivityLogDetailGet_sp", new TitleIVeActivityLogDetailGet_spParams
            {
                ActivityLogID = activityLogId,
                UserID = UserManager.UserExtended.UserID
            });

            viewModel.IVeActivityLogDetails = JsonConvert.DeserializeObject<List<IVeActivityLogDetail>>(jsonData);
            return PartialView("_IVEActivitySheetMonthView", viewModel);
        }

        [HttpPost]
        public virtual ActionResult GetActivityMonthlySummary(int activityLogId)
        {
            var monthlySummary = UtilityService.ExecStoredProcedureWithResults<TitleIVeActivityLogMonthlySummaryGet_spResult>("TitleIVeActivityLogMonthlySummaryGet_sp", new TitleIVeActivityLogMonthlySummaryGet_spParams
            {
                ActivityLogID = activityLogId,
                UserID = UserManager.UserExtended.UserID
            }).FirstOrDefault() ?? new TitleIVeActivityLogMonthlySummaryGet_spResult();

            return Json(monthlySummary);
        }

        public virtual ActionResult GetAddOtherCounty()
        {
            var otherCountyText = UtilityService.ExecStoredProcedureWithResults<TitleIVeActivityLogGetOtherCountyText_spResult>("TitleIVeActivityLogGetOtherCountyText_sp", new TitleIVeActivityLogGetOtherCountyText_spParams { }).FirstOrDefault();
            return Json(new { isSuccess = true, data = otherCountyText }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult IVEActvityLog(List<IVeActivityLogDetailModel> viewModel)
        {
            if (viewModel != null)
            {
                var jsonData = JsonConvert.SerializeObject(viewModel);
                UtilityService.ExecStoredProcedureWithoutResults("TitleIVeActivityLogDetailInsertUpdate_sp", new TitleIVeActivityLogDetailInsertUpdate_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    InputData = jsonData
                });
                return Json(new { isSuccess = true });
            }
            return Json(new { isSuccess = false, message = "There is some issue while saving data" });
        }

        [HttpPost]
        public virtual ActionResult SaveSignature(int activityLogId, string signatureType)
        {
            UtilityService.ExecStoredProcedureWithoutResults("TitleIVeActivityLogSignatureUpdate_sp", new TitleIVeActivityLogSignatureUpdate_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                ActivityLogID = activityLogId,
                SignatureType = signatureType
            });

            return Json(new { isSuccess = true });
        }

        public virtual ActionResult IVEActvityLogListAgencyEmployees()
        {
            var list = UtilityService.ExecStoredProcedureWithResults<TitleIVeActivityLogListAgencyEmployees_Result>("TitleIVeActivityLogListAgencyEmployees", new TitleIVeActivityLogListAgencyEmployees_Params
            {

                UserID = UserManager.UserExtended.UserID
            }).ToList();

            return View(list);
        }

    }


}