using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class MotionAddEditViewModel
    {
        public string ControlType { get; set; }
        public IEnumerable<SelectListItem> MotionType { get; set; }
        public IEnumerable<SelectListItem> RequestedBy { get; set; }
        public IEnumerable<SelectListItem> Decision { get; set; }
        public int? MotionID { get; set; }
        public int? IntPetitionID { get; set; }
        [Display(Name = "Decision")]
        public int? MotionDecisionCodeID { get; set; }
        [Display(Name = "File Date")]
        public string MotionFileDate { get; set; }
        [Display(Name = "Filing Due Date")]
        public string MotionFilingDueDate { get; set; }
        [Display(Name = "Motion Type")]
        public int? MotionTypeCodeID { get; set; }
        [Display(Name = "Decision Date")]
        public string MotionDecisionDate { get; set; }
        [Display(Name = "Requested By")]
        public int? MotionRequestByCodeID { get; set; }
        public Int16? RecordStateID { get; set; }
        public string MotionTypeCodeValue { get; set; }
        public string MotionDecisionCodeValue { get; set; }
        public int? NoteID { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        public int? EntityPrimaryKeyID { get; set; }
        public int? NoteTypeCodeID { get; set; }
        public string NoteSubject { get; set; }
        [Display(Name = "Note")]
        public string NoteEntry { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public Int16? NoteRecordStateID { get; set; }
        public bool CanAddAccess{ get; set; }
        public bool CanEditAccess { get; set; }


        public string MotionHeaderDisplay { get; set; }
    }
}