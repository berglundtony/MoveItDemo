using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData.Builder;

namespace MoveItDemo.App_Start
{
    public class ODataConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes(); //This has to be called before the following OData mapping, so also before WebApi mapping

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            builder.EntitySet<PriceOffert>("DataHandling");
        }
    }
}