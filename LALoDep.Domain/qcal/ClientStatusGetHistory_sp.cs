namespace LALoDep.Domain.qcal
{
    public class ClientStatusGetHistory_spParams
    {
        public int? PersonID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class ClientStatusGetHistory_spResult
    {
       
        public string PersonNameDisplay { get; set; }
        public int? PersonID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ClientStatus { get; set; }
        public string SortBy { get; set; }
    }
}
