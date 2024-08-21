using System;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Domain.pd_Role;
using DataTables.Mvc;
using LALoDep.Models;
using LALoDep.Custom;
using LALoDep.Domain.pd_Conflict;
using LALoDep.Domain.pd_Note;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.ConflictPage)]
        public virtual ActionResult Conflict(string id, string caseId = "")
        {
            var viewModel = new ConflictViewModel();
            if (!string.IsNullOrEmpty(caseId))
                UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());

            if (UserManager.UserExtended.CaseID > 0)
            {
                viewModel.CanAddAccess = UserManager.IsUserAccessTo(SecurityToken.ConflictAdd);
                viewModel.CanEditAccess = UserManager.IsUserAccessTo(SecurityToken.ConfictEdit);
                viewModel.CanDeleteAccess = UserManager.IsUserAccessTo(SecurityToken.ConflictDelete);

                if (!string.IsNullOrEmpty(id))
                {
                    //Edit mode
                    var conflictInfo = GetConflictInfo(id);
                    if (conflictInfo != null)
                    {
                        viewModel.ConflictID = conflictInfo.ConflictID;
                        viewModel.AgencyID = conflictInfo.AgencyID;
                        viewModel.RoleID = conflictInfo.RoleID;
                        viewModel.ConflictDate = conflictInfo.ConflictDate;
                        viewModel.ConflictTypeCodeID = conflictInfo.ConflictTypeCodeID;
                        viewModel.ConflictStatusCodeID = conflictInfo.ConflictStatusCodeID;
                        viewModel.ConflictStatusDate = conflictInfo.ConflictStatusDate;
                        viewModel.StatusByUserID = conflictInfo.StatusByUserID;
                        viewModel.RecordStateID = conflictInfo.RecordStateID;
                        viewModel.NoteEntry = conflictInfo.NoteEntry;
                        viewModel.NoteID = conflictInfo.NoteID;
                        viewModel.NoteEntityCodeID = conflictInfo.NoteEntityCodeID;
                        viewModel.NoteEntityTypeCodeID = conflictInfo.NoteEntityTypeCodeID;
                        viewModel.NoteTypeCodeID = conflictInfo.NoteTypeCodeID;
                        viewModel.NoteSubject = conflictInfo.NoteSubject;
                    }
                }
                viewModel.ControlType = UtilityFunctions.GetNoteControlType("Case/Conflict", noteId: viewModel.NoteID);
                viewModel.ConfilctTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(60, includeCodeId: viewModel.ConflictTypeCodeID ?? 0, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                viewModel.RoleList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDChildRespondent_spResult>(
                                    "pd_RoleGetByCaseIDChildRespondent_sp", new pd_RoleGetByCaseIDChildRespondent_spParams()
                                    {
                                        CaseID = UserManager.UserExtended.CaseID,
                                        UserID = UserManager.UserExtended.UserID,
                                        BatchLogJobID = Guid.NewGuid()
                                    }).Select(x => new CodeViewModel()
                                    {
                                        CodeID = x.RoleID,
                                        CodeValue = x.DisplayName + " (" + x.Role + ")"
                                    }).ToList();
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        [HttpPost]
        public virtual JsonResult ClientContactList()
        {
            var conflictList = UtilityService.ExecStoredProcedureWithResults<pd_ConflictGetByCaseID_spResult>(
                                 "pd_ConflictGetByCaseID_sp", new pd_ConflictGetByCaseID_spParams()
                                 {
                                     CaseID = UserManager.UserExtended.CaseID,
                                     UserID = UserManager.UserExtended.UserID,
                                     BatchLogJobID = Guid.NewGuid()
                                 }).ToList();

            var list = conflictList.Select(x => new
            {
                ConflictID = x.ConflictID,
                EncrypredConflictID = x.ConflictID.ToEncrypt(),
                RoleType = x.RoleType,
                ConflictDate = x.ConflictDate,
                ConflictType = x.ConflictType,
                CaseRoleDisplay = x.CaseRoleDisplay
            }).ToList();
            var total = list.Count;
            return Json(new DataTablesResponse(0, list, total, total));

        }
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ConflictPage)]
        public virtual JsonResult ConflictSave(ConflictViewModel viewModel)
        {
            if (viewModel.ConflictID.HasValue && UserManager.IsUserAccessTo(SecurityToken.ConfictEdit))
            {
                if (viewModel.UpdateConflictRecord)
                {


                    //edit
                    var pd_ConflictUpdate_spParams = new pd_ConflictUpdate_spParams()
                    {
                        ConflictID = viewModel.ConflictID.Value,
                        AgencyID = viewModel.AgencyID,
                        RoleID = viewModel.RoleID,
                        ConflictDate = viewModel.ConflictDate,
                        ConflictTypeCodeID = viewModel.ConflictTypeCodeID,
                        ConflictStatusCodeID = viewModel.ConflictStatusCodeID,
                        ConflictStatusDate = viewModel.ConflictStatusDate,
                        StatusByUserID = viewModel.StatusByUserID,
                        RecordStateID = viewModel.RecordStateID,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    };
                    UtilityService.ExecStoredProcedureWithResults<object>("pd_ConflictUpdate_sp", pd_ConflictUpdate_spParams).FirstOrDefault();
                }
                if (viewModel.UpdateConflictNote)
                {
                    if (viewModel.NoteID.HasValue)
                    {
                        if (!viewModel.NoteEntry.IsNullOrEmpty())
                        {
                            UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteUpdate_sp",
                                       new del_NoteUpdate_spParams()
                                       {
                                           NoteID = viewModel.NoteID,
                                           NoteEntityCodeID = viewModel.NoteEntityCodeID,
                                           NoteEntityTypeCodeID = viewModel.NoteEntityTypeCodeID,
                                           EntityPrimaryKeyID = viewModel.ConflictID.Value,
                                           NoteTypeCodeID = 0,
                                           NoteEntry = viewModel.NoteEntry,
                                           CaseID = UserManager.UserExtended.CaseID,
                                           RecordStateID = viewModel.RecordStateID.Value,
                                           BatchLogJobID = Guid.NewGuid(),
                                           UserID = UserManager.UserExtended.UserID
                                       }).FirstOrDefault();
                        }
                        else
                        {
                            UtilityFunctions.NoteDelete(viewModel.NoteID.Value);
                        }
                    }
                    else if (!viewModel.NoteEntry.IsNullOrEmpty())
                    {
                        var insertedNoteID = UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp",
                                     new pd_NoteInsert_spParams()
                                     {
                                         NoteEntitySystemValueTypeID = 121,
                                         NoteEntityTypeSystemValueTypeID = 123,
                                         EntityPrimaryKeyID = viewModel.ConflictID.Value,
                                         NoteTypeCodeID = 0,
                                         NoteEntry = viewModel.NoteEntry,
                                         CaseID = UserManager.UserExtended.CaseID,
                                         RecordStateID = 1,
                                         BatchLogJobID = Guid.NewGuid(),
                                         UserID = UserManager.UserExtended.UserID
                                     }).FirstOrDefault();
                    }
                }
            }
            else if (!viewModel.ConflictID.HasValue && UserManager.IsUserAccessTo(SecurityToken.ConflictAdd))
            {
                //add
                var pd_ConflictInsert_spParams = new pd_ConflictInsert_spParams()
                {
                    RoleID = viewModel.RoleID,
                    ConflictDate = viewModel.ConflictDate,
                    ConflictTypeCodeID = viewModel.ConflictTypeCodeID,
                    ConflictStatusCodeID = -1,
                    ConflictStatusDate = DateTime.Now,
                    StatusByUserID = UserManager.UserExtended.UserID,
                    RecordStateID = 1,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                };
                var inserteId = UtilityService.ExecStoredProcedureWithResults<int>("pd_ConflictInsert_sp", pd_ConflictInsert_spParams).FirstOrDefault();
                if (!viewModel.NoteEntry.IsNullOrEmpty())
                {
                    UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp",
                        new pd_NoteInsert_spParams()
                        {
                            NoteEntitySystemValueTypeID = 121,
                            NoteEntityTypeSystemValueTypeID = 123,
                            EntityPrimaryKeyID = inserteId,
                            NoteTypeCodeID = 0,
                            NoteEntry = viewModel.NoteEntry,
                            CaseID = UserManager.UserExtended.CaseID,
                            RecordStateID = 1,
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID
                        }).FirstOrDefault();
                }
            }
            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ConflictPage, PageSecurityItemID = SecurityToken.ConflictDelete)]
        public virtual JsonResult ConflictDelete(string id)
        {
            var pd_ConflictDelete_spParams = new pd_ConflictDelete_spParams()
            {
                ID = id.ToDecrypt().ToInt(),
                RecordStateID = 10,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "Conflict",
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_ConflictDelete_sp", pd_ConflictDelete_spParams).ToList();
            var conflictInfo = GetConflictInfo(id);
            if (conflictInfo != null)
            {
                if (conflictInfo.NoteID.HasValue)
                {
                    var pd_NoteDelete_spParams = new pd_NoteDelete_spParams()
                    {
                        ID = conflictInfo.NoteID.Value,
                        RecordStateID = 10,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        LoadOption = "Note",
                    };
                    UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteDelete_sp", pd_NoteDelete_spParams).ToList();
                }
            }
            return Json(new { isSuccess = true });
        }

        private pd_ConflictGet_spResult GetConflictInfo(string id)
        {
            var conflictInfo = UtilityService.ExecStoredProcedureWithResults<pd_ConflictGet_spResult>(
                              "pd_ConflictGet_sp", new pd_ConflictGet_spParams()
                              {
                                  ConflictID = id.ToDecrypt().ToInt(),
                                  UserID = UserManager.UserExtended.UserID,
                                  BatchLogJobID = Guid.NewGuid()
                              }).FirstOrDefault();
            return conflictInfo;
        }
    }
}