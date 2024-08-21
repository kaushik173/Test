using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Calendar
{
    public class pd_CalendarGetSummaryByStaffStartDateEndDate_spParams
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }


    }
}
