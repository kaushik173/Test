using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_JudicialOfficer
{
    public class pd_JudicialOfficerListSearchGet_spParams
    {
        public int? AgencyID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }

    public class pd_JudicialOfficerListSearchGet_spResult
    {
        public int PersonID { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public string RoleStartDate { get; set; }
        public string RoleEndDate { get; set; }
    }
}
