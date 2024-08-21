using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_HourlyInvoiceList
{
    public class pd_HourlyInvoiceSearch_spParams
    {
        public int? PersonID { get; set; }
        public int? AgencyCountyID { get; set; }
        public string HourlyInvoiceID { get; set; }
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
