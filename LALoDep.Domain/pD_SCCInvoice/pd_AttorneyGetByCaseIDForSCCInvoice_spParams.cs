using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pD_SCCInvoice
{
    public class pd_AttorneyGetByCaseIDForSCCInvoice_spParams
    {
        public int AttorneyPersonID { get; set; }
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_AttorneyGetByCaseIDForSCCInvoice_spResult
    {
        public int AttorneyPersonID { get; set; }
        public string Attorney { get; set; }
    }
}
