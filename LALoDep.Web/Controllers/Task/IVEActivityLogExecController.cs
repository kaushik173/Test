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
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.IVEActivityLogExecPage, PageSecurityItemID = SecurityToken.IVEActivityLogExecDirector)]
        public virtual ActionResult IVEActivityLogExec()
        {
            var viewModel = new IVEActvityLogExecViewModel() { PersonName = UserManager.UserExtended.FullName };
            viewModel.PersonID = UserManager.UserExtended.PersonID;
            if (Request.QueryString["personId"] != null)
            {
                viewModel.PersonID = Request.QueryString["personId"].ToDecrypt().ToInt();

            }
            var header = UtilityService.ExecStoredProcedureWithResults<TitleIVeActivityLogExecDirGet_spResult>("TitleIVeActivityLogExecDirGet_sp", new TitleIVeActivityLogExecDirGet_spParams
            {
                AgencyID = UserManager.UserExtended.AgencyID,

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

                viewModel.DateSignedEmployee = header.DateSignedEmployee?.ToString("MM/dd/yyyy");
                viewModel.DateSignedSupervisor = header.DateSignedSupervisor?.ToString("MM/dd/yyyy");


                viewModel.PersonName = viewModel.EmployeeName = header.EmployeeName;


            }

            //viewModel.AgencyCountyList = UtilityService.ExecStoredProcedureWithResults<TitleIVeCountyDropDown_spResult>("TitleIVeCountyDropDown_sp", new TitleIVeCountyDropDown_spParams { UserID = UserManager.UserExtended.UserID })
            //                                            .Select(x => new SelectListItem() { Text = x.AgencyCounty, Value = x.AgencyCountyID.ToString() });
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult GetActivityExecHeader(int? countyId, int year, int month, int personId)
        {
            var header = UtilityService.ExecStoredProcedureWithResults<TitleIVeActivityLogExecDirGet_spResult>("TitleIVeActivityLogExecDirGet_sp", new TitleIVeActivityLogExecDirGet_spParams
            {
                AgencyID = UserManager.UserExtended.AgencyID,
                AgencyCountyID = countyId,
                ActivityYear = year,
                ActivityMonth = month,
                PersonID = personId,
                UserID = UserManager.UserExtended.UserID,
            }).FirstOrDefault();
            var jsonData = UtilityService.ExecStoredProcedureWithJsonResult("TitleIVeExecDirCountyAllocationGet_sp", new TitleIVeExecDirCountyAllocationGet_spParams
            {
                ActivityLogID =  header.ActivityLogID,
                UserID = UserManager.UserExtended.UserID
            });
            List<TitleIVeExecDirCountyAllocationGet_spResult> oTitleIVeExecDirCountyAllocationList = new List<TitleIVeExecDirCountyAllocationGet_spResult>();
            if (jsonData != "")
                oTitleIVeExecDirCountyAllocationList = JsonConvert.DeserializeObject<List<TitleIVeExecDirCountyAllocationGet_spResult>>(jsonData);

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
                    header.EmployeeName,
                    FFDRPPaidTimeOff = header.FFDRPPaidTimeOff.HasValue ? header.FFDRPPaidTimeOff.Value : 0,
                    FFDRPWorked = header.FFDRPWorked.HasValue ? header.FFDRPWorked.Value : 0,
                    TotalFFDRPEligible = header.TotalFFDRPEligible.HasValue ? header.TotalFFDRPEligible.Value : 0,
                    TotalFFDRPIneligible = header.TotalFFDRPIneligible.HasValue ? header.TotalFFDRPIneligible.Value : 0,
                    NonDependPercent_ExecDir = header.NonDependPercent_ExecDir.HasValue ? header.NonDependPercent_ExecDir.Value : 0,
                    PTOReimbursmentRate_ExecDir = header.PTOReimbursmentRate_ExecDir.HasValue ? header.PTOReimbursmentRate_ExecDir.Value : 0,
                    TimeOffPercent_ExecDir = header.TimeOffPercent_ExecDir.HasValue ? header.TimeOffPercent_ExecDir.Value : 0
                    ,
                    TitleIVeExecDirCountyAllocationList = oTitleIVeExecDirCountyAllocationList
                }
            });
        }






        [HttpPost]
        public virtual ActionResult IVEActivityLogExec(IVEActvityLogExecViewModel viewModel)
        {
            if (viewModel != null)
            {

                UtilityService.ExecStoredProcedureWithoutResults("TitleIVeActivityLogExecDirInsertUpdate_sp", new TitleIVeActivityLogExecDirInsertUpdate_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    ActivityLogID = viewModel.ActivityLogID,
                    ActivityMonth = viewModel.ActivityMonth.Month,
                    ActivityYear = viewModel.ActivityMonth.Year,
                    AgencyCountyID = viewModel.AgencyCountyID,
                    AgencyID = UserManager.UserExtended.AgencyID,
                    NonDependPercent_ExecDir = viewModel.NonDependPercent_ExecDir,
                    PersonID = viewModel.PersonID,
                    TimeOffPercent_ExecDir = viewModel.TimeOffPercent_ExecDir,
                    DateSignedEmployee = viewModel.DateSignedSupervisor.ToDateTimeNullableValue(),
                });
                return Json(new { isSuccess = true });
            }
            return Json(new { isSuccess = false, message = "There is some issue while saving data" });
        }





    }


}