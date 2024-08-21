using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.Services;
using LALoDep.Core.Enums;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using LALoDep.Core.Custom.Extensions;
using Omu.ValueInjecter;
using LALoDep.Domain;
using LALoDep.Domain.pd_System;
using LALoDep.Controllers.Administration;
using LALoDep.Domain.pd_Code;

namespace LALoDep.Controllers.Administration
{
    public partial class AdministrationController : Controller
    {
        // GET: /SystemValue/
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.SystemValuePage, PageSecurityItemID = SecurityToken.ViewSystemValue)]
        public virtual ActionResult SystemValues()
        {
            var viewModel = new AdminSystemValueViewModel();
            var CodeType = GetSyatemValueList();
            viewModel.CodeTypeFilter = CodeType.Select(c => new SystemValueViewModel()
            {
                EncryptedSystemValueTypeCodeTypeID = c.SystemValueTypeCodeTypeID.ToEncrypt(),
                CodeTypeValue = c.CodeTypeValue,
                SystemValueTypeCodeTypeID = c.SystemValueTypeCodeTypeID
            }).GroupBy(x => x.SystemValueTypeCodeTypeID).Select(g => g.First()).ToList();
            return View(viewModel);
        }


        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.SystemValuePage, PageSecurityItemID = SecurityToken.ViewSystemValue)]
        public virtual JsonResult SystemLValueList(string id)
        {
            var sysValueList = GetSyatemValueList();
            var systemValueTypeCodeTypeID = id.ToDecrypt().ToInt();

            var list = sysValueList.Where(x => string.IsNullOrEmpty(id) || x.SystemValueTypeCodeTypeID == systemValueTypeCodeTypeID)
                .Select(x => new
            {
                SystemValueTypeID = x.SystemValueTypeID,
                SystemValue = x.SystemValue,
                EncryptedSystemValueTypeID = x.SystemValueTypeID.ToEncrypt(),
                SystemValueTypeCodeTypeID = x.SystemValueTypeCodeTypeID,
                EncryptedSystemValue = x.SystemValue.ToEncrypt(),
                CanEditFlag = x.CanEditFlag
            }).ToList();
            
            var total = list.Count;
            return Json(new DataTablesResponse(0, list, total, total), JsonRequestBehavior.AllowGet);
        }

        public List<pd_SystemValueTypeGetAll_spResult> GetSyatemValueList()
        {
            var pd_SystemValueTypeGetAll_spParams = new pd_SystemValueTypeGetAll_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var sysValueList = UtilityService.ExecStoredProcedureWithResults<pd_SystemValueTypeGetAll_spResult>("pd_SystemValueTypeGetAll_sp", pd_SystemValueTypeGetAll_spParams).OrderBy(m => m.CodeTypeValue).Select(c => (pd_SystemValueTypeGetAll_spResult)(new pd_SystemValueTypeGetAll_spResult()).InjectFrom(c)).ToList();
            return sysValueList;
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.SystemValuePage, PageSecurityItemID = SecurityToken.UpdateSystemValue)]
        public virtual ActionResult SystemValuesAddEdit(string id, string systemValueTypeEntry)
        {

            var viewModel = new AdminSystemValueUpdateViewModel();
            var systemValID=id.ToDecrypt().ToInt(); 
            viewModel.SystemValueTypeID = systemValID;
            if (!string.IsNullOrEmpty(systemValueTypeEntry))
            viewModel.SystemValueTypeEntry = systemValueTypeEntry.ToDecrypt();
            var pd_CodeGetBySystemValueTypeID_spParams = new pd_CodeGetBySystemValueTypeID_spParams()
            {
                SystemValueIDList = systemValID.ToString(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                IncludeCodeID = -88,
            };
            var syatemValList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetBySystemValueTypeID_spResults>("pd_CodeGetBySystemValueTypeID_sp", pd_CodeGetBySystemValueTypeID_spParams).Select(x => new SystemValueListViewModel()
            {
                CodeID=x.CodeID,
                CodeValue=x.CodeValue,
                SystemValueID=x.SystemValueID,
                Selected=x.Selected==1,
                SortSeq=x.SortSeq,
                SystemValueSequence=x.SystemValueSequence,
                RecordStateID = x.RecordStateID,
                ActiveAgencyCodeFlag = x.ActiveAgencyCodeFlag
            }).ToList();
            viewModel.SytemsValueUpdateList = syatemValList;
            ViewBag.SelectedCounts = syatemValList.Where(x => x.Selected == true).Select(x => x.Selected).Count();
            return View(viewModel);
        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.SystemValuePage, PageSecurityItemID = SecurityToken.UpdateSystemValue)]
        public virtual JsonResult SystemValuesAddEdit(AdminSystemValueUpdateViewModel viewModel)
        {
            foreach (var item in viewModel.SytemsValueUpdateList)
            {
                if (item.Insert)
                {
                    var pd_SystemValueInsert_spParams= new pd_SystemValueInsert_spParams()
                    {

                        SystemValueTypeID = viewModel.SystemValueTypeID,
                        SystemValueCodeID = item.CodeID,
                        SystemValueSequence = item.SystemValueSequence != null ? (item.SystemValueSequence) : null,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    var _InsertObj = UtilityService.ExecStoredProcedureWithResults<decimal>("pd_SystemValueInsert_sp", pd_SystemValueInsert_spParams).SingleOrDefault();

                }
                else if (item.Update)
                {
                    var pd_SystemValueUpdate_spParams = new pd_SystemValueUpdate_spParams()
                    {
                        SystemValueID = item.SystemValueID,
                        SystemValueTypeID = viewModel.SystemValueTypeID,
                        SystemValueCodeID = item.CodeID,
                        SystemValueSequence = item.SystemValueSequence != null ? item.SystemValueSequence : null,
                        RecordStateID = item.RecordStateID ?? 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    List<object> _UpdateObj = UtilityService.ExecStoredProcedureWithResults<object>("pd_SystemValueUpdate_sp", pd_SystemValueUpdate_spParams).ToList();

                }
                else if (item.Delete)
                {
                    var pd_SystemValueDelete_spParams = new pd_SystemValueDelete_spParams()
                    {
                        ID = item.SystemValueID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        RecordStateID=10,
                        LoadOption = "SystemValue"
                    };
                    List<object> _DeleteObj = UtilityService.ExecStoredProcedureWithResults<object>("pd_SystemValueDelete_sp", pd_SystemValueDelete_spParams).ToList();

                }
            }
            string URL = string.Empty;
            if (viewModel.ButtonID == 1)
            {
                URL = MVC.Administration.Name + "/" + MVC.Administration.ActionNames.SystemValues;
            }
            else if (viewModel.ButtonID == 2)
            {
                URL = MVC.Administration.Name + "/" + MVC.Administration.ActionNames.SystemValuesAddEdit + "/" + viewModel.SystemValueTypeID.ToEncrypt() + "?systemValueTypeEntry=" + viewModel.SystemValueTypeEntry.ToEncrypt();
            }
            else { URL = ""; }
            return Json(new { URL = URL }, JsonRequestBehavior.AllowGet);
        }
    }
}