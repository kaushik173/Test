using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Work
{
    public class pd_WorkGet_spResult
    {
        public int? CanEditFlag { get; set; }
        public int WorkID { get; set; }
        public int? AgencyID{ get; set; }        
        public int? PersonID { get; set; }
        public decimal? WorkHours { get; set; }        
        public DateTime? WorkStartDate { get; set; }
        public DateTime? WorkEndDate { get; set; }
        public DateTime? WorkTimeStart{ get; set; }
        public DateTime? WorkTimeEnd { get; set; }
        public int? WorkPhaseCodeID { get; set; }
        public int? WorkDescriptionCodeID { get; set; }
        public short? RecordStateID { get; set; }        
        public decimal? WorkHoursOverTime { get; set; }
        public decimal? WorkMileage { get; set; }
        public int? HearingID { get; set; }
        public int? RoleID { get; set; }
        public string WorkDescriptionCodeValue { get; set; }
        public string WorkDescriptionCodeShortValue { get; set; }
        public string WorkerFirstName { get; set; }
        public string WorkerLastName { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public int? HearingTypeCodeID { get; set; }
        public string HearingDisplay { get; set; }
        public int? WorkIVeEligibleCodeID { get; set; }
        public int? WorkTimeID { get; set; }
        public int? WorkZipCodeID { get; set; }
        public string WorkZipCodeFrom { get; set; }
        public string WorkZipCodeTo { get; set; }

    }
}
