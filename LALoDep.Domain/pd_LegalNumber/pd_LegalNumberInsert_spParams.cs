using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_LegalNumber
{
    public class pd_LegalNumberInsert_spParams
    {
        public int? LegalNumberID { get; set; }
        public int? AgencyID { get; set; }
        public int PersonID { get; set; }
        public int LegalNumberTypeCodeID { get; set; }
        public string LegalNumberEntry { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string LegalNumberComment { get; set; }
        
    }
}
