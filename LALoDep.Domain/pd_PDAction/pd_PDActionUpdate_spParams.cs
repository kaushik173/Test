using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.PD_PDAction
{
    public class pd_PDActionUpdate_spParams
    {
        public int PDActionID { get; set; }
        public int? AgencyID { get; set; }
        public int? BranchID { get; set; }
        public int? CaseID { get; set; }
        public int? AssignedToPersonID { get; set; }
        public int? ActionTypeCodeID { get; set; }
        public string ActionNote { get; set; }
        public int? ActionStatusCodeID { get; set; }
        public string ActionStatusDate { get; set; }
        public string ActionDueDate { get; set; }
        public string ActionReminderDate { get; set; }
        public int? ActionAssociatedToEntityID { get; set; }
        public int? ActionAssociatedToEntityTypeCodeID { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? PersonID { get; set; }
    }
}
