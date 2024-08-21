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
using LALoDep.Domain.JcatsESignature;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {

        #region IVEInvoice

         [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.IVEInvoicePage )]
        public virtual ActionResult IVEInvoice()
        {
            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];

            var activityMonth = DateTime.Now;
            int? agencyCountyId = null;
            int? agencyGroupId = null;

            if (Request.QueryString["year"] != null && Request.QueryString["month"] != null)
            {
                var str = string.Format("{0}/01/{1}", Request.QueryString["month"], Request.QueryString["year"]);
                if (!DateTime.TryParse(str, out activityMonth))
                {
                    activityMonth = DateTime.Now;
                }

            }
            if (Request.QueryString["agencyCountyId"] != null)
            {

                agencyCountyId = Request.QueryString["agencyCountyId"].ToDecrypt().ToInt();


            }
            
            if (Request.QueryString["AgencyGroupID"] != null)
            {
                agencyGroupId = Request.QueryString["AgencyGroupID"].ToDecrypt().ToInt();
                
            }
            var viewModel = UtilityService.ExecStoredProcedureWithResults<IVEInvoiceViewModel>("TitleIVeInvoiceGet_sp", new TitleIVeInvoiceGet_spParams
            {
                InvoiceYear = activityMonth.Year,
                InvoiceMonth = activityMonth.Month,
                AgencyCountyID = agencyCountyId,
                UserID = UserManager.UserExtended.UserID,
                AgencyGroupID= agencyGroupId
            }).FirstOrDefault();
            if (viewModel == null)
                viewModel = new IVEInvoiceViewModel();

            if (viewModel.AgencyCountyID.HasValue)
                viewModel.AgencyCountyEncryptedID = viewModel.AgencyCountyID.ToEncrypt();
            else if (agencyCountyId > 0)
            {
                viewModel.AgencyCountyEncryptedID = agencyCountyId.ToEncrypt();
                viewModel.AgencyCountyID = agencyCountyId;

            }
            viewModel.ActivityMonth = activityMonth;
            var oJcatsESignature = UtilityService.ExecStoredProcedureWithResults<JcatsESignatureGetByJcatsUserID_spResult>("JcatsESignatureGetByJcatsUserID_sp", new JcatsESignatureGetByJcatsUserID_spParams()
            {
                JcatsUserID = UserManager.UserExtended.UserID,
                UserID = UserManager.UserExtended.UserID
            }).FirstOrDefault();
            if (oJcatsESignature != null)
            {
                viewModel.IsUserSignatureExists = true;
                if (oJcatsESignature.SignatureFilePath.IsNullOrEmpty())
                {
                    viewModel.IsUserSignatureExists = false;
                }
            }
            if (viewModel.ContractorSigningPersonID.HasValue)
            {
                oJcatsESignature = UtilityService.ExecStoredProcedureWithResults<JcatsESignatureGetByJcatsUserID_spResult>("JcatsESignatureGetByJcatsUserID_sp", new JcatsESignatureGetByJcatsUserID_spParams()
                {
                    JcatsUserID = viewModel.ContractorSigningPersonID.Value,
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();
                if (oJcatsESignature != null)
                {

                    if (!oJcatsESignature.SignatureFilePath.IsNullOrEmpty())
                    {
                        viewModel.AgencySignaturePath = rootPath + "\\" + oJcatsESignature.SignatureFilePath;
                    }
                }

            }
            if (viewModel.JCCSigningPersonID.HasValue)
            {
                oJcatsESignature = UtilityService.ExecStoredProcedureWithResults<JcatsESignatureGetByJcatsUserID_spResult>("JcatsESignatureGetByJcatsUserID_sp", new JcatsESignatureGetByJcatsUserID_spParams()
                {
                    JcatsUserID = viewModel.JCCSigningPersonID.Value,
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();
                if (oJcatsESignature != null)
                {

                    if (!oJcatsESignature.SignatureFilePath.IsNullOrEmpty())
                    {
                        viewModel.JccSignaturePath = rootPath + "\\" + oJcatsESignature.SignatureFilePath;
                    }
                }

            }
            viewModel.AgencyCountyList = UtilityService.ExecStoredProcedureWithResults<TitleIVeCountyDropDown_spResult>("TitleIVeCountyDropDown_sp", new TitleIVeCountyDropDown_spParams { UserID = UserManager.UserExtended.UserID })
                                                  .Select(x => new SelectListItem() { Text = x.AgencyCounty, Value = x.AgencyCountyID.ToEncrypt() });

            return View(viewModel);
        }
        [HttpPost]
        public virtual ActionResult IVEInvoice(IVEInvoiceViewModel model)
        {

            var id = UtilityService.ExecStoredProcedureScalar("TitleIVeInvoiceInsertUpdate_sp", new TitleIVeInvoiceInsertUpdate_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                AgencyCountyID = model.AgencyCountyID,
                AgencyGroupID = model.AgencyGroupID,
                AgreementNumber = model.AgreementNumber,
                AmountDue = model.AmountDue,
                CaliforniaEligibilityAmount = model.CaliforniaEligibilityAmount,
                ContractdirectlyWithCourt = model.ContractdirectlyWithCourt,
                ErrorID = 0,
                InvoiceID = model.InvoiceID.HasValue ? model.InvoiceID.Value.ToString() : "",
                InvoiceDate = model.InvoiceDate,
                InvoiceMonth = model.InvoiceMonth,
                InvoiceYear = model.InvoiceDate.ToDateTimeValue().HasValue ? model.InvoiceDate.ToDateTimeValue().Value.Year : (int?)null,
                Personnel = model.Personnel,
                ProfessionalServices = model.ProfessionalServices,
                OperationalExpenses = model.OperationalExpenses,
                TotalExpenses = model.TotalExpenses,
                TravelExpenses = model.TravelExpenses,
                TitleIVeInvoiceID = model.TitleIVeInvoiceID,
                JCCSigningPersonID = model.JCCSigningPersonID,
                JCCSigningDate = model.JCCSigningDate,
                IVeReimburseAmount = model.IVeReimburseAmount,
                CourtSigningTitle = model.CourtSigningTitle,
                CourtSigningPerson = model.CourtSigningPerson,
                CourtSigningDate = model.CourtSigningDate,
                CourtContractAgreementNbr = model.CourtContractAgreementNbr,
                ContractorSigningDate = model.ContractorSigningDate,
                ContractorSigningPersonID = model.ContractorSigningPersonID,


            });
            if (id.ToInt() > 0)
                model.TitleIVeInvoiceID = id.ToInt();

            if (!model.SignatureType.IsNullOrEmpty())
            {


                UtilityService.ExecStoredProcedureWithoutResults("TitleIVeInvoiceSignatureUpdate_sp", new TitleIVeInvoiceSignatureUpdate_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    JCCSigningPersonID = model.JCCSigningPersonID,
                    JCCSigningDate = model.JCCSigningDate,
                    CourtSigningTitle = model.CourtSigningTitle,
                    CourtSigningPersonName = model.CourtSigningPerson,
                    CourtSigningDate = model.CourtSigningDate,
                    SignatureType = model.SignatureType,
                    TitleIVeInvoiceID = model.TitleIVeInvoiceID
                });
            }



            return Json(new { Status = "Done", Model = model });
        }
        [HttpPost]
        public virtual ActionResult IVeInvoiceInsertRemoveAgencySignature(int TitleIVeInvoiceID)
        {

            //UtilityService.ExecStoredProcedureWithoutResults("TitleIVeInvoiceInsertRemoveAgencySignature", new TitleIVeInvoiceInsertRemoveAgencySignature_Params
            //{
            //    UserID = UserManager.UserExtended.UserID,
            //    TitleIVeInvoiceID = TitleIVeInvoiceID


            //});




            UtilityService.ExecStoredProcedureWithoutResults("TitleIVeInvoiceSignatureUpdate_sp", new TitleIVeInvoiceSignatureUpdate_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                SignatureType = "Remove",
                TitleIVeInvoiceID = TitleIVeInvoiceID
            });
            return Json(new { Status = "Done" });
        }



        #endregion

        #region IVEProfessionalServices

        //  [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.IVEInvoicePage, PageSecurityItemID = SecurityToken.IVEInvoice)]
        public virtual ActionResult IVEProfessionalServices(string id)
        {
            var viewModel = new IVEProfessionalServicesViewModel()
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
            var jsonData = UtilityService.ExecStoredProcedureWithJsonResult("TitleIVeProfessionalServiceGet_sp", new TitleIVeProfessionalServiceGet_spParams
            {
                InvoiceID = viewModel.InvoiceID,
                UserID = UserManager.UserExtended.UserID
            });
            if (jsonData != "")
                viewModel.TitleIVeProfessionalServiceList = JsonConvert.DeserializeObject<List<TitleIVeProfessionalServiceGet_spResult>>(jsonData);
            if (viewModel.TitleIVeProfessionalServiceList.Any())
            {
                viewModel.ProviderOverheadRate=viewModel.TitleIVeProfessionalServiceList.FirstOrDefault().OverHeadRate;
            }
            viewModel.TypeOfServiceList = UtilityFunctions.TitleIVeCodeDropDown(2002).ToList();
            viewModel.SpecifyUnitList = UtilityFunctions.TitleIVeCodeDropDown(2003).ToList();
            viewModel.TitleIVeProfessionalServiceList.Add(new TitleIVeProfessionalServiceGet_spResult()
            {
                TitleIVeProfessionalServiceID = 0,
                TitleIVeInvoiceID = viewModel.InvoiceID,
                RecordStateID = 1,

            });
            viewModel.TitleIVeProfessionalServiceList.Add(new TitleIVeProfessionalServiceGet_spResult()
            {
                TitleIVeProfessionalServiceID = 0,
                TitleIVeInvoiceID = viewModel.InvoiceID,
                RecordStateID = 1,

            }); viewModel.TitleIVeProfessionalServiceList.Add(new TitleIVeProfessionalServiceGet_spResult()
            {
                TitleIVeProfessionalServiceID = 0,
                TitleIVeInvoiceID = viewModel.InvoiceID,
                RecordStateID = 1,

            });
            if (viewModel.TitleIVeProfessionalServiceList.Any())
            {
                viewModel.TotalCACFundAmount = viewModel.TitleIVeProfessionalServiceList.Sum(c => c.CACFundAmount);
                viewModel.TotalFFDRPFundAmount = viewModel.TitleIVeProfessionalServiceList.Sum(c => c.FFDRPFundAmount);
                viewModel.TotalEligibleCost = viewModel.TitleIVeProfessionalServiceList.Sum(c => c.EligibleCost);
            }
            return View(viewModel);
        }
        [HttpPost]
        public virtual ActionResult IVEProfessionalServices(List<TitleIVeProfessionalServiceAddEditModel> viewModel)
        {
            if (viewModel != null)
            {
                var jsonData = JsonConvert.SerializeObject(viewModel);
                UtilityService.ExecStoredProcedureWithoutResults("TitleIVeProfessionalServiceInsertUpdate_sp", new TitleIVeProfessionalServiceInsertUpdate_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    InputData = jsonData
                });
                jsonData = UtilityService.ExecStoredProcedureWithJsonResult("TitleIVeProfessionalServiceGet_sp", new TitleIVeProfessionalServiceGet_spParams
                {
                    InvoiceID = viewModel[0].TitleIVeInvoiceID,
                    UserID = UserManager.UserExtended.UserID
                });
                if (jsonData != "")
                {
                    var data = JsonConvert.DeserializeObject<List<TitleIVeProfessionalServiceGet_spResult>>(jsonData).OrderByDescending(o => o.TitleIVeProfessionalServiceID).Select(o => o.TitleIVeProfessionalServiceID).FirstOrDefault();
                    if (data.HasValue)
                    {
                        return Json(new { isSuccess = true, LastTitleIVEProfessionalServiceID = data.Value.ToEncrypt() });
                    }
                };
                return Json(new { isSuccess = true, LastTitleIVEProfessionalServiceID = "" });
            }
            return Json(new { isSuccess = false, message = "There is some issue while saving data" });
        }
        [HttpPost]

        public virtual JsonResult IVEProfessionalServicesDelete(string id)
        {
            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];

            var oParams = new TitleIVeProfessionalServiceDelete_spParams()
            {


                UserID = UserManager.UserExtended.UserID,
                TitleIVeProfessionalServiceID = id.ToInt()
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<TitleIVeProfessionalServiceDelete_spResult>("TitleIVeProfessionalServiceDelete_sp", oParams).ToList();
            foreach (var item in deletedData)
            {
                item.DocumentName = rootPath + "\\" + item.DocumentName;
                if (System.IO.File.Exists(item.DocumentName))
                {
                    try
                    {
                        System.IO.File.Delete(item.DocumentName);
                    }
                    catch
                    {

                    }
                }
            }
            return Json(new { isSuccess = true });
        }

        #endregion


        #region IVETravelExpenses
        public virtual ActionResult IVETravelExpenses(string id)
        {
            var viewModel = new IVETravelExpensesViewModel()
            {
                InvoiceID = id.ToDecrypt().ToInt(),
            };
            viewModel.ContractorName = "Contractor Name";
            viewModel.ContractorStreetAddress = "Street Address";
            viewModel.ContractorCityStateZipCode = "City, State ZipCode";
            viewModel.ContractorTelephone = "Telephone";

            var headerData = UtilityService.ExecStoredProcedureWithResults<TitleIVeExpenseHeader_spResult>("TitleIVeExpenseHeader_sp", new TitleIVeExpenseHeader_spParams
            {
                InvoiceID = viewModel.InvoiceID,
                UserID = UserManager.UserExtended.UserID
            }).FirstOrDefault();

            viewModel.CourtSystem = headerData?.CourtSystem;
            viewModel.InvoicePeriod = headerData?.InvoicePeriod;
            viewModel.InvoiceDate = headerData?.InvoiceDate;
            viewModel.HeaderInvoiceID = headerData?.InvoiceID;

            var jsonData = UtilityService.ExecStoredProcedureWithJsonResult("TitleIVeTravelExpenseGet_sp", new TitleIVeTravelExpensesGet_spParams
            {
                InvoiceID = viewModel.InvoiceID,
                UserID = UserManager.UserExtended.UserID
            });
            if (jsonData != "")
                viewModel.TitleIVeTravelExpensesList = JsonConvert.DeserializeObject<List<TitleIVeTravelExpensesGet_spResult>>(jsonData);

            if (viewModel.TitleIVeTravelExpensesList.Any())
            {
                viewModel.TotalCACFundAmount = viewModel.TitleIVeTravelExpensesList.Sum(c => c.CACFundAmount);
                viewModel.TotalFFDRPFundAmount = viewModel.TitleIVeTravelExpensesList.Sum(c => c.FFDRPFundAmount);
                viewModel.TotalEligibleCost = viewModel.TitleIVeTravelExpensesList.Sum(c => c.EligibleCost);
            }

            viewModel.SpecifyUnitList = UtilityFunctions.TitleIVeCodeDropDown(2003).ToList();

            viewModel.TitleIVeTravelExpensesList.Add(new TitleIVeTravelExpensesGet_spResult { TitleIVeTravelExpenseID = 0, TitleIVeInvoiceID = viewModel.InvoiceID, RecordStateID = 1 });
            viewModel.TitleIVeTravelExpensesList.Add(new TitleIVeTravelExpensesGet_spResult { TitleIVeTravelExpenseID = 0, TitleIVeInvoiceID = viewModel.InvoiceID, RecordStateID = 1 });
            viewModel.TitleIVeTravelExpensesList.Add(new TitleIVeTravelExpensesGet_spResult { TitleIVeTravelExpenseID = 0, TitleIVeInvoiceID = viewModel.InvoiceID, RecordStateID = 1 });
            return View(viewModel);

        }
        [HttpPost]
        public virtual ActionResult IVETravelExpenses(List<TitleIVETravelExpensesAddEditModel> viewModel)
        {
            if (viewModel != null)
            {
                var jsonData = JsonConvert.SerializeObject(viewModel);
                UtilityService.ExecStoredProcedureWithoutResults("TitleIVeTravelExpenseInsertUpdate_sp", new TitleIVTravelExpensesAInsertUpdate_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    InputData = jsonData
                });
                jsonData = UtilityService.ExecStoredProcedureWithJsonResult("TitleIVeTravelExpenseGet_sp", new TitleIVeTravelExpensesGet_spParams
                {
                    InvoiceID = viewModel[0].TitleIVeInvoiceID,
                    UserID = UserManager.UserExtended.UserID
                });
                if (jsonData != "")
                {
                    var data = JsonConvert.DeserializeObject<List<TitleIVeTravelExpensesGet_spResult>>(jsonData).OrderByDescending(o => o.TitleIVeTravelExpenseID).Select(o => o.TitleIVeTravelExpenseID).FirstOrDefault();
                    if (data.HasValue)
                    {
                        return Json(new { isSuccess = true, LastTitleIVeTravelExpenseID = data.Value.ToEncrypt() });
                    }
                };
                return Json(new { isSuccess = true, LastTitleIVeTravelExpenseID = "" });
            }
            return Json(new { isSuccess = false, message = "There is some issue while saving data" });
        }


        [HttpPost]

        public virtual JsonResult IVETravelExpensesDelete(string id)
        {
            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];

            var oParams = new TitleIVeTravelExpenseDelete_spParams()
            {


                UserID = UserManager.UserExtended.UserID,
                TitleIVeTravelID = id.ToInt()
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<TitleIVeTravelExpenseDelete_spResult>("TitleIVeTravelExpenseDelete_sp", oParams).ToList();
            foreach (var item in deletedData)
            {
                item.DocumentName = rootPath + "\\" + item.DocumentName;
                if (System.IO.File.Exists(item.DocumentName))
                {
                    try
                    {
                        System.IO.File.Delete(item.DocumentName);
                    }
                    catch
                    {

                    }
                }
            }
            return Json(new { isSuccess = true });
        }

        #endregion

        #region IVEOperatingExpenses
        public virtual ActionResult IVEOperatingExpenses(string id)
        {
            var viewModel = new IVEOperatingExpensesViewModel()
            {
                InvoiceID = id.ToDecrypt().ToInt(),

            };
            viewModel.ContractorName = "Contractor Name";
            viewModel.ContractorStreetAddress = "Street Address";
            viewModel.ContractorCityStateZipCode = "City, State ZipCode";
            viewModel.ContractorTelephone = "Telephone";

            var headerData = UtilityService.ExecStoredProcedureWithResults<TitleIVeExpenseHeader_spResult>("TitleIVeExpenseHeader_sp", new TitleIVeExpenseHeader_spParams
            {
                InvoiceID = viewModel.InvoiceID,
                UserID = UserManager.UserExtended.UserID
            }).FirstOrDefault();

            viewModel.CourtSystem = headerData?.CourtSystem;
            viewModel.InvoicePeriod = headerData?.InvoicePeriod;
            viewModel.InvoiceDate = headerData?.InvoiceDate;
            viewModel.HeaderInvoiceID = headerData?.InvoiceID;
            viewModel.NonTrainingExpenses = "";
            
            var jsonData = UtilityService.ExecStoredProcedureWithJsonResult("TitleIVeOperatingExpenseGet_sp", new TitleIVeOperatingExpensesGet_spParams
            {
                InvoiceID = viewModel.InvoiceID,
                UserID = UserManager.UserExtended.UserID
            });
            if (jsonData != "")
                viewModel.TitleIVeOperatingExpensesList = JsonConvert.DeserializeObject<List<TitleIVeOperatingExpensesGet_spResult>>(jsonData);
         
            if (viewModel.TitleIVeOperatingExpensesList.Any())
            {
                viewModel.TotalOperatingExpensesAmount = viewModel.TitleIVeOperatingExpensesList.Sum(c => c.ExpenseAmount);
                viewModel.TotalCACFundAmount = viewModel.TitleIVeOperatingExpensesList.Sum(c => c.CACFundAmount);
                viewModel.TotalFFDRPFundAmount = viewModel.TitleIVeOperatingExpensesList.Sum(c => c.FFDRPFundAmount);
                viewModel.TotalEligibleCost = viewModel.TitleIVeOperatingExpensesList.Sum(c => c.EligibleCost);
                viewModel.TotalEligibleAmountNonTraining = viewModel.TitleIVeOperatingExpensesList.Sum(c => c.EligibleAmountNonTraining);
                viewModel.TotalEligibleAmountAttorneyTraining = viewModel.TitleIVeOperatingExpensesList.Sum(c => c.EligibleAmountAttorneyTraining);
                viewModel.TotalEligibleAmountNonAttorneyTraining = viewModel.TitleIVeOperatingExpensesList.Sum(c => c.EligibleAmountNonAttorneyTraining);
                 
                viewModel.NonTrainingExpenses = viewModel.TitleIVeOperatingExpensesList.FirstOrDefault().OverHeadRate.ToDecimal().ToString();
            }

            viewModel.TitleIVeOperatingExpensesList.Add(new TitleIVeOperatingExpensesGet_spResult { TitleIVeOperatingExpenseID = 0, TitleIVeInvoiceID = viewModel.InvoiceID, RecordStateID = 1 });
            viewModel.TitleIVeOperatingExpensesList.Add(new TitleIVeOperatingExpensesGet_spResult { TitleIVeOperatingExpenseID = 0, TitleIVeInvoiceID = viewModel.InvoiceID, RecordStateID = 1 });
            viewModel.TitleIVeOperatingExpensesList.Add(new TitleIVeOperatingExpensesGet_spResult { TitleIVeOperatingExpenseID = 0, TitleIVeInvoiceID = viewModel.InvoiceID, RecordStateID = 1 });

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult IVEOperatingExpenses(List<TitleIVEOperatingExpensesAddEditModel> viewModel)
        {
            if (viewModel != null)
            {
                var jsonData = JsonConvert.SerializeObject(viewModel);
                    UtilityService.ExecStoredProcedureWithoutResults("TitleIVeOperatingExpenseInsertUpdate_sp", new TitleIVOperatingExpensesInsertUpdate_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    InputData = jsonData
                });

                jsonData = UtilityService.ExecStoredProcedureWithJsonResult("TitleIVeOperatingExpenseGet_sp", new TitleIVeOperatingExpensesGet_spParams
                {
                    InvoiceID = viewModel[0].TitleIVeInvoiceID,
                    UserID = UserManager.UserExtended.UserID
                });
                if (jsonData != "")
                {
                    var data = JsonConvert.DeserializeObject<List<TitleIVeOperatingExpensesGet_spResult>>(jsonData).OrderByDescending(o => o.TitleIVeOperatingExpenseID).Select(o => o.TitleIVeOperatingExpenseID).FirstOrDefault();
                    if (data.HasValue)
                    {
                        return Json(new { isSuccess = true, LastTitleIVeOperatingExpenseID = data.Value.ToEncrypt() });
                    }
                };



                return Json(new { isSuccess = true, LastTitleIVeOperatingExpenseID = "" });



            }

            return Json(new { isSuccess = false, message = "There is some issue while saving data" });
        }
        [HttpPost]

        public virtual JsonResult IVEOperatingExpensesDelete(string id)
        {
            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];

            var oParams = new TitleIVeOperatingExpenseDelete_spParams()
            {


                UserID = UserManager.UserExtended.UserID,
                TitleIVeOperatingExpenseID = id.ToInt()
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<TitleIVeOperatingExpenseDelete_spResult>("TitleIVeOperatingExpenseDelete_sp", oParams).ToList();
            foreach (var item in deletedData)
            {
                item.DocumentName = rootPath + "\\" + item.DocumentName;
                if (System.IO.File.Exists(item.DocumentName))
                {
                    try
                    {
                        System.IO.File.Delete(item.DocumentName);
                    }
                    catch
                    {

                    }
                }
            }
            return Json(new { isSuccess = true });
        }

        #endregion

        #region IVeInvoiceStatus

        //  [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.IVEInvoicePage, PageSecurityItemID = SecurityToken.IVEInvoice)]
        public virtual ActionResult IVeInvoiceStatus()
        {
            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];

            var activityMonth = DateTime.Now;
            int? agencyCountyId = null;

            if (Request.QueryString["year"] != null && Request.QueryString["month"] != null)
            {
                var str = string.Format("{0}/01/{1}", Request.QueryString["month"], Request.QueryString["year"]);
                if (!DateTime.TryParse(str, out activityMonth))
                {
                    activityMonth = DateTime.Now;
                }

            }
            if (Request.QueryString["agencyCountyId"] != null && Request.QueryString["agencyCountyId"].Length > 0)
            {

                agencyCountyId = Request.QueryString["agencyCountyId"].ToDecrypt().ToInt();


            }
            var viewModel = new IVeInvoiceStatusViewModel();
            //viewModel.TitleIVeInvoiceStatusList = UtilityService.ExecStoredProcedureWithResults<TitleIVeInvoiceStatusGet_spResult>("TitleIVeInvoiceStatusGet_sp", new TitleIVeInvoiceStatusGet_spParams
            //{
            //    InvoiceYear = activityMonth.Year,
            //    InvoiceMonth = activityMonth.Month,
            //    AgencyCountyID = agencyCountyId,
            //    UserID = UserManager.UserExtended.UserID,
            //    StatusType = (Request.QueryString["EnumStatusCode"] != null && Request.QueryString["EnumStatusCode"].Length > 0) ? Request.QueryString["EnumStatusCode"] : null
            //}).ToList();

            viewModel.TitleIVeInvoiceStatusList = UtilityService.ExecStoredProcedureWithResults<TitleIVePendingInvoiceGet_spResult>("TitleIVePendingInvoiceGet_sp", new TitleIVePendingInvoiceGet_spParams
            {
                UserID = UserManager.UserExtended.UserID,
            }).ToList();

            if (viewModel.AgencyCountyID.HasValue)
                viewModel.AgencyCountyEncryptedID = viewModel.AgencyCountyID.ToEncrypt();
            else if (agencyCountyId > 0)
            {
                viewModel.AgencyCountyEncryptedID = agencyCountyId.ToEncrypt();
                viewModel.AgencyCountyID = agencyCountyId;

            }
            viewModel.ActivityMonth = activityMonth;

            //viewModel.AgencyCountyList = UtilityService.ExecStoredProcedureWithResults<TitleIVeCountyDropDown_spResult>("TitleIVeCountyDropDown_sp", new TitleIVeCountyDropDown_spParams { UserID = UserManager.UserExtended.UserID })
            //                                      .Select(x => new SelectListItem() { Text = x.AgencyCounty, Value = x.AgencyCountyID.ToEncrypt() });
            //viewModel.StatusList = UtilityFunctions.TitleIVeCodeDropDown(2004, enumValueBind: true).ToList();
            viewModel.EnumStatusCode = Request.QueryString["EnumStatusCode"];
            return View(viewModel);
        }


        #endregion
    }
}