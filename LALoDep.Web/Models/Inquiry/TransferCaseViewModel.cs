using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Inquiry
{
    [Serializable]
    public class TransferCaseViewModel
    {
        public int PersonID { get; set; }
        public string TransferToPersonID { get; set; }
        public IEnumerable<SelectListItem> TransferToPersonList { get; set; }
        public string TransferDate { get; set; }

        public int? TotalCases { get; set; }

        public List<CaseListViewModel> CaseList { get; set; }
    }
    [Serializable]
    public class CaseListViewModel
    {
        public bool IsOn { get; set; }
        public int? CaseID { get; set; }        
        public string Clients { get; set; }
        public string Department { get; set; }        
        public string PetitionNumber { get; set; }
        public string CaseName { get; set; }
        public string AttorneyRoleStartDate { get; set; }
        public int? RoleTypeCodeID { get; set; }
    }
}