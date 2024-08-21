using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_CodeTables;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using LALoDep.Domain.Agency;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Address;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_JcatsGroup;
using LALoDep.Domain.pd_Conflict;
namespace LALoDep.Controllers.Administration
{
    public partial class CodeTablesController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;
        public CodeTablesController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CodeTable, PageSecurityItemID = SecurityToken.CodeTable)]
        public virtual ActionResult List()
        {
            return View(new CodeTableViewModel
            {
                OnViewLoad = true
            });
        }

        public virtual JsonResult GetCodeTableData()
        {
            var result = UtilityService
                .ExecStoredProcedureWithResults<pd_CodeTypeGetAll_spResult>(
                    "pd_CodeTypeGetAll_sp", new pd_CodeTypeGetAll_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = new Guid()
                    }).Select(x => new
                    {
                        CodeTypeID = x.CodeTypeID.ToEncrypt(),
                        CodeTypeValue = x.CodeTypeValue,
                        CodeTypeURL = x.CodeTypeURL
                    }).ToList();

            return Json(new DataTablesResponse(0, result, result.Count, result.Count), JsonRequestBehavior.AllowGet);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CodeTablesValue, PageSecurityItemID = SecurityToken.CodeTable)]
        public virtual ActionResult Values(string codeTypeID)
        {
            return View(new CodeTableViewModel
            {
                OnViewLoad = true,
                CodeTypeID = codeTypeID.ToDecrypt().ToInt()
            });
        }

        public virtual JsonResult GetCurrent(int codeTypeID)
        {
            var codes = UtilityService
                       .ExecStoredProcedureWithResults<pd_CodeGetByTypeIDForAdministration_spResult>(
                           "pd_CodeGetByTypeIDForAdministration_sp", new  pd_CodeGetByTypeIDForAdministration_spParams
                           {
                               CodeTypeID = codeTypeID,
                               UserID = UserManager.UserExtended.UserID,
                               LoadOption="Active"
                           }).Select(x => new
                           {
                               CodeID = x.CodeID.ToEncrypt(),
                               CodeValue = x.CodeValue,
                               CodeShortValue = x.CodeShortValue
                           }).ToList();

            var otherCodes = UtilityService
                             .ExecStoredProcedureWithResults<pd_CodeGetByTypeIDForAdministration_spResult>(
                           "pd_CodeGetByTypeIDForAdministration_sp", new pd_CodeGetByTypeIDForAdministration_spParams
                           {
                               CodeTypeID = codeTypeID,
                               UserID = UserManager.UserExtended.UserID,
                               LoadOption = "Inactive"
                           }).Select(x => new
                                {
                                    CodeID = x.CodeID.ToEncrypt(),
                                    CodeValue = x.CodeValue,
                                    CodeShortValue = x.CodeShortValue
                                }).ToList();

            return Json(new
            {
                current = new DataTablesResponse(0, codes, codes.Count, codes.Count),
                otherCodes = new DataTablesResponse(0, otherCodes, otherCodes.Count, otherCodes.Count)
            }, JsonRequestBehavior.AllowGet);
        }

        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.CodeTable)]
        public virtual ActionResult AddCode(string id)
        {
            var viewModel = new AddEditCodeViewModel();
            viewModel.CodeTypeID = id.ToDecrypt().ToInt();

            viewModel.CodeAgencies = UtilityService.ExecStoredProcedureWithResults<pd_AgencyCodeGetByCodeID_spResult>("pd_AgencyCodeGetByCodeID_sp",
                                            new pd_AgencyCodeGetByCodeID_spParams { UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() })
                                            .Select(x => new CodeAgency
                                            {
                                                AgencyID = x.AgencyID,
                                                AgencyName = x.AgencyName,
                                                Selected = x.Selected == 1,
                                                InAgency = x.InAgency == 1,
                                                AgencyCodeID = x.AgencyCodeID
                                            }).ToList();
            if (viewModel.CodeTypeID == 7)
            {
                viewModel.StateCode = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(5);
                viewModel.CountryCode = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(27);

            }
            viewModel.StateCodeID = 841;
            viewModel.CountryCodeID = 2246;
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult AddCode(AddEditCodeViewModel viewModel)
        {
            var codeId = UtilityService.ExecStoredProcedureScalar("pd_CodeInsert_sp",
                                              new pd_CodeInsert_spParams
                                              {
                                                  CodeValue = viewModel.CodeValue,
                                                  CodeShortValue = viewModel.CodeShortValue,
                                                  CodeTypeID = viewModel.CodeTypeID,
                                                  RecordStateID = 1,
                                                  UserID = UserManager.UserExtended.UserID,
                                                  BatchLogJobID = new Guid()
                                              }).ToInt();

            if (viewModel.CodeAgencies != null && viewModel.CodeAgencies.Count > 0)
            {
                foreach (var agency in viewModel.CodeAgencies)
                {
                    UtilityService.ExecStoredProcedureWithResults<object>("pd_AgencyCodeInsert_sp",
                                              new pd_AgencyCodeInsert_spParams
                                              {
                                                  AgencyID = agency.AgencyID,
                                                  CodeID = codeId,
                                                  AgencyCodeSystemUseOnly = 0,
                                                  AgencyCodeStartDate = DateTime.Now,
                                                  AgencyCodeDisplayOrder = 0,
                                                  RecordStateID = 1,
                                                  UserID = UserManager.UserExtended.UserID,
                                                  BatchLogJobID = new Guid()
                                              }).FirstOrDefault();
                }
            }
            if (viewModel.CodeTypeID == 7)
            {
                var daddressId =
             UtilityService.ExecStoredProcedureWithResults<decimal>("pd_AddressInsert_sp",
                     new pd_AddressInsert_spParams()
                     {
                         AddressCity = viewModel.City,
                         AddressCountryCodeID = viewModel.CountryCodeID,
                         AddressHomePhone = viewModel.HomePhone,
                         AddressStreet = viewModel.Street,
                         AddressStateCodeID = viewModel.StateCodeID,
                         AddressZipCode = viewModel.ZipCode,
                         BatchLogJobID = Guid.NewGuid(),
                         AgencyID = UserManager.UserExtended.AgencyID,
                         UserID = UserManager.UserExtended.UserID,
                         RecordStateID = 1,
                         RecordTimeStamp = null,

                     }).FirstOrDefault();
                var addressId = (int)daddressId;
                UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_PlacementAgencyAddressInsert_sp",
                             new pd_PlacementAgencyAddressInsert_spParams()
                             {
                                 AddressID = addressId,
                                 BatchLogJobID = Guid.NewGuid(),
                                 AgencyID = UserManager.UserExtended.AgencyID,
                                 UserID = UserManager.UserExtended.UserID,
                                 RecordStateID = 1,
                                 PlacementAgencyCodeID = codeId,
                             }).FirstOrDefault();
            }
            return Json(new { isSuccess = true });
        }


        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.EditCode, PageSecurityItemID = SecurityToken.CodeTable)]
        public virtual ActionResult EditCode(string id, string typeId)
        {
            var viewModel = new AddEditCodeViewModel();
            viewModel.CodeID = id.ToDecrypt().ToInt();
            viewModel.CodeTypeID = typeId.ToDecrypt().ToInt();

            var code = UtilityService.ExecStoredProcedureWithResults<pd_CodeGet_spResult>("pd_CodeGet_sp",
                                            new pd_CodeGet_spParams { CodeID = viewModel.CodeID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() })
                                            .FirstOrDefault();

            viewModel.CodeValue = code.CodeValue;
            viewModel.CodeShortValue = code.CodeShortValue;
            viewModel.RecordStateID = code.RecordStateID;

            viewModel.CodeAgencies = UtilityService.ExecStoredProcedureWithResults<pd_AgencyCodeGetByCodeID_spResult>("pd_AgencyCodeGetByCodeID_sp",
                                            new pd_AgencyCodeGetByCodeID_spParams { CodeID = viewModel.CodeID ?? 0, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() })
                                            .Select(x => new CodeAgency
                                            {
                                                AgencyID = x.AgencyID,
                                                AgencyName = x.AgencyName,
                                                Selected = x.Selected == 1,
                                                InAgency = x.InAgency == 1,
                                                AgencyCodeID = x.AgencyCodeID
                                            }).ToList();

            if (viewModel.CodeTypeID == 7)
            {
                var address = UtilityService.ExecStoredProcedureWithResults<pd_PlacementAddressGetByPlacementAgencyCodeID_spResult>("pd_PlacementAddressGetByPlacementAgencyCodeID_sp",
                                                  new pd_PlacementAddressGetByPlacementAgencyCodeID_spParams { PlacementAgencyCodeID = viewModel.CodeID.Value, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                                  .FirstOrDefault();
                if (address != null)
                {
                    viewModel.AddressID = address.AddressID;
                    viewModel.CountryCodeID = address.AddressCountryCodeID.HasValue ? address.AddressCountryCodeID.Value : 0;
                    viewModel.StateCodeID = address.AddressStateCodeID.HasValue ? address.AddressStateCodeID.Value : 0;
                    viewModel.Street = address.AddressStreet;
                    viewModel.City = address.AddressCity;
                    viewModel.ZipCode = address.AddressZipCode;
                    viewModel.HomePhone = address.AddressHomePhone;
                    var placementAddressGetPermission = UtilityService.ExecStoredProcedureWithResults<pd_PlacementAddressGetPermissions_spResult>("pd_PlacementAddressGetPermissions_sp",
                                              new pd_PlacementAddressGetPermissions_spParams { AddressID = address.AddressID, PlacementAgencyCodeID = viewModel.CodeID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                              .FirstOrDefault();
                    if (placementAddressGetPermission != null)
                    {
                        viewModel.DisplayBanner = placementAddressGetPermission.DisplayBanner;
                        if (placementAddressGetPermission.ReadOnlyCodeFlag.HasValue)
                            viewModel.ReadOnlyCodeFlag = placementAddressGetPermission.ReadOnlyCodeFlag.Value;
                        if (placementAddressGetPermission.ReadOnlyAddressFlag.HasValue)
                            viewModel.ReadOnlyAddressFlag = placementAddressGetPermission.ReadOnlyAddressFlag.Value;
                    }
                }


                viewModel.StateCode = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(5, includeCodeId: viewModel.StateCodeID);
                viewModel.CountryCode = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(27, includeCodeId: viewModel.CountryCodeID);

                viewModel.Notes = UtilityService.ExecStoredProcedureWithResults<pd_NoteGetForCodeID_spResult>("pd_NoteGetForCodeID_sp",
                                                new pd_NoteGetForCodeID_spParams { CodeID = viewModel.CodeID.Value, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                                .ToList();
                viewModel.NoteTypeList = UtilityFunctions.CodeGetBySystemValueTypeId(254);

                viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<pd_JcatsGroupAgencyGetByJcatsUserID_spResults>("pd_JcatsGroupAgencyGetByJcatsUserID_sp", new pd_JcatsGroupAgencyGetByJcatsUserID_spParams()
                {
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).Select(x => new SelectListItem()
                {
                    Text = x.AgencyDisplay,
                    Value = x.AgencyID.ToString()
                });
                viewModel.Notes.Insert(0, new pd_NoteGetForCodeID_spResult()
                {
                    NoteID = 0,

                });
            }

            return View(viewModel);
        }


        [HttpPost]
        public virtual ActionResult EditCode(AddEditCodeViewModel viewModel)
        {
            UtilityService.ExecStoredProcedureScalar("pd_CodeUpdate_sp",
                                              new pd_CodeUpdate_spParams
                                              {
                                                  CodeID = viewModel.CodeID.Value,
                                                  CodeValue = viewModel.CodeValue,
                                                  CodeShortValue = viewModel.CodeShortValue,
                                                  CodeTypeID = viewModel.CodeTypeID,
                                                  RecordStateID = viewModel.RecordStateID ?? 1,
                                                  UserID = UserManager.UserExtended.UserID,
                                                  BatchLogJobID = new Guid()
                                              }).ToInt();
            if (viewModel.CodeTypeID == 7)
            {
                if (viewModel.AddressID.HasValue)
                {
                    UtilityService.ExecStoredProcedureWithoutResults("pd_AddressUpdate_sp",
                     new pd_AddressUpdate_spParams()
                     {
                         AddressCity = viewModel.City,
                         AddressCountryCodeID = viewModel.CountryCodeID,
                         AddressHomePhone = viewModel.HomePhone,
                         AddressStreet = viewModel.Street,
                         AddressStateCodeID = viewModel.StateCodeID,
                         AddressZipCode = viewModel.ZipCode,
                         BatchLogJobID = Guid.NewGuid(),
                         AgencyID = UserManager.UserExtended.AgencyID,
                         UserID = UserManager.UserExtended.UserID,
                         RecordStateID = 1,
                         RecordTimeStamp = null,
                         AddressID = viewModel.AddressID.Value

                     });
                }
                if (viewModel.Notes.Any())
                {
                    foreach (var note in viewModel.Notes)
                    {
                        if (note.NoteID.HasValue && note.NoteID.Value == 0)
                        {
                            UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", new pd_NoteInsertForCode_spParams
                            {
                                NoteEntitySystemValueTypeID = 255,
                                NoteEntityTypeSystemValueTypeID = 123,
                                EntityPrimaryKeyID = viewModel.CodeID.Value,
                                NoteTypeCodeID = note.NoteTypeCodeID.Value,
                                NoteSubject = note.NoteSubject,
                                NoteEntry = note.NoteEntry,

                                RecordStateID = 1,
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid(),
                                AgencyID = note.AgencyID,

                            }).FirstOrDefault();
                        }
                        else if (note.NoteID.HasValue && note.NoteID.Value > 0)
                        {
                            if (note.RecordStateID.Value < 10)
                            {
                                UtilityService.ExecStoredProcedureWithoutResultADO("pd_NoteUpdate_sp", new pd_NoteUpdate_spParams
                                {
                                    NoteEntityCodeID = note.NoteEntityCodeID,
                                    NoteEntityTypeCodeID = note.NoteEntityTypeCodeID,
                                    NoteID = note.NoteID,
                                    EntityPrimaryKeyID = viewModel.CodeID.Value,
                                    NoteTypeCodeID = note.NoteTypeCodeID.Value,
                                    NoteSubject = note.NoteSubject,
                                    NoteEntry = note.NoteEntry,
                                    CaseID = note.CaseID,
                                    RecordStateID = note.RecordStateID,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid(),
                                    AgencyID = note.AgencyID,
                                    HearingID = 0,
                                    PetitionID = 0
                                });
                            }
                            else
                            {

                                UtilityService.ExecStoredProcedureWithoutResultADO("pd_NoteDelete_sp", new pd_NoteDelete_spParams()
                                {
                                    ID = note.NoteID.Value,
                                    RecordStateID = 10,
                                    UserID = UserManager.UserExtended.UserID,
                                    BatchLogJobID = Guid.NewGuid(),
                                    LoadOption = "Note",
                                });
                            }
                        }

                    }


                }
            }
            if (viewModel.CodeAgencies != null && viewModel.CodeAgencies.Count > 0)
            {
                foreach (var agency in viewModel.CodeAgencies)
                {
                    if (agency.Selected)
                    {
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_AgencyCodeInsert_sp",
                                                  new pd_AgencyCodeInsert_spParams
                                                  {
                                                      AgencyID = agency.AgencyID,
                                                      CodeID = viewModel.CodeID,
                                                      AgencyCodeSystemUseOnly = 0,
                                                      AgencyCodeStartDate = DateTime.Now,
                                                      AgencyCodeDisplayOrder = 0,
                                                      RecordStateID = 1,
                                                      UserID = UserManager.UserExtended.UserID,
                                                      BatchLogJobID = new Guid()
                                                  }).FirstOrDefault();
                    }
                    else
                    {
                        if (agency.AgencyCodeID.HasValue)
                        {
                            UtilityService.ExecStoredProcedureWithResults<object>("pd_AgencyCodeDelete_sp",
                                                      new pd_AgencyCodeDelete_spParams
                                                      {
                                                          ID = agency.AgencyCodeID.Value,
                                                          UserID = UserManager.UserExtended.UserID,
                                                          BatchLogJobID = new Guid()
                                                      }).FirstOrDefault();
                        }
                    }
                }
            }

            return Json(new { isSuccess = true });
        }

    }
}