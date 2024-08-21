using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_SearchByPhysicalFile;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models;
using LALoDep.Models.Inquiry;

namespace LALoDep.Controllers
{
    public partial class SearchByPhysicalFileController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public SearchByPhysicalFileController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.SearchByPhysicalFile, PageSecurityItemID = SecurityToken.SearchByPhysicalFile)]
        public virtual ActionResult Search()
        {
            var viewModel = new SearchByPhysicalFileViewModel()
            {
                OnViewLoad = true
            };
            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupAgencyGetByJcatsGroupID_spResult>("pd_JcatsGroupAgencyGetByJcatsGroupID_sp",
        new pd_JcatsGroupAgencyGetByJcatsGroupID_spParams() { JcatsGroupID = 31, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).Select(e => new SelectListItem()
        {
            Value = e.JcatsGroupAgencyID.ToString(),
            Text = e.AgencyName
        }).ToList();
            return View(viewModel);
        }
        [HttpPost]
        public virtual JsonResult Search(SearchByPhysicalFileViewModel model)
        {
            var result =
                    UtilityService.ExecStoredProcedureWithResults<pd_PhysicalFileSearch_spResult>(
                        "pd_PhysicalFileSearch_sp",
                        new pd_PhysicalFileSearch_spParams()
                        {
                            FirstName = model.ClientFirstName,
                            LastName = model.ClientLastName,
                            PhysicalFileName1 = model.FileName1,
                            PetitionDocketNumber = null, //need to confirm
                            PhysicalFileName2Operator = "AND",
                            PhysicalFileName2 = model.FileName2,
                            PhysicalFileName3Operator = "AND",
                            PhysicalFileName3 = model.FileName3,
                            AgencyID = model.AgencyID,
                            SortOption = null, //need to change
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = new Guid()
                        }).ToList();

            var physicalFileSearchModel = result.Select(x => new
            {
                PhysicalFileName = x.PhysicalFileName,
                Client = x.Client,
                DOB = x.DOB,
                Role = x.Role,
                CaseID = x.CaseID,
                EncryptedCaseID = x.CaseID.ToEncrypt(),
                CloseDate = x.PetitionCloseDate,
                CaseNumber = x.CaseNumber,
                PetitionDocketNumber = x.PetitionDocketNumber,
                Attorney = x.Attorney,
                RoleClient = x.RoleClient,
                EncryptedRoleID = x.RoleID.ToEncrypt()
            });

            return Json(new DataTablesResponse(0, physicalFileSearchModel, result.Count, result.Count));

        }
        
    }
}