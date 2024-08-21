
using DataTables.Mvc;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_JcatsGroup;
using LALoDep.Domain.Services;
using LALoDep.Core.Enums;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Role;
using Microsoft.Ajax.Utilities;
using Omu.ValueInjecter;
using LALoDep.Domain;
using System.Data.Entity.Core.Objects;
using LALoDep.Models.Case;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class CaseController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;
        //private int _userId = 0;
        public CaseController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
            //_userId = UserManager.UserExtended.UserID;
        }
        // GET: /Case/
        [AllowAnonymous] /*#Todo Remove this once tokens are added to db*/
        [ChildActionOnly]
        public virtual PartialViewResult Menu(string viewTitle)
        {
            var data = (MenuCaseModelView)new MenuCaseModelView().InjectFrom(UserManager.UserExtended);
            data.ViewTitle = viewTitle;
            if (!string.IsNullOrEmpty(UserManager.UserExtended.AccessDeniedReason) && ControllerContext.ParentActionViewContext.RouteData.GetRequiredString("action") != "Secure")
            {
                Response.Redirect("~/Case/Secure", false);
            }

            return PartialView("_MenuCase", data);
        }

        #region Search for case
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.SearchCasePage, PageSecurityItemID = SecurityToken.SearchCase)]

        public virtual ActionResult Search(string ClientNameLast, string ClientNameFirst, string CaseNumber)
        {

            var viewmodel = new CaseSearchViewModel();
            if (Session["CaseAccessLevelMessage"] != null)
            {
                viewmodel.CaseAccessLevelMessage = Session["CaseAccessLevelMessage"].ToString();
                if (UserManager.UserExtended.CaseID == 0)
                    Session["CaseAccessLevelMessage"] = null;
                if (UserManager.UserExtended.CaseID > 0)
                    UserManager.UpdateCaseStatusBar(0);
            }
            if (!(string.IsNullOrEmpty(ClientNameLast)))
            {
                viewmodel.LastName = ClientNameLast;
                viewmodel.OnViewLoad = true;
            }
            if (!(string.IsNullOrEmpty(ClientNameFirst)))
            {
                viewmodel.FirstName = ClientNameFirst;
                viewmodel.OnViewLoad = true;
            }

            if (!(string.IsNullOrEmpty(CaseNumber)))
            {
                viewmodel.DocketNumber = CaseNumber;
                viewmodel.OnViewLoad = true;
            }
            if (Request.QueryString["caseTextbox"] != null)
            {
                Session["CaseTextBoxFocus"] = true;
            }
            if (viewmodel.OnViewLoad)
            {
                var searchData = GetSearchData(viewmodel).DistinctBy(o => o.CaseID).ToList();
                if (searchData.Count == 1)
                {
                    return RedirectToAction("Main", "Case", new { id = searchData[0].CaseID.ToEncrypt() });
                }
            }

            var pd_JcatsGroupAgencyGetByJcatsUserID_spParams = new pd_JcatsGroupAgencyGetByJcatsUserID_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var agencyList = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupAgencyGetByJcatsUserID_spResults>("pd_JcatsGroupAgencyGetByJcatsUserID_sp", pd_JcatsGroupAgencyGetByJcatsUserID_spParams).ToList();
            viewmodel.AgancyList = agencyList.Select(x => new AgencyModel()
            {
                AgencyID = x.AgencyID,
                AgencyName = x.AgencyName
            }).ToList();

            if (viewmodel.AgancyList.Count == 1)
            {
                viewmodel.AgencyID = viewmodel.AgancyList.First().AgencyID;
            }
            return View(viewmodel);
        }

        public List<pd_CaseSearch_spResults> GetSearchData(CaseSearchViewModel searchParams)
        {
            //var list = new List<pd_CaseSearch_sp_Result>();

            //ObjectParameter gUID = new ObjectParameter("gUID", UserManager.UserExtended.Guid);
            //ObjectParameter totalRecords = new ObjectParameter("totalRecords", DBNull.Value);
            //list = UtilityService.Context.pd_CaseSearch_sp(string.IsNullOrEmpty(searchParams.FirstName) ? searchParams.FirstName : searchParams.FirstName.Trim(),
            //                                                 string.IsNullOrEmpty(searchParams.LastName) ? searchParams.LastName : searchParams.LastName.Trim(),
            //                                                 string.IsNullOrEmpty(searchParams.DocketNumber) ? searchParams.DocketNumber : searchParams.DocketNumber.Trim(),
            //                                                 string.IsNullOrEmpty(searchParams.JcatsNumber) ? searchParams.JcatsNumber : searchParams.JcatsNumber.Trim(),
            //                                                 1,
            //                                                 string.IsNullOrEmpty(searchParams.HHSA) ? searchParams.HHSA : searchParams.HHSA.Trim()
            //                                                 , gUID, 0, 200000, 1, totalRecords, UserManager.UserExtended.UserID, Guid.NewGuid(), searchParams.AgencyID).ToList();

            var list = UtilityService.ExecStoredProcedureWithResults<pd_CaseSearch_spResults>("pd_CaseSearch_sp", new pd_CaseSearch_spParams
            {
                FirstName = searchParams.FirstName?.Trim(),
                LastName = searchParams.LastName?.Trim(),
                DocketNumber = searchParams.DocketNumber?.Trim(),
                CaseNumber = searchParams.JcatsNumber?.Trim(),
                Appointment = 1,
                HHSA = searchParams.HHSA?.Trim(),

                GUID = Guid.NewGuid(),

                StartRecord = 0,
                Range = 50,

                SortID = 1,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyID = searchParams.AgencyID
            }).ToList();

            return list;
        }

        /*
        [HttpPost]
        public virtual JsonResult Search(CaseSearchViewModel searchParams)
        {
            var list = GetSearchData(searchParams);
            var total = list.Count;
            var dob = string.Empty;
            var name = string.Empty;
            var sex = string.Empty;
            var caseRole = string.Empty;
            var personId = 0;
            foreach (var item in list)
            {

                if (personId == (item.PersonID.HasValue ? item.PersonID.Value : 0) && name == item.PersonNameFirst && dob == item.DOB && sex == item.Sex && caseRole == item.Role)
                {
                    item.PersonNameFirst = string.Empty;
                    item.PersonNameLast = string.Empty;
                    item.DOB = string.Empty;
                    item.Role = string.Empty;
                    item.Sex = string.Empty;
                }
                else
                {
                    name = item.PersonNameFirst;
                    personId = item.PersonID.HasValue ? item.PersonID.Value : 0;
                    dob = item.DOB;
                    caseRole = item.Role;
                    sex = item.Sex;
                }

            }
            var searchList = list.Select(c => new
            {
                PersonNameLast = c.PersonNameLast,
                PersonNameFirst = c.PersonNameFirst,
                DOB = c.DOB,
                Sex = c.Sex,
                Role = c.Role,
                HHSANumber = c.HHSANumber,
                PetitionDocketNumber = c.CaseNumber,//need to confirn
                ClosedDate = c.ClosedDate,
                PetitionType = c.PetitionType,
                PetitionCloseDate = c.PetitionCloseDate,
                CaseNumber = c.PetitionDocketNumber,//need to confirn
                LeadAttorney = c.LeadAttorney,
                RoleID = c.RoleID,
                PersonID = c.PersonID,
                CaseID = c.CaseID,
                EncryptedCaseID = c.CaseID.ToEncrypt(),
                ConflictFlag = c.ConflictFlag,
                RoleClient = c.RoleClient,
                EncryptedRoleID = c.RoleID.ToEncrypt()

            }).ToList();
            var jsonResult = Json(new DataTablesResponse(0, searchList, total, total), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        */
        [HttpPost]
        public virtual JsonResult Search(LALoDep.Core.DataTable.DTParameters param, CaseSearchViewModel searchParams)
        {
            if (string.IsNullOrEmpty(searchParams.FirstName) && string.IsNullOrEmpty(searchParams.LastName) && string.IsNullOrEmpty(searchParams.DocketNumber) && string.IsNullOrEmpty(searchParams.JcatsNumber) && string.IsNullOrEmpty(searchParams.HHSA))
            {
                TempData["SearchParamGUID"] = null;
                return Json(new DataTablesResponse(param.Draw, new List<dynamic>(), 0, 0), JsonRequestBehavior.AllowGet);
            }

            Guid? wtCaseSearchGUID = null;
            if (!searchParams.ParamChanged && TempData["SearchParamGUID"] != null)
                wtCaseSearchGUID = Guid.Parse(TempData["SearchParamGUID"].ToString());
            if (!searchParams.SearchGuid.IsNullOrEmpty() && searchParams.SearchGuid.Length > 0)
            {
                wtCaseSearchGUID = Guid.Parse(searchParams.SearchGuid);
                TempData["SearchParamGUID"] = wtCaseSearchGUID;
            }

            var list = UtilityService.ExecStoredProcedureWithResults<pd_CaseSearch_spResults>("pd_CaseSearch_sp", new pd_CaseSearch_spParams
            {
                FirstName = searchParams.FirstName?.Trim(),
                LastName = searchParams.LastName?.Trim(),
                DocketNumber = searchParams.DocketNumber?.Trim(),
                CaseNumber = searchParams.JcatsNumber?.Trim(),
                Appointment = 1,
                HHSA = searchParams.HHSA?.Trim(),

                GUID = wtCaseSearchGUID,

                StartRecord = param.Start + 1,
                Range = param.Length,

                SortID = 1,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyID = searchParams.AgencyID
            }).ToList();


            var total = 0;

            if (list != null && list.Any())
            {
                var firstRow = list.FirstOrDefault();
                total = firstRow.TotalRecords;
                wtCaseSearchGUID = firstRow.wtCaseSearchGUID;
            }

            TempData["SearchParamGUID"] = wtCaseSearchGUID;

            //Star from here

            var dob = string.Empty;
            var name = string.Empty;
            var sex = string.Empty;
            var caseRole = string.Empty;
            var personId = 0;

            foreach (var item in list)
            {
                if (personId == item.PersonID && name == item.PersonNameFirst && dob == item.DOB && sex == item.Sex && caseRole == item.Role)
                {
                    item.PersonNameFirst = string.Empty;
                    item.PersonNameLast = string.Empty;
                    item.DOB = string.Empty;
                    item.Role = string.Empty;
                    item.Sex = string.Empty;
                }
                else
                {
                    name = item.PersonNameFirst;
                    personId = item.PersonID ?? 0;
                    dob = item.DOB;
                    caseRole = item.Role;
                    sex = item.Sex;
                }
            }

            var searchList = list.Select(c => new
            {
                c.PersonNameLast,
                c.PersonNameFirst,
                c.DOB,
                c.Sex,
                c.Role,
                c.HHSANumber,
                c.CaseNumber,//need to confirn                
                c.ClosedDate,
                c.PetitionType,
                c.PetitionCloseDate,
                c.PetitionDocketNumber,//need to confirn
                c.LeadAttorney,
                c.RoleID,
                c.PersonID,
                c.CaseID,
                EncryptedCaseID = c.CaseID.ToEncrypt(),
                c.ConflictFlag,
                c.RoleClient,
                EncryptedRoleID = c.RoleID.ToEncrypt(),
                c.wtCaseSearchGUID
            }).ToList();
            var jsonResult = Json(new DataTablesResponse(param.Draw, searchList, total, total), JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public virtual ActionResult PetitionAndRolePopUp(string id)
        {
            PetitionaAndRolePopUpVewModel viewModel = new PetitionaAndRolePopUpVewModel();

            viewModel.PetitionaDetailList = UtilityService.Context.pd_PetitionGetClosedByRoleID_sp(id.ToDecrypt().ToInt(), UserManager.UserExtended.UserID, Guid.NewGuid()).ToList();
            var roleInfo = UtilityService.Context.pd_RoleGet_sp(id.ToDecrypt().ToInt(), UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
            if (roleInfo != null)
            {
                viewModel.PersonNameFirst = roleInfo.PersonNameFirst;
                viewModel.PersonNameLast = roleInfo.PersonNameLast;
            }

            return View(viewModel);

        }

        #endregion

        #region AdvSearch for case
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AdvSearchCasePage, PageSecurityItemID = SecurityToken.AdvCaseSearch)]
        public virtual ActionResult AdvCaseSearch()
        {
            var viewmodel = new CaseSearchExtendedViewModel();
            //state
            viewmodel.State = UtilityService.Context.pd_CodeGetByTypeIDAndUserIDSortShortValue_sp(CodeType.State.GetHashCode(), UserManager.UserExtended.UserID, Guid.NewGuid()).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();

            //RoleAgency Person
            var pd_RoleAgencyGet_spParams = new pd_RoleAgencyGet_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            //Attorney
            viewmodel.Attorney = UtilityService.Context.pd_RoleAgencyAttorneyGet_sp(UserManager.UserExtended.UserID, Guid.NewGuid()).Select(x => new PersonViewModel()
            {
                PersonNameDisplay = x.PersonNameLast + ", " + x.PersonNameFirst,
                PersonID = x.PersonID
            }).ToList();

            //Investigator
            viewmodel.Investigator = UtilityService.Context.pd_RoleAgencyInvestigatorGet_sp(UserManager.UserExtended.UserID, Guid.NewGuid()).Select(x => new PersonViewModel()
            {
                PersonNameDisplay = x.PersonNameLast + ", " + x.PersonNameFirst,
                PersonID = x.PersonID
            }).ToList();

            //Paralegal
            viewmodel.Paralegal = UtilityService.Context.pd_RoleAgencyLegalAssistantGet_sp(UserManager.UserExtended.UserID, Guid.NewGuid()).Select(x => new PersonViewModel()
            {
                PersonNameDisplay = x.PersonNameLast + ", " + x.PersonNameFirst,
                PersonID = x.PersonID
            }).ToList();

            //Hearing Type
            viewmodel.HearingType = UtilityService.Context.pd_CodeGetByTypeIDAndUserIDSortShortValue_sp(CodeType.HearingType.GetHashCode(), UserManager.UserExtended.UserID, Guid.NewGuid()).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();

            //Hering Officer
            viewmodel.HearingOfficer = UtilityService.Context.pd_RoleAgencyHearingOfficerGet_sp(UserManager.UserExtended.UserID, Guid.NewGuid()).Select(x => new PersonViewModel()
            {
                PersonNameDisplay = x.PersonNameLast + ", " + x.PersonNameFirst,
                PersonID = x.PersonID
            }).ToList();


            var pd_CodeGetByTypeIDAndUserID_spParams = new pd_CodeGetByTypeIDAndUserID_spParams()
            {
                CodeTypeID = CodeType.RFDType.GetHashCode(),//RFD type
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            //RFD Type
            viewmodel.RFDType = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();

            //Petition Type
            pd_CodeGetByTypeIDAndUserID_spParams.CodeTypeID = CodeType.PetitionType.GetHashCode();
            viewmodel.PetitionType = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();

            //Language
            pd_CodeGetByTypeIDAndUserID_spParams.CodeTypeID = CodeType.Language.GetHashCode();
            viewmodel.Language = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();

            //Allegation
            pd_CodeGetByTypeIDAndUserID_spParams.CodeTypeID = CodeType.Allegation.GetHashCode();
            viewmodel.Allegation = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();

            //Department
            pd_CodeGetByTypeIDAndUserID_spParams.CodeTypeID = CodeType.Department.GetHashCode();
            viewmodel.HearingDepartment = viewmodel.CaseDepartment = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", pd_CodeGetByTypeIDAndUserID_spParams).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();

            return View(viewmodel);
        }

        [HttpPost]
        public virtual ActionResult SearchForCaseAdvanced(CaseSearchExtendedViewModel viewModel)
        {
            var list = new List<pd_CaseSearchExtended_spResults>();
            var pd_CaseSearchExtended_spParams = new pd_CaseSearchExtended_spParams
            {

                ClientLastName = string.IsNullOrEmpty(viewModel.ClientLastName) ? viewModel.ClientLastName : viewModel.ClientLastName.Trim(),
                ClientFirstName = string.IsNullOrEmpty(viewModel.ClientFirstName) ? viewModel.ClientFirstName : viewModel.ClientFirstName.Trim(),
                ParentFirstName = string.IsNullOrEmpty(viewModel.ParentFirstName) ? viewModel.ParentFirstName : viewModel.ParentFirstName.Trim(),
                ParentLastName = string.IsNullOrEmpty(viewModel.ParentLastName) ? viewModel.ParentLastName : viewModel.ParentLastName.Trim(),
                AgencyAttorneyID = viewModel.AgencyAttorneyID,
                InvestigatorID = viewModel.InvestigatorID,
                LegalAssistantID = viewModel.LegalAssistantID,
                SocialWorkerLastName = string.IsNullOrEmpty(viewModel.SocialWorkerLastName) ? viewModel.SocialWorkerLastName : viewModel.SocialWorkerLastName.Trim(),
                SocialWorkerFirstName = string.IsNullOrEmpty(viewModel.SocialWorkerFirstName) ? viewModel.SocialWorkerFirstName : viewModel.SocialWorkerFirstName.Trim(),
                CaretakerLastName = string.IsNullOrEmpty(viewModel.CaretakerLastName) ? viewModel.CaretakerLastName : viewModel.CaretakerLastName.Trim(),
                CaretakerFirstName = string.IsNullOrEmpty(viewModel.CaretakerFirstName) ? viewModel.CaretakerFirstName : viewModel.CaretakerFirstName.Trim(),
                ChildLastName = string.IsNullOrEmpty(viewModel.ChildLastName) ? viewModel.ChildLastName : viewModel.ChildLastName.Trim(),
                ChildFirstName = string.IsNullOrEmpty(viewModel.ChildFirstName) ? viewModel.ChildFirstName : viewModel.ChildFirstName.Trim(),
                ChildDOB = viewModel.ChildDOB,
                ClientDOB = viewModel.ClientDOB,
                ParentDOB = viewModel.ParentDOB,
                ClientAddress = string.IsNullOrEmpty(viewModel.ClientAddress) ? viewModel.ClientAddress : viewModel.ClientAddress.Trim(),
                ClientCity = string.IsNullOrEmpty(viewModel.ClientCity) ? viewModel.ClientCity : viewModel.ClientCity.Trim(),
                ClientStateID = viewModel.ClientStateID,
                ClientZip = string.IsNullOrEmpty(viewModel.ClientZip) ? viewModel.ClientZip : viewModel.ClientZip.Trim(),
                ClientPhoneNumber = string.IsNullOrEmpty(viewModel.ClientPhoneNumber) ? viewModel.ClientPhoneNumber : viewModel.ClientPhoneNumber.Trim(),
                ClientSSN = string.IsNullOrEmpty(viewModel.ClientSSN) ? viewModel.ClientSSN : viewModel.ClientSSN.Trim(),
                ParentAddress = string.IsNullOrEmpty(viewModel.ParentAddress) ? viewModel.ParentAddress : viewModel.ParentAddress.Trim(),
                ParentCity = string.IsNullOrEmpty(viewModel.ParentCity) ? viewModel.ParentCity : viewModel.ParentCity.Trim(),
                ParentStateID = viewModel.ParentStateID,
                ParentZip = string.IsNullOrEmpty(viewModel.ParentZip) ? viewModel.ParentZip : viewModel.ParentZip.Trim(),
                ParentPhone = string.IsNullOrEmpty(viewModel.ParentPhone) ? viewModel.ParentPhone : viewModel.ParentPhone.Trim(),
                CaretakerAddress = string.IsNullOrEmpty(viewModel.CaretakerAddress) ? viewModel.CaretakerAddress : viewModel.CaretakerAddress.Trim(),
                SocialWorkerPhoneNumber = string.IsNullOrEmpty(viewModel.SocialWorkerPhoneNumber) ? viewModel.SocialWorkerPhoneNumber : viewModel.SocialWorkerPhoneNumber.Trim(),
                CaretakerPhoneNumber = string.IsNullOrEmpty(viewModel.CaretakerPhoneNumber) ? viewModel.CaretakerPhoneNumber : viewModel.CaretakerPhoneNumber.Trim(),
                LanguageID = viewModel.LanguageID,
                PetitionNumber = string.IsNullOrEmpty(viewModel.PetitionNumber) ? viewModel.PetitionNumber : viewModel.PetitionNumber.Trim(),
                HearingTypeID = viewModel.HearingTypeID,
                HearingDate = viewModel.HearingDate,
                HearingTime = viewModel.HearingTime,
                HearingOfficerID = viewModel.HearingOfficerID,
                HearingDepartmentID = viewModel.HearingDepartmentID,
                CaseOpenDate = null,
                CaseClosedDate = null,
                AllegationID = viewModel.AllegationID,
                PetitionTypeID = viewModel.PetitionTypeID,
                ReportFilingDueTypeID = viewModel.ReportFilingDueTypeID,
                ParentalRightsTerminationDate = viewModel.ParentalRightsTerminationDate,
                CaseNumber = string.IsNullOrEmpty(viewModel.CaseNumber) ? viewModel.CaseNumber : viewModel.CaseNumber.Trim(),
                CountyCounselNumber = string.IsNullOrEmpty(viewModel.CountyCounselNumber) ? viewModel.CountyCounselNumber : viewModel.CountyCounselNumber.Trim(),
                HHSANumber = string.IsNullOrEmpty(viewModel.HHSANumber) ? viewModel.HHSANumber : viewModel.HHSANumber.Trim(),
                ParentSSN = string.IsNullOrEmpty(viewModel.ParentSSN) ? viewModel.ParentSSN : viewModel.ParentSSN.Trim(),
                BookingNumber = string.IsNullOrEmpty(viewModel.BookingNumber) ? viewModel.BookingNumber : viewModel.BookingNumber.Trim(),
                InmateNumber = string.IsNullOrEmpty(viewModel.InmateNumber) ? viewModel.InmateNumber : viewModel.InmateNumber.Trim(),
                AppointmentCase = null,
                SortField = null,
                SortDirection = null,
                CaseStatus = string.IsNullOrEmpty(viewModel.CaseStatus) ? viewModel.CaseStatus : viewModel.CaseStatus.Trim(),
                CaseDepartmentID = viewModel.CaseDepartmentID,
                RoleStatus = viewModel.RoleStatus,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                PetitionStatus = viewModel.PetitionStatus,
            };

            list = UtilityService.ExecStoredProcedureWithResults<pd_CaseSearchExtended_spResults>("pd_CaseSearchExtended_sp", pd_CaseSearchExtended_spParams).ToList();

            var total = list.Count;
            var data = list.Select(x => new
            {
                PersonNameLast = x.PersonNameLast,
                PersonNameFirst = x.PersonNameFirst,
                PersonDob = (x.PersonDob != null) ? x.PersonDob.Value.ToString("MM/dd/yyyy") : x.PersonDob.ToString(),
                Sex = x.Sex,
                Role = x.Role,
                CaseNumber = x.PetitionDocketNumber,
                HHSANumber = x.HHSANumber,
                casecloseddate = (x.casecloseddate != null) ? x.casecloseddate.Value.ToString("MM/dd/yyyy") : x.casecloseddate.ToString(),
                LeadAttorney = x.LeadAttorney,
                PetitionDocketNumber = x.CaseNumber,
                PersonID = x.PersonID,
                CaseID = x.CaseID,
                EncryptedCaseID = x.CaseID.ToEncrypt()
            }).ToList();
            return Json(new DataTablesResponse(0, data, total, total));
        }

        #endregion
    }
}