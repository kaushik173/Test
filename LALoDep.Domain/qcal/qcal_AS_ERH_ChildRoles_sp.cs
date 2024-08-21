namespace LALoDep.Domain.qcal
{
    public class qcal_AS_ERH_ChildRoles_spParams
    {
        public int? CaseAgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class qcal_AS_ERH_ChildRoles_spResult
    {
        public qcal_AS_ERH_ChildRoles_spResult()
        {
        }
        public string ChildDisplay { get; set; }
        public int? ChildPersonID { get; set; }

        public int? AssociationToChildID { get; set; }
    }
}
