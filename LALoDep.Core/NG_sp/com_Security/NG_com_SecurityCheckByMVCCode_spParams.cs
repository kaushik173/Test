using System;

namespace LALoDep.Core.NG_sp.com_Security
{
    public class NG_com_SecurityCheckByMVCCode_spParams
    {
        public string MVCSecurityCode { get; set; }
        public Guid LoginKey { get; set; }
        public int UserID { get; set; }
        public int AgencyID { get; set; }
        public string ClientAddress { get; set; }
        public string ClientHost { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
