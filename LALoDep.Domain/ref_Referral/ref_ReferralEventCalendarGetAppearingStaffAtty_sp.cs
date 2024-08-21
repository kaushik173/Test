namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralEventCalendarGetAppearingStaffAtty_spParams
    {
        public string LoadOption { get; set; }
        public int UserID { get; set; }
    }
    public class ref_ReferralEventCalendarGetAppearingStaffAtty_spResult
    {
        public ref_ReferralEventCalendarGetAppearingStaffAtty_spResult()
        {
        }
        public int? ReferralTypeCodeID { get; set; }
        public string DisplayName { get; set; }
        public int? PersonID { get; set; }
    }
}
