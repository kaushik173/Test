namespace LALoDep.Domain.pd_CaseLoad
{
    public class MyCaseloadGetCodes_spParams
    {
        public int? CaseloadPersonID { get; set; }
        public string CodeType { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class MyCaseloadGetCodes_spResult
    {
         
        public int? CodeID { get; set; }
        public string CodeDisplay { get; set; }
        public string CodeType { get; set; }
        public int? DefaultFlag { get; set; }
    }
}
