using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models
{


    public class SaveNoteViewModel
    {
        public int? EntityPrimaryKeyID { get; set; }
        public int? NoteID { get; set; }
        public string NoteSubject { get; set; }
        public string NoteEntry { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        public int? NoteTypeCodeID { get; set; }

    }
}