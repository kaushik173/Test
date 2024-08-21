using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_HearingRates
{
    public class pd_HearingRateGetLatest_spResult
    {
        public int? HearingRateID { get; set; }
        public int HearingTypeID { get; set; }
        public string HearingType { get; set; }
        public string HearingRate { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int AgencyID { get; set; }
        public string AgencyName { get; set; }
    }
}
