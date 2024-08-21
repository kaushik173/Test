using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pD_SCCInvoice
{
   public class pd_SCCInvoiceRateTypeGetAll_spParams
    {
        public int SCCInvoiceRateID{ get; set; }
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
   public class pd_SCCInvoiceRateTypeGetAll_spResult
   {
       public int? SCCInvoiceRateID { get; set; }
       public int? AgencyID { get; set; }
       public int? SCCInvoiceRateTypeCodeID { get; set; }
       public decimal? SCCInvoiceRateAmount { get; set; }
       public DateTime? SCCInvoiceRateStartDate { get; set; }
       public DateTime? SCCInvoiceRateEndDate { get; set; }
       public string SCCInvoiceRateType { get; set; }
       public string SCCInvoiceRateTypeShort { get; set; }

   }
}
