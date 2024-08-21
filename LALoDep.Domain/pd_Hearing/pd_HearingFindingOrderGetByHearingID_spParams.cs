using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingFindingOrderGetByHearingID_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_HearingFindingOrderGetByHearingID_spResult
    {
        public int? HearingFindingOrderID { get; set; }
        public int? AgencyID { get; set; }
        public int? HearingID { get; set; }
        public int? HearingFindingOrderCodeID { get; set; }
        public short? RecordStateID { get; set; }
        public string HearingFindingOrderCodeValue { get; set; }
        public string HearingFindingOrderComment { get; set; }
        public int? FindingOrderID { get; set; }

    }
}
