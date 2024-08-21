using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_CodeTables
{
    public class pd_CodeGetByTypeIDAndNotUserID_spResult
    {
        public int CodeID { get; set; }
        public int CodeTypeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
    }
}
