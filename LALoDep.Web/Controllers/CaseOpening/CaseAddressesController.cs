using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using LALoDep.Domain.com_Report;
using LALoDep.Domain.pd_Address;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.ErrorLog;
using LALoDep.Custom.Security;
using LALoDep.Models.CaseOpening;
using LALoDep.Domain.pd_Code;

namespace LALoDep.Controllers.CaseOpening
{
    public partial class CaseOpeningController : Controller
    {


        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningAddressPlacement, PageSecurityItemID = SecurityToken.ViewAddress)]
        public virtual ActionResult CaseAddresses(string addressId)
        {

            return View(GetAddressModel(addressId));
        }

        public CaseAddressesViewModel GetAddressModel(string addressId)
        {
            int? AddressId = null;
            if (!string.IsNullOrEmpty(addressId))
            {
                AddressId = addressId.ToDecrypt().ToInt();
            }


            var model = new CaseAddressesViewModel();

            if (!AddressId.HasValue)
            {
                model.PeopleInCase =
                    UtilityService.ExecStoredProcedureWithResults<pd_PersonAddressGetAllRolesByCaseID_spResult>(
                        "pd_PersonAddressGetAllRolesByCaseID_sp", new pd_PersonAddressGetAllRolesByCaseID_spParams
                        {
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()

                        }).ToList();

                model.StateCodeID = 841;
                model.CountryCodeID = 2246;
                var addressess = UtilityService
                        .ExecStoredProcedureWithResults
                        <pd_AddressGetByCaseID_spResult>(
                            "pd_AddressGetByCaseID_sp",
                            new pd_AddressGetByCaseID_spParams
                            {
                                CaseID = UserManager.UserExtended.CaseID,
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid()

                            }).ToList();
                model.AddressStreet = String.Join(",",
                     addressess.Select(o => o.AddressStreet)
                        .ToList()
                    );

                model.AddressHomePhone = String.Join(",",
                    addressess
                       .Select(o => (o.AddressHomePhone.IsNullOrEmpty() ? "" : o.AddressHomePhone.Replace("[Can Text]", "")))
                       .ToList()
                   );

                model.ExistingAddresses =
                    addressess
                        .Select(
                            o =>
                                new SelectListItem
                                {
                                    Text =
                                        o.AddressStreet + @"," + o.AddressCity + @"," + o.State +
                                        @"," + o.AddressZipCode + @"," + o.AddressHomePhone,
                                    Value = o.AddressID.ToString(CultureInfo.InvariantCulture)
                                })
                        .ToList();
                var placementAgencyList = UtilityService
                        .ExecStoredProcedureWithResults
                        <pd_AddressGetPlacementCodesByCaseID_spResult>(
                            "pd_AddressGetPlacementCodesByCaseID_sp",
                            new pd_AddressGetPlacementCodesByCaseID_spParams
                            {
                                CaseID = UserManager.UserExtended.CaseID,
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid()

                            });
                model.PlacementAgencyAddressList =                        placementAgencyList.Select(
                            o =>
                                new SelectListItem
                                {
                                    Text =
                                        o.PlacementAgency + @" - " + o.AddressStreet + @"," + o.AddressCity + @"," + o.State +
                                        @" " + o.AddressZipCode + @"," + o.AddressHomePhone,
                                    Value = o.AddressID.HasValue ? o.AddressID.Value.ToString() +"|"+ o.PlacementAgencyCodeID.Value.ToString() : "0"
                                })
                        .ToList();
                //model.TypeCode = UtilityFunctions.CodeGetByTypeIdAndUserId(6);
                model.PlacementAgencyAddress = String.Join(",",
                 placementAgencyList.Select(o => o.AddressStreet)
                    .ToList()
                );

                var addressTypes = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>(
                "pd_CodeGetByTypeIDAndUserID_sp", new pd_CodeGetByTypeIDAndUserID_spParams
                {
                    CodeTypeID = 6,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID
                }).ToList();

                model.TypeCode = addressTypes.Where(o => o.ChildAddressType == "Y").Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() }).ToList();
                model.NonChildAddressType = addressTypes.Where(o => o.NonChildAddressType == "Y").Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() }).ToList();

            }
            else
            {
                model.AddressID = AddressId.Value;

                var address = UtilityService
                    .ExecStoredProcedureWithResults<pd_AddressGet_spResult>(
                        "pd_AddressGet_sp",
                        new pd_AddressGet_spParams
                        {
                            AddressID = AddressId.Value,
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID
                        }).FirstOrDefault();

                if (address != null)
                {
                    model.City = address.AddressCity;
                    model.Street = address.AddressStreet;
                    model.ZipCode = address.AddressZipCode;
                    model.StateCodeID = address.AddressStateCodeID.HasValue ? address.AddressStateCodeID.Value : 0;
                    model.CountryCodeID = address.AddressCountryCodeID.HasValue ? address.AddressCountryCodeID.Value : 0;
                    model.HomePhone = address.AddressHomePhone;
                    model.RecordStateID = address.RecordStateID;
                    model.CanText = address.CanTextFlag == 1;
                }
            }

            model.StateCode = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(5, includeCodeId: model.StateCodeID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);
            model.CountryCode = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(27, includeCodeId: model.CountryCodeID, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

            return model;
        }
        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningAddressPlacement)]

        public virtual JsonResult CaseAddresses(CaseAddressesViewModel model, FormCollection collection)
        {
            model.CanEdit = UserManager.IsUserAccessTo(SecurityToken.EditAddress);
            model.CanAdd = UserManager.IsUserAccessTo(SecurityToken.AddAddress);
            if ((!model.CanAdd && !model.CanEdit))
            {
                return Json(new { Status = "Fail", URL = MVC.Home.Name + "/" + MVC.Home.ActionNames.AccessDenied });
            }
            var addressId = 0;

            #region Get Address ID

            if (model.ExistingAddressID > 0 && model.CanAdd)
            {
                addressId = model.ExistingAddressID;
            }
            else if (model.PlacementAgencyAddressID > 0 && model.CanAdd)
            {
                addressId = model.PlacementAgencyAddressID;
            }
            else if (model.CanAdd && (!string.IsNullOrEmpty(model.City) || !string.IsNullOrEmpty(model.HomePhone)))
            {
                var daddressId =
                 UtilityService.ExecStoredProcedureWithResults<decimal>("pd_AddressInsert_sp",
                         new pd_AddressInsert_spParams()
                         {
                             AddressCity = model.City,
                             AddressCountryCodeID = model.CountryID,
                             AddressHomePhone = model.HomePhone,
                             AddressStreet = model.Street,
                             AddressStateCodeID = model.StateID,
                             AddressZipCode = model.ZipCode,
                             BatchLogJobID = Guid.NewGuid(),
                             AgencyID = UserManager.UserExtended.AgencyID,
                             UserID = UserManager.UserExtended.UserID,
                             RecordStateID = 1,
                             CanTextFlag = (byte)(model.CanText ? 1 : 0),
                             RecordTimeStamp = null,

                         }).FirstOrDefault();
                addressId = (int)daddressId;

            }

            #endregion

            #region Insert Person Address
            for (var i = 0; i < model.PersonCount; i++)
            {
                var attributeId = collection["PersonID" + i].ToInt();
                var typeId = collection["PersonAddressTypeCodeID" + i].ToInt();
                var startDate = collection["StartDate" + i];
                var endDate = collection["EndDate" + i];
                var recordStateID = collection["RecordStateID" + i].ToInt();
                if (addressId > 0)
                {


                    if (typeId > 0 && !string.IsNullOrEmpty(startDate) && model.CanAdd)
                    {
                        var personAddressId =
                            UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonAddressInsert_sp",
                                new pd_PersonAddressInsert_spParams()
                                {
                                    AddressID = addressId,
                                    PersonAddressEndDate = (!string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate) : (DateTime?)null),
                                    PersonAddressStartDate = DateTime.Parse(startDate),

                                    PersonAddressTypeCodeID = typeId,
                                    BatchLogJobID = Guid.NewGuid(),
                                    AgencyID = UserManager.UserExtended.AgencyID,
                                    UserID = UserManager.UserExtended.UserID,
                                    PersonID = attributeId,
                                    PersonAddressWorkPhone = "",
                                    PersonAddressConfidential = 0,

                                    RecordStateID = 1,

                                }).FirstOrDefault();
                    }




                }



            }
            #endregion

            #region Update Addresses

            for (var i = 0; i < model.PersonCount; i++)
            {
                var attributeId = collection["PersonAttributeID" + i].ToInt();
                var typeId = collection["PersonAddressTypeCodeID" + i].ToInt();
                var startDate = collection["StartDate" + i];
                var endDate = collection["EndDate" + i];
                var recordStateID = collection["RecordStateID" + i].ToInt();

                var addressCount = collection["AddressCount" + i].ToInt();
                for (var x = 0; x < addressCount; x++)
                {
                    var personAddressId = collection["PersonAddressID" + i + "-" + x].ToInt();
                    typeId = collection["PersonAddressTypeCodeID" + i + "-" + x].ToInt();
                    var _addressId = collection["AddressID" + i + "-" + x].ToInt();
                    startDate = collection["StartDate" + i + "-" + x];
                    endDate = collection["EndDate" + i + "-" + x];
                    recordStateID = collection["RecordStateID" + i + "-" + x].ToInt();
                    var oldStartDate = collection["StartDate" + i + "-" + x + "_oldValue"];
                    var oldEndDate = collection["EndDate" + i + "-" + x + "_oldValue"];
                    //typeId > 0 && 
                    if (!string.IsNullOrEmpty(startDate) && model.CanEdit && (oldStartDate != startDate || oldEndDate != endDate))
                    {
                        UtilityService.ExecStoredProcedureWithoutResultADO("pd_PersonAddressUpdate_sp",
                            new pd_PersonAddressUpdate_spParams()
                            {
                                PersonAddressID = personAddressId,
                                AddressID = _addressId,
                                PersonAddressEndDate =
                                    (!string.IsNullOrEmpty(endDate) ? DateTime.Parse(endDate) : (DateTime?)null),
                                PersonAddressStartDate = DateTime.Parse(startDate),
                                PersonAddressTypeCodeID = typeId,
                                BatchLogJobID = Guid.NewGuid(),
                                AgencyID = UserManager.UserExtended.AgencyID,
                                UserID = UserManager.UserExtended.UserID,
                                PersonID = attributeId,
                                PersonAddressConfidential = 0,

                                RecordStateID = recordStateID,
                                RecordTimeStamp = null,

                            });
                    }
                }
            }

            #endregion

            return Json(new { Status = "Done" });
        }
        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningAddressPlacement)]

        public virtual JsonResult CaseAddressEdit(CaseAddressesViewModel model, FormCollection collection)
        {
            if (!UserManager.IsUserAccessTo(SecurityToken.EditAddress))
            {
                return Json(new { Status = "Fail", URL = MVC.Home.Name + "/" + MVC.Home.ActionNames.AccessDenied });
            }

            if (!string.IsNullOrEmpty(model.Street) && !string.IsNullOrEmpty(model.City) && !string.IsNullOrEmpty(model.ZipCode) && model.StateCodeID > 0)
            {

                var address = UtilityService
                       .ExecStoredProcedureWithResults<pd_AddressGet_spResult>(
                           "pd_AddressGet_sp",
                           new pd_AddressGet_spParams
                           {
                               AddressID = model.AddressID,
                               BatchLogJobID = Guid.NewGuid(),
                               UserID = UserManager.UserExtended.UserID
                           }).ToList().FirstOrDefault();

                if (address != null)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("pd_AddressUpdate_sp",
                     new pd_AddressUpdate_spParams()
                     {
                         AddressCity = model.City,
                         AddressCountryCodeID = model.CountryID,
                         AddressHomePhone = model.HomePhone,
                         AddressStreet = model.Street,
                         AddressStateCodeID = model.StateCodeID,
                         AddressZipCode = model.ZipCode,
                         BatchLogJobID = Guid.NewGuid(),
                         AgencyID = address.AgencyID,
                         UserID = UserManager.UserExtended.UserID,
                         RecordStateID = address.RecordStateID,
                         RecordTimeStamp = null,
                         CanTextFlag = (byte)(model.CanText ? 1 : 0),
                         AddressID = model.AddressID

                     });

                }
            }

            return Json(new { Status = "Done" });
        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningAddressPlacement)]

        public virtual JsonResult CaseAddressDelete(int id)
        {


            if (!UserManager.IsUserAccessTo(SecurityToken.DeleteAddress))
            {
                return Json(new { Status = "Fail", URL = MVC.Home.Name + "/" + MVC.Home.ActionNames.AccessDenied });
            }
            UtilityService.ExecStoredProcedureWithoutResultADO("pd_PersonAddressDelete_sp", new pd_PersonAddressDelete_spParams
            {
                BatchLogJobID = Guid.NewGuid(),
                ID = id,
                RecordTimeStamp = null,
                UserID = UserManager.UserExtended.UserID
            });
            return Json(new { Status = "Done" });
        }


        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningAddressPlacement)]

        [HttpPost]
        public virtual ActionResult CaseAddressesPrint(CaseAddressesViewModel model)
        {

            /*Insert New Report Parameters saved for this User*/





            var rpt = new ReportClass
            {
                FileName = HttpContext.Server.MapPath("~/Reports/rptAddressPrintableVersion.rpt")
            };
            try
            {
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTableADO("rpt_AddressPrintableVersion_sp",
                     new Dictionary<string, object>()
                {
                {"@caseid", UserManager.UserExtended.CaseID},
                {"@userid", UserManager.UserExtended.UserID},
                {"@batchlogjobid", Guid.NewGuid()}
                });
                rpt.SetDataSource(table);


                var filename = Guid.NewGuid() + ".pdf";

                //if file already exists then delete it
                if (System.IO.File.Exists(Server.MapPath("~") + "MergeTemplate\\Download\\" + filename))
                    System.IO.File.Delete(Server.MapPath("~") + "MergeTemplate\\Download\\" + filename);

                if (UserManager.UserExtended.PrintDocumentOn == "NewWindow")
                {


                    var filePath = UtilityFunctions.GetDocumentDownloadFolderPath() + filename;
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, filePath);



                    rpt.Close();
                    rpt.Dispose();
                    GC.Collect();

                    return RedirectToAction("Preview", "Home", new { path = Utility.Encrypt(UtilityFunctions.GetDocumentDownloadFolderRelativePath() + filename) });
                }
                Stream stream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
                return File(stream, "application/pdf", filename);
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
            return Content("Report not generating");

        }



    }
}