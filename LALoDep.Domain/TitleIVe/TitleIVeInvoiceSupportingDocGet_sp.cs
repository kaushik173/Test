namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeInvoiceSupportingDocGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? InvoiceID { get; set; }
        public string DocumentType { get; set; }
        public int? TitleIVeItemID { get; set; }
        public int? UserID { get; set; }

    }


    public class TitleIVeInvoiceSupportingDocGet_spResult
    {
        public int? TitleIVeInvoiceSupportingDocID { get; set; }


        public int? TitleIVeInvoiceID { get; set; }
        public string DocumentName { get; set; }
        public string Path { get; set; }
        public string Note { get; set; }
    }
}
