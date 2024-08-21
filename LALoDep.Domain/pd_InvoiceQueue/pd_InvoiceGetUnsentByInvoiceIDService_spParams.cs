using System;

namespace LALoDep.Domain.pd_InvoiceQueue
{
    public class pd_InvoiceGetUnsentByInvoiceIDService_spParams
    {
        public int UserID { get; set; }
        public int InvoiceID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_InvoiceGetUnsentByInvoiceIDService_spResult
    {
        public int InvoiceID { get; set; }
        public int ClientID { get; set; }
        public string EndDate { get; set; }
        public string StartDate { get; set; }
        public string ServiceType { get; set; }
        public string Amount { get; set; }
        public string Indicator { get; set; }
        public int? Identifier { get; set; }
        public int? AgencyID { get; set; }
        public int? HearingID { get; set; }
    }
}
