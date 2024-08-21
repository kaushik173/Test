using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.rpt_Print
{
    public class rpt_IndividualCalendar_spParams
    {
        public int PersonID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? HearingTypeID { get; set; }
        public int? OtherPersonID { get; set; }
        public string OtherPersonRoleType { get; set; }
        public int? PendingOnlyFlag { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
