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
using LALoDep.Domain.CSEC;

namespace LALoDep.Controllers
{
    public partial class TaskController
    {
        // [ClaimsAuthorize(IsCasePage = false, CustomSecurityItemIds = PageLevelSecurityItemIds.MyARQueuePage, PageSecurityItemID = SecurityToken.ViewActionRequest)]
        public virtual ActionResult CSECSummary()
        {

            var viewModel = new CSECSummaryViewModel();


            viewModel.StaffTypeList = UtilityService.ExecStoredProcedureWithResults<CSECGetStaffType_spResult>("CSECGetStaffType_sp", new CSECGetStaffType_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            }).Select(o => new SelectListItem() { Value = o.CodeID.ToString(), Text = o.CodeDisplay }).ToList();

            var agencies = UtilityService.ExecStoredProcedureWithResults<CSECGetAgency_spResult>("CSECGetAgency_sp", new CSECGetStaffType_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            });
            var agency = agencies.FirstOrDefault(o => o.HomeAgencyFlag == 1);
            if (agency != null)
            {
                viewModel.AgencyID = agency.AgencyID;
            }

            viewModel.AgencyList = agencies.Select(o => new SelectListItem() { Value = o.AgencyID.ToString(), Text = o.AgencyDisplay }).ToList();

            //#450: CSE-IT (CSEC) Summary page: default Completed Date Range to last 30 days 
            viewModel.CompletedStartDate = DateTime.Now.AddDays(-30).ToString("MM/dd/yyyy");
            viewModel.CompletedEndDate = DateTime.Now.ToString("MM/dd/yyyy");

            return View(viewModel);
        }


        [HttpPost]
        public virtual ActionResult CSECSummary(CSECSummaryViewModel model)
        {
            var parms = new CSECGetSummary_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                AgencyID = model.AgencyID,
                CSECAssignedToRoleTypeCodeID = model.StaffTypeID,
                StartDate = model.CompletedStartDate.ToDateTimeNullableValue(),
                EndDate = model.CompletedEndDate.ToDateTimeNullableValue(),
                DateRangeType = "COMPLETED"
            };

            var data = UtilityService.ExecStoredProcedureWithResults<CSECGetSummary_spResult>("CSECGetSummary_sp", parms).ToList();
            return PartialView("~/Views/Task/_partialCSECSummary.cshtml", data);
        }


        public virtual ActionResult MyCSECQueue(string id)
        {

            var viewModel = new MyCSECQueueViewModel();
            viewModel.EncryptedPersonID = id;
            viewModel.PageTitle = "My CSE-IT Queue";
            viewModel.PersonID = id.IsNullOrEmpty() ? UserManager.UserExtended.PersonID : id.ToDecrypt().ToInt();
            if (!id.IsNullOrEmpty())
            {
                var personGet = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp", new pd_PersonGet_spParams()
                {
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid(),
                    PersonID = viewModel.PersonID
                }).FirstOrDefault();
                if (personGet != null)
                {

                    viewModel.PageTitle = "My CSE-IT Queue For<br/>" + personGet.LastName + "," + personGet.FirstName;
                }

            }


            viewModel.DateRangeTypeList = new List<SelectListItem>() { new SelectListItem() { Value = "Due", Text = "Due" }, new SelectListItem() { Value = "Completed", Text = "Completed" } };

            return View(viewModel);
        }


        [HttpPost]
        public virtual ActionResult MyCSECQueue(MyCSECQueueViewModel model)
        {
            if (!model.IncompleteList.IsNullOrEmpty()){
                var list = model.IncompleteList.Split(',').ToList();
                foreach(var csid in list)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("CSECUpdateStatus_sp", new CSECUpdateStatus_spParams {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        CSECID= csid.ToInt(),
                        UpdateMode= "INCOMPLETE"
                    });

                    
                }
            }else if (!model.RestoreList.IsNullOrEmpty())
            {
                var list = model.RestoreList.Split(',').ToList();
                foreach (var csid in list)
                {
                    UtilityService.ExecStoredProcedureWithoutResultADO("CSECUpdateStatus_sp", new CSECUpdateStatus_spParams
                    {
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                        CSECID = csid.ToInt(),
                        UpdateMode = "RESTORE"
                    });


                }
            }



            var parms = new CSECGetMyQueue_spParams()
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),

                StartDate = model.StartDate.ToDateTimeNullableValue(),
                EndDate = model.EndDate.ToDateTimeNullableValue(),
                DateRangeType = model.DateRangeType,
                CSECAssignedToPersonID = model.PersonID,
                IncludeCompleted = model.IncludeCompleted ? 1 : 0,
                ShowIncompletes=model.ShowCompletes ? 1 : 0,
            };

            var data = UtilityService.ExecStoredProcedureWithResults<CSECGetMyQueue_spResult>("CSECGetMyQueue_sp", parms).ToList();

            var total = data.Count;
            var searchList = data.Select(c => new
            {
                c.CanEditFlag,
                c.Child,
                c.CompletionDate,
                c.CSECID,
                c.DueDate,
                c.ScoreLiteral,
                c.ScoreNumeric,
                c.SortBy,
                EncryptedCSECID = c.CSECID.ToEncrypt(),
                c.CanMarkAsIncompleteFlag,
                c.CanRestoreFlag,c.StatusDate,c.StatusBy

            }).ToList();
            if (model.ShowCompletes  )
            {
                searchList = searchList.OrderByDescending(o => o.StatusDate).ToList();

            }
            return Json(new { Status = "Done", SearchData = new DataTablesResponse(0, searchList, total, total) });

        }


    }
}