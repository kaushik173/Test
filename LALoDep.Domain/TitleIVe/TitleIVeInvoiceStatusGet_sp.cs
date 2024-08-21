namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeInvoiceStatusGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? AgencyGroupID { get; set; }
        public int? AgencyCountyID { get; set; }
        public int? InvoiceYear { get; set; }
        public int? InvoiceMonth { get; set; }
        public int? TitleIVeInvoiceID { get; set; }
        public string StatusType { get; set; }
        public int? UserID { get; set; }

    }


    public class TitleIVeInvoiceStatusGet_spResult
    {
       
        public int? TitleIVeInvoiceID { get; set; }
        public string StatusDescription { get; set; }
        public System.Nullable<System.DateTime> InvoiceStatusStartDate { get; set; }
        public System.Nullable<System.DateTime> InvoiceStatusEndDate { get; set; }
        public string AgencyGroup { get; set; }
        public int? InvoiceYear { get; set; }
        public int? InvoiceMonth { get; set; }
        public int? AgencyCountyID { get; set; }
        public string AgencyCounty { get; set; }
        public string InvoiceID { get; set; }
        public System.Nullable<System.DateTime> InvoiceDate { get; set; }
    }
}
