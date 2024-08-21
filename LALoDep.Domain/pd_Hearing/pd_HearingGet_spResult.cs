using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
    public class pd_HearingGet_spResult
    {
        public int? HearingID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingTypeCodeID { get; set; }
        public System.DateTime? HearingDateTime { get; set; }
        public int? HearingOfficerPersonID { get; set; }
        public int? HearingCourtDepartmentCodeID { get; set; }
        public int? HearingRequestedByCodeID { get; set; }
        public int? HearingResultCodeID { get; set; }
        public int? HearingFollowedRecommendations { get; set; }
        public decimal? HearingInvoiceAmount { get; set; }
        public short? RecordStateID { get; set; }
          public int? InsertedByUserID { get; set; }
          public System.DateTime? InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public System.DateTime? UpdatedOnDateTime { get; set; }
        public string HearingTypeCodeValue { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string HearingCourtDepartmentCodeValue { get; set; }
        public string HearingRequestedByCodeValue { get; set; }
        public int? Resulted { get; set; }
        public string HearingType { get; set; }
        public string HearingDept { get; set; }
        public string HearingJudge { get; set; }
        public int? NoteID { get; set; }
        public int? NoteAgencyID { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        public int? EntityPrimaryKeyID { get; set; }
        public int? NoteTypeCodeID { get; set; }
        public string NoteSubject { get; set; }
        public string NoteEntry { get; set; }
        public int? NoteCaseID { get; set; }
        public int? NotePetitionID { get; set; }
        public int? NoteHearingID { get; set; }
        public short? NoteRecordStateID { get; set; }
         public int? IssueNoteID { get; set; }
        public int? IssueNoteAgencyID { get; set; }
        public int? IssueNoteEntityCodeID { get; set; }
        public int? IssueNoteEntityTypeCodeID { get; set; }
        public int? IssueEntityPrimaryKeyID { get; set; }
        public int? IssueNoteTypeCodeID { get; set; }
        public string IssueNoteSubject { get; set; }
        public string IssueNoteEntry { get; set; }
        public int? IssueNoteCaseID { get; set; }
        public int? IssueNotePetitionID { get; set; }
        public int? IssueNoteHearingID { get; set; }
        public short? IssueNoteRecordStateID { get; set; }
         public int? SearchNoteID { get; set; }
        public int? SearchNoteAgencyID { get; set; }
        public int? SearchNoteEntityCodeID { get; set; }
        public int? SearchNoteEntityTypeCodeID { get; set; }
        public int? SearchEntityPrimaryKeyID { get; set; }
        public int? SearchNoteTypeCodeID { get; set; }
        public string SearchNoteSubject { get; set; }
        public string SearchNoteEntry { get; set; }
        public int? SearchNoteCaseID { get; set; }
        public int? SearchNotePetitionID { get; set; }
        public int? SearchNoteHearingID { get; set; }
        public short? SearchNoteRecordStateID { get; set; }
         public int? ConflictNoteID { get; set; }
        public int? ConflictNoteAgencyID { get; set; }
        public int? ConflictNoteEntityCodeID { get; set; }
        public int? ConflictNoteEntityTypeCodeID { get; set; }
        public int? ConflictEntityPrimaryKeyID { get; set; }
        public int? ConflictNoteTypeCodeID { get; set; }
        public string ConflictNoteSubject { get; set; }
        public string ConflictNoteEntry { get; set; }
        public int? ConflictNoteCaseID { get; set; }
        public int? ConflictNotePetitionID { get; set; }
        public int? ConflictNoteHearingID { get; set; }
        public short? ConflictNoteRecordStateID { get; set; }
         public int? EffortNoteID { get; set; }
        public int? EffortNoteAgencyID { get; set; }
        public int? EffortNoteEntityCodeID { get; set; }
        public int? EffortNoteEntityTypeCodeID { get; set; }
        public int? EffortEntityPrimaryKeyID { get; set; }
        public int? EffortNoteTypeCodeID { get; set; }
        public string EffortNoteSubject { get; set; }
        public string EffortNoteEntry { get; set; }
        public int? EffortNoteCaseID { get; set; }
        public int? EffortNotePetitionID { get; set; }
        public int? EffortNoteHearingID { get; set; }
        public short? EffortNoteRecordStateID { get; set; }
         public int? WorkPersonID { get; set; }
        public int? WorkID { get; set; }
        public decimal? WorkHours { get; set; }
        public int? WorkDescriptionCodeID { get; set; }
        public int? WorkPhaseCodeID { get; set; }
        public decimal? WorkHoursOvertime { get; set; }
        public decimal? WorkMileage { get; set; }
        public short? WorkRecordStateID { get; set; }
        public int? WorkHasInvoiceFlag { get; set; }
        public int? HearingMediaPresentFlag { get; set; }
     
    }
}
