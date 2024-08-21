using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Case
{
    public class pd_HearingGetByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int UnresultedFlag { get; set; }
    }

    public class pd_HearingGetByCaseID_spResult
    {
        public int HearingID { get; set; }
        public int AgencyID { get; set; }
        public int CaseID { get; set; }
        public int? HearingTypeCodeID { get; set; }
        public System.DateTime? HearingDateTime { get; set; }
        public int? HearingOfficerPersonID { get; set; }
        public int? HearingCourtDepartmentCodeID { get; set; }
        public int? HearingRequestedByCodeID { get; set; }
        public int? HearingResultCodeID { get; set; }
        public int? HearingFollowedRecommendations { get; set; }
        public decimal? HearingInvoiceAmount { get; set; }
         public string HearingOfficerFirstName { get; set; }
        public string HearingOfficerLastName { get; set; }
        public string HearingTypeCodeValue { get; set; }
        public string HearingTypeCodeShortValue { get; set; }
        public string HearingCourtDepartmentCodeValue { get; set; }
        public string HearingRequestedByCodeValue { get; set; }
        public string HearingNote { get; set; }
        public int? NoteID { get; set; }
        public string AppearingAttorneyFirstName { get; set; }
        public string AppearingAttorneyLastName { get; set; }
        public decimal? AppearingAttorneyHours { get; set; }
        public string HearingDisplay { get; set; }
        public int? WorkFlag { get; set; }
        public int? NoDeleteFlag { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }
        public int? ReportID { get; set; }
    }
    public class pd_HearingPersonsGetByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? CaseAgencyID { get; set; }
    }
    public class pd_HearingPersonsGetByCaseID_spResult
    {
        public int HearingID { get; set; }
        public DateTime HearingDateTime { get; set; }
        public string PersonName { get; set; }
        public int RoleClient { get; set; }
        public string RoleTypeCodeValue { get; set; }
        public int Resulted { get; set; }

         
    }

    public class pd_PetitionAndAllegationGetByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
   
    }

    public class pd_PetitionAndAllegationGetByCaseID_spResult
    {
        public int PetitionID { get; set; }
        public System.DateTime? PetitionFileDate { get; set; }
        public string PetitionDocketNumber { get; set; }
        public int? PetitionTypeCodeID { get; set; }
        public int AgencyID { get; set; }
        public int CaseID { get; set; }
        public int InsertedByUserID { get; set; }
        public System.DateTime InsertedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public System.DateTime? UpdatedOnDateTime { get; set; }
        public string Type { get; set; }
        public string Agency { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string Allegation { get; set; }
        public byte? RoleClient { get; set; }
        public System.DateTime? PetitionCloseDate { get; set; }
        public int? RespondentFlag { get; set; }
        public string Attorney { get; set; }
        public string CloseDate { get; set; }
        public string CloseReason { get; set; }
        
    }
    public class pd_RoleGetByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        
    }
    public class pd_RoleGetByCaseID_spResult
    {
        public pd_RoleGetByCaseID_spResult()
        {
        }
        public int? RoleID { get; set; }
        public string Role { get; set; }
        public string PersonNameDisplay { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string DOBAge { get; set; }
        public int? Sort { get; set; }
        public int? CaseID { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public int? PersonID { get; set; }
        public int? AgencyID { get; set; }
        public int? InsertedByUserID { get; set; }
        public System.Nullable<System.DateTime> InsertedOndatetime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public System.Nullable<System.DateTime> UpdatedOndatetime { get; set; }
        public string PersonDOB { get; set; }
        public int? PersonAgencyID { get; set; }
        public byte? RoleClient { get; set; }
        public string RoleStartDate { get; set; }
        public string RoleEndDate { get; set; }
        public string DocketNumber { get; set; }
        public int? ChildRespondent { get; set; }
        public int? Editable { get; set; }
        public int? PRoleID { get; set; }
        public string SS { get; set; }
        public int? RecordTimeFlag { get; set; }
        public int? CanDeleteFlag { get; set; }
        public string SortRoleStartDate { get; set; }
        public string SortRoleEndDate { get; set; }
        public int? IsAgencyAttorneyFlag { get; set; }
        public string LatestConflictID { get; set; }
        public string Language { get; set; }
        public string Attorney { get; set; }
        public int? IsDeceasedFlag { get; set; }
        public string ClassificationList { get; set; }
        public string Visitation { get; set; }
        public string RaceNeededAlert { get; set; }
    }
}
