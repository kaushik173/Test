
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.Expense
{

    public class ExpenseIUD_spParams
    {
        public string IUD { get; set; }
        public int? ExpenseID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public DateTime? ExpenseDate { get; set; }
        public int? ExpenseTypeCodeID { get; set; }
        public string ExpenseVendorName { get; set; }
        public int? ExpenseRequestedByPersonID { get; set; }
        public int? ExpenseIVeEligibleCodeID { get; set; }
        public decimal? ExpenseAmount { get; set; }
        public string ExpenseNote { get; set; }
        public string ExpenseNoteAdmin { get; set; }
        public int? ExpenseStatusCodeID { get; set; }
        public int? UserID { get; set; }

    }

}