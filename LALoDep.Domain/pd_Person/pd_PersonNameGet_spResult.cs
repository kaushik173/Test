
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Person
{
    public class pd_PersonNameGet_spResult
    {
        public int? PersonNameID { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameMiddle { get; set; }
        public Int16? RecordStateID { get; set; }
        public int? PersonNameTypeCodeID { get; set; }
        public string PersonNameSoundex { get; set; }
    }
}
