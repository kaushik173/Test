using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Domain.com_Report;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using DataTables.Mvc;
using LALoDep.Models.Task;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Role;
using LALoDep.Models;
using LALoDep.Custom;
using System.Data;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using LALoDep.Domain.pd_Training;
using LALoDep.Domain.TrainingImport;
using ClosedXML.Excel;
using System.Threading;
using System.Globalization;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {


        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.TrainingImport)]
        public virtual ActionResult TrainingUpload()
        {

            var viewModel = new TrainingUploadViewModel();



            return View(viewModel);
        }


        [HttpPost]
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.TrainingImport)]
        public virtual ActionResult TrainingUpload(FormCollection form)
        {
            var viewModel = new TrainingUploadViewModel();

            #region FileUpload
            HttpPostedFileBase file = Request.Files[0];
            string FileExtension = Path.GetExtension(file.FileName);
            if (FileExtension.ToLower() != ".xls" && FileExtension.ToLower() != ".xlsx")
            {
                viewModel.ErrorMessage = "Invalid File";
                return View(viewModel);
            }

            string FileName = Path.GetFileNameWithoutExtension(file.FileName);
            string UploadPath = Server.MapPath("~/Documents/UploadTrainingFiles/");
            if (!Directory.Exists(UploadPath))
            {
                Directory.CreateDirectory(UploadPath);
            }
            viewModel.FileName = Guid.NewGuid().ToString() + "-" + DateTime.Now.ToString("yyyyMMddhhss") + "-" + FileName.Trim() + FileExtension;
            FileName = UploadPath + viewModel.FileName;


            file.SaveAs(FileName);


            #endregion
            try
            {


                var dt = GetDataTableFromXlsFile(FileName);
                var columns = new[] { "JcatsPersonID", "CourseTitle", "Provider", "SubjectMatter", "JcatsCreditTypeCodeID", "Participatory", "Hours", "StartDate", "EndDate", "JcatsVenueCodeID" };
                foreach (var col in columns)
                {
                    if (dt.Columns[col] == null)
                    {
                        viewModel.ErrorMessage = col + " column not found in uploaded file";
                        break;
                    }
                }
                if (!viewModel.ErrorMessage.IsNullOrEmpty())
                {
                    return View(viewModel);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    var s = dr["StartDate"];
                    viewModel.TrainingUploadFileModelList.Add(new TrainingUploadFileModel()
                    {
                        CourseTitle = dr["CourseTitle"] != DBNull.Value ? dr["CourseTitle"].ToString() : "",
                        EndDate = dr["EndDate"] != DBNull.Value ? dr["EndDate"].ToDateTime().ToString("d") : "",
                        Hours = dr["Hours"].ToDecimal(),
                        JcatsCreditTypeCodeID = dr["JcatsCreditTypeCodeID"].ToIntNullable(),
                        JcatsPersonID = dr["JcatsPersonID"].ToIntNullable(),
                        JcatsVenueCodeID = dr["JcatsVenueCodeID"].ToIntNullable(),
                        Participatory = dr["Participatory"].ToInt() == 1,
                        Provider = dr["Provider"] != DBNull.Value ? dr["Provider"].ToString() : "",
                        StartDate = dr["StartDate"] != DBNull.Value ? dr["StartDate"].ToDateTime().ToString("d") : "",
                        SubjectMatter = dr["SubjectMatter"] != DBNull.Value ? dr["SubjectMatter"].ToString() : "",

                    });
                }
                viewModel.TrainingUploadFileModelForAddModeList.Add(new TrainingUploadFileModel()
                {
                    CourseTitle = "",
                    EndDate = "",





                });

                viewModel.VenueList = UtilityService.ExecStoredProcedureWithResults<TrainingImport_GetVenueList_spResult>(
                             "TrainingImport_GetVenueList_sp", new TrainingImport_GetVenueList_spParams
                             {

                                 BatchLogJobID = Guid.NewGuid(),
                                 UserID = UserManager.UserExtended.UserID

                             }).Select(o => new SelectListItem() { Text = o.Venue, Value = o.VenueCodeID.ToString() }).ToList();

                viewModel.CreditTypeList = UtilityService.ExecStoredProcedureWithResults<TrainingImport_GetCreditTypeList_spResult>(
                        "TrainingImport_GetCreditTypeList_sp", new TrainingImport_GetCreditTypeList_spParams
                        {

                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID

                        }).Select(o => new SelectListItem() { Text = o.CreditType, Value = o.CreditTypeCodeID.ToString() }).ToList();

                viewModel.PersonList = UtilityService.ExecStoredProcedureWithResults<TrainingImport_GetPersonList_spResult>(
                  "TrainingImport_GetPersonList_sp", new TrainingImport_GetPersonList_spParams
                  {

                      BatchLogJobID = Guid.NewGuid(),
                      UserID = UserManager.UserExtended.UserID

                  }).Select(o => new SelectListItem() { Text = o.PersonDisplay, Value = o.PersonID.ToString() }).ToList();
            }
            catch
            {
                viewModel.ErrorMessage = "Invalid File";
                return View(viewModel);
            }
            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult ImportTraining(TrainingUploadViewModel importList)
        {
            var MessageDisplay = "";
            if (importList.TrainingUploadFileModelList.Any())
            {

                foreach (var item in importList.TrainingUploadFileModelList)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO(
            "TrainingImport_StageRecord_sp", new TrainingImport_StageRecord_spParams
            {

                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID,
                CourseTitle = item.CourseTitle,
                EndDate = item.EndDate,
                FileName = importList.FileName,
                Hours = item.Hours.Value.ToString(),
                JcatsCreditTypeCodeID = item.JcatsCreditTypeCodeID.Value.ToString(),
                JcatsPersonID = item.JcatsPersonID.Value.ToString(),
                JcatsVenueCodeID = item.JcatsVenueCodeID.Value.ToString(),
                Participatory = item.Participatory ? "1" : "0",
                Provider = item.Provider,
                StartDate = item.StartDate,
                SubjectMatter = item.SubjectMatter
            });

                }

                var result = UtilityService.ExecStoredProcedureWithResults<TrainingImport_ProcessPendingRecords_spResult>(
                "TrainingImport_ProcessPendingRecords_sp", new TrainingImport_ProcessPendingRecords_spParams
                {

                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID,

                    FileName = importList.FileName,

                }).FirstOrDefault();
                if (result != null)
                {
                    MessageDisplay = result.MessageDisplay;
                }

            }


            return Json(new { Status = "Done", Message = MessageDisplay });
        }

        public DataTable GetDataTableFromXlsFile(string filePath)
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


        public DataTable GetDataTableFromXlsxFile(string filePath)
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


    }
}