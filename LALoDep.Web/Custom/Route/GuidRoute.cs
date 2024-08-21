using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LALoDep.Custom
{
    public class GuidRoute : Route
    {
        private readonly bool _isGuidRoute;

        public GuidRoute(string uri, object defaults, string[] namespaces)
            : base(uri, new RouteValueDictionary(defaults), new MvcRouteHandler())
        {
            _isGuidRoute = uri.Contains("guid");

            DataTokens = new RouteValueDictionary();
            DataTokens["namespaces"] = namespaces;
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var routeData = base.GetRouteData(httpContext);
            if (routeData == null)
                return null;
            if (httpContext.Request.QueryString["CurrentSessionGuid"] != null)
            {
                routeData.Values["guid"] = httpContext.Request.QueryString["CurrentSessionGuid"];
            }
            else if (!routeData.Values.ContainsKey("guid") || routeData.Values["guid"].ToString() == "")
                routeData.Values["guid"] = Guid.NewGuid().ToString("N");

            return routeData;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return !_isGuidRoute ? null : base.GetVirtualPath(requestContext, values);
        }
    }
}