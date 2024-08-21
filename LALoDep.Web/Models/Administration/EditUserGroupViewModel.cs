using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataTables.Mvc;
using LALoDep.Domain.pd_UserGroups;

namespace LALoDep.Models.Administration
{
    public class EditUserGroupViewModel
    {
        public List<pd_JcatsGroupAgencyGetByJcatsGroupID_spResult> GroupAgencyList { get; set; }
        public pd_JcatsGroupGet_spResult JcatsGroup { get; set; }
        public DataTablesResponse dtable { get; set; }
        public string Mode { get; set; }
    }
}