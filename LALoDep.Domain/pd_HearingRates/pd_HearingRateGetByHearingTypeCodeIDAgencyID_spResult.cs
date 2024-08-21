using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_HearingRates
{
    public class pd_HearingRateGetByHearingTypeCodeIDAgencyID_spResult
    {
        public int HearingRateID { get; set; }
        public int HearingTypeCodeID { get; set; }
        public string HearingType { get; set; }
        public decimal HearingRate { get; set; }
        public string HearingRateStartDate { get; set; }
        public string HearingRateEndDate { get; set; }
        public string AgencyName { get; set; }
        public bool Deleted{ get; set; }
        public Int16? RecordStateID { get; set; }
        public int? AgencyID{ get; set; }
        
    }
}
