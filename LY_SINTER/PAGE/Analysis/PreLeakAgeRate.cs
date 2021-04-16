using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LY_SINTER.Custom;
using DataBase;
using OxyPlot.WindowsForms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Threading;

namespace LY_SINTER.PAGE.Analysis
{
    public partial class PreLeakAgeRate : UserControl
    {
        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        public PreLeakAgeRate()
        {
            InitializeComponent();
            plotView1.Margin = plotView2.Margin = plotView3.Margin = plotView4.Margin = plotView5.Margin = plotView6.Margin = plotView7.Margin = plotView8.Margin = plotView9.Margin = new Padding(0, 0, 0, 0);

            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            time_begin_end();
            lData();
            /*DateTime start = DateTime.Now.AddMonths(-1);
            DateTime end = DateTime.Now;
            getPlotView(start, end);*/
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    lData();
                    DateTime start = DateTime.Now.AddDays(-7);
                    DateTime end = DateTime.Now;
                    getPlotView(start, end);
                    Thread.Sleep(60000);
                }
            });
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
        /// 获取左侧textBox的数据
        /// </summary>
        public void lData()
        {
            try
            {
                string sql = "select top(1) TIMESTAMP,CFP_PLC_O2_1,CFP_PLC_O2_2,CFP_PLC_O2_3,CFP_PLC_O2_4,CFP_PLC_O2_5,CFP_PLC_O2_6 from C_CFP_PLC_1MIN order by timestamp desc";
                DataTable table = dBSQL.GetCommand(sql);
                if (table.Rows.Count > 0)
                {
                    textBox1.Text = table.Rows[0][1].ToString();
                    textBox2.Text = table.Rows[0][2].ToString();
                    textBox3.Text = table.Rows[0][3].ToString();
                    textBox4.Text = table.Rows[0][4].ToString();
                    textBox5.Text = table.Rows[0][5].ToString();
                    textBox6.Text = table.Rows[0][6].ToString();
                    label6.Text = "最新调整时间:" + table.Rows[0][0].ToString();
                }
                string sql2 = "select top(1) SIN_PLC_MA_OUT_1_FLUE_O2,SIN_PLC_MA_OUT_2_FLUE_O2 from C_SIN_PLC_1MIN order by timestamp desc";
                DataTable table2 = dBSQL.GetCommand(sql2);
                if (table2.Rows.Count > 0)
                {
                    textBox7.Text = table2.Rows[0][0].ToString();
                    textBox8.Text = table2.Rows[0][1].ToString();
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 获取曲线数据
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        public void getPlotView(DateTime start, DateTime end)
        {
            //定义曲线
            LineSeries series1 = new LineSeries();
            LineSeries series2 = new LineSeries();
            LineSeries series3 = new LineSeries();
            LineSeries series4 = new LineSeries();
            LineSeries series5 = new LineSeries();
            LineSeries series6 = new LineSeries();
            LineSeries series7 = new LineSeries();
            LineSeries series8 = new LineSeries();

            //数据源
            List<DataPoint> Line1 = new List<DataPoint>();
            List<DataPoint> Line2 = new List<DataPoint>();
            List<DataPoint> Line3 = new List<DataPoint>();
            List<DataPoint> Line4 = new List<DataPoint>();
            List<DataPoint> Line5 = new List<DataPoint>();
            List<DataPoint> Line6 = new List<DataPoint>();
            List<DataPoint> Line7 = new List<DataPoint>();
            List<DataPoint> Line8 = new List<DataPoint>();

            Line1.Clear();
            Line2.Clear();
            Line3.Clear();
            Line4.Clear();
            Line5.Clear();
            Line6.Clear();
            Line7.Clear();
            Line8.Clear();
            string sql = "select TIMESTAMP,CFP_PLC_O2_1,CFP_PLC_O2_2,CFP_PLC_O2_3,CFP_PLC_O2_4,CFP_PLC_O2_5,CFP_PLC_O2_6 from C_CFP_PLC_1MIN where TIMESTAMP >= '" + start + "' and TIMESTAMP <= '" + end + "'";
            DataTable table = dBSQL.GetCommand(sql);
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), table.Rows[i]["CFP_PLC_O2_1"].ToString() == "" ? 0 : Convert.ToDouble(table.Rows[i]["CFP_PLC_O2_1"]));
                    Line1.Add(line1);
                    DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), table.Rows[i]["CFP_PLC_O2_2"].ToString() == "" ? 0 : Convert.ToDouble(table.Rows[i]["CFP_PLC_O2_2"]));
                    Line2.Add(line2);
                    DataPoint line3 = new DataPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), table.Rows[i]["CFP_PLC_O2_3"].ToString() == "" ? 0 : Convert.ToDouble(table.Rows[i]["CFP_PLC_O2_3"]));
                    Line3.Add(line3);
                    DataPoint line4 = new DataPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), table.Rows[i]["CFP_PLC_O2_4"].ToString() == "" ? 0 : Convert.ToDouble(table.Rows[i]["CFP_PLC_O2_4"]));
                    Line4.Add(line4);
                    DataPoint line5 = new DataPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), table.Rows[i]["CFP_PLC_O2_5"].ToString() == "" ? 0 : Convert.ToDouble(table.Rows[i]["CFP_PLC_O2_5"]));
                    Line5.Add(line5);
                    DataPoint line6 = new DataPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), table.Rows[i]["CFP_PLC_O2_6"].ToString() == "" ? 0 : Convert.ToDouble(table.Rows[i]["CFP_PLC_O2_6"]));
                    Line6.Add(line6);
                }
                var PlotModel1 = new PlotModel()
                {
                    PlotMargins = new OxyThickness(20, 10, 0, 0),
                    PlotAreaBorderThickness = new OxyThickness(0),
                };
                var PlotModel2 = new PlotModel()
                {
                    PlotMargins = new OxyThickness(20, 10, 0, 0),
                    PlotAreaBorderThickness = new OxyThickness(0),
                };
                var PlotModel3 = new PlotModel()
                {
                    PlotMargins = new OxyThickness(20, 10, 0, 0),
                    PlotAreaBorderThickness = new OxyThickness(0),
                };
                var PlotModel4 = new PlotModel()
                {
                    PlotMargins = new OxyThickness(20, 10, 0, 0),
                    PlotAreaBorderThickness = new OxyThickness(0),
                };
                var PlotModel5 = new PlotModel()
                {
                    PlotMargins = new OxyThickness(20, 10, 0, 0),
                    PlotAreaBorderThickness = new OxyThickness(0),
                };
                var PlotModel6 = new PlotModel()
                {
                    PlotMargins = new OxyThickness(20, 10, 0, 0),
                    PlotAreaBorderThickness = new OxyThickness(0),
                };

                var _dateAxis1 = new DateTimeAxis()
                {
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 100,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    AxisTickToLabelDistance = 0,
                    IsAxisVisible = false,
                };
                var _dateAxis2 = new DateTimeAxis()
                {
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 100,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    AxisTickToLabelDistance = 0,
                    IsAxisVisible = false,
                };
                var _dateAxis3 = new DateTimeAxis()
                {
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 100,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    AxisTickToLabelDistance = 0,
                    IsAxisVisible = false,
                };
                var _dateAxis4 = new DateTimeAxis()
                {
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 100,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    AxisTickToLabelDistance = 0,
                    IsAxisVisible = false,
                };
                var _dateAxis5 = new DateTimeAxis()
                {
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 100,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    AxisTickToLabelDistance = 0,
                    IsAxisVisible = false,
                };
                var _dateAxis6 = new DateTimeAxis()
                {
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 100,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    AxisTickToLabelDistance = 0,
                    IsAxisVisible = false,
                };
                var _valueAxis1_1 = new LinearAxis()
                {
                    Key = "3#风箱含氧量",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Black,
                    MinorTicklineColor = OxyColors.Black,
                    TicklineColor = OxyColors.Black,
                    TextColor = OxyColors.Black,
                    FontSize = 9.0,
                    IsAxisVisible = true,
                    MinorTickSize = 0,
                    Maximum = 23,
                    Minimum = -1,
                    MajorStep = 10,
                };
                var _valueAxis1_2 = new LinearAxis()
                {
                    Key = "6#风箱含氧量",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Red,
                    MinorTicklineColor = OxyColors.Red,
                    TicklineColor = OxyColors.Red,
                    TextColor = OxyColors.Red,
                    FontSize = 9.0,
                    IsAxisVisible = true,
                    MinorTickSize = 0,
                    Maximum = 23,
                    Minimum = -1,
                    MajorStep = 10,
                };
                var _valueAxis1_3 = new LinearAxis()
                {
                    Key = "9#风箱含氧量",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Green,
                    MinorTicklineColor = OxyColors.Green,
                    TicklineColor = OxyColors.Green,
                    TextColor = OxyColors.Green,
                    FontSize = 9.0,
                    IsAxisVisible = true,
                    MinorTickSize = 0,
                    Maximum = 23,
                    Minimum = -1,
                    MajorStep = 10,
                };
                var _valueAxis1_4 = new LinearAxis()
                {
                    Key = "12#风箱含氧量",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Purple,
                    MinorTicklineColor = OxyColors.Purple,
                    TicklineColor = OxyColors.Purple,
                    TextColor = OxyColors.Purple,
                    FontSize = 9.0,
                    IsAxisVisible = true,
                    MinorTickSize = 0,
                    Maximum = 23,
                    Minimum = -1,
                    MajorStep = 10,
                };
                var _valueAxis1_5 = new LinearAxis()
                {
                    Key = "15#风箱含氧量",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.BurlyWood,
                    MinorTicklineColor = OxyColors.BurlyWood,
                    TicklineColor = OxyColors.BurlyWood,
                    TextColor = OxyColors.BurlyWood,
                    FontSize = 9.0,
                    IsAxisVisible = true,
                    MinorTickSize = 0,
                    Maximum = 23,
                    Minimum = -1,
                    MajorStep = 10,
                };
                var _valueAxis1_6 = new LinearAxis()
                {
                    Key = "18#风箱含氧量",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Blue,
                    MinorTicklineColor = OxyColors.Blue,
                    TicklineColor = OxyColors.Blue,
                    TextColor = OxyColors.Blue,
                    FontSize = 9.0,
                    IsAxisVisible = true,
                    MinorTickSize = 0,
                    Maximum = 23,
                    Minimum = -1,
                    MajorStep = 10,
                };

                PlotModel1.Axes.Add(_valueAxis1_1);
                PlotModel2.Axes.Add(_valueAxis1_2);
                PlotModel3.Axes.Add(_valueAxis1_3);
                PlotModel4.Axes.Add(_valueAxis1_4);
                PlotModel5.Axes.Add(_valueAxis1_5);
                PlotModel6.Axes.Add(_valueAxis1_6);

                series1.Color = OxyColors.Black;
                series2.Color = OxyColors.Red;
                series3.Color = OxyColors.Green;
                series4.Color = OxyColors.Purple;
                series5.Color = OxyColors.BurlyWood;
                series6.Color = OxyColors.Blue;

                series1.ItemsSource = Line1;
                series2.ItemsSource = Line2;
                series3.ItemsSource = Line3;
                series4.ItemsSource = Line4;
                series5.ItemsSource = Line5;
                series6.ItemsSource = Line6;

                series1.StrokeThickness = 1;
                series2.StrokeThickness = 1;
                series3.StrokeThickness = 1;
                series4.StrokeThickness = 1;
                series5.StrokeThickness = 1;
                series6.StrokeThickness = 1;
                series1.TrackerFormatString = "{0}\n{2:MM/dd HH:mm} 3#含氧量:{4}%";
                series2.TrackerFormatString = "{0}\n{2:MM/dd HH:mm} 6#含氧量:{4}%";
                series3.TrackerFormatString = "{0}\n{2:MM/dd HH:mm} 9#含氧量:{4}%";
                series4.TrackerFormatString = "{0}\n{2:MM/dd HH:mm} 12#含氧量:{4}%";
                series5.TrackerFormatString = "{0}\n{2:MM/dd HH:mm} 15#含氧量:{4}%";
                series6.TrackerFormatString = "{0}\n{2:MM/dd HH:mm} 18#含氧量:{4}%";

                PlotModel1.Axes.Add(_dateAxis1);
                _dateAxis1.IsAxisVisible = false;
                PlotModel2.Axes.Add(_dateAxis2);
                _dateAxis2.IsAxisVisible = false;
                PlotModel3.Axes.Add(_dateAxis3);
                _dateAxis3.IsAxisVisible = false;
                PlotModel4.Axes.Add(_dateAxis4);
                _dateAxis4.IsAxisVisible = false;
                PlotModel5.Axes.Add(_dateAxis5);
                _dateAxis5.IsAxisVisible = false;
                PlotModel6.Axes.Add(_dateAxis6);
                _dateAxis6.IsAxisVisible = false;

                PlotModel1.Series.Add(series1);
                PlotModel2.Series.Add(series2);
                PlotModel3.Series.Add(series3);
                PlotModel4.Series.Add(series4);
                PlotModel5.Series.Add(series5);
                PlotModel6.Series.Add(series6);

                plotView2.Model = PlotModel1;
                plotView3.Model = PlotModel2;
                plotView4.Model = PlotModel3;
                plotView5.Model = PlotModel4;
                plotView6.Model = PlotModel5;
                plotView7.Model = PlotModel6;
                var PlotController = new OxyPlot.PlotController();
                PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
                plotView2.Controller = PlotController;
                plotView3.Controller = PlotController;
                plotView4.Controller = PlotController;
                plotView5.Controller = PlotController;
                plotView6.Controller = PlotController;
                plotView7.Controller = PlotController;
            }
            string sql2 = "select TIMESTAMP,SIN_PLC_MA_OUT_1_FLUE_O2,SIN_PLC_MA_OUT_2_FLUE_O2 from C_SIN_PLC_1MIN where TIMESTAMP >= '" + start + "' and TIMESTAMP <= '" + end + "'";
            DataTable table2 = dBSQL.GetCommand(sql2);
            if (table2.Rows.Count > 0)
            {
                for (int i = 0; i < table2.Rows.Count; i++)
                {
                    DataPoint line7 = new DataPoint(DateTimeAxis.ToDouble(table2.Rows[i]["TIMESTAMP"]), table2.Rows[i]["SIN_PLC_MA_OUT_1_FLUE_O2"].ToString() == "" ? 0 : Convert.ToDouble(table2.Rows[i]["SIN_PLC_MA_OUT_1_FLUE_O2"]));
                    Line7.Add(line7);
                    DataPoint line8 = new DataPoint(DateTimeAxis.ToDouble(table2.Rows[i]["TIMESTAMP"]), table2.Rows[i]["SIN_PLC_MA_OUT_2_FLUE_O2"].ToString() == "" ? 0 : Convert.ToDouble(table2.Rows[i]["SIN_PLC_MA_OUT_2_FLUE_O2"]));
                    Line8.Add(line8);
                }
                var PlotModel7 = new PlotModel()
                {
                    PlotMargins = new OxyThickness(20, 20, 0, 0),
                    PlotAreaBorderThickness = new OxyThickness(0),
                };
                var PlotModel8 = new PlotModel()
                {
                    PlotMargins = new OxyThickness(20, 20, 0, 10),
                    PlotAreaBorderThickness = new OxyThickness(0),
                };
                var _dateAxis7 = new DateTimeAxis()
                {
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 100,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    AxisTickToLabelDistance = 0,
                    FontSize = 5.0,
                    IsAxisVisible = false,
                };
                var _dateAxis8 = new DateTimeAxis()
                {
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 100,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    AxisTickToLabelDistance = 0,
                    FontSize = 9.0,
                    MinorIntervalType = DateTimeIntervalType.Days,
                    IntervalType = DateTimeIntervalType.Days,
                    StringFormat = "yyyy/MM/dd HH:mm",
                };
                var _valueAxis1_7 = new LinearAxis()
                {
                    Key = "1号除尘器",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.HotPink,
                    MinorTicklineColor = OxyColors.HotPink,
                    TicklineColor = OxyColors.HotPink,
                    TextColor = OxyColors.HotPink,
                    FontSize = 9.0,
                    IsAxisVisible = true,
                    MinorTickSize = 0,
                    Maximum = 23,
                    Minimum = -1,
                    MajorStep = 10,
                };
                var _valueAxis1_8 = new LinearAxis()
                {
                    Key = "2号除尘器",
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    IsZoomEnabled = false,
                    IsPanEnabled = false,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.DarkOrchid,
                    MinorTicklineColor = OxyColors.DarkOrchid,
                    TicklineColor = OxyColors.DarkOrchid,
                    TextColor = OxyColors.DarkOrchid,
                    FontSize = 9.0,
                    IsAxisVisible = true,
                    MinorTickSize = 0,
                    /*Maximum = int.Parse(num8.Max().ToString()) + 1,
                    Minimum = int.Parse(num8.Min().ToString()) - 1,*/
                    Maximum = 25,
                    Minimum = -1,
                    MajorStep = 10,
                };

                PlotModel7.Axes.Add(_valueAxis1_7);
                PlotModel7.Axes.Add(_dateAxis7);
                _dateAxis7.IsAxisVisible = false;
                PlotModel8.Axes.Add(_valueAxis1_8);
                PlotModel8.Axes.Add(_dateAxis8);

                series7.Color = OxyColors.HotPink;
                series8.Color = OxyColors.DarkOrchid;
                series7.ItemsSource = Line7;
                series8.ItemsSource = Line8;
                series7.StrokeThickness = 1;
                series8.StrokeThickness = 1;
                series7.TrackerFormatString = "{0}\n{2:HH:mm} 1号除尘器:{4}%";
                series8.TrackerFormatString = "{0}\n{2:HH:mm} 2号除尘器:{4}%";
                PlotModel7.Series.Add(series7);
                PlotModel8.Series.Add(series8);

                plotView8.Model = PlotModel7;
                plotView9.Model = PlotModel8;
                var PlotController = new OxyPlot.PlotController();
                PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
                plotView8.Controller = PlotController;
                plotView9.Controller = PlotController;
            }
        }

        //查询按钮
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            DateTime d1 = Convert.ToDateTime(textBox_begin.Text);
            DateTime d2 = Convert.ToDateTime(textBox_end.Text);
            getPlotView(d1, d2);
        }

        //实时按钮
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now.AddDays(-7);
            DateTime end = DateTime.Now;
            getPlotView(start, end);
        }

        //历史数据
        private void simpleButton1_Click(object sender, EventArgs e)
        {
        }

        //参数维护
        private void simpleButton2_Click(object sender, EventArgs e)
        {
        }

        public void Timer_state()
        {
        }

        public void _Clear()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {
        }
    }
}