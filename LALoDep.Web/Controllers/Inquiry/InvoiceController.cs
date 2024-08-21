using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_HearingRates;
using LALoDep.Domain.pd_HourlyInvoiceList;
using LALoDep.Domain.pd_Invoice;
using LALoDep.Domain.pd_InvoiceQueue;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models;
using LALoDep.Models.Inquiry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Controllers.Inquiry
{
    public partial class InvoiceController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public InvoiceController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.InvoiceList, PageSecurityItemID = SecurityToken.InvoiceQueue)]
        public virtual ActionResult Search()
        {
            var invoiceList = new List<InvoiceListViewModel>();
            if (UserManager.UserExtended.CaseID > 0)
            {
                var invoices = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetByCaseID_spResult>("pd_InvoiceGetByCaseID_sp",
                                        new pd_InvoiceGetByCaseID_spParams() { CaseID = UserManager.UserExtended.CaseID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).ToList();

                var invoiceItems = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceItemGetByCaseID_spResult>("pd_InvoiceItemGetByCaseID_sp",
                                        new pd_InvoiceItemGetByCaseID_spParams() { CaseID = UserManager.UserExtended.CaseID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).ToList();


                invoiceList = (from inv in invoices
                               join invItem in invoiceItems on inv.InvoiceID equals invItem.InvoiceID
                               select new InvoiceListViewModel
                               {
                                   InvoiceID = inv.InvoiceID,
                                   InvoiceDate = inv.InvoiceDate,
                                   ClientDisplayName = inv.ClientLastName + ", " + inv.ClientFirstName,
                                   OtherParties = inv.OtherParties,
                                   Void = inv.Void,
                                   NoteID = inv.NoteID,
                                   Status = inv.Status,
                                   InvoiceStatusCodeID = inv.InvoiceStatusCodeID,

                                   HearingID = invItem.HearingID,
                                   ItemType = invItem.ItemType,
                                   InvoiceHearingAmount = invItem.InvoiceHearingAmount ?? 0,
                                   ItemDescription = invItem.ItemDescription,
                               }).ToList();
            }
            return View(invoiceList);
        }

        public virtual ActionResult InvoiceNote(string id, string pkId)
        {
            var viewModel = new SaveNoteViewModel();
            viewModel.EntityPrimaryKeyID = pkId.ToDecrypt().ToInt();
            if (id.ToDecrypt().ToInt() != 0)
                viewModel.NoteID = id.ToDecrypt().ToInt();

            var noteEntry = UtilityFunctions.NoteGetByEntity(viewModel.EntityPrimaryKeyID ?? 0, 122, 123).FirstOrDefault();
            if (noteEntry != null)
            {
                viewModel.NoteSubject = noteEntry.NoteSubject;
                viewModel.NoteEntry = noteEntry.NoteEntry;
                viewModel.NoteID = noteEntry.NoteID;
                viewModel.NoteEntityCodeID = noteEntry.NoteEntityCodeID;
                viewModel.NoteEntityTypeCodeID = noteEntry.NoteEntityTypeCodeID;
                viewModel.NoteTypeCodeID = noteEntry.NoteTypeCodeID;
            }
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult InvoiceNote(SaveNoteViewModel viewModel)
        {
            if (!viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteInsert(122, 123, viewModel.EntityPrimaryKeyID.Value, 16, null, viewModel.NoteEntry);
            }
            else if (viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteUpdate(viewModel.NoteID.Value, viewModel.NoteEntityCodeID.Value, viewModel.NoteEntityTypeCodeID.Value, viewModel.EntityPrimaryKeyID.Value,
                                                            viewModel.NoteTypeCodeID.Value, null, viewModel.NoteEntry);

            }
            else if (viewModel.NoteID.HasValue && string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var pd_NoteDelete_spParams = new LALoDep.Domain.pd_Conflict.pd_NoteDelete_spParams()
                {
                    ID = viewModel.NoteID ?? 0,
                    RecordStateID = 10,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    LoadOption = "Note",
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteDelete_sp", pd_NoteDelete_spParams).ToList();
            }

            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.EditHearing)]
        public virtual ActionResult EditHearingAmount(string id)
        {
            var viewModel = new EditHearingAmountViewModel();
            viewModel.HearingID = id.ToDecrypt().ToInt();
            var hearingGet = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp",
                        new pd_HearingGet_spParams { HearingID = viewModel.HearingID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

            if (hearingGet != null)
            {
                viewModel.HearingDate = hearingGet.HearingDateTime.ToDefaultFormat();
                viewModel.BaseRate = UtilityService.ExecStoredProcedureScalar("pd_HearingRateGetByHearingID_sp",
                                        new pd_HearingRateGetByHearingID_spParams { HearingID = viewModel.HearingID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).ToDecimal();

                viewModel.ModifiedRate = hearingGet.HearingInvoiceAmount;
            }


            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult EditHearingAmount(EditHearingAmountViewModel viewModel)
        {

            UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingUpdateInvoiceAmount_sp",
                                        new pd_HearingUpdateInvoiceAmount_spParams
                                        {
                                            HearingID = viewModel.HearingID,
                                            HearingInvoiceAmount = viewModel.ModifiedRate,
                                            RecordStateID = 1,
                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid()
                                        }).FirstOrDefault();

            return Json(new { isSuccess = true });
        }

        public virtual ActionResult InvoiceSentPopup(string id)
        {
            //var personContact = UtilityService.ExecStoredProcedureWithResults<pd_PersonContactGetJcatsEmailByUserID_spResult>("pd_PersonContactGetJcatsEmailByUserID_sp",
            //                                new pd_PersonContactGetJcatsEmailByUserID_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).ToList();

            //var personContactRevenueRecovery = UtilityService.ExecStoredProcedureWithResults<pd_PersonContactGetJcatsEmailByUserID_spResult>("pd_PersonContactGetJcatsEmailForRevenueRecovery_sp",
            //                                new pd_PersonContactGetJcatsEmailByUserID_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).ToList();
            var viewModel = new InvoiceVerifyViewModel();
            viewModel.InvoiceID = id.ToDecrypt().ToInt();

            var agencyInfo = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceID_spResult>("pd_InvoiceGetUnsentByInvoiceID_sp",
                                            new pd_InvoiceGetUnsentByInvoiceID_spParams() { InvoiceID = viewModel.InvoiceID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).FirstOrDefault();
            viewModel.AgencyName = agencyInfo.AgencyName;
            viewModel.HHSANumber = agencyInfo.HHSA;
            viewModel.JCATSNumber = agencyInfo.CaseNumber;
            viewModel.Client = agencyInfo.PersonNameFirst + " " + agencyInfo.PersonNameLast;
            viewModel.DOB = agencyInfo.PersonDOB.ToDefaultFormat();
            if (!agencyInfo.MailStreet.IsNullOrEmpty())
            {
                viewModel.Address = agencyInfo.MailStreet + ", " + agencyInfo.MailCity + ", " + agencyInfo.MailState + " " + agencyInfo.MailZipCode + " " + agencyInfo.MailCountry;

            }
            else if (!agencyInfo.HomeStreet.IsNullOrEmpty())
            {
                viewModel.Address = agencyInfo.HomeStreet + ", " + agencyInfo.HomeCity + ", " + agencyInfo.HomeState + " " + agencyInfo.HomeZipCode + " " + agencyInfo.HomeCountry;

            }
            else
            {
                viewModel.Address = "";
            }
            var caseInfo = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceIDPetition_spResult>("pd_InvoiceGetUnsentByInvoiceIDPetition_sp",
                                            new pd_InvoiceGetUnsentByInvoiceIDPetition_spParams() { InvoiceID = viewModel.InvoiceID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).FirstOrDefault();

            viewModel.CaseNumber = caseInfo.DocketNumber;

            viewModel.ParentList = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceIDParent_spResult>("pd_InvoiceGetUnsentByInvoiceIDParent_sp",
                                            new pd_InvoiceGetUnsentByInvoiceIDParent_spParams() { InvoiceID = viewModel.InvoiceID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() })
                                        .Select(x => new ParentViewModel
                                        {
                                            Parent = x.PersonNameFirst + " " + x.PersonNameLast,
                                            Address = !x.Mailstreet.IsNullOrEmpty() ? ((x.Mailstreet + "," + x.Mailcity + "," + x.Mailstate + " " + x.MailzipCode + "," + x.Mailstreet).Trim(',').Trim()) : ((x.Homestreet + "," + x.Homecity + "," + x.Homestate + " " + x.HomezipCode + "," + x.HomeCountry).Trim(',').Trim()),
                                            DOB = x.DOB.ToDefaultFormat(),
                                            SSN = x.ssn
                                        }).ToList();


            //var bilingInfo = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceIDService_spResult>("pd_InvoiceGetUnsentByInvoiceIDService_sp",
            //                                new pd_InvoiceGetUnsentByInvoiceIDService_spParams() { InvoiceID = viewModel.InvoiceID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() })
            //                                .FirstOrDefault();


            //viewModel.Amount = bilingInfo.Amount;
            //viewModel.ServiceStartDate = bilingInfo.StartDate;
            //viewModel.ServiceEndDate = bilingInfo.EndDate;
            //viewModel.ServiceType = bilingInfo.ServiceType;

            viewModel.InvoiceDetails = UtilityService.ExecStoredProcedureWithResults<pd_InvoiceGetUnsentByInvoiceIDService_spResult>("pd_InvoiceGetUnsentByInvoiceIDService_sp",
                                            new pd_InvoiceGetUnsentByInvoiceIDService_spParams() { InvoiceID = viewModel.InvoiceID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() })
                                            .Select(x => new InvoiceDetails
                                            {
                                                Amount = x.Amount.ToDecimal(),
                                                ServiceStartDate = x.StartDate,
                                                ServiceEndDate = x.EndDate,
                                                ServiceType = x.ServiceType
                                            }).ToList();


            var note = UtilityFunctions.NoteGetByEntity(viewModel.InvoiceID, 122, 123).FirstOrDefault();
            if (note != null)
            {
                viewModel.NoteEntry = note.NoteEntry;
            }

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult InvoiceDelete(int id)
        {

            UtilityService.ExecStoredProcedureWithoutResultADO("pd_InvoiceDelete_sp",
                                        new pd_InvoiceDelete_spParams
                                        {
                                            ID = id,
                                            RecordStateID = 10,
                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid(),
                                            LoadOption = "Invoice"
                                        });

            return Json(new { isSuccess = true });
        }
        [HttpPost]
        public virtual ActionResult InvoiceVoid(int id)
        {

            UtilityService.ExecStoredProcedureWithoutResultADO("pd_InvoiceVoidByInvoiceID_sp",
                                        new pd_InvoiceVoidByInvoiceID_spParams
                                        {
                                            InvoiceID = id,

                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid(),

                                        });

            return Json(new { isSuccess = true });
        }
        [HttpPost]
        public virtual ActionResult RegenerateInvoice(int id)
        {

            UtilityService.ExecStoredProcedureWithoutResultADO("pd_RegenerateInvoice_sp",
                                        new pd_RegenerateInvoice_spParams
                                        {
                                            InvoiceID = id,

                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid(),

                                        });

            return Json(new { isSuccess = true });
        }
    }
}