
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.qcal
{


    public class ClientStatusIUD_spParams
    {
        public string IUD { get; set; }
        public int? CS_ID { get; set; }
        public int? CS_PersonID { get; set; }
        public int? CS_CodeID { get; set; }
        public DateTime? CS_StartDate { get; set; }
        public int? HearingID { get; set; }
        public DateTime? HearingDateTime { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }

}