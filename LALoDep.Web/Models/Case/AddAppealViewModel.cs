using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class AddAppealViewModel
    {
        public IEnumerable<CodeViewModel> Types { get; set; }
        public IEnumerable<SelectListItem> AttorneyTypes { get; set; }
        public IEnumerable<SelectListItem> Attorney { get; set; }
        public IEnumerable<SelectListItem> Decisions { get; set; }
        
        [Display(Name = "File Date")]

        public string AppealFileDate { get; set; }
        [Display(Name = "Type")]
        public int AppealTypeCodeID { get; set; }
        [Display(Name = "Appeal Docket #")]
        public string AppealDocketNumber { get; set; }
        [Display(Name = "Attorney Type")]
        public int AttorneyTypeCodeID { get; set; }

        [Display(Name = "Attorney")]
        public int PersonID { get; set; }
        [Display(Name = "Oral Argument Date")]
        public string AppealOralArgumentDate{ get; set; }
        [Display(Name = "Decision")]
        public int MotionDecisionCodeID{ get; set; }
        [Display(Name = "Decision Date")]
        public string AppealDecisionDate { get; set; }
        [Display(Name = "Note ")]
        public string NoteEntry{ get; set; }
        public int? RecordStateID{ get; set; }
        public int PetitionID { get; set; }
        public int? NoteID { get; set; }
        public int? AttorneyRoleID{ get; set; }
        public string EncryptedAppealID { get; set; }
        public bool IsEdit{ get; set; }
    }
    
}