using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLog
{
    /// <summary>
    /// 日志类                    通过调用存储过程进行日志写入
    /// </summary>
    public class vLog:LLog
    {
        /// <summary>
        /// 日志类构造函数 
        /// 
        /// 无参数，只提供日志写入数据库表EventLog表中
        /// </summary>
        public vLog() { }

        /// <summary>
        /// 日志构造参数
        /// 带参数，提供日志写入数据库表EventLog表中且可以写入本地文件
        /// </summary>
        /// <param name="logpath">
        /// 本地文件日志路径
        /// </param>
        public vLog(string logpath)
        { 
            logBasePath = logpath;
            
        }
        public string connstring { get; set; }
        
       /// <summary>
       /// 日志写入数据库
       /// </summary>
       /// <param name="modelname">模型命名</param>
       /// <param name="funcname">方法命名</param>
       /// <param name="errorinfo">日志信息</param>
       /// <param name="errorcode">错误代码</param>
       /// <returns></returns>
        public bool WriteLog(string modelname,string funcname,string errorinfo,int errorcode)
        {
            bool bflag = false;
            using (var _conn = new SqlConnection(connstring))
            {

                try
                {
                    _conn.Open();

                    using (var _cmd = new SqlCommand())
                    {
                        try
                        {
                            _cmd.Connection = _conn;
                            _cmd.CommandText = "dbo.WriteLog";
                            _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            var param1 = _cmd.CreateParameter();
                            param1.Value = DateTime.Now;
                            param1.DbType = System.Data.DbType.DateTime;
                            param1.Direction = System.Data.ParameterDirection.Input;
                            param1.ParameterName = "@timedate";
                            var param2 = _cmd.CreateParameter();
                            param2.Value = modelname;
                            param2.DbType = System.Data.DbType.String;
                            param2.Direction = System.Data.ParameterDirection.Input;
                            param2.ParameterName = "@modelName";
                            var param3 = _cmd.CreateParameter();
                            param3.Value = funcname;
                            param3.DbType = System.Data.DbType.String;
                            param3.Direction = System.Data.ParameterDirection.Input;
                            param3.ParameterName = "@funcName";
                            var param4 = _cmd.CreateParameter();
                            param4.Value = errorinfo;
                            param4.DbType = System.Data.DbType.String;
                            param4.Direction = System.Data.ParameterDirection.Input;
                            param4.ParameterName = "@errorInfo";
                            var param5 = _cmd.CreateParameter();
                            param5.Value = errorcode;
                            param5.DbType = System.Data.DbType.Int32;
                            param5.Direction = System.Data.ParameterDirection.Input;
                            param5.ParameterName = "@errorCode";

                            _cmd.Parameters.Add(param1);
                            _cmd.Parameters.Add(param2);
                            _cmd.Parameters.Add(param3);
                            _cmd.Parameters.Add(param4);
                            _cmd.Parameters.Add(param5);

                            int rows = _cmd.ExecuteNonQuery();
                            string str = "调用存储过程WriteLog成功(影响行数:" + rows + " \n\r";
                            Console.WriteLine(str);
                            bflag = true;
                            //xlogx.writelog(str);
                        }
                        catch (Exception e)
                        {
                            string str = "调用存储过程WriteLog失败(" + e.Message + ") \n\r";
                            bflag = false;
                            Console.WriteLine(str);
                            //xlogx.writelog(str);
                        }
                        finally
                        {
                            _cmd.Dispose();

                        }
                    }

                }
                catch (Exception e)
                {
                    string str = "调用存储过程WriteLog失败(" + e.Message + ") \n\r";
                    Console.WriteLine(str);
                    //xlogx.writelog(str);
                }
                finally
                {
                    _conn.Close();
                    _conn.Dispose();
                }


            }

            return bflag;
        }
  


    }
    public class LLog
    {
        public string logBasePath { get; set; }
        private string GetName()
        {
            
            string strdt = DateTime.Now.ToShortDateString().Replace("/","-") + ".txt";
            return strdt;
        }
        private string GetLongTime()
        {
            DateTime _dt = DateTime.Now;
            string strdt = "\n\r" + _dt.ToShortDateString() + " " + _dt.ToLongTimeString()+"."+ _dt.Millisecond + "   ";
            return strdt;
        }
        /// <summary>
        /// 本地文件日志记录
        /// </summary>
        /// <param name="log">记录日志信息内容</param>
        /// <param name="flag">
        /// 1:warnning
        /// 0:ok
        /// -1:bad
        /// </param>
        /// <returns></returns>
        public bool writelog(string log,int flag)
        {
            try
            {
                
                if(!Directory.Exists(logBasePath))
                {
                    Directory.CreateDirectory(logBasePath);
                }
                string errType = flag == 0 ? "OK" : flag == 1 ? "WARNING" : "ERROR";
                string path = logBasePath + GetName();
                
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                byte[] bytes = new byte[4096];
                string outstr = GetLongTime() +"  "+errType+"  "+ log+"\n\r";
                byte[] csvstr = System.Text.Encoding.Default.GetBytes(outstr);
                fs.Write(csvstr, 0, csvstr.Length);
                fs.Flush();
                fs.Close();
                Console.WriteLine(outstr);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:" + e.Message);
                return false;
            }

        }
    }
}
