using System.Web.Http;
using System.Web.Http.Cors;

namespace MySQLApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var corsAttri = new EnableCorsAttribute("http://localhost", "*","*");
            config.EnableCors(corsAttri);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
