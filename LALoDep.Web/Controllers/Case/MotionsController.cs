using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Motions;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_Appeals;

namespace LALoDep.Controllers.Case
{
    public partial class MotionsController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;
        public MotionsController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.MotionsView, PageSecurityItemID = SecurityToken.MotionsView)]
        public virtual ActionResult List()
        {
            MotionViewModel model = new MotionViewModel
            {
                CanDeleteAccess = UserManager.IsUserAccessTo(SecurityToken.MotionDelete),
                OnViewLoad = true
            };
            return View(model);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.MotionsView, PageSecurityItemID = SecurityToken.MotionsView)]
        [HttpPost]
        public virtual JsonResult GetPetitions()
        {
            var result = UtilityService.ExecStoredProcedureWithResults<pd_PetitionGetByCaseID_spResult>(
                "pd_PetitionGetByCaseID_sp", new pd_PetitionGetByCaseID_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).Select(x => new
                {
                    PetitionID = x.PetitionID.ToEncrypt(),
                    PetitionFileDate = x.PetitionFileDate.ToShortDateString(),
                    PetitionCloseDate = x.PetitionCloseDate.ToShortDateString(),
                    PetitionDocketNumber = x.PetitionDocketNumber,
                    PetitionTypeCodeValue = x.PetitionTypeCodeValue,
                    Child = x.FirstName + " " + x.LastName,
                    x.MotionCount
                }).ToList();
            return Json(new DataTablesResponse(0, result, result.Count, result.Count));
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.MotionsView, PageSecurityItemID = SecurityToken.MotionsView)]
        [HttpPost]
        public virtual JsonResult GetMotions(string petitionID)
        {
            var result = UtilityService.ExecStoredProcedureWithResults<pd_MotionGetByPetitionID_spResult>(
            "pd_MotionGetByPetitionID_sp", new pd_MotionGetByPetitionID_spParams()
            {
                PetitionID = petitionID.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).Select(x => new
            {
                MotionFileDate = x.MotionFileDate,
                MotionTypeCodeValue = x.MotionTypeCodeValue,
                MotionRequestByCodeValue = x.MotionRequestByCodeValue,
                MotionDecisionCodeValue = x.MotionDecisionCodeValue,
                MotionDecisionDate = x.MotionDecisionDate.ToShortDateString(),
                EncryptedMotionID = x.MotionID.ToEncrypt(),
                EncryptedPetitionId = x.PetitionID.ToEncrypt()

            }).ToList();
            return Json(new DataTablesResponse(0, result, result.Count, result.Count));
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.MotionsView)]
        public virtual ActionResult MotionAddEdit(string id, string petitionID)
        {

            if (UserManager.UserExtended.CaseID > 0)
            {

                var viewModel = new MotionAddEditViewModel
                {
                    CanAddAccess = UserManager.IsUserAccessTo(SecurityToken.MotionAdd),
                    CanEditAccess = UserManager.IsUserAccessTo(SecurityToken.MotionEdit),
                    IntPetitionID = petitionID.ToDecrypt().ToInt()
                };


                int motionsID = 0;
                ViewBag.DisplayTitle = "Add Motion";
                if (!string.IsNullOrEmpty(id))
                {
                    ViewBag.DisplayTitle = "Edit Motion";
                    motionsID = id.ToDecrypt().ToInt();

                    var pd_MotionGet_spParams = new pd_MotionGet_spParams()
                    {
                        MotionID = motionsID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    var motionInfo = UtilityService.ExecStoredProcedureWithResults<pd_MotionGet_spResult>("pd_MotionGet_sp", pd_MotionGet_spParams).FirstOrDefault();
                    if (motionInfo != null)
                    {
                        viewModel.IntPetitionID = motionInfo.PetitionID;
                        viewModel.MotionID = motionInfo.MotionID;
                        viewModel.MotionDecisionCodeID = motionInfo.MotionDecisionCodeID;
                        viewModel.MotionFileDate = (motionInfo.MotionFileDate != null) ? motionInfo.MotionFileDate.Value.ToShortDateString() : string.Empty;
                        viewModel.MotionFilingDueDate = (motionInfo.MotionFilingDueDate != null) ? motionInfo.MotionFilingDueDate.Value.ToShortDateString() : string.Empty;
                        viewModel.MotionTypeCodeID = motionInfo.MotionTypeCodeID;
                        viewModel.MotionDecisionDate = (motionInfo.MotionDecisionDate != null) ? motionInfo.MotionDecisionDate.Value.ToShortDateString() : string.Empty;
                        viewModel.MotionRequestByCodeID = motionInfo.MotionRequestByCodeID;
                        viewModel.MotionTypeCodeValue = motionInfo.MotionTypeCodeValue;
                        viewModel.MotionDecisionCodeValue = motionInfo.MotionDecisionCodeValue;
                        viewModel.RecordStateID = motionInfo.RecordStateID;
                    }

                    var pd_NoteGetByEntity_spParams = new pd_NoteGetByEntity_spParams()
                    {
                        EntityPrimaryKeyID = motionsID,
                        EntityCodeSystemValueTypeID = 116,
                        EntityCodeTypeSystemValueTypeID = 123,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    var noteInfo = UtilityService.ExecStoredProcedureWithResults<pd_NoteGetByEntity_spResult>("pd_NoteGetByEntity_sp", pd_NoteGetByEntity_spParams).FirstOrDefault();
                    if (noteInfo != null)
                    {
                        viewModel.NoteID = noteInfo.NoteID;
                        viewModel.NoteEntityCodeID = noteInfo.NoteEntityCodeID;
                        viewModel.NoteEntityTypeCodeID = noteInfo.NoteEntityTypeCodeID;
                        viewModel.EntityPrimaryKeyID = noteInfo.EntityPrimaryKeyID;
                        viewModel.NoteTypeCodeID = noteInfo.NoteTypeCodeID;
                        viewModel.NoteSubject = noteInfo.NoteSubject;
                        viewModel.NoteEntry = noteInfo.NoteEntry;
                        viewModel.CaseID = noteInfo.CaseID;
                        viewModel.HearingID = noteInfo.HearingID;
                        viewModel.NoteRecordStateID = noteInfo.RecordStateID;
                    }
                }
                viewModel.ControlType = UtilityFunctions.GetNoteControlType("Motions/MotionAddEdit", noteId: viewModel.NoteID);

                viewModel.MotionType = UtilityFunctions.CodeGetByTypeIdAndUserId(41, includeCodeId: viewModel.MotionTypeCodeID ?? 0, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                viewModel.RequestedBy = UtilityFunctions.CodeGetByTypeIdAndUserId(28, includeCodeId: viewModel.MotionRequestByCodeID ?? 0, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                viewModel.Decision = UtilityFunctions.CodeGetByTypeIdAndUserId(19, includeCodeId: viewModel.MotionDecisionCodeID ?? 0, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                var motionHeaderInfo = UtilityService.ExecStoredProcedureWithResults<pd_MotionGetHeader_spResult>("pd_MotionGetHeader_sp", new pd_MotionGetHeader_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    MotionID = motionsID,
                    PetitionID = viewModel.IntPetitionID
                }).FirstOrDefault();
                if (motionHeaderInfo != null)
                {
                    viewModel.MotionHeaderDisplay = motionHeaderInfo.MotionHeaderDisplay;

                }

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.MotionsView)]
        [HttpPost]
        public virtual JsonResult MotionAddEditSave(MotionAddEditViewModel viewModel)
        {
            if (viewModel.MotionID.HasValue)
            {
                if (viewModel.MotionFileDate != Request.Form["MotionFileDate_oldValue"]
                     || viewModel.MotionTypeCodeID != Request.Form["MotionTypeCodeID_oldValue"].ToInt()
                     || viewModel.MotionRequestByCodeID != Request.Form["MotionRequestByCodeID_oldValue"].ToInt()
                     || viewModel.MotionDecisionCodeID != Request.Form["MotionDecisionCodeID_oldValue"].ToInt()
                     || viewModel.MotionDecisionDate != Request.Form["MotionDecisionDate_oldValue"]
                     || viewModel.MotionFilingDueDate != Request.Form["MotionFilingDueDate_oldValue"]
                    )
                {
                    var pd_MotionUpdate1_spParams = new pd_MotionUpdate1_spParams()
                    {
                        MotionID = viewModel.MotionID,
                        PetitionID = viewModel.IntPetitionID,
                        DecisionID = viewModel.MotionDecisionCodeID,
                        MotionFileDate = viewModel.MotionFileDate.ToDateTime(),
                        MotionTypeCodeID = viewModel.MotionTypeCodeID,
                        MotionDecisionDate = viewModel.MotionDecisionDate.ToDateTimeNullableValue(),
                        MotionRequestByCodeID = viewModel.MotionRequestByCodeID,
                        RecordStateID = viewModel.RecordStateID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),

                    };
                    if (!string.IsNullOrEmpty(viewModel.MotionFilingDueDate))
                        pd_MotionUpdate1_spParams.MotionFilingDueDate = viewModel.MotionFilingDueDate.ToDateTime();

                    UtilityService.ExecStoredProcedureWithResults<object>("pd_MotionUpdate1_sp", pd_MotionUpdate1_spParams).FirstOrDefault();

                }
                if (viewModel.NoteID.HasValue)
                {
                    if (viewModel.NoteEntry != Request.Form["NoteEntry_oldValue"] && !viewModel.NoteEntry.IsNullOrEmpty())
                    {
                        var pd_NoteUpdate_spParams = new del_NoteUpdate_spParams()
                        {
                            NoteID = viewModel.NoteID,
                            NoteEntityCodeID = viewModel.NoteEntityCodeID,
                            NoteEntityTypeCodeID = viewModel.NoteEntityTypeCodeID,
                            EntityPrimaryKeyID = viewModel.MotionID.Value,
                            NoteTypeCodeID = viewModel.NoteTypeCodeID.Value,
                            NoteSubject = viewModel.NoteSubject,
                            NoteEntry = viewModel.NoteEntry,
                            CaseID = viewModel.CaseID.Value,
                            PetitionID = viewModel.IntPetitionID,
                            RecordStateID = viewModel.NoteRecordStateID.Value,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        };
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteUpdate_sp", pd_NoteUpdate_spParams).FirstOrDefault();

                    }
                    else if (viewModel.NoteEntry.IsNullOrEmpty())
                    {
                        UtilityFunctions.NoteDelete(viewModel.NoteID.Value);
                    }
                }
                else
                {
                    NewNoteInsert(viewModel);
                }
            }
            else
            {
                var pd_MotionInsert1_spParams = new pd_MotionInsert1_spParams()
                {

                    PetitionID = viewModel.IntPetitionID,
                    MotionDecisionCodeID = viewModel.MotionDecisionCodeID,
                    MotionTypeCodeID = viewModel.MotionTypeCodeID,
                    MotionRequestByCodeID = viewModel.MotionRequestByCodeID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };

                if (!string.IsNullOrEmpty(viewModel.MotionFileDate))
                    pd_MotionInsert1_spParams.MotionFileDate = viewModel.MotionFileDate.ToDateTime();

                if (!string.IsNullOrEmpty(viewModel.MotionFilingDueDate))
                    pd_MotionInsert1_spParams.MotionFilingDueDate = viewModel.MotionFilingDueDate.ToDateTime();

                if (!string.IsNullOrEmpty(viewModel.MotionDecisionDate))
                    pd_MotionInsert1_spParams.MotionDecisionDate = viewModel.MotionDecisionDate.ToDateTime();

                viewModel.MotionID = UtilityService.ExecStoredProcedureWithResults<int>("pd_MotionInsert1_sp", pd_MotionInsert1_spParams).FirstOrDefault();

                if (!viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
                {
                    NewNoteInsert(viewModel);
                }
            }
            return Json(new { isSuccess = true });
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.MotionsView, PageSecurityItemID = SecurityToken.MotionDelete)]
        public virtual JsonResult MotionDelete(string id)
        {
            var pd_DecisionDelete_spParams = new pd_DecisionDelete_spParams()
            {
                ID = id.ToDecrypt().ToInt(),
                RecordStateID = 10,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "Motion",
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_MotionDelete_sp", pd_DecisionDelete_spParams).ToList();
            return Json(new { isSuccess = true });
        }

        private void NewNoteInsert(MotionAddEditViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var pd_NoteInsert_spParams = new pd_NoteInsert_spParams()
                {
                    NoteEntitySystemValueTypeID = 116,
                    NoteEntityTypeSystemValueTypeID = 123,
                    EntityPrimaryKeyID = viewModel.MotionID.Value,
                    NoteTypeCodeID = 0,
                    NoteEntry = viewModel.NoteEntry,
                    CaseID = UserManager.UserExtended.CaseID,
                    PetitionID = viewModel.IntPetitionID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var noteID = UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", pd_NoteInsert_spParams).FirstOrDefault();
            }
        }
    }
}