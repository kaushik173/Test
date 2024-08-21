using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MonthlyInvoiceList
{
    public class pd_MonthlyInvoiceSearch_spResult
    {
        public string County { get; set; }
        public string Attorney { get; set; }
        public string YearMonth { get; set; }
        public int InvoiceNumber { get; set; }
        public string InvoiceAmount { get; set; }
        public string SubmitDate { get; set; }
        public string Status { get; set; }
        public string StatusDate { get; set; }
        public int InvoiceMonthlyID { get; set; }
        public int AllowUpdateFlag { get; set; }
    }
}
