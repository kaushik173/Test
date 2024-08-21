using System;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Models.Task;
using LALoDep.Custom;
using LALoDep.Domain.ref_Referral;
using LALoDep.Core.Custom.Extensions;
using DataTables.Mvc;
using LALoDep.Domain.pd_Person;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.IVEActvityLog;
using LALoDep.Domain.IVeActivity;
using Newtonsoft.Json;
using System.Collections.Generic;
using LALoDep.Domain.TitleIVe;
using DocumentFormat.OpenXml.Bibliography;
using LALoDep.Domain.pd_CodeTables;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {



        #region IVEPersonnel

        public virtual ActionResult IVEPersonnel(string id)
        {
            var viewModel = new IVEPersonnelViewModel()
            {
                InvoiceID = id.ToDecrypt().ToInt(),
            };


            var headerData = UtilityService.ExecStoredProcedureWithResults<TitleIVeExpenseHeader_spResult>("TitleIVeExpenseHeader_sp", new TitleIVeExpenseHeader_spParams
            {
                InvoiceID = viewModel.InvoiceID,
                UserID = UserManager.UserExtended.UserID
            }).FirstOrDefault();
            
            viewModel.CourtSystem = headerData?.CourtSystem;
            viewModel.InvoicePeriod = headerData?.InvoicePeriod;
            viewModel.InvoiceDate = headerData?.InvoiceDate;
            viewModel.HeaderInvoiceID = headerData?.InvoiceID;
            var jsonData = UtilityService.ExecStoredProcedureWithJsonResult("TitleIVePersonnelGet_sp", new TitleIVePersonnelGet_spParams
            {
                InvoiceID = viewModel.InvoiceID,
                UserID = UserManager.UserExtended.UserID
            });
            if (jsonData != "")
                viewModel.TitleIVePersonnelList = JsonConvert.DeserializeObject<List<TitleIVePersonnelGetListViewModel>>(jsonData);

            if (viewModel.TitleIVePersonnelList.Any())
            {
                viewModel.ProviderOverheadRate = viewModel.TitleIVePersonnelList.FirstOrDefault().OverHeadRate;
            }
            viewModel.TitleIVePersonnelList.Add(new TitleIVePersonnelGetListViewModel()
            {
                TitleIVePersonnelID = 0,
                TitleIVeInvoiceID = viewModel.InvoiceID,
                RecordStateID = 1,
                EmployeePersonID = 0

            });
            viewModel.TitleIVePersonnelList.Add(new TitleIVePersonnelGetListViewModel()
            {
                TitleIVePersonnelID = 0,
                TitleIVeInvoiceID = viewModel.InvoiceID,
                RecordStateID = 1,
                EmployeePersonID = 0

            });
            viewModel.TitleIVePersonnelList.Add(new TitleIVePersonnelGetListViewModel()
            {
                TitleIVePersonnelID = 0,
                TitleIVeInvoiceID = viewModel.InvoiceID,
                RecordStateID = 1,
                EmployeePersonID = 0

            });


            var messageToDisplay = UtilityService.ExecStoredProcedureScalar("TitleIVePersonnelGet_CheckPTO_sp", new TitleIVePersonnelGet_spParams
            {
                InvoiceID = viewModel.InvoiceID,
                UserID = UserManager.UserExtended.UserID
            });
            if (messageToDisplay != null)
            {
                viewModel.MessageToDisplay = messageToDisplay.ToString();
            }
            //viewModel.OHCodeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndNotUserID_spResult>("TitleIVeOverheadCodeDropDown_sp", new TitleIVeOverheadCodeDropDown_spParams
            //{
            //    PersonID =  UserManager.UserExtended.PersonID,
            //    UserID = UserManager.UserExtended.UserID
            //}).Select(o => new SelectListItem() { Text = o.CodeShortValue, Value = o.CodeID.ToString() }).ToList();
            foreach (var item in viewModel.TitleIVePersonnelList)
            {

                item.OHCodeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndNotUserID_spResult>("TitleIVeOverheadCodeDropDown_sp", new TitleIVeOverheadCodeDropDown_spParams
                {
                    PersonID = item.EmployeePersonID.ToInt(),
                    UserID = UserManager.UserExtended.UserID
                }).Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() }).ToList();

            }
            return View(viewModel);
        }
        [HttpPost]
        public virtual ActionResult IVEPersonnel(List<IVEPersonnelAddEditModel> viewModel)
        {
            if (viewModel != null)
            {
                var jsonData = JsonConvert.SerializeObject(viewModel);
                UtilityService.ExecStoredProcedureWithoutResults("TitleIVePersonnelInsertUpdate_sp", new TitleIVePersonnelInsertUpdate_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    InputData = jsonData
                });
                jsonData = UtilityService.ExecStoredProcedureWithJsonResult("TitleIVePersonnelGet_sp", new TitleIVePersonnelGet_spParams
                {
                    InvoiceID = viewModel[0].TitleIVeInvoiceID,
                    UserID = UserManager.UserExtended.UserID
                });
                if (jsonData != "")
                {
                    var data = JsonConvert.DeserializeObject<List<TitleIVePersonnelGet_spResult>>(jsonData).OrderByDescending(o => o.TitleIVePersonnelID).Select(o => o.TitleIVePersonnelID).FirstOrDefault();
                    if (data.HasValue)
                    {
                        return Json(new { isSuccess = true, LastTitleIVePersonnelID = data.Value.ToEncrypt() });
                    }
                };
                return Json(new { isSuccess = true, LastTitleIVePersonnelID = "" });
            }
            return Json(new { isSuccess = false, message = "There is some issue while saving data" });
        }
        #endregion
        #region IVEClaimForm

        public virtual ActionResult IVEClaimForm(string id)
        {
            var viewModel = new IVEClaimFormViewModel()
            {
                InvoiceID = id.ToDecrypt().ToInt(),
            };


            var headerData = UtilityService.ExecStoredProcedureWithResults<TitleIVeExpenseHeader_spResult>("TitleIVeExpenseHeader_sp", new TitleIVeExpenseHeader_spParams
            {
                InvoiceID = viewModel.InvoiceID,
                UserID = UserManager.UserExtended.UserID
            }).FirstOrDefault();

            viewModel.CourtSystem = headerData?.CourtSystem;
            viewModel.InvoicePeriod = headerData?.InvoicePeriod;
            viewModel.InvoiceDate = headerData?.InvoiceDate;
            viewModel.HeaderInvoiceID = headerData?.InvoiceID;
            viewModel.TitleIVeClaimForm = UtilityService.ExecStoredProcedureWithResults<TitleIVeClaimFormGet_spResult>("TitleIVeClaimFormGet_sp", new TitleIVeClaimFormGet_spParams
            {
                InvoiceID = viewModel.InvoiceID,
                UserID = UserManager.UserExtended.UserID
            }).FirstOrDefault();

            viewModel.HeaderInvoiceID = viewModel.TitleIVeClaimForm.DisplayTitleIVeInvoiceID;
            viewModel.StandardAgreementNumber = viewModel.TitleIVeClaimForm.StandardAgreementNumber;
            return View(viewModel);
        }
        [HttpPost]
        public virtual ActionResult IVEClaimForm(IVEClaimFormViewModel viewModel)
        {
            if (viewModel != null)
            {

                UtilityService.ExecStoredProcedureWithoutResults("TitleIVeClaimFormUpdate_sp", new TitleIVeClaimFormUpdate_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    TitleIVeInvoiceID = viewModel.InvoiceID,
                    AdjustmentAmount = viewModel.TitleIVeClaimForm.AdjustmentAmount,
                    Note = viewModel.TitleIVeClaimForm.Note,
                    PreparedByEmail = viewModel.TitleIVeClaimForm.PreparedByEmail,
                    PreparedByName = viewModel.TitleIVeClaimForm.PreparedByName,
                    PreparedByPhone = viewModel.TitleIVeClaimForm.PreparedByPhone,
                    PreparedByTitle = viewModel.TitleIVeClaimForm.PreparedByTitle,
                    NbrChildrenCasesWorked = viewModel.TitleIVeClaimForm.NbrChildrenCasesWorked,
                    PercentageDependency = viewModel.TitleIVeClaimForm.PercentageDependency,
                    TrainingEligibleAmount = viewModel.TitleIVeClaimForm.TrainingEligibleAmount,
                    TrainingEligibleAmtCA = viewModel.TitleIVeClaimForm.TrainingEligibleAmtCA,
                    TrainingEligibleRateCA = viewModel.TitleIVeClaimForm.TrainingEligibleRateCA,
                    TrainingReimbursementAmt = viewModel.TitleIVeClaimForm.TrainingReimbursementAmt,
                    TrainingReimbursementRateFed = viewModel.TitleIVeClaimForm.TrainingReimbursementRateFed
                });
                return Json(new { isSuccess = true });
            }
            return Json(new { isSuccess = false, message = "There is some issue while saving data" });
        }
        #endregion

    }
}