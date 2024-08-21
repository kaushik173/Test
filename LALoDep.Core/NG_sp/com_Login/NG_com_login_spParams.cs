using System;

namespace LALoDep.Core.NG_sp.com_Login
{
    public class NG_com_login_spParams
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public byte AdminFlag { get; set; }
        public Guid LoginKey { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int AvailWidth { get; set; }
        public int AvailHeight { get; set; }
        public string ClientInfo { get; set; }
        public string ClientAddress { get; set; }
        public string ClientHost { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
