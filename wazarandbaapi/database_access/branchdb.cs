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
    public class branchdb
    {
        connec conection = new connec();

        public List<serverdata> getServers()
        {
            SqlConnection con = conection.serverChoice(ConfigurationManager.ConnectionStrings["wzdba"].ConnectionString);
            var query = "select * from server";
            SqlCommand com = new SqlCommand(query, con);
            var list = new List<serverdata> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new serverdata
                {
                    id = rdr["id"].ToString(),
                    dbName = rdr["brDbName"].ToString(),
                    branch = rdr["branch"].ToString(),
                    master = rdr["master"].ToString(),
                    msdb = rdr["msdb"].ToString(),
                    status = rdr["status"].ToString(),
                    connection = rdr["connection"].ToString(),
                    address = rdr["branch_address"].ToString(),
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<connection> checkConnection(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<connection> { };
            try
            {
                con.Open();

                var ss = new connection
                {
                    result = "Success",
                };
                list.Add(ss);
                return list;
            }
            catch (SqlException)
            {
                var ss = new connection
                {
                    result = "ERROR",
                };
                list.Add(ss);
                return list;
            }
        }
        public List<connection> setConnection(changeServerConnection db)
        {
            SqlConnection con = conection.serverChoice(ConfigurationManager.ConnectionStrings["wzdba"].ConnectionString);
            var list = new List<connection> { };
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("update server set connection =" + db.connection + " where id=" + db.id, con);
                cmd.CommandTimeout = 500000;
                cmd.ExecuteReader();
                var ss = new connection
                {
                    result = "sucess",
                };
                list.Add(ss);
                return list;
            }
            catch (SqlException)
            {
                var ss = new connection
                {
                    result = "Error",
                };
                list.Add(ss);
                return list;
            }
        }


        public List<databaseDetailPhisics> brMonitor(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            SqlCommand com = new SqlCommand("sp_checkServerPhisic", con);
            com.Parameters.AddWithValue("@dbname", db.dbName);
            com.CommandType = CommandType.StoredProcedure;
            
            var usersList = new List<databaseDetailPhisics> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            if ((con.State & ConnectionState.Open) <= 0)
            {
                var ss = new databaseDetailPhisics
                {
                    serverName = "error",
                    dbName = "error",
                    fileName = "error",
                    type = "error",
                    path = "error",
                    fileSize = "error",
                    fileUsed = "error",
                    fileFree = "error",
                    fileFreePercentage = "error",
                    autoGrowth = "error",
                    drive = "error",
                    driveTotalVolume = "error",
                    driveSpaceVolume = "error",
                    driveSpacePercentage = "error"
                };
                usersList.Add(ss);
                con.Close();

            }
            else
            {
                while (rdr.Read())
                {
                    var ss = new databaseDetailPhisics
                    {
                        serverName = rdr["Server"].ToString(),
                        dbName = rdr["Database"].ToString(),
                        fileName = rdr["File Name"].ToString(),
                        type = rdr["Type"].ToString(),
                        path = rdr["Path"].ToString(),
                        fileSize = rdr["File Size"].ToString(),
                        fileUsed = rdr["File Used Space"].ToString(),
                        fileFree = rdr["File Free Space"].ToString(),
                        fileFreePercentage = rdr["% Free File Space"].ToString(),
                        autoGrowth = rdr["Autogrowth"].ToString(),
                        drive = rdr["volume_mount_point"].ToString(),
                        driveTotalVolume = rdr["Total volume size"].ToString(),
                        driveSpaceVolume = rdr["Free Space"].ToString(),
                        driveSpacePercentage = rdr["% Free"].ToString()
                    };
                    usersList.Add(ss);
                }
                con.Close();
            }
            return usersList;
        }
        public List<dbDetailLog> logMonitor(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            SqlCommand com = new SqlCommand("sp_checkLog", con);
            com.Parameters.AddWithValue("@dbname", db.dbName);
            com.CommandType = CommandType.StoredProcedure;
            

            var usersList = new List<dbDetailLog> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new dbDetailLog
                {
                    dbName = rdr["Database Name"].ToString(),
                    logSize = rdr["Log Size (MB)"].ToString(),
                    logSpace = rdr["Log Space Used (%)"].ToString(),
                    status = rdr["Status"].ToString(),
                    maxSize = rdr["Max Size"].ToString()
                };
                usersList.Add(ss);
            }
            con.Close();
            return usersList;
        }
        public List<dbStatus> checkStatus(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            SqlCommand com = new SqlCommand("SELECT (SELECT @@SERVERNAME)[Server Name], state_desc FROM SYS.DATABASES where name = '" + db.dbName + "'", con);
            var usersList = new List<dbStatus> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new dbStatus
                {
                    serverName = rdr["Server Name"].ToString(),
                    status = rdr["state_desc"].ToString()
                };
                usersList.Add(ss);
            }
            con.Close();
            return usersList;
        }
        public List<dbBackupMonitor> bkMonitoring(databasePostBkHistory db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var query = "SELECT TOP "+db.many+" s.database_name, m.physical_device_name, CAST(CAST(s.backup_size / 1000000 AS INT) AS VARCHAR(14)) + ' ' + 'MB' AS bkSize, CAST(DATEDIFF(second, s.backup_start_date, s.backup_finish_date) AS VARCHAR(4)) + ' ' + 'Seconds' TimeTaken, s.backup_start_date, CAST(s.first_lsn AS VARCHAR(50)) AS first_lsn, CAST(s.last_lsn AS VARCHAR(50)) AS last_lsn, CASE s.[type] WHEN 'D' THEN 'Full' WHEN 'I' THEN 'Differential' WHEN 'L' THEN 'Transaction Log'END AS BackupType, s.server_name, s.recovery_model FROM msdb.dbo.backupset s INNER JOIN msdb.dbo.backupmediafamily m ON s.media_set_id = m.media_set_id WHERE s.database_name = DB_NAME()  AND backup_start_date BETWEEN DATEADD(hh, -"+db.lastDate+", GETDATE()) AND GETDATE() ORDER BY backup_start_date desc";
            SqlCommand com = new SqlCommand(query, con);
            var list = new List<dbBackupMonitor> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new dbBackupMonitor
                {
                    dbName = rdr["database_name"].ToString(),
                    path = rdr["physical_device_name"].ToString(),
                    size = rdr["bkSize"].ToString(),
                    time = rdr["TimeTaken"].ToString(),
                    bkdate = rdr["backup_start_date"].ToString(),
                    firstlsn = rdr["first_lsn"].ToString(),
                    lastlsn = rdr["last_lsn"].ToString(),
                    bktype = rdr["BackupType"].ToString(),
                    serverName = rdr["server_name"].ToString(),
                    recModel = rdr["recovery_model"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<dbCreateDate> dbCreateDate(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var query = "select name, crdate from sys.sysdatabases Where [name]='"+db.dbName+"'";
            SqlCommand com = new SqlCommand(query, con);
            var list = new List<dbCreateDate> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new dbCreateDate
                {
                    date = rdr["crdate"].ToString(),
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<sqlcompatiblity> setSqlComp(databasePostFull db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            SqlConnection con2 = conection.serverChoice(db.master);
            var query = "ALTER DATABASE [" + db.dbName + "] SET COMPATIBILITY_LEVEL = 100";
            var query2 = "select name, compatibility_level from sys.databases;";
            SqlCommand com = new SqlCommand(query, con);
            SqlCommand com2 = new SqlCommand(query2, con2);
            var list = new List<sqlcompatiblity> { };
            con.Open();
            con2.Open();
            SqlDataReader rdr = com2.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new sqlcompatiblity
                {
                    dbName = rdr["name"].ToString(),
                    compLevel = rdr["compatibility_level"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            con2.Close();
            return list;
        }
        public List<sqlcompatiblity> getSqlComp(databasePostFull db)
        {
            SqlConnection con2 = conection.serverChoice(db.master);
            var query2 = "select name, compatibility_level from sys.databases";
            SqlCommand com2 = new SqlCommand(query2, con2);
            var list = new List<sqlcompatiblity> { };
            con2.Open();
            SqlDataReader rdr = com2.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new sqlcompatiblity
                {
                    dbName = rdr["name"].ToString(),
                    compLevel = rdr["compatibility_level"].ToString()
                };
                list.Add(ss);
            }
            con2.Close();
            return list;
        }
        public List<phisicStatus> phisicMonitor(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var query = "SELECT * FROM [dbo].[ufn_LogicalDiskDrives]()";
            SqlCommand com = new SqlCommand(query, con);
            var list = new List<phisicStatus> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new phisicStatus
                {
                    drive = rdr["DriveLetter"].ToString(),
                    name = rdr["VolumeName"].ToString(),
                    fileSystem = rdr["FileSystem"].ToString(),
                    totalSize = rdr["TotalSize"].ToString(),
                    freeSize = rdr["FreeSpace"].ToString(),
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<checkDB> ckDB(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<checkDB> { };
            var txtMessages ="";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " <br>" + e.Message;
            };
            SqlCommand cmd = new SqlCommand("DBCC CHECKDB", con);
            cmd.CommandTimeout = 500000;
            cmd.ExecuteReader();
            var ss = new checkDB
            {
                msg = txtMessages,
            };
            list.Add(ss);
            return list;
        }
        public List<shrinkLog> shrinkLog(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var query = "alter database sbtcdb set recovery simple;checkpoint;checkpoint;dbcc shrinkfile('sbtcdb_log',100);alter database sbtcdb set recovery full;";
            SqlCommand com = new SqlCommand(query, con);
            var list = new List<shrinkLog> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new shrinkLog
                {
                    DbId = rdr["DbId"].ToString(),
                    FileId = rdr["fileid"].ToString(),
                    currentSize = rdr["CurrentSize"].ToString(),
                    minSize = rdr["MinimumSize"].ToString(),
                    usedPage = rdr["UsedPages"].ToString(),
                    estPage = rdr["EstimatedPages"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<connection> backupDB(backupPost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<connection> { };
            var txtMessages = "";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " " + e.Message;
            };

            SqlCommand cmd = new SqlCommand("BACKUP " + db.bktype + " [" + db.dbname + "] TO DISK = '" + db.bkpath + "' WITH " + db.retaindays + db.format + db.init + db.nounloud + db.checksum + "NAME = N'" + db.bkname + "' ", con);
            cmd.CommandTimeout = 500000;
            cmd.ExecuteReader();
            var ss = new connection
            {
                result = txtMessages,
            };
            list.Add(ss);
            return list;
        }
        public List<connection> bkverify(bkverifyPost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<connection> { };
            var txtMessages = "";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " " + e.Message;
            };

            SqlCommand cmd = new SqlCommand("RESTORE VERIFYONLY FROM DISK = '"+db.path+"'", con);
            cmd.CommandTimeout = 500000;
            cmd.ExecuteReader();
            var ss = new connection
            {
                result = txtMessages,
            };
            list.Add(ss);
            return list;
        }
        public List<dbGeneral> dbGeneral(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<dbGeneral> { };
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT name, database_id, source_database_id, create_date, compatibility_level, collation_name, user_access_desc, state_desc, snapshot_isolation_state_desc, recovery_model_desc, page_verify_option_desc, log_reuse_wait_desc, log_reuse_wait_desc FROM sys.databases where name = '"+db.dbName+"' ", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new dbGeneral
                {
                    name = rdr["name"].ToString(),
                    database_id = rdr["database_id"].ToString(),
                    source_database_id = rdr["source_database_id"].ToString(),
                    create_date = rdr["create_date"].ToString(),
                    compatibility_level = rdr["compatibility_level"].ToString(),
                    collation_name = rdr["collation_name"].ToString(),
                    user_access_desc = rdr["user_access_desc"].ToString(),
                    state_desc = rdr["state_desc"].ToString(),
                    snapshot_isolation_state_desc = rdr["snapshot_isolation_state_desc"].ToString(),
                    recovery_model_desc = rdr["recovery_model_desc"].ToString(),
                    page_verify_option_desc = rdr["page_verify_option_desc"].ToString(),
                    log_reuse_wait_desc = rdr["log_reuse_wait_desc"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<logSize> logsize(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<logSize> { };
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT (size * 8.0)/1024.0 AS size_in_mb, CASE WHEN max_size = -1 THEN 9999999 ELSE (max_size * 8.0)/1024.0 END AS max_size_in_mb FROM SBTCDB.sys.database_files WHERE data_space_id = 0;", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new logSize
                {
                    size = rdr["size_in_mb"].ToString(),
                    maxsize = rdr["max_size_in_mb"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<ckServiceServer> checkService(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var MSSQLServer = "";
            var SQLServerAgent = "";
            var msdtc = "";
            var sqlbrowser = "";
            var MSSQLServerOLAPService = "";

            var list = new List<ckServiceServer> { };
            con.Open();

            SqlCommand cmd = new SqlCommand("EXEC xp_servicecontrol N'querystate',N'MSSQLServer';", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                MSSQLServer = rdr["Current Service State"].ToString();
            }
            con.Close();
            con.Open();

            SqlCommand cmd2 = new SqlCommand("EXEC xp_servicecontrol N'querystate',N'SQLServerAGENT';", con);
            SqlDataReader rdr2 = cmd2.ExecuteReader();
            while (rdr2.Read())
            {
                SQLServerAgent = rdr2["Current Service State"].ToString();
            }


            con.Close();
            con.Open();

            SqlCommand cmd3 = new SqlCommand("EXEC xp_servicecontrol N'querystate',N'msdtc';", con);
            SqlDataReader rdr3 = cmd3.ExecuteReader();
            while (rdr3.Read())
            {
                msdtc = rdr3["Current Service State"].ToString();
            }

            con.Close();
            con.Open();

            SqlCommand cmd4 = new SqlCommand("EXEC xp_servicecontrol N'querystate',N'sqlbrowser';", con);
            SqlDataReader rdr4 = cmd4.ExecuteReader();
            while (rdr4.Read())
            {
                sqlbrowser = rdr4["Current Service State"].ToString();
            }
            
            con.Close();
            con.Open();

            SqlCommand cmd5 = new SqlCommand("EXEC xp_servicecontrol N'querystate',N'MSSQLServerOLAPService';", con);
            SqlDataReader rdr5 = cmd5.ExecuteReader();
            while (rdr5.Read())
            {
                MSSQLServerOLAPService = rdr5["Current Service State"].ToString();
            }
            
            con.Close();
           

            var ss = new ckServiceServer
            {
                MSSQLServer = MSSQLServer,
                SQLServerAgent = SQLServerAgent,
                msdtc = msdtc,
                sqlbrowser = sqlbrowser,
                MSSQLServerOLAPService = MSSQLServerOLAPService,
            };
            list.Add(ss);

            return list;

        }
        public List<checkServerAgent> checkServerAgent(databasePostFull db)
        {


            SqlConnection con = conection.serverChoice(db.msdb);
            SqlCommand com = new SqlCommand("jobAgentFailed", con);
            com.CommandType = CommandType.StoredProcedure;


            var list = new List<checkServerAgent> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new checkServerAgent
                {
                    status = rdr["Status"].ToString(),
                    name = rdr["Job Name"].ToString(),
                    stepID = rdr["Step ID"].ToString(),
                    stepName = rdr["Step Name"].ToString(),
                    date = rdr["Start Date Time"].ToString(),
                    message = rdr["Message"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<checkLogFailed> checkLogFailed(databasePostFull db)
        {
            SqlConnection con = conection.serverChoice(db.msdb);
            SqlCommand com = new SqlCommand("sp_errorLog", con);
            com.CommandType = CommandType.StoredProcedure;

            var list = new List<checkLogFailed> { };
            con.Open();
            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new checkLogFailed
                {
                    logDate = rdr["LogDate"].ToString(),
                    message = rdr["Message"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<processConnect> processConnect(databasePostFull db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<processConnect> { };
            con.Open();
            var query = "SELECT spid, kpid, blocked, d.name, open_tran, status, hostname, cmd, login_time, loginame, net_library FROM sys.sysprocesses p INNER JOIN sys.databases d  on p.dbid=d.database_id";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new processConnect
                {
                    spid = rdr["spid"].ToString(),
                    kpid = rdr["kpid"].ToString(),
                    blocked = rdr["blocked"].ToString(),
                    name = rdr["name"].ToString(),
                    opentran = rdr["open_tran"].ToString(),
                    status = rdr["status"].ToString(),
                    hostname = rdr["hostname"].ToString(),
                    cmd = rdr["cmd"].ToString(),
                    logintime = rdr["login_time"].ToString(),
                    loginname = rdr["loginame"].ToString(),
                    netlibrary = rdr["net_library"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<batchSpeed> batchSpeed(databasePostFull db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<batchSpeed> { };
            con.Open();
            SqlCommand cmd = new SqlCommand("DECLARE @BRPS BIGINT SELECT @BRPS=cntr_value  FROM sys.dm_os_performance_counters WHERE counter_name LIKE 'Batch Requests/sec%' WAITFOR DELAY '000:00:10' SELECT (cntr_value-@BRPS)/10.0 AS 'Batch Requests/sec' FROM sys.dm_os_performance_counters WHERE counter_name LIKE 'Batch Requests/sec%'", con);
            cmd.CommandTimeout = 500000000;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new batchSpeed
                {
                    batch = rdr["Batch Requests/sec"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<checkBlockBy> checkBlockBy(databasePostFull db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<checkBlockBy> { };
            con.Open();
            SqlCommand cmd = new SqlCommand("sp_who2", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new checkBlockBy
                {
                    spid = rdr["SPID"].ToString(),
                    status = rdr["Status"].ToString(),
                    login = rdr["Login"].ToString(),
                    hostname = rdr["HostName"].ToString(),
                    blkby = rdr["BlkBy"].ToString(),
                    DBName = rdr["DBName"].ToString(),
                    command = rdr["Command"].ToString(),
                    CPUtime = rdr["CPUTime"].ToString(),
                    diskIO = rdr["diskIO"].ToString(),
                    lastBatch = rdr["lastBatch"].ToString(),
                    ProgramName = rdr["ProgramName"].ToString(),
                    REQID = rdr["REQUESTID"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<connection> killSpid(killSpidPost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<connection> { };
            var txtMessages = "";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " " + e.Message;
            };

            SqlCommand cmd = new SqlCommand("kill "+db.spid, con);
            cmd.CommandTimeout = 500000;
            cmd.ExecuteReader();
            var ss = new connection
            {
                result = txtMessages,
            };
            list.Add(ss);
            return list;
        }
        public List<connection> updateStats(killSpidPost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<connection> { };
            var txtMessages = "";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " " + e.Message;
            };

            SqlCommand cmd = new SqlCommand("sp_updatestats ", con);
            cmd.CommandTimeout = 500000;
            cmd.ExecuteReader();
            var ss = new connection
            {
                result = txtMessages,
            };
            list.Add(ss);
            return list;
        }
        public List<connection> createUserLogin(createUserPost db)
        {
            SqlConnection con = conection.serverChoice(db.master);
            var list = new List<connection> { };
            var txtMessages = "";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " " + e.Message;
            };

            SqlCommand cmd = new SqlCommand("CREATE LOGIN " + db.user + " WITH PASSWORD = '" + db.password + "' ", con);
            cmd.CommandTimeout = 500000;
            cmd.ExecuteReader();
            var ss = new connection
            {
                result = txtMessages,
            };
            list.Add(ss);
            return list;
        }
        public List<connection> dropUserLogin(addUserDb db)
        {
            SqlConnection con = conection.serverChoice(db.master);
            var list = new List<connection> { };
            var txtMessages = "";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " " + e.Message;
            };

            SqlCommand cmd = new SqlCommand("DROP LOGIN " + db.user, con);
            cmd.CommandTimeout = 500000;
            cmd.ExecuteReader();
            var ss = new connection
            {
                result = txtMessages,
            };
            list.Add(ss);
            return list;
        }
        public List<connection> editUserLogin(editUserDb db)
        {
            SqlConnection con = conection.serverChoice(db.master);
            var list = new List<connection> { };
            var txtMessages = "";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " " + e.Message;
            };

            SqlCommand cmd = new SqlCommand("ALTER LOGIN " + db.lastname + " WITH NAME = " + db.newuser + "; ALTER LOGIN " + db.newuser + " WITH PASSWORD = '" + db.newpassword + "';", con);
            cmd.CommandTimeout = 500000;
            cmd.ExecuteReader();
            var ss = new connection
            {
                result = txtMessages,
            };
            list.Add(ss);
            return list;
        }
        public List<viewUserLogin> viewUserLogin(databasePost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<viewUserLogin> { };
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT name, is_disabled, create_date, modify_date, default_database_name, default_language_name, credential_id, is_fixed_role FROM sys.server_principals WHERE TYPE = 'S'", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new viewUserLogin
                {
                    name = rdr["name"].ToString(),
                    is_disabled = rdr["is_disabled"].ToString(),
                    modify_date = rdr["modify_date"].ToString(),
                    default_database_name = rdr["default_database_name"].ToString(),
                    default_language_name = rdr["default_language_name"].ToString(),
                    credential_id = rdr["credential_id"].ToString(),
                    is_fixed_role = rdr["is_fixed_role"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<connection> addUserDB(addUserDb db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<connection> { };
            var txtMessages = "";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " " + e.Message;
            };

            SqlCommand cmd = new SqlCommand("EXEC sp_adduser '"+db.user+"'", con);
            cmd.CommandTimeout = 500000;
            cmd.ExecuteReader();
            var ss = new connection
            {
                result = txtMessages,
            };
            list.Add(ss);
            return list;
        }
        public List<viewUserServerRole> viewUserServerRole(addUserDb db)
        {
            SqlConnection con = conection.serverChoice(db.master);
            var list = new List<viewUserServerRole> { };
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT sys.server_role_members.role_principal_id, role.name AS RoleName, sys.server_role_members.member_principal_id, member.name AS MemberName  FROM sys.server_role_members  JOIN sys.server_principals AS role  ON sys.server_role_members.role_principal_id = role.principal_id  JOIN sys.server_principals AS member  ON sys.server_role_members.member_principal_id = member.principal_id  Where member.name ='" + db.user + "';", con);
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new viewUserServerRole
                {
                    role_principal_id = rdr["role_principal_id"].ToString(),
                    RoleName = rdr["RoleName"].ToString(),
                    member_principal_id = rdr["member_principal_id"].ToString(),
                    MemberName = rdr["MemberName"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<connection> addDropRoleMamber(addDropUserServerRole db)
        {
            
            if (db.addDelete)
            {
                SqlConnection con = conection.serverChoice(db.master);
                var list = new List<connection> { };
                var txtMessages = "";
                con.Open();
                con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
                {
                    txtMessages += " " + e.Message;
                };
                SqlCommand cmd = new SqlCommand("ALTER SERVER ROLE  " + db.type + "  ADD MEMBER "+db.user, con);
                cmd.CommandTimeout = 500000;
                cmd.ExecuteReader();
                var ss = new connection
                {
                    result = txtMessages,
                };
                list.Add(ss);
                return list;
            }
            else
            {
                SqlConnection con = conection.serverChoice(db.master);
                var list = new List<connection> { };
                var txtMessages = "";
                con.Open();
                con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
                {
                    txtMessages += " " + e.Message;
                };
                SqlCommand cmd = new SqlCommand("ALTER SERVER ROLE  " + db.type + "  DROP MEMBER " + db.user, con);

                cmd.CommandTimeout = 500000;
                cmd.ExecuteReader();
                var ss = new connection
                {
                    result = txtMessages,
                };
                list.Add(ss);
                return list;
            }
           
        }

        public List<emailLog> emailLog(emailLogPost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<emailLog> { };
            var s = (db.listperpage * (db.page + 1));
            var d = (db.listperpage * db.page);
            var query = "select * from msdb.dbo.sysmail_allitems where send_request_date  ";
            if (db.lastDay == "")
            {
                query = "WITH NumberedMyTable AS(select *, ROW_NUMBER() OVER (ORDER BY send_request_date desc) AS RowNumber from msdb.dbo.sysmail_allitems ";
                query = query + "SELECT * FROM NumberedMyTable WHERE RowNumber BETWEEN " + d + " AND " + s;

            }
            else
            {
                query = "WITH NumberedMyTable AS(select *, ROW_NUMBER() OVER (ORDER BY send_request_date desc) AS RowNumber from msdb.dbo.sysmail_allitems where send_request_date   BETWEEN GETDATE()-" + db.lastDay + " AND GETDATE())";
                query = query +"SELECT * FROM NumberedMyTable WHERE RowNumber BETWEEN " + d + " AND " + s;

            }
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con); ;
           
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new emailLog
                {
                    mailitem_id = rdr["mailitem_id"].ToString(),
                    profile_id = rdr["profile_id"].ToString(),
                    recipients = rdr["recipients"].ToString(),
                    copy_recipients = rdr["copy_recipients"].ToString(),
                    blind_copy_recipients = rdr["blind_copy_recipients"].ToString(),
                    subject = rdr["subject"].ToString(),
                    body = rdr["body"].ToString(),
                    body_format = rdr["body_format"].ToString(),
                    importance = rdr["importance"].ToString(),
                    sensitivity = rdr["sensitivity"].ToString(),
                    file_attachments = rdr["file_attachments"].ToString(),
                    attachment_encoding = rdr["attachment_encoding"].ToString(),
                    query = rdr["query"].ToString(),
                    execute_query_database = rdr["execute_query_database"].ToString(),
                    attach_query_result_as_file = rdr["attach_query_result_as_file"].ToString(),
                    query_result_header = rdr["query_result_header"].ToString(),
                    query_result_width = rdr["query_result_width"].ToString(),
                    query_result_separator = rdr["query_result_separator"].ToString(),
                    exclude_query_output = rdr["exclude_query_output"].ToString(),
                    append_query_error = rdr["append_query_error"].ToString(),
                    send_request_date = rdr["send_request_date"].ToString(),
                    send_request_user = rdr["send_request_user"].ToString(),
                    sent_account_id = rdr["sent_account_id"].ToString(),
                    sent_status = rdr["sent_status"].ToString(),
                    sent_date = rdr["sent_date"].ToString(),
                    last_mod_date = rdr["last_mod_date"].ToString(),
                    last_mod_user = rdr["last_mod_user"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<emailLog> emailLogFailed(emailLogPost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<emailLog> { };
            var s = (db.listperpage * (db.page + 1));
            var d = (db.listperpage * db.page);
            var query = "select * from msdb.dbo.sysmail_allitems where send_request_date  ";
            if (db.lastDay == "")
            {
                query = "WITH NumberedMyTable AS(select *, ROW_NUMBER() OVER (ORDER BY send_request_date) AS RowNumber from msdb.dbo.sysmail_allitems WHERE  sent_status='failed'";
                query = query + "SELECT * FROM NumberedMyTable WHERE RowNumber BETWEEN " + d + " AND " + s;

            }
            else
            {
                query = "WITH NumberedMyTable AS(select *, ROW_NUMBER() OVER (ORDER BY send_request_date) AS RowNumber from msdb.dbo.sysmail_allitems where  sent_status='failed' and send_request_date   BETWEEN GETDATE()-" + db.lastDay + " AND GETDATE())";
                query = query +"SELECT * FROM NumberedMyTable WHERE RowNumber BETWEEN " + d + " AND " + s;

            }
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con); ;
           
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new emailLog
                {
                    mailitem_id = rdr["mailitem_id"].ToString(),
                    profile_id = rdr["profile_id"].ToString(),
                    recipients = rdr["recipients"].ToString(),
                    copy_recipients = rdr["copy_recipients"].ToString(),
                    blind_copy_recipients = rdr["blind_copy_recipients"].ToString(),
                    subject = rdr["subject"].ToString(),
                    body = rdr["body"].ToString(),
                    body_format = rdr["body_format"].ToString(),
                    importance = rdr["importance"].ToString(),
                    sensitivity = rdr["sensitivity"].ToString(),
                    file_attachments = rdr["file_attachments"].ToString(),
                    attachment_encoding = rdr["attachment_encoding"].ToString(),
                    query = rdr["query"].ToString(),
                    execute_query_database = rdr["execute_query_database"].ToString(),
                    attach_query_result_as_file = rdr["attach_query_result_as_file"].ToString(),
                    query_result_header = rdr["query_result_header"].ToString(),
                    query_result_width = rdr["query_result_width"].ToString(),
                    query_result_separator = rdr["query_result_separator"].ToString(),
                    exclude_query_output = rdr["exclude_query_output"].ToString(),
                    append_query_error = rdr["append_query_error"].ToString(),
                    send_request_date = rdr["send_request_date"].ToString(),
                    send_request_user = rdr["send_request_user"].ToString(),
                    sent_account_id = rdr["sent_account_id"].ToString(),
                    sent_status = rdr["sent_status"].ToString(),
                    sent_date = rdr["sent_date"].ToString(),
                    last_mod_date = rdr["last_mod_date"].ToString(),
                    last_mod_user = rdr["last_mod_user"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<connection> sendEmail(sendEmail db)
        {
            SqlConnection con = conection.serverChoice(db.branchmsdb);
            var list = new List<connection> { };
            var txtMessages = "";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " " + e.Message;
            };
            var query = "EXEC sp_send_dbmail @profile_name='"+db.profile_name+"',";
            query += "@recipients='"+db.recipients+"',";
            if (db.copy_recipients != "" && db.copy_recipients != null){
                query += "@copy_recipients = " + db.copy_recipients + ",";
            }
            if (db.blind_copy_recipients != "" && db.blind_copy_recipients != null)
            {
                query += "@blind_copy_recipients='" + db.blind_copy_recipients + "',";
            }
            query += "@subject='" + db.subject + "',";
            query += "@body='" + db.body + "',";
            if (db.file_attachments != "" && db.file_attachments != null)
            {
                query += "@file_attachments = '" + db.file_attachments + "',";
            }
            if (db.query != "" && db.query != null)
            {
                query += "@query = " + db.query + ",";
            }
            query += "@body_format="+db.body_format+";";


            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandTimeout = 500000;
            cmd.ExecuteReader();
            var ss = new connection
            {
                result = txtMessages,
            };
            list.Add(ss);
            return list;
        }
        public List<emailProfile> viewEmailProfile(emailProfilePost db)
        {
            SqlConnection con = conection.serverChoice(db.msdb);
            var list = new List<emailProfile> { };
            con.Open();
            var query = "sysmail_help_profileaccount_sp;";

            if (db.ids == "" || db.ids == null)
            {
               query = "sysmail_help_profileaccount_sp;";

            }
            else
            {
                query = "sysmail_help_profileaccount_sp "+db.ids+";";
            }

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandTimeout = 500000;
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new emailProfile
                {
                    profile_id = rdr["profile_id"].ToString(),
                    profile_name = rdr["profile_name"].ToString(),
                    account_id = rdr["account_id"].ToString(),
                    account_name = rdr["account_name"].ToString(),
                    sequence_number = rdr["sequence_number"].ToString()

                };
                list.Add(ss);
            }
            return list;
        }
        public List<totalEmailPage> totalEmailLogPage(emailLogPost db)
        {
            SqlConnection con = conection.serverChoice(db.branch);
            var list = new List<totalEmailPage> { };
            var query = "select count(*)/"+db.listperpage+" as total from msdb.dbo.sysmail_allitems where send_request_date   BETWEEN GETDATE()-"+db.lastDay+" AND GETDATE() ";
            
            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new totalEmailPage
                {
                    total = rdr["total"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<connection> hosynch(synchhobr db)
        {
            SqlConnection con = conection.serverChoice("ho");

            var list = new List<connection> { };
            var txtMessages = "";
            con.Open();
            con.InfoMessage += delegate(object sender, SqlInfoMessageEventArgs e)
            {
                txtMessages += " " + e.Message;
            };
            var procname= "";
            if (db.type == "Adjustment Price")
            {
                procname = "sp_syncadjustprice" + db.syncbranch;
            }
            else if (db.type == "Booking Price")
            {
                procname = "sp_syncadjustprice" + db.syncbranch;
            }
            
            SqlCommand com = new SqlCommand(procname, con);

            SqlDataReader rdr = com.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new connection
                {
                    result = txtMessages
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<memoryUsage> memoryusage(databasePostFull db)
        {
            SqlConnection con = conection.serverChoice(db.master);
            var list = new List<memoryUsage> { };
            var query = "select";
            query += " total_physical_memory_kb/1024 AS total_physical_memory_mb,";
            query +=  " available_physical_memory_kb/1024 AS available_physical_memory_mb,";
            query +=  " total_page_file_kb/1024 AS total_page_file_mb,";
            query +=  " available_page_file_kb/1024 AS available_page_file_mb,";
            query +=  " 100 - (100 * CAST(available_physical_memory_kb AS DECIMAL(18,3))/CAST(total_physical_memory_kb AS DECIMAL(18,3))) ";
            query +=  " AS 'Percentage_Used',";
            query +=  " system_memory_state_desc";
            query +=  " from  sys.dm_os_sys_memory;";

            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new memoryUsage
                {
                    total_physical_memory_mb = rdr["total_physical_memory_mb"].ToString(),
                    available_physical_memory_mb = rdr["available_physical_memory_mb"].ToString(),
                    total_page_file_mb = rdr["total_page_file_mb"].ToString(),
                    available_page_file_mb = rdr["available_page_file_mb"].ToString(),
                    Percentage_Used = rdr["Percentage_Used"].ToString(),
                    system_memory_state_desc = rdr["system_memory_state_desc"].ToString()

                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }
        public List<memoryCpuUsage> memorycpuUsage(databasePostFull db)
        {
            SqlConnection con = conection.serverChoice(db.master);
            var list = new List<memoryCpuUsage> { };
            var query = "DECLARE @memory_usage FLOAT, @cpu_usage FLOAT;";
            query += " SET @memory_usage = ( SELECT    (1.0 - ( available_physical_memory_kb / ( total_physical_memory_kb * 1.0 ) ))*100 memory_usage FROM      sys.dm_os_sys_memory );";
            query += " SET @cpu_usage = ( SELECT TOP ( 1 ) [CPU] / 100.0 AS [CPU_usage] FROM     ( SELECT    record.value('(./Record/@id)[1]', 'int') AS record_id , record.value('(./Record/SchedulerMonitorEvent/SystemHealth/ProcessUtilization)[1]', 'int') AS [CPU] FROM      ( SELECT    [timestamp] , CONVERT(XML, record) AS [record] FROM      sys.dm_os_ring_buffers WITH ( NOLOCK ) WHERE     ring_buffer_type = N'RING_BUFFER_SCHEDULER_MONITOR' AND record LIKE N'%<SystemHealth>%' ) AS x ) AS y ORDER BY record_id DESC );";
            query += " SELECT  @memory_usage [memory_usage] , @cpu_usage [cpu_usage];";
          

            con.Open();
            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                var ss = new memoryCpuUsage
                {
                    memory_usage = rdr["memory_usage"].ToString(),
                    CPU_usage = rdr["CPU_usage"].ToString()
                };
                list.Add(ss);
            }
            con.Close();
            return list;
        }

    }
}