using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LALoDep.Models.Inquiry
{
    public class ReportRFDCaseloadSummaryViewModel
    {
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        [Display(Name = "Agency")]
        public int AgencyID { get; set; }
        [Display(Name = "Report Type")]
        public string ReportType { get; set; }
        [Display(Name = "Requested For Role Type")]
        public int RoleTypeID { get; set; }
        [Display(Name = "Completed Columns A Subset Of The Due Column")]
        public bool CompletedIsSubset { get; set; }

        [Display(Name = "Exclude if Due Date within 2 Days of Request Date")]
        public bool ExcludeDueDate2Days { get; set; }

        [Display(Name = "Due Date within 6 weeks of Request Date")]
        public bool DueDate6Weeks { get; set; }

        [Display(Name = "Only Active AR’s")]
        public bool OnlyActiveAR { get; set; }

        public List<AgencyModel> AgencyList { get; set; }
        public List<RFDCaseloadRoleTypeModel> RoleTypeList { get; set; }

        public ReportRFDCaseloadSummaryViewModel()
        {
            AgencyList = new List<AgencyModel>();
            RoleTypeList = new List<RFDCaseloadRoleTypeModel>();
        }

    }

    public class RFDCaseloadRoleTypeModel
    {
        public int RoleTypeCodeID { get; set; }
        public string RoleTypeDisplay { get; set; }
    }
}