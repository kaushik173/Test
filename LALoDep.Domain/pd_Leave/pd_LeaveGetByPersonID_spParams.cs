using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Leave
{
    public class pd_LeaveGetByPersonID_spParams
    {
        public int PersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_LeaveGetByPersonID_spResult
    {
        public DateTime? LeaveStartDateTime { get; set; }
        public int? LeaveID { get; set; }
        public string LeaveType { get; set; }
        public string LeaveDateTimeDisplay { get; set; }
        public string AgencyAttorneyLastName { get; set; }
        public string AgencyAttorneyFirstName { get; set; }
        public int? AgencyAttorneyID { get; set; }
    }
}
