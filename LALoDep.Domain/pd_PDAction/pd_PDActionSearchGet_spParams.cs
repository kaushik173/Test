using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.PD_PDAction
{
    public class pd_PDActionSearchGet_spParams
    {
        public int? ActionStatusCodeID { get; set; }
        public int? ActionTypeCodeID { get; set; }
        public string ReminderStartDate { get; set; }
        public string ReminderEndDate { get; set; }
        public string DueStartDate { get; set; }
        public string DueEndDate { get; set; }
        public int? CaseID { get; set; }
        public int? AssignedToPersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
