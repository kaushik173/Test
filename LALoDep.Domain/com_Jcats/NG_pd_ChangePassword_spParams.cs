using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.com_Jcats
{
    public class NG_pd_ChangePassword_spParams
    {
        public int ChangeUserID { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public byte ResetFlag { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
