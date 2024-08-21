using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_wt;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using LALoDep.Domain.pd_Work;

namespace LALoDep.Models.Case
{
    public class RecordTimeEditViewModel
    {
        public int WorkID { get; set; }
        public int? AgencyID { get; set; }
        public int WorkTimeID { get; set; }

        [Display(Name = "Worker")]
        public int? PersonID { get; set; }

        [Display(Name = "Hours(use tenths for partial hours)")]
        public decimal? WorkHours { get; set; }
        [Display(Name = "Mileage")]
        public decimal? WorkMileage { get; set; }
        public decimal? WorkHoursOverTime { get; set; }
        [Display(Name = "Start Date")]
        public string WorkStartDate { get; set; }

        [Display(Name = "End Date")]
        public string WorkEndDate { get; set; }

        [Display(Name = "Description")]
        public int? WorkDescriptionCodeID { get; set; }
        public short? RecordStateID { get; set; }
        [Display(Name = "Phase")]
        public int? WorkPhaseCodeID { get; set; }

        public int? HearingID { get; set; }
        public int? HearingTypeCodeID { get; set; }
        [Display(Name = "Hearing")]
        public string HearingDisplay { get; set; }

        [Display(Name = "Note")]
        public string NoteEntry { get; set; }
        public int? NoteID { get; set; }
        public int? NoteAgencyID { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        public int? NoteEntityPrimaryKeyID { get; set; }
        public int? NoteTypeCodeID { get; set; }
        public string NoteSubject { get; set; }
        public int? NoteCaseID { get; set; }
        public int? NotePetitionID { get; set; }
        public int? NoteHearingID { get; set; }
        public short? NoteRecordStateID { get; set; }
        public int WorkHoursRequiredFlag { get; set; }
        public IEnumerable<SelectListItem> Descriptions { get; set; }
        public IEnumerable<SelectListItem> Phases { get; set; }
        public List<pd_RoleGetByCaseIDBillingWorker_spResult> WorkerList { get; set; }
        public List<pd_WorkRoleGetByWorkID_spResult> DeleteWorkForList { get; set; }
        public List<pd_WorkRoleGetByWorkID_spResult> WorkForList { get; set; }
        public string ControlType { get; set; }
        public int RecordTimeNoteSubjectFlag { get; set; }
        public int UseWorkHoursForActivityLog { get; set; }
        public int? WorkIVeEligibleCodeID { get; set; }
        public IEnumerable<SelectListItem> IVeEligibleList { get; set; }
        public string WorkStartTime { get; set; }
        public string WorkEndTime { get; set; }
        public string FromZipCode { get; set; }
        public string ToZipCode { get; set; }
        public int? WorkZipCodeID { get; set; }
        public int? CanEditFlag { get; set; }
        public int? WorkPhaseRequiredFlag { get; set; }

        public string QHEHearingID { get; set; }
        public RecordTimeEditViewModel()
        {
            IVeEligibleList = new List<SelectListItem>();

            WorkerList = new List<pd_RoleGetByCaseIDBillingWorker_spResult>();
            WorkForList = new List<pd_WorkRoleGetByWorkID_spResult>();
            DeleteWorkForList = new List<pd_WorkRoleGetByWorkID_spResult>();
        }
    }
}