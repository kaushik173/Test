using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models
{
    public class HourlyInvoiceListViewModel
    {
        public bool OnViewLoad { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceAmount { get; set; }
        public string AttorneyID { get; set; }
        public string Attorney { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalAmount { get; set; }
        public IEnumerable<SelectListItem> AttorneyList { get; set; }
    }

    public class HourlyInvoiceEditViewModel
    {
        [Display(Name = "Invoice #")]
        public int HourlyInvoiceID { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public int? HourlyInvoiceStatusCodeID { get; set; }
        [Display(Name = "Status Date")]
        public string HourlyInvoiceStatusDate { get; set; }

        [Display(Name = "Invoice Amount")]
        public decimal? TotalInvoiceAmount { get; set; }        
        public short? RecordStateID { get; set; }

        [Display(Name = "Attorney")]
        public string HourlyInvoicePersonName { get; set; }
        [Display(Name = "Status")]
        public string HourlyInvoiceStatus { get; set; }
        [Display(Name = "Status By")]
        public string HourlyInvoiceStatusPersonName { get; set; }
        
        [Display(Name = "Authorized Amount")]
        public decimal? HourlyInvoiceCourtApprovalAmount { get; set; }
        [Display(Name = "Authorized Date")]
        public string HourlyInvoiceCourtApprovalDate { get; set; }        
    }
}