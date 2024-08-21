namespace LALoDep.Domain.pd_LegalNumber
{
    public class pd_LegalNumberValidateRelatedNumber_spParams
    {
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? LegalNumberTypeCodeID { get; set; }
        public string LegalNumberEntry { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_LegalNumberValidateRelatedNumber_spResult
    {
       
        public int? ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
