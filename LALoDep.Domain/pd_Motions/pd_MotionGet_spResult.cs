using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Motions
{
    public class pd_MotionGet_spResult
    {
        public int? MotionID { get; set; }
        public int? AgencyID { get; set; }
        public int? PetitionID { get; set; }
        public int?  MotionDecisionCodeID { get; set; }
        public DateTime? MotionFileDate { get; set; }
        public DateTime? MotionFilingDueDate { get; set; }
        public int? MotionTypeCodeID { get; set; }
        public DateTime? MotionDecisionDate { get; set; }
        public int? MotionRequestByCodeID { get; set; }
        public Int16? RecordStateID { get; set; }
        public string MotionTypeCodeValue { get; set; }
        public string MotionDecisionCodeValue { get; set; }
    }
}
