using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.Agency
{
    public class pd_AgencyCodeInsert_spParams
    {
        public int? AgencyCodeID { get; set; }
        public int? AgencyID { get; set; }
        public int? CodeID { get; set; }
        public short? AgencyCodeSystemUseOnly { get; set; }
        public DateTime? AgencyCodeStartDate { get; set; }
        public DateTime? AgencyCodeEndDate { get; set; }
        public int? AgencyCodeDisplayOrder { get; set; }
        public int? RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
