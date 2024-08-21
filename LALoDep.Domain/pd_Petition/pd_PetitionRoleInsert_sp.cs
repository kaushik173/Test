
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.pd_Petition
{

    public class pd_PetitionRoleInsert_spParams
    {
        public int? PetitionRoleID { get; set; }
        public int? AgencyID { get; set; }
        public int? PetitionID { get; set; }
        public int? RoleID { get; set; }
        public int? RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public DateTime? PetitionRoleStartDate { get; set; }
        public DateTime? PetitionRoleEndDate { get; set; }

    }

}