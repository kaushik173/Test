using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Case
{
    public class pd_CaseSearchExtended_spParams
    {
        public string ClientLastName { get; set; }
        public string ClientFirstName { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public int? AgencyAttorneyID { get; set; }
        public int? InvestigatorID { get; set; }
        public int? LegalAssistantID { get; set; }
        public string SocialWorkerLastName { get; set; }
        public string SocialWorkerFirstName { get; set; }
        public string CaretakerLastName { get; set; }
        public string CaretakerFirstName { get; set; }
        public string ChildLastName { get; set; }
        public string ChildFirstName { get; set; }
        public string ChildDOB { get; set; }
        public string ClientDOB { get; set; }
        public string ParentDOB { get; set; }
        public string ClientAddress { get; set; }
        public string ClientCity { get; set; }
        public int? ClientStateID { get; set; }
        public string ClientZip { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string ClientSSN { get; set; }
        public string ParentAddress { get; set; }
        public string ParentCity { get; set; }
        public int? ParentStateID { get; set; }
        public string ParentZip { get; set; }
        public string ParentPhone { get; set; }
        public string CaretakerAddress { get; set; }
        public string SocialWorkerPhoneNumber { get; set; }
        public string CaretakerPhoneNumber { get; set; }
        public int? LanguageID { get; set; }
        public string PetitionNumber { get; set; }
        public int? HearingTypeID { get; set; }
        public string HearingDate { get; set; }
        public string HearingTime { get; set; }
        public int? HearingOfficerID { get; set; }
        public int? HearingDepartmentID { get; set; }
        public string CaseOpenDate { get; set; }
        public string CaseClosedDate { get; set; }
        public int? AllegationID { get; set; }
        public int? PetitionTypeID { get; set; }
        public int? ReportFilingDueTypeID { get; set; }
        public string ParentalRightsTerminationDate { get; set; }
        public string CaseNumber { get; set; }
        public string CountyCounselNumber { get; set; }
        public string HHSANumber { get; set; }
        public string ParentSSN { get; set; }
        public string BookingNumber { get; set; }
        public string InmateNumber { get; set; }
        public int? AppointmentCase { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public string CaseStatus { get; set; }
        public int? CaseDepartmentID { get; set; }
        public int? RoleStatus { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? PetitionStatus { get; set; }
    }
}
