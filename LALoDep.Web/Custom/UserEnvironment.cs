using System.Web.Mvc;

namespace LALoDep.Custom
{
    public static class UserEnvironment
    {
        public static UserManager UserManager
        {
            get
            {
                return DependencyResolver.Current.GetService<UserManager>(); 
                
            }
        }

        public static bool IsDevEnvironmentUrl()
        {
            return ServerEnvironment()?.ToLower() == "dev";
        }

        public static string ServerEnvironment()
        {
            if (System.Web.Configuration.WebConfigurationManager.AppSettings["ServerEnvironment"] != null)
            {
                return System.Web.Configuration.WebConfigurationManager.AppSettings["ServerEnvironment"];
            }
            return "Dev";
        }
    }
}