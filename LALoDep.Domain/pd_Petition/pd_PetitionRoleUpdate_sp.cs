
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.pd_Petition
{

    public class pd_PetitionRoleUpdate_spParams
    {
        public int? PetitionRoleID { get; set; }
        public DateTime? PetitionRoleStartDate { get; set; }
        public DateTime? PetitionRoleEndDate { get; set; }
        public int? UserID { get; set; }

    }

}