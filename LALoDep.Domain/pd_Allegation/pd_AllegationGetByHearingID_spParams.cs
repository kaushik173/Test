using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Allegation
{
    public class pd_AllegationGetByHearingID_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_AllegationGetByHearingID_spResult
    {
        public int PetitionID { get; set; }
        public string PetitionNumber { get; set; }
        public DateTime? PetitionFileDate { get; set; }
        public string ChildLastName { get; set; }
        public string ChildFirstName { get; set; }
        public int? AllegationID { get; set; }
        public string AllegationTypeCodeValue { get; set; }
        public int? AllegationTypeCodeID { get; set; }
        public string AllegationCount { get; set; }
        public int? AllegationFindingCodeID { get; set; }
        public short? RecordStateID { get; set; }

    }

}
