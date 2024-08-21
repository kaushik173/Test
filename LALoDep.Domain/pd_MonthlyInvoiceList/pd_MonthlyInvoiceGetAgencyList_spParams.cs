using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MonthlyInvoiceList
{
    public class pd_MonthlyInvoiceGetAgencyList_spParams
    {
        public string LoadOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_MonthlyInvoiceGetCountyList_spParams
    {
        public string LoadOption { get; set; }

        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
