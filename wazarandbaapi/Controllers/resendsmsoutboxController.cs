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
    public class resendsmsoutboxController : ApiController
    {
        database_access.wazarans brcdb = new database_access.wazarans();

        public HttpResponseMessage Post([FromBody] resendoutboxsms db)
        {
            var res = brcdb.resend_sms_outbox(db);

            var response = new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent(JsonConvert.SerializeObject(res), System.Text.Encoding.UTF8, "application/json")
            };
            return response;
        }
    }
}
