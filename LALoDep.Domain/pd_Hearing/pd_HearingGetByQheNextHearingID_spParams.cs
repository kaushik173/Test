using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingGetByQheNextHearingID_spParams
    {
        public int UserID { get; set; }
        public int HearingID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_HearingGetByQheNextHearingID_spResult
    {
        public int NextHearingID { get; set; }
        public int CaseID { get; set; }
        public int? AgencyID { get; set; }
    }
}
