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
       
        public static void DBConnection()
        {
            Debug.WriteLine("資料庫設定檔位置" + SystemParameterPath.iniFile);
            Dictionary<string, string> DBKey = new Dictionary<string, string>();
            DBKey.Add("ServerName", ExternalFile.GetProfileString("SERVER", "ServerName", "DataServer", SystemParameterPath.iniFile));
            DBKey.Add("Database", ExternalFile.GetProfileString("SERVER", "Database", "DataServer", SystemParameterPath.iniFile));
            DBKey.Add("LogId", ExternalFile.GetProfileString("SERVER", "LogId", "DataServer", SystemParameterPath.iniFile));
            DBKey.Add("LogPass", ExternalFile.GetProfileString("SERVER", "LogPass", "DataServer", SystemParameterPath.iniFile));

            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = DBKey["ServerName"];
            sqlBuilder.InitialCatalog = DBKey["Database"];
            sqlBuilder.UserID = DBKey["LogId"];
            sqlBuilder.Password = DBKey["LogPass"];
            sqlBuilder.ApplicationName = "MVCDataBase";
            SysConnectString = sqlBuilder.ToString();
        }
        public static void Log(String message)
        {
            new Task(() => LogMessage(message)).Start();
        }
        private static object lockerLogMessage = new object();

        private static void LogMessage(String message)
        {
            lock (lockerLogMessage)
            {
                DateTime time = DateTime.Now;
                String lsFile = $"{SystemParameterPath.AppPath}\\Log\\Log_{time.Day.ToString()}.log";
                Boolean bExpired = CreateLogFile(lsFile);
                if (!Directory.Exists($"{SystemParameterPath.AppPath}\\Log\\")) Directory.CreateDirectory($"{SystemParameterPath.AppPath}\\Log\\");
                using (StreamWriter sw = new StreamWriter(lsFile, !bExpired))
                {
                    sw.WriteLine(time.ToString("yyyy/MM/dd HH:mm:ss.ffff"));
                    sw.WriteLine(message);
                    sw.Flush();
                }
            }
        }
        private static Boolean CreateLogFile(String file)
        {
            Boolean lbExpired = true;
            String sLogCurrentDate = $"Log Date:{DateTime.Now.ToString("yyyy/MM/dd")}";
            if (File.Exists(file))
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    String sLine = sr.ReadLine();
                    if (!String.IsNullOrEmpty(sLine))
                    {
                        if (sLine.Length > 100)
                        {
                            if (sLine.Substring(0, 10) == DateTime.Now.ToString("yyyy/MM/dd"))
                            {
                                lbExpired = false;
                            }
                        }
                    }
                }
            }
            return lbExpired;
        }

    }
}
