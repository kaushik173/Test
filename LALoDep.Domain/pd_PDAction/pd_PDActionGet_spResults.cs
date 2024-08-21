using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.PD_PDAction
{
    public class pd_PDActionGet_spResults
    {
        public int PDActionID { get; set; }
        public int? ActionTypeCodeID { get; set; }
        public int? AssignedToPersonID { get; set; }
        public int? CaseID { get; set; }
        public string ActionDueDate { get; set; }
        public string ActionReminderDate { get; set; }
        public string ActionNote { get; set; }

        public string ActionStatusDate { get; set; }
        public int? AgencyID { get; set; }
        public int? BranchID { get; set; }
        public int? ActionStatusCodeID { get; set; }
        public int? ActionAssociatedToEntityID { get; set; }
        public int? ActionAssociatedToEntityTypeCodeID { get; set; }
        public int? PersonID { get; set; }
        public byte? RecordStateID { get; set; }
    }
}
