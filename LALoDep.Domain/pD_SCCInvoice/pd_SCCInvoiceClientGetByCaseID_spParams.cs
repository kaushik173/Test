using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pD_SCCInvoice
{
    public class pd_SCCInvoiceClientGetByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int SCCInvoiceID{ get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
