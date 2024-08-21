using System.Collections.Generic;
using System.Web.Mvc;
using LALoDep.Domain.pd_Profile;
using LALoDep.Domain.pd_Role;

namespace LALoDep.Models.Task
{
    public class EditRfdProfileQuestionViewModel
    {
        public int HearingReportFilingDueID { get; set; }
        public string ControlType { get; set; }
        public string RfdHeader { get; set; }
        public int  QuestionID { get; set; }
        public int ProfileID { get; set; }
        public int RoleID { get; set; }
        public int ProfileTypeID { get; set; }
        public int NoteID { get; set; }
        public string NoteEntry { get; set; }
        public string Question { get; set; }
        public string QuestionHeader { get; set; }
     
        public bool IsFreeFormAnswer { get; set; }
        public string FreeFormAnswer { get; set; }
        public int ProfileAnswerID { get; set; }
        public IEnumerable<SelectListItem> ProfileAnswerList { get; set; }

        public string SelectedRoleIdsForCopyAnswer { get; set; }
       
        public List<pd_RoleGetByCaseIDAndSysValue_spResult> PersonRoleList { get; set; }
        public List<pd_ProfileQuestionGetAllByQuestionIDRoleID_spResult> ProfileAnswerHistoryList { get; set; }
        public string NextQuestionUrl { get; set; }
       
        public EditRfdProfileQuestionViewModel()
        {
            ProfileAnswerList = new List<SelectListItem>();
            PersonRoleList = new List<pd_RoleGetByCaseIDAndSysValue_spResult>();
            ProfileAnswerHistoryList = new List<pd_ProfileQuestionGetAllByQuestionIDRoleID_spResult>();
         

        }
    }
}