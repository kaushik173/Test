using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Users.Edit
{
    public class pd_AgencyGetHomeByPersonID_spResult
    {
        public int AgencyID { get; set; }
        public int AgencyGroupID { get; set; }
        public string AgencyName { get; set; }
        public int HomeAgencyFlag { get; set; }
    }
}
