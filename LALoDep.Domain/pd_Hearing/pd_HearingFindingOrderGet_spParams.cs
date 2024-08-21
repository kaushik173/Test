using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingFindingOrderGet_spParams
    {
        public int HearingFindingOrderID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_HearingFindingOrderGet_spResult
    {
        public int? HearingFindingOrderID { get; set; }
        public int? AgencyID { get; set; }
        public int? HearingID { get; set; }
        public int? HearingFindingOrderCodeID { get; set; }
        public short? RecordStateID { get; set; }
        public string HearingFindingOrderComment { get; set; }
    }
}
