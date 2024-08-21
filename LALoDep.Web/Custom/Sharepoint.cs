using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using LALoDep.Domain.pd_Case;
using LALoDep.Core.Custom.Extensions;
using System.Web;

namespace LALoDep.Custom
{



    public class Sharepoint
    {
        private string _tenantId = "";// System.Configuration.ConfigurationSettings.AppSettings["TenantID"];
        private string _clientId = "";// System.Configuration.ConfigurationSettings.AppSettings["ClientID"];
        private string _certificateThumbprint = "";// System.Configuration.ConfigurationSettings.AppSettings["CertificateThumbprint"];
        private string _certificatePath = "";// System.Configuration.ConfigurationSettings.AppSettings["CertificatePath"];
        private string _certificatePassword = "";// System.Configuration.ConfigurationSettings.AppSettings["CertificatePassword"];
        private string _sharepointUrl = "";// System.Configuration.ConfigurationSettings.AppSettings["CertificatePassword"];
        private string _sharepointRoot = "";
        private string _sharepointFilePath = "";
        private string _subFolderFilePath = "";
        private string _authCode = "";
        private string _fileSavePath = "";

        public Sharepoint(CaseFileGetPathByCaseID_spResult oCaseFilePath = null, string entityType = "", int entityId = 0)
        {
            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];


            _fileSavePath = rootPath;
            var subRootPath = "";
            if (oCaseFilePath == null)
            {
                oCaseFilePath =
             LALoDep.Custom.UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<CaseFileGetPathByCaseID_spResult>(
                 "CaseFileGetPathByCaseID_sp",
                 new CaseFileGetPathByCaseID_spParams
                 {
                     BatchLogJobID = Guid.NewGuid(),
                     UserID = LALoDep.Custom.UserEnvironment.UserManager.UserExtended.UserID,
                     CaseID = LALoDep.Custom.UserEnvironment.UserManager.UserExtended.CaseID,
                     RootPath = rootPath,
                     ServerName = "stowe",
                     EntityID = entityId,
                     EntityType = entityType
                 }).FirstOrDefault();
            }

            if (oCaseFilePath != null)
            {
                _fileSavePath = oCaseFilePath.FullPath;
                _subFolderFilePath = subRootPath = oCaseFilePath.SubRootPath;



                if (oCaseFilePath != null && oCaseFilePath.SharePoint_UseFlag.ToInt() > 0)
                {
                    var environment = System.Configuration.ConfigurationManager.AppSettings["ServerEnvironment"];

                    if (environment == "Test" || environment == "Dev")
                    {
                        _sharepointUrl = oCaseFilePath.SharePoint_URL_TEST;
                        //    _sharepointFilePath = pathSpsResult.SharePoint_FilePath_TEST;
                        _sharepointRoot = oCaseFilePath.SharePoint_Root_Test;

                    }
                    else if (environment == "Prod")
                    {
                        _sharepointUrl = oCaseFilePath.SharePoint_URL_PROD;
                        //    _sharepointFilePath = pathSpsResult.SharePoint_FilePath_PROD;
                        _sharepointRoot = oCaseFilePath.SharePoint_Root;
                    }

                    _tenantId = oCaseFilePath.SharePoint_TenantID;
                    _clientId = oCaseFilePath.SharePoint_ClientID;
                    _certificatePath = oCaseFilePath.SharePoint_CertificatePath;
                    _certificatePassword = GetCertPassword(oCaseFilePath.SharePoint_CertificatePassword);
                    _sharepointFilePath = oCaseFilePath.SharePoint_FilePath_PROD;

                }

            }

        }

        #region
        public string GetCertPassword(string password)
        {

            if (!password.IsNullOrEmpty())
            {
                return password;
            }

            switch (_sharepointUrl)
            {
                case "https://ladlinc.sharepoint.com/sites/JCAT/":
                    password = "JCATs22@";
                    break;
                default:
                    password = "JCATs22@";
                    break;
            }
            return password;

        }
        public string SharepointFilePath
        {
            get
            {
                return _subFolderFilePath;
            }
        }
        public async Task<string> GetAccessTokenAsync()
        {
            var url = GetTenantUrl(_sharepointUrl + "/default.aspx");

            var authContext = new AuthenticationContext($"https://login.microsoftonline.com/{_tenantId}/oauth2/token"); // you can also use the v2.0 endpoint URL
            var token = (await authContext.AcquireTokenAsync(url, GetCertificate(_clientId, _certificateThumbprint)));
            _authCode = token.AccessToken;
            return token.AccessToken;
        }

        private static string GetTenantUrl(string url)
        {
            const string suffix = "sharepoint.com";
            var index = url.IndexOf(suffix, StringComparison.OrdinalIgnoreCase);
            return index != -1 ? url.Substring(0, index + suffix.Length) : url;
        }

        private ClientAssertionCertificate GetCertificate(string clientId, string thumbprint)
        {
            // var certificate = Debugger.IsAttached ? GetCertificateFromDirectory(_certificatePath, _certificatePassword) : GetCertificateFromStore(thumbprint);
            var certificate = GetCertificateFromDirectory(HttpContext.Current.Server.MapPath(_certificatePath), _certificatePassword);
            // var certificate = GetCertificateFromStore(thumbprint);
            var a = new ClientAssertionCertificate(clientId, certificate);
            return a;
        }

        private static X509Certificate2 GetCertificateFromDirectory(string path, string password)
        {

            var file = System.IO.Path.GetFullPath(path);
            return new X509Certificate2(file, password, X509KeyStorageFlags.MachineKeySet);
        }

        private static X509Certificate2 GetCertificateFromStore(string thumbprint)
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadOnly);

            var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

            store.Close();

            return certificates[0];
        }



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

        #endregion


        string errorMessage = "";
        public bool GetCaseFolder(string sharepointPath)
        {
            var flag = true;

            var oFolder = CreateFolder(sharepointPath);
            if (oFolder.d != null)
            {
                return flag;
            }
            var arrFolders = sharepointPath.Split('\\');

            var folderRoot = "";
            foreach (var item in arrFolders)
            {
                if (item.Length > 0)
                {
                    folderRoot += item + "\\";
                    oFolder = CreateFolder(folderRoot);
                    if (oFolder.error != null)
                    {
                        errorMessage = oFolder.error.message.value;
                        flag = false;
                        break;
                    }
                }


            }

            return flag;
        }






        #region

        public HttpResponseMessage GetResponse(string url, string accessToken, string body)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer {accessToken}");
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                return httpClient.PostAsync(url, content).Result;
            }
        }
        public HttpResponseMessage GetResponse(string url, string accessToken)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                //     httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer {accessToken}");

                return httpClient.GetAsync(url).Result;
            }


        }

        public HttpWebResponse GetResponseData(string url, string accessToken)
        {


            HttpWebRequest endpointRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            endpointRequest.Method = "GET";
            endpointRequest.Accept = "application/json;odata=verbose";
            endpointRequest.Headers.Add("Authorization",
              "Bearer " + accessToken);

            HttpWebResponse endpointResponse =
              (HttpWebResponse)endpointRequest.GetResponse();

            return endpointResponse;
        }
        public LALoDep.Models.Sharepoint.CreateFolder.CreateFolderResponse CreateFolder(string folderPath)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authCode);
                    //  httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");



                    //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer {_authCode}");
                }
                catch (Exception ex)
                {

                }


                var json = JsonConvert.SerializeObject(new LALoDep.Models.Sharepoint.CreateFolder.CreateFolderRequest()
                {
                    ServerRelativeUrl = _sharepointRoot + folderPath
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync(_sharepointUrl + "_api/web/folders", content).Result;
                var strContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult(); //right!
                var oCreateFolder = JsonConvert.DeserializeObject<LALoDep.Models.Sharepoint.CreateFolder.CreateFolderResponse>(strContent);
               

                return oCreateFolder;
            }

        }
        public LALoDep.Models.Sharepoint.SharePointFiles.SharePointFiles GetSharePointFiles()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authCode);
                    //  httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");


                }
                catch (Exception ex)
                {

                }




                var response = httpClient.GetAsync(_sharepointUrl + $"_api/web/GetFolderByServerRelativeUrl('{_sharepointRoot}/{_sharepointFilePath}')/Files").Result;
                var strContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var oFiles = JsonConvert.DeserializeObject<LALoDep.Models.Sharepoint.SharePointFiles.SharePointFiles>(strContent);


                return oFiles;
            }

        }
        public LALoDep.Models.Sharepoint.CreateFile.CreateFileResponse UploadFile(HttpPostedFileBase uploadedFile)
        {


            var isFolderExists = this.GetCaseFolder(_sharepointFilePath);
            if (isFolderExists)
            {
                string fileName = uploadedFile.FileName.ToRemoveSplChars();
                var sharePointList = new List<LALoDep.Models.Sharepoint.SharePointFiles.Result>();

                #region Rename file if duplicate locally
                var oFiles = GetSharePointFiles();

                if (oFiles != null)
                {
                    if (oFiles.d != null && oFiles.d.results.Any())
                    {
                        sharePointList = oFiles.d.results.ToList();

                    }
                }
                var fileCount = -1;
                var extension = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                var fileNameWithOutExtension = fileName.Substring(0,
                    fileName.LastIndexOf(".", StringComparison.Ordinal));
                do
                {
                    fileCount++;
                } while (
                  System.IO.File.Exists(_fileSavePath + "\\" + fileNameWithOutExtension +
                                        (fileCount > 0 ? "[" + fileCount + "]" : "") + extension) || sharePointList.Any(o => o.Name == (fileNameWithOutExtension + (fileCount > 0 ? "[" + fileCount + "]" : "") + extension)));

                fileName = fileNameWithOutExtension + (fileCount > 0 ? "[" + fileCount + "]" : "") + extension;

                #endregion


                byte[] bytefile = uploadedFile.InputStream.ReadAllBytes();

                HttpWebRequest endpointRequest = (HttpWebRequest)HttpWebRequest.Create($"{_sharepointUrl}/_api/web/GetFolderByServerRelativeUrl('{_sharepointRoot}/{_sharepointFilePath}')/Files/add(url='{EncodeFileName(fileName)}',overwrite=true)");
                endpointRequest.Method = "POST";
                endpointRequest.Headers.Add("binaryStringRequestBody", "true");
                endpointRequest.Headers.Add("Authorization", "Bearer " + _authCode);
                endpointRequest.Accept = "application/json;odata=verbose";
                endpointRequest.GetRequestStream().Write(bytefile, 0, bytefile.Length);

                HttpWebResponse endpointresponse = (HttpWebResponse)endpointRequest.GetResponse();
                var receiveStream = endpointresponse.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var strContent = readStream.ReadToEnd();
                var oCreateFile = JsonConvert.DeserializeObject<LALoDep.Models.Sharepoint.CreateFile.CreateFileResponse>(strContent);

                oCreateFile.FileName = fileName;
                return oCreateFile;
            }else
            {
                throw new Exception(errorMessage);
            }
            return null;
        }
        public byte[] DownloadFile(string filePath)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authCode);
                    //  httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");


                }
                catch (Exception ex)
                {

                }




                var response = httpClient.GetAsync(_sharepointUrl + $"_api/web/GetFileByServerRelativePath(decodedurl='{_sharepointRoot}{EncodeFileName(filePath)}')/$value").Result;
                var strContent = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();


                return strContent;
            }

        }
        public void DeleteFile(string filePath)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {

                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _authCode);
                    //  httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");


                }
                catch (Exception ex)
                {

                }





                var response = httpClient.DeleteAsync(_sharepointUrl + $"_api/web/GetFileByServerRelativePath(decodedurl='{_sharepointRoot}/{EncodeFileName(filePath)}')").Result;
                var strContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();



            }

        }


        public string EncodeFileName(string fileName)
        {
            return fileName.Replace("#", "%23").Replace("'", "''");

        }
        #endregion

    }

}
