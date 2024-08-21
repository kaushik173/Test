namespace LALoDep.Domain.pd_Person
{
    public class pd_PersonClassificationGetCodes_spParams
    {
        public string CodeTypeEnum { get; set; }
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_PersonClassificationGetCodes_spResult
    {
        
        public int? CodeID { get; set; }
        public string CodeDisplay { get; set; }
    }
}
