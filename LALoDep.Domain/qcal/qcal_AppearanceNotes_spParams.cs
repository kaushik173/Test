using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{
    public class qcal_AppearanceNotes_spParams
    {
        public int CaseID { get; set; }
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class qcal_AppearanceNotes_spResult
    {
        public int ReportID { get; set; }
        public int HearingID { get; set; }
        public string HearingDisplay { get; set; }
        public string NoteType { get; set; }
        public string NoteDisplay { get; set; }
        public string SortDate { get; set; }
    }
}
