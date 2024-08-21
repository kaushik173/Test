namespace LALoDep.Domain.NgInvoice
{
    public class InvoiceCreateExtractFileForPaymentParams
    {
        public int? YearQuarterID { get; set; }
        public int? UserID { get; set; }
        public int? InvoiceID { get; set; }
        public int? InvoiceCounter { get; set; }

    }


    public class InvoiceCreateExtractFileForPaymentResult
    {
        public InvoiceCreateExtractFileForPaymentResult()
        {
        }
        public string DATA { get; set; }
    }
}
