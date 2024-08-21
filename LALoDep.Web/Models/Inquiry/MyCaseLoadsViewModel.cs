using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jcats.SD.UI.ViewModels
{
    public class MyCaseLoadsViewModel
    {
        [Display(Name = "")]
        [ScaffoldColumn(false)]
        public string HighlightCharacter { get; set; }
        [Display(Name="Type")]
        public string CountType { get; set; }
        [Display(Name = "Count")]
        public string CountValue { get; set; }
        [ScaffoldColumn(false)]
        public int? ActionTypeCodeID { get; set; }
        [ScaffoldColumn(false)]
        public string RouteToFormName { get; set; }
        [ScaffoldColumn(false)]
        public string RouteToPage { get; set; }
    }
 
    public class MyReportsViewModel
    {
        [Display(Name = "Name")]
        public string JcatsReportName { get; set; }

        [ScaffoldColumn(false)]
        public int ReportID { get; set; }
    }
    public class MyCalendarViewModel
    {
        [ScaffoldColumn(false)]
        public int PersonID { get; set; }
        [ScaffoldColumn(false)]
        public int StaffID { get; set; }
        [Display(Name = "Attorney")]
        public string AttorneyName { get; set; }
        [Display(Name = "Pending Leaves")]
        public int PendingLeaveCount { get; set; }
        [Display(Name = "Pending Hearings")]
        public int PendingHearingCount { get; set; }
        [Display(Name = "Trial Call")]
        public int TrialCount { get; set; }
    
    }
}