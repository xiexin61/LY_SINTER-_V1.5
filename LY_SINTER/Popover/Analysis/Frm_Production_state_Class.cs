using DataBase;
using LY_SINTER.Custom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLog;

namespace LY_SINTER.Popover.Analysis
{
    public partial class Frm_Production_state_Class : Form
    {
        string[] col_name = new string[] {

                "time",

                "SHIFT_FLAG",
                "HEAP_NUM",
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
                "GAS_CNSP_PER",
                "M_ST_NUM",
                "M_ST_TIME"

            };
        public static bool isopen = false;
        DBSQL dBSQL = new DBSQL(DataBase.ConstParameters.strCon);
        public vLog _vLog { get; set; }
        public Frm_Production_state_Class()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Analysis\\Frm_Production_state_Class\\");
            time_begin_end();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            show();
            d2_col();
        }
        public void d2_col()
        {
            //添加列说明
            this.d2.AddSpanHeader(19, 7, "BTP温度");
            this.d2.AddSpanHeader(26, 3, "大烟道温度");
            this.d2.AddSpanHeader(29, 3, "BTP位置");
            this.d2.AddSpanHeader(32, 33, "大烟道负压");
            this.d2.AddSpanHeader(39, 33, "主抽频率");
            this.d2.AddSpanHeader(42, 3, "主排电流");

        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void show()
        {
            DataTable data_1 = new DataTable();
          
            for (int x = 0; x < col_name.Count(); x++)
            {
                data_1.Columns.Add(col_name[x]);
            }
            //当前系统时间
            DateTime date = DateTime.Now;
            textBox_end.Text = DateTime.Now.ToString();
            int day = date.Day;
            //生成本月月初时间
            DateTime time_old = date.AddDays(-day);

            string time_begin = "";
            time_begin += time_old.Year.ToString();
            time_begin += "-" + time_old.Month.ToString();
            time_begin += "-" + time_old.Day.ToString();
            time_begin += " 20:01:00";
            //当前系统的月初时间
            DateTime date1 = DateTime.Parse(time_begin);
            textBox_begin.Text = date1.ToString();
            int hour = date.Hour;
            //当前系统时间的班次开始时间
            string date_begin_1 = "";
            if (hour < 8)
            {
                DateTime dateTime = date.AddDays(-1);
                date_begin_1 += dateTime.Year.ToString();
                date_begin_1 += "-" + dateTime.Month.ToString();
                date_begin_1 += "-" + dateTime.Day.ToString();
                date_begin_1 += " 20:01:00";

            }
            else if (hour >= 8 && hour < 20)
            {
                date_begin_1 += date.Year.ToString();
                date_begin_1 += "-" + date.Month.ToString();
                date_begin_1 += "-" + date.Day.ToString();
                date_begin_1 += " 8:01:00";
            }
            else if (hour >= 20)
            {
                date_begin_1 += date.Year.ToString();
                date_begin_1 += "-" + date.Month.ToString();
                date_begin_1 += "-" + date.Day.ToString();
                date_begin_1 += " 20:01:00";
            }
            DateTime dateTime_begin = DateTime.Parse(date_begin_1);
            //月累计计算
            //当前班次的结束时间
            DateTime time_end = dateTime_begin.AddHours(12).AddMinutes(-1);
            string sql_MC_NUMCAL_INTERFACE_10_CLASS_1 = "select " +
                "sum(P_CAL) as P_CAL" +
                ",sum(HHL_W) as HHL_W" +
                ",sum(SF_BALA_PB) as SF_BALA_PB," +
                "sum(GF_BALA_PB) as GF_BALA_PB," +
                "sum(GF_BALA_W) as GF_BALA_W," +
                "sum(M_Y) as M_Y," +
                "sum(M_P) as M_P," +
                "sum(MOI_1) as MOI_1," +
                "sum(MOI_2) as MOI_2," +
                "sum(WTR_Q_HOUR) as WTR_Q_HOUR," +
                "sum(PH_STP_NUM) as PH_STP_NUM" +
                ",sum(BED_THICK_AD_NUM) as BED_THICK_AD_NUM," +
                "sum(BED_THICK) as BED_THICK," +
                "sum(M_SPEED) as M_SPEED" +
                ",sum(M_C_SPEED) as M_C_SPEED," +
                "sum(IG_T) as IG_T," +
                "sum(BTP_D) as BTP_D," +
                "sum(BTP_X) as BTP_X," +
                "sum(BTP_AV) as BTP_AV," +
                "sum(BTP_ZONE_RATE) as BTP_ZONE_RATE," +
                "sum(ZONE_RATE_300) as ZONE_RATE_300," +
                "sum(ZONE_RATE_450) as ZONE_RATE_450," +
                "sum(MA_T_D) as MA_T_D," +
                "sum(MA_T_X) as MA_T_X," +
                "sum(MA_T_AD) as MA_T_AD," +
                "sum(BTP_POS_D) as BTP_POS_D," +
                "sum(BTP_POS_X) as BTP_POS_X," +
                "sum(BTP_POS_AV) as BTP_POS_AV," +
                "sum(MA_P_D) as MA_P_D," +
                "sum(MA_P_X) as MA_P_X," +
                "sum(MA_P_AV) as MA_P_AV," +
                "sum(V_BED_SPEED) as V_BED_SPEED," +
                "sum(SINTER_END_T) as SINTER_END_T," +
                "sum(COLD_END_T) as COLD_END_T," +
                "sum(COLD_FAN_NUM) as COLD_FAN_NUM," +
                "sum(MA_HZ_D) as MA_HZ_D," +
                "sum(MA_HZ_X) as MA_HZ_X," +
                "sum(MA_HZ_AV) as MA_HZ_AV," +
                "sum(MA_CURT_D) as MA_CURT_D," +
                "sum(MA_CURT_X) as MA_CURT_X," +
                "sum(MA_CURT_AV) as MA_CURT_AV," +
                "sum(M_FAN_CNSP_E) as M_FAN_CNSP_E," +
                "sum(FUEL_CNSP_PER) as FUEL_CNSP_PER," +
                "sum(GAS_CNSP_PER) as GAS_CNSP_PER," +
                "sum(M_ST_NUM) as M_ST_NUM," +
                "sum(M_ST_TIME) as M_ST_TIME " +
                "from MC_NUMCAL_INTERFACE_10_CLASS where TIMESTAMP >= '" + date1 + "' and TIMESTAMP <= '" + time_end + "' ";
            DataTable table_1 = dBSQL.GetCommand(sql_MC_NUMCAL_INTERFACE_10_CLASS_1);
            if (table_1.Rows.Count > 0)
            {
                DataRow row_1 = data_1.NewRow();
                row_1["time"] = "当月累计";
                row_1["P_CAL"] = table_1.Rows[0]["P_CAL"].ToString();
                row_1["HHL_W"] = table_1.Rows[0]["HHL_W"].ToString();
                row_1["SF_BALA_PB"] = table_1.Rows[0]["SF_BALA_PB"].ToString();
                row_1["GF_BALA_PB"] = table_1.Rows[0]["GF_BALA_PB"].ToString();
                row_1["GF_BALA_W"] = table_1.Rows[0]["GF_BALA_W"].ToString();
                row_1["M_Y"] = table_1.Rows[0]["M_Y"].ToString();
                row_1["M_P"] = table_1.Rows[0]["M_P"].ToString();
                row_1["MOI_1"] = table_1.Rows[0]["MOI_1"].ToString();
                row_1["MOI_2"] = table_1.Rows[0]["MOI_2"].ToString();
                row_1["WTR_Q_HOUR"] = table_1.Rows[0]["WTR_Q_HOUR"].ToString();
                row_1["PH_STP_NUM"] = table_1.Rows[0]["PH_STP_NUM"].ToString();
                row_1["BED_THICK_AD_NUM"] = table_1.Rows[0]["BED_THICK_AD_NUM"].ToString();
                row_1["BED_THICK"] = table_1.Rows[0]["BED_THICK"].ToString();
                row_1["M_SPEED"] = table_1.Rows[0]["M_SPEED"].ToString();
                row_1["M_C_SPEED"] = table_1.Rows[0]["M_C_SPEED"].ToString();
                row_1["IG_T"] = table_1.Rows[0]["IG_T"].ToString();
                row_1["BTP_D"] = table_1.Rows[0]["BTP_D"].ToString();
                row_1["BTP_X"] = table_1.Rows[0]["BTP_X"].ToString();
                row_1["BTP_AV"] = table_1.Rows[0]["BTP_AV"].ToString();
                row_1["BTP_ZONE_RATE"] = table_1.Rows[0]["BTP_ZONE_RATE"].ToString();
                row_1["ZONE_RATE_300"] = table_1.Rows[0]["ZONE_RATE_300"].ToString();
                row_1["ZONE_RATE_450"] = table_1.Rows[0]["ZONE_RATE_450"].ToString();
                row_1["MA_T_D"] = table_1.Rows[0]["MA_T_D"].ToString();
                row_1["MA_T_X"] = table_1.Rows[0]["MA_T_X"].ToString();
                row_1["MA_T_AD"] = table_1.Rows[0]["MA_T_AD"].ToString();
                row_1["BTP_POS_D"] = table_1.Rows[0]["BTP_POS_D"].ToString();
                row_1["BTP_POS_X"] = table_1.Rows[0]["BTP_POS_X"].ToString();
                row_1["BTP_POS_AV"] = table_1.Rows[0]["BTP_POS_AV"].ToString();
                row_1["MA_P_D"] = table_1.Rows[0]["MA_P_D"].ToString();
                row_1["MA_P_X"] = table_1.Rows[0]["MA_P_X"].ToString();
                row_1["MA_P_AV"] = table_1.Rows[0]["MA_P_AV"].ToString();
                row_1["V_BED_SPEED"] = table_1.Rows[0]["V_BED_SPEED"].ToString();
                row_1["SINTER_END_T"] = table_1.Rows[0]["SINTER_END_T"].ToString();
                row_1["COLD_END_T"] = table_1.Rows[0]["COLD_END_T"].ToString();
                row_1["COLD_FAN_NUM"] = table_1.Rows[0]["COLD_FAN_NUM"].ToString();
                row_1["MA_HZ_D"] = table_1.Rows[0]["MA_HZ_D"].ToString();
                row_1["MA_HZ_X"] = table_1.Rows[0]["MA_HZ_X"].ToString();
                row_1["MA_HZ_AV"] = table_1.Rows[0]["MA_HZ_AV"].ToString();
                row_1["MA_CURT_D"] = table_1.Rows[0]["MA_CURT_D"].ToString();
                row_1["MA_CURT_X"] = table_1.Rows[0]["MA_CURT_X"].ToString();
                row_1["MA_CURT_AV"] = table_1.Rows[0]["MA_CURT_AV"].ToString();
                row_1["M_FAN_CNSP_E"] = table_1.Rows[0]["M_FAN_CNSP_E"].ToString();
                row_1["FUEL_CNSP_PER"] = table_1.Rows[0]["FUEL_CNSP_PER"].ToString();
                row_1["GAS_CNSP_PER"] = table_1.Rows[0]["GAS_CNSP_PER"].ToString();
                row_1["M_ST_NUM"] = table_1.Rows[0]["M_ST_NUM"].ToString();
                row_1["M_ST_TIME"] = table_1.Rows[0]["M_ST_TIME"].ToString();
                data_1.Rows.Add(row_1);
            }

            for (DateTime time = dateTime_begin; time >= date1; time = time.AddHours(-12))
            {
                DataRow row_2 = data_1.NewRow();
                //周期查询班次的开始时间
                DateTime time_1 = time;
                //周期查询班次的结束时间
                DateTime time_2 = time.AddHours(12).AddMinutes(-1);
                string sql = "select " +
                    "HEAP_NUM,SHIFT_FLAG,P_CAL,HHL_W," +
                    "SF_BALA_PB,GF_BALA_PB,GF_BALA_W," +
                    "M_Y,M_P,MOI_1,MOI_2,WTR_Q_HOUR," +
                    "PH_STP_NUM,BED_THICK_AD_NUM,BED_THICK," +
                    "M_SPEED,M_C_SPEED,IG_T,BTP_D,BTP_X," +
                    "BTP_AV,TEMP_ZONE,BTP_ZONE_RATE," +
                    "ZONE_RATE_300,ZONE_RATE_450,MA_T_D," +
                    "MA_T_X,MA_T_AD,BTP_POS_D,BTP_POS_X," +
                    "BTP_POS_AV,MA_P_D,MA_P_X,MA_P_AV," +
                    "V_BED_SPEED,SINTER_END_T,COLD_END_T," +
                    "COLD_FAN_NUM,MA_HZ_D,MA_HZ_X,MA_HZ_AV," +
                    "MA_CURT_D,MA_CURT_X,MA_CURT_AV," +
                    "M_FAN_CNSP_E,FUEL_CNSP_PER,GAS_CNSP_PER," +
                    "M_ST_NUM,M_ST_TIME " +
                    " " +
                    "from MC_NUMCAL_INTERFACE_10_CLASS " +
                    "where TIMESTAMP >= '" + time_1 + "' and TIMESTAMP <= '" + time_2 + "' order by TIMESTAMP desc";
                DataTable table_2 = dBSQL.GetCommand(sql);
                if (table_2.Rows.Count > 0)
                {
                    //判断班次时间
                    int class_hour = time_1.Hour;
                    string class_time = "";
                    if (class_hour == 8)
                    {
                        class_time += time_1.Year.ToString();
                        class_time += "年" + time_1.Month.ToString();
                        class_time += "月" + time_1.Day.ToString() + "日-白班";
                    }
                    else
                    {
                        class_time += time_1.Year.ToString();
                        class_time += "年" + time_1.Month.ToString();
                        class_time += "月" + time_1.Day.ToString() + "日-夜班";
                    }
                    row_2["time"] = class_time;
                    row_2["SHIFT_FLAG"] = table_2.Rows[0]["SHIFT_FLAG"].ToString();
                    row_2["P_CAL"] = table_2.Rows[0]["P_CAL"].ToString();
                    row_2["HHL_W"] = table_2.Rows[0]["HHL_W"].ToString();
                    row_2["SF_BALA_PB"] = table_2.Rows[0]["SF_BALA_PB"].ToString();
                    row_2["GF_BALA_PB"] = table_2.Rows[0]["GF_BALA_PB"].ToString();
                    row_2["GF_BALA_W"] = table_2.Rows[0]["GF_BALA_W"].ToString();
                    row_2["M_Y"] = table_2.Rows[0]["M_Y"].ToString();
                    row_2["M_P"] = table_2.Rows[0]["M_P"].ToString();
                    row_2["MOI_1"] = table_2.Rows[0]["MOI_1"].ToString();
                    row_2["MOI_2"] = table_2.Rows[0]["MOI_2"].ToString();
                    row_2["WTR_Q_HOUR"] = table_2.Rows[0]["WTR_Q_HOUR"].ToString();
                    row_2["PH_STP_NUM"] = table_2.Rows[0]["PH_STP_NUM"].ToString();
                    row_2["BED_THICK_AD_NUM"] = table_2.Rows[0]["BED_THICK_AD_NUM"].ToString();
                    row_2["BED_THICK"] = table_2.Rows[0]["BED_THICK"].ToString();
                    row_2["M_SPEED"] = table_2.Rows[0]["M_SPEED"].ToString();
                    row_2["M_C_SPEED"] = table_2.Rows[0]["M_C_SPEED"].ToString();
                    row_2["IG_T"] = table_2.Rows[0]["IG_T"].ToString();
                    row_2["BTP_D"] = table_2.Rows[0]["BTP_D"].ToString();
                    row_2["BTP_X"] = table_2.Rows[0]["BTP_X"].ToString();
                    row_2["BTP_AV"] = table_2.Rows[0]["BTP_AV"].ToString();
                    row_2["TEMP_ZONE"] = table_2.Rows[0]["TEMP_ZONE"].ToString();
                    row_2["BTP_ZONE_RATE"] = table_2.Rows[0]["BTP_ZONE_RATE"].ToString();
                    row_2["ZONE_RATE_300"] = table_2.Rows[0]["ZONE_RATE_300"].ToString();
                    row_2["ZONE_RATE_450"] = table_2.Rows[0]["ZONE_RATE_450"].ToString();
                    row_2["MA_T_D"] = table_2.Rows[0]["MA_T_D"].ToString();
                    row_2["MA_T_X"] = table_2.Rows[0]["MA_T_X"].ToString();
                    row_2["MA_T_AD"] = table_2.Rows[0]["MA_T_AD"].ToString();
                    row_2["BTP_POS_D"] = table_2.Rows[0]["BTP_POS_D"].ToString();
                    row_2["BTP_POS_X"] = table_2.Rows[0]["BTP_POS_X"].ToString();
                    row_2["BTP_POS_AV"] = table_2.Rows[0]["BTP_POS_AV"].ToString();
                    row_2["MA_P_D"] = table_2.Rows[0]["MA_P_D"].ToString();
                    row_2["MA_P_X"] = table_2.Rows[0]["MA_P_X"].ToString();
                    row_2["MA_P_AV"] = table_2.Rows[0]["MA_P_AV"].ToString();
                    row_2["V_BED_SPEED"] = table_2.Rows[0]["V_BED_SPEED"].ToString();
                    row_2["SINTER_END_T"] = table_2.Rows[0]["SINTER_END_T"].ToString();
                    row_2["COLD_END_T"] = table_2.Rows[0]["COLD_END_T"].ToString();
                    row_2["COLD_FAN_NUM"] = table_2.Rows[0]["COLD_FAN_NUM"].ToString();
                    row_2["MA_HZ_D"] = table_2.Rows[0]["MA_HZ_D"].ToString();
                    row_2["MA_HZ_X"] = table_2.Rows[0]["MA_HZ_X"].ToString();
                    row_2["MA_HZ_AV"] = table_2.Rows[0]["MA_HZ_AV"].ToString();
                    row_2["MA_CURT_D"] = table_2.Rows[0]["MA_CURT_D"].ToString();
                    row_2["MA_CURT_X"] = table_2.Rows[0]["MA_CURT_X"].ToString();
                    row_2["MA_CURT_AV"] = table_2.Rows[0]["MA_CURT_AV"].ToString();
                    row_2["M_FAN_CNSP_E"] = table_2.Rows[0]["M_FAN_CNSP_E"].ToString();
                    row_2["FUEL_CNSP_PER"] = table_2.Rows[0]["FUEL_CNSP_PER"].ToString();
                    row_2["GAS_CNSP_PER"] = table_2.Rows[0]["GAS_CNSP_PER"].ToString();
                    row_2["M_ST_NUM"] = table_2.Rows[0]["M_ST_NUM"].ToString();
                    row_2["M_ST_TIME"] = table_2.Rows[0]["M_ST_TIME"].ToString();
                    data_1.Rows.Add(row_2);
                }

            }
            d2.DataSource = data_1;
        }
        /// <summary>
        /// 开始时间-结束时间赋值
        /// </summary>
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddDays(-7);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            DataTable data_1 = new DataTable();
            string[] col_name = new string[] {

                "time",

                "SHIFT_FLAG",
                "HEAP_NUM",
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
                "GAS_CNSP_PER",
                "M_ST_NUM",
                "M_ST_TIME",
                "SA_PB",
                "AUS_PB",
                "FINE_PB",
            };
            for (int x = 0; x < col_name.Count(); x++)
            {
                data_1.Columns.Add(col_name[x]);
            }
            //查询开始时间
            DateTime date_1 = Convert.ToDateTime(textBox_begin.Text);
            string time1 = "";
            time1 += date_1.Year.ToString();
            time1 += "-" + date_1.Month.ToString();
            time1 += "-" + date_1.Day.ToString();
            time1 += " 08:01:00";
            DateTime time_begin = DateTime.Parse(time1);
            //查询结束时间
            DateTime date_2 = Convert.ToDateTime(textBox_end.Text);
            string time2 = "";
            time2 += date_2.Year.ToString();
            time2 += "-" + date_2.Month.ToString();
            time2 += "-" + date_2.Day.ToString();
            time2 += " 20:01:00";
            DateTime time_end = DateTime.Parse(time2);
            //生成本月月初时间

            //   DateTime time_end = dateTime_begin.AddHours(12).AddMinutes(-1);
            for (DateTime time = time_end; time >= time_begin; time = time.AddHours(-12))
            {
                DataRow row_2 = data_1.NewRow();
                //周期查询班次的开始时间
                DateTime time_1 = time;
                //周期查询班次的结束时间
                DateTime time_2 = time.AddHours(12).AddMinutes(-1);
                string sql = "select " +
                    "HEAP_NUM,SHIFT_FLAG,P_CAL,HHL_W," +
                    "SF_BALA_PB,GF_BALA_PB,GF_BALA_W," +
                    "M_Y,M_P,MOI_1,MOI_2,WTR_Q_HOUR," +
                    "PH_STP_NUM,BED_THICK_AD_NUM,BED_THICK," +
                    "M_SPEED,M_C_SPEED,IG_T,BTP_D,BTP_X," +
                    "BTP_AV,TEMP_ZONE,BTP_ZONE_RATE," +
                    "ZONE_RATE_300,ZONE_RATE_450,MA_T_D," +
                    "MA_T_X,MA_T_AD,BTP_POS_D,BTP_POS_X," +
                    "BTP_POS_AV,MA_P_D,MA_P_X,MA_P_AV," +
                    "V_BED_SPEED,SINTER_END_T,COLD_END_T," +
                    "COLD_FAN_NUM,MA_HZ_D,MA_HZ_X,MA_HZ_AV," +
                    "MA_CURT_D,MA_CURT_X,MA_CURT_AV," +
                    "M_FAN_CNSP_E,FUEL_CNSP_PER,GAS_CNSP_PER," +
                    "M_ST_NUM,M_ST_TIME,SA_PB,AUS_PB," +
                    "FINE_PB " +
                    "from MC_NUMCAL_INTERFACE_10_CLASS " +
                    "where TIMESTAMP >= '" + time_1 + "' and TIMESTAMP <= '" + time_2 + "' order by TIMESTAMP desc";
                DataTable table_2 = dBSQL.GetCommand(sql);
                if (table_2.Rows.Count > 0)
                {
                    //判断班次时间
                    int class_hour = time_1.Hour;
                    string class_time = "";
                    if (class_hour == 8)
                    {
                        class_time += time_1.Year.ToString();
                        class_time += "年" + time_1.Month.ToString();
                        class_time += "月" + time_1.Day.ToString() + "日-白班";
                    }
                    else
                    {
                        class_time += time_1.Year.ToString();
                        class_time += "年" + time_1.Month.ToString();
                        class_time += "月" + time_1.Day.ToString() + "日-夜班";
                    }
                    row_2["time"] = class_time;
                    row_2["SHIFT_FLAG"] = table_2.Rows[0]["SHIFT_FLAG"].ToString();
                    row_2["P_CAL"] = table_2.Rows[0]["P_CAL"].ToString();
                    row_2["HHL_W"] = table_2.Rows[0]["HHL_W"].ToString();
                    row_2["SF_BALA_PB"] = table_2.Rows[0]["SF_BALA_PB"].ToString();
                    row_2["GF_BALA_PB"] = table_2.Rows[0]["GF_BALA_PB"].ToString();
                    row_2["GF_BALA_W"] = table_2.Rows[0]["GF_BALA_W"].ToString();
                    row_2["M_Y"] = table_2.Rows[0]["M_Y"].ToString();
                    row_2["M_P"] = table_2.Rows[0]["M_P"].ToString();
                    row_2["MOI_1"] = table_2.Rows[0]["MOI_1"].ToString();
                    row_2["MOI_2"] = table_2.Rows[0]["MOI_2"].ToString();
                    row_2["WTR_Q_HOUR"] = table_2.Rows[0]["WTR_Q_HOUR"].ToString();
                    row_2["PH_STP_NUM"] = table_2.Rows[0]["PH_STP_NUM"].ToString();
                    row_2["BED_THICK_AD_NUM"] = table_2.Rows[0]["BED_THICK_AD_NUM"].ToString();
                    row_2["BED_THICK"] = table_2.Rows[0]["BED_THICK"].ToString();
                    row_2["M_SPEED"] = table_2.Rows[0]["M_SPEED"].ToString();
                    row_2["M_C_SPEED"] = table_2.Rows[0]["M_C_SPEED"].ToString();
                    row_2["IG_T"] = table_2.Rows[0]["IG_T"].ToString();
                    row_2["BTP_D"] = table_2.Rows[0]["BTP_D"].ToString();
                    row_2["BTP_X"] = table_2.Rows[0]["BTP_X"].ToString();
                    row_2["BTP_AV"] = table_2.Rows[0]["BTP_AV"].ToString();
                    row_2["TEMP_ZONE"] = table_2.Rows[0]["TEMP_ZONE"].ToString();
                    row_2["BTP_ZONE_RATE"] = table_2.Rows[0]["BTP_ZONE_RATE"].ToString();
                    row_2["ZONE_RATE_300"] = table_2.Rows[0]["ZONE_RATE_300"].ToString();
                    row_2["ZONE_RATE_450"] = table_2.Rows[0]["ZONE_RATE_450"].ToString();
                    row_2["MA_T_D"] = table_2.Rows[0]["MA_T_D"].ToString();
                    row_2["MA_T_X"] = table_2.Rows[0]["MA_T_X"].ToString();
                    row_2["MA_T_AD"] = table_2.Rows[0]["MA_T_AD"].ToString();
                    row_2["BTP_POS_D"] = table_2.Rows[0]["BTP_POS_D"].ToString();
                    row_2["BTP_POS_X"] = table_2.Rows[0]["BTP_POS_X"].ToString();
                    row_2["BTP_POS_AV"] = table_2.Rows[0]["BTP_POS_AV"].ToString();
                    row_2["MA_P_D"] = table_2.Rows[0]["MA_P_D"].ToString();
                    row_2["MA_P_X"] = table_2.Rows[0]["MA_P_X"].ToString();
                    row_2["MA_P_AV"] = table_2.Rows[0]["MA_P_AV"].ToString();
                    row_2["V_BED_SPEED"] = table_2.Rows[0]["V_BED_SPEED"].ToString();
                    row_2["SINTER_END_T"] = table_2.Rows[0]["SINTER_END_T"].ToString();
                    row_2["COLD_END_T"] = table_2.Rows[0]["COLD_END_T"].ToString();
                    row_2["COLD_FAN_NUM"] = table_2.Rows[0]["COLD_FAN_NUM"].ToString();
                    row_2["MA_HZ_D"] = table_2.Rows[0]["MA_HZ_D"].ToString();
                    row_2["MA_HZ_X"] = table_2.Rows[0]["MA_HZ_X"].ToString();
                    row_2["MA_HZ_AV"] = table_2.Rows[0]["MA_HZ_AV"].ToString();
                    row_2["MA_CURT_D"] = table_2.Rows[0]["MA_CURT_D"].ToString();
                    row_2["MA_CURT_X"] = table_2.Rows[0]["MA_CURT_X"].ToString();
                    row_2["MA_CURT_AV"] = table_2.Rows[0]["MA_CURT_AV"].ToString();
                    row_2["M_FAN_CNSP_E"] = table_2.Rows[0]["M_FAN_CNSP_E"].ToString();
                    row_2["FUEL_CNSP_PER"] = table_2.Rows[0]["FUEL_CNSP_PER"].ToString();
                    row_2["GAS_CNSP_PER"] = table_2.Rows[0]["GAS_CNSP_PER"].ToString();
                    row_2["M_ST_NUM"] = table_2.Rows[0]["M_ST_NUM"].ToString();
                    row_2["M_ST_TIME"] = table_2.Rows[0]["M_ST_TIME"].ToString();
                    row_2["SA_PB"] = table_2.Rows[0]["SA_PB"].ToString();
                    row_2["AUS_PB"] = table_2.Rows[0]["AUS_PB"].ToString();
                    row_2["FINE_PB"] = table_2.Rows[0]["FINE_PB"].ToString();
                    data_1.Rows.Add(row_2);
                }

            }
            d2.DataSource = data_1;
        }
        /// <summary>
        /// 实时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            show();
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Execl files (*.xls)|*.xls";
            dlg.FilterIndex = 0;
            dlg.RestoreDirectory = true;
            dlg.CreatePrompt = true;
            dlg.Title = "保存为Excel文件";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Stream myStream;
                myStream = dlg.OpenFile();
                StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                string columnTitle = "";
                try
                {
                    //写入列标题    
                    for (int i = 0; i < d2.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            columnTitle += "\t";
                        }
                        columnTitle += d2.Columns[i].HeaderText;
                    }
                    sw.WriteLine(columnTitle);

                    //写入列内容    
                    for (int j = 0; j < d2.Rows.Count; j++)
                    {
                        string columnValue = "";
                        for (int k = 0; k < d2.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                columnValue += "\t";
                            }
                            if (d2.Rows[j].Cells[k].Value == null)
                                columnValue += "";
                            else
                                columnValue += d2.Rows[j].Cells[k].Value.ToString().Trim();
                        }
                        sw.WriteLine(columnValue);
                    }
                    sw.Close();
                    myStream.Close();
                }
                catch (Exception ee)
                {
                    MessageBox.Show(e.ToString());
                }
                finally
                {
                    sw.Close();
                    myStream.Close();
                }
            }
        }

        private void d2_Scroll(object sender, ScrollEventArgs e)
        {
            d2_col();
        }
    }
}
