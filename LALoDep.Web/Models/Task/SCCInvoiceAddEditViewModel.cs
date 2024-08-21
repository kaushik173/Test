using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LALoDep.Domain.pD_SCCInvoice;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{
    public class SCCInvoiceAddEditViewModel
    {
        public bool IsSelected{ get; set; }
        [Display(Name="Client")]
        public string ClientDisplay { get; set; }
        [Display(Name = "Current Petition Date")]
        public string PetitionDate { get; set; }
        [Display(Name = "Invoice Number")]
        public string SCCInvoiceNumber { get; set; }
        [Display(Name = "Invoice Date")]
        public string SCCInvoiceDateSubmitted{ get; set; }
        [Display(Name = "Invoice Type")]
        public int? SCCInvoiceRateID { get; set; }
        [Display(Name = "Attorney")]
        public int? SCCInvoiceStatusByPersonID { get; set; }
           [Display(Name = "Attorney")]
        public int? SCCInvoiceSubmittedByPersonID { get; set; }


        
        [Display(Name = "Dept")]
        public int? SCCInvoiceDepartmentCodeID { get; set; }
        [Display(Name = "Next Hrg Date")]
        public string SCCInvoiceNextHearingDate{ get; set; }
        [Display(Name = "Petition Date")]
        public string SCCInvoicePetitionFileDate { get; set; }
        [Display(Name = "Appointment Date")]
        public string SCCInvoiceAppointmentDate { get; set; }
        [Display(Name = "Service Hrg Date")]
        public string SCCInvoiceServiceHearingDate { get; set; }
        [Display(Name = "1st RPP Date")]
        public string SCCInvoiceFirstRPPDate{ get; set; }
        [Display(Name = "Relief Date")]
        public string SCCInvoiceReliefDate { get; set; }
        [Display(Name = "Date To Be Paid")]
        public string SCCInvoicePaidDate{ get; set; }
        [Display(Name = "Admin Note(required if status is Denied)")]
        public string AdminNote { get; set; }
        public int AdminNoteID { get; set; }
        [Display(Name = "Attorney Note")]
        public string AttorneyNote { get; set; }
        public int AttorneyNoteID { get; set; }
        public int? SCCInvoiceID { get; set; }
        public int? SCCInvoiceStatusCodeID{ get; set; }
        public int? ReferralSourceCodeID { get; set; }

        public string CourtNumber { get; set; }
        public int? AttorneyPersonID { get; set; }
        public string AttorneyFirstName { get; set; }
        public string AttorneyLastName { get; set; }
        public string AttorneyPhoneNumber { get; set; }
        public string AttorneySSNTaxID { get; set; }
        public string AttorneyBarNumber { get; set; }
        
        public List<pd_SCCInvoiceGetByCaseID_spResult> SSCInvoiceList { get; set; }
        public List<pd_AttorneyGetByCaseIDForSCCInvoice_spResult> AttorneyList { get; set; }
        public List<CodeViewModel> InvoiceType { get; set; }
        public IEnumerable<SelectListItem> Department { get; set; }
        public List<pd_SCCInvoiceClientGetByCaseID_spResult> Clients { get; set; }

        public SCCInvoiceAddEditViewModel()
        {
            SSCInvoiceList = new List<pd_SCCInvoiceGetByCaseID_spResult>();
            AttorneyList = new List<pd_AttorneyGetByCaseIDForSCCInvoice_spResult>();
            Clients = new List<pd_SCCInvoiceClientGetByCaseID_spResult>();
        }
    }
}