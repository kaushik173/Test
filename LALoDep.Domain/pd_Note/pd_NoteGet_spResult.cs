using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Note
{
    public class pd_NoteGet_spResult
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
        public Int16 RecordStateID{ get; set; }
        
        public int? CanEditFlag { get; set; }
        public int? CanDeleteFlag { get; set; }
        public int? CanEditSubjectFlag { get; set; }
        public int? HideNoteTypeFlag { get; set; }
        public int? HideBroadcastNotesFlag { get; set; }
        public int? HideClientsAttachedFlag { get; set; }

    }

}
