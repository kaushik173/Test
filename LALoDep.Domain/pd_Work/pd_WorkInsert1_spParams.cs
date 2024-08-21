using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Work
{
    public class Default_RecordTime_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class Default_RecordTime_spResult
    {
        public string WorkStartDate { get; set; }
        public string WorkEndDate { get; set; }
        public int? WorkPhaseCodeID { get; set; }
        public int? WorkPhaseRequiredFlag { get; set; }
        public int? WorkHoursRequiredFlag { get; set; }
        public int? AgencyID { get; set; }
        public string AgencyName { get; set; }
        public int? RecordTimeNoteSubjectFlag { get; set; }
        public string HoursLabel { get; set; }
        public int? UseWorkHoursForActivityLog { get; set; }
        public int? HideWorkHoursOnHearingPages { get; set; }
    }

    public class pd_WorkInsert1_spParams
    {
        public int? AgencyID { get; set; }
        public int CaseID { get; set; }
        public int PersonID { get; set; }
        public decimal? WorkHours { get; set; }
        public decimal? WorkHoursOverTime { get; set; }
        public decimal? WorkMileage { get; set; }
        public DateTime? WorkStartDate { get; set; }
        public DateTime? WorkEndDate { get; set; }
        public int WorkDescriptionCodeID { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int WorkPhaseCodeID { get; set; }
        public int? WorkIVeEligibleCodeID { get; set; }
        public int? AgencyCountyID { get; set; }

        //public int? HearingID { get; set; }
    }
}
