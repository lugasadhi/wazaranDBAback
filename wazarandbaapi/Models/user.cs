using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wazarandbaapi.Models
{
    public class user
    {
        public string name { get; set; }
        public string password { get; set; }
    }
    public class getuser
    {
        public int id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
    public class register
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}