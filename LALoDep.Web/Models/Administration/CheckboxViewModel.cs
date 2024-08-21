using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Administration
{
    public class CheckboxViewModel
    {
        public int Id { get; set; }
        public bool Checked { get; set; }
        public int? JcatsGroupAgencyID{ get; set; }
    }
}