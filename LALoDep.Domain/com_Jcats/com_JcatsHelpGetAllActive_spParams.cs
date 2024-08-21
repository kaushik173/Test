using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcats.SD.Domain.com_Jcats
{
    public class com_JcatsHelpGetAllActive_spParams
    {
        public string JcatsHelpPageName { get; set; }
        public int? JcatsHelpID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
