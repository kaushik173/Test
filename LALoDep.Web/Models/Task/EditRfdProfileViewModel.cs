using System.Collections.Generic;
using System.Web.Mvc;
using LALoDep.Domain.pd_Profile;

namespace LALoDep.Models.Task
{
    public class EditRfdProfileViewModel
    {
        public int HearingReportFilingDueID { get; set; }
        public int CurrentRoleID { get; set; }
        public int CurrentProfileTypeID { get; set; }
        public int CurrentProfileID { get; set; }
        public int PersonID { get; set; }
        public IEnumerable<SelectListItem> PersonList { get; set; }
        public int ProfileTypeID { get; set; }
        public IEnumerable<SelectListItem> ProfileTypeList { get; set; } 
        public string RfdHeader { get; set; }

        public List<pd_ProfileGetList_spResult> ProfileList { get; set; }
        public List<pd_ProfileGetCurrentByRoleIDRFD_spResult> ProfileQuestionList { get; set; }

    }
}