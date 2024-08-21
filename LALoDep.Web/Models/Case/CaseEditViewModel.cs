using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class CaseEditViewModel
    {
        public int? CaseID { get; set; }
        public int? AgencyID { get; set; }
        [Display(Name = "JCATS #")]
        public string CaseNumber { get; set; }
        [Display(Name = "Appointment Date")]
        public string CaseAppointmentDate { get; set; }
        [Display(Name = "Closed Date")]
        public string CaseClosedDate { get; set; }
        [Display(Name = "Panel Case")]
        public bool CasePanelCase { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentID { get; set; }
        [Display(Name = "Case Name")]
        public int? CaseNameRoleID { get; set; }
        [Display(Name = "Referral Source")]
        public int? ReferralSourceCodeID { get; set; }
        public int? ReferralSourceAttrID { get; set; }

        [Display(Name = "File Location")]
        public int? FileLocationID { get; set; }
        public int? FileLocationAttrID { get; set; }

        [Display(Name = "File Box # or Barcode #")]
        public string FileBox { get; set; }
        public int? FileBoxAttrID { get; set; }

        public IEnumerable<CodeViewModel> CaseName { get; set; }
        public IEnumerable<SelectListItem> FileLocation { get; set; }
        public IEnumerable<SelectListItem> ReferatlSource { get; set; }
        public IEnumerable<SelectListItem> Department { get; set; }

        public bool IsSSCInvoiceExist { get; set; }
        public short? RecordStateID { get; set; }

        public bool HasOpenPetitions { get; set; }

        public bool IsUpdateCase { get; set; }
        public bool IsUpdateName { get; set; }
        public bool IsUpdateFileLocation { get; set; }
        public bool IsUpdateFileBox { get; set; }
        public bool IsUpdateReferral { get; set; }
        public int? CaseSecuredID { get; set; }
        public bool CaseSecured  { get; set; }
    }
}