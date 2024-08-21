using LALoDep.Domain.pd_Petition;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.CaseOpening
{
    public class EditAllegationFindingViewModel
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

        public int? GlobalFindingCodeId { get; set; }
        public int buttonID { get; set; }

        public IEnumerable<SelectListItem> Findings { get; set; }


        public List<pd_PetitionGetByCaseID_spResult> Petitions { get; set; }

        public List<AllegationViewModel> Allegations { get; set; }
    }
}