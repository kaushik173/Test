
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LALoDep.Domain.qcal
{

    public class qcal_AS_AddNewStaff_spParams
    {
        public string AddMode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? HearingID { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }

    }
    public class qcal_AS_AddNewStaff_spResult
    { 
        public string ErrorMessage { get; set; }
        public int? NewPersonID { get; set; }
        public int? NewPersonNameID { get; set; }
        public int? NewRoleID { get; set; }
    }

    public class qcal_AS_IncarcerationFacilityAddNew_spParams
    {
        public int? AgencyID { get; set; }
        public int? CaseID { get; set; }
        public int? HearingID { get; set; }
        public string FacilityName { get; set; }
        public int? UserID { get; set; }
        public System.Guid BatchLogJobID { get; set; }
        public int? SendEmailFlag { get; set; }
        public string mail_profile_name { get; set; }

    }
    public class qcal_AS_IncarcerationFacilityAddNew_spResult
    {
        
        public int? CodeID { get; set; }
        public string CodeDisplay { get; set; }
       
    }


}