using LALoDep.Custom;
using System.Linq;
using System.Web.Mvc;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Inquiry;
using LALoDep.Domain.pd_Role;
using System;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_CaseLoad;
using LALoDep.Domain.pd_Case;

namespace LALoDep.Controllers
{
    public partial class InquiryController : Controller
    {

        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.TransferCase)]
        public virtual ActionResult TransferCase(string id, string pId, string roleTypeId)
        {
            var caseID = id.ToDecrypt().ToInt();
            UserManager.UpdateCaseStatusBar(caseID);
            var viewModel = new TransferCaseViewModel();
            viewModel.PersonID = pId.ToDecrypt().ToInt();
            if (viewModel.PersonID == 0)
                viewModel.PersonID = UserManager.UserExtended.PersonID;
            var person = UtilityService.ExecStoredProcedureWithResults<pd_PersonGet_spResult>("pd_PersonGet_sp",
                                    new pd_PersonGet_spParams { PersonID = viewModel.PersonID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();



            ViewBag.PersonName = person.FirstName + " " + person.LastName;
            viewModel.TransferToPersonList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForTransferCase_spResult>("pd_RoleGetForTransferCase_sp",
                                                    new pd_RoleGetForTransferCase_spParams
                                                    {
                                                        RoleTypeCodeID = roleTypeId.ToDecrypt().ToInt(),
                                                        UserID = UserManager.UserExtended.UserID,
                                                        BatchLogJobID = Guid.NewGuid()
                                                    })
                                                    .Where(x => x.PersonID != viewModel.PersonID)
                                                    .Select(x => new SelectListItem { Value = x.PersonID + "_" + x.TransferToAgencyID, Text = x.DisplayName }).ToList();

            var caseListParams = new pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3Transfer_spParams()
            {
                PersonID = viewModel.PersonID,
                CaseStatus = UserManager.UserExtended.Status,
                RoleType = UserManager.UserExtended.Type,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                SortOption = "PetitionNumber",
                SortDirection = "ASC",
                ParmCaseID = (Request.QueryString["pageId"] != null ? caseID : (int?)null)

            };
            if (Session["MyCaseLoadFilter"] != null)
            {

                var filter = (MyCaseLoadIndexViewModel)Session["MyCaseLoadFilter"];
                if (filter != null)
                {
                    caseListParams.PersonNameFirst = filter.FirstName;
                    caseListParams.PersonNameLast = filter.LastName;
                    caseListParams.PetitionNumber = filter.CaseNumber;
                    caseListParams.CaseStatus = filter.CaseStatus;
                    if (!filter.StartDate.IsNullOrEmpty())
                    {
                        caseListParams.StartDate = filter.StartDate.ToDateTime();
                    }
                    if (!filter.EndDate.IsNullOrEmpty())
                    {
                        caseListParams.EndDate = filter.EndDate.ToDateTime();
                    }
                }
            }


            var cases = UtilityService.ExecStoredProcedureWithResults<pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3Transfer_spResult>
                                                            ("pd_CaseloadGetByPersonIDCaseStatusAppointmentDate3Transfer_sp", caseListParams).ToList();

            viewModel.TotalCases = cases.Select(x => x.TotalCases).FirstOrDefault();
            viewModel.CaseList = cases.Select(x => new CaseListViewModel
            {
                CaseID = x.CaseID,
                Clients = x.Clients,
                Department = x.Department,
                PetitionNumber = x.PetitionNumber,
                CaseName = x.CaseName,
                AttorneyRoleStartDate = x.AttorneyRoleStartDate,
                RoleTypeCodeID = x.RoleTypeCodeID,
                IsOn = x.CaseID == UserManager.UserExtended.CaseID
            }).Where(o => o.CaseID.HasValue).ToList();

            var onCase = viewModel.CaseList.FirstOrDefault(x => x.CaseID == UserManager.UserExtended.CaseID);
            if (onCase != null)
            {
                var index = viewModel.CaseList.IndexOf(onCase);
                viewModel.CaseList.RemoveAt(index);
                viewModel.CaseList.Insert(0, onCase);
            }
            if (TempData["TransferCaseViewModel"] != null)
            {
                viewModel.TransferToPersonID = (TempData["TransferCaseViewModel"] as TransferCaseViewModel).TransferToPersonID;
                viewModel.TransferDate = (TempData["TransferCaseViewModel"] as TransferCaseViewModel).TransferDate;
            }

            return View(viewModel);
        }

        [ClaimsAuthorize(PageSecurityItemID = SecurityToken.TransferCase)]
        [HttpPost]
        public virtual ActionResult TransferCase(TransferCaseViewModel viewModel)
        {
            TempData["TransferCaseViewModel"] = viewModel;
            var personAndAgency = viewModel.TransferToPersonID.Split('_');
            var ToPersonId = personAndAgency[0].ToInt();
            var ToAgencyId = personAndAgency[1].ToInt();
            if (viewModel.CaseList != null)
            {


                foreach (var item in viewModel.CaseList)
                {
                    if (ToAgencyId > 0)
                    {
                        var copyCaseParam = new pd_CopyCaseProcess_spParams
                        {
                            FromCaseID = item.CaseID,
                            ToAgencyID = ToAgencyId,
                            ToAttorneyPersonID = ToPersonId,
                            IncludeRoleID = 0,
                            CopyAllFlag = 1,
                            TransferDate = DateTime.Parse(viewModel.TransferDate),
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        };
                        UtilityService.ExecStoredProcedureWithResults<object>("pd_CopyCaseProcess_sp", copyCaseParam,timeout:180).FirstOrDefault();
                    }
                    else
                    {
                        var transferCaseParam = new pd_CaseTransfer_spParams
                        {
                            CaseID = item.CaseID,
                            FromPersonID = viewModel.PersonID,
                            TransferToPersonID = ToPersonId,
                            TransferDate = DateTime.Parse(viewModel.TransferDate),
                            RoleTypeCodeID = item.RoleTypeCodeID,
                            UserID = UserManager.UserExtended.UserID,
                            BatchLogJobID = Guid.NewGuid()
                        };

                        UtilityService.ExecStoredProcedureWithResults<object>("pd_CaseTransfer_sp", transferCaseParam, timeout: 180).FirstOrDefault();
                    }
                }
            }
            return Json(new { isSuccess = true });
        }
    }
}