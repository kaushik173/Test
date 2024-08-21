using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingFindingOrderNoticeGetByHearingID_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_HearingFindingOrderNoticeGetByHearingID_spResult
    {
        public int? HearingFindingOrderNoticeID { get; set; }
        public int? HearingFindingOrderID { get; set; }
        public int? NoticeID { get; set; }
        public string Notice { get; set; }
        public int? AgencyID { get; set; }
        public int? InsertedByUserID { get; set; }
        public DateTime? InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public int? UpdatedOnDateTime { get; set; }
    }
}
