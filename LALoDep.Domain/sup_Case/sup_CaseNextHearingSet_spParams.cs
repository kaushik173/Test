using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.sup_Case
{
    public class sup_CaseNextHearingSet_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        //public int? AdminFlag { get; set; }
    }
}
