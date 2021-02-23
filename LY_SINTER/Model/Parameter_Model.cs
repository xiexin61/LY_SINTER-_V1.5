using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY_SINTER.Model
{
    class Parameter_Model
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        /// <summary>
        /// 返回烧结生产信息记录配置表
        /// flag =1 原因大类
        /// flag = 2 原因小类
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, DataTable> _GetTs(int flag)
        {
            try
            {
                if (flag == 1 )//原因大类
                {
                    var sql_1 = "  select FLAG_1 AS 'VALUES',DESCRIBE AS NAME from M_PRO_RECORDS_CONFIG_1 where FLAG = 1";
                    DataTable table_1 = dBSQL.GetCommand(sql_1);
                    if (table_1 != null && table_1.Rows.Count > 0 )
                    {
                        return new Tuple<bool, DataTable>(true, table_1);
                    }
                    else
                    {
                        return new Tuple<bool, DataTable>(false, null);
                    }
                }
                else//原因小类
                {
                    var sql_1 = "  select FLAG_1 AS 'VALUES',DESCRIBE AS NAME from M_PRO_RECORDS_CONFIG_1 where FLAG = 2";
                    DataTable table_1 = dBSQL.GetCommand(sql_1);
                    if (table_1 != null && table_1.Rows.Count > 0)
                    {
                        return new Tuple<bool, DataTable>(true, table_1);
                    }
                    else
                    {
                        return new Tuple<bool, DataTable>(false, null);
                    }
                }
            }
            catch
            {
                return new Tuple<bool, DataTable>(false, null);
            }
        }
        /// <summary>
        /// 返回原因类对应关系
        /// return  itme1：是否正常  itme2：  key:中文名称  values 对应编码
        /// set _flag  = 1 原因大类    = 2 原因小类
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, Dictionary<string,string>> _GetRule(int _FLAG)
        {
            try
            {
                if (_FLAG == 1 )
                {
                    string _sql = "select * from M_PRO_RECORDS_CONFIG_1 where  FLAG = 1 ";
                    DataTable _table = dBSQL.GetCommand(_sql);
                    if (_table != null && _table.Rows.Count > 0 )
                    {
                        Dictionary<string, string> _a = new Dictionary<string, string>();
                        for (int x = 0; x < _table.Rows.Count;x++)
                        {
                            _a.Add(_table.Rows[x]["DESCRIBE"].ToString(), _table.Rows[x]["FLAG_1"].ToString());
                        }
                        return new Tuple<bool, Dictionary<string, string>>(true, _a);
                    }
                    else
                    {
                        return new Tuple<bool, Dictionary<string, string>>(false, null);
                    }
                }
                else
                {
                    string _sql = "select * from M_PRO_RECORDS_CONFIG_1 where  FLAG = 2 ";
                    DataTable _table = dBSQL.GetCommand(_sql);
                    if (_table != null && _table.Rows.Count > 0)
                    {
                        Dictionary<string, string> _a = new Dictionary<string, string>();
                        for (int x = 0; x < _table.Rows.Count; x++)
                        {
                            _a.Add(_table.Rows[x]["DESCRIBE"].ToString(), _table.Rows[x]["FLAG_1"].ToString());
                        }
                        return new Tuple<bool, Dictionary<string, string>>(true, _a);
                    }
                    else
                    {
                        return new Tuple<bool, Dictionary<string, string>>(false, null);
                    }
                }
            }
            catch
            {
                return new Tuple<bool, Dictionary<string, string>>(false,null);
            }
        }

        /// <summary>
        /// 返回原因类对应关系
        /// return  itme1：是否正常  itme2：  key:对应编码  values 中文描述
        /// set _flag  = 1 原因大类    = 2 原因小类
        /// </summary>
        /// <returns></returns>
        public Tuple<bool, Dictionary<string, string>> _GetRule_1(int _FLAG)
        {
            try
            {
                if (_FLAG == 1)
                {
                    string _sql = "select * from M_PRO_RECORDS_CONFIG_1 where  FLAG = 1 ";
                    DataTable _table = dBSQL.GetCommand(_sql);
                    if (_table != null && _table.Rows.Count > 0)
                    {
                        Dictionary<string, string> _a = new Dictionary<string, string>();
                        for (int x = 0; x < _table.Rows.Count; x++)
                        {
                            _a.Add(_table.Rows[x]["FLAG_1"].ToString(), _table.Rows[x]["DESCRIBE"].ToString());
                        }
                        return new Tuple<bool, Dictionary<string, string>>(true, _a);
                    }
                    else
                    {
                        return new Tuple<bool, Dictionary<string, string>>(false, null);
                    }
                }
                else
                {
                    string _sql = "select * from M_PRO_RECORDS_CONFIG_1 where  FLAG = 2 ";
                    DataTable _table = dBSQL.GetCommand(_sql);
                    if (_table != null && _table.Rows.Count > 0)
                    {
                        Dictionary<string, string> _a = new Dictionary<string, string>();
                        for (int x = 0; x < _table.Rows.Count; x++)
                        {
                            _a.Add(_table.Rows[x]["FLAG_1"].ToString(), _table.Rows[x]["DESCRIBE"].ToString());
                        }
                        return new Tuple<bool, Dictionary<string, string>>(true, _a);
                    }
                    else
                    {
                        return new Tuple<bool, Dictionary<string, string>>(false, null);
                    }
                }
            }
            catch
            {
                return new Tuple<bool, Dictionary<string, string>>(false, null);
            }
        }
        /// <summary>
        /// 判断开始&结束时间是否正常
        /// return item1:是否正常 item2：白/夜班  item3：班名
        /// </summary>
        /// <returns></returns>
        public Tuple<bool,string,string>  Judge_Class_Time(DateTime a,DateTime b)
        {
            try
            {
                var sql = "select * from M_CLASS_PLAN where START_TIME <= '"+ a+ "' and END_TIME >= '"+b+"'";
                DataTable _date = dBSQL.GetCommand(sql);
                if (_date != null && _date.Rows.Count > 0 )
                {
                    return new Tuple<bool, string, string>(true, _date.Rows[0]["DAY_NIGHT"].ToString(), _date.Rows[0]["SHIFT_FLAG"].ToString());
                }
                else
                {
                    return new Tuple<bool, string, string>(false, "", "");
                }
            }
            catch
            {
                return new Tuple<bool, string, string>(false,"","");
            }
        }
        /// <summary>
        /// insert :当前事件
        /// return ：item1：是否正常，item2：白夜班 ，itme3：班次
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Tuple<bool, string,string> _Get_Class_Plan(DateTime a)
        {
            try
            {
                var sql = "select *  from M_CLASS_PLAN where START_TIME <= '"+ a+ "' and END_TIME > = '"+ a+"'";
                DataTable _date = dBSQL.GetCommand(sql);
                if (_date != null && _date.Rows.Count > 0 )
                {
                    return new Tuple<bool, string, string>(true, _date.Rows[0]["DAY_NIGHT"].ToString(), _date.Rows[0]["SHIFT_FLAG"].ToString());
                }
                else
                {
                    return new Tuple<bool, string, string>(false, "", "");
                }
            }
            catch
            {
                return new Tuple<bool, string, string>(false, "", "");
            }
            
        }
        /// <summary>
        /// flag = 1 白夜班  =2 班次
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public DataTable _GetClass(int flag)
        {
            try
            {
                if (flag == 2)
                {
                    var sql = "select ROW_NUMBER() OVER (ORDER BY START_TIME asc) AS 'VALUES',SHIFT_FLAG as NAME from M_CLASS_PLAN_CFG order by START_TIME asc";
                    DataTable data = dBSQL.GetCommand(sql);
                    return data;
                }
                else
                {
                    DataTable data_1 = new DataTable();
                    data_1.Columns.Add("NAME");
                    data_1.Columns.Add("VALUES");
                    DataRow row1_1 = data_1.NewRow();
                    row1_1["NAME"] = "夜班";
                    row1_1["VALUES"] = 1;
                    data_1.Rows.Add(row1_1);
                    DataRow row1_2 = data_1.NewRow();
                    row1_2["NAME"] = "白班";
                    row1_2["VALUES"] = 2;
                    data_1.Rows.Add(row1_2);
                    return data_1;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
