using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_UserGroups;

namespace LALoDep.Models.Administration
{
    public class CountyCounselListViewModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string BarNumber { get; set; }
        public int? AgencyID { get; set; }
        public bool OnViewLoad { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }
        public List<pd_JcatsGroupGetByUserID_spResult> GroupList { get; set; }
    }
}