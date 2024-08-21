using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain;
using LALoDep.Domain.pd_Case;
using System.Web.Mvc;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Subpoena;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Conflict;
using System.ComponentModel.DataAnnotations;

namespace LALoDep.Models.Case
{
    public class ConflictViewModel
    {
        public IEnumerable<SelectListItem> ConfilctTypeList { get; set; }
        public IEnumerable<CodeViewModel> RoleList { get; set; }
        public bool UpdateConflictRecord { get; set; }
        public bool UpdateConflictNote { get; set; }
        public List<pd_ConflictGetByCaseID_spResult> ConflictList { get; set; }
        public bool CanAddAccess{ get; set; }
        public bool CanEditAccess { get; set; }
        public bool CanDeleteAccess { get; set; }
        public int? ConflictID { get; set; }
        [Display(Name = "Case Role")]
        public int? RoleID { get; set; }
        [Display(Name = "Conflict Date")]
        public string ConflictDate { get; set; }
        [Display(Name = "Conflict Type")]
        public int? ConflictTypeCodeID { get; set; }
        public int? ConflictStatusCodeID { get; set; }
        public string ConflictStatusDate { get; set; }
        public int? StatusByUserID { get; set; }
        public int? AgencyID { get; set; }
        public Int16? RecordStateID { get; set; }
        [Display(Name = "Basis")]
        public string NoteEntry { get; set; }
        public int? NoteID { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        public int? NoteTypeCodeID { get; set; }
        public string NoteSubject { get; set; }
        public string ControlType { get; set; }
        public ConflictViewModel()
        {
            ConflictList = new List<pd_ConflictGetByCaseID_spResult>();
        }
    }
}