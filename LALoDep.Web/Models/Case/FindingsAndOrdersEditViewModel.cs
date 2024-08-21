using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class FindingsAndOrdersEditViewModel
    {
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? AgencyID { get; set; }
        public int? HearingFindingOrderID { get; set; }
        [Display(Name = "Comment")]
        public string HearingFindingOrderComment { get; set; }
        [Display(Name = "Hearing Type")]
        public string HearingType { get; set; }
        [Display(Name = "Hearing Date")]
        public string HearingDateTime { get; set; }
        [Display(Name = "Hearing Officer")]
        public string HearingJudge { get; set; }
        [Display(Name = "Court Department")]
        public string HearingDept { get; set; }

        [Display(Name = "Note")]
        public string NoteEntry { get; set; }
        public int? NoteID { get; set; }
        public string NoteSubject { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        
        public int? HearingFindingOrderCodeID { get; set; }
        public int? RecordStateID { get; set; }

        public bool IsFindingUpdate { get; set; }

        public IEnumerable<SelectListItem> FindingOrderTypeList { get; set; }
        public List<FindingOrderPerson> FindingOrderPersonList { get; set; }
        public List<FindingOrderNotice> FindingOrderNoticeList { get; set; }        
    }    
}