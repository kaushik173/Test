using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Aspose.Words;
using LALoDep.Core.Custom.Extensions;
using System.Data;
using Aspose.Words.Tables;
using System.Reflection;

namespace LALoDep.Core.Custom.Utility
{
    public static partial class Utility
    {




        public static byte[] ExecuteMergeDocument(byte[] template, string[] fieldNames, Object[] fieldValues)
        {
            var stream = new MemoryStream(template);
            // Load the template document.
            var doc = new Document(stream);

            // Setup mail merge event handler to do the custom work.
            doc.MailMerge.FieldMergingCallback = new HandleMergeField();

            // Execute the mail merge.
            doc.MailMerge.Execute(fieldNames, fieldValues);

            byte[] generatedFileContent = null;

            using (var ms = new MemoryStream())
            {
                doc.Save(ms, SaveFormat.Doc);
                generatedFileContent = ms.ToArray();
            }

            return generatedFileContent;
        }


        //private static Table AddTable(DataTable dt, Document doc)
        //{

        //    Table table = new Table(doc);
        //    var trH = new Row(doc);
        //    for (var j = 0; j < dt.Columns.Count; j++)
        //    {
        //        var cell = new Cell(doc);
        //        cell.AppendChild(new Paragraph(doc));
        //        cell.FirstParagraph.AppendChild(new Run(doc, dt.Columns[j].Caption != null ? dt.Columns[j].Caption : ""));

        //        // Assume you want columns that are automatically sized.

        //        trH.AppendChild(cell);
        //    }
        //    table.AppendChild(trH);

        //    for (var i = 0; i < dt.Rows.Count; i++)
        //    {
        //        var tr = new Row(doc);
        //        for (var j = 0; j < dt.Columns.Count; j++)
        //        {
        //            var cell = new Cell(doc);
        //            cell.AppendChild(new Paragraph(doc));
        //            cell.FirstParagraph.AppendChild(new Run(doc, dt.Rows[i][j] != null ? dt.Rows[i][j].ToString() : ""));

        //            // Assume you want columns that are automatically sized.

        //            tr.AppendChild(cell);
        //        }
        //        table.AppendChild(tr);
        //    }

        //    return table;
        //}
        public static string Encrypt(string text)
        {
            try
            {
                return CodeComet.Crypto.Encryption.Encrypt(text);
            }
            catch
            {
                return "";
            }

        }
        public static string Decrypt(string encryptText)
        {
            try
            {
                return CodeComet.Crypto.Encryption.Decrypt(encryptText);
            }
            catch
            {
                return "";
            }

        }
        //public static void RaiseEmailError(string message, string responseText = "")
        //  {
        //      var customEx = new Exception(message);

        //      if (!string.IsNullOrEmpty(responseText))
        //          customEx.Data.Add("ResponseText", responseText);


        //      Elmah.ErrorSignal.FromCurrentContext().Raise(customEx);
        //  }


        public static IEnumerable<SelectListItem> GetHoursList(string selectedValue = "")
        {
            var list = new List<SelectListItem>();

            for (int i = 1; i <= 12; i++)
            {
                list.Add(new SelectListItem() { Text = i.ToString(CultureInfo.InvariantCulture), Value = i.ToString(CultureInfo.InvariantCulture), Selected = (selectedValue == i.ToString(CultureInfo.InvariantCulture)) });
            }
            return list;
        }
        public static IEnumerable<SelectListItem> GetMinutesList(string selectedValue = "")
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = @"00",
                    Value = @"00", Selected = (selectedValue=="00")
                },
                new SelectListItem()
                {
                    Text = @"05",
                    Value = @"05", Selected = (selectedValue=="05")
                },
                new SelectListItem()
                {
                    Text = @"10",
                    Value = @"10", Selected = (selectedValue=="10")
                },
                new SelectListItem()
                {
                    Text = @"15",
                    Value = @"15", Selected = (selectedValue=="15")
                },
                new SelectListItem()
                {
                    Text = @"20",
                    Value = @"20", Selected = (selectedValue=="20")
                },
                new SelectListItem()
                {
                    Text = @"25",
                    Value = @"25", Selected = (selectedValue=="25")
                },
                new SelectListItem()
                {
                    Text = @"30",
                    Value = @"30", Selected = (selectedValue=="30")
                },
                new SelectListItem()
                {
                    Text = @"35",
                    Value = @"35", Selected = (selectedValue=="35")
                },
                new SelectListItem()
                {
                    Text = @"40",
                    Value = @"40", Selected = (selectedValue=="40")
                },
                new SelectListItem()
                {
                    Text = @"45",
                    Value = @"45", Selected = (selectedValue=="45")
                },
                new SelectListItem()
                {
                    Text = @"50",
                    Value = @"50", Selected = (selectedValue=="50")
                },
                new SelectListItem()
                {
                    Text = @"55",
                    Value = @"55", Selected = (selectedValue=="55")
                }
            };

            return list;


        }
        public static IEnumerable<SelectListItem> GetMinutesListIncrementOne(string selectedValue = "")
        {
            var list = new List<SelectListItem>();

            for (var i = 0; i <= 59; i++)
            { var min = i.ToString().Length == 1 ? "0" + i.ToString() : i.ToString();
                list.Add(new SelectListItem()
                {
                    Text = min,
                    Value = min,
                    Selected = (selectedValue == min)
                });

            }


            return list;


        }
        public static IEnumerable<SelectListItem> GetTimeAmPm(string selectedValue = "")
        {
            var list = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = @"AM",
                    Value = @"AM", Selected = (selectedValue=="AM")
                },
                new SelectListItem()
                {
                    Text = @"PM",
                    Value = @"PM", Selected = (selectedValue=="PM")
                }
            };

            return list;


        }


        public static SmtpClient GetSmtpClient()
        {
            var smtp = new SmtpClient();// ("smtp.sendgrid.com", 587) { EnableSsl = true };
      //      var networkCred = new System.Net.NetworkCredential("ngapp", "L0nd0n@123");
        //    smtp.UseDefaultCredentials = false;
          //  smtp.Credentials = networkCred;
            smtp.Timeout = 10000;

            return smtp;
        }

        public static string GetClientIpAddress()
        {
            string VisitorsIPAddr = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                VisitorsIPAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = HttpContext.Current.Request.UserHostAddress;
            }
            return VisitorsIPAddr;
        }
        public static string FormatPhoneNumber(this string number)
        {
            try
            {
                if (!string.IsNullOrEmpty(number) && number.Length >= 9)
                {
                    return string.Format("{0:(###) ###-####}", double.Parse(number.Replace("+1", "")));

                }
            }
            catch
            {

            }

            return number;
        }

        public static T GetValueFromDataRow<T>(DataRow dr, string Key)
        {
            try
            {
                if (dr[Key] != DBNull.Value)
                {
                    return (T)dr[Key];
                }

                return default(T);

            }
            catch
            {
                return default(T);
            }
        }
        public static Dictionary<string, object> GetModelPropertiesByModelClass(object atype)
        {
            if (atype == null) return new Dictionary<string, object>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(atype, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }
        public static string GetHiddenFieldsByModel(object atype, string name)
        {
            string model = "";
            var properties = GetModelPropertiesByModelClass(atype);
            foreach (var property in properties)
            {

                model += string.Format("<input name=\"{0}\" id=\"{0}\" value=\"{1}\" type=\"hidden\"/> ", name + property.Key, property.Value) + "\r\n";
            }

            return model;
        }

        public static string GetJavascriptClassModel(object atype, string name)
        {
            string model = "";
            var properties = GetModelPropertiesByModelClass(atype);
            foreach (var property in properties)
            {
                if (model != "")
                    model += ",";
                model += string.Format("{0}:$('#{1}{0}').val()", property.Key, name) + "\r\n";
            }
            model = "{ \r\n" + model + "\r\n}";
            return model;
        }

    }
}