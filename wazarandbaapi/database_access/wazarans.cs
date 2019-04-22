using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using wazarandbaapi.Models;

namespace wazarandbaapi.database_access
{
    public class wazarans
    {
        connec conection = new connec();
        public List<tsynch> tsynch(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<tsynch> { };
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from [dbo].[tsynclog] where sync_dt  BETWEEN GETDATE()-1 AND GETDATE() ORDER BY sync_dt desc", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new tsynch
                {
                    ids = rdr["ids"].ToString(),
                    sync_dt = rdr["sync_dt"].ToString(),
                    sync_typ = rdr["sync_typ"].ToString(),
                    salespointcd = rdr["salespointcd"].ToString(),
                    count_branch = rdr["count_branch"].ToString(),
                    count_ho = rdr["count_ho"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<idBranch> branchId(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<idBranch> { };
            con.Open();
            SqlCommand cmd = new SqlCommand("select * from tmst_salespoint", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new idBranch
                {
                    salespointcd = rdr["salespointcd"].ToString(),
                    salespoint_nm = rdr["salespoint_nm"].ToString(),
                    salespoint_sn = rdr["salespoint_sn"].ToString(),
                    salespoint_typ = rdr["salespoint_typ"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<smsoutbox> smsoutbox(smsoutboxPost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<smsoutbox> { };
            con.Open();
            SqlCommand cmd = new SqlCommand("select top 100 * from tsms_outbox where doc_no like'%"+db.doc_no+"%' and doc_typ like '%"+db.doc_typ+"%' order by sms_dt desc", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new smsoutbox
                {
                    ids = rdr["ids"].ToString(),
                    date = rdr["sms_dt"].ToString(),
                    smsto = rdr["smsto"].ToString(),
                    smsmsg = rdr["smsmsg"].ToString(),
                    process = rdr["processed"].ToString(),
                    docno = rdr["doc_no"].ToString(),
                    token = rdr["token"].ToString(),
                    doctype = rdr["doc_typ"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }

        public List<smsinbox> smsinbox(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<smsinbox> { };
            con.Open();
            SqlCommand cmd = new SqlCommand("select top 100 * from tsms_inbox order by sms_dt desc", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new smsinbox
                {
                    ids = rdr["ids"].ToString(),
                    date = rdr["sms_dt"].ToString(),
                    smsfrom = rdr["smsfrom"].ToString(),
                    smsmsg = rdr["smsmsg"].ToString(),
                    process = rdr["processed"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }

        public List<connection> resend_sms_outbox(resendoutboxsms db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<connection> { };
            con.Open();
            SqlCommand cmd = new SqlCommand("update tsms_outbox set processed=0 where ids='"+db.ids+"'", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new connection
                {
                    result = "Resend Success"
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
    }
}