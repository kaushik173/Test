using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models
{
    [Serializable]
    public class AgencyCountyModel
    {
        public int AgencyCountyID { get; set; }
        public string AgencyCounty { get; set; }
        public string AgencyCountyAbbreviation { get; set; }
    }
}