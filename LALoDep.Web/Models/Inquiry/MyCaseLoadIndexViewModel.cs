using LALoDep.Domain.pd_CaseLoad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LALoDep.Models.Inquiry
{
    [Serializable] 
    public class MyCaseLoadIndexViewModel
    {
        public int MyCaseloadVersionNumber { get; set; }
        public int MyCaseloadRoleTypeCodeID { get; set; }

        [Display(Name = "Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        [Display(Name = "Client's Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Client's First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Case #")]
        public string CaseNumber { get; set; }
        [Display(Name = "Case Status")]
        public string CaseStatus { get; set; }
        [Display(Name = "Client Type")]
        public string ClientType { get; set; }
        [Display(Name = "County")]
        public int? CountyID { get; set; }
        [Display(Name = "Print Sort")]
        public string PrintSort { get; set; }
        public int AgencyID { get; set; }
        public int PersonID { get; set; }
        public string PersonName{ get; set; }


        
        [Display(Name = "Hearing Date Range")]
        public string HearingDateRangeStartDate { get; set; }      
        public string HearingDateRangeEndDate { get; set; }


        [Display(Name = "Age Range")]
        public int? AgeRangeStart { get; set; }
        
        public int? AgeRangeEnd { get; set; }

        public int? SortByID { get; set; }
        public List<SelectListItem> SortByList { get; set; }
        public int? ClientStatusID { get; set; }
        public List<SelectListItem> ClientStatusList { get; set; }
        public int? HearingTypeID { get; set; }
        public List<SelectListItem> HearingTypeList { get; set; }
        public int? AddressTypeID { get; set; }
        public List<SelectListItem> AddressTypeList { get; set; }
        public int? ClassificationID { get; set; }
        public List<SelectListItem> ClassificationList { get; set; }
        public int? MedicationCurrentID { get; set; }
        public List<SelectListItem> MedicationCurrentList { get; set; }
        public int? MedicationEverID { get; set; }
        public List<SelectListItem> MedicationEverList { get; set; }
        public int? ReportID { get; set; }

        [Display(Name = "Out of State")]
        public bool OutOfState { get; set; }
        [Display(Name = "No Future AR")]
        public bool NoFutureAR { get; set; }
        [Display(Name = "No Race")]
        public bool NoRace { get; set; }
        [Display(Name = "No Completed AR 6M")]
        public bool NoCompletedARM { get; set; }


        public List<AgencyCountyModel> AgencyCountyList { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Status
        {
            get
            {
                return new List<KeyValuePair<string, string>>() 
                {
                    new KeyValuePair<string, string>("Open", "Open"),
                    new KeyValuePair<string, string>("Close", "Close"),
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
        public IEnumerable<KeyValuePair<string, string>> PrintSortList
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

        public MyCaseLoadIndexViewModel()
        {
            AgencyCountyList = new List<AgencyCountyModel>();
        }
         
    }
}