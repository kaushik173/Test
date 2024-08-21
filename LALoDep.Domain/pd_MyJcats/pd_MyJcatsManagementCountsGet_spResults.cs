using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_MyJcats
{
    public class pd_MyJcatsManagementCountsGet_spResults
    {
        public string CountType { get; set; }
        public string CountValue { get; set; }
        public int? ActionTypeCodeID { get; set; }
        public int? ActionStatusCodeID { get; set; }
        public string ASPSystemValue { get; set; }
        public string RouteToFormName { get; set; }
        public string RouteToPage { get; set; }
        public string HighlightCharacter { get; set; }
    }
}
