using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Calendar;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Inquiry;
using LALoDep.Models;
using LALoDep.Domain.pd_CaseLoad;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class InquiryController : Controller
    {

        // GET: CaseloadManagement
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseLoadManagementPage, PageSecurityItemID = SecurityToken.ViewCaseLoadManagement)]
        public virtual ActionResult CaseloadManagement()
        {
            var ViewModel = new CaseLoadManagementViewModel();
            ViewModel.AgencyList = UtilityService.Context.pd_AgencyGetHomeByPersonID_sp(UserManager.UserExtended.PersonID, UserManager.UserExtended.UserID, Guid.NewGuid())
                                                .Select(x => new AgencyModel() { AgencyName = x.AgencyName, AgencyID = x.AgencyID, Selected = x.Selected.HasValue ? x.Selected.Value : 0 }).OrderBy(o=>o.AgencyName).ToList();
            var selected = ViewModel.AgencyList.Where(o => o.Selected == 1).FirstOrDefault();
            if (ViewModel.AgencyList.Count() == 1)
                ViewModel.AgencyID = ViewModel.AgencyList.FirstOrDefault().AgencyID;
            else if (selected != null)
            {
                ViewModel.AgencyID = selected.AgencyID;
            }

                return View(ViewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseLoadManagementPage, PageSecurityItemID = SecurityToken.ViewCaseLoadManagement)]
        [HttpPost]
        public virtual PartialViewResult CaseloadManagement(CaseLoadManagementViewModel model)
        {
            var spParams = new pd_CaseloadGetByStaff1_spParams()
            {

                RoleTypeCodeID = -99,
                AgencyID = model.AgencyID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),

            };

            var data = UtilityService.ExecStoredProcedureWithResults<pd_CaseloadGetByStaff1_spResults>("pd_CaseloadGetByStaff1_sp", spParams).ToList();
            return PartialView("_caseloadSearchresult", data); ;
        }
    }
}