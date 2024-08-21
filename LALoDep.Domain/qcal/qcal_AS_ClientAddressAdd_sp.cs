using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{
    public class qcal_AS_ClientAddressAdd_spParams
    {
        public int HearingID { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int? StateCodeID { get; set; }
        public int? CountryCodeID { get; set; }
        public string ZipCode { get; set; }
        public string HomePhone { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
