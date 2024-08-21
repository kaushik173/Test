using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Code
{
    public class pd_CodeGet_spParams
    {
        public int? CodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_CodeGet_spResult
    {
        public int CodeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public int CodeTypeID { get; set; }
        public short? RecordStateID { get; set; }
        public string CodeEnumName { get; set; }
        public string CodeMobileValue { get; set; }

    }
}
