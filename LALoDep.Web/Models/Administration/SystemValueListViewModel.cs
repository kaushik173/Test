using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_UserGroups;

namespace LALoDep.Models.Administration
{
    public class SystemValueListViewModel
    {
        public int CodeID { get; set; }
        public int CodeTypeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public int? SystemValueID { get; set; }
        public bool Selected { get; set; }
        public int? SystemValueSequence { get; set; }
        public int? SortSeq { get; set; }
        public bool Delete { get; set; }
        public bool Insert { get; set; }
        public bool Update { get; set; }
        public int? RecordStateID{ get; set; }
        public int ActiveAgencyCodeFlag { get; set; }

    }
}