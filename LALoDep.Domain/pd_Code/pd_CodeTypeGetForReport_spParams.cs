using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Code
{
    public class pd_CodeTypeGetForReport_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_CodeTypeGetForReport_spResult
    {
        public int CodeTypeID { get; set; }
        public string CodeTypeValue { get; set; }
    }
}
