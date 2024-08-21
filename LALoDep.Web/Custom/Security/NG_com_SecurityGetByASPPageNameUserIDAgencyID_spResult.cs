using LALoDep.Core.Enums;

namespace LALoDep.Custom.Security
{
    public partial class NG_com_SecurityGetByASPPageNameUserIDAgencyID_spResult
    {
        public LogonStatusCode LogonStatusCodeID { get; set; }
        public SecurityToken SecurityItemID { get; set; }

        public string MVCSecurityCode { get; set; }
    }
}
