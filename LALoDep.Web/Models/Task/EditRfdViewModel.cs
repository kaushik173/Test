using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_Role;

namespace LALoDep.Models.Task
{
    public class EditRfdViewModel
    {
        public int RoleCount { get; set; }
        public string ControlType { get; set; }
        public int HearingReportFilingDueID { get; set; }
        public string RequestDate { get; set; }
        public string RequestType { get; set; }
        public string Hearing { get; set; }
        public string RequestBy { get; set; }
        public int RequestForID { get; set; }
        public IEnumerable<SelectListItem> RequestForList { get; set; }
        public string DueDate { get; set; }
        public bool Completed { get; set; }
        public string CompletedDate { get; set; }
        public string LegalResearchType { get; set; }
        public string RequestNote { get; set; }
        public string InvestigatorEvaluationNote { get; set; }
        public int? InvestigatorEvaluationNoteID { get; set; }
        public string InvestigatorEvaluationNoteControlType { get; set; }

        public string CaretakerEvaluationNote { get; set; }
        public int? CaretakerEvaluationNoteID { get; set; }
        public string CaretakerEvaluationNoteControlType { get; set; }

        public string SocialWorkerEvaluationNote { get; set; }
        public int? SocialWorkerEvaluationNoteID { get; set; }
        public string SocialWorkerEvaluationNoteControlType { get; set; }
        public string TherapistEvaluationNoteControlType { get; set; }
        public string TherapistEvaluationNote { get; set; }
        public int? TherapistEvaluationNoteID { get; set; }
        public string ProbationOfficerEvaluationNote { get; set; }
        public string ProbationOfficerEvaluationNoteControlType { get; set; }

        public int? ProbationOfficerEvaluationNoteID { get; set; }
        public bool ForceCreateARAnyway { get; set; }
        public IEnumerable<pd_RFDRoleContactGetByReportFilingDueID_spResult> ClientRoleList { get; set; }
        public IList<pd_NoteGetByRFDIDSystemValueTypeID_spResult> NoteBoxList { get; set; }

    }


}