
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.Advisement
{

    public class AdvisementIUD_spParams
    {
        public string IUD { get; set; }
        public int? AdvisementID { get; set; }
        public int? CaseID { get; set; }
        public int? ClientRoleID { get; set; }
        public int? AttorneyPersonID { get; set; }
        public int? AdvisementCodeID { get; set; }
        public DateTime? AdvisementDateTime { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public int? HearingID { get; set; }
        public DateTime? FileDueDate { get; set; }
        public int? StatusCodeID { get; set; }
        public DateTime? StatusDate { get; set; }
    }

}