using System;

namespace LALoDep.Domain.PD_PDAction
{
    public class pd_ProfileInsert_spParams
    {
        public int ProfileID { get; set; }
        public int AgencyID { get; set; }
        public int RoleID { get; set; }
        public int ProfileQuestionID { get; set; }
        public int ProfileAnswerID { get; set; }
        public string ProfileFreeformAnswer { get; set; }
        public DateTime ProfileDate { get; set; }
        public int HearingReportFilingDueID { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}