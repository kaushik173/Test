using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Conflict
{
    public class pd_ConflictGet_spResult
    {
        public int ConflictID { get; set; }
        public int? RoleID { get; set; }
        public string ConflictDate { get; set; }
        public int? ConflictTypeCodeID { get; set; }
        public int? ConflictStatusCodeID { get; set; }
        public string ConflictStatusDate { get; set; }
        public int? StatusByUserID { get; set; }
        public int? AgencyID { get; set; }
        public Int16? RecordStateID { get; set; }
        public string NoteEntry { get; set; }
        public int? NoteID { get; set; }
        public int? NoteEntityCodeID { get; set; }
        public int? NoteEntityTypeCodeID { get; set; }
        public int? NoteTypeCodeID { get; set; }
        public string NoteSubject { get; set; }
    }
}
