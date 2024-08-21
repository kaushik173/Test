using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pD_SCCInvoice
{
   public class pd_SCCInvoiceClientGetByCaseID_spResult
    {

        public string ClientName { get; set; }
        public string ClientDisplay { get; set; }
        public int? RoleID { get; set; }
        public int? RoleTypeCodeID { get; set; }
        public int? ActiveFlag { get; set; }
        public int? Selected { get; set; }
        public int? SCCInvoiceClientID { get; set; }
        public int? SCCInvoiceID { get; set; }
        public Int16? SCCInvoiceClient_RecordStateID { get; set; }
        public string PetitionDate { get; set; }
        public string PetitionDocketNumber { get; set; }
        public string PetitionCloseDate { get; set; }
    }
}
