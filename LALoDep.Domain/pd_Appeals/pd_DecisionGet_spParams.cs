
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Appeals
{
    public class pd_DecisionGet_spParams
    {
        public int DecisionID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
}
