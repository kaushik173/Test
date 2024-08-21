using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingReportFilingDueGetByDateRangeTypePersonID_spResult
    {
        public string RequestedByName { get; set; }
        public string ReportFilingDueType { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string CompleteDate { get; set; }
        public string PetitionNumber { get; set; }
        public int? ReportFilingDueID { get; set; }
        public int? CaseID { get; set; }
        public string Client { get; set; }
        public string HearingType { get; set; }
        public DateTime? HearingDate { get; set; }
        //public string ParameterText { get; set; }
        public string RequestedForName { get; set; }
       // public byte? ShowFlag { get; set; }
        public int? TotalRFD { get; set; }
        public int? AgencyID { get; set; }
        public int? HearingID { get; set; }
        public int? HearingReportFilingDueTypeCodeID { get; set; }
        public int? RequestedByPersonID { get; set; }
        public int? RequestedForPersonID { get; set; }
        public int? HearingReportFilingDueLegalResearchTypeCodeID { get; set; }
        public int? RecordStateID { get; set; }
    }
}
