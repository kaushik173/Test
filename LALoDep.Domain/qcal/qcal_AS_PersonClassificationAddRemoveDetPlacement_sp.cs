using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{
    public class qcal_AS_PersonClassificationAddRemoveDetPlacement_spParams
    {
        public int? DetPlacementPersonClassificationID { get; set; }
        public int? PersonID { get; set; }
        public int? DetPlacementCodeID { get; set; }
        public DateTime? DetPlacementStartDate { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int?  DetPlacementHearingID { get; set; }
        public string  DetPlacementComment { get; set; }
        
 
    }
}
