using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LALoDep.Models
{
    public class EmployeeRosterViewModel
    {
        [Display(Name="Agency")]
        public int AgencyID { get; set; }
        [Display(Name = "Role")]
        public int StaffPositionCodeID { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        public IEnumerable<CodeViewModel> AgencyList { get; set; }
        public IEnumerable<CodeViewModel> RoleList { get; set; }
    }
}