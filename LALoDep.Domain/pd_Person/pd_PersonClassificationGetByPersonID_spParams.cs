using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Person
{
    public class pd_PersonClassificationGetByPersonID_spParams
    {
        public int PersonID { get; set; }
        public int? SystemValueTypeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_PersonClassificationGetByPersonID_spResult
    {
        public int PersonClassificationID { get; set; }
        public int? PersonID { get; set; }
        public int? PersonClassificationCodeID { get; set; }
        public string PersonClassificationStartDate { get; set; }
        public string PersonClassificationEndDate { get; set; }
        public int? PersonClassificationEndReasonCodeID { get; set; }
        public short? RecordStateID { get; set; }        
        public string PersonClassification { get; set; }
        public string PersonClassificationEndReason { get; set; }
        public DateTime? SortDate { get; set; }
    }
}
