using LALoDep.Domain.pd_CopyCase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Models.Case
{
    public class CopyCaseTransferViewModel
    {
   
        public IEnumerable<SelectListItem> TransferToList { get; set; }
      
        public IEnumerable<pd_CopyCaseTransferSubsetClients_spResult> ClientList { get; set; }
        
        

        public string TransferDate { get; set; }
        
        public string TransferToID { get; set; }
        public string IncludeClientPersonIDList { get; set; }
        public string ExcludeClientPersonIDList { get; set; }
       

    }

}