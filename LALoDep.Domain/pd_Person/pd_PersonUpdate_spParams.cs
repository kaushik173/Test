using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Person
{
    public class pd_PersonUpdate_spParams
    {
        public int PersonID { get; set; }
        public int? AgencyID { get; set; }
        public DateTime? PersonDOB { get; set; }
        public int? PersonRaceCodeID { get; set; }
        public int? PersonSexCodeID { get; set; }
        public int? RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int? UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? PersonLanguageCodeID { get; set; }
    }
}
