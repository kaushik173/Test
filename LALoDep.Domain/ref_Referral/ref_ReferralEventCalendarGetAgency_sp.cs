namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralEventCalendarGetAgency_spParams
    {
        public string LoadOption { get; set; }
        public int UserID { get; set; }
    }
    public class ref_ReferralEventCalendarGetAgency_spResult
    {
        public ref_ReferralEventCalendarGetAgency_spResult()
        {
        }
        public string AgencyDisplay { get; set; }
        public int? AgencyID  { get; set; }
        public int? HomeAgencyFlag { get; set; }
    }
}
