using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Motions
{
    public class pd_PetitionGetByCaseID_spResult
    {
        public int PetitionID { get; set; }
        public DateTime? PetitionFileDate { get; set; }
        public DateTime? PetitionCloseDate { get; set; }
        public string PetitionDocketNumber { get; set; }
        public int PetitionTypeCodeID { get; set; }
        public string PetitionTypeCodeValue { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleID { get; set; }
        public int MotionCount { get; set; }
        public int AppealCount { get; set; }
    }
}
