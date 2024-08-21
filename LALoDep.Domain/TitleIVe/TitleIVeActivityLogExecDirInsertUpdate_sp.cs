
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.TitleIVe
{

    public class TitleIVeActivityLogExecDirInsertUpdate_spParams
    {
        public int? ErrorID { get; set; }
        public int? UserID { get; set; }
        public int? ActivityLogID { get; set; }
        public int? AgencyID { get; set; }
        public int? AgencyCountyID { get; set; }
        public int? ActivityYear { get; set; }
        public int? ActivityMonth { get; set; }
        public int? PersonID { get; set; }
        public decimal? NonDependPercent_ExecDir { get; set; }
        public decimal? TimeOffPercent_ExecDir { get; set; }
        public DateTime? DateSignedEmployee { get; set; }

    }

}