using System.Web;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using System;

namespace MoveItDemo
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start()
        {
      
        }

        private object Application_PostAuthenticateRequest(object @object, object sender, object eventArgs, object e)
        { 
            throw new NotImplementedException();
        }
    }
}
