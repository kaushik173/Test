using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeExpenseHeader_spParams
    {
        public int? ErrorID { get; set; }
        public int? InvoiceID { get; set; }
        public int? UserID { get; set; }
    }
    public class TitleIVeExpenseHeader_spResult
    {
        public string CourtSystem { get; set; }
        public string InvoicePeriod { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceID { get; set; }
    }
}
