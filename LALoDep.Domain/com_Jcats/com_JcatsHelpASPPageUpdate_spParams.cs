using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcats.SD.Domain.com_Jcats
{
    public class com_JcatsHelpUpdate_spParams
    {
        public int JcatsHelpID { get; set; }
        public string JcatsHelpPageName { get; set; }
        public string JcatsHelpPageDisplayName { get; set; }
        public int JcatsHelpPageDisplayOrder { get; set; }
        public string JcatsHelpGroupDisplayName { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class com_JcatsHelpNavigationUpdate_spParams
    {
        public int JcatsHelpID { get; set; }
        public int NavigationID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}