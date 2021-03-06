﻿using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VLog;

namespace LY_SINTER.Model
{
    internal class ANALYSIS_MODEL
    {
        public vLog _vLog { get; set; }
        private DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);

        public ANALYSIS_MODEL()
        {
            _vLog = new vLog(".\\ANALYSIS_MODEL\\");
            _vLog.connstring = DataBase.ConstParameters.strCon;
        }

        /// <summary>
        /// 20210414没有小数位数限制
        /// 通过传入的数据组计算数据，
        /// _A = :计算数据组
        /// _FLAG ：1平均值  2标准偏差，3最大值，4最小值，5极差
        /// </summary>
        /// <param name="_FLAG"></param>
        /// <returns></returns>
        public double Computer_MODEL_1(List<float> _A, int _FLAG)
        {
            try
            {
                if (_A.Count > 0)
                {
                    //20210410，剔除空值进行计算
                    float SUM_A = 0;//数据组和
                    int count = 0;//满足条件个数
                    for (int x = 0; x < _A.Count; x++)
                    {
                        //if ()
                        //{
                        //}
                        SUM_A += _A[x];
                    }
                    if (_FLAG == 1)
                    {
                        return SUM_A / _A.Count;
                    }
                    else if (_FLAG == 2)
                    {
                        float _A_AVG = SUM_A / _A.Count;
                        double _A_1 = 0;
                        int COUNT = -1;
                        for (int x = 0; x < _A.Count; x++)
                        {
                            _A_1 += Math.Pow(_A[x] - _A_AVG, 2);
                            COUNT = COUNT + 1;
                        }
                        return Math.Sqrt(_A_1 / COUNT);
                    }
                    else if (_FLAG == 3)
                    {
                        float _GETMIN = _A[0];
                        for (int x = 1; x < _A.Count; x++)
                        {
                            if (_GETMIN < _A[x])
                                _GETMIN = _A[x];
                        }
                        return _GETMIN;
                    }
                    else if (_FLAG == 4)
                    {
                        float _GETMax = _A[0];
                        for (int x = 1; x < _A.Count; x++)
                        {
                            if (_GETMax > _A[x])
                                _GETMax = _A[x];
                        }
                        return _GETMax;
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
                        return _GETMax - _GETMIN;
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
        /// 通过传入的数据组计算数据，
        /// _A = :计算数据组
        /// _FLAG ：1平均值  2标准偏差，3最大值，4最小值，5极差
        /// _B：小数位数
        /// </summary>
        /// <param name="_FLAG"></param>
        /// <returns></returns>
        public double Computer_MODEL(List<float> _A, int _FLAG, int _B)
        {
            try
            {
                if (_A.Count > 0)
                {
                    //20210410，剔除空值进行计算
                    float SUM_A = 0;//数据组和
                    int count = 0;//满足条件个数
                    for (int x = 0; x < _A.Count; x++)
                    {
                        //if ()
                        //{
                        //}
                        SUM_A += _A[x];
                    }
                    if (_FLAG == 1)
                    {
                        return Math.Round(SUM_A / _A.Count, _B);
                    }
                    else if (_FLAG == 2)
                    {
                        float _A_AVG = SUM_A / _A.Count;
                        double _A_1 = 0;
                        int COUNT = -1;
                        for (int x = 0; x < _A.Count; x++)
                        {
                            _A_1 += Math.Pow(_A[x] - _A_AVG, 2);
                            COUNT = COUNT + 1;
                        }
                        return Math.Round(Math.Sqrt(_A_1 / COUNT), _B);
                    }
                    else if (_FLAG == 3)
                    {
                        float _GETMIN = _A[0];
                        for (int x = 1; x < _A.Count; x++)
                        {
                            if (_GETMIN < _A[x])
                                _GETMIN = _A[x];
                        }
                        return Math.Round(_GETMIN, _B);
                    }
                    else if (_FLAG == 4)
                    {
                        float _GETMax = _A[0];
                        for (int x = 1; x < _A.Count; x++)
                        {
                            if (_GETMax > _A[x])
                                _GETMax = _A[x];
                        }
                        return Math.Round(_GETMax, _B);
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
        public Tuple<DateTime, DateTime> _CLASS_TIME(int _flag)
        {
            try
            {
                //当前系统时间
                DateTime time_now = DateTime.Now;
                if (_flag == 1)
                {
                    string sql_class_now = "select START_TIME,END_TIME from  M_CLASS_PLAN where  START_TIME < '" + time_now + "' and END_TIME >= '" + time_now + "'";
                    DataTable data_class_now = _dBSQL.GetCommand(sql_class_now);
                    if (data_class_now.Rows.Count > 0)
                    {
                        data_class_now.Rows[0]["START_TIME"].ToString();
                        data_class_now.Rows[0]["END_TIME"].ToString();
                        return new Tuple<DateTime, DateTime>(DateTime.Parse(data_class_now.Rows[0]["START_TIME"].ToString()), DateTime.Parse(data_class_now.Rows[0]["END_TIME"].ToString()));
                    }
                    else
                    {
                        return new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now);
                    }
                }
                else if (_flag == 2)
                {
                    var sql_hour_class = "select HOUR from M_CLASS_PLAN_HOUR";
                    DataTable table_class = _dBSQL.GetCommand(sql_hour_class);
                    if (table_class != null && table_class.Rows.Count > 0)
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
            catch (Exception ee)
            {
                _vLog.writelog("_CLASS_TIME调用失败" + ee.ToString(), -1);
                return new Tuple<DateTime, DateTime>(DateTime.Now, DateTime.Now);
            }
        }

        /// <summary>
        /// 返回剔除计算数据   _D:，数据源 _B:开始索引 _C：班别 _COL_NAME:条件检索名称 _DIG:小数位数 _MODEL：计算方式（1：平均值，2最优值）
        /// </summary>
        /// <param name="_D"></param>
        /// <param name="_B"></param>
        /// <param name="_C"></param>
        /// <param name="_Model"></param>
        /// <returns></returns>

        public Tuple<bool, DataRow> _GETAVG(DataTable _D, int _B, String _C, string _COL_NAME, int _DIG, int _Model = 1)
        {
            try
            {
                if (_D.Rows.Count > 0 && _D != null)
                {
                    DataRow _Tem_row = _D.NewRow();
                    if (_Model == 1)
                    {
                        for (int _COL = _B; _COL < _D.Columns.Count; _COL++)
                        {
                            float _Tem_date = 0;
                            int _Tem_Count = 0;
                            for (int _ROW = 0; _ROW < _D.Rows.Count; _ROW++)
                            {
                                if (_D.Rows[_ROW][_COL_NAME].ToString() == _C)
                                {
                                    if (_D.Rows[_ROW][_COL].ToString() != "")
                                    {
                                        _Tem_Count++;
                                        _Tem_date += float.Parse(_D.Rows[_ROW][_COL].ToString());
                                    }
                                }
                            }
                            if (_Tem_Count != 0 || _Tem_date != 0)
                            {
                                _Tem_row[_COL] = Math.Round(_Tem_date / _Tem_Count, _DIG);
                            }
                        }
                        return new Tuple<bool, DataRow>(true, _Tem_row);
                    }
                    else if (_Model == 1)
                    {
                        return new Tuple<bool, DataRow>(false, null);
                    }
                    else
                    {
                        return new Tuple<bool, DataRow>(false, null);
                    }
                }
                else
                {
                    return new Tuple<bool, DataRow>(false, null);
                }
            }
            catch (Exception EE)
            {
                _vLog.writelog("_GETAVG调用失败" + EE.ToString(), -1);
                return new Tuple<bool, DataRow>(false, null);
            }
        }
    }
}