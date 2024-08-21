namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVePendingInvoiceGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? UserID { get; set; }

    }


    public class TitleIVePendingInvoiceGet_spResult
    {

        public string DisplayAgencyGroup { get; set; }
        public string County { get; set; }
        public string DisplayInvoiceMonth { get; set; }
        public int? DisplayInvoiceYear { get; set; }
        public string ContactorSigned { get; set; }
        public string ContactorSigning { get; set; }
        public decimal AmountDue { get; set; }
        public int? AgencyGroup { get; set; }
        public int? AgencyCountyID { get; set; }
        public int? InvoiceYear { get; set; }
        public int? InvoiceMonth { get; set; }
        public string InvoiceStatus { get; set; }
    }
}
