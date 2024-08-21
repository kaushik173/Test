using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Note
{
    public class pd_NotePanelGetAllByNoteID_spParams
    {

        public int NoteID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }

    public class pd_NoteGetByRFDIDSystemValueTypeID_spParams
    {


        public int RFDID { get; set; }
        public int SystemValueTypeID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
    public class pd_NoteGetByRFDIDSystemValueTypeID_spResult
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
        public byte[] RecordTimeStamp { get; set; }
        public System.Nullable<System.DateTime> InsertedOnDateTime { get; set; }
        public int? InsertedByUserID { get; set; }
        public System.Nullable<System.DateTime> UpdatedOnDateTime { get; set; }
        public int? UpdatedByUserID { get; set; }
        public string NoteTypeCodeValue { get; set; }
        public int? CodeID { get; set; }
        public int? RequiredFlag { get; set; }
        public int? DisabledFlag { get; set; }
        
    }

}
