using LALoDep.Domain.pd_MonthlyInvoiceList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Task
{    
    public class MonthlyInvoiceEditModel
    {
        public int? InvoiceMonthlyID { get; set; }
        public string SummaryDisplay { get; set; }        
        public string SubmitDate { get; set; }
        public string StatusDate { get; set; }        
        public int? StatusCodeID { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentNumber { get; set; }

        public string DetailsHeaderDisplay { get; set; }
        public List<pd_MonthlyInvoiceDetailGetByMonthlyInvoiceID_spResult> ClientDetails { get; set; }
    }

    public class MonthlyInvoiceAddModel
    {
        public string PersonName { get; set; }
        public string InvoiceMonth { get; set; }
        public string NotYetIncludedHeaderDisplay { get; set; }
        public string PrevioulyIncludedHeaderDisplay { get; set; }
        public List<pd_MonthlyInvoiceDetailGetByMonthlyInvoiceID_spResult> ClientNotIncluded { get; set; }
        public List<pd_MonthlyInvoiceDetailGetByMonthlyInvoiceID_spResult> ClientIncluded { get; set; }
    }

    public class ClientDetail
    {   
        public string Client { get; set; }        
        public string PetitionDocketNumber { get; set; }
        public string NextCourtDate { get; set; }
        public string CaseNumber { get; set; }        
        public string CaseID { get; set; }
    }
}