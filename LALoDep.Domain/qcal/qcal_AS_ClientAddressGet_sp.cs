using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{
    public class qcal_AS_ClientAddressGet_spParams
    {
        public int HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class qcal_AS_ClientAddressGet_spResult
    {   
        public string PhoneDisplay { get; set; }
        public string AddressDisplay { get; set; }
    }
}
