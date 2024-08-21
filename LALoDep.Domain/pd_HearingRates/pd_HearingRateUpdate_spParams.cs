using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_HearingRates
{
    public class pd_HearingRateUpdate_spParams
    {
        public int? HearingRateID { get; set; }
        public int? AgencyID { get; set; }
        public int HearingTypeCodeID { get; set; }
        public decimal HearingRateEntry { get; set; }
        public DateTime? HearingRateStartDate { get; set; }
        public DateTime? HearingRateEndDate { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
