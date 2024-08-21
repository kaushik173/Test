using System;

namespace LALoDep.Domain.pd_Role
{
    public class pd_RoleGetByCaseIDAndSysValue_spParams
    {

        public int CaseID { get; set; }
        public int  SysVal { get; set; }
   
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}