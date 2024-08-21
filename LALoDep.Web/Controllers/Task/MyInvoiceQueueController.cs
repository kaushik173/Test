using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using DataTables.Mvc;
using LALoDep.Models.Task;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Role;
using LALoDep.Models;
using LALoDep.Custom;
using LALoDep.Domain.CSEC;
using LALoDep.Domain.NgInvoice;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {


        public virtual ActionResult MyInvoiceQueue(string id, string yQId,string status)
        {

            var viewModel = new MyInvoiceQueueViewModel
            {
                NotInvoicedFlag = 1,
                EncryptedPersonID = id,
                PageTitle = "My Invoice Queue",
                PersonID = id.IsNullOrEmpty() ? UserManager.UserExtended.PersonID : id.ToDecrypt().ToInt(),

            };
            if (id.IsNullOrEmpty())
            {
                viewModel.ResetPageCache = true;
            }
            if (viewModel.PersonID > 0)
            {
                var personGet = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", new pd_PersonGet_spParams()
                {
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    PersonID = viewModel.PersonID.Value
                }).FirstOrDefault();
                if (personGet != null)
                {

                    viewModel.PageTitle = "My Invoice Queue <br/>For " + personGet.FirstName + " " + personGet.LastName;
                }

            }
            var yearList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetYearQuarter_spResult>(
                     "NgInvoice_GetYearQuarter_sp",
                     new NgInvoice_GetYearQuarter_spParams
                     {
                         CurrentDate = DateTime.Now,
                         PersonID = viewModel.PersonID,
                         LoadOption = "MyInvoiceQueue",
                         UserID = UserManager.UserExtended.UserID,


                     }).ToList();
            var oYear = yearList.FirstOrDefault(o => o.Selected == 1);
            if (oYear != null) { 
                viewModel.YearQuarterID = oYear.YearQuarterID.Value;
            }
            if (!yQId.IsNullOrEmpty())
            {
                viewModel.ResetPageCache = true;
                viewModel.YearQuarterID = yQId.ToDecrypt().ToInt();
                viewModel.NotInvoicedFlag = 0;
            }
            if (!status.IsNullOrEmpty())
            {
                viewModel.InvoiceStatusCodeID = status.ToDecrypt().ToInt();
            }
            
            viewModel.YearQuarterList = yearList.Select(o => new SelectListItem() { Text = o.YearQuaterDisplay, Value = o.YearQuarterID.ToString() });

            viewModel.InvoiceStatusList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetStatusCodes_spResult>(
                   "NgInvoice_GetStatusCodes_sp",
                   new NgInvoice_GetStatusCodes_spParams
                   {

                       PersonID = viewModel.PersonID,
                       LoadOption = "MyInvoiceQueue",
                       UserID = UserManager.UserExtended.UserID,
                       AgencyID = UserManager.UserExtended.AgencyID,


                   }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() });
            return View(viewModel);
        }


        [HttpPost]
        public virtual ActionResult MyInvoiceQueue(MyInvoiceQueueViewModel model)
        {


            var data = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetMyInvoiceQueue_spResult>("NgInvoice_GetMyInvoiceQueue_sp", new NgInvoice_GetMyInvoiceQueue_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                InvoiceNumber = model.InvoiceNumber,
                YearQuarterID = model.YearQuarterID,
                InvoiceStatusCodeID = model.InvoiceStatusCodeID,
                NotInvoicedFlag = model.NotInvoicedFlag,
                PersonID = model.PersonID,
                LoadOption = "AttorneyApprovalNeeded",
                DocNumber = model.SAPNumber,
            }).ToList();


            var dataMyInvoiceQueue = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetMyInvoiceQueue_spResult>("NgInvoice_GetMyInvoiceQueue_sp", new NgInvoice_GetMyInvoiceQueue_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                InvoiceNumber = model.InvoiceNumber,
                YearQuarterID = model.YearQuarterID,
                InvoiceStatusCodeID = model.InvoiceStatusCodeID,
                NotInvoicedFlag = model.NotInvoicedFlag,
                PersonID = model.PersonID,
                LoadOption = "MyInvoiceQueue",
                DocNumber = model.SAPNumber,
            }).ToList();

            foreach (var item in dataMyInvoiceQueue)
            {
                item.SectionHeader = item.SectionHeader + " (" + dataMyInvoiceQueue.Count + ")";
                item.EncryptedCaseID = item.CaseID.ToEncrypt();
                item.EncryptedInvoiceID = item.NgInvoiceID.ToEncrypt();
                item.EncryptedYearQuarterID = item.YearQuarterID.ToEncrypt();
                item.EncryptedContractorPersonID = item.ContractorPersonID.ToEncrypt();
            }

            foreach (var item in data)
            {
                item.SectionHeader = item.SectionHeader + " (" + data.Count + ")";

                item.EncryptedCaseID = item.CaseID.ToEncrypt();
                item.EncryptedInvoiceID = item.NgInvoiceID.ToEncrypt();
                item.EncryptedYearQuarterID = item.YearQuarterID.ToEncrypt();
                item.EncryptedContractorPersonID = item.ContractorPersonID.ToEncrypt();

            }
            return Json(new
            {
                Status = "Done",
                SearchData = new
                {
                    EncryptedInvStatusCodeID = model.InvoiceStatusCodeID.ToEncrypt(),
                    SearchAttorneyApprovalNeededData = new DataTablesResponse(0, data, data.Count, data.Count),
                    SearchMyInvoiceQueueData = new DataTablesResponse(0, dataMyInvoiceQueue, dataMyInvoiceQueue.Count, dataMyInvoiceQueue.Count)
                }
            });

        }


    }
}