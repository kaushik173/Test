using LALoDep.Domain.pd_Calendar;
using Jcats.SD.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models
{
    public class MyJCATSMgmtViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public string ClientNameFirst { get; set; }
        public string ClientNameLast { get; set; }
        public string CaseNumber { get; set; }

        public bool Start { get; set; }
        public List<MyReportsViewModel> MyReports { get; set; }
        public List<MyCaseLoadsViewModel> MyCaseLoads { get; set; }
        public List<pd_CalendarGetSummaryByStaffStartDateEndDate_spResult> MyCalendar { get; set; }
        public string MyDailyTimeDate { get; set; }
        public MyJCATSMgmtViewModel()
        {
            StartDate = DateTime.Today.ToShortDateString();
            EndDate = DateTime.Today.ToShortDateString();
            MyReports = new List<MyReportsViewModel>();
            MyCaseLoads = new List<MyCaseLoadsViewModel>();
            MyCalendar = new List<pd_CalendarGetSummaryByStaffStartDateEndDate_spResult>();
        }
    }
}