using System.Collections.Generic;
using LALoDep.Domain.pd_LegalNumber;
using LALoDep.Domain.pd_Role;

namespace LALoDep.Models.CaseOpening
{
    public class LegalNumberModel
    {

        public IEnumerable<LegalNumberRoleAddList> RoleList { get; set; }

        public IEnumerable<pd_LegalNumberGetByCaseID_spResult> LegalNumberList { get; set; }

        public LegalNumberModel()
        {
            RoleList = new List<LegalNumberRoleAddList>();
            LegalNumberList = new List<pd_LegalNumberGetByCaseID_spResult>();


        }
    }

    public class LegalNumberRoleAddList 
    {
        public byte RoleClient { get; set; }
        public int PersonID { get; set; }
        public string PersonName { get; set; }
        public string Role { get; set; }

        public string CCNumber { get; set; }

        public string HHSANumber { get; set; }

        public string SSNumber { get; set; }

    }
}