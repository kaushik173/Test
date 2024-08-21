
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.qcal
{

    public class qcal_AS_ERH_AddNewCaseRole_spParams
    {
        public int? CaseAgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime? StartDate { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_ERH_AddNewCaseRole_spResult
    {
        public int? NewPersonID { get; set; }
        public int? NewRoleID { get; set; }
      

    }

}