using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MasterCalendar
{
    public class pd_MasterCalendarGet_spResults
    {
        public string EventDate { get; set; }
        public string EventTime { get; set; }
        public string EventType { get; set; }
        public string AgencyAttorneyPersonNameFirst { get; set; }
        public string AgencyAttorneyPersonNameLast { get; set; }
        public int? AgencyAttorneyID { get; set; }
        public string HearingType { get; set; }
        public string Department { get; set; }
        public string Petitions { get; set; }
        public string PetitionFileName { get; set; }
        public string Clients { get; set; }
        public int? HearingChangeFlag { get; set; }
        public string Result { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public string LeavePersonNameFirst { get; set; }
        public string LeavePersonNameLast { get; set; }
        public string LeaveStartDate { get; set; }
        public string LeaveEndDate { get; set; }
        public string LeaveStartTime { get; set; }
        public string LeaveENDTime { get; set; }
        public string LeaveType { get; set; }

        public int HearingCount { get; set; }
    }
}
