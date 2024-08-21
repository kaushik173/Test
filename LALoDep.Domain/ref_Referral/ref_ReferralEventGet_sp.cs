namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralEventGet_spParams
    {
        public int? ReferralEventID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class ref_ReferralEventGet_spResult
    {
        public ref_ReferralEventGet_spResult()
        {


        }
        public int? ReferralEventID { get; set; }
        public int? ReferralID { get; set; }
        public System.Nullable<System.DateTime> ReferralEventDateTime { get; set; }
        public int? ReferralEventTypeCodeID { get; set; }
        public int? ReferralEventLocationCodeID { get; set; }
        public int? ReferralEventAppearingPersonID { get; set; }
        public short? ReferralEventClientPresentFlag { get; set; }
        public string ReferralEventNote { get; set; }
        public byte? RecordStateID { get; set; }
    }
}
