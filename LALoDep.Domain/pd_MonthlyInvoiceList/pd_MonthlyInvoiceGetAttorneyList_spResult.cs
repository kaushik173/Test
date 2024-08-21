using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MonthlyInvoiceList
{
    public class pd_MonthlyInvoiceGetAttorneyList_spResult
    {
        public int PersonID { get; set; }
        public string PersonNameDisplay { get; set; }
    }
    public class pd_MonthlyInvoiceGetCountyList_spResult
    {
        public int AgencyCountyID { get; set; }
        public string CountyDisplay { get; set; }
    }

    
}
