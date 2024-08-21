using LALoDep.Domain.JcatsMessage;
using LALoDep.Domain.NG_com;
using LALoDep.Domain.Services;
using LALoDep.Core.Custom.Extensions;
using LALoDep.Custom;
using LALoDep.Models.Twillio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Twilio.AspNet.Common;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;

namespace LALoDep.Controllers
{
    public partial class TwilioWebhookController : TwilioController
    {
        private IUtilityService UtilityService;
        private UserManager UserManager;

        public TwilioWebhookController(UserManager userManager, IUtilityService utilityService)
        {
            UserManager = userManager;
            UtilityService = utilityService;
        }

    

        [HttpPost]
        public virtual TwiMLResult Reply(SmsRequest request)
        {
            if (request != null)
            {
                //var filePath = Server.MapPath("/Log/NotificationLog.txt");
                //System.IO.File.AppendAllText(filePath, "Reply called" + Environment.NewLine);

                var oResponse = UtilityService.ExecStoredProcedureWithResults<JcatsMessageReply_spResult>("JcatsMessageReply_sp",
                                                   new JcatsMessageReply_spParams
                                                   {
                                                       SmsID = request.SmsSid,
                                                       ResponseFromAddress = request.From,
                                                       MessageBody = request.Body,
                                                       UserID = 1,
                                                       BatchLogJobID = Guid.NewGuid(),
                                                       ResponseToAddress = request.To,
                                                       SendEmailFlag = 0
                                                   }).FirstOrDefault();

                //System.IO.File.AppendAllText(filePath, "recipients" + oResponse.recipients + Environment.NewLine);

                if (oResponse != null && !string.IsNullOrEmpty(oResponse.recipients))
                {

                    #region Send push Notification
                    var deviceTokens = UtilityService.ExecStoredProcedureWithResults<NG_DeviceTokenGetByUserID_spResult>("NG_DeviceTokenGetByUserID_sp",
                                               new NG_DeviceTokenGetByUserID_spParams
                                               {
                                                   UserIDList = oResponse.JcatsUserIDList,
                                                   UserID = 1,
                                                   BatchLogJobID = Guid.NewGuid()
                                               }).ToList();



                    //System.IO.File.AppendAllText(filePath, string.Join(",", deviceTokens.Select(x => x.Token)) +Environment.NewLine);
                    //System.IO.File.AppendAllText(filePath, "UserIds: "+ oResponse.JcatsUserIDList + Environment.NewLine);
                    //System.IO.File.AppendAllText(filePath, "CaseID: " + oResponse.CaseID + Environment.NewLine);

                    if (deviceTokens != null && deviceTokens.Any())
                    {
                        var appIds = deviceTokens.Select(x => x.Token).ToList();

                        var summary = UtilityService.Context.pd_CaseGet_sp(oResponse.CaseID, 1, Guid.NewGuid()).FirstOrDefault();

                        var dataToSend = new
                        {
                            id = oResponse.CaseID ?? 0,
                            encryptedCaseID = oResponse.CaseID.ToEncrypt(),
                            pageUrl = Url.Action(MVC.Case.Main(oResponse.CaseID.ToEncrypt()), protocol: Request.Url.Scheme)
                        };

                        var resultFCM = FCMHelper.SendNotification(appIds, request.Body, "Message received for JCATS# " + summary?.CaseNumber, "", "", dataToSend);
                        //System.IO.File.AppendAllText(filePath, "FCM Result: " + resultFCM + Environment.NewLine);
                    }
                    #endregion Send push Notification

                    if (oResponse.recipients.Contains(";"))
                        oResponse.recipients = oResponse.recipients.Replace(";", ",");

                    oResponse.recipients = oResponse.recipients.Trim(',');

                    emailStep:
                    try
                    {
                        var emailRecipient = new Models.EmailRecipient
                        {
                            ToAddress = oResponse.recipients,
                            FromAddress = oResponse.from_address,

                            Subject = oResponse.subject
                        };

                        emailRecipient.CustomData.Add("Message", oResponse.body);
                        new MailController().TwilioReplyEmailNotification(emailRecipient).Deliver();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("The server response was: 4.4.2 Timeout waiting for data from client."))
                        {
                            goto emailStep;
                        }
                    }
                }
            }
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message("Got Reply with: " + request?.Body);
            return null;
        }

        [HttpPost]
        public virtual TwiMLResult UpdateStatus([Form]TwillioSmsStatus status)
        {
            if (status != null)
            {
                UtilityService.ExecStoredProcedureWithoutResults("JcatsMessageUpdateStatus_sp",
                                               new JcatsMessageUpdateStatus_spParams
                                               {
                                                   SmsId = status.SmsSid,
                                                   DeliveryStatus = status.MessageStatus,
                                                   UserID = 1,
                                                   BatchLogJobID = Guid.NewGuid(),
                                               });
            }
            var messagingResponse = new MessagingResponse();
            messagingResponse.Message("Message Status: " + status?.MessageStatus);
            return TwiML(messagingResponse);
        }
    }
}