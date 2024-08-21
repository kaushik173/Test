using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.Agency;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_TrainingSummary;
using LALoDep.Domain.Services;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using LALoDep.Core.Custom.Extensions;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using CrystalDecisions.Shared;
using LALoDep.Domain.pd_Training;

namespace LALoDep.Controllers.Administration
{
    public partial class TrainingSummaryController : Controller
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public TrainingSummaryController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.TrainingSummary, PageSecurityItemID = SecurityToken.ViewTrainingSummary)]
        public virtual ActionResult Search()
        {
            var model = new TrainingSummaryViewModel() { OnViewLoad = true };
            model.VenueList = UtilityService.ExecStoredProcedureWithResults<pd_TrainingGetVenueList_spResult>(
             "pd_TrainingGetVenueList_sp", new pd_TrainingGetVenueList_spParams
             {
                 LoadOption = "SUMMARY",
                 BatchLogJobID = Guid.NewGuid(),
                 UserID = UserManager.UserExtended.UserID

             }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();
            model.SupervisorList = UtilityService.ExecStoredProcedureWithResults<pd_TrainingGetSupervisorList_spResult>(
            "pd_TrainingGetSupervisorList_sp", new pd_TrainingGetSupervisorList_spParams
            {

                BatchLogJobID = Guid.NewGuid(),
                UserID = UserManager.UserExtended.UserID

            }).Select(o => new SelectListItem() { Text = o.PersonDisplay, Value = o.PersonID.ToString() }).ToList();
            model.AgencyList = UtilityService.ExecStoredProcedureWithResults<AgencyGetByUserID_spResult>(
                "AgencyGetByUserID_sp", new AgencyGetByUserID_spParams
                {
                    SortOption = "AgencyName",
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID

                }).Select(o => new SelectListItem() { Text = o.AgencyName, Value = o.AgencyID.ToString() }).ToList();
            if (model.AgencyList.Count() == 1)
            {
                model.AgencyID = model.AgencyList.First().Value.ToInt();
            }
            model.AgencyGroupList = UtilityService.ExecStoredProcedureWithResults<AgencyGetGroupByUserID_spResults>(
                "AgencyGetGroupByUserID_sp", new AgencyGetGroupByUserID_spParams
                {
                    SortOption = "AgencyGroup",
                    BatchLogJobID = Guid.NewGuid(),
                    UserID = UserManager.UserExtended.UserID

                }).Select(o => new SelectListItem() { Text = o.AgencyGroup, Value = o.AgencyGroupID.ToString() }).ToList();
            if (model.AgencyGroupList.Count() == 1)
            {
                model.AgencyGroupID = model.AgencyGroupList.First().Value.ToInt();
            }
            return View(model);
        }

        [HttpPost]
        public virtual PartialViewResult Filter(TrainingSummaryViewModel model)
        {
            var searchList =
                UtilityService.ExecStoredProcedureWithResults<pd_TrainingGetSummary_spResult>(
                    "pd_TrainingGetSummary_sp",
                    new pd_TrainingGetSummary_spParams
                    {
                        StartDate = model.StartDate.ToDateTimeNullableValue(),
                        EndDate = model.EndDate.ToDateTimeNullableValue(),
                        AgencyGroupID = model.AgencyGroupID,
                        AgencyID = model.AgencyID,
                        IncludeAllActiveStaff = model.IncludeAllActiveStuffBool ? 1 : 0,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = new Guid(),
                        VenueCodeID = model.VenueID,
                        SupervisorPersonID = model.SupervisorID
                    }).ToList();

            return PartialView("_trainingSummaryPartial", searchList);
        }


        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Training)]
        public virtual ActionResult TrainingEdit(string Id, string personID)
        {
            int? TrainingID = null;

            var viewModel = new TrainingEditViewModel();
            var canEdit = UserManager.IsUserAccessTo(SecurityToken.EditTraining);
            var canAdd = UserManager.IsUserAccessTo(SecurityToken.AddTraining);
            if ((!canAdd && !canEdit))
            {
                return RedirectToAction(MVC.Home.Name, MVC.Home.ActionNames.AccessDenied);
            }

            if (!string.IsNullOrEmpty(Id))
            {
                TrainingID = Id.ToDecrypt().ToInt();
                var training = UtilityService.ExecStoredProcedureWithResults<pd_TrainingGet_spResult>(
                 "pd_TrainingGet_sp", new pd_TrainingGet_spParams()
                 {
                     TrainingID = Id.ToDecrypt().ToInt(),
                     UserID = UserManager.UserExtended.UserID,
                     BatchLogJobID = new Guid()
                 }).FirstOrDefault();
                if (training != null)
                {
                    viewModel.CourseTitle = training.TrainingCourseTitle;
                    viewModel.TrainingProvider = training.TrainingProvider;
                    viewModel.SubjectMatter = training.TrainingSubjectMatter;
                    viewModel.StartDate = training.TrainingCompletionDate;
                    viewModel.EndDate = training.TrainingCompletionDateTwo;
                    viewModel.Participatory = training.TrainingParticipatory == 1 ? true : false;
                    viewModel.Hours = training.TrainingHours;
                    viewModel.CreditTypeID = training.TrainingCreditTypeCodeID;
                    viewModel.TrainingID = training.TrainingID;
                    viewModel.AgencyID = training.AgencyID;
                    viewModel.RecordStateID = training.RecordStateID;
                    viewModel.VenueID = training.TrainingVenueCodeID;
                    viewModel.TrainingIVeEligibleCodeID = training.TrainingIVeEligibleCodeID;


                }

            }
            else
            {
                viewModel.TrainingID = 0;
            }
            viewModel.VenueList = UtilityService.ExecStoredProcedureWithResults<pd_TrainingGetVenueList_spResult>(
       "pd_TrainingGetVenueList_sp", new pd_TrainingGetVenueList_spParams
       {
           TrainingID = TrainingID,
           BatchLogJobID = Guid.NewGuid(),
           UserID = UserManager.UserExtended.UserID

       }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();
            viewModel.Person = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>(
           "pd_PersonGet_sp", new pd_PersonGet_spParams()
           {
               PersonID = personID.ToDecrypt().ToInt(),
               UserID = UserManager.UserExtended.UserID,
               BatchLogJobID = new Guid()
           }).First();

            viewModel.CreditTypeList = UtilityFunctions.CodeGetByTypeIdAndUserId(codeTypeId: 59, includeCodeId: (viewModel.CreditTypeID ?? 0));

            //viewModel.CreditTypeList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>("pd_CodeGetByTypeIDAndUserID_sp",
            //        new pd_CodeGetByTypeIDAndUserID_spParams() { CodeTypeID = 59, UserID = UserManager.UserExtended.UserID, BatchLogJobID = new Guid(), IncludeCodeID = 2470, SortOption = null, AgencyID = 0 }).Select(e => new SelectListItem()
            //        {
            //            Value = e.CodeID.ToString(),
            //            Text = e.CodeValue
            //        }).ToList();
            viewModel.TrainingIVeEligibleCodeList = UtilityService.ExecStoredProcedureWithResults<CodeGetTrainingIVeEligible_spResult>(
   "CodeGetTrainingIVeEligible_sp", new CodeGetTrainingIVeEligible_spParams
   {
       AgencyID = UserManager.UserExtended.AgencyID,

       TrainingID = TrainingID,
       BatchLogJobID = Guid.NewGuid(),
       UserID = UserManager.UserExtended.UserID

   }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();


            return View(viewModel);
        }

        [HttpPost]
        public virtual JsonResult TrainingEdit(TrainingEditViewModel updateModel)
        {
            if (!updateModel.TrainingID.HasValue || updateModel.TrainingID == 0)
            {
                //insert
                UtilityService.ExecStoredProcedureWithResults<Decimal>("pd_TrainingInsert_sp", new pd_TrainingUpdate_spParams()
                {
                    AgencyID = UserManager.UserExtended.AgencyID,
                    PersonID = updateModel.Person.PersonID,
                    TrainingCourseTitle = updateModel.CourseTitle,
                    TrainingProvider = updateModel.TrainingProvider,
                    TrainingSubjectMatter = updateModel.SubjectMatter,
                    TrainingCreditTypeCodeID = updateModel.CreditTypeID,
                    TrainingParticipatory = updateModel.Participatory ? 1 : 0,
                    TrainingHours = updateModel.Hours,
                    TrainingCompletionDate = updateModel.StartDate,
                    TrainingCompletionDateTwo = updateModel.EndDate,
                    RecordStateID = 1,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = new Guid(),
                    TrainingVenueCodeID = updateModel.VenueID,
                    TrainingIVeEligibleCodeID =  updateModel.TrainingIVeEligibleCodeID,
                }).FirstOrDefault();
            }
            else
            {
                UtilityService.ExecStoredProcedureWithoutResults("pd_TrainingUpdate_sp", new pd_TrainingUpdate_spParams()
                {
                    TrainingID = updateModel.TrainingID.Value,
                    AgencyID = updateModel.AgencyID,
                    PersonID = updateModel.Person.PersonID,
                    TrainingCourseTitle = updateModel.CourseTitle,
                    TrainingProvider = updateModel.TrainingProvider,
                    TrainingSubjectMatter = updateModel.SubjectMatter,
                    TrainingCreditTypeCodeID = updateModel.CreditTypeID,
                    TrainingParticipatory = updateModel.Participatory ? 1 : 0,
                    TrainingHours = updateModel.Hours,
                    TrainingCompletionDate = updateModel.StartDate,
                    TrainingCompletionDateTwo = updateModel.EndDate,
                    RecordStateID = updateModel.RecordStateID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = new Guid(),
                    TrainingVenueCodeID = updateModel.VenueID,
                    TrainingIVeEligibleCodeID = updateModel.TrainingIVeEligibleCodeID,
                });
            }
            return Json(new { Id = updateModel.Person.PersonID.ToEncrypt(), isSuccess = true });
        }

        [HttpPost]
        public virtual ActionResult TraniningList(TrainingViewModel viewModel)
        {
            var pd_TrainingGetSummaryByPersonIDStartDateEndDate_spParams = new pd_TrainingGetSummaryByPersonIDStartDateEndDate_spParams()
            {
                PersonID = viewModel.PersonID,
                StartDate = viewModel.StartDate,
                EndDate = viewModel.EndDate,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = new Guid()
            };

            string fileName = "TrainingSummary_" + viewModel.Person.PersonID.ToEncrypt() + ".pdf";
            ReportClass rpt = new ReportClass();
            try
            {
                rpt.FileName = HttpContext.Server.MapPath("~/Reports/rptTraining.rpt");
                rpt.Load();
                var table = UtilityService.ExecStoredProcedureForDataTable("dbo.pd_TrainingGetSummaryByPersonIDStartDateEndDate_sp", pd_TrainingGetSummaryByPersonIDStartDateEndDate_spParams);
                rpt.SetDataSource(table);

                //var subTableData = (UtilityService.ExecStoredProcedureForDataTable("dbo.pd_TrainingGetByPersonIDStartDateEndDate_sp", pd_TrainingGetSummaryByPersonIDStartDateEndDate_spParams));
                //rpt.Subreports["rptTraining.rpt"].SetDataSource(subTableData);


                rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, UtilityFunctions.GetDocumentDownloadFolderPath() + fileName);

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


            return new LALoDep.Custom.Actions.DownloadActionResult(fileName);
        }

    }
}