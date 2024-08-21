using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_CaseLoad
{
    public class pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3_spParams
    {
        public int PersonID { get; set; }
        public string Casestatus { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string PetitionNumber { get; set; }
        public string RoleType { get; set; }
        public string ParmCaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int? AgencyCountyID { get; set; }
    }
}
