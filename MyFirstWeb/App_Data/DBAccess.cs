using Dapper;
using MyFirstWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseController
{
    public class DBAccess
    {
        string SysConnString = string.Empty;
        public DBAccess()
        {
            DBUtility.DBConnection();
            SysConnString = DBUtility.SysConnectString;

        }
        
        public bool DBOpen()
        {
            //if (sqlcon.State != ConnectionState.Open)
            //    sqlcon.Open();
            bool bRet = false;

            using (SqlConnection SysConn = new SqlConnection(SysConnString))
            {
                try
                {
                    SysConn.Open();
                    bRet = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return bRet;
        }
        public List<User> GetAccount()
        {
            using (SqlConnection SysConn = new SqlConnection(SysConnString))
            {
                try {
                    return SysConn.Query<User>("Select * from [User]").ToList();
                } catch (Exception ex)
                {
                    throw ex;
                }
                
            }
        }
        public bool SetAccount(User Acc)
        {
            using (SqlConnection SysConn = new SqlConnection(SysConnString))
            {
                try
                {
                    SysConn.Execute("Insert into [User] Values(@UserId,@UserName,@Password,@Email,@Address)", Acc);
                    return true;
                }
                catch (Exception ex)
                {                    
                    throw ex;
                    
                }

            }
        }
        public bool UpdateAccount(User Acc)
        {
            using (SqlConnection SysConn = new SqlConnection(SysConnString))
            {
                try
                {
                    SysConn.Execute("update [User] Set Password = @Password where UserName = @UserName and Email = @Email", Acc);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }
     
    

        public int StroreProcedure(string room)
        {
            try
            {
                // Bcoding Write CODE
                // Bcoding Write CODE2
                DynamicParameters parm = new DynamicParameters();
                //intput
                parm.Add("@Room", room, dbType: DbType.String);                
                //output
                parm.Add("@SeqNo", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                using (SqlConnection SysConn = new SqlConnection(SysConnString))
                {
                    SysConn.Execute("GetSequence", parm, commandType: CommandType.StoredProcedure);
                }
                return parm.Get<int>("@SeqNo");
            }
            catch 
            {
                return -1;
            }
        }
        

    }
}
