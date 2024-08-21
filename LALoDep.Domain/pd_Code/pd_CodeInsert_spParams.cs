using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Code
{
    public class pd_CodeInsert_spParams
    {
        public int? CodeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public int CodeTypeID { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
