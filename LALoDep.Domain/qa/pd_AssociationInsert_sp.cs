namespace LALoDep.Domain.qa
{
    public class pd_AssociationInsert_spParams
    {
        public int? AssociationID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? PersonID { get; set; }
        public int? RelatedPersonID { get; set; }
        public int? AssociationCodeID { get; set; }
        public System.DateTime? AssociationStartDate { get; set; }
        public System.DateTime? AssociationEndDate { get; set; }
        public int? RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

}