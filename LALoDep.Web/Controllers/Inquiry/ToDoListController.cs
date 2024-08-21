using DataTables.Mvc;
using LALoDep.Domain.pd_EmployeeRoster;
using LALoDep.Domain.Services;
using LALoDep.Custom;
using LALoDep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.PD_PDAction;
using LALoDep.Domain;
using Omu.ValueInjecter;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using LALoDep.Domain.rpt_Print;
using LALoDep.Domain.pd_Staff;
using System.Data.Entity.Core.Objects;

namespace LALoDep.Controllers
{
    public partial class InquiryController : Controller
    {
        // GET: /ToDoListController/
        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.ToDoList)]
        public virtual ActionResult ToDoList(string id)
        {
            var viewModel = new ToDoListViewModel();

            viewModel.ActionTypes = UtilityService.Context.pd_CodeGetAllBySystemValueTypeID_sp(150, UserManager.UserExtended.UserID, Guid.NewGuid()).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();

            viewModel.Status = UtilityService.Context.pd_CodeGetAllBySystemValueTypeID_sp(151, UserManager.UserExtended.UserID, Guid.NewGuid()).Select(x => new CodeViewModel()
            {
                CodeID = x.CodeID,
                CodeValue = x.CodeValue
            }).ToList();

            var person = UtilityService.Context.pd_PersonGet_sp(UserManager.UserExtended.PersonID, UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
            if (person != null)
                viewModel.UserName = person.FirstName + " " + person.LastName;

            //defualt val
            viewModel.StartDate = DateTime.Now.AddMonths(-2).ToShortDateString();
            viewModel.EndDate = DateTime.Now.AddMonths(1).ToShortDateString();
            viewModel.DateType = 1;
            viewModel.ActionStatusCodeID = 3382;
            if (!string.IsNullOrEmpty(id))
            {
                viewModel.ActionTypeCodeID = id.ToDecrypt().ToInt();
                //viewModel.StartDate = null;
                if (TempData["EndDate"] != null)
                    viewModel.EndDate = TempData["EndDate"].ToString();

                TempData["TODOSaved"] = true;
            }

            ViewBag.PageTitle = "To Do List <br/>" + viewModel.UserName;

            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult ToDoListSearch(ToDoListViewModel viewModel)
        {
            if (viewModel.StatusChangeIDList != "," && !string.IsNullOrEmpty(viewModel.StatusChangeIDList))
            {
                string updateIdList = string.Empty;
                viewModel.StatusChangeIDList = viewModel.StatusChangeIDList.Remove(viewModel.StatusChangeIDList.IndexOf(","), 1);
                viewModel.StatusChangeIDList = viewModel.StatusChangeIDList.Remove(viewModel.StatusChangeIDList.LastIndexOf(","), 1);
                string[] Idlist = viewModel.StatusChangeIDList.Split(',');
                if (Idlist != null && Idlist.Count() > 0)
                {
                    var count = 0;
                    for (var i = 0; i < Idlist.Length; i++)
                    {
                        updateIdList = updateIdList + Idlist[i].ToDecrypt() + ",";

                        count++;
                        if (count == 80)
                        {
                            count = 0;
                            var paramsData = new pd_PDActionUpdateStatusOnly_spParams()
                            {
                                PDActionIDList = updateIdList.Remove(updateIdList.LastIndexOf(","), 1),
                                ActionStatusCodeID = viewModel.ActionStatus,
                                UserID = UserManager.UserExtended.UserID,
                                ActionStatusDate = DateTime.Now.ToShortDateString(),
                                BatchLogJobID = Guid.NewGuid()
                            };
                            UtilityService.ExecStoredProcedureWithResults<object>("pd_PDActionUpdateStatusOnly_sp", paramsData).ToList();
                            updateIdList = string.Empty;
                        }
                    }
                    var pd_PDActionUpdateStatusOnly_spParams = new pd_PDActionUpdateStatusOnly_spParams()
                    {
                        PDActionIDList = updateIdList.Remove(updateIdList.LastIndexOf(","), 1),
                        ActionStatusCodeID = viewModel.ActionStatus,
                        UserID = UserManager.UserExtended.UserID,
                        ActionStatusDate = DateTime.Now.ToShortDateString(),
                        BatchLogJobID = Guid.NewGuid()
                    };
                    UtilityService.ExecStoredProcedureWithResults<object>("pd_PDActionUpdateStatusOnly_sp", pd_PDActionUpdateStatusOnly_spParams).ToList();

                }
            }
            if (viewModel.DeleteIDList != "," && !string.IsNullOrEmpty(viewModel.DeleteIDList))
            {
                viewModel.DeleteIDList = viewModel.DeleteIDList.Remove(viewModel.DeleteIDList.IndexOf(","), 1);
                viewModel.DeleteIDList = viewModel.DeleteIDList.Remove(viewModel.DeleteIDList.LastIndexOf(","), 1);
                string[] Idlist = viewModel.DeleteIDList.Split(',');
                if (Idlist != null && Idlist.Count() > 0)
                {
                    foreach (var id in Idlist)
                    {
                        var pd_PDActionDelete_spParams = new pd_PDActionDelete_spParams()
                        {
                            ID = id.ToDecrypt().ToInt(),
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        };
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_PDActionDelete_sp", pd_PDActionDelete_spParams).FirstOrDefault();
                    }
                }

            }
            var pd_PDActionSearchGet_spParams = new pd_PDActionSearchGet_spParams()
            {
                ActionStatusCodeID = viewModel.ActionStatusCodeID,
                ActionTypeCodeID = viewModel.ActionTypeCodeID,
                ReminderStartDate = (viewModel.DateType == 1) ? viewModel.StartDate : null,
                ReminderEndDate = (viewModel.DateType == 1) ? viewModel.EndDate : null,
                DueStartDate = (viewModel.DateType == 2) ? viewModel.StartDate : null,
                DueEndDate = (viewModel.DateType == 2) ? viewModel.EndDate : null,
                CaseID = null,
                AssignedToPersonID = UserManager.UserExtended.PersonID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };

            var dataList = UtilityService.ExecStoredProcedureWithResults<pd_PDActionSearchGet_spResult>("pd_PDActionSearchGet_sp", pd_PDActionSearchGet_spParams).ToList();
            var list = dataList.Select(x => new
            {
                ActionType = x.ActionType,
                Action = x.ActionNote,
                Jcats = x.CaseNumber ?? string.Empty,
                CaseName = x.CaseName,
                ReminderDate = x.ActionReminderDate,
                DueDate = x.ActionDueDate,
                PDActionEcryptedID = x.PDActionID.ToEncrypt(),
                PDActionID = x.PDActionID,
                ActionStatusCodeID = x.ActionStatusCodeID,
                EncryptedCaseID = x.CaseID.ToEncrypt(),
                CaseFileDisplay = x.CaseFileDisplay,
                CaseFilePath = x.CaseFilePath
            }).ToList();

            var total = list.Count > 0 ? list.Count : 0;
            return Json(new DataTablesResponse(0, list, total, total), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult PrintToDoList(ToDoListViewModel viewModel)
        {
            var rpt_ToDoListPrintableVersion_spParams = new rpt_ToDoListPrintableVersion_spParams()
            {
                ActionStatusCodeID = viewModel.ActionStatusCodeID,
                StartDate = (viewModel.DateType == 1) ? viewModel.StartDate : null,
                EndDate = (viewModel.DateType == 1) ? viewModel.EndDate : null,
                DueStartDate = (viewModel.DateType == 2) ? viewModel.StartDate : null,
                DueEndDate = (viewModel.DateType == 2) ? viewModel.EndDate : null,
                AssignedToPersonID = UserManager.UserExtended.PersonID,
                ActionTypeCodeID = viewModel.ActionTypeCodeID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            ReportClass rpt = new ReportClass();
            rpt.FileName = HttpContext.Server.MapPath("~/Reports/rptToDoPrintableVersion.rpt");
            rpt.Load();
            var table = UtilityService.ExecStoredProcedureForDataTable("rpt_ToDoListPrintableVersion_sp", rpt_ToDoListPrintableVersion_spParams);
            rpt.SetDataSource(table);

            string filename = "ToDoList_" + UserManager.UserExtended.UserID.ToEncrypt() + ".pdf";
            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + filename);

            rpt.Close();
            rpt.Dispose();


            return Download(filename);
        }

        public virtual ActionResult ToDoAddEdit(string id)
        {
            var viewModel = new ToDoAddEditViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                var action = UtilityService.Context.pd_PDActionGet_sp(id.ToDecrypt().ToInt(), UserManager.UserExtended.UserID, Guid.NewGuid()).FirstOrDefault();
                viewModel.InjectFrom(action);

                viewModel.PDActionID = id.ToDecrypt().ToInt();

                if (viewModel.CaseID != null)
                {
                    //Update CaseStatusBar with related case and strip Client/PDAPDNumber for display in place of client dropdown.
                    UserManager.UpdateCaseStatusBar((int)viewModel.CaseID);
                    //viewModel.ClientWithCaseNumber = String.Concat(UserManager.UserExtended.Client.Split(' ').Last(), ", ", UserManager.UserExtended.Client.Split(' ').First(), "(", UserManager.UserExtended.PDAPDNumber, ")");
                    var caseInfo = new CodeViewModel();
                    caseInfo.CodeID = viewModel.CaseID.Value;
                    caseInfo.CodeValue = "Current Case";
                    viewModel.CaseList.Insert(0, caseInfo);
                }
            }

            if (viewModel.CaseID == null)
            {
                //PDAction does not have a case, load clients to assign
                var pd_PDActionGetAllCasesByUserID_spParams = new pd_PDActionGetAllCasesByUserID_spParams()
                {

                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };

                viewModel.AssignedToPersonID = UserManager.UserExtended.PersonID;

                viewModel.CaseList = UtilityService.ExecStoredProcedureWithResults<pd_PDActionGetAllCasesByUserID_spResults>("pd_PDActionGetAllCasesByUserID_sp", pd_PDActionGetAllCasesByUserID_spParams).Select(x => new CodeViewModel
                {
                    CodeID = x.caseid.Value,
                    CodeValue = x.FullName + "(" + x.casenumber + ")"
                }).ToList();
                if (UserManager.UserExtended.CaseID != 0)
                {
                    var caseInfo = new CodeViewModel();
                    caseInfo.CodeID = UserManager.UserExtended.CaseID;
                    caseInfo.CodeValue = "Current Case";
                    viewModel.CaseList.Insert(0, caseInfo);
                }

            }

            var pd_CodeGetBySystemValueTypeID_spParams = new pd_CodeGetBySystemValueTypeID_spParams()
            {
                SystemValueIDList = "150",
                IncludeCodeID = viewModel.ActionTypeCodeID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.ActionTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetBySystemValueTypeID_spResults>("pd_CodeGetBySystemValueTypeID_sp", pd_CodeGetBySystemValueTypeID_spParams).Select(c => (CodeViewModel)(new CodeViewModel()).InjectFrom(c)).ToList();

            var pd_StaffGetAllByUserID_spParams = new pd_StaffGetAllByUserID_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            viewModel.AssignedToPersonList = UtilityService.ExecStoredProcedureWithResults<pd_StaffGetAllByUserID_spResults>("pd_StaffGetAllByUserID_sp", pd_StaffGetAllByUserID_spParams).Select(x => new PersonViewModel()
            {
                PersonID = x.PersonID,
                PersonNameDisplay = x.FullName
            }).ToList();
            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult ToDoAddEdit(ToDoAddEditViewModel viewModel)
        {

            if (viewModel.PDActionID != 0)
            {
                var pd_PDActionUpdate_spParams = new pd_PDActionUpdate_spParams()
                {
                    PDActionID = viewModel.PDActionID,
                    AgencyID = viewModel.AgencyID,
                    BranchID = viewModel.BranchID,
                    CaseID = viewModel.CaseID,
                    AssignedToPersonID = viewModel.AssignedToPersonID,
                    ActionTypeCodeID = viewModel.ActionTypeCodeID,
                    ActionNote = viewModel.ActionNote,
                    ActionStatusCodeID = viewModel.ActionStatusCodeID,
                    ActionStatusDate = viewModel.ActionStatusDate,
                    ActionDueDate = viewModel.ActionDueDate,
                    ActionReminderDate = viewModel.ActionReminderDate,
                    ActionAssociatedToEntityID = viewModel.ActionAssociatedToEntityID,
                    ActionAssociatedToEntityTypeCodeID = viewModel.ActionAssociatedToEntityTypeCodeID,
                    PersonID = viewModel.PersonID,
                    RecordStateID = (int)viewModel.RecordStateID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithoutResults("pd_PDActionUpdate_sp", pd_PDActionUpdate_spParams);
            }
            else
            {
                var pd_PDActionInsert_spParams = new pd_PDActionInsert_spParams()
                {
                    AgencyID = UserManager.UserExtended.AgencyID,
                    BranchID = UserManager.UserExtended.BranchID,
                    CaseID = viewModel.CaseID,
                    AssignedToPersonID = viewModel.AssignedToPersonID,
                    ActionTypeCodeID = viewModel.ActionTypeCodeID,
                    ActionNote = viewModel.ActionNote,
                    ActionStatusCodeID = 3382,
                    ActionStatusDate = DateTime.Now.ToString(),
                    ActionDueDate = viewModel.ActionDueDate,
                    ActionReminderDate = viewModel.ActionReminderDate,
                    ActionAssociatedToEntityID = viewModel.ActionAssociatedToEntityID,
                    ActionAssociatedToEntityTypeCodeID = viewModel.ActionAssociatedToEntityTypeCodeID,

                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_PDActionInsert_sp", pd_PDActionInsert_spParams).FirstOrDefault();
            }
            TempData["TODOSaved"] = true;
            return Json(new { isSuccess = true, URL = Url.Action(MVC.Inquiry.ActionNames.ToDoList, MVC.Inquiry.Name) });
        }
        [HttpPost]
        public virtual ActionResult DownloadToDoFile(string name, string path)
        {

            var rootPath = System.Web.Configuration.WebConfigurationManager.AppSettings["FileUploadRootPath"];
            var fileSavePath = rootPath + path;

            if (!System.IO.File.Exists(fileSavePath))
            {

                Bugsnag.AspNet.Client.Current.Notify(new Exception(string.Format("File not found File URL :{0} CaseID is {1}", fileSavePath, UserManager.UserExtended.CaseID)));

                TempData["ErrorMessage"] = "File doesn't exist";
                if (Request.QueryString["quickcalendar"] != null)
                {
                    return RedirectToAction("QuickCalMyCalendar", "Task", new { HearingDate = Request.QueryString["HearingDate"] });
                }
                else
                    return RedirectToAction("ToDoList", "Inquiry");
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(fileSavePath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(fileSavePath));

        }
    }
}