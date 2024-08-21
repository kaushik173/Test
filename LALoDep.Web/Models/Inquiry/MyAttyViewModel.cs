using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain.pd_Calendar;
using LALoDep.Domain.pd_MyJcats;
using LALoDep.Domain.Advisement;

namespace Jcats.SD.UI.ViewModels
{
    public class MyAttyViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public string ClientNameFirst { get; set; }
        public string ClientNameLast { get; set; }
        public string CaseNumber { get; set; }
        public string MyDailyTimeDate { get; set; }
        public bool Start { get; set; }
        public List<MyCaseLoadsViewModel> MyCaseLoads { get; set; }
        public List<MyReportsViewModel> MyReports { get; set; }
        public List<pd_IndividualCalendar_spResult> MyCalendar { get; set; }
        public List<Advisement_GetForAttorney_spResult> AdvisementList { get; set; }


        public MyAttyViewModel()
        {
            MyCaseLoads = new List<MyCaseLoadsViewModel>();
            MyReports = new List<MyReportsViewModel>();
            MyCalendar = new List<pd_IndividualCalendar_spResult>(); 
            StartDate = DateTime.Today.ToShortDateString();
            EndDate = DateTime.Today.ToShortDateString();
            AdvisementList = new List<Advisement_GetForAttorney_spResult>();
        }
    }
}