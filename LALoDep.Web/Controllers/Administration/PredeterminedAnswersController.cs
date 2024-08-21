using CrystalDecisions.CrystalReports.Engine;
using DataTables.Mvc;
using LALoDep.Domain.Agency;
using LALoDep.Domain.com_Report;
using LALoDep.Domain.pd_Case;
using LALoDep.Domain.pd_Person;
using LALoDep.Domain.pd_Role;
using LALoDep.Domain.pd_SearchByPhysicalFile;
using LALoDep.Domain.pd_Users.Edit;
using LALoDep.Domain.qcal;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Administration;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LALoDep.Controllers.Administration
{
    [AuthenticationAuthorize]
    public partial class AdministrationController : Controller
    {

        // [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseCleanupPage, PageSecurityItemID = SecurityToken.CaseCleanup_SeeAllAttorneys)]
        public virtual ActionResult PredeterminedAnswersList()
        {
            var viewModel = new PredeterminedAnswersViewModel();


            viewModel.Agencies = UtilityService.ExecStoredProcedureWithResults<AgencyGetByUserID_spResult>(
                    "AgencyGetByUserID_sp", new AgencyGetByUserID_spParams
                    {

                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid(),
                    }).Select(x => new SelectListItem { Text = x.AgencyName, Value = x.AgencyID.ToString() }).ToList();
            viewModel.AgencyGroupList = UtilityService.ExecStoredProcedureWithResults<AgencyGetGroupByUserID_spResult>(
                  "AgencyGetGroupByUserID_sp", new AgencyGetGroupByUserID_sppParams
                  {

                      UserID = UserManager.UserExtended.UserID,
                      BatchLogJobID = Guid.NewGuid(),
                  }).Select(x => new SelectListItem { Text = x.AgencyGroup, Value = x.AgencyGroupID.ToString() }).ToList();

            viewModel.NoteTypeList = UtilityService.ExecStoredProcedureWithResults<qcal_NoteType_spResult>(
               "qcal_NoteType_sp", new qcal_NoteType_spParams
               {
                   LoadOption = "PredeterminedAnswersList",
                   UserID = UserManager.UserExtended.UserID,
                   BatchLogJobID = Guid.NewGuid(),
               }).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() }).ToList();

            viewModel.HearingTypeList = UtilityService.ExecStoredProcedureWithResults<qcal_HearingType_spResult>(
               "qcal_HearingType_sp", new qcal_HearingType_spParams
               {
                   LoadOption = "PredeterminedAnswersList",

                   UserID = UserManager.UserExtended.UserID,
                   BatchLogJobID = Guid.NewGuid(),
               }).Select(x => new SelectListItem { Text = x.CodeDisplay, Value = x.CodeID.ToString() }).ToList();


            viewModel.HearingTypeGroupList = UtilityFunctions.CodeGetByTypeIdAndUserId(1210);

            viewModel.ClientTypeList = UtilityService.ExecStoredProcedureWithResults<qcal_ClientType_spResult>(
               "qcal_ClientType_sp", new qcal_ClientType_spParams
               {
                   LoadOption = "PredeterminedAnswersList",

                   UserID = UserManager.UserExtended.UserID,
                   BatchLogJobID = Guid.NewGuid(),
               }).Select(x => new SelectListItem { Text = x.ClientTypeDisplay, Value = x.ClientType.ToString() }).ToList();




            return View(viewModel);
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CaseCleanupPage, PageSecurityItemID = SecurityToken.CaseCleanup_SeeAllAttorneys)]
        [HttpPost]
        public virtual JsonResult PredeterminedAnswersList(PredeterminedAnswersViewModel viewModel)
        {

            var result = UtilityService.ExecStoredProcedureWithResults<qcal_PredeterminedAnswersList_spResult>("qcal_PredeterminedAnswersList_sp",
                    new qcal_PredeterminedAnswersList_spParams()
                    {
                        AgencyGroupID = viewModel.AgencyGroupID,
                        AgencyID = viewModel.AgencyID,
                        ClientType = viewModel.ClientTypeID,
                        NoteTypeCodeID = viewModel.NoteTypeID,
                        HearingTypeCodeID = viewModel.HearingTypeID,
                        LoadOption = "PredeterminedAnswersList",
                        HearingTypeGroupCodeID = viewModel.HearingTypeGroupID,
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).Select(o => new
                    {

                        o.InactiveFlag,
                        o.PredeterminedAnswer,
                        o.ShortValue,
                        o.Seq,
                        o.SortSeq,
                        o.QuickNoteID,
                        EncryptedQuickNoteID = o.QuickNoteID.ToEncrypt()

                    })
                  .ToList();
            var active = result.Where(o => o.InactiveFlag == 0).ToList();
            var inactive = result.Where(o => o.InactiveFlag == 1).ToList();

            List<DataTablesResponse> dt = new List<DataTablesResponse> { new DataTablesResponse(0, active, active.Count, active.Count),
            new DataTablesResponse(1, inactive, inactive.Count, inactive.Count)
           };

            return Json(dt);

        }


        public virtual ActionResult PredeterminedAnswersAddEdit(string id)
        {
            var viewModel = new PredeterminedAnswersAddEditModel();

            var spParams = new QuickNoteGet_spParams
            {
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid(),
                QuickNoteID = string.IsNullOrEmpty(id) ? (int?)null : id.ToDecrypt().ToInt()

            };
            if (spParams.QuickNoteID.HasValue)
            {
                var quickNote = UtilityService.ExecStoredProcedureWithResults<QuickNoteGet_spResult>(
              "QuickNoteGet_sp", spParams).FirstOrDefault();
                if (quickNote != null)
                {
                    viewModel.QuickNoteID = quickNote.QuickNoteID;

                    viewModel.Seq = quickNote.QuickNoteSequence;


                    viewModel.ShortValue = quickNote.QuickNoteValue;



                    viewModel.PredeterminedAnswer = quickNote.QuickNoteLongValue;
                    viewModel.ChildClients = quickNote.QuickNoteChildClientFlag.HasValue ? quickNote.QuickNoteChildClientFlag.Value == 1 : false;
                    viewModel.AdultClients = quickNote.QuickNoteAdultClientFlag.HasValue ? quickNote.QuickNoteAdultClientFlag.Value == 1 : false;
                    viewModel.InactiveFlag = quickNote.QuickNoteInactiveFlag.HasValue ? quickNote.QuickNoteInactiveFlag.Value == 1 : false;
                }
            }

            viewModel.Agencies = UtilityService.ExecStoredProcedureWithResults<QuickNoteGetAgencies_spResult>(
                    "QuickNoteGetAgencies_sp", spParams).ToList();

            viewModel.HearingTypeList = UtilityService.ExecStoredProcedureWithResults<QuickNoteGetHearingTypes_spResult>(
                   "QuickNoteGetHearingTypes_sp", spParams).ToList();
            viewModel.NoteTypeList = UtilityService.ExecStoredProcedureWithResults<QuickNoteGetNoteTypes_spResult>(
                   "QuickNoteGetNoteTypes_sp", spParams).ToList();


            return View(viewModel);
        }
        [HttpPost]
        public virtual JsonResult PredeterminedAnswersAddEditSave(PredeterminedAnswersAddEditModel model)
        {
            var qnote = UtilityService.ExecStoredProcedureWithResults<QuickNoteGet_spResult>("QuickNoteIUD_sp",
                    new QuickNoteIUD_spParams()
                    {
                        IUD = model.QuickNoteID.HasValue ? "UPDATE" : "INSERT",
                        QuickNoteID = model.QuickNoteID,
                        QuickNoteAdultClientFlag = model.AdultClients ? 1 : 0,
                        QuickNoteChildClientFlag = model.ChildClients ? 1 : 0,
                        QuickNoteInactiveFlag = model.InactiveFlag ? 1 : 0,
                        QuickNoteLongValue = model.PredeterminedAnswer,
                        QuickNoteValue = model.ShortValue,
                        QuickNoteSequence = model.Seq,

                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).FirstOrDefault();

            if (model.AddAgencies != null)
            {
                foreach (var item in model.AddAgencies)
                {

                    UtilityService.ExecStoredProcedureWithResults<object>("QuickNoteAgencyIUD_sp",
                           new QuickNoteAgencyIUD_spParams()
                           {
                               IUD = item.IUD,
                               QuickNoteID = qnote.QuickNoteID,
                               AgencyID = item.CodeID,
                               QuickNoteAgencyID = item.QuickNoteDependentID,

                               UserID = UserManager.UserExtended.UserID,
                               BatchLogJobID = Guid.NewGuid()
                           }).FirstOrDefault();

                }
            }
            if (model.AddHearingTypes != null)
            {
                foreach (var item in model.AddHearingTypes)
                {

                    UtilityService.ExecStoredProcedureWithResults<object>("QuickNoteCodeIUD_sp",
                           new QuickNoteCodeIUD_spParams()
                           {
                               IUD = item.IUD,
                               QuickNoteID = qnote.QuickNoteID,
                               CodeID = item.CodeID,
                               QuickNoteCodeID = item.QuickNoteDependentID,
                               QuickNoteCodeType = "HearingType",
                               UserID = UserManager.UserExtended.UserID,
                               BatchLogJobID = Guid.NewGuid()
                           }).FirstOrDefault();

                }
            }
            if (model.AddNodeTypes != null)
            {
                foreach (var item in model.AddNodeTypes)
                {

                    UtilityService.ExecStoredProcedureWithResults<object>("QuickNoteCodeIUD_sp",
                           new QuickNoteCodeIUD_spParams()
                           {
                               IUD = item.IUD,
                               QuickNoteID = qnote.QuickNoteID,
                               CodeID = item.CodeID,
                               QuickNoteCodeID = item.QuickNoteDependentID,
                               QuickNoteCodeType = "NoteType",
                               UserID = UserManager.UserExtended.UserID,
                               BatchLogJobID = Guid.NewGuid()
                           }).FirstOrDefault();

                }
            }

            return Json(new { Status = "Done", note = qnote });
        }

        [HttpPost]
        public virtual JsonResult PredeterminedAnswerCopy(string id)
        {
            var newId = UtilityService.ExecStoredProcedureWithResults<object>("qcal_CopyPredeterminedAnswers_sp",
                    new qcal_CopyPredeterminedAnswers_spParams()
                    {
                        FromQuickNoteID = id.ToDecrypt().ToInt(),
                        UserID = UserManager.UserExtended.UserID,
                        BatchLogJobID = Guid.NewGuid()
                    }).FirstOrDefault();
            if (newId != null)
            {

                return Json(new { Status = "Done", URL = "/Administration/PredeterminedAnswersAddEdit/" + newId.ToEncrypt() });

            }

            return Json(new { Status = "fail" });
        }




    }
}