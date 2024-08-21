using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class FindingsAndOrdersAddViewModel
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

        [Display(Name = "Note")]
        public string NoteEntry { get; set; }
        public int? NoteID { get; set; }
        public string NoteSubject { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        public int buttonID { get; set; }

        public IEnumerable<SelectListItem> FindingOrderTypeList { get; set; }
        public List<FindingOrderPerson> FindingOrderPersonList { get; set; }
        public List<FindingOrderNotice> FindingOrderNoticeList { get; set; }

        public List<FindingsAndOrdersAddSaveModel> FindingsAndOrders { get; set; }        

    }

    public class FindingOrderPerson
    {
        public string PersonDisplayName { get; set; }
        public int? PersonID { get; set; }
        public int? HearingFindingOrderPersonID { get; set; }
        public int? RoleID { get; set; }
        public bool Selected { get; set; }
        public string RoleTypeCodeValue { get; set; }
        public bool IsRoleClient { get; set; }
    }

    public class FindingOrderNotice
    {
        public int? HearingFindingOrderNoticeID { get; set; }
        public int? NoticeID { get; set; }
        public string Notice { get; set; }
        public bool Selected { get; set; }
    }

    public class FindingsAndOrdersAddSaveModel
    {
        public int CodeHearingFindingOrderID { get; set; }
        public string HearingFindingOrderComment { get; set; }
        public List<FindingOrderPerson> FindingOrderPersonList { get; set; }
        public List<FindingOrderNotice> FindingOrderNoticeList { get; set; }

    }
}