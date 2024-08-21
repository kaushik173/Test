
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.ref_Referral
{
    public class ref_ReferralClientCategoryIUD_spParams
    {
        public string IUD { get; set; }
        public int? ReferralClientCategoryID { get; set; }
        public int? ReferralID { get; set; }
        public int? PersonClassificationCodeID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class ref_ReferralIUD_spParams
    {
        public string IUD { get; set; }
        public int? CaseID { get; set; }
        public int? CaseAgencyID { get; set; }
        public int? ReferralID { get; set; }
        public int? RoleID { get; set; }
        public int? ReferralRequestedByPersonID { get; set; }
        public int? ReferralRequestedForPersonID { get; set; }
        public int? ReferralTypeCodeID { get; set; }
        public DateTime? ReferralRequestDate { get; set; }
        public DateTime? ReferralDueDate { get; set; }
        public DateTime? ReferralEndDate { get; set; }
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
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

        public int? CountryOfOriginCodeID { get; set; }
        public int? UnaccompaniedChildCodeID { get; set; }
        public int? EOIRCodeID { get; set; }
        public int? StatusForAssignmentCodeID { get; set; }
        public int? SIJSFindingStatusCodeID { get; set; }
        public DateTime? SIJSFindingStatusDate { get; set; }
        public byte? CDSSReportingFlag { get; set; }
        public int? CDSSGrantYearQuarterID { get; set; }
        public int? CDSSCaseTypeCodeID { get; set; }
        public int? ANumberPersonID { get; set; }
        public int? ANumber_LegalnumberID { get; set; }
        public string ANumber { get; set; }

    }

    public class ref_ReferralIUD_spResult
    { 
        public int? ReferralID { get; set; }
        

    }

    
}