using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Allegation
{
    public class pd_AllegationGetByPetitionID_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int PetitionID { get; set; }
  
    }
    public class pd_AllegationGetByPetitionID_spResult
    {
        public int AllegationID { get; set; }
        public int AgencyID { get; set; }
        public int PetitionID { get; set; }
        public int? AllegationTypeCodeID { get; set; }
        public int? AllegationFindingCodeID { get; set; }
        public string AllegationIdentifier { get; set; }
        public short RecordStateID { get; set; }
      public int? Selected { get; set; }
        public string AllegationTypeCodeValue { get; set; }
        public int? NoteID { get; set; }
        public string NoteEntry { get; set; }



    }
}
