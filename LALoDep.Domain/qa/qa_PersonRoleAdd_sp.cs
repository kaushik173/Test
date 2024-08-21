namespace LALoDep.Domain.qa
{
    public class qa_PersonRoleAdd_spParams
    {
        public string AddMode { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameMiddle { get; set; }
        public System.DateTime? PersonDOB { get; set; }
        public int? PersonRaceCodeID { get; set; }
        public int? PersonSexCodeID { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public System.DateTime? RoleStartDate { get; set; }
        public byte? RoleClient { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class qa_PersonRoleAdd_spResult
    {
        public int? PersonID { get; set; }
        public int? PersonNameID { get; set; }
        public int? RoleID { get; set; }
        public int? AssociationID { get; set; }

    }
}