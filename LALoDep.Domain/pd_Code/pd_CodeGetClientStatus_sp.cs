namespace LALoDep.Domain.pd_Code
{
    public class pd_CodeGetClientStatus_spParams
    {
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_CodeGetClientStatus_spResult
    {
       
        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
    }
}
