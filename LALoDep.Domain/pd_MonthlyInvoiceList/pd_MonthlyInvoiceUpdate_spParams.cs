using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MonthlyInvoiceList
{
    public class pd_MonthlyInvoiceUpdate_spParams
    {
        public int? InvoiceMonthlyID { get; set; }
        public int? StatusCodeID { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentNumber { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
