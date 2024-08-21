using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Note

{
    public class pd_NoteGetByCaseID_spResult
    {
        public int NoteID { get; set; }
        public string NoteSubject { get; set; }
        public string NoteEntry { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string NoteTypeCodeValue { get; set; }
        public string NoteDate { get; set; }
        public int NoteLinkFlag { get; set; }
        public int AllowDeleteFlag { get; set; }
        public int? IsRTF { get; set; }

    }
}
