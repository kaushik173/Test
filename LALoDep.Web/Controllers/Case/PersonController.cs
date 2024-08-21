using LALoDep.Domain.AddEditCountyCounsel;
using LALoDep.Domain.CaseAttribute;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Medication;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.qcal;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Controllers
{

    public partial class CaseController
    {
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.EditPersonPage, PageSecurityItemID = SecurityToken.EditPerson)]
        public virtual ActionResult EditPerson(string id, string roleId)
        {
            int personId = id.ToDecrypt().ToInt();
            var viewModel = new EditPersonViewModel();
            var pd_PersonGet_spParams = new pd_PersonGet_spParams()
            {
                PersonID = personId,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var person = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", pd_PersonGet_spParams).FirstOrDefault();
            if (person != null)
            {
                viewModel.InjectFrom(person);
                if (person.DeceasedDate.HasValue)
                {
                    viewModel.IsDeceased = person.DeceasedDate.HasValue;
                    if (person.DeceasedDate.HasValue && person.DeceasedDate.Value > (new DateTime(1800, 1, 2)))
                    {
                        viewModel.DeceasedDate = person.DeceasedDate;
                    }
                    else
                    {
                        viewModel.DeceasedDate = null;
                    }
                }
                else
                {
                    viewModel.DeceasedDate = null;
                    viewModel.IsDeceased = false;
                }
            }

            var personNmaeInfo = UtilityService.ExecStoredProcedureWithResults<pd_PersonNameGet_spResult>("pd_PersonNameGet_sp", new pd_PersonNameGet_spParams()
            {
                PersonNameID = person.PersonNameID ?? 0,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).FirstOrDefault();

            if (personNmaeInfo != null)
            {
                viewModel.PersonNameID = personNmaeInfo.PersonNameID.Value;
                viewModel.PersonNameTypeCodeID = personNmaeInfo.PersonNameTypeCodeID;
                viewModel.PersonNameMiddle = personNmaeInfo.PersonNameMiddle;
                viewModel.PersonNameRecordStateID = personNmaeInfo.RecordStateID;
            }

            var personRoleParam = new pd_RoleGet_spParams()
            {
                RoleID = roleId.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            var personRole = UtilityService.ExecStoredProcedureWithResults<pd_RoleGet_spResult>("pd_RoleGet_sp", personRoleParam).FirstOrDefault();
            if (personRole != null)
            {
                viewModel.IsClient = personRole.RoleClient == 1;
                viewModel.RoleID = personRole.RoleID;
                viewModel.RoleStartDate = personRole.RoleStartDate;
                viewModel.RoleEndDate = personRole.RoleEndDate;
                viewModel.RoleTypeCodeID = personRole.RoleTypeCodeID;
                viewModel.RoleTypeList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetRoleTypesForPersonEdit_spResult>("pd_RoleGetRoleTypesForPersonEdit_sp", new pd_RoleGetRoleTypesForPersonEdit_spParams
                {
                    RoleID = personRole.RoleID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()

                }).Select(o => new SelectListItem { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();

                //if (viewModel.RoleTypeCodeID.HasValue && viewModel.RoleTypeCodeID.Value != 3)
                //{
                //    viewModel.RoleTypeList = UtilityFunctions.CodeGetBySystemValueTypeId(1);

                //}
                //else
                //{
                //    viewModel.RoleTypeList = new List<SelectListItem>() { new SelectListItem { Text = "Child", Value = "3" } };
                //}
            }

            viewModel.PersonRaceList = UtilityFunctions.CodeGetByTypeIdAndUserId(codeTypeId: 35, includeCodeId: person.PersonRaceCodeID ?? 0);
            viewModel.PersonSexList = UtilityFunctions.CodeGetByTypeIdAndUserId(codeTypeId: 1, includeCodeId: person.PersonSexCodeID ?? 0);
            viewModel.PersonLanguageList = UtilityFunctions.CodeGetByTypeIdAndUserId(codeTypeId: 53, includeCodeId: person.PersonLanguageCodeID ?? 0);
            viewModel.DesignatedDayList = UtilityFunctions.CodeGetBySystemValueTypeId(231);

            var caseAttrParams = new CaseAttributeGetByType_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                CaseID = UserManager.UserExtended.CaseID,
                CaseAttributeTypeID = 2000,
                TableID = personRoleParam.RoleID
            };

            var designatedDayAttr = UtilityService.ExecStoredProcedureWithResults<CaseAttributeGetByType_spResult<int>>("CaseAttributeGetByType_sp", caseAttrParams).FirstOrDefault();
            if (designatedDayAttr != null)
            {
                viewModel.DesignatedDayCaseAttrID = designatedDayAttr.CaseAttributeID;
                viewModel.DesignatedDayCodeID = designatedDayAttr.CaseAttributeGenericValue.ToInt();
            }

            caseAttrParams.CaseAttributeTypeID = 2010;
            var vocAttr = UtilityService.ExecStoredProcedureWithResults<CaseAttributeGetByType_spResult<string>>("CaseAttributeGetByType_sp", caseAttrParams).FirstOrDefault();
            if (vocAttr != null)
            {
                viewModel.VOCCaseAttrID = vocAttr.CaseAttributeID;
                viewModel.VOC = vocAttr.CaseAttributeGenericValue;
            }

            caseAttrParams.CaseAttributeTypeID = 2020;
            var vocStatusAttr = UtilityService.ExecStoredProcedureWithResults<CaseAttributeGetByType_spResult<int>>("CaseAttributeGetByType_sp", caseAttrParams).FirstOrDefault();
            if (vocStatusAttr != null)
            {
                viewModel.VOCStatusCaseAttrID = vocStatusAttr.CaseAttributeID;
                viewModel.VOCStatusCodeID = vocStatusAttr.CaseAttributeGenericValue.ToInt();
            }
            viewModel.VOCStatusList = UtilityFunctions.CodeGetByTypeIdAndUserId(codeTypeId: 960, sortOption: "CodeShortValue", includeCodeId: viewModel.VOCStatusCodeID ?? 0, agencyId: UserManager.UserExtended.CaseNumberAgencyID);

            var ClassificationParams = new pd_PersonClassificationGetByPersonID_spParams
            {
                PersonID = personId,
                SystemValueTypeID = 242,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.PersonRaceGetList = UtilityService.ExecStoredProcedureWithResults<PersonRace_GetList_spResult>("PersonRace_GetList_sp", new PersonRace_GetList_spParams
            {
                PersonID = personId,
                CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).ToList();
            var oPersonRaceVerbalValue = viewModel.PersonRaceGetList.FirstOrDefault(o => !string.IsNullOrEmpty(o.PersonRaceVerbalValue));
            if (oPersonRaceVerbalValue != null)
            {
                viewModel.PersonRaceVerbalValue = oPersonRaceVerbalValue.PersonRaceVerbalValue;
            }
            viewModel.PersonClassifications = UtilityService.ExecStoredProcedureWithResults<pd_PersonClassificationGetByPersonID_spResult>("pd_PersonClassificationGetByPersonID_sp", ClassificationParams)
                                                    .Select(c => new PersonClassificationViewModel
                                                    {
                                                        PersonClassificationID = c.PersonClassificationID,
                                                        PersonClassificationCodeID = c.PersonClassificationCodeID,
                                                        PersonClassificationStartDate = c.PersonClassificationStartDate,
                                                        PersonClassificationEndDate = c.PersonClassificationEndDate,
                                                        PersonClassificationEndReasonCodeID = c.PersonClassificationEndReasonCodeID,
                                                        RecordStateID = c.RecordStateID
                                                    }).ToList();

            viewModel.PersonClassificationsClientStatus = UtilityService.ExecStoredProcedureWithResults<pd_PersonClassificationGetByPersonIDClientStatus_spResult>("pd_PersonClassificationGetByPersonIDClientStatus_sp", new pd_PersonClassificationGetByPersonIDClientStatus_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                PersonID = personId
            }).Select(c => new PersonClassificationViewModel
            {
                PersonClassificationID = c.PersonClassificationID,
                PersonClassificationCodeID = c.PersonClassificationCodeID,
                PersonClassificationStartDate = c.PersonClassificationStartDate,
                PersonClassificationEndDate = c.PersonClassificationEndDate,
                PersonClassificationEndReasonCodeID = c.PersonClassificationEndReasonCodeID,
                RecordStateID = c.RecordStateID,
                CanEditFlag = c.CanEditFlag.HasValue ? c.CanEditFlag.Value : 0
            }).ToList();

            viewModel.PersonClassificationList = UtilityService.ExecStoredProcedureWithResults<pd_PersonClassificationGetCodes_spResult>(
                    "pd_PersonClassificationGetCodes_sp",
                    new pd_PersonClassificationGetCodes_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        CodeTypeEnum = "Classification",
                        PersonID = personId,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();
            viewModel.ClassificationEndReasonList = UtilityService.ExecStoredProcedureWithResults<pd_PersonClassificationGetCodes_spResult>(
                    "pd_PersonClassificationGetCodes_sp",
                    new pd_PersonClassificationGetCodes_spParams()
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                        CodeTypeEnum = "EndReason",
                        PersonID = personId,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();


            viewModel.CaseStatusList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetClientStatus_spResult>(
                "pd_CodeGetClientStatus_sp",
                new pd_CodeGetClientStatus_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,

                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();

            var pd_MedicationParams = new pd_MedicationGetByPersonID_spParams
            {
                PersonID = personId,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            viewModel.PersionMedications = UtilityService.ExecStoredProcedureWithResults<pd_MedicationGetByPersonID_spResult>("pd_MedicationGetByPersonID_sp", pd_MedicationParams)
                                                .Select(m => new PersionMedicationViewModel
                                                {
                                                    MedicationID = m.MedicationID,
                                                    MedicationCodeID = m.MedicationCodeID,
                                                    MedicationDosage = m.MedicationDosage,
                                                    MedicationFrequencyCodeID = m.MedicationFrequencyCodeID,
                                                    MedicationStartDate = m.MedicationStartDate,
                                                    MedicationEndDate = m.MedicationEndDate,
                                                    RecordStateID = m.RecordStateID

                                                }).ToList();

            viewModel.MedicationList = UtilityFunctions.CodeGetByTypeIdAndUserId(87);
            viewModel.MedicationFrequencyList = UtilityFunctions.CodeGetBySystemValueTypeId(243);
            var caseDefault =
       UtilityService.ExecStoredProcedureWithResults<pd_CaseGetDefaults_spResults>("pd_CaseGetDefaults_sp",
           new pd_CaseGetDefaults_spParams
           {
               UserID = UserManager.UserExtended.UserID,
               BatchLogJobID = Guid.NewGuid(),
               CaseID = UserManager.UserExtended.CaseID
           }).FirstOrDefault();
            viewModel.DOBRequiredForChildren = caseDefault.DOBRequiredForChildren.ToInt();
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult EditPerson(EditPersonViewModel viewModel)
        {
            #region Update Person
            if (viewModel.UpdatePerson) //Update Persion
            {
                var pd_PersonUpdateParams = new pd_PersonUpdate_spParams
                {
                    PersonID = viewModel.PersonID,
                    AgencyID = viewModel.AgencyID,
                    PersonDOB = viewModel.PersonDOB,
                    PersonRaceCodeID = viewModel.PersonRaceCodeID,
                    PersonSexCodeID = viewModel.PersonSexCodeID,
                    RecordStateID = viewModel.RecordStateID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    PersonLanguageCodeID = viewModel.PersonLanguageCodeID,
                };
                UtilityService.ExecStoredProcedureWithoutResults("pd_PersonUpdate_sp", pd_PersonUpdateParams);
            }
            #endregion

            #region Update Person Name 
            if (viewModel.UpdatePersonName)//Update Persion Name
            {
                UtilityService.ExecStoredProcedureWithoutResults("pd_PersonNameUpdate_sp", new pd_PersonNameUpdate_spParams()
                {
                    PersonNameID = viewModel.PersonNameID ?? 0,
                    AgencyID = viewModel.AgencyID,
                    PersonID = viewModel.PersonID,
                    PersonNameFirst = viewModel.FirstName,
                    PersonNameLast = viewModel.LastName,
                    PersonNameMiddle = null,
                    PersonNameTypeCodeID = 3200, //Needtochange
                    PersonNameSoundex = null,
                    RecordStateID = (viewModel.PersonNameRecordStateID ?? 1),
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = new Guid(),
                    PersonNameSalutationCodeID = -1,
                    PersonNameSuffixCodeID = -1
                });
            }
            #endregion

            #region Update Role
            if (viewModel.UpdateRole)//Update Role
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("pd_RoleUpdate_sp", new pd_RoleUpdate_spParams
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    AgencyID = viewModel.AgencyID,
                    BatchLogJobID = Guid.NewGuid(),
                    PersonID = viewModel.PersonID,
                    RecordStateID = 1,
                    RoleClient = (byte)(viewModel.IsClient ? 1 : 0),
                    RoleStartDate = viewModel.RoleStartDate,
                    RoleEndDate = viewModel.RoleEndDate,
                    RoleID = viewModel.RoleID.Value,
                    RoleTypeCodeID = viewModel.RoleTypeCodeID.Value,
                    UserID = UserManager.UserExtended.UserID
                });
            }
            #endregion

            #region Update Deceased
            if (viewModel.UpdateDeceased)
            {
                if (viewModel.DeceasedDate_PersonClassID.HasValue)
                {
                    UtilityService.ExecStoredProcedureWithoutResults("pd_PersonClassificationDelete_sp", new pd_PersonClassificationDelete_spParams
                    {
                        ID = viewModel.DeceasedDate_PersonClassID.Value,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        LoadOption = "PersonClassification",
                        RecordStateID = 10
                    });
                }

                if (viewModel.IsDeceased)
                {
                    var deceasedInsert = new pd_PersonClassificationInsert_spParams()
                    {
                        PersonID = viewModel.PersonID,
                        PersonClassificationCodeID = 22860,
                        RecordStateID = 1,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                    };

                    if (viewModel.DeceasedDate.HasValue)
                        deceasedInsert.PersonClassificationStartDate = viewModel.DeceasedDate;
                    else
                        deceasedInsert.PersonClassificationStartDate = new DateTime(1800, 1, 1);

                    viewModel.DeceasedDate_PersonClassID = UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonClassificationInsert_sp", deceasedInsert).FirstOrDefault();
                }
            }

            #endregion

            #region Designated Day
            if (viewModel.DesignatedDayCaseAttrID.HasValue && viewModel.DesignatedDayCodeID.HasValue)
            {
                UtilityService.ExecStoredProcedureWithoutResults("CaseAttributeUpdate_sp", new CaseAttributeUpdate_spParams()
                {
                    CaseAttributeID = viewModel.DesignatedDayCaseAttrID.Value,
                    CaseID = UserManager.UserExtended.CaseID,
                    CaseAttributeTypeID = 2000,
                    CaseAttributeGenericValue = viewModel.DesignatedDayCodeID.ToString(),
                    TableID = viewModel.RoleID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });
            }
            else if (!viewModel.DesignatedDayCaseAttrID.HasValue && viewModel.DesignatedDayCodeID.HasValue)
            {
                viewModel.DesignatedDayCaseAttrID = UtilityService.ExecStoredProcedureWithResults<int>("CaseAttributeInsert_sp", new CaseAttributeInsert_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    CaseAttributeTypeID = 2000,
                    CaseAttributeGenericValue = viewModel.DesignatedDayCodeID.ToString(),
                    TableID = viewModel.RoleID.Value,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    RecordStateID = 1

                }).FirstOrDefault();

            }
            else if (viewModel.DesignatedDayCaseAttrID.HasValue && !viewModel.DesignatedDayCodeID.HasValue)
            {
                UtilityService.ExecStoredProcedureWithoutResults("CaseAttributeDelete_sp", new CaseAttributeDelete_spParams()
                {
                    ID = viewModel.DesignatedDayCaseAttrID.Value,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });
            }
            #endregion

            #region Voc #
            if (viewModel.VOCCaseAttrID.HasValue && !string.IsNullOrEmpty(viewModel.VOC))
            {
                UtilityService.ExecStoredProcedureWithoutResults("CaseAttributeUpdate_sp", new CaseAttributeUpdate_spParams()
                {
                    CaseAttributeID = viewModel.VOCCaseAttrID.Value,
                    CaseID = UserManager.UserExtended.CaseID,
                    CaseAttributeTypeID = 2010,
                    CaseAttributeGenericValue = viewModel.VOC,
                    TableID = viewModel.RoleID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });
            }
            else if (!viewModel.VOCCaseAttrID.HasValue && !string.IsNullOrEmpty(viewModel.VOC))
            {
                viewModel.VOCCaseAttrID = UtilityService.ExecStoredProcedureWithResults<int>("CaseAttributeInsert_sp", new CaseAttributeInsert_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    CaseAttributeTypeID = 2010,
                    CaseAttributeGenericValue = viewModel.VOC,
                    TableID = viewModel.RoleID.Value,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    RecordStateID = 1
                }).FirstOrDefault();

            }
            else if (viewModel.VOCCaseAttrID.HasValue && string.IsNullOrEmpty(viewModel.VOC))
            {
                UtilityService.ExecStoredProcedureWithoutResults("CaseAttributeDelete_sp", new CaseAttributeDelete_spParams()
                {
                    ID = viewModel.VOCCaseAttrID.Value,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });
            }
            #endregion

            #region Voc Status
            if (viewModel.VOCStatusCaseAttrID.HasValue && viewModel.VOCStatusCodeID.HasValue)
            {
                UtilityService.ExecStoredProcedureWithoutResults("CaseAttributeUpdate_sp", new CaseAttributeUpdate_spParams()
                {
                    CaseAttributeID = viewModel.VOCStatusCaseAttrID.Value,
                    CaseID = UserManager.UserExtended.CaseID,
                    CaseAttributeTypeID = 2020,
                    CaseAttributeGenericValue = viewModel.VOCStatusCodeID.ToString(),
                    TableID = viewModel.RoleID,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });
            }
            else if (!viewModel.VOCStatusCaseAttrID.HasValue && viewModel.VOCStatusCodeID.HasValue)
            {
                viewModel.DesignatedDayCaseAttrID = UtilityService.ExecStoredProcedureWithResults<int>("CaseAttributeInsert_sp", new CaseAttributeInsert_spParams()
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    CaseAttributeTypeID = 2020,
                    CaseAttributeGenericValue = viewModel.VOCStatusCodeID.ToString(),
                    TableID = viewModel.RoleID.Value,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,
                    RecordStateID = 1
                }).FirstOrDefault();

            }
            else if (viewModel.VOCStatusCaseAttrID.HasValue && !viewModel.VOCStatusCodeID.HasValue)
            {
                UtilityService.ExecStoredProcedureWithoutResults("CaseAttributeDelete_sp", new CaseAttributeDelete_spParams()
                {
                    ID = viewModel.VOCStatusCaseAttrID.Value,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });
            }
            #endregion

            #region Persion  Classifications
            if (viewModel.PersonClassifications != null && viewModel.PersonClassifications.Any())
            {
                foreach (var classification in viewModel.PersonClassifications)
                {
                    if (!classification.PersonClassificationID.HasValue)  // Add new 
                    {
                        var ClassificationInsert = new pd_PersonClassificationInsert_spParams()
                        {
                            PersonID = viewModel.PersonID,
                            PersonClassificationCodeID = classification.PersonClassificationCodeID,
                            PersonClassificationEndReasonCodeID = classification.PersonClassificationEndReasonCodeID,
                            RecordStateID = 1,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                        };

                        if (!string.IsNullOrEmpty(classification.PersonClassificationStartDate))
                            ClassificationInsert.PersonClassificationStartDate = Convert.ToDateTime(classification.PersonClassificationStartDate);

                        if (!string.IsNullOrEmpty(classification.PersonClassificationEndDate))
                            ClassificationInsert.PersonClassificationEndDate = Convert.ToDateTime(classification.PersonClassificationEndDate);

                        classification.PersonClassificationID = UtilityService.ExecStoredProcedureWithResults<int>("pd_PersonClassificationInsert_sp", ClassificationInsert).FirstOrDefault();
                    }
                    else if (classification.PersonClassificationID.HasValue && !classification.DoDelete) //Update existing
                    {
                        var ClassificationUpdate = new pd_PersonClassificationUpdate_spParams()
                        {
                            PersonClassificationID = classification.PersonClassificationID,
                            PersonID = viewModel.PersonID,
                            PersonClassificationCodeID = classification.PersonClassificationCodeID,
                            PersonClassificationEndReasonCodeID = classification.PersonClassificationEndReasonCodeID,
                            RecordStateID = classification.RecordStateID,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                        };

                        if (!string.IsNullOrEmpty(classification.PersonClassificationStartDate))
                            ClassificationUpdate.PersonClassificationStartDate = Convert.ToDateTime(classification.PersonClassificationStartDate);

                        if (!string.IsNullOrEmpty(classification.PersonClassificationEndDate))
                            ClassificationUpdate.PersonClassificationEndDate = Convert.ToDateTime(classification.PersonClassificationEndDate);

                        UtilityService.ExecStoredProcedureWithoutResults("pd_PersonClassificationUpdate_sp", ClassificationUpdate);
                    }
                    else if (classification.DoDelete && classification.PersonClassificationID.HasValue) //delete existing
                    {
                        UtilityService.ExecStoredProcedureWithoutResults("pd_PersonClassificationDelete_sp", new pd_PersonClassificationDelete_spParams
                        {
                            ID = classification.PersonClassificationID.Value,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                            LoadOption = "PersonClassification",
                            RecordStateID = 10
                        });
                    }
                }
            }

            #endregion

            #region Persion Medications
            if (viewModel.PersionMedications != null && viewModel.PersionMedications.Any())
            {
                foreach (var medication in viewModel.PersionMedications)
                {
                    if (!medication.MedicationID.HasValue) // Add new 
                    {
                        var MedicationInsert = new pd_MedicationInsert_spParams
                        {
                            PersonID = viewModel.PersonID,
                            MedicationCodeID = medication.MedicationCodeID,
                            MedicationDosage = medication.MedicationDosage,
                            MedicationFrequencyCodeID = medication.MedicationFrequencyCodeID,
                            RecordStateID = 1,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                        };

                        if (!string.IsNullOrEmpty(medication.MedicationStartDate))
                            MedicationInsert.MedicationStartDate = Convert.ToDateTime(medication.MedicationStartDate);

                        if (!string.IsNullOrEmpty(medication.MedicationEndDate))
                            MedicationInsert.MedicationEndDate = Convert.ToDateTime(medication.MedicationEndDate);

                        medication.MedicationID = UtilityService.ExecStoredProcedureWithResults<int>("pd_MedicationInsert_sp", MedicationInsert).FirstOrDefault();
                    }
                    else if (medication.MedicationID.HasValue && !medication.DoDelete) // Update existing
                    {
                        var MedicationUpdate = new pd_MedicationUpdate_spParams
                        {
                            MedicationID = medication.MedicationID.Value,
                            PersonID = viewModel.PersonID,
                            MedicationCodeID = medication.MedicationCodeID,
                            MedicationDosage = medication.MedicationDosage,
                            MedicationFrequencyCodeID = medication.MedicationFrequencyCodeID,
                            RecordStateID = medication.RecordStateID,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                        };

                        if (!string.IsNullOrEmpty(medication.MedicationStartDate))
                            MedicationUpdate.MedicationStartDate = Convert.ToDateTime(medication.MedicationStartDate);

                        if (!string.IsNullOrEmpty(medication.MedicationEndDate))
                            MedicationUpdate.MedicationEndDate = Convert.ToDateTime(medication.MedicationEndDate);

                        UtilityService.ExecStoredProcedureWithoutResults("pd_MedicationUpdate_sp", MedicationUpdate);
                    }
                    else if (medication.MedicationID.HasValue && medication.DoDelete) // Delete existing
                    {
                        UtilityService.ExecStoredProcedureWithoutResults("pd_MedicationDelete_sp", new pd_MedicationDelete_spParams
                        {
                            ID = medication.MedicationID.Value,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                            LoadOption = "Medication",
                            RecordStateID = 10
                        });
                    }
                }
            }

            #endregion
            #region Persion Medications
            if (viewModel.PersonRaceIUDList != null && viewModel.PersonRaceIUDList.Any())
            {
                foreach (var item in viewModel.PersonRaceIUDList)
                {
                    item.UserID = UserManager.UserExtended.UserID;
                    item.BatchLogJobID = Guid.NewGuid();
                    item.CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID;
                    UtilityService.ExecStoredProcedureWithoutResults("PersonRaceIUD_sp", item);
                }
            }
            #endregion




            var url = Url.Action(MVC.Case.EditPerson(viewModel.PersonID.ToEncrypt(), viewModel.RoleID.ToEncrypt()));
            if (viewModel.ButtonId == 2)
            {
                url = Url.Action(MVC.Case.Main(UserManager.UserExtended.CaseID.ToEncrypt()));
            }
            return Json(new { isSuccess = true, URL = url });
        }
    }
}