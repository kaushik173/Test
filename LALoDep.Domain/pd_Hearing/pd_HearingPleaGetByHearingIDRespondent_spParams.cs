using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Hearing
{
   public class pd_HearingPleaGetByHearingIDRespondent_spParams
    {
       public int HearingID { get; set; }
       public int UserID         {get;set;}
       public Guid BatchLogJobID  {get;set;}
    }

   public class pd_HearingPleaGetByHearingIDRespondent_spResult
    {
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public int? PersonID { get; set; }
        public int? PetitionRoleID { get; set; }
        public int? PetitionID { get; set; }
        public string RoleType { get; set; }
        public byte? RoleClient { get; set; }
        public int? PleaID { get; set; }
        public int? PleaTypeID { get; set; }
        public string PleaType { get; set; }
    }
}
