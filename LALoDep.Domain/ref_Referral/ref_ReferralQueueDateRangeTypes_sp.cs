namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralQueueDateRangeTypes_spParams
    {
        public int? DateRangeCodeID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class ref_ReferralQueueDateRangeTypes_spResult
    {
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
        public int? Selected { get; set; }

    }
}
