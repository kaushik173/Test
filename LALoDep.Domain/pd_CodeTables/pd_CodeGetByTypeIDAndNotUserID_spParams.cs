using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_CodeTables
{
    public class pd_CodeGetByTypeIDAndNotUserID_spParams
    {
        public int CodeTypeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class TitleIVeOverheadCodeDropDown_spParams
    {
        public int PersonID { get; set; }
        public int UserID { get; set; } 
    }
    
}
