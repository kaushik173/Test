using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Calendar
{
    public class pd_GetLeaveForMyCalendar_spParams
    {
        public int PersonID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_GetLeaveForMyCalendar_spResult
    { 
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public string LeaveTypeCodeValue { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
    }
    public class pd_IndividualCalendar_spParams
    {
        public int PersonID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
          public int? HearingTypeCodeID { get; set; }
          public int? OtherPersonID { get; set; }
         public string OtherPersonRoleType { get; set; }
         public int? PendingOnlyFlag { get; set; }
       public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
      
    }
    public class pd_IndividualCalendar_spResult
    { 
        public System.DateTime? EventDateTime { get; set; }
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string EventType { get; set; }
        public string AgencyAttorneyFirstName { get; set; }
        public string AgencyAttorneyLastName { get; set; }
        public int? AgencyAttorneyID { get; set; }
        public string HearingType { get; set; }
        public string Department { get; set; }
        public string Petitions { get; set; }
        public string Clients { get; set; }
        public int? HearingChangeFlag { get; set; }
        public string Result { get; set; }
        public string ReportFilingDueType { get; set; }
        public int? ReportFilingDueID { get; set; }
        public string ReportFilingDueOrderDate { get; set; }
        public string ReportFilingDueDate { get; set; }
        public string ReportFilingDueEndDate { get; set; }
        public string ReportFilingDueCaseNumber { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public string LeaveFirstName { get; set; }
        public string LeaveLastName { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveEndDate { get; set; }
        public string LeaveStartTime { get; set; }
        public string LeaveEndTime { get; set; }
        public string LeaveType { get; set; }
        public string SortDate { get; set; }
        public string SortTime { get; set; }
        public string FillInFor { get; set; }
        public string QHE_NavigationURL { get; set; }
    }
}
