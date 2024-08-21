using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.Agency;
using LALoDep.Domain.CaseAttribute;
using LALoDep.Domain.pd_Allegation;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_Petition;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.Services;
using LALoDep.Domain.sup_Case;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.CaseOpening;
using LALoDep.Models;

namespace LALoDep.Controllers.CaseOpening
{
    public partial class CaseOpeningController : Controller
    {
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningPetitionPage, IsCasePage = true, PageSecurityItemID = SecurityToken.ViewPetition)]
        public virtual ActionResult PetitionList()
        {

            var petitions = UtilityService.ExecStoredProcedureWithResults<pd_PetitionGetByCaseID_spResult>("pd_PetitionGetByCaseID_sp",
                    new pd_PetitionGetByCaseID_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID
                    }).ToList();
            if (petitions.Count == 0 && Request.QueryString["route"] != null)
            {
                return RedirectToAction("PetitionAdd");
            }
            return View(petitions);
        }



        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningPetitionAddEditPage, PageSecurityItemID = SecurityToken.AddPetition)]
        public virtual ActionResult PetitionAdd()
        {

            var model = GetPetitionAddEditModel(0);
            UserManager.UpdateCaseStatusBar(UserManager.UserExtended.CaseID);
            model.CaseNumber = UserManager.UserExtended.PDAPDNumber;
            return View("PetitionAddEdit", model);
        }
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningPetitionAddEditPage, PageSecurityItemID = SecurityToken.EditPetition)]
        public virtual ActionResult PetitionEdit(string id)
        {
            var petitionId = id.ToDecrypt().ToInt();
            var model = GetPetitionAddEditModel(petitionId);

            return View("PetitionAddEdit", model);
        }

        public PetitionAddEditModel GetPetitionAddEditModel(int petitionId)
        {

            var model = new PetitionAddEditModel
            {
                AttorneyAgencyRoleTypeCodeID = (int)UtilityService.Context.pd_AgencyAttorneyGetRoleType_sp(UserManager.UserExtended.CaseNumberAgencyID,
                                                                        UserManager.UserExtended.CaseID,
                                                                        UserManager.UserExtended.UserID, Guid.NewGuid()).First(),
                AllegationTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(22),
                AllegationFindingList = UtilityFunctions.CodeGetByTypeIdAndUserId(68),

            };
            model.RoleTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetBySystemValueTypeID_spResults>(
                 "pd_CodeGetBySystemValueTypeID_sp", new pd_CodeGetBySystemValueTypeID_spParams()
                 {

                     UserID = UserManager.UserExtended.UserID,

                     BatchLogJobID = Guid.NewGuid(),
                     SystemValueIDList = "1"
                 }).ToList();
            model.RoleList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForPetition_spResult>(
                  "pd_RoleGetForPetition_sp", new pd_RoleGetForPetition_spParams()
                  {
                      CaseID = UserManager.UserExtended.CaseID,
                      UserID = UserManager.UserExtended.UserID,
                      AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                      BatchLogJobID = Guid.NewGuid(),
                      PetitionID = petitionId
                  }).ToList();



            var caseAttribute = UtilityService.ExecStoredProcedureWithResults<CaseAttributeGetByType_spResult<string>>(
                  "CaseAttributeGetByType_sp", new CaseAttributeGetByType_spParams()
                  {

                      UserID = UserManager.UserExtended.UserID,

                      BatchLogJobID = Guid.NewGuid(),
                      CaseID = UserManager.UserExtended.CaseID,
                      CaseAttributeTypeID = 3000,
                      TableID = petitionId
                  }).FirstOrDefault();
            if (caseAttribute != null)
            {

                model.PhysicalFileName = caseAttribute.CaseAttributeGenericValue?.ToString();
                model.CaseAttributeID = caseAttribute.CaseAttributeID;
            }
            if (petitionId > 0)
            {


                var petition = UtilityService.ExecStoredProcedureWithResults<pd_PetitionGet_spResult>(
                    "pd_PetitionGet_sp", new pd_PetitionGet_spParams()
                    {

                        UserID = UserManager.UserExtended.UserID,

                        BatchLogJobID = Guid.NewGuid(),
                        PetitionID = petitionId
                    }).FirstOrDefault();
                if (petition != null)
                {

                    model.FileDate = petition.PetitionFileDate.ToString("d");
                    model.CloseDate = petition.PetitionCloseDate.ToDefaultFormat("d");
                    model.CaseNumber = petition.PetitionDocketNumber;
                    model.PetitionID = petitionId;
                    model.PetitionTypeID = petition.PetitionTypeCodeID;

                }

                model.AllegationList =
                    UtilityService.ExecStoredProcedureWithResults<pd_AllegationGetByPetitionID_spResult>(
                        "pd_AllegationGetByPetitionID_sp", new pd_AllegationGetByPetitionID_spParams()
                        {

                            UserID = UserManager.UserExtended.UserID,

                            BatchLogJobID = Guid.NewGuid(),

                            PetitionID = petitionId
                        }).ToList();



            }
            else
            {
                var roles = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForPetition_spResult>(
                    "pd_RoleGetForPetition_sp", new pd_RoleGetForPetition_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        BatchLogJobID = Guid.NewGuid()

                    });//.Where(o => o.RoleTypeCodeID == 10997).Select(o => new SelectListItem() { Value = o.PersonID.ToString(), Text = o.PersonName }).ToList();

                model.AttorneyList = roles.Where(o => o.AgencyAttorneyFlag == 1)
                    .Select(o => new SelectListItem() { Value = o.PersonID.ToString() + "|" + o.RoleID.ToString() + "|" + o.RoleTypeCodeID.ToString(), Text = o.OnCase == 1 ? o.PersonName + " (On Case)" : o.PersonName })
                    .ToList();

                model.RoleList = roles.Where(o => o.RoleID > 0).ToList();

            }

            model.PetitionTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(3, includeCodeId: model.PetitionTypeID);
            if (!model.AllegationList.Any())
            {
                var allegation = new List<pd_AllegationGetByPetitionID_spResult>();
                allegation.Add(new pd_AllegationGetByPetitionID_spResult() { AllegationID = 0 });
                model.AllegationList = allegation;
            }

            var summary = UtilityService.Context.pd_CaseGet_sp(UserManager.UserExtended.CaseID, UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
            if (summary != null)
            {
                model.CaseClosedDate = summary.CaseClosedDate.HasValue ? 1 : 0;
            }

            if (model.CaseClosedDate == 1 && model.PetitionID == 0)
            {
                model.RoleList = model.RoleList.Where(o => o.UsePetitionRoleDateRange != 1).ToList();
            }
            return model;
        }


        [HttpPost]
        public virtual ActionResult PetitionSave(PetitionAddEditModel model, PetitionAddEditModel oldModel)
        {
            //    Bugsnag.Clients.WebMVCClient.Notify(new Exception("Petition Add/Edit Custom Error For Data Debugging"));


            var logFileName = DateTime.Now.ToFileTimeUtc() + UserManager.UserExtended.CaseJcatsNumber + "_PetitionAddEdit.txt";
            #region Permission Check

            // UtilityFunctions.LogFile(logFileName, "Permission check ");

            UserManager.CheckSecurityPermission(PageLevelSecurityItemIds.CaseOpeningPetitionAddEditPage);
            if ((model.PetitionID == 0 && !UserManager.IsUserAccessTo(SecurityToken.AddPetition)) ||
                model.PetitionID > 0 && !UserManager.IsUserAccessTo(SecurityToken.EditPetition))
            {
                return Json(new
                {
                    Status = "Faild",
                    URL = "/Home/AccessDenied"
                });
            }

            #endregion

            var isAddMode = false;
            var attorneyRoleId = 0;
            if (model.PetitionID == 0)
            {
                if (!string.IsNullOrEmpty(model.AttorneyID))
                {
                    // UtilityFunctions.LogFile(logFileName, "AttorneyID is not null for add petition");

                    var personId = model.AttorneyID.Split('|')[0].ToInt();
                    attorneyRoleId = model.AttorneyID.Split('|')[1].ToInt();
                    var roleTypeId = model.AttorneyID.Split('|')[2].ToInt();
                    if (attorneyRoleId == 0)
                    {
                        attorneyRoleId = UtilityService.ExecStoredProcedureWithResults<int>("pd_RoleInsert_sp", new pd_RoleInsert_spParams()
                        {
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID,
                            RecordStateID = 1,
                            PersonID = personId,
                            CaseID = UserManager.UserExtended.CaseID,
                            RoleClient = 0,
                            RoleTypeCodeID = roleTypeId,
                            RoleStartDate = model.FileDate.ToDateTime(),


                        }).FirstOrDefault();

                        // UtilityFunctions.LogFile(logFileName, "Attorney RoleID returned after inserted " + attorneyRoleId.ToString());

                    }


                }

            }
            if (model.PetitionID == 0)
            {
                isAddMode = true;
                var petitionId = UtilityService.Context.pd_PetitionInsert_sp(new ObjectParameter("PetitionID", 0), UserManager.UserExtended.CaseNumberAgencyID,
                 UserManager.UserExtended.CaseID, model.FileDate.ToDateTime(), model.CaseNumber, model.PetitionTypeID,
                 model.CloseDate.IsNullOrEmpty() ? (DateTime?)null : model.CloseDate.ToDateTime(), 1, new ObjectParameter("RecordTimeStamp", ""),
                 UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();


                // UtilityFunctions.LogFile(logFileName, "PetitionID returned after inserted " + petitionId.Value.ToString());

                if (petitionId != null)
                    model.PetitionID = (int)petitionId;
                if (attorneyRoleId > 0)
                {
                    UtilityService.Context.pd_PetitionRoleInsert_sp(new ObjectParameter("PetitionRoleID", 0),
                                UserManager.UserExtended.CaseNumberAgencyID,
                                model.PetitionID, attorneyRoleId, 1, new ObjectParameter("RecordTimeStamp", ""), UserManager.UserExtended.UserID, Guid.NewGuid())
                                .FirstOrDefault();

                    // UtilityFunctions.LogFile(logFileName, "Petition Role Inserted " + attorneyRoleId);

                }
            }
            else
            {
                if (model.PetitionTypeID != oldModel.PetitionTypeID || model.CloseDate != oldModel.CloseDate ||
                    model.FileDate != oldModel.FileDate || model.CaseNumber != oldModel.CaseNumber)
                {
                    UtilityService.Context.pd_PetitionUpdate_sp(model.PetitionID, UserManager.UserExtended.CaseNumberAgencyID,
                 UserManager.UserExtended.CaseID, model.FileDate.ToDateTime(), model.CaseNumber, model.PetitionTypeID,
                 model.CloseDate.IsNullOrEmpty() ? (DateTime?)null : model.CloseDate.ToDateTime(), 1, new ObjectParameter("RecordTimeStamp", ""),
                 UserManager.UserExtended.UserID, Guid.NewGuid());
                    // UtilityFunctions.LogFile(logFileName, "Petition Updated ");

                }


            }


            if (model.RoleList != null && model.RoleList.Any() && UserManager.IsUserAccessTo(SecurityToken.AddDeletePetitionRole))
            {
                foreach (var roleItem in model.RoleList)
                {
                    if (roleItem.Selected == 1)
                    {
                        //UtilityService.Context.pd_PetitionRoleInsert_sp(new ObjectParameter("PetitionRoleID", 0),
                        //    UserManager.UserExtended.CaseNumberAgencyID,
                        //    model.PetitionID, roleItem.RoleID, 1, new ObjectParameter("RecordTimeStamp", ""), UserManager.UserExtended.UserID, Guid.NewGuid())
                        //    .FirstOrDefault();

                        // UtilityFunctions.LogFile(logFileName, "Petition Role From Checkbox Checked List Inserted ");
                        if (roleItem.PetitionRoleID.ToInt() == 0)
                        {
                            UtilityService.ExecStoredProcedureWithoutResults("pd_PetitionRoleInsert_sp", new pd_PetitionRoleInsert_spParams
                            {
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid(),
                                AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                PetitionID = model.PetitionID,
                                RoleID = roleItem.RoleID,
                                RecordStateID = 1,
                                PetitionRoleStartDate = roleItem.PetitionRoleStartDate.ToDateTimeNullableValue(),
                                PetitionRoleEndDate = roleItem.PetitionRoleEndDate.ToDateTimeNullableValue(),
                                RecordTimeStamp = null
                            });
                        }
                        else
                        {
                            UtilityService.ExecStoredProcedureWithoutResults("pd_PetitionRoleUpdate_sp", new pd_PetitionRoleUpdate_spParams
                            {
                                UserID = UserManager.UserExtended.UserID,
                                PetitionRoleID = roleItem.PetitionRoleID,
                                PetitionRoleStartDate = roleItem.PetitionRoleStartDate.ToDateTimeNullableValue(),
                                PetitionRoleEndDate = roleItem.PetitionRoleEndDate.ToDateTimeNullableValue(),
                            });

                        }

                    }
                    else
                    {
                        UtilityService.ExecStoredProcedureWithoutResults("pd_PetitionRoleDelete_sp", new pd_PetitionRoleDelete_spParams
                        {
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                            ID = roleItem.PetitionRoleID.ToInt(),

                            RecordTimeStamp = null
                        });
                        // UtilityFunctions.LogFile(logFileName, "Petition Role Deleted which are unchecked ");

                    }
                }
            }



            if (model.PhysicalFileName != oldModel.PhysicalFileName)
            {
                var caseAttributeId = UtilityService.ExecStoredProcedureWithResults<int>("CaseAttributeInsert_sp", new CaseAttributeInsert_spParams()
                {
                    CaseAttributeID = model.CaseAttributeID,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    RecordStateID = 1,

                    CaseID = UserManager.UserExtended.CaseID,
                    CaseAttributeGenericValue = model.PhysicalFileName,
                    CaseAttributeTypeID = 3000,
                    TableID = model.PetitionID,
                }).FirstOrDefault();

                // UtilityFunctions.LogFile(logFileName, "PhysicalFileName Updated ");

            }



            UtilityService.Context.sup_CasePetitionNumberSet_sp(UserManager.UserExtended.CaseID,
                UserManager.UserExtended.UserID, Guid.NewGuid(), 0);

            // UtilityFunctions.LogFile(logFileName, "sup_CasePetitionNumberSet_sp vs ");

            UtilityService.Context.sup_SetAttorneyFlags_sp(UserManager.UserExtended.CaseID,
                UserManager.UserExtended.UserID, Guid.NewGuid(), 0);
            // UtilityFunctions.LogFile(logFileName, "sup_SetAttorneyFlags_sp SPS Call Done ");

            if (model.AllegationList.Any())
            {
                // UtilityFunctions.LogFile(logFileName, "<---- AllegationList exists Loop Started ----> ");

                #region


                foreach (var item in model.AllegationList)
                {
                    if (item.AllegationID == 0)
                    {
                        // UtilityFunctions.LogFile(logFileName, "if AllegationID is  zero then insert");

                        if (item.RecordStateID == 1)
                        {
                            // UtilityFunctions.LogFile(logFileName, "if RecordStateID is  1 then insert");

                            if (item.AllegationTypeCodeID.HasValue)
                            {
                                // UtilityFunctions.LogFile(logFileName, "if AllegationTypeCodeID is selected");

                                var id =
                                    UtilityService.ExecStoredProcedureScalar("pd_AllegationInsert_sp", new pd_AllegationInsert_spParams()
                                    {
                                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                        AllegationFindingCodeID = item.AllegationFindingCodeID,
                                        AllegationIdentifier = item.AllegationIdentifier,
                                        AllegationTypeCodeID = item.AllegationTypeCodeID,
                                        PetitionID = model.PetitionID,
                                        RecordStateID = 1,
                                        BatchLogJobID = Guid.NewGuid(),
                                        UserID = UserManager.UserExtended.UserID
                                    }).ToInt();

                                item.AllegationID = id;
                                // UtilityFunctions.LogFile(logFileName, "Allegation inserted " + item.AllegationID);

                                if (!item.NoteEntry.IsNullOrEmpty() && item.AllegationID > 0)
                                {
                                    item.NoteID = UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", new pd_NoteInsert_spParams()
                                    {
                                        NoteEntitySystemValueTypeID = 114,
                                        NoteEntityTypeSystemValueTypeID = 123,
                                        EntityPrimaryKeyID = item.AllegationID,
                                        NoteTypeCodeID = 0,
                                        NoteEntry = item.NoteEntry,
                                        CaseID = UserManager.UserExtended.CaseID,
                                        PetitionID = model.PetitionID,
                                        RecordStateID = 1,
                                        BatchLogJobID = Guid.NewGuid(),
                                        UserID = UserManager.UserExtended.UserID
                                    }).FirstOrDefault();

                                    // UtilityFunctions.LogFile(logFileName, "Allegation note inserted " + item.NoteID);


                                }
                            }


                        }
                        else
                        {
                            // UtilityFunctions.LogFile(logFileName, "RecordStateID is not 1 so record skip for insert.");

                        }
                    }
                    else
                    {
                        if (item.RecordStateID == 10)
                        {
                            UtilityService.ExecStoredProcedureWithoutResults("pd_AllegationDelete_sp", new pd_AllegationDelete_spParams()
                            {
                                ID = item.AllegationID,
                                BatchLogJobID = Guid.NewGuid(),
                                UserID = UserManager.UserExtended.UserID
                            });
                            // UtilityFunctions.LogFile(logFileName, "RecordStateID is 10 so record deleted AllegationID=" + item.AllegationID);

                        }
                        else
                        {


                            if (item.AllegationTypeCodeID.HasValue)
                            {
                                UtilityService.ExecStoredProcedureWithoutResults("pd_AllegationUpdate_sp",
                                    new pd_AllegationUpdate_spParams()
                                    {
                                        AllegationID = item.AllegationID,
                                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                        AllegationFindingCodeID = item.AllegationFindingCodeID,
                                        AllegationIdentifier = item.AllegationIdentifier,
                                        AllegationTypeCodeID = item.AllegationTypeCodeID,
                                        PetitionID = model.PetitionID,
                                        RecordStateID = item.RecordStateID,
                                        BatchLogJobID = Guid.NewGuid(),
                                        UserID = UserManager.UserExtended.UserID
                                    });

                                // UtilityFunctions.LogFile(logFileName, "pd_AllegationUpdate_sp Allegation updated. AllegationID=" + item.AllegationID);

                            }
                            if (!item.NoteEntry.IsNullOrEmpty() && !item.NoteID.HasValue)
                            {
                                item.NoteID = UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", new pd_NoteInsert_spParams()
                                {
                                    NoteEntitySystemValueTypeID = 114,
                                    NoteEntityTypeSystemValueTypeID = 123,
                                    EntityPrimaryKeyID = item.AllegationID,
                                    NoteTypeCodeID = 0,
                                    NoteEntry = item.NoteEntry,
                                    CaseID = UserManager.UserExtended.CaseID,
                                    PetitionID = model.PetitionID,
                                    RecordStateID = 1,
                                    BatchLogJobID = Guid.NewGuid(),
                                    UserID = UserManager.UserExtended.UserID
                                }).FirstOrDefault();
                                // UtilityFunctions.LogFile(logFileName, "Allegation note inserted. NoteID=" + item.NoteID);

                            }
                            else if (item.NoteID.HasValue && item.NoteID.Value > 0)
                            {
                                if (item.NoteEntry.IsNullOrEmpty())
                                {
                                    UtilityService.ExecStoredProcedureWithoutResults("pd_NoteDelete_sp",
                                      new LALoDep.Domain.pd_Conflict.pd_NoteDelete_spParams()
                                      {
                                          ID = item.NoteID.Value,

                                          BatchLogJobID = Guid.NewGuid(),
                                          UserID = UserManager.UserExtended.UserID
                                      });
                                    // UtilityFunctions.LogFile(logFileName, "Allegation note deleted because note textbox was empty. NoteID=" + item.NoteID.Value);

                                }
                                else
                                {
                                    UtilityService.ExecStoredProcedureWithoutResults("pd_NoteUpdate_sp",
                                   new del_NoteUpdate_spParams()
                                   {
                                       NoteID = item.NoteID.Value,
                                       NoteEntityCodeID = 3280,
                                       NoteEntityTypeCodeID = 3288,
                                       EntityPrimaryKeyID = item.AllegationID,
                                       NoteTypeCodeID = 0,
                                       NoteEntry = item.NoteEntry,
                                       CaseID = UserManager.UserExtended.CaseID,
                                       PetitionID = model.PetitionID,
                                       RecordStateID = 1,
                                       BatchLogJobID = Guid.NewGuid(),
                                       UserID = UserManager.UserExtended.UserID
                                   });

                                    // UtilityFunctions.LogFile(logFileName, "Allegation note updated. NoteID=" + item.NoteID.Value);

                                }

                            }
                        }
                    }
                }



                #endregion

                // UtilityFunctions.LogFile(logFileName, "<---- AllegationList exists Loop Ended ----> ");

            }
            UserManager.UpdateCaseStatusBar(UserManager.UserExtended.CaseID);
            // UtilityFunctions.LogFile(logFileName, "UpdateCaseStatusBar to refresh the Status bar ");

            return Json(new { Status = "Done", ReturnUrl = "/CaseOpening/PetitionEdit/" + model.PetitionID.ToEncrypt() });
        }



        [HttpPost]
        public virtual ActionResult PetitionDelete(int id)
        {


            #region Permission Check

            UserManager.CheckSecurityPermission(PageLevelSecurityItemIds.CaseOpeningPetitionAddEditPage);
            if (!UserManager.IsUserAccessTo(SecurityToken.DeletePetition))
            {
                return Json(new
                {
                    Status = "Faild",
                    URL = "/Home/AccessDenied"
                });
            }

            #endregion


            UtilityService.ExecStoredProcedureWithoutResults("pd_PetitionDelete_sp", new pd_PetitionDelete_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                ID = id,
                LoadOption = "Petition",
                RecordStateID = 10,
            });



            return Json(new { Status = "Done" });
        }


        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningPetitionPage, IsCasePage = true, PageSecurityItemID = SecurityToken.ViewPetition)]
        public virtual ActionResult PetitionCopy(int id)
        {
            var model = new PetitionCopyModel
            {
                PetitionID = id,
                PetitionRoleList =
                    UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForPetitionCopy_spResult>(
                        "pd_RoleGetForPetitionCopy_sp",
                        new pd_RoleGetForPetitionCopy_spParams()
                        {
                            FromPetitionID = id,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                            CaseID = UserManager.UserExtended.CaseID
                        }).ToList(),
                Petition = UtilityService.ExecStoredProcedureWithResults<pd_PetitionGet_spResult>(
                    "pd_PetitionGet_sp", new pd_PetitionGet_spParams()
                    {

                        UserID = UserManager.UserExtended.UserID,

                        BatchLogJobID = Guid.NewGuid(),
                        PetitionID = id
                    }).FirstOrDefault()
            };
            if (model.Petition != null)
            {
                model.PetitionFileDate = model.Petition.PetitionFileDate;
                model.PetitionTypeCodeID = model.Petition.PetitionTypeCodeID;
            }


            return View(model);
        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseOpeningPetitionPage, IsCasePage = true, PageSecurityItemID = SecurityToken.CopyPetition)]
        public virtual ActionResult PetitionCopy(PetitionCopyModel model)
        {



            if (model.PetitionRoleList.Any())
            {

                var roles = UtilityService.ExecStoredProcedureWithResults<pd_PetitionRoleGetForPetitionCopy_spResult>(
                    "pd_PetitionRoleGetForPetitionCopy_sp", new pd_PetitionRoleGetForPetitionCopy_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        PetitionID = model.PetitionID
                    }).Where(o => o.Selected == 1 && o.Role != "Child").ToList();
                var allegations = UtilityService.ExecStoredProcedureWithResults<pd_AllegationGetByPetitionID_spResult>(
                    "pd_AllegationGetByPetitionID_sp", new pd_AllegationGetByPetitionID_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        PetitionID = model.PetitionID
                    }).ToList();




                foreach (var item in model.PetitionRoleList)
                {
                    var petition = UtilityService.Context.pd_PetitionInsert_sp(new ObjectParameter("PetitionID", 0), null,
                  UserManager.UserExtended.CaseID, model.PetitionFileDate.ToDateTime(), item.DefaultPetitonNumber, model.PetitionTypeCodeID,
                (DateTime?)null, 1, new ObjectParameter("RecordTimeStamp", ""),
                  UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();

                    if (petition != null)
                    {

                        var petitionId = petition.Value;

                        PetitionRoleInsert(null, petitionId, item.RoleID.Value);
                        foreach (var role in roles)
                        {
                            //  PetitionRoleInsert(null, petitionId, role.RoleID.Value);

                            UtilityService.ExecStoredProcedureWithoutResults("pd_PetitionRoleInsert_sp", new pd_PetitionRoleInsert_spParams
                            {
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid(),
                                AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                PetitionID = petitionId,
                                RoleID = role.RoleID,
                                RecordStateID = 1,
                                PetitionRoleStartDate = role.PetitionRoleStartDate,

                                RecordTimeStamp = null
                            });
                        }
                        foreach (var allegation in allegations)
                        {
                            AllegationInsert(allegation, petitionId);
                        }
                    }
                }
                UtilityService.ExecStoredProcedureScalar("sup_CasePetitionNumberSet_sp",
               new sup_CasePetitionNumberSet_spParams()
               {
                   CaseID = UserManager.UserExtended.CaseID,
                   AdminFlag = 0,
                   BatchLogJobID = Guid.NewGuid(),
                   UserID = UserManager.UserExtended.UserID
               });
                UtilityService.ExecStoredProcedureScalar("sup_SetAttorneyFlags_sp",
                  new sup_SetAttorneyFlags_spParams()
                  {
                      CaseID = UserManager.UserExtended.CaseID,
                      AdminFlag = 0,
                      BatchLogJobID = Guid.NewGuid(),
                      UserID = UserManager.UserExtended.UserID
                  });
            }

            return Json(new { Status = "Done" });

        }


        private int AllegationInsert(pd_AllegationGetByPetitionID_spResult allegation, int petitionId)
        {
            return UtilityService.ExecStoredProcedureScalar("pd_AllegationInsert_sp", new pd_AllegationInsert_spParams()
            {
                AllegationTypeCodeID = allegation.AllegationTypeCodeID,
                PetitionID = petitionId,
                RecordStateID = 1,
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID,
                AgencyID = allegation.AgencyID,
                AllegationFindingCodeID = allegation.AllegationFindingCodeID,
                AllegationIdentifier = allegation.AllegationIdentifier,

                CopyNoteID = allegation.NoteID
            }).ToInt();
        }
        private int PetitionRoleInsert(int? agencyId, int petitionId, int roleId)
        {
            return (int)UtilityService.Context.pd_PetitionRoleInsert_sp(new ObjectParameter("PetitionRoleID", 0),
                agencyId,
                petitionId, roleId, 1, new ObjectParameter("RecordTimeStamp", ""), UserManager.UserExtended.UserID, Guid.NewGuid())
                .FirstOrDefault();
        }



        #region Step 6. QHE Allegations
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.EditPetition)]
        public virtual ActionResult EditAllegationFinding()
        {
            var viewModel = new EditAllegationFindingViewModel();

            viewModel.Petitions = UtilityService.ExecStoredProcedureWithResults<pd_PetitionGetByCaseID_spResult>("pd_PetitionGetByCaseID_sp",
                    new pd_PetitionGetByCaseID_spParams()
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID
                    }).ToList();

            viewModel.Findings = UtilityFunctions.CodeGetByTypeIdAndUserId(68);
            var allegations = new List<AllegationViewModel>();
            foreach (var item in viewModel.Petitions)
            {
                var data = UtilityService.ExecStoredProcedureWithResults<pd_AllegationGetByPetitionID_spResult>(
                           "pd_AllegationGetByPetitionID_sp", new pd_AllegationGetByPetitionID_spParams()
                           {

                               UserID = UserManager.UserExtended.UserID,

                               BatchLogJobID = Guid.NewGuid(),

                               PetitionID = item.PetitionID
                           }).Select(x => new AllegationViewModel
                           {
                               PetitionID = x.PetitionID,
                               AllegationID = x.AllegationID,
                               AllegationTypeCodeValue = x.AllegationTypeCodeValue,
                               AllegationTypeCodeID = x.AllegationTypeCodeID,
                               AllegationFindingCodeID = x.AllegationFindingCodeID,
                               RecordStateID = x.RecordStateID,
                               AllegationIdentifier = x.AllegationIdentifier,
                               NoteEntry = x.NoteEntry
                           }).ToList();

                allegations.AddRange(data);
            }
            viewModel.Allegations = allegations;

            return View(viewModel);
        }
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.EditAllegation)]
        [HttpPost]
        public virtual ActionResult EditAllegationFinding(EditAllegationFindingViewModel viewModel)
        {
            if (viewModel.Allegations != null && viewModel.Allegations.Count > 0)
            {
                foreach (var item in viewModel.Allegations)
                {
                    int? findingCode = viewModel.GlobalFindingCodeId;

                    if (item.PetitionGlobalFindingCodeId.HasValue && !findingCode.HasValue)
                        findingCode = item.PetitionGlobalFindingCodeId;

                    if (item.IsChanged && !findingCode.HasValue)
                        findingCode = item.AllegationFindingCodeID;
                    if (!item.IsChanged && item.AllegationFindingCodeID.HasValue && item.AllegationFindingCodeID.Value > 0)
                    {
                        continue;
                    }

                    UtilityService.ExecStoredProcedureWithResults<object>("pd_AllegationUpdate_sp", new pd_AllegationUpdate_spParams()
                    {
                        AllegationID = item.AllegationID ?? 0,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        PetitionID = item.PetitionID,
                        AllegationTypeCodeID = item.AllegationTypeCodeID,
                        AllegationFindingCodeID = findingCode,
                        RecordStateID = item.RecordStateID ?? 1,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserManager.UserExtended.UserID,
                        AllegationIdentifier = item.AllegationIdentifier,

                    }).FirstOrDefault();
                }

            }



            return Json(new { isSuccess = true });
        }
        #endregion

    }
}