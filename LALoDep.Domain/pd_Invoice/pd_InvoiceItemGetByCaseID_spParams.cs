using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Invoice
{
    public class pd_InvoiceItemGetByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_InvoiceItemGetByCaseID_spResult
    {
        public int InvoiceID { get; set; }
        public DateTime? InvoiceDateTime { get; set; }
        public string ItemDescription { get; set; }
        public string ItemType { get; set; }
        public decimal? InvoiceHearingAmount { get; set; }
        public int? HearingID { get; set; }
        public int? HearingExpenseID { get; set; }
    }
}
