using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY_SINTER.Model
{
    class Quality_Model
    {
        #region 烧结生产信息参数
        /// <summary>
        /// 停机记录标志位
        /// </summary>
        private static int flag_1 = -1;
        public static int FLAG_1
        {
            get { return flag_1; }
            set { flag_1 = value; }
        }
        /// <summary>
        /// 烧结事件标志位
        /// </summary>
        private static int flag_2 = -1;
        public static int FLAG_2
        {
            get { return flag_2; }
            set { flag_2 = value; }
        }
        #endregion

        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        /// <summary>
        /// 获取返矿变化状况
        /// item1：是否正常
        /// item2：总出料量
        /// item3：仓位变化
        /// item4：时间
        /// </summary>
        /// <returns></returns>
        public Tuple<bool , List<double>, List<double>, List<string>> _Get_Mine_Change()
        {
            try
            {
                DateTime time_now = DateTime.Now;
                DateTime time_now1 = time_now.AddHours(1);
                //总出料量
                List<double> list = new List<double>();
                //仓位变化
                List<double> list_1 = new List<double>();
                //时间
                List<string> time = new List<string>();

                for (int x = 0; x < 12; x++)
                {
                    DateTime time1 = time_now.AddHours(-x); 
                    DateTime time2 = time_now1.AddHours(-x);
                    string t1 = time1.ToString("yyyy-MM-dd HH") + ":00:00";
                    string t2 = time2.ToString("yyyy-MM-dd HH") + ":00:00";
                    //总出料量  取C_MAT_PLC_1MIN表每个小时，SUM (MAT_PLC_PV_W_17+MAT_PLC_PV_W_18）/60
                    string sql_ZC = "select isnull(sum(MAT_PLC_PV_W_15 +MAT_PLC_PV_W_16 )/60,0) AS sum from C_MAT_PLC_1MIN where TIMESTAMP >= '" + t1 + "' and TIMESTAMP < '" + t2 + "'";
                    //仓位变化 取C_MAT_PLC_1MIN表每个小时，最后一条记录的仓位 - 第一条记录的仓位 仓位计算方法：MAT_PLC_W_15+MAT_PLC_W_16
                    string sql_CW_NEW = "select TOP 1  isnull(MAT_PLC_W_15 +MAT_PLC_W_16 ,0) AS sum_new from C_MAT_PLC_1MIN where  TIMESTAMP <  '" + t2 + "' order by TIMESTAMP desc";
                    string sql_CW_OLD = "select TOP 1  isnull(MAT_PLC_W_15 +MAT_PLC_W_16 ,0) AS sum_old from C_MAT_PLC_1MIN where  TIMESTAMP > =  '" + t1 + "' order by TIMESTAMP asc";
                    DataTable dataTable_ZC = dBSQL.GetCommand(sql_ZC);
                    DataTable dt_cw_new = dBSQL.GetCommand(sql_CW_NEW);
                    DataTable dt_cw_old = dBSQL.GetCommand(sql_CW_OLD);
                    if (dt_cw_new.Rows.Count > 0 && dt_cw_old.Rows.Count > 0)
                    {
                        double CW_changes = Math.Round(double.Parse(dt_cw_new.Rows[0]["sum_new"].ToString()) - double.Parse(dt_cw_old.Rows[0]["sum_old"].ToString()), 2);
                        list_1.Add(CW_changes);
                    }
                    else
                    {
                        list_1.Add(0);
                    }
                    if (dataTable_ZC.Rows.Count > 0)
                    {
                        double total_holdup = Math.Round(double.Parse(dataTable_ZC.Rows[0]["sum"].ToString()), 2);
                        list.Add(total_holdup);
                    }
                    else
                    {
                        list.Add(0);
                    }
                    //总出料量
                    time.Add(time1.ToString());
                }
                return new Tuple<bool, List<double>, List<double>, List<string>>(true, list, list_1, time);
            }
            catch(Exception ee)
            {
                return new Tuple<bool, List<double>, List<double>, List<string>>(false,null, null, null);
            }
        }
    }
}
