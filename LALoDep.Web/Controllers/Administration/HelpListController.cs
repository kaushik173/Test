using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.AddEditCountyCounsel;
using LALoDep.Domain.com_Jcats;
using LALoDep.Domain.pd_CountyCounselList;
using LALoDep.Domain.pd_JcatsGroup;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain.pd_UserGroups;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using Jcats.SD.Domain.com_Jcats;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models;
using LALoDep.Models.Administration;


namespace LALoDep.Controllers.Administration
{
    public partial class AdministrationController : Controller
    {
        [ClaimsAuthorize(IsPageMainMethod = true, PageSecurityItemID = SecurityToken.ViewHelpPages, CustomSecurityItemIds = PageLevelSecurityItemIds.AdministratorHelpPage)]
        public virtual ActionResult HelpList()
        {
            var viewModel = new HelpListViewModel
            {
                LinkPages = GetAspPageList(),
                HasAddAccess = UserManager.IsUserAccessTo(SecurityToken.AddHelpPage),
                HasEditAccess = UserManager.IsUserAccessTo(SecurityToken.EditHelpPage)
            };

            return View(viewModel);

        }
        public virtual JsonResult HelpTree()
        {
            var com_JcatsHelpGetAllActive_spParams = new com_JcatsHelpGetAllActive_spParams
            {
                UserID = 1,
                BatchLogJobID = Guid.NewGuid()
            };

            var data = UtilityService.ExecStoredProcedureWithResults<com_JcatsHelpGetAllActive_spResult>("com_JcatsHelpGetAllActive_sp", com_JcatsHelpGetAllActive_spParams).ToList();
            var selected = data.FirstOrDefault(o => o.JcatsHelpSelectedFlag == 1);
            var reportCategories = data.Select(o => o.JcatsHelpGroupDisplayName).Distinct();
         
            var groupedData = reportCategories.Select(reportGroup => new TreeViewModel
            {
                name = reportGroup,
                type = "folder",
                children = data.Where(o1 => o1.JcatsHelpGroupDisplayName == reportGroup)
                    .Select(o1 => new TreeViewModel
                    {
                        name = string.Format(@"<a href='javascript:void(0);' id='Help_{0}'><i class='fa fa-file-text-o'></i>{1}</a></div>",
                        o1.JcatsHelpID,
                        o1.JcatsHelpPageDisplayName),
                        type = "item",
                        id = o1.JcatsHelpID,
                        pageUrl = "/Help/PageContent/?page=" + o1.JcatsHelpPageName
                    }).ToList(),
                selectedId = selected != null ? selected.JcatsHelpID : 0
            });
            //for select last selected report based on flag
            int selectedTree = 0;
            string selectedPageContent = string.Empty;
            int JcatsHelpID = 0;
            if (selected != null)
            {
                foreach (var item in groupedData)
                {
                    selectedTree++;
                    var itemSelect = item.children.Where(m => m.id == item.selectedId).ToList();
                    int ID = 0;
                    if (itemSelect.Count > 0)
                    {
                        var List = itemSelect.Where(m => m.id == item.selectedId).FirstOrDefault();
                        if (List != null) { ID = List.id; JcatsHelpID = item.selectedId; break; }
                        selectedPageContent = "/Help/PageContent/?page=Searching_for_a_Case.htm";
                    }
                }
            }
            if (selectedTree == 0) { selectedTree = -1; }
            else { selectedTree = selectedTree - 1; selectedPageContent = "/Help/PageContent/?page=" + selected.JcatsHelpPageName; }

            return Json(new { groupedData = groupedData, SelectedTree = selectedTree, JcatsHelpID = JcatsHelpID, selectedPageContent = selectedPageContent }, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult AspPageList(int? helpID)
        {
            if (helpID != null)
            {
                var aspAspPagelist = GetAspPageList(helpID);
                aspAspPagelist = aspAspPagelist.OrderByDescending(x => x.SelectedFlag).ToList();

                var helpPage = UtilityService.ExecStoredProcedureWithResults<com_JcatsHelpGet_spResult>("com_JcatsHelpGet_sp", new com_JcatsHelpGet_spParams()
                {
                    JcatsHelpID = helpID.Value,
                    UserID = 1,
                    BatchLogJobID = Guid.NewGuid()
                }).FirstOrDefault();

                return Json(new { isSuccess = true, data = aspAspPagelist, helpPageData = helpPage });
            }
            return Json(new { isSuccess = false });
        }

        [HttpPost]
        [ValidateInput(false)]
        [ClaimsAuthorize(IsPageMainMethod = true, PageSecurityItemID = SecurityToken.AddHelpPage, CustomSecurityItemIds = PageLevelSecurityItemIds.AdministratorHelpPage)]
        public virtual JsonResult SaveHelp(HelpAddEditViewModel viewModel)
        {
            if (viewModel.JcatsHelpID.HasValue && viewModel.JcatsHelpID.Value > 0)
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("com_JcatsHelpUpdate_sp",
                    new com_JcatsHelpUpdate_spParams()
                    {
                        JcatsHelpPageName = viewModel.HelpFileUrl,
                        JcatsHelpID = viewModel.JcatsHelpID.Value,
                        JcatsHelpGroupDisplayName = viewModel.GroupName,
                        JcatsHelpPageDisplayName = viewModel.PageName,
                        JcatsHelpPageDisplayOrder = viewModel.Order,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    });
            }
            else
            {
                if (viewModel.HelpFileUrl.IsNullOrEmpty())
                    viewModel.HelpFileUrl = viewModel.PageName + ".htm";
                viewModel.JcatsHelpID = UtilityService.ExecStoredProcedureScalar("com_JcatsHelpInsert_sp",
                     new com_JcatsHelpUpdate_spParams()
                     {
                         JcatsHelpPageName = viewModel.HelpFileUrl,

                         JcatsHelpGroupDisplayName = viewModel.GroupName,
                         JcatsHelpPageDisplayName = viewModel.PageName,
                         JcatsHelpPageDisplayOrder = viewModel.Order,
                         RecordStateID = 1,
                         UserID = UserManager.UserExtended.UserID,
                         BatchLogJobID = Guid.NewGuid()
                     }).ToInt();

            }

            if (!string.IsNullOrEmpty(viewModel.RemoveAspPageIDs))
            {
                var pageIds = viewModel.RemoveAspPageIDs.Split(',');

                foreach (var aspPageID in pageIds)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("com_JcatsHelpNavigationUpdate_sp", new com_JcatsHelpNavigationUpdate_spParams()
                    {

                        JcatsHelpID =0,
                        NavigationID = aspPageID.ToInt(),
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    });

                }
            }

            if (!string.IsNullOrEmpty(viewModel.ASPPageIDs))
            {
                var pageIds = viewModel.ASPPageIDs.Split(',');

                foreach (var aspPageID in pageIds)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("com_JcatsHelpNavigationUpdate_sp", new com_JcatsHelpNavigationUpdate_spParams()
                    {

                        JcatsHelpID = viewModel.JcatsHelpID.Value,
                        NavigationID = aspPageID.ToInt(),
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    });

                }
            }
            var HelpRootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["HelpRootPath"];

            if (!string.IsNullOrEmpty(viewModel.HelpContent))
            {
                if (!viewModel.HelpFileUrl.Contains(HelpRootPath))
                    viewModel.HelpFileUrl = HelpRootPath + viewModel.HelpFileUrl;
             
                System.IO.File.WriteAllText(viewModel.HelpFileUrl, viewModel.HelpContent, System.Text.Encoding.GetEncoding("iso-8859-1"));
            }
            return Json(new { isSuccess = true, message = "Help Saved successfully." });
        }
        private List<com_JcatsHelpGetNavigation_spResult> GetAspPageList(int? helpId = null)
        {
            var items = new List<com_JcatsHelpGetNavigation_spResult>();

            var aspPageList = UtilityService.ExecStoredProcedureWithResults<com_JcatsHelpGetNavigation_spResult>("com_JcatsHelpGetNavigation_sp", new com_JcatsHelpGetASPPage_spParams()
            {
                JcatsHelpID = helpId,
                UserID = 1,
                BatchLogJobID = Guid.NewGuid()
            }).ToList();
            if (helpId.HasValue && helpId.Value > 0)
            {
               // var page = aspPageList.Where(o => o.JcatsHelpID == helpId).FirstOrDefault();
                //if (page != null)
                {
                 //   items.Add(page);
                }
            }
            var aspPages =
                  aspPageList.ToList();

            items.AddRange(aspPages);
            return items;
        }

    }
}