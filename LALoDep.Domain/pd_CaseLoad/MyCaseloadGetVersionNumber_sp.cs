namespace LALoDep.Domain.pd_CaseLoad
{
    public class MyCaseloadGetVersionNumber_spParams
    {
        public int? CaseloadPersonID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class MyCaseloadGetVersionNumber_spResult
    {

        public int? MyCaseloadVersionNumber { get; set; }

        public int? MyCaseloadRoleTypeCodeID { get; set; }
    }
}
