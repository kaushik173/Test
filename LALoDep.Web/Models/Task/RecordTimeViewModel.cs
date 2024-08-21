using LALoDep.Domain.pd_Role;
using LALoDep.Domain.RTNC;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
    public class RecordTimeViewModel
    {






        public List<RTNC_StaffMember_spResult> StaffMemberList { get; set; }
        public List<RTNC_WorkDescription_spResult> WorkDescriptionList { get; set; }
        public List<RTNC_AgencyGroup_spResult> AgencyGroupList { get; set; }
        public List<RTNC_Agency_spResult> AgencyList { get; set; }
        public List<RTNC_Supervisor_spResult> SupervisorList { get; set; }

        public int StaffMemberID { get; set; }
        public int WorkDescriptionID { get; set; }
        public int AgencyGroupID { get; set; }
        public int AgencyID { get; set; }
        public int SupervisorID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

        public RecordTimeViewModel()
        {
            StaffMemberList = new List<RTNC_StaffMember_spResult>();
            WorkDescriptionList = new List<RTNC_WorkDescription_spResult>();
            AgencyGroupList = new List<RTNC_AgencyGroup_spResult>();
            AgencyList = new List<RTNC_Agency_spResult>();
            SupervisorList = new List<RTNC_Supervisor_spResult>();

        }
    }
    public class RecordTimeNonCaseAddEditViewModel
    {
        public int WorkID { get; set; }
        public int? AgencyID { get; set; }

        [Display(Name = "Staff Member")]
        public int? PersonID { get; set; }

        [Display(Name = "Hours")]
        public decimal? WorkHours { get; set; }
        [Display(Name = "Mileage")]
        public decimal? WorkMileage { get; set; }
        public decimal? WorkHoursOverTime { get; set; }
        [Display(Name = "Date")]
        public string WorkStartDate { get; set; }

        [Display(Name = "End Date")]
        public string WorkEndDate { get; set; }

        [Display(Name = "Work Description")]
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
      
        public List<RTNC_StaffMember_spResult> WorkerList { get; set; }
     
        public List<RTNC_WorkDescription_spResult> WorkDescriptionList { get; set; }
        public string ControlType { get; set; }
        public int RecordTimeNoteSubjectFlag { get; set; }
        public int? WorkIVeEligibleCodeID { get; set; }
        public IEnumerable<SelectListItem> IVeEligibleList { get; set; }
        public int? CountyID { get; set; }

        public IEnumerable<SelectListItem> CountyList { get; set; }
        public RecordTimeNonCaseAddEditViewModel()
        {
            IVeEligibleList = new List<SelectListItem>();
            CountyList = new List<SelectListItem>();

            WorkerList = new List<RTNC_StaffMember_spResult>();
            WorkDescriptionList = new List<RTNC_WorkDescription_spResult>();
           
        }
    }
}