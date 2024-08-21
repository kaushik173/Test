using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Note
{
    public class pd_CodeGetNewNoteTypeByCaseID_spResult

    {
        public int CodeTypeID { get; set; }
        public int CodeID { get; set; }
        public Int16 RecordStateID { get; set; }
        public string CodeEnumName { get; set; }
        public int? CodeMobileValue { get; set; }
        public int? SystemValueSequence { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public Decimal RecordTimeStamp { get; set; }
    }

}
