using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Case
{
    public class pd_CaseTransfer_spParams
    {
        public int? CaseID { get; set; }
        public int FromPersonID { get; set; }
        public int TransferToPersonID { get; set; }
        public DateTime TransferDate { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
