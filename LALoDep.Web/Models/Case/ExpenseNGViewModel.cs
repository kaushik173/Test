using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LALoDep.Domain;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using LALoDep.Domain.HourlyExpense;
using LALoDep.Domain.Expense;

namespace LALoDep.Models.Case
{
    public class ExpenseNGViewModel : Expense_Get_spResult
    {

        public IEnumerable<SelectListItem> EligibleList { get; set; }
        public IEnumerable<SelectListItem> StaffMemberList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> TypeList { get; set; }

        public bool IsAdmin { get; set; }
        public IEnumerable<Expense_GetByCaseID_spResult> ExpenseList { get; set; }


        public ExpenseNGViewModel()
        {
            ExpenseList = new List<Expense_GetByCaseID_spResult>();

        }

    }
}