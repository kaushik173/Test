using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Appeals
{
    public class pd_DecisionUpdate_spParams
    {
        public int? DecisionID { get; set; }
        public int? AppealID { get; set; }
        public int? MotionDecisionCodeID { get; set; }
        public string DecisionDate { get; set; }
        public int? AgencyID { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
