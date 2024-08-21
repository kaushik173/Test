using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Person
{
    public class pd_PersonGet_spParams
    {
        public int PersonID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }
    public class pd_PersonGet_spResult
    {

        public int PersonID { get; set; }
        public int AgencyID { get; set; }
        public System.DateTime? PersonDOB { get; set; }
        public int? PersonRaceCodeID { get; set; }
        public int? PersonSexCodeID { get; set; }
        public short RecordStateID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Sex { get; set; }
        public string Race { get; set; }
        public string Email { get; set; }
        public int? PersonNameID { get; set; }
        public string Language { get; set; }
        public int? PersonNameTypeCodeID { get; set; }
        public int? PersonLanguageCodeID { get; set; }
        public int? RoleAgencyID { get; set; }
        public System.DateTime? DeceasedDate { get; set; }
        public int? DeceasedDate_PersonClassID { get; set; }
    }
}
