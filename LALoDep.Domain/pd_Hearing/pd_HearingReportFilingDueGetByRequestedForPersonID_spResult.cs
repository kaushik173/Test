using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingReportFilingDueGetByRequestedForPersonID_spResult
    {
        public string RequestedByLastName { get; set; }
        public string RequestedByFirstName { get; set; }
        public string ReportFilingDueType { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime DueDate { get; set; }
        public string CaseNumber { get; set; }
        public string CasePetitionNumber { get; set; }
        public int ReportFilingDueID { get; set; }
        public int? CaseID { get; set; }
        public string Clients { get; set; }
        public string HearingType { get; set; }
        public DateTime? HearingDate { get; set; }
}
}
