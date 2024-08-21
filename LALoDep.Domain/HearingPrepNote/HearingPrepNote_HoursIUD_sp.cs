
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.HearingPrepNote
{

    public class HearingPrepNote_HoursIUD_spParams
    {
        public string IUD { get; set; }
        public int? WorkID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public decimal? WorkHours { get; set; }
        public DateTime? WorkDate { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

}