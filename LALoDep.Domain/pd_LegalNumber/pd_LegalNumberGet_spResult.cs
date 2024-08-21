using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_LegalNumber
{
    public class pd_LegalNumberGet_spResult
    {
        public int LegalNumberID { get; set; }
        public int AgencyID { get; set; }
        public int PersonID { get; set; }
        public int LegalNumberTypeCodeID { get; set; }
        public string LegalNumberEntry { get; set; }
        public Int16 RecordStateID { get; set; }
        public string LegalNumberTypeCodeValue { get; set; }
        public string LegalNumberComment{ get; set; }
    }
}
