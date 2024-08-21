using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.qcal
{
    public class qcal_AS_HearingCaseFileGetList_spParams
    {
        public int? HearingID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
    }

    public class qcal_AS_HearingCaseFileGetList_spResult
    {
        public string HearingInfo { get; set; }
        public int? OnHearingFlag { get; set; }
        public int? HearingCaseFileID { get; set; }
        public int? CaseFileID { get; set; }
        public int? HearingID { get; set; }
        public int? CaseID { get; set; }
        public string Category { get; set; }
        public string FileDisplay { get; set; }
        public string Description { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedOn { get; set; }
        public string FilePath { get; set; }
        public DateTime? SortBy { get; set; }
        public string SharePointFile_URL_PROD { get; set; }
        public string SharePointFile_URL_TEST { get; set; }
        public string KamiUrl { get; set; }
        public int? SharePointFile_AllowDownloadLink { get; set; }
        public string SharePointFileID { get; set; }

        //custom
        public int SharePoint_UseFlag { get; set; }
        public string SharePoint_FilePath { get; set; }
        public string DownloadPath { get; set; }
        public string GoogleFileID { get; set; }
        public string GoogleFolderID { get; set; }
        public int UseGoogleDocsFlag { get; set; }

    }
}
