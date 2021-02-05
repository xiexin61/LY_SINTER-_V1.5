using DataBase;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLog;

namespace LY_SINTER.Popover.Course
{
    public partial class Frm_BTP_Guide : Form
    {
        public vLog vLog { get; set; }
        /// <summary>
        /// 时间前进标志位
        /// </summary>
        int time_min_GO = -3;
        /// <summary>
        /// 时间后退标志位
        /// </summary>
        int time_min_retreat = -2;
        /// <summary>
        ///  前进按钮对应时间
        /// </summary>
        DateTime time_go = DateTime.Now;
        /// <summary>
        ///  后退按钮对应时间
        /// </summary>
        DateTime time_retreat = DateTime.Now;

        #region 曲线数据声明
        /// <summary>
        /// 名称x轴数据
        /// </summary>
        List<string> time_x = new List<string> { "1", "2", "3", "4", "5", "6", "平均" };

        /// <summary>
        /// TRP位置目标值 y
        /// </summary>
        List<double> TRP_X_MB = new List<double>();
        /// <summary>
        /// TRP位置 位置 y
        /// </summary>
        List<double> TRP_X_WZ = new List<double>();

        /// <summary>
        /// TRP温度 y
        /// </summary>
        List<double> TRP_TH_WD = new List<double>();


        /// <summary>
        /// BRP位置目标值 y
        /// </summary>
        List<double> BRP_X_MB = new List<double>();
        /// <summary>
        /// BRP位置 位置 y
        /// </summary>
        List<double> BRP_X_WZ = new List<double>();

        /// <summary>
        /// BRP温度 y
        /// </summary>
        List<double> BRP_TH_WD = new List<double>();


        /// <summary>
        /// BTP位置目标值 y
        /// </summary>
        List<double> BTP_X_MB = new List<double>();
        /// <summary>
        /// BTP位置 位置 y
        /// </summary>
        List<double> BTP_X_WZ = new List<double>();

        /// <summary>
        /// BTP温度 y
        /// </summary>
        List<double> BTP_TH_WD = new List<double>();


        /// <summary>
        /// TRP偏差
        /// </summary>
        List<double> TRP_PC = new List<double>();
        /// <summary>
        /// BRP偏差
        /// </summary>
        List<double> BRP_PC = new List<double>();
        /// <summary>
        /// BTP偏差
        /// </summary>
        List<double> BTP_PC = new List<double>();
        #endregion

        #region 曲线柱形图声明 
        /// <summary>
        /// TRP 位置
        /// </summary>
        ColumnSeries COLUM_BAR_TRP_X { get; set; }
        /// <summary>
        /// TRP温度
        /// </summary>
        ColumnSeries COLUM_BAR_TRP_TH { get; set; }

        /// <summary>
        /// BRP位置
        /// </summary>
        ColumnSeries COLUM_BAR_BRP_X { get; set; }
        /// <summary>
        /// BRP温度
        /// </summary>
        ColumnSeries COLUM_BAR_BRP_TH { get; set; }

        /// <summary>
        /// BTP位置
        /// </summary>
        ColumnSeries COLUM_BAR_BTP_X { get; set; }
        /// <summary>
        /// BTP温度
        /// </summary>
        ColumnSeries COLUM_BAR_BTP_TH { get; set; }

        /// <summary>
        /// TRP偏差
        /// </summary>
        ColumnSeries COLUM_BAR_TRP_PC { get; set; }
        /// <summary>
        /// BRP偏差
        /// </summary>
        ColumnSeries COLUM_BAR_BRP_PC { get; set; }
        /// <summary>
        /// BTP偏差
        /// </summary>
        ColumnSeries COLUM_BAR_BTP_PC { get; set; }
        #endregion

        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        /// <summary>
        /// 曲线背景颜色
        /// </summary>
        Color Color_curve = Color.White;
        public static bool isopen = false;
        public Frm_BTP_Guide()
        {
            InitializeComponent();
            if (vLog == null)
                vLog = new vLog(".\\log_Page\\Course\\Frm_BTP_Guide\\");
            curve_MBWZ();
            curve();
            statement_surve();
            curve_text();
        }
        /// <summary>
        /// 实时按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //重置时间前进后退分钟标志位
            time_min_GO = -1;
            time_min_retreat = 0;
            time_go = DateTime.Now;
            time_retreat = DateTime.Now;
            LIST_CLRAE();
            curve_MBWZ();
            curve();
            curve_text();
        }
        /// <summary>
        /// 时间前进按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            time_min_GO = time_min_GO - 1;
            time_min_retreat = time_min_retreat - 1;
            LIST_CLRAE();
            curve_MBWZ();
            curve();
            curve_text();
        }
        /// <summary>
        /// 时间后退按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            time_min_GO = time_min_GO + 1;
            time_min_retreat = time_min_retreat + 1;
            LIST_CLRAE();
            curve_MBWZ();
            curve();
            curve_text();
        }
        /// <summary>
        /// 曲线 数据
        /// </summary>
        public void curve()
        {
            try
            {


                if (time_min_retreat >= 0)
                {
                    button_time_end.Visible = false;
                }
                else
                {
                    button_time_end.Visible = true;
                }
                //开始时间
                DateTime time_begin = time_go.AddMinutes(time_min_GO);
                //  this.label8.Text = time_begin.ToString();
                //结束时间
                DateTime time_end = time_retreat.AddMinutes(time_min_retreat);
                //  this.label9.Text = time_end.ToString();
                //最新调整时间
                this.label10.Text = "最新查询时间:" + time_end.ToString();

                #region  TRP位置
                //位置
                string sql_TRP_X = "SELECT top (1) " +
                    "isnull(TRP_X_1,0) as TRP_X_1," +
                    "isnull(TRP_X_2,0) as TRP_X_2," +
                    "isnull(TRP_X_3,0) as TRP_X_3," +
                    "isnull(TRP_X_4,0) as TRP_X_4," +
                    "isnull(TRP_X_5,0) as TRP_X_5," +
                    "isnull(TRP_X_6,0) as TRP_X_6," +
                    "isnull(TRP_X_AVG,0) as TRP_X_AVG " +
                    "FROM MC_BTPCAL_INTERFACE_HIST " +
                    "WHERE  TIMESTAMP >'" + time_begin + "' AND TIMESTAMP <='" + time_end + "' order by TIMESTAMP desc";
                DataTable data_TRP_X = dBSQL.GetCommand(sql_TRP_X);
                //Y轴
                if (data_TRP_X.Rows.Count > 0)
                {
                    for (int col = 0; col < data_TRP_X.Columns.Count; col++)
                    {
                        TRP_X_WZ.Add(Math.Round(double.Parse(data_TRP_X.Rows[0][col].ToString()), 2));
                    }
                }
                else
                {
                    for (int col = 0; col < data_TRP_X.Columns.Count; col++)
                    {
                        TRP_X_WZ.Add(0);
                    }
                }


                #endregion

                #region TRP温度
                string sql_TRP_TE = "SELECT TOP (1) " +
                    "ISNULL(TRP_TE_1,0) AS TRP_TE_1," +
                    "ISNULL(TRP_TE_2,0) AS TRP_TE_2," +
                    "ISNULL(TRP_TE_3,0) AS TRP_TE_3," +
                    "ISNULL(TRP_TE_4,0) AS TRP_TE_4," +
                    "ISNULL(TRP_TE_5,0) AS TRP_TE_5," +
                    "ISNULL(TRP_TE_6,0) AS TRP_TE_6," +
                    "ISNULL(TRP_TE_AVG,0) AS TRP_TE_AVG " +
                    "FROM MC_BTPCAL_INTERFACE_HIST " +
                    "WHERE  TIMESTAMP >'" + time_begin + "' AND TIMESTAMP <='" + time_end + "' ORDER BY TIMESTAMP DESC";
                DataTable data_TRP_TE = dBSQL.GetCommand(sql_TRP_TE);
                //Y轴
                if (data_TRP_TE.Rows.Count > 0)
                {
                    for (int COL = 0; COL < data_TRP_TE.Columns.Count; COL++)
                    {
                        TRP_TH_WD.Add(Math.Round(double.Parse(data_TRP_TE.Rows[0][COL].ToString()), 2));
                    }
                }
                else
                {
                    for (int COL = 0; COL < data_TRP_TE.Columns.Count; COL++)
                    {
                        TRP_TH_WD.Add(0);
                    }
                }
                //X轴
                #endregion

                #region BRP位置
                string sql_BRP_X = "SELECT TOP (1) " +
                    "ISNULL(BRP_X_1,0) AS BRP_X_1," +
                    "ISNULL(BRP_X_2,0) AS BRP_X_2," +
                    "ISNULL(BRP_X_3,0) AS BRP_X_3," +
                    "ISNULL(BRP_X_4,0) AS BRP_X_4," +
                    "ISNULL(BRP_X_5,0) AS BRP_X_5," +
                    "ISNULL(BRP_X_6,0) AS BRP_X_6," +
                    "ISNULL(BRP_X_AVG,0) AS BRP_X_AVG " +
                    "FROM MC_BTPCAL_INTERFACE_HIST " +
                    "WHERE  TIMESTAMP >'" + time_begin + "' AND TIMESTAMP <='" + time_end + "' ORDER BY TIMESTAMP DESC";
                DataTable data_BRP_X = dBSQL.GetCommand(sql_BRP_X);
                //Y轴
                if (data_BRP_X.Rows.Count > 0)
                {
                    for (int COL = 0; COL < data_BRP_X.Columns.Count; COL++)
                    {
                        BRP_X_WZ.Add(Math.Round(double.Parse(data_BRP_X.Rows[0][COL].ToString()), 2));
                    }
                }
                else
                {
                    for (int COL = 0; COL < data_BRP_X.Columns.Count; COL++)
                    {
                        BRP_X_WZ.Add(0);
                    }
                }
                //X轴
                #endregion

                #region BRP温度
                string sql_BRP_TE = "SELECT TOP (1) " +
                    "ISNULL(BRP_TE_1,0) AS BRP_TE_1," +
                    "ISNULL(BRP_TE_2,0) AS BRP_TE_2," +
                    "ISNULL(BRP_TE_3,0) AS BRP_TE_3," +
                    "ISNULL(BRP_TE_4,0) AS BRP_TE_4," +
                    "ISNULL(BRP_TE_5,0) AS BRP_TE_5," +
                    "ISNULL(BRP_TE_6,0) AS BRP_TE_6," +
                    "ISNULL(BRP_TE_AVG,0) AS BRP_TE_AVG " +
                    "FROM MC_BTPCAL_INTERFACE_HIST " +
                    "WHERE  TIMESTAMP >'" + time_begin + "' AND TIMESTAMP <='" + time_end + "' ORDER BY TIMESTAMP DESC";
                DataTable data_BRP_TE = dBSQL.GetCommand(sql_BRP_TE);
                //Y轴
                if (data_BRP_TE.Rows.Count > 0)
                {
                    for (int COL = 0; COL < data_BRP_TE.Columns.Count; COL++)
                    {
                        BRP_TH_WD.Add(Math.Round(double.Parse(data_BRP_TE.Rows[0][COL].ToString()), 2));
                    }
                }
                else
                {
                    for (int COL = 0; COL < data_BRP_TE.Columns.Count; COL++)
                    {
                        BRP_TH_WD.Add(0);
                    }

                }
                //X轴
                #endregion

                #region BTP位置
                string sql_BTP_X = "SELECT TOP (1) " +
                    "ISNULL(BTP_X_1,0) AS BTP_X_1," +
                    "ISNULL(BTP_X_2,0) AS BTP_X_2," +
                    "ISNULL(BTP_X_3,0) AS BTP_X_3," +
                    "ISNULL(BTP_X_4,0) AS BTP_X_4 ," +
                    "ISNULL(BTP_X_5,0) AS BTP_X_5," +
                    "ISNULL(BTP_X_6,0) AS BTP_X_6," +
                    "ISNULL(BTP_X_AVG,0) AS BTP_X_AVG " +
                    "FROM MC_BTPCAL_INTERFACE_HIST " +
                    "WHERE  TIMESTAMP >'" + time_begin + "' AND TIMESTAMP <='" + time_end + "' ORDER BY TIMESTAMP DESC";
                DataTable data_BTP_X = dBSQL.GetCommand(sql_BTP_X);
                //Y轴
                if (data_BTP_X.Rows.Count > 0)
                {
                    for (int COL = 0; COL < data_BTP_X.Columns.Count; COL++)
                    {
                        BTP_X_WZ.Add(Math.Round(double.Parse(data_BTP_X.Rows[0][COL].ToString()), 2));
                    }
                }
                else
                {
                    for (int COL = 0; COL < data_BTP_X.Columns.Count; COL++)
                    {
                        BTP_X_WZ.Add(0);
                    }
                }

                //X轴
                #endregion

                #region BTP温度
                string sql_BTP_TE = "SELECT TOP(1) " +
                    "ISNULL(BTP_TE_1,0) AS BTP_TE_1," +
                    "ISNULL(BTP_TE_2,0) AS BTP_TE_2," +
                    "ISNULL(BTP_TE_3,0) AS BTP_TE_3," +
                    "ISNULL(BTP_TE_4,0) AS BTP_TE_4," +
                    "ISNULL(BTP_TE_5,0) AS BTP_TE_5," +
                    "ISNULL(BTP_TE_6,0) AS BTP_TE_6," +
                    "ISNULL(BTP_TE_AVG,0) AS BTP_TE_AVG " +
                    "FROM MC_BTPCAL_INTERFACE_HIST " +
                    "WHERE  TIMESTAMP >'" + time_begin + "' AND TIMESTAMP <='" + time_end + "' ORDER BY TIMESTAMP DESC";
                DataTable data_BTP_TE = dBSQL.GetCommand(sql_BTP_TE);
                //Y轴
                if (data_BTP_TE.Rows.Count > 0)
                {
                    for (int COL = 0; COL < data_BTP_TE.Columns.Count; COL++)
                    {
                        BTP_TH_WD.Add(Math.Round(double.Parse(data_BTP_TE.Rows[0][COL].ToString()), 2));
                    }
                }
                else
                {
                    for (int COL = 0; COL < data_BTP_TE.Columns.Count; COL++)
                    {
                        BTP_TH_WD.Add(0);
                    }
                }
                //X轴
                #endregion

                #region 位置偏差 TRP
                string sql_DEV_TRP = "SELECT TOP (1)" +
                    "ISNULL(TRP_DEV_1,0) AS TRP_DEV_1," +
                    "ISNULL(TRP_DEV_2,0) AS TRP_DEV_2," +
                    "ISNULL(TRP_DEV_3,0) AS TRP_DEV_3," +
                    "ISNULL(TRP_DEV_4,0) AS TRP_DEV_4," +
                    "ISNULL(TRP_DEV_5,0) AS TRP_DEV_5," +
                    "ISNULL(TRP_DEV_6,0) AS TRP_DEV_6," +
                    "ISNULL(TRP_DEV_AVG,0) AS TRP_DEV_AVG  " +
                    "FROM MC_BTPCAL_INTERFACE_HIST " +
                    "WHERE  TIMESTAMP >'" + time_begin + "' AND TIMESTAMP <='" + time_end + "' ORDER BY TIMESTAMP DESC";
                DataTable data_DEV_TRP = dBSQL.GetCommand(sql_DEV_TRP);
                if (data_DEV_TRP.Rows.Count > 0)
                {
                    for (int COL = 0; COL < data_DEV_TRP.Columns.Count; COL++)
                    {
                        TRP_PC.Add(Math.Round(double.Parse(data_DEV_TRP.Rows[0][COL].ToString()), 2));
                    }
                }
                else
                {
                    for (int COL = 0; COL < data_DEV_TRP.Columns.Count; COL++)
                    {
                        TRP_PC.Add(0);
                    }

                }
                #endregion

                #region 位置偏差 BRP
                string sql_DEV_BRP = "SELECT TOP (1)" +
                 "ISNULL(BRP_DEV_1,0) AS BRP_DEV_1," +
                 "ISNULL(BRP_DEV_2,0) AS BRP_DEV_2," +
                 "ISNULL(BRP_DEV_3,0) AS BRP_DEV_3," +
                 "ISNULL(BRP_DEV_4,0) AS BRP_DEV_4," +
                 "ISNULL(BRP_DEV_5,0) AS BRP_DEV_5," +
                 "ISNULL(BRP_DEV_6,0) AS BRP_DEV_6," +
                 "ISNULL(BRP_DEV_AVG,0) AS BRP_DEV_AVG  " +
                 "FROM MC_BTPCAL_INTERFACE_HIST " +
                 "WHERE  TIMESTAMP >'" + time_begin + "' AND TIMESTAMP <='" + time_end + "' ORDER BY TIMESTAMP DESC";
                DataTable data_DEV_BRP = dBSQL.GetCommand(sql_DEV_BRP);
                if (data_DEV_BRP.Rows.Count > 0)
                {
                    for (int COL = 0; COL < data_DEV_BRP.Columns.Count; COL++)
                    {
                        BRP_PC.Add(Math.Round(double.Parse(data_DEV_BRP.Rows[0][COL].ToString()), 2));
                    }
                }
                else
                {
                    for (int COL = 0; COL < data_DEV_BRP.Columns.Count; COL++)
                    {
                        BRP_PC.Add(0);
                    }

                }
                #endregion

                #region 位置偏差 BTP
                string sql_DEV_BTP = "SELECT TOP (1)" +
                 "ISNULL(BTP_DEV_1,0) AS BTP_DEV_1," +
                 "ISNULL(BTP_DEV_2,0) AS BTP_DEV_2," +
                 "ISNULL(BTP_DEV_3,0) AS BTP_DEV_3," +
                 "ISNULL(BTP_DEV_4,0) AS BTP_DEV_4," +
                 "ISNULL(BTP_DEV_5,0) AS BTP_DEV_5," +
                 "ISNULL(BTP_DEV_6,0) AS BTP_DEV_6," +
                 "ISNULL(BTP_DEV_AVG,0) AS BTP_DEV_AVG  " +
                 "FROM MC_BTPCAL_INTERFACE_HIST " +
                 "WHERE  TIMESTAMP >'" + time_begin + "' AND TIMESTAMP <='" + time_end + "' ORDER BY TIMESTAMP DESC";
                DataTable data_DEV_BTP = dBSQL.GetCommand(sql_DEV_BTP);
                if (data_DEV_BTP.Rows.Count > 0)
                {
                    for (int COL = 0; COL < data_DEV_BTP.Columns.Count; COL++)
                    {
                        BTP_PC.Add(Math.Round(double.Parse(data_DEV_BTP.Rows[0][COL].ToString()), 2));
                    }
                }
                else
                {
                    for (int COL = 0; COL < data_DEV_BTP.Columns.Count; COL++)
                    {
                        BTP_PC.Add(0);
                    }

                }
                #endregion
            }
            catch (Exception ee)
            {
                string mistake = "均匀性弹出框曲线数据查询报错" + ee.ToString();
                vLog.writelog(mistake, -1);
            }

        }

        /// <summary>
        /// TRP、BRP、BTP目标位置
        /// </summary>
        public void curve_MBWZ()
        {
            try
            {
                //查询参数表的设定位置值
                string sql_MC_BTPCAL_PAR = "SELECT TOP (1) " +
                    "ISNULL(PAR_AIM_TRP,0) AS PAR_AIM_TRP," +
                    "ISNULL(PAR_AIM_BRP,0) AS PAR_AIM_BRP," +
                    "ISNULL(PAR_AIM_BTP,0) AS PAR_AIM_BTP " +
                    "FROM MC_BTPCAL_PAR ORDER BY TIMESTAMP DESC";
                DataTable table_MC_BTPCAL_PAR = dBSQL.GetCommand(sql_MC_BTPCAL_PAR);
                if (table_MC_BTPCAL_PAR.Rows.Count > 0)
                {
                    //TRP目标位置
                    double PAR_AIM_TRP = Math.Round(double.Parse(table_MC_BTPCAL_PAR.Rows[0]["PAR_AIM_TRP"].ToString()), 2);
                    //BRP目标位置
                    double PAR_AIM_BRP = Math.Round(double.Parse(table_MC_BTPCAL_PAR.Rows[0]["PAR_AIM_BRP"].ToString()), 2);
                    //BTP目标位置
                    double PAR_AIM_BTP = Math.Round(double.Parse(table_MC_BTPCAL_PAR.Rows[0]["PAR_AIM_BTP"].ToString()), 2);
                    //曲线位置的目标值都使用一个数据
                    for (int count = 0; count < time_x.Count; count++)
                    {
                        TRP_X_MB.Add(PAR_AIM_TRP);
                        BRP_X_MB.Add(PAR_AIM_BRP);
                        BTP_X_MB.Add(PAR_AIM_BTP);
                    }

                }
                else
                {
                    for (int count = 0; count < time_x.Count; count++)
                    {
                        TRP_X_MB.Add(0);
                        BRP_X_MB.Add(0);
                        BTP_X_MB.Add(0);
                    }
                }
            }
            catch (Exception ee)
            {
                string mistake = "均匀性对比分析弹出框MC_BTPCAL_PAR表查询报错" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 清空曲线数据
        /// </summary>
        public void LIST_CLRAE()
        {
            TRP_X_MB.Clear();
            TRP_X_WZ.Clear();
            TRP_TH_WD.Clear();

            BRP_X_MB.Clear();
            BRP_X_WZ.Clear();
            BRP_TH_WD.Clear();


            BTP_X_MB.Clear();
            BTP_X_WZ.Clear();
            BTP_TH_WD.Clear();


            TRP_PC.Clear();
            BRP_PC.Clear();
            BTP_PC.Clear();
        }
        /// <summary>
        /// 曲线声明
        /// </summary>
        public void statement_surve()
        {
            try
            {

                #region 背景颜色
                //TRP位置
                this.CHART_TRP_X.LChart.BackColor = Color_curve;
                ///TRP温度
                this.CHART_TRP_TH.LChart.BackColor = Color_curve;
                //BRP
                this.CHART_BRP_X.LChart.BackColor = Color_curve;
                this.CHART_BRP_TH.LChart.BackColor = Color_curve;
                //BTP
                this.CHART_BTP_X.LChart.BackColor = Color_curve;
                this.CHART_BTP_TH.LChart.BackColor = Color_curve;
                //偏差
                this.CHART_PC.LChart.BackColor = Color_curve;
                #endregion

                #region TRP 曲线定义

                //******位置***** begin
                CHART_TRP_X.LChart.Zoom = LiveCharts.ZoomingOptions.None;
                //柱形图
                COLUM_BAR_TRP_X = CHART_TRP_X.MakeCol(0, 0, "COLUM_BAR_TRP_X");
                CHART_TRP_X.LChart.Series.Add(COLUM_BAR_TRP_X);
                //曲线图
                var COLUM_QX_TRP_X = CHART_TRP_X.MakeLine(0, 0, "COLUM_QX_TRP_X");
                CHART_TRP_X.LChart.Series.Add(COLUM_QX_TRP_X);
                //x轴显示数据个数
                CHART_TRP_X.LPageSize = 7;
                //******位置******end

                //********温度*****begin
                CHART_TRP_TH.LChart.Zoom = LiveCharts.ZoomingOptions.None;
                //柱形图
                COLUM_BAR_TRP_TH = CHART_TRP_TH.MakeCol(0, 0, "COLUM_BAR_TRP_TH");
                CHART_TRP_TH.LChart.Series.Add(COLUM_BAR_TRP_TH);
                //x轴显示数据个数
                CHART_TRP_TH.LPageSize = 7;
                //*********温度*****end
                #endregion


                #region BRP 曲线定义

                //******位置***** begin
                CHART_BRP_X.LChart.Zoom = LiveCharts.ZoomingOptions.None;
                //柱形图
                COLUM_BAR_BRP_X = CHART_BRP_X.MakeCol(0, 0, "COLUM_BAR_BRP_X");
                CHART_BRP_X.LChart.Series.Add(COLUM_BAR_BRP_X);
                //曲线图
                var COLUM_QX_BRP_X = CHART_BRP_X.MakeLine(0, 0, "COLUM_QX_BRP_X");
                CHART_BRP_X.LChart.Series.Add(COLUM_QX_BRP_X);
                //x轴显示数据个数
                CHART_BRP_X.LPageSize = 7;
                //******位置******end

                //********温度*****begin
                CHART_BRP_TH.LChart.Zoom = LiveCharts.ZoomingOptions.None;
                //柱形图
                COLUM_BAR_BRP_TH = CHART_BRP_TH.MakeCol(0, 0, "COLUM_BAR_BRP_TH");
                CHART_BRP_TH.LChart.Series.Add(COLUM_BAR_BRP_TH);
                //x轴显示数据个数
                CHART_BRP_TH.LPageSize = 7;
                //*********温度*****end
                #endregion


                #region BTP 曲线定义
                //******位置***** begin
                CHART_BTP_X.LChart.Zoom = LiveCharts.ZoomingOptions.None;
                //柱形图
                COLUM_BAR_BTP_X = CHART_BTP_X.MakeCol(0, 0, "COLUM_BAR_BTP_X");
                CHART_BTP_X.LChart.Series.Add(COLUM_BAR_BTP_X);
                //曲线图
                var COLUM_QX_BTP_X = CHART_BTP_X.MakeLine(0, 0, "COLUM_QX_BTP_X");
                CHART_BTP_X.LChart.Series.Add(COLUM_QX_BTP_X);
                //x轴显示数据个数
                CHART_BTP_X.LPageSize = 7;
                //******位置******end

                //********温度*****begin
                CHART_BTP_TH.LChart.Zoom = LiveCharts.ZoomingOptions.None;
                //柱形图
                COLUM_BAR_BTP_TH = CHART_BTP_TH.MakeCol(0, 0, "COLUM_BAR_BTP_TH");
                CHART_BTP_TH.LChart.Series.Add(COLUM_BAR_BTP_TH);
                //x轴显示数据个数
                CHART_BTP_TH.LPageSize = 7;
                //*********温度*****end
                #endregion

                #region 偏差


                CHART_PC.LChart.Zoom = LiveCharts.ZoomingOptions.None;
                //柱形图
                COLUM_BAR_TRP_PC = CHART_PC.MakeCol(0, 0, "COLUM_BAR_TRP_PC");
                CHART_PC.LChart.Series.Add(COLUM_BAR_TRP_PC);

                COLUM_BAR_BRP_PC = CHART_PC.MakeCol(0, 0, "COLUM_BAR_BRP_PC");
                CHART_PC.LChart.Series.Add(COLUM_BAR_BRP_PC);

                COLUM_BAR_BTP_PC = CHART_PC.MakeCol(0, 0, "COLUM_BAR_BTP_PC");
                CHART_PC.LChart.Series.Add(COLUM_BAR_BTP_PC);
                //x轴显示数据个数
                CHART_PC.LPageSize = 7;

                #endregion
            }
            catch (Exception ee)
            {
                string mistake = "均匀性弹出框曲线声明报错" + ee.ToString();
                vLog.writelog(mistake, -1);
            }

        }
        /// <summary>
        /// 曲线赋值
        /// </summary>
        public void curve_text()
        {
            try
            {
                //TRP位置
                CHART_TRP_X.LBindDataC<string, Double>("COLUM_BAR_TRP_X", time_x, TRP_X_MB, System.Windows.Media.Brushes.CornflowerBlue, "", "", 2);
                CHART_TRP_X.LBindData<string, double>("COLUM_QX_TRP_X", time_x, TRP_X_WZ, System.Windows.Media.Brushes.Red, "", "", 2);
                this.CHART_TRP_X.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示
                                                                                          //TRP温度
                CHART_TRP_TH.LBindDataC<string, Double>("COLUM_BAR_TRP_TH", time_x, TRP_TH_WD, System.Windows.Media.Brushes.CornflowerBlue, "", "", 2);
                this.CHART_TRP_TH.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示

                //BRP位置
                CHART_BRP_X.LBindDataC<string, Double>("COLUM_BAR_BRP_X", time_x, BRP_X_MB, System.Windows.Media.Brushes.CornflowerBlue, "", "", 2);
                CHART_BRP_X.LBindData<string, double>("COLUM_QX_BRP_X", time_x, BRP_X_WZ, System.Windows.Media.Brushes.Red, "", "", 2);
                this.CHART_BRP_X.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示
                                                                                          //BRP温度
                CHART_BRP_TH.LBindDataC<string, Double>("COLUM_BAR_BRP_TH", time_x, BRP_TH_WD, System.Windows.Media.Brushes.CornflowerBlue, "", "", 2);
                this.CHART_BRP_TH.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示

                //BTP位置
                CHART_BTP_X.LBindDataC<string, Double>("COLUM_BAR_BTP_X", time_x, BTP_X_MB, System.Windows.Media.Brushes.CornflowerBlue, "", "", 2);
                CHART_BTP_X.LBindData<string, double>("COLUM_QX_BTP_X", time_x, BTP_X_WZ, System.Windows.Media.Brushes.Red, "", "", 2);
                this.CHART_BTP_X.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示

                //BTP温度
                CHART_BTP_TH.LBindDataC<string, Double>("COLUM_BAR_BTP_TH", time_x, BTP_TH_WD, System.Windows.Media.Brushes.CornflowerBlue, "", "", 2);
                this.CHART_BTP_TH.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示

                //TRP偏差
                CHART_PC.LBindDataC<string, Double>("COLUM_BAR_TRP_PC", time_x, TRP_PC, System.Windows.Media.Brushes.CornflowerBlue, "", "", 2);

                //BRP偏差
                CHART_PC.LBindDataC<string, Double>("COLUM_BAR_BRP_PC", time_x, BRP_PC, System.Windows.Media.Brushes.DarkOrange, "", "", 2);
                //BTP偏差
                CHART_PC.LBindDataC<string, Double>("COLUM_BAR_BTP_PC", time_x, BTP_PC, System.Windows.Media.Brushes.Green, "", "", 2);

                this.CHART_PC.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示
            }
            catch (Exception ee)
            {
                string mistake = "均匀性弹出框曲线赋值报错" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }
    }
}
