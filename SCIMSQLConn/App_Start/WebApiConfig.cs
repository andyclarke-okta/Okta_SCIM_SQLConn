using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Tracing;
using SCIMSQLConn.Services;


namespace SCIMSQLConn
{
    public static class WebApiConfig
    {
        public static string basicAuthUser = ConfigurationManager.AppSettings["basicAuth.username"];
        public static string basicAuthPassword = ConfigurationManager.AppSettings["basicAuth.password"];
        public static string basicAuthRealm = ConfigurationManager.AppSettings["basicAuth.realm"];
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // ignore null json values
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            // return json rather than xml in chrome.
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeWithQualityHeaderValue("text/html"));

            //added for SCIM2.0 accept http type
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/scim+json"));

            //added for Basic Authentication Handling
            config.MessageHandlers.Add(new BasicAuthHandler(basicAuthUser, basicAuthPassword));

            // added for bearer token authentication.
            // Must reference OWIN libraries for the following 2 lines to work
            //config.SuppressDefaultHostAuthentication();
            //config.Filters.Add(new HostAuthenticationFilter(Microsoft.Owin.Security.OAuth.OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            //for SCIM 1.1
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { controller = "ServiceProviderConfigs", id = RouteParameter.Optional }

            //for SCIM 2.0
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { controller = "ServiceProviderConfig", id = RouteParameter.Optional }

            );

            
        }
    }
}
