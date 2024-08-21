using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Domain.Services;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Domain.pd_Note;
using CrystalDecisions.CrystalReports.Engine;
using LALoDep.Domain.rpt_Print;
using System.IO;
using LALoDep.Models.Case;
using LALoDep.Domain.pd_Case;

namespace LALoDep.Controllers.Note
{
    [AuthenticationAuthorize]
    public partial class NoteController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public NoteController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

        // GET: Note
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.NotesPage, PageSecurityItemID = SecurityToken.ViewNote)]
        public virtual ActionResult Index()
        {
            var pd_NoteGetByCaseID_spParams = new pd_NoteGetByCaseID_spParams
            {
                CaseID = UserManager.UserExtended.CaseID,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            var noteList = UtilityService.ExecStoredProcedureWithResults<pd_NoteGetByCaseID_spResult>("pd_NoteGetByCaseID_sp", pd_NoteGetByCaseID_spParams).Select(x => new pd_NoteGetByCaseID_spResult()
            {
                NoteDate = x.NoteDate,
                PersonNameFirst = x.PersonNameFirst,
                PersonNameLast = x.PersonNameLast,
                NoteTypeCodeValue = x.NoteTypeCodeValue,
                NoteSubject = x.NoteSubject,
                NoteEntry = x.NoteEntry,
                NoteID = x.NoteID,
                AllowDeleteFlag = x.AllowDeleteFlag,
                NoteLinkFlag = x.NoteLinkFlag,
                IsRTF=x.IsRTF
            }).ToList();
            return View(noteList);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.NotesPage)]
        public virtual ActionResult NoteAddEdit(string id)
        {

            if (UserManager.UserExtended.CaseID > 0)
            {
                var viewModel = new NoteAddEditViewModel();
                viewModel.CanAddNote = UserManager.IsUserAccessTo(SecurityToken.AddNote);


                if (!string.IsNullOrEmpty(id))
                {
                    //in Edit Mode
                    //get NoteInfo
                    var pd_NoteGet_spParams = new pd_NoteGet_spParams()
                    {
                        NoteID = id.ToDecrypt().ToInt(),
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    };
                    var noteInfo = UtilityService.ExecStoredProcedureWithResults<pd_NoteGet_spResult>("pd_NoteGet_sp", pd_NoteGet_spParams).FirstOrDefault();
                    if (noteInfo != null)
                    {
                        viewModel.NoteID = noteInfo.NoteID;
                        viewModel.AgencyID = noteInfo.AgencyID;
                        viewModel.NoteEntityCodeID = noteInfo.NoteEntityCodeID;
                        viewModel.NoteEntityTypeCodeID = noteInfo.NoteEntityTypeCodeID;
                        viewModel.EntityPrimaryKeyID = noteInfo.EntityPrimaryKeyID;
                        viewModel.NoteTypeCodeID = noteInfo.NoteTypeCodeID;
                        viewModel.NoteSubject = noteInfo.NoteSubject;
                        viewModel.NoteEntry = noteInfo.NoteEntry;
                        viewModel.CaseID = noteInfo.CaseID;
                        viewModel.PetitionID = noteInfo.PetitionID;
                        viewModel.HearingID = noteInfo.HearingID;
                        viewModel.RecordStateID = noteInfo.RecordStateID;
                        viewModel.HideBroadcastNotesFlag = noteInfo.HideBroadcastNotesFlag;
                        viewModel.HideClientsAttachedFlag = noteInfo.HideClientsAttachedFlag;
                        viewModel.HideNoteTypeFlag = noteInfo.HideNoteTypeFlag;
                        viewModel.CanEditSubjectFlag = noteInfo.CanEditSubjectFlag;
                    }

                    viewModel.NoteTypeList = UtilityFunctions.CodeGetWithSortValByTypeIdAndUserId(16, includeCodeId: viewModel.NoteTypeCodeID ?? 0, agencyId: UserManager.UserExtended.CaseNumberAgencyID)
                                                                    .Select(x => new NoteTypeList { NoteTypeDisplay = x.CodeValue, CodeID = x.CodeID }).ToList();
                }
                else //new Note Type
                {
                    var pd_CodeGetNewNoteTypeByCaseID_spParams = new LALoDep.Domain.pd_Note.pd_CodeGetNewNoteTypeByCaseID_spParams
                    {
                        CaseID = UserManager.UserExtended.CaseID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        CaseAgencyID = UserManager.UserExtended.CaseNumberAgencyID
                    };
                    viewModel.NoteTypeList = UtilityService.ExecStoredProcedureWithResults<NoteTypeList>("pd_CodeGetNewNoteTypeByCaseID_sp", pd_CodeGetNewNoteTypeByCaseID_spParams).Select(x => new NoteTypeList()
                    {
                        NoteTypeDisplay = x.NoteTypeDisplay,
                        CodeID = x.CodeID
                    }).ToList();

                    viewModel.HideBroadcastNotesFlag = 0;
                    viewModel.HideClientsAttachedFlag = 0;
                    viewModel.HideNoteTypeFlag = 0;
                    viewModel.CanEditSubjectFlag = 1;
                }

                //panelList
                var pd_NotePanelGetAllByNoteID_spParams = new pd_NotePanelGetAllByNoteID_spParams
                {
                    NoteID = (!string.IsNullOrEmpty(id)) ? id.ToDecrypt().ToInt() : 0,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                viewModel.PanelList = UtilityService.ExecStoredProcedureWithResults<PanelList>("pd_NotePanelGetAllByNoteID_sp", pd_NotePanelGetAllByNoteID_spParams).Select(x => new PanelList()
                {
                    Type = x.Type,
                    Selected = x.Selected,
                    CodeID = x.CodeID,
                    NotePanelKey = x.NotePanelKey
                }).ToList();
                viewModel.NotePersonList = UtilityService.ExecStoredProcedureWithResults<NotePersonGetAll_spResult>("NotePersonGetAll_sp", new NotePersonGetAll_spParams
                {
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    NoteID = (!string.IsNullOrEmpty(id)) ? id.ToDecrypt().ToInt() : 0,
                    CaseID = UserManager.UserExtended.CaseID

                }).ToList();

                var controlType = UtilityService.ExecStoredProcedureWithResults<NoteControlTypeGet_spResult>("NoteControlTypeGet_sp", new NoteControlTypeGet_spParams()
                {
                    NoteID = viewModel.NoteID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    CaseID = UserManager.UserExtended.CaseID,
                    AgencyID = UserManager.UserExtended.CaseNumberAgencyID

                }).FirstOrDefault();
                if (controlType != null)
                {
                    viewModel.ControlType = controlType.ControlType;
                }
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("AccessDenied", "Home");
            }
        }


        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.NotesPage)]
        [HttpPost]
        public virtual JsonResult NoteAddEditSave(NoteAddEditViewModel viewModel)
        {
            if (viewModel.NoteID != null)
            {
                //update
                var pd_NoteUpdate_spParams = new del_NoteUpdate_spParams()
                {
                    NoteID = viewModel.NoteID,
                    NoteEntityCodeID = viewModel.NoteEntityCodeID,
                    NoteEntityTypeCodeID = viewModel.NoteEntityTypeCodeID,
                    EntityPrimaryKeyID = viewModel.EntityPrimaryKeyID.Value,
                    NoteTypeCodeID = viewModel.NoteTypeCodeID.Value,
                    NoteSubject = viewModel.NoteSubject,
                    NoteEntry = viewModel.NoteEntry,
                    CaseID = viewModel.CaseID.Value,
                    RecordStateID = viewModel.RecordStateID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };
                UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteUpdate_sp", pd_NoteUpdate_spParams).FirstOrDefault();
            }
            else
            {
                var pd_NoteInsert_spParams = new pd_NoteInsert_spParams()
                {
                    NoteEntitySystemValueTypeID = 112,
                    NoteEntityTypeSystemValueTypeID = 123,
                    EntityPrimaryKeyID = UserManager.UserExtended.CaseID,
                    NoteTypeCodeID = viewModel.NoteTypeCodeID.Value,
                    NoteSubject = viewModel.NoteSubject,
                    NoteEntry = viewModel.NoteEntry,
                    CaseID = UserManager.UserExtended.CaseID,
                    RecordStateID = 1,
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID
                };
                viewModel.NoteID = UtilityService.ExecStoredProcedureWithResults<int>("pd_NoteInsert_sp", pd_NoteInsert_spParams).SingleOrDefault();
            }
            if (viewModel.PanelList != null)
            {
                foreach (var item in viewModel.PanelList)
                {
                    if (item.Selected == 0 && item.IsCurrentSelected == 1)
                    {
                        //insert
                        UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_NotePanelInsert_sp", new pd_NotePanelInsert_spParams()
                        {
                            AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                            NoteID = (int)viewModel.NoteID,
                            NotePanelCodeID = item.CodeID,
                            RecordStateID = 1,
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID
                        }).FirstOrDefault();
                    }
                    else if (item.Selected == 1 && item.IsCurrentSelected == 0)
                    {
                        //delete
                        var pd_NotePanelDelete_spParams = new pd_NotePanelDelete_spParams()
                        {
                            ID = item.NotePanelKey.Value,
                            LoadOption = "NotePanel",
                            RecordStateID = 10,
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID
                        };
                        var deletedData = UtilityService.ExecStoredProcedureWithResults<dynamic>("pd_NotePanelDelete_sp", pd_NotePanelDelete_spParams).ToList();
                    }
                }

            }
            if (viewModel.NoteClientListForAddEdit != null)
            {
                foreach (var item in viewModel.NoteClientListForAddEdit)
                {
                    var iud = "";
                    if (item.Selected == 0 && item.NotePersonID > 0)
                    {
                        iud = "DELETE";

                    }
                    else if (item.Selected == 1 && item.NotePersonID == 0)
                    {
                        iud = "INSERT";

                    }
                    if (iud != "")
                    {
                        var oParams = new qcal_AS_NotePersonIUD_spParams()
                        {
                            IUD = iud,
                            NoteID = (int)viewModel.NoteID,
                            PersonID = item.PersonID,
                            NotePersonID = item.NotePersonID > 0 ? item.NotePersonID : 0,
                            BatchLogJobID = Guid.NewGuid(),
                            UserID = UserManager.UserExtended.UserID
                        };
                        var deletedData = UtilityService.ExecStoredProcedureWithResults<dynamic>("qcal_AS_NotePersonIUD_sp", oParams).ToList();

                    }

                }

            }
            return Json(new { isSuccess = true });
        }

        [HttpPost]
        public virtual ActionResult PrintNotes(string id)
        {
            var rpt_NotesPrintableVersion_spParams = new rpt_NotesPrintableVersion_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,
                NoteIDList = id.TrimEnd(',').ToString(),
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            ReportClass rpt = new ReportClass();
            string filename = "NotesPrintableVersion_" + UserManager.UserExtended.CaseID.ToEncrypt() + ".pdf";
            try
            {
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/rptNotesPrintableVersion.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("rpt_NotesPrintableVersion_sp", rpt_NotesPrintableVersion_spParams);
                rpt.SetDataSource(table);

                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + filename);

            }
            catch (Exception ex)
            {
            }
            finally
            {
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
            }

            return Download(filename);

        }
        [HttpPost]
        public virtual ActionResult PrintNotesWithoutNoteList()
        {
            var rpt_NotesPrintableVersion_spParams = new rpt_NotesPrintableVersion_spParams()
            {
                CaseID = UserManager.UserExtended.CaseID,

                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            };
            ReportClass rpt = new ReportClass();
            string filename = "NotesPrintableVersion_" + UserManager.UserExtended.CaseID.ToEncrypt() + ".pdf";
            try
            {
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/rptNotesPrintableVersion.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("rpt_NotesPrintableVersion_sp", rpt_NotesPrintableVersion_spParams);
                rpt.SetDataSource(table);

                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + filename);

            }
            catch (Exception ex)
            {
            }
            finally
            {
                rpt.Close();
                rpt.Dispose();
                GC.Collect();
            }

            return Download(filename);

        }
        public virtual ActionResult Download(string file)
        {
            string fullPath = UtilityFunctions.GetDocumentDownloadFolderPath() + file;


            if (UserManager.UserExtended.PrintDocumentOn == "NewWindow" && Path.GetExtension(fullPath).Contains("pdf"))
            {
                Response.AppendHeader("Content-Disposition", "inline; filename=" + file);
                return File(fullPath, "application/pdf");
            }
            if (System.IO.File.Exists(fullPath))
            {

                return File(fullPath, "application/force-download", file);
            }
            else
            {
                return Content("File not found");
            }

        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.NotesPage, PageSecurityItemID = SecurityToken.DeleteNote)]
        public virtual JsonResult NoteDelete(string id)
        {
            var pd_NoteDelete_spParams = new pd_NotePanelDelete_spParams()
            {
                ID = id.ToDecrypt().ToInt(),
                LoadOption = "Note",
                RecordStateID = 10,
                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID
            };
            var deletedData = UtilityService.ExecStoredProcedureWithResults<object>("pd_NoteDelete_sp", pd_NoteDelete_spParams).ToList();
            return Json(new { isSuccess = true });
        }

        [ChildActionOnly]
        public virtual PartialViewResult Render()
        {
            var parentRouteData = ControllerContext.ParentActionViewContext.RouteData;
            var actionRoute = string.Format("{0}/{1}", parentRouteData.GetRequiredString("controller"), parentRouteData.GetRequiredString("action"));
            ViewBag.ActionRoute = actionRoute;
            var data = new List<NG_pd_NoteGetByCaseIDASPPageName_spResult>();
            if (UserManager.UserExtended.CaseID > 0)
            {
                data = UtilityService.ExecStoredProcedureWithResults<NG_pd_NoteGetByCaseIDASPPageName_spResult>("NG_pd_NoteGetByCaseIDASPPageName_sp", new NG_pd_NoteGetByCaseIDASPPageName_spParams
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    ASPPageName = actionRoute,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                }).ToList();
            }

            return PartialView("_PageNote", data);
        }

        [HttpPost]
        public virtual PartialViewResult PageNoteDetail(string actionRoute)
        {
            var data = new List<NG_pd_NoteGetByCaseIDASPPageName_spResult>();
            if (UserManager.UserExtended.CaseID > 0)
            {
                data = UtilityService.ExecStoredProcedureWithResults<NG_pd_NoteGetByCaseIDASPPageName_spResult>("NG_pd_NoteGetByCaseIDASPPageName_sp", new NG_pd_NoteGetByCaseIDASPPageName_spParams
                {
                    CaseID = UserManager.UserExtended.CaseID,
                    ASPPageName = actionRoute,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                }).ToList();
            }

            ViewBag.ActionRoute = actionRoute;
            return PartialView("_PageNoteDetail", data);
        }
    }
}