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
using LALoDep.Core.Custom.Extensions;
using LALoDep.Domain.Advisement;

namespace LALoDep.Controllers
{
    public partial class InquiryController
    {
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.MyJCATSSupvAttorney)]

        public virtual ActionResult SupvAtty()
        {
            var guid = Guid.NewGuid();
            var viewModel = new SupervisingAttorneyViewModel();
            if (TempData["StartDate"] != null)
                viewModel.StartDate = TempData["StartDate"].ToString();
            else
                viewModel.StartDate = DateTime.Now.ToShortDateString();

            if (TempData["EndDate"] != null)
                viewModel.EndDate = TempData["EndDate"].ToString();
            else
                viewModel.EndDate = DateTime.Now.ToShortDateString();
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
            viewModel.MyDailyTimeDate = viewModel.StartDate;
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
            viewModel.MyCalendar = UtilityService.ExecStoredProcedureWithResults<pd_CalendarGetSummaryByStaffStartDateEndDate_spResult>("pd_CalendarGetSummaryByStaffStartDateEndDate_sp", new pd_CalendarGetSummaryByStaffStartDateEndDate_spParams()
            {

                StartDate = viewModel.StartDate.ToDateTime(),
                EndDate = viewModel.EndDate.ToDateTime(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).ToList();
            viewModel.Start = true;
            viewModel.AdvisementList = UtilityService.ExecStoredProcedureWithResults<Advisement_GetForSupervisor_spResult>("Advisement_GetForSupervisor_sp",
                 new Advisement_GetForSupervisor_spParams()
                 {
                     BatchLogJobID = Guid.NewGuid(),
                     UserID = UserManager.UserExtended.UserID,
                     SupervisorPersonID = UserManager.UserExtended.PersonID

                 }).ToList();
            TempData["StartDate"] = viewModel.StartDate;
            TempData["EndDate"] = viewModel.EndDate;


            return View(viewModel);
        }

        /// <summary>
        /// Created By:Humair Ahmed
        /// Created On: 18 FEB, 2016
        /// Purpose: This action is used for search button click event in MyJCATS - Supv Atty Page
        /// Last Updated On: 
        /// Last Updated By: 
        /// Reason: 
        /// </summary>
        [HttpPost]
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.MyJCATSSupvAttorney)]

        public virtual ActionResult SupvAtty(DateTime StartDate, DateTime EndDate)
        {
            var viewModel = new SupervisingAttorneyViewModel();
            viewModel.StartDate = StartDate.ToShortDateString();
            viewModel.EndDate = EndDate.ToShortDateString();
            viewModel.MyDailyTimeDate = viewModel.StartDate;
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


            viewModel.MyCalendar = UtilityService.ExecStoredProcedureWithResults<pd_CalendarGetSummaryByStaffStartDateEndDate_spResult>("pd_CalendarGetSummaryByStaffStartDateEndDate_sp", new pd_CalendarGetSummaryByStaffStartDateEndDate_spParams()
            {

                StartDate = StartDate,
                EndDate = EndDate,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).ToList();

            viewModel.Start = false;

            TempData["StartDate"] = viewModel.StartDate;
            TempData["EndDate"] = viewModel.EndDate;

            return View(viewModel);
        }
        public virtual ActionResult WritAppealNoticeDuesByAttorney(int id)
        {
            var data = UtilityService.ExecStoredProcedureWithResults<Advisement_GetForAttorney_spResult>("Advisement_GetForAttorney_sp",
                          new Advisement_GetForAttorney_spParams()
                          {
                              BatchLogJobID = Guid.NewGuid(),
                              UserID = UserManager.UserExtended.UserID,
                              AttorneyPersonID = id

                          }).ToList();

            return View(data);
        }
    }
}