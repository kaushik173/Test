using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain.pd_Case
{
    public class CaseFileGetByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string SortOption { get; set; }


        public string EntityType { get; set; }
        public int EntityID { get; set; }



    }
    public class CaseFileGetByCaseID_spResult
    {
        public int? CaseFileID { get; set; }
        public int? CaseID { get; set; }
        public int? RoleID { get; set; }
        public string CaseFileDate { get; set; }
        public string CaseFileName { get; set; }
        public string CaseFileNameDisplay { get; set; }
        public string CaseFileDescription { get; set; }
        public string CaseFilePath { get; set; }
        public string CaseFileURL { get; set; }
        public int? CaseFileDisplayOrder { get; set; }
        public long? CaseFileSizeInBytes { get; set; }
        public byte? RecordStateID { get; set; }
        public decimal RecordTimeStamp { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedOn { get; set; }
        public int? RemovableFlag { get; set; }
        public int? ViewableFlag { get; set; }
        public string CaseNumber { get; set; }
        public string RoleDisplay { get; set; }
        public string Category { get; set; }
        public string SortDate { get; set; }
        public string EncryptFilePath { get; set; }
        public string GoogleFileID { get; set; }
        public string GoogleFolderID { get; set; }
        public int? CaseFileID_LastestByCurrentUser { get; set; }
        public string CaseFileName_LastestByCurrentUser { get; set; }
        public string DownloadPath { get; set; }
        public string SharePointFile_URL_PROD { get; set; }
        public string SharePointFile_URL_TEST { get; set; }
        public string KamiUrl { get; set; }
        public int? SharePointFile_AllowDownloadLink { get; set; }
        public string SharePointFileID { get; set; }


        public string SharePoint_FilePath { get; set; }

    }
    public class CaseFileGetPathByCaseID_spParams
    {
        public int CaseID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string ServerName { get; set; }
        public string RootPath { get; set; }
        public string EntityType { get; set; }
        public int EntityID { get; set; }



    }
    public class CaseFileGetPathByCaseID_spResult
    {

        public string FullPath { get; set; }
        public string RootPath { get; set; }
        public string SubRootPath { get; set; }
        public int? CaseID { get; set; }
        public string AttachFileMyDocumentsDirectory { get; set; }
        public byte? AttachFileDeleteFileAfterUploadFlag { get; set; }
        public int? UseGoogleDocsFlag { get; set; }

        public string GoogleFolder_PROD { get; set; }

        public string GoogleFolder_TEST { get; set; }

        public string GoogleFolderID_PROD { get; set; }
        public string GoogleFolderID_TEST { get; set; }


        public int? SharePoint_UseFlag { get; set; }
        public string SharePoint_TenantID { get; set; }
        public string SharePoint_ClientID { get; set; }
        public string SharePoint_CertificatePath { get; set; }
        public string SharePoint_CertificatePassword { get; set; }
        public string SharePoint_URL_PROD { get; set; }
        public string SharePoint_URL_TEST { get; set; }
        public string SharePoint_Root { get; set; }
        public string SharePoint_Root_Test { get; set; }
        public string SharePoint_FilePath_PROD { get; set; }
        public string SharePoint_FilePath_TEST { get; set; }

      
    }
    public class CaseFileGet_spParams
    {
        public int CaseFileID { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }



    }
    public class CaseFileGet_spResult
    {

        public int CaseFileID { get; set; }
        public int CaseID { get; set; }
        public int? RoleID { get; set; }
        public int? CaseFileCategoryCodeID { get; set; }
        public string CaseFileDate { get; set; }
        public string CaseFileName { get; set; }
        public string CaseFileNameDisplay { get; set; }
        public string CaseFileDescription { get; set; }
        public string CaseFilePath { get; set; }
        public string CaseFileURL { get; set; }
        public int? CaseFileDisplayOrder { get; set; }
        public long? CaseFileSizeInBytes { get; set; }
        public byte? RecordStateID { get; set; }

    }
    public class CaseFileUpdate_spParams
    {
        public int CaseFileID { get; set; }
        public int CaseID { get; set; }
        public int RoleID { get; set; }
        public string CaseFileName { get; set; }
        public string CaseFileNameDisplay { get; set; }
        public string CaseFileDescription { get; set; }
        public string CaseFilePath { get; set; }
        public string CaseFileURL { get; set; }
        public int CaseFileDisplayOrder { get; set; }
        public int CaseFileSizeInBytes { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int CaseFileCategoryCodeID { get; set; }
        public DateTime CaseFileDate { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }



    }
    public class CaseFileInsert_spParams
    {
        public int CaseFileID { get; set; }
        public int CaseID { get; set; }
        public int RoleID { get; set; }
        public string CaseFileName { get; set; }
        public string CaseFileNameDisplay { get; set; }
        public string CaseFileDescription { get; set; }
        public string CaseFilePath { get; set; }
        public string CaseFileURL { get; set; }
        public int CaseFileDisplayOrder { get; set; }
        public int CaseFileSizeInBytes { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int CaseFileCategoryCodeID { get; set; }
        public DateTime CaseFileDate { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string EntityType { get; set; }
        public int EntityID { get; set; }


    }
    public class NG_CaseFileInsert_spParams
    {
        public int CaseFileID { get; set; }
        public int CaseID { get; set; }
        public int RoleID { get; set; }
        public string CaseFileName { get; set; }
        public string CaseFileNameDisplay { get; set; }
        public string CaseFileDescription { get; set; }
        public string CaseFilePath { get; set; }
        public string CaseFileURL { get; set; }
        public int CaseFileDisplayOrder { get; set; }
        public int CaseFileSizeInBytes { get; set; }
        public int RecordStateID { get; set; }
        public decimal? RecordTimeStamp { get; set; }
        public int CaseFileCategoryCodeID { get; set; }
        public DateTime CaseFileDate { get; set; }
        public int UserID { get; set; }
        public Guid BatchLogJobID { get; set; }
        public string GoogleFileID { get; set; }

        public string GoogleFolderID { get; set; }
        public string EntityType { get; set; }
        public int EntityID { get; set; }

        public string SharePointFileID { get; set; }

    }
}
