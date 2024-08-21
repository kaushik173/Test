namespace LALoDep.Domain.pd_Case
{
    public class pd_CaseSearch_spParams
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocketNumber { get; set; }
        public string CaseNumber { get; set; }
        public short? Appointment { get; set; }
        public string HHSA { get; set; }
        public System.Guid? GUID { get; set; }
        public int? StartRecord { get; set; }
        public int? Range { get; set; }
        public int? SortID { get; set; }
        public int? TotalRecords { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public int? AgencyID { get; set; }

    }

}
