using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_InvoiceQueue
{
    public class pd_InvoiceGetPending_spParams
    {
        public int UserID { get; set; }
        public int? BranchCodeID { get; set; }
        public int? AgencyID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
