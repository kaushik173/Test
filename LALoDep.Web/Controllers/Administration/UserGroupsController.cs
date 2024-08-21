using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_UserGroups;
using LALoDep.Domain.PD_JcatsUser;
using LALoDep.Domain.Services;
using LALoDep.Core.Enums;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using LALoDep.Models.Inquiry;
using LALoDep.Core.Custom.Extensions;
using Omu.ValueInjecter;
using LALoDep.Domain;
using System.Globalization;
using LALoDep.Domain.pd_JcatsGroup;

namespace LALoDep.Controllers.Administration
{
    [AuthenticationAuthorize]
    public partial class UserGroupsController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public UserGroupsController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
        // GET: UserGroups
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.UserGroups, PageSecurityItemID = SecurityToken.UserGroups)]
        public virtual ActionResult Search()
        {
            return View(new UserGroupsViewModel
            {
                OnViewLoad = true
            });

            return View();
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.UserGroups, PageSecurityItemID = SecurityToken.EditUserGroup)]
        public virtual ActionResult EditUserGroup(string GroupId, string Mode = null)
        {
            int? groupID = UserManager.UserExtended.JcatsGroupID;
            if (!string.IsNullOrEmpty(GroupId))
            {
                groupID = GroupId.ToDecrypt().ToInt();
            }
            var viewModel = new EditUserGroupViewModel();
            if (Mode == "New")
            {
                viewModel.Mode = Mode;
                var insertedIdResult = UtilityService.ExecStoredProcedureWithResults<com_JcatsGroupInsert_spResult>(
                    "com_JcatsGroupInsert_sp",
                    new com_JcatsGroupInsert_spParams() { JcatsGroupName = null, JcatsGroupDescription = null, JcatsGroupDisplayOrder = null, RecordStateID = 1, RecordTimeStamp = null, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid(), CopySecurityOfJcatsGroupID = groupID.Value }).First();
                groupID = insertedIdResult.InsertedID;
                ViewBag.Display = "Add User Group";
            }
            else
                ViewBag.Display = "Edit User Group";

            viewModel.JcatsGroup = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupGet_spResult>(
                    "pd_JcatsGroupGet_sp",
                    new pd_JcatsGroupGet_spParams() { JcatsGroupID = groupID.Value, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).First();


            return View(viewModel);
        }

        public virtual JsonResult GetGroupAgencyList(string GroupId)
        {
            var tableData = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupAgencyGetByJcatsGroupID_spResult>(
                    "pd_JcatsGroupAgencyGetByJcatsGroupID_sp",
                    new pd_JcatsGroupAgencyGetByJcatsGroupID_spParams() { JcatsGroupID = GroupId.ToDecrypt().ToInt(), UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).Select(x => new
                    {
                        JcatsGroupAgencyID = x.JcatsGroupAgencyID,
                        AgencyId = x.AgencyID,
                        AgencyName = x.AgencyName,
                        Selected = x.Selected
                    }).ToList();
            return Json(new DataTablesResponse(0, tableData, tableData.Count, tableData.Count), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual JsonResult SaveChangedBoxes(List<CheckboxViewModel> changedBoxes, string newGroupName, string oldGroupName, int groupId)
        {
            if (newGroupName != oldGroupName)
            {
                UtilityService.ExecStoredProcedureWithoutResults("pd_JcatsGroupUpdate_sp", new pd_JcatsGroupUpdate_spParams()
                {
                    JcatsGroupID = groupId,
                    JcatsGroupName = newGroupName,
                    RecordStateID = 1,
                    RecordTimeStamp = null,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });
            }
            if (changedBoxes != null && changedBoxes.Count > 0)
            {
                foreach (var checkbox in changedBoxes)
                {
                    if (checkbox.Checked == true)
                    // if (checkbox.Checked == true && checkbox.OldSelected == false)
                    {
                        var id = UtilityService.ExecStoredProcedureWithResults<object>("pd_JcatsGroupAgencyInsert_sp",
                            new pd_JcatsGroupAgencyInsert_spParams()
                            {
                                AgencyID = checkbox.Id,
                                JcatsGroupID = groupId,
                                RecordStateID = 1,
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid()
                            }).FirstOrDefault();
                    }
                    else //if(checkbox.Checked==false && checkbox.OldSelected==true)
                    {
                        UtilityService.ExecStoredProcedureWithoutResults("pd_JcatsGroupAgencyDelete_sp",
                            new pd_JcatsGroupAgencyDelete_spParams()
                            {
                                ID = checkbox.JcatsGroupAgencyID,
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid(),
                                LoadOption = "JcatsGroupAgency",
                                RecordStateID = 10,
                            });
                    }

                }
            }
            return
                Json(new { isSuccess = true });
        }
        public virtual JsonResult GetData()
        {
            var viewModel = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupGetByUserID_spResult>("pd_JcatsGroupGetByUserID_sp",
                new pd_JcatsGroupGetByUserID_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).Select(e => new
                {
                    GroupId = e.JcatsGroupID,
                    Group = e.JcatsGroupName,
                    EncryptedGroupID = e.JcatsGroupID.ToEncrypt(),e.UserCount
                }).ToList();
            return Json(new DataTablesResponse(0, viewModel, viewModel.Count, viewModel.Count), JsonRequestBehavior.AllowGet);
        }

        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.EditUserGroupSecurity)]
        public virtual ActionResult GroupSecurity(string id)
        {
            GroupSecurityViewModel viewModel = new GroupSecurityViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                var jcatsGroupID = id.ToDecrypt().ToInt();
                var jcatsGroup = UtilityService.Context.pd_JcatsGroupGet_sp(jcatsGroupID, UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
                if (jcatsGroup != null)
                {
                    viewModel.JcatsGroupName = jcatsGroup.JcatsGroupName;
                }
                viewModel.JcatsGroupID = jcatsGroupID;
                viewModel.AccessRightsItemList = UtilityService.Context.pd_SecurityGetByJcatsGroupID_sp(jcatsGroupID,
                                                                                                        UserManager.UserExtended.UserID,
                                                                                                        Guid.NewGuid()).Select(c => (pd_SecurityGetByJcatsGroupID_sp_Result)(new pd_SecurityGetByJcatsGroupID_sp_Result()).InjectFrom(c)).ToList();
                viewModel.RestrictedItemsList = UtilityService.Context.pd_SecurityGetByNotJcatsGroupID_sp(jcatsGroupID,
                                                                                                          UserManager.UserExtended.UserID,
                                                                                                          Guid.NewGuid()).Select(c => (pd_SecurityGetByNotJcatsGroupID_sp_Result)(new pd_SecurityGetByNotJcatsGroupID_sp_Result()).InjectFrom(c)).ToList();
            }
            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult GroupSecuritySave(GroupSecurityViewModel viewModel)
        {
            //delete items selected from accessRights List
            if (viewModel.AccessRightsItemList != null && viewModel.AccessRightsItemList.Count() > 0)
                foreach (var item in viewModel.AccessRightsItemList)
                {
                    var pd_SecurityDelete_spParams = new pd_SecurityDelete_spParams()
                    {
                        SecurityItemID = item.SecurityItemID,
                        JcatsGroupID = item.JcatsGroupID,
                        RecordTimeStamp = null,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        LoadOption = "Security",
                        RecordStateID = 10
                    };
                    UtilityService.ExecStoredProcedureWithoutResults("pd_SecurityDelete_sp", pd_SecurityDelete_spParams);
                }


            //add items selected for RestrictedItems List
            if (viewModel.RestrictedItemsList != null && viewModel.RestrictedItemsList.Count() > 0)
            {
                foreach (var item in viewModel.RestrictedItemsList)
                {
                    var pd_SecurityInsert_spParams = new pd_SecurityInsert_spParams()
                    {
                        JcatsGroupID = viewModel.JcatsGroupID,
                        SecurityItemID = item.SecurityItemID,
                        RecordStateID = 1,
                        RecordTimeStamp = null,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    UtilityService.ExecStoredProcedureWithResults<decimal>("pd_SecurityInsert_sp", pd_SecurityInsert_spParams).FirstOrDefault();
                }
            }
            return Json(new { isSuccess = true });
        }


        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.EditUserGroupSecurity)]
        public virtual ActionResult GroupSecurityBySI(string id)
        {
            var securityItemId = 0;
            if (!string.IsNullOrEmpty(id))
                securityItemId = id.ToDecrypt().ToInt();
            //if (securityItemId == 0)
            //{
            //    return RedirectToAction("AccessDenied", "Home");
            //}


            var securityItem =
                UtilityService.ExecStoredProcedureWithResults<pd_SecurityGetAllBySecurityItemID_spResults>("pd_SecurityGetAllBySecurityItemID_sp",
                    new pd_SecurityGetAllBySecurityItemID_spParams()
                    {
                        SecurityItemID = securityItemId,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();

            if (securityItem == null)
                return RedirectToAction("AccessDenied", "Home");

            var model = new UserGroupBySecurityItemViewModel
            {
                SecurityItemDescription = securityItem.SecurityItemDisplay,
                SecurityItemId = securityItemId,
                AgencyList = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupAgencyGetByJcatsUserID_spResults>(
                    "pd_JcatsGroupAgencyGetByJcatsUserID_sp", new pd_JcatsGroupAgencyGetByJcatsUserID_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    })
                    .Select(
                        o =>
                            new SelectListItem()
                            {
                                Value = o.AgencyID.ToString(CultureInfo.InvariantCulture),
                                Text = o.AgencyName
                            })
            };


            return View(model);
        }

        /// <summary>
        /// Created By:Humair Ahmed
        /// Created On: 25 Oct, 2017
        /// Purpose: This action is to Get List Of Active & Inactive User Group Items.
        /// Last Updated On:
        /// Last Updated By:
        /// Reason: 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult UserGroupListForActiveAndInactive(UserGroupBySecurityItemViewModel model)
        {

            var resultList = UtilityService.ExecStoredProcedureWithResults<pd_SecurityGetAllBySecurityItemID_spResults>("pd_SecurityGetAllBySecurityItemID_sp", new pd_SecurityGetAllBySecurityItemID_spParams()
            {
                SecurityItemID = model.SecurityItemId,
                JcatsGroupName = model.JcatsGroupName,
                AgencyID = model.AgencyId,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).ToList();
            var activeList = resultList.Where(c => c.HasSecurityFlag == 1).ToList();
            var inactiveList = resultList.Where(c => c.HasSecurityFlag == 0).ToList();
            var activeListTotal = activeList.Count();
            var inactiveListTotal = inactiveList.Count();
            var dt = new List<DataTablesResponse> { new DataTablesResponse(0, activeList, activeListTotal, activeListTotal),
            new DataTablesResponse(1, inactiveList, inactiveListTotal, inactiveListTotal)};
            return Json(dt);
        }
        /// <summary>
        /// Created By:Humair Ahmed
        /// Created On: 25 Oct, 2017
        /// Purpose: Save
        /// Last Updated On:
        /// Last Updated By:
        /// Reason: 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public virtual JsonResult UserGroupBySecurityItemSave(UserGroupBySecurityItemViewModel model)
        {

            if (!string.IsNullOrEmpty(model.AuthorizedToDeactivateGroupIds))
            {
                foreach (var id in model.AuthorizedToDeactivateGroupIds.Split(',').Where(id => id.ToInt() > 0))
                {

                    UtilityService.ExecStoredProcedureWithoutResults(
                       "pd_SecurityDelete_sp",
                       new pd_SecurityDelete_spParams()
                       {
                           JcatsGroupID = id.ToInt(),
                           SecurityItemID = model.SecurityItemId,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid(),
                           LoadOption = "Security",
                           RecordStateID = 10
                       });
                }


            }
            if (!string.IsNullOrEmpty(model.NotAuthorizedToActivateGroupIds))
            {
                foreach (var id in model.NotAuthorizedToActivateGroupIds.Split(',').Where(id => id.ToInt() > 0))
                {
                    UtilityService.ExecStoredProcedureScalar("pd_SecurityInsert_sp", new pd_SecurityInsert_spParams()
                    {
                        JcatsGroupID = id.ToInt(),
                        SecurityItemID = model.SecurityItemId,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        RecordStateID = 1

                    });
                }


            }



          
            return Json(new { Status="Done" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public virtual JsonResult UserGroupDelete(int id)
        {
            if (id > 0)
            {
                UtilityService.ExecQueryWithoutResultADO(string.Format(" pd_JcatsGroupDelete_sp {0}, null, {1},'{2}'", id, UserManager.UserExtended.UserID, Guid.NewGuid()));
            }

            return Json(new { Status = "Done" });
        }
    }
}

//EXECUTE dbo.pd_JcatsGroupDelete_sp 1209,NULL,41307,NULL