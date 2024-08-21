using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Leave
{
    public class pd_LeaveGet_spParams
    {
        public int LeaveID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_LeaveGet_spResult
    {
        public int? LeaveID { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public string LeaveStartDateTime { get; set; }
        public string LeaveEndDateTime { get; set; }
        public int? LeaveTypeCodeID { get; set; }
        public short? RecordStateID { get; set; }
        public string LeaveTypeCodeValue { get; set; }

    }
}
