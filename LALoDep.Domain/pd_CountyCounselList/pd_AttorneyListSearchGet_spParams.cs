using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_CountyCounselList
{
    public class pd_AttorneyListSearchGet_spParams
    {
        public int? AgencyID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BarNumber { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
