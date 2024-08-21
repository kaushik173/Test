using LALoDep.Domain.pd_Allegation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models
{
    public class QHEAllegationViewModel
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
        public List<AllegationViewModel> Allegations { get; set; }

    }

    public class AllegationViewModel
    {
        public int PetitionID { get; set; }
        public string PetitionNumber { get; set; }
        public string PetitionFileDate { get; set; }
        public string ChildLastName { get; set; }
        public string ChildFirstName { get; set; }
        public int? AllegationID { get; set; }
        public string AllegationTypeCodeValue { get; set; }
        public int? AllegationTypeCodeID { get; set; }        
        public int? AllegationFindingCodeID { get; set; }
        public short? RecordStateID { get; set; }

        public bool IsChanged { get; set; }
        public int? PetitionGlobalFindingCodeId { get; set; }
        public string NoteEntry { get; set; }
        public string AllegationIdentifier { get; set; }

    }
}