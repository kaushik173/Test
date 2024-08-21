using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Work
{
    public class pd_WorkUpdate1_spParams
    {
        public int? WorkID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? PersonID { get; set; }
        public decimal? WorkHours { get; set; }
        public decimal? WorkHoursOverTime { get; set; }
        public decimal? WorkMileage { get; set; }
        public DateTime? WorkStartDate { get; set; }
        public DateTime? WorkEndDate { get; set; }
        public int? WorkDescriptionCodeID { get; set; }
        public short? RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? WorkPhaseCodeID { get; set; }
        public int? HearingID { get; set; }
        public int? WorkIVeEligibleCodeID { get; set; }
        public int? AgencyCountyID { get; set; }

    }
}
