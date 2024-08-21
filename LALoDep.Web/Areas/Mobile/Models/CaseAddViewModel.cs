using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Areas.Mobile.Models
{
    public class CaseAddViewModel
    {   
        public string ClientLastName { get; set; }
        public string ClientFirstName { get; set; }
        public int? ClientRoleTypeCodeID { get; set; }
        public IEnumerable<SelectListItem> ClientRoleList { get; set; }

        public string CaseLastName { get; set; }
        public string CaseFirstName { get; set; }
        public string CaseAppointmentDate { get; set; }
        public string PetitionNumber { get; set; }
        public int? DepartmentCodeID { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public int? AssociationCodeID { get; set; }
        public IEnumerable<SelectListItem> AssociationList { get; set; }
        
        public int AllegationTypeCodeID1 { get; set; }
        public int AllegationTypeCodeID2 { get; set; }
        public int AllegationTypeCodeID3 { get; set; }
        public int AllegationTypeCodeID4 { get; set; }

        public IEnumerable<SelectListItem> AllegationList { get; set; }
        public CaseAddViewModel()
        {
            ClientRoleList = new List<SelectListItem>();
            DepartmentList = new List<SelectListItem>();
            AssociationList = new List<SelectListItem>();
            AssociationList = new List<SelectListItem>();
        }
    }
}