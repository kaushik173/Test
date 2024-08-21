using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Appeals;
using LALoDep.Domain.pd_Motions;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Domain.pd_Role;
using LALoDep.Models;
using LALoDep.Domain.pd_Note;

namespace LALoDep.Controllers.Case
{
    public partial class WritsAppealsController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;
        public WritsAppealsController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AppealsView, PageSecurityItemID = SecurityToken.AppealsView)]
        public virtual ActionResult List()
        {
            MotionViewModel model = new MotionViewModel
            {
                OnViewLoad = true
            };
            return View(model);
        }

        [HttpGet]
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
                    PetitionFileDate = x.PetitionFileDate.ToDefaultFormat(),
                    PetitionCloseDate = x.PetitionCloseDate.ToDefaultFormat(),
                    PetitionDocketNumber = x.PetitionDocketNumber,
                    PetitionTypeCodeValue = x.PetitionTypeCodeValue,
                    Child = x.FirstName + " " + x.LastName,
                    x.AppealCount
                }).ToList();
            return Json(new DataTablesResponse(0, result, result.Count, result.Count), JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetAppeals(string petitionID)
        {
            var result = UtilityService.ExecStoredProcedureWithResults<pd_AppealGetByPetitionID_spResult>(
                "pd_AppealGetByPetitionID_sp", new pd_AppealGetByPetitionID_spParams()
                {
                    PetitionID = petitionID.ToDecrypt().ToInt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).ToList();
            var data = result.Select(x => new
            {

                AppealFileDate = x.AppealFileDate.ToDefaultFormat(),
                AppealTypeCodeValue = x.AppealTypeCodeValue,
                AppealOralArgumentDate = x.AppealOralArgumentDate.ToDefaultFormat(),
                DecisionCodeValue = (!string.IsNullOrEmpty(x.DecisionCodeValue)) ? x.DecisionCodeValue : "Add New Decision",
                AppealDecisionDate = x.DecisionDate.ToDefaultFormat(),
                EncryptedAppealID = x.AppealID.ToEncrypt(),
                EncryptedDecisionID = x.DecisionID.ToEncrypt(),
                PetitionDocketNumber = x.PetitionDocketNumber,

            }).ToList();


            return Json(new DataTablesResponse(0, data, data.Count, result.Count), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AppealsView, PageSecurityItemID = SecurityToken.DeleteAppeal)]
        public virtual JsonResult ApplealDelete(string id)
        {
            var appealID = id.ToDecrypt().ToInt();
            var pd_AppealDelete_spParams = new pd_AppealDelete_spParams()
            {
                ID = appealID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "Appeal",
                RecordStateID = 10,
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_AppealDelete_sp", pd_AppealDelete_spParams).ToList();
            return Json(new { isSuccess = true });
        }


        [ClaimsAuthorize(IsCasePage = true, PageSecurityItemID = SecurityToken.AddAppealsView)]
        public virtual ActionResult AddEditAppeals(string id, string appealID, string docketNo, string pageTitle = "")
        {
            if (UserManager.UserExtended.CaseID > 0)
            {
                ViewBag.pageTitle = pageTitle;
                var viewModel = new AddAppealViewModel();
                viewModel.AppealDocketNumber = docketNo;
                viewModel.PetitionID = id.ToDecrypt().ToInt();

                if (!string.IsNullOrEmpty(appealID))
                {
                    viewModel.EncryptedAppealID = appealID;
                    viewModel.IsEdit = true;
                    var pd_AppealGet_spParams = new pd_AppealGet_spParams()
                    {
                        AppealID = appealID.ToDecrypt().ToInt(),
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    var appealInfo = UtilityService.ExecStoredProcedureWithResults<pd_AppealGet_spResults>("pd_AppealGet_sp", pd_AppealGet_spParams).FirstOrDefault();
                    if (appealInfo != null)
                    {
                        viewModel.AppealFileDate = appealInfo.AppealFileDate.ToShortDateString();
                        viewModel.AppealOralArgumentDate = appealInfo.AppealOralArgumentDate.ToShortDateString();
                        viewModel.AppealTypeCodeID = appealInfo.AppealTypeCodeID.Value;
                        viewModel.AppealDocketNumber = appealInfo.AppealDocketNumber;
                        viewModel.NoteEntry = appealInfo.NoteEntry;
                        viewModel.PetitionID = appealInfo.PetitionID.Value;
                        viewModel.NoteID = appealInfo.NoteID;
                        viewModel.AttorneyRoleID = appealInfo.AttorneyRoleID;
                        viewModel.RecordStateID = appealInfo.RecordStateID;
                    }

                    var pd_AppealAttorneyGetByCaseID_spParams = new pd_AppealAttorneyGetByCaseID_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    viewModel.Attorney = UtilityService.ExecStoredProcedureWithResults<pd_AppealAttorneyGetByCaseID_spResult>("pd_AppealAttorneyGetByCaseID_sp", pd_AppealAttorneyGetByCaseID_spParams)
                                            .Select(x => new SelectListItem { Value = x.RoleID.ToString(), Text = x.PersonNameLast + ", " + x.PersonNameFirst + " (" + x.AttorneyType + ")" }).ToList();


                }
                else
                {
                    viewModel.AttorneyTypes = UtilityFunctions.CodeGetBySystemValueTypeId(41, agencyId: UserManager.UserExtended.CaseNumberAgencyID, userShortValue: true);
                    viewModel.Decisions = UtilityFunctions.CodeGetByTypeIdAndUserId(18, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                    var pd_RoleAgencyAttorneyGetByCaseID_spParams = new pd_RoleAgencyAttorneyGetByCaseID_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        CaseAgencyID = null,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    viewModel.Attorney = UtilityService.ExecStoredProcedureWithResults<pd_RoleAgencyAttorneyGetByCaseID_spResult>("pd_RoleAgencyAttorneyGetByCaseID_sp", pd_RoleAgencyAttorneyGetByCaseID_spParams)
                                            .Select(x => new SelectListItem { Value = x.PersonID.ToString(), Text = x.PersonNameLast + ", " + x.PersonNameFirst }).ToList();
                }

                viewModel.Types = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(29, includeCodeId: viewModel.AppealTypeCodeID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, PageSecurityItemID = SecurityToken.AddAppealsView)]
        public virtual JsonResult AddEditAppealsSave(AddAppealViewModel viewModel)
        {
            if (viewModel.IsEdit)
            {
                if ((!string.IsNullOrEmpty(viewModel.EncryptedAppealID)))
                {
                    var pd_AppealUpdate_spParams = new pd_AppealUpdate_spParams()
                    {
                        AppealID = viewModel.EncryptedAppealID.ToDecrypt().ToInt(),
                        PetitionID = viewModel.PetitionID,
                        AppealFileDate = viewModel.AppealFileDate,
                        AppealTypeCodeID = viewModel.AppealTypeCodeID,
                        AppealOralArgumentDate = viewModel.AppealOralArgumentDate,
                        AttorneyRoleID = viewModel.AttorneyRoleID,
                        AppealDocketNumber = viewModel.AppealDocketNumber,
                        RecordStateID = viewModel.RecordStateID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    UtilityService.ExecStoredProcedureWithResults<object>("pd_AppealUpdate_sp", pd_AppealUpdate_spParams).FirstOrDefault();
                }

                if (viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
                {
                    UtilityFunctions.NoteUpdate(viewModel.NoteID.Value, 3281, 3288, viewModel.EncryptedAppealID.ToDecrypt().ToInt(), 0, null, viewModel.NoteEntry, petitionId: viewModel.PetitionID);
                }
                else if (viewModel.NoteID.HasValue && string.IsNullOrEmpty(viewModel.NoteEntry))
                {
                    UtilityFunctions.NoteDelete(viewModel.NoteID.Value);
                }
                else if (!viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
                {
                    UtilityFunctions.NoteInsert(115, 123, viewModel.EncryptedAppealID.ToDecrypt().ToInt(), 0, null, viewModel.NoteEntry, petitionId: viewModel.PetitionID);
                }
            }
            else
            {
                var pd_AppealAttorneyGetByCaseID_spParams = new pd_AppealAttorneyGetByCaseID_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var appealAttorney = UtilityService.ExecStoredProcedureWithResults<pd_AppealAttorneyGetByCaseID_spResult>("pd_AppealAttorneyGetByCaseID_sp", pd_AppealAttorneyGetByCaseID_spParams).FirstOrDefault();

                var roleId = UtilityService.ExecStoredProcedureWithResults<int>("pd_RoleInsert_sp", new pd_RoleInsert_spParams
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    PersonID = viewModel.PersonID,
                    RecordStateID = 1,
                    RoleClient = 0,
                    RoleStartDate = DateTime.Parse(viewModel.AppealFileDate),
                    RoleTypeCodeID = viewModel.AttorneyTypeCodeID,
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();

                var appealID = UtilityService.ExecStoredProcedureWithResults<decimal>("pd_AppealInsert_sp", new pd_AppealInsert_spParams
                {
                    PetitionID = viewModel.PetitionID,
                    AppealFileDate = viewModel.AppealFileDate,
                    AppealTypeCodeID = viewModel.AppealTypeCodeID,
                    AppealOralArgumentDate = viewModel.AppealOralArgumentDate,
                    AttorneyRoleID = roleId,
                    AppealDocketNumber = viewModel.AppealDocketNumber,
                    MotionDecisionCodeID = viewModel.MotionDecisionCodeID,
                    AppealDecisionDate = viewModel.AppealDecisionDate,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),

                }).FirstOrDefault();

                if (!string.IsNullOrEmpty(viewModel.NoteEntry))
                {
                    UtilityFunctions.NoteInsert(115, 123, appealID.ToInt(), 0, null, viewModel.NoteEntry, petitionId: viewModel.PetitionID);
                }

                if (viewModel.MotionDecisionCodeID > 0)
                {
                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_DecisionInsert_sp", new pd_DecisionInsert_spParams()
                    {
                        AppealID = appealID.ToInt(),
                        MotionDecisionCodeID = viewModel.MotionDecisionCodeID,
                        DecisionDate = viewModel.AppealDecisionDate,
                        RecordStateID = 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();
                }
            }

            return Json(new { IsSuccess = true });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AppealsDecisionViewPage, PageSecurityItemID = SecurityToken.ViewAppealDecision)]
        public virtual ActionResult AppealDecisionAddEdit(string id, string appealID)
        {
            if (UserManager.UserExtended.CaseID > 0)
            {
                var viewModel = new AppealDecisionAddEditViewModel();
                viewModel.Decisions = UtilityFunctions.CodeGetByTypeIdAndUserId(18, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                var decisionID = 0;
                if (!string.IsNullOrEmpty(id))
                {
                    decisionID = id.ToDecrypt().ToInt();
                }

                var pd_DecisionGet_spParams = new pd_DecisionGet_spParams()
                {
                    DecisionID = decisionID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var decisionInfo = UtilityService.ExecStoredProcedureWithResults<pd_DecisionGet_spResult>("pd_DecisionGet_sp", pd_DecisionGet_spParams).FirstOrDefault();

                if (decisionInfo != null)
                {
                    viewModel.DecisionID = decisionInfo.DecisionID;
                    viewModel.DecisionCodeID = decisionInfo.DecisionCodeID;
                    viewModel.DecisionDate = decisionInfo.DecisionDate;
                    viewModel.AgencyID = decisionInfo.AgencyID;
                    viewModel.RecordStateID = decisionInfo.RecordStateID;
                }


                var pd_DecisionGetByAppealID_spParams = new pd_DecisionGetByAppealID_spParams()
                {
                    AppealID = appealID.ToDecrypt().ToInt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                viewModel.DecisionList = UtilityService.ExecStoredProcedureWithResults<pd_DecisionGetByAppealID_spResult>("pd_DecisionGetByAppealID_sp", pd_DecisionGetByAppealID_spParams).ToList();
                viewModel.EncryptedAppleaID = appealID;
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AppealsDecisionViewPage, PageSecurityItemID = SecurityToken.ViewAppealDecision)]
        public virtual JsonResult AppealsDecisionAddEditSave(AppealDecisionAddEditViewModel viewModel)
        {

            if (viewModel.DecisionID > 0)
            {
                UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_DecisionUpdate_sp", new pd_DecisionUpdate_spParams()
                {
                    DecisionID = viewModel.DecisionID,
                    AppealID = viewModel.EncryptedAppleaID.ToDecrypt().ToInt(),
                    MotionDecisionCodeID = viewModel.DecisionCodeID,
                    DecisionDate = viewModel.DecisionDate,
                    RecordStateID = 1,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();
            }
            else
            {
                UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_DecisionInsert_sp", new pd_DecisionInsert_spParams()
                {
                    AppealID = viewModel.EncryptedAppleaID.ToDecrypt().ToInt(),
                    MotionDecisionCodeID = viewModel.DecisionCodeID,
                    DecisionDate = viewModel.DecisionDate,
                    RecordStateID = 1,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();
            }
            return Json(new { IsSuccess = true });
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AppealsDecisionViewPage, PageSecurityItemID = SecurityToken.DeleteAppealDecision)]
        public virtual JsonResult DecisionDelete(string id)
        {
            var pd_DecisionDelete_spParams = new pd_DecisionDelete_spParams()
            {
                ID = id.ToDecrypt().ToInt(),
                RecordStateID = 10,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "Decision",
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_DecisionDelete_sp", pd_DecisionDelete_spParams).ToList();
            return Json(new { isSuccess = true });
        }
    }
}