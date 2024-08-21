using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain;
using LALoDep.Domain.AddEditCountyCounsel;
using LALoDep.Domain.com_Jcats;
using LALoDep.Domain.pd_Address;
using LALoDep.Domain.PD_JcatsUser;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Users;
using LALoDep.Domain.pd_Users.Edit;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models;
using LALoDep.Models.Administration;
using LALoDep.Models.Case;
using Microsoft.Owin.Security;
using pd_AgencyGetSystemRoleByPersonID_spParams = LALoDep.Domain.pd_Users.Edit.pd_AgencyGetSystemRoleByPersonID_spParams;
using pd_AgencyGetSystemRoleByPersonID_spResult = LALoDep.Domain.pd_Users.Edit.pd_AgencyGetSystemRoleByPersonID_spResult;
using pd_CodeGetBySysValAndUserID_spParams = LALoDep.Domain.pd_Users.pd_CodeGetBySysValAndUserID_spParams;
using pd_CodeGetBySysValAndUserID_spResult = LALoDep.Domain.pd_Users.pd_CodeGetBySysValAndUserID_spResult;
using pd_JcatsGroupGetByUserID_spParams = LALoDep.Domain.pd_Users.pd_JcatsGroupGetByUserID_spParams;
using pd_JcatsGroupGetByUserID_spResult = LALoDep.Domain.pd_Users.pd_JcatsGroupGetByUserID_spResult;
using LALoDep.Core.Enums;
using LALoDep.Domain.pd_Department;
using Omu.ValueInjecter;
using LALoDep.Domain.TitleIVe;
using LALoDep.Domain.JcatsESignature;
using LALoDep.Domain.pd_CodeTables;

namespace LALoDep.Controllers.Administration
{
    public partial class UsersController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;
        public UsersController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.UsersPage, PageSecurityItemID = SecurityToken.UsersPage)]
        public virtual ActionResult Search()
        {
            UsersViewModel viewModel = new UsersViewModel
            {
                OnViewLoad = true,
                ActiveUserOnly = true,//defalut active user checked
            };

            viewModel.Agencies = UtilityService
                .ExecStoredProcedureWithResults<pd_JcatsGroupAgencyGetByJcatsUserID_spResult>(
                    "pd_JcatsGroupAgencyGetByJcatsUserID_sp", new pd_JcatsGroupAgencyGetByJcatsUserID_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).Select(x => new SelectListItem()
                    {
                        Text = x.AgencyName,
                        Value = x.AgencyID.ToString()
                    });
            if (viewModel.Agencies.Count() == 1)
            {
                viewModel.AgencyID = viewModel.Agencies.First().Value;
            }

            viewModel.SecurityGroups = UtilityService
                .ExecStoredProcedureWithResults<pd_JcatsGroupGetByUserID_spResult>(
                    "pd_JcatsGroupGetByUserID_sp", new pd_JcatsGroupGetByUserID_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()

                    }).Select(x => new SelectListItem()
                    {
                        Text = x.JcatsGroupName,
                        Value = x.JcatsGroupID.ToString()
                    });

            viewModel.Roles = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetBySysValAndUserID_spResult>(
                "pd_CodeGetBySysValAndUserID_sp", new pd_CodeGetBySysValAndUserID_spParams()
                {
                    SystemValueIDList = "196",
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).Select(x => new SelectListItem()
                {
                    Text = x.CodeValue,
                    Value = x.CodeID.ToString()
                });
            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult Search(UsersViewModel model)
        {
            var searchParams = new pd_StaffUserSearchGet_spParams
            {
                AgencyID = model.AgencyID != null ? Convert.ToInt32(model.AgencyID) : (int?)null,
                FirstName = model.FirstName,
                LastName = model.LastName,
                JcatsGroupID = model.JcatsGroupID != null ? Convert.ToInt32(model.JcatsGroupID) : (int?)null,
                RoleTypeCodeID = model.RoleTypeCodeID != null ? Convert.ToInt32(model.RoleTypeCodeID) : (int?)null,
                ActiveUserOnly = model.ActiveUserOnly ? (byte)1 : (byte)0,
                OpenPositionsOnly = model.OpenPositionsOnly ? (byte)1 : (byte)0,
                SortOption = null,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var result =
                UtilityService.ExecStoredProcedureWithResults<pd_StaffUserSearchGet_spResult>(
                    "pd_StaffUserSearchGet_sp", searchParams).Select(x => new
                    {
                        Name = x.PersonNameLast + ", " + x.PersonNameFirst,
                        Agency = x.AgencyName,
                        SecurityGroup = x.JcatsGroupName,
                        Role = x.RoleType,
                        UserLevel = x.UserLevel,
                        LastLogin = x.LastLoginDate,
                        UserEndDate = x.UserEndDate,
                        LoginName = x.JcatsUserLoginName.IsNullOrEmpty() ? "Edit" : x.JcatsUserLoginName,
                        PersonID = x.PersonID.ToEncrypt(),
                        JcatsUserID = x.JcatsUserID.ToEncrypt(),
                        JcatsGroupID = x.JcatsGroupID.HasValue ? x.JcatsGroupID.ToEncrypt() : ""
                    }).OrderBy(o => o.Name).ToList();
            return Json(new DataTablesResponse(0, result, result.Count, result.Count));
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.UsersEdit, PageSecurityItemID = SecurityToken.ViewUser)]
        public virtual ActionResult AddEdit(string jcatsUserID, string personID)
        {
            var jcatsId = 0;
            var PersonId = 0;
            if (!string.IsNullOrEmpty(jcatsUserID))
                jcatsId = jcatsUserID.ToDecrypt().ToInt();
            if (!string.IsNullOrEmpty(personID))
                PersonId = personID.ToDecrypt().ToInt();

            UserAddEditViewModel viewModel = new UserAddEditViewModel
            {
                OnViewLoad = true,
                JcatsUserID = jcatsId,//jcatsUserID.ToDecrypt().ToInt(),
                PersonID = PersonId//personID.ToDecrypt().ToInt()
            };
            if (Request.QueryString["lastname"] != null || Request.QueryString["FirstName"] != null)
            {
                viewModel.JcatsUserData = new pd_JCATSUserGet_spData();

                viewModel.JcatsUserData.PersonNameLast = Request.QueryString["lastname"];
                viewModel.JcatsUserData.PersonNameFirst = Request.QueryString["FirstName"];
            }
            if (!string.IsNullOrEmpty(jcatsUserID))
            {
                viewModel.JcatsUserData =
                    UtilityService.ExecStoredProcedureWithResults<pd_JCATSUserGet_spResult>(
                        "pd_JCATSUserGet_sp", new pd_JCATSUserGet_spParams
                        {
                            JcatsUserID = jcatsId, //jcatsUserID.ToDecrypt().ToInt(),
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        }).Select(x => new pd_JCATSUserGet_spData
                        {
                            PersonNameID = x.PersonNameID,
                            AgencyID = x.AgencyID,
                            JcatsGroupID = x.JcatsGroupID,
                            JcatsUserStartDate = x.JcatsUserStartDate,
                            JcatsUserEndDate = x.JcatsUserEndDate,
                            PersonNameLast = x.PersonNameLast,
                            PersonNameFirst = x.PersonNameFirst,
                            PersonNameSuffixCodeID = x.PersonNameSuffixCodeID,
                            PersonNameSalutationCodeID = x.PersonNameSalutationCodeID,
                            RoleTypeCodeID = x.RoleTypeCodeID,
                            JcatsUserLevelCodeID = x.JcatsUserLevelCodeID,
                            JcatsUserLoginName = x.JcatsUserLoginName,
                            JcatsUserPassword = x.JcatsUserPassword,
                            JcatsUserID = x.JcatsUserID,
                            PersonID = x.PersonID
                        }).FirstOrDefault();



                if (viewModel.JcatsUserData == null)
                {
                    var person = UtilityService.ExecStoredProcedureWithResults<pd_JcatsUserGetByPersonID_spResult>(
                 "pd_JcatsUserGetByPersonID_sp", new pd_JcatsUserGetByPersonID_spParams
                 {
                     PersonID = PersonId, //personID.ToDecrypt().ToInt(),
                     UserID = UserManager.UserExtended.UserID,
                     BatchLogJobID = Guid.NewGuid()
                 }).FirstOrDefault();
                    if (person != null)
                    {
                        viewModel.JcatsUserData = new pd_JCATSUserGet_spData();
                        viewModel.JcatsUserData.PersonNameFirst = person.PersonNameFirst;
                        viewModel.JcatsUserData.PersonNameLast = person.PersonNameLast;
                        viewModel.JcatsUserData.RoleTypeCodeID = person.RoleTypeCodeID;
                        viewModel.JcatsUserData.PersonNameID = person.PersonNameID;
                    }
                }
                if (viewModel.JcatsUserData != null)
                {
                    var timeOut = UtilityService.ExecQueryScalerADO("select JcatsUserTimeOut from JcatsUser where JcatsUserID=" + jcatsId.ToString());
                    if (timeOut != null)
                    {
                        if (timeOut.ToInt() > 0)
                            viewModel.JcatsUserData.JcatsUserTimeOut = timeOut.ToInt();
                    }
                }

            }

            if (!string.IsNullOrEmpty(personID))
            {
                viewModel.StaffInfo = UtilityService.ExecStoredProcedureWithResults<pd_StaffInfoGetByPersonID_spResult>("pd_StaffInfoGetByPersonID_sp", new pd_StaffInfoGetByPersonID_spParams
                {
                    PersonID = PersonId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).Select(x => new StaffInfo
                {
                    StaffInfoID = x.StaffInfoID,
                    StaffInfoBarNumber = x.StaffInfoBarNumber,
                    StaffInfoComment = x.StaffInfoComment,
                    EmailPrimary = x.EmailPrimary,
                    EmailSecondary = x.EmailSecondary,
                    Fax = x.Fax,
                    MobilePhone = x.MobilePhone,
                    WorkPhone = x.WorkPhone,
                    StaffInfoBarAdmittedDate = x.StaffInfoBarAdmittedDate,
                    StaffInfoEligibilityEffectiveDate = x.StaffInfoEligibilityEffectiveDate,
                    StaffInfoEligibilityEndingDate = x.StaffInfoEligibilityEndingDate,
                    EmailToPrimaryPersonContactID = x.EmailToPrimaryPersonContactID,
                    EmailToSecondaryPersonContactID = x.EmailToSecondaryPersonContactID,
                    EmailToAlternatePersonContactFlag = x.EmailToAlternatePersonContactFlag == 1,
                    AlternateContactPersonID = x.AlternateContactPersonID,
                    StaffInfoEmployeeStatusCodeID = x.StaffInfoEmployeeStatusCodeID,
                    FaxPersonContactID = x.FaxPersonContactID,
                    MobilePersonContactID = x.MobilePhonePersonContactID,
                    WorkPersonContactID = x.WorkPhonePersonContactID,
                    EmailPrimaryPersonContactID = x.EmailPrimaryPersonContactID,
                    EmailSecondaryPersonContactID = x.EmailSecondaryPersonContactID,
                    UsePrimaryEmail = x.EmailToPrimaryPersonContactID > 0,
                    UseSecondaryEmail = x.EmailToSecondaryPersonContactID > 0,
                    StaffInfoEmployeeID = x.StaffInfoEmployeeID
                }).FirstOrDefault();

                viewModel.TitleIVeStaffInfo = UtilityService.ExecStoredProcedureWithResults<TitleIVeStaffGet_spResult>("TitleIVeStaffGet_sp", new TitleIVeStaffGet_spParams
                {
                    PersonID = PersonId,
                    UserID = UserManager.UserExtended.UserID,

                }).FirstOrDefault();
                if (viewModel.TitleIVeStaffInfo != null)
                {

                    viewModel.IsEmployee = viewModel.TitleIVeStaffInfo.IsEmployee.ToBoolean();
                }
            }

            viewModel.SupervisorList = UtilityService.ExecStoredProcedureWithResults<Supervisor_GetList_spResult>("Supervisor_GetList_sp", new Supervisor_GetList_spParams
            {
                StaffPersonID = PersonId,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).ToList();
            viewModel.SupervisorPersonID = viewModel.SupervisorList.FirstOrDefault(x => x.Selected == 1)?.PersonID;


            viewModel.Groups =
                UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupGetByUserID_spResult>(
                    "pd_JcatsGroupGetByUserID_sp", new pd_JcatsGroupGetByUserID_spParams
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        IncludeJcatsGroupID = viewModel.JcatsUserData?.JcatsGroupID
                    }).Select(x => new SelectListItem
                    {
                        Text = x.JcatsGroupName,
                        Value = x.JcatsGroupID.ToString()
                    }).ToList();


            viewModel.Roles =
                UtilityService.ExecStoredProcedureWithResults<pd_CodeGetBySysValAndUserID_spResult>(
                    "pd_CodeGetBySysValAndUserID_sp",
                    new LALoDep.Domain.pd_Users.Edit.pd_CodeGetBySysValAndUserID_spParams
                    {
                        SystemValueIDList = "196",
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        IncludeCodeID = (viewModel.JcatsUserData != null) ? viewModel.JcatsUserData.RoleTypeCodeID.HasValue ? viewModel.JcatsUserData.RoleTypeCodeID.Value : 0 : 0
                    }).Select(x => new SelectListItem
                    {
                        Text = x.CodeValue,
                        Value = x.CodeID.ToString()
                    }).ToList();

            viewModel.AlternateContacts =
                UtilityService.ExecStoredProcedureWithResults<pd_StaffInfoGetAlternateContacts_spResult>(
                    "pd_StaffInfoGetAlternateContacts_sp", new pd_StaffInfoGetAlternateContacts_spParams
                    {
                        PersonID = PersonId,//personID.ToDecrypt().ToInt(),
                        IncludePersonID = 0,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).Select(x => new SelectListItem
                    {
                        Text = x.PersonNameDisplay,
                        Value = x.PersonID.ToString()
                    }).ToList();

            viewModel.SalutationList = UtilityFunctions.CodeGetByTypeIdAndUserId(200, includeCodeId: (viewModel.JcatsUserData != null) ? viewModel.JcatsUserData.PersonNameSalutationCodeID.HasValue ? viewModel.JcatsUserData.PersonNameSalutationCodeID.Value : 0 : 0);
            viewModel.SuffixList = UtilityFunctions.CodeGetByTypeIdAndUserId(201, includeCodeId: (viewModel.JcatsUserData != null) ? viewModel.JcatsUserData.PersonNameSuffixCodeID.HasValue ? viewModel.JcatsUserData.PersonNameSuffixCodeID.Value : 0 : 0);
            viewModel.UserLevelList = UtilityFunctions.CodeGetByTypeIdAndUserId(300, includeCodeId: (viewModel.JcatsUserData != null) ? viewModel.JcatsUserData.JcatsUserLevelCodeID.HasValue ? viewModel.JcatsUserData.JcatsUserLevelCodeID.Value : 0 : 0);
            viewModel.EmployeeStatusList = UtilityFunctions.CodeGetByTypeIdAndUserId(970, includeCodeId: (viewModel.StaffInfo != null) ? viewModel.StaffInfo.StaffInfoEmployeeStatusCodeID.HasValue ? viewModel.StaffInfo.StaffInfoEmployeeStatusCodeID.Value : 0 : 0);
            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupAgencyGetByJcatsUserID_spResult>(
                         "pd_JcatsGroupAgencyGetByJcatsUserID_sp", new pd_JcatsGroupAgencyGetByJcatsUserID_spParams
                         {
                             UserID = UserManager.UserExtended.UserID,
                             BatchLogJobID = Guid.NewGuid()
                         }).Select(x => new SelectListItem
                         {
                             Value = x.AgencyID.ToString(),
                             Text = x.AgencyName
                         }).ToList();

            var selectedAgencies = UtilityService.ExecStoredProcedureWithResults<pd_AgencyGetSystemRoleByPersonID_spResult>(
                   "pd_AgencyGetSystemRoleByPersonID_sp", new pd_AgencyGetSystemRoleByPersonID_spParams
                   {
                       PersonID = personID.ToDecrypt().ToInt(),
                       UserID = UserManager.UserExtended.UserID,
                       BatchLogJobID = Guid.NewGuid(),
                       SortFlag = 1
                   }).Where(o => o.Selected == 1 && o.RecordStateID.HasValue && o.RecordStateID.Value == 1).ToList();
            selectedAgencies.Add(new pd_AgencyGetSystemRoleByPersonID_spResult()
            {
                AgencyID = 0,
                RecordStateID = 1,

            });

            viewModel.SelectedAgencyList = selectedAgencies;

            viewModel.HomeAgencyID = UtilityService.ExecStoredProcedureWithResults<pd_AgencyGetHomeByPersonID_spResult>(
                    "pd_AgencyGetHomeByPersonID_sp", new pd_AgencyGetHomeByPersonID_spParams
                    {
                        PersonID = PersonId,//personID.ToDecrypt().ToInt(),
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                    }).Where(o => o.HomeAgencyFlag == 1).Select(o => o.AgencyID).FirstOrDefault();

            viewModel.OHCodeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndNotUserID_spResult>("TitleIVeOverheadCodeDropDown_sp", new TitleIVeOverheadCodeDropDown_spParams
            {
                PersonID = PersonId,
                UserID = UserManager.UserExtended.UserID
            }).Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() }).ToList();

            return View(viewModel);
        }
        [HttpPost]
        public virtual JsonResult AddEdit(UserAddEditViewModel model)
        {
            if (!model.JcatsUserData.JcatsUserLoginName.IsNullOrEmpty())
            {
                var user = UtilityService.ExecStoredProcedureWithResults<pd_JcatsUserGetByJcatsUserLoginName_spResult>("pd_JcatsUserGetByJcatsUserLoginName_sp",
                        new pd_JcatsUserGetByJcatsUserLoginName_spParams
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID,
                            ExcludeCurrentJcatsUserID = model.JcatsUserData.JcatsUserID,
                            JcatsUserLoginName = model.JcatsUserData.JcatsUserLoginName
                        }).FirstOrDefault();
                if (user != null)
                {
                    return Json(new { Status = "Fail", Message = "Login name " + model.JcatsUserData.JcatsUserLoginName + " already exists" });

                }
            }

            if (model.JcatsUserData.JcatsUserID > 0 && !model.JcatsUserData.JcatsUserPassword.IsNullOrEmpty())
            {
                var pwdStatus = UtilityService.ExecStoredProcedureWithResults<NG_pd_ChangePassword_spResult>("NG_pd_ChangePassword_sp",
                         new NG_pd_ChangePassword_spParams
                         {
                             BatchLogJobID = Guid.NewGuid(),
                             UserID = UserManager.UserExtended.UserID,
                             ChangeUserID = model.JcatsUserData.JcatsUserID,
                             NewPassword = model.JcatsUserData.JcatsUserPassword,
                             ResetFlag = 1
                         }).FirstOrDefault();
                if (pwdStatus != null)
                {
                    if (pwdStatus.Status != "SUCCESS")
                    {
                        return Json(new { Status = "Fail", Message = pwdStatus.StatusMessage });
                    }
                    else
                    {
                        if (!UserManager.UpdatePassword(model.JcatsUserData.JcatsUserLoginName,
                               model.JcatsUserData.JcatsUserPassword, 1))
                        {
                            return Json(new { Status = "Fail", Message = "Password Not Reset Successfully" });

                        }

                    }
                }
            }

            //var agencyId = model.SelectedAgencyList.Where(o => o.Selected == 1).Select(o => o.AgencyID).FirstOrDefault();
            //if (agencyId == 0)
            //{
            //    agencyId = model.SelectedAgencyList.Select(o => o.AgencyID).FirstOrDefault();
            //}

            var personId = model.JcatsUserData.PersonID;
            if (model.JcatsUserData.PersonID == 0)
            {


                personId =
                    UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonInsert_sp",
                        new pd_PersonInsert_spParams()
                        {
                            //AgencyID = agencyId,
                            AgencyID = model.JcatsUserData.AgencyID,
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID,
                            RecordStateID = 1,
                        }).FirstOrDefault();
                model.JcatsUserData.PersonID = personId;
                var personNameId =
                    UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonNameInsert_sp",
                        new pd_PersonNameInsert_spParams()
                        {
                            //AgencyID = agencyId,
                            AgencyID = model.JcatsUserData.AgencyID,
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID,
                            RecordStateID = 1,
                            PersonID = personId,
                            PersonNameFirst = model.JcatsUserData.PersonNameFirst,
                            PersonNameLast = model.JcatsUserData.PersonNameLast,
                            PersonNameTypeCodeID = 3200,
                            PersonNameSalutationCodeID = model.JcatsUserData.PersonNameSalutationCodeID.HasValue ? model.JcatsUserData.PersonNameSalutationCodeID.Value : 0,
                            PersonNameSuffixCodeID = model.JcatsUserData.PersonNameSuffixCodeID.HasValue ? model.JcatsUserData.PersonNameSuffixCodeID.Value : 0,


                        }).FirstOrDefault();
            }
            else if (model.JcatsUserData.PersonNameID.HasValue)
            {
                UtilityService.ExecStoredProcedureWithoutResults("pd_PersonNameUpdate_sp",
                    new pd_PersonNameUpdate_spParams()
                    {
                        //AgencyID = agencyId,
                        AgencyID = model.JcatsUserData.AgencyID,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        RecordStateID = 1,
                        PersonID = personId,
                        PersonNameFirst = model.JcatsUserData.PersonNameFirst,
                        PersonNameLast = model.JcatsUserData.PersonNameLast,
                        PersonNameTypeCodeID = 3200,
                        PersonNameSalutationCodeID = model.JcatsUserData.PersonNameSalutationCodeID.HasValue ? model.JcatsUserData.PersonNameSalutationCodeID.Value : 0,
                        PersonNameSuffixCodeID = model.JcatsUserData.PersonNameSuffixCodeID.HasValue ? model.JcatsUserData.PersonNameSuffixCodeID.Value : 0,
                        PersonNameID = model.JcatsUserData.PersonNameID.Value
                    });

            }

            if (model.JcatsUserData.JcatsUserID == 0 && !string.IsNullOrEmpty(model.JcatsUserData.JcatsUserLoginName))
            {
                var userid = UtilityService.ExecStoredProcedureWithResults<decimal>("pd_JcatsUserInsert_sp", new pd_JcatsUserInsert_spParams()
                {
                    //AgencyID = agencyId,
                    AgencyID = model.JcatsUserData.AgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    JcatsGroupID = model.JcatsUserData.JcatsGroupID,
                    JcatsUserLoginName = model.JcatsUserData.JcatsUserLoginName,
                    JcatsUserPassword = model.JcatsUserData.JcatsUserPassword,
                    PersonID = personId,
                    JcatsUserStartDate = model.JcatsUserData.JcatsUserStartDate.IsNullOrEmpty() ? DateTime.Now : model.JcatsUserData.JcatsUserStartDate.ToDateTime(),
                    JcatsUserEndDate = model.JcatsUserData.JcatsUserEndDate.IsNullOrEmpty() ? (DateTime?)null : model.JcatsUserData.JcatsUserEndDate.ToDateTime(),
                    RecordStateID = 1,


                }).FirstOrDefault();

                model.JcatsUserData.JcatsUserID = (int)userid;

                UserManager.UpdatePassword(model.JcatsUserData.JcatsUserLoginName, model.JcatsUserData.JcatsUserPassword);
            }
            else if (model.JcatsUserData.JcatsUserID > 0)
            {
                UtilityService.ExecStoredProcedureWithoutResults("pd_JcatsUserUpdate_sp", new pd_JcatsUserUpdate_spParams()
                {
                    //AgencyID = agencyId,
                    AgencyID = model.JcatsUserData.AgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    JcatsGroupID = model.JcatsUserData.JcatsGroupID,
                    JcatsUserLoginName = model.JcatsUserData.JcatsUserLoginName,
                    JcatsUserPassword = model.JcatsUserData.JcatsUserPassword,
                    PersonID = personId,
                    JcatsUserStartDate = model.JcatsUserData.JcatsUserStartDate.IsNullOrEmpty() ? DateTime.Now : model.JcatsUserData.JcatsUserStartDate.ToDateTime(),
                    JcatsUserEndDate = model.JcatsUserData.JcatsUserEndDate.IsNullOrEmpty() ? (DateTime?)null : model.JcatsUserData.JcatsUserEndDate.ToDateTime(),
                    RecordStateID = 1,
                    JcatsUserID = model.JcatsUserData.JcatsUserID
                });


            }
            if (model.JcatsUserData.JcatsUserID > 0)
            {
                UtilityService.ExecStoredProcedureWithoutResults("pd_JcatsUserUpdateUserLevel_sp", new pd_JcatsUserUpdateUserLevel_spParams()
                {
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,

                    JcatsUserID = model.JcatsUserData.JcatsUserID,
                    JcatsUserLevelCodeID = model.JcatsUserData.JcatsUserLevelCodeID
                });
                UtilityService.ExecStoredProcedureWithoutResultADO("NG_pd_JcatsUserUpdateTimeOut_sp", new NG_pd_JcatsUserUpdateTimeOut_spParams()
                {
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,

                    JcatsUserID = model.JcatsUserData.JcatsUserID,
                    JcatsUserTimeOut = model.JcatsUserData.JcatsUserTimeOut
                });
            }

            #region Supervisor
            if (model.SupervisorChanged)
            {
                UtilityService.ExecStoredProcedureWithoutResults("Supervisor_UpdateCurrent_sp", new Supervisor_UpdateCurrent_spParams()
                {
                    StaffPersonID = personId,
                    SupervisorPersonID = model.SupervisorPersonID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                });
            }
            #endregion

            #region Staff


            model.StaffInfo.EmailToPrimaryPersonContactID = PersonContactInsertOrUpdate(model.StaffInfo.EmailPrimaryPersonContactID, personId,
                  model.StaffInfo.EmailPrimary, 2007);

            model.StaffInfo.EmailToSecondaryPersonContactID = PersonContactInsertOrUpdate(model.StaffInfo.EmailSecondaryPersonContactID, personId,
               model.StaffInfo.EmailSecondary, 2801);

            model.StaffInfo.FaxPersonContactID = PersonContactInsertOrUpdate(model.StaffInfo.FaxPersonContactID, personId,
               model.StaffInfo.Fax, 1976);

            model.StaffInfo.MobilePersonContactID = PersonContactInsertOrUpdate(model.StaffInfo.MobilePersonContactID, personId,
               model.StaffInfo.MobilePhone, 1975);

            model.StaffInfo.WorkPersonContactID = PersonContactInsertOrUpdate(model.StaffInfo.WorkPersonContactID, personId,
               model.StaffInfo.WorkPhone, 1974);


            // if (!String.IsNullOrEmpty(model.StaffInfo.StaffInfoBarNumber))
            //{
            if (model.StaffInfo.StaffInfoID == 0)
            {
                var staffId =
                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_StaffInfoInsert_sp",
                        new pd_StaffInfoInsert_spParams()
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID,
                            PersonID = personId,
                            RecordStateID = 1,
                            AlternateContactPersonID = model.StaffInfo.AlternateContactPersonID,
                            EmailToAlternatePersonContactFlag = model.StaffInfo.EmailToAlternatePersonContactFlag ? 1 : 0,
                            EmailToPrimaryPersonContactID = model.StaffInfo.UsePrimaryEmail ? model.StaffInfo.EmailToPrimaryPersonContactID : null,
                            EmailToSecondaryPersonContactID = model.StaffInfo.UseSecondaryEmail ? model.StaffInfo.EmailToSecondaryPersonContactID : null,
                            StaffInfoBarAdmittedDate = model.StaffInfo.StaffInfoBarAdmittedDate.IsNullOrEmpty() ? null : model.StaffInfo.StaffInfoBarAdmittedDate.ToDateTimeValue(),
                            StaffInfoEligibilityEffectiveDate = model.StaffInfo.StaffInfoEligibilityEffectiveDate.IsNullOrEmpty() ? null : model.StaffInfo.StaffInfoEligibilityEffectiveDate.ToDateTimeValue(),
                            StaffInfoEligibilityEndingDate = model.StaffInfo.StaffInfoEligibilityEndingDate.IsNullOrEmpty() ? null : model.StaffInfo.StaffInfoEligibilityEndingDate.ToDateTimeValue(),
                            StaffInfoBarNumber = model.StaffInfo.StaffInfoBarNumber,
                            StaffInfoEmployeeStatusCodeID = model.StaffInfo.StaffInfoEmployeeStatusCodeID.HasValue ? model.StaffInfo.StaffInfoEmployeeStatusCodeID.Value : 0,
                            StaffInfoEmployeeID = model.StaffInfo.StaffInfoEmployeeID
                        }).FirstOrDefault();

            }
            else
            {
                UtilityService.ExecStoredProcedureWithoutResults("pd_StaffInfoUpdate_sp",
                    new pd_StaffInfoInsert_spParams()
                    {

                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        PersonID = personId,
                        RecordStateID = 1,
                        AlternateContactPersonID = model.StaffInfo.AlternateContactPersonID,
                        EmailToAlternatePersonContactFlag = model.StaffInfo.EmailToAlternatePersonContactFlag ? 1 : 0,

                        EmailToPrimaryPersonContactID = model.StaffInfo.UsePrimaryEmail ? model.StaffInfo.EmailToPrimaryPersonContactID : null,
                        EmailToSecondaryPersonContactID = model.StaffInfo.UseSecondaryEmail ? model.StaffInfo.EmailToSecondaryPersonContactID : null,

                        StaffInfoBarAdmittedDate = model.StaffInfo.StaffInfoBarAdmittedDate.IsNullOrEmpty() ? null : model.StaffInfo.StaffInfoBarAdmittedDate.ToDateTimeValue(),
                        StaffInfoEligibilityEffectiveDate = model.StaffInfo.StaffInfoEligibilityEffectiveDate.IsNullOrEmpty() ? null : model.StaffInfo.StaffInfoEligibilityEffectiveDate.ToDateTimeValue(),
                        StaffInfoEligibilityEndingDate = model.StaffInfo.StaffInfoEligibilityEndingDate.IsNullOrEmpty() ? null :
                            model.StaffInfo.StaffInfoEligibilityEndingDate.ToDateTimeValue(),
                        StaffInfoBarNumber = model.StaffInfo.StaffInfoBarNumber,
                        StaffInfoEmployeeStatusCodeID =
                            model.StaffInfo.StaffInfoEmployeeStatusCodeID.HasValue ? model.StaffInfo.StaffInfoEmployeeStatusCodeID.Value : 0,
                        StaffInfoID = model.StaffInfo.StaffInfoID,
                        StaffInfoEmployeeID = model.StaffInfo.StaffInfoEmployeeID
                    });

            }
            //}
            #endregion

            #region TitleIVe
            if (model.TitleIVeStaffInfo != null)
            {
                UtilityService.ExecStoredProcedureWithResults<dynamic>("TitleIVeStaffInsertUpdate_sp",
                    new TitleIVeStaffInsertUpdate_spParams()
                    {

                        UserID = UserManager.UserExtended.UserID,
                        PersonID = personId,
                        AlternateWorkSchedule = model.TitleIVeStaffInfo.AlternateWorkSchedule,
                        FullTime = model.TitleIVeStaffInfo.FullTime,
                        MonthlySalaryAndBenefits = model.TitleIVeStaffInfo.MonthlySalaryAndBenefits,
                        NeedsActivityLog = model.TitleIVeStaffInfo.NeedsActivityLog,
                        NormalWorkHours = model.TitleIVeStaffInfo.NormalWorkHours,
                        PercentBenefitsCAC = model.TitleIVeStaffInfo.PercentBenefitsCAC,
                        PercentBenefitsFFDRP = model.TitleIVeStaffInfo.PercentBenefitsFFDRP,
                        StaffTitle = model.TitleIVeStaffInfo.StaffTitle,
                        TotalContractPayments = model.TitleIVeStaffInfo.TotalContractPayments,
                        IsEmployee = model.TitleIVeStaffInfo.IsEmployee.ToInt() == 1,
                        OHCodeID = model.TitleIVeStaffInfo.OHCodeID
                    }).FirstOrDefault();
            }
            #endregion

            #region Agency

            foreach (var agency in model.SelectedAgencyList)
            {
                if ((agency.RoleID.HasValue && agency.RoleID.Value == 0) || !agency.RoleID.HasValue)
                {
                    if (agency.AgencyID > 0 && !agency.RoleStartDate.IsNullOrEmpty() && !(agency.RecordStateID.HasValue && agency.RecordStateID.Value == 10))
                    {
                        var roleid = UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_UserRoleInsert_sp",
                      new pd_UserRoleInsert_spParams()
                      {

                          BatchLogJobID = Guid.NewGuid(),
                          UserID = UserManager.UserExtended.UserID,
                          PersonID = personId,
                          RecordStateID = 1,
                          RoleAgencyID = agency.AgencyID,
                          RoleStartDate = agency.RoleStartDate.ToDateTimeValue(),
                          RoleEndDate = agency.RoleEndDate.IsNullOrEmpty() ? null : agency.RoleEndDate.ToDateTimeValue(),
                          //RoleTypeCodeID = model.JcatsUserData.RoleTypeCodeID.Value,
                          RoleTypeCodeID = agency.RoleTypeCodeID.Value
                      }).FirstOrDefault();
                    }

                }
                else
                {
                    if (agency.RecordStateID.HasValue && agency.RecordStateID.Value == 10)
                    {

                        UtilityService.ExecStoredProcedureWithoutResults("pd_RoleDelete_sp",
                      new pd_RoleDelete_spParams()
                      {
                          ID = agency.RoleID.Value,
                          BatchLogJobID = Guid.NewGuid(),
                          UserID = UserManager.UserExtended.UserID,
                          LoadOption = "Role"
                      });
                    }
                    else
                    {
                        UtilityService.ExecStoredProcedureWithoutResults("pd_UserRoleUpdate_sp",
                      new pd_UserRoleUpdate_spParams()
                      {

                          BatchLogJobID = Guid.NewGuid(),
                          UserID = UserManager.UserExtended.UserID,

                          RecordStateID = 1,
                          AgencyID = agency.AgencyID,
                          RoleStartDate = agency.RoleStartDate,
                          RoleEndDate = agency.RoleEndDate,
                          //RoleTypeCodeID = model.JcatsUserData.RoleTypeCodeID.Value,
                          RoleTypeCodeID = agency.RoleTypeCodeID.Value,
                          RoleID = agency.RoleID.Value
                      });
                    }
                }
            }
            #endregion

            return Json(new { Status = "Done", data = model, UserID = model.JcatsUserData.JcatsUserID.ToEncrypt(), PersonID = model.JcatsUserData.PersonID.ToEncrypt() });
        }

        [HttpPost]

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.UsersEdit, PageSecurityItemID = SecurityToken.DeleteUser)]
        public virtual JsonResult DeleteUser(int userId)
        {
            if (userId > 0)
            {
                UtilityService.ExecQueryWithoutResultADO(string.Format(" pd_JcatsUserDelete_sp {0}, null, {1},'{2}'", userId, UserManager.UserExtended.UserID, Guid.NewGuid()));
            }

            return Json(new { Status = "Done" });
        }
        [HttpPost]
        public virtual JsonResult DeleteUserRole(int roleId)
        {
            if (roleId > 0)
            {
                UtilityService.ExecStoredProcedureWithoutResults("pd_RoleDelete_sp",
                          new pd_RoleDelete_spParams()
                          {
                              ID = roleId,
                              BatchLogJobID = Guid.NewGuid(),
                              UserID = UserManager.UserExtended.UserID,
                              LoadOption = "Role",
                              RecordStateID = 10
                          });
            }

            return Json(new { Status = "Done" });
        }


        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.UsersEdit, PageSecurityItemID = SecurityToken.EditUser)]

        public virtual JsonResult LoginInAs(string username)
        {
            if (UserManager.UserExtended.SystemAdminFlag != 1)
            {
                return Json(new { Status = "Fail", URL = "/" + MVC.Home.Name + "/" + MVC.Home.ActionNames.AccessDenied });

            }
            string message = "";
            string landingPage = "";
            var model = new AccountLoginViewModel()
            {
                AvailHeight = 0,
                AvailWidth = 0,
                Height = 0,
                Width = 0,
                Username = username
            };
            var isAuthenticated = UserManager.LoginAs(model, out message, out landingPage, 1);
            if (!isAuthenticated)
                return Json(new { Status = "NotAuthenticate", Message = message });

            var identity = UserManager.CreateIdentity(model, 1);
            HttpContext.GetOwinContext().Authentication.SignOut("ApplicationCookie");
            HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

            UserManager.UpdateCaseStatusBar(0);
            return Json(new { Status = "Done", URL = landingPage });
        }
        public int? PersonContactInsertOrUpdate(int? contactId, int personId, string value, int contactTypeId)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (!contactId.HasValue || contactId == 0)
                {
                    contactId = (int?)UtilityService.ExecStoredProcedureWithResults<pd_PersonContactInsert_spResult>("pd_PersonContactInsert_sp",
                        new pd_PersonContactInsert_spParams()
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            AgencyID = UserManager.UserExtended.AgencyID,
                            UserID = UserManager.UserExtended.UserID,
                            PersonID = personId,
                            PersonContactInfo = value,
                            PersonContactTypeCodeID = contactTypeId,

                            RecordStateID = 1,

                        }).FirstOrDefault().PersonContactID;


                }
                else
                {
                    UtilityService.ExecStoredProcedureWithoutResults("pd_PersonContactUpdate_sp",
                     new pd_PersonContactUpdate_spParams()
                     {
                         BatchLogJobID = Guid.NewGuid(),
                         AgencyID = UserManager.UserExtended.AgencyID,
                         UserID = UserManager.UserExtended.UserID,
                         PersonID = personId,
                         PersonContactInfo = value,
                         PersonContactTypeCodeID = contactTypeId,
                         PersonContactID = contactId,
                         RecordStateID = 1,

                     });
                }
            }
            else if (contactId.HasValue && contactId > 0)
            {
                UtilityService.ExecStoredProcedureWithResults<object>("pd_PersonContactDelete_sp",
                 new pd_PersonContactDelete_spParams()
                 {
                     RecordStateID = 10,
                     UserID = UserManager.UserExtended.UserID,
                     BatchLogJobID = Guid.NewGuid(),
                     LoadOption = "PersonContact",
                     ID = contactId.Value,

                 }).ToList();
                contactId = 0;
            }
            return contactId;

        }


        #region User LegalNumner
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.LegalNumberPage, PageSecurityItemID = SecurityToken.ViewLegalNumber)]
        public virtual ActionResult PersonLegalNumbers(string id)
        {
            var viewModel = new LegalNumberAddEditViewModel();
            var pd_PersonGet_spParams = new pd_PersonGet_spParams()
            {
                PersonID = id.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var personInfo = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", pd_PersonGet_spParams).FirstOrDefault();
            if (personInfo != null)
            {
                viewModel.LastName = personInfo.LastName;
                viewModel.FirstName = personInfo.FirstName;
                viewModel.PersonID = personInfo.PersonID;
            }
            viewModel.LegalNumbers = UtilityService.ExecStoredProcedureWithResults<pd_LegalNumberGetByPersonID_spResult>("pd_LegalNumberGetByPersonID_sp", pd_PersonGet_spParams).ToList();
            ViewBag.PageID = (1).ToEncrypt();
            return View(viewModel);
        }

        #endregion

        #region User ContactInfo

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.ViewContactInformation, PageSecurityItemID = SecurityToken.ViewContactInformation)]
        public virtual ActionResult UserContact(string id)
        {
            var personId = 0;
            if (!string.IsNullOrEmpty(id))
                personId = id.ToDecrypt().ToInt();
            var viewModel = new ContactInformationViewModel();
            viewModel.CanAddAccess = UserManager.IsUserAccessTo(SecurityToken.AddContactInformation);
            viewModel.CanEditAccess = UserManager.IsUserAccessTo(SecurityToken.EditContactInformation);
            viewModel.PersonID = personId;
            var pd_PersonContactGetByPersonID_spParams = new pd_PersonContactGetByPersonID_spParams()
            {
                PersonID = personId,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.ContactInfoList = UtilityService.ExecStoredProcedureWithResults<pd_PersonContactGetByPersonID_spResult>("pd_PersonContactGetByPersonID_sp", pd_PersonContactGetByPersonID_spParams).Select(x => new pd_PersonContactGetByCaseID_spResults()
            {
                PersonContactID = x.PersonContactID,
                PersonContactInfo = x.PersonContactInfo,
                PersonContactTypeCodeID = x.PersonContactTypeCodeID,
                PersonID = x.PersonID,
                AgencyID = x.AgencyID,
                RecordStateID = x.RecordStateID
            }).ToList();
            var person = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_sp_Result>("pd_PersonGet_sp",
                 new pd_PersonGet_spParams
                 {
                     PersonID = personId,
                     UserID = UserManager.UserExtended.UserID,
                     BatchLogJobID = Guid.NewGuid()
                 }).FirstOrDefault();
            if (person != null)
            {
                ViewBag.PersonName = "Of " + person.LastName + ", " + person.FirstName;
            }
            viewModel.ContactTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(40);

            //defult display two row
            for (int i = 0; i < 2; i++)
            {
                viewModel.ContactInfoAddList.Add(new pd_PersonContactUpdate_spParams());
            }
            return View(viewModel);
        }

        #endregion ContactInfo

        #region User Department Info
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CourtDepartmentPage, PageSecurityItemID = SecurityToken.ViewCourtDepartment)]
        public virtual ActionResult Department(string id, string deptId = "")
        {
            int personId = id.ToDecrypt().ToInt(),
                departmentId = deptId.ToDecrypt().ToInt();

            var viewModel = new UserDepartmentViewModel();
            viewModel.PersonID = personId;

            var dept = UtilityService.ExecStoredProcedureWithResults<pd_DepartmentGet_spResult>("pd_DepartmentGet_sp",
                                        new pd_DepartmentGet_spParams { DepartmentID = departmentId, UserID = UserManager.UserExtended.UserID }).FirstOrDefault();

            if (dept != null)
                viewModel.InjectFrom(dept);

            viewModel.DepartmentList = UtilityFunctions.CodeGetByTypeIdAndUserId(CodeType.Department.GetHashCode(), includeCodeId: (viewModel.DepartmentCodeID ?? 0));
            var pd_DepartmentGetByPersonID_spParams = new pd_DepartmentGetByPersonID_spParams()
            {
                PersonID = id.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var person = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_sp_Result>("pd_PersonGet_sp",
              new pd_PersonGet_spParams
              {
                  PersonID = personId,
                  UserID = UserManager.UserExtended.UserID,
                  BatchLogJobID = Guid.NewGuid()
              }).FirstOrDefault();
            if (person != null)
            {
                ViewBag.PersonName = "Of " + person.LastName + ", " + person.FirstName;
            }
            viewModel.DepartmentHistory = UtilityService.ExecStoredProcedureWithResults<pd_DepartmentGetByPersonID_spResult>("pd_DepartmentGetByPersonID_sp", pd_DepartmentGetByPersonID_spParams).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult SaveDepartment(UserDepartmentViewModel viewModel)
        {
            if (viewModel.DepartmentID.HasValue)
            {
                var deptUpdateParams = new pd_DepartmentUpdate_spParams
                {
                    DepartmentID = viewModel.DepartmentID,
                    AgencyID = viewModel.PersonID,
                    PersonID = viewModel.PersonID,
                    DepartmentCodeID = viewModel.DepartmentCodeID,
                    RecordStateID = viewModel.RecordStateID,
                    UserID = UserManager.UserExtended.UserID
                };

                if (!string.IsNullOrEmpty(viewModel.DepartmentStartDate))
                    deptUpdateParams.DepartmentStartDate = DateTime.Parse(viewModel.DepartmentStartDate);

                if (!string.IsNullOrEmpty(viewModel.DepartmentEndDate))
                    deptUpdateParams.DepartmentEndDate = DateTime.Parse(viewModel.DepartmentEndDate);

                UtilityService.ExecStoredProcedureWithResults<object>("pd_DepartmentUpdate_sp", deptUpdateParams).FirstOrDefault();
            }
            else
            {
                var deptInsertParams = new pd_DepartmentInsert_spParams
                {
                    PersonID = viewModel.PersonID,
                    DepartmentCodeID = viewModel.DepartmentCodeID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID
                };

                if (!string.IsNullOrEmpty(viewModel.DepartmentStartDate))
                    deptInsertParams.DepartmentStartDate = DateTime.Parse(viewModel.DepartmentStartDate);

                if (!string.IsNullOrEmpty(viewModel.DepartmentEndDate))
                    deptInsertParams.DepartmentEndDate = DateTime.Parse(viewModel.DepartmentEndDate);


                UtilityService.ExecStoredProcedureWithResults<object>("pd_DepartmentInsert_sp", deptInsertParams).FirstOrDefault();
            }

            return Json(new { isSuccess = true, URL = Url.Action(MVC.Users.Department(viewModel.PersonID.ToEncrypt())) });
        }


        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CourtDepartmentPage, PageSecurityItemID = SecurityToken.DeleteCourtDepartment)]
        public virtual ActionResult DeleteDepartment(string id)
        {
            UtilityService.ExecStoredProcedureWithResults<object>("pd_DepartmentDelete_sp",
                            new pd_DepartmentDelete_spParams { ID = id.ToDecrypt().ToInt(), UserID = UserManager.UserExtended.UserID }).FirstOrDefault();

            return Json(new { isSuccess = true });
        }

        #endregion


        public virtual ActionResult UploadSignature(string id)
        {
            var model = new UploadSignatureViewModel();
            model.JcatsUserID = id.ToDecrypt().ToInt();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult SaveSignatureImage(UploadSignatureViewModel viewModel)
        {
            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];


            if (Request.Files != null)
            {
                var UploadedFile = Request.Files[0];
                viewModel.UploadFileName = UploadedFile.FileName;
            }
            var oJcatsESignature = UtilityService.ExecStoredProcedureWithResults<JcatsESignatureGetByJcatsUserID_spResult>("JcatsESignatureGetByJcatsUserID_sp", new JcatsESignatureGetByJcatsUserID_spParams()
            {
                JcatsUserID = viewModel.JcatsUserID,
                UserID = UserManager.UserExtended.UserID
            }).FirstOrDefault();



            if (oJcatsESignature != null)
            {
                viewModel.UploadToFolderPath = rootPath + "\\" + oJcatsESignature.NewFilePath;

                if (!System.IO.Directory.Exists(viewModel.UploadToFolderPath))
                {
                    System.IO.Directory.CreateDirectory(viewModel.UploadToFolderPath);
                }
                Request.Files[0].SaveAs(viewModel.UploadToFolderPath + "\\" + viewModel.UploadFileName);


                viewModel.SigFile = true;
                viewModel.SigFileID = oJcatsESignature.JcatsESignatureID;
                viewModel.SigFilePath = oJcatsESignature.NewFilePath + viewModel.UploadFileName;
                viewModel.InitFilePath = oJcatsESignature.InitialsFilePath;
                viewModel.NewSigFilePath = oJcatsESignature.NewFilePath;
                // viewModel.UploadToFolderPath = oJcatsESignature.FileRootPath + oJcatsESignature.NewFilePath;
                viewModel.SigFilePath = viewModel.NewSigFilePath + viewModel.UploadFileName;


                if (viewModel.SigFileID > 0)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("JcatsESignatureDelete_sp", new JcatsESignatureDelete_spParams()
                    {
                        ID = viewModel.SigFileID,
                        UserID = UserManager.UserExtended.UserID

                    });
                }
                UtilityService.ExecStoredProcedureWithoutResultADO("JcatsESignatureInsert_sp", new JcatsESignatureInsert_spParams()
                {
                    JcatsUserID = viewModel.JcatsUserID,
                    SignatureFilePath = viewModel.SigFilePath,
                    InitialsFilePath = viewModel.InitFilePath,
                    UserID = UserManager.UserExtended.UserID

                });


            }



            return Json(new { IsSuccess = true });
        }


    }
}