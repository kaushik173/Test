namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralStatusGetHistory_spParams
    {
        public int? ReferralID { get; set; }
        public int? UserID { get; set; }

    }


    public class ref_ReferralStatusGetHistory_spResult
    {
        public ref_ReferralStatusGetHistory_spResult()
        {
        }
        public int? ReferralStatusID { get; set; }
        public int? ReferralStatusCodeID { get; set; }
        public string ReferralStatusDate { get; set; }
        public string SortDate { get; set; }
    }
}
