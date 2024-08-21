using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Leave
{
    public class pd_LeaveInsert_spParams
    {

        public int? LeaveID { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public DateTime? LeaveStartDateTime { get; set; }
        public DateTime? LeaveEndDateTime { get; set; }
        public int? LeaveTypeCodeID { get; set; }
        public int? RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
