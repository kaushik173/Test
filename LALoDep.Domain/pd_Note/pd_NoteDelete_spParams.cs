﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Conflict
{
    public class pd_NoteDelete_spParams
    {
        public int ID { get; set; }
        public string RecordTimeStamp{ get; set; }
        public string LoadOption{ get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int RecordStateID{ get; set; }
    }
    public class pd_Delete_spParams
    {
        public int ID { get; set; }
        public string RecordTimeStamp { get; set; }
     
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
     
    }
}
