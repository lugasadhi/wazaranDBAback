using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace wazarandbaapi.Models
{
    public class branchMonitoring{    }

    public class databasePostBkHistory
    {
        public string dbName { get; set; }
        public string branch { get; set; }
        public string many { get; set; }
        public string lastDate { get; set; }
    }
    public class databasePost
    {
        public string dbName { get; set; }
        public string branch { get; set; }

    }
    public class databasePostFull
    {
        public string dbName { get; set; }
        public string branch { get; set; }
        public string master { get; set; }
        public string msdb { get; set; }
    }
    public class serverdata
    {
        public string id { get; set; }
        public string dbName { get; set; }
        public string branch { get; set; }
        public string master { get; set; }
        public string msdb { get; set; }
        public string status { get; set; }
        public string connection { get; set; }
        public string address { get; set; }
    }
    public class changeServerConnection
    {
        public string id { get; set; }
        public string connection { get; set; }
    }
    public class changeServerStatus
    {
        public string id { get; set; }
        public string status { get; set; }
    }
    public class logSize
    {
        public string size {get;set;}
        public string maxsize {get;set;}
    }
    public class databaseDetailPhisics
    {
        public string serverName { get; set; }
        public string dbName { get; set; }
        public string fileName { get; set; }
        public string type { get; set; }
        public string path { get; set; }
        public string fileSize { get; set; }
        public string fileUsed { get; set; }
        public string fileFree { get; set; }
        public string fileFreePercentage { get; set; }
        public string autoGrowth { get; set; }
        public string drive { get; set; }
        public string driveTotalVolume { get; set; }
        public string driveSpaceVolume { get; set; }
        public string driveSpacePercentage { get; set; }
    }
    public class dbDetailLog
    {
        public string dbName{get;set;}
        public string logSize { get; set; }
        public string logSpace { get; set; }
        public string status { get; set; }
        public string maxSize { get; set; }
    }
    public class dbStatus
    {
        public string serverName { get; set; }
        public string status { get; set; }
    }
    public class dbBackupMonitor
    {
        public string dbName { get; set; }
        public string path { get; set; }
        public string size { get; set; }
        public string time { get; set; }
        public string bkdate { get; set; }
        public string firstlsn { get; set; }
        public string lastlsn{ get; set; }
        public string bktype { get; set; }
        public string serverName { get; set; }
        public string recModel { get; set; }
    }
    public class dbCreateDate
    {
        public string date { get; set; }
    }
    public class phisicStatus
    {
        public string drive { get; set; }
        public string name { get; set; }
        public string fileSystem { get; set; }
        public string totalSize { get; set; }
        public string freeSize { get; set; }
    }
    public class sqlcompatiblity
    {
        public string dbName { get; set; }
        public string compLevel { get; set; }
    }
    public class checkDB
    {
        public string msg { get;set;}
    }
    public class shrinkLog
    {
        public string DbId { get; set; }
        public string FileId { get; set; }
        public string currentSize { get; set; }
        public string minSize { get; set; }
        public string usedPage { get; set; }
        public string estPage { get; set; }
    }
    public class backupPost
    {
        public string branch { get; set; }
        public string dbname { get; set; }
        public string bkname { get; set; }
        public string bktype { get; set; }
        public string bkpath { get; set; }
        public string differential { get; set; }
        public string retaindays{ get; set; }
        public string format { get; set; }
        public string init { get; set; }
        public string nounloud { get; set; }
        public string checksum { get; set; }
    }
    public class bkverifyPost
    {
        public string dbName { get; set; }
        public string branch { get; set; }
        public string path { get; set; }
    }
    public class dbGeneral
    {
        public string name { get; set; }
        public string database_id { get; set; }
        public string source_database_id { get; set; }
        public string create_date { get; set; }
        public string compatibility_level { get; set; }
        public string collation_name { get; set; }
        public string user_access_desc { get; set; }
        public string state_desc { get; set; }
        public string snapshot_isolation_state_desc { get; set; }
        public string recovery_model_desc { get; set; }
        public string page_verify_option_desc { get; set; }
        public string log_reuse_wait_desc { get; set; }
    }
    public class ckServiceServer
    {
        public string MSSQLServer { get; set; }
        public string SQLServerAgent { get; set; }
        public string msdtc { get; set; }
        public string sqlbrowser { get; set; }
        public string MSSQLServerOLAPService { get; set; }
    }
    public class checkServerAgent
    {
        public string status { get; set; }
        public string name { get; set; }
        public string stepID { get; set; }
        public string stepName { get; set; }
        public string date { get; set; }
        public string message { get; set; }
    }
    public class checkLogFailed
    {
        public string logDate {get;set;}
        public string message {get;set;}
    }
    public class processConnect
    {
        public string spid { get; set; }
        public string kpid { get; set; }
        public string blocked { get; set; }
        public string name { get; set; }
        public string opentran { get; set; }
        public string status { get; set; }
        public string hostname { get; set; }
        public string cmd { get; set; }
        public string logintime { get; set; }
        public string loginname { get; set; }
        public string netlibrary { get; set; }
    }
    public class batchSpeed
    {
        public string batch { get; set; }
    }
    public class checkBlockBy
    {
        public string spid { get; set; }
        public string status { get; set; }
        public string login { get; set; }
        public string hostname { get; set; }
        public string blkby { get; set; }
        public string DBName { get; set; }
        public string command { get; set; }
        public string CPUtime { get; set; }
        public string diskIO { get; set; }
        public string lastBatch { get; set; }
        public string ProgramName { get; set; }
        public string REQID { get; set; }
    }
    public class killSpidPost
    {
        public string dbName { get; set; }
        public string branch { get; set; }
        public string spid { get; set; }
    }
    public class createUserPost
    {
        public string dbName { get; set; }
        public string master { get; set; }
        public string user { get; set; }
        public string password { get; set; }
    }
    public class addUserDb
    {
        public string dbName { get; set; }
        public string branch { get; set; }
        public string master { get; set; }
        public string user { get; set; }
    }
    public class editUserDb
    {
        public string dbName { get; set; }
        public string master { get; set; }
        public string lastname { get; set; }
        public string newuser { get; set; }
        public string newpassword { get; set; }
    }
    public class addDropUserServerRole
    {
        public string dbName { get; set; }
        public string master { get; set; }
        public string user { get; set; }
        public string type { get; set; }
        public Boolean addDelete { get; set; }
    }
    public class viewUserLogin
    {
        public string name { get; set; }
        public string is_disabled { get; set; } 
        public string create_date { get; set; }
        public string modify_date { get; set; }
        public string default_database_name { get; set; }
        public string default_language_name { get; set; }
        public string credential_id { get; set; }
        public string is_fixed_role { get; set; }
    }
    public class viewUserServerRole
    {
        public string role_principal_id { get; set; }
        public string RoleName { get; set; }
        public string member_principal_id { get; set; }
        public string MemberName { get; set; }
    }
   
    public class idBranch
    {
        public string salespointcd { get; set; }
        public string salespoint_nm { get; set; }
        public string salespoint_sn { get; set; }
        public string salespoint_typ { get; set; }
     
    }
    public class emailLogPost
    {
        public string dbName { get; set; }
        public string branch { get; set; }
        public string lastDay{ get; set; }
        public int listperpage { get; set; }
        public int page { get; set; }

    }
    public class emailLog
    {
        public string mailitem_id { get; set; }
        public string profile_id { get; set; }
        public string recipients { get; set; }
        public string copy_recipients { get; set; }
        public string blind_copy_recipients { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string body_format { get; set; }
        public string importance { get; set; }
        public string sensitivity { get; set; }
        public string file_attachments { get; set; }
        public string attachment_encoding { get; set; }
        public string query { get; set; }
        public string execute_query_database { get; set; }
        public string attach_query_result_as_file { get; set; }
        public string query_result_header { get; set; }
        public string query_result_width { get; set; }
        public string query_result_separator { get; set; }
        public string exclude_query_output { get; set; }
        public string append_query_error { get; set; }
        public string send_request_date { get; set; }
        public string send_request_user { get; set; }
        public string sent_account_id { get; set; }
        public string sent_status { get; set; }
        public string sent_date { get; set; }
        public string last_mod_date { get; set; }
        public string last_mod_user { get; set; }
    }
    public class sendEmail
    {
        public string branchmsdb { get; set; }
        public string profile_name { get; set; }
        public string recipients { get; set; }
        public string copy_recipients { get; set; }
        public string blind_copy_recipients { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string body_format { get; set; }
        public string query { get; set; }
        public string file_attachments { get; set; }
    }
    public class emailProfile
    {
        public string profile_id { get; set; }
        public string profile_name { get; set; }
        public string account_id { get; set; }
        public string account_name { get; set; }
        public string sequence_number { get; set; }
    }
    public class totalEmailPage
    {
        public string total { get; set; }
    }
    public class synchhobr
    {
        public string syncbranch { get; set; }
        public string type { get; set; }
    }
    public class emailProfilePost
    {
        public string msdb { get; set; }
        public string ids { get; set; }
    }
    public class memoryUsage
    {
        public string total_physical_memory_mb { get; set; }
        public string available_physical_memory_mb { get; set; }
        public string total_page_file_mb { get; set; }
        public string available_page_file_mb { get; set; }
        public string Percentage_Used { get; set; }
        public string system_memory_state_desc { get; set; }
    }
    public class memoryCpuUsage
    {
        public string memory_usage { get; set; }
        public string CPU_usage { get; set; }
    }
    public class getsmsoutbox
    {
        public string memory_usage { get; set; }
        public string CPU_usage { get; set; }
    }

}
