using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using DocumentManagement.API.Handlers;
using DocumentManagement.API.Logger;
using Microsoft.Owin.Security.OAuth;

namespace DocumentManagement.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Services.Add(typeof(IExceptionLogger), new Log4NetExceptionLogger());
            config.Services.Replace(typeof(IExceptionHandler), new GlobalDocumentManagementExceptionHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


        }
    }
}
