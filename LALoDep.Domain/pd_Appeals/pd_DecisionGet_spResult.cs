
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Appeals
{
    public class pd_DecisionGet_spResult
    {
        public int DecisionID { get; set; }
        public int? AppealID { get; set; }
        public int? DecisionCodeID { get; set; }
        public string DecisionDate { get; set; }
        public int? AgencyID { get; set; }
        public Int16? RecordStateID { get; set; }
    }
}
