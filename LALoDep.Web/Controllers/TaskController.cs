using LALoDep.Domain.Services;
using LALoDep.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Custom.Attributes;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class TaskController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;
        public TaskController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
    }
}