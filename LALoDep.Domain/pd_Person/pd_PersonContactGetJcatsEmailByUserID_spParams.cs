using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Person
{
    public class pd_PersonContactGetJcatsEmailByUserID_spParams
    {
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class pd_PersonContactGetJcatsEmailByUserID_spResult
    {
        public string PersonNameFirst { get; set; }
        public string PersonNameLast { get; set; }
        public string Email { get; set; }
    }

}
