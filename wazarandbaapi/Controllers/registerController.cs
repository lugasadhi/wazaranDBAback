using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using wazarandbaapi.Models;
using System.Data;
using Newtonsoft.Json;

namespace wazarandbaapi.Controllers
{
    public class registerController : ApiController
    {
        database_access.db db_layer = new database_access.db();

        public HttpResponseMessage Post([FromBody] register us)
        {
            var res = db_layer.register(us);
            var response = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(JsonConvert.SerializeObject(res), System.Text.Encoding.UTF8, "application/json")
            };
  
            return response;
        }
    }
}
