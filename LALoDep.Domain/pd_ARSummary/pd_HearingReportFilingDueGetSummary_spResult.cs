using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_ARSummary
{
    public class pd_HearingReportFilingDueGetSummary_spResult
    {
        public int PersonID { get; set; }
        public string RoleType { get; set; }
        public int RoleTypeID { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public byte ActiveUser { get; set; }
        public int? TotalDue { get; set; }
        public string PastDue { get; set; }
        public string DuePrior7Days { get; set; }
        public string DuePrior30Days { get; set; }
        public string DuePrior60Days { get; set; }
        public string DuePrior90Days { get; set; }
        public string DuePrior180Days { get; set; }
        public string DueAfter180Days { get; set; }
    }
}
