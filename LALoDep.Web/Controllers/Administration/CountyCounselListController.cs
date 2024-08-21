using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.AddEditCountyCounsel;
using LALoDep.Domain.pd_CountyCounselList;
using LALoDep.Domain.pd_JcatsGroup;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain.pd_UserGroups;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using LALoDep.Domain.pd_Person;

namespace LALoDep.Controllers.Administration
{
    public partial class CountyCounselListController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;
        public CountyCounselListController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CountyCounsel, PageSecurityItemID = SecurityToken.ViewCountyCounsel)]
        // GET: CountyCounselList
        public virtual ActionResult Search()
        {
            var viewModel = new CountyCounselListViewModel()
            {
                OnViewLoad = true
            };
            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupAgencyGetByJcatsUserID_spResults>("pd_JcatsGroupAgencyGetByJcatsUserID_sp", new pd_JcatsGroupAgencyGetByJcatsUserID_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).Select(x => new SelectListItem()
            {
                Text = x.AgencyName,
                Value = x.AgencyID.ToString()
            });
            if (viewModel.AgencyList.Count() == 1)
            {
                viewModel.AgencyID = viewModel.AgencyList.First().Value.ToInt();
            }

            viewModel.GroupList = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupGetByUserID_spResult>("pd_JcatsGroupGetByUserID_sp",
                new pd_JcatsGroupGetByUserID_spParams() { UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult Search(CountyCounselListViewModel parameters)
        {
            var result = UtilityService.ExecStoredProcedureWithResults<pd_AttorneyListSearchGet_spResult>("pd_AttorneyListSearchGet_sp",
                new pd_AttorneyListSearchGet_spParams() { AgencyID = parameters.AgencyID, FirstName = parameters.FirstName, LastName = parameters.LastName, BarNumber = parameters.BarNumber, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).Select(x => new
                {
                    FirstName = x.PersonNameFirst,
                    LastName = x.PersonNameLast,
                    StartDate = x.RoleStartDate.HasValue == true ? x.RoleStartDate.Value.ToString("MM/dd/yyyy") : null,
                    EndDate = x.RoleEndDate.HasValue == true ? x.RoleEndDate.Value.ToString("MM/dd/yyyy") : null,
                    BarNumber = x.BarNumber,
                    PersonID = x.PersonID.ToEncrypt()
                }).ToList();
            return Json(new DataTablesResponse(0, result, result.Count, result.Count));
        }

        [HttpPost]
        public virtual JsonResult CountyCounselDelete(string id)
        {
            UtilityService.ExecStoredProcedureWithoutResults("pd_StaffDelete_sp", new pd_StaffDelete_spParams
            {
                ID = id.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                RecordStateID = 10

            });
            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AddEditCountyCounsel, PageSecurityItemID = SecurityToken.EditUser)]
        public virtual ActionResult AddEditCountyCounsel(string Id)
        {
            var viewModel = new CountyCounselPerson();
            if (!string.IsNullOrEmpty(Id))
            {
                viewModel = UtilityService.ExecStoredProcedureWithResults<pd_StaffGetAttorneyByPersonID_spResult>("pd_StaffGetAttorneyByPersonID_sp",
              new pd_StaffGetAttorneyByPersonID_spParams() { PersonID = (!string.IsNullOrEmpty(Id)) ? Id.ToDecrypt().ToInt() : UserManager.UserExtended.PersonID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).Select(x => new CountyCounselPerson()
              {
                  Id = Id.ToDecrypt().ToInt(),
                  LastName = x.PersonNameLast,
                  PLastName = x.PersonNameLast,
                  FirstName = x.PersonNameFirst,
                  PFirstName = x.PersonNameFirst,
                  StartDate = x.RoleStartDate.HasValue ? x.RoleStartDate.Value.ToString("MM/dd/yyyy") : null,
                  EndDate = x.RoleEndDate.HasValue ? x.RoleEndDate.Value.ToString("MM/dd/yyyy") : null,
                  BarNumber = x.BarNumber,
                  PBarNumber = x.BarNumber,
                  RecordStateID = x.RecordStateID,
                  RoleID = x.RoleID,
                  AgencyID = x.AgencyID
              }).FirstOrDefault();

                viewModel.PersonId = viewModel.Id;
            }

            return View(viewModel);
        }

        [HttpGet]
        public virtual JsonResult GetAllCountyCheckboxes(string Id)
        {
            var agencyList =
                UtilityService.ExecStoredProcedureWithResults<pd_AgencyGetSystemRoleByPersonID_spResult>(
                    "pd_AgencyGetSystemRoleByPersonID_sp",
                    new pd_AgencyGetSystemRoleByPersonID_spParams()
                    {
                        PersonID = Id.ToDecrypt().ToInt(),
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        SortFlag = 0
                    }).Select(x => new
                    {
                        AgencyID = x.AgencyID,
                        AgencyName = x.AgencyName,
                        Selected = x.Selected,
                        RoleID = x.RoleID,
                        RoleStartDate = x.RoleStartDate
                    }).OrderBy(o => o.AgencyName).ToList();
            return Json(new DataTablesResponse(0, agencyList, agencyList.Count, agencyList.Count), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public virtual JsonResult UpdateCountyCounsel(CountyCounselPerson person, CountyCounselAgencyCheckbox[] checkboxes)
        {
            var personNameId = 0;
            if (person.PersonId == 0)
            {
                // Add person
                person.PersonId = UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonInsert_sp", new pd_PersonInsert_spParams
                {
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).FirstOrDefault();

                //add person name
                personNameId = UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonNameInsert_sp", new pd_PersonNameInsert_spParams
                {
                    PersonID = person.PersonId,
                    PersonNameFirst = person.FirstName,
                    PersonNameLast = person.LastName,
                    PersonNameTypeCodeID = 3200, //Need o confirm
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    PersonNameSalutationCodeID = -1,
                    PersonNameSuffixCodeID = -1,
                    BatchLogJobID = Guid.NewGuid()
                }).FirstOrDefault();

            }
            else
            {
                var personInfo = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", new pd_PersonGet_spParams
                {
                    PersonID = person.PersonId,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID

                }).FirstOrDefault();

                personNameId = personInfo.PersonNameID ?? 0;



                if (person.FirstName != person.PFirstName || person.LastName != person.PLastName)
                {
                    UtilityService.ExecStoredProcedureWithoutResults("pd_PersonNameUpdate_sp",
                        new pd_PersonNameUpdate_spParams()
                        {
                            PersonNameID = personNameId,
                            AgencyID = null,
                            PersonID = person.PersonId,
                            PersonNameFirst = person.FirstName,
                            PersonNameLast = person.LastName,
                            PersonNameMiddle = null,
                            PersonNameTypeCodeID = 3200, //Need o confirm
                            PersonNameSoundex = null,
                            RecordStateID = person.RecordStateID,
                            RecordTimeStamp = null,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                            PersonNameSalutationCodeID = -1,
                            PersonNameSuffixCodeID = -1
                        });
                }
            }


            if (person.BarNumber != person.PBarNumber)
            {
                UtilityService.ExecStoredProcedureWithoutResults("pd_LegalNumberInsert_sp",
                    new pd_LegalNumberInsert_spParams()
                    {
                        LegalNumberID = null,
                        AgencyID = null,
                        PersonID = person.PersonId,
                        LegalNumberTypeCodeID = 824, //need to change
                        LegalNumberEntry = person.BarNumber,
                        RecordStateID = person.RecordStateID,
                        RecordTimeStamp = null,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    });
            }

            foreach (var box in checkboxes)
            {
                if (box.Changed && box.Selected && box.RoleID == 0)
                {
                    UtilityService.ExecStoredProcedureWithoutResults("pd_UserRoleInsert_sp", new pd_UserRoleInsert_spParams()
                    {
                        RoleID = null,
                        PersonID = person.PersonId,
                        RoleTypeCodeID = 768, //need to change
                        RoleAgencyID = box.AgencyID,
                        RoleStartDate = DateTime.Now,
                        RoleEndDate = null,
                        RecordStateID = person.RecordStateID,
                        RecordTimeStamp = null,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    });
                }

                if (box.Changed && !box.Selected)
                {
                    UtilityService.ExecStoredProcedureWithoutResults("pd_RoleDelete_sp", new pd_RoleDelete_spParams()
                    {
                        //ID = box.AgencyID,
                        ID = box.RoleID,
                        RecordStateID = 10,
                        LoadOption = "Role",
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    });
                }

                if (!box.Changed)
                {
                    UtilityService.ExecStoredProcedureWithoutResults("pd_UserRoleUpdate_sp", new pd_UserRoleUpdate_spParams()
                    {
                        //RoleID = person.RoleID,
                        RoleID = box.RoleID,
                        RoleTypeCodeID = 768,//need to change
                        AgencyID = box.AgencyID,
                        RoleStartDate = person.StartDate,
                        RoleEndDate = person.EndDate,
                        RecordStateID = person.RecordStateID,
                        RecordTimeStamp = null,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    });
                }

            }
            return Json(new { isSuccess = true });
        }
    }
}