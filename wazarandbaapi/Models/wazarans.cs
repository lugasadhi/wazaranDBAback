using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wazarandbaapi.Models
{
    public class wazarans{}
    public class tsynch
    {
        public string ids { get; set; }
        public string sync_dt { get; set; }
        public string sync_typ { get; set; }
        public string salespointcd { get; set; }
        public string count_branch { get; set; }
        public string count_ho { get; set; }
    }
    public class smsoutboxPost
    {
        public string dbName { get; set; }
        public string branch { get; set; }
        public string doc_typ { get; set; }
        public string doc_no { get; set; }
    }
    public class smsoutbox
    {
        public string ids { get; set; }
        public string date { get; set; }
        public string smsto { get; set; }
        public string smsmsg { get; set; }
        public string process { get; set; }
        public string docno { get; set; }
        public string token { get; set; }
        public string doctype { get; set; }
    }
    public class smsinbox
    {
        public string ids { get; set; }
        public string date { get; set; }
        public string smsfrom { get; set; }
        public string smsmsg { get; set; }
        public string process { get; set; }
    }
    public class resendoutboxsms
    {
        public string dbName { get; set; }
        public string branch { get; set; }
        public string ids { get; set; }

    }
}