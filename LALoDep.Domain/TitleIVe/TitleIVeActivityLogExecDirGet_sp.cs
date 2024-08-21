namespace LALoDep.Domain.TitleIVe
{
    public class TitleIVeActivityLogExecDirGet_spParams
    {
        public int? ErrorID { get; set; }
        public int? AgencyID { get; set; }
        public int? AgencyCountyID { get; set; }
        public int? ActivityYear { get; set; }
        public int? ActivityMonth { get; set; }
        public int? PersonID { get; set; }
        public int? UserID { get; set; }

    }
  

    public class TitleIVeActivityLogExecDirGet_spResult
    {

        public int? ActivityLogID { get; set; }
        public int? AgencyID { get; set; }
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
        public System.Nullable<System.DateTime> DateSignedEmployee { get; set; }
        public System.Nullable<System.DateTime> DateSignedSupervisor { get; set; }
        public int? SupervisorPersonID { get; set; }
        public decimal? NonDependPercent_ExecDir { get; set; }
        public decimal? TimeOffPercent_ExecDir { get; set; }
        public decimal? PTOReimbursmentRate_ExecDir { get; set; }
        public int? InsertedByUserID { get; set; }
        public System.Nullable<System.DateTime> InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public System.Nullable<System.DateTime> UpdatedOnDateTime { get; set; }
        public short? RecordStateID { get; set; }
        public byte[] RecordTimeStamp { get; set; }
        public string EmployeeName { get; set; }
    }
    public class TitleIVeExecDirCountyAllocationGet_spResult
    {

        public string CountyName { get; set; }
        public decimal? TimeWorkPercent { get; set; }
        public decimal? PaidTimeOFf { get; set; }
        public decimal? TotalByCounty { get; set; }

    }
    public class TitleIVeExecDirCountyAllocationGet_spParams
    {
        public int? ErrorID { get; set; }

        public int? ActivityLogID { get; set; }
        public int? UserID { get; set; }

    }
}
