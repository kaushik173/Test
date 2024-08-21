
using LALoDep.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Administration
{
    public class SecurityListViewModel
    {
        public int SecurityID { get; set; }
        public int JcatsGroupID { get; set; }
        public int SecurityItemID { get; set; }
        public string Category { get; set; }
        public string SecurityItemDescription { get; set; }
    }
}