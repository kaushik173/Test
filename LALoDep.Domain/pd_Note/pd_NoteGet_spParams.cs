using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Note
{
    public class pd_NoteGet_spParams
    {
        public int NoteID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int UserID { get; set; }
    }

}
