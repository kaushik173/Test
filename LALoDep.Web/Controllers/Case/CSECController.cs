using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataTables.Mvc;
using LALoDep.Domain.CSEC;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Models.Case;
using Omu.ValueInjecter;
using LALoDep.Core.Custom.Utility;
using LALoDep.Custom;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CSECQuestionnaire)]
        public virtual ActionResult CSEC()
        {

            ViewBag.AddQuestionFor = UtilityService.ExecStoredProcedureWithResults<CSECGetAddQuestionaireFor_spResult>("CSECGetAddQuestionaireFor_sp",
                                    new CSECGetAddQuestionaireFor_spParams() { CaseID = UserManager.UserExtended.CaseID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                  .Select(x => new SelectListItem() { Text = x.ChildDisplay, Value = x.ChildRoleID.ToString() });

            return View();
        }
        [HttpPost]
        public virtual ActionResult CSECList()
        {
            var result = UtilityService.ExecStoredProcedureWithResults<CSECGetList_spResult>("CSECGetList_sp",
                new CSECGetList_spParams() { CaseID = UserManager.UserExtended.CaseID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() })
                                  .Select(x => new
                                  {
                                      CSECID = x.CSECID,
                                      Child = x.Child,
                                      DueDate = x.DueDate,
                                      CompletionDate = x.CompletionDate,
                                      AssignedTo = x.AssignedTo,
                                      ScoreNumeric = x.ScoreNumeric,
                                      ScoreLiteral = x.ScoreLiteral,
                                      CanEditFlag = x.CanEditFlag,
                                      CanDeleteFlag = x.CanDeleteFlag,
                                      SortBy = x.SortBy,
                                      EncrypredCSECID = x.CSECID.ToEncrypt()
                                  })
                                  .ToList();

            var totalCompleted = result.Where(x => !string.IsNullOrEmpty(x.CompletionDate)).Count();
            var total = result.Count;
            return Json(new { isSuccess = true, data = result, total = total, totalCompleted = totalCompleted });
        }


        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CSECQuestionnaire, PageSecurityItemID = SecurityToken.CSECQuestionnaireDelete)]
        public virtual ActionResult CSECDelete(int CSECId)
        {
            UtilityService.ExecStoredProcedureScalar("CSECDelete_sp", new CSECDelete_spParams
            {
                CSECID = CSECId,
                UserID = UserManager.UserExtended.UserID,
                BatchLogJobID = Guid.NewGuid()
            });

            return CSECList();
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CSECQuestionnaire, PageSecurityItemID = SecurityToken.CSECQuestionnaireAdd)]
        public virtual ActionResult CSECAdd(int childRoleID)
        {
            var result = UtilityService.ExecStoredProcedureScalar("CSECAddNew_sp",
                                    new CSECAddNew_spParams() { ChildRoleID = childRoleID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() });
            var CSECId = result.ToInt();

            return Json(new { isSuccess = true, Url = Url.Action(MVC.Case.CSECEdit(CSECId.ToEncrypt())) });
        }

        [ClaimsAuthorize(CustomSecurityItemIds = PageLevelSecurityItemIds.CSECQuestionnaire, PageSecurityItemID = SecurityToken.CSECQuestionnaireEdit)]
        public virtual ActionResult CSECEdit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction(MVC.Home.AccessDenied());

            var CSECID = id.ToDecrypt().ToInt();
            var dataTable = UtilityService.ExecStoredProcedureForDataTable("CSECGet_sp",
                                    new CSECGet_spParams() { CSECID = CSECID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() });

            CSECEditViewModel viewModel = new CSECEditViewModel();

            viewModel.CSECID = Utility.GetValueFromDataRow<int>(dataTable.Rows[0], "CSECID");
            viewModel.CaseID = Utility.GetValueFromDataRow<int>(dataTable.Rows[0], "CaseID");
            if (UserManager.UserExtended.CaseID != viewModel.CaseID)
                UserManager.UpdateCaseStatusBar(viewModel.CaseID);

            viewModel.Child = Utility.GetValueFromDataRow<string>(dataTable.Rows[0], "Child");
            viewModel.DueDate = Utility.GetValueFromDataRow<string>(dataTable.Rows[0], "DueDate");
            viewModel.CompletionDate = Utility.GetValueFromDataRow<string>(dataTable.Rows[0], "CompletionDate");
            viewModel.AssignedTo = Utility.GetValueFromDataRow<string>(dataTable.Rows[0], "AssignedTo");
            viewModel.CSECNote = Utility.GetValueFromDataRow<string>(dataTable.Rows[0], "CSECNote");

            viewModel.Answers.Add("1A", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "1A"));
            viewModel.Answers.Add("1B", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "1B"));
            viewModel.Answers.Add("1C", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "1C"));
            viewModel.Answers.Add("1D", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "1D"));
            viewModel.Answers.Add("1E", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "1E"));
            viewModel.Answers.Add("1F", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "1F"));
            viewModel.Answers.Add("1G", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "1G"));

            viewModel.Answers.Add("2A", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "2A"));
            viewModel.Answers.Add("2B", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "2B"));
            viewModel.Answers.Add("2C", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "2C"));
            viewModel.Answers.Add("2D", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "2D"));

            viewModel.Answers.Add("3A", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "3A"));
            viewModel.Answers.Add("3B", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "3B"));
            viewModel.Answers.Add("3C", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "3C"));
            viewModel.Answers.Add("3D", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "3D"));
            viewModel.Answers.Add("3E", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "3E"));
            viewModel.Answers.Add("3F", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "3F"));
            viewModel.Answers.Add("3G", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "3G"));
            viewModel.Answers.Add("3H", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "3H"));

            viewModel.Answers.Add("4A", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "4A"));
            viewModel.Answers.Add("4B", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "4B"));
            viewModel.Answers.Add("4C", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "4C"));
            viewModel.Answers.Add("4D", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "4D"));
            viewModel.Answers.Add("4E", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "4E"));
            viewModel.Answers.Add("4F", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "4F"));
            viewModel.Answers.Add("4G", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "4G"));

            viewModel.Answers.Add("5A", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "5A"));
            viewModel.Answers.Add("5B", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "5B"));
            viewModel.Answers.Add("5C", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "5C"));
            viewModel.Answers.Add("5D", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "5D"));
            viewModel.Answers.Add("5E", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "5E"));
            viewModel.Answers.Add("5F", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "5F"));

            viewModel.Answers.Add("6A", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "6A"));
            viewModel.Answers.Add("6B", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "6B"));
            viewModel.Answers.Add("6C", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "6C"));
            viewModel.Answers.Add("6D", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "6D"));

            viewModel.Answers.Add("7A", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "7A"));
            viewModel.Answers.Add("7B", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "7B"));
            viewModel.Answers.Add("7C", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "7C"));
            viewModel.Answers.Add("7D", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "7D"));
            viewModel.Answers.Add("7E", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "7E"));
            viewModel.Answers.Add("7F", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "7F"));

            viewModel.Answers.Add("8A", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "8A"));
            viewModel.Answers.Add("8B", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "8B"));
            viewModel.Answers.Add("8C", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "8C"));
            viewModel.Answers.Add("8D", Utility.GetValueFromDataRow<short?>(dataTable.Rows[0], "8D"));

            viewModel.Questions = UtilityService.ExecStoredProcedureWithResults<CSECGetQuestions_spResult>("CSECGetQuestions_sp",
                                    new CSECGetQuestions_spParams() { CSECID = CSECID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.CSECQuestionnaire, PageSecurityItemID = SecurityToken.CSECQuestionnaireEdit)]
        public virtual ActionResult CSECEditSave(CSECEditViewModel viewModel)
        {
            foreach (var answer in viewModel.Answers)
            {
                var answerParam = new CSECUpdateAnswer_spParams()
                {
                    CSECID = viewModel.CSECID,
                    QuestionEnum = answer.Key,
                    Value = answer.Value,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                };

                UtilityService.ExecStoredProcedureScalar("CSECUpdateAnswer_sp", answerParam);
            }

            if (viewModel.CSECNoteChanged)
            {
                UtilityService.ExecStoredProcedureScalar("CSECUpdateNote_sp", new CSECUpdateNote_spParams
                {
                    CSECID = viewModel.CSECID,
                    Note = viewModel.CSECNote,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });
            }
            var alertMessage = "";
            var alertTitle = "";
            //Submit button clicked
            if (viewModel.buttonId == 2)
            {
                UtilityService.ExecStoredProcedureScalar("CSECUpdateToSubmitted_sp", new CSECUpdateToSubmitted_spParams
                {
                    CSECID = viewModel.CSECID,
                    UserID = UserManager.UserExtended.UserID,
                    BatchLogJobID = Guid.NewGuid()
                });

                var csecNotification = UtilityService.ExecStoredProcedureWithResults<CSECGetForEmail_spResult>("CSECGetForEmail_sp",
                                 new CSECGetForEmail_spParams() { CSECID = viewModel.CSECID, UserID = UserManager.UserExtended.UserID, BatchLogJobID = Guid.NewGuid() }).FirstOrDefault();
                if (csecNotification != null)
                {
                    alertMessage = csecNotification.AlertMessage;
                    alertTitle = csecNotification.AlertTitle;
                    if (csecNotification.SendEmailFlag == 1)
                    {
                        ClearConsernEmailNotification(csecNotification);
                    }
                }
            }
            return Json(new { isSuccess = true, showMessage = !alertTitle.IsNullOrEmpty(), alertMessage = alertMessage, alertTitle = alertTitle });
        }

        private void ClearConsernEmailNotification(CSECGetForEmail_spResult model)
        {
            if (string.IsNullOrEmpty(model.EmailTo))
                return;

            if (model.EmailTo.Contains(";"))
                model.EmailTo = model.EmailTo.Replace(";", ",");

            model.EmailTo = model.EmailTo.Trim(',');

            var emailRecipient = new Models.EmailRecipient
            {
                ToAddress = model.EmailTo,
                FromAddress = model.EmailFrom,

                Subject = model.EmailSubject
            };

            emailRecipient.CustomData.Add("Message", model.EmailBody);


            new MailController().ClearConsernEmailNotification(emailRecipient).Deliver();
        }
    }
}