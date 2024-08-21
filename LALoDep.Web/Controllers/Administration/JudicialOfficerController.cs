using DataTables.Mvc;
using LALoDep.Domain.AddEditCountyCounsel;
using LALoDep.Domain.pd_CountyCounselList;
using LALoDep.Domain.pd_JudicialOfficer;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Staff;
using LALoDep.Domain.pd_Users;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using System;
using System.Linq;
using System.Web.Mvc;

namespace LALoDep.Controllers.Administration
{
    public partial class JudicialOfficerController : Controller
    {

        private IUtilityService UtilityService;
        private UserManager UserManager;
        public JudicialOfficerController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.JudicialOfficersListPage, PageSecurityItemID = SecurityToken.ViewJudicialOfficer)]
        public virtual ActionResult Search()
        {
            var viewModel = new JudicialOfficerListViewModel();

            var getAgencyParams = new pd_JcatsGroupAgencyGetByJcatsUserID_spParams();

            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupAgencyGetByJcatsUserID_spResult>(
                    "pd_JcatsGroupAgencyGetByJcatsUserID_sp", new pd_JcatsGroupAgencyGetByJcatsUserID_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() })
                    .Select(x => new SelectListItem { Value = x.AgencyID.ToString(), Text = x.AgencyName }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult Search(JudicialOfficerListViewModel model)
        {
            var searchParams = new pd_JudicialOfficerListSearchGet_spParams
            {
                AgencyID = model.AgencyID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = new Guid()
            };
            var result = UtilityService.ExecStoredProcedureWithResults<pd_JudicialOfficerListSearchGet_spResult>(
                                                    "pd_JudicialOfficerListSearchGet_sp", searchParams).Select(x => new
                                                    {
                                                        x.PersonNameFirst,
                                                        x.PersonNameLast,
                                                        x.RoleStartDate,
                                                        x.RoleEndDate,
                                                        PersonID = x.PersonID.ToEncrypt()
                                                    }).ToList();
            return Json(new DataTablesResponse(0, result, result.Count, result.Count));
        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.JudicialOfficersListPage, PageSecurityItemID = SecurityToken.DeleteJudicialOfficer)]
        public virtual JsonResult DeleteJudicialOfficer(string id)
        {
            UtilityService.ExecStoredProcedureWithResults<object>("pd_StaffDelete_sp", new pd_StaffDelete_spParams
            {
                ID = id.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = new Guid(),
                RecordStateID = 10
            }).FirstOrDefault();
            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.UsersEdit, PageSecurityItemID = SecurityToken.EditUser)]
        public virtual ActionResult AddEdit(string id)
        {
            var viewModel = new JudicialOfficerViewModel();
            if (!string.IsNullOrEmpty(id))
            {
                viewModel.PersonID = id.ToDecrypt().ToInt();
                var result = UtilityService.ExecStoredProcedureWithResults<pd_StaffGetByPersonID_spResult>("pd_StaffGetByPersonID_sp",
                                    new pd_StaffGetByPersonID_spParams { PersonID = viewModel.PersonID ?? 0, UserID = UserManager.UserExtended.UserID }).FirstOrDefault();

                if (result != null)
                {
                    viewModel.FirstName = result.PersonNameFirst;
                    viewModel.LastName = result.PersonNameLast;
                    viewModel.StartDate = result.RoleStartDate.HasValue ? result.RoleStartDate.Value.ToString("MM/dd/yyyy") : null;
                    viewModel.EndDate = result.RoleEndDate.HasValue ? result.RoleEndDate.Value.ToString("MM/dd/yyyy") : null;
                }
            }

            var agenciesParams = new pd_AgencyGetSystemRoleByPersonID_spParams
            {
                PersonID = viewModel.PersonID,
                UserID = UserManager.UserExtended.UserID,
                SortFlag = 0
            };

            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<pd_AgencyGetSystemRoleByPersonID_spResult>("pd_AgencyGetSystemRoleByPersonID_sp", agenciesParams).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult AddEditSave(JudicialOfficerViewModel viewModel)
        {
            DateTime? dtStartDate = null;
            if (!string.IsNullOrEmpty(viewModel.StartDate))
                dtStartDate = DateTime.Parse(viewModel.StartDate);

            DateTime? dtEndDate = null;
            if (!string.IsNullOrEmpty(viewModel.EndDate))
                dtEndDate = DateTime.Parse(viewModel.EndDate);

            if (!viewModel.PersonID.HasValue)
            {
                // Add person
                viewModel.PersonID = UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonInsert_sp", new pd_PersonInsert_spParams
                {
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).FirstOrDefault();

                //add person name
                UtilityService.ExecStoredProcedureWithResults<object>("pd_PersonNameInsert_sp", new pd_PersonNameInsert_spParams
                {
                    PersonID = viewModel.PersonID.Value,
                    PersonNameFirst = viewModel.FirstName,
                    PersonNameLast = viewModel.LastName,
                    PersonNameTypeCodeID = 3200,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    PersonNameSalutationCodeID = -1,
                    PersonNameSuffixCodeID = -1,
                    BatchLogJobID = Guid.NewGuid()
                }).FirstOrDefault();
            }
            else
            {
                var personInfo = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", new pd_PersonGet_spParams
                {
                    PersonID = viewModel.PersonID.Value,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID

                }).FirstOrDefault();

                if (viewModel.FirstName != personInfo.FirstName || viewModel.LastName != personInfo.LastName)
                {
                    UtilityService.ExecStoredProcedureWithResults<object>("pd_PersonNameUpdate_sp", new pd_PersonNameUpdate_spParams
                    {
                        PersonNameID = personInfo.PersonNameID.Value,
                        PersonID = personInfo.PersonID,
                        PersonNameFirst = viewModel.FirstName,
                        PersonNameLast = viewModel.LastName,
                        PersonNameTypeCodeID = personInfo.PersonNameTypeCodeID ?? 3200,
                        RecordStateID = personInfo.RecordStateID,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        PersonNameSalutationCodeID = -1,
                        PersonNameSuffixCodeID = -1
                    }).FirstOrDefault();
                }
            }

            foreach (var agency in viewModel.AgencyList)
            {
                if (agency.Selected == 1 && agency.RoleID.HasValue) //Update agency
                {
                    UtilityService.ExecStoredProcedureWithResults<object>("pd_UserRoleUpdate_sp", new pd_UserRoleUpdate_spParams
                    {
                        RoleID = agency.RoleID.Value,
                        RoleTypeCodeID = 7,
                        AgencyID = agency.AgencyID,
                        RoleStartDate = viewModel.StartDate,
                        RoleEndDate = viewModel.EndDate,
                        RecordStateID = (int)agency.RecordStateID,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID

                    }).FirstOrDefault();
                }
                else if (agency.Selected == 1) //Add agency
                {
                    UtilityService.ExecStoredProcedureWithResults<object>("pd_UserRoleInsert_sp", new pd_UserRoleInsert_spParams
                    {
                        PersonID = viewModel.PersonID ?? 0,
                        RoleTypeCodeID = 7,
                        RoleAgencyID = agency.AgencyID,
                        RoleStartDate = dtStartDate,
                        RoleEndDate = dtEndDate,
                        RecordStateID = 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();
                }
                else if (agency.Selected == 0 && agency.RoleID.HasValue)//Delete
                {
                    UtilityService.ExecStoredProcedureWithResults<object>("pd_RoleDelete_sp", new pd_RoleDelete_spParams
                    {
                        ID = agency.RoleID.Value,
                        RecordStateID = 10,
                        LoadOption = "Role",
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                        
                    }).FirstOrDefault();
                }
            }

            return Json(new { isSuccess = true });
        }
    }
}