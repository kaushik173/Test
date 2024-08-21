using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_spParams
    {
        public int HearingFindingOrderID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_HearingFindingOrderNoticeGetByHearingFindingOrderID_spResult
    {
        public int? HearingFindingOrderNoticeID { get; set; }
        public int? NoticeID { get; set; }
        public string Notice { get; set; }
        public int Selected { get; set; }
    }
}
