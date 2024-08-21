using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_System
{
    public class pd_SystemValueTypeGetAll_spResult
    {
        public int? SystemValueTypeID { get; set; }
        public string SystemValue { get; set; }
        public int? SystemValueTypeCodeTypeID { get; set; }
        public short? SystemValueTypeAdmin { get; set; }
        public string SystemValueTypeDescription { get; set; }
        public string CodeTypeValue { get; set; }
        public int CanEditFlag { get; set; }
    }
}
