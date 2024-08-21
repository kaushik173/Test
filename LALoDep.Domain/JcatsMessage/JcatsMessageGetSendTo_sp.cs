namespace LALoDep.Domain.JcatsMessage
{
    public class JcatsMessageGetSendTo_spParams
    {
        public int? CaseID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class JcatsMessageGetSendTo_spResult
    {
        public string NameDisplay { get; set; }
        public string RoleDisplay { get; set; }
        public string SendTo { get; set; }
        public int? RoleID { get; set; }
        public byte? RoleClient { get; set; }

    }
}