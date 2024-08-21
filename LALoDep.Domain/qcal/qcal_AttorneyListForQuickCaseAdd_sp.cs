using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{
    public class qcal_AttorneyListForQuickCaseAdd_spParams
    {
        public int? AttorneyPersonID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }


    public class qcal_AttorneyListForQuickCaseAdd_spResult
    {
       
       
        public string NameDisplay { get; set; }
        public int? PersonID { get; set; }
        public int? AgencyID { get; set; }
        public int? DefaultFlag { get; set; }
    }
}
