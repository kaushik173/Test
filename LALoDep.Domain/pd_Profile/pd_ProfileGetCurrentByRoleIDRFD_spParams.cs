using System;

namespace LALoDep.Domain.pd_Profile
{
    public class pd_ProfileGetCurrentByRoleIDRFD_spParams
    { 
 
        public int RoleID { get; set; }
        public int RFDID { get; set; }
        public int ProfileTypeCodeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}