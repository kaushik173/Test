using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Work
{
    public class pd_WorkGet_spParams
    {
        public int WorkID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        
    }
}
