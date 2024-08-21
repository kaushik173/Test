using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Administration
{
    public class UsersViewModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public bool ActiveUsers { get; set; }
        public bool OpenPositions { get; set; }
        public string AgencyID { get; set; }
        public string JcatsGroupID { get; set; }
        public string RoleTypeCodeID { get; set; }
        public bool ActiveUserOnly { get; set; }
        public bool OpenPositionsOnly { get; set; }
        public IEnumerable<SelectListItem> Agencies { get; set; }
        public IEnumerable<SelectListItem> SecurityGroups { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public bool OnViewLoad { get; set; }
    }
}