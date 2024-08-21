using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{


    public class qcal_HearingUpdateCalendarNumber_spParams
    {
        public int? HearingID { get; set; }
        public string HearingCourtCalendarNumber { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

}