using LALoDep.Domain.pd_Role;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class QuickARModel
    {
        public string ControlType { get; set; }
        public int HearingReportFilingDueID { get; set; }
        [Display(Name = "Request Date")]
        public string RequestDate { get; set; }
        public int? RequestTypeID { get; set; }
        [Display(Name = "Request Type")]
        public string RequestType { get; set; }
        public int? HearingID { get; set; }
        public int? RequestByID { get; set; }

        [Display(Name = "Request By")]
        public string RequestBy { get; set; }
        public string Hearing { get; set; }
        public int? RequestForID { get; set; }

        [Display(Name = "Request For")]
        public string RequestFor { get; set; }
        [Display(Name = "Due Date")]
        public string DueDate { get; set; }
        public string CompletedDate { get; set; }
        [Display(Name = "Completed")]
        public bool Completed { get; set; }
        public string RequestNote { get; set; }
        public string InvestigatorEvaluationNote { get; set; }
        public string InvestigatorEvaluationNoteControlType { get; set; }
        public int? InvestigatorEvaluationNoteID { get; set; }

        [Display(Name = "Association/Role")]
        public int? AssociationTypeCodeID { get; set; }
        public IEnumerable<SelectListItem> AssociationTypeList { get; set; }
        
        public bool CareTaker { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Role Date")]
        public string RoleDate { get; set; }
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        [Display(Name = "State")]
        public int StateID { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }
        [Display(Name = "Country")]
        public int CountryID { get; set; }
        public IEnumerable<SelectListItem> CountryList { get; set; }
        public string Zip { get; set; }
        [Display(Name = "Address Phone")]
        public string AddressPhone { get; set; }
        [Display(Name = "Work Phone")]
        public string WorkPhone { get; set; }
        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }
        
        [Display(Name = "Placement Agency/Institution")]
        public int PlacementAgencyID { get; set; }
        public IEnumerable<SelectListItem> PlacementAgencyList { get; set; }
        [Display(Name ="Start Date")]
        public string StartDate { get; set; }

        [Display(Name = "Record Time Type")]
        public int RecordTimeTypeID { get; set; }
        public IEnumerable<SelectListItem> RecordTimeTypeList { get; set; }
        [Display(Name = "Phase")]
        public int PhaseID { get; set; }
        public IEnumerable<SelectListItem> PhaseList { get; set; }
        [Display(Name = "Date")]
        public string RecordDate { get; set; }
        public decimal? Hours { get; set; }
        [Display(Name = "Record Time Note")]
        public string RecordTimeNote { get; set; }
        public IEnumerable<pd_QuickRFDClientGet_spResult> ClientList { get; set; }

        public int? WorkIVeEligibleCodeID { get; set; }
        public IEnumerable<SelectListItem> IVeEligibleList { get; set; }
        public QuickARModel()
        {
            AssociationTypeList = new List<SelectListItem>();
            ClientList = new List<pd_QuickRFDClientGet_spResult>();
       }
    }
}