using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Conflict
{
    public class pd_ConflictInsert_spParams
    {
        public int? AgencyID { get; set; }
        public int? RoleID { get; set; }
        public string ConflictDate { get; set; }
        public int? ConflictTypeCodeID { get; set; }
        public int? ConflictStatusCodeID { get; set; }
        public DateTime? ConflictStatusDate { get; set; }
        public int StatusByUserID { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
