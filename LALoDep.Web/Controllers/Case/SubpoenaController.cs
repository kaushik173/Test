using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aspose.Words;
using DocumentFormat.OpenXml.Drawing;
using LALoDep.Domain.pd_Case;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using LALoDep.Domain.pd_Role;
using DataTables.Mvc;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain.pd_Code;
using LALoDep.Core.Enums;
using LALoDep.Models;
using LALoDep.Domain.pd_Person;
using LALoDep.Custom;
using LALoDep.Domain.pd_Address;
using LALoDep.Domain.pd_Subpoena;
using LALoDep.Domain.pd_Hearing;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ViewSubpoenaPage, PageSecurityItemID = SecurityToken.ViewSubpoena)]
        public virtual ActionResult SubpoenaList()
        {
            var viewModel = new SubpoenaListViewModel();

            if (UserManager.UserExtended.CaseID > 0)
            {
                viewModel.CanEditSubpoena = UserManager.IsUserAccessTo(SecurityToken.EditSubpoena);

                var pd_HearingGetByCaseID_spParams = new LALoDep.Domain.pd_Case.pd_HearingGetByCaseID_spParams()
                {
                    BatchLogJobID = Guid.NewGuid(),
                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    UnresultedFlag = 0
                };
                viewModel.HearingList = UtilityService.ExecStoredProcedureWithResults<pd_HearingGetByCaseID_spResult>("pd_HearingGetByCaseID_sp", pd_HearingGetByCaseID_spParams).Select(x => new CodeViewModel()
                {
                    CodeID = x.HearingID,
                    CodeValue = x.HearingTypeCodeShortValue + " " + x.HearingDateTime + " " + x.HearingCourtDepartmentCodeValue
                }).ToList();

                var pd_SubpoenaGetByCaseID_spParams = new pd_SubpoenaGetByCaseID_spParams()
                {
                    BatchLogJobID = Guid.NewGuid(),
                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID
                };
                viewModel.SubpoenaList = UtilityService.ExecStoredProcedureWithResults<pd_SubpoenaGetByCaseID_spResult>("pd_SubpoenaGetByCaseID_sp", pd_SubpoenaGetByCaseID_spParams).ToList();

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ViewSubpoenaPage, PageSecurityItemID = SecurityToken.DeleteSubpoena)]
        public virtual JsonResult SubpoenaDelete(string id)
        {

            var pd_SubpoenaDelete_spParams = new pd_SubpoenaDelete_spParams()
            {
                ID = id.ToDecrypt().ToInt(),
                LoadOption = "Subpoena",
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_SubpoenaDelete_sp", pd_SubpoenaDelete_spParams).ToList();

            return Json(new { isSuccess = true });
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ViewSubpoenaPage, PageSecurityItemID = SecurityToken.EditSubpoena)]
        public virtual JsonResult SubpoenaListSave(SubpoenaListViewModel viewModel)
        {
            foreach (var item in viewModel.SubpoenaList)
            {
                var pd_SubpoenaUpdate_spParams = new pd_SubpoenaUpdate_spParams()
                {
                    SubpoenaID = item.SubpoenaID.Value,
                    AgencyID = item.AgencyID.Value,
                    HearingID = item.HearingID.Value,
                    SubpoenaServedToRoleID = item.SubpoenaServedToRoleID.Value,
                    SubpoenaServedDate = item.SubpoenaServedDate.ToDateTime(),
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };

                UtilityService.ExecStoredProcedureWithResults<object>("pd_SubpoenaUpdate_sp", pd_SubpoenaUpdate_spParams).FirstOrDefault();
            }


            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ViewSubpoenaPage, PageSecurityItemID = SecurityToken.AddSubpoena)]
        public virtual ActionResult SubpoenaAddEdit(string id)
        {
            if (UserManager.UserExtended.CaseID > 0)
            {
                var viewModel = new SubpoenaAddEditViewModel();
                var pd_HearingGet_spParams = new pd_HearingGet_spParams()
                {
                    HearingID = id.ToDecrypt().ToInt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                };
                var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp", pd_HearingGet_spParams).FirstOrDefault();
                if (hearingInfo != null)
                {
                    viewModel.HearingID = hearingInfo.HearingID;
                    viewModel.AgencyID = hearingInfo.AgencyID;
                    viewModel.HearingType = hearingInfo.HearingType;
                    viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToString();
                    viewModel.HearingDept = hearingInfo.HearingDept;
                }

                var pd_RoleGetForSubpoenaByCaseID_spParams = new pd_RoleGetForSubpoenaByCaseID_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                viewModel.SubpoenaList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForSubpoenaByCaseID_spResult>("pd_RoleGetForSubpoenaByCaseID_sp", pd_RoleGetForSubpoenaByCaseID_spParams).ToList();

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }

        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.ViewSubpoenaPage, PageSecurityItemID = SecurityToken.AddSubpoena)]
        public virtual JsonResult SubpoenaAddEditSave(SubpoenaAddEditViewModel viewModel)
        {
            var subpoenaID = 0;
            foreach (var item in viewModel.SubpoenaList)
            {
                //For each selected box 
                if (item.PersonID != 0)
                {
                    var pd_SubpoenaInsert_spParam = new pd_SubpoenaInsert_spParams()
                    {
                        AgencyID = viewModel.AgencyID.Value,
                        HearingID = viewModel.HearingID.Value,
                        SubpoenaServedToRoleID = item.RoleID.Value,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    subpoenaID = UtilityService.ExecStoredProcedureWithResults<int>("pd_SubpoenaInsert_sp", pd_SubpoenaInsert_spParam).FirstOrDefault();
                    foreach (var address in viewModel.SubpoenaList)
                    {
                        //For each address 
                        if (address.PersonAddressID != 0)
                        {
                            var pd_SubpoenaAddressInsert_spParams = new pd_SubpoenaAddressInsert_spParams()
                            {
                                AgencyID = viewModel.AgencyID.Value,
                                SubpoenaID = subpoenaID,
                                PersonAddressID = address.PersonAddressID.Value,
                                RecordStateID = 1,
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid()
                            };
                            var subpoenaaddressID = UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_SubpoenaAddressInsert_sp", pd_SubpoenaAddressInsert_spParams).FirstOrDefault();
                        }
                    }
                }

            }
            return Json(new { isSuccess = true });
        }

        [HttpPost]



        public virtual ActionResult SubpoenaPrint(string id)
        {
            var comReportSourceGetByReportIdSpParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 57,
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


            #region Insert Report Parameter
            /*Insert New Report Parameters saved for this User*/

            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueInsert_sp",
                new Dictionary<string, object>()
                {
                    {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                    {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                    {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
                    ,
                    {"@ReportParameterValueID", null},
                    {"@Sequence", 1}
                    ,
                    {"@Value", id.IsNullOrEmpty()?null:id}//Subpoena IDs
                });

            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueInsert_sp",
               new Dictionary<string, object>()
                {
                    {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                    {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                    {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
                    ,
                    {"@ReportParameterValueID", null},
                    {"@Sequence", 2}
                    ,
                    {"@Value", UserManager.UserExtended.CaseID}//Case Number
                });
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueInsert_sp",
             new Dictionary<string, object>()
                {
                    {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                    {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                    {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
                    ,
                    {"@ReportParameterValueID", null},
                    {"@Sequence", 3}
                    ,
                    {"@Value", null}//HearingID
                });
            #endregion

            var dictionaryParamForSource = new Dictionary<string, object>()
            {
                {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID},
                {"@CaseID", UserManager.UserExtended.CaseID }
            };

            var spResult =
              UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>(
                  "com_ReportSourceGetByReportID_sp", dictionaryParamForSource).ToList();

            var reportSource = spResult.FirstOrDefault();



            if (reportSource != null)
            {
                if (reportSource.ReportSourceDocumentName.Contains(".doc"))
                {
                    var reportSourceStoredProcedureName = reportSource.ReportSourceStoredProcedureName.Replace("dbo.", "");

                    var mergeDataTAble = UtilityService.ExecStoredProcedureForDataTable(reportSourceStoredProcedureName, comReportSourceGetByReportIdSpParams);


                    if (mergeDataTAble != null && mergeDataTAble.Rows.Count > 0)
                    {
                        string[] fieldNames = (from object column in mergeDataTAble.Columns select column.ToString()).ToArray();
                        var mergeTemplateRootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["MergeTemplateRootPath"];

                        var templatePath = mergeTemplateRootPath + reportSource.ReportSourceDocumentName;
                        if (reportSource.UseMasterFlag != 1)
                            templatePath = mergeTemplateRootPath + reportSource.AgencyMergeTemplatePath + @"\" + reportSource.ReportSourceDocumentName;

                        // get mail merge document in byte[]
                        var template = System.IO.File.ReadAllBytes(templatePath);

                        var doc = new Document();
                        var filename = spResult[0].ReportDisplayName + ".doc";
                        filename = filename.Replace("&", "_");
                        filename = filename.Replace(",", "_");
                        filename = filename.Replace("(", "_");
                        filename = filename.Replace(")", "_");
                        filename = filename.Replace("/", "_");
                        doc.RemoveAllChildren();
                        for (var i = 0; i < mergeDataTAble.Rows.Count; i++)
                        {
                            var fieldValues = mergeDataTAble.Rows[i].ItemArray;

                            // get mail merge document in byte[]

                            var generated = Utility.ExecuteMergeDocument(template, fieldNames, fieldValues);

                            var steam = new MemoryStream(generated);
                            var srcDoc = new Document(steam);
                            doc.AppendDocument(srcDoc, ImportFormatMode.KeepSourceFormatting);
                            doc.Save(UtilityFunctions.GetDocumentDownloadFolderPath() + filename);
                        }

                        var generatedFilePath = UtilityFunctions.GetDocumentDownloadFolderPath() + filename;





                        return File(generatedFilePath, "application/force-download", filename);
                    }

                    else
                    {
                        return Json(new { errorMessage = "No records found" }, JsonRequestBehavior.AllowGet);
                    }
                }

            }


            return Json(new { errorMessage = "No records found" }, JsonRequestBehavior.AllowGet);


        }


    }
}