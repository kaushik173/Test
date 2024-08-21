
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Appeals
{
    public class pd_AppealGet_spResults
    {
        public int? AppealID { get; set; }
        public int? AgencyID { get; set; }
        public int? PetitionID { get; set; }
        public DateTime? AppealFileDate { get; set; }
        public int? AppealTypeCodeID { get; set; }
        public DateTime? AppealOralArgumentDate { get; set; }
        public int? AttorneyRoleID { get; set; }
        public string AppealDocketNumber { get; set; }
        public int? DecisionCodeID { get; set; }
        public DateTime? AppealDecisionDate { get; set; }
        public Int16? RecordStateID { get; set; }
        public string AppealTypeCodeValue { get; set; }
        public string NoteEntry { get; set; }
        public int? NoteID { get; set; }
    }
}
