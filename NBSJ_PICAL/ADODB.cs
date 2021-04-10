using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace NBSJ_PICAL
{
    public class IDataBase
    {
        #region  全局变量
          public string ConnectionString = "Server=127.0.0.1;Database=LGSJ;Trusted_Connection=yes;uid=sa;password=Yjs88291280;MultipleActiveResultSets=true;Integrated Security=True;Connect Timeout=900";
       // public string ConnectionString = "Server=172.168.12.36;Database=LGSJ;Trusted_Connection=yes;uid=sa;password=Yjs88291280;MultipleActiveResultSets=true;Integrated Security=True;Connect Timeout=900";

        //public string ConnectionString = "Server=10.202.11.1\\NGSJ;Database=NBSJ;Trusted_Connection=yes;uid=sa;password=Yjs88291280;MultipleActiveResultSets=true;Integrated Security=True;Connect Timeout=900";
        public static SqlConnection conn1;//定义一个OracleConnection类型的公共变量My_con，用于判断数据库是否连接成功
        public static SqlConnection conn2;//定义一个OracleConnection类型的公共变量My_con，用于判断数据库是否连接成功
        public static SqlConnection conn3;//定义一个OracleConnection类型的公共变量My_con，用于判断数据库是否连接成功
        public static SqlConnection conn4;//定义一个OracleConnection类型的公共变量My_con，用于判断数据库是否连接成功
        public static SqlConnection conn5;//定义一个OracleConnection类型的公共变量My_con，用于判断数据库是否连接成功
        public static SqlConnection conn6;//定义一个OracleConnection类型的公共变量My_con，用于判断数据库是否连接成功
        public static SqlConnection conn7;//定义一个OracleConnection类型的公共变量My_con，用于判断数据库是否连接成功

        public static SqlConnection conn_comm;//定义一个OracleConnection类型的公共变量My_con，用于判断数据库是否连接成功

        public SqlDataReader odr1;
        public SqlDataReader odr2;
        public SqlDataReader odr3;
        public SqlDataReader odr4;
        public SqlDataReader odr5;
        public SqlDataReader odr6;
        public SqlDataReader odr7;

        //public OracleDataReader odr_comm;


        public bool m_connect1;
        public bool m_connect2;
        public bool m_connect3;
        public bool m_connect4;
        public bool m_connect5;
        public bool m_connect6;
        public bool m_connect7;

        public bool m_connect_comm;

        //  public string ConnectionString = "user id=QM;data source=JTQM;password=admin";

     
        //public string ConnectionString1 = "user id=jt_bof_sys;data source=shougang;password=jtsdm"; //库2写连接串 

        public string Error_Mes;
        //public OracleCommand cmd;
        public static string dir = @"D:\QGQM\LOG_Process\";
        public static string spacename = "ADODB_COMM_QMProcess_#";
        public int row = 0;

        public string Invoke_Mes;
        public static string dir2 = @"D:\QGQM\LOG_Process\";
        public static string spacename2 = "Scheme";

        #endregion


        #region  建立数据库连接1
        public bool OpenMyConnection1()
        {
            m_connect1 = false;
            try
            {
                if (conn1 == null || conn1.State == ConnectionState.Closed)
                {
                    conn1 = new SqlConnection(ConnectionString);
                    conn1.Open();
                    m_connect1 = true;
                }
                else
                {
                    m_connect1 = true;
                }

            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------";
                logtxt(dir, spacename, Error_Mes);
                conn1 = null;
                m_connect1 = false;

            }

            return m_connect1;
        }


        #endregion


        #region  建立数据库连接2

        public bool OpenMyConnection2()
        {
            m_connect2 = false;
            try
            {
                if (conn2 == null || conn2.State == ConnectionState.Closed)
                {
                    conn2 = new SqlConnection(ConnectionString);
                    conn2.Open();
                    m_connect2 = true;
                }
                else
                {
                    m_connect2 = true;
                }

            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------";
                logtxt(dir, spacename, Error_Mes);
                conn2 = null;
                m_connect2 = false;

            }

            return m_connect2;
        }

        #endregion


        #region  建立数据库连接3

        public bool OpenMyConnection3()
        {
            m_connect3 = false;
            try
            {
                if (conn3 == null || conn3.State == ConnectionState.Closed)
                {
                    conn3 = new SqlConnection(ConnectionString);
                    conn3.Open();
                    m_connect3 = true;
                }
                else
                {
                    m_connect3 = true;
                }

            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------";
                logtxt(dir, spacename, Error_Mes);
                conn3 = null;
                m_connect3 = false;

            }

            return m_connect3;
        }

        #endregion


        #region  建立数据库连接4

        public bool OpenMyConnection4()
        {
            m_connect4 = false;
            try
            {
                if (conn4 == null || conn4.State == ConnectionState.Closed)
                {
                    conn4 = new SqlConnection(ConnectionString);
                    conn4.Open();
                    m_connect4 = true;
                }
                else
                {
                    m_connect4 = true;
                }

            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------";
                logtxt(dir, spacename, Error_Mes);
                conn4 = null;
                m_connect4 = false;

            }

            return m_connect4;
        }

        #endregion


        #region  建立数据库连接5

        public bool OpenMyConnection5()
        {
            m_connect5 = false;
            try
            {
                if (conn5 == null || conn5.State == ConnectionState.Closed)
                {
                    conn5 = new SqlConnection(ConnectionString);
                    conn5.Open();
                    m_connect5 = true;
                }
                else
                {
                    m_connect5 = true;
                }

            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------";
                logtxt(dir, spacename, Error_Mes);
                conn5 = null;
                m_connect5 = false;

            }

            return m_connect5;
        }

        #endregion


        #region  建立数据库连接6

        public bool OpenMyConnection6()
        {
            m_connect6 = false;
            try
            {
                if (conn6 == null || conn6.State == ConnectionState.Closed)
                {
                    conn6 = new SqlConnection(ConnectionString);
                    conn6.Open();
                    m_connect6 = true;
                }
                else
                {
                    m_connect6 = true;
                }

            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------";
                logtxt(dir, spacename, Error_Mes);
                conn6 = null;
                m_connect6 = false;

            }

            return m_connect6;
        }

        #endregion


        #region  建立数据库连接7

        public bool OpenMyConnection7()
        {
            m_connect7 = false;
            try
            {
                if (conn7 == null || conn7.State == ConnectionState.Closed)
                {
                    conn7 = new SqlConnection(ConnectionString);
                    conn7.Open();
                    m_connect7 = true;
                }
                else
                {
                    m_connect7 = true;
                }

            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------";
                logtxt(dir, spacename, Error_Mes);
                conn7 = null;
                m_connect7 = false;

            }

            return m_connect7;
        }

        #endregion

        #region  建立数据库连接COMM

        public bool OpenMyConnection_Comm()
        {
            m_connect_comm = false;
            try
            {

                // ConnectionString1 = "user id=rh;data source=sgailf-84;password=lev2"; //写连接串 

                if (conn_comm == null || conn_comm.State == ConnectionState.Closed)
                {
                    conn_comm = new SqlConnection(ConnectionString);
                    conn_comm.Open();
                    m_connect_comm = true;
                }

                else
                {
                    m_connect_comm = true;
                }

            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------";
                logtxt(dir, spacename, Error_Mes);
                conn_comm = null;
                m_connect_comm = false;

            }

            return m_connect_comm;
        }

        #endregion

        #region  关闭数据库连接1

        public void CloseMyConnection1()
        {

            if (conn1.State == ConnectionState.Open)   //判断是否打开与数据库的连接
            {
                conn1.Close();   //关闭数据库的连接
                conn1.Dispose();   //释放My_con变量的所有空间
                conn1 = null;
            }
        }
        #endregion


        #region  关闭数据库连接2

        public void CloseMyConnection2()
        {
            if (conn2.State == ConnectionState.Open)   //判断是否打开与数据库的连接
            {
                conn2.Close();   //关闭数据库的连接
                conn2.Dispose();   //释放My_con变量的所有空间
                conn2 = null;
            }
        }
        #endregion


        #region  关闭数据库连接3

        public void CloseMyConnection3()
        {
            if (conn3.State == ConnectionState.Open)   //判断是否打开与数据库的连接
            {
                conn3.Close();   //关闭数据库的连接
                conn3.Dispose();   //释放My_con变量的所有空间
                conn3 = null;
            }
        }
        #endregion


        #region  关闭数据库连接4

        public void CloseMyConnection4()
        {
            if (conn4.State == ConnectionState.Open)   //判断是否打开与数据库的连接
            {
                conn4.Close();   //关闭数据库的连接
                conn4.Dispose();   //释放My_con变量的所有空间
                conn4 = null;
            }
        }
        #endregion


        #region  关闭数据库连接5

        public void CloseMyConnection5()
        {
            if (conn5.State == ConnectionState.Open)   //判断是否打开与数据库的连接
            {
                conn5.Close();   //关闭数据库的连接
                conn5.Dispose();   //释放My_con变量的所有空间
                conn5 = null;
            }
        }
        #endregion


        #region  关闭数据库连接6

        public void CloseMyConnection6()
        {
            if (conn6.State == ConnectionState.Open)   //判断是否打开与数据库的连接
            {
                conn6.Close();   //关闭数据库的连接
                conn6.Dispose();   //释放My_con变量的所有空间
                conn6 = null;
            }
        }
        #endregion


        #region  关闭数据库连接7

        public void CloseMyConnection7()
        {
            if (conn7.State == ConnectionState.Open)   //判断是否打开与数据库的连接
            {
                conn7.Close();   //关闭数据库的连接
                conn7.Dispose();   //释放My_con变量的所有空间
                conn7 = null;
            }
        }
        #endregion


        #region  关闭数据库连接COMM

        public void CloseMyConnection_Comm()
        {
            if (conn_comm.State == ConnectionState.Open)   //判断是否打开与数据库的连接
            {
                conn_comm.Close();   //关闭数据库的连接
                conn_comm.Dispose();   //释放My_con变量的所有空间
                conn_comm = null;
            }
            else
            {

            }

        }
        #endregion


        #region 读取表中信息1

        public bool OpenMyRecordset1(string SQLstr)
        {

            try
            {
                //                OpenMyConnection1();  
                if (conn1.State == ConnectionState.Closed)
                {
                    OpenMyConnection1();
                }

                SqlCommand cmd = conn1.CreateCommand();
                cmd.CommandText = SQLstr;//sql语句  
                odr1 = cmd.ExecuteReader();//创建一个OracleDateReader对象 

                return true;

            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                return false;
                //CloseMyConnection1();

            }

        }

        #endregion


        #region 读取表中信息2

        public bool OpenMyRecordset2(string SQLstr)
        {

            try
            {
                //                 OpenMyConnection2();
                if (conn2.State == ConnectionState.Closed)
                {
                    OpenMyConnection2();
                }
                SqlCommand cmd = conn2.CreateCommand();
                cmd.CommandText = SQLstr;//sql语句  
                odr2 = cmd.ExecuteReader();//创建一个OracleDateReader对象  
                return true;
            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                return false;
                //CloseMyConnection2();
            }

        }

        #endregion


        #region 读取表中信息3

        public bool OpenMyRecordset3(string SQLstr)
        {

            try
            {
                //                OpenMyConnection3();
                if (conn3.State == ConnectionState.Closed)
                {
                    OpenMyConnection3();
                }
                SqlCommand cmd = conn3.CreateCommand();
                cmd.CommandText = SQLstr;//sql语句  
                odr3 = cmd.ExecuteReader();//创建一个OracleDateReader对象   

                return true;
            }
            catch (Exception ee3)
            {

                Error_Mes = ee3.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                return false;
                //CloseMyConnection3();
            }

        }

        #endregion


        #region 读取表中信息4

        public bool OpenMyRecordset4(string SQLstr)
        {

            try
            {
                //                OpenMyConnection4();
                if (conn4.State == ConnectionState.Closed)
                {
                    OpenMyConnection4();
                }
                SqlCommand cmd = conn4.CreateCommand();
                cmd.CommandText = SQLstr;//sql语句  
                odr4 = cmd.ExecuteReader();//创建一个OracleDateReader对象

                return true;
            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);

                return false;
                //CloseMyConnection4();
            }

        }

        #endregion


        #region 读取表中信息5

        public bool OpenMyRecordset5(string SQLstr)
        {

            try
            {
                //                OpenMyConnection4();
                if (conn5.State == ConnectionState.Closed)
                {
                    OpenMyConnection5();
                }
                SqlCommand cmd = conn5.CreateCommand();
                cmd.CommandText = SQLstr;//sql语句  
                odr5 = cmd.ExecuteReader();//创建一个OracleDateReader对象 

                return true;
            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);

                return false;
                // CloseMyConnection5();
            }

        }

        #endregion


        #region 读取表中信息6

        public bool OpenMyRecordset6(string SQLstr)
        {

            try
            {
                //                OpenMyConnection4();
                if (conn6.State == ConnectionState.Closed)
                {
                    OpenMyConnection6();
                }
                SqlCommand cmd = conn6.CreateCommand();
                cmd.CommandText = SQLstr;//sql语句  
                odr6 = cmd.ExecuteReader();//创建一个OracleDateReader对象 

                return true;
            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);

                return false;
                // CloseMyConnection6();
            }

        }

        #endregion


        #region 读取表中信息7

        public bool OpenMyRecordset7(string SQLstr)
        {

            try
            {
                //                OpenMyConnection4();
                if (conn7.State == ConnectionState.Closed)
                {
                    OpenMyConnection7();
                }
                SqlCommand cmd = conn7.CreateCommand();
                cmd.CommandText = SQLstr;//sql语句  
                odr7 = cmd.ExecuteReader();//创建一个OracleDateReader对象 

                return true;
            }
            catch (Exception ee)
            {

                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);

                return false;
                // CloseMyConnection6();
            }

        }

        #endregion















        #region  关闭记录集1

        public void CloseMyRecordset1()
        {
            try
            {
                if (conn1.State == ConnectionState.Open)
                {
                    if (odr1 != null)
                    {
                        odr1.Close();
                        odr1.Dispose();
                        odr1 = null;

                    }

                }
            }
            catch (System.Exception ee)
            {
                if (odr1 != null)
                {
                    odr1.Dispose();
                }
                Error_Mes = ee.Message;
                logtxt(dir, spacename, Error_Mes);
            }


        }
        #endregion


        #region  关闭记录集2

        public void CloseMyRecordset2()
        {
            try
            {
                if (conn2.State == ConnectionState.Open)
                {
                    if (odr2 != null)
                    {
                        odr2.Close();
                        odr2.Dispose();
                        odr2 = null;
                    }

                }
            }
            catch (System.Exception ee)
            {
                if (odr2 != null)
                {
                    odr2.Dispose();
                }

                Error_Mes = ee.Message;
                logtxt(dir, spacename, Error_Mes);
            }


        }
        #endregion


        #region  关闭记录集3

        public void CloseMyRecordset3()
        {
            try
            {
                if (conn3.State == ConnectionState.Open)
                {
                    if (odr3 != null)
                    {
                        odr3.Close();
                        odr3.Dispose();
                        odr3 = null;
                    }

                }
            }
            catch (System.Exception ee)
            {
                if (odr3 != null)
                {
                    odr3.Dispose();
                }
                Error_Mes = ee.Message;
                logtxt(dir, spacename, Error_Mes);
            }


        }
        #endregion


        #region  关闭记录集4

        public void CloseMyRecordset4()
        {
            try
            {
                if (conn4.State == ConnectionState.Open)
                {
                    if (odr4 != null)
                    {
                        odr4.Close();
                        odr4.Dispose();
                        odr4 = null;
                    }

                }
            }
            catch (System.Exception ee)
            {
                if (odr4 != null)
                {
                    odr4.Dispose();
                }
                Error_Mes = ee.Message;
                logtxt(dir, spacename, Error_Mes);
            }


        }
        #endregion


        #region  关闭记录集5

        public void CloseMyRecordset5()
        {
            try
            {
                if (conn5.State == ConnectionState.Open)
                {
                    if (odr5 != null)
                    {
                        odr5.Close();
                        odr5.Dispose();
                        odr5 = null;
                    }

                }
            }
            catch (System.Exception ee)
            {
                if (odr5 != null)
                {
                    odr5.Dispose();
                }
                Error_Mes = ee.Message;
                logtxt(dir, spacename, Error_Mes);
            }


        }
        #endregion


        #region  关闭记录集6

        public void CloseMyRecordset6()
        {
            try
            {
                if (conn6.State == ConnectionState.Open)
                {
                    if (odr6 != null)
                    {
                        odr6.Close();
                        odr6.Dispose();
                        odr6 = null;
                    }

                }
            }
            catch (System.Exception ee)
            {
                if (odr6 != null)
                {
                    odr6.Dispose();
                }
                Error_Mes = ee.Message;
                logtxt(dir, spacename, Error_Mes);
            }


        }
        #endregion


        #region  关闭记录集7

        public void CloseMyRecordset7()
        {
            try
            {
                if (conn7.State == ConnectionState.Open)
                {
                    if (odr7 != null)
                    {
                        odr7.Close();
                        odr7.Dispose();
                        odr7 = null;
                    }

                }
            }
            catch (System.Exception ee)
            {
                if (odr7 != null)
                {
                    odr7.Dispose();
                }
                Error_Mes = ee.Message;
                logtxt(dir, spacename, Error_Mes);
            }


        }
        #endregion




        #region 查询获取表中行数1

        public int Recordcount1(string SQLstr)
        {
            int record = 0;

            try
            {
                if (conn1.State == ConnectionState.Open)
                {

                    SqlDataAdapter myDa = new SqlDataAdapter();
                    myDa.SelectCommand = new SqlCommand(SQLstr, conn1);
                    DataTable myDt = new DataTable();
                    myDa.Fill(myDt);
                    record = myDt.Rows.Count;
                    return record;
                }
                else
                    return 0;

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                //CloseMyConnection1();
                return 0;


            }
        }


        #endregion


        #region 查询获取表中行数2

        public int Recordcount2(string SQLstr)
        {
            int record = 0;

            try
            {
                if (conn2.State == ConnectionState.Open)
                {

                    SqlDataAdapter myDa = new SqlDataAdapter();
                    myDa.SelectCommand = new SqlCommand(SQLstr, conn2);
                    DataTable myDt = new DataTable();
                    myDa.Fill(myDt);
                    record = myDt.Rows.Count;
                    return record;
                }
                else
                    return 0;

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                //CloseMyConnection2();
                return 0;


            }
        }


        #endregion


        #region 查询获取表中行数3

        public int Recordcount3(string SQLstr)
        {
            int record = 0;

            try
            {
                if (conn3.State == ConnectionState.Open)
                {
                    SqlDataAdapter myDa = new SqlDataAdapter();
                    myDa.SelectCommand = new SqlCommand(SQLstr, conn3);
                    DataTable myDt = new DataTable();
                    myDa.Fill(myDt);
                    record = myDt.Rows.Count;

                    return record;
                }
                else
                    return 0;

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                //CloseMyConnection3();
                return 0;

            }
        }


        #endregion


        #region 查询获取表中行数4

        public int Recordcount4(string SQLstr)
        {
            int record = 0;

            try
            {
                if (conn4.State == ConnectionState.Open)
                {
                    SqlDataAdapter myDa = new SqlDataAdapter();
                    myDa.SelectCommand = new SqlCommand(SQLstr, conn4);
                    DataTable myDt = new DataTable();
                    myDa.Fill(myDt);
                    record = myDt.Rows.Count;
                    return record;
                }
                else
                    return 0;

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                //CloseMyConnection4();
                return 0;


            }
        }


        #endregion


        #region 查询获取表中行数5

        public int Recordcount5(string SQLstr)
        {
            int record = 0;

            try
            {
                if (conn5.State == ConnectionState.Open)
                {
                    SqlDataAdapter myDa = new SqlDataAdapter();
                    myDa.SelectCommand = new SqlCommand(SQLstr, conn5);
                    DataTable myDt = new DataTable();
                    myDa.Fill(myDt);
                    record = myDt.Rows.Count;
                    return record;
                }
                else
                    return 0;

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                //CloseMyConnection5();
                return 0;


            }
        }


        #endregion


        #region 查询获取表中行数6

        public int Recordcount6(string SQLstr)
        {
            int record = 0;

            try
            {
                if (conn6.State == ConnectionState.Open)
                {
                    SqlDataAdapter myDa = new SqlDataAdapter();
                    myDa.SelectCommand = new SqlCommand(SQLstr, conn6);
                    DataTable myDt = new DataTable();
                    myDa.Fill(myDt);
                    record = myDt.Rows.Count;
                    return record;
                }
                else
                    return 0;

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                //CloseMyConnection6();
                return 0;


            }
        }


        #endregion


        #region 查询获取表中行数7

        public int Recordcount7(string SQLstr)
        {
            int record = 0;

            try
            {
                if (conn7.State == ConnectionState.Open)
                {
                    SqlDataAdapter myDa = new SqlDataAdapter();
                    myDa.SelectCommand = new SqlCommand(SQLstr, conn7);
                    DataTable myDt = new DataTable();
                    myDa.Fill(myDt);
                    record = myDt.Rows.Count;
                    return record;
                }
                else
                    return 0;

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                //CloseMyConnection6();
                return 0;


            }
        }


        #endregion

        public void Recordex(string SQLstr)
        {
        }

        #region Operate表1  包括UPDATE/INSERT/DELETE

        public void ExecuteSQL1(string SQLstr)
        {

            try
            {
                if (OpenMyConnection1())
                {
                    SqlCommand cmd = conn1.CreateCommand();
                    cmd.CommandTimeout = 180;
                    cmd.CommandText = SQLstr;//sql语句 
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                CloseMyConnection1();

            }

        }
        #endregion


        #region Operate表2   包括UPDATE/INSERT/DELETE

        public void ExecuteSQL2(string SQLstr)
        {

            try
            {
                if (OpenMyConnection2())
                {
                    SqlCommand cmd = conn2.CreateCommand();
                    cmd.CommandTimeout = 180;
                    cmd.CommandText = SQLstr;//sql语句 
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                CloseMyConnection2();

            }

        }

        #endregion


        #region Operate表3   包括UPDATE/INSERT/DELETE

        public void ExecuteSQL3(string SQLstr)
        {

            try
            {
                if (OpenMyConnection3())
                {
                    SqlCommand cmd = conn3.CreateCommand();
                    cmd.CommandTimeout = 180;
                    cmd.CommandText = SQLstr;//sql语句 
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                CloseMyConnection3();

            }

        }

        #endregion


        #region Operate表4   包括UPDATE/INSERT/DELETE

        public void ExecuteSQL4(string SQLstr)
        {

            try
            {
                if (OpenMyConnection4())
                {
                    SqlCommand cmd = conn4.CreateCommand();
                    cmd.CommandTimeout = 180;
                    cmd.CommandText = SQLstr;//sql语句 
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                CloseMyConnection4();

            }

        }


        #endregion


        #region Operate表5   包括UPDATE/INSERT/DELETE

        public void ExecuteSQL5(string SQLstr)
        {

            try
            {
                if (OpenMyConnection5())
                {
                    SqlCommand cmd = conn5.CreateCommand();
                    cmd.CommandTimeout = 180;
                    cmd.CommandText = SQLstr;//sql语句 
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                CloseMyConnection5();

            }

        }


        #endregion


        #region Operate表6   包括UPDATE/INSERT/DELETE

        public void ExecuteSQL6(string SQLstr)
        {

            try
            {
                if (OpenMyConnection6())
                {
                    SqlCommand cmd = conn6.CreateCommand();
                    cmd.CommandTimeout = 180;
                    cmd.CommandText = SQLstr;//sql语句 
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                CloseMyConnection6();

            }

        }


        #endregion


        #region Operate表7   包括UPDATE/INSERT/DELETE

        public void ExecuteSQL7(string SQLstr)
        {

            try
            {
                if (OpenMyConnection7())
                {
                    SqlCommand cmd = conn7.CreateCommand();
                    cmd.CommandTimeout = 180;
                    cmd.CommandText = SQLstr;//sql语句 
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                CloseMyConnection7();

            }

        }


        #endregion




        #region Operate表COMM   包括UPDATE/INSERT/DELETE

        public void ExecuteSQL_Comm(string SQLstr)
        {

            try
            {

                if (OpenMyConnection_Comm())
                {
                    SqlCommand cmd = conn_comm.CreateCommand();
                    cmd.CommandTimeout = 180;
                    cmd.CommandText = SQLstr;//sql语句 
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ee)
            {
                Error_Mes = ee.Message + "------" + SQLstr;
                logtxt(dir, spacename, Error_Mes);
                CloseMyConnection_Comm();

            }

        }


        #endregion

        public void ExecuteSQL_Proc(string SQLstr, string desNO, int creatType, int rec)
        {

            if (OpenMyConnection_Comm())
            {
                try
                {
                    SqlCommand cmd = conn_comm.CreateCommand();
                    cmd.CommandTimeout = 180;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = SQLstr;//sql语句 

                    cmd.Parameters.Add("V_DES_NO", SqlDbType.NChar).Direction = ParameterDirection.Input;
                    cmd.Parameters["V_DES_NO"].Value = desNO;

                    cmd.Parameters.Add("V_CREATE_TYPE", SqlDbType.Int).Direction = ParameterDirection.Input;
                    cmd.Parameters["V_CREATE_TYPE"].Value = creatType;

                    cmd.Parameters.Add("V_HM_REC", SqlDbType.Int).Direction = ParameterDirection.Input;
                    cmd.Parameters["V_HM_REC"].Value = rec;

                    int rem = cmd.ExecuteNonQuery();

                    // MessageBox.Show("调用成功！");
                }
                catch (Exception ee)
                {
                    Error_Mes = ee.Message + "------" + SQLstr;
                    logtxt(dir, spacename, Error_Mes);

                    CloseMyConnection_Comm();
                }
            }
        }

        public void ExecuteSQL_Proc(string SQLstr, int creatType, int rec)
        {

            if (OpenMyConnection_Comm())
            {
                try
                {
                    SqlCommand cmd = conn_comm.CreateCommand();
                    cmd.CommandTimeout = 180;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = SQLstr;//sql语句 

                    cmd.Parameters.Add("V_CREATE_TYPE", SqlDbType.Int).Direction = ParameterDirection.Input;
                    cmd.Parameters["V_CREATE_TYPE"].Value = creatType;

                    cmd.Parameters.Add("V_HM_REC", SqlDbType.Int).Direction = ParameterDirection.Input;
                    cmd.Parameters["V_HM_REC"].Value = rec;

                    int rem = cmd.ExecuteNonQuery();

                    // MessageBox.Show("调用成功！");
                }
                catch (Exception ee)
                {
                    Error_Mes = ee.Message + "------" + SQLstr;
                    logtxt(dir, spacename, Error_Mes);

                    CloseMyConnection_Comm();
                }
            }
        }

        public int ExecuteSQL_Proc(string SQLstr, string desNO, int rec)
        {
            int stirTime = -1;
            if (OpenMyConnection_Comm())
            {
                try
                {
                    SqlCommand cmd = conn_comm.CreateCommand();
                    cmd.CommandTimeout = 180;


                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = SQLstr;//sql语句 

                    cmd.Parameters.Add("V_DES_NO", SqlDbType.NVarChar).Direction = ParameterDirection.Input;
                    cmd.Parameters["V_DES_NO"].Value = desNO;


                    cmd.Parameters.Add("V_HM_REC", SqlDbType.Int).Direction = ParameterDirection.Input;
                    cmd.Parameters["V_HM_REC"].Value = rec;

                    cmd.Parameters.Add("v_stir_time", SqlDbType.Int).Direction = ParameterDirection.Output;
                    stirTime = Convert.ToInt32(cmd.Parameters["v_stir_time"].Value);

                    int rem = cmd.ExecuteNonQuery();

                    Invoke_Mes = "V_DES_NO:" + desNO + ";  V_HM_REC:" + rec + ";  v_stir_time:" + stirTime;
                    logtxt(dir2, spacename2, Invoke_Mes);

                    Console.WriteLine("计算时间调用成功！");

                    return stirTime;

                }
                catch (Exception ee)
                {
                    Error_Mes = ee.Message + "------" + SQLstr;
                    logtxt(dir, spacename, Error_Mes);

                    CloseMyConnection_Comm();

                    return stirTime;
                }
            }
            return stirTime;
        }

        public int ExecuteSQL_Proc(string SQLstr, int rec)
        {
            if (OpenMyConnection_Comm())
            {
                try
                {
                    SqlCommand cmd = conn_comm.CreateCommand();
                    cmd.CommandTimeout = 180;

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = SQLstr;//sql语句 

                    cmd.Parameters.Add("V_DES_STATION_NO", SqlDbType.Int).Direction = ParameterDirection.Input;
                    cmd.Parameters["V_DES_STATION_NO"].Value = rec;

                    int rem = cmd.ExecuteNonQuery();

                    Invoke_Mes = "V_DES_STATION_NO:" + rec;
                    logtxt(dir2, spacename2, Invoke_Mes);

                    Console.WriteLine("计算时间调用成功！");
                }
                catch (Exception ee)
                {
                    Error_Mes = ee.Message + "------" + SQLstr;
                    logtxt(dir, spacename, Error_Mes);

                    CloseMyConnection_Comm();

                    return 1;
                }
            }
            return -1;
        }


        public DataTable GetDataTable(string CmdString)
        {
            try
            {

                SqlDataAdapter myDa = new SqlDataAdapter();
                myDa.SelectCommand = new SqlCommand(CmdString, conn1);
                DataTable myDt = new DataTable();
                myDa.Fill(myDt);

                return myDt;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Database_Copy(string sql, DataSet database)
        {
            try
            {

                SqlDataAdapter myDa = new SqlDataAdapter();
                myDa.SelectCommand = new SqlCommand(sql, conn1);
                DataTable myDt = new DataTable();
                myDa.Fill(myDt);

                DataSet db = new DataSet();

                for (int i = 0; i < myDt.DataSet.Tables[0].Rows.Count; i++)
                {
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        #region 错误日志
        public void logtxt(string logdir, string pro_name, string error_str)
        {

            string dsf = DateTime.Now.ToString("yyyy_MM_dd");
            string dsf_all = DateTime.Now.ToString("yyyy_MM_dd HH:mm:ss");
            string Fullname = logdir + pro_name + dsf + ".txt";
            FileInfo file1 = new FileInfo(Fullname);
            string ERR = dsf_all + "----" + error_str;
            try
            {
                StreamWriter sw = new StreamWriter(Fullname, true, Encoding.GetEncoding("gb2312"));
                sw.WriteLine(ERR);
                sw.Flush();
                sw.Close();
            }
            catch (IOException ioe)
            {
                //throw ioe;
            }
        }


        #endregion

    }
}
