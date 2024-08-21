using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleAgencyAttorneyGet_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_RoleAgencyAttorneyGet_spResult
    {
        public string PersonNameDisplay { get; set; }
        public int PersonID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int RoleTypeID { get; set; }
        public int AgencyID { get; set; }
    }
}
