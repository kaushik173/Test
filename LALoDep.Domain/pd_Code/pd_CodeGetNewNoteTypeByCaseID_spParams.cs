using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Code
{
    public class pd_CodeGetNewNoteTypeByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int CaseAgencyID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_CodeGetNewNoteTypeByCaseID_spResult
    {
        public string NoteTypeDisplay { get; set; }
        public int CodeID { get; set; }
 
    }
}