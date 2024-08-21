using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Calendar;
using LALoDep.Domain.pd_MyJcats;
using LALoDep.Domain.pd_Report;
using Jcats.SD.UI.ViewModels;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using Omu.ValueInjecter;
using LALoDep.Models;

namespace LALoDep.Controllers
{
    public partial class InquiryController
    {
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.MyJCATSManagement)]
        public virtual ActionResult MyJcatsMgmt()
        {
            var viewModel = new MyJCATSMgmtViewModel
            {
                StartDate = DateTime.Now.ToShortDateString(),
                EndDate = DateTime.Now.ToShortDateString(),
                MyDailyTimeDate = DateTime.Now.ToShortDateString()
            };
            if (Request.QueryString["sdate"] != null)
            {
                viewModel.MyDailyTimeDate = viewModel.StartDate = viewModel.EndDate = DateTime.Parse(Request.QueryString["sdate"]).ToShortDateString();
            }
            if (Request.QueryString["StartDate"] != null)
            {
                viewModel.MyDailyTimeDate = viewModel.StartDate = DateTime.Parse(Request.QueryString["StartDate"]).ToShortDateString();
            }
            if (Request.QueryString["EndDate"] != null)
            {
                viewModel.EndDate = DateTime.Parse(Request.QueryString["EndDate"]).ToShortDateString();
            }

            viewModel.MyCaseLoads = UtilityService.ExecStoredProcedureWithResults<pd_MyJcatsManagementCountsGet_spResults>("pd_MyJcatsManagementCountsGet_sp", new pd_MyJcatsManagementCountsGet_spParams()
            {
                StartDate = Convert.ToDateTime(viewModel.StartDate),
                EndDate = Convert.ToDateTime(viewModel.EndDate),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                PersonID = UserManager.UserExtended.PersonID,
                StaffID = UserManager.UserExtended.StaffID
            }).Select(c => (MyCaseLoadsViewModel)(new MyCaseLoadsViewModel()).InjectFrom(c)).ToList();

            viewModel.MyReports = UtilityService.ExecStoredProcedureWithResults<pd_ReportPersonGetByPersonID_spResult>("pd_ReportPersonGetByPersonID_sp", new pd_ReportPersonGetByPersonID_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                PersonID = UserManager.UserExtended.PersonID
            }).Select(c => (MyReportsViewModel)(new MyReportsViewModel()).InjectFrom(c)).ToList();
            viewModel.MyCalendar = UtilityService.ExecStoredProcedureWithResults<pd_CalendarGetSummaryByStaffStartDateEndDate_spResult>("pd_CalendarGetSummaryByStaffStartDateEndDate_sp",
                                                               new pd_CalendarGetSummaryByStaffStartDateEndDate_spParams()
                                                               {
                                                                   StartDate = DateTime.Parse(viewModel.StartDate),
                                                                   EndDate = DateTime.Parse(viewModel.EndDate),
                                                                   UserID = UserManager.UserExtended.UserID,
                                                               })
                                                               .Where(x => x.RoleTypeCodeID == 219).ToList();

            viewModel.Start = true;

            TempData["StartDate"] = viewModel.StartDate;
            TempData["EndDate"] = viewModel.EndDate;

            return View(viewModel);
        }

        [HttpPost]
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.MyJCATSManagement)]
        public virtual ActionResult MyJcatsMgmt(DateTime StartDate, DateTime EndDate)
        {
            var viewModel = new MyJCATSMgmtViewModel();
            viewModel.StartDate = StartDate.ToShortDateString();
            viewModel.EndDate = EndDate.ToShortDateString();
            viewModel.MyDailyTimeDate = viewModel.StartDate;
            viewModel.MyCaseLoads = UtilityService.ExecStoredProcedureWithResults<pd_MyJcatsManagementCountsGet_spResults>("pd_MyJcatsManagementCountsGet_sp", new pd_MyJcatsManagementCountsGet_spParams()
            {
                StartDate = Convert.ToDateTime(viewModel.StartDate),
                EndDate = Convert.ToDateTime(viewModel.EndDate),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                PersonID = UserManager.UserExtended.PersonID,
                StaffID = UserManager.UserExtended.StaffID
            }).Select(c => (MyCaseLoadsViewModel)(new MyCaseLoadsViewModel()).InjectFrom(c)).ToList();

            viewModel.MyReports = UtilityService.ExecStoredProcedureWithResults<pd_ReportPersonGetByPersonID_spResult>("pd_ReportPersonGetByPersonID_sp", new pd_ReportPersonGetByPersonID_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                PersonID = UserManager.UserExtended.PersonID
            }).Select(c => (MyReportsViewModel)(new MyReportsViewModel()).InjectFrom(c)).ToList();


            viewModel.MyCalendar = UtilityService.ExecStoredProcedureWithResults<pd_CalendarGetSummaryByStaffStartDateEndDate_spResult>("pd_CalendarGetSummaryByStaffStartDateEndDate_sp",
                                                                    new pd_CalendarGetSummaryByStaffStartDateEndDate_spParams()
                                                                    {
                                                                        StartDate = DateTime.Parse(viewModel.StartDate),
                                                                        EndDate = DateTime.Parse(viewModel.EndDate),
                                                                        UserID = UserManager.UserExtended.UserID,
                                                                    })
                                                                    .Where(x=>x.RoleTypeCodeID == 219).ToList();

            viewModel.Start = false;
            TempData["StartDate"] = viewModel.StartDate;
            TempData["EndDate"] = viewModel.EndDate;
            return View("MyJcatsMgmt", viewModel);
        }

    }
}