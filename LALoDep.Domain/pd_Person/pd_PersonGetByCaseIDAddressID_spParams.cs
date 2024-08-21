using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Person
{
    public class pd_PersonGetByCaseIDAddressID_spParams
    {
        public int CaseID { get; set; }
        public int AddressID     {get;set;}
        public int UserID        {get;set;}
        public Guid BatchLogJobID {get;set;}
    }
    public class pd_PersonGetByCaseIDAddressID_spResult
    {
        public int PersonID { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }

}
