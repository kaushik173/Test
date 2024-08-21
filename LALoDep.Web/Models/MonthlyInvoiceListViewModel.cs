using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models
{
    public class MonthlyInvoiceListViewModel
    {
        public MonthlyInvoiceListViewModel()
        {
            AddInvoiceForList = new List<SelectListItem>();

        }
        public bool AddMonthlyInvoiceAdminMode { get; set; }
        public string AsOfDate { get; set; }
        public int? AttorneyPersonID { get; set; }

        public bool OnViewLoad { get; set; }
        public int AgencyID { get; set; }

        public int CountyID { get; set; }
        public int AttorneyID { get; set; }
        public int StatusCodeID { get; set; }
        public int? InvoiceNumber { get; set; }
        public IEnumerable<SelectListItem> AttorneyList { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> CountyList { get; set; }
        public IEnumerable<SelectListItem> AddInvoiceForList { get; set; }
    }
}