using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Case
{
    public class FindingsAndOrdersListViewModel
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

        public List<FindingAndOrderListModel> FindingAndOrderList { get; set; }
        public FindingsAndOrdersListViewModel()
        {
            FindingAndOrderList = new List<FindingAndOrderListModel>();
        }
    }

    public class FindingAndOrderListModel
    {
        public string HearingFindingOrderID { get; set; }

        public string HearingFindingOrderCodeValue { get; set; }
        public string Person { get; set; }
        public string Notices { get; set; }
        public string HearingFindingOrderComment { get; set; }
    }
}