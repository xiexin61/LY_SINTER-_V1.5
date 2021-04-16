using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataBase;
using LY_SINTER.Custom;
using VLog;
using LY_SINTER.Popover.Analysis;
using LY_SINTER.Model;

namespace LY_SINTER.PAGE.Analysis
{
    public partial class Production_state : UserControl
    {
        public vLog _vLog { get; set; }
        private DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        private ANALYSIS_MODEL aNALYSIS = new ANALYSIS_MODEL();//数据分析模型

        private string[] col_name = new string[] //d1数据使用部分
                 {
                "NAME","SINTER_COST_TOT", "R_COUNT", "R_TZ", "R_TRU", "SSH_PB",
                "SSH_TZ", "C_FEO", "FUEL_PB", "FUEL_TZ",
                "R_005", "R_008" , "FEO_1",
                "SJK_AV_PTCL", "SJK_UND_10", "HHL_W",
                "SF_BALA_PB", "GF_BALA_PB",
                "M_P", "FUEL_CNSP_PER" , "GAS_CNSP_PER" , "M_FAN_CNSP_E" ,
                "WTR_Q_HOUR", "PH_STP_NUM", "BED_THICK_AD_NUM", "BED_THICK",
                "M_SPEED", "BTP_D", "BTP_X", "BTP_POS_D", "BTP_POS_X",
                "TEMP_ZONE", "BTP_ZONE_RATE", "ZONE_RATE_300",
                "ZONE_RATE_450", "MA_T_D", "MA_T_X", "MA_P_D",
                "MA_P_X", "V_BED_SPEED", "SINTER_END_T", "COLD_END_T",
                "COLD_FAN_NUM", "MA_HZ_D", "MA_HZ_X", "MA_CURT_D",
                "MA_CURT_X"
                };

        public Production_state()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Analysis\\Production_state\\");
            time_begin_end();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            label2.Text = "最新调整时间:" + DateTime.Now.ToString();

            SHOW_D1();//d1数据刷新
            SHOW_D2();//d2数据刷新
            d1_col();//二级表头
        }

        /// <summary>
        /// 开始时间-结束时间赋值
        /// </summary>
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddDays(-1);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }

        /// <summary>
        /// 二级标题
        /// </summary>
        public void d1_col()
        {
            try
            {
                #region 添加列说明

                //d1
                this.d1.AddSpanHeader(2, 13, "质量");
                this.d1.AddSpanHeader(27, 2, "BTP温度");
                this.d1.AddSpanHeader(29, 2, "BTP位置");
                this.d1.AddSpanHeader(31, 4, "BTP其他情况");
                this.d1.AddSpanHeader(35, 2, "大烟道温度");
                this.d1.AddSpanHeader(37, 2, "大烟道负压");
                this.d1.AddSpanHeader(43, 2, "主排频率");
                this.d1.AddSpanHeader(45, 2, "主排电流");

                //d2
                this.d2.AddSpanHeader(20, 7, "BTP温度");
                this.d2.AddSpanHeader(27, 3, "大烟道温度");
                this.d2.AddSpanHeader(30, 3, "BTP位置");
                this.d2.AddSpanHeader(33, 3, "大烟道负压");
                this.d2.AddSpanHeader(40, 3, "主排频率");
                this.d2.AddSpanHeader(43, 3, "主排电流");

                #endregion 添加列说明
            }
            catch (Exception ee)
            {
                _vLog.writelog("d1_col方法错误" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 刷新表头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void d1_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                d1.ReDrawHead();
            }
            catch (Exception ee)
            {
                _vLog.writelog("d1_Scroll方法错误" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 页面初始化
        /// </summary>
        public void SHOW_D1()
        {
            try
            {
                //当前系统时间
                DateTime time_now = DateTime.Now;
                //调整值检索的开始时间
                DateTime time_TZ = time_now.AddHours(-1);
                Tuple<DateTime, DateTime> tuple = aNALYSIS._CLASS_TIME(1);
                //本班开始时间 /
                string time_class_now_begin = tuple.Item1.ToString();
                //本班结束时间
                string time_class_now_end = tuple.Item2.ToString();
                Tuple<DateTime, DateTime> tuple_old = aNALYSIS._CLASS_TIME(2);
                //上班开始时间
                string time_class_old_begin = tuple_old.Item1.ToString();
                //上班结束时间
                string time_class_old_end = tuple_old.Item2.ToString();
                //累计月份开始时间 本月的月初时间
                string TIME_MON = "";
                TIME_MON += DateTime.Now.Year.ToString();
                TIME_MON += "-" + DateTime.Now.Month.ToString();
                TIME_MON += "-" + "01 00:00:00 ";
                //昨日开始时间
                string TIME_OLD_BEGIN = "";
                DateTime time_old = time_now.AddDays(-1);
                TIME_OLD_BEGIN = time_old.Year.ToString();
                TIME_OLD_BEGIN += "-" + time_old.Month.ToString();
                TIME_OLD_BEGIN += "-" + time_old.Day.ToString();
                TIME_OLD_BEGIN += " 00:00:00";
                //昨日结束时间
                string TIME_OLD_END = "";
                TIME_OLD_END = time_old.Year.ToString();
                TIME_OLD_END += "-" + time_old.Month.ToString();
                TIME_OLD_END += "-" + time_old.Day.ToString();
                TIME_OLD_END += " 23:59:59";

                #region 一烧当前生产状态信息

                //d1使用数据源
                DataTable data_1 = new DataTable();

                for (int x = 0; x < col_name.Count(); x++)
                {
                    data_1.Columns.Add(col_name[x]);
                }

                #region R、生石灰、燃料调整值 MC_NUMCAL_INTERFACE_10

                string sql_MC_NUMCAL_INTERFACE_10 = "select top (1) " +
                    "isnull(R_COMP_TOT,0) as R_COMP_TOT," +
                    "isnull(R_COMP_YSD,0) as R_COMP_YSD," +
                    "isnull(R_COMP_LASTSFT,0) as R_COMP_LASTSFT," +
                    "isnull(R_COMP_CURSFT,0) as R_COMP_CURSFT," +
                    "isnull(SSH_PB_COMP_TOT,0) as SSH_PB_COMP_TOT," +
                    "isnull(SSH_PB_COMP_YSD,0) as SSH_PB_COMP_YSD," +
                    "isnull(SSH_PB_COMP_LASTSFT,0) as SSH_PB_COMP_LASTSFT," +
                    "isnull(SSH_PB_COMP_CURSFT,0) as SSH_PB_COMP_CURSFT," +
                    "isnull(FUEL_PB_COMP_TOT,0) as FUEL_PB_COMP_TOT," +
                    "isnull(FUEL_PB_COMP_YSD,0) as FUEL_PB_COMP_YSD," +
                    "isnull(FUEL_PB_COMP_LASTSFT,0) as FUEL_PB_COMP_LASTSFT," +
                    "isnull(FUEL_PB_COMP_CURSFT,0) as FUEL_PB_COMP_CURSFT   " +
                    "from MC_NUMCAL_INTERFACE_10 where TIMESTAMP >= '" + time_TZ + "' and  TIMESTAMP < '" + time_now + "' order by TIMESTAMP desc ";
                DataTable data_MC_NUMCAL_INTERFACE_10 = _dBSQL.GetCommand(sql_MC_NUMCAL_INTERFACE_10);

                #endregion R、生石灰、燃料调整值 MC_NUMCAL_INTERFACE_10

                #region 目标

                DataRow row_1 = data_1.NewRow();
                row_1["NAME"] = "目标";
                string SQL_MC_NUMCAL_INTERFACE_10_TARGET = "select top (1) " +
                 "isnull(SINTER_COST_TOT,0) as SINTER_COST_TOT," +
                 "isnull(R_COUNT,0) as R_COUNT," +
                 "isnull(R_TRU,0) as R_TRU," +
                 "isnull(SSH_PB,0) as SSH_PB," +
                 "isnull(FEO_TRU,0) as FEO_TRU," +
                 "isnull(FUEL_PB,0) as FUEL_PB," +
                 "isnull(R_005,0) as R_005," +
                 "isnull(R_008,0) as R_008," +
                 "isnull(FEO_1,0) as FEO_1," +
                 "isnull(SJK_AV_PTCL,0) as SJK_AV_PTCL," +
                 "isnull(SJK_UND_10,0) as SJK_UND_10," +
                 "isnull(HHL_W,0) as HHL_W," +
                 "isnull(SF_BALA_PB,0) as SF_BALA_PB," +
                 "isnull(GF_BALA_PB,0) as GF_BALA_PB," +
                 "isnull(M_P,0) as M_P," +
                 "isnull(FUEL_CNSP_PER,0) as FUEL_CNSP_PER," +
                 "isnull(GAS_CNSP_PER,0) as GAS_CNSP_PER," +
                 "isnull(M_FAN_CNSP_E,0) as M_FAN_CNSP_E," +
                 "isnull(WTR_Q_HOUR,0) as WTR_Q_HOUR," +
                 "isnull(PH_STP_NUM,0) as PH_STP_NUM," +
                 "isnull(BED_THICK_AD_NUM,0) as BED_THICK_AD_NUM," +
                 "isnull(BED_THICK,0) as BED_THICK," +
                 "isnull(M_SPEED,0) as M_SPEED," +
                 "isnull(BTP_D,0) as BTP_D," +
                 "isnull(BTP_X,0) as BTP_X," +
                 "isnull(BTP_POS_D,0) as BTP_POS_D," +
                 "isnull(BTP_POS_X,0) as BTP_POS_X," +
                 "isnull(TEMP_ZONE,0) as TEMP_ZONE," +
                 "isnull(BTP_ZONE_RATE,0) as BTP_ZONE_RATE," +
                 "isnull(ZONE_RATE_300,0) as ZONE_RATE_300," +
                 "isnull(ZONE_RATE_450,0) as ZONE_RATE_450," +
                 "isnull(MA_T_D,0) as MA_T_D," +
                 "isnull(MA_T_X,0) as MA_T_X," +
                 "isnull(MA_P_D,0) as MA_P_D," +
                 "isnull(MA_P_X,0) as MA_P_X," +
                 "isnull(V_BED_SPEED,0) as V_BED_SPEED," +
                 "isnull(SINTER_END_T,0) as SINTER_END_T," +
                 "isnull(COLD_END_T,0) as COLD_END_T," +
                 "isnull(COLD_FAN_NUM,0) as COLD_FAN_NUM," +
                 "isnull(MA_HZ_D,0) as MA_HZ_D," +
                 "isnull(MA_HZ_X,0) as MA_HZ_X," +
                 "isnull(MA_CURT_D,0) as MA_CURT_D," +
                 "isnull(MA_CURT_X,0) as MA_CURT_X   " +
                 " from MC_NUMCAL_INTERFACE_10_TARGET " +
                 " ORDER BY TIMESTAMP DESC";
                DataTable dataTable_NUMCAL_INTERFACE_10_TARGET = _dBSQL.GetCommand(SQL_MC_NUMCAL_INTERFACE_10_TARGET);
                if (dataTable_NUMCAL_INTERFACE_10_TARGET.Rows.Count > 0)
                {
                    int _YM_INDEX = 0;
                    for (int x = 0; x < dataTable_NUMCAL_INTERFACE_10_TARGET.Columns.Count; x++)
                    {
                        _YM_INDEX = _YM_INDEX + 1;
                        if (_YM_INDEX == 3 || _YM_INDEX == 6 || _YM_INDEX == 9)
                        {
                            _YM_INDEX = _YM_INDEX + 1;
                        }
                        row_1[_YM_INDEX] = dataTable_NUMCAL_INTERFACE_10_TARGET.Rows[0][x].ToString();
                    }
                }
                data_1.Rows.Add(row_1);

                #endregion 目标

                #region 累计实际

                DataRow row_2 = data_1.NewRow();
                row_2["NAME"] = "累计实际";
                //查询MC_NUMCAL_INTERFACE_10_MONTH表最新的时间，通过比较数据库的时间和系统时间进行对比，若系统时间和数据库时间的偏差在5min则视为正常数据，或无数据
                string SQL_LJ_MC_NUMCAL_INTERFACE_10_MONTH = "select top (1) " +
                     "SINTER_COST_TOT," +
                    "R_COUNT," +
                    "R_TRU," +
                    "SSH_PB," +
                    "FEO_TRU," +
                    "FUEL_PB," +
                    "R_005," +
                    "R_008," +
                    "FEO_1," +
                    "SJK_AV_PTCL," +
                    "SJK_UND_10," +
                    "HHL_W," +
                    "SF_BALA_PB," +
                    "GF_BALA_PB," +
                    "M_P,FUEL_CNSP_PER," +
                    "GAS_CNSP_PER," +
                    "M_FAN_CNSP_E," +
                    "WTR_Q_HOUR," +
                    "PH_STP_NUM," +
                    "BED_THICK_AD_NUM," +
                    "BED_THICK," +
                    "M_SPEED," +
                    "BTP_D," +
                    "BTP_X," +
                    "BTP_POS_D," +
                    "BTP_POS_X," +
                    "TEMP_ZONE," +
                    "BTP_ZONE_RATE," +
                    "ZONE_RATE_300," +
                    "ZONE_RATE_450," +
                    "MA_T_D," +
                    "MA_T_X," +
                    "MA_P_D," +
                    "MA_P_X," +
                    "V_BED_SPEED," +
                    "SINTER_END_T," +
                    "COLD_END_T," +
                    "COLD_FAN_NUM," +
                    "MA_HZ_D," +
                    "MA_HZ_X," +
                    "MA_CURT_D," +
                    "MA_CURT_X " +
                    "from MC_NUMCAL_INTERFACE_10_MONTH " +
                    "where  TIMESTAMP >='" + TIME_MON + "' and TIMESTAMP <= '" + time_now + "' order by TIMESTAMP desc";
                DataTable dataTable_LJ_MC_NUMCAL_INTERFACE_10_MONTH = _dBSQL.GetCommand(SQL_LJ_MC_NUMCAL_INTERFACE_10_MONTH);
                if (dataTable_LJ_MC_NUMCAL_INTERFACE_10_MONTH.Rows.Count > 0)
                {
                    int _YM_INDEX = 0;
                    for (int x = 0; x < dataTable_LJ_MC_NUMCAL_INTERFACE_10_MONTH.Columns.Count; x++)
                    {
                        _YM_INDEX = _YM_INDEX + 1;
                        if (_YM_INDEX == 3 || _YM_INDEX == 6 || _YM_INDEX == 9)
                        {
                            _YM_INDEX = _YM_INDEX + 1;
                        }
                        row_2[_YM_INDEX] = dataTable_LJ_MC_NUMCAL_INTERFACE_10_MONTH.Rows[0][x].ToString();
                    }
                }
                else
                {
                }
                //调整值
                if (data_MC_NUMCAL_INTERFACE_10.Rows.Count > 0)
                {
                    row_2["R_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["R_COMP_TOT"].ToString();
                    row_2["SSH_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["SSH_PB_COMP_TOT"].ToString();
                    row_2["FUEL_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["FUEL_PB_COMP_TOT"].ToString();
                }
                data_1.Rows.Add(row_2);

                #endregion 累计实际

                #region 昨日

                DataRow row_3 = data_1.NewRow();
                row_3["NAME"] = "昨日";
                string SQL_ZR_MC_NUMCAL_INTERFACE_10_DAY = "select top (1) " +
                    "SINTER_COST_TOT," +
                     "R_COUNT," +
                     "R_TRU," +
                     "SSH_PB," +
                     "FEO_TRU," +
                     "FUEL_PB," +
                     "R_005," +
                     "R_008," +
                     "FEO_1," +
                     "SJK_AV_PTCL," +
                     "SJK_UND_10," +
                     "HHL_W," +
                     "SF_BALA_PB," +
                     "GF_BALA_PB," +
                     "M_P,FUEL_CNSP_PER," +
                     "GAS_CNSP_PER," +
                     "M_FAN_CNSP_E," +
                     "WTR_Q_HOUR," +
                     "PH_STP_NUM," +
                     "BED_THICK_AD_NUM," +
                     "BED_THICK," +
                     "M_SPEED," +
                     "BTP_D," +
                     "BTP_X," +
                     "BTP_POS_D," +
                     "BTP_POS_X," +
                     "TEMP_ZONE," +
                     "BTP_ZONE_RATE," +
                     "ZONE_RATE_300," +
                     "ZONE_RATE_450," +
                     "MA_T_D," +
                     "MA_T_X," +
                     "MA_P_D," +
                     "MA_P_X," +
                     "V_BED_SPEED," +
                     "SINTER_END_T," +
                     "COLD_END_T," +
                     "COLD_FAN_NUM," +
                     "MA_HZ_D," +
                     "MA_HZ_X," +
                     "MA_CURT_D," +
                     "MA_CURT_X " +
                     "from MC_NUMCAL_INTERFACE_10_DAY " +
                     "where  TIMESTAMP >='" + TIME_OLD_BEGIN + "' and TIMESTAMP <= '" + TIME_OLD_END + "' order by TIMESTAMP desc";
                DataTable dataTable_ZR_MC_NUMCAL_INTERFACE_10_DAY = _dBSQL.GetCommand(SQL_ZR_MC_NUMCAL_INTERFACE_10_DAY);
                if (dataTable_ZR_MC_NUMCAL_INTERFACE_10_DAY.Rows.Count > 0)
                {
                    int _YM_INDEX = 0;
                    for (int x = 0; x < dataTable_ZR_MC_NUMCAL_INTERFACE_10_DAY.Columns.Count; x++)
                    {
                        _YM_INDEX = _YM_INDEX + 1;
                        if (_YM_INDEX == 3 || _YM_INDEX == 6 || _YM_INDEX == 9)
                        {
                            _YM_INDEX = _YM_INDEX + 1;
                        }
                        row_3[_YM_INDEX] = dataTable_ZR_MC_NUMCAL_INTERFACE_10_DAY.Rows[0][x].ToString();
                    }
                }
                else
                {
                }
                if (data_MC_NUMCAL_INTERFACE_10.Rows.Count > 0)
                {
                    row_3["R_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["R_COMP_YSD"].ToString();
                    row_3["SSH_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["SSH_PB_COMP_YSD"].ToString();
                    row_3["FUEL_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["FUEL_PB_COMP_YSD"].ToString();
                }
                data_1.Rows.Add(row_3);

                #endregion 昨日

                #region 上班

                DataRow row_4 = data_1.NewRow();
                row_4["NAME"] = "上班";
                string SQL_SB_MC_NUMCAL_INTERFACE_10_CLASS = "select top (1) " +
                    "SINTER_COST_TOT," +
                     "R_COUNT," +
                     "R_TRU," +
                     "SSH_PB," +
                     "FEO_TRU," +
                     "FUEL_PB," +
                     "R_005," +
                     "R_008," +
                     "FEO_1," +
                     "SJK_AV_PTCL," +
                     "SJK_UND_10," +
                     "HHL_W," +
                     "SF_BALA_PB," +
                     "GF_BALA_PB," +
                     "M_P,FUEL_CNSP_PER," +
                     "GAS_CNSP_PER," +
                     "M_FAN_CNSP_E," +
                     "WTR_Q_HOUR," +
                     "PH_STP_NUM," +
                     "BED_THICK_AD_NUM," +
                     "BED_THICK," +
                     "M_SPEED," +
                     "BTP_D," +
                     "BTP_X," +
                     "BTP_POS_D," +
                     "BTP_POS_X," +
                     "TEMP_ZONE," +
                     "BTP_ZONE_RATE," +
                     "ZONE_RATE_300," +
                     "ZONE_RATE_450," +
                     "MA_T_D," +
                     "MA_T_X," +
                     "MA_P_D," +
                     "MA_P_X," +
                     "V_BED_SPEED," +
                     "SINTER_END_T," +
                     "COLD_END_T," +
                     "COLD_FAN_NUM," +
                     "MA_HZ_D," +
                     "MA_HZ_X," +
                     "MA_CURT_D," +
                     "MA_CURT_X " +
                     "from MC_NUMCAL_INTERFACE_10_CLASS " +
                     "where  TIMESTAMP >='" + time_class_old_begin + "' and TIMESTAMP <= '" + time_class_old_end + "' order by TIMESTAMP desc";
                DataTable dataTable_SB_MC_NUMCAL_INTERFACE_10_CLASS = _dBSQL.GetCommand(SQL_SB_MC_NUMCAL_INTERFACE_10_CLASS);
                if (dataTable_SB_MC_NUMCAL_INTERFACE_10_CLASS.Rows.Count > 0)
                {
                    int _YM_INDEX = 0;
                    for (int x = 0; x < dataTable_SB_MC_NUMCAL_INTERFACE_10_CLASS.Columns.Count; x++)
                    {
                        _YM_INDEX = _YM_INDEX + 1;
                        if (_YM_INDEX == 3 || _YM_INDEX == 6 || _YM_INDEX == 9)
                        {
                            _YM_INDEX = _YM_INDEX + 1;
                        }
                        row_4[_YM_INDEX] = dataTable_SB_MC_NUMCAL_INTERFACE_10_CLASS.Rows[0][x].ToString();
                    }
                }
                else
                {
                }
                if (data_MC_NUMCAL_INTERFACE_10.Rows.Count > 0)
                {
                    row_4["R_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["R_COMP_LASTSFT"].ToString();
                    row_4["SSH_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["SSH_PB_COMP_LASTSFT"].ToString();
                    row_4["FUEL_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["FUEL_PB_COMP_LASTSFT"].ToString();
                }
                data_1.Rows.Add(row_4);

                #endregion 上班

                #region 本班

                DataRow row_5 = data_1.NewRow();
                row_5["NAME"] = "本班";
                string SQL_BB_MC_NUMCAL_INTERFACE_10_CLASS = "select top (1) " +
                    "SINTER_COST_TOT," +
                     "R_COUNT," +
                     "R_TRU," +
                     "SSH_PB," +
                     "FEO_TRU," +
                     "FUEL_PB," +
                     "R_005," +
                     "R_008," +
                     "FEO_1," +
                     "SJK_AV_PTCL," +
                     "SJK_UND_10," +
                     "HHL_W," +
                     "SF_BALA_PB," +
                     "GF_BALA_PB," +
                     "M_P,FUEL_CNSP_PER," +
                     "GAS_CNSP_PER," +
                     "M_FAN_CNSP_E," +
                     "WTR_Q_HOUR," +
                     "PH_STP_NUM," +
                     "BED_THICK_AD_NUM," +
                     "BED_THICK," +
                     "M_SPEED," +
                     "BTP_D," +
                     "BTP_X," +
                     "BTP_POS_D," +
                     "BTP_POS_X," +
                     "TEMP_ZONE," +
                     "BTP_ZONE_RATE," +
                     "ZONE_RATE_300," +
                     "ZONE_RATE_450," +
                     "MA_T_D," +
                     "MA_T_X," +
                     "MA_P_D," +
                     "MA_P_X," +
                     "V_BED_SPEED," +
                     "SINTER_END_T," +
                     "COLD_END_T," +
                     "COLD_FAN_NUM," +
                     "MA_HZ_D," +
                     "MA_HZ_X," +
                     "MA_CURT_D," +
                     "MA_CURT_X " +
                     "from MC_NUMCAL_INTERFACE_10_CLASS " +
                     "where  TIMESTAMP >='" + time_class_now_begin + "' and TIMESTAMP <= '" + time_class_now_end + "' order by TIMESTAMP desc";
                DataTable dataTable_BB_MC_NUMCAL_INTERFACE_10_CLASS = _dBSQL.GetCommand(SQL_BB_MC_NUMCAL_INTERFACE_10_CLASS);
                if (dataTable_BB_MC_NUMCAL_INTERFACE_10_CLASS.Rows.Count > 0)
                {
                    int _YM_INDEX = 0;
                    for (int x = 0; x < dataTable_BB_MC_NUMCAL_INTERFACE_10_CLASS.Columns.Count; x++)
                    {
                        _YM_INDEX = _YM_INDEX + 1;
                        if (_YM_INDEX == 3 || _YM_INDEX == 6 || _YM_INDEX == 9)
                        {
                            _YM_INDEX = _YM_INDEX + 1;
                        }
                        row_5[_YM_INDEX] = dataTable_BB_MC_NUMCAL_INTERFACE_10_CLASS.Rows[0][x].ToString();
                    }
                }
                else
                {
                }
                if (data_MC_NUMCAL_INTERFACE_10.Rows.Count > 0)
                {
                    row_5["R_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["R_COMP_CURSFT"].ToString();
                    row_5["SSH_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["SSH_PB_COMP_CURSFT"].ToString();
                    row_5["FUEL_TZ"] = data_MC_NUMCAL_INTERFACE_10.Rows[0]["FUEL_PB_COMP_CURSFT"].ToString();
                }
                data_1.Rows.Add(row_5);

                #endregion 本班

                this.d1.DataSource = data_1;

                #endregion 一烧当前生产状态信息
            }
            catch (Exception ee)
            {
                _vLog.writelog("SHOW_D1方法错误" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// d2数据设置
        /// </summary>
        public void SHOW_D2()
        {
            try
            {
                //d2使用数据源
                DataTable data_2 = new DataTable();
                string[] col_name = new string[] {
                "date",
                "time",
                "HEAP_NUM",
                "SHIFT_FLAG",
                "P_CAL",
                "HHL_W",
                "SF_BALA_PB",
                "GF_BALA_PB",
                "GF_BALA_W",
                "M_Y",
                "M_P",
                "MOI_1",
                "MOI_2",
                "WTR_Q_HOUR" ,
                "PH_STP_NUM",
                "BED_THICK_AD_NUM",
                "BED_THICK",
                "M_SPEED",
                "M_C_SPEED",
                "IG_T",
                "BTP_D",
                "BTP_X" ,
                "BTP_AV" ,
                "TEMP_ZONE" ,
                "BTP_ZONE_RATE",
                "ZONE_RATE_300",
                "ZONE_RATE_450",
                "MA_T_D",
                "MA_T_X",
                "MA_T_AD",
                "BTP_POS_D",
                "BTP_POS_X",
                "BTP_POS_AV",
                "MA_P_D",
                "MA_P_X",
                "MA_P_AV",
                "V_BED_SPEED",
                "SINTER_END_T",
                "COLD_END_T",
                "COLD_FAN_NUM",
                "MA_HZ_D",
                "MA_HZ_X",
                "MA_HZ_AV",
                "MA_CURT_D",
                "MA_CURT_X",
                "MA_CURT_AV",
                "M_FAN_CNSP_E",
                "FUEL_CNSP_PER",
                "GAS_CNSP_PER"
            };
                for (int x = 0; x < col_name.Count(); x++)
                {
                    data_2.Columns.Add(col_name[x]);
                }
                //当前系统时间
                DateTime date = DateTime.Now;
                //页面显示的日期
                string date_day = "";
                date_day += date.Year.ToString();
                date_day += "年" + date.Month.ToString();
                date_day += "月" + date.Day.ToString() + "日";

                //通过系统时间获取的小时数去判断需要显示多少条数据，截至到0点为止，页面最多展示24条数据，排列顺序为倒序
                int HOUR = date.Hour;
                HOUR = HOUR + 1;
                for (int X = 0; X < HOUR; X++)
                {
                    //时间段
                    string time_day = "";
                    //开始时间
                    string time_begin = "";
                    time_begin += date.Year.ToString();
                    time_begin += "-" + date.Month.ToString();
                    time_begin += "-" + date.Day.ToString();
                    DateTime dateTime = date.AddHours(-X);
                    time_day = dateTime.Hour.ToString();
                    time_begin += " " + dateTime.Hour.ToString();
                    time_begin += ":00:00";
                    //结束时间
                    string time_end = "";
                    time_end += date.Year.ToString();
                    time_end += "-" + date.Month.ToString();
                    time_end += "-" + date.Day.ToString();
                    DateTime dateTime_1 = dateTime.AddHours(+1);
                    time_day += "~" + dateTime_1.Hour.ToString();
                    time_end += " " + dateTime_1.Hour.ToString();
                    time_end += ":00:00";
                    DataRow data = data_2.NewRow();
                    //日期
                    data["date"] = date_day;
                    //时间段
                    data["time"] = time_day;
                    string sql_MC_NUMCAL_INTERFACE_10 = "select top (1)" +
                        "HEAP_NUM,SHIFT_FLAG,P_CAL,HHL_W,SF_BALA_PB," +
                        "GF_BALA_PB,GF_BALA_W,M_Y,M_P,MOI_1,MOI_2," +
                        "WTR_Q_HOUR,PH_STP_NUM,BED_THICK_AD_NUM," +
                        "BED_THICK,M_SPEED,M_C_SPEED,IG_T,BTP_D," +
                        "BTP_X,BTP_AV,TEMP_ZONE,BTP_ZONE_RATE," +
                        "ZONE_RATE_300,ZONE_RATE_450,MA_T_D,MA_T_X," +
                        "MA_T_AD,BTP_POS_D,BTP_POS_X,BTP_POS_AV," +
                        "MA_P_D,MA_P_X,MA_P_AV,V_BED_SPEED," +
                        "SINTER_END_T,COLD_END_T,COLD_FAN_NUM," +
                        "MA_HZ_D,MA_HZ_X,MA_HZ_AV,MA_CURT_D," +
                        "MA_CURT_X,MA_CURT_AV,M_FAN_CNSP_E," +
                        "FUEL_CNSP_PER,GAS_CNSP_PER " +
                        "from MC_NUMCAL_INTERFACE_10 " +
                        "where TIMESTAMP >= '" + time_begin + "' and TIMESTAMP <= '" + time_end + "' order by TIMESTAMP desc";
                    DataTable table = _dBSQL.GetCommand(sql_MC_NUMCAL_INTERFACE_10);
                    if (table.Rows.Count > 0)
                    {
                        int _YM_INDEX = 2;
                        for (int x = 0; x < table.Columns.Count; x++)
                        {
                            data[_YM_INDEX] = table.Rows[0][x].ToString();
                            _YM_INDEX = _YM_INDEX + 1;
                        }
                    }

                    data_2.Rows.Add(data);
                }
                d2.DataSource = data_2;
            }
            catch (Exception ee)
            {
                _vLog.writelog("SHOW_D2方法错误" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 参数修改弹出框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Frm_Production_state_Parameter form_display = new Frm_Production_state_Parameter();
            if (Frm_Production_state_Parameter.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 班生产弹出框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Frm_Production_state_Class form_display = new Frm_Production_state_Class();
            if (Frm_Production_state_Class.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 堆生产弹出框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Frm_Production_state_Heap form_display = new Frm_Production_state_Heap();
            if (Frm_Production_state_Heap.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            DateTime begin_time = Convert.ToDateTime(textBox_begin.Text);
            //结束时间
            DateTime end_time = Convert.ToDateTime(textBox_end.Text);
            //结束时间累加1h
            DateTime time_end = end_time.AddHours(1);
            DataTable table_MC_NUMCAL_INTERFACE_10 = new DataTable();
            string[] col_name = new string[] {
                "date",
                "time",
                "HEAP_NUM",
                "SHIFT_FLAG",
                "P_CAL",
                "HHL_W",
                "SF_BALA_PB",
                "GF_BALA_PB",
                "GF_BALA_W",
                "M_Y",
                "M_P",
                "MOI_1",
                "MOI_2",
                "WTR_Q_HOUR" ,
                "PH_STP_NUM",
                "BED_THICK_AD_NUM",
                "BED_THICK",
                "M_SPEED",
                "M_C_SPEED",
                "IG_T",
                "BTP_D",
                "BTP_X" ,
                "BTP_AV" ,
                "TEMP_ZONE" ,
                "BTP_ZONE_RATE",
                "ZONE_RATE_300",
                "ZONE_RATE_450",
                "MA_T_D",
                "MA_T_X",
                "MA_T_AD",
                "BTP_POS_D",
                "BTP_POS_X",
                "BTP_POS_AV",
                "MA_P_D",
                "MA_P_X",
                "MA_P_AV",
                "V_BED_SPEED",
                "SINTER_END_T",
                "COLD_END_T",
                "COLD_FAN_NUM",
                "MA_HZ_D",
                "MA_HZ_X",
                "MA_HZ_AV",
                "MA_CURT_D",
                "MA_CURT_X",
                "MA_CURT_AV",
                "M_FAN_CNSP_E",
                "FUEL_CNSP_PER",
                "GAS_CNSP_PER"
            };
            for (int x = 0; x < col_name.Count(); x++)
            {
                table_MC_NUMCAL_INTERFACE_10.Columns.Add(col_name[x]);
            }
            int count = 0;
            for (DateTime time = end_time; time > begin_time; time = time.AddHours(-1))
            {
                //日期
                string day_text = "";
                //时间段
                string time_day = "";
                DateTime time_1 = time_end.AddHours(-count - 1);

                //查询的开始时间
                string date_begin = "";
                day_text += time_1.Year.ToString() + "年";
                day_text += time_1.Month.ToString() + "月";
                day_text += time_1.Day.ToString() + "日";
                date_begin += time_1.Year.ToString();
                date_begin += "-" + time_1.Month.ToString();
                date_begin += "-" + time_1.Day.ToString();

                time_day += time_1.Hour.ToString();
                date_begin += " " + time_1.Hour.ToString();
                date_begin += ":00:00";

                DateTime time_2 = time_end.AddHours(-count);
                //查询的结束时间
                string date_end = "";
                //查询的结束时间

                date_end += time_2.Year.ToString();
                date_end += "-" + time_2.Month.ToString();
                date_end += "-" + time_2.Day.ToString();

                time_day += "~" + time_2.Hour.ToString();
                date_end += " " + time_2.Hour.ToString();
                date_end += ":00:00";
                count++;
                DataRow data = table_MC_NUMCAL_INTERFACE_10.NewRow();

                //通过自动生成的时间去检索MC_NUMCAL_INTERFACE_10表对应的数据
                string sql_MC_NUMCAL_INTERFACE_10 = "select " +
                     "HEAP_NUM,SHIFT_FLAG,P_CAL,HHL_W,SF_BALA_PB," +
                    "GF_BALA_PB,GF_BALA_W,M_Y,M_P,MOI_1,MOI_2," +
                    "WTR_Q_HOUR,PH_STP_NUM,BED_THICK_AD_NUM," +
                    "BED_THICK,M_SPEED,M_C_SPEED,IG_T,BTP_D," +
                    "BTP_X,BTP_AV,TEMP_ZONE,BTP_ZONE_RATE," +
                    "ZONE_RATE_300,ZONE_RATE_450,MA_T_D,MA_T_X," +
                    "MA_T_AD,BTP_POS_D,BTP_POS_X,BTP_POS_AV," +
                    "MA_P_D,MA_P_X,MA_P_AV,V_BED_SPEED," +
                    "SINTER_END_T,COLD_END_T,COLD_FAN_NUM," +
                    "MA_HZ_D,MA_HZ_X,MA_HZ_AV,MA_CURT_D," +
                    "MA_CURT_X,MA_CURT_AV,M_FAN_CNSP_E," +
                    "FUEL_CNSP_PER,GAS_CNSP_PER " +
                    " from MC_NUMCAL_INTERFACE_10 where TIMESTAMP >='" + date_begin + "' and TIMESTAMP <='" + date_end + "' order by TIMESTAMP desc";
                DataTable table = _dBSQL.GetCommand(sql_MC_NUMCAL_INTERFACE_10);
                if (table.Rows.Count > 0)
                {
                    data["date"] = day_text;
                    data["time"] = time_day;
                    int _YM_INDEX = 2;
                    for (int x = 0; x < table.Columns.Count; x++)
                    {
                        data[_YM_INDEX] = table.Rows[0][x].ToString();
                        _YM_INDEX = _YM_INDEX + 1;
                    }
                    table_MC_NUMCAL_INTERFACE_10.Rows.Add(data);
                }
            }
            d2.DataSource = table_MC_NUMCAL_INTERFACE_10;
        }

        /// <summary>
        /// 实时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
        }

        private void d2_Scroll(object sender, ScrollEventArgs e)
        {
            try
            {
                d2.ReDrawHead();
            }
            catch (Exception ee)
            {
                _vLog.writelog("d1_Scroll方法错误" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 定时器启用
        /// </summary>
        public void Timer_state()
        {
            // _Timer1.Enabled = true;
        }

        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {
            // _Timer1.Enabled = false;
        }

        /// <summary>
        /// 控件关闭
        /// </summary>
        public void _Clear()
        {
            //  _Timer1.Close();//释放定时器资源
            this.Dispose();//释放资源
            GC.Collect();//调用GC
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Frm_Production_state_Mon form_display = new Frm_Production_state_Mon();
            if (Frm_Production_state_Mon.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }
    }
}