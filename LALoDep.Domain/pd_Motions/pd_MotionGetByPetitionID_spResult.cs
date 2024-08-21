using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Motions
{
    public class pd_MotionGetByPetitionID_spResult
    {
        public int MotionID { get; set; }
        public int AgencyID { get; set; }
        public int PetitionID { get; set; }
        public string MotionFileDate { get; set; }
        public DateTime? MotionDecisionDate { get; set; }
        public string MotionDecisionCodeValue { get; set; }
        public string MotionTypeCodeValue { get; set; }
        public string MotionRequestByCodeValue{ get; set; }
    }
}
