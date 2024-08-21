namespace LALoDep.Domain.qa
{
    public class qa_HearingGetForPetitionAdd_spParams
    {
        public string LoadMode { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class qa_HearingGetForPetitionAdd_spResult
    {
        public int? Selected { get; set; }
        public int? HearingID { get; set; }
        public string HearingDate { get; set; }
        public string HearingType { get; set; }
        public string HearingDept { get; set; }
        public string Sort1 { get; set; }
    }
}