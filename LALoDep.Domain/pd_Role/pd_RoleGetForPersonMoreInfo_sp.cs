namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetForPersonMoreInfo_spParams
    {
        public int? CaseID { get; set; }
        public int? RoleID { get; set; }
        public int? PersonID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class pd_RoleGetForPersonMoreInfo_spResult
    {
        public int? DisplayOrder { get; set; }
        public string Role { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Language { get; set; }

    }
}