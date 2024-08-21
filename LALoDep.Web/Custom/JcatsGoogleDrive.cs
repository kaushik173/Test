using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Download;
using LALoDep.Domain.GoogleDrive;
using LALoDep.Domain.pd_Case;
using LALoDep.Core.Custom.Extensions;
 

namespace LALoDep.Custom
{
    public class JcatsGoogleDrive
    {
        private Google.Apis.Drive.v3.DriveService _service { get; set; }
        private string _testFolderId;
        private string _prodFolderId;
        public JcatsGoogleDrive(int caseId = 0)
        {
            var environment = System.Configuration.ConfigurationManager.AppSettings["GoogleDriveEnvironment"];

            var agencyId = UserEnvironment.UserManager.UserExtended.CaseNumberAgencyID;
            string[] scopes = new string[] { Google.Apis.Drive.v3.DriveService.Scope.Drive, Google.Apis.Drive.v3.DriveService.Scope.DriveFile, Google.Apis.Drive.v3.DriveService.Scope.DriveMetadata }; // Full access
            var googleApiCredentials = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<NG_GoogleDriveAPICredentialByAgencyID_spResult>("NG_GoogleDriveAPICredentialByAgencyID_sp", new NG_GoogleDriveAPICredentialByAgencyID_spParams()
            {
                AccountType = environment,
                AgencyID = agencyId
            }).FirstOrDefault();

            if (googleApiCredentials == null)
                throw new Exception("Google Drive API Credentials not found!");
            if (caseId > 0)
            {
                var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];

                var pathSpsResult =
                       UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<CaseFileGetPathByCaseID_spResult>(
                          "CaseFileGetPathByCaseID_sp",
                          new CaseFileGetPathByCaseID_spParams
                          {
                              BatchLogJobID = Guid.NewGuid(),
                              UserID = UserEnvironment.UserManager.UserExtended.UserID,
                              CaseID = caseId,
                              RootPath = rootPath,
                              ServerName = "stowe"

                          }).FirstOrDefault();
                if (pathSpsResult != null)
                {
                    _prodFolderId = pathSpsResult.GoogleFolderID_PROD;
                    _testFolderId = pathSpsResult.GoogleFolderID_TEST;

                }
            }
            else
            {
                _prodFolderId = googleApiCredentials.GoogleDriveProdFolderID;
                _testFolderId = googleApiCredentials.GoogleDriveTestFolderID;

            }



            var keyFilePath = HttpContext.Current.Server.MapPath(googleApiCredentials.APIKeyFilePath);
            var serviceAccountEmail = googleApiCredentials.APIAccountEmail;// "aocprj@aocprj.iam.gserviceaccount.com";


            //loading the Key file
            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = scopes
            }.FromCertificate(certificate));


            _service = new Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "AOCPRJ",
            });

        }

        //// 

        /// Create a new Directory.
        /// Documentation: https://developers.google.com/drive/v2/reference/files/insert
        /// 
        public string GetFolderIdIfExists(string _title, string parentFolderId)
        {

            var fileId = "";
            var request = _service.Files.List();
            request.Q = string.Format("name='{0}' and mimeType = 'application/vnd.google-apps.folder' and trashed = false and '{1}' in parents", _title, parentFolderId);
            request.Spaces = "drive";
            request.SupportsTeamDrives = true;
            request.IncludeTeamDriveItems = true;

            var result = request.Execute();
            foreach (var file in result.Files)
            {
                var name = file.Name;

                if (file.Name == _title && (!file.Trashed.HasValue || (file.Trashed.HasValue && !file.Trashed.Value)))
                {
                    fileId = file.Id;
                    break;
                }

            }
            return fileId;
        }
        /// a Valid authenticated DriveService
        /// The title of the file. Used to identify file or folder name.
        /// A short description of the file.
        /// Collection of parent folders which contain this file. 
        ///                       Setting this field will put the file in all of the provided folders. root folder.
        /// 


        public File createDirectory(string _title, string _description, string _parent)
        {

            File NewDirectory = null;

            // Create metaData for a new Directory
            File body = new File();
            body.Name = _title;
            body.Description = _description;
            body.MimeType = "application/vnd.google-apps.folder";
            body.Parents = new List<string>() { _parent };

            try
            {

                Google.Apis.Drive.v3.FilesResource.CreateRequest request = _service.Files.Create(body);
                request.SupportsTeamDrives = true;
                NewDirectory = request.Execute();
                InsertPermission(NewDirectory.Id);
            }
            catch (Exception e)
            {
                throw e;
                Console.WriteLine("An error occurred: " + e.Message);
            }

            return NewDirectory;
        }
        // ...

        /// <summary>
        /// Insert a new permission.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <param name="fileId">ID of the file to insert permission for.</param>
        /// <param name="value">
        /// User or group e-mail address, domain name or null for "default" type.
        /// </param>
        /// <param name="type">The value "user", "group", "domain" or "default".</param>
        /// <param name="role">The value "owner", "writer" or "reader".</param>
        /// <returns>The inserted permission, null is returned if an API error occurred</returns>
        public Permission InsertPermission(String fileId, String value = "uploads@clccalifornia.org",
            String type = "user", String role = "owner")
        {
            Permission newPermission = new Permission();
            newPermission.EmailAddress = value;
            newPermission.Type = type;
            newPermission.Role = role;
            try
            {
                return _service.Permissions.Create(newPermission, fileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
            return null;
        }
        // ...
        // tries to figure out the mime type of the file.
        public string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            try
            {


                string ext = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
                if (regKey != null && regKey.GetValue("Content Type") != null)
                    mimeType = regKey.GetValue("Content Type").ToString();
            }
            catch
            {

            }
            return mimeType;
        }
        /// 

        /// Uploads a file
        /// Documentation: https://developers.google.com/drive/v2/reference/files/insert
        /// 

        /// a Valid authenticated DriveService
        /// path to the file to upload
        /// Collection of parent folders which contain this file. 
        ///                       Setting this field will put the file in all of the provided folders. root folder.
        /// If upload succeeded returns the File resource of the uploaded file 
        ///          If the upload fails returns null
        public File uploadFile(string _uploadFile, string _parent)
        {

            if (System.IO.File.Exists(_uploadFile))
            {
                File body = new File();
                body.Name = System.IO.Path.GetFileName(_uploadFile);
                body.Description = "File uploaded by Diamto Drive Sample";
                body.MimeType = GetMimeType(_uploadFile);
                body.Parents = new List<string>() { _parent };

                // File's content.
                byte[] byteArray = System.IO.File.ReadAllBytes(_uploadFile);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                try
                {
                    FilesResource.CreateMediaUpload request = _service.Files.Create(body, stream, GetMimeType(_uploadFile));
                    request.SupportsTeamDrives = true;
                    request.Upload();
                    return request.ResponseBody;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    return null;
                }
            }
            else
            {
                Console.WriteLine("File does not exist: " + _uploadFile);
                return null;
            }

        }

        /// a Valid authenticated DriveService
        /// path to the file to upload
        /// Collection of parent folders which contain this file. 
        ///                       Setting this field will put the file in all of the provided folders. root folder.
        /// the resource id for the file we would like to update                      
        /// If upload succeeded returns the File resource of the uploaded file 
        ///          If the upload fails returns null
        public File updateFile(HttpPostedFileBase _uploadFile, string _parent, string _fileId, string fileName)
        {

            if (_uploadFile != null)
            {
                File body = new File();
                body.Name = fileName;
                body.Description = "File updated by Diamto Drive Sample";
                body.MimeType = GetMimeType(fileName);
                body.Parents = new List<string>() { _parent };

                // File's content.


                try
                {
                    FilesResource.CreateMediaUpload request = _service.Files.Create(body, _uploadFile.InputStream, GetMimeType(fileName));
                    request.SupportsTeamDrives = true;
                    request.Upload();
                    InsertPermission(request.ResponseBody.Id);
                    return request.ResponseBody;
                }
                catch (Exception e)
                {
                    Console.WriteLine("An error occurred: " + e.Message);
                    return null;
                }
            }
            else
            {
                Console.WriteLine("File does not exist: " + _uploadFile);
                return null;
            }

        }


        public void DeleteFile(string _fileId)
        {



            FilesResource.DeleteRequest DeleteRequest = _service.Files.Delete(_fileId);
            DeleteRequest.SupportsTeamDrives = true;
            DeleteRequest.Execute();


        }

        public byte[] DownloadFile(string _fileId, string fileName)
        {



            var fileId = _fileId;
            var request = _service.Files.Get(fileId);

            request.SupportsTeamDrives = true;
            var stream = new System.IO.MemoryStream();

            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged +=
                (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case DownloadStatus.Downloading:
                            {
                                Console.WriteLine(progress.BytesDownloaded);
                                break;
                            }
                        case DownloadStatus.Completed:
                            {
                                Console.WriteLine("Download complete.");
                                break;
                            }
                        case DownloadStatus.Failed:
                            {
                                Console.WriteLine("Download failed.");
                                break;
                            }
                    }
                };
            request.Download(stream);
            //   File f = request.Execute();

            //   var stream = new System.IO.MemoryStream();
            //request.DownloadAsync(stream);
            byte[] arrBytes = stream.ToArray();
            if (arrBytes.Length > 0)
                return arrBytes;
            return null;


            //if (!String.IsNullOrEmpty(f.WebContentLink))
            //{
            //    try
            //    {
            //        var x = _service.HttpClient.GetByteArrayAsync(f.WebContentLink);
            //        byte[] arrBytes = x.Result;
            //        return arrBytes;
            //    }
            //    catch (Exception e)
            //    {
            //        throw e;
            //    }
            //}


            // return null;
        }

        private void SaveStream(System.IO.MemoryStream stream, string saveTo)
        {
            using (System.IO.FileStream file = new System.IO.FileStream(saveTo, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }


        #region Create Agency/CaseID folder

        private string GetAgencyFolderID(int agencyId)
        {
            var environment = System.Configuration.ConfigurationManager.AppSettings["GoogleRootFolder"];
            var parentFolderId = "";
            if (environment == "Test")
            {
                parentFolderId = _testFolderId;
            }
            else if (environment == "Prod")
            {
                parentFolderId = _prodFolderId;

            }

            return parentFolderId;
            //var gfolderId = "";
            //var db = new DbManager();

            //db.AddInParam("AgencyID", agencyId);
            //db.AddInParam("GoogleDriveParentFolderID", parentFolderId);
            //var folderId = db.ExecuteScalar("select GoogleDriveID from GoogleDriveAgencyFolder where AgencyID=@AgencyID and GoogleDriveParentFolderID=@GoogleDriveParentFolderID", System.Data.CommandType.Text);

            //if (folderId != null)
            //{
            //    gfolderId = folderId.ToString();
            //}
            //else
            //{
            //    var newDirectory = this.createDirectory(agencyId.ToString(), "AgencyID: " + agencyId, parentFolderId);
            //    gfolderId = newDirectory.Id;
            //    db.Parameters.Clear();
            //    db.AddInParam("AgencyID", agencyId);
            //    db.AddInParam("GoogleDriveID", gfolderId);
            //    db.AddInParam("GoogleDriveParentFolderID", parentFolderId);

            //    db.ExecuteScalar("NG_GoogleDriveAgencyFolderInsert", System.Data.CommandType.StoredProcedure);


            //}
            //return gfolderId;

        }


        public string GetCaseFolderID(int caseId, int agencyId)
        {
            var parentFolderId = GetAgencyFolderID(agencyId);

            var gfolderId = "";
            //var db = new DbManager();

            //db.AddInParam("CaseID", caseId);
            //db.AddInParam("GoogleDriveParentFolderID", parentFolderId);
            //var folderId = db.ExecuteScalar("select GoogleDriveID from [GoogleDriveCaseFolder] where CaseID=@CaseID and GoogleDriveParentFolderID=@GoogleDriveParentFolderID", System.Data.CommandType.Text);

            //if (folderId != null)
            //{
            //    gfolderId = folderId.ToString();
            //}
            //else
            //{

            gfolderId = GetFolderIdIfExists(caseId.ToString(), parentFolderId);
            if (gfolderId.IsNullOrEmpty())
            {
                var newDirectory = this.createDirectory(caseId.ToString(), "CaseID: " + caseId, parentFolderId);
                gfolderId = newDirectory.Id;

            }
            //if (!gfolderId.IsNullOrEmpty())
            //{
            //    db.Parameters.Clear();
            //    db.AddInParam("CaseID", caseId);
            //    db.AddInParam("GoogleDriveID", gfolderId);
            //    db.AddInParam("GoogleDriveParentFolderID", parentFolderId);

            //    db.ExecuteScalar("NG_GoogleDriveCaseFolderInsert", System.Data.CommandType.StoredProcedure);
            //}

            //   }
            return gfolderId;

        }
        #endregion

    }
}