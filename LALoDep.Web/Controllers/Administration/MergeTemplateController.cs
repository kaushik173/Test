using CrystalDecisions.CrystalReports.Engine;
using DataTables.Mvc;
using LALoDep.Domain.Agency;
using LALoDep.Domain.com_Report;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_SearchByPhysicalFile;
using LALoDep.Domain.pd_Users.Edit;
using LALoDep.Domain.qcal;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Report;
namespace LALoDep.Controllers.Administration
{
    [AuthenticationAuthorize]
    public partial class AdministrationController : Controller
    {

        // [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseCleanupPage, PageSecurityItemID = SecurityToken.CaseCleanup_SeeAllAttorneys)]
        public virtual ActionResult MergeTemplateAdmin(string id)
        {
            var viewModel = new MergeTemplateViewModel();


            viewModel.Agencies = UtilityService.ExecStoredProcedureWithResults<AgencyGetByUserID_spResult>(
                    "AgencyGetByUserID_sp", new AgencyGetByUserID_spParams
                    {

                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = new Guid(),
                    }).Select(x => new SelectListItem { Text = x.AgencyName, Value = x.AgencyID.ToString() }).ToList();

            if (viewModel.Agencies.Count() == 1)
            {
                viewModel.AgencyID = viewModel.Agencies.ToList()[0].Value.ToInt();
            }
            else
            {
                viewModel.AgencyID = id.ToDecrypt().ToInt();
            }


             


            return View(viewModel);
        }
     [HttpPost]
        public virtual JsonResult MergeTemplateAdmin(int? AgencyID)
        {

            var result = UtilityService.ExecStoredProcedureWithResults<MergeTemplateSearch_spResult>("MergeTemplateSearch_sp",
                    new MergeTemplateSearch_spParams()
                    {

                        AgencyID = AgencyID,

                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).Select(o => new
                    {

                        o.ReportID,
                        o.DataSource,
                        o.DocumentName,
                        o.TemplatePath,

                        EncryptedReportID = o.ReportID.ToEncrypt()

                    })
                  .ToList();




            return Json(new DataTablesResponse(1, result, result.Count, result.Count));

        }


      
        [HttpPost]
        public virtual JsonResult MergeTemplateAdminCopy(string id, int? AgencyID)
        {
            var newId = UtilityService.ExecStoredProcedureWithResults<object>("MergeTemplateCopyProcess_sp",
                    new MergeTemplateCopyProcess_spParams()
                    {
                        FromReportID = id.ToDecrypt().ToInt(),
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).FirstOrDefault();
            if (newId != null)
            {

                return Json(new { Status = "Done", URL = "/Administration/MergeTemplate/" +(AgencyID.HasValue? AgencyID.Value.ToEncrypt():"") });

            }

            return Json(new { Status = "fail" });
        }




    }
}