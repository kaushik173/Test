using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LALoDep.Models.Administration
{
    public class AddEditCountyCounselViewModel
    {
        public bool OnViewLoad { get; set; }
        public CountyCounselPerson Person { get; set; }
    }

    public class CountyCounselPerson
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PFirstName { get; set; }
        public string PLastName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string BarNumber { get; set; }
        public string PBarNumber { get; set; }
        public Int16 RecordStateID { get; set; }
        public int RoleID { get; set; }
        public int AgencyID { get; set; }

        public int PersonId { get; set; }

    }

    public class CountyCounselAgencyCheckbox
    {
        public int AgencyID { get; set; }
        public string AgencyName { get; set; }
        public bool Selected { get; set; }
        public bool Changed { get; set; }
        public int RoleID { get; set; }
        public string RoleStartDate { get; set; }
    }
}