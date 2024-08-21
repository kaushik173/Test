using LALoDep.Domain.JcatsMessage;
using LALoDep.Domain.ref_Referral;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class ReferralViewModel
    {
        public string ReferralPersonId { get; set; }
        public IEnumerable<SelectListItem> ReferralPersonList { get; set; }

        public string ReferralTypeId { get; set; }
        public IEnumerable<SelectListItem> ReferralTypeList { get; set; }

        public List<ref_ReferralList_spResult> ReferralList { get; set; }
    }
    public class ReferralActivitySheetViewModel
    {

        public IEnumerable<LALoDep.Domain.pd_Work.pd_WorkGetByCaseID_spResult> WorkList { get; set; }
        public IEnumerable<SelectListItem> WorkGetFilterByStaffTypeList { get; set; }
        public string FilterByStaffType { get; set; }
        public ReferralActivitySheetViewModel()
        {
            WorkGetFilterByStaffTypeList = new List<SelectListItem>();
        }

    }
    public class ReferralAddEditStepsViewModel
    {

        public ref_ReferralGet_spResult ReferralModel { get; set; }



        public ref_ReferralHeader_spResult ReferralHeader { get; set; }



    }


    public class ReferralAddEditViewModel
    {
        public ReferralAddEditViewModel()
        {
            ReferralClientCategoryList = new List<ReferralClientCategoryListViewModel>();
            ReferralReliefGetByReferralList = new List<ReferralReliefGetByReferralViewModel>();
            ReferralImmigrationGetByReferralList = new List<ReferralImmigrationGetByReferralViewModel>();

            ReferralStatusGetHistoryList = new List<ref_ReferralStatusGetHistory_spResult>();
            ReferralStatusList = new List<SelectListItem>();

             
        }
        public IEnumerable<SelectListItem> ReferralRequestedByList { get; set; }

        public IEnumerable<SelectListItem> ReferralRequestedForList { get; set; }

        public IEnumerable<SelectListItem> ReferralFrequencyOfUpdatesList { get; set; }
        public IEnumerable<SelectListItem> ReferralEducationalStatusList { get; set; }
        public IEnumerable<SelectListItem> ReferralProgramEligibilityList { get; set; }
        public IEnumerable<ref_ReferralContactInfo_spResult> ReferralContactInfoList { get; set; }
        public IEnumerable<ReferralClientCategoryListViewModel> ReferralClientCategoryList { get; set; }
        public IEnumerable<SelectListItem> ImmigrationAttorneyList { get; set; }




        public List<ReferralImmigrationGetByReferralViewModel> ReferralImmigrationGetByReferralList { get; set; }
        public IEnumerable<ref_ReferralGet602Petitions_spResult> ReferralGet602PetitionsList { get; set; }
        public List<ReferralReliefGetByReferralViewModel> ReferralReliefGetByReferralList { get; set; }

        public IEnumerable<SelectListItem> ReferralUrgencyCodeList { get; set; }


        public ref_ReferralHeader_spResult ReferralHeader { get; set; }

        public int? ReferralID { get; set; }
        public int? AgencyID { get; set; }
        public int? RoleID { get; set; }
        public int? ReferralRequestedByPersonID { get; set; }
        public int? ReferralRequestedForPersonID { get; set; }
        public int? ReferralTypeCodeID { get; set; }
        public System.Nullable<System.DateTime> ReferralRequestDate { get; set; }
        public System.Nullable<System.DateTime> ReferralDueDate { get; set; }
        public System.Nullable<System.DateTime> ReferralEndDate { get; set; }
        public byte? ReferralCompanionCaseFlag { get; set; }
        public byte? ReferralConflictHistoryFlag { get; set; }
        public byte? ReferralHasChildrenFlag { get; set; }
        public byte? ReferralHaveLatestCourtReportFlag { get; set; }
        public byte? ReferralYouthHasWorkingPhoneFlag { get; set; }
        public int? ReferralUrgencyCodeID { get; set; }
        public byte? ReferralHasActiveCourtCaseFlag { get; set; }
        public byte? ReferralEIPFlag { get; set; }
        public byte? ReferralEIPMostRecentFlag { get; set; }
        public int? ReferralFrequencyOfUpdatesCodeID { get; set; }
        public int? ReferralEducationalStatusCodeID { get; set; }
        public string ReferralSchoolPreference { get; set; }
        public int? ReferralProgramEligibilityCodeID { get; set; }
        public string SpecialEducationNote { get; set; }
        public string SchoolProblemsNote { get; set; }
        public string ExpulsionNote { get; set; }
        public string EducationUnitOfServiceNote { get; set; }
        public string RelationshipsWithOtherClientsNote { get; set; }
        public string ReasonForReferralNote { get; set; }
        public string IssuesAndDependencyStatusSummaryNote { get; set; }
        public string ReferralReasonSummaryNote { get; set; }
        public string ReferralInternalNote { get; set; }
        public string DelinquencyCaseNote { get; set; }
        public byte? RecordStateID { get; set; }
        public int? CaseID { get; set; }




        public IEnumerable<SelectListItem> CountryofOriginList { get; set; }
        public IEnumerable<SelectListItem> UnaccompaniedChildList { get; set; }
        public IEnumerable<SelectListItem> EOIRList { get; set; }
        public IEnumerable<SelectListItem> StatusForAssignmentList { get; set; }
        public IEnumerable<SelectListItem> E317List { get; set; }

        public IEnumerable<SelectListItem> SIJSFindingsStatusList { get; set; }
        public IEnumerable<SelectListItem> CDSSGrantYearQuarterList { get; set; }
        public IEnumerable<SelectListItem> CDSSCaseTypeList { get; set; }



        public int? CountryOfOriginCodeID { get; set; }
        public int? UnaccompaniedChildCodeID { get; set; }
        public int? EOIRCodeID { get; set; }
        public int? StatusForAssignmentCodeID { get; set; }
        public int? SIJSFindingStatusCodeID { get; set; }
        public System.Nullable<System.DateTime> SIJSFindingStatusDate { get; set; }
        public byte? CDSSReportingFlag { get; set; }
        public int? CDSSGrantYearQuarterID { get; set; }
        public int? CDSSCaseTypeCodeID { get; set; }
        public int? ANumber_PersonID { get; set; }
        public int? ANumber_LegalNumberID { get; set; }
        public string ANumber { get; set; }
        public List<ref_ReferralStatusGetHistory_spResult> ReferralStatusGetHistoryList { get; set; }

        public IEnumerable<SelectListItem> ReferralStatusList { get; set; }




    }



    public class ReferralClientCategoryListViewModel
    {

        public string CodeDisplay { get; set; }
        public int? CodeID { get; set; }
        public int? ReferralClientCategoryID { get; set; }

        //this for model page
        public bool IsChecked { get; set; }
    }



    public class ReferralImmigrationGetByReferralViewModel
    {

        public int? ReferralImmigrationID { get; set; }
        public int? ReferralID { get; set; }
        public int? ImmigrationAgencyCodeID { get; set; }
        public int? ImmigrationAttorneyPersonID { get; set; }
        public int? Immigration317eCodeID { get; set; }


        public System.Nullable<System.DateTime> ImmigrationStartDate { get; set; }
        public System.Nullable<System.DateTime> ImmigrationEndDate { get; set; }
        public string SortDate { get; set; }
        public IEnumerable<SelectListItem> ImmigrationAgencyList { get; set; }
    }
    public class ReferralReliefGetByReferralViewModel
    {

        public int? ReferralReliefID { get; set; }
        public int? ReferralID { get; set; }
        public int? ReferralReliefTypeCodeID { get; set; }
        public int? ReferralReliefStatusCodeID { get; set; }
        public string ReferralReliefStatusDate { get; set; }
        public string ReferralReliefNote { get; set; }
        public string SortDate { get; set; }
        public string ReferralReliefPriorityDate { get; set; }
        public IEnumerable<SelectListItem> ReliefTypeList { get; set; }
        public IEnumerable<SelectListItem> ReliefStatusList { get; set; }

    }



    public class ReferralEventViewModel : ref_ReferralEventGet_spResult
    {
        public string ControlType { get; set; }

        public List<SelectListItem> EventTypeList { get; set; }
        public List<SelectListItem> LocationList { get; set; }
        public List<SelectListItem> AppearingAttorneyList { get; set; }
        public List<SelectListItem> ClientPresentList { get; set; }
        public List<ref_ReferralEventGetHistory_spResult> EventHistoryList { get; set; }

    }

    public class ReferralAddEditCodePopupViewModel
    {
        public string RecordType { get; set; }

        public string ControlID { get; set; }


        public string Value { get; set; }
        public string ShortValue { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}