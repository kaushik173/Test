using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Case
{
    public class AKAsAddEditViewModel
    {
        public int? PersonNameID { get; set; }
        public int AgencyID { get; set; }
        public int PersonID { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public int PersonNameTypeCodeID { get; set; }
        public string PersonNameSoundex { get; set; }
        public string PersonNameMiddle{ get; set; }
        public int RecordStateID{ get; set; }
    }
}