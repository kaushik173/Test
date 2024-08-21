using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Appeals
{
    public class pd_AppealUpdate_spParams
    {
        public int AppealID { get; set; }
        public int? AgencyID { get; set; }
        public int? PetitionID { get; set; }
        public string AppealFileDate { get; set; }
        public int? AppealTypeCodeID { get; set; }
        public string AppealOralArgumentDate { get; set; }
        public int? AttorneyRoleID { get; set; }
        public string AppealDocketNumber { get; set; }
        public int? MotionDecisionCodeID { get; set; }
        public string AppealDecisionDate { get; set; }
        public int? RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
}
