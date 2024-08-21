using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using LALoDep.Domain.HourlyExpense;

namespace LALoDep.Models.Case
{
    public class ExpenseViewModel
    {
        public IEnumerable<SelectListItem> ExpenseType { get; set; }
        public IEnumerable<PersonViewModel> Attorney { get; set; }
        public IEnumerable<SelectListItem> Provider { get; set; }
        public bool CanAddAccess { get; set; }
        public bool CanEditAccess { get; set; }
        public bool CanDeleteAccess { get; set; }
        public int? HourlyExpenseID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        [Display(Name = "Attorney")]
        public int? PersonID { get; set; }
        [Display(Name = "Date")]
        public string HourlyExpenseDate { get; set; }
        [Display(Name = "Expense Type")]
        public int? HourlyExpenseTypeCodeID { get; set; }
        [Display(Name = "Amount")]
        public decimal? HourlyExpenseAmount { get; set; }
        [Display(Name = "Provider")]
        public int? HourlyExpenseProviderCodeID { get; set; }
        [Display(Name = "Description ")]
        public string HourlyExpenseDescription { get; set; }

        public int RecordStateID { get; set; }
       
    }
}