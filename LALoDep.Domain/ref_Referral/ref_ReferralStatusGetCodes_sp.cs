namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralStatusGetCodes_spParams
    {
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? ReferralID { get; set; }
        public string LoadOption { get; set; }
        public int? UserID { get; set; }

    }


    public class ref_ReferralStatusGetCodes_spResult
    {
        public ref_ReferralStatusGetCodes_spResult()
        {
        }
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
    }
}
