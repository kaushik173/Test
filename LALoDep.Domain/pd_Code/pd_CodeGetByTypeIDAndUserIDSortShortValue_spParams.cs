using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Code
{
    public class pd_CodeGetByTypeIDAndUserIDSortShortValue_spParams
    {
        public int CodeTypeID{ get; set; }
        public int UserID { get; set; }
        public int AgencyID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
