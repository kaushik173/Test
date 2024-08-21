using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Appeals
{
    public class pd_AppealAttorneyGetByCaseID_spResult
    {
        public int PersonID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string AddressStreet { get; set; }
        public string AddressCity { get; set; }
        public int? AddressStateCodeID { get; set; }
        public int? AddressCountryCodeID { get; set; }
        public string AddressZipCode { get; set; }
        public string AddressHomePhone { get; set; }
        public int? AttorneyTypeID { get; set; }
        public string AttorneyType { get; set; }
        public int? RoleID { get; set; }

    }
}
