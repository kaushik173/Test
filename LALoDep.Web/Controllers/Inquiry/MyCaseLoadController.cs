using DataTables.Mvc;
using LALoDep.Domain.pd_EmployeeRoster;
using LALoDep.Domain.Services;
using LALoDep.Custom;
using LALoDep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.PD_PDAction;
using LALoDep.Domain;
using Omu.ValueInjecter;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using LALoDep.Domain.rpt_Print;
using LALoDep.Domain.pd_Staff;
using System.Data.Entity.Core.Objects;
using LALoDep.Models.Inquiry;
using LALoDep.Domain.pd_CaseLoad;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.Agency;
using Microsoft.Ajax.Utilities;
using LALoDep.Domain.com_Report;

namespace LALoDep.Controllers
{
    public partial class InquiryController : Controller
    {
        // GET: /MyCaseLoad Controller/
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.MyCaseLoadPage, PageSecurityItemID = SecurityToken.viewMyCaseLaod)]
        public virtual ActionResult MyCaseLoad(string id = null, string ClientNameLast = null, string ClientNameFirst = null, string CaseNumber = null)
        {



            var oMyCaseloadGetVersionNumber = UtilityService.ExecStoredProcedureWithResults<MyCaseloadGetVersionNumber_spResult>("MyCaseloadGetVersionNumber_sp",
                    new MyCaseloadGetVersionNumber_spParams()
                    {
                        CaseloadPersonID = (id.ToDecrypt().ToInt() == 0) ? UserManager.UserExtended.PersonID : id.ToDecrypt().ToInt(),
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = new Guid()
                    }).FirstOrDefault();

            var viewModel = new MyCaseLoadIndexViewModel();
            viewModel.MyCaseloadVersionNumber = 1;
            if (oMyCaseloadGetVersionNumber != null)
            {
                viewModel.MyCaseloadVersionNumber = oMyCaseloadGetVersionNumber.MyCaseloadVersionNumber.Value;
                if (oMyCaseloadGetVersionNumber.MyCaseloadRoleTypeCodeID.HasValue)
                    viewModel.MyCaseloadRoleTypeCodeID = oMyCaseloadGetVersionNumber.MyCaseloadRoleTypeCodeID.Value;
            }
            if (!string.IsNullOrEmpty(id))
            {
                viewModel.PersonID = id.ToDecrypt().ToInt();
                viewModel.CaseStatus = "Open";
                viewModel.ClientType = "Both";
                viewModel.PrintSort = "NextHearingDate";
                var Person = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp",
                   new pd_PersonGet_spParams()
                   {
                       PersonID = id.ToDecrypt().ToInt(),
                       UserID = UserManager.UserExtended.UserID,
                       BatchLogJobID = new Guid()
                   }).FirstOrDefault();
                if (Person != null)
                {
                    viewModel.AgencyID = (int)Person.RoleAgencyID;
                    viewModel.PersonName = Person.FirstName + " " + Person.LastName;
                }
            }else
            {
                viewModel.PersonID = UserManager.UserExtended.PersonID;
            }

            viewModel.LastName = ClientNameLast;
            viewModel.FirstName = ClientNameFirst;
            viewModel.CaseNumber = CaseNumber;

            //ViewBag.viewOnLoad = false;
            if (!string.IsNullOrEmpty(ClientNameLast) || !string.IsNullOrEmpty(ClientNameFirst) || !string.IsNullOrEmpty(CaseNumber))
            {
                var searchData = GetMyCaseLoads(viewModel).DistinctBy(o => o.CaseID).ToList();
                if (searchData.Count == 1)
                {
                    return RedirectToAction("Main", "Case", new { id = searchData[0].CaseID.ToEncrypt() });
                }

            }



            var AgencyGetCountyByAttorneyPersonID_spParams = new AgencyGetCountyByAttorneyPersonID_spParams()
            {
                PersonID = (viewModel.PersonID == 0) ? UserManager.UserExtended.PersonID : viewModel.PersonID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),

            };
            viewModel.AgencyCountyList = UtilityService.ExecStoredProcedureWithResults<AgencyGetCountyByAttorneyPersonID_spResults>("AgencyGetCountyByAttorneyPersonID_sp", AgencyGetCountyByAttorneyPersonID_spParams).Select(c => (AgencyCountyModel)(new AgencyCountyModel()).InjectFrom(c)).ToList();
            if (viewModel.MyCaseloadVersionNumber == 2)
            {
                int? defaultValue = null;
                viewModel.ClientStatusList = GetMyCaseloadGetCodes((viewModel.PersonID == 0) ? UserManager.UserExtended.PersonID : viewModel.PersonID, "ClientStatus", ref defaultValue);
                viewModel.ClientStatusID = defaultValue;


                viewModel.HearingTypeList = GetMyCaseloadGetCodes((viewModel.PersonID == 0) ? UserManager.UserExtended.PersonID : viewModel.PersonID, "HearingType", ref defaultValue);
                viewModel.HearingTypeID = defaultValue;

                viewModel.AddressTypeList = GetMyCaseloadGetCodes((viewModel.PersonID == 0) ? UserManager.UserExtended.PersonID : viewModel.PersonID, "AddressType", ref defaultValue);
                viewModel.AddressTypeID = defaultValue;
                viewModel.SortByList = GetMyCaseloadGetCodes((viewModel.PersonID == 0) ? UserManager.UserExtended.PersonID : viewModel.PersonID, "SortBy", ref defaultValue);
                viewModel.SortByID = defaultValue;
                viewModel.ClassificationList = GetMyCaseloadGetCodes((viewModel.PersonID == 0) ? UserManager.UserExtended.PersonID : viewModel.PersonID, "Classification", ref defaultValue);
                viewModel.ClassificationID = defaultValue;
                viewModel.MedicationCurrentList = GetMyCaseloadGetCodes((viewModel.PersonID == 0) ? UserManager.UserExtended.PersonID : viewModel.PersonID, "MedicationCurrent", ref defaultValue);
                viewModel.MedicationCurrentID = defaultValue;
                viewModel.MedicationEverList = GetMyCaseloadGetCodes((viewModel.PersonID == 0) ? UserManager.UserExtended.PersonID : viewModel.PersonID, "MedicationEver", ref defaultValue);
                viewModel.MedicationEverID = defaultValue;
                return View("~/Views/Inquiry/MyCaseLoadV2.cshtml", viewModel);
            }
            return View(viewModel);
        }
        public List<SelectListItem> GetMyCaseloadGetCodes(int personId, string codeType, ref int? id)
        {
            id = 0;
            var list = UtilityService.ExecStoredProcedureWithResults<MyCaseloadGetCodes_spResult>("MyCaseloadGetCodes_sp",
                       new MyCaseloadGetCodes_spParams()
                       {
                           CaseloadPersonID = personId,
                           CodeType = codeType,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = new Guid()
                       });

            if (list.FirstOrDefault(o => o.DefaultFlag == 1) != null)
            {
                id = list.FirstOrDefault(o => o.DefaultFlag == 1).CodeID;

            }

            return list.Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString(), Selected = o.DefaultFlag.Value == 1 }).ToList();




        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.MyCaseLoadPage, PageSecurityItemID = SecurityToken.viewMyCaseLaod)]
        [HttpPost]
        public virtual PartialViewResult MyCaseLoad(MyCaseLoadIndexViewModel viewModel)
        {
            Session["MyCaseLoadFilter"] = viewModel;

            var data = GetMyCaseLoads(viewModel);
            ViewBag.PersonID = (viewModel.PersonID == 0 ? UserManager.UserExtended.PersonID : viewModel.PersonID).ToEncrypt();
            return PartialView("_myCaseLoadSearchResult", data);
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.MyCaseLoadPage, PageSecurityItemID = SecurityToken.viewMyCaseLaod)]
        [HttpPost]
        public virtual PartialViewResult MyCaseLoadV2(MyCaseLoadIndexViewModel viewModel)
        {
            Session["MyCaseLoadFilter"] = viewModel;
            ViewBag.PersonID = (viewModel.PersonID == 0 ? UserManager.UserExtended.PersonID : viewModel.PersonID).ToEncrypt();
            var MyCaseloadGetVersion2_spParams = new MyCaseloadGetVersion2_spParams()
            {
                CaseloadPersonID = (viewModel.PersonID == 0) ? UserManager.UserExtended.PersonID : viewModel.PersonID,

                ClientLastName = viewModel.LastName,
                ClientFirstName = viewModel.FirstName,
                PetitionNumber = viewModel.CaseNumber,
                ClientType = viewModel.ClientType,

                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyCountyID = viewModel.CountyID,
                AgeStartRange = viewModel.AgeRangeStart,
                AgeEndRange = viewModel.AgeRangeEnd,
                AddressTypeCodeID = viewModel.AddressTypeID,
                ClassificationCodeID = viewModel.ClassificationID,
                ClientStatusCodeID = viewModel.ClientStatusID,
                HearingTypeCodeID = viewModel.HearingTypeID,
                MedicationCurrentCodeID = viewModel.MedicationCurrentID,
                MedicationEverCodeID = viewModel.MedicationEverID,
                NoCompletedAR6MFlag = viewModel.NoCompletedARM ? 1 : 0,
                NoFutureARFlag = viewModel.NoFutureAR ? 1 : 0,
                OutOfStateFlag = viewModel.OutOfState ? 1 : 0,
                SortByCodeID = viewModel.SortByID,
                NoRaceFlag = viewModel.NoRace ? 1 : 0,
            };
            if (viewModel.ReportID.HasValue)
                MyCaseloadGetVersion2_spParams.ReportID = viewModel.ReportID.Value;

            if (!string.IsNullOrEmpty(viewModel.HearingDateRangeStartDate))
                MyCaseloadGetVersion2_spParams.HearingStartDate = DateTime.Parse(viewModel.HearingDateRangeStartDate);
            if (!string.IsNullOrEmpty(viewModel.HearingDateRangeEndDate))
                MyCaseloadGetVersion2_spParams.HearingEndDate = DateTime.Parse(viewModel.HearingDateRangeEndDate);

            var data = UtilityService.ExecStoredProcedureWithResults<MyCaseloadGetVersion2_spResult>("MyCaseloadGetVersion2_sp", MyCaseloadGetVersion2_spParams).ToList();


            return PartialView("_myCaseLoadV2SearchResult", data);
        }
        [HttpPost]
        public virtual ActionResult PrintMyCaseload(MyCaseLoadIndexViewModel viewModel)
        {
            var rpt_Caseload2_spParams = new rpt_Caseload2_spParams()
            {
                Casestatus = viewModel.CaseStatus,
                AttorneyID = (viewModel.PersonID == 0) ? UserManager.UserExtended.PersonID : viewModel.PersonID,
                RoleType = viewModel.ClientType,
                AgencyCountyID = viewModel.CountyID,
                SortOption = viewModel.PrintSort,
                AgencyID = (viewModel.AgencyID == 0) ? UserManager.UserExtended.AgencyID : viewModel.AgencyID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),

            };

            if (!string.IsNullOrEmpty(viewModel.StartDate))
                rpt_Caseload2_spParams.AppointmentStartDate = DateTime.Parse(viewModel.StartDate);
            if (!string.IsNullOrEmpty(viewModel.EndDate))
                rpt_Caseload2_spParams.AppointmentEndDate = DateTime.Parse(viewModel.EndDate);
            string filename = "MyCaseload_" + UserManager.UserExtended.UserID.ToEncrypt() + ".pdf";

            ReportClass rpt = new ReportClass();
            try
            {
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/Caseload2.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("rpt_Caseload2_sp", rpt_Caseload2_spParams);
                rpt.SetDataSource(table);

                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + filename);

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


            return Download(filename);
        }
        [HttpPost]
        public virtual ActionResult PrintMyCaseloadV2(MyCaseLoadIndexViewModel viewModel)
        { 
             var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 132,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };

            string filename = "MyCaseload_" + UserManager.UserExtended.UserID.ToEncrypt() + ".pdf";

            ReportClass rpt = new ReportClass();
            try
            {
                var spResult = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", com_ReportSourceGetByReportID_spParams).ToList();
                var defaultSps = spResult.FirstOrDefault().ReportSourceStoredProcedureName.Replace("dbo.", "");
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/MyCaseloadVersion2.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable(defaultSps, com_ReportSourceGetByReportID_spParams);
                rpt.SetDataSource(table);
                foreach (var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != "MyCaseloadVersion2.rpt"))
                {
                    var subTableData = (UtilityService.ExecStoredProcedureForDataTable(subRptDt.ReportSourceStoredProcedureName.Replace("dbo.", ""), com_ReportSourceGetByReportID_spParams));
                    rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(subTableData);
                }

                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + filename);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
            }


            return Download(filename);
        }

        [NonAction]
        private List<pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3_spResult> GetMyCaseLoads(MyCaseLoadIndexViewModel viewModel)
        {
            var pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3_spParams = new pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3_spParams()
            {
                PersonID = (viewModel.PersonID == 0) ? UserManager.UserExtended.PersonID : viewModel.PersonID,
                Casestatus = viewModel.CaseStatus,
                PersonNameLast = viewModel.LastName,
                PersonNameFirst = viewModel.FirstName,
                PetitionNumber = viewModel.CaseNumber,
                RoleType = viewModel.ClientType,
                ParmCaseID = null,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyCountyID = viewModel.CountyID
            };
            if (!string.IsNullOrEmpty(viewModel.StartDate))
                pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3_spParams.StartDate = DateTime.Parse(viewModel.StartDate);
            if (!string.IsNullOrEmpty(viewModel.EndDate))
                pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3_spParams.EndDate = DateTime.Parse(viewModel.EndDate);
            var data = UtilityService.ExecStoredProcedureWithResults<pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3_spResult>("pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3_sp", pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3_spParams).ToList();
            return data;

        }

    }
}