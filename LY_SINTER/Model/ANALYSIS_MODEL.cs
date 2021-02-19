using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLog;

namespace LY_SINTER.Model
{
    class ANALYSIS_MODEL
    {
        public vLog _vLog { get; set; }
        DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        public ANALYSIS_MODEL()
        {
            _vLog = new vLog(".\\ANALYSIS_MODEL\\");
            _vLog.connstring = DataBase.ConstParameters.strCon;
        }
        /// <summary>
        /// 通过传入的数据组计算数据，
        /// _A = :计算数据组
        /// _FLAG ：1平均值  2标准偏差，3最大值，4最小值，5极差
        /// _B：小数位数
        /// </summary>
        /// <param name="_FLAG"></param>
        /// <returns></returns>
        public double Computer_MODEL(List<float> _A, int _FLAG,int _B)
        {
            try
            {
                if (_A.Count > 0 )
                {
                    float SUM_A = 0;//数据组和
                    for(int x = 0; x < _A.Count; x++)
                    {
                        SUM_A += _A[x];
                    }
                    if (_FLAG == 1)
                    {
                        return Math.Round( SUM_A / _A.Count, _B);
                    }
                    else if(_FLAG == 2)
                    {
                        float _A_AVG = SUM_A / _A.Count;
                        float _A_1 = 0;
                        int COUNT = -1;
                        for (int x = 0; x < _A.Count;x++)
                        {
                            _A_1 += _A[x] - _A_AVG;
                            COUNT = COUNT + 1;
                        }
                        return Math.Round(Math.Sqrt(_A_1 / COUNT),_B);
                    }
                    else if (_FLAG == 3)
                    {
                        float _GETMIN = _A[0];
                        for (int x = 1; x < _A.Count; x++)
                        {
                            if (_GETMIN < _A[x])
                                _GETMIN = _A[x];
                        }
                        return Math.Round(_GETMIN,_B);
                    }
                    else if (_FLAG == 4)
                    {
                        float _GETMax = _A[0];
                        for (int x = 1; x < _A.Count; x++)
                        {
                            if (_GETMax > _A[x])
                                _GETMax = _A[x];
                        }
                        return Math.Round(_GETMax,_B);
                    }
                    else if (_FLAG == 5)
                    {
                        float _GETMIN = _A[0];
                        float _GETMax = _A[0];
                        for (int x = 1; x < _A.Count; x++)
                        {
                            if (_GETMIN < _A[x])
                                _GETMIN = _A[x];
                            if (_GETMax > _A[x])
                                _GETMax = _A[x];
                        }
                        return Math.Round(_GETMax - _GETMIN, _B);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取烧结机号datatable
        /// </summary>
        /// <returns></returns>
        public DataTable _get_Data()
        {
            try
            {
                DataTable _data = new DataTable();
                _data.Columns.Add("Name");
                _data.Columns.Add("Values");

                DataRow row_now = _data.NewRow();
                row_now["Name"] = "3#烧结机";
                row_now["Values"] = 1;
                _data.Rows.Add(row_now);

                //DataRow row_now_1 = _data.NewRow();
                //row_now_1["Name"] = "2#烧结机";
                //row_now_1["Values"] = 2;
                //_data.Rows.Add(row_now_1);

                //DataRow row_now_2 = _data.NewRow();
                //row_now_2["Name"] = "全部";
                //row_now_2["Values"] = 3;
                //_data.Rows.Add(row_now_2);

                return _data;

            }
            catch
            {
                return null;
            }
           
        }

        /// <summary>
        /// 获取班级开始时间、结束时间
        /// _flag = 1当班 =2上班
        /// </summary>
        /// <param name="_flag"></param>
        /// <returns></returns>
        public Tuple<DateTime,DateTime> _CLASS_TIME(int _flag)
        {
            try
            {
                //当前系统时间
                DateTime time_now = DateTime.Now;
                if (_flag ==1)
                {
                    string sql_class_now = "select START_TIME,END_TIME from  M_CLASS_PLAN where  START_TIME < '" + time_now + "' and END_TIME >= '" + time_now + "'";
                    DataTable data_class_now = _dBSQL.GetCommand(sql_class_now);
                    if (data_class_now.Rows.Count > 0)
                    {
                          data_class_now.Rows[0]["START_TIME"].ToString();
                          data_class_now.Rows[0]["END_TIME"].ToString();
                        return new Tuple<DateTime, DateTime>(DateTime.Parse( data_class_now.Rows[0]["START_TIME"].ToString()), DateTime.Parse(data_class_now.Rows[0]["END_TIME"].ToString()));
                    }
                    else
                    {
                        return new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now);
                    }
                }
                else if (_flag ==2)
                {
                    var sql_hour_class = "select HOUR from M_CLASS_PLAN_HOUR";
                    DataTable table_class = _dBSQL.GetCommand(sql_hour_class);
                    if (table_class != null && table_class.Rows.Count > 0 )
                    {
                        DateTime data_old = time_now.AddHours(-int.Parse(table_class.Rows[0][0].ToString()));
                        string sql_class_old = "select START_TIME,END_TIME from  M_CLASS_PLAN where  START_TIME < '" + data_old + "' and END_TIME >= '" + data_old + "'";
                        DataTable data_class_old = _dBSQL.GetCommand(sql_class_old);
                        if (data_class_old.Rows.Count > 0)
                        {
                            return new Tuple<DateTime, DateTime>(DateTime.Parse(data_class_old.Rows[0]["START_TIME"].ToString()), DateTime.Parse(data_class_old.Rows[0]["END_TIME"].ToString()));
                        }
                        else
                        {
                            return new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now);
                        }
                    }
                    else
                    {
                        _vLog.writelog("_CLASS_TIME调用失败,未查到M_CLASS_PLAN_HOUR规则" + sql_hour_class.ToString(), -1);
                        return new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now);
                    }
               
                }
                else
                {
                    return new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now);
                }
            }
            catch(Exception ee)
            {
                _vLog.writelog("_CLASS_TIME调用失败"+ee.ToString(),-1);
                return new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now);
            }
        }

    }
}
