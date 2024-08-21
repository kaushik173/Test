namespace LALoDep.Domain.pd_Address
{
    public class pd_AddressGetForPersonMoreInfo_spParams
    {
        public int? CaseID { get; set; }
        public int? RoleID { get; set; }
        public int? PersonID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class pd_AddressGetForPersonMoreInfo_spResult
    {
        public string AddressType { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCSZ { get; set; }
        public string AddressPhone { get; set; }
        public string AddressStartDate { get; set; }
        public string AddressEndDate { get; set; }
        public string AddressComment { get; set; }
        public int? OpenAddress { get; set; }

    }
}