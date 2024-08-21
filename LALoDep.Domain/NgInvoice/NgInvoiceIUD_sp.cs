
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.NgInvoice
{
                         
           public class NgInvoiceIUD_spParams {
  public string IUD { get; set; }
  public int? NgInvoiceID { get; set; }
  public int? AgencyID { get; set; }
  public int? CaseID { get; set; }
  public int? YearQuarterID { get; set; }
  public int? ContractorPersonID { get; set; }
  public string NgInvoiceNote { get; set; }
  public int? NgInvoiceStatusID { get; set; }
  public int? NgInvoiceStatusCodeID { get; set; }
  public string SaveAction { get; set; }
  public int? UserID { get; set; }
        public string  NGInvoiceAdminNote { get; set; }

    }

}