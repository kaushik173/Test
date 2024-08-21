using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.HourlyExpense
{
    public class pd_HourlyExpenseUpdate_spParams
    {
        public int? HourlyExpenseID { get; set; }
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? PersonID { get; set; }
        public DateTime? HourlyExpenseDate { get; set; }
        public int? HourlyExpenseTypeCodeID { get; set; }
        public decimal? HourlyExpenseAmount { get; set; }
        public int? HourlyExpenseProviderCodeID { get; set; }
        public string HourlyExpenseDescription { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
