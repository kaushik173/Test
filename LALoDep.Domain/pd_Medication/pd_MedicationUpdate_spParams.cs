using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Medication
{
    public class pd_MedicationUpdate_spParams
    {
        public int MedicationID { get; set; }
        public int? PersonID { get; set; }
        public int? MedicationCodeID { get; set; }
        public decimal? MedicationDosage { get; set; }
        public int? MedicationFrequencyCodeID { get; set; }
        public DateTime? MedicationStartDate { get; set; }
        public DateTime? MedicationEndDate { get; set; }
        public byte? RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
