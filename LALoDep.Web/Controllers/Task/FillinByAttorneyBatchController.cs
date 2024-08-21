using System;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.CaseAttribute;
using LALoDep.Domain.pd_Allegation;
using LALoDep.Domain.pd_Association;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.Services;
using LALoDep.Domain.sup_Case;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Task;
using LALoDep.Domain.Agency;
using LALoDep.Core.Custom.Extensions;
using System.Collections.Generic;
using LALoDep.Custom;
using LALoDep.Domain.pd_Code;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_Hearing;
using LALoDep.Domain.pd_Address;
using System.Globalization;
using pd_CodeGetNewNoteTypeByCaseID_spParams = LALoDep.Domain.pd_Code.pd_CodeGetNewNoteTypeByCaseID_spParams;
using pd_CodeGetNewNoteTypeByCaseID_spResult = LALoDep.Domain.pd_Code.pd_CodeGetNewNoteTypeByCaseID_spResult;
using LALoDep.Models.CaseOpening;
using LALoDep.Domain.PD_PDAction;
using LALoDep.Domain.pd_Profile;
using LALoDep.Models.Case;
using LALoDep.Domain.Tasks;

namespace LALoDep.Controllers
{
    [AuthenticationAuthorize]
    public partial class TaskController
    {

        // [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseCleanupPage, PageSecurityItemID = SecurityToken.CaseCleanup_SeeAllAttorneys)]
        public virtual ActionResult FillinByAttorneyBatch()
        {
            var viewModel = new FillinByAttorneyBatchViewModel();


            viewModel.Agencies = UtilityService.ExecStoredProcedureWithResults<AgencyGetByUserID_spResult>(
                    "AgencyGetByUserID_sp", new AgencyGetByUserID_spParams
                    {

                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = new Guid(),
                    }).Select(x => new SelectListItem { Text = x.AgencyName, Value = x.AgencyID.ToString() }).ToList();

            if (viewModel.Agencies.Count() == 1)
            {
                viewModel.AgencyID = viewModel.Agencies.ToList()[0].Value.ToInt();
            }

            viewModel.AssignedAttorneyList = UtilityService.ExecStoredProcedureWithResults<pd_RoleAgencyAttorneyGet_spResult>(
                 "pd_RoleAgencyAttorneyGet_sp", new pd_RoleAgencyAttorneyGet_spParams
                 {

                     UserID = UserManager.UserExtended.UserID,
                     BatchLogJobID = new Guid(),
                 }).Select(x => new SelectListItem { Text = x.PersonNameDisplay, Value = x.PersonID.ToString() }).ToList();

            viewModel.DepartmentList = UtilityService.ExecStoredProcedureWithResults<pd_CodeGetByTypeIDAndUserID_spResults>(
                 "pd_CodeGetByTypeIDAndUserID_sp", new pd_CodeGetByTypeIDAndUserID_spParams
                 {
                     CodeTypeID = 30,
                     UserID = UserManager.UserExtended.UserID,
                     BatchLogJobID = new Guid(),
                 }).Select(x => new SelectListItem { Text = x.CodeValue, Value = x.CodeID.ToString() }).ToList();

            viewModel.StartDate = DateTime.Now.ToString("d");
            viewModel.EndDate = DateTime.Now.AddDays(15).ToString("d");
            viewModel.FillinCoverYesNo = true;

            return View(viewModel);
        }
        [HttpPost]
        public virtual JsonResult FillinByAttorneyBatch(FillinByAttorneyBatchViewModel model)
        {

            var result = UtilityService.ExecStoredProcedureWithResults<FillinByAttorneyBatchSearch_spResult>("FillinByAttorneyBatchSearch_sp",
                    new FillinByAttorneyBatchSearch_spParams()
                    {
                        StartDate = model.StartDate.ToDateTime(),
                        EndDate = model.EndDate.ToDateTime(),
                        AssignedAttorneyPersonID = model.AssignedAttorneyID,
                        DepartmentCodeID = model.DepartmentID,
                        AgencyID = model.AgencyID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).Select(o => new
                    {

                        o.RootCaseNbr,
                        o.HearingDate,
                        o.HearingDept,
                        o.HearingID,
                        o.HearingType,
                        o.CaseName,
                        o.CaseID,
                        o.AgencyID,
                        o.SortDateTime,
                        o.AssignedAttorney,
                        EncryptedCaseID = o.CaseID.Value.ToEncrypt()
                        ,
                        o.FillinAttorney
                    })
                  .ToList();




            return Json(new DataTablesResponse(1, result, result.Count, result.Count));

        }
        [HttpPost]
        public virtual JsonResult AddFillinByAttorneyBatch(FillinByAttorneyBatchViewModel model)
        {
            if (model.FillinCoverYesNo)
            {

                var result = UtilityService.ExecStoredProcedureWithResults<AttorneyFillinIUD_spResult>("AttorneyFillinIUD_sp",
                        new AttorneyFillinIUD_spParams()
                        {

                            AttorneyFillinDepartmentCodeID = model.DepartmentID,
                            AttorneyFillinEndDate = model.EndDate.ToDateTime(),
                            AttorneyFillinStartDate = model.StartDate.ToDateTime(),
                            PersonID = model.AssignedAttorneyID, // Need to ask
                            IUD = "INSERT",
                            RecordStateID = 1,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid(),
                            ForPersonID = model.AddFillInAttorneyID
                        })
                      .ToList();
            }
            else
            {
                if (!model.FillinCoverCheckedData.IsNullOrEmpty())
                {
                    var items = model.FillinCoverCheckedData.Split(',');
                    foreach (var item in items)
                    {
                        var data = item.Split('|');
                        UtilityService.ExecStoredProcedureWithoutResultADO("FillinByAttorneyBatchAdd_sp",
                       new FillinByAttorneyBatchAdd_spParams()
                       {
                           AgencyID = data[3].ToInt(),
                           CaseID = data[2].ToInt(),
                           HearingDate = data[1],
                           HearingID = data[0].ToInt(),
                           FillinAttorneyPersonID = model.AddFillInAttorneyID,
                           UserID = UserManager.UserExtended.UserID,
                           BatchLogJobID = Guid.NewGuid(),

                       });
                    }
                }
            }
            return Json(new { Status = "Done" });

        }

    }

}