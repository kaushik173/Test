using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Administration
{
    public class AdminChangePasswordViewModel
    {
        public string UserFullName { get; set; }
        public string JcatsUserName { get; set; }
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        
        [Display(Name = " Current Password")]
        public string EmailPassword { get; set; }
        public bool EmailUpdate { get; set; }
        public int JcatsUserID { get; set; }
        public string JcatsUserLoginName { get; set; }
        
    }
}