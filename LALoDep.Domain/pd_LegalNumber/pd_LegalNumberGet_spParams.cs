using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_LegalNumber
{
    public class pd_LegalNumberGet_spParams
    {
        public int LegalNumberID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }

    }
}
