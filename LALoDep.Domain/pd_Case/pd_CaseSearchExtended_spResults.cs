using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Case
{
    public class pd_CaseSearchExtended_spResults
    {
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public DateTime? PersonDob{ get; set; }
        public string Sex { get; set; }
        public string Role { get; set; }
        public string CaseNumber { get; set; }
        public string HHSANumber{ get; set; }
        public DateTime? casecloseddate{ get; set; }
        public string LeadAttorney { get; set; }
        public string PetitionDocketNumber { get; set; }
        public int RoleID { get; set; }
        public int PersonID { get; set; }
        public int CaseID { get; set; }
    }
}
