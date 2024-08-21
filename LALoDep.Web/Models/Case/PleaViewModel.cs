using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Hearing;

namespace LALoDep.Models.Case
{
    public class PleaViewModel
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
        public int? GloabalPleaTypeCodeID { get; set; }
        public int? buttonID { get; set; }
        public IEnumerable<SelectListItem> GloabalPlea { get; set; }

        public List<pd_HearingPleaGetByHearingIDRespondent_spResult> HearingRespondentList { get; set; }
        public List<pd_HearingPleaGetByHearingIDChildren_spResult> HearingChildernList { get; set; }
        public List<pd_HearingPleaGetByHearingIDRespondent_spResult> HearingRespondentGlobalList { get; set; }

        public  PleaViewModel()
        {
            HearingRespondentList = new List<pd_HearingPleaGetByHearingIDRespondent_spResult>();
            HearingChildernList = new List<pd_HearingPleaGetByHearingIDChildren_spResult>();
            
        }

    }
}