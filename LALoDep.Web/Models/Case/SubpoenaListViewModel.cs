using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain;
using LALoDep.Domain.pd_Case;
using System.Web.Mvc;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Subpoena;

namespace LALoDep.Models.Case
{
    public class SubpoenaListViewModel
    {
        public IEnumerable<CodeViewModel> HearingList { get; set; }
        public int HearingID { get; set; }
        public List<pd_SubpoenaGetByCaseID_spResult> SubpoenaList { get; set; }
        public bool CanEditSubpoena { get; set; }

        public SubpoenaListViewModel()
        {
            SubpoenaList = new List<pd_SubpoenaGetByCaseID_spResult>();
        }
    }
}