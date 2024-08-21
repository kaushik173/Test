
namespace LALoDep.Domain.HearingPrepNote
{

    public class HearingPrepNote_HoursGetHistory_spParams
    {
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class HearingPrepNote_HoursGetHistory_spResult
    {
        public HearingPrepNote_HoursGetHistory_spResult()
        {
        }
        public string StaffDisplay { get; set; }
        public System.Nullable<System.DateTime> WorkDate { get; set; }
        public decimal WorkHours { get; set; }
        public int? WorkID { get; set; }
    }
}
