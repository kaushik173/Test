﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Petition
{
    public class pd_PetitionDelete_spParams
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public int RecordStateID{ get; set; }
        public string LoadOption { get; set; }
        public string RecordTimeStamp { get; set; }
    }
}
