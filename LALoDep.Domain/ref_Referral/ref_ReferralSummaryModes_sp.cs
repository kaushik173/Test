namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralSummaryModes_spParams
    {
        public int? ModeCodeID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class ref_ReferralSummaryModes_spResult
    {
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
        public int? Selected { get; set; }

    }
}
