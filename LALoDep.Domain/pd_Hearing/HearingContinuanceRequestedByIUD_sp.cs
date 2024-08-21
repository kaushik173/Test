
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace LALoDep.Domain.pd_Hearing
{

    public class HearingContinuanceRequestedByIUD_spParams
    {
        public string IUD { get; set; }
        public int? HearingContinuanceRequestedByID { get; set; }
        public int? HearingID { get; set; }
        public int? CodeID { get; set; }
        public int? UserID { get; set; }

    }

}