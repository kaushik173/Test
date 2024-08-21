using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pD_SCCInvoice;

namespace LALoDep.Models
{
    public class SCCInvoiceQueueSearchViewModel
    {
        public string Attorney { get; set; }
        public string Client { get; set; }
        public string Source { get; set; }
        public int? CaseID { get; set; }
        public string NextDate { get; set; }
        public string CourtNumber{ get; set; }
        public DateTime DateNextDate { get; set; }
        public int? JcatsNumber { get; set; }
        public string Type { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string DateToBePaid { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
        public DateTime DateDateToBePaid { get; set; }
        public string SCCInvoicePaidDateStart { get; set; }
        public string SCCInvoicePaidDateEnd { get; set; }
        public string InvoiceDt { get; set; }
        public DateTime DateInvoiceDt { get; set; }
        public int? InvoiceNumber { get; set; }
        public string AttorneyId { get; set; }
        public int SourceId { get; set; }
        public int InvoiceStatusId { get; set; }
        public bool OnViewLoad { get; set; }
        public IEnumerable<SelectListItem> AttorneyList { get; set; }
        public IEnumerable<SelectListItem> SourceList { get; set; }
        public IEnumerable<SelectListItem> InvoiceStatusList { get; set; }
        public bool CanEditAccess { get; set; }
    }
}