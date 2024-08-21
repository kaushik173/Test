using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_SearchByPhysicalFile
{
    public class pd_PhysicalFileSearch_spResult
    {
        public int MaxCount { get; set; }
        public string PhysicalFileName { get; set; }
        public string Client { get; set; }
        public string DOB { get; set; }
        public string Role { get; set; }
        public byte RoleClient { get; set; }
        public int RoleID { get; set; }
        public string CaseNumber { get; set; }
        public string PetitionCloseDate { get; set; }
        public string PetitionDocketNumber { get; set; }
        public string Attorney { get; set; }
        public int CaseID { get; set; }
        public int AgencyID { get; set; }
    }
}
