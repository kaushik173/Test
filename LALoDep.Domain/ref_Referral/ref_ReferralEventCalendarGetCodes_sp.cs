namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralEventCalendarGetCodes_spParams
    {
        public string CodeType { get; set; }
        public string LoadOption { get; set; }
        public int UserID { get; set; }
    }
    public class ref_ReferralEventCalendarGetCodes_spResult
    {
        public ref_ReferralEventCalendarGetCodes_spResult()
        {
        }
        public int? ReferralTypeCodeID { get; set; }
        public int? CodeID { get; set; }
        public string CodeDisplay { get; set; }
    }
}
