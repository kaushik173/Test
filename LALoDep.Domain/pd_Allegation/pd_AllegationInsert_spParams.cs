using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Allegation
{
    public class pd_AllegationInsert_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int AllegationID { get; set; }
        public int AgencyID { get; set; }
        public int PetitionID { get; set; }
        public int? AllegationTypeCodeID { get; set; }
        public int? AllegationFindingCodeID { get; set; }
        public string AllegationIdentifier { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }
        public int? CopyNoteID
        { get; set; }




}
    public class pd_AllegationUpdate_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int AllegationID { get; set; }
        public int AgencyID { get; set; }
        public int PetitionID { get; set; }
        public int? AllegationTypeCodeID { get; set; }
        public int? AllegationFindingCodeID { get; set; }
        public string AllegationIdentifier { get; set; }
        public int RecordStateID { get; set; }
        public string RecordTimeStamp { get; set; }


    }
    public class pd_AllegationDelete_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int ID { get; set; }
     
        public string RecordTimeStamp { get; set; }


    }
     
 
}
