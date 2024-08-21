using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_UserGroups;

namespace LALoDep.Models.Administration
{
    public class SystemValueViewModel
    {
        public int? SystemValueTypeCodeTypeID { get; set; }
        public string CodeTypeValue { get; set; }
        public string EncryptedSystemValueTypeCodeTypeID { get; set; }
    }
}