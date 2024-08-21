using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Code
{
    public class pd_CodeGetBySystemValueTypeID_spResults
    {
        public int CodeID { get; set; }
        public int CodeTypeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public int? SystemValueID{ get; set; }
        public int? Selected{ get; set; }
        public int? SystemValueSequence{ get; set; }
        public int? SortSeq{ get; set; }
        public int? RecordStateID{ get; set; }
        public int ActiveAgencyCodeFlag { get; set; }
    }
}
