using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_UserGroups;
using LALoDep.Domain.pd_System;

namespace LALoDep.Models.Administration
{
    public class AdminSystemValueViewModel
    {
        public int SystemValueTypeCodeTypeID { get; set; }
        public List<SystemValueViewModel> CodeTypeFilter { get; set; }

        public AdminSystemValueViewModel()
        {
            CodeTypeFilter = new List<SystemValueViewModel>();
        }
    }
}