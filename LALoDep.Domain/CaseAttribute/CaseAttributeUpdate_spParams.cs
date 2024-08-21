using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.CaseAttribute
{
    public class CaseAttributeUpdate_spParams
    {
        public int CaseAttributeID { get; set; }
        public int CaseID { get; set; }
        public int? CaseAttributeTypeID { get; set; }
        public string CaseAttributeGenericValue { get; set; }
        public int? TableID { get; set; }
        public int? RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
