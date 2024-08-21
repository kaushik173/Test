//using LALoDep.Domain.JcatsMessage;
//using LALoDep.Domain.Services;
//using LALoDep.Core.Custom.Extensions;
//using LALoDep.Custom;
//using LALoDep.Custom.Twillio;
//using LALoDep.Models.Api;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Web.Http;

//namespace LALoDep.Controllers.Api
//{    
//    public partial class TextMessagesController: ApiBaseController
//    {
//        public TextMessagesController(UserManager _userManager, IUtilityService _utilityService) : base(_userManager, _utilityService) { }

//        [HttpGet]
//        public ApiResult<TextMessageSendToModel> GetSendTo(int caseId)
//        {
//            var result = new ApiResult<TextMessageSendToModel> { IsSuccess = true, Result = new TextMessageSendToModel() };
//            try
//            {
//                result.Result.SendTo = UtilityService.ExecStoredProcedureWithResults<JcatsMessageGetSendTo_spResult>("JcatsMessageGetSendTo_sp",
//                                                new JcatsMessageGetSendTo_spParams
//                                                {
//                                                    CaseID = caseId,
//                                                    BatchLogJobID = Guid.NewGuid(),
//                                                    UserID = UserManager.UserExtended.UserID
//                                                }).ToList();

//                var permission = UtilityService.ExecStoredProcedureWithResults<JcatsMessageGetPermissions_spResult>("JcatsMessageGetPermissions_sp",
//                                                    new JcatsMessageGetPermissions_spParams
//                                                    {
//                                                        CaseID = caseId,
//                                                        BatchLogJobID = Guid.NewGuid(),
//                                                        UserID = UserManager.UserExtended.UserID
//                                                    }).FirstOrDefault();

//                result.Result.IsDisabled = permission?.DisableTextingFlag == 1;
//                result.Result.DisabledMessage = permission?.DisableTextingAlert;
//            }
//            catch (Exception ex)
//            {
//                result.Message = ex.Message;
//                result.IsSuccess = false;
//            }
//            return result;
//        }

//        [HttpGet]
//        public ApiResult<List<JcatsMessageGetHistoryForCase_spResult>> HistoryForCase(int caseId)
//        {
//            var result = new ApiResult<List<JcatsMessageGetHistoryForCase_spResult>> { IsSuccess = true };
//            try
//            {
//                result.Result = UtilityService.ExecStoredProcedureWithResults<JcatsMessageGetHistoryForCase_spResult>("JcatsMessageGetHistoryForCase_sp",
//                                                  new JcatsMessageGetHistoryForCase_spParams
//                                                  {
//                                                      CaseID = caseId,
//                                                      BatchLogJobID = Guid.NewGuid(),
//                                                      UserID = UserManager.UserExtended.UserID
//                                                  }).ToList();
//            }
//            catch (Exception ex)
//            {
//                result.Message = ex.Message;
//                result.IsSuccess = false;
//            }
//            return result;
//        }

//        [HttpPost]
//        public async Task<ApiResult<string>> SendMessage(SendTextMessagesModel sendTextMessages)
//        {
//            var result = new ApiResult<string> { IsSuccess = true };
//            try
//            {
//                if (sendTextMessages.MessageSendToList != null && sendTextMessages.MessageSendToList.Any())
//                {
//                    var otherMobileNo = sendTextMessages.MessageSendToList.FirstOrDefault(o => o.RoleID.HasValue == false);
//                    if (otherMobileNo != null)
//                    {
//                        var message = UtilityService.ExecStoredProcedureWithResults<JcatsMessageOptOutCheck_spResult>("JcatsMessageOptOutCheck_sp",
//                                             new JcatsMessageOptOutCheck_spParams
//                                             {
//                                                 AddressToCheck = otherMobileNo.SendTo,
//                                                 UserID = UserManager.UserExtended.UserID,
//                                                 BatchLogJobID = Guid.NewGuid(),

//                                             }).FirstOrDefault();
//                        if (message != null)
//                        {
//                            result.IsSuccess = false;
//                            result.Message = message.AlertDisplay;
//                            return result;
//                        }

//                    }


//                    foreach (var item in sendTextMessages.MessageSendToList)
//                    {
//                        var message = UtilityService.ExecStoredProcedureWithResults<JcatsMessageInsert_spResult>("JcatsMessageInsert_sp",
//                                                    new JcatsMessageInsert_spParams
//                                                    {
//                                                        InsertMode = "MANUAL-UI",
//                                                        CaseID = sendTextMessages.CaseID,
//                                                        EntityType = "Role",
//                                                        EntityID = item.RoleID,
//                                                        AddressSentTo = item.SendTo,
//                                                        JcatsMessageType = "Text",
//                                                        MessageBody = sendTextMessages.Message,
//                                                        UserID = UserManager.UserExtended.UserID,
//                                                        BatchLogJobID = Guid.NewGuid(),
//                                                        DoNotReturnRecordset = 0,
//                                                    }).FirstOrDefault();

//                        if (message != null)
//                        {
//                            item.SendTo = item.SendTo.StartsWith("+") ? item.SendTo : "+1" + item.SendTo;
//                            var response = await new TwilioSMSHelper().SendMessage(item.SendTo, message.MessageBody, message.SentFrom);

//                            UtilityService.ExecStoredProcedureWithoutResults("JcatsMessageUpdateStatus_sp",
//                                                        new JcatsMessageUpdateStatus_spParams
//                                                        {
//                                                            JcatsMessageID = message.JcatsMessageID,
//                                                            SmsId = response.Sid,
//                                                            DeliveryStatus = response.Status.ToString(),
//                                                            UserID = UserManager.UserExtended.UserID,
//                                                            BatchLogJobID = Guid.NewGuid(),
//                                                        });
//                        }
//                    }

//                }                
//            }
//            catch (Exception ex)
//            {
//                result.Message = ex.Message;
//                result.IsSuccess = false;
//            }
//            return result;
//        }
//    }
//}
