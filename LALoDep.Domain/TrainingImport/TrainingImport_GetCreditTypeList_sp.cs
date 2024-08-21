namespace LALoDep.Domain.TrainingImport
{
    public class TrainingImport_GetCreditTypeList_spParams
    {
        public int? AgencyGroupID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class TrainingImport_GetCreditTypeList_spResult
    {
        public TrainingImport_GetCreditTypeList_spResult()
        {
        }
        public string CreditType { get; set; }
        public int? CreditTypeCodeID { get; set; }
    }
}
