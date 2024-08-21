using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingFindingOrderPersonGetByHearingFindingOrderID_spParams
    {
        public int HearingFindingOrderID { get; set; }
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_HearingFindingOrderPersonGetByHearingFindingOrderID_spResult
    {
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int? PersonID { get; set; }
        public int? HearingFindingOrderPersonID { get; set; }
        public byte? RoleClient { get; set; }
        public int? RoleID { get; set; }
        public int Selected { get; set; }
        public string RoleTypeCodeValue { get; set; }
    }
}
