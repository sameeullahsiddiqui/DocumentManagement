using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DocumentApp.API.Capsule;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Cors;
using DocumentApp.API.Filters;

[assembly: OwinStartup(typeof(DocumentApp.API.Startup))]

namespace DocumentApp.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            WebApiConfig.Register(config);

            new WebCapsule().Initialise(config);

            app.UseCors(CorsOptions.AllowAll);

            app.UseWebApi(config);

            GlobalConfiguration.Configuration.Filters.Add(new DocumentAppExceptionFilterAttribute());
            log4net.Config.XmlConfigurator.Configure();
            
        }

       
    }
}
