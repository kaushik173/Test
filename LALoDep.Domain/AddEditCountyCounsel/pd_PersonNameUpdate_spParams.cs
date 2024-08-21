using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.AddEditCountyCounsel
{
    public class pd_PersonNameUpdate_spParams
    {
        public int PersonNameID { get; set; }
        public int? AgencyID { get; set; }
        public int PersonID { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameMiddle { get; set; }
        public int PersonNameTypeCodeID { get; set; }
        public string PersonNameSoundex { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int PersonNameSalutationCodeID { get; set; }
        public int PersonNameSuffixCodeID { get; set; }
    }
}
