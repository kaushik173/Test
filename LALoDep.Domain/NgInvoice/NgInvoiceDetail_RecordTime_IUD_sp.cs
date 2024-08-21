
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.NgInvoice
{

    public class NgInvoiceDetail_RecordTime_IUD_spParams
    {
        public string IUD { get; set; }
        public int? NgInvoiceDetailID { get; set; }
        public int? NgInvoiceID { get; set; }
        public int? WorkID { get; set; }
        public decimal? NgInvoiceDetailWorkHours { get; set; }
        public decimal? NgInvoiceDetailWorkHourlyRate { get; set; }
        public decimal? NgInvoiceDetailWorkAmount { get; set; }
        public int? UserID { get; set; }
        public string NgInvoiceDetailWorkAdminComment { get; set; }

    }

}