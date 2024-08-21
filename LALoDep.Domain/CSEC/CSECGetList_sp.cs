using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.CSEC
{
    public class CSECGetList_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class CSECGetList_spResult
    {
        public int CSECID { get; set; }
        public string Child { get; set; }
        public string DueDate { get; set; }
        public string CompletionDate { get; set; }
        public string AssignedTo { get; set; }
        public short? ScoreNumeric { get; set; }
        public string ScoreLiteral { get; set; }
        public int CanEditFlag { get; set; }
        public int CanDeleteFlag { get; set; }
        public string SortBy { get; set; }
    }
}
