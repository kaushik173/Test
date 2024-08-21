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
using LALoDep.Controllers.Inquiry;
using LALoDep.Core.Enums;
using LALoDep.Domain.pd_MasterCalendar;
using LALoDep.Domain.com_Report;
using CrystalDecisions.Shared;
using LALoDep.Domain.PD_JcatsUser;

namespace LALoDep.Controllers
{
    public partial class InquiryController : Controller
    {
        // GET: /MasterCalendarController/

        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.ViewCalendarManagement)]
        public virtual ActionResult MasterCalendar()
        {
            var viewModel = new MasterCalendarViewModel();
            TempData["StartDate"] = viewModel.StartDate = DateTime.Now.ToString("MM/dd/yyyy");
            TempData["EndDate"] = viewModel.EndDate = DateTime.Now.ToString("MM/dd/yyyy");

            //if (Session["CalendarStartDate"] != null)
            //{
            //    viewModel.StartDate = Session["CalendarStartDate"].ToString();
            //}
            //else
            //{
            //    Session["CalendarStartDate"] = viewModel.StartDate = DateTime.Now.ToString("MM/dd/yyyy");
            //}
            //if (Session["CalendarEndDate"] != null)
            //{
            //    viewModel.EndDate = Session["CalendarEndDate"].ToString();
            //}
            //else
            //{
            //    Session["CalendarEndDate"] = viewModel.EndDate = DateTime.Now.AddDays(7).ToString("MM/dd/yyyy");
            //}

            //Agency 
            viewModel.AgencyList = UtilityService.Context.pd_JcatsGroupAgencyGetByJcatsGroupID_sp(UserManager.UserExtended.JcatsGroupID,
                                                                                                  UserManager.UserExtended.UserID, Guid.NewGuid()).Select(x => new AgencyModel()
                                                                                                  {
                                                                                                      AgencyID = x.AgencyID,
                                                                                                      AgencyName = x.AgencyName
                                                                                                  }).ToList();
            if (viewModel.AgencyList.Count() == 1)
            {
                viewModel.AgencyID = viewModel.AgencyList.First().AgencyID;
            }

            var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
            {
                CodeTypeID = CodeType.Department.GetHashCode(),//Department
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            //Department
            viewModel.DepartmentList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();

            //DayofWeek
            var pd_CodeGetBySysValAndUserID_spParams = new pd_CodeGetBySysValAndUserID_spParams()
            {
                SystemValueIDList = CodeType.DayOfWeek.GetHashCode(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            viewModel.DayOfWeekList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetBySysValAndUserID_spResults>("pd_CodeGetBySysValAndUserID_sp", pd_CodeGetBySysValAndUserID_spParams).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();

            //Hearing Type
            pd_CodeGetByTypeIDAndUserID_spParams.CodeTypeID = CodeType.HearingType.GetHashCode();
            viewModel.HearingTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();


            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult SearchMasterCalendar(MasterCalendarViewModel viewModel)
        {
            //Session["CalendarStartDate"] = viewModel.StartDate;
            //Session["CalendarEndDate"] = viewModel.EndDate;

            TempData["StartDate"] = viewModel.StartDate;
            TempData["EndDate"] = viewModel.EndDate;

            var pd_MasterCalendarGet_spParams = new pd_MasterCalendarGet_spParams()
            {
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                AgencyID = viewModel.AgencyID,
                DepartmentID = viewModel.DepartmentID,
                HearingTypeCodeID = viewModel.HearingTypeCodeID,
                DesignatedDayCodeID = viewModel.DesignatedDayCodeID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
            };
            var hearingData = UtilityService.ExecStoredProcedureWithResults<pd_MasterCalendarGet_spResults>("pd_MasterCalendarGet_sp", pd_MasterCalendarGet_spParams);

            var hearingCount = 0;
            var displayData = new List<pd_MasterCalendarGet_spResults>();
            var hearingId = 0;
            foreach (var data in hearingData)
            {
                if (hearingCount == 0)
                    hearingCount = data.HearingCount;

                if (data.HearingID.Value == hearingId)
                {

                    data.EventTime = "";
                    data.HearingType = "";
                    data.Department = "";
                    data.Result = "";
                    data.AgencyAttorneyPersonNameFirst = data.AgencyAttorneyPersonNameLast = "";

                }
                else
                {
                    data.Result = data.Result.IsNullOrEmpty() ? "No" : data.Result;
                }
                hearingId = data.HearingID.Value;
                displayData.Add(data);
            }
            var responseData = displayData.Select(x => new
            {
                EventDate = x.EventDate,
                EventTime = x.EventTime,
                EventType = x.HearingType,
                Department = x.Department,
                Petitions = x.Petitions,
                Clients = x.Clients,
                Result = x.Result,
                Attorney = x.AgencyAttorneyPersonNameFirst + " " + x.AgencyAttorneyPersonNameLast,
                CaseID = x.CaseID,
                EncryptedCaseID = x.CaseID.ToEncrypt(),
                HearingID = x.HearingID,
                EncryptedHearingID = x.HearingID.ToEncrypt(),
                EncryptedAgencyAttorneyID = x.AgencyAttorneyID.ToEncrypt()

            }).ToList();

            var dt = new List<DataTablesResponse> { new DataTablesResponse(0, responseData, hearingCount, hearingCount) };
          

            return Json(dt);
        }

        [HttpPost]
        public virtual ActionResult PrintMasterCalendar(MasterCalendarViewModel viewModel)
        {

            var com_ReportParameterHeaderDelete_spParams = new com_ReportParameterHeaderDelete_spParams()
            {
                ReportID = 91,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            List<object> deletedHeaderValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderDelete_sp", com_ReportParameterHeaderDelete_spParams).ToList();

            //start date
            var reportHeaderParams = new com_ReportParameterHeaderInsert_spParams()
            {
                ReportID = 91,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                JcatsUserID = UserManager.UserExtended.UserID,
                ReportParameterHeaderName = "Start Date",
                ReportParameterHeaderValue = viewModel.StartDate,
                RecordStateID = 1
            };
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterHeaderInsert_sp", reportHeaderParams);

            //end date
            reportHeaderParams.ReportParameterHeaderName = "End Date";
            reportHeaderParams.ReportParameterHeaderValue = viewModel.EndDate;
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterHeaderInsert_sp", reportHeaderParams);

            var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 91,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };
            ReportClass rpt = new ReportClass();
            string fileName = "MasterCalendar_" + UserManager.UserExtended.UserID + ".pdf";
            try
            {
                var spResult = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", com_ReportSourceGetByReportID_spParams).ToList();

                rpt.FileName = HttpContext.Server.MapPath("~/Reports/MasterCalendar.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("dbo.rpt_MasterCalendarPrintableVersion_sp", com_ReportSourceGetByReportID_spParams);
                rpt.SetDataSource(table);
                foreach (var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != "MasterCalendar.rpt"))
                {
                    var subTableData = (UtilityService.ExecStoredProcedureForDataTable(subRptDt.ReportSourceStoredProcedureName.Replace("dbo.", ""), com_ReportSourceGetByReportID_spParams));
                    rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(subTableData);
                }


                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + fileName);

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

            return new LALoDep.Custom.Actions.DownloadActionResult(fileName);

        }

        [HttpPost]
        public virtual ActionResult PrintWithPhysicalFile(MasterCalendarViewModel viewModel)
        {

            var com_ReportParameterHeaderDelete_spParams = new com_ReportParameterHeaderDelete_spParams()
            {
                ReportID = 96,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            List<object> deletedHeaderValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderDelete_sp", com_ReportParameterHeaderDelete_spParams).ToList();

            //start date
            var reportHeaderParams = new com_ReportParameterHeaderInsert_spParams()
            {
                ReportID = 96,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                JcatsUserID = UserManager.UserExtended.UserID,
                ReportParameterHeaderName = "Start Date",
                ReportParameterHeaderValue = viewModel.StartDate,
                RecordStateID = 1
            };
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterHeaderInsert_sp", reportHeaderParams);

            //end date
            reportHeaderParams.ReportParameterHeaderName = "End Date";
            reportHeaderParams.ReportParameterHeaderValue = viewModel.EndDate;
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterHeaderInsert_sp", reportHeaderParams);

            var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 96,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };
            ReportClass rpt = new ReportClass();
            string fileName = "MasterCalendar_" + "PetitionFileName" + ".pdf";
            try
            {
                var spResult = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", com_ReportSourceGetByReportID_spParams).ToList();

                rpt.FileName = HttpContext.Server.MapPath("~/Reports/MasterCalendarWithPetitionFileName.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("dbo.rpt_MasterCalendarPrintableVersion_sp", com_ReportSourceGetByReportID_spParams);
                rpt.SetDataSource(table);
                foreach (var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != "MasterCalendarWithPetitionFileName.rpt"))
                {
                    var subTableData = (UtilityService.ExecStoredProcedureForDataTable(subRptDt.ReportSourceStoredProcedureName.Replace("dbo.", ""), com_ReportSourceGetByReportID_spParams));
                    rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(subTableData);
                }



                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + fileName);

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


            return new LALoDep.Custom.Actions.DownloadActionResult(fileName);
        }




        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
    }
}