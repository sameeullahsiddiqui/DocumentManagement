using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using DocumentApp.API.Handlers;
using DocumentApp.Logging.Logging;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace DocumentApp.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnsureInitialized();

            config.SuppressDefaultHostAuthentication();

            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Services.Add(typeof(IExceptionLogger), new Log4NetExceptionLogger());
            config.Services.Replace(typeof(IExceptionHandler), new GlobalDocumentAppExceptionHandler());


            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Routes.MapHttpRoute(
                name: "ApiRoute",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
