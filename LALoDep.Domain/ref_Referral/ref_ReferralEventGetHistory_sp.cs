namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralEventGetHistory_spParams
    {
        public int? ReferralID { get; set; }
        public string LoadOption { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
    }


    public class ref_ReferralEventGetHistory_spResult
    {
        public ref_ReferralEventGetHistory_spResult()
        {
        }
        public int? ReferralEventID { get; set; }
        public string EventDateTime { get; set; }
        public string EventType { get; set; }
        public string EventLocation { get; set; }
        public string AppearingAttorney { get; set; }
        public string ClientPresent { get; set; }
        public string NoteDisplay { get; set; }
        public string SortDateTime { get; set; }
    }
}
