using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NewstonsoftDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Catch requests and responses
            config.MessageHandlers.Add(new HttpHandler());

            // Web API configuration and services
            config.Formatters.JsonFormatter.SerializerSettings = Shared.SerializerSettings;

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
