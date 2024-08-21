namespace LALoDep.Domain.JcatsMessage
{
    public class JcatsMessageGetPermissions_spParams
    {
        public int? CaseID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class JcatsMessageGetPermissions_spResult
    {
        public JcatsMessageGetPermissions_spResult()
        {
        }
        public int? DisableTextingFlag { get; set; }
        public string DisableTextingAlert { get; set; }
    }
}
