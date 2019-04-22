using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wazarandbaapi.Models
{
    public class connection
    {
        public string result { get; set; }
    }
    public class log
    {
        public string username { get; set; }
        public string branch { get; set; } 
        public string logs { get; set; }
        public string descript   { get; set; }
    }
    public class viewlog
    {
        public string id { get; set; }
        public string date_time { get; set; }
        public string username { get; set; }
        public string branch { get; set; }
        public string logs { get; set; }
        public string descript { get; set; }
    }
    public class getlog
    {
        public string username { get; set; }

    }
}