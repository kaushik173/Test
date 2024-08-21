namespace LALoDep.Domain.pd_Profile
{
    public class pd_ProfileQuestionGetAllByQuestionIDRoleID_spResult
    {

        public string ProfileQuestionEntry { get; set; }
        public short FreeformQuestion { get; set; }
        public int? ProfileAnswerID { get; set; }
        public int? ProfileID { get; set; }
        public string Answer { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public System.DateTime? ProfileDate { get; set; }
        public string ProfileFreeformAnswer { get; set; }
        public int? Note { get; set; }
        public int? ProfileInsertedByUserID { get; set; }

        public string ProfileInsertedOnDateTime { get; set; }
    }
}

