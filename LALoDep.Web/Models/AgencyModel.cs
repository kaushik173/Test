using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models
{
    public class AgencyModel
    {
        public string AgencyDisplay { get; set; }
        public int JcatsGroupAgencyID { get; set; }
        public int AgencyID { get; set; }
        public string AgencyName { get; set; }
        public string AgencyAbbreviation { get; set; }
        public int Selected { get; set; }        
    }
}