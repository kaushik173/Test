using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Case
{
    public class pd_CaseClosePetitionGet_spParams
    {
        public int? CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_CaseClosePetitionGet_spResults
    {
        public int PetitionID { get; set; }
        public string PetitionDocketNumber { get; set; }
        public string Type { get; set; }
        public DateTime? PetitionFileDate { get; set; }
        public int Resulted { get; set; }
    }
}
