
using System;

namespace LALoDep.Domain.pd_MyJcats
{
    public class MyDailyTime_spParams
    {
        public int? PersonID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class MyDailyTime_spResult
    {
        public MyDailyTime_spResult()
        {
        }
        public string WorkDescription { get; set; }
        public decimal? WorkHours { get; set; }
        public byte? IneligibleFlag { get; set; }
        public string WorkHoursDisplay { get; set; }
        public string Clients { get; set; }
        public int? WorkID { get; set; }
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        public int? HearingID { get; set; }
        public string RoutingURL { get; set; }
        public int? SortOrder { get; set; }
        public int? MainDataID { get; set; }
    }
}
