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
    public class dropuserloginController : ApiController
    {
        database_access.branchdb brcdb = new database_access.branchdb();

        public HttpResponseMessage Post([FromBody] addUserDb db)
        {
            var res = brcdb.dropUserLogin(db);

            var response = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(JsonConvert.SerializeObject(res), System.Text.Encoding.UTF8, "application/json")
            };
            return response;
        }
    }
}
