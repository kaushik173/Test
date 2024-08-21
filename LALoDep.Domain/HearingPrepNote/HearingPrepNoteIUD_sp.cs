
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.HearingPrepNote
{

    public class HearingPrepNoteIUD_spParams
    {
        public string IUD { get; set; }
        public int? NoteID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public string NoteEntry { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

}