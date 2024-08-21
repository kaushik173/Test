using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Role;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class AttendanceViewModel
    {
        public int? HearingID { get; set; }
        public int? AgencyID { get; set; }
        [Display(Name = "Hearing Type")]
        public string HearingType { get; set; }
        [Display(Name = "Hearing Date")]
        public string HearingDateTime { get; set; }
        [Display(Name = "Hearing Officer")]
        public string HearingOfficer { get; set; }
        [Display(Name = "Court Department")]
        public string HearingDept { get; set; }
        [Display(Name = "Appearing Attorney")]
        public string AppearingAttorneyID { get; set; }        
        public int OldAppearingAttorneyID { get; set; }
        public int HearingAttendanceID { get; set; }
        public int HearingAttandanceRoleID { get; set; }

        public int? NoteID { get; set; }
        public int? NoteAgencyID { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        public int? EntityPrimaryKeyID { get; set; }
        public int? NoteTypeCodeID { get; set; }
        public string NoteSubject { get; set; }
        [Display(Name = "Note")]
        public string NoteEntry { get; set; }
        public int? NoteCaseID { get; set; }
        public int? PetitionID { get; set; }
        public int? buttonID { get; set; }

        public Int16? NoteRecordStateID { get; set; }
        public IEnumerable<SelectListItem> AppearingAttorney { get; set; }
        public List<HearingAttendanceListViewModel> HearingAttendance { get; set; }
        public AttendanceViewModel()
        {
            AppearingAttorney = new List<SelectListItem>();
            HearingAttendance = new List<HearingAttendanceListViewModel>();
        }
    }
}