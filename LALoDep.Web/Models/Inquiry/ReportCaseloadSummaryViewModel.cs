using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LALoDep.Models.Inquiry
{
    public class ReportCaseloadSummaryViewModel
    {

        [Display(Name = "Agency")]
        public int AgencyID { get; set; }
        [Display(Name = "Case Status")]
        public string CaseStatus { get; set; }
        [Display(Name = "Client Type")]
        public string ClientType { get; set; }
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }   
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        [Display(Name = "Hearing Date")]
        public string HearingDate { get; set; }
        [Display(Name = "Hearing Type")]
        public int? HearingTypeID { get; set; }
        [Display(Name = "Attorney")]
        public int? AttorneyID { get; set; }
        [Display(Name = "Pending AR Investigator")]
        public int? InvestigatorID { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentID { get; set; }
        [Display(Name = "County")]
        public int? AgencyCountyID { get; set; }
        [Display(Name = "Designated Day (Active Clients Only)")]
        public int? DesignatedDayID { get; set; }
        [Display(Name = "Active Child Client Placed With Parent")]
        public bool PlacedWithParent { get; set; }
        [Display(Name = "Address Type (Client's Current Address)")]
        public int? AddressTypeID { get; set; }
        [Display(Name = "Sort By")]
        public string SortBy { get; set; }

        public List<AgencyModel> AgencyList { get; set; }
        public List<CodeViewModel> HearingTypeList { get; set; }
        public List<ReportParameterAttorneyListViewModel> AttorneyList { get; set; }
        public List<ReportParameterInvestigatorListViewModel> InvestigatorList { get; set; }
        public List<CodeViewModel> DepartmentList { get; set; }
        public List<AgencyCountyModel> AgencyCountyList { get; set; }
        public List<CodeViewModel> DesignatedDayList { get; set; }
        public List<CodeViewModel> AddressTypeList { get; set; }

        public IEnumerable<KeyValuePair<string, string>> StatusList
        {
            get
            {
                return new List<KeyValuePair<string, string>>() 
                {
                    new KeyValuePair<string, string>("Open", "Open"),
                    new KeyValuePair<string, string>("Closed", "Closed"),
                    new KeyValuePair<string, string>("Both", "Both"),
                };
            }
        }
        public IEnumerable<KeyValuePair<string, string>> ClientTypeList
        {
            get
            {
                return new List<KeyValuePair<string, string>>() 
                {
                    new KeyValuePair<string, string>("Both", "Both"),
                    new KeyValuePair<string, string>("Child", "Minor"),
                    new KeyValuePair<string, string>("Parent", "Parent"),
                };
            }
        }
        public IEnumerable<KeyValuePair<string, string>> SortByList
        {
            get
            {
                return new List<KeyValuePair<string, string>>() 
                {
                    new KeyValuePair<string, string>("CaseNumber", "Case #"),
                    new KeyValuePair<string, string>("CaseName", "Case Name"),
                    new KeyValuePair<string, string>("NextHearingDate", "Next Hearing Date"),
                };
            }
        }


        public ReportCaseloadSummaryViewModel()
        {
            AgencyList = new List<AgencyModel>();
            HearingTypeList = new List<CodeViewModel>();
            AttorneyList = new List<ReportParameterAttorneyListViewModel>();
            InvestigatorList = new List<ReportParameterInvestigatorListViewModel>();
            DepartmentList = new List<CodeViewModel>();
            AgencyCountyList = new List<AgencyCountyModel>();
            DesignatedDayList = new List<CodeViewModel>();
            AddressTypeList = new List<CodeViewModel>();
        }

    }
}