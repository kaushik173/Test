namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralContactInfo_spParams
    {
        public int? CaseID { get; set; }
        public int? ReferralID { get; set; }
        public int? RoleID { get; set; }
        public int? ReferralTypeCodeID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class ref_ReferralContactInfo_spResult
    {
        public ref_ReferralContactInfo_spResult()
        {
        }
        public string RoleDisplay { get; set; }
        public string NameDisplay { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
