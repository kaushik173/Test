using Jcats.SD.Domain.com_Jcats;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models 
{
    public class HelpAddEditViewModel
    {
        public int? JcatsHelpID { get; set; }
        public string RemoveAspPageIDs { get; set; }
        public string ASPPageIDs { get; set; }
        public string HelpFileUrl { get; set; }
        public string GroupName { get; set; }
        public string PageName { get; set; }
        public int LinkToPageID { get; set; }
        public int Order { get; set; }

        public string HelpContent { get; set; }
     
    
    }
}