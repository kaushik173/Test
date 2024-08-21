using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
using LALoDep.Domain.AddEditCountyCounsel;
using LALoDep.Domain.pd_Petition;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Address;
using LALoDep.Domain.pd_Association;
using LALoDep.Models;
using LALoDep.Custom;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_Calendar;
using LALoDep.Domain.CaseAttribute;
using LALoDep.Domain.sup_Case;
using LALoDep.Domain.pD_SCCInvoice;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain;
using LALoDep.Domain.NG_com;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        #region Main Page
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseMainPage, PageSecurityItemID = SecurityToken.ViewCaseMainPage)]
        public virtual ActionResult Main(string id)
        {
            pd_CaseGet_sp_Result oCase;
            var caseId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                caseId = id.ToDecrypt().ToInt();
                oCase = UtilityService.Context.pd_CaseGet_sp(caseId, UserManager.UserExtended.UserID, Guid.NewGuid())
                        .FirstOrDefault();
                if (oCase != null)
                {
                    var flag = UserManager.UpdateCaseStatusBar(caseId, oCase: oCase);
                    if (!flag)
                    {
                        return Content("Redirecting");
                    }
                }
            }
            else
            {
                caseId = UserManager.UserExtended.CaseID;
                oCase = UtilityService.Context.pd_CaseGet_sp(caseId, UserManager.UserExtended.UserID, Guid.NewGuid())
                   .FirstOrDefault();
                if (oCase != null)
                {
                    var flag = UserManager.UpdateCaseStatusBar(caseId, oCase: oCase);
                    if (!flag)
                    {
                        return Content("Redirecting");
                    }
                }
            }


            var model = new CaseMainViewModel();
            if (oCase != null)
            {

                model.CaseFiles =
                    UtilityService.ExecStoredProcedureWithResults<CaseFileGetByCaseID_spResult>(
                        "NG_CaseFileGetByCaseID_sp",
                        new CaseFileGetByCaseID_spParams
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            CaseID = caseId,
                            UserID = UserManager.UserExtended.UserID,
                            SortOption = "FileDate"

                        }).ToList();
                model.RelatedCasesGetList =
                  UtilityService.ExecStoredProcedureWithResults<pd_RelatedCasesGetList_spResult>(
                      "pd_RelatedCasesGetList_sp",
                      new pd_RelatedCasesGetList_spParams
                      {
                          BatchLogJobID = Guid.NewGuid(),
                          CaseID = caseId,
                          UserID = UserManager.UserExtended.UserID,


                      }).ToList();

                model.PetitionAndAllegation =
                    UtilityService.ExecStoredProcedureWithResults<pd_PetitionAndAllegationGetByCaseID_spResult>(
                        "pd_PetitionAndAllegationGetByCaseID_sp",
                        new pd_PetitionAndAllegationGetByCaseID_spParams
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            CaseID = caseId,
                            UserID = UserManager.UserExtended.UserID

                        }).ToList();
                model.Hearing =
                    UtilityService.ExecStoredProcedureWithResults<pd_HearingGetByCaseID_spResult>(
                        "pd_HearingGetByCaseID_sp",
                        new LALoDep.Domain.pd_Case.pd_HearingGetByCaseID_spParams
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            CaseID = caseId,
                            UserID = UserManager.UserExtended.UserID,
                            UnresultedFlag = 1

                        }).ToList();
                model.HearingPersons =
                    UtilityService.ExecStoredProcedureWithResults<pd_HearingPersonsGetByCaseID_spResult>(
                        "pd_HearingPersonsGetByCaseID_sp",
                        new pd_HearingPersonsGetByCaseID_spParams
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            CaseID = caseId,
                            UserID = UserManager.UserExtended.UserID


                        }).ToList();
                model.CaseRole = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseID_spResult>(
                       "pd_RoleGetByCaseID_sp",
                       new pd_RoleGetByCaseID_spParams
                       {
                           BatchLogJobID = Guid.NewGuid(),
                           CaseID = caseId,
                           UserID = UserManager.UserExtended.UserID


                       }).ToList();


                model.CaseGet = oCase;

            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return View(model);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseMainPage, PageSecurityItemID = SecurityToken.ViewCaseMainPage)]
        public virtual ActionResult LoadHearingList(int caseId, int unresultedFlag)
        {

            var model = new CaseMainViewModel();
            if (caseId > 0)
            {


                model.Hearing = UtilityService.ExecStoredProcedureWithResults<pd_HearingGetByCaseID_spResult>("pd_HearingGetByCaseID_sp",
                        new LALoDep.Domain.pd_Case.pd_HearingGetByCaseID_spParams
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            CaseID = caseId,
                            UserID = UserManager.UserExtended.UserID,
                            UnresultedFlag = unresultedFlag

                        }).ToList();
                model.HearingPersons = UtilityService.ExecStoredProcedureWithResults<pd_HearingPersonsGetByCaseID_spResult>("pd_HearingPersonsGetByCaseID_sp",
                     new pd_HearingPersonsGetByCaseID_spParams
                     {
                         BatchLogJobID = Guid.NewGuid(),
                         CaseID = caseId,
                         UserID = UserManager.UserExtended.UserID


                     }).ToList();

            }
            return PartialView("_partialHearingListForMainCase", model);
        }

        [HttpPost]
        public virtual ActionResult PrintMain(string id)
        {

            var com_ReportParameterValueDelete_spParams = new com_ReportParameterValueDelete_spParams()
            {
                ReportID = 54,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            List<object> valueDelete = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueDelete_sp", com_ReportParameterValueDelete_spParams).ToList();
            List<object> headerDelete = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderDelete_sp", com_ReportParameterValueDelete_spParams).ToList();

            if (!string.IsNullOrEmpty(id))
            {
                var com_ReportParameterValueInsert_spParams = new com_ReportParameterValueInsert_spParams()
                {
                    ReportID = 54,
                    Sequence = 1,
                    Value = id.ToDecrypt(),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueInsert_sp", com_ReportParameterValueInsert_spParams).ToList();
            }


            var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 54,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };
            ReportClass rpt = new ReportClass();

            string fileName = "CaseMain_PrintableVersion_" + id + ".pdf";
            try
            {


                var spResult = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", com_ReportSourceGetByReportID_spParams).ToList();

                rpt.FileName = HttpContext.Server.MapPath("~/Reports/rptCaseSummaryMain.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("dbo.rpt_CaseSummaryMain_sp", com_ReportSourceGetByReportID_spParams);
                rpt.SetDataSource(table);
                foreach (var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != "rptCaseSummaryMain.rpt"))
                {
                    var subTableData = (UtilityService.ExecStoredProcedureForDataTable(subRptDt.ReportSourceStoredProcedureName.Replace("dbo.", ""), com_ReportSourceGetByReportID_spParams));
                    rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(subTableData);
                }


                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + fileName);

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


            return new LALoDep.Custom.Actions.DownloadActionResult(fileName);
        }

        [HttpPost]
        public virtual ActionResult PrintPetition(string id)
        {
            var petitionId = id.ToDecrypt().ToInt();

            var rpt = new ReportClass();
            try
            {
                var spResult = new List<com_ReportSourceGetByReportID_spResult>
            {
                new com_ReportSourceGetByReportID_spResult()
                {
                    ReportDisplayName = "CaseSummary",
                    ReportSourceStoredProcedureName = "rpt_CaseSummary_sp",
                    ReportSourceDocumentName = "CaseSummary.rpt",
                    ReportSourceSequence = 1
                },
                new com_ReportSourceGetByReportID_spResult()
                {
                    ReportDisplayName = "CaseSummaryChildren",
                    ReportSourceStoredProcedureName = "rpt_CaseSummaryChildren_sp",
                    ReportSourceDocumentName = "rptCaseSummaryChildren.rpt",
                    ReportSourceSequence = 2
                },
                new com_ReportSourceGetByReportID_spResult()
                {
                    ReportDisplayName = "CaseSummaryNextHearings",
                    ReportSourceStoredProcedureName = "rpt_CaseSummaryNextHearings_sp",
                    ReportSourceDocumentName = "rptCaseSummaryNextHearings.rpt",
                    ReportSourceSequence = 3
                },
                new com_ReportSourceGetByReportID_spResult()
                {
                    ReportDisplayName = "CaseSummaryParents",
                    ReportSourceStoredProcedureName = "rpt_CaseSummaryParents_sp",
                    ReportSourceDocumentName = "CaseSummaryParents.rpt",
                    ReportSourceSequence = 4
                },
                new com_ReportSourceGetByReportID_spResult()
                {
                    ReportDisplayName = "CaseSummaryOther",
                 ReportSourceStoredProcedureName    = "rpt_CaseSummaryOther_sp",
                 ReportSourceDocumentName    = "CaseSummaryOther.rpt",
                    ReportSourceSequence = 5
                },
                //new com_ReportSourceGetByReportID_spResult()
                //{
                //    ReportDisplayName = "CaseSummaryAssociation",
                // ReportSourceStoredProcedureName    = "rpt_CaseSummaryOther_sp",
                // ReportSourceDocumentName    = "CaseSummaryAssociation.rpt",
                //    ReportSourceSequence = 5
                //}
            };



                var parameters = new rpt_CaseSummary_spParams
                {
                    BatchLogJobID = Guid.NewGuid(),
                    PetitionID = petitionId,
                    UserID = UserManager.UserExtended.UserID

                };
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/CaseSummary.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("dbo.rpt_CaseSummary_sp", parameters);
                rpt.SetDataSource(table);
                foreach (var subRptDt in spResult.Where(c => c.ReportSourceDocumentName != "CaseSummary.rpt"))
                {
                    if (subRptDt.ReportDisplayName == "CaseSummaryNextHearings")
                    {

                        var subreport = rpt.Subreports[subRptDt.ReportSourceDocumentName];
                        if (subreport != null)
                        {
                            var subTableData = (UtilityService.ExecStoredProcedureForDataTable(subRptDt.ReportSourceStoredProcedureName.Replace("dbo.", ""), new rpt_CaseSummaryNextHearings_spParams
                            {
                                BatchLogJobID = Guid.NewGuid(),
                                PetitionID = petitionId,
                                UserID = UserManager.UserExtended.UserID
                            }));
                            subreport.SetDataSource(subTableData);
                        }
                    }
                    else
                    {
                        var subreport = rpt.Subreports[subRptDt.ReportSourceDocumentName];
                        if (subreport != null)
                        {
                            var subTableData = (UtilityService.ExecStoredProcedureForDataTable(subRptDt.ReportSourceStoredProcedureName.Replace("dbo.", ""), parameters));

                            subreport.SetDataSource(subTableData);
                        }
                    }

                }
                var fileName = "CaseSummary" + id + ".pdf";

                if (UserManager.UserExtended.PrintDocumentOn == "NewWindow")
                {
                    var filePath = UtilityFunctions.GetDocumentDownloadFolderPath() + fileName;
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);
                    rpt.Close();
                    rpt.Dispose();
                    GC.Collect();

                    return RedirectToAction("Preview", "Home", new { path = Utility.Encrypt(UtilityFunctions.GetDocumentDownloadFolderRelativePath() + fileName) });
                }

                var stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
                return File(stream, "application/pdf", fileName);
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


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CaseMainPage, PageSecurityItemID = SecurityToken.DeleteRole)]
        [HttpPost]
        public virtual JsonResult RoleDelete(string id)
        {
            var pd_RoleDelete_spParams = new pd_RoleDelete_spParams()
            {
                ID = id.ToDecrypt().ToInt(),
                RecordStateID = 10,
                LoadOption = "Role",
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = new Guid()
            };
            UtilityService.ExecStoredProcedureWithResults<object>("pd_RoleDelete_sp", pd_RoleDelete_spParams).ToList();

            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CaseMainPage, PageSecurityItemID = SecurityToken.DeletePetition)]
        [HttpPost]
        public virtual JsonResult PetitionDelete(string id)
        {
            UtilityService.ExecStoredProcedureWithResults<object>("pd_PetitionDelete_sp", new pd_PetitionDelete_spParams
            {
                ID = id.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                RecordStateID = 10,
                LoadOption = "Petition"
            }).ToList();
            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CaseMainPage, PageSecurityItemID = SecurityToken.DeleteHearing)]
        [HttpPost]
        public virtual JsonResult DeleteHearing(string id)
        {
            var hearingId = id.ToDecrypt().ToInt();
            //UtilityService.ExecStoredProcedureWithoutResultADO("pd_HearingDelete_sp", new pd_HearingDelete_spParams
            //{
            //    UserID = UserManager.UserExtended.UserID,
            //    BatchLogJobID = Guid.NewGuid(),
            //    ID = hearingId,
            //    RecordTimeStamp = null
            //});

            return Json(new { isSuccess = true });
        }
        #endregion Main Page

        #region Main Page->More Info on Person

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CaseMainInfoPages, PageSecurityItemID = SecurityToken.ViewPerson)]
        public virtual ActionResult MoreInfoOnPerson(string id, string pid)
        {
            var viewModel = new MoreInfoOnPersonViewModel();
            if (!id.IsNullOrEmpty())
            {
                //role
                var roleID = id.ToDecrypt().ToInt();
                var personId = pid.ToDecrypt().ToInt();

                viewModel.PersonRoles = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForPersonMoreInfo_spResult>("pd_RoleGetForPersonMoreInfo_sp", new pd_RoleGetForPersonMoreInfo_spParams
                {
                    RoleID = roleID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).ToList();

                //Address
                viewModel.Addresses = UtilityService.ExecStoredProcedureWithResults<pd_AddressGetForPersonMoreInfo_spResult>("pd_AddressGetForPersonMoreInfo_sp", new pd_AddressGetForPersonMoreInfo_spParams
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    RoleID = roleID,
                    PersonID = personId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).ToList();


                //Contact Info
                viewModel.Contacts = UtilityService.ExecStoredProcedureWithResults<pd_PersonContactGetByPersonID_spResult>("pd_PersonContactGetByPersonID_sp", new pd_PersonContactGetByPersonID_spParams()
                {
                    PersonID = personId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).ToList();

                //Legal Numbers
                viewModel.LegalNumbers = UtilityService.ExecStoredProcedureWithResults<pd_LegalNumberGetByPersonID_spResult>("pd_LegalNumberGetForPersonMoreInfo_sp", new pd_LegalNumberGetByPersonID_spParams()
                {
                    PersonID = personId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).ToList();

                viewModel.Associations = UtilityService.ExecStoredProcedureWithResults<pd_AssociationGetForPersonMoreInfo_spResult>("pd_AssociationGetForPersonMoreInfo_sp", new pd_AssociationGetForPersonMoreInfo_spParams
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    PersonID = personId,
                    RoleID = roleID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).ToList();


            }


            return View(viewModel);
        }

        #endregion More Info on Person

        #region Case Edit

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CaseMainCaseEditPages, PageSecurityItemID = SecurityToken.EditCase)]
        public virtual ActionResult CaseEdit()
        {
            var viewModel = new CaseEditViewModel();
            var pd_CaseGet_spParams = new pd_CaseGet_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                CaseID = UserManager.UserExtended.CaseID,
                BatchLogJobID = Guid.NewGuid()
            };

            var caseInfo = UtilityService.Context.pd_CaseGet_sp(UserManager.UserExtended.CaseID, UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
            if (caseInfo != null)
            {
                viewModel.CaseID = caseInfo.CaseID;
                viewModel.AgencyID = caseInfo.AgencyID;
                viewModel.CaseNumber = caseInfo.CaseNumber;
                viewModel.CaseAppointmentDate = caseInfo.CaseAppointmentDate.ToDefaultFormat();
                viewModel.CaseClosedDate = caseInfo.CaseClosedDate.ToDefaultFormat();
                viewModel.CasePanelCase = caseInfo.CasePanelCase.ToBoolean();
                viewModel.DepartmentID = caseInfo.DepartmentID;
                viewModel.CaseNameRoleID = caseInfo.CaseNameRoleID;
                viewModel.ReferralSourceCodeID = caseInfo.ReferralSourceCodeID;
                viewModel.RecordStateID = caseInfo.RecordStateID;
                viewModel.CaseSecuredID = caseInfo.CaseSecuredID;
                //viewModel.CaseSecured = caseInfo.CaseSecuredID >1;
                viewModel.CaseSecured = caseInfo.CaseSecuredID > 0;

            }

            viewModel.HasOpenPetitions = UtilityService.ExecStoredProcedureWithResults<pd_CaseClosePetitionGet_spResults>("pd_CaseClosePetitionGet_sp", new pd_CaseClosePetitionGet_spParams
            {
                CaseID = viewModel.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).Any(x => x.Resulted == 0);


            var caseAttrParams = new CaseAttributeGetByType_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                CaseID = UserManager.UserExtended.CaseID,
                CaseAttributeTypeID = 1000,
                TableID = UserManager.UserExtended.CaseID
            };

            var fileLocationAttr = UtilityService.ExecStoredProcedureWithResults<CaseAttributeGetByType_spResult<int>>("CaseAttributeGetByType_sp", caseAttrParams).FirstOrDefault();
            if (fileLocationAttr != null)
            {
                viewModel.FileLocationAttrID = fileLocationAttr.CaseAttributeID;
                viewModel.FileLocationID = fileLocationAttr.CaseAttributeGenericValue;
            }

            caseAttrParams.CaseAttributeTypeID = 1010;
            var fileBoxAttr = UtilityService.ExecStoredProcedureWithResults<CaseAttributeGetByType_spResult<string>>("CaseAttributeGetByType_sp", caseAttrParams).FirstOrDefault();
            if (fileBoxAttr != null)
            {
                viewModel.FileBoxAttrID = fileBoxAttr.CaseAttributeID;
                viewModel.FileBox = fileBoxAttr.CaseAttributeGenericValue;
            }

            caseAttrParams.CaseAttributeTypeID = 1020;
            var referralSourceAttr = UtilityService.ExecStoredProcedureWithResults<CaseAttributeGetByType_spResult<int>>("CaseAttributeGetByType_sp", caseAttrParams).FirstOrDefault();
            if (referralSourceAttr != null)
            {
                viewModel.ReferralSourceAttrID = referralSourceAttr.CaseAttributeID;
                viewModel.ReferralSourceCodeID = referralSourceAttr.CaseAttributeGenericValue;
            }

            //caseName
            viewModel.CaseName = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDChildRespondent_spResult>("pd_RoleGetByCaseIDChildRespondent_sp",
                                        new pd_RoleGetByCaseIDChildRespondent_spParams()
                                        {
                                            CaseID = UserManager.UserExtended.CaseID,
                                            LoadOption = "FirstLastRole",
                                            UserID = UserManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid()
                                        })
                                        .Select(x => new CodeViewModel() { CodeID = x.RoleID, CodeValue = x.DisplayName }).ToList();



            viewModel.IsSSCInvoiceExist = UtilityService.ExecStoredProcedureWithResults<pd_SCCInvoiceGetByCaseID_spResult>("pd_SCCInvoiceGetByCaseID_sp",
                                                                    new pd_SCCInvoiceGetByCaseID_spParams
                                                                    {
                                                                        CaseID = UserManager.UserExtended.CaseID,
                                                                        UserID = UserManager.UserExtended.UserID,
                                                                        BatchLogJobID = Guid.NewGuid()
                                                                    }).Any();

            //File Location
            viewModel.FileLocation = UtilityFunctions.CodeGetByTypeIdAndUserId(950, "CodeShortValue", viewModel.FileLocationID ?? 0, viewModel.AgencyID ?? 0);
            //Referatl Source 
            viewModel.ReferatlSource = UtilityFunctions.CodeGetByTypeIdAndUserId(81, "CodeValue", viewModel.ReferralSourceCodeID ?? 0, viewModel.AgencyID ?? 0);

            //department 
            viewModel.Department = UtilityFunctions.CodeGetByTypeIdAndUserId(30, "CodeShortValue", viewModel.DepartmentID ?? 0);

            return View(viewModel);
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CaseMainCaseEditPages, PageSecurityItemID = SecurityToken.EditCase)]
        [HttpPost]
        public virtual ActionResult CaseEdit(CaseEditViewModel viewModel)
        {
            #region Update Case
            if (viewModel.IsUpdateCase)
            {
                var caseParams = new pd_CaseUpdate_spParams
                {
                    AgencyID = viewModel.AgencyID ?? 0,
                    CaseID = viewModel.CaseID ?? 0,
                    CaseNumber = viewModel.CaseNumber,
                    CasePanelCase = (short)(viewModel.CasePanelCase ? 1 : 0),
                    DepartmentID = viewModel.DepartmentID ?? 0,

                    RecordStateID = (int)viewModel.RecordStateID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                };

                if (!string.IsNullOrEmpty(viewModel.CaseAppointmentDate))
                    caseParams.CaseAppointmentDate = DateTime.Parse(viewModel.CaseAppointmentDate);

                if (!string.IsNullOrEmpty(viewModel.CaseClosedDate))
                    caseParams.CaseClosedDate = DateTime.Parse(viewModel.CaseClosedDate);

                UtilityService.ExecStoredProcedureWithoutResultADO("pd_CaseUpdate_sp", caseParams);
            }
            #endregion Update Case

            #region Case Name
            if (viewModel.IsUpdateName)
            {
                UtilityService.ExecStoredProcedureWithResults<object>("pd_CaseUpdateCaseName_sp", new pd_CaseUpdateCaseName_spParams()
                {
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    CaseID = viewModel.CaseID.Value,
                    CaseNameRoleID = viewModel.CaseNameRoleID.Value
                }).FirstOrDefault();

                UtilityService.ExecStoredProcedureWithResults<object>("sup_CasePetitionNumberSet_sp", new sup_CasePetitionNumberSet_spParams()
                {
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    CaseID = viewModel.CaseID.Value,
                    AdminFlag = 0
                }).FirstOrDefault();
            }
            #endregion

            #region File Location
            if (viewModel.IsUpdateFileLocation)
            {
                if (viewModel.FileLocationAttrID.HasValue && viewModel.FileLocationID.HasValue)
                {
                    UtilityService.ExecStoredProcedureWithResults<object>("CaseAttributeUpdate_sp", new CaseAttributeUpdate_spParams()
                    {
                        CaseAttributeID = viewModel.FileLocationAttrID.Value,
                        CaseID = UserManager.UserExtended.CaseID,
                        CaseAttributeTypeID = 1000,
                        CaseAttributeGenericValue = viewModel.FileLocationID.ToString(),
                        TableID = viewModel.CaseID,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).FirstOrDefault();
                }
                else if (!viewModel.FileLocationAttrID.HasValue && viewModel.FileLocationID.HasValue)
                {
                    viewModel.FileLocationAttrID = UtilityService.ExecStoredProcedureWithResults<int>("CaseAttributeInsert_sp", new CaseAttributeInsert_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        CaseAttributeTypeID = 1000,
                        CaseAttributeGenericValue = viewModel.FileLocationID.ToString(),
                        TableID = viewModel.CaseID.Value,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        RecordStateID = 1
                    }).FirstOrDefault();

                }
                else if (viewModel.FileLocationAttrID.HasValue && !viewModel.FileLocationID.HasValue)
                {
                    UtilityService.ExecStoredProcedureWithoutResults("CaseAttributeDelete_sp", new CaseAttributeDelete_spParams()
                    {
                        ID = viewModel.FileLocationAttrID.Value,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    });
                }
            }
            #endregion File Location

            #region File Box
            if (viewModel.IsUpdateFileBox)
            {
                if (viewModel.FileBoxAttrID.HasValue && !string.IsNullOrEmpty(viewModel.FileBox))
                {
                    UtilityService.ExecStoredProcedureWithResults<object>("CaseAttributeUpdate_sp", new CaseAttributeUpdate_spParams()
                    {
                        CaseAttributeID = viewModel.FileBoxAttrID.Value,
                        CaseID = UserManager.UserExtended.CaseID,
                        CaseAttributeTypeID = 1010,
                        CaseAttributeGenericValue = viewModel.FileBox,
                        TableID = viewModel.CaseID,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).FirstOrDefault();
                }
                else if (!viewModel.FileBoxAttrID.HasValue && !string.IsNullOrEmpty(viewModel.FileBox))
                {
                    viewModel.FileBoxAttrID = UtilityService.ExecStoredProcedureWithResults<int>("CaseAttributeInsert_sp", new CaseAttributeInsert_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        CaseAttributeTypeID = 1010,
                        CaseAttributeGenericValue = viewModel.FileBox,
                        TableID = viewModel.CaseID.Value,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        RecordStateID = 1

                    }).FirstOrDefault();

                }
                else if (viewModel.FileBoxAttrID.HasValue && string.IsNullOrEmpty(viewModel.FileBox))
                {
                    UtilityService.ExecStoredProcedureWithoutResults("CaseAttributeDelete_sp", new CaseAttributeDelete_spParams()
                    {
                        ID = viewModel.FileBoxAttrID.Value,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    });
                }
            }
            #endregion File Box

            #region Referral Source
            if (viewModel.IsUpdateReferral)
            {
                if (viewModel.ReferralSourceAttrID.HasValue && viewModel.ReferralSourceCodeID.HasValue)
                {
                    UtilityService.ExecStoredProcedureWithResults<object>("CaseAttributeUpdate_sp", new CaseAttributeUpdate_spParams()
                    {
                        CaseAttributeID = viewModel.ReferralSourceAttrID.Value,
                        CaseID = UserManager.UserExtended.CaseID,
                        CaseAttributeTypeID = 1020,
                        CaseAttributeGenericValue = viewModel.ReferralSourceCodeID.ToString(),
                        TableID = viewModel.CaseID,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).FirstOrDefault();
                }
                else if (!viewModel.ReferralSourceAttrID.HasValue && viewModel.ReferralSourceCodeID.HasValue)
                {
                    viewModel.ReferralSourceAttrID = UtilityService.ExecStoredProcedureWithResults<int>("CaseAttributeInsert_sp", new CaseAttributeInsert_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        CaseAttributeTypeID = 1020,
                        CaseAttributeGenericValue = viewModel.ReferralSourceCodeID.ToString(),
                        TableID = viewModel.CaseID.Value,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        RecordStateID = 1

                    }).FirstOrDefault();

                }
                else if (viewModel.ReferralSourceAttrID.HasValue && !viewModel.ReferralSourceCodeID.HasValue)
                {
                    UtilityService.ExecStoredProcedureWithoutResults("CaseAttributeDelete_sp", new CaseAttributeDelete_spParams()
                    {
                        ID = viewModel.ReferralSourceAttrID.Value,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    });
                }
            }
            #endregion Referral Source

            if (viewModel.CaseSecured && viewModel.CaseSecuredID.ToInt() == 0)
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("CaseSecuredIUD_sp", new CaseSecuredIUD_spParams()
                {

                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    CaseSecuredID = viewModel.CaseSecuredID,
                    IUD = "INSERT"

                });
            }
            else if (viewModel.CaseSecuredID > 0 && !viewModel.CaseSecured)
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("CaseSecuredIUD_sp", new CaseSecuredIUD_spParams()
                {

                    CaseID = UserManager.UserExtended.CaseID,
                    UserID = UserManager.UserExtended.UserID,
                    CaseSecuredID = viewModel.CaseSecuredID,
                    IUD = "DELETE"

                });
            }



            //Update the case bar
            UserManager.UpdateCaseStatusBar(UserManager.UserExtended.CaseID);

            return Json(new { isSuccess = true });
        }


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CaseMainCaseEditPages, PageSecurityItemID = SecurityToken.CaseDelete)]
        [HttpPost]
        public virtual JsonResult CaseDelete()
        {
            UtilityService.ExecStoredProcedureWithResults<object>("pd_CaseDelete_sp", new pd_CaseDelete_spParams
            {
                ID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                RecordStateID = 10,
                LoadOption = "Cases"
            }).ToList();
            //clear session
            UserManager.ClearCaseStatusBar(UserManager.UserExtended.CaseID);
            return Json(new { isSuccess = true });
        }

        #endregion Case Edit

        #region Main Page->Plea

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.MainPleaPages, PageSecurityItemID = SecurityToken.ViewPlea)]
        public virtual ActionResult Plea(string id)
        {
            var viewModel = new PleaViewModel();

            var hearingId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                hearingId = id.ToDecrypt().ToInt();
                var pd_HearingGet_spParams = new pd_HearingGet_spParams()
                {
                    HearingID = hearingId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp", pd_HearingGet_spParams).FirstOrDefault();
                if (hearingInfo != null)
                {
                    viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToString();
                    viewModel.HearingDept = hearingInfo.HearingDept;
                    viewModel.HearingJudge = hearingInfo.HearingJudge;
                    viewModel.HearingType = hearingInfo.HearingTypeCodeValue;
                    viewModel.HearingID = hearingId;
                    viewModel.AgencyID = hearingInfo.AgencyID;
                    viewModel.CaseID = hearingInfo.CaseID;
                }

                viewModel.GloabalPlea = UtilityFunctions.CodeGetByTypeIdAndUserId(48);

                viewModel.HearingRespondentList = UtilityService.ExecStoredProcedureWithResults<pd_HearingPleaGetByHearingIDRespondent_spResult>("pd_HearingPleaGetByHearingIDRespondent_sp", pd_HearingGet_spParams).ToList();
                viewModel.HearingChildernList = UtilityService.ExecStoredProcedureWithResults<pd_HearingPleaGetByHearingIDChildren_spResult>("pd_HearingPleaGetByHearingIDChildren_sp", pd_HearingGet_spParams).ToList();

            }


            return View(viewModel);
        }


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.MainPleaPages, PageSecurityItemID = SecurityToken.EditPlea)]
        [HttpPost]
        public virtual ActionResult Plea(PleaViewModel viewModel)
        {

            foreach (var plea in viewModel.HearingRespondentList)
            {
                //use plea dropdown value
                int intPleaTypeID = plea.PleaTypeID ?? 0;

                // check for Respondent plea type is selected
                if (intPleaTypeID == 0 && viewModel.HearingRespondentGlobalList != null && viewModel.HearingRespondentGlobalList.Any())
                {
                    var globalPlea = viewModel.HearingRespondentGlobalList.FirstOrDefault(x => x.PersonID == plea.PersonID);
                    if (globalPlea != null)
                        intPleaTypeID = globalPlea.PleaTypeID ?? 0;
                }

                //check if global plea is selected
                if (intPleaTypeID == 0 && viewModel.GloabalPleaTypeCodeID.HasValue)
                    intPleaTypeID = viewModel.GloabalPleaTypeCodeID ?? 0;

                // if plea is selected any where then update it
                if (intPleaTypeID != 0)
                {
                    if (plea.PleaID.HasValue && plea.PleaID > 0) // Update plea
                    {
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingPleaUpdate_sp",
                                            new pd_HearingPleaUpdate_spParams
                                            {
                                                HearingPleaID = plea.PleaID,
                                                HearingID = viewModel.HearingID,
                                                PetitionRoleID = plea.PetitionRoleID,

                                                HearingPleaCodeID = intPleaTypeID,

                                                RecordStateID = 1,
                                                UserID = UserManager.UserExtended.UserID,
                                                BatchLogJobID = Guid.NewGuid()
                                            }).FirstOrDefault();
                    }
                    else // Add new plea
                    {
                        plea.PleaID = UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingPleaInsert_sp",
                                            new pd_HearingPleaInsert_spParams
                                            {
                                                HearingID = viewModel.HearingID,
                                                PetitionRoleID = plea.PetitionRoleID,

                                                HearingPleaCodeID = intPleaTypeID,

                                                RecordStateID = 1,
                                                UserID = UserManager.UserExtended.UserID,
                                                BatchLogJobID = Guid.NewGuid()
                                            }).FirstOrDefault().ToInt();
                    }
                }

            }
            return Json(new { isSuccess = true });
        }
        #endregion Main Page->Plea

        #region Main Page->Att

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AttendancePages, PageSecurityItemID = SecurityToken.ViewAttendance)]
        public virtual ActionResult Attendance(string id)
        {
            var viewModel = new AttendanceViewModel();

            var hearingId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                hearingId = id.ToDecrypt().ToInt();
                var pd_HearingGet_spParams = new pd_HearingGet_spParams()
                {
                    HearingID = hearingId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp", pd_HearingGet_spParams).FirstOrDefault();
                if (hearingInfo != null)
                {
                    viewModel.HearingID = hearingInfo.HearingID;
                    viewModel.AgencyID = hearingInfo.AgencyID;

                    viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToString();
                    viewModel.HearingDept = hearingInfo.HearingDept;
                    viewModel.HearingOfficer = hearingInfo.HearingJudge;
                    viewModel.HearingID = hearingInfo.HearingID;
                    viewModel.HearingType = hearingInfo.HearingTypeCodeValue;
                }
            }

            var noteInfo = UtilityFunctions.NoteGetByEntity(hearingId, 113, 125).FirstOrDefault();
            if (noteInfo != null)
            {
                viewModel.NoteID = noteInfo.NoteID;
                viewModel.NoteEntityCodeID = noteInfo.NoteEntityCodeID;
                viewModel.NoteEntityTypeCodeID = noteInfo.NoteEntityTypeCodeID;
                viewModel.EntityPrimaryKeyID = noteInfo.EntityPrimaryKeyID;
                viewModel.NoteTypeCodeID = noteInfo.NoteTypeCodeID;
                viewModel.NoteSubject = noteInfo.NoteSubject;
                viewModel.NoteEntry = noteInfo.NoteEntry;
                viewModel.NoteCaseID = noteInfo.CaseID;
                viewModel.NoteRecordStateID = noteInfo.RecordStateID;
            }

            var pd_RoleGetForHearingAttendingAttorney_spParams = new pd_RoleGetForHearingAttendingAttorney_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                HearingID = hearingId,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var appearingAttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForHearingAttendingAttorney_spResult>("pd_RoleGetForHearingAttendingAttorney_sp", pd_RoleGetForHearingAttendingAttorney_spParams).ToList();
            viewModel.AppearingAttorney = appearingAttorneyList.Select(x => new SelectListItem
            {
                Value = x.RoleID + "_" + x.PersonID + "_" + x.HearingAttendanceID,
                Text = x.PersonNameDisplay
            }).ToList();

            var appearingAttorney = appearingAttorneyList.FirstOrDefault(x => x.AttendingHearingFlag == 1);
            if (appearingAttorney != null)
            {
                viewModel.AppearingAttorneyID = appearingAttorney.RoleID + "_" + appearingAttorney.PersonID + "_" + appearingAttorney.HearingAttendanceID;

                viewModel.OldAppearingAttorneyID = appearingAttorney.PersonID;
                viewModel.HearingAttendanceID = appearingAttorney.HearingAttendanceID;
                viewModel.HearingAttandanceRoleID = appearingAttorney.RoleID;
            }

            var pd_HearingAttendanceGetByHearingID_spParams = new pd_HearingAttendanceGetByHearingID_spParams()
            {
                HearingID = hearingId,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                LoadOption = "HearingAttendance"
            };
            viewModel.HearingAttendance = UtilityService.ExecStoredProcedureWithResults<pd_HearingAttendanceGetByHearingID_spResult>("pd_HearingAttendanceGetByHearingID_sp", pd_HearingAttendanceGetByHearingID_spParams)
                                                    .Select(x => new HearingAttendanceListViewModel()
                                                    {
                                                        PersonNameDisplay = x.PersonNameDisplay,
                                                        PersonNameLast = x.PersonNameLast,
                                                        PersonNameFirst = x.PersonNameFirst,
                                                        RoleID = x.RoleID,
                                                        RoleTypeID = x.RoleTypeID,
                                                        RoleType = x.RoleType,
                                                        RoleClient = x.RoleClient,
                                                        AttendanceID = x.AttendanceID,
                                                        IsSelected = x.AttendedFlag == 1,
                                                        IsEditable = x.Editable == 1 ? true : false,

                                                    }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult AttendanceSave(AttendanceViewModel viewModel)
        {

            if (!viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteInsert(113, 125, viewModel.HearingID.Value, 16, null, viewModel.NoteEntry, viewModel.HearingID);
            }
            else if (viewModel.NoteID.HasValue && !string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteUpdate(viewModel.NoteID.Value, viewModel.NoteEntityCodeID.Value, viewModel.NoteEntityTypeCodeID.Value, viewModel.EntityPrimaryKeyID.Value,
                                                            viewModel.NoteTypeCodeID.Value, null, viewModel.NoteEntry, viewModel.HearingID);

            }
            else if (viewModel.NoteID.HasValue && string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                var pd_NoteDelete_spParams = new LALoDep.Domain.pd_Conflict.pd_NoteDelete_spParams()
                {
                    ID = viewModel.NoteID ?? 0,
                    RecordStateID = 10,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    LoadOption = "Note",
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteDelete_sp", pd_NoteDelete_spParams).ToList();
            }

            int roleId = 0;
            int? personId = null;
            int attandanceId = 0;
            if (!string.IsNullOrEmpty(viewModel.AppearingAttorneyID))
            {
                var hearingPerson = viewModel.AppearingAttorneyID.Split('_');
                roleId = hearingPerson[0].ToInt();
                personId = hearingPerson[1].ToInt();
                attandanceId = hearingPerson[2].ToInt();
            }

            //Attendance
            if (viewModel.HearingAttendance.Any())
            {
                foreach (var item in viewModel.HearingAttendance)
                {
                    if (item.IsSelected)
                    {
                        //insert
                        var pd_HearingAttendanceInsert_spParams = new pd_HearingAttendanceInsert_spParams()
                        {
                            HearingAttendanceID = item.AttendanceID.Value,
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                            HearingID = viewModel.HearingID.Value,
                            RoleID = item.RoleID.Value,
                            RecordStateID = 1,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        };

                        if (roleId == 0)
                            pd_HearingAttendanceInsert_spParams.NewAttendingAttorneyPersonID = personId;

                        var hearingAttendanceID = UtilityService.ExecStoredProcedureWithResults<int>("pd_HearingAttendanceInsert_sp", pd_HearingAttendanceInsert_spParams).FirstOrDefault();
                    }
                    else if (item.AttendanceID != 0 && !item.IsSelected)
                    {
                        //delete
                        var pd_HearingAttendanceDelete_spParams = new pd_HearingAttendanceDelete_spParams()
                        {
                            ID = item.AttendanceID.Value,
                            RecordStateID = 10,
                            LoadOption = "HearingAttendance",
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        };
                        var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingAttendanceDelete_sp", pd_HearingAttendanceDelete_spParams).ToList();
                    }
                }
            }

            if (viewModel.OldAppearingAttorneyID != personId)
            {

                if (viewModel.HearingAttendanceID != 0)
                {
                    //delete
                    var pd_HearingAttendanceDelete_spParams = new pd_HearingAttendanceDelete_spParams()
                    {
                        ID = viewModel.HearingAttendanceID,
                        RecordStateID = 10,
                        LoadOption = "HearingAttendance",
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingAttendanceDelete_sp", pd_HearingAttendanceDelete_spParams).ToList();
                }

                if (personId.HasValue && personId != 0)
                {
                    var pd_HearingAttendanceInsert_spParams = new pd_HearingAttendanceInsert_spParams()
                    {
                        AgencyID = viewModel.AgencyID ?? UserManager.UserExtended.CaseNumberAgencyID,
                        HearingID = viewModel.HearingID.Value,
                        RoleID = roleId,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };

                    if (roleId == 0)
                        pd_HearingAttendanceInsert_spParams.NewAttendingAttorneyPersonID = personId;

                    var hearingAttendanceID = UtilityService.ExecStoredProcedureWithResults<int>("pd_HearingAttendanceInsert_sp", pd_HearingAttendanceInsert_spParams).FirstOrDefault();
                }
            }

            return Json(new { isSuccess = true });
        }

        #endregion Main Page->Att

        #region Main Page->Pos


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.MainPosPages, PageSecurityItemID = SecurityToken.ViewCourtPosition)]
        public virtual ActionResult Pos(string id)
        {
            var viewModel = new PosViewModel();

            var hearingId = 0;
            if (!string.IsNullOrEmpty(id))
            {
                hearingId = id.ToDecrypt().ToInt();
                var hearingSP_Params = new pd_HearingGet_spParams()
                {
                    HearingID = hearingId,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var hearingInfo = UtilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp", hearingSP_Params).FirstOrDefault();
                if (hearingInfo != null)
                {
                    viewModel.HearingDateTime = hearingInfo.HearingDateTime.ToString();
                    viewModel.HearingDept = hearingInfo.HearingDept;
                    viewModel.HearingJudge = hearingInfo.HearingJudge;
                    viewModel.HearingID = hearingInfo.HearingID;
                    viewModel.HearingType = hearingInfo.HearingTypeCodeValue;
                }
                viewModel.PeopleOnHearingList = UtilityService.ExecStoredProcedureWithResults<pd_HearingOpinionGetByHearingID_spResult>("pd_HearingOpinionGetByHearingID_sp", hearingSP_Params).ToList();
            }


            return View(viewModel);
        }

        public virtual ActionResult HearingOpinion(string id, string opinionid, string enhearingID, string ennoteID)
        {
            var viewModel = new HearingOpinionViewModel();
            if (!string.IsNullOrEmpty(id))
                viewModel.RoleID = id.ToDecrypt().ToInt();

            if (!string.IsNullOrEmpty(opinionid))
                viewModel.HearingOpinionID = opinionid.ToDecrypt().ToInt();

            if (!string.IsNullOrEmpty(enhearingID))
                viewModel.HearingID = enhearingID.ToDecrypt().ToInt();

            if (!string.IsNullOrEmpty(ennoteID))
                viewModel.NoteID = ennoteID.ToDecrypt().ToInt();

            if (viewModel.NoteID > 0)
            {
                var opinionNote = UtilityFunctions.NoteGet(viewModel.NoteID.Value);
                viewModel.OpinionNote = opinionNote?.NoteEntry;
            }


            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult HearingOpinionSave(HearingOpinionViewModel viewModel)
        {
            if (viewModel.HearingOpinionID == 0 || viewModel.HearingOpinionID == null)
            {
                var pd_HearingOpinionInsert_spParams = new pd_HearingOpinionInsert_spParams()
                {
                    HearingID = viewModel.HearingID,
                    RoleID = viewModel.RoleID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var insertedID = (int)UtilityService.ExecStoredProcedureWithResults<decimal>("pd_HearingOpinionInsert_sp", pd_HearingOpinionInsert_spParams).FirstOrDefault();

                var pd_NoteInsert_spParams = new pd_NoteInsert_spParams()
                {
                    NoteEntitySystemValueTypeID = 118,
                    NoteEntityTypeSystemValueTypeID = 123,
                    EntityPrimaryKeyID = insertedID,
                    NoteEntry = viewModel.OpinionNote,
                    CaseID = UserManager.UserExtended.CaseID,
                    HearingID = viewModel.HearingID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var insertedNoteID = (int)UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", pd_NoteInsert_spParams).FirstOrDefault();
            }
            else
            {
                var pd_NoteUpdate_spParams = new del_NoteUpdate_spParams()
                {
                    NoteID = viewModel.NoteID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    NoteEntityCodeID = 3284,
                    NoteEntityTypeCodeID = 3288,
                    EntityPrimaryKeyID = viewModel.HearingOpinionID.Value,
                    NoteEntry = viewModel.OpinionNote,
                    CaseID = UserManager.UserExtended.CaseID,
                    HearingID = viewModel.HearingID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteUpdate_sp", pd_NoteUpdate_spParams).FirstOrDefault();

                var pd_HearingOpinionUpdate_spParams = new pd_HearingOpinionUpdate_spParams()
                {
                    HearingOpinionID = viewModel.HearingOpinionID.Value,
                    HearingID = viewModel.HearingID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_HearingOpinionUpdate_sp", pd_HearingOpinionUpdate_spParams).FirstOrDefault();
            }
            return Json(new { isSuccess = true });
        }

        #endregion Main Page->Pos

        #region Main Page->MoreInfoOnPetition

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.MoreInfoOnPetitionPages, PageSecurityItemID = SecurityToken.ViewPetition)]
        public virtual ActionResult MoreInfoOnPetition(string id)
        {
            var viewModel = new MoreInfoOnPetitionViewModel();
            var petitionID = 0;
            if (!id.IsNullOrEmpty())
            {
                petitionID = id.ToDecrypt().ToInt();

                var pd_PetitionGet_spParams = new pd_PetitionGet_spParams()
                {
                    PetitionID = petitionID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                var petitionInfo = UtilityService.ExecStoredProcedureWithResults<pd_PetitionGet_spResult>("pd_PetitionGet_sp", pd_PetitionGet_spParams).FirstOrDefault();
                if (petitionInfo != null)
                {
                    viewModel.PetitionID = petitionID;
                    viewModel.PetitionFileDate = (petitionInfo.PetitionFileDate != null) ? petitionInfo.PetitionFileDate.ToShortDateString() : string.Empty;
                    viewModel.CloseDate = (petitionInfo.CloseDate != null) ? petitionInfo.CloseDate.ToShortDateString() : string.Empty;
                    viewModel.PetitionDocketNumber = petitionInfo.PetitionDocketNumber;
                    viewModel.PetitionTypeCodeValue = petitionInfo.PetitionTypeCodeValue;
                    viewModel.Child = petitionInfo.FirstName + " " + petitionInfo.LastName;
                }

                viewModel.PetitionList = UtilityService.ExecStoredProcedureWithResults<pd_CalendarGetByPetitionID_spResult>("pd_CalendarGetByPetitionID_sp", pd_PetitionGet_spParams).Select(x => new PetitionCalendarListViewModel()
                {
                    HearingID = x.HearingID,
                    HearingType = x.HearingType,
                    ItemDate = (x.ItemDateTime != null) ? x.ItemDateTime.ToShortDateString() : string.Empty,
                    ItemTime = (x.ItemDateTime != null) ? (x.ItemDateTime.Value.ToShortTimeString() == "12:00 AM" ? string.Empty : x.ItemDateTime.Value.ToShortTimeString()) : string.Empty,
                    ItemType = x.ItemType
                }).ToList();
            }
            return View(viewModel);
        }

        #endregion Main Page->MoreInfoOnPetition



        public virtual ActionResult GoTo(string id)
        {
            var model = new GoToViewModel();
            model.HearingID = id.ToDecrypt().ToInt();
            model.NavigationList = UtilityService.ExecStoredProcedureWithResults<NG_GoToNavigation_spResult>("NG_GoToNavigation_sp", new NG_GoToNavigation_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                HearingID = model.HearingID,
                NG_AspPageName = "Case/Main",

                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).ToList();
            foreach (var item in model.NavigationList)
            {
                if (item.GoToURL == "Case/AddFindingsAndOrders")
                    item.GoToURL = "Case/FindingsAndOrders";
            }
            return View(model);

        }
    }
}