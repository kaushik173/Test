using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain;
using LALoDep.Domain.pd_Case;
using System.Web.Mvc;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Subpoena;
using LALoDep.Domain.pd_Role;

namespace LALoDep.Models.Case
{
    public class SubpoenaAddEditViewModel
    {
        public int? HearingID { get; set; }
        public int? AgencyID { get; set; }
        public string HearingType { get; set; }
        public string HearingDateTime { get; set; }
        public string HearingDept { get; set; }
        public List<pd_RoleGetForSubpoenaByCaseID_spResult> SubpoenaList { get; set; }
        public SubpoenaAddEditViewModel()
        {
            SubpoenaList = new List<pd_RoleGetForSubpoenaByCaseID_spResult>();
        }
    }
}