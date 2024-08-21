using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingReportFilingDueGet_spParams
    {
        public int? HearingReportFilingDueID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_HearingReportFilingDueUpdate_spParams
    {
        public int? HearingReportFilingDueID { get; set; }
        public int? AgencyID { get; set; }
        public int? HearingID { get; set; }
        public int? HearingReportFilingDueTypeCodeID { get; set; }
        public DateTime? HearingReportFilingDueOrderDate { get; set; }
        public DateTime? HearingReportFilingDueDate { get; set; }
        public DateTime? HearingReportFilingDueEndDate { get; set; }
        public int? RequestedByPersonID { get; set; }
        public int? RequestedForPersonID { get; set; }
        public int? HearingReportFilingDueLegalResearchTypeCodeID { get; set; }
        public int? RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }











}
