using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pD_SCCInvoice;
using LALoDep.Domain.Services;
using LALoDep.Custom;
using LALoDep.Models;
using LALoDep.Core.Enums;
using Omu.ValueInjecter;
using System.IO;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Models.Task;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class SCCInvoiceQueueController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public SCCInvoiceQueueController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
        // GET: SCCInvoiceQueue

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.SCCInvoiceQueuePage, PageSecurityItemID = SecurityToken.SCCInvoiceQueue)]
        public virtual ActionResult Search(bool load = false)
        {
            var viewModel = new SCCInvoiceQueueSearchViewModel();
            viewModel.CanEditAccess = UserManager.IsUserAccessTo(SecurityToken.EditSCCInvoiceQueue);
            viewModel.AttorneyList =
                UtilityService.ExecStoredProcedureWithResults<pd_SCCInvoiceGetAttorneyList_spResult>(
                    "pd_SCCInvoiceGetAttorneyList_sp",
                    new pd_SCCInvoiceGetAttorneyList_spParams() { BatchLogJobID = new Guid(), UserID = UserManager.UserExtended.UserID }).Select(e => new SelectListItem()
                    {
                        Value = e.PersonID.ToString() + ',' + e.AgencyCountyID.ToString(),
                        Text = e.PersonNameDisplay
                    }).ToList();

            viewModel.SourceList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>(
        "pd_CodeGetByTypeIDAndUserID_sp",
        new pd_CodeGetByTypeIDAndUserID_spParams() { UserID = UserManager.UserExtended.UserID, CodeTypeID = CodeType.Sources.GetHashCode() }).Select(e => new SelectListItem()
        {
            Value = e.CodeID.ToString(),
            Text = e.CodeShortValue
        }).ToList();
            viewModel.InvoiceStatusList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>(
        "pd_CodeGetByTypeIDAndUserID_sp",
        new pd_CodeGetByTypeIDAndUserID_spParams() { UserID = UserManager.UserExtended.UserID, CodeTypeID = CodeType.InvoiceStatus.GetHashCode() }).Select(e => new SelectListItem()
        {
            Value = e.CodeID.ToString(),
            Text = e.CodeShortValue
        }).ToList();

            ViewBag.LoadFromCache = load;
            ViewBag.JCATSNumber = UserManager.UserExtended.CaseJcatsNumber;
            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult Search(SCCInvoiceQueueSearchViewModel searchParams)
        {
            var list = new List<pd_SCCInvoiceSearch_spResult>();
            string[] Idlist = null;
            if (!string.IsNullOrEmpty(searchParams.AttorneyId))
                Idlist = searchParams.AttorneyId.Split(',');


            var pd_SCCInvoiceSearch_spParams = new pd_SCCInvoiceSearch_spParams
            {
                PersonID = (!string.IsNullOrEmpty(searchParams.AttorneyId)) ? Convert.ToInt32(Idlist[0].ToString()) : 0,
                AgencyCountyID = (!string.IsNullOrEmpty(searchParams.AttorneyId)) ? Convert.ToInt32(Idlist[1].ToString()) : 2,
                SCCInvoiceID = searchParams.InvoiceNumber,
                SCCInvoiceStatusCodeID = searchParams.InvoiceStatusId,
                ClientFirstName = string.IsNullOrEmpty(searchParams.ClientFirstName) ? searchParams.ClientFirstName : searchParams.ClientFirstName.Trim(),
                ClientLastName = string.IsNullOrEmpty(searchParams.ClientLastName) ? searchParams.ClientLastName : searchParams.ClientLastName.Trim(),
                CaseID = searchParams.CaseID,
                CourtNumber = searchParams.CourtNumber,
                SCCInvoicePaidDateStart = searchParams.SCCInvoicePaidDateStart,
                SCCInvoicePaidDateEnd = searchParams.SCCInvoicePaidDateEnd,
                ReferralSourceCodeID = searchParams.SourceId,
                SortOption = null,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = new Guid()
            };

            list = UtilityService.ExecStoredProcedureWithResults<pd_SCCInvoiceSearch_spResult>("pd_SCCInvoiceSearch_sp", pd_SCCInvoiceSearch_spParams).ToList();

            var total = list.Count;

            var currentAttorney = 0;
            foreach (var item in list)
            {
                if (currentAttorney == item.AttorneyPersonID)
                    item.Attorney = string.Empty;
                else
                    currentAttorney = item.AttorneyPersonID;
            }

            var searchList = list.Select(c => new
            {
                AttorneyPersonID = c.AttorneyPersonID,
                Attorney = c.Attorney,
                Client = c.Client,
                Source = c.ReferralSource,
                CaseNumber = c.CourtNumber,//nedd to confirm
                NextDate = c.NextHearingDate,
                JcatsNumber = c.CaseNumber,
                Type = c.InvoiceRateType,
                Amount = c.InvoiceAmount,
                Status = c.InvoiceStatus,
                DateToBePaid = c.InvoicePaidDate,
                InvoiceDt = c.InvoiceDate,
                InvoiceNumber = c.InvoiceNumber,
                EncryptedInvoiceID = c.SCCInvoiceID.ToEncrypt(),
                EncryptedCaseID = c.CaseID.ToEncrypt()
            }).ToList();
            return Json(new DataTablesResponse(0, searchList, total, total));
        }

        [HttpPost]
        public virtual ActionResult PrintSSCInvoiceQueue()
        {
            var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 105,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };
            var spResult = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", com_ReportSourceGetByReportID_spParams).ToList();
            ReportClass rpt = new ReportClass();
            string filename = "SCCInvoiceQueue_" + UserManager.UserExtended.UserID.ToEncrypt() + ".pdf";
            try
            {
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/SCCInvoiceQueue.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable(spResult[0].ReportSourceStoredProcedureName, com_ReportSourceGetByReportID_spParams);
                rpt.SetDataSource(table);
                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + filename);

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

            return Download(filename);
        }

        #region Add/Edit        
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.SCCInvoiceQueuePage)]
        public virtual ActionResult SCCInvoiceAddEdit(string id, string caseID)
        {
            //update status bar with new case
            if (caseID.ToDecrypt().ToInt() > 0)
                UserManager.UpdateCaseStatusBar(caseID.ToDecrypt().ToInt(),false);

            //if (UserManager.IsUserAccessTo(SecurityToken.EditSCCInvoiceQueue))
            //{
            var viewModel = new SCCInvoiceAddEditViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                if (!UserManager.IsUserAccessTo(SecurityToken.EditSCCInvoiceQueue))
                    return RedirectToAction("AccessDenied", "Home");

                var pd_SCCInvoiceGet_spParams = new pd_SCCInvoiceGet_spParams()
                {
                    SCCInvoiceID = id.ToDecrypt().ToInt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var invoiceInfo =
                    UtilityService.ExecStoredProcedureWithResults<pd_SCCInvoiceGet_spResult>("pd_SCCInvoiceGet_sp",
                        pd_SCCInvoiceGet_spParams).FirstOrDefault();
                if (invoiceInfo != null)
                {
                    viewModel.SCCInvoiceNumber = invoiceInfo.SCCInvoiceNumber;
                    viewModel.SCCInvoiceDateSubmitted = invoiceInfo.SCCInvoiceDateSubmitted;
                    viewModel.SCCInvoiceRateID = invoiceInfo.SCCInvoiceRateID;
                    viewModel.SCCInvoiceSubmittedByPersonID = invoiceInfo.SCCInvoiceSubmittedByPersonID;
                    viewModel.SCCInvoiceStatusByPersonID = invoiceInfo.SCCInvoiceStatusByPersonID;
                    viewModel.SCCInvoiceDepartmentCodeID = invoiceInfo.SCCInvoiceDepartmentCodeID;
                    viewModel.SCCInvoiceNextHearingDate = invoiceInfo.SCCInvoiceNextHearingDate;
                    viewModel.SCCInvoicePetitionFileDate = invoiceInfo.SCCInvoicePetitionFileDate;
                    viewModel.SCCInvoiceAppointmentDate = invoiceInfo.SCCInvoiceAppointmentDate;
                    viewModel.SCCInvoiceServiceHearingDate = invoiceInfo.SCCInvoiceServiceHearingDate;
                    viewModel.SCCInvoiceFirstRPPDate = invoiceInfo.SCCInvoiceFirstRPPDate;
                    viewModel.SCCInvoiceReliefDate = invoiceInfo.SCCInvoiceReliefDate;
                    viewModel.SCCInvoicePaidDate = invoiceInfo.SCCInvoicePaidDate;
                    viewModel.SCCInvoiceID = invoiceInfo.SCCInvoiceID;
                    viewModel.SCCInvoiceStatusCodeID = invoiceInfo.SCCInvoiceStatusCodeID;
                    viewModel.ReferralSourceCodeID = invoiceInfo.ReferralSourceCodeID;

                    viewModel.CourtNumber = invoiceInfo.CourtNumber;
                    viewModel.AttorneyPersonID = invoiceInfo.AttorneyPersonID;
                    viewModel.AttorneyFirstName = invoiceInfo.AttorneyFirstName;
                    viewModel.AttorneyLastName = invoiceInfo.AttorneyLastName;
                    viewModel.AttorneyPhoneNumber = invoiceInfo.AttorneyPhoneNumber;
                    viewModel.AttorneySSNTaxID = invoiceInfo.AttorneySSNTaxID;
                    viewModel.AttorneyBarNumber = invoiceInfo.AttorneyBarNumber;
                }
            }
            else
            {

                if (!UserManager.IsUserAccessTo(SecurityToken.AddSCCInvoiceQueue))
                    return RedirectToAction("AccessDenied", "Home");

                viewModel.SCCInvoiceDateSubmitted = DateTime.Now.ToString("d");
                viewModel.SCCInvoiceAppointmentDate = UserManager.UserExtended.ApptDate;
                viewModel.SCCInvoiceNextHearingDate = UserManager.UserExtended.NextHearingDate;
            }

            var pd_SCCInvoiceClientGetByCaseID_spParams = new pd_SCCInvoiceClientGetByCaseID_spParams()
            {
                SCCInvoiceID = id.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                CaseID = UserManager.UserExtended.CaseID,
            };
            viewModel.Clients = UtilityService.ExecStoredProcedureWithResults<pd_SCCInvoiceClientGetByCaseID_spResult>("pd_SCCInvoiceClientGetByCaseID_sp", pd_SCCInvoiceClientGetByCaseID_spParams).ToList();

            var pd_SCCInvoiceRateTypeGetAll_spParams = new pd_SCCInvoiceRateTypeGetAll_spParams()
            {
                SCCInvoiceRateID = viewModel.SCCInvoiceRateID.HasValue ? viewModel.SCCInvoiceRateID.Value : 0,//need to change
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                CaseID = UserManager.UserExtended.CaseID,
            };
            viewModel.InvoiceType = UtilityService.ExecStoredProcedureWithResults<pd_SCCInvoiceRateTypeGetAll_spResult>("pd_SCCInvoiceRateTypeGetAll_sp", pd_SCCInvoiceRateTypeGetAll_spParams).Select(x => new CodeViewModel()
            {
                CodeID = x.SCCInvoiceRateID.Value,
                CodeValue = x.SCCInvoiceRateType
            }).ToList();
            viewModel.Department = UtilityFunctions.CodeGetByTypeIdAndUserId(30, includeCodeId: viewModel.SCCInvoiceDepartmentCodeID.HasValue ? viewModel.SCCInvoiceDepartmentCodeID.Value : 0);
            var pd_SCCInvoiceGetByCaseID_spParams = new pd_SCCInvoiceGetByCaseID_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.SSCInvoiceList = UtilityService.ExecStoredProcedureWithResults<pd_SCCInvoiceGetByCaseID_spResult>("pd_SCCInvoiceGetByCaseID_sp", pd_SCCInvoiceGetByCaseID_spParams).ToList();
            viewModel.AttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_AttorneyGetByCaseIDForSCCInvoice_spResult>("pd_AttorneyGetByCaseIDForSCCInvoice_sp", new pd_AttorneyGetByCaseIDForSCCInvoice_spParams()
            {
                //AttorneyPersonID = UserManager.UserExtended.AttorneyPersonID,//need to confirm
                BatchLogJobID = Guid.NewGuid(),
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID
            }).ToList();

            if (viewModel.SCCInvoiceID.HasValue)
            {
                var adminNote = UtilityFunctions.NoteGetByEntity(viewModel.SCCInvoiceID.Value, 235, 123).FirstOrDefault();
                if (adminNote != null)
                {
                    viewModel.AdminNote = adminNote.NoteEntry;
                    viewModel.AdminNoteID = (int)adminNote.NoteID;
                }
            }

            return View(viewModel);
            //}
            //else
            //{
            //    return RedirectToAction("AccessDenied", "Home");
            //}
        }

        [HttpPost]
        public virtual ActionResult SCCInvoiceAddEdit(SCCInvoiceAddEditViewModel model)
        {
            var oUpdate = new pd_SCCInvoiceUpdateOrInsert_spParams
            {
                SCCInvoiceID = model.SCCInvoiceID.HasValue ? model.SCCInvoiceID.Value : 0,

                AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                CaseID = UserManager.UserExtended.CaseID,
                SCCInvoiceSubmittedByPersonID = (int)model.SCCInvoiceSubmittedByPersonID,
                SCCInvoiceDateSubmitted = !model.SCCInvoiceDateSubmitted.IsNullOrEmpty() ? model.SCCInvoiceDateSubmitted.ToDateTime() : (DateTime?)null,
                SCCInvoiceRateID = model.SCCInvoiceRateID,
                SCCInvoiceStatusCodeID = model.SCCInvoiceStatusCodeID,
                SCCInvoiceStatusDate = DateTime.Now,
                SCCInvoiceStatusByPersonID = UserManager.UserExtended.PersonID,
                SCCInvoiceDepartmentCodeID = model.SCCInvoiceDepartmentCodeID,
                SCCInvoiceNextHearingDate = !model.SCCInvoiceNextHearingDate.IsNullOrEmpty() ? model.SCCInvoiceNextHearingDate.ToDateTime() : (DateTime?)null,
                SCCInvoicePetitionFileDate = !model.SCCInvoicePetitionFileDate.IsNullOrEmpty() ? model.SCCInvoicePetitionFileDate.ToDateTime() : (DateTime?)null,
                SCCInvoiceAppointmentDate = !model.SCCInvoiceAppointmentDate.IsNullOrEmpty() ? model.SCCInvoiceAppointmentDate.ToDateTime() : (DateTime?)null,
                SCCInvoiceServiceHearingDate = !model.SCCInvoiceServiceHearingDate.IsNullOrEmpty() ? model.SCCInvoiceServiceHearingDate.ToDateTime() : (DateTime?)null,
                SCCInvoiceFirstRPPDate = !model.SCCInvoiceFirstRPPDate.IsNullOrEmpty() ? model.SCCInvoiceFirstRPPDate.ToDateTime() : (DateTime?)null,
                SCCInvoiceReliefDate = !model.SCCInvoiceReliefDate.IsNullOrEmpty() ? model.SCCInvoiceReliefDate.ToDateTime() : (DateTime?)null,
                SCCInvoicePaidDate = !model.SCCInvoicePaidDate.IsNullOrEmpty() ? model.SCCInvoicePaidDate.ToDateTime() : (DateTime?)null,
                SCCInvoiceNote = model.AttorneyNote,
                CourtNumber = UserManager.UserExtended.PDAPDNumber,

                BatchLogJobID = Guid.NewGuid(),
                AttorneyPersonID = model.SCCInvoiceSubmittedByPersonID,
                RecordStateID = 1,


                UserID = UserManager.UserExtended.UserID,

            };
            if (model.SCCInvoiceID.HasValue && model.SCCInvoiceID.Value > 0)//update
            {
                oUpdate.ReferralSourceCodeID = model.ReferralSourceCodeID;
                oUpdate.CourtNumber = model.CourtNumber;
                oUpdate.AttorneyPersonID = model.AttorneyPersonID;
                oUpdate.AttorneyFirstName = model.AttorneyFirstName;
                oUpdate.AttorneyLastName = model.AttorneyLastName;
                oUpdate.AttorneyPhoneNumber = model.AttorneyPhoneNumber;
                oUpdate.AttorneySSNTaxID = model.AttorneySSNTaxID;
                oUpdate.AttorneyBarNumber = model.AttorneyBarNumber;

                UtilityService.ExecStoredProcedureWithoutResults("pd_SCCInvoiceUpdate_sp", oUpdate);
            }
            else// insert
            {
                var id = UtilityService.ExecStoredProcedureScalar("pd_SCCInvoiceInsert_sp", oUpdate);
                if (id != null)
                {
                    model.SCCInvoiceID = id.ToInt();
                }

            }
            if (model.SCCInvoiceID.HasValue)
            {
                if (Request.Form["ClientCount"] != null)
                {
                    var count = Request.Form["ClientCount"].ToInt();
                    for (var i = 0; i < count; i++)
                    {
                        if (Request.Form["chkClient" + i] != null && Request.Form["chkClient" + i] == "on" && Request.Form["SCCInvoiceClientID" + i].IsNullOrEmpty())
                        {
                            //insert
                            UtilityService.ExecStoredProcedureScalar("pd_SCCInvoiceClientInsert_sp", new pd_SCCInvoiceClientInsert_spParams
                            {
                                SCCInvoiceID = model.SCCInvoiceID.Value,
                                SCCInvoiceClientRoleID = Request.Form["RoleID" + i].ToInt(),
                                SCCInvoiceClientRoleTypeCodeID = Request.Form["RoleTypeCodeID" + i].ToInt(),
                                SCCInvoiceClientName = Request.Form["ClientName" + i],
                                RecordStateID = 1,
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid(),


                            });
                        }
                        else if (Request.Form["SCCInvoiceClientID" + i].ToInt() > 0 && Request.Form["chkClient" + i] != "on")
                        {//delete
                            UtilityService.ExecStoredProcedureWithoutResults("pd_SCCInvoiceClientDelete_sp", new pd_SCCInvoiceClientDelete_spParams
                            {
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid(),
                                ID = Request.Form["SCCInvoiceClientID" + i].ToInt(),
                                RecordStateID = 10
                            });
                        }
                    }
                }

                if (model.AdminNoteID > 0)
                {
                    if (model.AdminNote.IsNullOrEmpty())
                        UtilityFunctions.NoteDelete(model.AdminNoteID);
                    else if (model.AdminNote != Request.Form["AdminNote_oldValue"])
                        UtilityFunctions.NoteUpdate(model.AdminNoteID, 235, 123, model.SCCInvoiceID.Value, 22646, "SCC Invoice Admin Note", model.AdminNote, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

                }
                else if (!model.AdminNote.IsNullOrEmpty())
                {
                    UtilityFunctions.NoteInsert(235, 123, model.SCCInvoiceID.Value, 22646, "SCC Invoice Admin Note", model.AdminNote);
                }

            }


            return Json(new { Status = "Done", Model = model });
        }
        #endregion

        [HttpPost]
        public virtual ActionResult PrintSSCInvoiceQueueAddEdit(string id)
        {
            var com_ReportParameterValueDelete_spParams = new com_ReportParameterValueDelete_spParams()
            {
                ReportID = 103,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            List<object> deletedValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueDelete_sp", com_ReportParameterValueDelete_spParams).ToList();
            List<object> deletedHeaderValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderDelete_sp", com_ReportParameterValueDelete_spParams).ToList();
            if (!string.IsNullOrEmpty(id))
            {
                var com_ReportParameterValueInsert_spParams = new com_ReportParameterValueInsert_spParams()
                {
                    ReportID = 103,
                    Sequence = 1,
                    Value = id,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueInsert_sp", com_ReportParameterValueInsert_spParams).ToList();
            }
            var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 103,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };
            ReportClass rpt = new ReportClass();
            var spResult = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", com_ReportSourceGetByReportID_spParams).ToList();

            rpt.FileName = HttpContext.Server.MapPath("~/Reports/SCCInvoice.rpt");
            rpt.Load();
            var table = UtilityService.ExecStoredProcedureForDataTable("dbo.rpt_SCCInvoice_sp", com_ReportSourceGetByReportID_spParams);
            rpt.SetDataSource(table);
            foreach (var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != "SCCInvoice.rpt"))
            {
                var subTableData = (UtilityService.ExecStoredProcedureForDataTable(subRptDt.ReportSourceStoredProcedureName.Replace("dbo.", ""), com_ReportSourceGetByReportID_spParams));
                rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(subTableData);
            }


            string fileName = "SCCInvoice_" + id.ToInt().ToEncrypt() + ".pdf";
            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + fileName);

            rpt.Close();
            rpt.Dispose();


            return Download(fileName);
        }
        public virtual ActionResult Download(string file)
        {
            string fullPath = UtilityFunctions.GetDocumentDownloadFolderPath() + file;

            // Use once user export preferences are implemented
            /*if (_userManager.UserExtended.PrintDocumentOn == "NewWindow"){
                Response.AppendHeader("Content-Disposition", "inline; filename=" + file);
                if (Path.GetExtension(fullPath).Contains(".pdf"))
                    return File(fullPath, "application/pdf");
                else if (Path.GetExtension(fullPath).Contains(".xlsx"))
                    return File(fullPath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            }*/
            if (UserManager.UserExtended.PrintDocumentOn == "NewWindow" && Path.GetExtension(fullPath).Contains("pdf"))
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=" + file);
                return File(fullPath, "application/pdf");
            }
            if (System.IO.File.Exists(fullPath))
            {

                return File(fullPath, "application/force-download", file);
            }
            else
            {
                return Content("File not found");
            }

        }
    }


}