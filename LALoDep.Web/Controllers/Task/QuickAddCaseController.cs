using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.CaseAttribute;
using LALoDep.Domain.pd_Allegation;
using LALoDep.Domain.pd_Association;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.Services;
using LALoDep.Domain.sup_Case;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Task;
using LALoDep.Domain.Agency;
using LALoDep.Core.Custom.Extensions;
using System.Collections.Generic;
using LALoDep.Custom;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Address;
using System.Globalization;
using pd_CodeGetNewNoteTypeByCaseID_spParams = LALoDep.Domain.pd_Code.pd_CodeGetNewNoteTypeByCaseID_spParams;
using pd_CodeGetNewNoteTypeByCaseID_spResult = LALoDep.Domain.pd_Code.pd_CodeGetNewNoteTypeByCaseID_spResult;
using LALoDep.Domain.qcal;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AddApptCasePage, PageSecurityItemID = SecurityToken.ViewCase)]
        public virtual ActionResult QuickAddCaseSearch()
        {

            if (Request.QueryString["_uniquerequest"] == null)//not ajax request
                UserManager.UpdateCaseStatusBar(0);

            var viewModel = new QuickAddCaseSearchViewModel();
            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<AgencyGetByUserID_spResult>("AgencyGetByUserID_sp",
                                            new AgencyGetByUserID_spParams { SortOption = "AgencyName", BatchLogJobID = Guid.NewGuid(), UserID = UserManager.UserExtended.UserID })
                                        .Select(o => new SelectListItem() { Text = o.AgencyName, Value = o.AgencyID + "|" + o.UniquePetitionNumbersFlag.ToString() }).ToList();

            viewModel.AttorneyList = UtilityService.ExecStoredProcedureWithResults<qcal_AttorneyListForQuickCaseAdd_spResult>("qcal_AttorneyListForQuickCaseAdd_sp",
                                          new qcal_AttorneyListForQuickCaseAdd_spParams { AttorneyPersonID = UserManager.UserExtended.PersonID, BatchLogJobID = Guid.NewGuid(), UserID = UserManager.UserExtended.UserID }).ToList();



            if (viewModel.AgencyList.Count() == 1)
            {

                viewModel.AgencyID = viewModel.AgencyList.FirstOrDefault().Value.Split('|')[0].ToInt();

            }
            return View(viewModel);
        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AddApptCasePage)]

        public virtual JsonResult QuickAddCaseSearch(QuickAddCaseSearchViewModel searchParams)
        {
            if (Request.QueryString["Bypass"] != null)
            {
                TempData["QuickCaseFormData"] = searchParams;

                return Json(new { Status = "CaseRedirect", URL = "/Task/QuickAddCase" });
            }
            var list = UtilityService.ExecStoredProcedureWithResults<pd_NewCaseSearch_spResult>("pd_NewCaseSearch_sp",
                   new pd_NewCaseSearch_spParams()
                   {
                       AgencyID = searchParams.AgencyID.Value,
                       BatchLogJobID = Guid.NewGuid(),
                       DocketNumber = searchParams.CaseNumber1,
                       DocketNumber2 = searchParams.CaseNumber2 ?? string.Empty,
                       DocketNumber3 = searchParams.CaseNumber3 ?? string.Empty,
                       MotherDOB = searchParams.MotherDOB ?? string.Empty,
                       UserID = UserManager.UserExtended.UserID,
                       MotherFirstName = searchParams.MotherFirstName ?? string.Empty,
                       MotherLastName = searchParams.MotherLastName ?? string.Empty,
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
                       FatherLastName2 = searchParams.FatherLastName2 ?? string.Empty

                   }).ToList();


            var total = list.Count;
            var searchList = list.Select(c => new
            {
                c.PersonNameLast,
                c.PersonNameFirst,
                c.DOB,
                c.Sex,
                c.Role,

                c.ClosedDate,

                c.CaseNumber,//need to confirn
                c.LeadAttorney,
                c.RoleID,
                c.CaseID,
                EncryptedCaseID = c.CaseID.ToEncrypt(),
                c.KeySequence,
                c.PetitionDocketNumber,
                c.Agency
                ,
                c.RoleClient
            }).ToList();

            //var url =                string.Format("/Task/QuickAddCase/{0}?case1={1}&case2={2}&case3={3}&mFirstName={4}&mLastName={5}&mDOB={6}", searchParams.AgencyID.ToEncrypt(), searchParams.CaseNumber1, searchParams.CaseNumber2.IsNullOrEmpty() ? searchParams.CaseNumber1 : searchParams.CaseNumber2, searchParams.CaseNumber3.IsNullOrEmpty() ? "" : searchParams.CaseNumber3, searchParams.MotherFirstName, searchParams.MotherLastName, searchParams.MotherDOB);
            var url =
              string.Format("/Task/QuickAddCase/{0}", searchParams.AgencyID.ToEncrypt());
            TempData["QuickCaseFormData"] = searchParams;

            if (total > 0)
                return Json(new { Status = "Done", SearchData = new DataTablesResponse(0, searchList, total, total), URL = url });




            return Json(new { Status = "CaseRedirect", URL = url });

        }



        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AddApptCasePage, PageSecurityItemID = SecurityToken.AddCase)]
        public virtual ActionResult QuickAddCase(string id, string case1, string case2 = "", string case3 = "", string mFirstName = "", string mLastName = "", string mDOB = "")
        {
            int agencyId = id.ToDecrypt().ToInt();
            var viewModel = new QuickAddCaseViewModel();
            if (TempData["QuickCaseFormData"] != null)
            {
                var formData = (QuickAddCaseSearchViewModel)TempData["QuickCaseFormData"];
                agencyId = viewModel.AgencyID = formData.AgencyID.Value;
                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Parent 1", IsParent = true, FirstName = formData.MotherFirstName, LastName = formData.MotherLastName, DOB = formData.MotherDOB, RoleID = 782, SexTypeCodeID = 762, ChildrenAssociationTypeCodeID = 1397 });//Mother Role
                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Child 1", CaseNumber = formData.CaseNumber1, RoleID = 3, FirstName = formData.ChildFirstName1, LastName = formData.ChildLastName1, DOB = formData.Child1DOB });//Child
                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Child 2", CaseNumber = formData.CaseNumber2, RoleID = 3, FirstName = formData.ChildFirstName2, LastName = formData.ChildLastName2, DOB = formData.Child2DOB });//Child
                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Child 3", CaseNumber = formData.CaseNumber3, RoleID = 3, FirstName = formData.ChildFirstName3, LastName = formData.ChildLastName3, DOB = formData.Child3DOB });//Child

                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Parent 2", IsParent = true, RoleID = 774, FirstName = formData.FatherFirstName1, LastName = formData.FatherLastName1, DOB = formData.Father1DOB });
                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Parent 3", IsParent = true, RoleID = 774, FirstName = formData.FatherFirstName2, LastName = formData.FatherLastName2, DOB = formData.Father2DOB });
            }
            else
            {
                viewModel.AgencyID = agencyId;
                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Parent 1", IsParent = true, FirstName = mFirstName, LastName = mLastName, DOB = mDOB, RoleID = 782, SexTypeCodeID = 762, ChildrenAssociationTypeCodeID = 1397 });//Mother Role
                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Child 1", CaseNumber = case1, RoleID = 3 });//Child
                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Child 2", CaseNumber = case2, RoleID = 3 });//Child
                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Child 3", CaseNumber = case3, RoleID = 3 });//Child

                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Parent 2", IsParent = true });
                viewModel.CasePersonList.Add(new QuickAddCasePerson() { CheckBoxLabel = "Parent 3", IsParent = true });
            }





            viewModel.SexList = UtilityFunctions.CodeGetByTypeIdAndUserId(1, userShortValue: true, agencyId: viewModel.AgencyID);

            viewModel.HearingDepartmentList = UtilityFunctions.CodeGetByTypeIdAndUserId(30, agencyId: viewModel.AgencyID);

            viewModel.ChildrenAssociationTypeList = UtilityFunctions.CodeGetByTypeIDAndUserIDSortShortValue(24, combineLongValue: true, agencyId: viewModel.AgencyID);

            viewModel.HearingOfficerList = UtilityService.ExecStoredProcedureWithResults<pd_HearingOfficerGet_spResults>("pd_HearingOfficerGet_sp",
                                            new pd_HearingOfficerGet_spParams { UserID = UserManager.UserExtended.UserID, AgencyID = agencyId, BatchLogJobID = Guid.NewGuid() })
                                            .Select(o => new SelectListItem() { Value = o.PersonID.ToString(), Text = o.PersonNameLast + ", " + o.PersonNameFirst }).ToList();

            viewModel.RoleParentList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetBySystemValueTypeID_spResults>("pd_CodeGetBySystemValueTypeID_sp",
                                        new pd_CodeGetBySystemValueTypeID_spParams { SystemValueIDList = "1", BatchLogJobID = Guid.NewGuid(), UserID = UserEnvironment.UserManager.UserExtended.UserID, AgencyID = viewModel.AgencyID })
                                        .Where(x => x.Selected == 1)
                                        .Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() });

            viewModel.RoleChildList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetBySystemValueTypeID_spResults>("pd_CodeGetBySystemValueTypeID_sp",
                                    new pd_CodeGetBySystemValueTypeID_spParams { SystemValueIDList = "3", BatchLogJobID = Guid.NewGuid(), UserID = UserEnvironment.UserManager.UserExtended.UserID, AgencyID = viewModel.AgencyID })
                                    .Where(x => x.Selected == 1)
                                    .Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() });

            viewModel.PetitionTypeList = UtilityFunctions.CodeGetBySystemValueTypeId("50", agencyId: viewModel.AgencyID);

            viewModel.AttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetLegalSpecific_spResult>("pd_RoleGetLegalSpecific_sp",
                                                new pd_RoleGetLegalSpecific_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid(), CaseID = (-1 * agencyId), AgencyID = viewModel.AgencyID })
                                                .Select(o => new SelectListItem() { Value = o.PersonID.ToString() + "|" + o.RealLegalTypeID.Value.ToString(), Text = o.FullName });

            viewModel.HearingTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(10,agencyId:viewModel.AgencyID);
            viewModel.NoteTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetNewNoteTypeByCaseID_spResult>("pd_CodeGetNewNoteTypeByCaseID_sp",
                                                    new pd_CodeGetNewNoteTypeByCaseID_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid(), CaseAgencyID = viewModel.AgencyID })
                                                    .Select(o => new SelectListItem() { Value = o.CodeID.ToString(), Text = o.NoteTypeDisplay });

            viewModel.StateList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(5, agencyId: viewModel.AgencyID);
            viewModel.AllegationList = UtilityFunctions.CodeGetByTypeIdAndUserId(22, agencyId: viewModel.AgencyID);
            viewModel.DesignatedDayList = UtilityFunctions.CodeGetByTypeIdAndUserId(955, sortOption: "CodeValue", agencyId: viewModel.AgencyID);
            viewModel.CaseRefrelSourceList = UtilityFunctions.CodeGetByTypeIdAndUserId(81, sortOption: "CodeValue", agencyId: viewModel.AgencyID);
            viewModel.PlacementAddressList = UtilityService.ExecStoredProcedureWithResults<pd_AddressGetPlacementCodesByCaseID_spResult>("pd_AddressGetPlacementCodesByCaseID_sp",
                                        new pd_AddressGetPlacementCodesByCaseID_spParams { CaseID = UserManager.UserExtended.CaseID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid(), AgencyID = agencyId })
                                        .Select(o => new SelectListItem { Text = o.AddressStreet + @"," + o.AddressCity + @"," + o.State + @"," + o.AddressZipCode + @"," + o.AddressHomePhone, Value = o.AddressID.ToString() });

            viewModel.StateID = 841;//California
            viewModel.PetitionFileDate = viewModel.AppointmentDate = DateTime.Now.ToString("d");
            var caseDefault =
             UtilityService.ExecStoredProcedureWithResults<pd_CaseGetDefaults_spResults>("pd_CaseGetDefaults_sp",
                 new pd_CaseGetDefaults_spParams
                 {
                     UserID = UserManager.UserExtended.UserID,
                     AgencyID = viewModel.AgencyID,
                     BatchLogJobID = Guid.NewGuid(),
                     CaseID = UserManager.UserExtended.CaseID,

                 }).FirstOrDefault();
            if (caseDefault != null)
            {

                viewModel.HearingTime = caseDefault.DefaultHearingTime;
            }
            if (!Request.QueryString["attorneyId"].IsNullOrEmpty())
            {
                viewModel.AttorneyPersonID = Request.QueryString["attorneyId"].ToDecrypt().ToInt();
            }


            var quickCaseAddGetDefaults =
        UtilityService.ExecStoredProcedureWithResults<qcal_QuickCaseAddGetDefaults_spResult>("qcal_QuickCaseAddGetDefaults_sp",
            new qcal_QuickCaseAddGetDefaults_spParams
            {
                UserID = UserManager.UserExtended.UserID,

                BatchLogJobID = Guid.NewGuid(),
                AgencyID = agencyId,
                AttorneyPersonID = viewModel.AttorneyPersonID
            }).FirstOrDefault();
            if (quickCaseAddGetDefaults != null)
            {
                viewModel.AppointmentDate = "";
                viewModel.PetitionFileDate = "";
                if (quickCaseAddGetDefaults.ApptDate.HasValue)
                    viewModel.AppointmentDate = quickCaseAddGetDefaults.ApptDate.Value.ToString("d");

                if (quickCaseAddGetDefaults.CaseDepartmentCodeID.HasValue)
                    viewModel.CaseDepartmentID = quickCaseAddGetDefaults.CaseDepartmentCodeID;

                if (quickCaseAddGetDefaults.CaseJudgePersonID.HasValue)
                    viewModel.CaseOfficerPersonID = quickCaseAddGetDefaults.CaseJudgePersonID;

                if (quickCaseAddGetDefaults.PetitionFileDate.HasValue)
                    viewModel.PetitionFileDate = quickCaseAddGetDefaults.PetitionFileDate.Value.ToString("d");

                if (quickCaseAddGetDefaults.HearingDate.HasValue)
                    viewModel.HearingDate = quickCaseAddGetDefaults.HearingDate.Value.ToString("d");

                if (quickCaseAddGetDefaults.HearingTypeCodeID.HasValue)
                    viewModel.HearingTypeID = quickCaseAddGetDefaults.HearingTypeCodeID;

                if (quickCaseAddGetDefaults.AttorneyPersonID.HasValue)
                    viewModel.AttorneyPersonID = quickCaseAddGetDefaults.AttorneyPersonID;

                if (quickCaseAddGetDefaults.CheckParent1Flag.HasValue && quickCaseAddGetDefaults.CheckParent1Flag.Value == 1)
                    viewModel.CasePersonList[0].IsDefaultChecked = true;

                if (quickCaseAddGetDefaults.CheckChild1Flag.HasValue && quickCaseAddGetDefaults.CheckChild1Flag.Value == 1)
                    viewModel.CasePersonList[1].IsDefaultChecked = true;

                if (quickCaseAddGetDefaults.PetitionTypeCodeID.HasValue)
                    viewModel.PetitionTypeCodeID = quickCaseAddGetDefaults.PetitionTypeCodeID;

                viewModel.DOBRequiredForChildren = quickCaseAddGetDefaults.DOBRequiredForChildren.ToInt();

            }
            return View(viewModel);
        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AddApptCasePage,
            PageSecurityItemID = SecurityToken.AddCase)]
        public virtual ActionResult QuickAddCase(QuickAddCaseViewModel model)
        {
            var caseNumber = model.ChildList.First().CaseNumber;

            #region Case Insert and Designated Day Insert

            var caseId = UtilityService.ExecStoredProcedureWithResults<int>("pd_CaseInsert_sp",
                new pd_CaseInsert_spParams()
                {
                    AgencyID = model.AgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    CaseAppointmentDate = model.AppointmentDate.ToDateTime(),
                    CaseNumber = caseNumber,
                    CasePanelCase = (short)(model.IsPanel ? 1 : 0),
                    DepartmentID = model.CaseDepartmentID.HasValue ? model.CaseDepartmentID.Value : 0,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID

                }).FirstOrDefault();
            if (model.DesignatedDayCodeID.HasValue && model.DesignatedDayCodeID.Value > 0)
            {
                var id = UtilityService.ExecStoredProcedureScalar("CaseAttributeInsert_sp",
                    new CaseAttributeInsert_spParams()
                    {
                        CaseID = caseId,
                        CaseAttributeTypeID = 2000,
                        CaseAttributeGenericValue = model.DesignatedDayCodeID.ToString(),
                        TableID = caseId,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        RecordStateID = 1

                    });
            }

            #endregion


            #region Judge and Attorney Role Insert

            var judgeRoleId = 0;
            if (model.CaseOfficerPersonID.HasValue)
            {
                judgeRoleId =
                    UtilityService.ExecStoredProcedureWithResults<int>("pd_RoleInsert_sp", new pd_RoleInsert_spParams
                    {
                        CaseID = caseId,
                        AgencyID = model.AgencyID,
                        BatchLogJobID = Guid.NewGuid(),
                        PersonID = model.CaseOfficerPersonID.Value,
                        RecordStateID = 1,
                        RoleClient = 0,
                        RoleStartDate = model.AppointmentDate.ToDateTime(),
                        RoleTypeCodeID = 7, //Judge
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();
            }
            var attorneyRoleId = 0;

            if (model.AttorneyPersonID.HasValue)
            {
                attorneyRoleId =
                    UtilityService.ExecStoredProcedureWithResults<int>("pd_RoleInsert_sp", new pd_RoleInsert_spParams
                    {
                        CaseID = caseId,
                        AgencyID = model.AgencyID,
                        BatchLogJobID = Guid.NewGuid(),
                        PersonID = model.AttorneyPersonID.Value,
                        RecordStateID = 1,
                        RoleClient = 0,
                        RoleStartDate = model.AppointmentDate.ToDateTime(),
                        RoleTypeCodeID = model.AttorneyRoleTypeID, //Attorney (Fallman)
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();
            }

            #endregion



            #region Address

            var addressId = 0;
            if (model.StateID != null && !model.Street.IsNullOrEmpty())
            {
                addressId =
                    UtilityService.ExecStoredProcedureScalar("pd_AddressInsert_sp",
                        new pd_AddressInsert_spParams()
                        {
                            AddressCity = model.City,

                            AddressHomePhone = model.AddressPhone,
                            AddressStreet = model.Street,
                            AddressStateCodeID = model.StateID.Value,
                            AddressZipCode = model.ZipCode,
                            BatchLogJobID = Guid.NewGuid(),
                            AgencyID = model.AgencyID,
                            UserID = UserManager.UserExtended.UserID,
                            RecordStateID = 1,
                            RecordTimeStamp = null,

                        }).ToInt();

            }

            #endregion


            #region Child List Insert

            var childPersonIds = new List<int>();
            var petitionIds = new List<int>();
            int? spanishLangaugeCode = 18864;
            foreach (var child in model.ChildList)
            {
                var personId =
                       UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonInsert_sp",
                           new pd_PersonInsert_spParams()
                           {
                               AgencyID = model.AgencyID,
                               BatchLogJobID = Guid.NewGuid(),
                               UserID = UserManager.UserExtended.UserID,
                               RecordStateID = 1,
                               PersonSexCodeID = child.SexTypeCodeID.HasValue ? child.SexTypeCodeID.Value : 0,
                               PersonDOB = !child.DOB.IsNullOrEmpty() ? child.DOB.ToDateTime() : (DateTime?)null,
                               PersonLanguageCodeID = child.IsSS ? spanishLangaugeCode : null
                           }).FirstOrDefault();

                childPersonIds.Add(personId);

                var personNameId =
                    UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonNameInsert_sp",
                        new pd_PersonNameInsert_spParams()
                        {
                            AgencyID = model.AgencyID,
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID,
                            RecordStateID = 1,
                            PersonID = personId,
                            PersonNameFirst = child.FirstName,
                            PersonNameLast = child.LastName,
                            PersonNameTypeCodeID = 3200,

                        }).FirstOrDefault();

                var roleId = UtilityService.ExecStoredProcedureScalar("pd_RoleInsert_sp", new pd_RoleInsert_spParams
                {
                    CaseID = caseId,
                    AgencyID = model.AgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    PersonID = personId,
                    RecordStateID = 1,
                    RoleClient = (byte)(child.IsClient ? 1 : 0),
                    RoleStartDate = model.AppointmentDate.ToDateTime(),
                    RoleTypeCodeID = child.RoleID.Value,//Judge
                    UserID = UserManager.UserExtended.UserID
                }).ToInt();
                if (model.PetitionTypeCodeID.HasValue)
                {

                    var petition = UtilityService.Context.pd_PetitionInsert_sp(new ObjectParameter("PetitionID", 0), model.AgencyID,
                    caseId, model.PetitionFileDate.ToDateTime(), !child.CaseNumber.IsNullOrEmpty() ? child.CaseNumber : caseNumber, model.PetitionTypeCodeID,
                  (DateTime?)null, 1, new ObjectParameter("RecordTimeStamp", ""),
                    UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();

                    if (petition != null)
                    {
                        var petitionId = (int)petition;
                        petitionIds.Add(petitionId);
                        if (!model.PhysicalFileName.IsNullOrEmpty())
                        {
                            var caseAttributeId = UtilityService.ExecStoredProcedureWithResults<int>("CaseAttributeInsert_sp", new CaseAttributeInsert_spParams()
                            {

                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID,
                                RecordStateID = 1,
                                CaseID = caseId,
                                CaseAttributeGenericValue = model.PhysicalFileName,
                                CaseAttributeTypeID = 3000,
                                TableID = petitionId,
                            }).FirstOrDefault();
                        }

                        if (model.Allegation1ID.HasValue)
                            AllegationInsert(model.Allegation1ID.Value, petitionId, model.AgencyID);
                        if (model.Allegation2ID.HasValue)
                            AllegationInsert(model.Allegation2ID.Value, petitionId, model.AgencyID);
                        if (model.Allegation3ID.HasValue)
                            AllegationInsert(model.Allegation3ID.Value, petitionId, model.AgencyID);
                        if (model.Allegation4ID.HasValue)
                            AllegationInsert(model.Allegation4ID.Value, petitionId, model.AgencyID);
                        if (model.Allegation5ID.HasValue)
                            AllegationInsert(model.Allegation5ID.Value, petitionId, model.AgencyID);
                        if (model.Allegation6ID.HasValue)
                            AllegationInsert(model.Allegation6ID.Value, petitionId, model.AgencyID);

                        if (model.DesignatedDayCodeID.HasValue)
                        {
                            UtilityService.ExecStoredProcedureScalar("CaseAttributeInsert_sp",
                              new CaseAttributeInsert_spParams()
                              {
                                  CaseID = caseId,
                                  CaseAttributeTypeID = 2000,
                                  CaseAttributeGenericValue = model.DesignatedDayCodeID.Value.ToString(),
                                  TableID = roleId,
                                  BatchLogJobID = Guid.NewGuid(),
                                  UserID = UserManager.UserExtended.UserID,
                                  RecordStateID = 1

                              });
                        }

                        PetitionRoleInsert(model.AgencyID, petitionId, roleId);
                        PetitionRoleInsert(model.AgencyID, petitionId, attorneyRoleId);
                        PetitionRoleInsert(model.AgencyID, petitionId, judgeRoleId);
                        //if (child.IsClient)
                        //{
                        //    UtilityService.ExecStoredProcedureWithResults<object>("pd_CaseUpdateCaseName_sp", new pd_CaseUpdateCaseName_spParams()
                        //    {
                        //        BatchLogJobID = Guid.NewGuid(),
                        //        UserID = UserManager.UserExtended.UserID,
                        //        CaseID = caseId,
                        //        CaseNameRoleID = roleId
                        //    }).FirstOrDefault();
                        //}

                        if (child.HasAddress && addressId > 0)
                        {
                            var personAddressId =
                                UtilityService.ExecStoredProcedureScalar("pd_PersonAddressInsert_sp",
                                    new pd_PersonAddressInsert_spParams()
                                    {
                                        AddressID = addressId,
                                        PersonAddressEndDate = (DateTime?)null,
                                        PersonAddressStartDate = DateTime.Now,

                                        PersonAddressTypeCodeID = 1515,//Home
                                        BatchLogJobID = Guid.NewGuid(),
                                        AgencyID = model.AgencyID,
                                        UserID = UserManager.UserExtended.UserID,
                                        PersonID = personId,
                                        PersonAddressWorkPhone = model.AddressPhone,
                                        PersonAddressConfidential = 0,

                                        RecordStateID = 1,

                                    }).ToInt();
                        }

                    }
                }

            }

            #endregion
            #region Parent List Insert
            foreach (var child in model.ParentList)
            {
                var personId =
                       UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonInsert_sp",
                           new pd_PersonInsert_spParams()
                           {
                               AgencyID = model.AgencyID,
                               BatchLogJobID = Guid.NewGuid(),
                               UserID = UserManager.UserExtended.UserID,
                               RecordStateID = 1,
                               PersonSexCodeID = child.SexTypeCodeID.HasValue ? child.SexTypeCodeID.Value : 0,
                               PersonDOB = !child.DOB.IsNullOrEmpty() ? child.DOB.ToDateTime() : (DateTime?)null,
                               PersonLanguageCodeID = child.IsSS ? spanishLangaugeCode : null
                           }).FirstOrDefault();

                var personNameId =
                    UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonNameInsert_sp",
                        new pd_PersonNameInsert_spParams()
                        {
                            AgencyID = model.AgencyID,
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID,
                            RecordStateID = 1,
                            PersonID = personId,
                            PersonNameFirst = child.FirstName,
                            PersonNameLast = child.LastName,
                            PersonNameTypeCodeID = 3200,

                        }).FirstOrDefault();

                var roleId = UtilityService.ExecStoredProcedureScalar("pd_RoleInsert_sp", new pd_RoleInsert_spParams
                {
                    CaseID = caseId,
                    AgencyID = model.AgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    PersonID = personId,
                    RecordStateID = 1,
                    RoleClient = (byte)(child.IsClient ? 1 : 0),
                    RoleStartDate = model.AppointmentDate.ToDateTime(),
                    RoleTypeCodeID = child.RoleID.Value,//Judge
                    UserID = UserManager.UserExtended.UserID
                }).ToInt();
                foreach (var petitionId in petitionIds)
                {
                    PetitionRoleInsert(model.AgencyID, petitionId, roleId);

                }


                //if (child.IsClient)
                //{
                //    UtilityService.ExecStoredProcedureWithResults<object>("pd_CaseUpdateCaseName_sp", new pd_CaseUpdateCaseName_spParams()
                //    {
                //        BatchLogJobID = Guid.NewGuid(),
                //        UserID = UserManager.UserExtended.UserID,
                //        CaseID = caseId,
                //        CaseNameRoleID = roleId
                //    }).FirstOrDefault();
                //}

                if (child.HasAddress && addressId > 0)
                {
                    var personAddressId =
                        UtilityService.ExecStoredProcedureScalar("pd_PersonAddressInsert_sp",
                            new pd_PersonAddressInsert_spParams()
                            {
                                AddressID = addressId,
                                PersonAddressEndDate = (DateTime?)null,
                                PersonAddressStartDate = DateTime.Now,

                                PersonAddressTypeCodeID = 1515,//Home
                                BatchLogJobID = Guid.NewGuid(),
                                AgencyID = model.AgencyID,
                                UserID = UserManager.UserExtended.UserID,
                                PersonID = personId,
                                PersonAddressWorkPhone = model.AddressPhone,
                                PersonAddressConfidential = 0,

                                RecordStateID = 1,

                            }).ToInt();
                }
                if (child.ChildrenAssociationTypeCodeID.HasValue)
                {
                    foreach (var pid in childPersonIds)
                    {


                        UtilityService.ExecStoredProcedureWithResults<decimal>("pd_AssociationInsert_sp", new pd_AssociationInsert_spParams
                        {
                            AssociationCodeID = child.ChildrenAssociationTypeCodeID.Value,
                            CaseID = caseId,
                            AgencyID = model.AgencyID,
                            BatchLogJobID = Guid.NewGuid(),
                            PersonID = personId,
                            RecordStateID = 1,
                            RelatedPersonID = pid,
                            AssociationStartDate = model.AppointmentDate.ToDateTime(),

                            UserID = UserManager.UserExtended.UserID

                        }).FirstOrDefault();
                    }
                }


            }


            #endregion

            #region Hearing

            if (model.HearingTypeID.HasValue)
            {
                var hearingId = UtilityService.ExecStoredProcedureScalar("pd_HearingInsert_sp", new pd_HearingInsert_spParams
                {
                    CaseID = caseId,

                    BatchLogJobID = Guid.NewGuid(),
                    HearingCourtDepartmentCodeID = model.HearingDepartmentID.Value,
                    HearingDateTime = DateTime.Parse(model.HearingDate + " " + model.HearingTime),
                    HearingOfficerPersonID = model.HearingOfficerID.Value,
                    HearingTypeCodeID = model.HearingTypeID.Value,
                    UserID = UserManager.UserExtended.UserID,
                    RecordStateID = 1,
                }).ToInt();

                foreach (var petitionId in petitionIds)
                {
                    UtilityService.ExecStoredProcedureScalar("pd_HearingPersonInsertByPetitionID_sp", new pd_HearingPersonInsertByPetitionID_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        HearingID = hearingId,
                        PetitionID = petitionId
                    });
                }

                if (model.AppearingAttorneyID.HasValue)
                {

                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingAttendanceInsert_sp", new pd_HearingAttendanceInsert_spParams()
                    {
                        HearingID = hearingId,
                        NewAttendingAttorneyPersonID = model.AppearingAttorneyID.HasValue ? model.AppearingAttorneyID.Value : (int?)null,
                        RoleID = 0,
                        AgencyID = model.AgencyID,
                        RecordStateID = 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();
                }


            }
            if (model.OtherHearingTypeID.HasValue)
            {
                var hearingId = UtilityService.ExecStoredProcedureScalar("pd_HearingInsert_sp", new pd_HearingInsert_spParams
                {
                    CaseID = caseId,

                    BatchLogJobID = Guid.NewGuid(),
                    HearingCourtDepartmentCodeID = model.OtherHearingDepartmentID.Value,
                    HearingDateTime = DateTime.Parse(model.OtherHearingDate + " " + model.OtherHearingTime),
                    HearingOfficerPersonID = model.OtherHearingOfficerID.Value,
                    HearingTypeCodeID = model.OtherHearingTypeID.Value,
                    UserID = UserManager.UserExtended.UserID,
                    RecordStateID = 1,
                }).ToInt();

                foreach (var petitionId in petitionIds)
                {
                    UtilityService.ExecStoredProcedureScalar("pd_HearingPersonInsertByPetitionID_sp", new pd_HearingPersonInsertByPetitionID_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        HearingID = hearingId,
                        PetitionID = petitionId
                    });
                }

                if (model.OtherAppearingAttorneyID.HasValue)
                {

                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingAttendanceInsert_sp", new pd_HearingAttendanceInsert_spParams()
                    {
                        HearingID = hearingId,
                        NewAttendingAttorneyPersonID = model.OtherAppearingAttorneyID.HasValue ? model.OtherAppearingAttorneyID.Value : (int?)null,
                        RoleID = 0,
                        AgencyID = model.AgencyID,
                        RecordStateID = 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();
                }


            }
            if (!model.Note.IsNullOrEmpty())
            {
                var noteId = UtilityFunctions.NoteInsert(112, 123, caseId, model.NoteTypeID.Value, model.NoteSubject, model.Note, caseId: caseId);
                if (model.IsPanel)
                {
                    UtilityService.ExecStoredProcedureScalar("pd_NotePanelInsert_sp", new pd_NotePanelInsert_spParams()
                    {

                        NoteID = noteId,
                        NotePanelCodeID = 1961,
                        RecordStateID = 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID
                    });

                }

            }
            #endregion
            if (model.CaseRefrelSourceID.HasValue)
            {
                UtilityService.ExecStoredProcedureScalar("CaseAttributeInsert_sp",
                  new CaseAttributeInsert_spParams()
                  {
                      CaseID = caseId,
                      CaseAttributeTypeID = 1020,
                      CaseAttributeGenericValue = model.CaseRefrelSourceID.Value.ToString(),
                      TableID = caseId,
                      BatchLogJobID = Guid.NewGuid(),
                      UserID = UserManager.UserExtended.UserID,
                      RecordStateID = 1

                  });
            }
            UtilityService.ExecStoredProcedureScalar("sup_CasePetitionNumberSet_sp",
                  new sup_CasePetitionNumberSet_spParams()
                  {
                      CaseID = caseId,
                      AdminFlag = 0,
                      BatchLogJobID = Guid.NewGuid(),
                      UserID = UserManager.UserExtended.UserID
                  });
            UtilityService.ExecStoredProcedureScalar("sup_SetAttorneyFlags_sp",
              new sup_SetAttorneyFlags_spParams()
              {
                  CaseID = caseId,
                  AdminFlag = 0,
                  BatchLogJobID = Guid.NewGuid(),
                  UserID = UserManager.UserExtended.UserID
              });
            UtilityService.ExecStoredProcedureScalar("sup_CaseNameSet_sp",
             new sup_CaseNameSet_spParams()
             {
                 CaseID = caseId,

                 BatchLogJobID = Guid.NewGuid(),
                 UserID = UserManager.UserExtended.UserID
             });
            return Json(new
            {
                Status = "Done",
                MainUrl = "/Case/Main/" + caseId.ToEncrypt()
            });
        }


        private int AllegationInsert(int allegationTypeCodeID, int petitionId, int agencyId)
        {
            return UtilityService.ExecStoredProcedureScalar("pd_AllegationInsert_sp", new pd_AllegationInsert_spParams()
            {
                AgencyID = agencyId,
                AllegationTypeCodeID = allegationTypeCodeID,
                PetitionID = petitionId,
                RecordStateID = 1,
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID
            }).ToInt();
        }
        private int PetitionRoleInsert(int agencyId, int petitionId, int roleId)
        {
            return (int)UtilityService.Context.pd_PetitionRoleInsert_sp(new ObjectParameter("PetitionRoleID", 0),
                agencyId,
                petitionId, roleId, 1, new ObjectParameter("RecordTimeStamp", ""), UserManager.UserExtended.UserID, Guid.NewGuid())
                .FirstOrDefault();
        }


    }

}