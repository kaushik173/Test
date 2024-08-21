using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Areas.Mobile.Models;
using LALoDep.Custom;
using LALoDep.Models;
using LALoDep.Domain.pd_Work;
using LALoDep.Domain.Mobile;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Petition;
using LALoDep.Domain.pd_Case;
using Omu.ValueInjecter;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Users;
using System.Data.Entity.Core.Objects;

namespace LALoDep.Areas.Mobile.Controllers
{
    public partial class CaseController : Controller
    {
        private IUtilityService _utilityService;
        private UserManager _userManager;

        public CaseController(UserManager userManager, IUtilityService utilityService)
        {
            _userManager = userManager;
            _utilityService = utilityService;
        }

        //       [MobileClaimsAuthorize]
        public virtual ActionResult Search()
        {
            var viewmodel = new CaseSearchViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        //  [MobileClaimsAuthorize]
        public virtual JsonResult Search(CaseSearchViewModel searchParams)
        {
            var list = _utilityService.ExecStoredProcedureWithResults<MobileCaseSearch_spResult>(
                "MobileCaseSearch_sp", new MobileCaseSearch_spParams
                {
                    UserID = _userManager.UserExtended.UserID,
                    BatchLogJobID = null
                    ,
                    FirstName = searchParams.FirstName,
                    LastName = searchParams.LastName,
                    AttorneyPersonID = _userManager.UserExtended.AttorneyPersonID,
                    OnlyOpenCases = searchParams.OnlyOpenCases,
                    PetitionNumber = searchParams.DocketNumber,
                    StartRecord = 1,
                    Range = 20

                }).ToList();
            var total = list.Count;

            var searchList = list.Select(c => new
            {
                Name = c.PersonNameLast + " " + c.PersonNameFirst,

                DOB = c.DOB,
                Sex = c.Sex,
                Role = c.Role,

                PetitionDocketNumber = c.PetitionDocketNumber,//need to confirn

                CaseID = c.CaseID,
                EncryptedCaseID = c.CaseID.ToEncrypt(),



            }).ToList();

            return Json(new DataTablesResponse(0, searchList, total, total));
        }

        // GET: Mobile/Case
        //   [MobileClaimsAuthorize]
        public virtual ActionResult CaseInfo(string id)
        {

            int? caseID = null;
            if (string.IsNullOrEmpty(id))
                caseID = _userManager.UserExtended.CaseID;
            else
                caseID = Convert.ToInt32(Utility.Decrypt(id));

            _userManager.UpdateCaseStatusBar(caseID.Value);

            var viewModel = new CaseInfoMobileViewModel
            {
                Hearings =
                    _utilityService.ExecStoredProcedureWithResults<MobileCaseInfoGetHearings_spResult>(
                        "MobileCaseInfoGetHearings_sp", new MobileCaseInfoGetHearings_spParams
                        {
                            UserID = _userManager.UserExtended.UserID,
                            BatchLogJobID = null
                            ,
                            CaseID = _userManager.UserExtended.CaseID
                        }).ToList(),
                Roles =
                    _utilityService.ExecStoredProcedureWithResults<MobileRoleInfoGet_spResult>("MobileRoleInfoGet_sp",
                        new MobileRoleInfoGet_spParams
                        {
                            UserID = _userManager.UserExtended.UserID,
                            BatchLogJobID = null
                            ,
                            CaseID = _userManager.UserExtended.CaseID,
                            AttorneyPersonID = _userManager.UserExtended.AttorneyPersonID
                        }).ToList(),
                CaseID = _userManager.UserExtended.CaseID,
                CaseNumber = _userManager.UserExtended.CaseJcatsNumber,
                ClientName = _userManager.UserExtended.Client
            };


            return View(viewModel);
        }

        #region Case Add
        public virtual ActionResult CaseAdd()
        {
            _userManager.UpdateCaseStatusBar(0);

            var viewModel = new CaseAddViewModel();
            viewModel.ClientRoleList = _utilityService.ExecStoredProcedureWithResults<pd_MobileCodesGet_spResult>("pd_MobileCodesGet_sp", new pd_MobileCodesGet_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,
                SystemValueIDList = "1",
                CodeDisplayLength = 10,
                CodePaddingChar = "&nbsp;",
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()

            }).Select(o => new SelectListItem { Value = o.CodeID.ToString(), Text = o.CodeDisplaySort }).ToList();

            viewModel.DepartmentList = _utilityService.ExecStoredProcedureWithResults<pd_MobileCodesGet_spResult>("pd_MobileCodesGet_sp", new pd_MobileCodesGet_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,
                CodeTypeID = 30,
                CodeDisplayLength = 10,
                CodePaddingChar = "&nbsp;",
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()

            }).Select(o => new SelectListItem { Value = o.CodeID.ToString(), Text = o.CodeDisplaySort }).ToList();

            viewModel.AssociationList = _utilityService.ExecStoredProcedureWithResults<pd_MobileCodesGet_spResult>("pd_MobileCodesGet_sp", new pd_MobileCodesGet_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,
                CodeTypeID = 24,
                CodeDisplayLength = 10,
                CodePaddingChar = "&nbsp;",
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()

            }).Select(o => new SelectListItem { Value = o.CodeID.ToString(), Text = o.CodeDisplaySort }).ToList();

            viewModel.AllegationList = UtilityFunctions.CodeGetByTypeIdAndUserId(22);

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult CaseAdd(CaseAddViewModel viewModel)
        {
            ObjectParameter gUID = new ObjectParameter("gUID", _userManager.UserExtended.Guid);
            ObjectParameter totalRecords = new ObjectParameter("totalRecords", DBNull.Value);
            var list = _utilityService.Context.pd_CaseSearch_sp(null, null, viewModel.PetitionNumber, null, 1, null, gUID, 1, 100, null, totalRecords,
                                _userManager.UserExtended.UserID, Guid.NewGuid(), null).ToList();

            if (list.Count > 0)
            {
                return Json(new { isSuccess = false, isCaseNumberExist = true, message = "Case # already exists. This case was not added." });
            }

            var caseID = _utilityService.ExecStoredProcedureWithResults<int>("MobileCaseInsert_sp", new MobileCaseInsert_spParams
            {
                AttorneyPersonID = _userManager.UserExtended.PersonID,
                ClientLastName = viewModel.ClientLastName,
                ClientFirstName = viewModel.ClientFirstName,
                ClientRoleTypeCodeID = viewModel.ClientRoleTypeCodeID,
                CaseLastName = viewModel.CaseLastName,
                CaseFirstName = viewModel.CaseFirstName,
                CaseAppointmentDate = viewModel.CaseAppointmentDate.ToDateTimeValue(),
                PetitionNumber = viewModel.PetitionNumber,
                DepartmentCodeID = viewModel.DepartmentCodeID,
                AssociationCodeID = viewModel.AssociationCodeID,
                PetitionFileDate = viewModel.CaseAppointmentDate.ToDateTimeValue(),

                RecordStateID = 1,

                AllegationTypeCodeID1 = viewModel.AllegationTypeCodeID1,
                AllegationTypeCodeID2 = viewModel.AllegationTypeCodeID2,
                AllegationTypeCodeID3 = viewModel.AllegationTypeCodeID3,
                AllegationTypeCodeID4 = viewModel.AllegationTypeCodeID4,

                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),


            }).FirstOrDefault();

            return Json(new { isSuccess = true, URL = Url.Action(MVC.Mobile.Case.CaseInfo(caseID.ToEncrypt())) });
        }
        #endregion

        #region Hearing Add

        public virtual ActionResult HearingAdd()
        {
            ViewBag.ClinetName = _userManager.UserExtended.Client;
            ViewBag.CaseNumber = _userManager.UserExtended.PDAPDNumber;

            var model = new HearingAddEditViewModel();
            model.HearingTypeList = _utilityService.ExecStoredProcedureWithResults<pd_MobileCodesGet_spResult>("pd_MobileCodesGet_sp", new pd_MobileCodesGet_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,
                CodeTypeID = 10,
                CodeDisplayLength = 15,
                CodePaddingChar = "&nbsp;"

            }).Select(o => new CodeViewModel { CodeID = o.CodeID, CodeValue = o.CodeDisplaySort }).ToList();
            model.DepartmentList = _utilityService.ExecStoredProcedureWithResults<pd_MobileCodesGet_spResult>("pd_MobileCodesGet_sp", new pd_MobileCodesGet_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,
                CodeTypeID = 30,
                CodeDisplayLength = 15,
                CodePaddingChar = "&nbsp;"

            }).Select(o => new CodeViewModel { CodeID = o.CodeID, CodeValue = o.CodeDisplaySort }).ToList();
            model.JudicialOfficerList = _utilityService.ExecStoredProcedureWithResults<pd_HearingOfficerGet_spResults>("pd_HearingOfficerGet_sp", new pd_HearingOfficerGet_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,

            }).Where(o => o.ActiveFlag == 1).Select(o => new CodeViewModel { CodeID = o.PersonID, CodeValue = o.PersonNameLast }).ToList();
            model.HearingDate = DateTime.Now.ToString("d");
            model.Petitions = _utilityService.ExecStoredProcedureWithResults<pd_PetitionGetByCaseID_spResult>("pd_PetitionGetByCaseID_sp", new pd_PetitionGetByCaseID_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),


            }).Where(o => string.IsNullOrEmpty(o.CloseDate)).ToList();

            var caseDefault =
              _utilityService.ExecStoredProcedureWithResults<pd_CaseGetDefaults_spResults>("pd_CaseGetDefaults_sp",
                  new pd_CaseGetDefaults_spParams
                  {
                      UserID = _userManager.UserExtended.UserID,

                      BatchLogJobID = Guid.NewGuid(),
                      CaseID = _userManager.UserExtended.CaseID
                  }).FirstOrDefault();
            if (caseDefault != null)
            {
                model.HearingTime = caseDefault.DefaultHearingTime;
                if (caseDefault.DefaultHearingCourtDepartmentCodeID != null)
                {
                    model.DepartmentID = caseDefault.DefaultHearingCourtDepartmentCodeID.Value;

                }
                if (caseDefault.DefaultHearingOfficerPersonID != null)
                    model.HearingOfficerStaffID = caseDefault.DefaultHearingOfficerPersonID.Value;
            }



            return View(model);
        }


        [HttpPost]
        public virtual ActionResult HearingAdd(HearingAddEditViewModel viewModel)
        {
            //     EXECUTE dbo.pd_HearingInsert_sp NULL, NULL,10144034,962,'02/01/2017 08:30:00 AM',23493082,20935, NULL, NULL, NULL, NULL,1, NULL,60001700, NULL
            //go
            // EXECUTE dbo.pd_HearingPersonInsertByPetitionID_sp NULL,61879352,60260937, NULL,1,60001700, NULL
            // go
            var hearingId = _utilityService.ExecStoredProcedureWithResults<decimal>("pd_HearingInsert_sp", new pd_HearingInsert_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,

                BatchLogJobID = Guid.NewGuid(),
                HearingCourtDepartmentCodeID = viewModel.DepartmentID.Value,
                HearingDateTime = DateTime.Parse(viewModel.HearingDate + " " + viewModel.HearingTime),

                HearingOfficerPersonID = viewModel.HearingOfficerStaffID.Value,
                HearingTypeCodeID = viewModel.HearingTypeCodeID.Value,
                UserID = _userManager.UserExtended.UserID,
                RecordStateID = 1,

            }).FirstOrDefault();
            if (Request.Form["petitionIds"] != null)
            {
                var ids = Request.Form["petitionIds"].Split(',');
                foreach (var id in ids)
                {
                    _utilityService.ExecStoredProcedureWithResults<dynamic>("pd_HearingPersonInsertByPetitionID_sp", new pd_HearingPersonInsertByPetitionID_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        RecordStateID = 1,
                        UserID = _userManager.UserExtended.UserID,
                        HearingID = (int)hearingId,
                        PetitionID = id.ToInt(),

                    }).FirstOrDefault();
                }
            }


            return Json(new { Status = "Done" });
        }

        #endregion

        #region Hearing Edit

        public virtual ActionResult HearingEdit(string id)
        {
            var hearingId = id.ToDecrypt().ToInt();
            ViewBag.ClinetName = _userManager.UserExtended.Client;
            ViewBag.CaseNumber = _userManager.UserExtended.PDAPDNumber;

            var model = new HearingAddEditViewModel();
            model.HearingTypeList = _utilityService.ExecStoredProcedureWithResults<pd_MobileCodesGet_spResult>("pd_MobileCodesGet_sp", new pd_MobileCodesGet_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,
                CodeTypeID = 10,
                CodeDisplayLength = 15,
                CodePaddingChar = "&nbsp;"

            }).Select(o => new CodeViewModel { CodeID = o.CodeID, CodeValue = o.CodeDisplaySort }).ToList();
            model.DepartmentList = _utilityService.ExecStoredProcedureWithResults<pd_MobileCodesGet_spResult>("pd_MobileCodesGet_sp", new pd_MobileCodesGet_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,
                CodeTypeID = 30,
                CodeDisplayLength = 15,
                CodePaddingChar = "&nbsp;"

            }).Select(o => new CodeViewModel { CodeID = o.CodeID, CodeValue = o.CodeDisplaySort }).ToList();
            model.JudicialOfficerList = _utilityService.ExecStoredProcedureWithResults<pd_HearingOfficerGet_spResults>("pd_HearingOfficerGet_sp", new pd_HearingOfficerGet_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,

            }).Where(o => o.ActiveFlag == 1).Select(o => new CodeViewModel { CodeID = o.PersonID, CodeValue = o.PersonNameLast }).ToList();
            model.HearingDate = DateTime.Now.ToString("d");
            model.SelectedPetitions = _utilityService.ExecStoredProcedureWithResults<pd_PetitionGetAllByHearingID_spResult>("pd_PetitionGetAllByHearingID_sp", new LALoDep.Domain.pd_Petition.pd_PetitionGetAllByHearingID_spParams
            {
                HearingID = hearingId,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),


            }).Where(o => o.Selected == 1).ToList();
            model.HourTypeList = _utilityService.ExecStoredProcedureWithResults<pd_CodeGetBySysValAndUserID_spResult>(
              "pd_CodeGetBySysValAndUserID_sp", new LALoDep.Domain.pd_Users.pd_CodeGetBySysValAndUserID_spParams()
              {
                  SystemValueIDList = "222",
                  UserID = _userManager.UserExtended.UserID,
                  BatchLogJobID = Guid.NewGuid(),
                  IncludeCodeID = model.HoursTypeID.HasValue ? model.HoursTypeID.Value : 0


              }).Select(o => new CodeViewModel { CodeID = o.CodeID, CodeValue = o.CodeValue }).ToList();


            model.PhaseList = _utilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp", new pd_CodeGetByTypeIDAndUserID_spParams()
            {
                CodeTypeID = 600,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeShortValue
            }).ToList();

            model.ResultCodeList = _utilityService.ExecStoredProcedureWithResults<pd_MobileCodesGet_spResult>("pd_MobileCodesGet_sp", new pd_MobileCodesGet_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,
                CodeTypeID = 11,
                CodeDisplayLength = 15,
                CodePaddingChar = "&nbsp;"

            }).Select(o => new CodeViewModel { CodeID = o.CodeID, CodeValue = o.CodeDisplaySort }).ToList();
            model.AppearingAttyList = _utilityService.ExecStoredProcedureWithResults<LALoDep.Domain.pd_Role.pd_RoleGetForHearingAttendingAttorney_spResult>("pd_RoleGetForHearingAttendingAttorney_sp", new LALoDep.Domain.pd_Role.pd_RoleGetForHearingAttendingAttorney_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                HearingID = hearingId

            }).Select(o => new CodeViewModel { CodeID = o.PersonID, CodeValue = o.PersonNameDisplay }).ToList();

            var caseDefault =
              _utilityService.ExecStoredProcedureWithResults<pd_CaseGetDefaults_spResults>("pd_CaseGetDefaults_sp",
                  new pd_CaseGetDefaults_spParams
                  {
                      UserID = _userManager.UserExtended.UserID,

                      BatchLogJobID = Guid.NewGuid(),
                      CaseID = _userManager.UserExtended.CaseID
                  }).FirstOrDefault();
            if (caseDefault != null)
            {
                model.HearingTime = caseDefault.DefaultHearingTime;
                if (caseDefault.DefaultHearingCourtDepartmentCodeID != null)
                {
                    model.DepartmentID = caseDefault.DefaultHearingCourtDepartmentCodeID.Value;

                }
                if (caseDefault.DefaultHearingOfficerPersonID != null)
                    model.HearingOfficerStaffID = caseDefault.DefaultHearingOfficerPersonID.Value;
            }
            var hearingInfo = _utilityService.ExecStoredProcedureWithResults<pd_HearingGet_spResult>("pd_HearingGet_sp", new pd_HearingGet_spParams()
            {
                HearingID = hearingId,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).FirstOrDefault();
            if (hearingInfo != null)
            {

                model.HearingDate = hearingInfo.HearingDateTime.Value.ToString("d");

                model.HearingTime = hearingInfo.HearingDateTime.Value.ToString("hh:mm");
                model.DepartmentID = hearingInfo.HearingCourtDepartmentCodeID;
                model.HearingOfficerStaffID = hearingInfo.HearingOfficerPersonID;
                model.HearingTypeCodeID = hearingInfo.HearingTypeCodeID;
                model.HearingID = hearingId;
                model.HearingNoteEntry = hearingInfo.NoteEntry;
                model.MediaPresent = hearingInfo.HearingMediaPresentFlag.HasValue ? hearingInfo.HearingMediaPresentFlag.Value == 1 : false;
                model.NoteID = hearingInfo.NoteID;
                model.WorkDetailHour = hearingInfo.WorkHours;
                model.PhaseID = hearingInfo.WorkPhaseCodeID;
                model.HoursTypeID = hearingInfo.WorkDescriptionCodeID;
                model.WorkID = hearingInfo.WorkID;
                model.AppearingAttyID = hearingInfo.WorkPersonID;
            }



            return View(model);
        }


        [HttpPost]
        public virtual ActionResult HearingEdit(HearingAddEditViewModel viewModel)
        {

            _utilityService.ExecStoredProcedureWithoutResultADO("pd_HearingUpdate_sp", new pd_HearingInsert_spParams
            {
                HearingID = viewModel.HearingID.Value,
                AgencyID = _userManager.UserExtended.AgencyID,
                CaseID = _userManager.UserExtended.CaseID,

                HearingTypeCodeID = viewModel.HearingTypeCodeID.Value,
                HearingDateTime = DateTime.Parse(viewModel.HearingDate + " " + viewModel.HearingTime),

                RecordStateID = 1,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                HearingCourtDepartmentCodeID = viewModel.DepartmentID.Value,
                HearingMediaPresentFlag = viewModel.MediaPresent ? 1 : 0,
                HearingOfficerPersonID = viewModel.HearingOfficerStaffID.Value,


            });

            if (viewModel.NoteID.HasValue)
            {
                UtilityFunctions.NoteUpdate(viewModel.NoteID.Value, 3279, 3288, viewModel.HearingID.Value, 0, "Hearing Note", viewModel.HearingNoteEntry, hearingId: viewModel.HearingID);
            }
            else if (!viewModel.HearingNoteEntry.IsNullOrEmpty())
            {
                UtilityFunctions.NoteInsert(113, 123, viewModel.HearingID.Value, 0, "Hearing Note", viewModel.HearingNoteEntry, hearingId: viewModel.HearingID, caseId: _userManager.UserExtended.CaseID);
            }


            if (!viewModel.AppearingAttyID.HasValue)
            {
                _utilityService.ExecStoredProcedureWithoutResultADO("pd_HearingAttendanceInsert_sp", new pd_HearingAttendanceInsert_spParams
                {
                    HearingID = viewModel.HearingID.Value,
                    AgencyID = _userManager.UserExtended.CaseNumberAgencyID,
                    NewAttendingAttorneyPersonID = viewModel.AppearingAttyID.Value,
                    BatchLogJobID = Guid.NewGuid(),
                    RoleID = 0,
                    UserID = _userManager.UserExtended.UserID,
                    RecordStateID = 1,
                });
            }


            #region
            if (viewModel.WorkID.HasValue)
            {
                _utilityService.ExecStoredProcedureWithoutResultADO("pd_WorkDelete_sp", new pd_WorkDelete_spParams
                {
                    ID = viewModel.WorkID.Value,
                    BatchLogJobID = Guid.NewGuid(),

                    UserID = _userManager.UserExtended.UserID,

                });
                _utilityService.ExecStoredProcedureWithoutResultADO("pd_WorkInsertByHearingID_sp", new pd_WorkInsertByHearingID_spParams
                {

                    AttorneyPersonID = viewModel.AppearingAttyID.Value,
                    HearingID = viewModel.HearingID.Value,
                    RecordStateID = 1,
                    WorkDescriptionCodeID = viewModel.HoursTypeID.Value,
                    WorkHours = viewModel.WorkDetailHour.Value,
                    WorkPhaseCodeID = viewModel.PhaseID.Value,
                    BatchLogJobID = Guid.NewGuid(),

                    UserID = _userManager.UserExtended.UserID

                });
            }
            else
            {
                _utilityService.ExecStoredProcedureWithoutResultADO("pd_WorkInsertByHearingID_sp", new pd_WorkInsertByHearingID_spParams
                {

                    AttorneyPersonID = viewModel.AppearingAttyID.Value,
                    HearingID = viewModel.HearingID.Value,
                    RecordStateID = 1,
                    WorkDescriptionCodeID = viewModel.HoursTypeID.Value,
                    WorkHours = viewModel.WorkDetailHour.Value,
                    WorkPhaseCodeID = viewModel.PhaseID.Value,
                    BatchLogJobID = Guid.NewGuid(),

                    UserID = _userManager.UserExtended.UserID

                });
            }
            #endregion
            var petitionIds = Request.Form["petitionIds"];
            if (!petitionIds.IsNullOrEmpty())
            {
                foreach (var id in petitionIds.Split(','))
                {
                    _utilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionUpdate_sp", new pd_HearingPetitionUpdate_spParams
                    {
                        HearingID = viewModel.HearingID.Value,
                        PetitionID = id.Split('|')[0].ToInt(),
                        HearingPetitionResultCodeID = id.Split('|')[1].ToInt(),
                        BatchLogJobID = Guid.NewGuid(),

                        UserID = _userManager.UserExtended.UserID,

                    });

                }
            }
            _utilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionUpdate_sp", new pd_HearingExpenseClosedGetByCaseID_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                BatchLogJobID = Guid.NewGuid(),
                UserID = _userManager.UserExtended.UserID
            });
            _utilityService.ExecStoredProcedureWithoutResultADO("pd_HearingExpenseGetPaidByCaseID_sp", new pd_HearingExpenseClosedGetByCaseID_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                BatchLogJobID = Guid.NewGuid(),
                UserID = _userManager.UserExtended.UserID
            });
            _utilityService.ExecStoredProcedureWithoutResultADO("pd_PetitionClosedDateGetByCaseID_sp", new pd_HearingExpenseClosedGetByCaseID_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                BatchLogJobID = Guid.NewGuid(),
                UserID = _userManager.UserExtended.UserID
            });
            _utilityService.ExecStoredProcedureWithoutResultADO("pd_HearingPetitionAutoUpdate_sp", new pd_HearingPetitionAutoUpdate_spParams
            {
                HearingID = viewModel.HearingID.Value,
                BatchLogJobID = Guid.NewGuid(),
                UserID = _userManager.UserExtended.UserID
            });
            return Json(new { Status = "Done" });
        }

        #endregion

        #region Record Time

        public virtual ActionResult RecordTime()
        {
            ViewBag.ClinetName = _userManager.UserExtended.Client;
            ViewBag.CaseNumber = _userManager.UserExtended.PDAPDNumber;

            var poParams = new LALoDep.Domain.Mobile.pd_WorkGetByCaseID_spParams
            {
                CaseID = _userManager.UserExtended.CaseID,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                ClientPersonID = -88,
                WorkerPersonID = -88
            };
            var list = _utilityService.ExecStoredProcedureWithResults<LALoDep.Domain.Mobile.pd_WorkGetByCaseID_spResult>("pd_WorkGetByCaseID_sp", poParams).ToList();

            return View(list);
        }

        public virtual ActionResult RecordTimeAdd()
        {
            ViewBag.ClinetName = _userManager.UserExtended.Client;
            ViewBag.CaseNumber = _userManager.UserExtended.PDAPDNumber;

            var viewModel = new RecordTimeAddViewModel();
            viewModel.WorkStartDate = DateTime.Now.ToString("MM/dd/yyyy");

            viewModel.WorkDescriptionList = UtilityFunctions.CodeGetByTypeIdAndUserId(66, "CodeMobileValue", viewModel.WorkDescriptionCodeID ?? 0, userShortValue: true, agencyId: _userManager.UserExtended.CaseNumberAgencyID);
            viewModel.WorkPhaseList = UtilityFunctions.CodeGetByTypeIdAndUserId(600, "CodeMobileValue", viewModel.WorkDescriptionCodeID ?? 0, userShortValue: true, agencyId: _userManager.UserExtended.CaseNumberAgencyID);
            viewModel.WorkedForList = _utilityService.ExecStoredProcedureWithResults<LALoDep.Domain.Mobile.pd_WorkRoleGetByWorkID_spResult>("pd_WorkRoleGetByWorkID_sp",
                                                        new LALoDep.Domain.Mobile.pd_WorkRoleGetByWorkID_spParams
                                                        {
                                                            CaseID = _userManager.UserExtended.CaseID,
                                                            UserID = _userManager.UserExtended.UserID,
                                                            BatchLogJobID = Guid.NewGuid()
                                                        }).ToList();

            var wtRecordTimeGetCases_spParams = new LALoDep.Domain.pd_wt.wtRecordTimeGetCases_spParams()
            {
                ReloadFlag = 1,
                CaseID = _userManager.UserExtended.CaseID,
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            viewModel.NextCaseList = _utilityService.ExecStoredProcedureWithResults<LALoDep.Domain.pd_wt.wtRecordTimeGetCases_spResult>
                                            ("wtRecordTimeGetCases_sp", wtRecordTimeGetCases_spParams).Select(x => new SelectListItem { Text = x.CaseDisplay, Value = x.CaseID.ToString() });


            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult RecordTimeAdd(RecordTimeAddViewModel viewModel)
        {
            var pd_WorkInsert1_spParams = new pd_WorkInsert1_spParams()
            {
                CaseID = _userManager.UserExtended.CaseID,
                AgencyID = _userManager.UserExtended.CaseNumberAgencyID,
                PersonID = _userManager.UserExtended.PersonID,
                WorkHours = viewModel.WorkHours,
                WorkMileage = viewModel.WorkMileage,
                WorkDescriptionCodeID = viewModel.WorkDescriptionCodeID ?? 0,
                WorkPhaseCodeID = viewModel.WorkPhaseCodeID ?? 0,
                RecordStateID = 1,
                UserID = _userManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
            };
            if (!string.IsNullOrEmpty(viewModel.WorkStartDate))
            {
                pd_WorkInsert1_spParams.WorkStartDate = viewModel.WorkStartDate.ToDateTime();
                pd_WorkInsert1_spParams.WorkEndDate = viewModel.WorkStartDate.ToDateTime();
            }
            else
            {
                pd_WorkInsert1_spParams.WorkStartDate = DateTime.Now;
                pd_WorkInsert1_spParams.WorkEndDate = DateTime.Now;
            }

            var workID = _utilityService.ExecStoredProcedureWithResults<decimal>("pd_WorkInsert1_sp", pd_WorkInsert1_spParams).FirstOrDefault();
            if (!string.IsNullOrEmpty(viewModel.NoteEntry))
            {
                UtilityFunctions.NoteInsert(152, 123, (int)workID, 16, "Record Time Note", viewModel.NoteEntry, caseId: _userManager.UserExtended.CaseID);
                //var noteID = _utilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", new pd_NoteInsert_spParams
                //{
                //    NoteEntitySystemValueTypeID = 152,
                //    NoteEntityTypeSystemValueTypeID = 123,
                //    EntityPrimaryKeyID = (int)workID,
                //    NoteTypeCodeID = 16,
                //    NoteSubject = "Record Time Note",
                //    NoteEntry = viewModel.NoteEntry,
                //    CaseID = _userManager.UserExtended.CaseID,
                //    RecordStateID = 1,
                //    UserID = _userManager.UserExtended.UserID,
                //    BatchLogJobID = Guid.NewGuid(),
                //}).FirstOrDefault();
            }

            foreach (var item in viewModel.WorkedForList)
            {
                var workRoleID = _utilityService.ExecStoredProcedureWithResults<object>("pd_WorkRoleInsert_sp", new pd_WorkRoleInsert_spParams
                {
                    WorkID = (int)workID,
                    RoleID = item.RoleID.Value,
                    RecordStateID = 1,
                    UserID = _userManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                }).FirstOrDefault();
            }

            if (viewModel.NextCaseID.HasValue && viewModel.NextCaseID > 0)
                _userManager.UpdateCaseStatusBar(viewModel.NextCaseID.Value);

            return Json(new { isSuccess = true, URL = Url.Action(MVC.Mobile.Case.RecordTime()) });
        }

        public virtual ActionResult RecordTimeEdit(string id)
        {

            if (Request.QueryString["CaseID"] != null)
            {
                _userManager.UpdateCaseStatusBar(Request.QueryString["CaseID"].ToDecrypt().ToInt());

            }



            ViewBag.ClinetName = _userManager.UserExtended.Client;
            ViewBag.CaseNumber = _userManager.UserExtended.PDAPDNumber;

            var viewModel = new RecordTimeEditViewModel();
            viewModel.WorkID = id.ToDecrypt().ToInt();

            //Get Work
            var work = _utilityService.ExecStoredProcedureWithResults<pd_WorkGet_spResult>("pd_WorkGet_sp",
                                        new pd_WorkGet_spParams
                                        {
                                            WorkID = viewModel.WorkID,
                                            UserID = _userManager.UserExtended.UserID,
                                            BatchLogJobID = Guid.NewGuid()
                                        }).FirstOrDefault();

            viewModel.InjectFrom(work);
            viewModel.WorkStartDate = work.WorkStartDate.HasValue ? work.WorkStartDate.ToShortDateString() : string.Empty;
            viewModel.WorkEndDate = work.WorkEndDate.HasValue ? work.WorkEndDate.ToShortDateString() : string.Empty;

            // Get Note
            var workNoteResult = UtilityFunctions.NoteGetByEntity(viewModel.WorkID, 152, 123).FirstOrDefault();
            if (workNoteResult != null)
            {
                viewModel.NoteEntry = workNoteResult.NoteEntry;
                viewModel.NoteID = workNoteResult.NoteID ?? 0;
                viewModel.NoteAgencyID = workNoteResult.AgencyID;
                viewModel.NoteEntityCodeID = workNoteResult.NoteEntityCodeID;
                viewModel.NoteEntityTypeCodeID = workNoteResult.NoteEntityTypeCodeID;
                viewModel.NoteEntityPrimaryKeyID = workNoteResult.EntityPrimaryKeyID;
                viewModel.NoteTypeCodeID = workNoteResult.NoteTypeCodeID;
                viewModel.NoteSubject = workNoteResult.NoteSubject;
                viewModel.NoteCaseID = workNoteResult.CaseID;
                viewModel.NotePetitionID = workNoteResult.PetitionID;
                viewModel.NoteHearingID = workNoteResult.HearingID;
                viewModel.NoteRecordStateID = workNoteResult.RecordStateID;
            }


            viewModel.WorkDescriptionList = UtilityFunctions.CodeGetByTypeIdAndUserId(66, "CodeMobileValue", viewModel.WorkDescriptionCodeID ?? 0, userShortValue: true, agencyId: _userManager.UserExtended.CaseNumberAgencyID);
            viewModel.WorkPhaseList = UtilityFunctions.CodeGetByTypeIdAndUserId(600, "CodeMobileValue", viewModel.WorkDescriptionCodeID ?? 0, userShortValue: true, agencyId: _userManager.UserExtended.CaseNumberAgencyID);

            viewModel.WorkedForList = _utilityService.ExecStoredProcedureWithResults<LALoDep.Domain.Mobile.pd_WorkRoleGetByWorkID_spResult>("pd_WorkRoleGetByWorkID_sp",
                                                        new LALoDep.Domain.Mobile.pd_WorkRoleGetByWorkID_spParams { CaseID = _userManager.UserExtended.CaseID, WorkID = viewModel.WorkID, UserID = _userManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                                        .Select(x => new WorkedRoleViewModel()
                                                        {
                                                            Selected = x.Selected == 1,
                                                            WorkRoleID = x.WorkRoleID,
                                                            RoleID = x.RoleID,
                                                            PersonName = x.PersonNameLast + ", " + x.PersonNameFirst,
                                                            AllMainPetitionsClosedFlag = x.AllMainPetitionsClosedFlag,
                                                            AllMainPetitionsDisplay = x.AllMainPetitionsDisplay
                                                        }).ToList();

            return View(viewModel);
        }

        public virtual JsonResult RecordTimeEditSave(RecordTimeEditViewModel viewModel)
        {
            //if any data changed then update.
            if (viewModel.IsWorkChanged)
            {
                // Update Work time
                var pd_WorkUpdate1_spParams = new pd_WorkUpdate1_spParams
                {
                    WorkID = viewModel.WorkID,
                    AgencyID = viewModel.AgencyID,
                    CaseID = _userManager.UserExtended.CaseID,
                    PersonID = viewModel.PersonID,
                    WorkHours = viewModel.WorkHours,
                    WorkHoursOverTime = viewModel.WorkHoursOverTime,
                    WorkMileage = viewModel.WorkMileage,
                    WorkDescriptionCodeID = viewModel.WorkDescriptionCodeID,
                    RecordStateID = viewModel.RecordStateID,
                    UserID = _userManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    WorkPhaseCodeID = viewModel.WorkPhaseCodeID,
                    HearingID = viewModel.HearingID.ToInt() < 0 ? -1 : viewModel.HearingID.ToInt(),
                    WorkIVeEligibleCodeID=-1
                };

                if (!string.IsNullOrEmpty(viewModel.WorkStartDate))
                    pd_WorkUpdate1_spParams.WorkStartDate = Convert.ToDateTime(viewModel.WorkStartDate);

                if (!string.IsNullOrEmpty(viewModel.WorkEndDate))
                    pd_WorkUpdate1_spParams.WorkEndDate = Convert.ToDateTime(viewModel.WorkEndDate);

                _utilityService.ExecStoredProcedureWithResults<object>("pd_WorkUpdate1_sp", pd_WorkUpdate1_spParams).FirstOrDefault();
            }

            if (viewModel.IsNoteChanged)
            {
                if (viewModel.NoteEntry.IsNullOrEmpty() && viewModel.NoteID.HasValue)
                {
                    UtilityFunctions.NoteDelete(viewModel.NoteID.Value);
                }
                else if (!viewModel.NoteEntry.IsNullOrEmpty() && viewModel.NoteID.HasValue)
                {
                    UtilityFunctions.NoteUpdate(viewModel.NoteID.Value, viewModel.NoteEntityCodeID.Value, viewModel.NoteEntityTypeCodeID.Value, viewModel.NoteEntityPrimaryKeyID.Value,
                        viewModel.NoteTypeCodeID.Value, viewModel.NoteSubject, viewModel.NoteEntry, viewModel.NoteHearingID, viewModel.NotePetitionID, viewModel.NoteAgencyID);
                }
                else if (!viewModel.NoteEntry.IsNullOrEmpty() && !viewModel.NoteID.HasValue)
                {
                    UtilityFunctions.NoteInsert(152, 123, viewModel.WorkID, 16, "Record Time Note", viewModel.NoteEntry, caseId: _userManager.UserExtended.CaseID);
                }
            }

            foreach (var item in viewModel.WorkedForList)
            {
                if (item.Selected)
                {
                    var workRoleID = _utilityService.ExecStoredProcedureWithResults<object>("pd_WorkRoleInsert_sp", new pd_WorkRoleInsert_spParams
                    {
                        WorkID = viewModel.WorkID,
                        RoleID = item.RoleID.Value,
                        RecordStateID = 1,
                        UserID = _userManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                    }).FirstOrDefault();
                }
                else
                {
                    _utilityService.ExecStoredProcedureWithResults<object>("pd_WorkRoleDelete_sp", new pd_WorkRoleDelete_spParams
                    {
                        ID = item.WorkRoleID.Value,
                        UserID = _userManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                    }).FirstOrDefault();
                }
            }

            return Json(new { isSuccess = true, URL = Url.Action(MVC.Mobile.Case.RecordTime()) });
        }
        #endregion

    }
}