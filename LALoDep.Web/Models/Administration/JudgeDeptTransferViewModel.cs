using LALoDep.Domain.JudgeDeptTrans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Administration
{
    public class JudgeDeptTransferViewModel
    {
        public int? AgencyGroupID { get; set; }
        public int? AgencyID { get; set; }
        public int? AttorneyPersonID { get; set; }
        public int? JudgePersonID { get; set; }
        public int? DeptCodeID { get; set; }
        public int? UpdateJudgePersonID { get; set; }
        public int? UpdateDeptCodeID { get; set; }

        public string HearingStartDate { get; set; }
        public string HearingEndDate { get; set; }
        public string TransferGUID { get; set; }


        public IEnumerable<SelectListItem> AgencyGroupList { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }
        public IEnumerable<SelectListItem> AttorneyPersonList { get; set; }
        public IEnumerable<SelectListItem> JudgePersonList { get; set; }
        public IEnumerable<SelectListItem> DeptCodeList { get; set; }
        public IEnumerable<SelectListItem> UpdateJudgePersonList { get; set; }
        public IEnumerable<SelectListItem> UpdateDeptCodeList { get; set; }
        public List<JudgeDeptTrans_Search_spResult> JudgeDeptTransSearchResult { get; set; }

        public JudgeDeptTransferViewModel()
        {
            AgencyGroupList = new List<SelectListItem>();
            AgencyList = new List<SelectListItem>();
            AttorneyPersonList = new List<SelectListItem>();
            JudgePersonList = new List<SelectListItem>();
            DeptCodeList = new List<SelectListItem>();
            UpdateJudgePersonList = new List<SelectListItem>();
            UpdateDeptCodeList = new List<SelectListItem>();
            JudgeDeptTransSearchResult = new List<JudgeDeptTrans_Search_spResult>();

        }












    }
}