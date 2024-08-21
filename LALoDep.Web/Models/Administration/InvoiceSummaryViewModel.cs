using LALoDep.Domain.NgInvoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Administration
{
    public class InvoiceSummaryViewModel
    {
      
     
     
        public int YearQuarterID { get; set; }
        public int ContractorPersonID { get; set; }
        public int StatusCodeID { get; set; }

        public int PendingInvoicesFlag { get; set; }

        public IEnumerable<SelectListItem> ContractorList
        {
            get; set;
        }
        public IEnumerable<SelectListItem> YearQuarterList
        {
            get; set;
        }
        public IEnumerable<NgInvoice_GetStatusCodes_spResult> StatusList
        {
            get; set;
        }
    }
}