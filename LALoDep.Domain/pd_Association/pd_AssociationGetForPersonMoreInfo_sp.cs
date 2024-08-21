namespace LALoDep.Domain.pd_Association
{
    public class pd_AssociationGetForPersonMoreInfo_spParams
    {
        public int? CaseID { get; set; }
        public int? RoleID { get; set; }
        public int? PersonID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class pd_AssociationGetForPersonMoreInfo_spResult
    {

        public string PersonDisplay { get; set; }
        public string Association { get; set; }
        public string ContactType { get; set; }
        public string ContactInfo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}