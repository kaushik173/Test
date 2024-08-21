using LALoDep.Domain.Services;
using LALoDep.Core.NG_sp.com_Navigation;
using System;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Controllers;
using LALoDep.Custom;
using LALoDep.Models;

namespace LALoDep.Areas.Mobile.Controllers
{
    [AllowAnonymous] /*#todo remove this once security tokens are added to db*/
    public partial class MenuController : BaseController
    {
        private IUtilityService UtilityService;
        private UserManager _userManager;

        public MenuController(UserManager userManager, IUtilityService utilityService)
        {
            _userManager = userManager;
            UtilityService = utilityService;
        }

        [ChildActionOnly]
        public virtual PartialViewResult Render()
        {
            var com_NavigationGetByCaseIDTaskID_spParams = new NG_com_NavigationGetByCaseIDTaskID_spParams
                    {
                        CaseID = _userManager.UserExtended.CaseID,
                        NavigationTaskID = 10,
                        NavigationID = 0,
                        UserID = _userManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };


            var data = UtilityService.ExecStoredProcedureWithResults<NG_com_NavigationGetByCaseIDTaskID_spResultMobile>("NG_com_NavigationGetByCaseIDTaskID_sp", com_NavigationGetByCaseIDTaskID_spParams).ToList();

            var mainMenuItems = data.OrderBy(o => o.NavigationGroupID).Select(o => o.NavigationGroupID).Distinct();

            var viewmodel = data.Select(o =>
                new MenuItem
                {
                    Title = o.NavigationDisplayName,
                    Controller = o.NG_NavigationURL.Split("/".ToCharArray()).First(),
                    Action = o.NG_NavigationURL.Split("/".ToCharArray()).Last()
                }).ToList();

            return PartialView("_Menu", viewmodel);
        }
    }
}