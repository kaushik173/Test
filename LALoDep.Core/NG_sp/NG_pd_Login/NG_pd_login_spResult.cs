using LALoDep.Core.Enums;

namespace LALoDep.Core.NG_pd_login_spResult
{
    public class NG_pd_login_spResult
    {
        public LoginStatus? LoginSuccessful { get; set; }
        public int? UserID { get; set; }
        public string LoginKey { get; set; }
        public int? InitialLogin { get; set; }
        public int? AgencyID { get; set; }
        public int? BranchID { get; set; }
        public int? PersonID { get; set; }
        public int? StaffID { get; set; }
        public string PasswordMessage { get; set; }
        public string WorkEmail { get; set; }
        public string InitialURL { get; set; }
    }
    public class NG_pd_LoginCheckSuccess_spResult
    {
        public LoginStatus StatusCode { get; set; }
       
        public string Status { get; set; }
        public string StatusDisplay { get; set; }

    }
}
