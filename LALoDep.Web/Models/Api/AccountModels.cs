using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Api
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResult
    {
        public bool IsSuccess { get; set; }
        public string AccessToken { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }

    }

    public class UpdateDeviceTokenModel
    {
        public int? DeleteDeviceTokenID { get; set; }

        public string NewToken { get; set; }
        public string DeviceType { get; set; }
    }
}