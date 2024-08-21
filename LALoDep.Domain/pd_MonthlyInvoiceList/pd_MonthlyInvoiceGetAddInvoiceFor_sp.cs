namespace LALoDep.Domain.pd_MonthlyInvoiceList
{
    public class pd_MonthlyInvoiceGetAddInvoiceFor_spParams
    {
        public string LoadOption { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class pd_MonthlyInvoiceGetAddInvoiceFor_spResult
    {
         
        public int? PersonID { get; set; }
        public string PersonNameDisplay { get; set; }
    }
}
