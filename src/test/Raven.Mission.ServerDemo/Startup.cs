using System.Net.Http.Formatting;
using System.Web.Http;
using Owin;

namespace Raven.Mission.ServerDemo
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "order",
                routeTemplate: "Order/{action}",
                defaults: new { controller = "Order" }
            );
            config.Formatters.Insert(0, new JsonMediaTypeFormatter());
            appBuilder.UseWebApi(config);
        }
    }

    
}
