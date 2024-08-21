using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.BatchHearing;

namespace LALoDep.Models.Task
{
    public class BatchHearingViewModel
    {
        [Display(Name = "Case #1")]
        public string Case1 { get; set; }
        [Display(Name = "Case #2")]
        public string Case2 { get; set; }
        [Display(Name = "Case #3")]
        public string Case3 { get; set; }
        [Display(Name = "Case #4")]
        public string Case4 { get; set; }
        [Display(Name = "Case #5")]
        public string Case5 { get; set; }
        [Display(Name = "Case #6")]
        public string Case6 { get; set; }


        [Display(Name = "Hearing Type")]
        public int HearingTypeID { get; set; }
        [Display(Name = "Hearing Date")]
        public string HearingDate { get; set; }
        [Display(Name = "Hearing Type")]
        public string HearingTime { get; set; }
        [Display(Name = "Hearing Officer")]
        public int HearingOfficerID { get; set; }
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        public IEnumerable<SelectListItem> HearingTypeList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> OfficcerList { get; set; }
        public List<pd_BatchHearingSearch_spResult> BatchHearingList { get; set; }
    }
}