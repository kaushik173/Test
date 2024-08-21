using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Bugsnag;

namespace LALoDep
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
         
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ValidateInputAttribute(false));

            

        }
    }
}
