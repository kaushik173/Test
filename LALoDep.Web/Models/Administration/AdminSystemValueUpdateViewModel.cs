using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_UserGroups;
using LALoDep.Domain.pd_Code;

namespace LALoDep.Models.Administration
{
    public class AdminSystemValueUpdateViewModel
    {
        public int? SystemValueTypeID { get; set; }
        public string SystemValueTypeEntry { get; set; }
        public int ButtonID { get; set; } //1 for Save and 2 for Save/Return
        public List<SystemValueListViewModel> SytemsValueUpdateList { get; set; }
        public AdminSystemValueUpdateViewModel()
        {
            SytemsValueUpdateList = new List<SystemValueListViewModel>();
        }
    }
}