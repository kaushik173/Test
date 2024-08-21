using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_CodeTables;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using LALoDep.Domain.pd_TrainingSummary;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Training;

namespace LALoDep.Controllers.Administration
{
    public partial class AdministrationController : Controller
    {

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Training, PageSecurityItemID = SecurityToken.ViewMyTraining)]
        public virtual ActionResult MyTraining(string id)
        {
            var viewModel = new MyTrainingViewModel();
            if (!id.IsNullOrEmpty())
            {
                viewModel.PersonID = id.ToDecrypt().ToInt();
                var personInfo = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", new pd_PersonGet_spParams()
                {
                    PersonID = viewModel.PersonID.Value,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                }).FirstOrDefault();
                if (personInfo != null)
                    viewModel.PersonName = personInfo.FirstName + " " + personInfo.LastName;
            }
            else
            {
                viewModel.PersonID = UserManager.UserExtended.PersonID;
                viewModel.PersonName = UserManager.UserExtended.FullName;
            }
            viewModel.VenueList = UtilityService.ExecStoredProcedureWithResults<pd_TrainingGetVenueList_spResult>(
                          "pd_TrainingGetVenueList_sp", new pd_TrainingGetVenueList_spParams
                          {

                              BatchLogJobID = Guid.NewGuid(),
                              UserID = UserManager.UserExtended.UserID

                          }).Select(o => new SelectListItem() { Text = o.CodeDisplay, Value = o.CodeID.ToString() }).ToList();

            return View(viewModel);
        }

        public virtual JsonResult GetTrainingSummaryData(TrainingViewModel parameters)
        {

            var viewModel = UtilityService.ExecStoredProcedureWithResults<pd_TrainingGetSummaryByPersonIDStartDateEndDate_spResult>(
                "pd_TrainingGetSummaryByPersonIDStartDateEndDate_sp",
                new pd_TrainingGetSummaryByPersonIDStartDateEndDate_spParams()
                {
                    PersonID = parameters.PersonID,
                    StartDate = parameters.StartDate,
                    EndDate = parameters.StartDate,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = new Guid(),
                    TrainingVenueCodeID = parameters.VenueID,

                }).Select(o => new
                {

                    Total = o.Total.ToString("N2"),
                    NP = o.NP.ToString("N2"),
                    P = o.P.ToString("N2"),
                    o.CreditType

                }).ToList();
            return Json(new DataTablesResponse(0, viewModel, viewModel.Count, viewModel.Count));
        }

        public virtual JsonResult GetTrainingData(TrainingViewModel parameters)
        {
            var viewModel = UtilityService
                .ExecStoredProcedureWithResults<pd_TrainingGetByPersonIDStartDateEndDate_spResult>(
                    "pd_TrainingGetByPersonIDStartDateEndDate_sp",
                    new pd_TrainingGetByPersonIDStartDateEndDate_spParams()
                    {
                        PersonID = parameters.PersonID,
                        StartDate = parameters.StartDate,
                        EndDate = parameters.EndDate,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = new Guid(),
                        TrainingVenueCodeID = parameters.VenueID
                    }).Select(x => new
                    {
                        TrainingID = x.TrainingID,
                        CourseTitle = x.TrainingCourseTitle,
                        StartDate = (x.TrainingCompletionDate.HasValue) ? x.TrainingCompletionDate.Value.ToShortDateString() : string.Empty,
                        EndDate = (x.TrainingCompletionDateTwo.HasValue) ? x.TrainingCompletionDateTwo.Value.ToShortDateString() : string.Empty,
                        Hours = x.TrainingHours.ToString(),
                        EncryptedTrainingID = x.TrainingID.ToEncrypt(),
                        EncryptedPersonID = x.PersonID.ToEncrypt(),
                        x.CreditType,
                        x.Venue
                    }).ToList();
            return Json(new DataTablesResponse(0, viewModel, viewModel.Count, viewModel.Count));
        }

        [HttpPost]
        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.Training, PageSecurityItemID = SecurityToken.DeleteTrainig)]
        public virtual JsonResult TrainingDelete(string id)
        {
            UtilityService.ExecStoredProcedureWithResults<object>("pd_TrainingDelete_sp", new pd_TrainingDelete_spParams
            {
                ID = id.ToDecrypt().ToInt(),
                UserID = UserManager.UserExtended.UserID,
                LoadOption = "Training",
                BatchLogJobID = new Guid(),
                RecordStateID = 10
            }).ToList();
            return Json(new { isSuccess = true });
        }
    }
}