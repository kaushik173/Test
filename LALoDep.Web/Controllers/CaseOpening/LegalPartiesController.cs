using System;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Domain.pd_Address;
using LALoDep.Domain.pd_Association;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Person;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.CaseOpening;
using LALoDep.Domain.pd_CopyCase;

namespace LALoDep.Controllers.CaseOpening
{
    public partial class CaseOpeningController : Controller
    {
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningLegalParties, IsCasePage = true)]
        public virtual ActionResult LegalParties()
        {
            var model = new LegalPartiesModel
            {
                AddressTypeList = UtilityFunctions.CodeGetBySystemValueTypeId(106, agencyId: UserManager.UserExtended.CaseNumberAgencyID),
                //StateList = UtilityFunctions.CodeGetByTypeIdAndUserId(5),

                StateList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(5, agencyId: UserManager.UserExtended.CaseNumberAgencyID),
                AssociationForNewRoleAssociationTypeList =
                    UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserIDSortShortValue_spResults>(
                        "pd_CodeGetByTypeIDAndUserIDSortShortValue_sp",
                        new pd_CodeGetByTypeIDAndUserIDSortShortValue_spParams()
                        {
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                            CodeTypeID = 24
                        }).Select(o => new SelectListItem()
                        {
                            Text = o.CodeShortValue + " (" + o.CodeValue + ")",
                            Value = o.CodeID.ToString()
                        }).ToList(),
                NewRoleList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetLegalFreeFormGeneral_spResults>(
                    "pd_RoleGetLegalFreeFormGeneral_sp",
                    new pd_RoleGetLegalFreeFormGeneral_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()


                    }).Select(o => new SelectListItem()
                    {
                        Text = o.LegalType,
                        Value = o.LegalTypeID.ToString()
                    }).ToList(),
                LegalPartiesList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetLegalAll_spResults>(
                    "pd_RoleGetLegalAll_sp",
                    new pd_RoleGetLegalAll_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID,
                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID

                    }).ToList(),
                AssociationRelatedToList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDForAssociationRelatedTo_spResults>(
                    "pd_RoleGetByCaseIDForAssociationRelatedTo_sp",
                    new pd_RoleGetByCaseIDForAssociationRelatedTo_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID,
                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID

                    }).ToList(),
                AssociationAttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDForAssociationAttorney_spResults>(
                    "pd_RoleGetByCaseIDForAssociationAttorney_sp",
                    new pd_RoleGetByCaseIDForAssociationAttorney_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID,
                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID

                    }).ToList(),
                RoleGetLegalSpecificList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetLegalSpecific_spResult>(
                    "pd_RoleGetLegalSpecific_sp",
                    new pd_RoleGetLegalSpecific_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID


                    }).ToList(),
                RoleLegalStatusList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetLegalStatus_spResult>(
                    "pd_RoleGetLegalStatus_sp",
                    new pd_RoleGetLegalStatus_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID,
                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID


                    }).ToList()
            };
            model.AddrStartDate = model.RoleStartDate = UserManager.UserExtended.ApptDate;
            ViewBag.caseId = UserManager.UserExtended.CaseID.ToEncrypt();
            model.AllowTransfer = false;
            var oCopyCaseTransferSubsetCheck = UtilityService.ExecStoredProcedureWithResults<pd_CopyCaseTransferSubsetCheck_spResult>(
                       "pd_CopyCaseTransferSubsetCheck_sp",
                       new pd_CopyCaseTransferSubsetCheck_spParams()
                       {
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid(),
                           CaseID = UserManager.UserExtended.CaseID,
                           AgencyID = UserManager.UserExtended.CaseNumberAgencyID


                       }).FirstOrDefault();
            if (oCopyCaseTransferSubsetCheck != null)
            {
                model.AllowTransfer = oCopyCaseTransferSubsetCheck.AllowTranfer == 1;
            }
            return View(model);
        }


        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningLegalParties, IsCasePage = true)]
        public virtual JsonResult LegalPartiesSave(LegalPartiesModel model)
        {
            if (model.LegalPartiesSelectedRoleList.Any() && UserManager.IsUserAccessTo(SecurityToken.AddRole))
            {
                foreach (var item in model.LegalPartiesSelectedRoleList)
                {

                    var roleId = UtilityService.ExecStoredProcedureWithResults<int>("pd_RoleInsert_sp", new pd_RoleInsert_spParams
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        BatchLogJobID = Guid.NewGuid(),
                        PersonID = item.PersonID,
                        RecordStateID = 1,
                        RoleClient = 0,
                        RoleStartDate = item.StartDate,
                        RoleTypeCodeID = item.RoleTypeCodeID,
                        UserID = UserManager.UserExtended.UserID
                    }).FirstOrDefault();


                    if (!string.IsNullOrEmpty(item.SelectedAttorneyAssociate))
                    {
                        var associateIds = item.SelectedAttorneyAssociate.Split(',');
                        foreach (var associateId in associateIds)
                        {

                            UtilityService.ExecStoredProcedureWithResults<decimal>("pd_AssociationInsert_sp", new pd_AssociationInsert_spParams
                            {
                                AssociationCodeID = 2358,
                                CaseID = UserManager.UserExtended.CaseID,
                                AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                BatchLogJobID = Guid.NewGuid(),
                                PersonID = item.PersonID,
                                RecordStateID = 1,
                                RelatedPersonID = associateId.ToInt(),
                                AssociationStartDate = item.StartDate,

                                UserID = UserManager.UserExtended.UserID

                            }).FirstOrDefault();


                        }
                    }

                }

            }
            if (model.NewRoleID.HasValue && model.NewRoleID.Value > 0 && UserManager.IsUserAccessTo(SecurityToken.AddRole))
            {

                var personId = UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonInsert_sp", new pd_PersonInsert_spParams()
                {
                    AgencyID = UserManager.UserExtended.AgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,

                    RecordStateID = 1,

                }).FirstOrDefault();

                var personNameId = UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonNameInsert_sp", new pd_PersonNameInsert_spParams()
                {
                    AgencyID = UserManager.UserExtended.AgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    RecordStateID = 1,
                    PersonID = personId,
                    PersonNameFirst = model.FirstName,
                    PersonNameLast = model.LastName,
                    PersonNameTypeCodeID = 1,

                }).FirstOrDefault();

                var roleId = UtilityService.ExecStoredProcedureWithResults<int>("pd_RoleInsert_sp", new pd_RoleInsert_spParams()
                {
                    AgencyID = UserManager.UserExtended.AgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    RecordStateID = 1,
                    PersonID = personId,
                    CaseID = UserManager.UserExtended.CaseID,
                    RoleClient = 0,
                    RoleTypeCodeID = model.NewRoleID.Value,
                    RoleStartDate = !string.IsNullOrEmpty(model.RoleStartDate) ? model.RoleStartDate.ToDateTime() : DateTime.Now,


                }).FirstOrDefault();


                if (!string.IsNullOrEmpty(model.Street))
                {
                    var addressId =
               UtilityService.ExecStoredProcedureWithResults<decimal>("pd_AddressInsert_sp",
                       new pd_AddressInsert_spParams()
                       {
                           AddressCity = model.City,
                           AddressHomePhone = model.AddressPhone,
                           AddressStreet = model.Street,
                           AddressStateCodeID = model.StateID,
                           AddressZipCode = model.ZipCode,
                           BatchLogJobID = Guid.NewGuid(),
                           AgencyID = UserManager.UserExtended.AgencyID,
                           UserID = UserManager.UserExtended.UserID,
                           RecordStateID = 1,
                           RecordTimeStamp = null,

                       }).FirstOrDefault();


                    var personAddressId =
                              UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonAddressInsert_sp",
                                  new pd_PersonAddressInsert_spParams()
                                  {
                                      AddressID = (int)addressId,
                                      PersonAddressTypeCodeID = model.AddressTypeID,
                                      PersonAddressStartDate = string.IsNullOrEmpty(model.AddrStartDate) ? model.RoleStartDate.ToDateTime() : model.AddrStartDate.ToDateTime(),
                                      BatchLogJobID = Guid.NewGuid(),
                                      AgencyID = UserManager.UserExtended.AgencyID,
                                      UserID = UserManager.UserExtended.UserID,
                                      PersonID = personId,
                                      PersonAddressWorkPhone = model.AddressPhone,
                                      PersonAddressConfidential = 0,

                                      RecordStateID = 1,

                                  }).FirstOrDefault();


                    if (!string.IsNullOrEmpty(model.AssociationForNewRoleSelectedIds))
                    {
                        var personAddressTypeCodeId = model.AddressTypeID;

                        if (model.AssociationForNewRoleAssociationTypeID > 0)
                        {
                            var parentCode = UtilityService.ExecStoredProcedureWithResults<CodeHierarchyGetByCodeRelationshipIDAgencyID_spResults>("CodeHierarchyGetByCodeRelationshipIDAgencyID_sp", new CodeHierarchyGetByCodeRelationshipIDAgencyID_spParams()
                            {
                                BatchLogJobID = Guid.NewGuid(),

                                UserID = UserManager.UserExtended.UserID,
                                CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                CodeRelationshipID = 7

                            }).FirstOrDefault(o => o.ParentCodeID.Value == model.AssociationForNewRoleAssociationTypeID);
                            if (parentCode != null)
                            {
                                personAddressTypeCodeId = parentCode.ChildCodeID.Value;
                            }
                        }
                        var associateIds = model.AssociationForNewRoleSelectedIds.Split(',');
                        foreach (var associateId in associateIds)
                        {
                            var associatePersonId = associateId.Split('|')[0].ToInt();
                            if (associateId.Split('|')[1].ToInt() == 1)
                            {
                                UtilityService.ExecStoredProcedureScalar("pd_PersonAddressInsert_sp", new pd_PersonAddressInsert_spParams()
                                {
                                    AddressID = (int)addressId,
                                    PersonAddressTypeCodeID = personAddressTypeCodeId,
                                    PersonAddressStartDate = string.IsNullOrEmpty(model.AddrStartDate) ? model.RoleStartDate.ToDateTime() : model.AddrStartDate.ToDateTime(),
                                    BatchLogJobID = Guid.NewGuid(),
                                    AgencyID = UserManager.UserExtended.AgencyID,
                                    UserID = UserManager.UserExtended.UserID,
                                    PersonID = associatePersonId,
                                    PersonAddressWorkPhone = model.AddressPhone,
                                    PersonAddressConfidential = 0,

                                    RecordStateID = 1,

                                }).ToInt();
                            }



                        }
                    }

                }
                if (!string.IsNullOrEmpty(model.WorkPhone))
                {
                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_PersonContactInsert_sp",
                           new pd_PersonContactInsert_spParams()
                           {
                               BatchLogJobID = Guid.NewGuid(),
                               AgencyID = UserManager.UserExtended.AgencyID,
                               UserID = UserManager.UserExtended.UserID,
                               PersonID = personId,
                               PersonContactInfo = model.WorkPhone,
                               PersonContactTypeCodeID = -1,
                               ContactTypeEnumName = "WorkPhone",
                               RecordStateID = 1,

                           }).FirstOrDefault();
                }
                if (!string.IsNullOrEmpty(model.MobilePhone))
                {
                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_PersonContactInsert_sp",
                                                            new pd_PersonContactInsert_spParams()
                                                            {
                                                                BatchLogJobID = Guid.NewGuid(),
                                                                AgencyID = UserManager.UserExtended.AgencyID,
                                                                UserID = UserManager.UserExtended.UserID,
                                                                PersonID = personId,
                                                                PersonContactInfo = model.MobilePhone,
                                                                PersonContactTypeCodeID = -1,
                                                                ContactTypeEnumName = "MobilePhone",
                                                                RecordStateID = 1,

                                                            }).FirstOrDefault();

                }
                if (!string.IsNullOrEmpty(model.EmailAddress))
                {
                    UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_PersonContactInsert_sp",
                                          new pd_PersonContactInsert_spParams()
                                          {
                                              BatchLogJobID = Guid.NewGuid(),
                                              AgencyID = UserManager.UserExtended.AgencyID,
                                              UserID = UserManager.UserExtended.UserID,
                                              PersonID = personId,
                                              PersonContactInfo = model.EmailAddress,
                                              PersonContactTypeCodeID = -1,
                                              ContactTypeEnumName = "EmailPrimary",
                                              RecordStateID = 1,

                                          }).FirstOrDefault();
                }




                if (!string.IsNullOrEmpty(model.AssociationForNewRoleSelectedIds))
                {
                    var associateIds = model.AssociationForNewRoleSelectedIds.Split(',');
                    foreach (var associateId in associateIds)
                    {
                        var associatePersonId = associateId.Split('|')[0].ToInt();
                        UtilityService.ExecStoredProcedureWithResults<decimal>("pd_AssociationInsert_sp", new pd_AssociationInsert_spParams
                        {
                            AssociationCodeID = model.AssociationForNewRoleAssociationTypeID,
                            CaseID = UserManager.UserExtended.CaseID,
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                            BatchLogJobID = Guid.NewGuid(),
                            PersonID = personId,
                            RecordStateID = 1,
                            RelatedPersonID = associatePersonId,
                            AssociationStartDate = model.RoleStartDate.ToDateTime(),

                            UserID = UserManager.UserExtended.UserID

                        }).FirstOrDefault();


                    }
                }

            }
            if (model.LegalPartiesList.Any() && UserManager.IsUserAccessTo(SecurityToken.EditRole))
            {
                foreach (var item in model.LegalPartiesList)
                {

                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_RoleUpdate_sp", new pd_RoleUpdate_spParams
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        BatchLogJobID = Guid.NewGuid(),
                        PersonID = item.PersonID.Value,
                        RecordStateID = 1,
                        RoleClient = 0,
                        RoleStartDate = item.RoleStartDate,
                        RoleEndDate = item.RoleEndDate.HasValue ? item.RoleEndDate.Value : (DateTime?)null,
                        RoleID = item.RoleID.Value,
                        RoleTypeCodeID = item.RoleTypeCodeID.Value,
                        UserID = UserManager.UserExtended.UserID


                    });
                }
            }
            return Json(new { Status = "Done", Data = model });
        }



    }
}