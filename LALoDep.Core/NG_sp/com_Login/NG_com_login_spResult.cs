using LALoDep.Core.Enums;

namespace LALoDep.Core.NG_sp.com_Login
{
    public class NG_com_login_spResult
    {
        public LoginStatus LoginStatus { get; set; }
        public int? UserID { get; set; }
        public string LoginKey { get; set; }
        public int InitialLoginFlag { get; set; }
        public int? AgencyID { get; set; }
        public int? BranchID { get; set; }
        public int? PersonID { get; set; }
        public int? StaffID { get; set; }
        public string PasswordMessage { get; set; }
        public string WorkEmail { get; set; }
        public string LandingPage { get; set; }
    }
}
