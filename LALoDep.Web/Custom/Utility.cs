using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Aspose.Words;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Conflict;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Models;
using ClosedXML.Excel;
using LALoDep.Core.NG_sp.NG_Jcats;
using LALoDep.Domain.TitleIVe;
using System.Text.RegularExpressions;
using System.Threading;
using NPOI.HSSF.UserModel;
using System.Globalization;
using NPOI.SS.UserModel;
using LALoDep.Models.CaseOpening;
using DocumentFormat.OpenXml.ExtendedProperties;
using LALoDep.Models.Task;
using LALoDep.Domain.pd_Hearing;

namespace LALoDep.Custom
{
    public class UtilityFunctions
    {
        public static IEnumerable<CodeViewModel> CodeGetWithSortValByTypeIdAndUserId(int codeTypeId, string sortOption = "",
            int includeCodeId = 0, int agencyId = 0)
        {

            return UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>(
                    "pd_CodeGetByTypeIDAndUserID_sp", new pd_CodeGetByTypeIDAndUserID_spParams
                    {

                        CodeTypeID = codeTypeId,
                        SortOption = sortOption,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserEnvironment.UserManager.UserExtended.UserID,
                        AgencyID = agencyId,
                        IncludeCodeID = includeCodeId

                    }).Select(o => new CodeViewModel() { CodeID = o.CodeID, CodeValue = o.CodeValue, CodeShortValue = o.CodeShortValue, CodeDisplay = o.CodeDisplay }).ToList();

        }
        public static IEnumerable<SelectListItem> TitleIVeCodeDropDown(int codeTypeId,
           bool userShortValue = false, bool encryptedValue = false, bool enumValueBind = false)
        {

            var data = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<TitleIVeCodeDropDown_spResult>(
                    "TitleIVeCodeDropDown_sp", new TitleIVeCodeDropDown_spParams
                    {

                        CodeTypeID = codeTypeId,

                        UserID = UserEnvironment.UserManager.UserExtended.UserID,


                    }).Select(o => new SelectListItem() { Text = userShortValue ? o.CodeShortValue : o.CodeValue, Value = enumValueBind ? o.CodeEnumName : (encryptedValue ? o.CodeID.ToEncrypt() : o.CodeID.ToString()) }).ToList();

            return data;
        }
        public static IEnumerable<SelectListItem> CodeGetByTypeIdAndUserId(int codeTypeId, string sortOption = "",
            int includeCodeId = 0, int agencyId = 0, bool userShortValue = false, bool encryptedValue = false, bool includeShortCodeValue = false)
        {

            var data = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>(
                    "pd_CodeGetByTypeIDAndUserID_sp", new pd_CodeGetByTypeIDAndUserID_spParams
                    {

                        CodeTypeID = codeTypeId,
                        SortOption = sortOption,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserEnvironment.UserManager.UserExtended.UserID,
                        AgencyID = agencyId,
                        IncludeCodeID = includeCodeId

                    }).Select(o => new SelectListItem() { Text = userShortValue ? o.CodeShortValue : o.CodeValue, Value = encryptedValue ? o.CodeID.ToEncrypt() + (includeShortCodeValue ? "|" + o.CodeShortValue : "") : o.CodeID.ToString() + (includeShortCodeValue ? "|" + o.CodeShortValue : "") }).ToList();

            return data;
        }
        public static IEnumerable<SelectListItem> CodeGetWorkIVeEligible(int? workIVeEligibleCodeId = null, int? workId = null, string sortOption = "",
         int includeCodeId = 0, int agencyId = 0, bool encryptedValue = false)
        {

            var data = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<CodeGetWorkIVeEligible_spResult>(
                    "CodeGetWorkIVeEligible_sp", new CodeGetWorkIVeEligible_spParams
                    {
                        SortOption = sortOption,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserEnvironment.UserManager.UserExtended.UserID,
                        AgencyID = agencyId,

                        WorkIVeEligibleCodeID = workIVeEligibleCodeId,
                        WorkID = workId
                    }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = encryptedValue ? o.CodeID.ToEncrypt() : o.CodeID.ToString() }).ToList();

            return data;
        }
        public static IEnumerable<SelectListItem> CodeGetWorkDescription(int? workDescriptionCodeId = null, int? workId = null, string sortOption = "",
          int includeCodeId = 0, int agencyId = 0, bool encryptedValue = false, int? referralId = null)
        {

            var data = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<CodeGetWorkDescription_spResult>(
                    "CodeGetWorkDescription_sp", new CodeGetWorkDescription_spParams
                    {
                        SortOption = sortOption,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserEnvironment.UserManager.UserExtended.UserID,
                        AgencyID = agencyId,
                        WorkDescriptionCodeID = workDescriptionCodeId,
                        WorkID = workId,
                        ReferralID = referralId
                    }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = encryptedValue ? o.CodeID.ToEncrypt() : o.CodeID.ToString() + "|" + (o.DefaultIVeEligibleCodeID.HasValue ? o.DefaultIVeEligibleCodeID.Value.ToString() : "") + "|" + (o.DefaultCanChangeFlag.HasValue ? o.DefaultCanChangeFlag.Value.ToString() : "0") + "|" + (o.AttorneyDefaultIVeEligibleCodeID.HasValue ? o.AttorneyDefaultIVeEligibleCodeID.Value.ToString() : "") + "|" + (o.AttorneyDefaultCanChangeFlag.HasValue ? o.AttorneyDefaultCanChangeFlag.Value.ToString() : "0") + "|" + (o.SupervisorDefaultIVeEligibleCodeID.HasValue ? o.SupervisorDefaultIVeEligibleCodeID.Value.ToString() : "") + "|" + (o.SupervisorDefaultCanChangeFlag.HasValue ? o.SupervisorDefaultCanChangeFlag.Value.ToString() : "0") + "|" + (o.UseWorkTimeFlag.HasValue ? o.UseWorkTimeFlag.Value.ToString() : "0") + "|" + (o.ZipCodeRequiredFlag.HasValue ? o.ZipCodeRequiredFlag.Value.ToString() : "0") }).ToList();

            return data;
        }
        public static IEnumerable<SelectListItem> CodeGetBySystemValueTypeId(string systemValueIdList, int includeCodeId = 0, int agencyId = 0)
        {

            return UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_CodeGetBySystemValueTypeID_spResults>(
                    "pd_CodeGetBySystemValueTypeID_sp", new pd_CodeGetBySystemValueTypeID_spParams
                    {

                        SystemValueIDList = systemValueIdList,

                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserEnvironment.UserManager.UserExtended.UserID,
                        AgencyID = agencyId,
                        IncludeCodeID = includeCodeId

                    }).Select(o => new SelectListItem() { Text = o.CodeValue, Value = o.CodeID.ToString() }).ToList();

        }

        public static IEnumerable<SelectListItem> CodeGetBySystemValueTypeId(int systemValueId, int includeCodeId = 0, int agencyId = 0, bool userShortValue = false)
        {

            return UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_CodeGetBySysValAndUserID_spResults>(
                    "pd_CodeGetBySysValAndUserID_sp", new pd_CodeGetBySysValAndUserID_spParams
                    {
                        SystemValueIDList = systemValueId,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserEnvironment.UserManager.UserExtended.UserID,
                        AgencyID = agencyId,
                        IncludeCodeID = includeCodeId
                    }).Select(o => new SelectListItem() { Text = userShortValue ? o.CodeShortValue : o.CodeValue, Value = o.CodeID.ToString() }).ToList();

        }

        public static IEnumerable<SelectListItem> CodeGetByTypeIDAndUserIDSortShortValue(int codeTypeId, bool combineShortValue = false, bool combineLongValue = false, int agencyId = 0)
        {

            return UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserIDSortShortValue_spResults>(
                    "pd_CodeGetByTypeIDAndUserIDSortShortValue_sp", new pd_CodeGetByTypeIDAndUserIDSortShortValue_spParams
                    {
                        CodeTypeID = codeTypeId,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserEnvironment.UserManager.UserExtended.UserID,
                        AgencyID = agencyId

                    }).Select(o => new SelectListItem() { Text = (combineShortValue ? o.CodeValue + " (" + o.CodeShortValue + ")" : (combineLongValue ? o.CodeShortValue + " (" + o.CodeValue + ")" : o.CodeValue)), Value = o.CodeID.ToString() }).ToList();



        }

        public static string GetGoogleApiKeyFromConfig()
        {
            return System.Web.Configuration.WebConfigurationManager.AppSettings["GoogleAddressApiKey"];
        }
        public static IEnumerable<pd_NoteGetByRFDIDSystemValueTypeID_spResult> NoteGetByRFDIDSystemValueTypeID(int systemValueTypeId, int rfdid)
        {

            return UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_NoteGetByRFDIDSystemValueTypeID_spResult>(
                    "pd_NoteGetByRFDIDSystemValueTypeID_sp", new pd_NoteGetByRFDIDSystemValueTypeID_spParams
                    {
                        RFDID = rfdid,
                        SystemValueTypeID = systemValueTypeId,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserEnvironment.UserManager.UserExtended.UserID,

                    });



        }
        public static pd_CodeGet_spResult CodeGet(int codeid)
        {

            return UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_CodeGet_spResult>("pd_CodeGet_sp",
                  new pd_CodeGet_spParams()
                  {
                      CodeID = codeid,
                      BatchLogJobID = Guid.NewGuid(),
                      UserID = UserEnvironment.UserManager.UserExtended.UserID,
                  }).FirstOrDefault();



        }

        public static pd_NoteGet_spResult NoteGet(int noteId)
        {

            return UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_NoteGet_spResult>("pd_NoteGet_sp",
                  new pd_NoteGet_spParams()
                  {
                      NoteID = noteId,
                      BatchLogJobID = Guid.NewGuid(),
                      UserID = UserEnvironment.UserManager.UserExtended.UserID,
                  }).FirstOrDefault();



        }

        public static int NoteInsert(int noteEntitySystemValueTypeId, int noteEntityTypeSystemValueTypeId, int entityPrimaryKeyId, int noteTypeCodeId, string subject, string noteEntry, int? hearingId = null, int? petitionId = null, int? agencyId = null, int? caseId = null)
        {

            var noteid = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureScalar("pd_NoteInsert_sp",
                new pd_NoteInsert_spParams()
                {
                    NoteEntitySystemValueTypeID = noteEntitySystemValueTypeId,
                    NoteEntityTypeSystemValueTypeID = noteEntityTypeSystemValueTypeId,
                    EntityPrimaryKeyID = entityPrimaryKeyId,
                    NoteTypeCodeID = noteTypeCodeId,
                    NoteSubject = subject,
                    NoteEntry = noteEntry,
                    CaseID = caseId.HasValue ? caseId.Value : UserEnvironment.UserManager.UserExtended.CaseID,

                    RecordStateID = 1,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserEnvironment.UserManager.UserExtended.UserID,
                });


            return noteid.ToInt();
        }
        public static void NoteUpdate(int noteId, int noteEntityCodeId, int noteEntityTypeCodeId, int entityPrimaryKeyId, int noteTypeCodeId, string subject, string noteEntry, int? hearingId = null, int? petitionId = null, int? agencyId = null)
        {

            UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithoutResultADO("pd_NoteUpdate_sp",
                new del_NoteUpdate_spParams()
                {
                    NoteEntityCodeID = noteEntityCodeId,
                    NoteEntityTypeCodeID = noteEntityTypeCodeId,
                    EntityPrimaryKeyID = entityPrimaryKeyId,
                    NoteTypeCodeID = noteTypeCodeId,
                    NoteSubject = subject,
                    NoteEntry = noteEntry,
                    CaseID = UserEnvironment.UserManager.UserExtended.CaseID,
                    RecordStateID = 1,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserEnvironment.UserManager.UserExtended.UserID,
                    NoteID = noteId
                });



        }
        public static void NoteDelete(int noteId)
        {

            UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithoutResultADO("pd_NoteDelete_sp",
               new pd_NoteDelete_spParams()
               {
                   BatchLogJobID = Guid.NewGuid(),
                   UserID = UserEnvironment.UserManager.UserExtended.UserID,
                   ID = noteId,
                   RecordStateID = 10,
                   LoadOption = "Note"
               });


        }
        public static void ExportDataTableToExcel(DataTable dt, string destination)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(dt);
            var table = ws.Tables.FirstOrDefault();

            table.ShowAutoFilter = false;
            table.HeadersRow().Style.Font.Bold = true;
            table.Theme = ClosedXML.Excel.XLTableTheme.None;

            var count = 1;
            var lastRow = table.LastRowUsed().RowNumber();
            foreach (var column in table.Columns())
            {
                if (column.Cell(1).Value.ToString().Contains("Date") || column.Cell(1).Value.ToString().Contains("DOB"))
                    column.Cells(2, lastRow).SetDataType(XLCellValues.DateTime);
                else if (column.Cell(1).Value.ToString().Contains("30Day Milestone")
                    || column.Cell(1).Value.ToString().Contains("60Day Milestone")
                    || column.Cell(1).Value.ToString().Contains("90Day Milestone")
                    || column.Cell(1).Value.ToString().Contains("6Month Milestone")
                    || column.Cell(1).Value.ToString().Contains("1Year Milestone")
                    || column.Cell(1).Value.ToString().Contains("Closure Note"))
                {

                    ws.Column(count).Width = 50;
                    ws.Column(count).Style.Alignment.WrapText = true;

                }
                count++;
            }
            foreach (var row in table.Rows())
            {
                foreach (var cell in row.Cells())
                {

                    if (cell.DataType == XLCellValues.Text)
                        cell.Value = ReplaceHexadecimalSymbols(cell.Value.ToString());
                }
            }

            wb.SaveAs(destination);

        }
        static string ReplaceHexadecimalSymbols(string txt)
        {
            string r = "[\x00-\x08\x0B\x0C\x0E-\x1F\x26]";
            return Regex.Replace(txt, r, "", RegexOptions.Compiled);
        }
        public static IEnumerable<pd_NoteGetByEntity_spResult> NoteGetByEntity(int entityPrimaryKeyId, int entityCodeSystemValueTypeId, int entityCodeTypeSystemValueTypeId)
        {

            return UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<pd_NoteGetByEntity_spResult>(
                    "pd_NoteGetByEntity_sp", new pd_NoteGetByEntity_spParams
                    {
                        EntityPrimaryKeyID = entityPrimaryKeyId,
                        EntityCodeSystemValueTypeID = entityCodeSystemValueTypeId,
                        EntityCodeTypeSystemValueTypeID = entityCodeTypeSystemValueTypeId,
                        BatchLogJobID = Guid.NewGuid(),
                        UserID = UserEnvironment.UserManager.UserExtended.UserID,

                    });



        }
        public static byte[] Execute(byte[] template, string[] fieldNames, Object[] fieldValues)
        {
            var stream = new MemoryStream(template);

            // Load the template document.
            Document doc = new Document(stream);
            // Setup mail merge event handler to do the custom work.
            doc.MailMerge.FieldMergingCallback = new HandleMergeField();

            // Execute the mail merge.
            doc.MailMerge.Execute(fieldNames, fieldValues);

            byte[] generatedFileContent = null;

            using (var ms = new MemoryStream())
            {
                doc.Save(ms, SaveFormat.Doc);
                generatedFileContent = ms.ToArray();
            }

            return generatedFileContent;
        }
        public static string GetDocumentDownloadFolderPath(int userId = 0)
        {

            var path = HttpContext.Current.Server.MapPath("~") + "MergeTemplate\\Download\\" + (userId > 0 ? userId.ToString() : UserEnvironment.UserManager.UserExtended.UserID.ToString()) + "\\";
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);




            return path;
        }
        public static void DeleteFilesFromDocumentDownloadFolderPath(int userId = 0)
        {

            var path = HttpContext.Current.Server.MapPath("~") + "MergeTemplate\\Download\\" + (userId > 0 ? userId.ToString() : UserEnvironment.UserManager.UserExtended.UserID.ToString()) + "\\";
            if (System.IO.Directory.Exists(path))
            {
                var files = System.IO.Directory.GetFiles(path);
                foreach (var file in files)
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch
                    {

                    }

                }

            }





        }
        public static string GetDocumentDownloadFolderRelativePath(int userId = 0)
        {

            var path = "~/MergeTemplate/Download/" + (userId > 0 ? userId.ToString() : UserEnvironment.UserManager.UserExtended.UserID.ToString()) + "/";

            return path;
        }
        public static void DeleteFilesOlderThanXDays(int userId)
        {
            var days = System.Web.Configuration.WebConfigurationManager.AppSettings["DeleteFilesOlderThanXDays"].ToInt();
            if (days > 0)
            {
                var files = Directory.GetFiles(GetDocumentDownloadFolderPath(userId));
                foreach (var file in files)
                {
                    try
                    {


                        var createDate = File.GetCreationTime(file);
                        var timeSpan = createDate - DateTime.Now;
                        if (timeSpan.Days > days)
                        {
                            File.Delete(file);
                        }
                    }
                    catch { }
                }
            }

        }
        public static void DeleteRecord(string spName, int id)
        {

            UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithoutResultADO(spName,
               new pd_Delete_spParams()
               {
                   BatchLogJobID = Guid.NewGuid(),
                   UserID = UserEnvironment.UserManager.UserExtended.UserID,
                   ID = id
               });


        }
        public static string GetNoteControlType(string pageUrl, int? caseId = null, int? noteId = null)
        {
            var controlType = UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<NoteControlTypeGet_spResult>("NoteControlTypeGet_sp", new NoteControlTypeGet_spParams()
            {
                NoteID = noteId,
                UserID = UserEnvironment.UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                CaseID = caseId ?? UserEnvironment.UserManager.UserExtended.CaseID,
                AgencyID = UserEnvironment.UserManager.UserExtended.CaseNumberAgencyID,
                NG_NavigationURL = pageUrl

            }).FirstOrDefault();
            if (controlType != null)
            {
                return controlType.ControlType;
            }
            return "";
        }
        #region Config Settings
        public static NG_JcatsNGConfigGet_spResult JcatsNGConfigGetByName(int caseId, string configName)
        {
            if (caseId == 0)
                caseId = UserEnvironment.UserManager.UserExtended.CaseID;

            var spParams = new NG_JcatsNGConfigGet_spParams()
            {
                UserID = UserEnvironment.UserManager.UserExtended.UserID,
                CaseID = caseId,
                JcatsNGConfigName = configName
            };
            return UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<NG_JcatsNGConfigGet_spResult>("NG_JcatsNGConfigGet_sp", spParams).FirstOrDefault();
        }

        public static List<NG_JcatsNGConfigGet_spResult> JcatsNGConfigGetAll(int caseId)
        {
            if (caseId == 0)
                caseId = UserEnvironment.UserManager.UserExtended.CaseID;

            var spParams = new NG_JcatsNGConfigGet_spParams()
            {
                UserID = UserEnvironment.UserManager.UserExtended.UserID,
                CaseID = caseId
            };
            return UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithResults<NG_JcatsNGConfigGet_spResult>("NG_JcatsNGConfigGet_sp", spParams).ToList();
        }
        public static void LogFile(string fileName, string content)
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("~/log/")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/log/"));
            }
            var path = HttpContext.Current.Server.MapPath("~/log/" + fileName);
            if (File.Exists(path))
            {
                var lines = File.ReadAllLines(path).ToList();
                lines.Add(content);

                File.WriteAllLines(path, lines);
            }
            else
            {
                File.WriteAllLines(path, new string[] { content });
            }


        }
        #endregion

        public static DataTable GetDataTableFromXlsFile(string filePath)
        {
            var prevCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            ISheet sheet;
            string filename = Path.GetFileName(filePath);
            //XSSFWorkbook hssfwbXSSFWorkbook = null;
            HSSFWorkbook hssfwb = null;
            var fileExt = Path.GetExtension(filePath);
            if (fileExt == ".xls")
            {
                Thread.Sleep(5000);
                hssfwb = new HSSFWorkbook(System.IO.File.OpenRead(filePath));
                sheet = hssfwb.GetSheetAt(0);
            }
            else
            {
                return GetDataTableFromXlsxFile(filePath);
                // hssfwbXSSFWorkbook = new XSSFWorkbook(filePath);

                // sheet = hssfwbXSSFWorkbook.GetSheetAt(0);
            }

            var table = new DataTable();
            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                if (headerRow.GetCell(i) != null)
                {
                    var column = new DataColumn(headerRow.GetCell(i).StringCellValue.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace(" ", ""));
                    table.Columns.Add(column);
                }
                else
                {
                    var column = new DataColumn(i.ToString());
                    table.Columns.Add(column);
                }
            }
            int rowCount = sheet.LastRowNum;
            for (int i = (sheet.FirstRowNum); i < sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i + 1);
                if (row != null)
                {
                    DataRow dataRow = table.NewRow();
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {


                        try
                        {

                            if (row.GetCell(j) != null)
                            {
                                if (table.Columns[j] != null)
                                {
                                    dataRow[j] = row.GetCell(j).ToString();
                                }
                            }


                        }
                        catch (Exception ex)
                        {

                        }


                    }
                    table.Rows.Add(dataRow);

                }

            }
            sheet = null;
            //   hssfwbXSSFWorkbook = null;

            hssfwb = null;
            return table;
        }


        public static DataTable GetDataTableFromXlsxFile(string filePath)
        {
            DataTable dt = new DataTable();
            using (XLWorkbook workBook = new XLWorkbook(filePath))
            {
                //Read the first Sheet from Excel file.
                IXLWorksheet workSheet = workBook.Worksheet(1);

                //Create a new DataTable.


                //Loop through the Worksheet rows.
                bool firstRow = true;
                foreach (IXLRow row in workSheet.Rows())
                {
                    //Use the first row to add columns to DataTable.
                    if (firstRow)
                    {
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Columns.Add(cell.Value.ToString());
                        }
                        firstRow = false;
                    }
                    else
                    {
                        //Add rows to DataTable.
                        dt.Rows.Add();
                        int i = 0;
                        foreach (IXLCell cell in row.Cells())
                        {
                            dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                            i++;
                        }
                    }


                }
            }
            return dt;
        }


        public static void HearingContinuanceReasonAndRequestDataInsert(HearingModel model = null, AppearanceSheetViewModel appearanceSheetViewModel = null)
        {
            var hearingContinuanceRequestedByIUDList = new List<HearingContinuanceRequestedByIUD_spParams>();

            var hearingContinuanceReasonIUDList = new List<HearingContinuanceReasonIUD_spParams>();

            var hearingId = 0;
            if (model != null)
            {
                hearingContinuanceRequestedByIUDList = model.HearingContinuanceRequestedByIUDList.ToList();
                hearingContinuanceReasonIUDList = model.HearingContinuanceReasonIUDList.ToList();
                hearingId = model.HearingID;

            }
            else if (appearanceSheetViewModel != null)
            {
                hearingId = appearanceSheetViewModel.HearingID;
                hearingContinuanceRequestedByIUDList = appearanceSheetViewModel.HearingContinuanceRequestedByIUDList.ToList();
                hearingContinuanceReasonIUDList = appearanceSheetViewModel.HearingContinuanceReasonIUDList.ToList();
            }
            if (hearingContinuanceRequestedByIUDList != null && hearingContinuanceRequestedByIUDList.Any())
            {
                foreach (var parms in hearingContinuanceRequestedByIUDList)
                {
                    parms.UserID = UserEnvironment.UserManager.UserExtended.UserID;
                    parms.HearingID = hearingId;
                    UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithoutResultADO("HearingContinuanceRequestedByIUD_sp", parms);


                }
            }
            if (hearingContinuanceReasonIUDList != null && hearingContinuanceReasonIUDList.Any())
            {
                foreach (var parms in hearingContinuanceReasonIUDList)
                {
                    parms.UserID = UserEnvironment.UserManager.UserExtended.UserID;
                    parms.HearingID = hearingId;
                    UserEnvironment.UserManager.UtilityService1.ExecStoredProcedureWithoutResultADO("HearingContinuanceReasonIUD_sp", parms);
                }

            }

        }

    }
}