using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.NG_com
{
    public class NG_com_NavigationGetByCurrentURL_spResults
    {
        public int NavigationID { get; set; }
        public int? NavigationGroupID { get; set; }
        public int? SecurityItemID { get; set; }
        public string NavigationURL { get; set; }
        public string NG_NavigationURL { get; set; }
    }
}
