using System;

namespace LALoDep.Domain.PD_PDAction
{
    public class pd_ProfileDelete_spParams
    {
        public int  ID { get; set; }
      
        public string RecordTimeStamp { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}