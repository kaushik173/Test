using LALoDep.Domain.Mobile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Areas.Mobile.Models
{
    public class RecordTimeAddViewModel
    {
        public decimal? WorkHours { get; set; }
        public int? WorkDescriptionCodeID { get; set; }        
        public IEnumerable<SelectListItem> WorkDescriptionList{ get; set; }
        public int? WorkPhaseCodeID { get; set; }
        public IEnumerable<SelectListItem> WorkPhaseList { get; set; }
        public decimal? WorkMileage { get; set; }

        public string WorkStartDate { get; set; }

        public string NoteEntry { get; set; }

        public List<pd_WorkRoleGetByWorkID_spResult> WorkedForList { get; set; }

        public int? NextCaseID { get; set; }

        public IEnumerable<SelectListItem> NextCaseList { get; set; }

        public RecordTimeAddViewModel()
        {
            WorkedForList = new List<pd_WorkRoleGetByWorkID_spResult>();
            WorkDescriptionList = new List<SelectListItem>();
            WorkPhaseList = new List<SelectListItem>();
            NextCaseList = new List<SelectListItem>();
        }
    }


    public class RecordTimeEditViewModel
    {
        public int WorkID { get; set; }
        public int? AgencyID { get; set; }        
        public int? PersonID { get; set; }        
        public decimal? WorkHours { get; set; }        
        public decimal? WorkMileage { get; set; }
        public decimal? WorkHoursOverTime { get; set; }        
        public string WorkStartDate { get; set; }        
        public string WorkEndDate { get; set; }        
        public int? WorkDescriptionCodeID { get; set; }
        public short? RecordStateID { get; set; }        
        public int? WorkPhaseCodeID { get; set; }


        public string WorkerFirstName { get; set; }
        public string WorkerLastName { get; set; }

        public bool IsWorkChanged { get; set; }


        public int? HearingID { get; set; }
        
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

        public bool IsNoteChanged { get; set; }


        public IEnumerable<SelectListItem> WorkDescriptionList { get; set; }
        public IEnumerable<SelectListItem> WorkPhaseList { get; set; }

        

        public List<WorkedRoleViewModel> WorkedForList { get; set; }
        public RecordTimeEditViewModel()
        {   
            WorkedForList = new List<WorkedRoleViewModel>();
        }
    }

    public class WorkedRoleViewModel
    {
        public bool Selected { get; set; }
        public int? WorkRoleID { get; set; }
        public int? RoleID { get; set; }
        public string PersonName { get; set; }
        public int? AllMainPetitionsClosedFlag { get; set; }
        public string AllMainPetitionsDisplay { get; set; }
    }
}