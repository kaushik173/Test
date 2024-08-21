namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralForClientList_spParams
    {
        public int? CaseID { get; set; }
        public int? ReferralID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
    }

    public class ref_ReferralForClientList_spResult
    {
        public int? RoleID { get; set; }
        public string NameDisplay { get; set; }
    }
}
