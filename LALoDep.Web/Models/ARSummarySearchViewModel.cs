using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models
{
    public class ARSummarySearchViewModel
    {
        public int PersonID { get; set; }
        public string RoleType { get; set; }
        public int RoleTypeID { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public int ActiveUser { get; set; }
        public bool IncludeAll { get; set; }
        public bool UnderAge5Only { get; set; }
        public string Mode { get; set; }
        public bool OnViewLoad { get; set; }
        public IEnumerable<SelectListItem> ModeList
        {
            get
            {
                var items = new List<SelectListItem>();
                {
                    items.Add(new SelectListItem() { Text = "By Requestee", Value = "ByRequestee"});
                    items.Add(new SelectListItem() { Text = "By Requestor", Value = "ByRequestor" });
                };
                return items;
            }

        }
    }
}