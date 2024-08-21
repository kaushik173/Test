namespace LALoDep.Models.Inquiry
{
    public class HearingListViewModel
    {
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string Result { get; set; }
        public string Clients { get; set; }
        public string Department { get; set; }
        public string HearingType { get; set; }
        public string Petitions { get; set; }
        public string EncryptedCaseID { get; set; }
        public string EncryptedHearingID { get; set; }
        public int? HearingID { get; set; }
        public string hearingIDList { get; set; }
        public string QHE { get; set; }
        public string FillInFor { get; set; }
        
    }
}