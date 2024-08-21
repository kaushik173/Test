using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.CSEC
{
    public class CSECUpdateAnswer_spParams
    {
        public int CSECID { get; set; }
        public string QuestionEnum { get; set; }
        public short? Value { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }    
}
