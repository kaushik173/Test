using System.Collections.Generic;
using System.Web.Mvc;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Petition;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Work;
using LALoDep.Domain.pd_Code;
using LALoDep.Models.Case;

namespace LALoDep.Models.CaseOpening
{
    public class HearingModel
    {
        public List<OrderedToAppearModel> OrderedToAppearModelList { get; set; }
        public int WorkHasInvoiceFlag { get; set; }
        public int OriginalHearingResulted { get; set; }
        public int DataValidation_RequireJudgeFlag { get; set; }
        public string HoursNotRequiredBeforeHearingDate { get; set; }

        public int HearingID { get; set; }

        public int HearingTypeID { get; set; }
        public IEnumerable<SelectListItem> HearingTypeList { get; set; }
        public string HearingDate { get; set; }
        public string HearingTime { get; set; }

        public string DefaultHearingTime { get; set; }
        public string DefaultHearingTimeContested { get; set; }

        public int HearingOfficerID { get; set; }
        public IEnumerable<SelectListItem> HearingOfficerList { get; set; }
        public int DepartmentID { get; set; }
        public int CaseDepartmentID { get; set; }

        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public string AppearingAttorneyID { get; set; }
        public int HearingAttendanceID { get; set; }
        public IEnumerable<SelectListItem> AppearingAttorneyList { get; set; }
        public decimal? Hours { get; set; }
        public int HourTypeID { get; set; }
        public IEnumerable<SelectListItem> HourTypeList { get; set; }
        public int PhaseID { get; set; }
        public int WorkID { get; set; }
        public IEnumerable<SelectListItem> PhaseList { get; set; }

        public IEnumerable<CodeHierarchyGetByCodeRelationshipIDAgencyID_spResults> PhaseParentList { get; set; }

        public IEnumerable<CodeHierarchyGetByCodeRelationshipIDAgencyID_spResults> HourTypeParentList { get; set; }


        public bool MediaPresent { get; set; }
        public string Note { get; set; }
        public int? NoteID { get; set; }
        public string PetitionsSelectedIds { get; set; }
        public bool UpdateDepartment { get; set; }


        public int? GlobalResultID { get; set; }
        public IEnumerable<SelectListItem> GlobalResultList { get; set; }
        public int? ContinuanceRequestedByID { get; set; }
        public IEnumerable<SelectListItem> ContinuanceRequestedByList { get; set; }




        public IEnumerable<pd_PetitionGetByCaseID_spResult> PetitionList { get; set; }
        public IEnumerable<pd_PetitionGetAllByHearingID_spResult> PetitionByHearingList { get; set; }
        public IEnumerable<pd_HearingGetByCaseID_spResults> HearingList { get; set; }
        public IEnumerable<pd_WorkGetByCaseID_spResult> WorkList { get; set; }
        public IEnumerable<SelectListItem> HearingTypeContestedList { get; set; }
        public IEnumerable<SelectListItem> PetitionType602List { get; set; }
        public IEnumerable<SelectListItem> HearingType602PetitionList { get; set; }
        public IEnumerable<SelectListItem> HearingResult602PetitionList { get; set; }
        public IEnumerable<SelectListItem> CourtDepartment602PetitionList { get; set; }
        public IEnumerable<SelectListItem> HearingResultContinuanceList { get; set; }
        public IEnumerable<SelectListItem> HearingTypeNonHearingEventList { get; set; }
        public IEnumerable<SelectListItem> HearingResultFutureHearingsList { get; set; }
        public IEnumerable<SelectListItem> HearingResultListForHoursNotRequiredValidation { get; set; }
        public IEnumerable<SelectListItem> HearingTypeHoursNotRequired{ get; set; }
        public List<HearingAttendanceListViewModel> HearingAttendance { get; set; }
        public List<HearingAttendanceListViewModel> ClientStatusList { get; set; }
        public int buttonId { get; set; }
        public string ControlType { get; set; }
        public IEnumerable<SelectListItem> CaseStatusList { get; set; }
        public bool IsCaseStatusExists { get; set; }
        public byte? DataValidation_RequireHearingHoursFlag { get; set; }
        public string HoursLabel { get; set; }
        public int WorkHoursRequiredFlag { get; set; }
        public int HideWorkHoursOnHearingPages { get; set; }

        public IEnumerable<HearingContinuanceRequestedByGetForHearing_spResult> HearingContinuanceRequestedByGetForHearingList { get; set; }

        public IEnumerable<HearingContinuanceReasonGetForHearing_spResult> HearingContinuanceReasonGetForHearingList { get; set; }



        public IEnumerable<HearingContinuanceRequestedByIUD_spParams> HearingContinuanceRequestedByIUDList { get; set; }

        public IEnumerable<HearingContinuanceReasonIUD_spParams> HearingContinuanceReasonIUDList { get; set; }


        public HearingModel()
        {
            ClientStatusList = new List<HearingAttendanceListViewModel>();
            CaseStatusList = new List<SelectListItem>();
            WorkList = new List<pd_WorkGetByCaseID_spResult>();
            HearingList = new List<pd_HearingGetByCaseID_spResults>();
            PetitionList = new List<pd_PetitionGetByCaseID_spResult>();
            ContinuanceRequestedByList = new List<SelectListItem>();
            GlobalResultList = new List<SelectListItem>();
            PhaseList = new List<SelectListItem>();
            HourTypeList = new List<SelectListItem>();
            AppearingAttorneyList = new List<SelectListItem>();
            DepartmentList = new List<SelectListItem>();
            HearingTypeList = new List<SelectListItem>();
            PetitionByHearingList = new List<pd_PetitionGetAllByHearingID_spResult>();
            OrderedToAppearModelList = new List<OrderedToAppearModel>();
            HearingAttendance = new List<HearingAttendanceListViewModel>();
            HearingTypeHoursNotRequired= new List<SelectListItem>();
            HearingContinuanceReasonGetForHearingList = new List<HearingContinuanceReasonGetForHearing_spResult>();
            HearingContinuanceRequestedByGetForHearingList = new List<HearingContinuanceRequestedByGetForHearing_spResult>();
            HearingContinuanceRequestedByIUDList = new List<HearingContinuanceRequestedByIUD_spParams>();
            HearingContinuanceReasonIUDList = new List<HearingContinuanceReasonIUD_spParams>();
        }

    }

    public class OrderedToAppearModel
    {
        public int? HA_ID { get; set; }
        public int? HA_RoleID { get; set; }
        public int? HA_AttendedFlag { get; set; }
        public int? HA_CounselPersonID { get; set; }
        public int? HA_FillinCounselPersonID { get; set; }
        public string HA_Placement { get; set; }
        public int? HA_AppearanceRequiredFlag { get; set; }
    }
}