using System.Web.Mvc;

namespace LALoDep.Areas.Mobile
{
    public class MobileAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Mobile";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Mobile_default",
                "Mobile/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[] { "LALoDep.Areas.Mobile.Controllers" } 
            ); context.MapRoute(
               "Mobile_default2",
               "g/{guid}/Mobile/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional },
               new string[] { "LALoDep.Areas.Mobile.Controllers" }
           );
        }
    }
}