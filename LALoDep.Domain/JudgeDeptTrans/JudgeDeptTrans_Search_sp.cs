
using System;

namespace LALoDep.Domain.JudgeDeptTrans
{
    public class JudgeDeptTrans_Search_spParams
    {
        public int? AgencyGroupID { get; set; }
        public int? AgencyID { get; set; }
        public int? AttorneyPersonID { get; set; }
        public int? JudgePersonID { get; set; }
        public int? DeptCodeID { get; set; }
        public DateTime? HearingStartDate { get; set; }
        public DateTime? HearingEndDate { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class JudgeDeptTrans_Search_spResult
    {
        public JudgeDeptTrans_Search_spResult()
        {
        }
        public string TransferGUID { get; set; }
        public string AssignedAttorney { get; set; }
        public int? CaseTotal { get; set; }
        public int? HearingTotal { get; set; }
    }
}
