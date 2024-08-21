using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Petition;
using LALoDep.Domain.qa;
using LALoDep.Domain.qcal;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
    public class AppearanceSheetViewModel
    {

        public int? GlobalResultID { get; set; }
        public IEnumerable<SelectListItem> GlobalResultList { get; set; }
        public int? ContinuanceRequestedByID { get; set; }
        public IEnumerable<SelectListItem> ContinuanceRequestedByList { get; set; }

        public IEnumerable<pd_PetitionGetAllByHearingID_spResult> PetitionByHearingList { get; set; }
        public IEnumerable<SelectListItem> PetitionType602List { get; set; }
        public IEnumerable<SelectListItem> HearingType602PetitionList { get; set; }
        public IEnumerable<SelectListItem> HearingResult602PetitionList { get; set; }
        public IEnumerable<SelectListItem> CourtDepartment602PetitionList { get; set; }
        public IEnumerable<SelectListItem> HearingResultContinuanceList { get; set; }
        public IEnumerable<SelectListItem> HearingTypeNonHearingEventList { get; set; }
        public IEnumerable<SelectListItem> HearingResultFutureHearingsList { get; set; }

        public IEnumerable<SelectListItem> HearingOfficerList { get; set; }
        public IEnumerable<SelectListItem> CounselList { get; set; }
        public IEnumerable<SelectListItem> CounselListDCC { get; set; }
        public IEnumerable<SelectListItem> NoteTypeList { get; set; }
        //public IEnumerable<qcal_AS_HearingAttendanceGet_spResult> HearingAttendance { get; set; }
        public IEnumerable<HearingAttendance> HearingAttendance { get; set; }

        public IEnumerable<qcal_AS_DCCGet_spResult> CurrentDCCList { get; set; }
        public IEnumerable<qcal_AS_HearingNoteGetList_spResult> NoteList { get; set; }
        public int NoteTypeCodeID { get; set; }
        public IEnumerable<SelectListItem> AppearingAttorneyList { get; set; }
        public IEnumerable<SelectListItem> HearingResultListForHoursNotRequiredValidation { get; set; }
        public IEnumerable<SelectListItem> HearingTypeHoursNotRequired { get; set; }
        public string AppearingAttorneyID
        {
            get; set;
        }
        public int HearingAttendanceID { get; set; }
        public int UpdateHearing { get; set; }
        public int HearingID { get; set; }
        public int OfficerPersonID { get; set; }
        public bool MediaPresentFlag { get; set; }
        public string CourtReporter { get; set; }
        public bool NoticeProperFlag { get; set; }
        public bool ReasonableEffortFlag { get; set; }
        public string GeneralHearingNote { get; set; }
        public string CourtOfficer { get; set; }
        public string CSW { get; set; }
        public int? CSWPresentFlag { get; set; }
        public string HearingDate { get; set; }

        public int DepartmentID { get; set; }
        public int HearingTypeID { get; set; }
        public string PetitionsSelectedIds { get; set; }

        //Client current home Address
        public IEnumerable<SelectListItem> IncarcerationFacilityList { get; set; }
        public qcal_AS_ClientAddressGet_spResult NewClientCurrentAddress { get; set; }
        public qcal_HearingNoteGetMain_spResult HearingNoteGetMain { get; set; }
        public List<qcal_HearingNoteGetPreviousNotes_spResult> HearingNoteGetPreviousNotes { get; set; }

        public bool AddressChanged { get; set; }
        public string NewAddressPhone { get; set; }
        public string NewAddressStreet { get; set; }
        public string NewAddressCity { get; set; }
        public int? NewAddressStateCodeID { get; set; }
        public string NewAddressZipCode { get; set; }
        public string MobilePhone { get; set; }
        public string WorkPhone { get; set; }
        public string EmailAddress { get; set; }
        public IEnumerable<SelectListItem> ContactPreferenceList { get; set; }
        public int? ContactPreferenceID { get; set; }
        public string ContactPreferenceComment { get; set; }

        public IEnumerable<CodeViewModel> StateList { get; set; }

        public IEnumerable<SelectListItem> Detention_PlacementList { get; set; }
        public IEnumerable<SelectListItem> AllegationFindingList { get; set; }


        public int? CanAddAPOFFE { get; set; }
        public bool APOFFE { get; set; }
        public qcal_AS_ClientAddressContactGet_spResult ClientAddressContactGetModel { get; set; }


        public bool IsForParent { get; set; }
        public decimal? Hours { get; set; }
       
        public int HourTypeID { get; set; }
        public IEnumerable<SelectListItem> HourTypeList { get; set; }
        public int PhaseID { get; set; }
        public int WorkID { get; set; }
        public int DataValidation_RequireHearingHoursFlag { get; set; }
        public string HoursNotRequiredBeforeHearingDate { get; set; }
        public bool IsHoursChanged { get; set; }
        public IEnumerable<SelectListItem> PhaseList { get; set; }
        public IEnumerable<CodeHierarchyGetByCodeRelationshipIDAgencyID_spResults> PhaseParentList { get; set; }

        public IEnumerable<CodeHierarchyGetByCodeRelationshipIDAgencyID_spResults> HourTypeParentList { get; set; }
        public string ControlType { get; set; }
        public IEnumerable<SelectListItem> CaseStatusList { get; set; }
        public bool IsCaseStatusExists { get; set; }

        public int? HearingPrepNoteID { get; set; }
        public string HearingPrepNoteEntry { get; set; }
        public bool HearingPrepNoteEntryChanged { get; set; }
        public string HearingPrepNoteTotalPrepHoursLink { get; set; }
        public decimal? HearingPrepNoteHours { get; set; }
        public int WorkHoursRequiredFlag { get; set; }
        public string HoursLabel { get; set; }
        public AppearanceSheetViewModel()
        {
            ClientAddressContactGetModel = new qcal_AS_ClientAddressContactGet_spResult();
            HearingOfficerList = new List<SelectListItem>();
            CaseStatusList = new List<SelectListItem>();
            NewClientCurrentAddress = new qcal_AS_ClientAddressGet_spResult();
            StateList = new List<CodeViewModel>();
            HearingAttendance = new List<HearingAttendance>();
            HearingContinuanceReasonGetForHearingList = new List<HearingContinuanceReasonGetForHearing_spResult>();
            HearingContinuanceRequestedByGetForHearingList = new List<HearingContinuanceRequestedByGetForHearing_spResult>();
            HearingContinuanceRequestedByIUDList = new List<HearingContinuanceRequestedByIUD_spParams>();
            HearingContinuanceReasonIUDList = new List<HearingContinuanceReasonIUD_spParams>();
        }
        public IEnumerable<HearingContinuanceRequestedByGetForHearing_spResult> HearingContinuanceRequestedByGetForHearingList { get; set; }

        public IEnumerable<HearingContinuanceReasonGetForHearing_spResult> HearingContinuanceReasonGetForHearingList { get; set; }


        public IEnumerable<HearingContinuanceRequestedByIUD_spParams> HearingContinuanceRequestedByIUDList { get; set; }

        public IEnumerable<HearingContinuanceReasonIUD_spParams> HearingContinuanceReasonIUDList { get; set; }
    }

    public class AppearanceSheetNotesViewModel
    {
        public string ControlType { get; set; }
        public IEnumerable<qcal_AS_PredeterminedAnswersGet_spResult> PredeterminedAnswers { get; set; }
        public IEnumerable<qcal_AS_NotePersonGetAll_spResult> NotePerson { get; set; }
        public int HearingID { get; set; }
        public int NoteID { get; set; }
        public string NoteText { get; set; }
        public int NoteTypeID { get; set; }
        public string NoteCodeTypeName { get; set; }
        public int? CanEditFlag { get; set; }
        public int? CanDeleteFlag { get; set; }

    }

    public class HearingAttendance : qcal_AS_HearingAttendanceGet_spResult
    {
        public bool UpdateFlags { get; set; }
        public bool UpdateHearingAttendenceRecord { get; set; }
        public bool UpdateCaseStatusRecord { get; set; }
        public bool UpdatePlacement { get; set; }
    }

    public class QuickAddAdultViewModel
    {
        public QuickAddAdultViewModel()
        {
            RaceList = new List<SelectListItem>();
            SexList = new List<SelectListItem>();
            RoleList = new List<SelectListItem>();
            PetitionList = new List<qa_PetitionGetForAdultPartyAdd_spResult>();
            AssociationList = new List<qa_AssociationCodes_spResult>();
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string BirthDate { get; set; }
        public int? RaceCodeID { get; set; }
        public IEnumerable<SelectListItem> RaceList { get; set; }
        public int? SexCodeID { get; set; }
        public IEnumerable<SelectListItem> SexList { get; set; }
        public int? RoleTypeID { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }

        public List<qa_PetitionGetForAdultPartyAdd_spResult> PetitionList { get; set; }
        public List<qa_AssociationCodes_spResult> AssociationList { get; set; }


         
    }

    public class QuickAddChildViewModel
    {

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string BirthDate { get; set; }
        public int? RaceCodeID { get; set; }
        public IEnumerable<SelectListItem> RaceList { get; set; }
        public int? SexCodeID { get; set; }
        public IEnumerable<SelectListItem> SexList { get; set; }
        public int? PetitionTypeCodeID { get; set; }
        public IEnumerable<SelectListItem> PetitionTypeList { get; set; }
        public string PetitionFileDate { get; set; }
        public string CaseNumber { get; set; }

        public int Allegation1 { get; set; }
        public int Allegation2 { get; set; }
        public int Allegation3 { get; set; }
        public int Allegation4 { get; set; }
        public int Allegation5 { get; set; }

        public IEnumerable<SelectListItem> AllegationList { get; set; }

        public List<qa_RoleGetForPetitionAdd_spResult> RoleList { get; set; }
        public List<qa_HearingGetForPetitionAdd_spResult> HearingList { get; set; }
        public List<qa_AssociationCodes_spResult> AssociationList { get; set; }
    }
}