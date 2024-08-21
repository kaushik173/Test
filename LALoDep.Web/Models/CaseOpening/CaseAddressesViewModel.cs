using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using LALoDep.Domain.pd_Address;
using Jcats.SD.UI.ViewModels;

namespace LALoDep.Models.CaseOpening
{
    public class CaseAddressesViewModel
    {
        public IEnumerable<CodeViewModel> StateCode { get; set; }
        public IEnumerable<SelectListItem> TypeCode { get; set; }
        public IEnumerable<SelectListItem> NonChildAddressType { get; set; }


        public IEnumerable<CodeViewModel> CountryCode { get; set; }

        public IEnumerable<pd_PersonAddressGetAllRolesByCaseID_spResult> PeopleInCase { get; set; }
        public IEnumerable<SelectListItem> ExistingAddresses { get; set; }
        public IEnumerable<SelectListItem> PlacementAgencyAddressList { get; set; }
        public int ExistingAddressID { get; set; }
        public int PlacementAgencyAddressID { get; set; }
        [Display(Name = "Street")]
        public string Street { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "State")]
        public int StateCodeID { get; set; }
        [Display(Name = "Country")]
        public int CountryCodeID { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public int InvoiceID { get; set; }
        public short RecordStateID { get; set; }

        public int ActionRequestID { get; set; }
    
        public int PersonCount { get; set; }

        public int AddressID { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public bool CanText { get; set; }
        public string AddressStreet { get; set; }
        public string AddressHomePhone { get; set; }
        public string PlacementAgencyAddress { get; set; }
    }
}