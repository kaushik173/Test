using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.BatchHearing
{
    public class pd_BatchHearingSearch_spResult
    {
        public int? CaseID{ get; set; }
        public int? PetitionID{ get; set; }
        public int? PetitionTypeCodeID{ get; set; }
        public string Agency{ get; set; }
        public string PetitionType { get; set; }
        public string CaseNumber{ get; set; }
        public string PetitionDocketNumber{ get; set; }
        public string PetitionFileDate { get; set; }
        public string Department{ get; set; }
        public string Client{ get; set; }
        public string ChildName { get; set; }
        public string NextHearing{ get; set; }
        public string NextHearingType { get; set; }
        public int? TotalRecords { get; set; }
        public int? TotalCases { get; set; }
    }
}
