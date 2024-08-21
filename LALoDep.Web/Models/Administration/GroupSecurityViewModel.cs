
using LALoDep.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Administration
{
    public class GroupSecurityViewModel
    {
        public List<pd_SecurityGetByJcatsGroupID_sp_Result> AccessRightsItemList { get; set; }
        public List<pd_SecurityGetByNotJcatsGroupID_sp_Result> RestrictedItemsList { get; set; }
        public string JcatsGroupName{ get; set; }
        public int JcatsGroupID{ get; set; }

        public GroupSecurityViewModel()
        {
            AccessRightsItemList = new List<pd_SecurityGetByJcatsGroupID_sp_Result>();
            RestrictedItemsList = new List<pd_SecurityGetByNotJcatsGroupID_sp_Result>();
        }
    }

    public class UserGroupBySecurityItemViewModel
    {
        public int SecurityItemId { get; set; }
        public string SecurityItemDescription { get; set; }
        public string JcatsGroupName { get; set; }
        public int AgencyId { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }
        public int ButtonId { get; set; }

        public string AuthorizedToDeactivateGroupIds { get; set; }
        public string NotAuthorizedToActivateGroupIds { get; set; }






    }
}