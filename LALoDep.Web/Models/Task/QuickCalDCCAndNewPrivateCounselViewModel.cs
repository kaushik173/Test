using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.qcal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
    public class QuickCalDCCAndNewPrivateCounselViewModel
    {
        public string AddMode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? HearingID { get; set; }
    }
    public class QuickCalNewFacilityViewModel
    {
      
        public string FacilityName { get; set; }
       
        public int? HearingID { get; set; }
    }
    public class QuickCalCurrentERHViewModel
    {
        public List<qcal_AS_ERH_ChildRoles_spResult> ERHChildRoles { get; set; }
        public List<qcal_AS_ERH_History_spResult> ERHHistory { get; set; }

        public List<SelectListItem> ExistingRolesList { get; set; }
        public List<SelectListItem> CaseRoleTypeList { get; set; }
        public List<SelectListItem> AssociationToChildList { get; set; }
        
        public QuickCalCurrentERHViewModel()
        {
            ERHHistory = new List<qcal_AS_ERH_History_spResult>();
            ExistingRolesList = new List<SelectListItem>();
            CaseRoleTypeList = new List<SelectListItem>();
            AssociationToChildList = new List<SelectListItem>();
        }

        public int? HearingID { get; set; }
        public int? CaseRoleTypeID { get; set; }
        public int? ExistingRoleID { get; set; }
        public int? AssociationToChildID { get; set; }
        public string StartDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EndDate { get; set; }
    }
    public class AdditionalAttendeesViewModel
    {
        public List<qcal_AS_HearingAttendanceGetAdditionalAttendee_spResult> HearingAttendance { get; set; }
       

        public AdditionalAttendeesViewModel()
        {
            HearingAttendance = new List<qcal_AS_HearingAttendanceGetAdditionalAttendee_spResult>();
            
        }

        public int? HearingID { get; set; }
   
    }


    
}