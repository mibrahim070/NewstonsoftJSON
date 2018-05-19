using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace NewstonsoftDemo.App_Start
{
    public class GlobalException : Exception
    {
        public GlobalException (HttpStatusCode httpStatusCode, String message)
        {
            throw new HttpResponseException(new HttpResponseMessage(httpStatusCode) { Content = new StringContent(message) });
        }

        public GlobalException(HttpStatusCode httpStatusCode, List<String> errorMessages)
        {
            StringBuilder message = new StringBuilder();
            for (int i = 0; i < errorMessages.Count; i++)
            {
                message.Append(errorMessages[i]);
                message.Append("\n");
            }
            throw new HttpResponseException(new HttpResponseMessage(httpStatusCode) { Content = new StringContent(message.ToString()) });
        }
    }
}
