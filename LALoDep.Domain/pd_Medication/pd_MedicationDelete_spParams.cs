using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Medication
{
    public class pd_MedicationDelete_spParams
    {
        public int ID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string LoadOption { get; set; }
        public byte? RecordStateID { get; set; }
    }
}
