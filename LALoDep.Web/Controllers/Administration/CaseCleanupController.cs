using CrystalDecisions.CrystalReports.Engine;
using LALoDep.Domain.Agency;
using LALoDep.Domain.com_Report;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_SearchByPhysicalFile;
using LALoDep.Domain.pd_Users.Edit;
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

namespace LALoDep.Controllers.Administration
{
    [AuthenticationAuthorize]
    public partial class CaseCleanupController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;
        public CaseCleanupController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseCleanupPage, PageSecurityItemID = SecurityToken.CaseCleanup_View)]
        public virtual ActionResult Search()
        {
            var viewModel = new CaseCleanupViewModel();

            viewModel.AgencyID = UtilityService.ExecStoredProcedureWithResults<pd_AgencyGetHomeByPersonID_spResult>(
                    "pd_AgencyGetHomeByPersonID_sp", new pd_AgencyGetHomeByPersonID_spParams
                    {
                        PersonID = UserManager.UserExtended.PersonID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = new Guid(),
                    }).Where(o => o.HomeAgencyFlag == 1).Select(o => o.AgencyID).FirstOrDefault();

            viewModel.Agencies = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupAgencyGetByJcatsGroupID_spResult>("pd_JcatsGroupAgencyGetByJcatsGroupID_sp",
                    new pd_JcatsGroupAgencyGetByJcatsGroupID_spParams() { JcatsGroupID = 31, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                    .Select(x => new SelectListItem { Text = x.AgencyName, Value = x.AgencyID.ToString() }).ToList();

            viewModel.AttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_RoleAgencyAttorneyGet_spResult>("pd_RoleAgencyAttorneyGet_sp",
                    new pd_RoleAgencyAttorneyGet_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                    .Select(x => new SelectListItem { Text = x.PersonNameDisplay, Value = x.PersonID.ToString() }).ToList();
            if (!UserManager.IsUserAccessTo(SecurityToken.CaseCleanup_SeeAllAttorneys))
            {
                viewModel.AttorneyList = viewModel.AttorneyList.Where(o => o.Value == UserManager.UserExtended.PersonID.ToString()).ToList();
            }
            return View(viewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseCleanupPage, PageSecurityItemID = SecurityToken.CaseCleanup_View)]
        [HttpPost]
        public virtual ActionResult Search(CaseCleanupViewModel viewModel)
        {

            var result = UtilityService.ExecStoredProcedureWithResults<pd_CaseCleanupSearch_spResult>("pd_CaseCleanupSearch_sp",
                    new pd_CaseCleanupSearch_spParams() { AgencyID = viewModel.AgencyID, AttorneyPersonID = viewModel.PersonID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() },90)
                    .GroupBy(x => x.SearchType)
                    .Select(x => new CaseCleanupResultViewModel
                    {
                        SearchType = x.Key,
                        CaseList = x.Select(c => (CaseDetail)(new CaseDetail()).InjectFrom(c)).ToList()
                    }).ToList();

            return PartialView(MVC.CaseCleanup.Views._CaseCleanupSearchResultPartial, result);
        }

        [HttpPost]
        public virtual ActionResult PrintCaseCleanup(CaseCleanupViewModel viewModel)
        {
            var comReportSourceGetByReportIdSpParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 83,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };

            #region Delete Report Parameter

            var dictionaryParam = new Dictionary<string, object>()
            {
                {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
            };

            /*Delete Existing Report Parameters saved for this User*/
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueDelete_sp", dictionaryParam);
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterHeaderDelete_sp", dictionaryParam);

            #endregion


            var Agency = UtilityService.ExecStoredProcedureWithResults<pd_AgencyGet_spResult>("pd_AgencyGet_sp",
                    new pd_AgencyGet_spParams() { AgencyID = viewModel.AgencyID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

            #region Insert Report Parameter
            // Insert report header Parameters
            var headerParams = new com_ReportParameterHeaderInsert_spParams()
            {
                ReportID = comReportSourceGetByReportIdSpParams.ReportID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                JcatsUserID = UserManager.UserExtended.UserID,
                ReportParameterHeaderName = "Agency",
                ReportParameterHeaderValue = Agency.AgencyName,
                RecordStateID = 1
            };
            UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderInsert_sp", headerParams).ToList();

            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueInsert_sp",
                                                    new Dictionary<string, object>()
                                                    {
                                                        {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                                                        {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                                                        {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID},
                                                        {"@ReportParameterValueID", null},
                                                        {"@Sequence", 1},
                                                        {"@Value", viewModel.AgencyID}
                                                    });

            if (viewModel.PersonID.HasValue)
            {
                var person = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp",
                    new pd_PersonGet_spParams() { PersonID = viewModel.PersonID.Value, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();

                headerParams.ReportParameterHeaderName = "Attorney";
                headerParams.ReportParameterHeaderValue = person.LastName + ", " + person.FirstName;
                UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderInsert_sp", headerParams).ToList();

                UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueInsert_sp",
                                                         new Dictionary<string, object>()
                                                         {
                                                                {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                                                                {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                                                                {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID},
                                                                {"@ReportParameterValueID", null},
                                                                {"@Sequence", 2},
                                                                {"@Value", viewModel.PersonID}
                                                         });
            }
            #endregion

            var spResult = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp",
                                                                                    comReportSourceGetByReportIdSpParams).ToList();
            var rpt = new ReportClass { FileName = HttpContext.Server.MapPath("~/Reports/" + spResult[0].ReportSourceDocumentName) };

            try
            {


                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable(spResult[0].ReportSourceStoredProcedureName.Replace(".dbo", ""), comReportSourceGetByReportIdSpParams);
                rpt.SetDataSource(table);

                foreach (var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != spResult[0].ReportSourceDocumentName))
                {
                    rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(
                                        UtilityService.ExecStoredProcedureForDataTable(subRptDt.ReportSourceStoredProcedureName.Replace(".dbo", ""),
                                        comReportSourceGetByReportIdSpParams));
                }

                var filename = spResult[0].ReportDisplayName + comReportSourceGetByReportIdSpParams.ReportID.ToEncrypt() + ".pdf";

                if (UserManager.UserExtended.PrintDocumentOn == "NewWindow")
                {
                    var filePath = UtilityFunctions.GetDocumentDownloadFolderPath() + filename;
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);
                    rpt.Close();
                    rpt.Dispose();
                    GC.Collect();
                    return RedirectToAction("Preview", "Home", new { path = Utility.Encrypt(UtilityFunctions.GetDocumentDownloadFolderRelativePath() + filename) });
                }

                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                return File(stream, "application/pdf", filename);
            }
            catch (Exception ex)
            {
              

            }
            finally
            {
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
            }
            return Content("Report not generating");
        }

        [HttpPost]
        public virtual ActionResult SetCaseInSession(string id)
        {
            UserManager.UpdateCaseStatusBar(id.ToDecrypt().ToInt());
            return Json(new { isSuccess = true }, JsonRequestBehavior.AllowGet);
        }

    }
}