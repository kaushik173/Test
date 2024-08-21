using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingOpinionGet_spParams
    {
        public int HearingOpinionID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_HearingOpinionGet_spResult
    {
        public int? HearingOpinionID { get; set; }
        public int? AgencyID { get; set; }
        public int? HearingID { get; set; }
        public int? RoleID { get; set; }
        public int? RecordStateID { get; set; }
    }
}
