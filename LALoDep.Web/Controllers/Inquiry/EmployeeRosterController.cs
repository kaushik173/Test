using DataTables.Mvc;
using LALoDep.Domain.pd_EmployeeRoster;
using LALoDep.Domain.Services;
using LALoDep.Custom;
using LALoDep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Domain.pd_Code;

namespace LALoDep.Controllers
{
    public partial class InquiryController : Controller
    {
        // GET: /EmployeeRoster/
        public virtual ActionResult EmployeeRoster()
        {
            var viewModel = new EmployeeRosterViewModel();

            viewModel.AgencyList = UtilityService.Context.pd_JcatsGroupAgencyGetByJcatsGroupID_sp(UserManager.UserExtended.JcatsGroupID,
                                                                                              UserManager.UserExtended.UserID,
                                                                                              Guid.NewGuid()).
                                                                                              Select(x => new CodeViewModel()
                                                                                              {
                                                                                                  CodeID = x.AgencyID,
                                                                                                  CodeValue = x.AgencyName
                                                                                              }).ToList();
            var pd_CodeGetBySysValAndUserID_spParams = new pd_CodeGetBySysValAndUserID_spParams()
              {
                  SystemValueIDList = 196,
                  UserID = UserManager.UserExtended.UserID,
                  BatchLogJobID = Guid.NewGuid()
              };
            viewModel.RoleList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetBySysValAndUserID_spResults>("pd_CodeGetBySysValAndUserID_sp", pd_CodeGetBySysValAndUserID_spParams).Select(x => new CodeViewModel()
            {
                CodeID=x.CodeID,
                CodeValue=x.CodeValue
            }).ToList();


            return View(viewModel);
        }


        [HttpPost]
        public virtual ActionResult EmployeeRoster(EmployeeRosterViewModel viewModel)
        {
            var pd_EmployeeRosterSearchGet_spParams = new pd_EmployeeRosterSearchGet_spParams()
            {
                AgencyID = viewModel.AgencyID,
                StaffPositionCodeID = viewModel.StaffPositionCodeID,
                LastName = viewModel.LastName,
                FirstName = viewModel.FirstName,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            var list = UtilityService.ExecStoredProcedureWithResults<pd_EmployeeRosterSearchGet_spResults>("pd_EmployeeRosterSearchGet_sp", pd_EmployeeRosterSearchGet_spParams).Select(o => new
            {
                StaffName = o.StaffName,
                StaffPosition = o.StaffPosition,
                EmailContact = o.EmailContact,
                WorkContact = o.WorkContact,
                MobileContact = o.MobileContact,
                EncryptedPersonID = o.PersonID.ToEncrypt(),
                PersonID = o.PersonID
            }).ToList();

            var total = list.Count > 0 ? list.Count : 0;
            return Json(new DataTablesResponse(0, list, total, total), JsonRequestBehavior.AllowGet);
        }
    }
}