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
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningAssociates, IsCasePage = true, PageSecurityItemID = SecurityToken.ViewAssociation)]
        public virtual ActionResult Associations()
        {
            var model = new AssociationsModel
            {
                AssociationTypeList = UtilityFunctions.CodeGetByTypeIDAndUserIDSortShortValue(24, combineLongValue: true),
                RelatedPersonList =
                    UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDForAssociationRelatedTo_spResults>(
                        "pd_RoleGetByCaseIDForAssociationRelatedTo_sp",
                        new pd_RoleGetByCaseIDForAssociationRelatedTo_spParams()
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                            BatchLogJobID = Guid.NewGuid()
                        }).ToList(),
                PersonList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDForAssociation_spResult>(
                    "pd_RoleGetByCaseIDForAssociation_sp",
                    new pd_RoleGetByCaseIDForAssociation_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    })
                    .Select(
                        o =>
                            new SelectListItem()
                            {
                                Value = o.PersonID.ToString(),
                                Text = o.PersonNameFirst + " " + o.PersonNameLast + " (" + o.Role + ")"
                            }).ToList(),
                AssociationsInCase =
        UtilityService.ExecStoredProcedureWithResults<pd_AssociationGetByCaseID_spResult>(
            "pd_AssociationGetByCaseID_sp",
            new pd_AssociationGetByCaseID_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).ToList()

            };
            var suggestions =
                 UtilityService.ExecStoredProcedureWithResults<pd_AssociationSuggestByCaseID_spResult>(
                     "pd_AssociationSuggestByCaseID_sp",
                     new pd_AssociationSuggestByCaseID_spParams()
                     {
                         CaseID = UserManager.UserExtended.CaseID,
                         UserID = UserManager.UserExtended.UserID,
                         BatchLogJobID = Guid.NewGuid()
                     }).ToList();
            var personIds = model.AssociationsInCase.Select(o => o.PersonID).ToList();
            var associateSuggestions = suggestions.Where(item => !personIds.Contains(item.PersonID1.Value)).ToList();
            model.AssociationSuggestions = associateSuggestions;
            return View(model);
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningAssociates, IsCasePage = true)]
        [HttpPost]
        public virtual JsonResult AssociationSave(AssociationsModel model)
        {
            if (!UserManager.IsUserAccessTo(SecurityToken.AddAssociation))
            {
                return Json(new { Status = "Fail", URL = MVC.Home.Name + "/" + MVC.Home.ActionNames.AccessDenied });
            }

            if (model.PersonID > 0 && model.AssociationTypeID > 0 && model.RelatedPersonList.Any())
            {
                foreach (var id in model.RelatedPersonList.Select(relatedPerson => UtilityService.ExecStoredProcedureWithResults<decimal>("pd_AssociationInsert_sp", new pd_AssociationInsert_spParams()
                {
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                    CaseID = UserManager.UserExtended.CaseID,
                    PersonID = model.PersonID.Value,
                    RecordStateID = 1,
                    RelatedPersonID = relatedPerson.PersonID,
                    AssociationCodeID = model.AssociationTypeID.Value,
                    AssociationStartDate = relatedPerson.RoleStartDate.Value,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault()))
                {
                }
            }

            if (model.SelectedAssociationSuggestions.Any())
            {
                foreach (var item in model.SelectedAssociationSuggestions)
                {
                    UtilityService.ExecStoredProcedureScalar("pd_AssociationInsert_sp",
                        new pd_AssociationInsert_spParams()
                        {
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                            CaseID = UserManager.UserExtended.CaseID,
                            PersonID = item.PersonID1,
                            RecordStateID = 1,
                            RelatedPersonID = item.PersonID2,
                            AssociationCodeID = item.AssociationTypeID,
                            AssociationStartDate = item.StartDate.ToDateTime(),
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID
                        });
                }
            }

            if (model.AssociationsInCase.Any())
            {
                foreach (var associate in model.AssociationsInCase)
                {
                    //update associate
                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_AssociationUpdateStartDateEndDate_sp",
                        new pd_AssociationUpdateStartDateEndDate_spParams()
                        {
                            AssociationID = associate.AssociationID,
                            StartDate = associate.AssociationStartDate.Value,
                            EndDate = associate.AssociationEndDate,
                            RecordStateID = 1,
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID
                        });
                }
            }

            return Json(new { Status = "Done" });
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningAssociates, IsCasePage = true)]
        [HttpPost]

        public virtual ActionResult AssociationDelete(int id)
        {
            if (!UserManager.IsUserAccessTo(SecurityToken.DeleteAssociation))
            {
                return Json(new { Status = "Fail", URL = MVC.Home.Name + "/" + MVC.Home.ActionNames.AccessDenied });
            }



            UtilityService.ExecStoredProcedureWithoutResultADO("pd_AssociationDelete_sp", new pd_AssociationDelete_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                ID = id,

                RecordTimeStamp = null
            });



            return Json(new { Status = "Done" });
        }


    }
}