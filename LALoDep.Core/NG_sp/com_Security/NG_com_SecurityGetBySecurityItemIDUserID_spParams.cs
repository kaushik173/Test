using System;

namespace LALoDep.Core.NG_sp.com_Security
{
    public class NG_com_SecurityGetBySecurityItemIDUserID_spParams
    { 
        public string ASPPageName { get; set; }
        public string LoginKey { get; set; }
        public string ClientAddress { get; set; }
        public string ClientHost { get; set; }
        public int SecurityItemID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
} 