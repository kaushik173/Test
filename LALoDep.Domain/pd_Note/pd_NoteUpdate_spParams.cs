using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Note
{
    public class pd_NoteUpdate_spParams
    {
        public int? NoteID { get; set; }
        public int? AgencyID { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        public int? EntityPrimaryKeyID { get; set; }
        public int? NoteTypeCodeID { get; set; }
        public string NoteSubject { get; set; }
        public string NoteEntry { get; set; }
        public int? CaseID { get; set; }
        public int? PetitionID { get; set; }
        public int? HearingID { get; set; }
        public short? RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid? BatchLogJobID { get; set; }
    }
}
