namespace LALoDep.Domain.qa
{
    public class qa_PetitionGetForAdultPartyAdd_spParams
    {
        public string LoadMode { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

    public class qa_PetitionGetForAdultPartyAdd_spResult
    {
        public int? Selected { get; set; }
        public int? PetitionID { get; set; }
        public int? ChildPersonID { get; set; }
        public string ChildDisplay { get; set; }
        public string PetitionDocketNumber { get; set; }
        public string PetitionFileDate { get; set; }
        public string PetitionType { get; set; }
        public int? ShowAssocationFlag { get; set; }
        public int? DefaultAssocationTypeCodeID { get; set; }
    }
}