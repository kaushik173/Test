using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.Mobile
{
    public class pd_WorkRoleGetByWorkID_spParams
    {
        public int CaseID { get; set; }
        public int WorkID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_WorkRoleGetByWorkID_spResult
    {
        public int? Selected { get; set; }
        public int? WorkRoleID { get; set; }
        public int? RoleID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public short? RecordStateID { get; set; }
        public int? AllMainPetitionsClosedFlag { get; set; }
        public string AllMainPetitionsDisplay { get; set; }

    }
}
