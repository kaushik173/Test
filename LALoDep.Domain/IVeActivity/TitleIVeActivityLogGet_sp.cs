namespace LALoDep.Domain.IVeActivity
{
    public class TitleIVeActivityLogGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? AgencyID { get; set; }
        public int? AgencyCountyID { get; set; }
        public int? ActivityYear { get; set; }
        public int? ActivityMonth { get; set; }
        public int? PersonID { get; set; }
        public int UserID { get; set; }

    }
    public class TitleIVeActivityLogGet_spResult
    {
        public int? ReadOnly { get; set; }
        public int? ActivityLogID { get; set; }
        public int? AgencyCountyID { get; set; }
        public int? ActivityYear { get; set; }
        public int? ActivityMonth { get; set; }
        public int? PersonID { get; set; }
        public decimal? MonthlySalary { get; set; }
        public string Title { get; set; }
        public int? FullTime { get; set; }
        public string AlternateWorkSchedule { get; set; }
        public decimal? FFDRPWorked { get; set; }
        public decimal? FFDRPPaidTimeOff { get; set; }
        public decimal? TotalFFDRPEligible { get; set; }
        public decimal? TotalFFDRPIneligible { get; set; }
        public System.DateTime? DateSignedEmployee { get; set; }
        public System.DateTime? DateSignedSupervisor { get; set; }
        public int? SupervisorPersonID { get; set; }
        public string EmployeeName { get; set; }
        public string SupervisorSignedName { get; set; }
        public int? UseWorkHoursForActivityLog { get; set; }
        public int? OKToSwitchToDifferentEmployee { get; set; }
        public string EmployeeSignedPersonName { get; set; }
        public string SaveSignatureNotAllowedMessage { get; set; }

    }
}