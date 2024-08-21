using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MonthlyInvoiceList
{
    public class pd_MonthlyInvoiceInsert_spParams
    {
        public int? InvoiceMonthlyID { get; set; }
        public int? InvoiceMonthlyRateID { get; set; }
        public int? AgencyID { get; set; }
        public int? AttorneyPersonID { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? SubmitDate { get; set; }
        public int SubmitYear { get; set; }
        public int SubmitMonth { get; set; }
        public int? StatusCodeID { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentNumber { get; set; }
        public string ClientPersonIDList { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
