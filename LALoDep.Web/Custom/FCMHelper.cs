using LALoDep.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace LALoDep.Custom
{
    public sealed class FCMHelper
    {
        public static string SendNotification(List<string> appIds, string body, string title, string icon, string clickAction,  dynamic data)
        {
            const string FcmUrl = "https://fcm.googleapis.com/fcm/send";
            try
            {
                string FcmServerKey = ConfigurationManager.AppSettings["FCMServerKey"];
                var notificationToSend = new
                {
                    registration_ids = appIds,
                    notification =
                      new
                      {
                          body,
                          title,
                          icon,
                          click_action = clickAction
                      },
                    data
                };

                byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(notificationToSend));

                //Make web request
                WebRequest tRequest = WebRequest.Create(FcmUrl);
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", FcmServerKey));


                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    WebResponse tResponse = tRequest.GetResponse();
                    Stream dataStreamResponse = tResponse.GetResponseStream();
                    StreamReader tReader = new StreamReader(dataStreamResponse);
                    string sResponseFromServer = tReader.ReadToEnd();
                    FCMResponse result = JsonConvert.DeserializeObject<FCMResponse>(sResponseFromServer);

                    return sResponseFromServer;
                }
            }
            catch (Exception ex)
            {
                //handle this exception if ndede.
                
            }

            return string.Empty;
        }
    }
}