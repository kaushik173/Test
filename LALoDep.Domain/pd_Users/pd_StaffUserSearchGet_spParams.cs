using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Users
{
    public class pd_StaffUserSearchGet_spParams
    {
        public int? AgencyID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? JcatsGroupID { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public byte ActiveUserOnly { get; set; }
        public byte OpenPositionsOnly { get; set; }
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_ChangePassword_spParams
    {
        public int  ChangeUserID { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public short ResetFlag { get; set; }
       
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_ChangePassword_spResult
    {
        public string Status { get; set; }
        public string StatusMessage { get; set; }
      
    }
    
}
