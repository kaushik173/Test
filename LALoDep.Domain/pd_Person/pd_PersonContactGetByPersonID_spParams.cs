using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Person
{
    public class pd_PersonContactGetByPersonID_spParams
    {
        public int PersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_PersonContactGetByPersonID_spResult
    {
        public int? PersonContactID { get; set; }
        public int? AgencyID { get; set; }
        public int? PersonID { get; set; }
        public int? PersonContactTypeCodeID { get; set; }
        public string PersonContactInfo { get; set; }
        public Int16? RecordStateID { get; set; }
        public string  ContactInfo { get; set; }

        public string TypeDisplay { get; set; }


    }
}
