using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.Agency;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.CaseOpening;
using Omu.ValueInjecter;
using LALoDep.Domain.CaseAttribute;

namespace LALoDep.Controllers.CaseOpening
{
    public partial class CaseOpeningController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public CaseOpeningController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        // GET: CaseOpening
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.AddCase, CustomSecurityItemIds = PageLevelSecurityItemIds.AddApptCasePage)]
        public virtual ActionResult AddApptCase()
        {
            if (Request.QueryString["_uniquerequest"] == null)//not ajax request
                UserManager.UpdateCaseStatusBar(0);


            var model = new AddAppointmentCaseSearchViewModel();
            model.AppointmentDate = DateTime.Now;
            model.AgencyList = UtilityService.ExecStoredProcedureWithResults<AgencyGetByUserID_spResult>(
                "AgencyGetByUserID_sp", new AgencyGetByUserID_spParams
                {
                    SortOption = "AgencyName",
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID

                }).Select(o => new SelectListItem() { Text = o.AgencyName, Value = o.AgencyID.ToString() }).ToList();
            if (model.AgencyList.Count() == 1)
            {
                model.AgencyID = model.AgencyList.FirstOrDefault().Value.ToInt();

            }

            model.ReferralSourceList = UtilityFunctions.CodeGetByTypeIdAndUserId(81, sortOption: "CodeValue");


            model.DepartmentList = UtilityFunctions.CodeGetByTypeIdAndUserId(30, sortOption: "CodeValue");
         



            return View(model);
        }


        [HttpPost]
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.AddCase)]

        public virtual JsonResult AddApptCase(AddAppointmentCaseSearchViewModel searchParams)
        {
          
            var list = UtilityService.ExecStoredProcedureWithResults<pd_NewCaseSearch_spResult>("pd_NewCaseSearch_sp",
                   new pd_NewCaseSearch_spParams()
                   {
                       AgencyID = searchParams.AgencyID,
                       BatchLogJobID = Guid.NewGuid(),
                       DocketNumber = searchParams.CaseNumber1,
                       MotherDOB = searchParams.MotherDOB,
                       UserID = UserManager.UserExtended.UserID,
                       MotherFirstName = searchParams.MotherFirstName,
                       MotherLastName = searchParams.MotherLastName,
                       ChildDOB = searchParams.Child1DOB ?? string.Empty,
                       ChildDOB2 = searchParams.Child2DOB ?? string.Empty,
                       ChildDOB3 = searchParams.Child3DOB ?? string.Empty,
                       ChildFirstName = searchParams.ChildFirstName1 ?? string.Empty,
                       ChildFirstName2 = searchParams.ChildFirstName2 ?? string.Empty,
                       ChildFirstName3 = searchParams.ChildFirstName3 ?? string.Empty,
                       ChildLastName = searchParams.ChildLastName1 ?? string.Empty,
                       ChildLastName2 = searchParams.ChildLastName2 ?? string.Empty,
                       ChildLastName3 = searchParams.ChildLastName3 ?? string.Empty,
                       FatherDOB = searchParams.Father1DOB ?? string.Empty,
                       FatherDOB2 = searchParams.Father2DOB ?? string.Empty,
                       FatherFirstName = searchParams.FatherFirstName1 ?? string.Empty,
                       FatherFirstName2 = searchParams.FatherFirstName2 ?? string.Empty,
                       FatherLastName = searchParams.FatherLastName1 ?? string.Empty,
                       FatherLastName2 = searchParams.FatherLastName2 ?? string.Empty,
                       DocketNumber2 = searchParams.CaseNumber2 ?? string.Empty,
                       DocketNumber3 = searchParams.CaseNumber3 ?? string.Empty,
                   }).ToList();

            var total = list.Count;
            var searchList = list.Select(c => new
            {
                c.PersonNameLast,
                c.PersonNameFirst,
                c.DOB,
                c.Sex,
                c.Role,
                c.Agency,
                c.ClosedDate,

                c.CaseNumber,//need to confirn
                c.LeadAttorney,
                c.RoleID,
                c.CaseID,
                EncryptedCaseID = c.CaseID.ToEncrypt(),
                c.KeySequence,
                c.PetitionDocketNumber,
                c.RoleClient

            }).ToList();
            if (total > 0)
                return Json(new { Status = "Done", SearchData = new DataTablesResponse(0, searchList, total, total) });
            
            
            //Insert Case if no search result return;
            var caseId = UtilityService.ExecStoredProcedureWithResults<int>("pd_CaseInsert_sp",
                  new pd_CaseInsert_spParams()
                  {
                      AgencyID = searchParams.AgencyID,
                      BatchLogJobID = Guid.NewGuid(),
                      CaseAppointmentDate = searchParams.AppointmentDate,
                      CaseNumber = searchParams.CaseNumber1,
                      CasePanelCase = (short)(searchParams.PanelCase ? 1 : 0),
                      DepartmentID = searchParams.DepartmentID,
                      RecordStateID = 1,
                      UserID = UserManager.UserExtended.UserID,
                      

                  }).FirstOrDefault();

            if (caseId <= 0) 
                return Json(new { Status = "Fail" });
            if ( searchParams.ReferralSourceID>0)
            {
                  UtilityService.ExecStoredProcedureWithResults<int>("CaseAttributeInsert_sp", new CaseAttributeInsert_spParams()
                {
                    CaseID = caseId,
                    CaseAttributeTypeID = 1020,
                    CaseAttributeGenericValue = searchParams.ReferralSourceID.ToString(),
                    TableID = caseId,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    RecordStateID = 1

                }).FirstOrDefault();

            }
            UserManager.UpdateCaseStatusBar(caseId);
            var parms = (AddAppointmentCaseSearchViewModelForSession)(new AddAppointmentCaseSearchViewModelForSession()).InjectFrom(searchParams);
            TempData["AddApptCaseFormData"] = parms;

            return Json(new { Status = "CaseRedirect", URL = "/CaseOpening/AddCaseRespondents" });

        }
        [HttpPost]
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.AddCase)]

        public virtual JsonResult InsertCase(AddAppointmentCaseSearchViewModel searchParams)
        {
            var caseId = UtilityService.ExecStoredProcedureWithResults<int>("pd_CaseInsert_sp",
                   new pd_CaseInsert_spParams()
                   {
                       AgencyID = searchParams.AgencyID,
                       BatchLogJobID = Guid.NewGuid(),
                       CaseAppointmentDate = searchParams.AppointmentDate,
                       CaseNumber = searchParams.CaseNumber1,
                       CasePanelCase = (short)(searchParams.PanelCase ? 1 : 0),
                       DepartmentID = searchParams.DepartmentID,
                       RecordStateID = 1,
                       UserID = UserManager.UserExtended.UserID

                   }).FirstOrDefault();

            if (caseId <= 0) return Json(new { Status = "Fail" });
          
            UserManager.UpdateCaseStatusBar(caseId);
            TempData["AddApptCaseFormData"] = (AddAppointmentCaseSearchViewModelForSession)(new AddAppointmentCaseSearchViewModelForSession()).InjectFrom(searchParams);



            return Json(new { Status = "Done", URL = "/CaseOpening/AddCaseRespondents" });
        }

    }
}