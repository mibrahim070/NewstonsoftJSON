using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NewstonsoftDemo.App_Start;
using Newtonsoft.Json.Linq;

namespace NewstonsoftDemo
{

    public class HttpHandler : DelegatingHandler
    {
        private string _schemaPath = "";

        public virtual string SchemasPath
        {
            get { return _schemaPath; }
            set { _schemaPath = value; }
        }

        // Request
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IList<string> errorMessages;
            var method = request.Method.Method;
            var baseRoute = request.RequestUri.Segments[1].Replace("/", "");
            int segments = 0;
            string endpoint = string.Empty;
            string lastSegment = request.RequestUri.Segments[request.RequestUri.Segments.Count() - 1].ToLower();
            string queryString = request.RequestUri.Query;
            bool skipSchemeValidation = false;

            if (baseRoute.Equals("api"))
            {
                segments = request.RequestUri.Segments.Count();
                endpoint = segments >= 2 ? request.RequestUri.Segments[2].Replace("/", "") : string.Empty;
            }

            JsonSchemas? schema = null;

            if (method.Equals("POST") || method.Equals("PUT"))
            {
                string requestBody = request.Content.ReadAsStringAsync().Result;

                if (requestBody == null)
                {
                    throw new GlobalException(HttpStatusCode.NoContent, "No request Body");        
                }

                switch (endpoint)
                {
                    case "clients":
                        if (method.Equals("POST") && segments.Equals(3)) schema = JsonSchemas.PostClientRequest;
                        if (method.Equals("PUT")) schema = JsonSchemas.PutClientRequest;
                        if (method.Equals("GET")) skipSchemeValidation = true;
                        break;
                }

                if (!skipSchemeValidation)
                {
                    if (!Shared.validateJsonSchema(requestBody, schema.Value, out errorMessages))
                    {
                        throw new GlobalException(HttpStatusCode.BadRequest, errorMessages.ToList<String>());
                    }
                }
            }
            
            // RESPONSE 
            return base.SendAsync(request, cancellationToken)
              .ContinueWith(task =>
              {
                  if (task.IsFaulted)
                      throw task.Exception.InnerException;
                  schema = null;

                  var response = task.Result;
                  return response;
              });
        }
    }
}
