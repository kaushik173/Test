using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jcats.SD.Domain.com_Jcats
{
    public class com_JcatsHelpGetASPPage_spResult
    {
        public int ASPPageID { get; set; }
        public string ASPPageName { get; set; }
        public string ASPPageDisplayName { get; set; }
        public int? JcatsHelpID { get; set; }
        public int? SortFlag { get; set; }
        public string ASPPageFileName { get; set; }
        public int? SelectedFlag { get; set; }
    }
    public class com_JcatsHelpGetNavigation_spResult
    {
        public int NavigationID { get; set; }
        public string GroupDisplayName { get; set; }
        public string PageName { get; set; }
        public string PageDisplayName { get; set; }
        public int? JcatsHelpID { get; set; }
        public int? SortFlag { get; set; }
        public string  PageFileName { get; set; }
        public int? SelectedFlag { get; set; }
    }
}
