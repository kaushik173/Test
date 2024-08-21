using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_MonthlyInvoiceList;
using LALoDep.Domain.Services;
using LALoDep.Core.Enums;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Models.Task;
using Omu.ValueInjecter;
using LALoDep.Domain.com_Report;
using System.IO;
using LALoDep.Core.Custom.Utility;
using Aspose.Words;
using System.Data;
using Aspose.Words.Tables;

namespace LALoDep.Controllers
{
    public partial class MonthlyInvoiceListController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public MonthlyInvoiceListController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;


        }
        // GET: MonthlyInvoiceList
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.MonthlyInvoiceList, PageSecurityItemID = SecurityToken.MonthlyInvoiceList)]
        public virtual ActionResult Search()
        {
            //UserManager.UserExtended.PersonID
            var viewModel = new MonthlyInvoiceListViewModel()
            {
                OnViewLoad = true
            };
            viewModel.AttorneyList =
                UtilityService.ExecStoredProcedureWithResults<pd_MonthlyInvoiceGetAttorneyList_spResult>(
                    "pd_MonthlyInvoiceGetAttorneyList_sp",
                    new pd_MonthlyInvoiceGetAttorneyList_spParams() { LoadOption = "SEARCH", UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).Select(e => new SelectListItem()
                    {
                        Value = e.PersonID.ToString(),
                        Text = e.PersonNameDisplay
                    }).ToList();

            viewModel.AgencyList = UtilityService.ExecStoredProcedureWithResults<pd_MonthlyInvoiceGetAgencyList_spResult>("pd_MonthlyInvoiceGetAgencyList_sp",
                            new pd_MonthlyInvoiceGetAgencyList_spParams() { LoadOption = "SEARCH", UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid() }).Select(e => new SelectListItem()
                            {
                                Value = e.AgencyID.ToString(),
                                Text = e.AgencyDisplay
                            }).ToList();
            viewModel.StatusList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>(
                        "pd_CodeGetByTypeIDAndUserID_sp",
                        new pd_CodeGetByTypeIDAndUserID_spParams() { UserID = UserManager.UserExtended.UserID, CodeTypeID = CodeType.MonthlyInvoiceStatus.GetHashCode(), BatchLogJobID = new Guid() }).Select(e => new SelectListItem()
                        {
                            Value = e.CodeID.ToString(),
                            Text = e.CodeShortValue
                        }).ToList();

            viewModel.CountyList = UtilityService.ExecStoredProcedureWithResults<pd_MonthlyInvoiceGetCountyList_spResult>(
                      "pd_MonthlyInvoiceGetCountyList_sp",
                      new pd_MonthlyInvoiceGetCountyList_spParams() { UserID = UserManager.UserExtended.UserID, LoadOption = "SEARCH", BatchLogJobID = new Guid() }).Select(e => new SelectListItem()
                      {
                          Value = e.AgencyCountyID.ToString(),
                          Text = e.CountyDisplay
                      }).ToList();


            if (UserManager.IsUserAccessToSecurity(SecurityToken.AddMonthlyInvoiceAdminMode))
            {
                viewModel.AddMonthlyInvoiceAdminMode = true;
                viewModel.AddInvoiceForList = UtilityService.ExecStoredProcedureWithResults<pd_MonthlyInvoiceGetAddInvoiceFor_spResult>(
                      "pd_MonthlyInvoiceGetAddInvoiceFor_sp",
                      new pd_MonthlyInvoiceGetAddInvoiceFor_spParams() { UserID = UserManager.UserExtended.UserID, LoadOption = "DEFAULT", BatchLogJobID = new Guid() }).Select(e => new SelectListItem()
                      {
                          Value = e.PersonID.ToEncrypt(),
                          Text = e.PersonNameDisplay
                      }).ToList();
                viewModel.AsOfDate = DateTime.Now.ToString("d");

            }
            if (viewModel.AgencyList.Count() == 1)
                viewModel.AgencyID = viewModel.AgencyList.ToList()[0].Value.ToInt();
            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult Search(MonthlyInvoiceListViewModel model)
        {
            var addMonthlyInvoiceAdminMode = UserManager.IsUserAccessToSecurity(SecurityToken.AddMonthlyInvoiceAdminMode);

            

                var result =
                UtilityService.ExecStoredProcedureWithResults<pd_MonthlyInvoiceSearch_spResult>(
                    "pd_MonthlyInvoiceSearch_sp",
                    new pd_MonthlyInvoiceSearch_spParams()
                    {
                        AgencyID = model.AgencyID,
                        AttorneyPersonID = model.AttorneyID,
                        InvoiceMonthlyID = model.InvoiceNumber,
                        BatchLogJobID = new Guid(),
                        InvoiceMonthlyStatusCodeID = model.StatusCodeID,
                        UserID = UserManager.UserExtended.UserID,
                        SortOption = null,
                        AgencyCountyID = model.CountyID,
                        
                    }).ToList();
            var monthlyInvoiceModel = result.Select(x => new
            {
                County = x.County,
                Attorney = x.Attorney,
                YearMonth = x.YearMonth,
                InvoiceNumber = x.InvoiceNumber,
                AmountNumber = x.InvoiceAmount,
                SubmitDate = x.SubmitDate,
                Status = x.Status,
                StatusDate = x.StatusDate,
                EncryptedID = x.InvoiceMonthlyID.ToEncrypt()
            });

            return Json(new DataTablesResponse(0, monthlyInvoiceModel, result.Count, result.Count));
        }

        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.EditMonthlyInvoice)]
        public virtual ActionResult EditInvoice(string id)
        {
            var invoiceParams = new pd_MonthlyInvoiceGet_spParams
            {
                InvoiceMontlyID = id.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            var viewModel = new MonthlyInvoiceEditModel();


            var invoiceInfo = UtilityService.ExecStoredProcedureWithResults<pd_MonthlyInvoiceGet_spResult>("pd_MonthlyInvoiceGet_sp", invoiceParams).FirstOrDefault();
            viewModel.InjectFrom(invoiceInfo);

            viewModel.StatusList = UtilityFunctions.CodeGetByTypeIdAndUserId(850, includeCodeId: viewModel.StatusCodeID ?? 0);

            viewModel.ClientDetails = UtilityService.ExecStoredProcedureWithResults<pd_MonthlyInvoiceDetailGetByMonthlyInvoiceID_spResult>(
                                            "pd_MonthlyInvoiceDetailGetByMonthlyInvoiceID_sp",
                                            new pd_MonthlyInvoiceDetailGetByMonthlyInvoiceID_spParams
                                            {
                                                LoadOption = "EDIT",
                                                InvoiceMonthlyID = invoiceParams.InvoiceMontlyID,
                                                UserID = UserManager.UserExtended.UserID,
                                                BatchLogJobID = Guid.NewGuid()
                                            }).ToList();
            if (viewModel.ClientDetails.Any())
                viewModel.DetailsHeaderDisplay = viewModel.ClientDetails.First().DetailsHeaderDisplay;

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult EditInvoice(MonthlyInvoiceEditModel viewModel)
        {
            var updateParams = new pd_MonthlyInvoiceUpdate_spParams
            {
                InvoiceMonthlyID = viewModel.InvoiceMonthlyID,
                StatusCodeID = viewModel.StatusCodeID,
                PaymentNumber = viewModel.PaymentNumber,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            if (!string.IsNullOrEmpty(viewModel.PaymentDate))
                updateParams.PaymentDate = viewModel.PaymentDate.ToDateTimeValue();

            UtilityService.ExecStoredProcedureWithResults<object>("pd_MonthlyInvoiceUpdate_sp", updateParams).FirstOrDefault();

            return Json(new { isSuccess = true });
        }

        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.AddMonthlyInvoice)]
        public virtual ActionResult AddInvoice(string addInvoiceFor,string asOfDate)
        {
            int? addInvoiceForId=null;

            if (!addInvoiceFor.IsNullOrEmpty())
            {
                addInvoiceForId = addInvoiceFor.ToDecrypt().ToInt();
            }
            var viewModel = new MonthlyInvoiceAddModel();
            viewModel.PersonName = UserManager.UserExtended.FullName;
            viewModel.InvoiceMonth = DateTime.Now.ToString("MMMM yyyy");

            var clientList = UtilityService.ExecStoredProcedureWithResults<pd_MonthlyInvoiceDetailGetByMonthlyInvoiceID_spResult>(
                                            "pd_MonthlyInvoiceDetailGetByMonthlyInvoiceID_sp",
                                            new pd_MonthlyInvoiceDetailGetByMonthlyInvoiceID_spParams
                                            {
                                                LoadOption = "DEFAULT",
                                                UserID = UserManager.UserExtended.UserID,
                                                BatchLogJobID = Guid.NewGuid(),
                                                AttorneyPersonID= addInvoiceForId,
                                                AsOfDate= asOfDate.ToDateTimeNullableValue()
                                            });

            if (clientList.Any())
            {
                var firstEle = clientList.First();
                viewModel.NotYetIncludedHeaderDisplay = firstEle.NotYetIncludedHeaderDisplay;
                viewModel.PrevioulyIncludedHeaderDisplay = firstEle.PrevioulyIncludedHeaderDisplay;
            }
            else
            {
                viewModel.NotYetIncludedHeaderDisplay = viewModel.PersonName + " - Clients Not Yet Included In " + viewModel.InvoiceMonth + " Invoice(0 Clients)";
                viewModel.PrevioulyIncludedHeaderDisplay = viewModel.PersonName + " - Clients Previously Included In " + viewModel.InvoiceMonth + "Invoice (0 Clients)";
            }
            viewModel.ClientNotIncluded = clientList.Where(x => x.AlreadySubmittedFlag != 1 && x.CaseID.HasValue).ToList();
            viewModel.ClientIncluded = clientList.Where(x => x.AlreadySubmittedFlag == 1 && x.CaseID.HasValue).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public virtual ActionResult AddInvoiceSave()
        {
            UtilityService.ExecStoredProcedureScalar(
                                            "pd_MonthlyInvoiceInsert_sp",
                                            new pd_MonthlyInvoiceInsert_spParams
                                            {
                                                SubmitDate = DateTime.Today,
                                                SubmitYear = DateTime.Today.Year,
                                                SubmitMonth = DateTime.Today.Month,
                                                UserID = UserManager.UserExtended.UserID,
                                                BatchLogJobID = Guid.NewGuid()
                                            });

            return Json(new { isSuccess = true });
        }

        [HttpPost]
        public virtual ActionResult PrintInvoice(string id)
        {
            var comReportSourceGetByReportIdSpParams = new com_ReportSourceGetByReportID_spParams()
            {
                ReportID = 114,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid().ToString(),
            };

            #region Delete Report Parameter

            var dictionaryParam = new Dictionary<string, object>()
            {
                {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
            };

            /*Delete Existing Report Parameters saved for this User*/
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueDelete_sp", dictionaryParam);
            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterHeaderDelete_sp", dictionaryParam);

            #endregion


            #region Insert Report Parameter
            /*Insert New Report Parameters saved for this User*/

            UtilityService.ExecStoredProcedureWithoutResults("com_ReportParameterValueInsert_sp",
                new Dictionary<string, object>()
                {
                    {"@ReportID", comReportSourceGetByReportIdSpParams.ReportID},
                    {"@UserID", comReportSourceGetByReportIdSpParams.UserID},
                    {"@BatchLogJobID", comReportSourceGetByReportIdSpParams.BatchLogJobID}
                    ,
                    {"@ReportParameterValueID", null},
                    {"@Sequence", 1}
                    ,
                    {"@Value", id.IsNullOrEmpty()?null:id}//Subpoena IDs
                });


            #endregion
            var spResult =
              UtilityService.ExecStoredProcedureWithResults<com_ReportSourceGetByReportID_spResult>(
                  "com_ReportSourceGetByReportID_sp", comReportSourceGetByReportIdSpParams).ToList();

            var reportSource = spResult.FirstOrDefault();



            if (reportSource != null)
            {
                if (reportSource.ReportSourceDocumentName.Contains(".doc"))
                {
                    //    var doc = new Document();
                    //    doc.RemoveAllChildren();

                    var filename = spResult[0].ReportDisplayName + ".doc";
                    filename = filename.Replace("&", "_");
                    filename = filename.Replace(",", "_");
                    filename = filename.Replace("(", "_");
                    filename = filename.Replace(")", "_");
                    filename = filename.Replace("/", "_");

                    var mergeTemplateRootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["MergeTemplateRootPath"];
                    if (reportSource.UseMasterFlag != 1)
                        mergeTemplateRootPath = mergeTemplateRootPath + reportSource.AgencyMergeTemplatePath + @"\";

                    var template = System.IO.File.ReadAllBytes(mergeTemplateRootPath + "MonthlyInvoiceNG.doc");




                    var mergeDataTAble = UtilityService.ExecStoredProcedureForDataTable("mrg_MonthlyInvoice_sp", comReportSourceGetByReportIdSpParams);
                    var mergeDataTAbleDetail = UtilityService.ExecStoredProcedureForDataTable("mrg_MonthlyInvoice_Sub_Details_sp", comReportSourceGetByReportIdSpParams);
                    mergeDataTAbleDetail.TableName = "tbl1";

                    if (mergeDataTAble != null && mergeDataTAble.Rows.Count > 0)
                    {
                        // get mail merge document in byte[]


                        string[] fieldNames = (from object column in mergeDataTAble.Columns select column.ToString()).ToArray();
                        var fieldValues = mergeDataTAble.Rows[0].ItemArray;

                        mergeDataTAbleDetail.Columns.Remove("InvoiceNbr");
                        mergeDataTAbleDetail.Columns["JcatsNbr"].Caption = "Jcats #";
                        mergeDataTAbleDetail.Columns["CaseNbr"].Caption = "Case #";
                        mergeDataTAbleDetail.Columns["ApptDate"].Caption = "Appt Date";
                        mergeDataTAbleDetail.Columns["PetitionDate"].Caption = "Petition Date";
                        mergeDataTAbleDetail.Columns["ClientNm"].Caption = "Client Name";
                        mergeDataTAbleDetail.Columns["PartyType"].Caption = "Party Type";
                        mergeDataTAbleDetail.Columns["CloseDate"].Caption = "Close Date";
                        // get mail merge document in byte[]

                        var doc = ExecuteMergeDocument(template, fieldNames, fieldValues, mergeDataTAbleDetail);
                        doc.Save(UtilityFunctions.GetDocumentDownloadFolderPath() + filename);

                        var generatedFilePath = UtilityFunctions.GetDocumentDownloadFolderPath() + filename;





                        return File(generatedFilePath, "application/force-download", filename);

                    }


                    return Json(new { errorMessage = "No records found" }, JsonRequestBehavior.AllowGet);



                }

            }
            else
            {
                return Json(new { errorMessage = "No records found" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { errorMessage = "No records found" }, JsonRequestBehavior.AllowGet);


        }
        public Document ExecuteMergeDocument(byte[] template, string[] fieldNames, Object[] fieldValues, DataTable dt)
        {
            var stream = new MemoryStream(template);
            // Load the template document.
            var doc = new Document(stream);

            // Setup mail merge event handler to do the custom work.
            //    doc.MailMerge.FieldMergingCallback = new HandleMergeField();
            doc.MailMerge.Execute(fieldNames, fieldValues);

            // Execute the mail merge.
            // doc.MailMerge.ExecuteWithRegions(dt);


            if (dt.Rows.Count > 0)
            {

                //   doc.LastSection.Body.AppendChild(AddTable(dt, doc));
                return AddTable(dt, doc);


            }



            return doc;
        }
        private static Document AddTable(DataTable dt, Document doc)
        {



            #region  Apend top 10 rows here
            Table tableTop = new Table(doc);
            var trH = new Row(doc);
            trH.RowFormat.HeadingFormat = true;
            for (var j = 0; j < dt.Columns.Count; j++)
            {
                var cell = new Cell(doc);
                cell.AppendChild(new Paragraph(doc));
                cell.FirstParagraph.AppendChild(new Run(doc, dt.Columns[j].Caption != null ? dt.Columns[j].Caption : ""));

                cell.CellFormat.Shading.BackgroundPatternColor = System.Drawing.Color.FromArgb(224, 224, 224);
                trH.AppendChild(cell);
            }
            tableTop.AppendChild(trH);

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var tr = new Row(doc);
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    var cell = new Cell(doc);
                    cell.AppendChild(new Paragraph(doc));
                    cell.FirstParagraph.AppendChild(new Run(doc, dt.Rows[i][j] != null ? dt.Rows[i][j].ToString() : ""));

                    tr.AppendChild(cell);
                }
                tableTop.AppendChild(tr);
            }
            doc.LastSection.Body.AppendChild(tableTop);

            #endregion
            return doc;


            #region Table created by document builder keep this for future refrence
            /*

            var doc1 = new Document();
            DocumentBuilder builder = new DocumentBuilder(doc1);
           
            Table table = builder.StartTable();
            builder.RowFormat.HeadingFormat = true;
            builder.Font.Size = 10;
            builder.Font.Bold = true;
          
             var trH = new Row(doc);
            for (var j = 0; j < dt.Columns.Count; j++)
            {

                builder.InsertCell();
                builder.Writeln(dt.Columns[j].Caption != null ? dt.Columns[j].Caption : "");

            }
            builder.EndRow();
            builder.Font.Size = 11;
            builder.Font.Bold = false;
            table.PreferredWidth = PreferredWidth.FromPercent(100);

            builder.CellFormat.Width = 50;
            builder.ParagraphFormat.ClearFormatting();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                
                for (var j = 0; j < dt.Columns.Count; j++)
                {

                    builder.InsertCell();
                    builder.RowFormat.HeadingFormat = false;
                    builder.Write(dt.Rows[i][j] != null ? dt.Rows[i][j].ToString() : "");
                }
                builder.EndRow();
            }
            var docReturn = new Document();

             doc.AppendDocument(doc1, ImportFormatMode.KeepSourceFormatting);
            return doc;*/
            #endregion
        }

    }
}