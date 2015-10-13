using System.Web.Http;

namespace KanbanBoardApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // enable tracing
            config.EnableSystemDiagnosticsTracing();

            // Web API configuration and services
            config.Filters.Add(new AuthorizeAttribute());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new {id = RouteParameter.Optional});
        }
    }
}