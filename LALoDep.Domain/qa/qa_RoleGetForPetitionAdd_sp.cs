namespace LALoDep.Domain.qa
{
    public class qa_RoleGetForPetitionAdd_spParams
    {
        public string LoadMode { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class qa_RoleGetForPetitionAdd_spResult
    {
        public int? Selected { get; set; }
        public int? RoleID { get; set; }
        public string RoleDisplay { get; set; }
        public string NameDisplay { get; set; }
        public string RoleStartDate { get; set; }
        public int? DefaultAssociationTypeCodeID { get; set; }
        public int? PersonID { get; set; }
        public int? Sort1 { get; set; }

    }
}