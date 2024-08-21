using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.CSEC
{
    public class CSECUpdateNote_spParams
    {
        public int CSECID { get; set; }
        public string Note { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }    
}
