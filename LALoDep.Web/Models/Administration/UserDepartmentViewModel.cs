using LALoDep.Domain.pd_Department;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Administration
{
    public class UserDepartmentViewModel
    {
        public int? PersonID { get; set; }                
        public int? DepartmentID { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentCodeID { get; set; }
        [Display(Name = "Start Date")]
        public string DepartmentStartDate { get; set; }
        [Display(Name = "End Date")]
        public string DepartmentEndDate { get; set; }
        
        public int? AgencyID { get; set; }
        public short? RecordStateID { get; set; }
        public string DepartmentCodeValue { get; set; }

        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public List<pd_DepartmentGetByPersonID_spResult> DepartmentHistory { get; set; }
    }
}