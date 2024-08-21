using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Medication
{
    public class pd_MedicationGetByPersonID_spParams
    {
        public int PersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_MedicationGetByPersonID_spResult
    {
        public int MedicationID { get; set; }
        public int? PersonID { get; set; }
        public int? MedicationCodeID { get; set; }
        public decimal? MedicationDosage { get; set; }
        public int? MedicationFrequencyCodeID { get; set; }
        public string MedicationStartDate { get; set; }
        public string MedicationEndDate { get; set; }
        public byte? RecordStateID { get; set; }        
        public string MedicationFrequency { get; set; }
    }
}
