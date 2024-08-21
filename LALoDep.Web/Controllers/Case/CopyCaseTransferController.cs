using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.pd_Motions;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using LALoDep.Domain.pd_Note;
using LALoDep.Domain.pd_Appeals;
using LALoDep.Domain.Advisement;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_CopyCase;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {

        //   [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.AdvisementsView)]
        public virtual ActionResult CopyCaseTransfer()
        {
            var model = new CopyCaseTransferViewModel();

            model.TransferToList = UtilityService.ExecStoredProcedureWithResults<pd_RoleGetForTransferCase_spResult>("pd_RoleGetForTransferCase_sp",
                                               new pd_RoleGetForTransferCase_spParams_new
                                               {

                                                   UserID = UserManager.UserExtended.UserID,
                                                   BatchLogJobID = Guid.NewGuid(),
                                                   AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                                   CaseID = UserManager.UserExtended.CaseID,
                                                   Mode = "CopyCaseTransferSubset"

                                               }).Select(x => new SelectListItem { Value = x.PersonID + "_" + x.TransferToAgencyID, Text = x.DisplayName }).ToList();

            model.ClientList = UtilityService.ExecStoredProcedureWithResults<pd_CopyCaseTransferSubsetClients_spResult>("pd_CopyCaseTransferSubsetClients_sp",
                                             new pd_CopyCaseTransferSubsetClients_spParams
                                             {

                                                 UserID = UserManager.UserExtended.UserID,
                                                 BatchLogJobID = Guid.NewGuid(),
                                                 AgencyID = UserManager.UserExtended.CaseNumberAgencyID,
                                                 CaseID = UserManager.UserExtended.CaseID,


                                             }).ToList();

            return View(model);
        }

        [HttpPost]
        public virtual ActionResult CopyCaseTransfer(CopyCaseTransferViewModel model)
        {
            if (model.TransferToID.IsNullOrEmpty())
                return Json(new { Status = "Fail", Message = "Transfer To is required" });
            var personId = model.TransferToID.Split('_')[0].ToInt();
            var transferToAgencyID = model.TransferToID.Split('_')[1].ToInt();

            UtilityService.ExecStoredProcedureWithoutResultADO("pd_CopyCaseProcess_sp", new pd_CopyCaseProcess_spParams()
            {
                BatchLogJobID = Guid.NewGuid(),
                CopyAllFlag = 1,
                DoNotRunSummaryFlag = 0,
                FromCaseID = UserManager.UserExtended.CaseID,
                ToAgencyID = transferToAgencyID,
                ToAttorneyPersonID = personId,
                TransferDate = model.TransferDate.ToDateTime(),
                UserID = UserManager.UserExtended.UserID,
                IncludeClientPersonIDList = model.IncludeClientPersonIDList
                ,
                ExcludeClientPersonIDList = model.ExcludeClientPersonIDList


            });

            return Json(new { Status = "Done" });
        }



    }
}