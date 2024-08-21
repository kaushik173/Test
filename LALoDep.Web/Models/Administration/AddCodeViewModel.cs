using LALoDep.Domain.pd_Note;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Administration
{
    public class AddEditCodeViewModel
    {
        public int? CodeID { get; set; }
        public int CodeTypeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public List<CodeAgency> CodeAgencies { get; set; }
        public IEnumerable<CodeViewModel> CountryCode { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Street")]
        public string Street { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }
        [Display(Name = "State")]
        public int StateCodeID { get; set; }
        [Display(Name = "Country")]
        public int CountryCodeID { get; set; }

        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }
        [Display(Name = "Phone")]
        public string HomePhone { get; set; }
        public int? AddressID { get; set; }
        
        public IEnumerable<CodeViewModel> StateCode { get; set; }
        public int? RecordStateID { get; set; }
        public List<pd_NoteGetForCodeID_spResult> Notes { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }
        public IEnumerable<SelectListItem> NoteTypeList { get; set; }

        public string DisplayBanner { get; set; }
        public int ReadOnlyCodeFlag { get; set; }
        public int ReadOnlyAddressFlag { get; set; }
        public AddEditCodeViewModel()
        {
            CodeAgencies = new List<CodeAgency>();
            StateCode = new List<CodeViewModel>();
            CountryCode = new List<CodeViewModel>();
            Notes = new List<pd_NoteGetForCodeID_spResult>();
            NoteTypeList = new List<SelectListItem>();
            AgencyList = new List<SelectListItem>();
        }

    }
    
    public class CodeAgency
    {
        public string AgencyName { get; set; }
        public int? AgencyID { get; set; }
        public bool Selected { get; set; }
        public bool InAgency { get; set; }
        public int? AgencyCodeID { get; set; }
    }
}