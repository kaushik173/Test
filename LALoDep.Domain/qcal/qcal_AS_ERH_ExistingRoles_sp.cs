namespace LALoDep.Domain.qcal
{
    public class qcal_AS_ERH_ExistingRoles_spParams
    {
        public int? CaseAgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class qcal_AS_ERH_ExistingRoles_spResult
    {
        public qcal_AS_ERH_ExistingRoles_spResult()
        {
        }
        public string FullName { get; set; }
        public int? PersonID { get; set; }
    }
}
