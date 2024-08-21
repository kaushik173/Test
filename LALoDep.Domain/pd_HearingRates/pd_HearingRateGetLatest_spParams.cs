using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_HearingRates
{
    public class pd_HearingRateGetLatest_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
