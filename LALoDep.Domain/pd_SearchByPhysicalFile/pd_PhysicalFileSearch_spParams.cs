using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_SearchByPhysicalFile
{
    public class pd_PhysicalFileSearch_spParams
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhysicalFileName1 { get; set; }
        public string PetitionDocketNumber { get; set; }
        public string PhysicalFileName2Operator { get; set; }
        public string PhysicalFileName2 { get; set; }
        public string PhysicalFileName3Operator { get; set; }
        public string PhysicalFileName3 { get; set; }
        public int? AgencyID { get; set; }
        public string SortOption { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
