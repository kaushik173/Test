﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Note
{
    public class pd_EditAddNote_spParams

    {
        public int NoteID { get; set; }
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int CaseAgencyID { get; set; }
    }

}
