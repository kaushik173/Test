using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Role;

namespace LALoDep.Models.Case
{
    public class FileUploadStatus
    {
       
         
        public string name { get; set; }
        public string type { get; set; }
        public int size { get; set; }  
        public string error { get; set; }

       

      
    }
    public class AttachPathViewModel
    {
        public IEnumerable<CaseFileGetPathByCaseID_spResult> CaseFileGetPathByCaseList { get; set; }
        public IEnumerable<CaseFileGetByCaseID_spResult> CaseFileGetByCaseList { get; set; }
        public AttachPathViewModel()
        {
            RoleList = new List<SelectListItem>();
            CategoryList = new List<SelectListItem>();
        }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public int RoleID { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public int CategoryID { get; set; }
        public string FileDate { get; set; }

        public int UseGoogleDriveUpload { get; set; }

        public int SharePoint_UseFlag { get; set; }
        public string SharePoint_URL  { get; set; }

    }
    public class AttachPathEditViewModel
    {
        public AttachPathEditViewModel()
        {
            RoleList = new List<SelectListItem>();
            CategoryList = new List<SelectListItem>();
        }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public int RoleID { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public int CategoryID { get; set; }
        public string FileDate { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public byte RecordStateID { get; set; }
        public int CaseFileID { get; set; }
       
    }



}