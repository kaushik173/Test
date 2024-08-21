using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.Agency;
using LALoDep.Domain.CaseAttribute;
using LALoDep.Domain.pd_Association;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.CaseOpening;

namespace LALoDep.Controllers.CaseOpening
{
    public partial class CaseOpeningController : Controller
    {
        [ClaimsAuthorize(IsCasePage = true, PageSecurityItemID = SecurityToken.AddNote)]
        public virtual ActionResult Notes()
        {
            var model = new NotesModel
            {
               BroadcastNotesPages= UtilityFunctions.CodeGetByTypeIdAndUserId(25),
               NoteTypeList =
                    UtilityService.ExecStoredProcedureWithResults<LALoDep.Domain.pd_Code.pd_CodeGetNewNoteTypeByCaseID_spResult>(
                        "pd_CodeGetNewNoteTypeByCaseID_sp",
                        new LALoDep.Domain.pd_Code.pd_CodeGetNewNoteTypeByCaseID_spParams()
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                            BatchLogJobID = Guid.NewGuid()
                        }).Select(o=>new SelectListItem(){Value = o.CodeID.ToString(),Text = o.NoteTypeDisplay}).ToList()

            };


            return View(model);
        }
        [ClaimsAuthorize(IsCasePage = true, PageSecurityItemID = SecurityToken.AddNote)]
        [HttpPost]
        public virtual JsonResult NoteSave(NotesModel model)
        {

            var noteId = UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", new pd_NoteInsert_spParams()
            {
                AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                       NoteEntitySystemValueTypeID = 112,
                       NoteEntityTypeSystemValueTypeID = 123,
                       EntityPrimaryKeyID = UserManager.UserExtended.CaseID,
                       NoteTypeCodeID = model.NoteTypeID,
                       NoteSubject = model.Subject,
                       CaseID = UserManager.UserExtended.CaseID,
                       NoteEntry = model.Notes,

                       RecordStateID = 1,
                       BatchLogJobID = Guid.NewGuid(),
                       UserID = UserManager.UserExtended.UserID
                   }).FirstOrDefault();
            if (!string.IsNullOrEmpty(model.SelectedPageIds))
            {
                var pageIds = model.SelectedPageIds.Split(',');
                foreach (var pageId in pageIds)
                {
                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_NotePanelInsert_sp", new pd_NotePanelInsert_spParams()
                    {AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        NoteID = (int)noteId,
                        NotePanelCodeID = pageId.ToInt(),
                        RecordStateID = 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();

                }
            }


            return Json(new { Status = "Done" });
        }


    }
}