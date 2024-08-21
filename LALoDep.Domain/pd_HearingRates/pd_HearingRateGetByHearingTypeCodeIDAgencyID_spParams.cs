using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_HearingRates
{
    public class pd_HearingRateGetByHearingTypeCodeIDAgencyID_spParams
    {
        public int HearingTypeCodeID { get; set; }
        public int AgencyID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
