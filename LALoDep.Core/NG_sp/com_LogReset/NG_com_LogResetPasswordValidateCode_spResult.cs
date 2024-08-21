using System;

namespace LALoDep.Core.NG_sp.com_LogReset
{
    public class NG_com_LogResetPasswordValidateCode_spResult
    {

        public string Username { get; set; }
        public string EmailId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsDone { get; set; }
    }
}
