using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingOfficerTransfer_spParams
    {
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int? FromPersonID { get; set; }
        public int? ToPersonID { get; set; }
        public short? ReturnHearings { get; set; }
        public int? PersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? HearingCourtDepartmentCodeID { get; set; }
    }
}
