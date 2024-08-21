namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralTypeList_spParams
    {
        public int? CaseID { get; set; }
        public int? ReferralID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
    }

    public class ref_ReferralTypeList_spResult
    {

        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
        public string NG_NavigationURL { get; set; }
    }
}
