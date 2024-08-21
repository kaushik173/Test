using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LALoDep.Domain
{
    public partial class LALoDepEntities
    {
        public LALoDepEntities()
            : base(GetEntityConnectionString())
        {
        }
        public static string GetEntityConnectionString()
        {
            

            var entityStringBuilder = new EntityConnectionStringBuilder();
            entityStringBuilder.ProviderConnectionString = GetSqlConnectionString() + ";MultipleActiveResultSets=True";
            entityStringBuilder.Provider = "System.Data.SqlClient";
           
            entityStringBuilder.Metadata = "res://*/CADbModel.csdl|res://*/CADbModel.ssdl|res://*/CADbModel.msl";
            return entityStringBuilder.ConnectionString;


        }
        // Elmah Connection String
        public static string GetSqlConnectionString()
        {
            var sqlStringBuilder = new SqlConnectionStringBuilder();
            if (ServerEnvironment() == "Dev")
            {
                sqlStringBuilder.InitialCatalog = "JcatsLouisiana";
                sqlStringBuilder.Password = "0h10@123";
                sqlStringBuilder.UserID = "JcatsNG";
                sqlStringBuilder.DataSource = "10.5.2.23";
            }
            else if (ServerEnvironment() == "Prod")
            {
                sqlStringBuilder.InitialCatalog = "JcatsFcaNG";
                sqlStringBuilder.Password = "jk78sr6";
                sqlStringBuilder.UserID = "JcatsFCAUser";
                sqlStringBuilder.DataSource = "VMWP-CHADVDB02";
            }
            else if (ServerEnvironment() == "Training")
            {
                sqlStringBuilder.InitialCatalog = "";
                sqlStringBuilder.Password = "";
                sqlStringBuilder.UserID = "";
                sqlStringBuilder.DataSource = "";
            }
            return sqlStringBuilder.ConnectionString;
        }
        public static string GetSessionConnectionString()
        {
            var sqlStringBuilder = new SqlConnectionStringBuilder();
            if (ServerEnvironment() == "Dev")
            {
                sqlStringBuilder.InitialCatalog = "JcatsUserSession";
                sqlStringBuilder.Password = "0h10@123";
                sqlStringBuilder.UserID = "JcatsNG";
                sqlStringBuilder.DataSource = "10.5.2.23";
            }
            else if (ServerEnvironment() == "Prod")
            {
                sqlStringBuilder.InitialCatalog = "JcatsUserSession";
                sqlStringBuilder.Password = "jk78sr6";
                sqlStringBuilder.UserID = "JcatsFCAUser";
                sqlStringBuilder.DataSource = "VMWP-CHADVDB02";
            }
            else if (ServerEnvironment() == "Training")
            {
                sqlStringBuilder.InitialCatalog = "";
                sqlStringBuilder.Password = "";
                sqlStringBuilder.UserID = "";
                sqlStringBuilder.DataSource = "";
            }
            return sqlStringBuilder.ConnectionString;

        }
        public static string ServerEnvironment()
        {
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["ServerEnvironment"] != null)
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["ServerEnvironment"];
            }
            return "Dev";
        }

        public static string GetSqlConnectionStringForDbManager()
        {
          
            return GetSqlConnectionString();
        }

    }
}
