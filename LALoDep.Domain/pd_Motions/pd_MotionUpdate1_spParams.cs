using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Motions
{
    public class pd_MotionUpdate1_spParams
    {
        public int? MotionID { get; set; }
        public int? AgencyID { get; set; }
        public int? PetitionID { get; set; }
        public int? DecisionID { get; set; }
        public DateTime MotionFileDate { get; set; }
        public DateTime? MotionFilingDueDate { get; set; }
        public int? MotionTypeCodeID { get; set; }
        public DateTime? MotionDecisionDate { get; set; }
        public int? MotionRequestByCodeID { get; set; }
        public int? RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int? UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
