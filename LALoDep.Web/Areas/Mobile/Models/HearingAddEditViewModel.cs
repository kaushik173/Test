using Jcats.SD.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Models;
namespace LALoDep.Areas.Mobile.Models
{
    public class HearingAddEditViewModel
    {
        public int? HearingID { get; set; }
        [Display(Name = "Hrg Type")]
        public int? HearingTypeCodeID { get; set; }
        [Display(Name = "Date")]
        public string HearingDate { get; set; }
        [Display(Name = "Time")]
        public string HearingTime { get; set; }
        [Display(Name = "Dept")]
        public int? DepartmentID { get; set; }
        [Display(Name = "Judge")]
        public int? HearingOfficerStaffID { get; set; }
        public int? HearingResultCodeID { get; set; }
        public int? PetitionID { get; set; }
        
        [Display(Name = "Appearing Atty")]
        public int? AppearingAttyID { get; set; }
        [Display(Name = "Hours Type")]
        public int? HoursTypeID { get; set; }
        [Display(Name = "Phase")]
        public int? PhaseID { get; set; }
        [Display(Name = "Media Present")]
        public bool MediaPresent { get; set; }

        [Display(Name = "Hrg Note")]
        public string HearingNoteEntry { get; set; }
        [Display(Name = "Hrs Worked")]
        public decimal? WorkDetailHour { get; set; }
        public int? WorkID { get; set; }
        public int? NoteID { get; set; }
        public List<CodeViewModel> PhaseList { get; set; }
        public List<CodeViewModel> HearingTypeList { get; set; }
        public List<CodeViewModel> HourTypeList { get; set; }
        public List<CodeViewModel> AppearingAttyList { get; set; }        
        public List<CodeViewModel> DepartmentList { get; set; }        
        public List<CodeViewModel> JudicialOfficerList { get; set; }
        public List<CodeViewModel> ResultCodeList { get; set; }
        public List<LALoDep.Domain.pd_Petition.pd_PetitionGetByCaseID_spResult> Petitions { get; set; }
        public List<LALoDep.Domain.pd_Petition.pd_PetitionGetAllByHearingID_spResult> SelectedPetitions { get; set; }
        
        public int? ClientRoleID { get; set; }
        public int? HearingNoteID { get; set; }
        public string CaseID { get; set; }
        public string CaseNumber { get; set; }
        public string ClientName { get; set; }

        public int? HearingNoteEntityCodeID { get; set; }
        public int? HearingNoteEntityTypeCodeID { get; set; }
        public int? HearingNoteEntityPrimaryKeyID { get; set; }
        public int? HearingNoteTypeCodeID { get; set; }
        public byte? HearingNoteRecordStateID { get; set; }
        public string HearingNoteSubject{ get; set; }
        public HearingAddEditViewModel()
        {
            HearingTypeList = new List<CodeViewModel>();
            AppearingAttyList = new List<CodeViewModel>();
            HourTypeList = new List<CodeViewModel>();
            DepartmentList = new List<CodeViewModel>();
            JudicialOfficerList = new List<CodeViewModel>();
            PhaseList = new List<CodeViewModel>();
            ResultCodeList = new List<CodeViewModel>();
            Petitions = new List<LALoDep.Domain.pd_Petition.pd_PetitionGetByCaseID_spResult>();
            SelectedPetitions = new List<LALoDep.Domain.pd_Petition.pd_PetitionGetAllByHearingID_spResult>();
        }
    }
    
      
}