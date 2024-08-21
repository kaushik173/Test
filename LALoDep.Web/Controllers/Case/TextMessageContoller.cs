using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LALoDep.Domain.JcatsMessage;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom.Attributes;
using LALoDep.Custom.Security;
using LALoDep.Custom.Twillio;
using LALoDep.Models.Case;

namespace LALoDep.Controllers
{
    public partial class CaseController
    {
        [ClaimsAuthorize(IsCasePage = true, CustomSecurityItemIds = PageLevelSecurityItemIds.TextMessage)]
        public virtual ActionResult TextMessages()
        {
            var viewModel = new TextMessagesViewModel
            {
                MessageSendToList = UtilityService.ExecStoredProcedureWithResults<JcatsMessageGetSendTo_spResult>("JcatsMessageGetSendTo_sp",
                                                new JcatsMessageGetSendTo_spParams
                                                {
                                                    CaseID = UserManager.UserExtended.CaseID,
                                                    BatchLogJobID = Guid.NewGuid(),
                                                    UserID = UserManager.UserExtended.UserID

                                                }).ToList(),

                MessageHistory = UtilityService.ExecStoredProcedureWithResults<JcatsMessageGetHistoryForCase_spResult>("JcatsMessageGetHistoryForCase_sp",
                                                new JcatsMessageGetHistoryForCase_spParams
                                                {
                                                    CaseID = UserManager.UserExtended.CaseID,
                                                    BatchLogJobID = Guid.NewGuid(),
                                                    UserID = UserManager.UserExtended.UserID
                                                }).ToList(),
                MessagePermission = UtilityService.ExecStoredProcedureWithResults<JcatsMessageGetPermissions_spResult>("JcatsMessageGetPermissions_sp",
                                                   new JcatsMessageGetPermissions_spParams
                                                   {
                                                       CaseID = UserManager.UserExtended.CaseID,
                                                       BatchLogJobID = Guid.NewGuid(),
                                                       UserID = UserManager.UserExtended.UserID
                                                   }).FirstOrDefault()
            };

            if (viewModel.MessagePermission != null)
            {
                viewModel.IsDisabled = (viewModel.MessagePermission.DisableTextingFlag.HasValue && viewModel.MessagePermission.DisableTextingFlag.Value == 1) ? true : false;
                viewModel.DisabledMessage = viewModel.MessagePermission.DisableTextingAlert;
            }

            return View(viewModel);
        }

        [HttpPost]
        public virtual async Task<ActionResult> TextMessages(TextMessagesViewModel viewModel)
        {
            if (viewModel.MessageSendToList != null && viewModel.MessageSendToList.Any())
            {
                var otherMobileNo = viewModel.MessageSendToList.FirstOrDefault(o => o.RoleID.HasValue == false);
                if (otherMobileNo != null)
                {
                    var message = UtilityService.ExecStoredProcedureWithResults<JcatsMessageOptOutCheck_spResult>("JcatsMessageOptOutCheck_sp",
                                         new JcatsMessageOptOutCheck_spParams
                                         {
                                             AddressToCheck = otherMobileNo.SendTo,
                                             UserID = UserManager.UserExtended.UserID,
                                             BatchLogJobID = Guid.NewGuid(),

                                         }).FirstOrDefault();
                    if (message != null)
                        return Json(new { isSuccess = false, Message = message.AlertDisplay });
                }


                foreach (var item in viewModel.MessageSendToList)
                {
                    item.SendTo = item.SendTo.StartsWith("+") ? item.SendTo : "+1" + item.SendTo;
                    var message = UtilityService.ExecStoredProcedureWithResults<JcatsMessageInsert_spResult>("JcatsMessageInsert_sp",
                                                new JcatsMessageInsert_spParams
                                                {
                                                    InsertMode = "MANUAL-UI",
                                                    CaseID = UserManager.UserExtended.CaseID,
                                                    EntityType = "Role",
                                                    EntityID = item.RoleID,
                                                    AddressSentTo = item.SendTo,
                                                    JcatsMessageType = "Text",
                                                    MessageBody = viewModel.Message,
                                                    UserID = UserManager.UserExtended.UserID,
                                                    BatchLogJobID = Guid.NewGuid(),
                                                    DoNotReturnRecordset = 0,
                                                }).FirstOrDefault();

                    if (message != null)
                    {
                        try
                        {
                            var response = await new TwilioSMSHelper().SendMessage(item.SendTo, message.MessageBody, message.SentFrom);
                            UtilityService.ExecStoredProcedureWithoutResults("JcatsMessageUpdateStatus_sp",
                            new JcatsMessageUpdateStatus_spParams
                            {
                                JcatsMessageID = message.JcatsMessageID,
                                SmsId = response.Sid,
                                DeliveryStatus = response.Status.ToString(),
                                UserID = UserManager.UserExtended.UserID,
                                BatchLogJobID = Guid.NewGuid(),
                            });
                        }
                        catch (Exception ex)
                        {
                            UtilityService.ExecStoredProcedureWithoutResults("JcatsMessageUpdateStatus_sp",
                           new JcatsMessageUpdateStatus_spParams
                           {
                               JcatsMessageID = message.JcatsMessageID,
                               DeliveryStatus = "Error",
                               UserID = UserManager.UserExtended.UserID,
                               BatchLogJobID = Guid.NewGuid(),
                               DebugErrorMessage = ex.Message  
                           });
                            return Json(new { isSuccess = false, Message = ex.Message });
                        }
                    }
                }

            }
            return Json(new { isSuccess = true });
        }
    }
}