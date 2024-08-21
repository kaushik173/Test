using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Domain.CaseAttribute;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Person;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.CaseOpening;

namespace LALoDep.Controllers.CaseOpening
{
    public partial class CaseOpeningController : Controller
    {
        // GET: CaseOpening
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.AddRole, IsCasePage = true)]
        public virtual ActionResult AddCaseRespondents(string dataentry)
        {

            var model = new AddChildOrParentsViewModel
            {

                SexList = UtilityFunctions.CodeGetByTypeIdAndUserId(1, agencyId: UserManager.UserExtended.CaseNumberAgencyID),
                RaceList = UtilityFunctions.CodeGetByTypeIdAndUserId(35, agencyId: UserManager.UserExtended.CaseNumberAgencyID),
                DesignatedDayList = UtilityFunctions.CodeGetBySystemValueTypeId(231, agencyId: UserManager.UserExtended.CaseNumberAgencyID),
                RoleList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetBySystemValueTypeID_spResults>(
                    "pd_CodeGetBySystemValueTypeID_sp", new pd_CodeGetBySystemValueTypeID_spParams
                    {
                        SystemValueIDList = "1,3",
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserEnvironment.UserManager.UserExtended.UserID,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID

                    })
                    .Where(o => o.SystemValueID.HasValue)
                    .Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() })
                    .ToList(),

            };

            if (TempData["AddApptCaseFormData"] != null)
            {
                var formData = (AddAppointmentCaseSearchViewModelForSession)TempData["AddApptCaseFormData"];


                if (!formData.MotherFirstName.IsNullOrEmpty())
                    model.AddChildOrParentList.Add(new AddChildOrParent() { FirstName = formData.MotherFirstName, LastName = formData.MotherLastName, DOB = formData.MotherDOB, RoleID = 782, SexID = 762, StartOrApptDate = formData.AppointmentDate.ToString("d") });//Mother Role

                if (!formData.ChildFirstName1.IsNullOrEmpty())
                    model.AddChildOrParentList.Add(new AddChildOrParent() { RoleID = 3, FirstName = formData.ChildFirstName1, LastName = formData.ChildLastName1, DOB = formData.Child1DOB, StartOrApptDate = formData.AppointmentDate.ToString("d") });//Child
                if (!formData.ChildFirstName2.IsNullOrEmpty())
                    model.AddChildOrParentList.Add(new AddChildOrParent() { RoleID = 3, FirstName = formData.ChildFirstName2, LastName = formData.ChildLastName2, DOB = formData.Child2DOB, StartOrApptDate = formData.AppointmentDate.ToString("d") });//Child
                if (!formData.ChildFirstName3.IsNullOrEmpty())
                    model.AddChildOrParentList.Add(new AddChildOrParent() { RoleID = 3, FirstName = formData.ChildFirstName3, LastName = formData.ChildLastName3, DOB = formData.Child3DOB, StartOrApptDate = formData.AppointmentDate.ToString("d") });//Child

                if (!formData.FatherFirstName1.IsNullOrEmpty())
                    model.AddChildOrParentList.Add(new AddChildOrParent() { RoleID = 774, FirstName = formData.FatherFirstName1, LastName = formData.FatherLastName1, DOB = formData.Father1DOB, StartOrApptDate = formData.AppointmentDate.ToString("d") });
                if (!formData.FatherFirstName2.IsNullOrEmpty())
                    model.AddChildOrParentList.Add(new AddChildOrParent() { RoleID = 774, FirstName = formData.FatherFirstName2, LastName = formData.FatherLastName2, DOB = formData.Father2DOB, StartOrApptDate = formData.AppointmentDate.ToString("d") });

                model.AddChildOrParentList.Add(new AddChildOrParent() { StartOrApptDate = UserManager.UserExtended.ApptDate });
                model.AddChildOrParentList.Add(new AddChildOrParent() { StartOrApptDate = UserManager.UserExtended.ApptDate });

                model.AddChildOrParentList.Add(new AddChildOrParent() { StartOrApptDate = UserManager.UserExtended.ApptDate });
                model.AddChildOrParentList.Add(new AddChildOrParent() { StartOrApptDate = UserManager.UserExtended.ApptDate });
                model.AddChildOrParentList.Add(new AddChildOrParent() { StartOrApptDate = UserManager.UserExtended.ApptDate });

            }
            else
            {
                model.AddChildOrParentList.Add(new AddChildOrParent() { StartOrApptDate = UserManager.UserExtended.ApptDate });
                model.AddChildOrParentList.Add(new AddChildOrParent() { StartOrApptDate = UserManager.UserExtended.ApptDate });
                model.AddChildOrParentList.Add(new AddChildOrParent() { StartOrApptDate = UserManager.UserExtended.ApptDate });
                model.AddChildOrParentList.Add(new AddChildOrParent() { StartOrApptDate = UserManager.UserExtended.ApptDate });
                model.AddChildOrParentList.Add(new AddChildOrParent() { StartOrApptDate = UserManager.UserExtended.ApptDate });


            }

            model.RespondentList =
                UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDChildRespondent_spResult>(
                    "pd_RoleGetByCaseIDChildRespondent_sp", new pd_RoleGetByCaseIDChildRespondent_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()

                    }).ToList();

            if (model.RespondentList.Any(o => o.CaseNameFlag == 1))
            {
                model.IsCnAdded = true;
                model.IsVisibleCnCheckbox = false;
            }
            else
            {
                model.IsVisibleCnCheckbox = true;
            }


            if (model.RespondentList.Any(o => o.RoleClient == 1))
                model.IsRoleClientAdded = true;

            if (model.RespondentList.Any(o => o.ChildFlag == 1))
                model.IsChildAdded = true;
            var caseDefault =
  UtilityService.ExecStoredProcedureWithResults<pd_CaseGetDefaults_spResults>("pd_CaseGetDefaults_sp",
      new pd_CaseGetDefaults_spParams
      {
          UserID = UserManager.UserExtended.UserID,
          BatchLogJobID = Guid.NewGuid(),
          CaseID = UserManager.UserExtended.CaseID,
          AgencyID = UserManager.UserExtended.CaseNumberAgencyID
      }).FirstOrDefault();
            if (caseDefault != null)
            {
                model.DOBRequiredForChildren = caseDefault.DOBRequiredForChildren.ToInt();
            }
            return View(model);
        }


        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.AddRole, IsCasePage = true)]
        [HttpPost]
        public virtual JsonResult AddCaseRespondents(List<AddChildOrParent> respondents)
        {

            if (respondents != null && respondents.Any())
            {
                int? spanishLangaugeCode = 18864;

                foreach (var respondent in respondents)
                {
                    var personId = UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonInsert_sp", new pd_PersonInsert_spParams()
                    {
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        PersonDOB = !string.IsNullOrEmpty(respondent.DOB) ? respondent.DOB.ToDateTime() : (DateTime?)null,
                        PersonRaceCodeID = respondent.RaceID,
                        PersonSexCodeID = respondent.SexID,
                        RecordStateID = 1,
                        PersonLanguageCodeID = respondent.IsSs ? spanishLangaugeCode : null,
                    }).FirstOrDefault();

                    var personNameId = UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonNameInsert_sp", new pd_PersonNameInsert_spParams()
                    {
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        RecordStateID = 1,
                        PersonID = personId,
                        PersonNameFirst = respondent.FirstName,
                        PersonNameLast = respondent.LastName,
                        PersonNameTypeCodeID = 3200,

                    }).FirstOrDefault();

                    var roleId = UtilityService.ExecStoredProcedureWithResults<int>("pd_RoleInsert_sp", new pd_RoleInsert_spParams()
                    {
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        RecordStateID = 1,
                        PersonID = personId,
                        CaseID = UserManager.UserExtended.CaseID,
                        RoleClient = (byte)(respondent.IsClient ? 1 : 0),
                        RoleTypeCodeID = respondent.RoleID,
                        RoleStartDate = !string.IsNullOrEmpty(respondent.StartOrApptDate) ? respondent.StartOrApptDate.ToDateTime() : DateTime.Now,


                    }).FirstOrDefault();
                    if (respondent.DesignatedDayID > 0)
                    {
                        var caseAttributeId = UtilityService.ExecStoredProcedureWithResults<int>("CaseAttributeInsert_sp", new CaseAttributeInsert_spParams()
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID,
                            RecordStateID = 1,

                            CaseID = UserManager.UserExtended.CaseID,
                            CaseAttributeGenericValue = respondent.DesignatedDayID.ToString(),
                            CaseAttributeTypeID = 2000,
                            TableID = roleId,
                        }).FirstOrDefault();

                    }
                    if (respondent.IsCn)
                    {
                        try
                        {

                            UtilityService.ExecStoredProcedureWithResults<int>("pd_CaseUpdateCaseName_sp",
                                new pd_CaseUpdateCaseName_spParams()
                                {
                                    BatchLogJobID = Guid.NewGuid(),
                                    UserID = UserManager.UserExtended.UserID,

                                    CaseID = UserManager.UserExtended.CaseID,
                                    CaseNameRoleID = roleId
                                }).ToList();



                        }
                        catch (Exception)
                        {

                        }
                        UserManager.UpdateCaseStatusBar(UserManager.UserExtended.CaseID);
                    }
                }

            }

            return Json(new { Status = "Done" });

        }

    }
}