using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.rpt_Print
{
    public class rpt_ToDoListPrintableVersion_spParams
    {
        public int ActionStatusCodeID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string DueStartDate { get; set; }
        public string DueEndDate { get; set; }
        public int? AssignedToPersonID { get; set; }
        public int? ActionTypeCodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
}
