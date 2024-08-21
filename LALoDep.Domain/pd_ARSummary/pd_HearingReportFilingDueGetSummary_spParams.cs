using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_ARSummary
{
    public class pd_HearingReportFilingDueGetSummary_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public byte IncludeAll { get; set; }
        public string Mode { get; set; }
        public int UnderAgeFiveOnlyFlag { get; set; }
    }
}
