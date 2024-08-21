namespace LALoDep.Domain.qa
{
    public class qa_RoleCodes_spParams
    {
        public string LoadMode { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class qa_RoleCodes_spResult
    {
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }

    }
}