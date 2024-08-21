using System;
using System.Linq;
using System.Web.Mvc;

using LALoDep.Domain.Services;
using LALoDep.Core.NG_sp.com_Navigation;
using LALoDep.Custom;
using LALoDep.Models;

namespace LALoDep.Controllers
{
    [AllowAnonymous] /*#todo remove this once security tokens are added to db*/
    public partial class MenuController : BaseController
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public MenuController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        [ChildActionOnly]
        public virtual PartialViewResult Render()
        {
            var com_NavigationGetByCaseIDTaskID_spParams = new NG_com_NavigationGetByCaseIDTaskID_spParams
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        NavigationTaskID = 0,
                        NavigationID = 0,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };


            var data = UtilityService.ExecStoredProcedureWithResults<NG_com_NavigationGetByCaseIDTaskID_spResult>("NG_com_NavigationGetByCaseIDTaskID_sp", com_NavigationGetByCaseIDTaskID_spParams).ToList();

            var mainMenuItems = data.OrderBy(o => o.NavigationGroupDisplayOrder).Select(o => o.NavigationGroupDisplayName).Distinct();

            var viewmodel = mainMenuItems.Select(o =>
                new MenuItem
                {
                    
                    Title = o,
                    Controller = data.First(o1 => o1.NavigationGroupDisplayName == o).NG_NavigationURL.Split("/".ToCharArray()).First(),
                    Action = data.First(o1 => o1.NavigationGroupDisplayName == o).NG_NavigationURL.Split("/".ToCharArray()).Last(),
                    Items = data.Where(o1 => o1.NavigationGroupDisplayName == o).OrderBy(o1 => o1.NavigationDisplayOrder).Select(o2 =>
                    new MenuItem
                    {
                        Title = o2.NavigationDisplayName.Replace("<BR>", string.Empty),
                        Controller = o2.NG_NavigationURL.Split("/".ToCharArray()).First(),
                        Action = o2.NG_NavigationURL.Split("/".ToCharArray()).Last()
                    }).Where(x => x.Title != "Merge Documents" && x.Title != "Conflict&nbsp;Queue-&nbsp;Supervisor" && x.Title != "Conflict&nbsp;Queue&nbsp;-&nbsp;Attorney").ToList()//not display Some Menu
                });

            return PartialView("_Menu", viewmodel);
        }
    }
}