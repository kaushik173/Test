using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LALoDep.Models
{
    public class CaseSearchExtendedViewModel 
    {
        public string ClientLastName { get; set; }
        public string ClientFirstName { get; set; }
        [Display(Name = "Parent First Name")]
        public string ParentFirstName { get; set; }
        [Display(Name = "Parent Last Name")]
        public string ParentLastName { get; set; }
        [Display(Name = "Attorney")]
        public int? AgencyAttorneyID { get; set; }
        [Display(Name = "Investigator")]
        public int? InvestigatorID { get; set; }
        [Display(Name = "Paralegal")]
        public int? LegalAssistantID { get; set; }
        [Display(Name = "Social Worker Last Name")]
        public string SocialWorkerLastName { get; set; }
        [Display(Name = "Social Worker First Name")]
        public string SocialWorkerFirstName { get; set; }
        [Display(Name = "Caretaker Last Name")]
        public string CaretakerLastName { get; set; }
        [Display(Name = "Caretaker First Name")]
        public string CaretakerFirstName { get; set; }
        [Display(Name = "Child Last Name")]
        public string ChildLastName { get; set; }
        [Display(Name = "Child First Name")]
        public string ChildFirstName { get; set; }
        [Display(Name = "Child DOB ")]
        public string ChildDOB { get; set; }
        
        public string ClientDOB { get; set; }
        [Display(Name = "Parent DOB ")]
        public string ParentDOB { get; set; }
        public string ClientAddress { get; set; }
        public string ClientCity { get; set; }
        public int? ClientStateID { get; set; }
        public string ClientZip { get; set; }
        public string ClientPhoneNumber { get; set; }
        public string ClientSSN { get; set; }
        [Display(Name = "Parent Address")]
        public string ParentAddress { get; set; }
        [Display(Name = "Parent City")]
        public string ParentCity { get; set; }
        [Display(Name = "Parent State")]
        public int? ParentStateID { get; set; }
        [Display(Name = "Parent Zip")]
        public string ParentZip { get; set; }
        [Display(Name = "Parent Phone")]
        public string ParentPhone { get; set; }
        [Display(Name = "Caretaker Address")]
        public string CaretakerAddress { get; set; }
        [Display(Name = "Social Worker Phone")]
        public string SocialWorkerPhoneNumber { get; set; }
        [Display(Name = "Caretaker Phone")]
        public string CaretakerPhoneNumber { get; set; }
        [Display(Name = "Language")]
        public int? LanguageID { get; set; }
        public string PetitionNumber { get; set; }
        [Display(Name = "Hearing Type")]
        public int? HearingTypeID { get; set; }
        [Display(Name = "Hearing Date")]
        public string HearingDate { get; set; }
        public string HearingTime { get; set; }
        [Display(Name = "Hearing Officer")]
        public int? HearingOfficerID { get; set; }
        [Display(Name = "Hearing Department")]
        public int? HearingDepartmentID { get; set; }
        public string CaseOpenDate { get; set; }
        public string CaseClosedDate { get; set; }
        [Display(Name = "Allegation")]
        public int? AllegationID { get; set; }
        [Display(Name = "Petition Type")]
        public int? PetitionTypeID { get; set; }
        [Display(Name = "RFDType")]
        public int? ReportFilingDueTypeID { get; set; }
        [Display(Name = "Parental Rights Term. Date")]
        public string ParentalRightsTerminationDate { get; set; }
        [Display(Name = "Case #")]
        public string CaseNumber { get; set; }
        [Display(Name = "County Counsel Number")]
        public string CountyCounselNumber { get; set; }
        [Display(Name = "HHSA Number")]
        public string HHSANumber { get; set; }
        [Display(Name = "Parent SSN")]
        public string ParentSSN { get; set; }
        [Display(Name = "Booking Number")]
        public string BookingNumber { get; set; }
        [Display(Name = "Inmate Number")]
        public string InmateNumber { get; set; }
        public int? AppointmentCase { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        [Display(Name = "Case Status")]
        public string CaseStatus { get; set; }
        [Display(Name = "Case Department")]
        public int? CaseDepartmentID { get; set; }
        [Display(Name = "Role Open/Close")]
        public int? RoleStatus { get; set; }
        [Display(Name = "Petition Status")]
        public int? PetitionStatus { get; set; }

        public IEnumerable<KeyValuePair<string, string>> Status
        {
            get
            {
                return new List<KeyValuePair<string, string>>() 
                {
                    new KeyValuePair<string, string>("1", "Open"),
                    new KeyValuePair<string, string>("2", "Close"),
                    new KeyValuePair<string, string>("3", "Both"),
                };
            }
        }

        public IEnumerable<CodeViewModel> State { get; set; }
        public IEnumerable<PersonViewModel> Attorney { get; set; }
        public IEnumerable<PersonViewModel> Investigator { get; set; }
        public IEnumerable<PersonViewModel> Paralegal { get; set; }
        public IEnumerable<CodeViewModel> HearingType { get; set; }
        public IEnumerable<PersonViewModel> HearingOfficer { get; set; }
        public IEnumerable<CodeViewModel> CaseDepartment { get; set; }
        public IEnumerable<CodeViewModel> HearingDepartment { get; set; }
        public IEnumerable<CodeViewModel> Allegation { get; set; }
        public IEnumerable<CodeViewModel> Language { get; set; }
        public IEnumerable<CodeViewModel> PetitionType { get; set; }
        public IEnumerable<CodeViewModel> RFDType { get; set; }

    }
}