using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts.Wpf;
using OxyPlot.Axes;
using OxyPlot;
using DataBase;
using LY_SINTER.Custom;
using VLog;
using LY_SINTER.Popover.Course;

namespace LY_SINTER.PAGE.Course
{
    public partial class Deviation_guide : UserControl
    {
        public vLog _vLog { get; set; }
       
        public System.Timers.Timer _Timer1 { get; set; }

        //实时曲线
        List<double> ysdlh = new List<double>();
        List<string> xvalDt = new List<string> { "1#闸门", "2#闸门", "3#闸门", "4#闸门", "5#闸门", "6#闸门", "平均料厚" };//{ 1, 2, 3, 4, 5, 6, 7 };
        List<double> ysjlh = new List<double>();
        List<double> yjyyzxzs = new List<double>();
        List<string> xvalDt1 = new List<string> { "1#闸门", "2#闸门", "3#闸门", "4#闸门", "5#闸门", "6#闸门" };
        List<double> yytlh = new List<double>();
        //实时曲线前12H数据
        //时间
        List<string> xvalDt_12h_1 = new List<string>();
        //设定料厚
        List<double> ydata_12h_1 = new List<double>();
        //实际料厚
        List<double> ydata_12h_2 = new List<double>();
        //闸门1应调料厚
        List<double> ydata_12h_3 = new List<double>();
        //闸门2应调料厚
        List<double> ydata_12h_4 = new List<double>();
        //闸门3应调料厚
        List<double> ydata_12h_5 = new List<double>();
        //闸门4应调料厚
        List<double> ydata_12h_6 = new List<double>();
        //闸门5应调料厚
        List<double> ydata_12h_7 = new List<double>();
        //闸门6应调料厚
        List<double> ydata_12h_8 = new List<double>();

        private PlotModel _myPlotModel;
        private DateTimeAxis _dateAxis;//X轴
        private LinearAxis _valueAxis1;//Y轴
        private LinearAxis _valueAxis2;//Y轴
        private LinearAxis _valueAxis3;//Y轴
        private LinearAxis _valueAxis4;//Y轴
        private LinearAxis _valueAxis5;//Y轴
        private LinearAxis _valueAxis6;//Y轴
        private LinearAxis _valueAxis7;//Y轴
        private LinearAxis _valueAxis8;//Y轴

        List<OxyPlot.DataPoint> Line1 = new List<OxyPlot.DataPoint>();
        List<OxyPlot.DataPoint> Line2 = new List<OxyPlot.DataPoint>();
        List<OxyPlot.DataPoint> Line3 = new List<OxyPlot.DataPoint>();
        List<OxyPlot.DataPoint> Line4 = new List<OxyPlot.DataPoint>();
        List<OxyPlot.DataPoint> Line5 = new List<OxyPlot.DataPoint>();
        List<OxyPlot.DataPoint> Line6 = new List<OxyPlot.DataPoint>();
        List<OxyPlot.DataPoint> Line7 = new List<OxyPlot.DataPoint>();
        List<OxyPlot.DataPoint> Line8 = new List<OxyPlot.DataPoint>();


        private OxyPlot.Series.LineSeries series1;//曲线
        private OxyPlot.Series.LineSeries series2;//曲线
        private OxyPlot.Series.LineSeries series3;//曲线
        private OxyPlot.Series.LineSeries series4;//曲线
        private OxyPlot.Series.LineSeries series5;//曲线
        private OxyPlot.Series.LineSeries series6;//曲线
        private OxyPlot.Series.LineSeries series7;//曲线
        private OxyPlot.Series.LineSeries series8;//曲线
        string[] curve_name = { "A_1", "A_2", "A_3", "A_4", "A_5", "A_6", "A_7", "A_8" };


        ColumnSeries lcheckBox9 { get; set; }
        ColumnSeries lcheckBox10 { get; set; }
        ColumnSeries lcheckBox11 { get; set; }
        ColumnSeries lcheckBox12 { get; set; }

        DBSQL dBSQL = new DBSQL(DataBase.ConstParameters.strCon);
        public Deviation_guide()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Course\\Deviation_guide\\");
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            //曲线控件背景颜色
            lChartPlus6.LChart.BackColor = Color.White;
            lChartPlus7.LChart.BackColor = Color.White;
            lChartPlus8.LChart.BackColor = Color.White;
            lcheckBox9 = lChartPlus6.MakeCol(0, 0, "checkBox9");
            lcheckBox10 = lChartPlus6.MakeCol(0, 0, "checkBox10");
            lcheckBox11 = lChartPlus7.MakeCol(0, 0, "checkBox11");
            lcheckBox12 = lChartPlus8.MakeCol(0, 0, "checkBox12");


            quxiandingyi();//实时曲线定义
            ss();//实时曲线赋值
                 //  ls12h();//历史中实时曲线初始化赋值12H
                 //jingtaiquxiandingyi();//历史中的静态曲线定义
            dataGridView1_CellContentClick();//dataGridView不显示行标题列
            time();
            xzmxx();
            shuju();
            tendency_curve_HIS(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));

            _Timer1 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { Timer1_Tick_1(); };//响应事件
            _Timer1.Enabled = true;
            _Timer1.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
        }

        private void Timer1_Tick_1()
        {
            Action invokeAction = new Action(Timer1_Tick_1);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                dataGridView1_CellContentClick();
                time();
                xzmxx();
                shuju();
                ss();//实时曲线赋值
                min1();//1min历史中实时曲线放一个数
            }
        }
        //实时曲线定义
        private void quxiandingyi()
        {

            lChartPlus6.Dock = DockStyle.Fill;
            lChartPlus7.Dock = DockStyle.Fill;
            lChartPlus8.Dock = DockStyle.Fill;

            lChartPlus6.LChart.Series.Add(lcheckBox9);
            lChartPlus6.LChart.Series.Add(lcheckBox10);
            lChartPlus7.LChart.Series.Add(lcheckBox11);
            lChartPlus8.LChart.Series.Add(lcheckBox12);
            lChartPlus6.LBindDataC<string, double>("checkBox9", xvalDt, ysdlh, System.Windows.Media.Brushes.Green, " ", " ", 2);//散点是S，X轴名，Y轴名，全部写2
            lChartPlus6.LBindDataC<string, double>("checkBox10", xvalDt, ysjlh, System.Windows.Media.Brushes.Purple, " ", " ", 2);
            lChartPlus7.LBindDataC<string, double>("checkBox11", xvalDt1, yjyyzxzs, System.Windows.Media.Brushes.Green, " ", " ", 2);
            lChartPlus8.LBindDataC<string, double>("checkBox12", xvalDt1, yytlh, System.Windows.Media.Brushes.Green, " ", " ", 2);
            this.lChartPlus6.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示
            this.lChartPlus7.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示
            this.lChartPlus8.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示
            lcheckBox9.Title = "设定厚度(mm)";
            lcheckBox10.Title = "实际厚度(mm)";
            lcheckBox11.Title = "均匀一致性指数";
            lcheckBox12.Title = "应调厚度(mm)";

        }
        //1min历史中实时曲线放一个数
        private void min1()
        {
            try
            {
                string sql1 = "select top 1 a.TIMESTAMP,(a.F_PLC_SMALL_SG_TH_SP_1+a.F_PLC_SMALL_SG_TH_SP_2+a.F_PLC_SMALL_SG_TH_SP_3+a.F_PLC_SMALL_SG_TH_SP_4+a.F_PLC_SMALL_SG_TH_SP_5+a.F_PLC_SMALL_SG_TH_SP_6)/6,(a.F_PLC_SMALL_SG_TH_PV_1+a.F_PLC_SMALL_SG_TH_PV_2+a.F_PLC_SMALL_SG_TH_PV_3+a.F_PLC_SMALL_SG_TH_PV_4+a.F_PLC_SMALL_SG_TH_PV_5+a.F_PLC_SMALL_SG_TH_PV_6)/6,b.UNCAL_SG_TH_AC_1,b.UNCAL_SG_TH_AC_2,b.UNCAL_SG_TH_AC_3,b.UNCAL_SG_TH_AC_4,b.UNCAL_SG_TH_AC_5,b.UNCAL_SG_TH_AC_6 from C_MFI_PLC_1MIN a,MC_UNIFORMCAL_result b where convert(varchar(16),a.TIMESTAMP,121)=convert(varchar(16),b.TIMESTAMP,121) order by a.TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    DateTime sj = DateTime.Parse(dataTable1.Rows[0][0].ToString());
                    long sjc = (DateTime.Now.Ticks / 10000000) - (sj.Ticks / 10000000);
                    if (sjc <= 60)
                    {
                        if (dataTable1.Rows[0][1].ToString() != "")
                        {
                            ydata_12h_1.Add(double.Parse(dataTable1.Rows[0][1].ToString()));
                        }
                        else
                        {
                            ydata_12h_1.Add(double.NaN);
                        }
                        if (dataTable1.Rows[0][2].ToString() != "")
                        {
                            ydata_12h_2.Add(double.Parse(dataTable1.Rows[0][2].ToString()));
                        }
                        else
                        {
                            ydata_12h_2.Add(double.NaN);
                        }
                        if (dataTable1.Rows[0][3].ToString() != "")
                        {
                            ydata_12h_3.Add(double.Parse(dataTable1.Rows[0][3].ToString()));
                        }
                        else
                        {
                            ydata_12h_3.Add(double.NaN);
                        }
                        if (dataTable1.Rows[0][4].ToString() != "")
                        {
                            ydata_12h_4.Add(double.Parse(dataTable1.Rows[0][4].ToString()));
                        }
                        else
                        {
                            ydata_12h_4.Add(double.NaN);
                        }
                        if (dataTable1.Rows[0][5].ToString() != "")
                        {
                            ydata_12h_5.Add(double.Parse(dataTable1.Rows[0][5].ToString()));
                        }
                        else
                        {
                            ydata_12h_5.Add(double.NaN);
                        }
                        if (dataTable1.Rows[0][6].ToString() != "")
                        {
                            ydata_12h_6.Add(double.Parse(dataTable1.Rows[0][6].ToString()));
                        }
                        else
                        {
                            ydata_12h_6.Add(double.NaN);
                        }
                        if (dataTable1.Rows[0][7].ToString() != "")
                        {
                            ydata_12h_7.Add(double.Parse(dataTable1.Rows[0][7].ToString()));
                        }
                        else
                        {
                            ydata_12h_7.Add(double.NaN);
                        }
                        if (dataTable1.Rows[0][8].ToString() != "")
                        {
                            ydata_12h_8.Add(double.Parse(dataTable1.Rows[0][8].ToString()));
                        }
                        else
                        {
                            ydata_12h_8.Add(double.NaN);
                        }
                                }
                    else
                    {
                        ydata_12h_1.Add(double.NaN);
                        ydata_12h_2.Add(double.NaN);
                        ydata_12h_3.Add(double.NaN);
                        ydata_12h_4.Add(double.NaN);
                        ydata_12h_5.Add(double.NaN);
                        ydata_12h_6.Add(double.NaN);
                        ydata_12h_7.Add(double.NaN);
                        ydata_12h_8.Add(double.NaN);
                              }
                }
                else
                {
                    ydata_12h_1.Add(double.NaN);
                    ydata_12h_2.Add(double.NaN);
                    ydata_12h_3.Add(double.NaN);
                    ydata_12h_4.Add(double.NaN);
                    ydata_12h_5.Add(double.NaN);
                    ydata_12h_6.Add(double.NaN);
                    ydata_12h_7.Add(double.NaN);
                    ydata_12h_8.Add(double.NaN);
                }
            }
            catch
            { }
        }
        //实时曲线赋值
        private void ss()
        {
            try
            {
                ysdlh.Clear();
                ysjlh.Clear();
                yjyyzxzs.Clear();
                yytlh.Clear();
                double a = 0;
                double b = 0;

                //取设定料厚
                string sql4 = "select top 1 ISNULL(F_PLC_SMALL_SG_TH_SP_1,0),ISNULL(F_PLC_SMALL_SG_TH_SP_2,0),ISNULL(F_PLC_SMALL_SG_TH_SP_3,0),ISNULL(F_PLC_SMALL_SG_TH_SP_4,0),ISNULL(F_PLC_SMALL_SG_TH_SP_5,0),ISNULL(F_PLC_SMALL_SG_TH_SP_6,0) from C_MFI_PLC_1MIN order by TIMESTAMP desc";
                DataTable dataTable4 = dBSQL.GetCommand(sql4);
                if (dataTable4.Rows.Count > 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        double sdlh = double.Parse(dataTable4.Rows[0][i].ToString());
                        ysdlh.Add(sdlh);
                        a += double.Parse(dataTable4.Rows[0][i].ToString());
                    }
                    double avgsdlh = a / 6;
                    ysdlh.Add(avgsdlh);
                    lChartPlus6.LBindDataC<string, double>("checkBox9", xvalDt, ysdlh, System.Windows.Media.Brushes.Green, " ", " ", 2);
                }
                else
                {
                    ysdlh.Add(0);
                    lChartPlus6.LBindDataC<string, double>("checkBox9", xvalDt, ysdlh, System.Windows.Media.Brushes.Green, " ", " ", 2);
                }
                //取实际料厚
                string sql5 = "select top 1 ISNULL(F_PLC_SMALL_SG_TH_PV_1,0),ISNULL(F_PLC_SMALL_SG_TH_PV_2,0),ISNULL(F_PLC_SMALL_SG_TH_PV_3,0),ISNULL(F_PLC_SMALL_SG_TH_PV_4,0),ISNULL(F_PLC_SMALL_SG_TH_PV_5,0),ISNULL(F_PLC_SMALL_SG_TH_PV_6,0) from C_MFI_PLC_1MIN order by TIMESTAMP desc";
                DataTable dataTable5 = dBSQL.GetCommand(sql5);
                if (dataTable5.Rows.Count > 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        double sjlh = double.Parse(dataTable5.Rows[0][i].ToString());
                        ysjlh.Add(sjlh);
                        b += double.Parse(dataTable5.Rows[0][i].ToString());
                    }
                    double avgsjlh = b / 6;
                    ysjlh.Add(avgsjlh);
                    lChartPlus6.LBindDataC<string, double>("checkBox10", xvalDt1, ysjlh, System.Windows.Media.Brushes.CornflowerBlue, " ", " ", 2);
                }
                else
                {
                    ysjlh.Add(0);
                    lChartPlus6.LBindDataC<string, double>("checkBox10", xvalDt1, ysjlh, System.Windows.Media.Brushes.CornflowerBlue, " ", " ", 2);
                }

                //均匀一致性指数
                string sql6 = "select top 1 ISNULL(UNCAL_E_1,0),ISNULL(UNCAL_E_2,0),ISNULL(UNCAL_E_3,0),ISNULL(UNCAL_E_4,0),ISNULL(UNCAL_E_5,0),ISNULL(UNCAL_E_6,0) from MC_UNIFORMCAL_result_1min order by TIMESTAMP desc";
                DataTable dataTable6 = dBSQL.GetCommand(sql6);
                if (dataTable6.Rows.Count > 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        double jyyzxzs = double.Parse(dataTable6.Rows[0][i].ToString());
                        yjyyzxzs.Add(jyyzxzs);
                    }
                    lChartPlus7.LBindDataC<string, double>("checkBox11", xvalDt1, yjyyzxzs, System.Windows.Media.Brushes.Green, " ", " ", 2);
                }
                else
                {
                    yjyyzxzs.Add(0);
                    lChartPlus7.LBindDataC<string, double>("checkBox11", xvalDt1, yjyyzxzs, System.Windows.Media.Brushes.Green, " ", " ", 2);
                }
                //应调料厚
                string sql7 = "select top 1 ISNULL(UNCAL_SG_TH_AC_1,0),ISNULL(UNCAL_SG_TH_AC_2,0),ISNULL(UNCAL_SG_TH_AC_3,0),ISNULL(UNCAL_SG_TH_AC_4,0),ISNULL(UNCAL_SG_TH_AC_5,0),ISNULL(UNCAL_SG_TH_AC_6,0) from MC_UNIFORMCAL_result order by TIMESTAMP desc";
                DataTable dataTable7 = dBSQL.GetCommand(sql7);
                if (dataTable7.Rows.Count > 0)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        double ytlh = double.Parse(dataTable7.Rows[0][i].ToString());
                        yytlh.Add(ytlh);
                    }
                    lChartPlus8.LBindDataC<string, double>("checkBox12", xvalDt1, yytlh, System.Windows.Media.Brushes.Green, " ", " ", 2);
                }
                else
                {
                    yytlh.Add(0);
                    lChartPlus8.LBindDataC<string, double>("checkBox12", xvalDt1, yytlh, System.Windows.Media.Brushes.Green, " ", " ", 2);
                }
                lcheckBox9.Title = "设定厚度(mm)";
                lcheckBox10.Title = "实际厚度(mm)";
                lcheckBox11.Title = "均匀一致性指数";
                lcheckBox12.Title = "应调厚度(mm)";
            }
            catch
            { }
        }

        //最新调整时间
        private void time()
        {
            try
            {
                string sql1 = "select top 1 timestamp from MC_UNIFORMCAL_result order by timestamp desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                string zxsj = dataTable1.Rows[0][0].ToString();
                label8.Text = "最新调整时间:" + zxsj;
            }
            catch
            {
            }
        }

        //小闸门信息
        private void xzmxx()
        {

            DataTable table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Gate_1");
            table.Columns.Add("Gate_2");
            table.Columns.Add("Gate_3");
            table.Columns.Add("Gate_4");
            table.Columns.Add("Gate_5");
            table.Columns.Add("Gate_6");
            table.Columns.Add("Gate_Avg");

            #region 设定料厚
            DataRow row_1 = table.NewRow();
            row_1["Name"] = "设定料厚(mm)";
            string sql1 = "select top (1) " +
                "ISNULL(F_PLC_SMALL_SG_TH_SP_1,0)," +
                "ISNULL(F_PLC_SMALL_SG_TH_SP_2,0)," +
                "ISNULL(F_PLC_SMALL_SG_TH_SP_3,0)," +
                "ISNULL(F_PLC_SMALL_SG_TH_SP_4,0)," +
                "ISNULL(F_PLC_SMALL_SG_TH_SP_5,0)," +
                "ISNULL(F_PLC_SMALL_SG_TH_SP_6,0) " +
                "from C_MFI_PLC_1MIN order by TIMESTAMP desc";
            DataTable dataTable1 = dBSQL.GetCommand(sql1);
            double avg_1 = 0;
            if (dataTable1.Rows.Count > 0)
            {
                for (int x = 1; x < 7; x++)
                {
                    double a = Math.Round(double.Parse(dataTable1.Rows[0][x - 1].ToString()), 2);
                    row_1[x] = a;
                    avg_1 += a;
                }
                row_1[7] = Math.Round(avg_1 / 6, 2);
                table.Rows.Add(row_1);
            }
            #endregion

            #region 实际料厚
            DataRow row_2 = table.NewRow();
            row_2["Name"] = "实际料厚(mm)";
            string sql2 = "select top (1) " +
                "ISNULL(F_PLC_SMALL_SG_TH_PV_1,0)," +
                "ISNULL(F_PLC_SMALL_SG_TH_PV_2,0)," +
                "ISNULL(F_PLC_SMALL_SG_TH_PV_3,0)," +
                "ISNULL(F_PLC_SMALL_SG_TH_PV_4,0)," +
                "ISNULL(F_PLC_SMALL_SG_TH_PV_5,0)," +
                "ISNULL(F_PLC_SMALL_SG_TH_PV_6,0) " +
                "from C_MFI_PLC_1MIN order by TIMESTAMP desc";
            DataTable dataTable2 = dBSQL.GetCommand(sql2);
            double avg_2 = 0;
            if (dataTable2.Rows.Count > 0)
            {
                for (int x = 1; x < 7; x++)
                {
                    double a = Math.Round(double.Parse(dataTable2.Rows[0][x - 1].ToString()), 2);
                    row_2[x] = a;
                    avg_2 += a;
                }
                row_2[7] = Math.Round(avg_2 / 6, 2);
                table.Rows.Add(row_2);
            }

            #endregion

            #region 应调料厚(mm)
            DataRow row_3 = table.NewRow();
            row_3["Name"] = "应调料厚(mm)";
            string sql3 = "select top 1 " +
                "ISNULL(UNCAL_SG_TH_AC_1, 0)," +
                "ISNULL(UNCAL_SG_TH_AC_2, 0)," +
                "ISNULL(UNCAL_SG_TH_AC_3, 0)," +
                "ISNULL(UNCAL_SG_TH_AC_4, 0)," +
                "ISNULL(UNCAL_SG_TH_AC_5, 0)," +
                "ISNULL(UNCAL_SG_TH_AC_6, 0) " +
                "from MC_UNIFORMCAL_result order by TIMESTAMP desc";
            DataTable dataTable3 = dBSQL.GetCommand(sql3);
            double avg_3 = 0;
            if (dataTable3.Rows.Count > 0)
            {
                for (int x = 1; x < 7; x++)
                {
                    double a = Math.Round(double.Parse(dataTable3.Rows[0][x - 1].ToString()), 2);
                    row_3[x] = a;
                    avg_3 += a;
                }
                row_3[7] = Math.Round(avg_3 / 6, 2);
                table.Rows.Add(row_3);
            }
            #endregion

            #region 开度设定值(%)
            DataRow row_4 = table.NewRow();
            row_4["Name"] = "开度设定值(%)";
            string sql4 = "select top (1) " +
                "ISNULL(F_PLC_SMALL_SG_O_SP_1, 0)," +
                "ISNULL(F_PLC_SMALL_SG_O_SP_2, 0)," +
                "ISNULL(F_PLC_SMALL_SG_O_SP_3, 0)," +
                "ISNULL(F_PLC_SMALL_SG_O_SP_4, 0)," +
                "ISNULL(F_PLC_SMALL_SG_O_SP_5, 0)," +
                "ISNULL(F_PLC_SMALL_SG_O_SP_6, 0) " +
                "from C_MFI_PLC_1MIN order by TIMESTAMP desc";
            DataTable dataTable4 = dBSQL.GetCommand(sql4);
            double avg_4 = 0;
            if (dataTable4.Rows.Count > 0)
            {
                for (int x = 1; x < 7; x++)
                {
                    double a = Math.Round(double.Parse(dataTable4.Rows[0][x - 1].ToString()), 2);
                    row_4[x] = a;
                    avg_4 += a;
                }
                row_4[7] = Math.Round(avg_4 / 6, 2);
                table.Rows.Add(row_4);
            }
            #endregion

            #region 开度反馈值(%)
            DataRow row_5 = table.NewRow();
            row_5["Name"] = "开度反馈值(%)";
            string sql5 = " select top (1) " +
                "ISNULL(F_PLC_SMALL_SG_O_PV_1, 0)," +
                "ISNULL(F_PLC_SMALL_SG_O_PV_2, 0)," +
                "ISNULL(F_PLC_SMALL_SG_O_PV_3, 0)," +
                "ISNULL(F_PLC_SMALL_SG_O_PV_4, 0)," +
                "ISNULL(F_PLC_SMALL_SG_O_PV_5, 0)," +
                "ISNULL(F_PLC_SMALL_SG_O_PV_6, 0) " +
                "from C_MFI_PLC_1MIN order by TIMESTAMP desc";
            DataTable dataTable5 = dBSQL.GetCommand(sql5);
            double avg_5 = 0;
            if (dataTable5.Rows.Count > 0)
            {
                for (int x = 1; x < 7; x++)
                {
                    double a = Math.Round(double.Parse(dataTable5.Rows[0][x - 1].ToString()), 2);
                    row_5[x] = a;
                    avg_5 += a;
                }
                row_5[7] = Math.Round(avg_5 / 6, 2);
                table.Rows.Add(row_5);
            }
            #endregion

            #region 均匀一致指数(m)
            DataRow row_6 = table.NewRow();
            row_6["Name"] = "均匀一致指数";
            string sql6 = "select top (1) " +
                "ISNULL(UNCAL_E_1, 0)," +
                "ISNULL(UNCAL_E_2, 0)," +
                "ISNULL(UNCAL_E_3, 0)," +
                "ISNULL(UNCAL_E_4, 0)," +
                "ISNULL(UNCAL_E_5, 0)," +
                "ISNULL(UNCAL_E_6, 0) " +
                "from MC_UNIFORMCAL_result_1min order by TIMESTAMP desc";
            DataTable dataTable6 = dBSQL.GetCommand(sql6);
            double avg_6 = 0;
            if (dataTable6.Rows.Count > 0)
            {
                for (int x = 1; x < 7; x++)
                {
                    double a = Math.Round(double.Parse(dataTable6.Rows[0][x - 1].ToString()), 2);
                    row_6[x] = a;
                    avg_6 += a;
                }
                row_6[7] = Math.Round(avg_6 / 6, 2);
                table.Rows.Add(row_6);
            }
            #endregion

            #region 终点位置(m)
            DataRow row_7 = table.NewRow();
            row_7["Name"] = "终点位置(m)";
            string sql7 = "select top (1) " +
                "ISNULL(UNCAL_X_BTP_1,0)," +
                "ISNULL(UNCAL_X_BTP_2,0)," +
                "ISNULL(UNCAL_X_BTP_3,0)," +
                "ISNULL(UNCAL_X_BTP_4,0)," +
                "ISNULL(UNCAL_X_BTP_5,0)," +
                "ISNULL(UNCAL_X_BTP_6,0) " +
                "from MC_UNIFORMCAL_result_1min order by TIMESTAMP desc";
            DataTable dataTable7 = dBSQL.GetCommand(sql7);
            double avg_7 = 0;
            if (dataTable7.Rows.Count > 0)
            {
                for (int x = 1; x < 7; x++)
                {
                    double a = Math.Round(double.Parse(dataTable7.Rows[0][x - 1].ToString()), 2);
                    row_7[x] = a;
                    avg_7 += a;
                }
                row_7[7] = Math.Round(avg_7 / 6, 2);
                table.Rows.Add(row_7);
            }
            #endregion
            dataGridView1.DataSource = table;


        }

        //数据项
        private void shuju()
        {
            try
            {
                //设定料厚（平均值）
                string sql1 = "select top 1 ISNULL(F_PLC_SMALL_SG_TH_SP_1,0),ISNULL(F_PLC_SMALL_SG_TH_SP_2,0),ISNULL(F_PLC_SMALL_SG_TH_SP_3,0),ISNULL(F_PLC_SMALL_SG_TH_SP_4,0),ISNULL(F_PLC_SMALL_SG_TH_SP_5,0),ISNULL(F_PLC_SMALL_SG_TH_SP_6,0) from C_MFI_PLC_1MIN order by TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                float sdlh1 = float.Parse(dataTable1.Rows[0][0].ToString());
                float sdlh2 = float.Parse(dataTable1.Rows[0][1].ToString());
                float sdlh3 = float.Parse(dataTable1.Rows[0][2].ToString());
                float sdlh4 = float.Parse(dataTable1.Rows[0][3].ToString());
                float sdlh5 = float.Parse(dataTable1.Rows[0][4].ToString());
                float sdlh6 = float.Parse(dataTable1.Rows[0][5].ToString());
                float avgsdlh = (sdlh1 + sdlh2 + sdlh3 + sdlh4 + sdlh5 + sdlh6) / 6;
                this.checkBox7.Text = "设定料厚(mm):" + avgsdlh.ToString();
            }
            catch
            { }
            try
            {
                //实际料厚（平均值）
                string sql2 = "select top 1 ISNULL(F_PLC_SMALL_SG_TH_PV_1,0),ISNULL(F_PLC_SMALL_SG_TH_PV_2,0),ISNULL(F_PLC_SMALL_SG_TH_PV_3,0),ISNULL(F_PLC_SMALL_SG_TH_PV_4,0),ISNULL(F_PLC_SMALL_SG_TH_PV_5,0),ISNULL(F_PLC_SMALL_SG_TH_PV_6,0) from C_MFI_PLC_1MIN order by TIMESTAMP desc";
                DataTable dataTable2 = dBSQL.GetCommand(sql2);
                float sjlh1 = float.Parse(dataTable2.Rows[0][0].ToString());
                float sjlh2 = float.Parse(dataTable2.Rows[0][1].ToString());
                float sjlh3 = float.Parse(dataTable2.Rows[0][2].ToString());
                float sjlh4 = float.Parse(dataTable2.Rows[0][3].ToString());
                float sjlh5 = float.Parse(dataTable2.Rows[0][4].ToString());
                float sjlh6 = float.Parse(dataTable2.Rows[0][5].ToString());
                float avgsjlh = (sjlh1 + sjlh2 + sjlh3 + sjlh4 + sjlh5 + sjlh6) / 6;
                this.checkBox8.Text = "实际料厚(mm):" + avgsjlh.ToString();
            }
            catch
            { }
            try
            {
                //闸门1应调料厚
                string sql3 = "select top 1 ISNULL(UNCAL_SG_TH_AC_1,0) from MC_UNIFORMCAL_result order by TIMESTAMP desc";
                DataTable dataTable3 = dBSQL.GetCommand(sql3);
                float zm1 = float.Parse(dataTable3.Rows[0][0].ToString());
                this.checkBox1.Text = "闸门1应调料厚(mm):" + zm1.ToString();
            }
            catch
            { }
            try
            {
                //闸门2应调料厚
                string sql4 = "select top 1 ISNULL(UNCAL_SG_TH_AC_2,0) from MC_UNIFORMCAL_result order by TIMESTAMP desc";
                DataTable dataTable4 = dBSQL.GetCommand(sql4);
                float zm2 = float.Parse(dataTable4.Rows[0][0].ToString());
                this.checkBox2.Text = "闸门2应调料厚(mm):" + zm2.ToString();
            }
            catch
            { }
            try
            {
                //闸门3应调料厚
                string sql5 = "select top 1 ISNULL(UNCAL_SG_TH_AC_3,0) from MC_UNIFORMCAL_result order by TIMESTAMP desc";
                DataTable dataTable5 = dBSQL.GetCommand(sql5);
                float zm3 = float.Parse(dataTable5.Rows[0][0].ToString());
                this.checkBox3.Text = "闸门3应调料厚(mm):" + zm3.ToString();
            }
            catch
            { }
            try
            {
                //闸门4应调料厚
                string sql6 = "select top 1 ISNULL(UNCAL_SG_TH_AC_4,0) from MC_UNIFORMCAL_result order by TIMESTAMP desc";
                DataTable dataTable6 = dBSQL.GetCommand(sql6);
                float zm4 = float.Parse(dataTable6.Rows[0][0].ToString());
                this.checkBox4.Text = "闸门4应调料厚(mm):" + zm4.ToString();
            }
            catch
            { }
            try
            {
                //闸门5应调料厚
                string sql7 = "select top 1 ISNULL(UNCAL_SG_TH_AC_5,0) from MC_UNIFORMCAL_result order by TIMESTAMP desc";
                DataTable dataTable7 = dBSQL.GetCommand(sql7);
                float zm5 = float.Parse(dataTable7.Rows[0][0].ToString());
                this.checkBox5.Text = "闸门5应调料厚(mm):" + zm5.ToString();
            }
            catch
            { }
            try
            {
                //闸门6应调料厚
                string sql8 = "select top 1 ISNULL(UNCAL_SG_TH_AC_6,0) from MC_UNIFORMCAL_result order by TIMESTAMP desc";
                DataTable dataTable8 = dBSQL.GetCommand(sql8);
                float zm6 = float.Parse(dataTable8.Rows[0][0].ToString());
                this.checkBox6.Text = "闸门6应调料厚(mm):" + zm6.ToString();
            }
            catch
            { }
        }

        //dataGridView不显示行标题列
        private void dataGridView1_CellContentClick()
        {
            dataGridView1.RowHeadersVisible = false;
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Frm_Deviation_guide_PAR form_display = new Frm_Deviation_guide_PAR();
            if (Frm_Deviation_guide_PAR.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            Frm_Deviation_guide_Adjust form_display = new Frm_Deviation_guide_Adjust();
            if (Frm_Deviation_guide_Adjust.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        //查询按钮
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                Check_All();
                tendency_curve_HIS(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));
             
            }
            catch (Exception ee)
            {
                var x = ee.ToString();
            }

        }

        public void tendency_curve_HIS(DateTime _d1, DateTime _d2)
        {

            try
            {

                string sql3 = "select b.TIMESTAMP,(ISNULL(a.F_PLC_SMALL_SG_TH_SP_1,0)+ISNULL(a.F_PLC_SMALL_SG_TH_SP_2,0)+ISNULL(a.F_PLC_SMALL_SG_TH_SP_3,0)+ISNULL(a.F_PLC_SMALL_SG_TH_SP_4,0)+ISNULL(a.F_PLC_SMALL_SG_TH_SP_5,0)+ISNULL(a.F_PLC_SMALL_SG_TH_SP_6,0))/6,(ISNULL(a.F_PLC_SMALL_SG_TH_PV_1,0)+ISNULL(a.F_PLC_SMALL_SG_TH_PV_2,0)+ISNULL(a.F_PLC_SMALL_SG_TH_PV_3,0)+ISNULL(a.F_PLC_SMALL_SG_TH_PV_4,0)+ISNULL(a.F_PLC_SMALL_SG_TH_PV_5,0)+ISNULL(a.F_PLC_SMALL_SG_TH_PV_6,0))/6,ISNULL(b.UNCAL_SG_TH_AC_1,0),ISNULL(b.UNCAL_SG_TH_AC_2,0),ISNULL(b.UNCAL_SG_TH_AC_3,0),ISNULL(b.UNCAL_SG_TH_AC_4,0),ISNULL(b.UNCAL_SG_TH_AC_5,0),ISNULL(b.UNCAL_SG_TH_AC_6,0) from C_MFI_PLC_1MIN a,MC_UNIFORMCAL_result b where convert(varchar(16),a.TIMESTAMP,121)=convert(varchar(16),b.TIMESTAMP,121) and b.TIMESTAMP between '" + _d1 + "' and '" + _d2 + "' order by b.TIMESTAMP";
                DataTable data_curve_ls = dBSQL.GetCommand(sql3);
                if (data_curve_ls.Rows.Count > 0)
                {
                    Line1.Clear();
                    Line2.Clear();
                    Line3.Clear();
                    Line4.Clear();
                    Line5.Clear();
                    Line6.Clear();
                    Line7.Clear();
                    Line8.Clear();
                    List<double> Mun1 = new List<double>();
                    List<double> Mun2 = new List<double>();
                    List<double> Mun3 = new List<double>();
                    List<double> Mun4 = new List<double>();
                    List<double> Mun5 = new List<double>();
                    List<double> Mun6 = new List<double>();
                    List<double> Mun7 = new List<double>();
                    List<double> Mun8 = new List<double>();
                    //定义model
                    _myPlotModel = new PlotModel()
                    {
                        Background = OxyColors.White,
                        Title = "历史",
                        TitleFontSize = 7,
                        TitleColor = OxyColors.White,
                        //LegendMargin = 100,
                    };
                    //X轴
                    _dateAxis = new DateTimeAxis()
                    {
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dot,
                        IntervalLength = 100,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        AxisTickToLabelDistance = 0,
                        FontSize = 9.0,
                    };
                    _myPlotModel.Axes.Add(_dateAxis);
                    for (int i = 0; i < data_curve_ls.Rows.Count; i++)
                    {
                        //******设定料厚******
                        OxyPlot.DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i][1]));
                        Line1.Add(line1);
                        Mun1.Add(Convert.ToDouble(data_curve_ls.Rows[i][1]));
                        //*****实际料厚*****
                        DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i][2]));
                        Line2.Add(line2);
                        Mun2.Add(Convert.ToDouble(data_curve_ls.Rows[i][2]));
                        //*****目标料位*****
                        DataPoint line3 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i][3]));
                        Line3.Add(line3);
                        Mun3.Add(Convert.ToDouble(data_curve_ls.Rows[i][3]));
                        //*****实际料位*****
                        DataPoint line4 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i][4]));
                        Line4.Add(line4);
                        Mun4.Add(Convert.ToDouble(data_curve_ls.Rows[i][4]));
                        //*****圆辊转速pv*****
                        DataPoint line5 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i][5]));
                        Line5.Add(line5);
                        Mun5.Add(Convert.ToDouble(data_curve_ls.Rows[i][5]));
                        //*****烧结机速pv*****
                        DataPoint line6 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i][6]));
                        Line6.Add(line6);
                        Mun6.Add(Convert.ToDouble(data_curve_ls.Rows[i][6]));
                        //*****总料量*****
                        DataPoint line7 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i][7]));
                        Line7.Add(line7);
                        Mun7.Add(Convert.ToDouble(data_curve_ls.Rows[i][7]));
                        //*****BTP位置*****
                        DataPoint line8 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i][8]));
                        Line8.Add(line8);
                        Mun8.Add(Convert.ToDouble(data_curve_ls.Rows[i][8]));
                    }

                    int x = 1;
                    if ((int)((Mun1.Max() - Mun1.Min()) / 5) > 0)
                    {
                        x = (int)((Mun1.Max() - Mun1.Min()) / 5);
                    }
                    _valueAxis1 = new LinearAxis()
                    {
                        Key = curve_name[0],
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun1.Max() + 1),
                        Minimum = (int)(Mun1.Min() - 1),
                        PositionTier = 1,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Red,
                        MinorTicklineColor = OxyColors.Red,
                        TicklineColor = OxyColors.Red,
                        TextColor = OxyColors.Red,
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x,
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis1);
                    //添加曲线
                    series1 = new OxyPlot.Series.LineSeries()
                    {
                        Color = OxyColors.Red,
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Red,
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[0],
                        ItemsSource = Line1,
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n设定料厚::{4}",

                    };
                    if (checkBox7.Checked == true)
                    {
                        _valueAxis1.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series1);
                    }


                    int x_1 = 1;//判断增长数据
                    if ((int)((Mun2.Max() - Mun2.Min()) / 5) > 0)
                    {
                        x_1 = (int)((Mun2.Max() - Mun2.Min()) / 5);
                    }
                    _valueAxis2 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[1],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun2.Max() + 1),//极值
                        Minimum = (int)(Mun2.Min() - 1),
                        PositionTier = 2,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Purple,//颜色
                        MinorTicklineColor = OxyColors.Purple,//颜色
                        TicklineColor = OxyColors.Purple,//颜色
                        TextColor = OxyColors.Purple,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_1,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis2);//添加曲线

                    series2 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Purple,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Purple,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[1],//识别符
                        ItemsSource = Line2,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n实际料厚:{4}",

                    };
                    if (checkBox8.Checked == true)
                    {
                        _valueAxis2.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series2);
                    }

                    int x_2 = 1;//判断增长数据
                    if ((int)((Mun3.Max() - Mun3.Min()) / 5) > 0)
                    {
                        x_2 = (int)((Mun3.Max() - Mun3.Min()) / 5);
                    }
                    _valueAxis3 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[2],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun3.Max() + 1),//极值
                        Minimum = (int)(Mun3.Min() - 1),
                        PositionTier = 3,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Green,//颜色
                        MinorTicklineColor = OxyColors.Green,//颜色
                        TicklineColor = OxyColors.Green,//颜色
                        TextColor = OxyColors.Green,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_2,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis3);//添加曲线

                    series3 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Green,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Green,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[2],//识别符
                        ItemsSource = Line3,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n闸门1应调料厚:{4}",

                    };
                    if (checkBox1.Checked == true)
                    {
                        _valueAxis3.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series3);
                    }

                    int x_3 = 1;//判断增长数据
                    if ((int)((Mun4.Max() - Mun4.Min()) / 5) > 0)
                    {
                        x_3 = (int)((Mun4.Max() - Mun4.Min()) / 5);
                    }
                    _valueAxis4 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[3],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun4.Max() + 1),//极值
                        Minimum = (int)(Mun4.Min() - 1),
                        PositionTier = 4,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Blue,//颜色
                        MinorTicklineColor = OxyColors.Blue,//颜色
                        TicklineColor = OxyColors.Blue,//颜色
                        TextColor = OxyColors.Blue,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_3,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis4);//添加曲线

                    series4 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Blue,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Blue,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[3],//识别符
                        ItemsSource = Line4,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n闸门2应调料厚:{4}",

                    };
                    if (checkBox2.Checked == true)
                    {
                        _valueAxis4.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series4);
                    }


                    int x_4 = 1;//判断增长数据
                    if ((int)((Mun5.Max() - Mun5.Min()) / 5) > 0)
                    {
                        x_4 = (int)((Mun5.Max() - Mun5.Min()) / 5);
                    }
                    _valueAxis5 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[4],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun5.Max() + 1),//极值
                        Minimum = (int)(Mun5.Min() - 1),
                        PositionTier = 5,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Olive,//颜色
                        MinorTicklineColor = OxyColors.Olive,//颜色
                        TicklineColor = OxyColors.Olive,//颜色
                        TextColor = OxyColors.Olive,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_4,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis5);//添加曲线

                    series5 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Olive,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Olive,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[4],//识别符
                        ItemsSource = Line5,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n闸门3应调料厚::{4}",

                    };
                    if (checkBox3.Checked == true)
                    {
                        _valueAxis5.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series5);
                    }


                    int x_5 = 1;//判断增长数据
                    if ((int)((Mun6.Max() - Mun6.Min()) / 5) > 0)
                    {
                        x_5 = (int)((Mun6.Max() - Mun6.Min()) / 5);
                    }
                    _valueAxis6 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[5],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun6.Max() + 1),//极值
                        Minimum = (int)(Mun6.Min() - 1),
                        PositionTier = 6,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Black,//颜色
                        MinorTicklineColor = OxyColors.Black,//颜色
                        TicklineColor = OxyColors.Black,//颜色
                        TextColor = OxyColors.Black,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_5,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis6);//添加曲线

                    series6 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Black,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Black,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[5],//识别符
                        ItemsSource = Line6,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n闸门4应调料厚:{4}",

                    };
                    if (checkBox4.Checked == true)
                    {
                        _valueAxis6.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series6);
                    }

                    int x_6 = 1;//判断增长数据
                    if ((int)((Mun7.Max() - Mun7.Min()) / 5) > 0)
                    {
                        x_6 = (int)((Mun7.Max() - Mun7.Min()) / 5);
                    }
                    _valueAxis7 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[6],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun7.Max() + 1),//极值
                        Minimum = (int)(Mun7.Min() - 1),
                        PositionTier = 7,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Orange,//颜色
                        MinorTicklineColor = OxyColors.Orange,//颜色
                        TicklineColor = OxyColors.Orange,//颜色
                        TextColor = OxyColors.Orange,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_6,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis7);//添加曲线

                    series7 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Orange,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Orange,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[6],//识别符
                        ItemsSource = Line7,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n闸门4应调料厚:{4}",

                    };
                    if (checkBox5.Checked == true)
                    {
                        _valueAxis7.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series7);
                    }

                    int x_7 = 1;//判断增长数据
                    if ((int)((Mun8.Max() - Mun8.Min()) / 5) > 0)
                    {
                        x_7 = (int)((Mun8.Max() - Mun8.Min()) / 5);
                    }
                    _valueAxis8 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[7],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun8.Max() + 1),//极值
                        Minimum = (int)(Mun8.Min() - 1),
                        PositionTier = 8,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Fuchsia,//颜色
                        MinorTicklineColor = OxyColors.Fuchsia,//颜色
                        TicklineColor = OxyColors.Fuchsia,//颜色
                        TextColor = OxyColors.Fuchsia,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_7,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis8);//添加曲线

                    series8 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Fuchsia,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Fuchsia,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[7],//识别符
                        ItemsSource = Line8,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n闸门5应调料厚:{4}",

                    };
                    if (checkBox6.Checked == true)
                    {
                        _valueAxis8.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series8);
                    }
                    curve_his.Model = _myPlotModel;
                    var PlotController = new OxyPlot.PlotController();
                    PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
                    curve_his.Controller = PlotController;
                }

            }
            catch (Exception ee)
            {
                string mistake = "趋势曲线历史报错" + ee.ToString();
                //  vLog.writelog(mistake, -1);
            }
        }

        //单击事件
        private void check_event(object sender, EventArgs e)
        {
            try
            {
                //lChartPlus9.ToggleCheckBoxY(sender, e, 0);
                //   lChartPlus22.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private bool changed;

        private void lChartPlus7_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            Check_Cancel();
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Check_All();
        }
        public void Check_All()
        {
            checkBox1.Checked = true;
            checkBox2.Checked = true;
            checkBox3.Checked = true;
            checkBox4.Checked = true;
            checkBox5.Checked = true;
            checkBox6.Checked = true;
            checkBox7.Checked = true;
            checkBox8.Checked = true;
        }
        /// <summary>
        /// 取消全选
        /// </summary>
        public void Check_Cancel()
        {
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = true;
            checkBox8.Checked = true;

        }

        public void Timer_state()
        {
            _Timer1.Start();
        }
        public void Timer_stop()
        {
            _Timer1.Stop();
        }
        public void _Clear()
        {
            _Timer1.Close();
            this.Dispose();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 开始时间&结束时间赋值
        /// </summary>
        public void dateTimePicker_value()
        {
            //结束时间
            DateTime time_end = DateTime.Now;
            //开始时间
            DateTime time_begin = time_end.AddDays(-1);

            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkBox7.Checked == true)
                {
                    _valueAxis1.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series1);
                }
                if (checkBox7.Checked == false)
                {
                    _valueAxis1.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series1);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkBox8.Checked == true)
                {
                    _valueAxis2.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series2);
                }
                if (checkBox8.Checked == false)
                {
                    _valueAxis2.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series2);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkBox1.Checked == true)
                {
                    _valueAxis3.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series3);
                }
                if (checkBox1.Checked == false)
                {
                    _valueAxis3.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series3);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkBox2.Checked == true)
                {
                    _valueAxis4.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series4);
                }
                if (checkBox2.Checked == false)
                {
                    _valueAxis4.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series4);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkBox3.Checked == true)
                {
                    _valueAxis5.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series5);
                }
                if (checkBox3.Checked == false)
                {
                    _valueAxis5.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series5);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkBox4.Checked == true)
                {
                    _valueAxis6.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series6);
                }
                if (checkBox4.Checked == false)
                {
                    _valueAxis6.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series6);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkBox5.Checked == true)
                {
                    _valueAxis7.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series7);
                }
                if (checkBox5.Checked == false)
                {
                    _valueAxis7.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series7);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkBox6.Checked == true)
                {
                    _valueAxis8.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series8);
                }
                if (checkBox6.Checked == false)
                {
                    _valueAxis8.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series8);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }
    }
}
