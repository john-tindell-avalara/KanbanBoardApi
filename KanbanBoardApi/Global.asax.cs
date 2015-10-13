using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using FluentValidation.WebApi;
using SimpleInjector;

namespace KanbanBoardApi
{
    public class WebApiApplication : HttpApplication
    {
        public static Container Container = new Container();

        protected void Application_Start()
        {
            SimpleInjectorConfig.Register(Container);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutomapperConfig.Register();

            FluentValidationModelValidatorProvider.Configure(GlobalConfiguration.Configuration,
                provider =>
                {
                    provider.ValidatorFactory = new FluentValidatorFactory(Container);
                });
        }
    }
}