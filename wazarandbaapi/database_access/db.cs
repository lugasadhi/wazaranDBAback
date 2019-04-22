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
    public class db
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["wzdba"].ConnectionString);
        SqlConnection conmst = new SqlConnection(ConfigurationManager.ConnectionStrings["wzdbamaster"].ConnectionString);

        public List<getuser> login(user us)
        {
            SqlCommand com = new SqlCommand("Select * from [dbo].[USER] where (username='"+us.name+"' or email='"+us.name+"' ) and password='"+us.password+"'", con);
            var usersList = new List<getuser> { };

            try {
                con.Open();
                SqlDataReader rdr = com.ExecuteReader();

                while (rdr.Read())
                {
                    var ss = new getuser
                    {
                        id = Convert.ToInt32(rdr["id"]),
                        name = rdr["username"].ToString(),
                        email = rdr["email"].ToString(),
                        password = rdr["password"].ToString()
                    };
                    usersList.Add(ss);
                }
            }
            catch
            {
                var ss = new getuser
                {
                    id = -1,
                    name = "not connected",
                    password = "not connected"
                };
                usersList.Add(ss);
            }
            finally
            {
                con.Close();
            }
            return usersList;
        }

        public List<getuser> register(register us)
        {
            SqlCommand com = new SqlCommand("select_user", con);
            com.CommandType = CommandType.StoredProcedure;
            var usersList = new List<getuser> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            var err = rdr["error"].ToString();
            while (rdr.Read())
            {
                var ss = new getuser
                {
                    id = Convert.ToInt32(rdr["id"]),
                    name = rdr["username"].ToString(),
                    email = rdr["email"].ToString(),
                    password = rdr["password"].ToString()
                };
                usersList.Add(ss);
            }
            con.Close();
            return usersList;
            
        }

        public List<getuser> getUser()
        {
            SqlCommand com = new SqlCommand("select * from [dbo].[user]", con);
            var usersList = new List<getuser> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new getuser
                {
                    id = Convert.ToInt32(rdr["id"]),
                    name = rdr["username"].ToString(),
                    email = rdr["email"].ToString(),
                    password = rdr["password"].ToString()
                };
                usersList.Add(ss);
            }
            con.Close();
            return usersList;
        }

        //db master
        public List<connection> setLog(log db){

            var list = new List<connection> { };
            var txtMessages = "";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " " + e.Message;
            };
            SqlCommand cmd = new SqlCommand("INSERT INTO logs (username,branch, date_time, logs, descript) VALUES ('" + db.username + "','" + db.branch + "',GETDATE(),'" + db.logs + "','" + db.descript + "');", con);
            cmd.CommandTimeout = 500000;
            cmd.ExecuteReader();
            var ss = new connection
            {
                result = txtMessages,
            };
            list.Add(ss);
            return list;
        }

        public List<viewlog> getLog()
        {

            SqlCommand com = new SqlCommand("select * from [dbo].[logs] order by id desc", con);
            var usersList = new List<viewlog> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new viewlog
                {
                    id = rdr["id"].ToString(),
                    date_time = rdr["date_time"].ToString(),
                    username = rdr["username"].ToString(),
                    branch = rdr["branch"].ToString(),
                    logs = rdr["logs"].ToString(),
                    descript = rdr["descript"].ToString()
                };
                usersList.Add(ss);
            }
            con.Close();
            return usersList;
        }

    }
}