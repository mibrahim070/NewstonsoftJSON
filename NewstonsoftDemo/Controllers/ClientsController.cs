using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace NewstonsoftDemo.Controllers
{
    public class ClientsController : ApiController
    {
        public IHttpActionResult Get()
        {
            return Ok();
        }
        
        public IHttpActionResult Post([FromBody]string value)
        {
            return Created("", new JObject());
        }
        
        public IHttpActionResult Put(string id, [FromBody]string value)
        {
            return Ok();
        }
    }
}
