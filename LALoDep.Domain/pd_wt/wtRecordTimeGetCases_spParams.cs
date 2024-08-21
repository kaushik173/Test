using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_wt
{
    public class wtRecordTimeGetCases_spParams
    {
        public int ReloadFlag { get; set; }
        public int CaseID { get; set; }
        public int? AgencyID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
