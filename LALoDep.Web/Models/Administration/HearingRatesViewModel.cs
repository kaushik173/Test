using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Administration
{
    public class HearingRatesViewModel
    {
        public string HearingType { get; set; }
        public string AgencyName { get; set; }
        public bool OnViewLoad { get; set; }
    }
}