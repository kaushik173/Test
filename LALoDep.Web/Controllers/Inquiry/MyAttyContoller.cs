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
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.MyJCATSAttorney)]
        public virtual ActionResult MyJcatsAtty()
        {
            var guid = Guid.NewGuid();
            var viewModel = new MyAttyViewModel
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
                viewModel.MyDailyTimeDate = viewModel.StartDate =  DateTime.Parse(Request.QueryString["StartDate"]).ToShortDateString();
            }
            if (Request.QueryString["EndDate"] != null)
            {
                viewModel.EndDate = DateTime.Parse(Request.QueryString["EndDate"]).ToShortDateString();
            }

            viewModel.MyCaseLoads = UtilityService.ExecStoredProcedureWithResults<pd_MyJcatsAttorneyCountsGet_spResults>("pd_MyJcatsAttorneyCountsGet_sp", new pd_MyJcatsAttorneyCountsGet_spParams()
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
            viewModel.MyCalendar = UtilityService.ExecStoredProcedureWithResults<pd_IndividualCalendar_spResult>("pd_IndividualCalendar_sp", new pd_IndividualCalendar_spParams()
            {
                PersonID = UserManager.UserExtended.PersonID,
                StartDate = DateTime.Parse(viewModel.StartDate),
                EndDate = DateTime.Parse(viewModel.EndDate),
                HearingTypeCodeID = null,
                OtherPersonID = null,
                OtherPersonRoleType = null,
                PendingOnlyFlag = null,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
            }).ToList();

            viewModel.AdvisementList = UtilityService.ExecStoredProcedureWithResults<Advisement_GetForAttorney_spResult>("Advisement_GetForAttorney_sp",
                  new Advisement_GetForAttorney_spParams()
                  {
                      BatchLogJobID = Guid.NewGuid(),
                      UserID = UserManager.UserExtended.UserID,
                      AttorneyPersonID = UserManager.UserExtended.PersonID

                  }).ToList();

            viewModel.Start = true;

            TempData["StartDate"] = viewModel.StartDate;
            TempData["EndDate"] = viewModel.EndDate;

            return View(viewModel);
        }

        [HttpPost]
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.MyJCATSAttorney)]

        public virtual ActionResult MyJcatsAtty(DateTime StartDate, DateTime EndDate)
        {
            var viewModel = new MyAttyViewModel();
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


            viewModel.MyCalendar = UtilityService.ExecStoredProcedureWithResults<pd_IndividualCalendar_spResult>("pd_IndividualCalendar_sp", new pd_IndividualCalendar_spParams()
            {
                PersonID = UserManager.UserExtended.PersonID,
                StartDate = DateTime.Parse(viewModel.StartDate),
                EndDate = DateTime.Parse(viewModel.EndDate),
                HearingTypeCodeID = null,
                OtherPersonID = null,
                OtherPersonRoleType = null,
                PendingOnlyFlag = null,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
            }).ToList();

            viewModel.Start = false;

            TempData["StartDate"] = viewModel.StartDate;
            TempData["EndDate"] = viewModel.EndDate;

            return View("MyJcatsAtty", viewModel);
        }




        public virtual ActionResult MyJcatsAttyDailyTime(string StartDate, string page)
        {
            page = page + "?sdate=" + StartDate;

            var data = UtilityService.ExecStoredProcedureWithResults<MyDailyTime_spResult>("MyDailyTime_sp", new MyDailyTime_spParams()
            {
                StartDate = Convert.ToDateTime(StartDate),

                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                PersonID = UserManager.UserExtended.PersonID,

            }).ToList();

            foreach (var item in data)
            {
                if (!item.RoutingURL.IsNullOrEmpty())
                {
                    var url = "";
                    switch (item.RoutingURL)
                    {
                        case "Case/RecordTimeEdit":
                            url =  "/" + item.RoutingURL + "/" + item.WorkID.ToEncrypt()+ "?caseId=" + item.CaseID.Value.ToEncrypt() + "&page=" + page;
                            break;
                        case "Task/RecordTimeNonCaseEdit":
                            url = "/" + item.RoutingURL + "/" + item.WorkID.ToEncrypt() + "?page=" + page;
                            break;
                        case "Task/CalendarAppearanceSheet":
                            url = "/" + item.RoutingURL + "/" + item.HearingID.ToEncrypt() + "?caseId=" + item.CaseID.Value.ToEncrypt() + "&page=" + page;

                            break;
                        case "CaseOpening/Hearing":
                            url ="/" + item.RoutingURL + "/" + item.HearingID.ToEncrypt() + "?caseId=" + item.CaseID.Value.ToEncrypt() + "&page=" + page;

                            break;

                    }
                    if (!url.IsNullOrEmpty())
                        item.WorkDescription = string.Format("<a href='{1}'>{0}</a>", item.WorkDescription, url);
                }
            }
            return View("_myJcatsAttyDailyTimePartial", data);
        }

    }
}