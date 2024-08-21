using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Work
{
    public class pd_WorkGetByCaseID_spResult
    {
        public int? HasInvoiceFlag { get; set; }
        public int? WorkID { get; set; }
        public int? CaseID { get; set; }
        public int? PersonID { get; set; }
        public int? RoleID { get; set; }
        public decimal? WorkHours { get; set; }
        public decimal? WorkHoursOverTime { get; set; }
        public decimal? WorkMileage { get; set; }
        public int? WorkPhaseCodeID { get; set; }
        public string Phase { get; set; }
        public int? WorkDescriptionCodeID { get; set; }
        public string WorkDescriptionCodeValue { get; set; }
        public string WorkDescriptionCodeShortValue { get; set; }
        public string WorkDescriptionCodeMobileValue { get; set; }
        public int? AgencyID { get; set; }
        public string WorkStartDate { get; set; }
        public string WorkEndDate { get; set; }
        public string WorkerFirstName { get; set; }
        public string WorkerLastName { get; set; }
        public short? RecordStateID { get; set; }
        public byte[] RecordTimeStamp { get; set; }
        public int? InsertedByUserID { get; set; }
        public System.Nullable<System.DateTime> InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public System.Nullable<System.DateTime> UpdatedOnDateTime { get; set; }
        public string SortDate { get; set; }
        public int? HearingID { get; set; }
        public short? HearingRecordStateID { get; set; }
        public byte[] HearingRecordTimeStamp { get; set; }
        public string HearingDisplay { get; set; }
        public string NoteDisplay { get; set; }
        public int? DisableFlag { get; set; }
        public int? CanDeleteFlag { get; set; }
    }
}
