using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_HourlyInvoiceList
{
    public class pd_HourlyInvoiceSearch_spResult
    {
        public int AgencyCountyID { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceAmount { get; set; }
        public string Attorney { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalAmount { get; set; }
        public int HourlyInvoiceID { get; set; }
    }
}
