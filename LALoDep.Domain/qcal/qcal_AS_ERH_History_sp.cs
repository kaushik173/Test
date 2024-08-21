namespace LALoDep.Domain.qcal
{
    public class qcal_AS_ERH_History_spParams
    {
        public int? CaseAgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class qcal_AS_ERH_History_spResult
    {
        public qcal_AS_ERH_History_spResult()
        {
        }
        public int? AssociationID { get; set; }
        public string Child { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string EHRName { get; set; }
        public string Role { get; set; }
        public string SortDate { get; set; }
    }
}
