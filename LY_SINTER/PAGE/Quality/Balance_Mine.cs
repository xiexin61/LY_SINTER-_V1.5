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
using DataBase;
using LY_SINTER.Custom;
using LY_SINTER.Model;
using OxyPlot.Axes;
using OxyPlot;
using LY_SINTER.Popover.Quality;

namespace LY_SINTER.PAGE.Quality
{
    public partial class Balance_Mine : UserControl
    {
        public System.Timers.Timer _Timer1 { get; set; }

        #region 返矿平衡变化状况曲线

        private PlotModel _myPlotModel;//容器
        private DateTimeAxis _dateAxis;//X轴
        private LinearAxis _valueAxis1;//Y轴
        private LinearAxis _valueAxis2;//Y轴
        private List<DataPoint> Line1 = new List<DataPoint>();//数据组
        private List<DataPoint> Line2 = new List<DataPoint>();//数据组
        private OxyPlot.Series.LineSeries series1;//曲线
        private OxyPlot.Series.LineSeries series2;//曲线
        private int max1 = 0, min1 = 0, max2 = 0, min2 = 0;

        #endregion 返矿平衡变化状况曲线

        #region 趋势历史曲线

        private string[] curve_name = { "A_1", "A_2", "A_3", "A_4", "A_5", "A_6" };
        private PlotModel _myPlotModel_His;//容器
        private DateTimeAxis _dateAxis_His;//X轴
        private LinearAxis _valueAxis1_His;//Y轴
        private LinearAxis _valueAxis2_His;//Y轴
        private LinearAxis _valueAxis3_His;//Y轴
        private LinearAxis _valueAxis4_His;//Y轴
        private List<DataPoint> Line1_His = new List<DataPoint>();//数据组
        private List<DataPoint> Line2_His = new List<DataPoint>();//数据组
        private List<DataPoint> Line3_His = new List<DataPoint>();//数据组
        private List<DataPoint> Line4_His = new List<DataPoint>();//数据组
        private OxyPlot.Series.LineSeries series1_His;//曲线
        private OxyPlot.Series.LineSeries series2_His;//曲线
        private OxyPlot.Series.LineSeries series3_His;//曲线
        private OxyPlot.Series.LineSeries series4_His;//曲线

        #endregion 趋势历史曲线

        private Quality_Model _quality = new Quality_Model();
        public vLog vLog { get; set; }
        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        public Balance_Mine()
        {
            InitializeComponent();
            if (vLog == null)
                vLog = new vLog(".\\log_Page\\Quality\\Balance_Mine\\");
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            fines_change();//返矿变化状况
            _Timer1 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { Timer1_Tick_1(); };//响应事件
            _Timer1.Enabled = true;
            _Timer1.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            Check_text();//勾选框数据
            parameter_inquire();//返矿平衡模型状态参数
            storehouse_min();//仓数据
            PLC_XL_3S();//3S数据
            tendency_curve_HIS(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));//历史趋势曲线
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
                fines_change();//返矿变化状况
                Check_text();//勾选框数据
                parameter_inquire();//返矿平衡模型状态参数
                storehouse_min();//仓数据
                PLC_XL_3S();//3S数据
            }
        }

        /// <summary>
        /// 趋势曲线历史查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            tendency_curve_HIS(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));
        }

        /// <summary>
        /// 趋势曲线历史
        /// </summary>
        public void tendency_curve_HIS(DateTime _d1, DateTime _d2)
        {
            try
            {
                string sql_curve_Ls = "SELECT TIMESTAMP, PAR_W_AIM,PAR_C_AIM ,MAT_L2_DQ_DRY_7 ,MAT_L2_DQ_DRY_6 FROM MC_SRMCAL_PAR_CURVE_HIS WHERE  TIMESTAMP between '" + _d1 + "' and '" + _d2 + "' order by TIMESTAMP asc";

                DataTable data_curve_ls = dBSQL.GetCommand(sql_curve_Ls);
                if (data_curve_ls.Rows.Count > 0)
                {
                    Line1_His.Clear();
                    Line2_His.Clear();
                    Line3_His.Clear();
                    Line4_His.Clear();
                    List<double> Mun1 = new List<double>();
                    List<double> Mun2 = new List<double>();
                    List<double> Mun3 = new List<double>();
                    List<double> Mun4 = new List<double>();

                    //定义model
                    _myPlotModel_His = new PlotModel()
                    {
                        Background = OxyColors.White,
                        Title = "历史",
                        TitleFontSize = 7,
                        TitleColor = OxyColors.White,
                        //LegendMargin = 100,
                    };
                    //X轴
                    _dateAxis_His = new DateTimeAxis()
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
                    _myPlotModel_His.Axes.Add(_dateAxis_His);//添加x轴
                    for (int i = 0; i < data_curve_ls.Rows.Count; i++)//数据整理
                    {
                        //******目标仓位******
                        DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["PAR_W_AIM"]));
                        Line1_His.Add(line1);
                        Mun1.Add(Convert.ToDouble(data_curve_ls.Rows[i]["PAR_W_AIM"]));
                        //*****综合仓位*****
                        DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["PAR_C_AIM"]));
                        Line2_His.Add(line2);
                        Mun2.Add(Convert.ToDouble(data_curve_ls.Rows[i]["PAR_C_AIM"]));
                        //*****烧返配比*****
                        DataPoint line3 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["MAT_L2_DQ_DRY_7"]));
                        Line3_His.Add(line3);
                        Mun3.Add(Convert.ToDouble(data_curve_ls.Rows[i]["MAT_L2_DQ_DRY_7"]));
                        //*****燃料配比*****
                        DataPoint line4 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["MAT_L2_DQ_DRY_6"]));
                        Line4_His.Add(line4);
                        Mun4.Add(Convert.ToDouble(data_curve_ls.Rows[i]["MAT_L2_DQ_DRY_6"]));
                    }
                    int x = 1;
                    if ((int)((Mun1.Max() - Mun1.Min()) / 5) > 0)
                    {
                        x = (int)((Mun1.Max() - Mun1.Min()) / 5);
                    }
                    _valueAxis1_His = new LinearAxis()
                    {
                        Key = curve_name[0],
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        IsZoomEnabled = false,
                        IsPanEnabled = false,
                        PositionTier = 1,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Red,
                        MinorTicklineColor = OxyColors.Red,
                        TicklineColor = OxyColors.Red,
                        TextColor = OxyColors.Red,
                        FontSize = 9.0,
                        IsAxisVisible = true,
                        MinorTickSize = 0,
                        Maximum = (int)(Mun1.Max() + 1),
                        Minimum = (int)(Mun1.Min() - 1),
                        MajorStep = x,
                    };
                    _myPlotModel_His.Axes.Add(_valueAxis1_His);
                    //添加曲线
                    series1_His = new OxyPlot.Series.LineSeries()
                    {
                        Color = OxyColors.Red,
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Red,
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[0],
                        ItemsSource = Line1_His,
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss} 目标仓位:{4}",
                    };
                    if (check_mbcw.Checked)
                    {
                        _valueAxis1_His.IsAxisVisible = true;
                        _myPlotModel_His.Series.Add(series1_His);
                    }
                    int x_1 = 1;//判断增长数据
                    if ((int)((Mun2.Max() - Mun2.Min()) / 5) > 0)
                    {
                        x_1 = (int)((Mun2.Max() - Mun2.Min()) / 5);
                    }
                    _valueAxis2_His = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[1],
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        IsZoomEnabled = false,
                        IsPanEnabled = false,
                        PositionTier = 2,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Purple,
                        MinorTicklineColor = OxyColors.Purple,
                        TicklineColor = OxyColors.Purple,
                        TextColor = OxyColors.Purple,
                        FontSize = 9.0,
                        IsAxisVisible = true,
                        MinorTickSize = 0,
                        Maximum = (int)(Mun2.Max() + 1),
                        Minimum = (int)(Mun2.Min() - 1),
                        MajorStep = x_1,
                    };
                    _myPlotModel_His.Axes.Add(_valueAxis2_His);//添加曲线

                    series2_His = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Purple,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Purple,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[1],//识别符
                        ItemsSource = Line2_His,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss} 综合仓位:{4}",
                    };
                    if (check_zhcw.Checked)
                    {
                        _valueAxis2_His.IsAxisVisible = true;
                        _myPlotModel_His.Series.Add(series2_His);
                    }

                    int x_2 = 1;//判断增长数据
                    if ((int)((Mun3.Max() - Mun3.Min()) / 5) > 0)
                    {
                        x_2 = (int)((Mun3.Max() - Mun3.Min()) / 5);
                    }
                    _valueAxis3_His = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[2],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        IsZoomEnabled = false,
                        IsPanEnabled = false,
                        PositionTier = 3,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Green,
                        MinorTicklineColor = OxyColors.Green,
                        TicklineColor = OxyColors.Green,
                        TextColor = OxyColors.Green,
                        FontSize = 9.0,
                        IsAxisVisible = true,
                        MinorTickSize = 0,
                        Maximum = (int)(Mun3.Max() + 1),
                        Minimum = (int)(Mun3.Min() - 1),
                        MajorStep = x_2,
                    };
                    _myPlotModel_His.Axes.Add(_valueAxis3_His);//添加曲线

                    series3_His = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Green,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Green,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[2],//识别符
                        ItemsSource = Line3_His,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss} 返矿配比:{4}",
                    };
                    if (check_sfpb.Checked)
                    {
                        _valueAxis3_His.IsAxisVisible = true;
                        _myPlotModel_His.Series.Add(series3_His);
                    }

                    int x_3 = 1;//判断增长数据
                    if ((int)((Mun4.Max() - Mun4.Min()) / 5) > 0)
                    {
                        x_3 = (int)((Mun4.Max() - Mun4.Min()) / 5);
                    }
                    _valueAxis4_His = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[3],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        IsZoomEnabled = false,
                        IsPanEnabled = false,
                        PositionTier = 4,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Blue,
                        MinorTicklineColor = OxyColors.Blue,
                        TicklineColor = OxyColors.Blue,
                        TextColor = OxyColors.Blue,
                        FontSize = 9.0,
                        IsAxisVisible = true,
                        MinorTickSize = 0,
                        Maximum = (int)(Mun4.Max() + 1),
                        Minimum = (int)(Mun4.Min() - 1),
                        MajorStep = x_3,
                    };
                    _myPlotModel_His.Axes.Add(_valueAxis4_His);//添加曲线

                    series4_His = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Blue,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Blue,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[3],//识别符
                        ItemsSource = Line4_His,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n燃料配比:{4}",
                    };
                    if (check_rlpb.Checked)
                    {
                        _valueAxis4_His.IsAxisVisible = true;
                        _myPlotModel_His.Series.Add(series4_His);
                    }
                    curve_his.Model = _myPlotModel_His;
                    var PlotController = new OxyPlot.PlotController();
                    PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
                    curve_his.Controller = PlotController;
                }
            }
            catch (Exception ee)
            {
                string mistake = "tendency_curve_HIS方法错误" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        #region 历史趋势曲线勾选框响应事件

        /// <summary>
        /// 目标仓位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check_mbcw_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (check_mbcw.Checked == true)
                {
                    _valueAxis1_His.IsAxisVisible = true;
                    _myPlotModel_His.Series.Add(series1_His);
                }
                if (check_mbcw.Checked == false)
                {
                    _valueAxis1_His.IsAxisVisible = false;
                    _myPlotModel_His.Series.Remove(series1_His);
                }
                curve_his.Model = _myPlotModel_His;
            }
            catch
            { }
        }

        /// <summary>
        /// 综合仓位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check_zhcw_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (check_zhcw.Checked == true)
                {
                    _valueAxis2_His.IsAxisVisible = true;
                    _myPlotModel_His.Series.Add(series2_His);
                }
                if (check_zhcw.Checked == false)
                {
                    _valueAxis2_His.IsAxisVisible = false;
                    _myPlotModel_His.Series.Remove(series2_His);
                }
                curve_his.Model = _myPlotModel_His;
            }
            catch
            { }
        }

        /// <summary>
        /// 返矿配比
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check_sfpb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (check_sfpb.Checked == true)
                {
                    _valueAxis3_His.IsAxisVisible = true;
                    _myPlotModel_His.Series.Add(series3_His);
                }
                if (check_sfpb.Checked == false)
                {
                    _valueAxis3_His.IsAxisVisible = false;
                    _myPlotModel_His.Series.Remove(series3_His);
                }
                curve_his.Model = _myPlotModel_His;
            }
            catch
            { }
        }

        /// <summary>
        /// 燃料配比
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check_rlpb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (check_rlpb.Checked == true)
                {
                    _valueAxis4_His.IsAxisVisible = true;
                    _myPlotModel_His.Series.Add(series4_His);
                }
                if (check_rlpb.Checked == false)
                {
                    _valueAxis4_His.IsAxisVisible = false;
                    _myPlotModel_His.Series.Remove(series4_His);
                }
                curve_his.Model = _myPlotModel_His;
            }
            catch
            { }
        }

        #endregion 历史趋势曲线勾选框响应事件

        /// <summary>
        /// 开始&结束时间赋值
        /// </summary>
        public void dateTimePicker_value()
        {
            //结束时间
            DateTime time_end = DateTime.Now;
            //开始时间
            DateTime time_begin = time_end.AddDays(-7);

            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }

        #region 实时曲线勾选框响应事件

        /// <summary>
        /// 仓位变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check_cwbh_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView1.Model = null;
                if (check_cwbh.Checked == true)
                {
                    _valueAxis2.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series2);
                }
                if (check_cwbh.Checked == false)
                {
                    _valueAxis2.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series2);
                }
                plotView1.Model = _myPlotModel;
            }
            catch
            { }
        }

        /// <summary>
        /// 总出料量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void check_zcll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView1.Model = null;
                if (check_zcll.Checked == true)
                {
                    _valueAxis1.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series1);
                }
                if (check_zcll.Checked == false)
                {
                    _valueAxis1.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series1);
                }
                plotView1.Model = _myPlotModel;
            }
            catch
            { }
        }

        #endregion 实时曲线勾选框响应事件

        /// <summary>
        /// 返矿变化状况 定时器 1h
        /// </summary>
        public void fines_change()
        {
            try
            {
                Tuple<bool, List<double>, List<double>, List<string>> _value = _quality._Get_Mine_Change();
                if (_value.Item1)
                {
                    //总出料量 1h
                    textBox30.Text = _value.Item2[0].ToString();
                    //总出料量 2h
                    textBox29.Text = _value.Item2[1].ToString();
                    //总出料量 3h
                    textBox28.Text = _value.Item2[2].ToString();
                    //总出料量 4h
                    textBox27.Text = _value.Item2[3].ToString();
                    //总出料量 5h
                    textBox26.Text = _value.Item2[4].ToString();
                    //总出料量 6h
                    textBox25.Text = _value.Item2[5].ToString();
                    //总出料量 7h
                    textBox24.Text = _value.Item2[6].ToString();
                    //总出料量 8h
                    textBox23.Text = _value.Item2[7].ToString();
                    //总出料量 9h
                    textBox22.Text = _value.Item2[8].ToString();
                    //总出料量 10h
                    textBox21.Text = _value.Item2[9].ToString();
                    //总出料量 11h
                    textBox20.Text = _value.Item2[10].ToString();
                    //总出料量 12h
                    textBox19.Text = _value.Item2[11].ToString();

                    //仓位变化 1h
                    textBox42.Text = _value.Item3[0].ToString();
                    //仓位变化 2h
                    textBox41.Text = _value.Item3[1].ToString();
                    //仓位变化 3h
                    textBox40.Text = _value.Item3[2].ToString();
                    //仓位变化 4h
                    textBox39.Text = _value.Item3[3].ToString();
                    //仓位变化 5h
                    textBox38.Text = _value.Item3[4].ToString();
                    //仓位变化 6h
                    textBox37.Text = _value.Item3[5].ToString();
                    //仓位变化 7h
                    textBox36.Text = _value.Item3[6].ToString();
                    //仓位变化 8h
                    textBox35.Text = _value.Item3[7].ToString();
                    //仓位变化 9h
                    textBox34.Text = _value.Item3[8].ToString();
                    //仓位变化 10h
                    textBox33.Text = _value.Item3[9].ToString();
                    //仓位变化 11h
                    textBox32.Text = _value.Item3[10].ToString();
                    //仓位变化 12h
                    textBox31.Text = _value.Item3[11].ToString();
                    //
                    List<DateTime> time_name = new List<DateTime>();
                    for (int x = _value.Item4.Count; x > 0; x--)
                    {
                        time_name.Add(DateTime.Parse(_value.Item4[x - 1]));
                    }

                    label27.Text = time_name[0].ToString("HH:mm");
                    label28.Text = time_name[1].ToString("HH:mm");
                    label29.Text = time_name[2].ToString("HH:mm");
                    label30.Text = time_name[3].ToString("HH:mm");
                    label31.Text = time_name[4].ToString("HH:mm");
                    label32.Text = time_name[5].ToString("HH:mm");
                    label33.Text = time_name[6].ToString("HH:mm");
                    label34.Text = time_name[7].ToString("HH:mm");
                    label35.Text = time_name[8].ToString("HH:mm");
                    label36.Text = time_name[9].ToString("HH:mm");
                    label37.Text = time_name[10].ToString("HH:mm");
                    label38.Text = time_name[11].ToString("HH:mm");
                    DataPoint _line1 = new DataPoint();
                    DataPoint _line2 = new DataPoint();
                    Line1.Clear();
                    Line2.Clear();
                    for (int i = 11; i >= 0; i--)
                    {
                        _line1 = new DataPoint(DateTimeAxis.ToDouble(time_name[11 - i]), Convert.ToDouble(_value.Item2[i]));//总出料量
                        _line2 = new DataPoint(DateTimeAxis.ToDouble(time_name[11 - i]), Convert.ToDouble(_value.Item3[i]));//仓位变化
                        Line1.Add(_line1);
                        Line2.Add(_line2);
                    }
                    max1 = (int)_value.Item2.Max();
                    min1 = (int)_value.Item2.Min();
                    max2 = (int)_value.Item3.Max();
                    min2 = (int)_value.Item3.Min();
                    _myPlotModel = new PlotModel()//定义容器
                    {
                        Background = OxyColors.White,
                        TitleFontSize = 7,
                        TitleColor = OxyColors.White,
                    };
                    _dateAxis = new DateTimeAxis()//x轴
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
                    _myPlotModel.Axes.Add(_dateAxis);//添加x轴
                    int m1 = 1;
                    if ((int)((max1 - min1) / 5) > 0)
                    {
                        m1 = (int)((max1 - min1) / 5);
                    }
                    _valueAxis1 = new LinearAxis()//添加y轴
                    {
                        //  Key = "A",
                        //  MajorGridlineStyle = LineStyle.None,
                        //  MinorGridlineStyle = LineStyle.None,
                        //  IntervalLength = 80,
                        ////  Angle = 60,
                        //  IsZoomEnabled = false,
                        //  IsPanEnabled = false,
                        //  Maximum = max1 + 2,
                        //  Minimum = min1 - 1,
                        //  PositionTier = 1,
                        //  AxislineStyle = LineStyle.Solid,
                        //  AxislineColor = OxyColors.Red,
                        //  MinorTicklineColor = OxyColors.Red,
                        //  TicklineColor = OxyColors.Red,
                        //  TextColor = OxyColors.Red,
                        //  FontSize = 9.0,
                        //  IsAxisVisible = false,
                        //  MajorStep = m1,
                        //  //  MajorStep = 50,
                        //  MinorTickSize = 0,

                        Key = "A",
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        IsZoomEnabled = false,
                        IsPanEnabled = false,
                        PositionTier = 1,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Red,
                        MinorTicklineColor = OxyColors.Red,
                        TicklineColor = OxyColors.Red,
                        TextColor = OxyColors.Red,
                        FontSize = 9.0,
                        IsAxisVisible = true,
                        MinorTickSize = 0,
                        Maximum = max1 + 2,
                        Minimum = min1 - 1,
                        MajorStep = m1,
                    };
                    _myPlotModel.Axes.Add(_valueAxis1);
                    //添加曲线
                    series1 = new OxyPlot.Series.LineSeries()
                    {
                        Color = OxyColors.Red,
                        StrokeThickness = 1,
                        MarkerSize = 2,
                        MarkerStroke = OxyColors.Red,
                        MarkerType = MarkerType.Circle,
                        YAxisKey = "A",
                        ItemsSource = Line1,
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}  总出料量:{4}t",
                    };
                    if (check_zcll.Checked == true)
                    {
                        _valueAxis1.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series1);
                    }

                    int m2 = 1;
                    if ((int)((max2 - min2) / 5) > 0)
                    {
                        m2 = (int)((max2 - min2) / 5);
                    }
                    _valueAxis2 = new LinearAxis()
                    {
                        Key = "B",
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        IsZoomEnabled = false,
                        IsPanEnabled = false,
                        PositionTier = 2,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Purple,
                        MinorTicklineColor = OxyColors.Purple,
                        TicklineColor = OxyColors.Purple,
                        TextColor = OxyColors.Purple,
                        FontSize = 9.0,
                        IsAxisVisible = true,
                        MinorTickSize = 0,
                        Maximum = max2 + 2,
                        Minimum = min2 - 1,
                        MajorStep = m2,
                    };
                    _myPlotModel.Axes.Add(_valueAxis2);
                    series2 = new OxyPlot.Series.LineSeries()
                    {
                        Color = OxyColors.Purple,
                        StrokeThickness = 1,
                        MarkerSize = 2,
                        MarkerStroke = OxyColors.BlueViolet,
                        MarkerType = MarkerType.Circle,
                        YAxisKey = "B",
                        ItemsSource = Line2,
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss} 仓位变化:{4}t",
                    };
                    if (check_cwbh.Checked == true)
                    {
                        _valueAxis2.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series2);
                    }
                    plotView1.Model = _myPlotModel;
                    var PlotController = new OxyPlot.PlotController();
                    PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
                    plotView1.Controller = PlotController;
                }
                else
                {
                    vLog.writelog("Quality_Model模型方法_Get_Mine_Change计算错误", -1);
                    return;
                }
            }
            catch (Exception ee)
            {
                string mistake = "返矿变化状况报错" + ee.ToString();
                vLog.writelog(mistake, -1);
                return;
            }
        }

        /// <summary>
        /// 定时刷新勾选框数据
        /// </summary>
        private void Check_text()
        {
            try
            {
                var SQL = "SELECT TOP (1) PAR_W_AIM,PAR_C_AIM ,MAT_L2_DQ_DRY_7 ,MAT_L2_DQ_DRY_6 FROM MC_SRMCAL_PAR_CURVE_HIS ORDER BY TIMESTAMP DESC";
                DataTable dataTable_1 = dBSQL.GetCommand(SQL);
                if (dataTable_1.Rows.Count > 0 && dataTable_1 != null)
                {
                    check_mbcw.Text = "目标仓位 t:" + dataTable_1.Rows[0]["PAR_W_AIM"].ToString();
                    check_zhcw.Text = "综合仓位 t:" + dataTable_1.Rows[0]["PAR_C_AIM"].ToString();
                    check_sfpb.Text = "返矿配比:" + dataTable_1.Rows[0]["MAT_L2_DQ_DRY_7"].ToString();
                    check_rlpb.Text = "燃料配比:" + dataTable_1.Rows[0]["MAT_L2_DQ_DRY_6"].ToString();
                }
                else
                {
                    check_mbcw.Text = "目标仓位 t: 0";
                    check_zhcw.Text = "综合仓位 t: 0";
                    check_sfpb.Text = "返矿配比: 0";
                    check_rlpb.Text = "燃料配比: 0";
                }
            }
            catch (Exception ee)
            {
                string mistake = "定时刷新勾选框数据报错" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        ///返矿平衡模型状态参数
        /// </summary>
        private void parameter_inquire()
        {
            try
            {
                #region 参数定义

                //投用状态
                int signal_FK = 0;
                //调用条件
                int signal_condition = 0;
                //调整状态
                int signal_adjustment = 0;
                //综合仓位
                double synthesize_bin = 0;
                //目标仓位
                double objective_bin = 0;
                //仓位偏差
                double deviation_bin = 0;
                //偏差变化
                double deviation_change = 0;
                //配比调整量
                double adjustment = 0;
                //调整前配比
                double adjustment_old = 0;
                //调整后配比
                double adjustment_new = 0;
                //15#下料比例
                double proportion_15 = 0;
                //16#下料比例
                double proportion_16 = 0;
                //调整周期
                double adjustment_period = 0;
                //下料比例周期
                double ration_period = 0;
                //超限延时周期
                double delayed_period = 0;
                //仓位死区
                double bin_dead = 0;
                //仓位上限
                double bin_up = 0;
                //仓位下限
                double bin_low = 0;

                #endregion 参数定义

                //CFG_MAT_L2_Butsig_INTERFACE表按钮状态
                string sql_1 = " select top (1) " +
                    "isnull(MAT_L2_but_fk,888) as MAT_L2_but_fk " +
                    "from CFG_MAT_L2_Butsig_INTERFACE  order by TIMESTAMP desc";
                //MC_SRMCAL_RESULT表使用参数
                string sql_2 = "select top (1) " +
                    "isnull(SRMCAL_FLAG,-1) as SRMCAL_FLAG," +
                    "isnull(SRMCAL_A_FLAG,-1) as SRMCAL_A_FLAG," +
                    "SRMCAL_W," +
                    "SRMCAL_W_AIM," +
                    "SRMCAL_E," +
                    "SRMCAL_EC," +
                    "SRMCAL_BILL_SP_A," +
                    "SRMCAL_BILL_SP_OLD," +
                    "SRMCAL_BILL_SP_NEW " +
                    "from MC_SRMCAL_RESULT order by TIMESTAMP DESC";
                //MC_SRMCAL_RESULT_DIST_T 表使用数据
                string sql_3 = "select isnull(SRMCAL_S_15,0) as SRMCAL_S_15,isnull(SRMCAL_S_16,0) as SRMCAL_S_16 from MC_SRMCAL_RESULT_DIST_T order by TIMESTAMP desc";
                //MC_SRMCAL_PAR表使用数据
                string sql_4 = "select top 1 PAR_BILL_T,PAR_DIST_T,PAR_W_T,PAR_W_E_LEVEL_1,PAR_W_UP,PAR_W_LOW  from MC_SRMCAL_PAR order by TIMESTAMP desc";
                DataTable dataTable_1 = dBSQL.GetCommand(sql_1);
                DataTable dataTable_2 = dBSQL.GetCommand(sql_2);
                DataTable dataTable_3 = dBSQL.GetCommand(sql_3);
                DataTable dataTable_4 = dBSQL.GetCommand(sql_4);

                #region MC_SRMCAL_RESULT表赋值

                if (dataTable_2.Rows.Count > 0)
                {
                    signal_condition = int.Parse(dataTable_2.Rows[0]["SRMCAL_FLAG"].ToString());
                    signal_adjustment = int.Parse(dataTable_2.Rows[0]["SRMCAL_A_FLAG"].ToString());
                    synthesize_bin = double.Parse(dataTable_2.Rows[0]["SRMCAL_W"].ToString());
                    objective_bin = double.Parse(dataTable_2.Rows[0]["SRMCAL_W_AIM"].ToString());
                    deviation_bin = double.Parse(dataTable_2.Rows[0]["SRMCAL_E"].ToString());
                    deviation_change = double.Parse(dataTable_2.Rows[0]["SRMCAL_EC"].ToString());
                    adjustment = double.Parse(dataTable_2.Rows[0]["SRMCAL_BILL_SP_A"].ToString());
                    adjustment_old = double.Parse(dataTable_2.Rows[0]["SRMCAL_BILL_SP_OLD"].ToString());
                    adjustment_new = double.Parse(dataTable_2.Rows[0]["SRMCAL_BILL_SP_NEW"].ToString());
                }

                #endregion MC_SRMCAL_RESULT表赋值

                #region MC_SRMCAL_RESULT_DIST_T表赋值

                if (dataTable_3.Rows.Count > 0)
                {
                    proportion_15 = double.Parse(dataTable_3.Rows[0]["SRMCAL_S_15"].ToString());
                    proportion_16 = double.Parse(dataTable_3.Rows[0]["SRMCAL_S_16"].ToString());
                }

                #endregion MC_SRMCAL_RESULT_DIST_T表赋值

                #region MC_SRMCAL_PAR表赋值

                if (dataTable_4.Rows.Count > 0)
                {
                    adjustment_period = double.Parse(dataTable_4.Rows[0]["PAR_BILL_T"].ToString());
                    ration_period = double.Parse(dataTable_4.Rows[0]["PAR_DIST_T"].ToString());
                    delayed_period = double.Parse(dataTable_4.Rows[0]["PAR_W_T"].ToString());
                    bin_dead = double.Parse(dataTable_4.Rows[0]["PAR_W_E_LEVEL_1"].ToString());
                    bin_up = double.Parse(dataTable_4.Rows[0]["PAR_W_UP"].ToString());
                    bin_low = double.Parse(dataTable_4.Rows[0]["PAR_W_LOW"].ToString());
                }

                #endregion MC_SRMCAL_PAR表赋值

                // 返矿投用状态信号
                if (dataTable_1.Rows.Count > 0)
                {
                    signal_FK = int.Parse(dataTable_1.Rows[0]["MAT_L2_but_fk"].ToString());
                    if (signal_FK == 1)
                    {
                        label49.Text = "模型投用";
                    }
                    else
                    if (signal_FK == 0)
                    {
                        label49.Text = "模型退出";
                    }
                    else
                    {
                        label49.Text = " ";
                        string error_message = "返矿投用状态信号异常";
                    }
                }

                //调用条件(1：人工干预烧返配比调用；2：烧返仓数量变化调用；3：目标仓位变化调用；4：仓位超限调用；5：周期调用)
                if (signal_condition == 1)
                {
                    label50.Text = "人工干预烧返配比调用";
                }
                else if (signal_condition == 2)
                {
                    label50.Text = " 烧返仓数量变化调用";
                }
                else if (signal_condition == 3)
                {
                    label50.Text = " 目标仓位变化调用";
                }
                else if (signal_condition == 4)
                {
                    label50.Text = " 仓位超限调用";
                }
                else if (signal_condition == 5)
                {
                    label50.Text = " 周期调用";
                }
                else
                {
                    label50.Text = " ";
                    string error_message = "调用条件信号异常";
                }
                //调整状态 1：模型计算完成；2：模型调整完成（界面点击确认按钮）；3：禁止模型调整（界面点击取消按钮）
                if (signal_adjustment == 1)
                {
                    label51.Text = "模型计算完成";
                }
                else if (signal_adjustment == 2)
                {
                    label51.Text = "模型调整完成（界面点击确认按钮）";
                }
                else if (signal_adjustment == 3)
                {
                    label51.Text = "禁止模型调整（界面点击取消按钮）";
                }
                else if (signal_adjustment == 4)
                {
                    label51.Text = "人工未点击，自动调整";
                    string error_message = "调整状态信号异常";
                }
                else
                {
                    label51.Text = "异常";
                    //  string error_message = "调整状态信号异常";
                }
                //综合仓位 t
                textBox1.Text = synthesize_bin.ToString() + "t";
                //目标仓位 t
                textBox2.Text = objective_bin.ToString() + "t";
                //仓位偏差 t
                textBox3.Text = deviation_bin.ToString() + "t";
                //偏差变化 t
                textBox4.Text = deviation_change.ToString() + "t";
                //配比调整量
                textBox5.Text = Math.Round(adjustment, 2).ToString();
                //调整前配比
                textBox6.Text = Math.Round(adjustment_old, 2).ToString();
                //调整后配比
                textBox7.Text = Math.Round(adjustment_new, 2).ToString();
                //15#下料比例
                textBox8.Text = Math.Round(proportion_15, 2).ToString();
                //16#下料比例
                textBox9.Text = Math.Round(proportion_16, 2).ToString();
                //调整周期 min
                textBox10.Text = Math.Round(adjustment_period, 2).ToString() + "min";
                //下料比例周期 min
                textBox11.Text = Math.Round(ration_period, 2).ToString() + "min";
                //超时延时周期 min
                textBox12.Text = Math.Round(delayed_period, 2).ToString() + "min";
                //仓位死区 t
                textBox13.Text = Math.Round(bin_dead, 2).ToString() + "t";
                //仓位上限 t
                textBox14.Text = Math.Round(bin_up, 2).ToString() + "t";
                //仓位下限 t
                textBox15.Text = Math.Round(bin_low, 2).ToString() + "t";
            }
            catch (Exception ee)
            {
                string mistake = "返矿平衡模型状态参数报错" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 参数弹出框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Frm_Balance_Mine_Par form_display = new Frm_Balance_Mine_Par();
            if (Frm_Balance_Mine_Par.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 调整数据弹出框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Frm_Balance_Mine_Adjust form_display = new Frm_Balance_Mine_Adjust();
            if (Frm_Balance_Mine_Adjust.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 页面仓刷新1min数据
        /// </summary>
        public void storehouse_min()
        {
            try
            {
                //15#烧返仓仓位
                float storehouse_15 = 0;
                //16#烧返仓仓位
                float storehouse_16 = 0;
                //15#烧返仓下料量
                float laying_15 = 0;
                //16#烧返仓下料量
                float laying_16 = 0;
                //15#烧返仓启停状态
                int signal_15 = 0;
                //16#烧返仓启停状态
                int signal_16 = 0;
                string sql = "select top 1 " +
                    "ISNULL(MAT_PLC_W_15,0) AS MAT_PLC_W_15," +
                    "ISNULL(MAT_PLC_W_16,0) AS MAT_PLC_W_16," +
                    "ISNULL(MAT_PLC_PV_W_15,0) AS MAT_PLC_PV_W_15," +
                    "ISNULL(MAT_PLC_PV_W_16,0) AS MAT_PLC_PV_W_16," +
                    "ISNULL(MAT_PLC_SS_SIGNAL_15,0) AS MAT_PLC_SS_SIGNAL_15," +
                    "ISNULL(MAT_PLC_SS_SIGNAL_16,0) AS MAT_PLC_SS_SIGNAL_16  from C_MAT_PLC_1MIN order by TIMESTAMP desc";
                DataTable dataTable = dBSQL.GetCommand(sql);

                if (dataTable.Rows.Count > 0)
                {
                    storehouse_15 = float.Parse(dataTable.Rows[0]["MAT_PLC_W_15"].ToString());
                    storehouse_16 = float.Parse(dataTable.Rows[0]["MAT_PLC_W_16"].ToString());
                    laying_15 = float.Parse(dataTable.Rows[0]["MAT_PLC_PV_W_15"].ToString());
                    laying_16 = float.Parse(dataTable.Rows[0]["MAT_PLC_PV_W_16"].ToString());
                    signal_15 = int.Parse(dataTable.Rows[0]["MAT_PLC_SS_SIGNAL_15"].ToString());
                    signal_16 = int.Parse(dataTable.Rows[0]["MAT_PLC_SS_SIGNAL_16"].ToString());
                    ////15#烧返仓仓位
                    label41.Text = Math.Round(storehouse_15, 2).ToString() + " t";
                    //16#烧返仓仓位
                    label42.Text = Math.Round(storehouse_16, 2).ToString() + " t";
                    //15#烧返仓下料量
                    label45.Text = "下料量SP:" + Math.Round(laying_15, 2) + "t/h";
                    //16#烧返仓下料量
                    label46.Text = "下料量SP:" + Math.Round(laying_16, 2) + "t/h";

                    #region 仓下料启停状态

                    if (signal_15 == 0)
                    {
                        button1.BackColor = Color.Red;
                    }
                    else if (signal_15 == 1)
                    {
                        button1.BackColor = Color.Green;
                    }
                    else
                    {
                        string error_message = "15#仓启停状态异常";
                        vLog.writelog(error_message, -1);
                    }
                    if (signal_16 == 0)
                    {
                        button2.BackColor = Color.Red;
                    }
                    else if (signal_16 == 1)
                    {
                        button2.BackColor = Color.Green;
                    }
                    else
                    {
                        string error_message = "16#仓启停状态异常";
                        vLog.writelog(error_message, -1);
                    }

                    #endregion 仓下料启停状态

                    #region 进度条

                    //查询进度条最大值
                    string sql_MIX = "select top 1 PAR_INTERFACE_MAX_W  from MC_SRMCAL_PAR order by TIMESTAMP ";
                    DataTable dataTable_MIX = dBSQL.GetCommand(sql_MIX);
                    float storehouse_UP = float.Parse(dataTable_MIX.Rows[0]["PAR_INTERFACE_MAX_W"].ToString());
                    //15#

                    float storehouse_x1 = storehouse_15 / storehouse_UP * 100;
                    int x_15 = (int)storehouse_x1;
                    SaveProgress_1(x_15);
                    //16#

                    float storehouse_x2 = storehouse_16 / storehouse_UP * 100;
                    int x_16 = (int)storehouse_x2;
                    SaveProgress_2(x_16);

                    #endregion 进度条
                }
            }
            catch (Exception ee)
            {
                string mistake = "页面刷新1min数据错误" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 15#进度条
        /// </summary>
        /// <param name="proportion"></param>
        public void SaveProgress_1(int proportion)
        {
            try
            {
                //判断仓位是否超过限制
                if (proportion <= 100)
                {
                    verticalProgressBar1.Value = proportion;
                }
                else
                {
                    MessageBox.Show("15#烧返仓实际下料量超过参数表设置下料量");
                    string mistake = "15#烧返仓实际下料量超过参数表设置下料量";
                    vLog.writelog(mistake, -1);
                }
            }
            catch (Exception ee)
            {
                string mistake = "15#进度条报错" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 16#进度条
        /// </summary>
        /// <param name="proportion"></param>
        public void SaveProgress_2(int proportion)
        {
            try
            {
                if (proportion <= 100)
                {
                    verticalProgressBar2.Value = proportion;
                }
                else
                {
                    MessageBox.Show("16#烧返仓实际下料量超过参数表设置下料量");
                    string mistake = "16#烧返仓实际下料量超过参数表设置下料量";
                    vLog.writelog(mistake, -1);
                }
            }
            catch (Exception ee)
            {
                string mistake = "16#进度条报错" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 3S刷新15#16#实际下料量
        /// </summary>
        public void PLC_XL_3S()
        {
            try
            {
                //20200819 修改实际下料量
                //string sql_3s_xl = "  select T_ACTUAL_W_15_3S,T_ACTUAL_W_16_3S    from C_PLC_3S where TIMESTAMP = (select max(TIMESTAMP) from C_PLC_3S)  ";
                string sql_3s_xl = "  select T_ACTUAL_W_15_3S,T_ACTUAL_W_16_3S    from C_PLC_3S where TIMESTAMP = (select max(TIMESTAMP) from C_PLC_3S)  ";

                DataTable data = dBSQL.GetCommand(sql_3s_xl);
                if (data.Rows.Count > 0)
                {
                    double _15XL = double.Parse(data.Rows[0]["T_ACTUAL_W_15_3S"].ToString());
                    double _16XL = double.Parse(data.Rows[0]["T_ACTUAL_W_16_3S"].ToString());
                    //15#
                    label47.Text = "下料量PV：" + Math.Round(_15XL, 2) + "t/h";
                    //16#
                    label48.Text = "下料量PV：" + Math.Round(_16XL, 2) + "t/h";
                }
                else
                {
                    //15#
                    label47.Text = "下料量PV：0t/h";
                    //16#
                    label48.Text = "下料量PV：0t/h";
                }

                //分仓系数
                var sql_1 = "select MAT_L2_FCXS from CFG_MAT_L2_XLK_INTERFACE where  MAT_L2_CH = 15 or MAT_L2_CH = 16 order by MAT_L2_CH asc";
                DataTable _data = dBSQL.GetCommand(sql_1);
                if (_data.Rows.Count > 0)
                {
                    //15
                    label53.Text = "分仓系数:" + Math.Round(double.Parse(_data.Rows[0][0].ToString()), 3);
                    //16
                    label52.Text = "分仓系数:" + Math.Round(double.Parse(_data.Rows[1][0].ToString()), 3);
                }
            }
            catch (Exception ee)
            {
                string mistake = "更新实际下料量错误" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        public void Timer_stop()
        {
            _Timer1.Stop();
        }

        public void Timer_state()
        {
            _Timer1.Start();
        }

        public void _Clear()
        {
            _Timer1.Close();
            this.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    /// <summary>
    /// 自定义控件，设置进度条的垂直显示
    /// </summary>
    public class VerticalProgressBar : ProgressBar
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x04;
                return cp;
            }
        }
    }
}