using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingFindingOrderPersonGetByHearingIDHearing_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_HearingFindingOrderPersonGetByHearingIDHearing_spResult
    {
        public int? HearingFindingOrderID { get; set; }
        public string FOType { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string RoleType { get; set; }
        public byte? RoleClient { get; set; }
    }
}
