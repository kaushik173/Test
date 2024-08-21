using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MonthlyInvoiceList
{
    public class pd_MonthlyInvoiceSearch_spParams
    {
        
       public int? AgencyCountyID { get; set; }
        public int AgencyID { get; set; }
        public int AttorneyPersonID { get; set; }
        public int? InvoiceMonthlyID { get; set; }
        public int InvoiceMonthlyStatusCodeID { get; set; }
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
