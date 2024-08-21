using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Appeals
{
    public class pd_AppealGetByPetitionID_spResult
    {
        public int AppealID { get; set; }
        public DateTime? AppealFileDate { get; set; }
        public DateTime? AppealOralArgumentDate { get; set; }
        public string AppealDocketNumber { get; set; }
        public DateTime? AppealDecisionDate { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string DecisionCodeValue { get; set; }
        public string AppealTypeCodeValue { get; set; }
        public string PetitionDocketNumber{ get; set; }
        public int? DecisionID { get; set; }
    }
}
