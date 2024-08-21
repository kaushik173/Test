using LALoDep.Domain.AddEditCountyCounsel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LALoDep.Models.Administration
{
    public class JudicialOfficerListViewModel
    {
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Agency")]
        public int? AgencyID { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }
    }

    public class JudicialOfficerViewModel
    {
        public int? PersonID { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        
        public IEnumerable<pd_AgencyGetSystemRoleByPersonID_spResult> AgencyList { get; set; }
    }
}