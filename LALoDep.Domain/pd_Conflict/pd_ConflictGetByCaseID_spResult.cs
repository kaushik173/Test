using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Conflict
{
    public class pd_ConflictGetByCaseID_spResult
    {
        public string CaseRoleDisplay { get; set; }
        public int? ConflictID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string RoleType { get; set; }
        public string ConflictDate { get; set; }
        public string ConflictType { get; set; }
        public string Status { get; set; }
        public Int32? StatusFlag { get; set; }
        public string StatusDate { get; set; }
        public string StatusBy { get; set; }
        public int? ConflictStatusCodeID { get; set; }
        public string SortDate { get; set; }
    }
}
