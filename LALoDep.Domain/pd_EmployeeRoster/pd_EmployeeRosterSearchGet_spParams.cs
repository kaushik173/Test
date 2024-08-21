using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_EmployeeRoster
{
    public class pd_EmployeeRosterSearchGet_spParams
    {
        public int? AgencyID { get; set; }
        public int StaffPositionCodeID { get; set; }
        public string LastName { get; set; }
        public string FirstName{ get; set; }
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
