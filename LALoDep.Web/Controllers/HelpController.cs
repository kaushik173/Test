using LALoDep.Domain.pd_JcatsReport;
using LALoDep.Domain.Services;
using Jcats.SD.Domain.com_Jcats;
using LALoDep.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Models;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Domain.com_Jcats;
using LALoDep.Domain.NG_com;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class HelpController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public HelpController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
        public virtual ActionResult Index(string nav)
        {
            string LastPageAccess = string.Empty;
            try
            {
                LastPageAccess = nav;// Request.UrlReferrer.AbsolutePath.Substring(1, Request.UrlReferrer.AbsolutePath.Length - 1);
                if (string.IsNullOrEmpty(LastPageAccess))
                {
                    LastPageAccess = "Case/Search";
                }
            }
            catch
            {
                LastPageAccess = "Case/Search";
            }

            if (!string.IsNullOrEmpty(LastPageAccess))
            {
                string[] currentURLArray = null;
                currentURLArray = LastPageAccess.Split('/');
                if (currentURLArray.Count() >= 2)
                {
                    LastPageAccess = currentURLArray[0] + '/' + currentURLArray[1];
                }

                var NG_com_NavigationGetByCurrentURL_spParams = new NG_com_NavigationGetByCurrentURL_spParams
                {
                    CurrentURL = LastPageAccess,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                };

                var PageAccess = UtilityService.ExecStoredProcedureWithResults<NG_com_NavigationGetByCurrentURL_spResults>("NG_com_NavigationGetByCurrentURL_sp", NG_com_NavigationGetByCurrentURL_spParams).FirstOrDefault();
                if (PageAccess != null)
                {
                    string[] navigationArray = null;
                    navigationArray = PageAccess.NG_NavigationURL.Split('?');
                    ViewBag.ASPPageID = navigationArray[0];
                }
            }
            else
            {
                ViewBag.ASPPageID = "Case/Search";
            }

            return View();
        }

        public virtual JsonResult HelpTree(string ASPPageID)
        {
            var com_JcatsHelpGetAllActive_spParams = new com_JcatsHelpGetAllActive_spParams
            {
                JcatsHelpPageName = ASPPageID,
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID
            };

            var data = UtilityService.ExecStoredProcedureWithResults<com_JcatsHelpGetAllActive_spResult>("com_JcatsHelpGetAllActive_sp", com_JcatsHelpGetAllActive_spParams).ToList();
            var selected = data.FirstOrDefault(o => o.JcatsHelpSelectedFlag == 1);
            if (selected == null)
            {
                selected = data.FirstOrDefault(o => o.JcatsHelpPageDisplayName.Trim().ToLower().CompareTo("search for case") == 0);
            }
            var reportCategories = data.Select(o => o.JcatsHelpGroupDisplayName).Distinct();

            var groupedData = reportCategories.Select(reportGroup => new TreeViewModel
            {
                name = reportGroup,
                type = "folder",
                children = data.Where(o1 => o1.JcatsHelpGroupDisplayName == reportGroup)
                    .Select(o1 => new TreeViewModel
                    {
                        name = string.Format(@"<i class='fa fa-file-text-o'></i><a onclick='displayPage(""{2}"")' id='Help_{0}'>{1}</a></div>",
                        o1.JcatsHelpID,
                        o1.JcatsHelpPageDisplayName,
                        "/Help/PageContent/?page=" + o1.JcatsHelpPageName),
                        type = "item",
                        id = o1.JcatsHelpID
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
                        selectedPageContent = "/Help/PageContent/?page=SearchForCase.htm";
                    }
                }
            }
            if (selectedTree == 0) { selectedTree = -1; }
            else { selectedTree = selectedTree - 1; selectedPageContent = "/Help/PageContent/?page=" + selected.JcatsHelpPageName; }

            //insert log for Help
            var com_JcatsHelpLogInsert_spParams = new com_JcatsHelpLogInsert_spParams()
            {
                JcatsHelpPageName = ASPPageID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            UtilityService.ExecStoredProcedureWithResults<int>("com_JcatsHelpLogInsert_sp", com_JcatsHelpLogInsert_spParams).FirstOrDefault();


            return Json(new { groupedData = groupedData, SelectedTree = selectedTree, JcatsHelpID = JcatsHelpID, selectedPageContent = selectedPageContent }, JsonRequestBehavior.AllowGet);
        }


        public virtual ActionResult PageContent(string page)
        {
            var HelpRootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["HelpRootPath"];
            var filePath = HelpRootPath + page;
            if (System.IO.File.Exists(filePath))
            {
                var content = System.IO.File.ReadAllText(filePath, System.Text.Encoding.GetEncoding("iso-8859-1"));
                return Content(content);

            }
            return Content("");
        }

        public virtual ActionResult TutorialConfig()
        {
            var path = System.Web.HttpContext.Current.Server.MapPath("/Scripts/Application-Script/tutorial/jtutorial.config.json");
            if (System.IO.File.Exists(path))
            {
                var content = System.IO.File.ReadAllText(path);
                return Content(content);

            }
            return Content("");
        }


        public virtual ActionResult Tutorials()
        {
            var tutorialsList = UtilityService.ExecStoredProcedureWithResults<JcatsTutorialGetList_spResult>("JcatsTutorialGetList_sp", new JcatsTutorialGetList_spParams()
            {

                CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
            }).ToList();

            return View(tutorialsList);
        }

        public virtual ActionResult DownloadTutorials(string file)
        {
            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];

            var fileName = rootPath + "\\Tutorials\\" + file;



            if (!System.IO.File.Exists(fileName))
            {
                return Content("File not exists ");
            }



            return File(fileName, System.IO.Path.GetFileName(fileName));

        }


    }
}