using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LALoDep.Models.CaseOpening
{
 
    public class AddAppointmentCaseSearchViewModel
    {

        public int AgencyID { get; set; }
        public IEnumerable<SelectListItem> AgencyList { get; set; }

        public string CaseNumber { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool PanelCase { get; set; }
        public int DepartmentID { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public int ReferralSourceID    { get; set; }
        public IEnumerable<SelectListItem> ReferralSourceList { get; set; }
        public string MotherLastName { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherDOB { get; set; }
        public int AddNew { get; set; }

        public string CaseNumber1 { get; set; }
 
        public string CaseNumber2 { get; set; }
        
        public string CaseNumber3 { get; set; }

        public string ChildFirstName1 { get; set; }
        public string ChildLastName1 { get; set; }
        public string Child1DOB { get; set; }


        public string ChildFirstName2 { get; set; }
        public string ChildLastName2 { get; set; }
        public string Child2DOB { get; set; }


        public string ChildFirstName3 { get; set; }
        public string ChildLastName3 { get; set; }
        public string Child3DOB { get; set; }




        public string FatherFirstName1 { get; set; }
        public string FatherLastName1 { get; set; }
        public string Father1DOB { get; set; }

        public string FatherFirstName2 { get; set; }
        public string FatherLastName2 { get; set; }
        public string Father2DOB { get; set; }
        public AddAppointmentCaseSearchViewModel()
        {
            AgencyList = new List<SelectListItem>();
            DepartmentList = new List<SelectListItem>();
            ReferralSourceList = new List<SelectListItem>();
        }
    }
    [Serializable]
    public class AddAppointmentCaseSearchViewModelForSession
    {

        public int AgencyID { get; set; }
        public string CaseNumber { get; set; }
        public DateTime AppointmentDate { get; set; }
        public bool PanelCase { get; set; }
        public int DepartmentID { get; set; }
         public int ReferralSourceID { get; set; }
         public string MotherLastName { get; set; }
        public string MotherFirstName { get; set; }
        public string MotherDOB { get; set; }
        public int AddNew { get; set; }

        public string CaseNumber1 { get; set; }

        public string CaseNumber2 { get; set; }

        public string CaseNumber3 { get; set; }

        public string ChildFirstName1 { get; set; }
        public string ChildLastName1 { get; set; }
        public string Child1DOB { get; set; }


        public string ChildFirstName2 { get; set; }
        public string ChildLastName2 { get; set; }
        public string Child2DOB { get; set; }


        public string ChildFirstName3 { get; set; }
        public string ChildLastName3 { get; set; }
        public string Child3DOB { get; set; }




        public string FatherFirstName1 { get; set; }
        public string FatherLastName1 { get; set; }
        public string Father1DOB { get; set; }

        public string FatherFirstName2 { get; set; }
        public string FatherLastName2 { get; set; }
        public string Father2DOB { get; set; }
       
    }

}          
   
