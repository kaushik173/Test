﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Note

{
    public class pd_NoteGetByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
