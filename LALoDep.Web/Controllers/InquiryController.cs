using LALoDep.Domain.pd_JcatsReport;
using LALoDep.Domain.Services;
using LALoDep.Custom;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Data;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Models;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Core.NG_sp.com_Report;
using LALoDep.Models.Inquiry;
using LALoDep.Domain.com_Report;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.Agency;
using LALoDep.Domain.pd_TrainingSummary;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.rpt_Print;
using Omu.ValueInjecter;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using LALoDep.Core.Custom.Utility;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class InquiryController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public InquiryController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        // GET: /Inquiry/Report

        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.ViewReports)]
        public virtual ActionResult Reports()
        {
            ViewBag.Title = "Reports";
            return View();
        }

        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.ViewReports)]
        public virtual JsonResult ReportTree(int ReportID)
        {
            var pd_JcatsReportGetByUserID_spParams = new pd_JcatsReportGetByUserID_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
            };

            var data = UtilityService.ExecStoredProcedureWithResults<pd_JcatsReportGetByUserID_spResults>("pd_JcatsReportGetByUserID_sp", pd_JcatsReportGetByUserID_spParams).Select(x => new
            {
                ReportID = x.JcatsReportID,
                JcatsReportName = x.JcatsReportName,
                ReportTypeCodeID = x.ReportTypeCodeID,
                JcatsReportDescription = x.JcatsReportDescription,
                ReportCategory = x.ReportCategory,
                SelectedFlag = x.SelectedFlag,
                EncryptedID = x.JcatsReportID.ToEncrypt(),
                ParmCount = x.ParmCount
            }).
                ToList();
            var selected = ReportID > 0 ? data.FirstOrDefault(o => o.ReportID == ReportID) : data.FirstOrDefault(o => o.SelectedFlag == 1);
            var reportCategories = data.Select(o => o.ReportCategory).Distinct();

            var groupedData = reportCategories.Select(reportGroup => new TreeViewModel
            {
                name = reportGroup,
                type = "folder",
                children = data.Where(o1 => o1.ReportCategory == reportGroup)
                    .Select(o1 => new TreeViewModel
                    {
                        name = string.Format(@"<a href='/Inquiry/ReportParameter/{0}' id='Report_{0}'  data-id='{3}' data-prm-count='{4}'> {1} </a></div><div class='desc'> {2}</div><div class='clearfix'></div>",
                        o1.EncryptedID,
                        o1.JcatsReportName,
                        o1.JcatsReportDescription, o1.ReportID, o1.ParmCount),
                        type = "item",
                        id = o1.ReportID
                    }).ToList(),
                selectedId = selected != null ? selected.ReportID : ReportID
            });
            //for select last selected report based on flag
            int selectedTree = 0;
            int reportID = 0;
            if (selected != null)
            {
                foreach (var item in groupedData)
                {
                    selectedTree++;
                    var itemSelect = item.children.Where(m => m.id == item.selectedId).ToList();
                    int ID = 0;
                    if (itemSelect.Count > 0)
                    {
                        var List = itemSelect.Where(m => m.id == item.selectedId).FirstOrDefault();
                        if (List != null) { ID = List.id; reportID = item.selectedId; break; }
                    }
                }
            }
            if (selectedTree == 0) { selectedTree = -1; }
            else { selectedTree = selectedTree - 1; }

            return Json(new { groupedData = groupedData, SelectedTree = selectedTree, ReportID = reportID }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Created By: JT Barrett
        /// Created On: 30 May, 2016
        /// Purpose: This action is to load report parameters
        /// Used: Respecitve page code from San Diego
        /// Last Updated On: 19 Aug, 2016
        /// Last Updated By: JT Barrett
        /// Reason: Added redirection to unique pages and checkbox multiselects
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual ActionResult ReportParameter(string ID, string PageID)
        {
            if (!string.IsNullOrEmpty(ID))
            {
                var decryptedID = ID.ToDecrypt().ToInt();

                //check for unique parameter pages
                if (decryptedID == 7)
                    return RedirectToAction("ReportCaseloadSummary", "Inquiry");
                else if (decryptedID == 13)
                    return RedirectToAction("ReportHearingSummary", "Inquiry");
                else if (decryptedID == 48)
                    return RedirectToAction("ReportRFDCaseloadSummary", "Inquiry");

                ViewBag.PageID = PageID.ToDecrypt().ToInt();
                var viewModel = new ReportParametersViewModel();
                var com_ReportParameterDefinitionGetByReportID_spParams = new NG_com_ReportParameterDefinitionGetByReportID_spParams()
                {
                    ReportID = decryptedID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };

                var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
                {
                    ReportID = decryptedID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid().ToString()

                };

                viewModel = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", com_ReportSourceGetByReportID_spParams).Select(c => (ReportParametersViewModel)(new ReportParametersViewModel()).InjectFrom(c)).FirstOrDefault();
                viewModel.ParameterDefinitionList = UtilityService.ExecStoredProcedureWithResults<NG_com_ReportParameterDefinitionGetByReportID_spResult>("com_ReportParameterDefinitionGetByReportID_sp", com_ReportParameterDefinitionGetByReportID_spParams).Select(c => (ReportParameterDefinition)(new ReportParameterDefinition()).InjectFrom(c)).ToList();
                if (viewModel.ParameterDefinitionList.Count == 0)
                {
                    return ReportParameter(viewModel);
                }
                foreach (var parameterDefinition in viewModel.ParameterDefinitionList)
                {
                    if (parameterDefinition.ControlName != null)
                    {
                        if (!parameterDefinition.Type.Equals("NEW COMBO") && !parameterDefinition.Type.Equals("MULTI SELECT"))
                        {
                            #region DDL


                            //Unique Lists
                            if (String.Equals(parameterDefinition.Caption, "Attorney"))
                            {
                                if (String.Equals(parameterDefinition.ReportName, "Attorney Information"))
                                {
                                    var pd_RoleGetByCaseIDAndSysVal_spParams = new pd_RoleGetByCaseIDAndSysVal_spParams()
                                    {
                                        CaseID = -1,
                                        SysVal = 10,
                                        UserID = UserManager.UserExtended.UserID,
                                        BatchLogJobID = Guid.NewGuid()
                                    };
                                    viewModel.AttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDAndSysVal_spResult>("pd_RoleGetByCaseIDAndSysVal_sp", pd_RoleGetByCaseIDAndSysVal_spParams).Select(c => (ReportParameterAttorneyListViewModel)(new ReportParameterAttorneyListViewModel()).InjectFrom(c)).ToList();

                                }
                                else
                                {
                                    var pd_RoleAgencyAttorneyGet_spParams = new pd_RoleAgencyAttorneyGet_spParams()
                                    {
                                        UserID = UserManager.UserExtended.UserID,
                                        BatchLogJobID = Guid.NewGuid()
                                    };
                                    viewModel.AttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_RoleAgencyAttorneyGet_spResult>("pd_RoleAgencyAttorneyGet_sp", pd_RoleAgencyAttorneyGet_spParams).Select(c => (ReportParameterAttorneyListViewModel)(new ReportParameterAttorneyListViewModel()).InjectFrom(c)).ToList();
                                }

                                var userInfo = UtilityService.ExecStoredProcedureWithResults<LALoDep.Domain.pd_Users.Edit.pd_JCATSUserGet_spResult>(
                                                         "pd_JCATSUserGet_sp", new LALoDep.Domain.pd_Users.Edit.pd_JCATSUserGet_spParams
                                                         {
                                                             JcatsUserID = UserManager.UserExtended.UserID,
                                                             UserID = UserManager.UserExtended.UserID,
                                                             BatchLogJobID = new Guid()
                                                         }).FirstOrDefault();
                                if (userInfo != null && userInfo.JcatsUserLevelCodeEnumName.Equals("LINE", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    parameterDefinition.DefaultValue = userInfo.PersonID.ToString();
                                    viewModel.AttorneyList = viewModel.AttorneyList.Where(x => x.PersonID == userInfo.PersonID).ToList();
                                }
                            }

                            if (String.Equals(parameterDefinition.Caption, "Agency"))
                            {
                                var AgencyGetByUserID_spParams = new AgencyGetByUserID_spParams()
                                {
                                    UserID = UserManager.UserExtended.UserID,
                                    SortOption = "AgencyName",
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<AgencyGetByUserID_spResult>("AgencyGetByUserID_sp", AgencyGetByUserID_spParams).Select(c => (AgencyModel)(new AgencyModel()).InjectFrom(c)).ToList();
                            }
                            if (String.Equals(parameterDefinition.Caption, "Agency Group"))
                            {
                                var AgencyGetGroupByUserID_spParams = new AgencyGetGroupByUserID_spParams()
                                {
                                    UserID = UserManager.UserExtended.UserID,
                                    SortOption = "AgencyGroup",
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.AgencyGroupList = UtilityService.ExecStoredProcedureWithResults<AgencyGetGroupByUserID_spResults>("AgencyGetGroupByUserID_sp", AgencyGetGroupByUserID_spParams).Select(c => (AgencyGroupModel)(new AgencyGroupModel()).InjectFrom(c)).ToList();
                            }
                            if (String.Equals(parameterDefinition.Caption, "County"))
                            {
                                var AgencyGetCountyByUserID_spParams = new AgencyGetCountyByUserID_spParams()
                                {
                                    UserID = UserManager.UserExtended.UserID,
                                    SortOption = "AgencyCounty",
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.CountyList = UtilityService.ExecStoredProcedureWithResults<AgencyGetCountyByUserID_spResult>("AgencyGetCountyByUserID_sp", AgencyGetCountyByUserID_spParams).Select(c => (AgencyCountyModel)(new AgencyCountyModel()).InjectFrom(c)).ToList();
                            }
                            if (String.Equals(parameterDefinition.Caption, "Region"))
                            {
                                var AgencyGetRegionByUserID_spParams = new AgencyGetRegionByUserID_spParams()
                                {
                                    UserID = UserManager.UserExtended.UserID,
                                    SortOption = "AgencyRegion",
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.RegionList = UtilityService.ExecStoredProcedureWithResults<AgencyGetRegionByUserID_spResult>("AgencyGetRegionByUserID_sp", AgencyGetRegionByUserID_spParams).Select(c => (AgencyRegionModel)(new AgencyRegionModel()).InjectFrom(c)).ToList();
                            }
                            if (String.Equals(parameterDefinition.ControlName, "Investigator"))
                            {
                                var pd_RoleAgencyInvestigatorGetByUserGroup_spParams = new pd_RoleAgencyInvestigatorGetByUserGroup_spParams()
                                {
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.InvestigatorList = UtilityService.ExecStoredProcedureWithResults<pd_RoleAgencyInvestigatorGetByUserGroup_spResult>("pd_RoleAgencyInvestigatorGetByUserGroup_sp", pd_RoleAgencyInvestigatorGetByUserGroup_spParams).Select(c => (ReportParameterInvestigatorListViewModel)(new ReportParameterInvestigatorListViewModel()).InjectFrom(c)).ToList();
                            }
                            if (String.Equals(parameterDefinition.Caption, "Code Type"))
                            {
                                var pd_CodeTypeGetForReport_spParams = new pd_CodeTypeGetForReport_spParams()
                                {
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.CodeTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeTypeGetForReport_spResult>("pd_CodeTypeGetForReport_sp", pd_CodeTypeGetForReport_spParams).Select(c => (CodeTypeViewModel)(new CodeTypeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == null && String.Equals(parameterDefinition.Caption, "Report Count Type"))
                            {
                                var pd_CodeGetAllBySystemValueTypeID_spParams = new pd_CodeGetAllBySystemValueTypeID_spParams()
                                {
                                    SystemValueTypeID = 232,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.ReportCountTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetAllBySystemValueTypeID_spResult>("pd_CodeGetAllBySystemValueTypeID_sp", pd_CodeGetAllBySystemValueTypeID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }

                            //Checkbox MultiSelects
                            if (String.Equals(parameterDefinition.Caption, "Designated Day"))
                            {
                                var pd_CodeGetAllBySystemValueTypeID_spParams = new pd_CodeGetAllBySystemValueTypeID_spParams()
                                {
                                    SystemValueTypeID = 231,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.DesignatedDayList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetAllBySystemValueTypeID_spResult>("pd_CodeGetAllBySystemValueTypeID_sp", pd_CodeGetAllBySystemValueTypeID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (String.Equals(parameterDefinition.Caption, "Only Active NMD/Special Needs"))
                            {
                                var pd_CodeGetAllBySystemValueTypeID_spParams = new pd_CodeGetAllBySystemValueTypeID_spParams()
                                {
                                    SystemValueTypeID = 242,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                parameterDefinition.MultiSelectList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetAllBySystemValueTypeID_spResult>("pd_CodeGetAllBySystemValueTypeID_sp", pd_CodeGetAllBySystemValueTypeID_spParams).Select(c => (CodeSelectedViewModel)(new CodeSelectedViewModel()).InjectFrom(c)).ToList();
                            }
                            if (String.Equals(parameterDefinition.Caption, "Phase"))
                            {
                                var pd_CodeGetAllBySystemValueTypeID_spParams = new pd_CodeGetAllBySystemValueTypeID_spParams()
                                {
                                    SystemValueTypeID = 211,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                parameterDefinition.MultiSelectList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetAllBySystemValueTypeID_spResult>("pd_CodeGetAllBySystemValueTypeID_sp", pd_CodeGetAllBySystemValueTypeID_spParams).Select(c => (CodeSelectedViewModel)(new CodeSelectedViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 6)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 6,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                parameterDefinition.MultiSelectList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeSelectedViewModel)(new CodeSelectedViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 66)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 66,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                parameterDefinition.MultiSelectList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeSelectedViewModel)(new CodeSelectedViewModel()).InjectFrom(c)).ToList();
                            }

                            // Code Lists
                            if (parameterDefinition.CodeType == 1)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 1,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.GenderList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 30)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 30,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.DepartmentList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 10)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 10,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.HearingTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 78)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 78,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.ServicePlanningAreaList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 81)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 81,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.ReferralSourceList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 1000)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 1000,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.ReportTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 1010)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 1010,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.RoleTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 1020)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 1020,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.ClientTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 1030)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 1030,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.QuarterList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 1040)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 1040,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.ReportTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 1050)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 1050,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.GroupingList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 22)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 22,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.AllegationList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 68)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 68,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.AllegationFindingList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 145)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 145,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.ReportCountTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 21)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 21,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.ARTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }
                            if (parameterDefinition.CodeType == 700)
                            {
                                var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
                                {
                                    CodeTypeID = 700,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                viewModel.SortBy = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();
                            }

                            #endregion
                        }
                        #region New COMBO TYPe
                        if (parameterDefinition.Type.Equals("NEW COMBO") || parameterDefinition.Type.Equals("MULTI SELECT"))// && (viewModel.ReportID == 140 || viewModel.ReportID == 133 || viewModel.ReportID == 136 || viewModel.ReportID == 139))
                        {
                            if (!parameterDefinition.SPName.IsNullOrEmpty())
                            {
                                var db = new DbManager();
                                if (!parameterDefinition.SPParms.IsNullOrEmpty())
                                {


                                    string[] myArray = parameterDefinition.SPParms.Split(new char[] { ',' });


                                    for (int i = 0; i < myArray.Length; i++)
                                    {
                                        var myArrayLeftValue = myArray[i].Split(new char[] { '=' })[0];
                                        var myArrayRightValue = myArray[i].Split(new char[] { '=' })[1];

                                        if (myArrayLeftValue == "CodeTypeID" && parameterDefinition.CodeType.HasValue)
                                        {
                                            myArrayRightValue = parameterDefinition.CodeType.Value.ToString();
                                        }
                                        else if (myArrayLeftValue == "ReportID" && viewModel.ReportID > 0)
                                        {
                                            myArrayRightValue = viewModel.ReportID.ToString();
                                        }
                                        else if (myArrayLeftValue == "SystemValueTypeID" && viewModel.ReportID > 0)
                                        {
                                            myArrayRightValue = parameterDefinition.SystemValueTypeID.ToInt().ToString();
                                        }
                                        db.AddInParam(myArrayLeftValue, myArrayRightValue);



                                    }
                                }
                                db.AddInParam("UserID", UserManager.UserExtended.UserID);
                                db.AddInParam("BatchLogJobID", Guid.NewGuid().ToString());
                                var dt = db.ExecuteDataTable(parameterDefinition.SPName);
                                if (parameterDefinition.Type.Equals("NEW COMBO"))
                                {
                                    var selectList = new List<SelectListItem>();
                                    if (dt.Rows.Count > 0)
                                    {

                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            selectList.Add(new SelectListItem()
                                            {
                                                Text = dr[parameterDefinition.SPValueFieldName].ToString(),
                                                Value = dr[parameterDefinition.SPIDFieldName].ToString()
                                            });
                                        }


                                    }
                                    parameterDefinition.ComboItemList = selectList;
                                    parameterDefinition.ComboSelectedValue = parameterDefinition.DefaultValue;
                                }
                                else
                                {
                                    var selectList = new List<CodeSelectedViewModel>();
                                    if (dt.Rows.Count > 0)
                                    {

                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            selectList.Add(new CodeSelectedViewModel()
                                            {
                                                CodeValue = dr[parameterDefinition.SPValueFieldName].ToString(),
                                                CodeID = dr[parameterDefinition.SPIDFieldName].ToInt()
                                            });
                                        }


                                    }
                                    parameterDefinition.MultiSelectList = selectList;



                                }


                            }
                        }

                        #endregion
                    }
                }

                if (viewModel.ReportSourceDocumentName.Contains(".rpt"))
                {
                    viewModel.ExportOption = 2;
                    ViewBag.PageName = "Report Parameters";
                }
                else
                {
                    viewModel.ExportOption = 1;
                    ViewBag.PageName = "Report Parameters";// "Merge Document Parameters";
                }
                viewModel.CaseNumber = UserManager.UserExtended.PDAPDNumber;
                viewModel.JcatsNumber = UserManager.UserExtended.CaseID.ToString();

                return View(viewModel);
            }
            else
            {
                return RedirectToAction(MVC.Home.ActionNames.AccessDenied, MVC.Home.Name);
            }
        }

        /// <summary>
        /// Created By: JT Barrett
        /// Created On: 30 May, 2016
        /// Purpose: This action is to post report parameters and export report
        /// Used: Respecitve page code from San Diego
        /// Last Updated On: 19 Aug, 2016
        /// Last Updated By: JT Barrett
        /// Reason: Added Excel export and multiselect checkbox support
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost]
        public virtual ActionResult ReportParameter(ReportParametersViewModel reportParametersViewModel)
        {
            var com_ReportParameterHeaderDelete_spParams = new com_ReportParameterHeaderDelete_spParams()
            {
                ReportID = reportParametersViewModel.ReportID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            List<object> deletedHeaderValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderDelete_sp", com_ReportParameterHeaderDelete_spParams).ToList();
            var com_ReportParameterValueDelete_spParams = new com_ReportParameterValueDelete_spParams()
            {
                ReportID = reportParametersViewModel.ReportID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            List<object> deletedValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueDelete_sp", com_ReportParameterValueDelete_spParams).ToList();

            string fDate = "fdate";
            string tDate = "tdate";

            foreach (var sequence in reportParametersViewModel.ParameterDefinitionList)
            {
                if (sequence.Caption != null)
                {
                    if (sequence.Caption == "From Date" && string.IsNullOrEmpty(sequence.Value))
                    {
                        fDate = sequence.Value;
                    }
                    if (sequence.Caption == "To Date" && string.IsNullOrEmpty(sequence.Value))
                    {
                        tDate = sequence.Value;
                    }
                }
            }

            foreach (var sequence in reportParametersViewModel.ParameterDefinitionList)
            {
                if (sequence.Caption != null)
                {
                    if (string.IsNullOrEmpty(fDate) || string.IsNullOrEmpty(tDate))
                    {
                        if (sequence.Caption == "From Time" || sequence.Caption == "To Time")
                        {
                            sequence.Value = string.Empty;
                        }
                    }

                    if (sequence.MultiSelectList.Count() > 0)
                    {
                        foreach (var item in sequence.MultiSelectList)
                        {
                            if (item.IsSelected)
                            {
                                var com_ReportParameterValueInsert_spParams = new com_ReportParameterValueInsert_spParams()
                                {
                                    ReportID = reportParametersViewModel.ReportID,
                                    Sequence = sequence.Sequence,
                                    Value = item.CodeID.ToString(),
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid()
                                };
                                List<object> insertedValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueInsert_sp", com_ReportParameterValueInsert_spParams).ToList();
                                var com_ReportParameterHeaderInsert_spParams = new com_ReportParameterHeaderInsert_spParams()
                                {
                                    ReportID = reportParametersViewModel.ReportID,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid(),
                                    JcatsUserID = UserManager.UserExtended.UserID,
                                    ReportParameterHeaderName = sequence.Caption,
                                    ReportParameterHeaderValue = item.CodeID.ToString(),
                                    RecordStateID = 1
                                };
                                insertedValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderInsert_sp", com_ReportParameterHeaderInsert_spParams).ToList();
                            }
                        }
                    }

                    else if (!string.IsNullOrEmpty(sequence.Value))
                    {
                        var com_ReportParameterValueInsert_spParams = new com_ReportParameterValueInsert_spParams()
                        {
                            ReportID = reportParametersViewModel.ReportID,
                            Sequence = sequence.Sequence,
                            Value = sequence.Value,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        };
                        List<object> insertedValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterValueInsert_sp", com_ReportParameterValueInsert_spParams).ToList();
                        var com_ReportParameterHeaderInsert_spParams = new com_ReportParameterHeaderInsert_spParams()
                        {
                            ReportID = reportParametersViewModel.ReportID,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                            JcatsUserID = UserManager.UserExtended.UserID,
                            ReportParameterHeaderName = sequence.Caption,
                            ReportParameterHeaderValue = sequence.HeaderValue,
                            RecordStateID = 1
                        };
                        insertedValue = UtilityService.ExecStoredProcedureWithResults<object>("com_ReportParameterHeaderInsert_sp", com_ReportParameterHeaderInsert_spParams).ToList();
                    }

                }
            }
            var com_ReportSourceGetByReportID_spParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = reportParametersViewModel.ReportID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString()

            };
            string filename = string.Empty;
            string errorMessage = string.Empty;

            //No Branches or merge documents YET, when applicable check for this first else onto next (reference SD code)
            reportParametersViewModel.ReportSourceList = UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>("com_ReportSourceGetByReportID_sp", com_ReportSourceGetByReportID_spParams).Select(c => (ReportSourceList)(new ReportSourceList()).InjectFrom(c)).ToList();
            if (reportParametersViewModel.ReportSourceList != null && reportParametersViewModel.ReportSourceList.Count > 0)
            {
                var reportSource = reportParametersViewModel.ReportSourceList[0];
                if (reportSource.ReportSourceDocumentName.Contains(".rpt"))
                {
                    ReportClass rpt = new ReportClass();
                    try
                    {
                        rpt.FileName = HttpContext.Server.MapPath("~/Reports/" + reportSource.ReportSourceDocumentName);
                        rpt.Load();
                        var table = UtilityService.ExecStoredProcedureForDataTableADO(reportSource.ReportSourceStoredProcedureName.Replace("dbo.", ""), com_ReportSourceGetByReportID_spParams, 60);
                        rpt.SetDataSource(table);
                        foreach (var subRptDt in reportParametersViewModel.ReportSourceList.Where(c => (c.ReportSourceDocumentName.CompareTo(reportSource.ReportSourceDocumentName) != 0)))
                        {
                            var subTableData = UtilityService.ExecStoredProcedureForDataTableADO(subRptDt.ReportSourceStoredProcedureName.Replace("dbo.", ""), com_ReportSourceGetByReportID_spParams, 60);
                            rpt.Subreports[subRptDt.ReportSourceDocumentName].SetDataSource(subTableData);
                        }
                        Stream stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
                        filename = Guid.NewGuid().ToString() + " " + filename + ".pdf";
                        filename = filename.Replace(" ", "");
                        filename = filename.Replace("&", "_");
                        filename = filename.Replace(",", "_");
                        filename = filename.Replace("(", "_");
                        filename = filename.Replace(")", "_");
                        filename = filename.Replace("/", "_");

                        //if file already exists then delete it
                        if (System.IO.File.Exists(Server.MapPath("~") + "MergeTemplate\\Download\\" + filename))
                            System.IO.File.Delete(Server.MapPath("~") + "MergeTemplate\\Download\\" + filename);
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + filename);
                        {
                            if (reportParametersViewModel.ParameterDefinitionList.Count == 0)
                            {
                                rpt.Close();
                                rpt.Dispose();
                                GC.Collect();
                                return Download(filename);

                            }
                        }
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

                    return Json(new { FileName = filename, errorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
                }
                else if (reportSource.ReportSourceDocumentName.Contains(".xls"))
                {

                    var reportSourceStoredProcedureName = reportParametersViewModel.ReportSourceStoredProcedureName.Replace("dbo.", "");
                    var sourceFile = HttpContext.Server.MapPath("~/Reports/" + reportSource.ReportSourceDocumentName);
                    var mergeDataTable = UtilityService.ExecStoredProcedureForDataTableADO(reportSourceStoredProcedureName, com_ReportSourceGetByReportID_spParams, 180);
                    if (mergeDataTable != null && mergeDataTable.Rows.Count > 0)
                    {
                        mergeDataTable.TableName = "Data Extract";

                        if (!(Directory.Exists(Server.MapPath("~") + "MergeTemplate\\Download\\")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~") + "MergeTemplate\\Download\\");
                        }

                        filename = Guid.NewGuid().ToString() + " " + reportParametersViewModel.CaseNumber + ".xlsx";
                        filename = filename.Replace(" ", "");
                        filename = filename.Replace("&", "_");
                        filename = filename.Replace(",", "_");
                        filename = filename.Replace("(", "_");
                        filename = filename.Replace(")", "_");
                        filename = filename.Replace("/", "_");
                        var generatedFilePath = UtilityFunctions.GetDocumentDownloadFolderPath() + filename;

                        UtilityFunctions.ExportDataTableToExcel(mergeDataTable, generatedFilePath);

                        if (reportParametersViewModel.ParameterDefinitionList.Count == 0)
                        {

                            return Download(filename);
                        }

                        return Json(new { FileName = filename }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { FileName = filename, errorMessage = "No records were found" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (reportSource.ReportSourceDocumentName.Contains(".doc"))
                {
                    var reportSourceStoredProcedureName = reportSource.ReportSourceStoredProcedureName.Replace("dbo.", "");

                    var mergeDataTAble = UtilityService.ExecStoredProcedureForDataTable(reportSourceStoredProcedureName, com_ReportSourceGetByReportID_spParams);


                    if (mergeDataTAble != null && mergeDataTAble.Rows.Count > 0)
                    {
                        string[] fieldNames = (from object column in mergeDataTAble.Columns select column.ToString()).ToArray();
                        var mergeTemplateRootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["MergeTemplateRootPath"];

                        var templatePath = mergeTemplateRootPath + reportSource.ReportSourceDocumentName;

                        // get mail merge document in byte[]
                        var template = System.IO.File.ReadAllBytes(templatePath);

                        var doc = new Aspose.Words.Document();
                        filename = reportSource.ReportDisplayName + ".doc";
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
                            var srcDoc = new Aspose.Words.Document(steam);
                            doc.AppendDocument(srcDoc, Aspose.Words.ImportFormatMode.KeepSourceFormatting);
                            doc.Save(UtilityFunctions.GetDocumentDownloadFolderPath() + filename);
                        }

                        var generatedFilePath = UtilityFunctions.GetDocumentDownloadFolderPath() + filename;

                        if (reportParametersViewModel.ParameterDefinitionList.Count == 0)
                        {

                            return Download(filename);
                        }

                        return Json(new { FileName = filename }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(new { FileName = filename, errorMessage = "No records were found" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { FileName = filename, errorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        //The following 3 have unique Parameter Requirements
        public virtual ActionResult ReportCaseloadSummary()
        {
            var viewModel = new ReportCaseloadSummaryViewModel();

            var AgencyGetByUserID_spParams = new AgencyGetByUserID_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                SortOption = "AgencyName",
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<AgencyGetByUserID_spResult>("AgencyGetByUserID_sp", AgencyGetByUserID_spParams).Select(c => (AgencyModel)(new AgencyModel()).InjectFrom(c)).ToList();

            var HearingTypeListParams = new pd_CodeGetByTypeIDAndUserID_spParams()
            {
                CodeTypeID = 10,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.HearingTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", HearingTypeListParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();

            var pd_RoleAgencyAttorneyGet_spParams = new pd_RoleAgencyAttorneyGet_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.AttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_RoleAgencyAttorneyGet_spResult>("pd_RoleAgencyAttorneyGet_sp", pd_RoleAgencyAttorneyGet_spParams).Select(c => (ReportParameterAttorneyListViewModel)(new ReportParameterAttorneyListViewModel()).InjectFrom(c)).ToList();
            if (viewModel.AttorneyList.Count == 1 && viewModel.AttorneyList[0].PersonID == UserManager.UserExtended.PersonID)
            {
                viewModel.AttorneyID = UserManager.UserExtended.PersonID;
            }

            var userInfo = UtilityService.ExecStoredProcedureWithResults<LALoDep.Domain.pd_Users.Edit.pd_JCATSUserGet_spResult>(
                                                   "pd_JCATSUserGet_sp", new LALoDep.Domain.pd_Users.Edit.pd_JCATSUserGet_spParams
                                                   {
                                                       JcatsUserID = UserManager.UserExtended.UserID,
                                                       UserID = UserManager.UserExtended.UserID,
                                                       BatchLogJobID = new Guid()
                                                   }).FirstOrDefault();
            ViewBag.IsLineUser = false;
            if (userInfo != null && userInfo.JcatsUserLevelCodeEnumName.Equals("LINE", StringComparison.CurrentCultureIgnoreCase))
            {
                viewModel.AttorneyID = userInfo.PersonID;
                viewModel.AttorneyList = viewModel.AttorneyList.Where(x => x.PersonID == userInfo.PersonID).ToList();
                ViewBag.IsLineUser = true;
            }

            var pd_RoleAgencyInvestigatorGet_spParams = new pd_RoleAgencyInvestigatorGet_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.InvestigatorList = UtilityService.ExecStoredProcedureWithResults<pd_RoleAgencyInvestigatorGet_spResults>("pd_RoleAgencyInvestigatorGet_sp", pd_RoleAgencyInvestigatorGet_spParams).Select(c => (ReportParameterInvestigatorListViewModel)(new ReportParameterInvestigatorListViewModel()).InjectFrom(c)).ToList();

            var DepartmentListParams = new pd_CodeGetByTypeIDAndUserID_spParams()
            {
                CodeTypeID = 30,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.DepartmentList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", DepartmentListParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();

            var AgencyCountyListParams = new AgencyGetCountyByUserID_spParams()
            {
                SortOption = "AgencyCounty",
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.AgencyCountyList = UtilityService.ExecStoredProcedureWithResults<AgencyGetCountyByUserID_spResult>("AgencyGetCountyByUserID_sp", AgencyCountyListParams).Select(c => (AgencyCountyModel)(new AgencyCountyModel()).InjectFrom(c)).ToList();

            var DesignatedDayListParams = new pd_CodeGetAllBySystemValueTypeID_spParams()
            {
                SystemValueTypeID = 231,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.DesignatedDayList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetAllBySystemValueTypeID_spResult>("pd_CodeGetAllBySystemValueTypeID_sp", DesignatedDayListParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();

            var AddressTypeListParams = new pd_CodeGetByTypeIDAndUserID_spParams()
            {
                CodeTypeID = 6,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.AddressTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", AddressTypeListParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();

            viewModel.CaseStatus = "Open";
            viewModel.ClientType = "Both";
            viewModel.SortBy = "NextHearingDate";

            return View(viewModel);
        }


        [HttpPost]
        public virtual ActionResult ReportCaseloadSummary(ReportCaseloadSummaryViewModel viewModel)
        {
            string errorMessage = "";
            string filename = Guid.NewGuid().ToString() + "CaseloadSummary.pdf";

            ReportClass rpt = new ReportClass();
            try
            {
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/" + "Caseload2.rpt");
                rpt.Load();

                //String.IsNullOrEmpty(viewModel.StartDate) ? (object)DBNull.Value : viewModel.StartDate ---- if DBNull is needed
                var table = UtilityService.ExecStoredProcedureForDataTableADO("rpt_Caseload2_sp", new
                {
                    UserID = UserManager.UserExtended.UserID,
                    AgencyID = viewModel.AgencyID,
                    Casestatus = viewModel.CaseStatus,
                    RoleType = viewModel.ClientType,
                    AppointmentStartDate = viewModel.StartDate,
                    AppointmentEndDate = viewModel.EndDate,
                    HearingDate = viewModel.HearingDate,
                    HearingTypeID = viewModel.HearingTypeID,
                    AttorneyID = viewModel.AttorneyID,
                    InvestigatorID = viewModel.InvestigatorID,
                    DepartmentID = viewModel.DepartmentID,
                    DesignatedDayCodeID = viewModel.DesignatedDayID,
                    PlacedWithParentFlag = (byte)((viewModel.PlacedWithParent) ? 1 : 0),
                    AddressTypeCodeID = viewModel.AddressTypeID,
                    SortOption = viewModel.SortBy,
                    BatchLogJobID = Guid.NewGuid(),
                    AgencyCountyID = viewModel.AgencyCountyID
                }, 80);

                if (table.Rows.Count == 0)
                {
                    rpt.Close();
                    rpt.Dispose();
                    GC.Collect();
                    return Json(new { FileName = filename, errorMessage = "The parameters that you entered did not return any records." }, JsonRequestBehavior.AllowGet);
                }

                rpt.SetDataSource(table);
                Stream stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
                if (System.IO.File.Exists(Server.MapPath("~") + "MergeTemplate\\Download\\" + filename))
                    System.IO.File.Delete(Server.MapPath("~") + "MergeTemplate\\Download\\" + filename);
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
            return Json(new { FileName = filename, errorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult ReportHearingSummary()
        {
            var viewModel = new ReportHearingSummaryViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult ReportHearingSummary(ReportHearingSummaryViewModel viewModel)
        {
            string errorMessage = "";
            string filename = "HearingsByAttorney.pdf";

            ReportClass rpt = new ReportClass();
            try
            {
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/" + "rptHearingSummary.rpt");
                rpt.Load();

                //DBNull conversion issues in a fixed type model, do it manually instead.
                var table = UtilityService.ExecStoredProcedureForDataTable("rpt_HearingSummary_sp", new
                {
                    UserID = UserManager.UserExtended.UserID,
                    StartDate = !viewModel.StartDate.HasValue ? (object)DBNull.Value : viewModel.StartDate.Value.ToString("yyyy/MM/dd hh:mm:ss tt"),
                    EndDate = !viewModel.EndDate.HasValue ? (object)DBNull.Value : viewModel.EndDate.Value.ToString("yyyy/MM/dd hh:mm:ss tt"),
                    BatchLogJobID = Guid.NewGuid()
                });

                if (table.Rows.Count == 0)
                {
                    rpt.Close();
                    rpt.Dispose();
                    GC.Collect();
                    return Json(new { FileName = filename, errorMessage = "The parameters that you entered did not return any records." }, JsonRequestBehavior.AllowGet);
                }

                rpt.SetDataSource(table);
                Stream stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
                if (System.IO.File.Exists(Server.MapPath("~") + "MergeTemplate\\Download\\" + filename))
                    System.IO.File.Delete(Server.MapPath("~") + "MergeTemplate\\Download\\" + filename);
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
            return Json(new { FileName = filename, errorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult ReportRFDCaseloadSummary()
        {
            var viewModel = new ReportRFDCaseloadSummaryViewModel();

            var AgencyGetByUserID_spParams = new AgencyGetByUserID_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                SortOption = "AgencyName",
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<AgencyGetByUserID_spResult>("AgencyGetByUserID_sp", AgencyGetByUserID_spParams).Select(c => (AgencyModel)(new AgencyModel()).InjectFrom(c)).ToList();

            //EXECUTE dbo.pd_RoleGetARRequestFor_sp NULL,327,NULL,60001698,NULL,'RoleTypesOnly'
            var pd_RoleGetARRequestFor_spParams = new pd_RoleGetARRequestFor_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                CaseAgencyID = UserManager.UserExtended.AgencyID,
                LoadOption = "RoleTypesOnly",
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.RoleTypeList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetARRequestFor_spResult>("pd_RoleGetARRequestFor_sp", pd_RoleGetARRequestFor_spParams).Select(c => (RFDCaseloadRoleTypeModel)(new RFDCaseloadRoleTypeModel()).InjectFrom(c)).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult ReportRFDCaseloadSummary(ReportRFDCaseloadSummaryViewModel viewModel)
        {
            string errorMessage = "";
            string filename = "ARCaseloadSummary.pdf";

            ReportClass rpt = new ReportClass();
            try
            {
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/" + "rptRFDCaseloadSummary.rpt");
                rpt.Load();

                //DBNull conversion issues in a fixed type model, do it manually instead.
                var table = UtilityService.ExecStoredProcedureForDataTable("rpt_RFDCaseloadSummary_sp", new
                {
                    UserID = UserManager.UserExtended.UserID,
                    AgencyID = viewModel.AgencyID,
                    StartDate = viewModel.StartDate,
                    EndDate = viewModel.EndDate,
                    ReportType = viewModel.ReportType,
                    RequestedForRoleTypeParm = viewModel.RoleTypeID,
                    CompletedIsSubsetOfDueParm = viewModel.CompletedIsSubset ? 1 : 0,
                    BatchLogJobID = Guid.NewGuid(),
                    ExcludeDueDate2Days = viewModel.ExcludeDueDate2Days ? 1 : 0,
                    DueDate6Weeks = viewModel.DueDate6Weeks ? 1 : 0,
                    OnlyActiveAR = viewModel.OnlyActiveAR ? 1 : 0
                });

                if (table.Rows.Count == 0)
                {
                    rpt.Close();
                    rpt.Dispose();
                    GC.Collect();
                    return Json(new { FileName = filename, errorMessage = "The parameters that you entered did not return any records." }, JsonRequestBehavior.AllowGet);
                }

                rpt.SetDataSource(table);
                Stream stream = rpt.ExportToStream(ExportFormatType.PortableDocFormat);
                if (System.IO.File.Exists(Server.MapPath("~") + "MergeTemplate\\Download\\" + filename))
                    System.IO.File.Delete(Server.MapPath("~") + "MergeTemplate\\Download\\" + filename);
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
            return Json(new { FileName = filename, errorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
        }

        // [HttpGet]
        public virtual ActionResult Download(string file)
        {
            string fullPath = UtilityFunctions.GetDocumentDownloadFolderPath() + file;

            // Use once user export preferences are implemented
            /*if (_userManager.UserExtended.PrintDocumentOn == "NewWindow"){
                Response.AppendHeader("Content-Disposition", "inline; filename=" + file);
                if (Path.GetExtension(fullPath).Contains(".pdf"))
                    return File(fullPath, "application/pdf");
                else if (Path.GetExtension(fullPath).Contains(".xlsx"))
                    return File(fullPath, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            }*/
            if (!System.IO.File.Exists(fullPath))
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (UserManager.UserExtended.PrintDocumentOn == "NewWindow" && Path.GetExtension(fullPath).Contains("pdf"))
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=" + file);
                return File(fullPath, "application/pdf");
            }
            if (System.IO.File.Exists(fullPath))
            {

                return File(fullPath, "application/force-download", file);
            }
            else
            {
                return Content(fullPath + " File not found");
            }

        }
    }
}