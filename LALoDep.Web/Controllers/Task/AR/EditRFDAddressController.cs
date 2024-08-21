using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Address;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Controllers.CaseOpening;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using DataTables.Mvc;
using LALoDep.Models.CaseOpening;
using LALoDep.Models.Task;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Role;
using LALoDep.Models;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningAddressPlacement, PageSecurityItemID = SecurityToken.ViewAddress)]
        public virtual ActionResult EditRFDAddress(string id, string addressId)
        {
            ViewBag.Id = id;//for populate tabs

            var caseAddress = new CaseOpeningController(UserManager, UtilityService);
            var model = caseAddress.GetAddressModel(addressId);

            return View("~/Views/Task/AR/EditRFDAddress.cshtml", model);
        }


    }
}