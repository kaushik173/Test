using System;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingReportFilingDueGetByCaseID_spParams
    {
        public int? CaseID { get; set; }
        public int? UserID { get; set; }
        public Guid BatchLogJobID { get; set; }


    }

    public class pd_HearingReportFilingDueGetByCaseID_spResults
    {
        public int? HearingReportFilingDueID { get; set; }
        public int? HearingID { get; set; }
        public int? HearingReportFilingDueTypeCodeID { get; set; }
        public System.DateTime? HearingReportFilingDueOrderDate { get; set; }
        public System.DateTime? HearingReportFilingDueDate { get; set; }
        public System.DateTime? HearingReportFilingDueEndDate { get; set; }
        public int? RequestedByPersonID { get; set; }
        public int? RequestedForPersonID { get; set; }
        public int? HearingReportFilingDueLegalResearchTypeCodeID { get; set; }
        public string Type { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedFor { get; set; }
    }
}