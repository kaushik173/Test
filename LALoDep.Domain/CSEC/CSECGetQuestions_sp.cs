using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.CSEC
{
    public class CSECGetQuestions_spParams
    {
        public int CSECID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class CSECGetQuestions_spResult
    {
        public int GroupID { get; set; }
        public string GroupDisplay { get; set; }
        public string QuestionEnum { get; set; }
        public string Question { get; set; }
    }
}
