using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MonthlyInvoiceList
{
    public class pd_MonthlyInvoiceGet_spParams
    {
        public int InvoiceMontlyID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_MonthlyInvoiceGet_spResult
    {   
        public string SummaryDisplay { get; set; }
        public int? InvoiceMonthlyID { get; set; }
        public string SubmitDate { get; set; }
        public string StatusDate { get; set; }
        public int? StatusCodeID { get; set; }        
        public string PaymentDate { get; set; }
        public string PaymentNumber { get; set; }
    }
}
