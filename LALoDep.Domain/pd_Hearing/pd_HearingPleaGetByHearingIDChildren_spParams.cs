using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
  public  class pd_HearingPleaGetByHearingIDChildren_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
     public  class pd_HearingPleaGetByHearingIDChildren_spResult
     {
         public string PersonNameLast { get; set; }
         public string PersonNameFirst { get; set; }
         public int? PetitionID { get; set; }
         public int? PersonID { get; set; }
         public byte? RoleClient { get; set; }
         public string Role { get; set; }
         public string PetitionType { get; set; }
         public string PetitionFileDate { get; set; }
         public string PetitionDocketNumber { get; set; }
         public string PetitionResult { get; set; }
     }
}
