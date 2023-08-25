using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseController
{
    public class DBUtility
    {
        public static string SysConnectString = string.Empty;
        public static Dictionary<string, string> DBSetup()
        {            
           
            Debug.WriteLine("資料庫設定檔位置" + SystemParameterPath.iniFile);
            Dictionary<string, string> DBKey = new Dictionary<string, string>();
            DBKey.Add("ServerName", ExternalFile.GetProfileString("SERVER", "ServerName", "DataServer", SystemParameterPath.iniFile));
            DBKey.Add("Database", ExternalFile.GetProfileString("SERVER", "Database", "DataServer", SystemParameterPath.iniFile));
            DBKey.Add("LogId", ExternalFile.GetProfileString("SERVER", "LogId", "DataServer", SystemParameterPath.iniFile));
            DBKey.Add("LogPass", ExternalFile.GetProfileString("SERVER", "LogPass", "DataServer", SystemParameterPath.iniFile));
            return DBKey;
        }
        public static void DBConnection()
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = DBSetup()["ServerName"];
            sqlBuilder.InitialCatalog = DBSetup()["Database"];
            sqlBuilder.UserID = DBSetup()["LogId"];
            sqlBuilder.Password = DBSetup()["LogPass"];
            sqlBuilder.ApplicationName = "MVCDataBase";
            SysConnectString = sqlBuilder.ToString();
            //Debug.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory + "teste.ini"); ;
        }
        
    }
}
