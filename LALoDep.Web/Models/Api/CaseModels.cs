using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Api
{
    public class CaseSearchModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }        
        public string DocketNumber { get; set; }
        public string JcatsNumber { get; set; }        
        public string HHSA { get; set; }
        public int? AgencyID { get; set; }

        public int StartRecord { get; set; }
        public int Range { get; set; }        
        public string SearchGuid { get; set; }
        
    }
}