using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Role;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Domain;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AttachFile,
            PageSecurityItemID = SecurityToken.AttachFileView)]
        public virtual ActionResult AttachFile()
        {

            var model = new AttachPathViewModel
            {
                CaseFileGetPathByCaseList =
                    UtilityService.ExecStoredProcedureWithResults<CaseFileGetPathByCaseID_spResult>(
                        "CaseFileGetPathByCaseID_sp",
                        new CaseFileGetPathByCaseID_spParams
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            CaseID = UserManager.UserExtended.CaseID,
                            UserID = UserManager.UserExtended.UserID,


                        }).ToList(),
                RoleList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDChildRespondent_spResult>(
                    "pd_RoleGetByCaseIDChildRespondent_sp",
                    new pd_RoleGetByCaseIDChildRespondent_spParams
                    {
                        BatchLogJobID = Guid.NewGuid(),
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,


                    }).Select(o => new SelectListItem() { Text = o.DisplayName, Value = o.RoleID.ToString() }),
                CategoryList =
                    UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(900, agencyId: UserManager.UserExtended.CaseNumberAgencyID)
                        .Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() })
                        .ToList(),

                FileDate = DateTime.Now.ToShortDateString()
            };

            var caseFileGetPath = model.CaseFileGetPathByCaseList.FirstOrDefault();
            if (caseFileGetPath != null)
            {
                if (caseFileGetPath.UseGoogleDocsFlag.HasValue && caseFileGetPath.UseGoogleDocsFlag.Value > 0)
                {
                    var environment = System.Configuration.ConfigurationManager.AppSettings["GoogleRootFolder"];
                    var parentFolderId = "";
                    if (environment == "Test")
                    {
                        parentFolderId = caseFileGetPath.GoogleFolderID_TEST;
                    }
                    else if (environment == "Prod")
                    {
                        parentFolderId = caseFileGetPath.GoogleFolderID_PROD;

                    }
                    if (!parentFolderId.IsNullOrEmpty())
                        model.UseGoogleDriveUpload = caseFileGetPath.UseGoogleDocsFlag.Value;

                }

                if (caseFileGetPath.SharePoint_UseFlag.HasValue && caseFileGetPath.SharePoint_UseFlag.Value > 0)
                {
                    var environment = System.Configuration.ConfigurationManager.AppSettings["ServerEnvironment"];

                    if (environment == "Test" || environment == "Dev")
                    {
                        model.SharePoint_URL = caseFileGetPath.SharePoint_URL_TEST;
                    }
                    else if (environment == "Prod")
                    {
                        model.SharePoint_URL = caseFileGetPath.SharePoint_URL_PROD;

                    }

                    model.SharePoint_UseFlag = caseFileGetPath.SharePoint_UseFlag.Value;
                }
            }

            var settings = UtilityFunctions.JcatsNGConfigGetAll(UserManager.UserExtended.CaseID);

            if (settings.Any(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_FileTypes))
                ViewBag.AttachFileTypes = settings.FirstOrDefault(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_FileTypes).JcatsNGConfigValue;
            else
                ViewBag.AttachFileTypes = "gif|jpe?g|png|pdf|txt|docx|doc|xls|xlsx|csv|ppt|pptx";

            if (settings.Any(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_MaxSize))
                ViewBag.AttachFileMaxSize = settings.FirstOrDefault(x => x.JcatsNGConfigName == NGConfigNames.AttachFile_MaxSize).JcatsNGConfigValue;
            else
                ViewBag.AttachFileMaxSize = "15000000";

            return View(model);
        }


        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AttachFile,
           PageSecurityItemID = SecurityToken.AttachFileView)]

        [HttpPost]
        public virtual JsonResult GetFiles(string entityType = "", int entityId = 0)
        {
            var oCaseFileGetPathByCase = UtilityService.ExecStoredProcedureWithResults<CaseFileGetPathByCaseID_spResult>(
                            "CaseFileGetPathByCaseID_sp",
                            new CaseFileGetPathByCaseID_spParams
                            {
                                BatchLogJobID = Guid.NewGuid(),
                                CaseID = UserManager.UserExtended.CaseID,
                                UserID = UserManager.UserExtended.UserID,


                            }).FirstOrDefault();

            var caseFileGetByCaseList =
               UtilityService.ExecStoredProcedureWithResults<CaseFileGetByCaseID_spResult>(
                   "NG_CaseFileGetByCaseID_sp",
                   new CaseFileGetByCaseID_spParams
                   {
                       BatchLogJobID = Guid.NewGuid(),
                       CaseID = UserManager.UserExtended.CaseID,
                       UserID = UserManager.UserExtended.UserID,
                       SortOption = "FileDate",
                       EntityType = entityType,
                       EntityID = entityId
                   }).ToList();

            if (Request.Form["Category"] != null && !Request.Form["Category"].IsNullOrEmpty())
            {
                caseFileGetByCaseList = caseFileGetByCaseList.Where(o => o.Category.ToLower().Contains(Request.Form["Category"].ToLower())).ToList();

            }
            if (Request.Form["Description"] != null && !Request.Form["Description"].IsNullOrEmpty())
            {
                caseFileGetByCaseList = caseFileGetByCaseList.Where(o => !string.IsNullOrEmpty(o.CaseFileDescription) && o.CaseFileDescription.ToLower().Contains(Request.Form["Description"].ToLower())).ToList();

            }
            var sharePoint_FilePath = "";


            for (var i = 0; i < caseFileGetByCaseList.Count; i++)
            {
                sharePoint_FilePath = "";
                var item = caseFileGetByCaseList[i];
                if (oCaseFileGetPathByCase != null && oCaseFileGetPathByCase.SharePoint_UseFlag.ToInt() > 0 && !item.SharePointFileID.IsNullOrEmpty())
                {
                    var environment = System.Configuration.ConfigurationManager.AppSettings["ServerEnvironment"];

                    if (environment == "Test" || environment == "Dev")
                    {
                        sharePoint_FilePath = item.SharePointFile_URL_TEST;

                    }
                    else if (environment == "Prod")
                    {
                        sharePoint_FilePath = item.SharePointFile_URL_PROD;

                    }


                }
                else
                {
                    sharePoint_FilePath = "";
                }
                item.SharePoint_FilePath = sharePoint_FilePath.Replace("#", "%23");
                item.EncryptFilePath = Utility.Encrypt(item.CaseFilePath);
                item.DownloadPath = item.GoogleFileID.IsNullOrEmpty() ? "/Case/DownloadCaseFile/" + item.CaseFileID.ToEncrypt() + "?CaseId=" + item.CaseID.Value.ToEncrypt() : "/Case/DownloadDriveFile/" + item.GoogleFileID + "?fileName=" + item.CaseFileName.ToEncrypt() + "&CaseId=" + item.CaseID.Value.ToEncrypt();
                item.KamiUrl = "https://web.kamihq.com/web/viewer.html?state=%7B\"ids\":%5B\"" + item.GoogleFileID + "\"%5D,\"action\":\"open\"%7D";
                caseFileGetByCaseList[i] = item;



            }
            var total = caseFileGetByCaseList.Count;

            return new JsonResult()
            {
                Data = new DataTablesResponse(0, caseFileGetByCaseList, total, total),

                MaxJsonLength = Int32.MaxValue
            };

        }

        [ClaimsAuthorize(IsPageMainMethod = false, PageSecurityItemID = SecurityToken.AttachFileDelete)]
        [HttpPost]
        public virtual async Task<JsonResult> DeleteFile(int id, string gfileid, string sFileId, string localfilePath)
        {


            if (!string.IsNullOrEmpty(gfileid))
            {
                var gservice = new JcatsGoogleDrive(UserManager.UserExtended.CaseID);

                gservice.DeleteFile(gfileid);
            }
            else if (!string.IsNullOrEmpty(sFileId))
            {
                var oSharepoint = new Sharepoint();

                await oSharepoint.GetAccessTokenAsync();
                oSharepoint.DeleteFile(sFileId);
            }
            else if (!localfilePath.IsNullOrEmpty())
            {

                var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];
                var fileSavePath = rootPath + localfilePath;
                if (System.IO.File.Exists(fileSavePath))
                {
                    try
                    {
                        System.IO.File.Delete(fileSavePath);
                    }
                    catch
                    {

                    }


                }

            }
            UtilityService.ExecStoredProcedureWithoutResultADO("CaseFileDelete_sp", new CaseFileDelete_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID,
                ID = id,


            });
            return Json(new { Status = "done" });
        }

        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AttachFile,
        PageSecurityItemID = SecurityToken.AttachFileEdit)]



        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AttachFile,
           PageSecurityItemID = SecurityToken.AttachFileEdit)]
        public virtual ActionResult EditAttachFile(int id)
        {
            var model = new AttachPathEditViewModel();
            var fileGet = UtilityService.ExecStoredProcedureWithResults<CaseFileGet_spResult>("CaseFileGet_sp",
                new CaseFileGet_spParams()
                {
                    BatchLogJobID = Guid.NewGuid(),
                    CaseFileID = id,
                    UserID = UserManager.UserExtended.UserID
                }).FirstOrDefault();
            if (fileGet != null)
            {
                model.RoleList =
              UtilityService.ExecStoredProcedureWithResults<pd_RoleGetByCaseIDChildRespondent_spResult>(
                  "pd_RoleGetByCaseIDChildRespondent_sp",
                  new pd_RoleGetByCaseIDChildRespondent_spParams
                  {
                      BatchLogJobID = Guid.NewGuid(),
                      CaseID = UserManager.UserExtended.CaseID,
                      UserID = UserManager.UserExtended.UserID,


                  }).Select(o => new SelectListItem() { Text = o.DisplayName, Value = o.RoleID.ToString() });

                model.CategoryList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(900, includeCodeId: fileGet.CaseFileCategoryCodeID ?? 0, agencyId: UserManager.UserExtended.CaseNumberAgencyID)
                                                                        .Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() }).ToList();

                model.Description = fileGet.CaseFileDescription;
                model.FileDate = fileGet.CaseFileDate;
                model.FileName = fileGet.CaseFileName;
                model.RoleID = fileGet.RoleID.HasValue ? fileGet.RoleID.Value : 0;
                model.CategoryID = fileGet.CaseFileCategoryCodeID.HasValue ? fileGet.CaseFileCategoryCodeID.Value : 0;
                model.RecordStateID = (byte)(fileGet.RecordStateID.HasValue ? fileGet.RecordStateID.Value : 0);
                model.CaseFileID = fileGet.CaseFileID;
            }


            return View(model);
        }
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AttachFile,
         PageSecurityItemID = SecurityToken.AttachFileEdit)]
        [HttpPost]
        public virtual ActionResult EditAttachFile(AttachPathEditViewModel model)
        {

            var fileGet = UtilityService.ExecStoredProcedureWithResults<CaseFileGet_spResult>("CaseFileGet_sp",
             new CaseFileGet_spParams()
             {
                 BatchLogJobID = Guid.NewGuid(),
                 CaseFileID = model.CaseFileID,
                 UserID = UserManager.UserExtended.UserID
             }).FirstOrDefault();
            if (fileGet != null)
            {
                UtilityService.ExecStoredProcedureWithoutResultADO("CaseFileUpdate_sp", new CaseFileUpdate_spParams()
                {
                    BatchLogJobID = Guid.NewGuid(),
                    RoleID = model.RoleID,
                    UserID = UserManager.UserExtended.UserID,
                    CaseID = UserManager.UserExtended.CaseID,
                    CaseFileID = model.CaseFileID,
                    CaseFileCategoryCodeID = model.CategoryID,
                    CaseFileDate = model.FileDate.ToDateTime(),
                    CaseFileDescription = model.Description,
                    CaseFileName = fileGet.CaseFileName,
                    CaseFileDisplayOrder = fileGet.CaseFileDisplayOrder.HasValue ? fileGet.CaseFileDisplayOrder.Value : 0,
                    CaseFileNameDisplay = fileGet.CaseFileNameDisplay,
                    CaseFilePath = fileGet.CaseFilePath,
                    CaseFileSizeInBytes = fileGet.CaseFileSizeInBytes.HasValue ? fileGet.CaseFileSizeInBytes.Value.ToInt() : 0,
                    CaseFileURL = fileGet.CaseFileURL,
                    RecordStateID = fileGet.RecordStateID.HasValue ? fileGet.RecordStateID.Value : 1

                });
            }
            return Json(new { Status = "Done" });
        }



        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.AttachFile,
        PageSecurityItemID = SecurityToken.AttachFileAdd)]

        [HttpPost]
        public virtual async Task<JsonResult> AttachFileUpload()
        {
            var fileStatus = new List<FileUploadStatus>();
            if (Request.Files.Count == 0)
            {

                Response.ContentType = "text/plain";
                Response.Write("No files received.");

            }
            else
            {
                string entityType = Request.Form["EntityType"];
                int entityId = Request.Form["EntityId"].ToInt();
                var caseId = UserManager.UserExtended.CaseID;
                var userId = UserManager.UserExtended.UserID;
                var uploadedfile = Request.Files[0];

                var fileName = uploadedfile.FileName.ToRemoveSplChars();
                var fileType = uploadedfile.ContentType;
                var fileSize = uploadedfile.ContentLength;
                var fileCount = -1; var extension = ""; var fileNameWithOutExtension = "";
                var isSharePointFileNameCheck = false;
                #region Get FullPath and SubRootPath
                var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];


                var fileSavePath = rootPath;
                var subRootPath = "";
                var pathSpsResult =
                    UtilityService.ExecStoredProcedureWithResults<CaseFileGetPathByCaseID_spResult>(
                        "CaseFileGetPathByCaseID_sp",
                        new CaseFileGetPathByCaseID_spParams
                        {
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID,
                            CaseID = UserManager.UserExtended.CaseID,
                            RootPath = rootPath,
                            ServerName = "stowe",
                            EntityType = entityType,
                            EntityID = entityId

                        }).FirstOrDefault();
                if (pathSpsResult != null)
                {
                    fileSavePath = pathSpsResult.FullPath;
                    subRootPath = pathSpsResult.SubRootPath;
                    isSharePointFileNameCheck = pathSpsResult.SharePoint_UseFlag.ToInt() > 0;
                    var environment = System.Configuration.ConfigurationManager.AppSettings["GoogleRootFolder"];
                    var parentFolderId = "";


                }



                if (!Directory.Exists(fileSavePath))
                    Directory.CreateDirectory(fileSavePath);

                #endregion





                #region Rename file if duplicate


                var sharePointList = new List<LALoDep.Models.Sharepoint.SharePointFiles.Result>();

                if (isSharePointFileNameCheck)
                {
                    var oSharepoint = new Sharepoint(pathSpsResult);
                    await oSharepoint.GetAccessTokenAsync();
                    var oFiles = oSharepoint.GetSharePointFiles();

                    if (oFiles != null)
                    {
                        if (oFiles.d != null && oFiles.d.results.Any())
                        {
                            sharePointList = oFiles.d.results.ToList();

                        }
                    }
                }

                fileCount = -1;
                extension = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal));
                fileNameWithOutExtension = fileName.Substring(0,
                   fileName.LastIndexOf(".", StringComparison.Ordinal));
                do
                {
                    fileCount++;
                } while (
                    System.IO.File.Exists(fileSavePath + "\\" + fileNameWithOutExtension +
                                          (fileCount > 0 ? "[" + fileCount + "]" : "") + extension) || sharePointList.Any(o => o.Name == (fileNameWithOutExtension + (fileCount > 0 ? "[" + fileCount + "]" : "") + extension)));

                fileName = fileNameWithOutExtension + (fileCount > 0 ? "[" + fileCount + "]" : "") + extension;

                #endregion
                uploadedfile.SaveAs(fileSavePath + "\\" + fileName);

                //rollback
                //  System.IO.File.SetAttributes(fileSavePath + "\\" + fileName, FileAttributes.ReadOnly);

                var roleId = Request.Form["RoleID"].ToInt();
                var categoryId = Request.Form["CategoryID"].ToInt();
                var fileDate = Request.Form["FileDate"].ToDateTime();
                var description = Request.Form["description[]"];


                var insertFileResult = UtilityService.ExecStoredProcedureScalar("NG_CaseFileInsert_sp",
                          new NG_CaseFileInsert_spParams()
                          {
                              BatchLogJobID = Guid.NewGuid(),
                              RoleID = roleId,
                              UserID = UserManager.UserExtended.UserID,
                              CaseID = UserManager.UserExtended.CaseID,

                              CaseFileCategoryCodeID = categoryId,
                              CaseFileDate = fileDate,
                              CaseFileDescription = description,
                              CaseFileName = fileName,
                              CaseFileDisplayOrder = 1,
                              CaseFileNameDisplay = fileName,
                              CaseFilePath = subRootPath + fileName,
                              CaseFileSizeInBytes = fileSize,
                              CaseFileURL = null,
                              RecordStateID = 1,
                              EntityID = entityId,
                              EntityType = entityType

                          });



                Response.ContentType = "application/json";
                fileStatus.Add(new FileUploadStatus()
                {
                    name = fileName,
                    size = fileSize,
                    error = "The file was successfully uploaded!",
                    type = fileType

                });
                return Json(new { files = fileStatus });

            }


            return Json(new { Status = "Done" });
        }

        //[HttpPost]
        public virtual ActionResult DownloadCaseFile(string id)
        {
            var caseFileId = id.ToDecrypt().ToInt();
            var fileGet = UtilityService.ExecStoredProcedureWithResults<CaseFileGet_spResult>("CaseFileGet_sp",
                                            new CaseFileGet_spParams() { BatchLogJobID = Guid.NewGuid(), CaseFileID = caseFileId, UserID = UserManager.UserExtended.UserID }).FirstOrDefault();

            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];
            var fileSavePath = rootPath;
            //var pathSpsResult = UtilityService.ExecStoredProcedureWithResults<CaseFileGetPathByCaseID_spResult>("CaseFileGetPathByCaseID_sp",
            //                                        new CaseFileGetPathByCaseID_spParams
            //                                        {
            //                                            BatchLogJobID = Guid.NewGuid(),
            //                                            UserID = UserManager.UserExtended.UserID,
            //                                            CaseID = UserManager.UserExtended.CaseID,
            //                                            RootPath = rootPath,
            //                                            ServerName = "stowe"
            //                                        }).FirstOrDefault();

            if (fileGet != null)
                fileSavePath += fileGet.CaseFilePath;


            // fileSavePath = fileSavePath + @"\" + fileGet.CaseFileName;

            if (!System.IO.File.Exists(fileSavePath))
            {

                Bugsnag.AspNet.Client.Current.Notify(new Exception(string.Format("File not found File URL :{0} CaseID is {1}", fileSavePath, UserManager.UserExtended.CaseID)));

                TempData["ErrorMessage"] = "File doesn't exist";
                return RedirectToAction("AttachFile");
            }


            //   byte[] fileBytes = System.IO.File.ReadAllBytes(fileSavePath);

            return File(fileSavePath, System.Net.Mime.MediaTypeNames.Application.Octet, fileGet.CaseFileName);

        }


        [HttpPost]
        public virtual ActionResult AttachFileUploadToGoogleDrive()
        {

            //notasecret

            var fileStatus = new List<FileUploadStatus>();


            if (Request.Files.Count == 0)
            {

                Response.ContentType = "text/plain";
                Response.Write("No files received.");

            }
            else
            {

                var caseId = UserManager.UserExtended.CaseID;
                var agencyId = UserManager.UserExtended.CaseNumberAgencyID;
                var userId = UserManager.UserExtended.UserID;
                var uploadedfile = Request.Files[0];

                var fileName = uploadedfile.FileName.ToRemoveSplChars();
                var fileType = uploadedfile.ContentType;
                var fileSize = uploadedfile.ContentLength;

                string entityType = Request.Form["EntityType"];
                int entityId = Request.Form["EntityId"].ToInt();
                var roleId = Request.Form["RoleID"].ToInt();
                var categoryId = Request.Form["CategoryID"].ToInt();
                var fileDate = Request.Form["FileDate"].ToDateTime();
                var description = Request.Form["description[]"];


                //  Response.Write("{\"files\":[{\"name\":\"Error Occur\",\"type\":\"" + fileType + "\",\"size\":\"" + fileSize + "\",\"error\":\"Error occur during uploading file \"}]}");
                //   return Content("");

                //   return Json(new { Status = "Done",name= "Error Occur",type= fileType, size= fileSize, error="error occur"});
                #region Get FullPath and SubRootPath



                var gservice = new JcatsGoogleDrive(UserManager.UserExtended.CaseID);
                var gfolderId = gservice.GetCaseFolderID(UserManager.UserExtended.CaseID, UserManager.UserExtended.CaseNumberAgencyID);



                #endregion


                var gfileId = "";
                var file = gservice.updateFile(uploadedfile, gfolderId, "", fileName);
                if (file != null)
                    gfileId = file.Id;
                if (gfileId.IsNullOrEmpty())
                {
                    Response.ContentType = "application/json";
                    fileStatus.Add(new FileUploadStatus()
                    {
                        name = fileName,
                        size = fileSize,
                        error = "The file could not uploaded",
                        type = fileType

                    });
                    return Json(new { files = fileStatus });
                }






                var insertFileResult = UtilityService.ExecStoredProcedureScalar("NG_CaseFileInsert_sp",
                          new NG_CaseFileInsert_spParams()
                          {
                              BatchLogJobID = Guid.NewGuid(),
                              RoleID = roleId,
                              UserID = UserManager.UserExtended.UserID,
                              CaseID = UserManager.UserExtended.CaseID,

                              CaseFileCategoryCodeID = categoryId,
                              CaseFileDate = fileDate,
                              CaseFileDescription = description,
                              CaseFileName = fileName,
                              CaseFileDisplayOrder = 1,
                              CaseFileNameDisplay = fileName,
                              //    CaseFilePath = subRootPath + fileName,
                              CaseFileSizeInBytes = fileSize,
                              CaseFileURL = null,
                              RecordStateID = 1,
                              GoogleFileID = gfileId,
                              GoogleFolderID = gfolderId,
                              EntityID = entityId,
                              EntityType = entityType

                          });
                Response.ContentType = "application/json";
                fileStatus.Add(new FileUploadStatus()
                {
                    name = fileName,
                    size = fileSize,
                    error = "The file was successfully uploaded!",
                    type = fileType

                });
                return Json(new { files = fileStatus });



            }


            return Json(new { Status = "Done" });
        }


        public virtual ActionResult DownloadDriveFile(string id, string fileName)
        {
            fileName = fileName.ToDecrypt().Replace(",", "").ToRemoveSplChars();
            var caseId = UserManager.UserExtended.CaseID;
            if (Request.QueryString["CaseId"] != null)
            {
                caseId = Request.QueryString["CaseId"].ToDecrypt().ToInt();
                UserManager.UpdateCaseStatusBar(caseId);
            }
            if (caseId == 0)
            {
                Bugsnag.AspNet.Client.Current.Notify(new Exception(string.Format("File not found File URL :{0} CaseID is {1}", fileName, UserManager.UserExtended.CaseID)));
                TempData["ErrorMessage"] = "File doesn't exist";
                return RedirectToAction("AttachFile");
            }
            var gservice = new JcatsGoogleDrive(caseId);
            Response.AddHeader("Content-Disposition", String.Format("{0}; filename={1}", ("Attachment"), fileName));

            byte[] fileBytes = gservice.DownloadFile(id, fileName);

            if (fileBytes == null)
            {
                Bugsnag.AspNet.Client.Current.Notify(new Exception(string.Format("File not found File URL :{0} CaseID is {1}", fileName, UserManager.UserExtended.CaseID)));
                TempData["ErrorMessage"] = "File doesn't exist";
                return RedirectToAction("AttachFile");
            }

            return File(fileBytes, gservice.GetMimeType(fileName));

        }





        [HttpPost]
        public virtual async Task<JsonResult> AttachFileUploadToSharePoint()
        {

            var fileStatus = new List<FileUploadStatus>();

            if (Request.Files.Count == 0)
            {

                Response.ContentType = "text/plain";
                Response.Write("No files received.");

            }
            else
            {
                var caseId = UserManager.UserExtended.CaseID;
                var agencyId = UserManager.UserExtended.CaseNumberAgencyID;
                var userId = UserManager.UserExtended.UserID;
                var uploadedfile = Request.Files[0];
                string entityType = Request.Form["EntityType"];
                int entityId = Request.Form["EntityId"].ToInt();

                var fileName = uploadedfile.FileName.ToRemoveSplChars();
                var fileType = uploadedfile.ContentType;
                var fileSize = uploadedfile.ContentLength;




                var oSharepoint = new Sharepoint(entityId: entityId, entityType: entityType);
                await oSharepoint.GetAccessTokenAsync();
                try
                {
                    #region
                    var oFile = oSharepoint.UploadFile(uploadedfile);
                    if (oFile != null)
                    {
                        if (oFile.d != null)
                        {
                            fileName = oFile.FileName;
                            var gfileId = oFile.d.UniqueId;
                            var roleId = Request.Form["RoleID"].ToInt();
                            var categoryId = Request.Form["CategoryID"].ToInt();
                            var fileDate = Request.Form["FileDate"].ToDateTime();
                            var description = Request.Form["description[]"];


                            var insertFileResult = UtilityService.ExecStoredProcedureScalar("NG_CaseFileInsert_sp",
                                      new NG_CaseFileInsert_spParams()
                                      {
                                          BatchLogJobID = Guid.NewGuid(),
                                          RoleID = roleId,
                                          UserID = UserManager.UserExtended.UserID,
                                          CaseID = UserManager.UserExtended.CaseID,

                                          CaseFileCategoryCodeID = categoryId,
                                          CaseFileDate = fileDate,
                                          CaseFileDescription = description,
                                          CaseFileName = fileName,
                                          CaseFileDisplayOrder = 1,
                                          CaseFileNameDisplay = fileName,

                                          CaseFileSizeInBytes = fileSize,
                                          CaseFilePath = oSharepoint.SharepointFilePath + fileName,
                                          RecordStateID = 1,
                                          EntityID = entityId,
                                          EntityType = entityType,
                                          SharePointFileID = gfileId

                                      });

                            Response.ContentType = "application/json";
                            fileStatus.Add(new FileUploadStatus()
                            {
                                name = fileName,
                                size = fileSize,
                                error = "The file was successfully uploaded!",
                                type = fileType

                            });
                            return Json(new { files = fileStatus });
                        }
                        else
                        {

                            Response.ContentType = "application/json";
                            fileStatus.Add(new FileUploadStatus()
                            {
                                name = fileName,
                                size = fileSize,
                                error = "The file could not uploaded",
                                type = fileType

                            });
                            return Json(new { files = fileStatus });


                        }
                    }
                    else
                    {
                        Response.ContentType = "application/json";
                        fileStatus.Add(new FileUploadStatus()
                        {
                            name = fileName,
                            size = fileSize,
                            error = "The file could not uploaded",
                            type = fileType

                        });
                        return Json(new { files = fileStatus });
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    Response.ContentType = "application/json";
                    fileStatus.Add(new FileUploadStatus()
                    {
                        name = fileName,
                        size = fileSize,
                        error = "Failed To Upload File:[" + ex.Message + "]",
                        type = fileType

                    });
                    return Json(new { files = fileStatus });
                }



            }


            return Json(new { Status = "Done" });
        }


        public virtual async Task<ActionResult> DownloadSharePointFile(string fileName, string filePath)
        {


            if (Request.QueryString["CaseId"] != null)
            {
                var caseId = Request.QueryString["CaseId"].ToDecrypt().ToInt();
                UserManager.UpdateCaseStatusBar(caseId);
            }
            var oSharepoint = new Sharepoint();

            await oSharepoint.GetAccessTokenAsync();

            Response.AddHeader("Content-Disposition", String.Format("{0}; filename={1}", ("Attachment"), fileName));

            byte[] fileBytes = oSharepoint.DownloadFile(filePath);

            if (fileBytes == null)
            {
                Bugsnag.AspNet.Client.Current.Notify(new Exception(string.Format("File not found File URL :{0} CaseID is {1}", fileName, UserManager.UserExtended.CaseID)));
                TempData["ErrorMessage"] = "File doesn't exist";
                return RedirectToAction("AttachFile");
            }

            return File(fileBytes, oSharepoint.GetMimeType(fileName));

        }




    }
}