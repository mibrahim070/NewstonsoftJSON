using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.Hosting;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using NewstonsoftDemo.App_Start;

namespace NewstonsoftDemo
{
    public enum JsonSchemas
    {
        PostClientRequest,
        PutClientRequest,
    }

    public static class Shared
    {

        public static JsonSerializerSettings SerializerSettings => new JsonSerializerSettings
        {
            Formatting = Newtonsoft.Json.Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static bool validateJsonSchema(string JSON, JsonSchemas schemeToValidate, out IList<string> errorMessages, string schemasPath = "")
        {
            string path = getDirectoryPath(schemasPath, @"Schemas/");
            errorMessages = new List<string>();

            var resolver = new JSchemaPreloadedResolver();
            getResolvers(ref resolver, path);
            string fileSchema = string.Empty;

            switch (schemeToValidate)
            {
                case JsonSchemas.PostClientRequest: fileSchema = @"ClientsAPI/postClient.json"; break;
                case JsonSchemas.PutClientRequest: fileSchema = @"ClientsAPI/putClient.json"; break;
            }

            if (string.IsNullOrEmpty(fileSchema)) { errorMessages.Add("JSON schema definition missing"); return false; }

            JSchema schema;
            JObject json;
            try
            {
                using (StreamReader file = File.OpenText(path + fileSchema))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    schema = JSchema.Load(reader, resolver);
                    json = new JObject();

                    try
                    {
                        json = JObject.Parse(JSON);
                    }
                    catch (JsonReaderException ex) { throw new GlobalException(HttpStatusCode.NotFound, "Not Valid JSON Format"); }
                }
            }
            catch (FileNotFoundException fex) { throw new GlobalException(HttpStatusCode.NotFound, "File not found"); }

            return json.IsValid(schema, out errorMessages);
        }

        private static void getResolvers(ref JSchemaPreloadedResolver resolver, string path)
        {
            #region Data
            resolver.Add(new Uri("dataJsonAPI.json", UriKind.RelativeOrAbsolute), new FileStream(path + @"Data/dataJsonAPI.json", FileMode.Open, FileAccess.Read));
            #endregion
            #region RegexPatterns
            resolver.Add(new Uri("clientIdRegex.json", UriKind.RelativeOrAbsolute), new FileStream(path + @"RegexPatterns/clientIdRegex.json", FileMode.Open, FileAccess.Read));
            #endregion
        }

        public static string getDirectoryPath(string currentPath, string expectedDirectory)
        {
            string path = "";

            if (string.IsNullOrEmpty(currentPath) && string.IsNullOrEmpty(HostingEnvironment.MapPath("~")))
            {
                path = Path.GetFullPath(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "../../../")
                          + expectedDirectory;
            }
            else
            {
                path = (string.IsNullOrEmpty(currentPath)) ?
                  Path.Combine((HostingEnvironment.MapPath("~") != null) ? HostingEnvironment.MapPath("~")
                  : "", expectedDirectory) : currentPath;
            }

            return path;
        }

  }
}
