using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_CaseLoad
{
    public class pd_CaseloadGetByStaff1_spResults
    {
        public int PersonID { get; set; }
        public string RoleType { get; set; }
        public int RoleTypeCodeID { get; set; }
        public string PersonNameDisplay { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public int? ChildClientCount { get; set; }
        public int? OtherClientCount { get; set; }
        public int? PetitionCount { get; set; }
        public int? CaseCount { get; set; }
    }
}
