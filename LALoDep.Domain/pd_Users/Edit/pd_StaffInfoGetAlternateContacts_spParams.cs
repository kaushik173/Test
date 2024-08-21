using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Users.Edit
{
    public class pd_StaffInfoGetAlternateContacts_spParams
    {
        public int PersonID { get; set; }
        public int IncludePersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
}
