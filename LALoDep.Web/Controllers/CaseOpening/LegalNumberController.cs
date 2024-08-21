using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.Agency;
using LALoDep.Domain.CaseAttribute;
using LALoDep.Domain.pd_Association;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.CaseOpening;

namespace LALoDep.Controllers.CaseOpening
{
    public partial class CaseOpeningController : Controller
    {
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningLegalNumbers, IsCasePage = true, PageSecurityItemID = SecurityToken.ViewLegalNumber)]
        public virtual ActionResult LegalNumbers()
        {
            var model = new LegalNumberModel
            {
                RoleList =
                    UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDChildRespondent_spResult>(
                        "pd_RoleGetByCaseIDChildRespondent_sp",
                        new pd_RoleGetByCaseIDChildRespondent_spParams()
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,

                            BatchLogJobID = Guid.NewGuid()
                        }).Select(o => new LegalNumberRoleAddList() { PersonID = o.PersonID, PersonName = o.PersonNameFirst + " " + o.PersonNameLast, Role = o.Role, RoleClient = o.RoleClient }).ToList(),
                LegalNumberList = UtilityService.ExecStoredProcedureWithResults<pd_LegalNumberGetByCaseID_spResult>(
                    "pd_LegalNumberGetByCaseID_sp",
                    new pd_RoleGetByCaseIDForAssociation_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).ToList()

            };


            return View(model);
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningLegalNumbers, IsCasePage = true)]
        [HttpPost]
        public virtual JsonResult LegalNumberSave(LegalNumberModel model)
        {
            if (!UserManager.IsUserAccessTo(SecurityToken.AddLegalNumber))
            {
                return Json(new { Status = "Fail", URL = MVC.Home.Name + "/" + MVC.Home.ActionNames.AccessDenied });
            }


            if (model.RoleList.Any())
            {

                foreach (var role in model.RoleList)
                {



                    if (!string.IsNullOrEmpty(role.CCNumber))
                        InsertLegalNumber(role.CCNumber, 827, role.PersonID);

                    if (!string.IsNullOrEmpty(role.HHSANumber))
                        InsertLegalNumber(role.HHSANumber, 830, role.PersonID);

                    if (!string.IsNullOrEmpty(role.SSNumber))
                        InsertLegalNumber(role.SSNumber, 836, role.PersonID);

                   
                }
            }
            return Json(new { Status = "Done" });
        }

        private void InsertLegalNumber(string legalNumberEntry, int legalTypeId, int personId)
        {
           
                          UtilityService.ExecStoredProcedureForDataTable("pd_LegalNumberInsert_sp",
                              new pd_LegalNumberInsert_spParams()
                              {

                                  BatchLogJobID = Guid.NewGuid(),
                                  AgencyID = UserManager.UserExtended.AgencyID,
                                  UserID = UserManager.UserExtended.UserID,
                                  PersonID = personId,
                                  LegalNumberEntry = legalNumberEntry,
                                  LegalNumberTypeCodeID = legalTypeId,
                                  RecordStateID = 1,

                              }) ;
        }
    }
}