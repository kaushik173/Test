using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_HourlyInvoiceList
{
    public class pd_HourlyInvoiceGetAttorneyList_spParams
    {
        public string LoadOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
