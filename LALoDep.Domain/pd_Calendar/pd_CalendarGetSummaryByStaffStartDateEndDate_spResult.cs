namespace LALoDep.Domain.pd_Calendar
{
    public class pd_CalendarGetSummaryByStaffStartDateEndDate_spResult
    {
        public string RoleType { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int? PersonID { get; set; }
        public int? PendingTrials { get; set; }
        public int? DisplayOrder { get; set; }
        public int? PendingHearingCount { get; set; }
        public int? HearingCount { get; set; }
        public int? ContestedHearingCount { get; set; }
        public int? TrialCount { get; set; }
    }
}