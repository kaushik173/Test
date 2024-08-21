
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.pd_Work
{

    public class WorkTimeIUD_spParams
    {
        public string IUD { get; set; }
        public int? WorkTimeID { get; set; }
        public int? WorkID { get; set; }
        public DateTime? WorkTimeStart { get; set; }
        public DateTime? WorkTimeEnd { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class WorkZipCodeIUD_spParams
    {
        public string IUD { get; set; }
        public int? WorkZipCodeID { get; set; }
        public int? WorkID { get; set; }
        public string WorkZipCodeFrom { get; set; }
        public string WorkZipCodeTo { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
}