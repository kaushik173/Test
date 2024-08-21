using Jcats.SD.Domain.com_Jcats;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models 
{
    public class HelpListViewModel
    {
        [Display(Name="Group Name")]
        public string GroupName { get; set; }
        [Display(Name = "Page Name")]
        public string PageName { get; set; }
        [Display(Name = "Order")]
        public string Order { get; set; }
        [Display(Name = "Link To Page")]
        public int LinkToPageID { get; set; }
        public int HelpID { get; set; }
        public string HelpContent { get; set; }

        public string HelpFileUrl { get; set; }
        public List<com_JcatsHelpGetNavigation_spResult> LinkPages { get; set; }

        public bool HasEditAccess { get; set; }  public bool HasAddAccess { get; set; }
       
        public HelpListViewModel()
        {
            LinkPages = new List<com_JcatsHelpGetNavigation_spResult>();
        }
    }
}