using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LALoDep.Core.Custom.Utility;

namespace LALoDep.Models
{
    public class PersonViewModel 
    {
        public int PersonID { get; set; }
        public string PersonNameLast { get; set; }
        public string PersonNameFirst { get; set; }
        public string PersonNameDisplay { get; set; }
    }
}