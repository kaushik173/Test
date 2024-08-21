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
using LALoDep.Domain.pd_Hearing;

namespace LALoDep.Controllers
{
    public partial class InquiryController
    {
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.MyJCATSInvParalegalPages, PageSecurityItemID = SecurityToken.ViewMyJCATSInvParalegal)]
        public virtual ActionResult MyJcatsInv()
        {
            var guid = Guid.NewGuid();
            var viewModel = new MyInvViewModel
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
            viewModel.MyCalendar = UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGetByRequestedForPersonID_spResult>("pd_HearingReportFilingDueGetByRequestedForPersonID_sp", new pd_HearingReportFilingDueGetByRequestedForPersonID_spParams()
            {
                RequestedForPersonID = UserManager.UserExtended.PersonID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
            }).ToList();


            viewModel.MyCaseLoads = UtilityService.ExecStoredProcedureWithResults<pd_MyJcatsInvestigatorCountsGet_spResult>("pd_MyJcatsInvestigatorCountsGet_sp", new pd_MyJcatsInvestigatorCountsGet_spParams()
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

            viewModel.Start = true;

            TempData["StartDate"] = viewModel.StartDate;
            TempData["EndDate"] = viewModel.EndDate;
            return View(viewModel);
        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.MyJCATSInvParalegalPages, PageSecurityItemID = SecurityToken.ViewMyJCATSInvParalegal)]

        public virtual ActionResult MyJcatsInv(DateTime? StartDate, DateTime? EndDate)
        {
            var viewModel = new MyInvViewModel();
            viewModel.StartDate = StartDate.HasValue ? StartDate.Value.ToShortDateString() : DateTime.Now.ToShortDateString();
                 
            viewModel.EndDate = EndDate.HasValue ? EndDate.Value.ToShortDateString() : DateTime.Now.ToShortDateString();
            viewModel.MyDailyTimeDate = viewModel.StartDate;
            viewModel.MyCalendar = UtilityService.ExecStoredProcedureWithResults<pd_HearingReportFilingDueGetByRequestedForPersonID_spResult>("pd_HearingReportFilingDueGetByRequestedForPersonID_sp", new pd_HearingReportFilingDueGetByRequestedForPersonID_spParams()
            {
                RequestedForPersonID = UserManager.UserExtended.PersonID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
            }).ToList();

            viewModel.MyCaseLoads = UtilityService.ExecStoredProcedureWithResults<pd_MyJcatsSupervisingAttorneyCountsGet_spResult>("pd_MyJcatsSupervisingAttorneyCountsGet_sp", new pd_MyJcatsSupervisingAttorneyCountsGet_spParams()
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


            viewModel.Start = false;
            TempData["StartDate"] = viewModel.StartDate;
            TempData["EndDate"] = viewModel.EndDate;

            return View(MVC.Inquiry.ActionNames.MyJcatsInv, viewModel);
        }
    }
}