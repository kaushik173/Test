using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Role;

namespace LALoDep.Models.CaseOpening
{
    public class AddChildOrParentsViewModel
    {

        public IEnumerable<SelectListItem> SexList { get; set; }
        public IEnumerable<SelectListItem> RaceList { get; set; }

        public IEnumerable<SelectListItem> RoleList { get; set; }

        public IEnumerable<SelectListItem> DesignatedDayList { get; set; }

        public bool IsVisibleCnCheckbox { get; set; }
        public bool IsRoleClientAdded { get; set; }
        public bool IsChildAdded { get; set; }
        public bool IsCnAdded { get; set; }
       
        public List<AddChildOrParent> AddChildOrParentList { get; set; }
        public List<pd_RoleGetByCaseIDChildRespondent_spResult> RespondentList { get; set; }
        public int DOBRequiredForChildren { get; set; }
        public AddChildOrParentsViewModel()
        {
            AddChildOrParentList = new List<AddChildOrParent>();
            RespondentList = new List<pd_RoleGetByCaseIDChildRespondent_spResult>();

        }
    }
    public class AddChildOrParent 
    {

        public int SexID { get; set; }
        public bool IsClient { get; set; }
        public bool IsCn { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string DOB { get; set; }
        public int RaceID { get; set; }
        public int RoleID { get; set; }
        public int DesignatedDayID { get; set; }
        public string StartOrApptDate { get; set; }

        public bool IsSs { get; set; }
    }
}