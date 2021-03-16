using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLog;
using LY_SINTER.Custom;
using DataBase;
using OxyPlot;
using OxyPlot.Axes;
using LY_SINTER.Popover.Quality;

namespace LY_SINTER.PAGE.Quality
{
    public partial class Quality_automatic : UserControl
    {
        #region 曲线定义

        private DateTimeAxis _dateAxis;//X轴
        private DateTimeAxis _dateAxis2;//X轴
        private DateTimeAxis _dateAxis3;//X轴
        private DateTimeAxis _dateAxis4;//X轴

        private LinearAxis _valueAxis1, _valueAxis1_1;//Y轴
        private LinearAxis _valueAxis2, _valueAxis2_1;//Y轴
        private LinearAxis _valueAxis3, _valueAxis3_1;//Y轴
        private LinearAxis _valueAxis4, _valueAxis4_1;//Y轴
        private LinearAxis _valueAxis5, _valueAxis5_1;//Y轴
        private LinearAxis _valueAxis6, _valueAxis6_1;//Y轴
        private LinearAxis _valueAxis7, _valueAxis7_1;//Y轴
        private LinearAxis _valueAxis8, _valueAxis8_1;//Y轴
        private LinearAxis _valueAxis9, _valueAxis9_1;//Y轴
        private LinearAxis _valueAxis10, _valueAxis10_1;//Y轴
        private LinearAxis _valueAxis11, _valueAxis11_1;//Y轴
        private LinearAxis _valueAxis12, _valueAxis12_1;//Y轴
        private LinearAxis _valueAxis13, _valueAxis13_1;//Y轴
        private LinearAxis _valueAxis14, _valueAxis14_1;//Y轴
        private LinearAxis _valueAxis15, _valueAxis15_1;//Y轴
        private LinearAxis _valueAxis16, _valueAxis16_1;//Y轴
        private LinearAxis _valueAxis17, _valueAxis17_1;//Y轴
        private LinearAxis _valueAxis18, _valueAxis18_1;//Y轴

        private PlotModel _myPlotModel, _myPlotModel_1;
        private PlotModel _myPlotMode2, _myPlotMode2_1;
        private PlotModel _myPlotMode3, _myPlotMode3_1;
        private PlotModel _myPlotMode4, _myPlotMode4_1;

        private List<DataPoint> Line1 = new List<DataPoint>();
        private List<DataPoint> Line2 = new List<DataPoint>();
        private List<DataPoint> Line3 = new List<DataPoint>();
        private List<DataPoint> Line4 = new List<DataPoint>();
        private List<DataPoint> Line5 = new List<DataPoint>();
        private List<DataPoint> Line6 = new List<DataPoint>();
        private List<DataPoint> Line7 = new List<DataPoint>();
        private List<DataPoint> Line8 = new List<DataPoint>();
        private List<DataPoint> Line9 = new List<DataPoint>();
        private List<DataPoint> Line10 = new List<DataPoint>();
        private List<DataPoint> Line11 = new List<DataPoint>();
        private List<DataPoint> Line12 = new List<DataPoint>();
        private List<DataPoint> Line13 = new List<DataPoint>();
        private List<DataPoint> Line14 = new List<DataPoint>();
        private List<DataPoint> Line15 = new List<DataPoint>();
        private List<DataPoint> Line16 = new List<DataPoint>();
        private List<DataPoint> Line17 = new List<DataPoint>();
        private List<DataPoint> Line18 = new List<DataPoint>();

        private OxyPlot.Series.LineSeries series1, series1_1;
        private OxyPlot.Series.LineSeries series2, series2_1;
        private OxyPlot.Series.LineSeries series3, series3_1;
        private OxyPlot.Series.LineSeries series4, series4_1;
        private OxyPlot.Series.LineSeries series5, series5_1;
        private OxyPlot.Series.LineSeries series6, series6_1;
        private OxyPlot.Series.LineSeries series7, series7_1;
        private OxyPlot.Series.LineSeries series8, series8_1;
        private OxyPlot.Series.LineSeries series9, series9_1;
        private OxyPlot.Series.LineSeries series10, series10_1;
        private OxyPlot.Series.LineSeries series11;
        private OxyPlot.Series.LineSeries series12;
        private OxyPlot.Series.LineSeries series13;
        private OxyPlot.Series.LineSeries series14;
        private OxyPlot.Series.LineSeries series15;
        private OxyPlot.Series.LineSeries series16;
        private OxyPlot.Series.LineSeries series17;
        private OxyPlot.Series.LineSeries series18;
        private string B = "B线段";
        private string A = "A线段";
        private string C = "C线段";
        private string D = "D线段";

        #endregion 曲线定义

        public System.Timers.Timer _Timer1 { get; set; }
        public System.Timers.Timer _Timer2 { get; set; }
        private int _HOUR = 12;//查询时间
        public vLog vLog { get; set; }
        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        public Quality_automatic()
        {
            InitializeComponent();
            if (vLog == null)
                vLog = new vLog(".\\log_Page\\Quality\\Quality_automatic\\");
            dateTimePicker_value1();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            Check_text();//勾选框
            R_text();
            C_text();
            MG_text();
            TIME_NOW();
            HIS_CURVE_SS(DateTime.Now.AddHours(-12), DateTime.Now);
            HIS_CURVE_SS2(DateTime.Now.AddHours(-12), DateTime.Now);
            HIS_CURVE_SS4(DateTime.Now.AddHours(-12), DateTime.Now);
            HIS_CURVE_Test(DateTime.Now.AddMonths(-1), DateTime.Now);

            _Timer1 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { Timer1_Tick_1(); };//响应事件
            _Timer1.Enabled = true;
            _Timer1.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer2 = new System.Timers.Timer(3000);//初始化颜色变化定时器响应事件
            _Timer2.Elapsed += (x, y) => { Timer1_Tick_1(); };//响应事件
            _Timer2.Enabled = true;
            _Timer2.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
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
                Check_text();
                TIME_NOW();
                HIS_CURVE_SS(DateTime.Now.AddHours(-12), DateTime.Now);
                HIS_CURVE_SS2(DateTime.Now.AddHours(-12), DateTime.Now);
                HIS_CURVE_SS4(DateTime.Now.AddHours(-12), DateTime.Now);
            }
        }

        private void Timer1_Tick_2()
        {
            Action invokeAction = new Action(Timer1_Tick_1);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                HIS_CURVE_SS(DateTime.Now.AddHours(-_HOUR), DateTime.Now);
                HIS_CURVE_SS2(DateTime.Now.AddHours(-_HOUR), DateTime.Now);
                HIS_CURVE_SS4(DateTime.Now.AddHours(-_HOUR), DateTime.Now);
                HIS_CURVE_Test(DateTime.Now.AddMonths(-1), DateTime.Now);
            }
        }

        public void HIS_CURVE_Test(DateTime time_BIGIN, DateTime time_END)
        {
            try
            {
                List<double> Mun1 = new List<double>();
                List<double> Mun2 = new List<double>();
                List<double> Mun3 = new List<double>();
                List<double> Mun4 = new List<double>();
                List<double> Mun5 = new List<double>();
                List<double> Mun6 = new List<double>();
                List<double> Mun7 = new List<double>();
                List<double> Mun8 = new List<double>();
                List<double> Mun9 = new List<double>();
                List<double> Mun10 = new List<double>();

                Line1.Clear();
                Line2.Clear();
                Line3.Clear();
                Line4.Clear();
                Line5.Clear();
                Line6.Clear();
                Line7.Clear();
                Line8.Clear();
                Line9.Clear();
                Line10.Clear();
                //定义model
                _myPlotModel_1 = new PlotModel()
                {
                    Background = OxyColors.White,
                    Title = "历史",
                    TitleFontSize = 7,
                    TitleColor = OxyColors.White,
                    //LegendMargin = 100,
                };
                //X轴
                var _dateAxis = new DateTimeAxis()
                {
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    IntervalLength = 100,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    AxisTickToLabelDistance = 0,
                    FontSize = 9.0,
                    StringFormat = "yyyy/MM/dd HH:mm",
                };
                _myPlotModel_1.Axes.Add(_dateAxis);

                //Y轴

                DBSQL dBSQL1 = new DBSQL(ConstParameters.strCon);
                string sql1 = "select TIMESTAMP,SINCAL_R_A_CURVE,SINCAL_SIN_SP_R_CURVE,C_R_CURVE,SINCAL_MG_A_CURVE,SINCAL_SIN_SP_MGO_CURVE,C_MGO_CURVE,PAR_AIM_FEO_CURVE,C_FEO_CURVE,SINCAL_C_A_CURVE,SINCAL_MIX_SP_C_CURVE,SINCAL_BFES_ORE_BILL_DRY_CURVE,SINCAL_FUEL_BILL_DRY_CURVE,SINCAL_BRUN_DRY_CURVE,SINCAL_SIN_SP_FEO_CURVE,SINCAL_MIX_SP_LOT_CURVE,SINCAL_NON_FUEL_SP_C_CURVE,BTPCAL_OUT_X_AVG_BTP_CURVE,SIN_PLC_MA_SB_1_FLUE_TE_CURVE,SIN_PLC_MA_SB_2_FLUE_TE_CURVE  from C_MAT_L2_RCAL_CUR_MIN where TIMESTAMP >= '" + time_BIGIN + "' and TIMESTAMP <= '" + time_END + "' order by TIMESTAMP";
                DataTable table1 = dBSQL1.GetCommand(sql1);

                for (int i = 0; i < table1.Rows.Count; i++)
                {
                    DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_R_A_CURVE"]));
                    Line1.Add(line1);
                    Mun1.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_R_A_CURVE"]));
                    DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_SIN_SP_R_CURVE"]));
                    Line2.Add(line2);
                    Mun2.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_SIN_SP_R_CURVE"]));
                    DataPoint line3 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["C_R_CURVE"]));
                    Line3.Add(line3);
                    Mun3.Add(Convert.ToDouble(table1.Rows[i]["C_R_CURVE"]));
                    //C自动控制
                    DataPoint line4 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["PAR_AIM_FEO_CURVE"]));
                    Line4.Add(line4);
                    Mun4.Add(Convert.ToDouble(table1.Rows[i]["PAR_AIM_FEO_CURVE"]));
                    DataPoint line5 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["C_FEO_CURVE"]));
                    Line5.Add(line5);
                    Mun5.Add(Convert.ToDouble(table1.Rows[i]["C_FEO_CURVE"]));
                    DataPoint line6 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_C_A_CURVE"]));
                    Line6.Add(line6);
                    Mun6.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_C_A_CURVE"]));
                    DataPoint line7 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_MIX_SP_C_CURVE"]));
                    Line7.Add(line7);
                    Mun7.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_MIX_SP_C_CURVE"]));
                    //mg自动控制
                    DataPoint line8 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_MG_A_CURVE"]));
                    Line8.Add(line8);
                    Mun8.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_MG_A_CURVE"]));
                    DataPoint line9 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_SIN_SP_MGO_CURVE"]));
                    Line9.Add(line9);
                    Mun9.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_SIN_SP_MGO_CURVE"]));
                    DataPoint line10 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["C_MGO_CURVE"]));
                    Line10.Add(line10);
                    Mun10.Add(Convert.ToDouble(table1.Rows[i]["C_MGO_CURVE"]));
                }

                _valueAxis1_1 = new LinearAxis()
                {
                    Key = A,
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
                    //MajorStep = 1,
                    MinorTickSize = 0,
                };
                _valueAxis1_1.MajorStep = (_valueAxis1_1.Maximum - _valueAxis1_1.Minimum) / 4;
                _myPlotModel_1.Axes.Add(_valueAxis1_1);
                //添加曲线
                series1_1 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Red,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.BlueViolet,
                    MarkerType = MarkerType.None,
                    YAxisKey = A,
                    ItemsSource = Line1,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nR目标:{4}%",
                };
                if (checkBox1_1.Checked == true)
                {
                    _valueAxis1_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series1_1);
                }
                _valueAxis2_1 = new LinearAxis()
                {
                    Key = B,
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    Maximum = (int)Mun2.Max() + 1,
                    Minimum = (int)Mun2.Min() - 1,
                    PositionTier = 2,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Purple,
                    MinorTicklineColor = OxyColors.Purple,
                    TicklineColor = OxyColors.Purple,
                    TextColor = OxyColors.Purple,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = 1,
                    MinorTickSize = 0,
                };
                _myPlotModel_1.Axes.Add(_valueAxis2_1);
                series2_1 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Purple,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.BlueViolet,
                    MarkerType = MarkerType.None,
                    YAxisKey = B,
                    ItemsSource = Line2,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n实际配料R:{4}%",
                };
                if (checkBox1_2.Checked == true)
                {
                    _valueAxis2_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series2_1);
                }

                _valueAxis3_1 = new LinearAxis()
                {
                    Key = C,
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    Maximum = (int)Mun3.Max() + 1,
                    Minimum = ((int)Mun3.Min() - 1) > 0 ? ((int)Mun3.Min() - 1) : 0,
                    PositionTier = 3,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Fuchsia,
                    MinorTicklineColor = OxyColors.Fuchsia,
                    TicklineColor = OxyColors.Fuchsia,
                    TextColor = OxyColors.Fuchsia,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = 1,
                    MinorTickSize = 0,
                };
                _myPlotModel_1.Axes.Add(_valueAxis3_1);
                series3_1 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Fuchsia,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.BlueViolet,
                    MarkerType = MarkerType.None,
                    YAxisKey = C,
                    ItemsSource = Line3,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nR检验值:{4}",
                };
                if (checkBox1_3.Checked == true)
                {
                    _valueAxis3_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series3_1);
                }
                _valueAxis4_1 = new LinearAxis()
                {
                    Key = "4",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    Maximum = (int)Mun4.Max() + 1,
                    Minimum = (int)Mun4.Min() - 1,
                    PositionTier = 4,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Blue,
                    MinorTicklineColor = OxyColors.Blue,
                    TicklineColor = OxyColors.Blue,
                    TextColor = OxyColors.Blue,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = 1,
                    MinorTickSize = 0,
                };
                _myPlotModel_1.Axes.Add(_valueAxis4_1);

                series4_1 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Blue,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "4",
                    ItemsSource = Line4,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nFeo目标:{4}%",
                };
                if (checkBox1_4.Checked == true)
                {
                    _valueAxis4_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series4_1);
                }

                _valueAxis5_1 = new LinearAxis()
                {
                    Key = "5",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    Maximum = (int)Mun5.Max() + 1,
                    Minimum = (int)Mun5.Min() > 0 ? (int)Mun5.Min() : 0,
                    PositionTier = 5,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.SlateGray,
                    MinorTicklineColor = OxyColors.SlateGray,
                    TicklineColor = OxyColors.SlateGray,
                    TextColor = OxyColors.SlateGray,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = ((int)Mun5.Max() + 1) - ((int)Mun5.Min() > 0 ? (int)Mun5.Min() : 0),
                    MinorTickSize = 0,
                };
                _myPlotModel_1.Axes.Add(_valueAxis5_1);

                //添加曲线

                series5_1 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.SlateGray,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "5",
                    ItemsSource = Line5,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n烧结矿FEO检验值:{4}",
                };
                if (checkBox1_5.Checked == true)
                {
                    _valueAxis5_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series5_1);
                }

                _valueAxis6_1 = new LinearAxis()
                {
                    Key = "6",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    Maximum = (int)Mun6.Max() + 1,
                    Minimum = (int)Mun6.Min() - 1,
                    PositionTier = 6,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Black,
                    MinorTicklineColor = OxyColors.Black,
                    TicklineColor = OxyColors.Black,
                    TextColor = OxyColors.Black,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = 1,
                    MinorTickSize = 0,
                };
                _myPlotModel_1.Axes.Add(_valueAxis6_1);

                //添加曲线

                series6_1 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Black,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "6",
                    ItemsSource = Line6,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nC目标:{4}%",
                };
                if (checkBox1_6.Checked == true)
                {
                    _valueAxis6_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series6_1);
                }

                _valueAxis7_1 = new LinearAxis()
                {
                    Key = "7",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    Maximum = (int)Mun7.Max() + 1,
                    Minimum = (int)Mun7.Min() - 1,
                    PositionTier = 7,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Orange,
                    MinorTicklineColor = OxyColors.Orange,
                    TicklineColor = OxyColors.Orange,
                    TextColor = OxyColors.Orange,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = 1,
                    MinorTickSize = 0,
                };
                _myPlotModel_1.Axes.Add(_valueAxis7_1);

                //添加曲线

                series7_1 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Orange,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "7",
                    ItemsSource = Line7,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n实际配料含碳:{4}%",
                };
                if (checkBox1_7.Checked == true)
                {
                    _valueAxis7_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series7_1);
                }
                //mg自动控制
                _valueAxis8_1 = new LinearAxis()
                {
                    Key = "8",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    Maximum = (int)Mun8.Max() + 1,
                    Minimum = (int)Mun8.Min() - 1,
                    PositionTier = 8,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Green,
                    MinorTicklineColor = OxyColors.Green,
                    TicklineColor = OxyColors.Green,
                    TextColor = OxyColors.Green,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = 1,
                    MinorTickSize = 0,
                };
                _myPlotModel_1.Axes.Add(_valueAxis8_1);
                series8_1 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Green,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "8",
                    ItemsSource = Line8,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nMg目标:{4}"
                };
                if (checkBox1_8.Checked == true)
                {
                    _valueAxis8_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series8_1);
                }

                _valueAxis9_1 = new LinearAxis()
                {
                    Key = "9",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    Maximum = (int)Mun9.Max() + 1,
                    Minimum = (int)Mun9.Min() - 1,
                    PositionTier = 9,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Pink,
                    MinorTicklineColor = OxyColors.Pink,
                    TicklineColor = OxyColors.Pink,
                    TextColor = OxyColors.Pink,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = 1,
                    MinorTickSize = 0,
                };
                _myPlotModel_1.Axes.Add(_valueAxis9_1);

                //添加曲线

                series9_1 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Pink,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "9",
                    ItemsSource = Line9,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n实际配料Mg:{4}",
                };
                if (checkBox1_9.Checked == true)
                {
                    _valueAxis9_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series9_1);
                }

                _valueAxis10_1 = new LinearAxis()
                {
                    Key = "10",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    Maximum = (int)Mun10.Max() + 1,
                    Minimum = ((int)Mun10.Min() - 1) > 0 ? ((int)Mun10.Min() - 1) : 0,
                    PositionTier = 10,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Olive,
                    MinorTicklineColor = OxyColors.Olive,
                    TicklineColor = OxyColors.Olive,
                    TextColor = OxyColors.Olive,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = 1,
                    MinorTickSize = 0,
                };
                _myPlotModel_1.Axes.Add(_valueAxis10_1);

                //添加曲线

                series10_1 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Olive,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "10",
                    ItemsSource = Line10,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n烧结矿Mg检验值:{4}"
                };
                if (checkBox1_10.Checked == true)
                {
                    _valueAxis10_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series10_1);
                }

                plotView5.Model = _myPlotModel_1;
                var PlotController = new OxyPlot.PlotController();
                PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
                plotView5.Controller = PlotController;
            }
            catch (Exception ee)
            {
                vLog.writelog("" + ee.ToString(), -1);
            }
        }

        /// <summary> 开始时间&结束时间赋值 </summary>
        public void dateTimePicker_value1()
        {
            //结束时间
            DateTime time_end = DateTime.Now;
            //开始时间
            DateTime time_begin = time_end.AddDays(-1);

            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }

        /// <summary>
        /// 勾选框显示数据
        /// </summary>
        private void Check_text()
        {
            try
            {
                //碱度目标,实际配料碱度,MG目标\实际配料MG
                string sql1 = "select top 1 convert(numeric(6,2),round(SINCAL_R_A,2)),convert(numeric(7,2),round(SINCAL_SIN_SP_R,2)),convert(numeric(6,2),round(SINCAL_MG_A,2)),convert(numeric(6,2),round(SINCAL_SIN_SP_MGO,2)) from MC_MIXCAL_RESULT_1MIN order by TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    this.checkBox1.Text = "R目标(%):" + dataTable1.Rows[0][0].ToString();
                    this.checkBox1_1.Text = "R目标(%):" + dataTable1.Rows[0][0].ToString();
                    this.checkBox2.Text = "实际配料R(%):" + dataTable1.Rows[0][1].ToString();
                    this.checkBox1_2.Text = "实际配料R(%):" + dataTable1.Rows[0][1].ToString();
                    this.checkBox5.Text = "MgO目标:" + dataTable1.Rows[0][2].ToString();
                    this.checkBox1_8.Text = "MgO目标:" + dataTable1.Rows[0][2].ToString();
                    this.checkBox4.Text = "实际配料MgO:" + dataTable1.Rows[0][3].ToString();
                    this.checkBox1_9.Text = "实际配料MgO:" + dataTable1.Rows[0][3].ToString();
                }
                //烧结矿碱度检验值,烧结矿MG检验值,烧结矿FEO检验值
                string sql10 = "select top (1) C_R,C_MGO,C_FEO from M_SINTER_ANALYSIS order by TIMESTAMP desc";
                DataTable dataTable10 = dBSQL.GetCommand(sql10);
                if (dataTable10.Rows.Count > 0)
                {
                    this.checkBox29.Text = "R检验值:" + Math.Round(double.Parse(dataTable10.Rows[0][0].ToString() == "" ? "0" : dataTable10.Rows[0][0].ToString()), 2).ToString();
                    this.checkBox1_3.Text = "R检验值:" + Math.Round(double.Parse(dataTable10.Rows[0][0].ToString() == "" ? "0" : dataTable10.Rows[0][0].ToString()), 2).ToString();
                    this.checkBox30.Text = "烧结矿FEO检验值:" + Math.Round(double.Parse(dataTable10.Rows[0][2].ToString() == "" ? "0" : dataTable10.Rows[0][2].ToString()), 2).ToString();
                    this.checkBox1_5.Text = "烧结矿FEO检验值:" + Math.Round(double.Parse(dataTable10.Rows[0][2].ToString() == "" ? "0" : dataTable10.Rows[0][2].ToString()), 2).ToString();
                }
                string sql21 = "select top (1) C_MGO_CURVE from C_MAT_L2_RCAL_CUR_MIN order by TIMESTAMP desc";
                DataTable dataTable21 = dBSQL.GetCommand(sql21);
                if (dataTable21.Rows.Count > 0)
                {
                    this.checkBox6.Text = "烧结矿MgO检验值:" + Math.Round(double.Parse(dataTable10.Rows[0][1].ToString() == "" ? "0" : dataTable10.Rows[0][1].ToString()), 2).ToString();
                    this.checkBox1_10.Text = "烧结矿MgO检验值:" + Math.Round(double.Parse(dataTable10.Rows[0][1].ToString() == "" ? "0" : dataTable10.Rows[0][1].ToString()), 2).ToString();
                }
                string sql2 = "select top 1 convert(numeric(6,2),round(PAR_AIM_FEO,2)) from MC_MIXCAL_PAR order by TIMESTAMP desc";
                DataTable dataTable2 = dBSQL.GetCommand(sql2);
                if (dataTable2.Rows.Count > 0)
                {
                    this.checkBox8.Text = "Feo目标(%):" + dataTable2.Rows[0][0].ToString();
                    this.checkBox1_4.Text = "Feo目标(%):" + dataTable2.Rows[0][0].ToString();
                }
                //C目标,实际配料含碳,燃料百分比,返矿百分比,高返百分比,混合料FEO
                string sql3 = "select top 1 convert(numeric(6,2),round(SINCAL_C_A,2)),convert(numeric(7,2),round(SINCAL_MIX_SP_C,2))," +
                              "convert(numeric(6,2),round(SINCAL_BFES_ORE_BILL_DRY / ( SINCAL_BLEND_ORE_BILL_DRY+ SINCAL_BFES_ORE_BILL_DRY+ SINCAL_FLUX_STONE_BILL_DRY+ SINCAL_DOLOMATE_BILL_DRY+ SINCAL_FLUX_BILL_DRY+ SINCAL_FUEL_BILL_DRY+ SINCAL_BRUN_DRY+ SINCAL_ASH_DUST_BILL_DRY)*100,2))," +
                              "convert(numeric(6,2),round(SINCAL_BRUN_DRY / ( SINCAL_BLEND_ORE_BILL_DRY+ SINCAL_BFES_ORE_BILL_DRY+ SINCAL_FLUX_STONE_BILL_DRY+ SINCAL_DOLOMATE_BILL_DRY+ SINCAL_FLUX_BILL_DRY+ SINCAL_FUEL_BILL_DRY+ SINCAL_BRUN_DRY+ SINCAL_ASH_DUST_BILL_DRY)*100,2))," +
                              "convert(numeric(6,2),round(SINCAL_MIX_SP_LOT,2)),convert(numeric(6,2),round(SINCAL_NON_FUEL_SP_C,2))," +
                              "convert(numeric(6,2),round(SINCAL_BFES_ORE_BILL_DRY / ( SINCAL_BLEND_ORE_BILL_DRY+ SINCAL_BFES_ORE_BILL_DRY+ SINCAL_FLUX_STONE_BILL_DRY+ SINCAL_DOLOMATE_BILL_DRY+ SINCAL_FLUX_BILL_DRY+ SINCAL_FUEL_BILL_DRY+ SINCAL_BRUN_DRY+ SINCAL_ASH_DUST_BILL_DRY)*100,2))," +
                              "convert(numeric(6,2),round(SINCAL_SIN_SP_FEO,2))" +
                              " from MC_MIXCAL_RESULT_1MIN order by TIMESTAMP desc";
                DataTable dataTable3 = dBSQL.GetCommand(sql3);
                if (dataTable3.Rows.Count > 0)
                {
                    this.checkBox7.Text = "C目标(%):" + dataTable3.Rows[0][0].ToString();
                    this.checkBox1_6.Text = "C目标(%):" + dataTable3.Rows[0][0].ToString();
                    this.checkBox3.Text = "实际配料含碳(%):" + dataTable3.Rows[0][1].ToString();
                    this.checkBox1_7.Text = "实际配料含碳(%):" + dataTable3.Rows[0][1].ToString();
                    this.checkBox9.Text = "燃料百分比(%):" + dataTable3.Rows[0][2].ToString();
                    this.checkBox10.Text = "返矿百分比(%):" + dataTable3.Rows[0][3].ToString();
                    this.checkBox11.Text = "混合料综合烧损(%):" + dataTable3.Rows[0][4].ToString();
                    this.checkBox12.Text = "非燃料含碳(%):" + dataTable3.Rows[0][5].ToString();
                    this.checkBox31.Text = "高返百分比(%):" + dataTable3.Rows[0][6].ToString();
                    this.checkBox32.Text = "混合料FEO:" + dataTable3.Rows[0][7].ToString();
                }
                //BTP实际
                string sql5 = "select top 1 convert(numeric(5,2),round(BTPCAL_OUT_TOTAL_AVG_X_BTP,2)) from MC_BTPCAL_result_1min";
                DataTable dataTable5 = dBSQL.GetCommand(sql5);
                if (dataTable5.Rows.Count > 0)
                {
                    this.checkBox34.Text = "BTP实际(m):" + dataTable5.Rows[0][0].ToString();
                }
                //总管温度
                string sql6 = "select top 1 convert(numeric(6,2),round((SIN_PLC_MA_SB_1_FLUE_TE+ SIN_PLC_MA_SB_2_FLUE_TE)/2,2)) from C_SIN_PLC_1MIN order by TIMESTAMP desc";
                DataTable dataTable6 = dBSQL.GetCommand(sql6);
                if (dataTable6.Rows.Count > 0)
                {
                    this.checkBox35.Text = "总管温度(℃):" + dataTable6.Rows[0][0].ToString();
                }
            }
            catch (Exception ee)
            {
            }
        }

        /// <summary>
        /// 碱度显示数据
        /// </summary>
        private void R_text()
        {
            try
            {
                string sql = "select top (1) TIMESTAMP,SINCAL_R_AIM,SINCAL_R_TEST,SINCAL_R_RE_ADJ,SINCAL_R_SV_R_BE,SINCAL_R_SV_R from MC_SINCAL_R_result order by TIMESTAMP desc";
                DataTable dataTable = dBSQL.GetCommand(sql);
                if (dataTable.Rows.Count > 0 && dataTable != null)
                {
                    //时间
                    String JD_1 = dataTable.Rows[0][0].ToString() == "" ? "0" : dataTable.Rows[0][0].ToString();
                    this.textBox2.Text = JD_1.ToString();
                    //碱度目标
                    // float JD_2 = float.Parse(dataTable.Rows[0][1].ToString());
                    this.textBox3.Text = dataTable.Rows[0][1].ToString() == "" ? "0" : dataTable.Rows[0][1].ToString();
                    //化验碱度
                    // float JD_3 = float.Parse(dataTable.Rows[0][2].ToString());
                    this.textBox4.Text = dataTable.Rows[0][2].ToString() == "" ? "0" : dataTable.Rows[0][2].ToString();
                    //本次碱度调整量
                    // float JD_4 = float.Parse(dataTable.Rows[0][3].ToString());
                    this.textBox5.Text = dataTable.Rows[0][3].ToString() == "" ? "0" : dataTable.Rows[0][3].ToString();
                    //调整前混合料碱度
                    // float JD_5 = float.Parse(dataTable.Rows[0][4].ToString());

                    this.textBox6.Text = dataTable.Rows[0][4].ToString() == "" ? "0" : dataTable.Rows[0][4].ToString();
                    //调整后混合料碱度
                    // float JD_6 = float.Parse(dataTable.Rows[0][5].ToString());
                    this.textBox7.Text = dataTable.Rows[0][5].ToString() == "" ? "0" : dataTable.Rows[0][5].ToString();
                }

                string sql1 = "select top (1) SINCAL_SIN_PV_R ,SINCAL_MIX_PV_C,SINCAL_SIN_PV_MGO from MC_MIXCAL_RESULT_1MIN order by TIMESTAMP desc";
                DataTable dataTable2 = dBSQL.GetCommand(sql1);
                if (dataTable2.Rows.Count > 0 && dataTable2 != null)
                {
                    //调整后混合料碱度
                    this.textBox8.Text = Math.Round(double.Parse(dataTable2.Rows[0][0].ToString() == "" ? "0" : dataTable2.Rows[0][0].ToString()), 2).ToString();
                    //调整后混合料C
                    this.textBox22.Text = Math.Round(double.Parse(dataTable2.Rows[0][1].ToString() == "" ? "0" : dataTable2.Rows[0][1].ToString()), 2).ToString();
                    //调整后混合料mg
                    this.textBox15.Text = Math.Round(double.Parse(dataTable2.Rows[0][2].ToString() == "" ? "0" : dataTable2.Rows[0][2].ToString()), 2).ToString();
                }
            }
            catch (Exception ee)
            {
                string mistake = "R_text方法错误" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 碳显示数据
        /// </summary>
        private void C_text()
        {
            try
            {
                string sql1 = "select top (1) TIMESTAMP,SINCAL_C_A,SINCAL_C_FEO_TEST,SINCAL_C_COM_RE_ADJ,SINCAL_C_BEFORE_SV_C,SINCAL_C_SV_C from MC_SINCAL_C_result order by TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0 && dataTable1 != null)
                {
                    this.textBox16.Text = dataTable1.Rows[0][0].ToString() == "" ? "0" : dataTable1.Rows[0][0].ToString();
                    this.textBox17.Text = dataTable1.Rows[0][1].ToString() == "" ? "0" : dataTable1.Rows[0][1].ToString();
                    this.textBox18.Text = dataTable1.Rows[0][2].ToString() == "" ? "0" : dataTable1.Rows[0][2].ToString();
                    this.textBox19.Text = dataTable1.Rows[0][3].ToString() == "" ? "0" : dataTable1.Rows[0][3].ToString();
                    this.textBox20.Text = dataTable1.Rows[0][4].ToString() == "" ? "0" : dataTable1.Rows[0][4].ToString();
                    this.textBox21.Text = dataTable1.Rows[0][5].ToString() == "" ? "0" : dataTable1.Rows[0][5].ToString();
                }
            }
            catch (Exception ee)
            {
                string mistake = "C_text方法失败" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// MG显示数据
        /// </summary>
        private void MG_text()
        {
            try
            {
                string sql2 = "select top (1) TIMESTAMP,SINCAL_MG_AIM,SINCAL_MG_TEST,SINCAL_MG_RE_ADJ,SINCAL_MG_SV_R_BE,SINCAL_MG_SV_R from MC_SINCAL_MG_result order by TIMESTAMP desc";
                DataTable dataTable = dBSQL.GetCommand(sql2);
                if (dataTable.Rows.Count > 0)
                {
                    this.textBox9.Text = dataTable.Rows[0][0].ToString();
                    this.textBox10.Text = dataTable.Rows[0][1].ToString() == "" ? "0" : dataTable.Rows[0][1].ToString();
                    this.textBox11.Text = dataTable.Rows[0][2].ToString() == "" ? "0" : dataTable.Rows[0][2].ToString();
                    this.textBox12.Text = dataTable.Rows[0][3].ToString() == "" ? "0" : dataTable.Rows[0][3].ToString();
                    this.textBox13.Text = dataTable.Rows[0][4].ToString() == "" ? "0" : dataTable.Rows[0][4].ToString();
                    this.textBox14.Text = dataTable.Rows[0][5].ToString() == "" ? "0" : dataTable.Rows[0][5].ToString();
                }
            }
            catch (Exception ee)
            {
                string mistake = "MG_text方法失败" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 最新调整时间
        /// </summary>
        private void TIME_NOW()
        {
            try
            {
                String str1 = textBox2.Text;
                String str2 = textBox9.Text;
                if (textBox2.Text != "" && textBox9.Text != "")
                {
                    DateTime dt1 = Convert.ToDateTime(str1);
                    DateTime dt2 = Convert.ToDateTime(str2);
                    if (DateTime.Compare(dt1, dt2) > 0)
                    {
                        label5.Text = "最新调整时间:" + str1;
                    }
                    else
                    {
                        label5.Text = "最新调整时间:" + str2;
                    }
                }
                else
                {
                    label5.Text = " ";
                }
            }
            catch (Exception ee)
            {
                vLog.writelog("TIME_NOW方法错误" + ee.ToString(), -1);
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            HIS_CURVE_Test(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));
        }

        //实时曲线,对应四个plotview

        public void HIS_CURVE_SS(DateTime time_BIGIN, DateTime time_END)
        {
            try
            {
                List<double> Mun1 = new List<double>();//定义上下限
                List<double> Mun2 = new List<double>();
                List<double> Mun3 = new List<double>();

                List<DataPoint> LineS1 = new List<DataPoint>();//x，y数据
                List<DataPoint> LineS2 = new List<DataPoint>();
                List<DataPoint> LineS3 = new List<DataPoint>();

                LineS1.Clear();
                LineS2.Clear();
                LineS2.Clear();
                DBSQL dBSQL1 = new DBSQL(ConstParameters.strCon);
                string sql1 = "select TIMESTAMP,SINCAL_R_A_CURVE,SINCAL_SIN_SP_R_CURVE,C_R_CURVE,SINCAL_MG_A_CURVE,SINCAL_SIN_SP_MGO_CURVE,C_MGO_CURVE,PAR_AIM_FEO_CURVE,C_FEO_CURVE,SINCAL_C_A_CURVE,SINCAL_MIX_SP_C_CURVE,SINCAL_BFES_ORE_BILL_DRY_CURVE,SINCAL_FUEL_BILL_DRY_CURVE,SINCAL_BRUN_DRY_CURVE,SINCAL_SIN_SP_FEO_CURVE,SINCAL_MIX_SP_LOT_CURVE,SINCAL_NON_FUEL_SP_C_CURVE,BTPCAL_OUT_X_AVG_BTP_CURVE,SIN_PLC_MA_SB_1_FLUE_TE_CURVE,SIN_PLC_MA_SB_2_FLUE_TE_CURVE  from C_MAT_L2_RCAL_CUR_MIN where TIMESTAMP >= '" + time_BIGIN + "' and TIMESTAMP <= '" + time_END + "' order by TIMESTAMP";
                DataTable table1 = dBSQL1.GetCommand(sql1);
                for (int i = 0; i < table1.Rows.Count; i++)
                {
                    DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_R_A_CURVE"]));
                    LineS1.Add(line1);
                    Mun1.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_R_A_CURVE"]));
                    DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_SIN_SP_R_CURVE"]));
                    LineS2.Add(line2);
                    Mun2.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_SIN_SP_R_CURVE"]));
                    DataPoint line3 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["C_R_CURVE"]));
                    LineS3.Add(line3);
                    Mun3.Add(Convert.ToDouble(table1.Rows[i]["C_R_CURVE"]));
                }
                //定义model
                //每一个曲线曲线控件对应一个model
                _myPlotModel = new PlotModel()
                {
                    Background = OxyColors.White,
                    Title = "实时1",
                    TitleFontSize = 5,
                    TitleColor = OxyColors.White,
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
                    StringFormat = "yyyy/MM/dd HH:mm",
                    //Title="时间",
                };
                _myPlotModel.Axes.Add(_dateAxis);

                _valueAxis1 = new LinearAxis()
                {
                    Key = A,//关联标志位
                    MajorGridlineStyle = LineStyle.None,//y轴主刻度
                    MinorGridlineStyle = LineStyle.None,//y轴副刻度
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,//x州y州是否旋转
                    IsPanEnabled = false,//是否缩放
                    PositionTier = 1,//控制曲线个数
                    AxislineStyle = LineStyle.Solid,//y轴风格
                    AxislineColor = OxyColors.Red,
                    MinorTicklineColor = OxyColors.Red,
                    TicklineColor = OxyColors.Red,
                    TextColor = OxyColors.Red,
                    FontSize = 9.0,//字体
                    IsAxisVisible = false,//y轴是否可见
                    MinorTickSize = 0,
                };
                if (LineS1.Count != 0)//添加上下限制，始终显示
                {
                    /*_valueAxis1.Maximum = (int)Mun1.Max() + 1;
                    _valueAxis1.Minimum = (int)Mun1.Min() - 1;
                    _valueAxis1.MajorStep = 1;*/
                    int max = (int)Mun1.Max() + 1;
                    int min = ((int)Mun1.Min()) > 0 ? ((int)Mun1.Min() - 1) : 0; ;
                    _valueAxis1.Maximum = getMax(max, min);
                    _valueAxis1.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis1.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis1.MajorStep = min;
                    }
                }

                _myPlotModel.Axes.Add(_valueAxis1);//添加y轴

                //添加曲线

                series1 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Red,
                    StrokeThickness = 1,//格式
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.BlueViolet,
                    MarkerType = MarkerType.None,
                    YAxisKey = A,//曲线关联
                    ItemsSource = LineS1,//赋值
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nR目标:{4}%",
                };
                if (checkBox1.Checked == true)
                {
                    _valueAxis1.IsAxisVisible = true;//曲线是否可以显示
                    _myPlotModel.Series.Add(series1);//曲线控件添加曲线
                }
                _valueAxis2 = new LinearAxis()
                {
                    Key = B,
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun2.Max() + 1,
                    //Minimum = (int)Mun2.Min() - 1,
                    PositionTier = 2,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Purple,
                    MinorTicklineColor = OxyColors.Purple,
                    TicklineColor = OxyColors.Purple,
                    TextColor = OxyColors.Purple,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep = 1,
                    MinorTickSize = 0,
                };
                if (LineS2.Count != 0)
                {
                    /*_valueAxis2.Maximum = (int)Mun2.Max() + 1;
                    _valueAxis2.Minimum = (int)Mun2.Min() - 1;
                    _valueAxis2.MajorStep = 1;*/
                    int max = (int)Mun2.Max() + 1;
                    int min = ((int)Mun2.Min()) > 0 ? ((int)Mun2.Min() - 1) : 0; ;
                    _valueAxis2.Maximum = getMax(max, min);
                    _valueAxis2.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis2.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis2.MajorStep = min;
                    }
                }
                _myPlotModel.Axes.Add(_valueAxis2);
                series2 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Purple,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.BlueViolet,
                    MarkerType = MarkerType.None,
                    YAxisKey = B,
                    ItemsSource = LineS2,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n实际配料:{4}%",
                };
                if (checkBox2.Checked == true)
                {
                    _valueAxis2.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series2);
                }

                _valueAxis3 = new LinearAxis()
                {
                    Key = C,
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun3.Max() + 1,
                    //Minimum = ((int)Mun3.Min() - 1) > 0 ? ((int)Mun3.Min() - 1) : 0,
                    PositionTier = 3,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Green,
                    MinorTicklineColor = OxyColors.Green,
                    TicklineColor = OxyColors.Green,
                    TextColor = OxyColors.Green,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=1,
                    MinorTickSize = 0,
                    //Title= "R检验值",
                };
                if (LineS3.Count != 0)
                {
                    /*_valueAxis3.Maximum = (int)Mun3.Max() + 1;
                    _valueAxis3.Minimum = (int)Mun3.Min() - 1;
                    _valueAxis3.MajorStep = 1;*/
                    int max = (int)Mun3.Max() + 1;
                    int min = ((int)Mun3.Min()) > 0 ? ((int)Mun3.Min() - 1) : 0; ;
                    _valueAxis3.Maximum = getMax(max, min);
                    _valueAxis3.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis3.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis3.MajorStep = min;
                    }
                }
                _myPlotModel.Axes.Add(_valueAxis3);
                series3 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Green,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.BlueViolet,
                    MarkerType = MarkerType.None,
                    YAxisKey = C,
                    ItemsSource = LineS3,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nR检验值:{4}",
                };
                if (checkBox29.Checked == true)
                {
                    _valueAxis3.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series3);
                }
                plotView1.Model = _myPlotModel;
                /*var PlotController = new OxyPlot.PlotController();
                PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
                plotView1.Controller = PlotController;*/
            }
            catch (Exception EE)
            {
                vLog.writelog("HIS_CURVE_SS方法报错" + EE.ToString(), -1);
            }
        }

        public void HIS_CURVE_SS2(DateTime time_BIGIN, DateTime time_END)
        {
            try
            {
                List<double> Mun4 = new List<double>();
                List<double> Mun5 = new List<double>();
                List<double> Mun6 = new List<double>();
                List<double> Mun7 = new List<double>();
                List<double> Mun8 = new List<double>();
                List<double> Mun9 = new List<double>();
                List<double> Mun10 = new List<double>();

                List<DataPoint> LineS4 = new List<DataPoint>();
                List<DataPoint> LineS5 = new List<DataPoint>();
                List<DataPoint> LineS6 = new List<DataPoint>();
                List<DataPoint> LineS7 = new List<DataPoint>();
                List<DataPoint> LineS8 = new List<DataPoint>();
                List<DataPoint> LineS9 = new List<DataPoint>();
                List<DataPoint> LineS10 = new List<DataPoint>();
                DBSQL dBSQL1 = new DBSQL(ConstParameters.strCon);
                string sql1 = "select TIMESTAMP,SINCAL_R_A_CURVE,SINCAL_SIN_SP_R_CURVE,C_R_CURVE,SINCAL_MG_A_CURVE,SINCAL_SIN_SP_MGO_CURVE,C_MGO_CURVE,PAR_AIM_FEO_CURVE,C_FEO_CURVE,SINCAL_C_A_CURVE,SINCAL_MIX_SP_C_CURVE,SINCAL_BFES_ORE_BILL_DRY_CURVE,SINCAL_FUEL_BILL_DRY_CURVE,SINCAL_BRUN_DRY_CURVE,SINCAL_SIN_SP_FEO_CURVE,SINCAL_MIX_SP_LOT_CURVE,SINCAL_NON_FUEL_SP_C_CURVE,BTPCAL_OUT_X_AVG_BTP_CURVE,SIN_PLC_MA_SB_1_FLUE_TE_CURVE,SIN_PLC_MA_SB_2_FLUE_TE_CURVE  from C_MAT_L2_RCAL_CUR_MIN where TIMESTAMP >= '" + time_BIGIN + "' and TIMESTAMP <= '" + time_END + "' order by TIMESTAMP";
                DataTable table1 = dBSQL1.GetCommand(sql1);
                for (int i = 0; i < table1.Rows.Count; i++)
                {
                    DataPoint line4 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["PAR_AIM_FEO_CURVE"]));
                    LineS4.Add(line4);
                    Mun4.Add(Convert.ToDouble(table1.Rows[i]["PAR_AIM_FEO_CURVE"]));
                    DataPoint line5 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["C_FEO_CURVE"]));
                    LineS5.Add(line5);
                    Mun5.Add(Convert.ToDouble(table1.Rows[i]["C_FEO_CURVE"]));
                    DataPoint line6 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_C_A_CURVE"]));
                    LineS6.Add(line6);
                    Mun6.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_C_A_CURVE"]));
                    DataPoint line7 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_MIX_SP_C_CURVE"]));
                    LineS7.Add(line7);
                    Mun7.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_MIX_SP_C_CURVE"]));
                    //mg自动控制
                    DataPoint line8 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_MG_A_CURVE"]));
                    LineS8.Add(line8);
                    Mun8.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_MG_A_CURVE"]));
                    DataPoint line9 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_SIN_SP_MGO_CURVE"]));
                    LineS9.Add(line9);
                    Mun9.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_SIN_SP_MGO_CURVE"]));
                    DataPoint line10 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["C_MGO_CURVE"]));
                    LineS10.Add(line10);
                    Mun10.Add(Convert.ToDouble(table1.Rows[i]["C_MGO_CURVE"]));
                }

                //定义model
                _myPlotMode2 = new PlotModel()
                {
                    Background = OxyColors.White,
                    Title = "实时2",
                    TitleFontSize = 5,
                    TitleColor = OxyColors.White,
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
                    StringFormat = "yyyy/MM/dd HH:mm",
                };
                _myPlotMode2.Axes.Add(_dateAxis);

                //Y轴
                _valueAxis4 = new LinearAxis()
                {
                    Key = A,
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun4.Max() + 1,
                    //Minimum = (int)Mun4.Min() - 1,
                    PositionTier = 1,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Red,
                    MinorTicklineColor = OxyColors.Red,
                    TicklineColor = OxyColors.Red,
                    TextColor = OxyColors.Red,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=1,
                    MinorTickSize = 0,
                };
                if (LineS4.Count != 0)
                {
                    int max = (int)Mun4.Max() + 1;
                    int min = ((int)Mun4.Min() - 1) > 0 ? ((int)Mun4.Min() - 1) : 0; ;
                    _valueAxis4.Maximum = getMax(max, min);
                    _valueAxis4.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis4.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis4.MajorStep = min;
                    }
                }
                _myPlotMode2.Axes.Add(_valueAxis4);

                series4 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Red,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = A,
                    ItemsSource = LineS4,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nFeo目标:{4}%",
                };
                if (checkBox8.Checked == true)
                {
                    _valueAxis4.IsAxisVisible = true;
                    _myPlotMode2.Series.Add(series4);
                }

                _valueAxis5 = new LinearAxis()
                {
                    Key = B,
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun5.Max() + 1,
                    //Minimum = ((int)Mun5.Min() - 1)>0? ((int)Mun5.Min() - 1):0,
                    PositionTier = 2,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Purple,
                    MinorTicklineColor = OxyColors.Purple,
                    TicklineColor = OxyColors.Purple,
                    TextColor = OxyColors.Purple,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep= ((int)Mun5.Max() + 1) - ((int)Mun5.Min() > 0 ? (int)Mun5.Min() : 0),
                    MinorTickSize = 0,
                };
                if (LineS5.Count != 0)
                {
                    int max = (int)Mun5.Max() + 1;
                    int min = ((int)Mun5.Min()) > 0 ? ((int)Mun5.Min() - 1) : 0; ;
                    _valueAxis5.Maximum = getMax(max, min);
                    _valueAxis5.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis5.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis5.MajorStep = min;
                    }
                }
                _myPlotMode2.Axes.Add(_valueAxis5);
                //添加曲线
                series5 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Purple,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = B,
                    ItemsSource = LineS5,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n烧结矿FEO检验值:{4}",
                };
                if (checkBox30.Checked == true)
                {
                    _valueAxis5.IsAxisVisible = true;
                    _myPlotMode2.Series.Add(series5);
                }
                _valueAxis6 = new LinearAxis()
                {
                    Key = C,
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun6.Max() + 1,
                    //Minimum = (int)Mun6.Min() - 1,
                    PositionTier = 3,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Green,
                    MinorTicklineColor = OxyColors.Green,
                    TicklineColor = OxyColors.Green,
                    TextColor = OxyColors.Green,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=1,
                    MinorTickSize = 0,
                };
                if (LineS6.Count != 0)
                {
                    //_valueAxis6.Maximum = (int)Mun6.Max() + 1;
                    //_valueAxis6.Minimum = ((int)Mun6.Min() - 1) > 0 ? ((int)Mun6.Min() - 1) : 0;
                    //_valueAxis6.MajorStep = 1;
                    int max = (int)Mun6.Max() + 1;
                    int min = ((int)Mun6.Min()) > 0 ? ((int)Mun6.Min() - 1) : 0; ;
                    _valueAxis6.Maximum = getMax(max, min);
                    _valueAxis6.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis6.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis6.MajorStep = min;
                    }
                }
                _myPlotMode2.Axes.Add(_valueAxis6);
                //添加曲线
                series6 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Green,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = C,
                    ItemsSource = LineS6,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nC目标:{4}%",
                };
                if (checkBox7.Checked == true)
                {
                    _valueAxis6.IsAxisVisible = true;
                    _myPlotMode2.Series.Add(series6);
                }
                _valueAxis7 = new LinearAxis()
                {
                    Key = D,
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    PositionTier = 4,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Blue,
                    MinorTicklineColor = OxyColors.Blue,
                    TicklineColor = OxyColors.Blue,
                    TextColor = OxyColors.Blue,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MinorTickSize = 0,
                };
                if (LineS7.Count != 0)
                {
                    //_valueAxis7.Maximum = (int)Mun7.Max() + 1;
                    //_valueAxis7.Minimum = ((int)Mun7.Min() - 1) > 0 ? ((int)Mun7.Min() - 1) : 0;
                    //_valueAxis7.MajorStep = 1;
                    int max = (int)Mun7.Max() + 1;
                    int min = ((int)Mun7.Min()) > 0 ? ((int)Mun7.Min() - 1) : 0; ;
                    _valueAxis7.Maximum = getMax(max, min);
                    _valueAxis7.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis7.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis7.MajorStep = min;
                    }
                }
                _myPlotMode2.Axes.Add(_valueAxis7);
                //添加曲线
                series7 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Blue,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = D,
                    ItemsSource = LineS7,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n实际配料含碳:{4}%"
                };
                if (checkBox3.Checked == true)
                {
                    _valueAxis7.IsAxisVisible = true;
                    _myPlotMode2.Series.Add(series7);
                }
                plotView2.Model = _myPlotMode2;
                /*var PlotController = new OxyPlot.PlotController();
                PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
                plotView2.Controller = PlotController;*/
                //mg自动控制
                _myPlotMode3 = new PlotModel()
                {
                    Background = OxyColors.White,
                    Title = "实时3",
                    TitleFontSize = 5,
                    TitleColor = OxyColors.White,
                };
                //X轴
                _dateAxis3 = new DateTimeAxis()
                {
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    IntervalLength = 100,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    AxisTickToLabelDistance = 0,
                    FontSize = 9.0,
                    StringFormat = "yyyy/MM/dd HH:mm",
                };
                _myPlotMode3.Axes.Add(_dateAxis3);
                _valueAxis8 = new LinearAxis()
                {
                    Key = "8",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun8.Max() + 1,
                    //Minimum = (int)Mun8.Min() - 1,
                    PositionTier = 1,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Red,
                    MinorTicklineColor = OxyColors.Red,
                    TicklineColor = OxyColors.Red,
                    TextColor = OxyColors.Red,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=1,
                    MinorTickSize = 0,
                };
                if (LineS8.Count != 0)
                {
                    //_valueAxis8.Maximum = (int)Mun8.Max() + 1;
                    //_valueAxis8.Minimum = ((int)Mun8.Min() - 1) > 0 ? ((int)Mun8.Min() - 1) : 0;
                    //_valueAxis8.MajorStep = 1;
                    int max = (int)Mun8.Max() + 1;
                    int min = ((int)Mun8.Min()) > 0 ? ((int)Mun8.Min() - 1) : 0; ;
                    _valueAxis8.Maximum = getMax(max, min);
                    _valueAxis8.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis8.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis8.MajorStep = min;
                    }
                }
                _myPlotMode3.Axes.Add(_valueAxis8);
                series8 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Red,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "8",
                    ItemsSource = LineS8,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nMg目标:{4}"
                };
                if (checkBox5.Checked == true)
                {
                    _valueAxis8.IsAxisVisible = true;
                    _myPlotMode3.Series.Add(series8);
                }
                _valueAxis9 = new LinearAxis()
                {
                    Key = B,
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun9.Max() + 1,
                    //Minimum = (int)Mun9.Min() - 1,
                    PositionTier = 2,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Purple,
                    MinorTicklineColor = OxyColors.Purple,
                    TicklineColor = OxyColors.Purple,
                    TextColor = OxyColors.Purple,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=1,
                    MinorTickSize = 0,
                };
                if (LineS9.Count != 0)
                {
                    /*_valueAxis9.Maximum = (int)Mun9.Max() + 1;
                    _valueAxis9.Minimum = ((int)Mun9.Min() - 1) > 0 ? ((int)Mun9.Min() - 1) : 0;
                    _valueAxis9.MajorStep = 1;*/
                    int max = (int)Mun9.Max() + 1;
                    int min = ((int)Mun9.Min()) > 0 ? ((int)Mun9.Min() - 1) : 0; ;
                    _valueAxis9.Maximum = getMax(max, min);
                    _valueAxis9.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis9.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis9.MajorStep = min;
                    }
                }
                _myPlotMode3.Axes.Add(_valueAxis9);
                //添加曲线
                series9 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Purple,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = B,
                    ItemsSource = LineS9,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n实际配料Mg:{4}"
                };
                if (checkBox4.Checked == true)
                {
                    _valueAxis9.IsAxisVisible = true;
                    _myPlotMode3.Series.Add(series9);
                }
                _valueAxis10 = new LinearAxis()
                {
                    Key = C,
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun10.Max()+1,
                    //Minimum = ((int)Mun10.Min() - 1)>0? ((int)Mun10.Min() - 1):0,
                    PositionTier = 3,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Green,
                    MinorTicklineColor = OxyColors.Green,
                    TicklineColor = OxyColors.Green,
                    TextColor = OxyColors.Green,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep = 1,
                    MinorTickSize = 0,
                };
                if (LineS10.Count != 0)
                {
                    /*_valueAxis10.Maximum = (int)Mun10.Max() + 1;
                    _valueAxis10.Minimum = ((int)Mun10.Min() - 1) > 0 ? ((int)Mun10.Min() - 1) : 0;
                    _valueAxis10.MajorStep = 1;*/
                    int max = (int)Mun10.Max() + 1;
                    int min = ((int)Mun10.Min()) > 0 ? ((int)Mun10.Min() - 1) : 0; ;
                    _valueAxis10.Maximum = getMax(max, min);
                    _valueAxis10.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis10.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis10.MajorStep = min;
                    }
                }
                _myPlotMode3.Axes.Add(_valueAxis10);
                //添加曲线
                series10 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Green,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = C,
                    ItemsSource = LineS10,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n烧结矿Mg检验值:{4}"
                };
                if (checkBox6.Checked == true)
                {
                    _valueAxis10.IsAxisVisible = true;
                    _myPlotMode3.Series.Add(series10);
                }
                plotView3.Model = _myPlotMode3;
                /*plotView3.Controller = PlotController;*/
            }
            catch (Exception ee)
            {
                vLog.writelog("HIS_CURVE_SS2方法报错" + ee.ToString() + ee.ToString(), -1);
            }
        }

        public void HIS_CURVE_SS4(DateTime time_BIGIN, DateTime time_END)
        {
            try
            {
                List<double> Mun11 = new List<double>();
                List<double> Mun12 = new List<double>();
                List<double> Mun13 = new List<double>();
                List<double> Mun14 = new List<double>();
                List<double> Mun15 = new List<double>();
                List<double> Mun16 = new List<double>();
                List<double> Mun17 = new List<double>();
                List<double> Mun18 = new List<double>();

                List<DataPoint> LineS11 = new List<DataPoint>();
                List<DataPoint> LineS12 = new List<DataPoint>();
                List<DataPoint> LineS13 = new List<DataPoint>();
                List<DataPoint> LineS14 = new List<DataPoint>();
                List<DataPoint> LineS15 = new List<DataPoint>();
                List<DataPoint> LineS16 = new List<DataPoint>();
                List<DataPoint> LineS17 = new List<DataPoint>();
                List<DataPoint> LineS18 = new List<DataPoint>();

                DBSQL dBSQL1 = new DBSQL(ConstParameters.strCon);
                string sql1 = "select TIMESTAMP,SINCAL_R_A_CURVE,SINCAL_SIN_SP_R_CURVE,C_R_CURVE,SINCAL_MG_A_CURVE,SINCAL_SIN_SP_MGO_CURVE,C_MGO_CURVE,PAR_AIM_FEO_CURVE,C_FEO_CURVE,SINCAL_C_A_CURVE,SINCAL_MIX_SP_C_CURVE,SINCAL_BFES_ORE_BILL_DRY_CURVE,SINCAL_FUEL_BILL_DRY_CURVE,SINCAL_BRUN_DRY_CURVE,SINCAL_SIN_SP_FEO_CURVE,SINCAL_MIX_SP_LOT_CURVE,SINCAL_NON_FUEL_SP_C_CURVE,BTPCAL_OUT_X_AVG_BTP_CURVE,SIN_PLC_MA_SB_1_FLUE_TE_CURVE,SIN_PLC_MA_SB_2_FLUE_TE_CURVE  from C_MAT_L2_RCAL_CUR_MIN where TIMESTAMP >= '" + time_BIGIN + "' and TIMESTAMP <= '" + time_END + "' order by TIMESTAMP";
                DataTable table1 = dBSQL1.GetCommand(sql1);
                for (int i = 0; i < table1.Rows.Count; i++)
                {
                    DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_FUEL_BILL_DRY_CURVE"]));
                    LineS11.Add(line1);
                    Mun11.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_FUEL_BILL_DRY_CURVE"]));
                    DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_BRUN_DRY_CURVE"]));
                    LineS12.Add(line2);
                    Mun12.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_BRUN_DRY_CURVE"]));
                    DataPoint line3 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SIN_PLC_MA_SB_2_FLUE_TE_CURVE"]));
                    LineS13.Add(line3);
                    Mun13.Add(Convert.ToDouble(table1.Rows[i]["SIN_PLC_MA_SB_2_FLUE_TE_CURVE"]));
                    DataPoint line4 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_NON_FUEL_SP_C_CURVE"]));
                    LineS14.Add(line4);
                    Mun14.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_NON_FUEL_SP_C_CURVE"]));
                    DataPoint line5 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_BFES_ORE_BILL_DRY_CURVE"]));
                    LineS15.Add(line5);
                    Mun15.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_BFES_ORE_BILL_DRY_CURVE"]));
                    DataPoint line6 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_SIN_SP_FEO_CURVE"]));
                    LineS16.Add(line6);
                    Mun16.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_SIN_SP_FEO_CURVE"]));
                    DataPoint line7 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["BTPCAL_OUT_X_AVG_BTP_CURVE"]));
                    LineS17.Add(line7);
                    Mun17.Add(Convert.ToDouble(table1.Rows[i]["BTPCAL_OUT_X_AVG_BTP_CURVE"]));
                    DataPoint line8 = new DataPoint(DateTimeAxis.ToDouble(table1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table1.Rows[i]["SINCAL_MIX_SP_LOT_CURVE"]));
                    LineS18.Add(line8);
                    Mun18.Add(Convert.ToDouble(table1.Rows[i]["SINCAL_MIX_SP_LOT_CURVE"]));
                }
                //定义model
                _myPlotMode4 = new PlotModel()
                {
                    Background = OxyColors.White,
                    Title = "实时4",
                    TitleFontSize = 5,
                    TitleColor = OxyColors.White,
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
                    StringFormat = "yyyy/MM/dd HH:mm",
                };
                _myPlotMode4.Axes.Add(_dateAxis);

                //Y轴

                _valueAxis11 = new LinearAxis()
                {
                    Key = A,
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun11.Max() + 1,
                    //Minimum = (int)Mun11.Min() - 1,
                    PositionTier = 1,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Red,
                    MinorTicklineColor = OxyColors.Red,
                    TicklineColor = OxyColors.Red,
                    TextColor = OxyColors.Red,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=1,
                    MinorTickSize = 0,
                };
                if (LineS11.Count != 0)
                {
                    /*_valueAxis11.Maximum = (int)Mun11.Max() + 1;
                    _valueAxis11.Minimum = ((int)Mun11.Min() - 1) > 0 ? ((int)Mun11.Min() - 1) : 0;
                    _valueAxis11.MajorStep = 1;*/
                    _valueAxis11.Maximum = getMax((int)Mun11.Max() + 1, (int)Mun11.Min() - 1);
                    int min = (int)Mun11.Min() - 1;
                    _valueAxis11.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis11.MajorStep = getMax((int)Mun11.Max() + 1, (int)Mun11.Min() - 1);
                    }
                    else
                    {
                        _valueAxis11.MajorStep = min;
                    }
                }
                _myPlotMode4.Axes.Add(_valueAxis11);
                //曲线11
                series11 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Red,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = A,
                    ItemsSource = LineS11,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n燃料百分比:{4}%",
                };
                if (checkBox9.Checked == true)
                {
                    _valueAxis11.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series11);
                }
                _valueAxis12 = new LinearAxis()
                {
                    Key = "12",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun12.Max() + 3,
                    //Minimum = (int)Mun12.Min() - 2,
                    PositionTier = 2,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Purple,
                    MinorTicklineColor = OxyColors.Purple,
                    TicklineColor = OxyColors.Purple,
                    TextColor = OxyColors.Purple,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=4,
                    MinorTickSize = 0,
                };
                if (LineS12.Count != 0)
                {
                    //_valueAxis12.Maximum = (int)Mun12.Max() + 1;
                    //_valueAxis12.Minimum = ((int)Mun12.Min() - 1) > 0 ? ((int)Mun12.Min() - 1) : 0;
                    //_valueAxis8.MajorStep = 1;
                    int max = getMax((int)Mun12.Max() + 1, (int)Mun12.Min() - 1);
                    _valueAxis12.Maximum = max;
                    int min = (int)Mun12.Min() - 1;
                    _valueAxis12.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis12.MajorStep = max;
                    }
                    else
                    {
                        _valueAxis12.MajorStep = min;
                    }
                }
                _myPlotMode4.Axes.Add(_valueAxis12);
                //添加曲线
                series12 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Purple,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "12",
                    ItemsSource = LineS12,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n烧返百分比:{4}%",
                };
                if (checkBox10.Checked == true)
                {
                    _valueAxis12.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series12);
                }
                _valueAxis13 = new LinearAxis()
                {
                    Key = "13",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun13.Max() + 1,
                    //Minimum = ((int)Mun13.Min() - 1)>0? ((int)Mun13.Min() - 1):0,
                    PositionTier = 3,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Fuchsia,
                    MinorTicklineColor = OxyColors.Fuchsia,
                    TicklineColor = OxyColors.Fuchsia,
                    TextColor = OxyColors.Fuchsia,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=137,
                    MinorTickSize = 0,
                };
                if (LineS13.Count != 0)
                {
                    //_valueAxis13.Maximum = (int)Mun13.Max() + 1;
                    //_valueAxis13.Minimum = ((int)Mun13.Min() - 1) > 0 ? ((int)Mun13.Min() - 1) : 0;
                    //_valueAxis8.MajorStep = 1;
                    int max = getMax((int)Mun13.Max() + 1, (int)Mun13.Min() - 1);
                    _valueAxis13.Maximum = max;
                    int min = (int)Mun12.Min() - 1;
                    _valueAxis13.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis13.MajorStep = max;
                    }
                    else
                    {
                        _valueAxis13.MajorStep = (max - min) / 4;
                    }
                }
                _myPlotMode4.Axes.Add(_valueAxis13);
                //添加曲线
                series13 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Fuchsia,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "13",
                    ItemsSource = LineS13,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n总管温度:{4}",
                };
                if (checkBox35.Checked == true)
                {
                    _valueAxis13.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series13);
                }
                _valueAxis14 = new LinearAxis()
                {
                    Key = "14",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun14.Max() + 1,
                    //Minimum = ((int)Mun14.Min() - 1)>0? ((int)Mun14.Min() - 1):0,
                    PositionTier = 4,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Blue,
                    MinorTicklineColor = OxyColors.Blue,
                    TicklineColor = OxyColors.Blue,
                    TextColor = OxyColors.Blue,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=1,
                    MinorTickSize = 0,
                };
                if (LineS14.Count != 0)
                {
                    /*_valueAxis14.Maximum = (int)Mun14.Max() + 1;
                    _valueAxis14.Minimum = ((int)Mun14.Min() - 1) > 0 ? ((int)Mun14.Min() - 1) : 0;
                    _valueAxis14.MajorStep = 1;*/
                    int max = (int)Mun14.Max() + 1;
                    int min = ((int)Mun14.Min() - 1) > 0 ? ((int)Mun14.Min() - 1) : 0; ;
                    _valueAxis14.Maximum = getMax(max, min);
                    _valueAxis14.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis14.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis14.MajorStep = min;
                    }
                }
                _myPlotMode4.Axes.Add(_valueAxis14);
                //添加曲线
                series14 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Blue,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "14",
                    ItemsSource = LineS14,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n非燃料含碳:{4}%",
                };
                if (checkBox12.Checked == true)
                {
                    _valueAxis14.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series14);
                }
                _valueAxis15 = new LinearAxis()
                {
                    Key = "15",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    ///Maximum = (int)Mun15.Max() + 1,
                    //Minimum = ((int)Mun15.Min() - 1)>0? ((int)Mun15.Min() - 1):0,
                    PositionTier = 5,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.SlateGray,
                    MinorTicklineColor = OxyColors.SlateGray,
                    TicklineColor = OxyColors.SlateGray,
                    TextColor = OxyColors.SlateGray,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=2,
                    MinorTickSize = 0,
                };
                if (LineS15.Count != 0)
                {
                    //_valueAxis15.Maximum = (int)Mun15.Max() + 1;
                    //_valueAxis15.Minimum = ((int)Mun15.Min() - 1) > 0 ? ((int)Mun15.Min() - 1) : 0;
                    //_valueAxis15.MajorStep = 2;
                    int max = (int)Mun15.Max() + 1;
                    int min = ((int)Mun15.Min() - 1) > 0 ? ((int)Mun15.Min() - 1) : 0; ;
                    _valueAxis15.Maximum = getMax(max, min);
                    _valueAxis15.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis15.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis15.MajorStep = min;
                    }
                }
                _myPlotMode4.Axes.Add(_valueAxis15);
                //添加曲线
                series15 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.SlateGray,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "15",
                    ItemsSource = LineS15,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n高返百分比:{4}%",
                };
                if (checkBox31.Checked == true)
                {
                    _valueAxis15.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series15);
                }

                _valueAxis16 = new LinearAxis()
                {
                    Key = "16",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun16.Max() + 1,
                    //Minimum = ((int)Mun16.Min() - 1)>0? ((int)Mun16.Min() - 1):0,
                    PositionTier = 6,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Black,
                    MinorTicklineColor = OxyColors.Black,
                    TicklineColor = OxyColors.Black,
                    TextColor = OxyColors.Black,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=1,
                    MinorTickSize = 0,
                };
                if (LineS16.Count != 0)
                {
                    int max = (int)Mun16.Max() + 1;
                    int min = ((int)Mun16.Min() - 1) > 0 ? ((int)Mun16.Min() - 1) : 0;
                    _valueAxis16.Maximum = getMax(max, min);
                    _valueAxis16.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis16.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis16.MajorStep = min;
                    }
                }
                _myPlotMode4.Axes.Add(_valueAxis16);
                //添加曲线
                series16 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Black,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "16",
                    ItemsSource = LineS16,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n混合料FEO:{4}",
                };
                if (checkBox32.Checked == true)
                {
                    _valueAxis16.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series16);
                }
                _valueAxis17 = new LinearAxis()
                {
                    Key = "17",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun17.Max() + 1,
                    //Minimum = ((int)Mun17.Min() - 1)>0? ((int)Mun17.Min() - 1):0,
                    PositionTier = 7,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Orange,
                    MinorTicklineColor = OxyColors.Orange,
                    TicklineColor = OxyColors.Orange,
                    TextColor = OxyColors.Orange,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=87,
                    MinorTickSize = 0,
                };
                if (LineS17.Count != 0)
                {
                    //_valueAxis17.Maximum = (int)Mun17.Max() + 1;
                    //_valueAxis17.Minimum = ((int)Mun17.Min() - 1) > 0 ? ((int)Mun17.Min() - 1) : 0;
                    //_valueAxis8.MajorStep = 1;
                    int max = (int)Mun17.Max() + 1;
                    int min = ((int)Mun17.Min() - 1) > 0 ? ((int)Mun17.Min() - 1) : 0;
                    _valueAxis17.Maximum = getMax(max, min);
                    _valueAxis17.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis17.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis17.MajorStep = min;
                    }
                }
                _myPlotMode4.Axes.Add(_valueAxis17);
                //添加曲线
                series17 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Orange,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "17",
                    ItemsSource = LineS17,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nBTP实际:{4}",
                };
                if (checkBox34.Checked == true)
                {
                    _valueAxis17.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series17);
                }
                _valueAxis18 = new LinearAxis()
                {
                    Key = "18",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    //Maximum = (int)Mun18.Max() + 1,
                    //Minimum = ((int)Mun18.Min() - 1)>0? ((int)Mun18.Min() - 1):0,
                    PositionTier = 8,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Green,
                    MinorTicklineColor = OxyColors.Green,
                    TicklineColor = OxyColors.Green,
                    TextColor = OxyColors.Green,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    //MajorStep=1,
                    MinorTickSize = 0,
                };
                if (LineS18.Count != 0)
                {
                    //_valueAxis18.Maximum = (int)Mun18.Max() + 1;
                    //_valueAxis18.Minimum = ((int)Mun18.Min() - 1) > 0 ? ((int)Mun18.Min() - 1) : 0;
                    //_valueAxis18.MajorStep = 1;
                    int max = (int)Mun18.Max() + 1;
                    int min = ((int)Mun18.Min() - 1) > 0 ? ((int)Mun18.Min() - 1) : 0;
                    _valueAxis18.Maximum = getMax(max, min);
                    _valueAxis18.Minimum = min;
                    if (min == 0)
                    {
                        _valueAxis18.MajorStep = getMax(max, min);
                    }
                    else
                    {
                        _valueAxis18.MajorStep = min;
                    }
                }
                _myPlotMode4.Axes.Add(_valueAxis18);
                //添加曲线
                series18 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Green,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkGreen,
                    MarkerType = MarkerType.None,
                    YAxisKey = "18",
                    ItemsSource = LineS18,
                    TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n混合料综合烧损:{4}%",
                };
                if (checkBox11.Checked == true)
                {
                    _valueAxis18.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series18);
                }
                plotView4.Model = _myPlotMode4;
                /*var PlotController = new OxyPlot.PlotController();
                PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);

                plotView4.Controller = PlotController;*/
            }
            catch (Exception ee)
            {
                vLog.writelog("HIS_CURVE_SS4方法报错" + ee.ToString(), -1);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Frm_Automatic_PAR form_display = new Frm_Automatic_PAR();
            if (Frm_Automatic_PAR.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Frm_Automatic_Adjust form_display = new Frm_Automatic_Adjust();
            if (Frm_Automatic_Adjust.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        public static int getMax(int max, int min)
        {
            int s = 0;
            if (min != 0)
            {
                if (max % min != 0)
                {
                    s = (max / min + 1) * min;
                }
                else
                {
                    s = max;
                }
            }
            else
            {
                s = max;
            }
            return s;
        }

        #region 曲线勾选框点击事件

        private void check_event_1(object sender, EventArgs e)
        {
            try
            {
                plotView1.Model = null;
                if (checkBox1.Checked == true)
                {
                    _valueAxis1.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series1);
                }
                if (checkBox1.Checked == false)
                {
                    _valueAxis1.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series1);
                }

                plotView1.Model = _myPlotModel;

                /*lChartPlus2.ToggleCheckBoxY(sender, e, 0);
                lChartPlus3.ToggleCheckBoxY(sender, e, 0);
                lChartPlus4.ToggleCheckBoxY(sender, e, 0);
                lChartPlus5.ToggleCheckBoxY(sender, e, 0);*/
            }
            catch
            { }
        }

        private void check_event2(object sender, EventArgs e)
        {
            try
            {
                plotView1.Model = null;
                if (checkBox2.Checked == true)
                {
                    _valueAxis2.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series2);
                }
                if (checkBox2.Checked == false)
                {
                    _valueAxis2.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series2);
                }
                plotView1.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void check_event3(object sender, EventArgs e)
        {
            try
            {
                plotView1.Model = null;

                if (checkBox29.Checked == true)
                {
                    _valueAxis3.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series3);
                }
                if (checkBox29.Checked == false)
                {
                    _valueAxis3.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series3);
                }
                plotView1.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void check_event4(object sender, EventArgs e)
        {
            try
            {
                plotView2.Model = null;
                if (checkBox8.Checked == true)
                {
                    _valueAxis4.IsAxisVisible = true;
                    _myPlotMode2.Series.Add(series4);
                }
                if (checkBox8.Checked == false)
                {
                    _valueAxis4.IsAxisVisible = false;
                    _myPlotMode2.Series.Remove(series4);
                }
                plotView2.Model = _myPlotMode2;
            }
            catch
            { }
        }

        private void check_event5(object sender, EventArgs e)
        {
            try
            {
                plotView2.Model = null;
                if (checkBox30.Checked == true)
                {
                    _valueAxis5.IsAxisVisible = true;
                    _myPlotMode2.Series.Add(series5);
                }
                if (checkBox30.Checked == false)
                {
                    _valueAxis5.IsAxisVisible = false;
                    _myPlotMode2.Series.Remove(series5);
                }
                plotView2.Model = _myPlotMode2;
            }
            catch
            { }
        }

        private void check_event6(object sender, EventArgs e)
        {
            try
            {
                plotView2.Model = null;
                if (checkBox7.Checked == true)
                {
                    _valueAxis6.IsAxisVisible = true;
                    _myPlotMode2.Series.Add(series6);
                }
                if (checkBox7.Checked == false)
                {
                    _valueAxis6.IsAxisVisible = false;
                    _myPlotMode2.Series.Remove(series6);
                }
                plotView2.Model = _myPlotMode2;
            }
            catch
            { }
        }

        private void check_event7(object sender, EventArgs e)
        {
            try
            {
                plotView2.Model = null;
                if (checkBox3.Checked == true)
                {
                    _valueAxis7.IsAxisVisible = true;
                    _myPlotMode2.Series.Add(series7);
                }
                if (checkBox3.Checked == false)
                {
                    _valueAxis7.IsAxisVisible = false;
                    _myPlotMode2.Series.Remove(series7);
                }
                plotView2.Model = _myPlotMode2;
            }
            catch
            { }
        }

        private void check_event8(object sender, EventArgs e)
        {
            try
            {
                plotView3.Model = null;
                if (checkBox5.Checked == true)
                {
                    _valueAxis8.IsAxisVisible = true;
                    _myPlotMode3.Series.Add(series8);
                }
                if (checkBox5.Checked == false)
                {
                    _valueAxis8.IsAxisVisible = false;
                    _myPlotMode3.Series.Remove(series8);
                }
                plotView3.Model = _myPlotMode3;
            }
            catch
            { }
        }

        private void check_event9(object sender, EventArgs e)
        {
            try
            {
                plotView3.Model = null;
                if (checkBox4.Checked == true)
                {
                    _valueAxis9.IsAxisVisible = true;
                    _myPlotMode3.Series.Add(series9);
                }
                if (checkBox4.Checked == false)
                {
                    _valueAxis9.IsAxisVisible = false;
                    _myPlotMode3.Series.Remove(series9);
                }
                plotView3.Model = _myPlotMode3;
            }
            catch
            { }
        }

        private void check_event10(object sender, EventArgs e)
        {
            try
            {
                plotView3.Model = null;
                if (checkBox6.Checked == true)
                {
                    _valueAxis10.IsAxisVisible = true;
                    _myPlotMode3.Series.Add(series10);
                }
                if (checkBox6.Checked == false)
                {
                    _valueAxis10.IsAxisVisible = false;
                    _myPlotMode3.Series.Remove(series10);
                }
                plotView3.Model = _myPlotMode3;
            }
            catch
            { }
        }

        private void check_event11(object sender, EventArgs e)
        {
            try
            {
                plotView4.Model = null;
                if (checkBox9.Checked == true)
                {
                    _valueAxis11.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series11);
                }
                if (checkBox9.Checked == false)
                {
                    _valueAxis11.IsAxisVisible = false;
                    _myPlotMode4.Series.Remove(series11);
                }
                plotView4.Model = _myPlotMode4;
            }
            catch
            { }
        }

        private void check_event12(object sender, EventArgs e)
        {
            try
            {
                plotView4.Model = null;
                if (checkBox10.Checked == true)
                {
                    _valueAxis12.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series12);
                }
                if (checkBox10.Checked == false)
                {
                    _valueAxis12.IsAxisVisible = false;
                    _myPlotMode4.Series.Remove(series12);
                }
                plotView4.Model = _myPlotMode4;
            }
            catch
            { }
        }

        private void check_event13(object sender, EventArgs e)
        {
            try
            {
                plotView4.Model = null;
                if (checkBox35.Checked == true)
                {
                    _valueAxis13.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series13);
                }
                if (checkBox35.Checked == false)
                {
                    _valueAxis13.IsAxisVisible = false;
                    _myPlotMode4.Series.Remove(series13);
                }
                plotView4.Model = _myPlotMode4;
            }
            catch
            { }
        }

        private void check_event14(object sender, EventArgs e)
        {
            try
            {
                plotView4.Model = null;
                if (checkBox12.Checked == true)
                {
                    _valueAxis14.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series14);
                }
                if (checkBox12.Checked == false)
                {
                    _valueAxis14.IsAxisVisible = false;
                    _myPlotMode4.Series.Remove(series14);
                }
                plotView4.Model = _myPlotMode4;
            }
            catch
            { }
        }

        private void check_event15(object sender, EventArgs e)
        {
            try
            {
                plotView4.Model = null;
                if (checkBox31.Checked == true)
                {
                    _valueAxis15.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series15);
                }
                if (checkBox31.Checked == false)
                {
                    _valueAxis15.IsAxisVisible = false;
                    _myPlotMode4.Series.Remove(series15);
                }
                plotView4.Model = _myPlotMode4;
            }
            catch
            { }
        }

        private void check_event16(object sender, EventArgs e)
        {
            try
            {
                plotView4.Model = null;
                if (checkBox32.Checked == true)
                {
                    _valueAxis16.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series16);
                }
                if (checkBox32.Checked == false)
                {
                    _valueAxis16.IsAxisVisible = false;
                    _myPlotMode4.Series.Remove(series16);
                }
                plotView4.Model = _myPlotMode4;
            }
            catch
            { }
        }

        private void check_event17(object sender, EventArgs e)
        {
            try
            {
                plotView4.Model = null;
                if (checkBox34.Checked == true)
                {
                    _valueAxis17.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series17);
                }
                if (checkBox34.Checked == false)
                {
                    _valueAxis17.IsAxisVisible = false;
                    _myPlotMode4.Series.Remove(series17);
                }
                plotView4.Model = _myPlotMode4;
            }
            catch
            { }
        }

        private void check_event18(object sender, EventArgs e)
        {
            try
            {
                plotView4.Model = null;
                if (checkBox11.Checked == true)
                {
                    _valueAxis18.IsAxisVisible = true;
                    _myPlotMode4.Series.Add(series18);
                }
                if (checkBox11.Checked == false)
                {
                    _valueAxis18.IsAxisVisible = false;
                    _myPlotMode4.Series.Remove(series18);
                }
                plotView4.Model = _myPlotMode4;
            }
            catch
            { }
        }

        private void check_event1_1(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox1_1.Checked == true)
                {
                    _valueAxis1_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series1_1);
                }
                if (checkBox1_1.Checked == false)
                {
                    _valueAxis1_1.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series1_1);
                }
                plotView5.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        private void check_event1_2(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox1_2.Checked == true)
                {
                    _valueAxis2_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series2_1);
                }
                if (checkBox1_2.Checked == false)
                {
                    _valueAxis2_1.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series2_1);
                }
                plotView5.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        private void check_event1_3(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox1_3.Checked == true)
                {
                    _valueAxis3_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series3_1);
                }
                if (checkBox1_3.Checked == false)
                {
                    _valueAxis3_1.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series3_1);
                }
                plotView5.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        private void check_event1_4(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox1_4.Checked == true)
                {
                    _valueAxis4_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series4_1);
                }
                if (checkBox1_4.Checked == false)
                {
                    _valueAxis4_1.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series4_1);
                }
                plotView5.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        private void check_event1_5(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox1_5.Checked == true)
                {
                    _valueAxis5_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series5_1);
                }
                if (checkBox1_5.Checked == false)
                {
                    _valueAxis5_1.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series5_1);
                }
                plotView5.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        private void check_event1_6(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox1_6.Checked == true)
                {
                    _valueAxis6_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series6_1);
                }
                if (checkBox1_6.Checked == false)
                {
                    _valueAxis6_1.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series6_1);
                }
                plotView5.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        private void check_event1_7(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox1_7.Checked == true)
                {
                    _valueAxis7_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series7_1);
                }
                if (checkBox1_7.Checked == false)
                {
                    _valueAxis7_1.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series7_1);
                }
                plotView5.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        private void check_event1_8(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox1_8.Checked == true)
                {
                    _valueAxis8_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series8_1);
                }
                if (checkBox1_8.Checked == false)
                {
                    _valueAxis8_1.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series8_1);
                }
                plotView5.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        private void check_event1_9(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox1_9.Checked == true)
                {
                    _valueAxis9_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series9_1);
                }
                if (checkBox1_9.Checked == false)
                {
                    _valueAxis9_1.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series9_1);
                }
                plotView5.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        private void check_event1_10(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox1_10.Checked == true)
                {
                    _valueAxis10_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series10_1);
                }
                if (checkBox1_10.Checked == false)
                {
                    _valueAxis10_1.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series10_1);
                }
                plotView5.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        #endregion 曲线勾选框点击事件

        /// <summary>
        /// 定时器启用
        /// </summary>
        public void Timer_state()
        {
            _Timer1.Enabled = true;
        }

        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {
            _Timer1.Enabled = false;
        }

        /// <summary>
        /// 控件关闭
        /// </summary>
        public void _Clear()
        {
            _Timer1.Close();
            this.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}