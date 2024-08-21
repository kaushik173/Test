using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models
{
    public class CodeSelectedViewModel
    {
        public int CodeID { get; set; }
        public int CodeTypeID { get; set; }
        public string CodeValue { get; set; }
        public string CodeShortValue { get; set; }
        public bool IsSelected { get; set; }

    }
}