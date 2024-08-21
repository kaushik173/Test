using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_HearingRates;
using LALoDep.Domain.Services;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using LALoDep.Core.Custom.Extensions;

namespace LALoDep.Controllers.Administration
{
    public partial class HearingRatesController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;
        public HearingRatesController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.HearingRates, PageSecurityItemID = SecurityToken.ViewHearingRates)]
        // GET: HearingRates
        public virtual ActionResult Search()
        {
            var viewModel = new HearingRatesViewModel()
            {
                OnViewLoad = true
            };
            return View(viewModel);
        }
        [HttpGet]
        public virtual JsonResult SearchHearingData()
        {
            var result = UtilityService.ExecStoredProcedureWithResults<pd_HearingRateGetLatest_spResult>(
                "pd_HearingRateGetLatest_sp",
                new pd_HearingRateGetLatest_spParams()
                {
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = new Guid()
                }).GroupBy(x => x.HearingType).Select(x =>
                {
                    var list = x.ToList();
                    return new
                    {
                        HearingType = x.Key,
                        HearingTypeID = list[0].HearingTypeID,
                        EncryptedHearingTypeID = list[0].HearingTypeID.ToEncrypt(),
                        MCO = list[0].AgencyID,
                        EncryptedMCO = list[0].AgencyID.ToEncrypt(),
                        MCOHearingRate = list[0].HearingRate,
                        EncryptedCPO = list[1].AgencyID.ToEncrypt(),
                        CPO = list[1].AgencyID,
                        CPOHearingRate = list[1].HearingRate,
                        PPO = list[2].AgencyID,
                        EncryptedPPO = list[2].AgencyID.ToEncrypt(),
                        PPOHearingRate = list[2].HearingRate,
                        CCO = list[3].AgencyID,
                        EncryptedCCO = list[3].AgencyID.ToEncrypt(),
                        CCOHearingRate = list[3].HearingRate
                    };
                }).ToList();


            return Json(new DataTablesResponse(0, result, result.Count, result.Count), JsonRequestBehavior.AllowGet);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AddEditHearingRates, PageSecurityItemID = SecurityToken.ViewHearingRates)]
        public virtual ActionResult AddEdit(string AgencyID, string HearingTypeID)
        {
            var viewModel = new HearingRatesAddEditViewModel()
            {
                OnViewLoad = true,
                AgencyID = AgencyID.ToDecrypt().ToInt(),
                HearingTypeID = HearingTypeID.ToDecrypt().ToInt()
            };

            viewModel.HearingRatesList = UtilityService.ExecStoredProcedureWithResults<pd_HearingRateGetByHearingTypeCodeIDAgencyID_spResult>("pd_HearingRateGetByHearingTypeCodeIDAgencyID_sp",new pd_HearingRateGetByHearingTypeCodeIDAgencyID_spParams()
            {
                HearingTypeCodeID = HearingTypeID.ToDecrypt().ToInt(),
                AgencyID = AgencyID.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = new Guid()
            }).Select(x => new pd_HearingRateGetByHearingTypeCodeIDAgencyID_spResult()
            {
                HearingRateID = x.HearingRateID,
                HearingRate = x.HearingRate,
                HearingRateStartDate = x.HearingRateStartDate,
                HearingRateEndDate = x.HearingRateEndDate,
                RecordStateID=x.RecordStateID,
                AgencyID=x.AgencyID,
                HearingTypeCodeID=x.HearingTypeCodeID,
                Deleted = false
            }).ToList();

            return View(viewModel);
        }

        public virtual JsonResult AddEditHearingRates(string AgencyID, string HearingTypeID)
        {
            var viewModel = UtilityService.ExecStoredProcedureWithResults<pd_HearingRateGetByHearingTypeCodeIDAgencyID_spResult>(
            "pd_HearingRateGetByHearingTypeCodeIDAgencyID_sp",
            new pd_HearingRateGetByHearingTypeCodeIDAgencyID_spParams()
            {
                HearingTypeCodeID = HearingTypeID.ToDecrypt().ToInt(),
                AgencyID = AgencyID.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = new Guid()
            }).Select(x => new pd_HearingRateGetByHearingTypeCodeIDAgencyID_spResult()
            {
                HearingRateID = x.HearingRateID,
                HearingRate = x.HearingRate,
                HearingRateStartDate = x.HearingRateStartDate,
                HearingRateEndDate = x.HearingRateEndDate,
                RecordStateID=x.RecordStateID,
                AgencyID=x.AgencyID,
                HearingTypeCodeID=x.HearingTypeCodeID,
                Deleted = false
            }).ToList();
            return Json(new DataTablesResponse(0, viewModel, viewModel.Count, viewModel.Count), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public virtual JsonResult SaveDeleteHearingRates(HearingRatesAddEditViewModel viewModel)
        {
            if (!string.IsNullOrEmpty(viewModel.StartDate) || !string.IsNullOrEmpty(viewModel.EndDate) || viewModel.HearingRate.HasValue)
            {
                //insert New data
                var pd_HearingRateInsert_spParams = new pd_HearingRateInsert_spParams()
                {
                    AgencyID = viewModel.AgencyID,
                    HearingTypeCodeID = viewModel.HearingTypeID,
                    HearingRateEntry = viewModel.HearingRate,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = new Guid()
                };
                if(!string.IsNullOrEmpty(viewModel.StartDate) || viewModel.StartDate=="")
                {
                    pd_HearingRateInsert_spParams.HearingRateStartDate = DateTime.Parse(viewModel.StartDate);
                }
                if (!string.IsNullOrEmpty(viewModel.EndDate) || viewModel.EndDate == "")
                {
                    pd_HearingRateInsert_spParams.HearingRateEndDate = DateTime.Parse(viewModel.EndDate);
                }
                var insertedRow = UtilityService.ExecStoredProcedureWithResults<decimal>("pd_HearingRateInsert_sp", pd_HearingRateInsert_spParams).SingleOrDefault();
            }
            
                //update or delete
                foreach (var item in viewModel.HearingRatesList)
                {
                    if(item.Deleted)
                    {
                        //delete row
                        var pd_HearingRateDelete_spParams = new pd_HearingRateDelete_spParams() {
                            ID = item.HearingRateID,
                            RecordStateID=10,
                            LoadOption = "HearingRate",
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = new Guid()
                        };
                        UtilityService.ExecStoredProcedureWithoutResults("pd_HearingRateDelete_sp", pd_HearingRateDelete_spParams);
                    }
                    else
                    {
                        //update row
                        var pd_HearingRateUpdate_spParams = new pd_HearingRateUpdate_spParams() {
                            HearingRateID =item.HearingRateID,
                            AgencyID = (item.AgencyID.HasValue)?item.AgencyID:viewModel.AgencyID,
                            HearingTypeCodeID = (item.HearingTypeCodeID==0)?item.HearingTypeCodeID:viewModel.HearingTypeID,
                            HearingRateEntry = item.HearingRate,
                            RecordStateID = (int)item.RecordStateID,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = new Guid()
                        };
                        if (!string.IsNullOrEmpty(item.HearingRateStartDate))
                        {
                            pd_HearingRateUpdate_spParams.HearingRateStartDate = DateTime.Parse(item.HearingRateStartDate);
                        }
                        if (!string.IsNullOrEmpty(item.HearingRateEndDate))
                        {
                            pd_HearingRateUpdate_spParams.HearingRateEndDate = DateTime.Parse(item.HearingRateEndDate);
                        }
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingRateUpdate_sp", pd_HearingRateUpdate_spParams).FirstOrDefault();
                    }
                }
            
             var returnUrl="";
             if (viewModel.ButtonID == 1)
                 returnUrl = "/" + MVC.HearingRates.Actions.Name + "/" + MVC.HearingRates.ActionNames.Search;
            else if(viewModel.ButtonID==2)
                 returnUrl = "/" + MVC.HearingRates.Actions.Name + "/" + MVC.HearingRates.ActionNames.AddEdit + "?AgencyID=" + viewModel.AgencyID.ToEncrypt() + "&HearingTypeID=" + viewModel.HearingTypeID.ToEncrypt();
            return Json(new { isSuccess = true, URL = returnUrl });
        }
    }
}