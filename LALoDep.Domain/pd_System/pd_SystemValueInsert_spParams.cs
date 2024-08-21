using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_System
{
    public class pd_SystemValueInsert_spParams
    {
        public int? SystemValueTypeID { get; set; }
        public int? SystemValueCodeID { get; set; }
        public int? SystemValueSequence { get; set; }
        public int? RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
