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
    public class getserversController : ApiController
    {
        database_access.branchdb brcdb = new database_access.branchdb();

        public IEnumerable<serverdata> getservers()
        {
            var servers = brcdb.getServers();
            return servers;
        }
    }
}
