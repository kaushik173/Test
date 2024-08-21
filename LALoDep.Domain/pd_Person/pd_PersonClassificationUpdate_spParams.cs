using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Person
{
    public class pd_PersonClassificationUpdate_spParams
    {
        public int? PersonClassificationID { get; set; }
        public int? PersonID { get; set; }
        public int? PersonClassificationCodeID { get; set; }
        public DateTime? PersonClassificationStartDate { get; set; }
        public DateTime? PersonClassificationEndDate { get; set; }
        public int? PersonClassificationEndReasonCodeID { get; set; }
        public int? RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
