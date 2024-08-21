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
using LALoDep.Models.Case;
using Omu.ValueInjecter;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        #region Invoices List


        public virtual ActionResult Invoices(string filterBy, string caseId)
        {
            if (caseId.ToDecrypt().ToInt() > 0)
            {

                UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());
            }

            var viewModel = new InvoicesViewModel();

            var filters = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetForCaseFilter_spResult>(
                     "NgInvoice_GetForCaseFilter_sp",
                     new NgInvoice_GetForCaseFilter_spParams
                     {
                         CaseID = UserManager.UserExtended.CaseID,
                         UserID = UserManager.UserExtended.UserID

                     }).ToList();

            if (!filterBy.IsNullOrEmpty())
                viewModel.FilterByEnum = filterBy;
            else
            {
                var selectedFilter = filters.FirstOrDefault(o => o.Selected == 1);
                if (selectedFilter != null)
                    viewModel.FilterByEnum = selectedFilter.FilterByEnumName;
            }

            viewModel.FilterByList = filters.Select(o => new SelectListItem() { Text = o.FilterByDisplay, Value = o.FilterByEnumName });

            viewModel.InvoiceList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetForCase_spResult>(
                   "NgInvoice_GetForCase_sp",
                   new NgInvoice_GetForCase_spParams
                   {

                       CaseID = UserManager.UserExtended.CaseID,
                       LoadOption = viewModel.FilterByEnum,
                       UserID = UserManager.UserExtended.UserID,



                   }).ToList();

            return View(viewModel);
        }

        public virtual ActionResult InvoicePrint(string id)
        {

            var com_ReportParameterValueDelete_spParams = new com_ReportParameterValueDelete_spParams()
            {
                ReportID = 141,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            List<object> valueDelete = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueDelete_sp", com_ReportParameterValueDelete_spParams).ToList();
            List<object> headerDelete = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderDelete_sp", com_ReportParameterValueDelete_spParams).ToList();

            if (!string.IsNullOrEmpty(id))
            {
                var com_ReportParameterValueInsert_spParams = new com_ReportParameterValueInsert_spParams()
                {
                    ReportID = 141,
                    Sequence = 1,
                    Value = id.ToDecrypt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueInsert_sp", com_ReportParameterValueInsert_spParams).ToList();
            }


            var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 141,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };
            ReportClass rpt = new ReportClass();

            string fileName = "Invoice_" + id + ".pdf";
            try
            {

                var reportSource = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", com_ReportSourceGetByReportID_spParams).ToList();

                rpt.FileName = HttpContext.Server.MapPath("~/Reports/" + reportSource[0].ReportSourceDocumentName);
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTableADO(reportSource[0].ReportSourceStoredProcedureName.Replace("dbo.", ""), com_ReportSourceGetByReportID_spParams, 60);
                rpt.SetDataSource(table);
                foreach (var subRptDt in reportSource.Where(c => (c.ReportSourceDocumentName.CompareTo(reportSource[0].ReportSourceDocumentName) != 0)))
                {
                    var subTableData = UtilityService.ExecStoredProcedureForDataTableADO(subRptDt.ReportSourceStoredProcedureName.Replace("dbo.", ""), com_ReportSourceGetByReportID_spParams, 60);
                    rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(subTableData);
                }
                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + fileName);

            }
            catch (Exception ex)
            {



            }
            finally
            {
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
            }

            return new LALoDep.Custom.Actions.DownloadActionResult(fileName);
        }

        #endregion
        #region Invoices Add/Edit

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseInvoiceAddEdit)]
        public virtual ActionResult InvoiceAddEdit(string id, string yQId, string cPId, string caseId, string personId,string status)
        {
          
            var caseUpdated = true;
            if (caseId.ToDecrypt().ToInt() > 0)
            {
                caseUpdated = UserManager.UpdateCaseStatusBar(caseId.ToDecrypt().ToInt());

            }
            if (!caseUpdated)
            {
                return View();
            }
            var viewModel = new InvoiceAddEditViewModel
            {
                NgInvoiceID = id.ToDecrypt().ToInt(),
                YearQuarterID = yQId.ToDecrypt().ToInt(),
                ContractorPersonID = cPId.ToDecrypt().ToInt()
            };





            var invoiceGet = UtilityService.ExecStoredProcedureWithResults<NgInvoice_Get_spResult>(
                       "NgInvoice_Get_sp",
                       new NgInvoice_Get_spParams
                       {
                           CaseID = UserManager.UserExtended.CaseID,
                           UserID = UserManager.UserExtended.UserID,
                           CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                           ContractorPersonID = viewModel.ContractorPersonID,
                           NgInvoiceID = viewModel.NgInvoiceID,
                           YearQuarterID = viewModel.YearQuarterID

                       }).FirstOrDefault();
            if (invoiceGet != null)
                viewModel.InjectFrom(invoiceGet);

            viewModel.InvoiceGetTotals = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetTotals_spResult>(
                    "NgInvoice_GetTotals_sp",
                    new NgInvoice_GetTotals_spParams
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        ContractorPersonID = viewModel.ContractorPersonID,
                        NgInvoiceID = viewModel.NgInvoiceID,
                        YearQuarterID = viewModel.YearQuarterID

                    }).ToList();
            viewModel.ChildNMDCurrentPlacmentList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetMinors_spResult>(
                     "NgInvoice_GetMinors_sp",
                     new NgInvoice_GetMinors_spParams
                     {
                         CaseID = UserManager.UserExtended.CaseID,
                         UserID = UserManager.UserExtended.UserID,
                         CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                         ContractorPersonID = viewModel.ContractorPersonID,
                         NgInvoiceID = viewModel.NgInvoiceID,
                         YearQuarterID = viewModel.YearQuarterID

                     }).ToList();
            viewModel.ExpenseDetailsList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetExpenses_spResult>(
                  "NgInvoice_GetExpenses_sp",
                  new NgInvoice_GetExpenses_spParams
                  {
                      CaseID = UserManager.UserExtended.CaseID,
                      UserID = UserManager.UserExtended.UserID,
                      CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                      ContractorPersonID = viewModel.ContractorPersonID,
                      NgInvoiceID = viewModel.NgInvoiceID,
                      YearQuarterID = viewModel.YearQuarterID

                  }).ToList();
            viewModel.RecordTimeDetailsList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetRecordTime_spResult>(
                  "NgInvoice_GetRecordTime_sp",
                  new NgInvoice_GetRecordTime_spParams
                  {
                      CaseID = UserManager.UserExtended.CaseID,
                      UserID = UserManager.UserExtended.UserID,
                      CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                      ContractorPersonID = viewModel.ContractorPersonID,
                      NgInvoiceID = viewModel.NgInvoiceID,
                      YearQuarterID = viewModel.YearQuarterID

                  }).ToList();

            viewModel.CounselHistoryList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetCounselHistory_spResult>(
      "NgInvoice_GetCounselHistory_sp",
      new NgInvoice_GetCounselHistory_spParams
      {
          CaseID = UserManager.UserExtended.CaseID,
          UserID = UserManager.UserExtended.UserID,
          CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
          ContractorPersonID = viewModel.ContractorPersonID,
          NgInvoiceID = viewModel.NgInvoiceID,
          YearQuarterID = viewModel.YearQuarterID

      }).ToList();

            viewModel.ChildTypeList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetCodes_spResult>(
                  "NgInvoice_GetCodes_sp",
                  new NgInvoice_GetCodes_spParams
                  {
                      CaseID = UserManager.UserExtended.CaseID,
                      UserID = UserManager.UserExtended.UserID,
                      AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                      NgInvoiceID = viewModel.NgInvoiceID,

                      CodeTypeCodeID = 1710
                  }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() });
            viewModel.StateList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetCodes_spResult>(
                 "NgInvoice_GetCodes_sp",
                 new NgInvoice_GetCodes_spParams
                 {
                     CaseID = UserManager.UserExtended.CaseID,
                     UserID = UserManager.UserExtended.UserID,
                     AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                     NgInvoiceID = viewModel.NgInvoiceID,

                     CodeTypeCodeID = 1705
                 }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() });

            viewModel.ExpenseStatusList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetCodes_spResult>(
                             "NgInvoice_GetCodes_sp",
                             new NgInvoice_GetCodes_spParams
                             {
                                 CaseID = UserManager.UserExtended.CaseID,
                                 UserID = UserManager.UserExtended.UserID,
                                 AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                 NgInvoiceID = viewModel.NgInvoiceID,

                                 CodeTypeCodeID = 1605
                             }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() });

            viewModel.CACountyList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetCounty_spResult>(
                         "NgInvoice_GetCounty_sp",
                         new NgInvoice_GetCounty_spParams
                         {
                             CaseID = UserManager.UserExtended.CaseID,
                             UserID = UserManager.UserExtended.UserID,

                             NgInvoiceID = viewModel.NgInvoiceID,


                         }).Select(o => new SelectListItem() { Text = o.AgencyCounty, Value = o.AgencyCountyID.ToString() });
            viewModel.InvoiceStatusList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetStatus_spResult>(
                     "NgInvoice_GetStatus_sp",
                     new NgInvoice_GetStatus_spParams
                     {
                         CaseID = UserManager.UserExtended.CaseID,
                         UserID = UserManager.UserExtended.UserID,

                         NgInvoiceID = viewModel.NgInvoiceID,


                     }).Select(o => new SelectListItem() { Text = o.StatusDisplay, Value = o.StatusCodeID.ToString() });


            viewModel.PartyTypeList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetCounselTypes_spResult>(
               "NgInvoice_GetCounselTypes_sp",
               new NgInvoice_GetCounselTypes_spParams
               {
                   CaseID = UserManager.UserExtended.CaseID,
                   UserID = UserManager.UserExtended.UserID,
                   CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                   ContractorPersonID = viewModel.ContractorPersonID,
                   NgInvoiceID = viewModel.NgInvoiceID,
                   YearQuarterID = viewModel.YearQuarterID
               }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() });

            viewModel.CounselList = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetAttorneys_spResult>(
                         "NgInvoice_GetAttorneys_sp",
                         new NgInvoice_GetAttorneys_spParams
                         {
                             CaseID = UserManager.UserExtended.CaseID,
                             UserID = UserManager.UserExtended.UserID,
                             CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                             ContractorPersonID = viewModel.ContractorPersonID,
                             NgInvoiceID = viewModel.NgInvoiceID,
                             YearQuarterID = viewModel.YearQuarterID
                         }).Select(o => new SelectListItem() { Text = o.PersonDisplay, Value = o.PersonID.ToString() });


            if (!personId.IsNullOrEmpty())
            {
                var personGet = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", new pd_PersonGet_spParams()
                {
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    PersonID = personId.ToDecrypt().ToInt()
                }).FirstOrDefault();
                if (personGet != null)
                {

                    viewModel.MyInvoiceQueuePersonName = personGet.FirstName + " " + personGet.LastName;
                }
                if (!status.IsNullOrEmpty())
                   viewModel.ReturnPageUrl = "/Task/MyInvoiceQueue/" + personId + "?yQId=" + viewModel.YearQuarterID.ToEncrypt() + "&status=" + status;
                else
                   viewModel.ReturnPageUrl = "/Task/MyInvoiceQueue/" + personId + "?yQId=" + viewModel.YearQuarterID.ToEncrypt();
            }

            return View(viewModel);
        }

        [HttpPost]

        public virtual JsonResult InvoiceAddEdit(InvoiceAddEditViewModel model)
        { 
            model.CaseID = UserManager.UserExtended.CaseID;
            var oData = UtilityService.ExecStoredProcedureForDataTableADO("NgInvoiceIUD_sp", new NgInvoiceIUD_spParams()
            {
                IUD = model.NgInvoiceID > 0 ? "UPDATE" : "INSERT",
                NgInvoiceID = model.NgInvoiceID.ToInt(),
                AgencyID = model.AgencyID.ToInt(),
                CaseID = model.CaseID,
                ContractorPersonID = model.ContractorPersonID,
                NgInvoiceNote = model.InvoiceNote,
                NgInvoiceStatusCodeID = model.InvoiceStatusCodeID,
                NgInvoiceStatusID = model.InvoiceStatusCodeID,
                SaveAction = model.ButtonID == "2" ? "SignatureAndSubmit" : model.SaveAction,
                YearQuarterID = model.YearQuarterID,
                UserID = UserManager.UserExtended.UserID,
                NGInvoiceAdminNote = model.InvoiceAdminNote

            });
            if (oData.Rows.Count > 0)
            {
                model.NgInvoiceID = oData.Rows[0]["NgInvoiceID"].ToInt();
                if (model.NgInvoiceCounselIUDList != null && model.NgInvoiceCounselIUDList.Any())
                {
                    foreach (var item in model.NgInvoiceCounselIUDList)
                    {
                        item.NgInvoiceID = model.NgInvoiceID;
                        item.UserID = UserManager.UserExtended.UserID;
                        UtilityService.ExecStoredProcedureForDataTableADO("NgInvoiceCounselIUD_sp", item);
                    }

                }
                if (model.ChildSaveParamList != null && model.ChildSaveParamList.Any())
                {
                    foreach (var item in model.ChildSaveParamList)
                    {
                        item.NgInvoiceID = model.NgInvoiceID;
                        item.UserID = UserManager.UserExtended.UserID;
                        UtilityService.ExecStoredProcedureForDataTableADO("NgInvoiceMinorIUD_sp", item);
                    }

                }
                if (model.ExpenseSaveParamList != null && model.ExpenseSaveParamList.Any())
                {
                    foreach (var item in model.ExpenseSaveParamList)
                    {
                        item.NgInvoiceID = model.NgInvoiceID;
                        item.UserID = UserManager.UserExtended.UserID;
                        UtilityService.ExecStoredProcedureForDataTableADO("NgInvoiceDetail_Expense_IUD_sp", item);
                    }

                }
                if (model.RecordTimeSaveParamList != null && model.RecordTimeSaveParamList.Any())
                {
                    foreach (var item in model.RecordTimeSaveParamList)
                    {
                        item.NgInvoiceID = model.NgInvoiceID;
                        item.UserID = UserManager.UserExtended.UserID;
                        UtilityService.ExecStoredProcedureForDataTableADO("NgInvoiceDetail_RecordTime_IUD_sp", item);
                    }

                }

                UtilityService.ExecStoredProcedureForDataTableADO("NgInvoice_CalcSummary_sp", new NgInvoice_CalcSummary_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    NgInvoiceID = model.NgInvoiceID
                });


            }


            return Json(new { Status = "Done" });
        }
        public virtual ActionResult NGInvoiceStatusHistory(int id)
        {


            var data = UtilityService.ExecStoredProcedureWithResults<NgInvoice_GetStatusHistory_spResult>(
                         "NgInvoice_GetStatusHistory_sp",
                         new NgInvoice_GetStatusHistory_spParams()
                         {
                             NgInvoiceID = id,
                             UserID = UserManager.UserExtended.UserID,

                         }).ToList();

            return View(data);
        }
        #endregion

    }
}