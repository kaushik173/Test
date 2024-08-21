using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Users
{
    public class pd_CodeGetBySysValAndUserID_spResult
    {
        public int CodeID { get; set; }
        public int CodeTypeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public int SystemValueSequence { get; set; }
        public string CodeEnumName { get; set; }
        public string CodeMobileValue { get; set; }
    }
}
