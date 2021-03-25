using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataBase
{
    public class DBSQL
    {
        private string m_dbs;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DBSQL() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectString">数据库连接字符串</param>
        public DBSQL(string connectString)
        {
            m_dbs = connectString;
        }

        public string ConnectString
        {
            get { return m_dbs; }
            set { m_dbs = value; }
        }

        /// <summary>
        /// 插入并获取ID
        /// </summary>
        /// <param name="connectString">数据库连接字符串</param>
        /// <param name="commandStr">SQL语句 包含获取ID的命令</param>
        /// <returns>新记录的ID</returns>
        public int ExecuteScalarInsert(string connectString, string commandStr)
        {
            string err = "";
            int ret = 0;
            if (string.IsNullOrEmpty(connectString))
            {
                return -1;
            }
            using (SqlConnection dbc = new SqlConnection(connectString))
            {
                SqlCommand insert = new SqlCommand(commandStr, dbc);

                try
                {
                    dbc.Open();
                    ret = Convert.ToInt32(insert.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
            }

            if (err.Length > 0)
            {
                return -1;
            }
            else
            {
                return ret;
            }
        }

        /// <summary>
        /// 插入并获取ID
        /// </summary>
        /// <param name="commandStr">SQL语句 包含获取ID的命令</param>
        /// <returns>新记录的ID</returns>
        public int ExecuteScalarInsert(string commandStr)
        {
            string err = "";
            int ret = 0;
            if (string.IsNullOrEmpty(m_dbs))
            {
                return -1;
            }
            using (SqlConnection dbc = new SqlConnection(m_dbs))
            {
                SqlCommand insert = new SqlCommand(commandStr, dbc);

                try
                {
                    dbc.Open();
                    ret = Convert.ToInt32(insert.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
            }

            if (err.Length > 0)
            {
                return -1;
            }
            else
            {
                return ret;
            }
        }

        /// <summary>
        /// 添加、删除、更新操作
        /// </summary>
        /// <param name="connectString">数据库连接字符串</param>
        /// <param name="commandstr">SQL语句</param>
        /// <returns>受影响的行数</returns>
        public int CommandExecuteNonQuery(string connectString, string commandstr)
        {
            if (string.IsNullOrEmpty(connectString) || string.IsNullOrEmpty(commandstr))
            {
                return -1;
            }
            string err = "";
            int result = 0;
            using (SqlConnection dbc = new SqlConnection(connectString))
            {
                SqlCommand command = new SqlCommand(commandstr, dbc);
                try
                {
                    dbc.Open();
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
            }

            if (err.Length > 0)
            {
                return -1;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 添加、删除、更新操作
        /// </summary>
        /// <param name="commandstr">SQL语句</param>
        /// <returns>受影响的行数</returns>
        public int CommandExecuteNonQuery(string commandstr)
        {
            if (string.IsNullOrEmpty(m_dbs) || string.IsNullOrEmpty(commandstr))
            {
                return -1;
            }
            string err = "";
            int result = 0;
            using (SqlConnection dbc = new SqlConnection(m_dbs))
            {
                SqlCommand command = new SqlCommand(commandstr, dbc);
                try
                {
                    dbc.Open();
                    result = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
            }

            if (err.Length > 0)
            {
                //throw new WebFaultException<SimpleException>(new SimpleException() { Message = err }, HttpStatusCode.InternalServerError);
                return -1;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="connectString">数据库连接字符串</param>
        /// <param name="selectstr">SQL语句</param>
        /// <returns>数据表</returns>
        public DataTable GetCommand(string connectString, string selectstr)
        {
            if (string.IsNullOrEmpty(connectString) || string.IsNullOrEmpty(selectstr))
            {
                return null;
            }
            DataTable table = new DataTable();

            string err = "";
            using (SqlConnection dbc = new SqlConnection(connectString))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(selectstr, dbc);
                    adapter.Fill(table);
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
            }

            if (err.Length > 0)
            {
                //throw new WebFaultException<SimpleException>(new SimpleException() { Message = err }, HttpStatusCode.InternalServerError);
                return null;
            }
            else
            {
                return table;
            }
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="selectstr">SQL语句</param>
        /// <returns>数据表</returns>
        public DataTable GetCommand(string selectstr)
        {
            if (string.IsNullOrEmpty(m_dbs))
            {
                return null;
            }
            DataTable table = new DataTable();
            string err = "";
            using (SqlConnection dbc = new SqlConnection(m_dbs))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand(selectstr, dbc);
                    adapter.Fill(table);
                }
                catch (Exception ex)
                {
                    err = ex.Message;
                }
            }

            if (err.Length > 0)
            {
                return null;
                //throw new WebFaultException<SimpleException>(new SimpleException() { Message = err }, HttpStatusCode.InternalServerError);
            }
            else
            {
                return table;
            }
        }

        /// <summary>
        /// 执行一个事务
        /// </summary>
        /// <param name="commands">事务中要执行的所有语句</param>
        /// <returns>事务是否成功执行</returns>
        public bool ExecuteTransaction(List<string> commands)
        {
            if (string.IsNullOrEmpty(m_dbs) || commands == null)
            {
                return false;
            }

            string err = "";
            bool ret = false;
            using (SqlConnection dbc = new SqlConnection(m_dbs))
            {
                dbc.Open();
                using (SqlTransaction transaction = dbc.BeginTransaction())
                {
                    try
                    {
                        foreach (string commandstr in commands)
                        {
                            SqlCommand command = new SqlCommand(commandstr, dbc);
                            command.Transaction = transaction;
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        err = ex.Message;
                    }
                }
            }

            if (err.Length > 0)
            {
                return false;
                //throw new WebFaultException<SimpleException>(new SimpleException() { Message = err }, HttpStatusCode.InternalServerError);
            }
            else
            {
                return ret;
            }
        }

        /// <summary>
        /// 执行一个事务
        /// </summary>
        /// <param name="connectString">数据库连接字符串</param>
        /// <param name="commands">事务中要执行的所有语句</param>
        /// <returns>事务是否成功执行</returns>
        public bool ExecuteTransaction(string connectString, List<string> commands)
        {
            if (string.IsNullOrEmpty(connectString) || commands == null)
            {
                return false;
            }

            string err = "";
            bool ret = false;
            using (SqlConnection dbc = new SqlConnection(connectString))
            {
                dbc.Open();
                using (SqlTransaction transaction = dbc.BeginTransaction())
                {
                    try
                    {
                        foreach (string commandstr in commands)
                        {
                            SqlCommand command = new SqlCommand(commandstr, dbc);
                            command.Transaction = transaction;
                            command.ExecuteNonQuery();
                        }
                        transaction.Commit();
                        ret = true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        err = ex.Message;
                    }
                }
            }

            if (err.Length > 0)
            {
                return false;
                //throw new WebFaultException<SimpleException>(new SimpleException() { Message = err }, HttpStatusCode.InternalServerError);
            }
            else
            {
                return ret;
            }
        }
    }
}