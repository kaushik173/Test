namespace LALoDep.Domain.qcal
{
    public class qcal_AS_ERH_NewAssociationTypes_spParams
    {
        public int? CaseAgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class qcal_AS_ERH_NewAssociationTypes_spResult
    {
        public qcal_AS_ERH_NewAssociationTypes_spResult()
        {
        }
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
    }
}
