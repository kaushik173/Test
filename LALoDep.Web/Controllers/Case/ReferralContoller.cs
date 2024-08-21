using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.JcatsMessage;
using LALoDep.Domain.ref_Referral;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Custom.Twillio;
using LALoDep.Models.Case;
using LALoDep.Custom;
using Omu.ValueInjecter;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.Mobile;
using LALoDep.Domain.pd_Work;
using LALoDep.Models;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        public virtual ActionResult ReferralList()
        {
            var viewModel = new ReferralViewModel()
            {
                ReferralPersonList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralForClientList_spResult>("ref_ReferralForClientList_sp",
                                                new ref_ReferralForClientList_spParams
                                                {
                                                    CaseID = UserManager.UserExtended.CaseID,
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid()
                                                }).Select(x => new SelectListItem { Text = x.NameDisplay, Value = x.RoleID.ToEncrypt() }),

                ReferralTypeList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralTypeList_spResult>("ref_ReferralTypeList_sp",
                                                new ref_ReferralTypeList_spParams
                                                {
                                                    CaseID = UserManager.UserExtended.CaseID,
                                                    BatchLogJobID = Guid.NewGuid(),
                                                    UserID = UserManager.UserExtended.UserID
                                                }).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToEncrypt() + "|" + x.NG_NavigationURL }),

                ReferralList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralList_spResult>("ref_ReferralList_sp",
                                                new ref_ReferralList_spParams
                                                {
                                                    CaseID = UserManager.UserExtended.CaseID,
                                                    SortOption = "ReferralType",
                                                    BatchLogJobID = Guid.NewGuid(),
                                                    UserID = UserManager.UserExtended.UserID
                                                }).ToList()
            };

            return View(viewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseMainPage, PageSecurityItemID = SecurityToken.ViewCaseMainPage)]
        public virtual ActionResult CaseRedirect(string id, string redirectUrl)
        {
            pd_CaseGet_sp_Result oCase = null;
            var caseId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                caseId = id.ToDecrypt().ToInt();
                oCase = UtilityService.Context.pd_CaseGet_sp(caseId, UserManager.UserExtended.UserID, Guid.NewGuid())
                        .FirstOrDefault();
                if (oCase != null)
                {
                    var flag = UserManager.UpdateCaseStatusBar(caseId, oCase: oCase);
                    if (!flag)
                    {
                        return Content("Redirecting");
                    }
                }
            }

            if (oCase == null)
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return Redirect(redirectUrl);
        }

        #region Educational Assistance Add Edit
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        public virtual ActionResult ReferralAddEdit(string id, string roleId, string referralTypeCodeId)
        {
            var viewModel = GetReferralAddEditViewModel(id, roleId, referralTypeCodeId);


            return View(viewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        [HttpPost]
        public virtual ActionResult ReferralAddEdit(ReferralAddEditViewModel model)
        {
            if (model.ReferralID.HasValue && model.ReferralID.Value > 0)
            {
                var oReferralGet = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGet_spResult>("ref_ReferralGet_sp",
                                     new ref_ReferralGet_spParams
                                     {
                                         CaseID = UserManager.UserExtended.CaseID,
                                         UserID = UserManager.UserExtended.UserID,
                                         BatchLogJobID = Guid.NewGuid(),
                                         ReferralID = model.ReferralID
                                     }).FirstOrDefault();
                if (oReferralGet != null)
                {

                    UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralIUD_sp",
                                new ref_ReferralIUD_spParams
                                {
                                    IUD = "UPDATE",
                                    CaseAgencyID = oReferralGet.AgencyID,
                                    CaseID = UserManager.UserExtended.CaseID,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid(),
                                    ReferralID = model.ReferralID,
                                    RoleID = model.RoleID,
                                    ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                    ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                    ReferralTypeCodeID = model.ReferralTypeCodeID,
                                    ReferralRequestDate = model.ReferralRequestDate,
                                    ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                    ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,
                                    ReferralEIPFlag = model.ReferralEIPFlag,
                                    ReferralEIPMostRecentFlag = model.ReferralEIPMostRecentFlag,
                                    ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,
                                    ReferralEducationalStatusCodeID = model.ReferralEducationalStatusCodeID,
                                    ReferralSchoolPreference = model.ReferralSchoolPreference,
                                    ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,
                                    SpecialEducationNote = model.SpecialEducationNote,
                                    SchoolProblemsNote = model.SchoolProblemsNote,
                                    ExpulsionNote = model.ExpulsionNote,
                                    EducationUnitOfServiceNote = model.EducationUnitOfServiceNote,
                                    ReferralInternalNote = model.ReferralInternalNote,
                                    ReferralDueDate = model.ReferralDueDate,
                                    ReferralEndDate = model.ReferralEndDate,
                                    DelinquencyCaseNote = oReferralGet.DelinquencyCaseNote,
                                    IssuesAndDependencyStatusSummaryNote = oReferralGet.IssuesAndDependencyStatusSummaryNote,
                                    ReasonForReferralNote = oReferralGet.ReasonForReferralNote,
                                    ReferralHasActiveCourtCaseFlag = oReferralGet.ReferralHasActiveCourtCaseFlag,
                                    ReferralHasChildrenFlag = oReferralGet.ReferralHasChildrenFlag,
                                    ReferralHaveLatestCourtReportFlag = oReferralGet.ReferralHaveLatestCourtReportFlag,
                                    ReferralReasonSummaryNote = oReferralGet.ReferralReasonSummaryNote,
                                    ReferralUrgencyCodeID = oReferralGet.ReferralUrgencyCodeID,
                                    ReferralYouthHasWorkingPhoneFlag = oReferralGet.ReferralYouthHasWorkingPhoneFlag,
                                    RelationshipsWithOtherClientsNote = oReferralGet.RelationshipsWithOtherClientsNote
                                });

                }
            }
            else
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralIUD_sp",
                                    new ref_ReferralIUD_spParams
                                    {
                                        IUD = "INSERT",
                                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                        CaseID = UserManager.UserExtended.CaseID,
                                        UserID = UserManager.UserExtended.UserID,
                                        BatchLogJobID = Guid.NewGuid(),
                                        ReferralEndDate = model.ReferralEndDate,
                                        RoleID = model.RoleID,
                                        ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                        ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                        ReferralTypeCodeID = model.ReferralTypeCodeID,
                                        ReferralRequestDate = model.ReferralRequestDate,

                                        ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                        ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,
                                        ReferralEIPFlag = model.ReferralEIPFlag,
                                        ReferralEIPMostRecentFlag = model.ReferralEIPMostRecentFlag,
                                        ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,
                                        ReferralEducationalStatusCodeID = model.ReferralEducationalStatusCodeID,
                                        ReferralSchoolPreference = model.ReferralSchoolPreference,
                                        ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,
                                        SpecialEducationNote = model.SpecialEducationNote,
                                        SchoolProblemsNote = model.SchoolProblemsNote,
                                        ExpulsionNote = model.ExpulsionNote,
                                        EducationUnitOfServiceNote = model.EducationUnitOfServiceNote,
                                        ReferralInternalNote = model.ReferralInternalNote,
                                        ReferralDueDate = model.ReferralDueDate
                                    });



            }



            return Json(new { isSuccess = true, dataModel = model });
        }

        #endregion


        #region CARE  Add Edit
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        public virtual ActionResult ReferralCareAddEdit(string id, string roleId, string referralTypeCodeId)
        {
            var viewModel = GetReferralAddEditViewModel(id, roleId, referralTypeCodeId);


            return View(viewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        [HttpPost]
        public virtual ActionResult ReferralCareAddEdit(ReferralAddEditViewModel model)
        {
            if (model.ReferralID.HasValue && model.ReferralID.Value > 0)
            {
                var oReferralGet = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGet_spResult>("ref_ReferralGet_sp",
                                     new ref_ReferralGet_spParams
                                     {
                                         CaseID = UserManager.UserExtended.CaseID,
                                         UserID = UserManager.UserExtended.UserID,
                                         BatchLogJobID = Guid.NewGuid(),
                                         ReferralID = model.ReferralID
                                     }).FirstOrDefault();
                if (oReferralGet != null)
                {

                    UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralIUD_sp",
                                new ref_ReferralIUD_spParams
                                {
                                    IUD = "UPDATE",
                                    CaseAgencyID = oReferralGet.AgencyID,
                                    CaseID = UserManager.UserExtended.CaseID,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid(),
                                    ReferralID = model.ReferralID,
                                    RoleID = model.RoleID,
                                    ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                    ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                    ReferralTypeCodeID = model.ReferralTypeCodeID,
                                    ReferralRequestDate = model.ReferralRequestDate,
                                    ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                    ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,
                                    ReferralEIPFlag = oReferralGet.ReferralEIPFlag,
                                    ReferralEIPMostRecentFlag = oReferralGet.ReferralEIPMostRecentFlag,
                                    ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,
                                    ReferralEducationalStatusCodeID = oReferralGet.ReferralEducationalStatusCodeID,
                                    ReferralSchoolPreference = oReferralGet.ReferralSchoolPreference,
                                    ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,
                                    SpecialEducationNote = oReferralGet.SpecialEducationNote,
                                    SchoolProblemsNote = oReferralGet.SchoolProblemsNote,
                                    ExpulsionNote = oReferralGet.ExpulsionNote,
                                    EducationUnitOfServiceNote = oReferralGet.EducationUnitOfServiceNote,
                                    ReferralInternalNote = model.ReferralInternalNote,
                                    ReferralDueDate = model.ReferralDueDate,
                                    ReferralEndDate = model.ReferralEndDate,
                                    DelinquencyCaseNote = oReferralGet.DelinquencyCaseNote,
                                    IssuesAndDependencyStatusSummaryNote = model.IssuesAndDependencyStatusSummaryNote,
                                    ReasonForReferralNote = oReferralGet.ReasonForReferralNote,
                                    ReferralHasActiveCourtCaseFlag = oReferralGet.ReferralHasActiveCourtCaseFlag,
                                    ReferralHasChildrenFlag = model.ReferralHasChildrenFlag,
                                    ReferralHaveLatestCourtReportFlag = oReferralGet.ReferralHaveLatestCourtReportFlag,
                                    ReferralReasonSummaryNote = oReferralGet.ReferralReasonSummaryNote,
                                    ReferralUrgencyCodeID = oReferralGet.ReferralUrgencyCodeID,
                                    ReferralYouthHasWorkingPhoneFlag = oReferralGet.ReferralYouthHasWorkingPhoneFlag,
                                    RelationshipsWithOtherClientsNote = model.RelationshipsWithOtherClientsNote,

                                });

                }
            }
            else
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralIUD_sp",
                                    new ref_ReferralIUD_spParams
                                    {
                                        IUD = "INSERT",
                                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                        CaseID = UserManager.UserExtended.CaseID,
                                        UserID = UserManager.UserExtended.UserID,
                                        BatchLogJobID = Guid.NewGuid(),
                                        ReferralEndDate = model.ReferralEndDate,
                                        RoleID = model.RoleID,
                                        ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                        ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                        ReferralTypeCodeID = model.ReferralTypeCodeID,
                                        ReferralRequestDate = model.ReferralRequestDate,

                                        ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                        ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,


                                        ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,


                                        ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,

                                        ReferralInternalNote = model.ReferralInternalNote,
                                        ReferralDueDate = model.ReferralDueDate,
                                        RelationshipsWithOtherClientsNote = model.RelationshipsWithOtherClientsNote,
                                        IssuesAndDependencyStatusSummaryNote = model.IssuesAndDependencyStatusSummaryNote,
                                        ReferralHasChildrenFlag = model.ReferralHasChildrenFlag
                                    });



            }



            return Json(new { isSuccess = true, dataModel = model });
        }

        #endregion
        #region MHAT  Add Edit
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        public virtual ActionResult ReferralMHATAddEdit(string id, string roleId, string referralTypeCodeId)
        {
            var viewModel = GetReferralAddEditViewModel(id, roleId, referralTypeCodeId);

            viewModel.ReferralClientCategoryList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralClientCategoryGetByReferralID_spResult>("ref_ReferralClientCategoryGetByReferralID_sp",
                                    new ref_ReferralClientCategoryGetByReferralID_spParams
                                    {
                                        CaseID = UserManager.UserExtended.CaseID,
                                        UserID = UserManager.UserExtended.UserID,
                                        BatchLogJobID = Guid.NewGuid(),
                                        ReferralTypeCodeID = viewModel.ReferralTypeCodeID,
                                        ReferralID = viewModel.ReferralID,
                                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID

                                    }).Select(c => (ReferralClientCategoryListViewModel)(new ReferralClientCategoryListViewModel()).InjectFrom(c)).ToList();
            return View(viewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        [HttpPost]
        public virtual ActionResult ReferralMHATAddEdit(ReferralAddEditViewModel model)
        {

            if (model.ReferralID.HasValue && model.ReferralID.Value > 0)
            {
                var oReferralGet = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGet_spResult>("ref_ReferralGet_sp",
                                     new ref_ReferralGet_spParams
                                     {
                                         CaseID = UserManager.UserExtended.CaseID,
                                         UserID = UserManager.UserExtended.UserID,
                                         BatchLogJobID = Guid.NewGuid(),
                                         ReferralID = model.ReferralID
                                     }).FirstOrDefault();
                if (oReferralGet != null)
                {

                    UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralIUD_sp",
                                new ref_ReferralIUD_spParams
                                {
                                    IUD = "UPDATE",
                                    CaseAgencyID = oReferralGet.AgencyID,
                                    CaseID = UserManager.UserExtended.CaseID,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid(),
                                    ReferralID = model.ReferralID,
                                    RoleID = model.RoleID,
                                    ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                    ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                    ReferralTypeCodeID = model.ReferralTypeCodeID,
                                    ReferralRequestDate = model.ReferralRequestDate,
                                    ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                    ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,
                                    ReferralEIPFlag = oReferralGet.ReferralEIPFlag,
                                    ReferralEIPMostRecentFlag = oReferralGet.ReferralEIPMostRecentFlag,
                                    ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,
                                    ReferralEducationalStatusCodeID = oReferralGet.ReferralEducationalStatusCodeID,
                                    ReferralSchoolPreference = oReferralGet.ReferralSchoolPreference,
                                    ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,
                                    SpecialEducationNote = oReferralGet.SpecialEducationNote,
                                    SchoolProblemsNote = oReferralGet.SchoolProblemsNote,
                                    ExpulsionNote = oReferralGet.ExpulsionNote,
                                    EducationUnitOfServiceNote = oReferralGet.EducationUnitOfServiceNote,
                                    ReferralInternalNote = model.ReferralInternalNote,
                                    ReferralDueDate = model.ReferralDueDate,
                                    ReferralEndDate = model.ReferralEndDate,
                                    DelinquencyCaseNote = oReferralGet.DelinquencyCaseNote,
                                    IssuesAndDependencyStatusSummaryNote = oReferralGet.IssuesAndDependencyStatusSummaryNote,
                                    ReasonForReferralNote = model.ReasonForReferralNote,
                                    ReferralHasActiveCourtCaseFlag = oReferralGet.ReferralHasActiveCourtCaseFlag,
                                    ReferralHasChildrenFlag = oReferralGet.ReferralHasChildrenFlag,
                                    ReferralHaveLatestCourtReportFlag = oReferralGet.ReferralHaveLatestCourtReportFlag,
                                    ReferralReasonSummaryNote = oReferralGet.ReferralReasonSummaryNote,
                                    ReferralUrgencyCodeID = oReferralGet.ReferralUrgencyCodeID,
                                    ReferralYouthHasWorkingPhoneFlag = oReferralGet.ReferralYouthHasWorkingPhoneFlag,
                                    RelationshipsWithOtherClientsNote = model.RelationshipsWithOtherClientsNote,

                                });

                }
            }
            else
            {
                var dt = UtilityService.ExecStoredProcedureForDataTableADO("ref_ReferralIUD_sp",
                                       new ref_ReferralIUD_spParams
                                       {
                                           IUD = "INSERT",
                                           CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                           CaseID = UserManager.UserExtended.CaseID,
                                           UserID = UserManager.UserExtended.UserID,
                                           BatchLogJobID = Guid.NewGuid(),
                                           ReferralEndDate = model.ReferralEndDate,
                                           RoleID = model.RoleID,
                                           ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                           ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                           ReferralTypeCodeID = model.ReferralTypeCodeID,
                                           ReferralRequestDate = model.ReferralRequestDate,

                                           ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                           ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,


                                           ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,


                                           ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,

                                           ReferralInternalNote = model.ReferralInternalNote,
                                           ReferralDueDate = model.ReferralDueDate,
                                           RelationshipsWithOtherClientsNote = model.RelationshipsWithOtherClientsNote,

                                           ReasonForReferralNote = model.ReasonForReferralNote
                                       });
                if (dt.Rows.Count > 0)
                {
                    model.ReferralID = dt.Rows[0]["ReferralID"].ToInt();
                }


            }
            if (model.ReferralID.HasValue && model.ReferralID.Value > 0)
            {
                if (model.ReferralClientCategoryList.Any())
                {
                    foreach (var item in model.ReferralClientCategoryList)
                    {
                        var iud = item.IsChecked ? "INSERT" : "DELETE";

                        UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralClientCategoryIUD_sp",
                              new ref_ReferralClientCategoryIUD_spParams
                              {
                                  IUD = iud,
                                  UserID = UserManager.UserExtended.UserID,
                                  BatchLogJobID = Guid.NewGuid(),
                                  ReferralID = model.ReferralID,
                                  PersonClassificationCodeID = item.CodeID,
                                  ReferralClientCategoryID = item.ReferralClientCategoryID

                              });

                    }
                }


            }


            return Json(new { isSuccess = true, dataModel = model });
        }

        #endregion

        #region CSEC  Add Edit
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        public virtual ActionResult ReferralCSECAddEdit(string id, string roleId, string referralTypeCodeId)
        {
            var viewModel = GetReferralAddEditViewModel(id, roleId, referralTypeCodeId);

            viewModel.ReferralClientCategoryList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralClientCategoryGetByReferralID_spResult>("ref_ReferralClientCategoryGetByReferralID_sp",
                                    new ref_ReferralClientCategoryGetByReferralID_spParams
                                    {
                                        CaseID = UserManager.UserExtended.CaseID,
                                        UserID = UserManager.UserExtended.UserID,
                                        BatchLogJobID = Guid.NewGuid(),
                                        ReferralTypeCodeID = viewModel.ReferralTypeCodeID,
                                        ReferralID = viewModel.ReferralID,
                                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID

                                    }).Select(c => (ReferralClientCategoryListViewModel)(new ReferralClientCategoryListViewModel()).InjectFrom(c)).ToList();
            return View(viewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        [HttpPost]
        public virtual ActionResult ReferralCSECAddEdit(ReferralAddEditViewModel model)
        {

            if (model.ReferralID.HasValue && model.ReferralID.Value > 0)
            {
                var oReferralGet = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGet_spResult>("ref_ReferralGet_sp",
                                     new ref_ReferralGet_spParams
                                     {
                                         CaseID = UserManager.UserExtended.CaseID,
                                         UserID = UserManager.UserExtended.UserID,
                                         BatchLogJobID = Guid.NewGuid(),
                                         ReferralID = model.ReferralID
                                     }).FirstOrDefault();
                if (oReferralGet != null)
                {

                    UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralIUD_sp",
                                new ref_ReferralIUD_spParams
                                {
                                    IUD = "UPDATE",
                                    CaseAgencyID = oReferralGet.AgencyID,
                                    CaseID = UserManager.UserExtended.CaseID,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid(),
                                    ReferralID = model.ReferralID,
                                    RoleID = model.RoleID,
                                    ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                    ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                    ReferralTypeCodeID = model.ReferralTypeCodeID,
                                    ReferralRequestDate = model.ReferralRequestDate,
                                    ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                    ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,
                                    ReferralEIPFlag = oReferralGet.ReferralEIPFlag,
                                    ReferralEIPMostRecentFlag = oReferralGet.ReferralEIPMostRecentFlag,
                                    ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,
                                    ReferralEducationalStatusCodeID = oReferralGet.ReferralEducationalStatusCodeID,
                                    ReferralSchoolPreference = oReferralGet.ReferralSchoolPreference,
                                    ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,
                                    SpecialEducationNote = oReferralGet.SpecialEducationNote,
                                    SchoolProblemsNote = oReferralGet.SchoolProblemsNote,
                                    ExpulsionNote = oReferralGet.ExpulsionNote,
                                    EducationUnitOfServiceNote = oReferralGet.EducationUnitOfServiceNote,
                                    ReferralInternalNote = model.ReferralInternalNote,
                                    ReferralDueDate = model.ReferralDueDate,
                                    ReferralEndDate = model.ReferralEndDate,
                                    DelinquencyCaseNote = oReferralGet.DelinquencyCaseNote,
                                    IssuesAndDependencyStatusSummaryNote = model.IssuesAndDependencyStatusSummaryNote,
                                    ReasonForReferralNote = oReferralGet.ReasonForReferralNote,
                                    ReferralHasActiveCourtCaseFlag = oReferralGet.ReferralHasActiveCourtCaseFlag,
                                    ReferralHasChildrenFlag = oReferralGet.ReferralHasChildrenFlag,
                                    ReferralHaveLatestCourtReportFlag = oReferralGet.ReferralHaveLatestCourtReportFlag,
                                    ReferralReasonSummaryNote = oReferralGet.ReferralReasonSummaryNote,
                                    ReferralUrgencyCodeID = oReferralGet.ReferralUrgencyCodeID,
                                    ReferralYouthHasWorkingPhoneFlag = oReferralGet.ReferralYouthHasWorkingPhoneFlag,
                                    RelationshipsWithOtherClientsNote = model.RelationshipsWithOtherClientsNote,

                                });

                }
            }
            else
            {
                var dt = UtilityService.ExecStoredProcedureForDataTableADO("ref_ReferralIUD_sp",
                                       new ref_ReferralIUD_spParams
                                       {
                                           IUD = "INSERT",
                                           CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                           CaseID = UserManager.UserExtended.CaseID,
                                           UserID = UserManager.UserExtended.UserID,
                                           BatchLogJobID = Guid.NewGuid(),
                                           ReferralEndDate = model.ReferralEndDate,
                                           RoleID = model.RoleID,
                                           ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                           ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                           ReferralTypeCodeID = model.ReferralTypeCodeID,
                                           ReferralRequestDate = model.ReferralRequestDate,

                                           ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                           ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,


                                           ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,


                                           ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,

                                           ReferralInternalNote = model.ReferralInternalNote,
                                           ReferralDueDate = model.ReferralDueDate,
                                           RelationshipsWithOtherClientsNote = model.RelationshipsWithOtherClientsNote,

                                           IssuesAndDependencyStatusSummaryNote = model.IssuesAndDependencyStatusSummaryNote
                                       });
                if (dt.Rows.Count > 0)
                {
                    model.ReferralID = dt.Rows[0]["ReferralID"].ToInt();
                }


            }
            if (model.ReferralID.HasValue && model.ReferralID.Value > 0)
            {
                if (model.ReferralClientCategoryList.Any())
                {
                    foreach (var item in model.ReferralClientCategoryList)
                    {
                        var iud = item.IsChecked ? "INSERT" : "DELETE";

                        UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralClientCategoryIUD_sp",
                              new ref_ReferralClientCategoryIUD_spParams
                              {
                                  IUD = iud,
                                  UserID = UserManager.UserExtended.UserID,
                                  BatchLogJobID = Guid.NewGuid(),
                                  ReferralID = model.ReferralID,
                                  PersonClassificationCodeID = item.CodeID,
                                  ReferralClientCategoryID = item.ReferralClientCategoryID

                              });

                    }
                }


            }


            return Json(new { isSuccess = true, dataModel = model });
        }

        #endregion

        #region Peer Advocate  Add Edit
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        public virtual ActionResult ReferralPeerAdvocateAddEdit(string id, string roleId, string referralTypeCodeId)
        {
            var viewModel = GetReferralAddEditViewModel(id, roleId, referralTypeCodeId);

            viewModel.ReferralClientCategoryList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralClientCategoryGetByReferralID_spResult>("ref_ReferralClientCategoryGetByReferralID_sp",
                                    new ref_ReferralClientCategoryGetByReferralID_spParams
                                    {
                                        CaseID = UserManager.UserExtended.CaseID,
                                        UserID = UserManager.UserExtended.UserID,
                                        BatchLogJobID = Guid.NewGuid(),
                                        ReferralTypeCodeID = viewModel.ReferralTypeCodeID,
                                        ReferralID = viewModel.ReferralID,
                                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID

                                    }).Select(c => (ReferralClientCategoryListViewModel)(new ReferralClientCategoryListViewModel()).InjectFrom(c)).ToList();
            viewModel.ReferralUrgencyCodeList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(1504, includeCodeId: (viewModel.ReferralProgramEligibilityCodeID.HasValue ? viewModel.ReferralProgramEligibilityCodeID.Value : 0), agencyId: UserManager.UserExtended.CaseNumberAgencyID).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() });


            return View(viewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        [HttpPost]
        public virtual ActionResult ReferralPeerAdvocateAddEdit(ReferralAddEditViewModel model)
        {

            if (model.ReferralID.HasValue && model.ReferralID.Value > 0)
            {
                var oReferralGet = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGet_spResult>("ref_ReferralGet_sp",
                                     new ref_ReferralGet_spParams
                                     {
                                         CaseID = UserManager.UserExtended.CaseID,
                                         UserID = UserManager.UserExtended.UserID,
                                         BatchLogJobID = Guid.NewGuid(),
                                         ReferralID = model.ReferralID
                                     }).FirstOrDefault();
                if (oReferralGet != null)
                {

                    UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralIUD_sp",
                                new ref_ReferralIUD_spParams
                                {
                                    IUD = "UPDATE",
                                    CaseAgencyID = oReferralGet.AgencyID,
                                    CaseID = UserManager.UserExtended.CaseID,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid(),
                                    ReferralID = model.ReferralID,
                                    RoleID = model.RoleID,
                                    ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                    ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                    ReferralTypeCodeID = model.ReferralTypeCodeID,
                                    ReferralRequestDate = model.ReferralRequestDate,
                                    ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                    ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,
                                    ReferralEIPFlag = oReferralGet.ReferralEIPFlag,
                                    ReferralEIPMostRecentFlag = oReferralGet.ReferralEIPMostRecentFlag,
                                    ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,
                                    ReferralEducationalStatusCodeID = oReferralGet.ReferralEducationalStatusCodeID,
                                    ReferralSchoolPreference = oReferralGet.ReferralSchoolPreference,
                                    ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,
                                    SpecialEducationNote = oReferralGet.SpecialEducationNote,
                                    SchoolProblemsNote = oReferralGet.SchoolProblemsNote,
                                    ExpulsionNote = oReferralGet.ExpulsionNote,
                                    EducationUnitOfServiceNote = oReferralGet.EducationUnitOfServiceNote,
                                    ReferralInternalNote = model.ReferralInternalNote,
                                    ReferralDueDate = model.ReferralDueDate,
                                    ReferralEndDate = model.ReferralEndDate,
                                    DelinquencyCaseNote = oReferralGet.DelinquencyCaseNote,
                                    IssuesAndDependencyStatusSummaryNote = oReferralGet.IssuesAndDependencyStatusSummaryNote,
                                    ReasonForReferralNote = oReferralGet.ReasonForReferralNote,

                                    ReferralHasChildrenFlag = oReferralGet.ReferralHasChildrenFlag,
                                    ReferralHaveLatestCourtReportFlag = model.ReferralHaveLatestCourtReportFlag,
                                    ReferralReasonSummaryNote = model.ReferralReasonSummaryNote,

                                    ReferralYouthHasWorkingPhoneFlag = model.ReferralYouthHasWorkingPhoneFlag,
                                    RelationshipsWithOtherClientsNote = model.RelationshipsWithOtherClientsNote,
                                    ReferralHasActiveCourtCaseFlag = model.ReferralHasActiveCourtCaseFlag,
                                    ReferralUrgencyCodeID = model.ReferralUrgencyCodeID,

                                });

                }
            }
            else
            {
                var dt = UtilityService.ExecStoredProcedureForDataTableADO("ref_ReferralIUD_sp",
                                       new ref_ReferralIUD_spParams
                                       {
                                           IUD = "INSERT",
                                           CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                           CaseID = UserManager.UserExtended.CaseID,
                                           UserID = UserManager.UserExtended.UserID,
                                           BatchLogJobID = Guid.NewGuid(),
                                           ReferralEndDate = model.ReferralEndDate,
                                           RoleID = model.RoleID,
                                           ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                           ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                           ReferralTypeCodeID = model.ReferralTypeCodeID,
                                           ReferralRequestDate = model.ReferralRequestDate,

                                           ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                           ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,
                                           ReferralReasonSummaryNote = model.ReferralReasonSummaryNote,

                                           ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,
                                           ReferralHaveLatestCourtReportFlag = model.ReferralHaveLatestCourtReportFlag,

                                           ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,

                                           ReferralInternalNote = model.ReferralInternalNote,
                                           ReferralDueDate = model.ReferralDueDate,
                                           RelationshipsWithOtherClientsNote = model.RelationshipsWithOtherClientsNote,
                                           ReferralYouthHasWorkingPhoneFlag = model.ReferralYouthHasWorkingPhoneFlag,
                                           IssuesAndDependencyStatusSummaryNote = model.IssuesAndDependencyStatusSummaryNote
                                           ,
                                           ReferralUrgencyCodeID = model.ReferralUrgencyCodeID,
                                           ReferralHasActiveCourtCaseFlag = model.ReferralHasActiveCourtCaseFlag
                                       });
                if (dt.Rows.Count > 0)
                {
                    model.ReferralID = dt.Rows[0]["ReferralID"].ToInt();
                }


            }
            if (model.ReferralID.HasValue && model.ReferralID.Value > 0)
            {
                if (model.ReferralClientCategoryList.Any())
                {
                    foreach (var item in model.ReferralClientCategoryList)
                    {
                        var iud = item.IsChecked ? "INSERT" : "DELETE";

                        UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralClientCategoryIUD_sp",
                              new ref_ReferralClientCategoryIUD_spParams
                              {
                                  IUD = iud,
                                  UserID = UserManager.UserExtended.UserID,
                                  BatchLogJobID = Guid.NewGuid(),
                                  ReferralID = model.ReferralID,
                                  PersonClassificationCodeID = item.CodeID,
                                  ReferralClientCategoryID = item.ReferralClientCategoryID

                              });

                    }
                }


            }


            return Json(new { isSuccess = true, dataModel = model });
        }

        #endregion


        #region CARE  Add Edit
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        public virtual ActionResult ReferralImmigrationAdvisorAddEdit(string id, string roleId, string referralTypeCodeId)
        {
            var viewModel = GetReferralAddEditViewModel(id, roleId, referralTypeCodeId, true);




            if (viewModel.ReferralID.HasValue && viewModel.ReferralID.Value > 0)
            {
                //  viewModel.ReferralEducationalStatusList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(1501, includeCodeId: (viewModel.ReferralEducationalStatusCodeID.HasValue ? viewModel.ReferralEducationalStatusCodeID.Value : 0)).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() });
                viewModel.ReferralProgramEligibilityList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(1502, includeCodeId: (viewModel.ReferralProgramEligibilityCodeID.HasValue ? viewModel.ReferralProgramEligibilityCodeID.Value : 0), agencyId: UserManager.UserExtended.CaseNumberAgencyID).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() });

                viewModel.ImmigrationAttorneyList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGetImmigrationAttorneyList_spResult>("ref_ReferralGetImmigrationAttorneyList_sp",
                                         new ref_ReferralGetImmigrationAttorneyList_spParams
                                         {
                                             CaseID = UserManager.UserExtended.CaseID,
                                             UserID = UserManager.UserExtended.UserID,
                                             BatchLogJobID = Guid.NewGuid(),
                                             ReferralID = viewModel.ReferralID,
                                             CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID

                                         }).Select(o => new SelectListItem() { Text = o.NameDisplay, Value = o.PersonID.ToString() + "|" + o.Email + "|" + o.Phone }).ToList();
                viewModel.ReferralImmigrationGetByReferralList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralImmigrationGetByReferralID_spResult>("ref_ReferralImmigrationGetByReferralID_sp",
                                        new ref_ReferralImmigrationGetByReferralID_spParams
                                        {
                                            CaseID = UserManager.UserExtended.CaseID,
                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid(),
                                            ReferralID = viewModel.ReferralID,
                                            CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID

                                        }).Select(c => (ReferralImmigrationGetByReferralViewModel)(new ReferralImmigrationGetByReferralViewModel()).InjectFrom(c)).ToList();
                viewModel.ReferralImmigrationGetByReferralList.Add(new ReferralImmigrationGetByReferralViewModel()
                {

                    ImmigrationAgencyCodeID = 0,
                    ImmigrationAttorneyPersonID = 0,
                    ReferralImmigrationID = 0

                });

                foreach (var item in viewModel.ReferralImmigrationGetByReferralList)
                {
                    item.ImmigrationAgencyList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(1505, includeCodeId: (item.ImmigrationAgencyCodeID.HasValue ? item.ImmigrationAgencyCodeID.Value : 0), agencyId: UserManager.UserExtended.CaseNumberAgencyID).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() });

                }

                viewModel.ReferralGet602PetitionsList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGet602Petitions_spResult>("ref_ReferralGet602Petitions_sp",
                                        new ref_ReferralGet602Petitions_spParams
                                        {
                                            CaseID = UserManager.UserExtended.CaseID,
                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid(),
                                            ReferralID = viewModel.ReferralID,
                                            ReferralTypeCodeID = viewModel.ReferralTypeCodeID,
                                            RoleID = viewModel.RoleID

                                        }).ToList();
                viewModel.ReferralReliefGetByReferralList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralReliefGetByReferralID_spResult>("ref_ReferralReliefGetByReferralID_sp",
                                      new ref_ReferralReliefGetByReferralID_spParams
                                      {
                                          CaseID = UserManager.UserExtended.CaseID,
                                          UserID = UserManager.UserExtended.UserID,
                                          BatchLogJobID = Guid.NewGuid(),
                                          ReferralID = viewModel.ReferralID,
                                          CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID

                                      }).Select(c => (ReferralReliefGetByReferralViewModel)(new ReferralReliefGetByReferralViewModel()).InjectFrom(c)).ToList();
                viewModel.ReferralReliefGetByReferralList.Add(new ReferralReliefGetByReferralViewModel()
                {
                    ReferralReliefStatusCodeID = 0,
                    ReferralReliefTypeCodeID = 0,
                    ReferralReliefID = 0
                });
                foreach (var item in viewModel.ReferralReliefGetByReferralList)
                {
                    item.ReliefTypeList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(1506, includeCodeId: (item.ReferralReliefTypeCodeID.HasValue ? item.ReferralReliefTypeCodeID.Value : 0), agencyId: UserManager.UserExtended.CaseNumberAgencyID).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() });
                    item.ReliefStatusList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(1507, includeCodeId: (item.ReferralReliefStatusCodeID.HasValue ? item.ReferralReliefStatusCodeID.Value : 0), agencyId: UserManager.UserExtended.CaseNumberAgencyID).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() });

                }

            }

            #region  Dropdown
            var oCountryofOriginList = ReferralGetCodes("CountryOfOrigin", viewModel.ReferralID.ToInt());
            if (oCountryofOriginList.Any(o => o.Selected == 1))
            {
                viewModel.CountryOfOriginCodeID = oCountryofOriginList.FirstOrDefault(o => o.Selected == 1).CodeID;

            }
            viewModel.CountryofOriginList = oCountryofOriginList.Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();

            var UnaccompaniedChildList = ReferralGetCodes("UnaccompaniedChild", viewModel.ReferralID.ToInt());
            if (UnaccompaniedChildList.Any(o => o.Selected == 1))
            {
                viewModel.UnaccompaniedChildCodeID = UnaccompaniedChildList.FirstOrDefault(o => o.Selected == 1).CodeID;

            }
            viewModel.UnaccompaniedChildList = UnaccompaniedChildList.Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();

            var EOIRList = ReferralGetCodes("EOIR", viewModel.ReferralID.ToInt());
            if (EOIRList.Any(o => o.Selected == 1))
            {
                viewModel.EOIRCodeID = EOIRList.FirstOrDefault(o => o.Selected == 1).CodeID;

            }
            viewModel.EOIRList = EOIRList.Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();


            var StatusForAssignmentList = ReferralGetCodes("StatusForAssignment", viewModel.ReferralID.ToInt());
            if (StatusForAssignmentList.Any(o => o.Selected == 1))
            {
                viewModel.StatusForAssignmentCodeID = StatusForAssignmentList.FirstOrDefault(o => o.Selected == 1).CodeID;

            }
            viewModel.StatusForAssignmentList = StatusForAssignmentList.Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();

            var SIJSFindingsStatusList = ReferralGetCodes("SIJSFindingsStatus", viewModel.ReferralID.ToInt());
            if (SIJSFindingsStatusList.Any(o => o.Selected == 1))
            {
                viewModel.SIJSFindingStatusCodeID = SIJSFindingsStatusList.FirstOrDefault(o => o.Selected == 1).CodeID;

            }
            viewModel.SIJSFindingsStatusList = SIJSFindingsStatusList.Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();


            var CDSSCaseTypeList = ReferralGetCodes("CDSSCaseType", viewModel.ReferralID.ToInt());
            if (CDSSCaseTypeList.Any(o => o.Selected == 1))
            {
                viewModel.CDSSCaseTypeCodeID = CDSSCaseTypeList.FirstOrDefault(o => o.Selected == 1).CodeID;

            }
            viewModel.CDSSCaseTypeList = CDSSCaseTypeList.Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();

            viewModel.E317List = UtilityService.ExecStoredProcedureWithResults<ref_ReferralImmigrationGetCodes_spResult>("ref_ReferralImmigrationGetCodes_sp",
                                        new ref_ReferralImmigrationGetCodes_spParams
                                        {
                                            CaseID = UserManager.UserExtended.CaseID,
                                            UserID = UserManager.UserExtended.UserID,
                                            ReferralID = viewModel.ReferralID.ToInt(),
                                            CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                            LoadOption = "317e",

                                        }).Select(o => new SelectListItem()
                                        {
                                            Text = o.CodeDisplay,
                                            Value = o.CodeID.ToString()
                                        });
            viewModel.CDSSGrantYearQuarterList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGetYearQuarter_spResult>("ref_ReferralGetYearQuarter_sp",
                                     new ref_ReferralGetYearQuarter_spParams
                                     {
                                         CaseID = UserManager.UserExtended.CaseID,
                                         UserID = UserManager.UserExtended.UserID,
                                         ReferralID = viewModel.ReferralID.ToInt(),
                                         CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                         LoadOption = "CDSSGrant",

                                     }).Select(o => new SelectListItem()
                                     {
                                         Text = o.YearQuarter,
                                         Value = o.YearQuarterID.ToString()
                                     });

            #endregion
            return View(viewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        [HttpPost]
        public virtual ActionResult ReferralImmigrationAdvisorAddEdit(ReferralAddEditViewModel model)
        {
            #region Add/Edit Save
            if (model.ReferralID.HasValue && model.ReferralID.Value > 0)
            {
                var oReferralGet = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGet_spResult>("ref_ReferralGet_sp",
                                     new ref_ReferralGet_spParams
                                     {
                                         CaseID = UserManager.UserExtended.CaseID,
                                         UserID = UserManager.UserExtended.UserID,
                                         BatchLogJobID = Guid.NewGuid(),
                                         ReferralID = model.ReferralID
                                     }).FirstOrDefault();
                if (oReferralGet != null)
                {

                    UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralIUD_sp",
                                new ref_ReferralIUD_spParams
                                {
                                    IUD = "UPDATE",
                                    CaseAgencyID = oReferralGet.AgencyID,
                                    CaseID = UserManager.UserExtended.CaseID,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid(),
                                    ReferralID = model.ReferralID,
                                    RoleID = model.RoleID,
                                    ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                    ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                    ReferralTypeCodeID = model.ReferralTypeCodeID,
                                    ReferralRequestDate = model.ReferralRequestDate,
                                    ReferralCompanionCaseFlag = oReferralGet.ReferralCompanionCaseFlag,
                                    ReferralConflictHistoryFlag = oReferralGet.ReferralConflictHistoryFlag,
                                    ReferralEIPFlag = oReferralGet.ReferralEIPFlag,
                                    ReferralEIPMostRecentFlag = oReferralGet.ReferralEIPMostRecentFlag,
                                    ReferralFrequencyOfUpdatesCodeID = oReferralGet.ReferralFrequencyOfUpdatesCodeID,
                                    ReferralEducationalStatusCodeID = oReferralGet.ReferralEducationalStatusCodeID,
                                    ReferralSchoolPreference = oReferralGet.ReferralSchoolPreference,
                                    ReferralProgramEligibilityCodeID = oReferralGet.ReferralProgramEligibilityCodeID,
                                    SpecialEducationNote = oReferralGet.SpecialEducationNote,
                                    SchoolProblemsNote = oReferralGet.SchoolProblemsNote,
                                    ExpulsionNote = oReferralGet.ExpulsionNote,
                                    EducationUnitOfServiceNote = oReferralGet.EducationUnitOfServiceNote,
                                    ReferralInternalNote = model.ReferralInternalNote,
                                    ReferralDueDate = model.ReferralDueDate,
                                    ReferralEndDate = model.ReferralEndDate,
                                    DelinquencyCaseNote = model.DelinquencyCaseNote,
                                    IssuesAndDependencyStatusSummaryNote = oReferralGet.IssuesAndDependencyStatusSummaryNote,
                                    ReasonForReferralNote = model.ReasonForReferralNote,
                                    ReferralHasActiveCourtCaseFlag = oReferralGet.ReferralHasActiveCourtCaseFlag,
                                    ReferralHasChildrenFlag = oReferralGet.ReferralHasChildrenFlag,
                                    ReferralHaveLatestCourtReportFlag = oReferralGet.ReferralHaveLatestCourtReportFlag,
                                    ReferralReasonSummaryNote = oReferralGet.ReferralReasonSummaryNote,
                                    ReferralUrgencyCodeID = oReferralGet.ReferralUrgencyCodeID,
                                    ReferralYouthHasWorkingPhoneFlag = oReferralGet.ReferralYouthHasWorkingPhoneFlag,
                                    RelationshipsWithOtherClientsNote = oReferralGet.RelationshipsWithOtherClientsNote,
                                    ANumber = model.ANumber,
                                    ANumberPersonID = model.ANumber_PersonID,
                                    ANumber_LegalnumberID = model.ANumber_LegalNumberID,
                                    CDSSCaseTypeCodeID = model.CDSSCaseTypeCodeID,
                                    CDSSGrantYearQuarterID = model.CDSSGrantYearQuarterID,
                                    CDSSReportingFlag = model.CDSSReportingFlag,
                                    CountryOfOriginCodeID = model.CountryOfOriginCodeID,
                                    EOIRCodeID = model.EOIRCodeID,
                                    SIJSFindingStatusCodeID = model.SIJSFindingStatusCodeID,
                                    SIJSFindingStatusDate = model.SIJSFindingStatusDate,
                                    StatusForAssignmentCodeID = model.StatusForAssignmentCodeID,
                                    UnaccompaniedChildCodeID = model.UnaccompaniedChildCodeID,
                                });

                }


                if (model.ReferralImmigrationGetByReferralList.Any())
                {
                    foreach (var item in model.ReferralImmigrationGetByReferralList)
                    {
                        var iud = item.ReferralImmigrationID.HasValue && item.ReferralImmigrationID.Value > 0 ? "UPDATE" : "INSERT";

                        UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralImmigrationIUD_sp",
                              new ref_ReferralImmigrationIUD_spParams
                              {
                                  IUD = iud,
                                  UserID = UserManager.UserExtended.UserID,
                                  BatchLogJobID = Guid.NewGuid(),
                                  ReferralID = model.ReferralID,
                                  ImmigrationAgencyCodeID = item.ImmigrationAgencyCodeID,
                                  ReferralImmigrationID = item.ReferralImmigrationID,
                                  ImmigrationAttorneyPersonID = item.ImmigrationAttorneyPersonID,
                                  ImmigrationEndDate = item.ImmigrationEndDate,
                                  ImmigrationStartDate = item.ImmigrationStartDate,
                                  Immigration317eCodeID = item.Immigration317eCodeID

                              });

                    }
                }
                if (model.ReferralReliefGetByReferralList.Any())
                {
                    foreach (var item in model.ReferralReliefGetByReferralList)
                    {
                        var iud = item.ReferralReliefID.HasValue && item.ReferralReliefID.Value > 0 ? "UPDATE" : "INSERT";

                        UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralReliefIUD_sp",
                              new ref_ReferralReliefIUD_spParams
                              {
                                  IUD = iud,
                                  UserID = UserManager.UserExtended.UserID,
                                  BatchLogJobID = Guid.NewGuid(),
                                  ReferralID = model.ReferralID,
                                  ReferralReliefID = item.ReferralReliefID,
                                  ReferralReliefNote = item.ReferralReliefNote,
                                  ReferralReliefStatusCodeID = item.ReferralReliefStatusCodeID,
                                  ReferralReliefStatusDate = item.ReferralReliefStatusDate.ToDateTime(),
                                  ReferralReliefTypeCodeID = item.ReferralReliefTypeCodeID,
                                  ReferralReliefPriorityDate = item.ReferralReliefPriorityDate.ToDateTimeNullableValue()

                              });

                    }
                }
            }
            else
            {
                var oReferralGet = UtilityService.ExecStoredProcedureWithResults<ref_ReferralIUD_spResult>("ref_ReferralIUD_sp",
                                       new ref_ReferralIUD_spParams
                                       {
                                           IUD = "INSERT",
                                           CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                           CaseID = UserManager.UserExtended.CaseID,
                                           UserID = UserManager.UserExtended.UserID,
                                           BatchLogJobID = Guid.NewGuid(),
                                           ReferralEndDate = model.ReferralEndDate,
                                           RoleID = model.RoleID,
                                           ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                           ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                           ReferralTypeCodeID = model.ReferralTypeCodeID,
                                           ReferralRequestDate = model.ReferralRequestDate,
                                           ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                           ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,
                                           ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,
                                           ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,
                                           ReferralInternalNote = model.ReferralInternalNote,
                                           ReferralDueDate = model.ReferralDueDate,
                                           ReasonForReferralNote = model.ReasonForReferralNote,
                                           ReferralHasChildrenFlag = model.ReferralHasChildrenFlag,
                                           ANumber = model.ANumber,
                                           ANumberPersonID = model.ANumber_PersonID,
                                           ANumber_LegalnumberID = model.ANumber_LegalNumberID,
                                           CDSSCaseTypeCodeID = model.CDSSCaseTypeCodeID,
                                           CDSSGrantYearQuarterID = model.CDSSGrantYearQuarterID,
                                           CDSSReportingFlag = model.CDSSReportingFlag,
                                           CountryOfOriginCodeID = model.CountryOfOriginCodeID,
                                           EOIRCodeID = model.EOIRCodeID,
                                           SIJSFindingStatusCodeID = model.SIJSFindingStatusCodeID,
                                           SIJSFindingStatusDate = model.SIJSFindingStatusDate,
                                           StatusForAssignmentCodeID = model.StatusForAssignmentCodeID,
                                           UnaccompaniedChildCodeID = model.UnaccompaniedChildCodeID,
                                       }).FirstOrDefault();


                if (oReferralGet != null)
                {
                    model.ReferralID = oReferralGet.ReferralID;
                }
            }
            #endregion


            return Json(new { isSuccess = true, dataModel = model, URL = "/Case/ReferralImmigrationAdvisorAddEdit/" + model.ReferralID.ToEncrypt() });
        }


        public IEnumerable<CodeViewModel> ReferralGetCodes(string loadOption, int referralId)
        {

            return UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<ref_ReferralGetCodes_spResult>(
                    "ref_ReferralGetCodes_sp", new ref_ReferralGetCodes_spParams
                    {
                        LoadOption = loadOption,
                        ReferralID = referralId,
                        CaseID = UserEnvironment.UserManager.UserExtended.CaseID,
                        UserID = UserEnvironment.UserManager.UserExtended.UserID,
                        CaseAgencyID = UserEnvironment.UserManager.UserExtended.CaseNumberAgencyID,

                    }).Select(o => new CodeViewModel() { CodeID = o.CodeID.Value, CodeDisplay = o.CodeDisplay, Selected = o.Selected.Value }).ToList();

        }



        public IEnumerable<ref_ReferralEventGetCodes_spResult> ReferralEventGetCodes(string loadOption, int? referralId, int? eventId)
        {

            return UtilityService.ExecStoredProcedureWithResults<ref_ReferralEventGetCodes_spResult>("ref_ReferralEventGetCodes_sp",
                                                      new ref_ReferralEventGetCodes_spParams
                                                      {
                                                          CaseID = UserManager.UserExtended.CaseID,
                                                          UserID = UserManager.UserExtended.UserID,
                                                          BatchLogJobID = Guid.NewGuid(),
                                                          ReferralID = referralId,
                                                          CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                                          LoadOption = loadOption,
                                                          ReferralEventID = eventId
                                                      });
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        public virtual ActionResult ReferralEventAddEdit(string id, string eventId, int? pID = 0)
        {
            var viewModel = new ReferralEventViewModel
            {
                ReferralID = id.ToDecrypt().ToInt(),
                ReferralEventID = eventId.ToDecrypt().ToInt(),
                ControlType = UtilityFunctions.GetNoteControlType("Case/ReferralEventAddEdit")
            };
            if (viewModel.ReferralEventID > 0)
            {
                var oEvent = UtilityService.ExecStoredProcedureWithResults<ref_ReferralEventGet_spResult>("ref_ReferralEventGet_sp",
                                    new ref_ReferralEventGet_spParams
                                    {
                                        ReferralEventID = viewModel.ReferralEventID,
                                        UserID = UserManager.UserExtended.UserID,
                                        BatchLogJobID = Guid.NewGuid()
                                    }).FirstOrDefault();
                viewModel.InjectFrom(oEvent);
            }


            if (viewModel.ReferralID.HasValue && viewModel.ReferralID.Value > 0)
            {
                #region  
                var eventTypeList = ReferralEventGetCodes("EventType", viewModel.ReferralID, viewModel.ReferralEventID);
                var oEventSelected = eventTypeList.FirstOrDefault(o => o.Selected == 1);
                if (oEventSelected != null)
                {
                    viewModel.ReferralEventTypeCodeID = oEventSelected.CodeID;
                }
                viewModel.EventTypeList = eventTypeList.Select(o => new SelectListItem { Value = o.CodeID.ToString(), Text = o.CodeDisplay }).ToList();

                var clientPresentList = ReferralEventGetCodes("ClientPresent", viewModel.ReferralID, viewModel.ReferralEventID);
                var oClientPresentSelected = clientPresentList.FirstOrDefault(o => o.Selected == 1);
                if (oClientPresentSelected != null)
                {
                    viewModel.ReferralEventClientPresentFlag = (short)oClientPresentSelected.CodeID;
                }
                viewModel.ClientPresentList = clientPresentList.Select(o => new SelectListItem { Value = o.CodeID.ToString(), Text = o.CodeDisplay }).ToList();

                var locationList = ReferralEventGetCodes("EventLocation", viewModel.ReferralID, viewModel.ReferralEventID);
                var oLocationListSelected = locationList.FirstOrDefault(o => o.Selected == 1);
                if (oLocationListSelected != null)
                {
                    viewModel.ReferralEventLocationCodeID = oLocationListSelected.CodeID;
                }
                viewModel.LocationList = locationList.Select(o => new SelectListItem { Value = o.CodeID.ToString(), Text = o.CodeDisplay }).ToList();

                #endregion

                viewModel.AppearingAttorneyList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralEventGetAppearingAttorneys_spResult>("ref_ReferralEventGetAppearingAttorneys_sp",
                                        new ref_ReferralEventGetAppearingAttorneys_spParams
                                        {
                                            CaseID = UserManager.UserExtended.CaseID,
                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid(),
                                            ReferralID = viewModel.ReferralID,
                                            CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID
                                            ,
                                            LoadOption = "EventAddEdit",
                                            ReferralEventID = viewModel.ReferralEventID
                                        }).Select(o => new SelectListItem { Value = o.PersonID.ToString(), Text = o.NameDisplay }).ToList();


                viewModel.EventHistoryList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralEventGetHistory_spResult>("ref_ReferralEventGetHistory_sp",
                                        new ref_ReferralEventGetHistory_spParams
                                        {

                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid(),
                                            ReferralID = viewModel.ReferralID,
                                            LoadOption = "EventAddEdit"
                                        }).ToList();


            }
            if (pID > 0)
                TempData["PageID"] = pID.ToDecimal().ToInt();
            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult ReferralEventAddEdit(ReferralEventViewModel viewModel)
        {
            UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralEventIUD_sp", new ref_ReferralEventIUD_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID,
                IUD = viewModel.ReferralEventID.ToInt() == 0 ? "INSERT" : "UPDATE",
                ReferralEventID = viewModel.ReferralEventID,
                ReferralEventAppearingPersonID = viewModel.ReferralEventAppearingPersonID,
                ReferralEventClientPresentFlag = viewModel.ReferralEventClientPresentFlag,
                ReferralEventDateTime = viewModel.ReferralEventDateTime,
                ReferralEventLocationCodeID = viewModel.ReferralEventLocationCodeID,
                ReferralEventNote = viewModel.ReferralEventNote,
                ReferralEventTypeCodeID = viewModel.ReferralEventTypeCodeID,
                ReferralID = viewModel.ReferralID,
            });

            return Json(new { isSuccess = true });
        }
        [HttpPost]
        public virtual JsonResult ReferralEventDelete(string id)
        {
            UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralEventIUD_sp", new ref_ReferralEventIUD_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID,
                IUD = "DELETE",
                ReferralEventID = id.ToDecrypt().ToInt(),

            });

            return Json(new { isSuccess = true });
        }

        #endregion
        #region CARE  Add Edit
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        public virtual ActionResult ReferralMentorAddEdit(string id, string roleId, string referralTypeCodeId)
        {
            var viewModel = GetReferralAddEditViewModel(id, roleId, referralTypeCodeId);
            viewModel.ReferralStatusList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralStatusGetCodes_spResult>("ref_ReferralStatusGetCodes_sp", new ref_ReferralStatusGetCodes_spParams
            {
                CaseID = UserManager.UserExtended.CaseID,
                CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                UserID = UserManager.UserExtended.UserID,
                ReferralID = viewModel.ReferralID,
                LoadOption = "Status"
            }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();
            viewModel.ReferralStatusGetHistoryList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralStatusGetHistory_spResult>("ref_ReferralStatusGetHistory_sp", new ref_ReferralStatusGetHistory_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                ReferralID = viewModel.ReferralID,
            }).ToList();

            viewModel.ReferralStatusGetHistoryList.Insert(0, new ref_ReferralStatusGetHistory_spResult()
            {
                ReferralStatusID = 0

            });

            return View(viewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Referral)]
        [HttpPost]
        public virtual ActionResult ReferralMentorAddEdit(ReferralAddEditViewModel model)
        {
            if (model.ReferralID.HasValue && model.ReferralID.Value > 0)
            {
                var oReferralGet = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGet_spResult>("ref_ReferralGet_sp",
                                     new ref_ReferralGet_spParams
                                     {
                                         CaseID = UserManager.UserExtended.CaseID,
                                         UserID = UserManager.UserExtended.UserID,
                                         BatchLogJobID = Guid.NewGuid(),
                                         ReferralID = model.ReferralID
                                     }).FirstOrDefault();
                if (oReferralGet != null)
                {

                    UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralIUD_sp",
                                new ref_ReferralIUD_spParams
                                {
                                    IUD = "UPDATE",
                                    CaseAgencyID = oReferralGet.AgencyID,
                                    CaseID = UserManager.UserExtended.CaseID,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid(),
                                    ReferralID = model.ReferralID,
                                    RoleID = model.RoleID,
                                    ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                    ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                    ReferralTypeCodeID = model.ReferralTypeCodeID,
                                    ReferralRequestDate = model.ReferralRequestDate,
                                    ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                    ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,
                                    ReferralEIPFlag = oReferralGet.ReferralEIPFlag,
                                    ReferralEIPMostRecentFlag = oReferralGet.ReferralEIPMostRecentFlag,
                                    ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,
                                    ReferralEducationalStatusCodeID = oReferralGet.ReferralEducationalStatusCodeID,
                                    ReferralSchoolPreference = oReferralGet.ReferralSchoolPreference,
                                    ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,
                                    SpecialEducationNote = oReferralGet.SpecialEducationNote,
                                    SchoolProblemsNote = oReferralGet.SchoolProblemsNote,
                                    ExpulsionNote = oReferralGet.ExpulsionNote,
                                    EducationUnitOfServiceNote = oReferralGet.EducationUnitOfServiceNote,
                                    ReferralInternalNote = model.ReferralInternalNote,
                                    ReferralDueDate = model.ReferralDueDate,
                                    ReferralEndDate = model.ReferralEndDate,
                                    DelinquencyCaseNote = oReferralGet.DelinquencyCaseNote,
                                    IssuesAndDependencyStatusSummaryNote = model.IssuesAndDependencyStatusSummaryNote,
                                    ReasonForReferralNote = oReferralGet.ReasonForReferralNote,
                                    ReferralHasActiveCourtCaseFlag = oReferralGet.ReferralHasActiveCourtCaseFlag,
                                    ReferralHasChildrenFlag = model.ReferralHasChildrenFlag,
                                    ReferralHaveLatestCourtReportFlag = oReferralGet.ReferralHaveLatestCourtReportFlag,
                                    ReferralReasonSummaryNote = oReferralGet.ReferralReasonSummaryNote,
                                    ReferralUrgencyCodeID = oReferralGet.ReferralUrgencyCodeID,
                                    ReferralYouthHasWorkingPhoneFlag = oReferralGet.ReferralYouthHasWorkingPhoneFlag,
                                    RelationshipsWithOtherClientsNote = model.RelationshipsWithOtherClientsNote,

                                });

                }
            }
            else
            {
                var oReferral = UtilityService.ExecStoredProcedureWithResults<ref_ReferralIUD_spResult>("ref_ReferralIUD_sp",
                                          new ref_ReferralIUD_spParams
                                          {
                                              IUD = "INSERT",
                                              CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                              CaseID = UserManager.UserExtended.CaseID,
                                              UserID = UserManager.UserExtended.UserID,
                                              BatchLogJobID = Guid.NewGuid(),
                                              ReferralEndDate = model.ReferralEndDate,
                                              RoleID = model.RoleID,
                                              ReferralRequestedByPersonID = model.ReferralRequestedByPersonID,
                                              ReferralRequestedForPersonID = model.ReferralRequestedForPersonID,
                                              ReferralTypeCodeID = model.ReferralTypeCodeID,
                                              ReferralRequestDate = model.ReferralRequestDate,

                                              ReferralCompanionCaseFlag = model.ReferralCompanionCaseFlag,
                                              ReferralConflictHistoryFlag = model.ReferralConflictHistoryFlag,


                                              ReferralFrequencyOfUpdatesCodeID = model.ReferralFrequencyOfUpdatesCodeID,


                                              ReferralProgramEligibilityCodeID = model.ReferralProgramEligibilityCodeID,

                                              ReferralInternalNote = model.ReferralInternalNote,
                                              ReferralDueDate = model.ReferralDueDate,
                                              RelationshipsWithOtherClientsNote = model.RelationshipsWithOtherClientsNote,
                                              IssuesAndDependencyStatusSummaryNote = model.IssuesAndDependencyStatusSummaryNote,
                                              ReferralHasChildrenFlag = model.ReferralHasChildrenFlag
                                          }).FirstOrDefault();
                if (oReferral != null)
                {
                    model.ReferralID = oReferral.ReferralID;
                }



            }

            if (model.ReferralStatusGetHistoryList.Any())
            {
                var callIUD = "";
                foreach (var item in model.ReferralStatusGetHistoryList)
                {
                    callIUD = "";
                    if (item.ReferralStatusID > 0 && item.ReferralStatusDate.IsNullOrEmpty() && item.ReferralStatusCodeID.ToInt() == 0)
                    {
                        //delete
                        callIUD = "DELETE";
                    }
                    else if (item.ReferralStatusID > 0 && !item.ReferralStatusDate.IsNullOrEmpty() && item.ReferralStatusCodeID.ToInt() > 0)
                    {
                        //update
                        callIUD = "UPDATE";
                    }
                    else if (item.ReferralStatusID.ToInt() == 0 && !item.ReferralStatusDate.IsNullOrEmpty() && item.ReferralStatusCodeID.ToInt() > 0)
                    {
                        //insert
                        callIUD = "INSERT";
                    }
                    if (!callIUD.IsNullOrEmpty())
                    {
                        UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralStatusIUD_sp",
                                                   new ref_ReferralStatusIUD_spParams
                                                   {
                                                       IUD = callIUD,
                                                       ReferralID = model.ReferralID,
                                                       ReferralStatusCodeID = item.ReferralStatusCodeID,
                                                       ReferralStatusDate = item.ReferralStatusDate.ToDateTimeNullableValue(),
                                                       ReferralStatusID = item.ReferralStatusID,
                                                       UserID = UserManager.UserExtended.UserID,
                                                       BatchLogJobID = Guid.NewGuid()
                                                   });
                    }


                }
            }


            return Json(new { isSuccess = true, dataModel = model });
        }

        #endregion
        #region Get Model

        public ReferralAddEditViewModel GetReferralAddEditViewModel(string id, string roleId, string referralTypeCodeId, bool isImmiAdvPage = false)
        {

            var viewModel = new ReferralAddEditViewModel();
            viewModel.RoleID = roleId.ToDecrypt().ToInt();
            viewModel.ReferralTypeCodeID = referralTypeCodeId.ToDecrypt().ToInt();
            viewModel.ReferralID = id.ToDecrypt().ToInt();

            if (viewModel.ReferralID.HasValue && viewModel.ReferralID.Value > 0)
            {
                var oReferralGet = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGet_spResult>("ref_ReferralGet_sp",
                                        new ref_ReferralGet_spParams
                                        {
                                            CaseID = UserManager.UserExtended.CaseID,
                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid(),
                                            ReferralID = viewModel.ReferralID,

                                        }).FirstOrDefault();
                if (oReferralGet != null)
                {

                    viewModel.InjectFrom(oReferralGet);
                    if (viewModel.CaseID.HasValue && viewModel.CaseID.Value > 0 && viewModel.CaseID.Value != UserManager.UserExtended.CaseID)
                    {
                        UserManager.UpdateCaseStatusBar(viewModel.CaseID.Value);
                    }

                }
            }
            else
            {
                viewModel.ReferralRequestDate = DateTime.Now;
            }


            #region Page Load 

            viewModel.ReferralRequestedByList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGetRequestBy_spResult>("ref_ReferralGetRequestBy_sp",
                                               new ref_ReferralGetRequestBy_spParams
                                               {
                                                   CaseID = UserManager.UserExtended.CaseID,
                                                   UserID = UserManager.UserExtended.UserID,
                                                   BatchLogJobID = Guid.NewGuid(),
                                                   ReferralTypeCodeID = viewModel.ReferralTypeCodeID,
                                                   ReferralID = viewModel.ReferralID,
                                                   ReferralRequestedByPersonID = viewModel.ReferralRequestedByPersonID,
                                                   CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,

                                               }).Select(x => new SelectListItem { Text = x.DisplayName, Value = x.PersonID.ToString() });
            var requestForList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGetRequestFor_spResult>("ref_ReferralGetRequestFor_sp",
                                                new ref_ReferralGetRequestFor_spParams
                                                {
                                                    CaseID = UserManager.UserExtended.CaseID,
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid(),
                                                    ReferralTypeCodeID = viewModel.ReferralTypeCodeID,
                                                    ReferralID = viewModel.ReferralID,
                                                    ReferralRequestedForPersonID = viewModel.ReferralRequestedForPersonID,
                                                    CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID

                                                }).ToList();

            if (viewModel.ReferralID <= 0)
            {
                var oSelectedReferralRequestedForPerson = requestForList.FirstOrDefault(o => o.Current == 1);
                if (oSelectedReferralRequestedForPerson != null)
                    viewModel.ReferralRequestedForPersonID = oSelectedReferralRequestedForPerson.PersonID;
            }

            viewModel.ReferralRequestedForList = requestForList.Select(x => new SelectListItem { Text = x.DisplayName, Value = x.PersonID.ToString() });

            viewModel.ReferralContactInfoList = UtilityService.ExecStoredProcedureWithResults<ref_ReferralContactInfo_spResult>("ref_ReferralContactInfo_sp",
                                            new ref_ReferralContactInfo_spParams
                                            {
                                                CaseID = UserManager.UserExtended.CaseID,
                                                UserID = UserManager.UserExtended.UserID,
                                                BatchLogJobID = Guid.NewGuid(),
                                                ReferralTypeCodeID = viewModel.ReferralTypeCodeID,
                                                ReferralID = viewModel.ReferralID,
                                                RoleID = viewModel.RoleID,


                                            }).ToList();

            viewModel.ReferralHeader = UtilityService.ExecStoredProcedureWithResults<ref_ReferralHeader_spResult>("ref_ReferralHeader_sp",
                                      new ref_ReferralHeader_spParams
                                      {
                                          CaseID = UserManager.UserExtended.CaseID,
                                          UserID = UserManager.UserExtended.UserID,
                                          BatchLogJobID = Guid.NewGuid(),
                                          ReferralTypeCodeID = viewModel.ReferralTypeCodeID,
                                          ReferralID = viewModel.ReferralID,
                                          RoleID = viewModel.RoleID

                                      }).FirstOrDefault();

            if (viewModel.ReferralHeader.ANumber_PersonID.HasValue)
                viewModel.ANumber_PersonID = viewModel.ReferralHeader.ANumber_PersonID;
            if (viewModel.ReferralHeader.ANumber_LegalNumberID.HasValue)
                viewModel.ANumber_LegalNumberID = viewModel.ReferralHeader.ANumber_LegalNumberID;
            if (!viewModel.ReferralHeader.ANumber.IsNullOrEmpty())
                viewModel.ANumber = viewModel.ReferralHeader.ANumber;


            if (!isImmiAdvPage)
            {
                viewModel.ReferralFrequencyOfUpdatesList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(1503, includeCodeId: (viewModel.ReferralFrequencyOfUpdatesCodeID.HasValue ? viewModel.ReferralFrequencyOfUpdatesCodeID.Value : 0), agencyId: UserManager.UserExtended.CaseNumberAgencyID).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() });
                viewModel.ReferralEducationalStatusList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(1501, includeCodeId: (viewModel.ReferralEducationalStatusCodeID.HasValue ? viewModel.ReferralEducationalStatusCodeID.Value : 0), agencyId: UserManager.UserExtended.CaseNumberAgencyID).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() });
                viewModel.ReferralProgramEligibilityList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(1502, includeCodeId: (viewModel.ReferralProgramEligibilityCodeID.HasValue ? viewModel.ReferralProgramEligibilityCodeID.Value : 0), agencyId: UserManager.UserExtended.CaseNumberAgencyID).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() });
            }
            #endregion


            return viewModel;
        }

        #endregion



        [ChildActionOnly]
        public virtual PartialViewResult ReferralTabs(int id)
        {
            if (id <= 0)
                return PartialView("_partialReferralTabs", null);
            var model = new ReferralAddEditStepsViewModel();
            model.ReferralModel = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGet_spResult>("ref_ReferralGet_sp",
                                          new ref_ReferralGet_spParams
                                          {
                                              CaseID = UserManager.UserExtended.CaseID,
                                              UserID = UserManager.UserExtended.UserID,
                                              BatchLogJobID = Guid.NewGuid(),
                                              ReferralID = id
                                          }).FirstOrDefault();
            if (model.ReferralModel != null)
            {
                model.ReferralHeader = UtilityService.ExecStoredProcedureWithResults<ref_ReferralHeader_spResult>("ref_ReferralHeader_sp",
                                        new ref_ReferralHeader_spParams
                                        {
                                            CaseID = UserManager.UserExtended.CaseID,
                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid(),
                                            ReferralTypeCodeID = model.ReferralModel.ReferralTypeCodeID,
                                            ReferralID = model.ReferralModel.ReferralID,
                                            RoleID = model.ReferralModel.RoleID

                                        }).FirstOrDefault();
            }





            return PartialView("_partialReferralTabs", model);
        }
        public virtual ActionResult ReferralActivitySheet(string id)
        {

            ViewBag.ReferralID = id.ToDecrypt().ToInt();
            ViewBag.EncryptReferralID = id;
            var model = new ReferralActivitySheetViewModel();
            model.FilterByStaffType = Request.QueryString["filterByStaffType"];

            if (Request.QueryString["filterByStaffType"] == null)
            {
                var filterList = UtilityService.ExecStoredProcedureWithResults<LALoDep.Domain.pd_Work.pd_WorkGetFilterByStaffType_spResult>("pd_WorkGetFilterByStaffType_sp",
              new LALoDep.Domain.pd_Work.pd_WorkGetFilterByStaffType_spParams()
              {
                  CaseID = UserManager.UserExtended.CaseID,
                  BatchLogJobID = Guid.NewGuid(),
                  UserID = UserManager.UserExtended.UserID,
                  ReferralID = id.ToDecrypt().ToInt()
              }).ToList();
                model.WorkGetFilterByStaffTypeList = filterList.Select(o => new SelectListItem() { Text = o.FilterBy, Value = o.FilterBy }).ToList();
                if (filterList.Any(o => o.DefaultFlag.Value == 1))
                {
                    model.FilterByStaffType = filterList.FirstOrDefault(o => o.DefaultFlag.Value == 1).FilterBy;
                }

            }



            model.WorkList = UtilityService.ExecStoredProcedureWithResults<LALoDep.Domain.pd_Work.pd_WorkGetByCaseID_spResult>("pd_WorkGetByCaseID_sp",
                new LALoDep.Domain.pd_Work.pd_WorkGetByCaseID_spParams()
                {

                    CaseID = UserManager.UserExtended.CaseID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    ReferralID = id.ToDecrypt().ToInt(),
                    FilterByStaffType = model.FilterByStaffType
                }).OrderByDescending(o => o.SortDate).ToList();
            return View(model);

        }


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AttachFile)]
        public virtual ActionResult ReferralAttachedFiles(string id)
        {
            ViewBag.ReferralID = id.ToDecrypt().ToInt();

            var model = new AttachPathViewModel
            {
                CaseFileGetPathByCaseList =
                     UtilityService.ExecStoredProcedureWithResults<CaseFileGetPathByCaseID_spResult>(
                         "CaseFileGetPathByCaseID_sp",
                         new CaseFileGetPathByCaseID_spParams
                         {
                             BatchLogJobID = Guid.NewGuid(),
                             CaseID = UserManager.UserExtended.CaseID,
                             UserID = UserManager.UserExtended.UserID,


                         }).ToList(),
                RoleList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDChildRespondent_spResult>(
                     "pd_RoleGetByCaseIDChildRespondent_sp",
                     new pd_RoleGetByCaseIDChildRespondent_spParams
                     {
                         BatchLogJobID = Guid.NewGuid(),
                         CaseID = UserManager.UserExtended.CaseID,
                         UserID = UserManager.UserExtended.UserID,


                     }).Select(o => new SelectListItem() { Text = o.DisplayName, Value = o.RoleID.ToString() }),
                CategoryList =
                     UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(900, agencyId: UserManager.UserExtended.CaseNumberAgencyID)
                         .Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() })
                         .ToList(),

                FileDate = DateTime.Now.ToShortDateString()
            };
            model.UseGoogleDriveUpload = model.CaseFileGetPathByCaseList.Any(o => o.UseGoogleDocsFlag.HasValue && o.UseGoogleDocsFlag.Value == 1) ? 1 : 0;
            var caseFileGetPath = model.CaseFileGetPathByCaseList.FirstOrDefault();
            if (caseFileGetPath != null)
            {
                if (caseFileGetPath.UseGoogleDocsFlag.HasValue && caseFileGetPath.UseGoogleDocsFlag.Value > 0)
                {
                    var environment = System.Configuration.ConfigurationManager.AppSettings["GoogleRootFolder"];
                    var parentFolderId = "";
                    if (environment == "Test")
                    {
                        parentFolderId = caseFileGetPath.GoogleFolderID_TEST;
                    }
                    else if (environment == "Prod")
                    {
                        parentFolderId = caseFileGetPath.GoogleFolderID_PROD;

                    }
                    if (!parentFolderId.IsNullOrEmpty())
                        model.UseGoogleDriveUpload = caseFileGetPath.UseGoogleDocsFlag.Value;

                }

                if (caseFileGetPath.SharePoint_UseFlag.HasValue && caseFileGetPath.SharePoint_UseFlag.Value > 0)
                {
                    var environment = System.Configuration.ConfigurationManager.AppSettings["ServerEnvironment"];

                    if (environment == "Test" || environment == "Dev")
                    {
                        model.SharePoint_URL = caseFileGetPath.SharePoint_URL_TEST;
                    }
                    else if (environment == "Prod")
                    {
                        model.SharePoint_URL = caseFileGetPath.SharePoint_URL_PROD;

                    }

                    model.SharePoint_UseFlag = caseFileGetPath.SharePoint_UseFlag.Value;
                }
            }

            var settings = UtilityFunctions.JcatsNGConfigGetAll(UserManager.UserExtended.CaseID);

            if (settings.Any(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_FileTypes))
                ViewBag.AttachFileTypes = settings.FirstOrDefault(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_FileTypes).JcatsNGConfigValue;
            else
                ViewBag.AttachFileTypes = "gif|jpe?g|png|pdf|txt|docx|doc|xls|xlsx|csv|ppt|pptx";

            if (settings.Any(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_MaxSize))
                ViewBag.AttachFileMaxSize = settings.FirstOrDefault(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_MaxSize).JcatsNGConfigValue;
            else
                ViewBag.AttachFileMaxSize = "15000000";
            var oReferral = UtilityService.ExecStoredProcedureWithResults<ref_ReferralGet_spResult>("ref_ReferralGet_sp",
                                      new ref_ReferralGet_spParams
                                      {
                                          CaseID = UserManager.UserExtended.CaseID,
                                          UserID = UserManager.UserExtended.UserID,
                                          BatchLogJobID = Guid.NewGuid(),
                                          ReferralID = id.ToDecrypt().ToInt()
                                      }).FirstOrDefault();
            if (oReferral != null)
            {
                if (oReferral.RoleID.HasValue)
                    model.RoleID = oReferral.RoleID.Value;
            }
            return View(model);

        }

        [HttpPost]
        public virtual JsonResult ReferralDelete(string id)
        {
            var pd_NoteDelete_spParams = new ref_ReferralIUD_spParams()
            {
                IUD = "DELETE",
                ReferralID = id.ToDecrypt().ToInt(),
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID
            };
            UtilityService.ExecStoredProcedureWithoutResultADO("ref_ReferralIUD_sp", pd_NoteDelete_spParams);
            return Json(new { isSuccess = true });
        }

        public virtual ActionResult ReferralAddEditCodePopup(ReferralAddEditCodePopupViewModel model)
        {
            return View(model);
        }
        [HttpPost]
        public virtual JsonResult ReferralAddEditCodePopupSave(ReferralAddEditCodePopupViewModel model)
        {
            int CodeID = 0;
            string CodeValue = "";
            if (model.RecordType == "ImmigrationAgency")
            {

                var oData = UtilityService.ExecStoredProcedureWithResults<ref_ReferralAddNewCode_spResult>("ref_ReferralAddNewCode_sp",
                           new ref_ReferralAddNewCode_spParams()
                           {
                               CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                               CaseID = UserManager.UserExtended.CaseID,
                               CodeShortValue = model.ShortValue,
                               CodeValue = model.Value,
                               CodeType = "ImmigrationAgency",
                               UserID = UserManager.UserExtended.UserID
                           }).ToList();
                if (oData.Any())
                {
                    CodeID = oData.FirstOrDefault().CodeID.ToInt();
                    CodeValue = oData.FirstOrDefault().CodeDisplay;
                }
            }
            else if (model.RecordType == "ImmigrationAttorney")
            {
                var oData = UtilityService.ExecStoredProcedureWithResults<ref_ReferralAddNewStaff_spResult>("ref_ReferralAddNewStaff_sp",
                              new ref_ReferralAddNewStaff_spParams()
                              {
                                  CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                  CaseID = UserManager.UserExtended.CaseID,
                                  LastName = model.LastName,
                                  FirstName = model.FirstName,
                                  StaffType = "ImmigrationAttorney",
                                  UserID = UserManager.UserExtended.UserID
                              }).ToList();
                if (oData.Any())
                {
                    CodeID = oData.FirstOrDefault().PersonID.ToInt();
                    CodeValue = oData.FirstOrDefault().NameDisplay;
                }
            }
            return Json(new { isSuccess = true, CodeValue, CodeID });
        }
    }
}
