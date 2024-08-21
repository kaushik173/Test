using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.com_Jcats
{
    public class com_JcatsHelpLogInsert_spParams
    {
        public string JcatsHelpPageName { get; set; }
        public decimal? RecordTimeStamp{ get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
