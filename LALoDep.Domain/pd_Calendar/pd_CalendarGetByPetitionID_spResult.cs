using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Calendar
{
    public class pd_CalendarGetByPetitionID_spResult
    {
        public string ItemType { get; set; }
        public DateTime? ItemDateTime { get; set; }
        public int? HearingID { get; set; }
        public string HearingType { get; set; }
        public string HearingOfficerPersonNameLast { get; set; }
        public string HearingOfficerPersonNameFirst { get; set; }
        public string HearingResult { get; set; }
        public int? EventID { get; set; }
        public string EventType { get; set; }
        public int? ReportFilingDueID { get; set; }
        public DateTime? ReportFilingDueOrderDate { get; set; }
        public DateTime? ReportFilingDueDueDate { get; set; }
        public DateTime? ReportFilingDueEndDate { get; set; }
        public string ReportFilingDueType { get; set; }
        public int? AppealID { get; set; }
        public DateTime? AppealFileDate { get; set; }
        public string AppealDecision { get; set; }
        public DateTime? AppealDecisionDate { get; set; }
        public int? MotionID { get; set; }
        public DateTime? MotionFileDate { get; set; }
        public string MotionType { get; set; }
        public string MotionDecision { get; set; }
        public DateTime? MotionDecisionDate { get; set; }
    }
}
