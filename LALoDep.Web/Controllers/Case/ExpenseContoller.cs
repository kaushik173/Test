using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using DataTables.Mvc;
using LALoDep.Core.Enums;
using LALoDep.Models;
using LALoDep.Custom;
using LALoDep.Domain.HourlyExpense;
using LALoDep.Domain.pd_Hearing;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ExpensePage)]
        public virtual ActionResult Expense(string id)
        {
            if (UserManager.UserExtended.CaseID > 0)
            {
                var viewModel = new ExpenseViewModel
                {
                    CanAddAccess = UserManager.IsUserAccessTo(SecurityToken.ExpenseAdd),
                    CanEditAccess = UserManager.IsUserAccessTo(SecurityToken.ExpenseEdit),
                    CanDeleteAccess = UserManager.IsUserAccessTo(SecurityToken.ExpenseDelete)
                };
                var pd_HearingExpenseRequestBy_spParams = new pd_HearingExpenseRequestBy_spParams()
                {
                    CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };

                viewModel.Attorney = UtilityService.ExecStoredProcedureWithResults<pd_HearingExpenseRequestBy_spResult>("pd_HearingExpenseRequestBy_sp", pd_HearingExpenseRequestBy_spParams)
                                                 .Select(x => new PersonViewModel()
                                                 {
                                                     PersonID=x.PersonID,
                                                     PersonNameDisplay=x.PersonNameLast+", "+x.PersonNameFirst
                                                 }).ToList();

               

                if (!string.IsNullOrEmpty(id))
                {
                    //Edit mode
                    var expenseInfo = GetHourlyExpenseInfo(id);
                    if (expenseInfo != null)
                    {
                        viewModel.HourlyExpenseID = expenseInfo.HourlyExpenseID;
                        viewModel.AgencyID = expenseInfo.AgencyID;
                        viewModel.CaseID = expenseInfo.CaseID;
                        viewModel.PersonID = expenseInfo.PersonID;
                        viewModel.HourlyExpenseDate = expenseInfo.HourlyExpenseDate;
                        viewModel.HourlyExpenseTypeCodeID = expenseInfo.HourlyExpenseTypeCodeID;
                        viewModel.HourlyExpenseAmount = expenseInfo.HourlyExpenseAmount;
                        viewModel.HourlyExpenseProviderCodeID = expenseInfo.HourlyExpenseProviderCodeID;
                        viewModel.HourlyExpenseDescription = expenseInfo.HourlyExpenseDescription;
                    }
                }

                viewModel.ExpenseType = UtilityFunctions.CodeGetBySystemValueTypeId(221, viewModel.HourlyExpenseTypeCodeID ?? 0, UserManager.UserExtended.CaseNumberAgencyID);
                viewModel.Provider = UtilityFunctions.CodeGetByTypeIdAndUserId(39, null, viewModel.HourlyExpenseProviderCodeID ??0, UserManager.UserExtended.CaseNumberAgencyID);
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ExpensePage, PageSecurityItemID = SecurityToken.ExpenseView)]
        [HttpPost]
        public virtual JsonResult GetExpenseByCaseList()
        {
            var pd_HourlyExpenseGetByCaseID_spParams = new pd_HourlyExpenseGetByCaseID_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var expenseList = UtilityService.ExecStoredProcedureWithResults<pd_HourlyExpenseGetByCaseID_spResult>("pd_HourlyExpenseGetByCaseID_sp", pd_HourlyExpenseGetByCaseID_spParams).ToList();

            var list = expenseList.Select(x => new
                                 {
                                     EncryptedHourlyExpenseID=x.HourlyExpenseID.ToEncrypt(),
                                     HourlyExpenseID=x.HourlyExpenseID,
                                     HourlyExpenseDate = x.HourlyExpenseDate,
                                     HourlyExpenseType = x.HourlyExpenseType,
                                     Attoreny = x.AttorneyFirstName + " " + x.AttorneyLastName,
                                     HourlyExpenseAmount = x.HourlyExpenseAmount + " $",
                                     HourlyExpenseProvider = x.HourlyExpenseProvider,
                                     HourlyExpenseDescription = x.HourlyExpenseDescription
                                 }).ToList();
            var total = list.Count;
            return Json(new DataTablesResponse(0, list, total, total));

        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ExpensePage)]
        [HttpPost]
        public virtual JsonResult ExpenseSave(ExpenseViewModel viewModel)
        {
            if (viewModel.HourlyExpenseID.HasValue && UserManager.IsUserAccessTo(SecurityToken.ExpenseEdit))
            {
                //edit
                var pd_HourlyExpenseUpdate_spParams = new pd_HourlyExpenseUpdate_spParams()
                {
                    HourlyExpenseID = viewModel.HourlyExpenseID.Value,
                    AgencyID = viewModel.AgencyID,
                    CaseID = viewModel.CaseID,
                    PersonID = viewModel.PersonID,
                    HourlyExpenseDate = viewModel.HourlyExpenseDate.ToDateTime(),
                    HourlyExpenseTypeCodeID = viewModel.HourlyExpenseTypeCodeID,
                    HourlyExpenseAmount = viewModel.HourlyExpenseAmount,
                    HourlyExpenseProviderCodeID = viewModel.HourlyExpenseProviderCodeID,
                    HourlyExpenseDescription=viewModel.HourlyExpenseDescription,
                    RecordStateID = viewModel.RecordStateID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_HourlyExpenseUpdate_sp", pd_HourlyExpenseUpdate_spParams).FirstOrDefault();
            }
            else if (!viewModel.HourlyExpenseID.HasValue && UserManager.IsUserAccessTo(SecurityToken.ExpenseAdd))
            {
                //add
                var pd_HourlyExpenseInsert_spParams = new pd_HourlyExpenseInsert_spParams()
                {
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    CaseID = UserManager.UserExtended.CaseID,
                    PersonID = viewModel.PersonID,
                    HourlyExpenseDate = viewModel.HourlyExpenseDate.ToDateTime(),
                    HourlyExpenseTypeCodeID = viewModel.HourlyExpenseTypeCodeID,
                    HourlyExpenseAmount = viewModel.HourlyExpenseAmount,
                    HourlyExpenseProviderCodeID = viewModel.HourlyExpenseProviderCodeID,
                    HourlyExpenseDescription = viewModel.HourlyExpenseDescription,
                    RecordStateID = viewModel.RecordStateID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                };
                var inserteId = UtilityService.ExecStoredProcedureWithResults<int>("pd_HourlyExpenseInsert_sp", pd_HourlyExpenseInsert_spParams).FirstOrDefault();
            }
            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ExpensePage, PageSecurityItemID = SecurityToken.ExpenseDelete)]
        [HttpPost]
        public virtual JsonResult ExpenseDelete(string id)
        {
            var pd_HourlyExpenseDelete_spParams = new pd_HourlyExpenseDelete_spParams()
            {
                ID = id.ToDecrypt().ToInt(),
                RecordStateID = 10,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "HourlyExpense",
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_HourlyExpenseDelete_sp", pd_HourlyExpenseDelete_spParams).ToList();
            return Json(new { isSuccess = true });
        }

        private pd_HourlyExpenseGet_spResult GetHourlyExpenseInfo(string id)
        {
            var expenseInfo = UtilityService.ExecStoredProcedureWithResults<pd_HourlyExpenseGet_spResult>(
                              "pd_HourlyExpenseGet_sp", new pd_HourlyExpenseGet_spParams()
                              {
                                  HourlyExpenseID = id.ToDecrypt().ToInt(),
                                  UserID = UserManager.UserExtended.UserID,
                                  BatchLogJobID = Guid.NewGuid()
                              }).FirstOrDefault();
            return expenseInfo;
        }
    }
}