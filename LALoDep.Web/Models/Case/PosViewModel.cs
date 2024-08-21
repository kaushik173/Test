using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LALoDep.Domain.pd_Hearing;

namespace LALoDep.Models.Case
{
    public class PosViewModel
    {
        public int? HearingID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        [Display(Name = "Hearing Type")]
        public string HearingType { get; set; }
        [Display(Name = "Hearing Date")]
        public string HearingDateTime { get; set; }
        [Display(Name = "Hearing Officer")]
        public string HearingJudge { get; set; }
        [Display(Name = "Court Department")]
        public string HearingDept { get; set; }

        public List<pd_HearingOpinionGetByHearingID_spResult> PeopleOnHearingList { get; set; }

        public PosViewModel()
        {
            PeopleOnHearingList = new List<pd_HearingOpinionGetByHearingID_spResult>();
        }
    }
}