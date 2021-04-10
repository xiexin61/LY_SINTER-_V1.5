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
using OxyPlot;
using OxyPlot.Axes;

namespace LY_SINTER.PAGE.HIS
{
    public partial class MatFlowMonitor : UserControl
    {
        //1H1(配-1)秤值
        private List<double> C1H1 = new List<double>();

        //1H3(混-1)水分
        private List<double> S1H3 = new List<double>();

        //2H1(混-2)秤值
        private List<double> C2H1 = new List<double>();

        //Z41(混-4)水分
        private List<double> SZ41 = new List<double>();

        //Z41(混-4)秤值
        private List<double> CZ41 = new List<double>();

        //S1(混-5)秤值
        private List<double> CS1 = new List<double>();

        //P5(返-3)流量
        private List<double> LP5 = new List<double>();

        //P9(铺-3)流量
        private List<double> LP9 = new List<double>();

        //LS2(成-2)
        //private List<double> zcfl2 = new List<double>();

        //时间
        private List<string> time = new List<string>();

        //曲线定义
        public OxyPlot.Series.LineSeries checkBox1_1;

        public OxyPlot.Series.LineSeries checkBox2_1;
        public OxyPlot.Series.LineSeries checkBox3_1;
        public OxyPlot.Series.LineSeries checkBox4_1;
        public OxyPlot.Series.LineSeries checkBox5_1;
        public OxyPlot.Series.LineSeries checkBox6_1;
        public OxyPlot.Series.LineSeries checkBox7_1;
        public OxyPlot.Series.LineSeries checkBox8_1;
        public OxyPlot.Series.LineSeries checkBox9_1;
        public OxyPlot.Series.LineSeries checkBox10_1;

        //曲线视图定义
        private PlotModel _myPlotModel;

        private PlotModel _myPlotMode2;
        private PlotModel _myPlotMode3;
        private PlotModel _myPlotMode4;
        private PlotModel _myPlotMode5;
        private PlotModel _myPlotMode6;
        private PlotModel _myPlotMode7;
        private PlotModel _myPlotMode8;
        private PlotModel _myPlotMode9;
        private PlotModel _myPlotModel0;

        //Y轴定义
        private LinearAxis _valueAxis1;//Y轴

        private LinearAxis _valueAxis2;//Y轴
        private LinearAxis _valueAxis3;//Y轴
        private LinearAxis _valueAxis4;//Y轴
        private LinearAxis _valueAxis5;//Y轴
        private LinearAxis _valueAxis6;//Y轴
        private LinearAxis _valueAxis7;//Y轴
        private LinearAxis _valueAxis8;//Y轴
        private LinearAxis _valueAxis9;//Y轴
        private LinearAxis _valueAxis10;//Y轴

        //X轴定义
        private DateTimeAxis _dateAxis1;//X轴

        private DateTimeAxis _dateAxis2;//X轴
        private DateTimeAxis _dateAxis3;//X轴
        private DateTimeAxis _dateAxis4;//X轴
        private DateTimeAxis _dateAxis5;//X轴
        private DateTimeAxis _dateAxis6;//X轴
        private DateTimeAxis _dateAxis7;//X轴
        private DateTimeAxis _dateAxis8;//X轴
        private DateTimeAxis _dateAxis9;//X轴
        private DateTimeAxis _dateAxis10;//X轴

        //绑定数据点定义
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

        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        public MatFlowMonitor()
        {
            InitializeComponent();
            shishiquxian();
            quxian();
            shuju();
        }

        //当前时间(1min)
        private void NewTime()
        {
            try
            {
                string sql1 = "select top 1 TIMESTAMP from C_PLC_3S order by TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                string zxsj = dataTable1.Rows[0][0].ToString();
                label3.Text = "最新调整时间:" + zxsj;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 左侧数据项(1min)
        /// </summary>
        private void shuju()
        {
            try
            {
                string sql1 = "select top(1) T_1M_PRE_BELT_W_1H_1_3S,T_1M_NEX_WATER_AVG_3S,T_1M_NEX_BELT_W_1H2_1_3S,T_2M_NEX_WATER_AVG_3S,M_PLC_2M_A_B_W_3S,M_PLC_2M_A_W_3S,T_COLD_AO_FT_3S,T_BED_MAT_AO_FT_3S,T_PROD_DELT1_FQ_3S from C_PLC_3S order by timestamp desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    float zcpl1 = float.Parse(dataTable1.Rows[0][0].ToString());
                    float zcpl2 = float.Parse(dataTable1.Rows[0][1].ToString());
                    float zcwd1 = float.Parse(dataTable1.Rows[0][2].ToString());
                    float zcwd2 = float.Parse(dataTable1.Rows[0][3].ToString());
                    float zcfy1 = float.Parse(dataTable1.Rows[0][4].ToString());
                    float zcfy2 = float.Parse(dataTable1.Rows[0][5].ToString());
                    float zcfl1 = float.Parse(dataTable1.Rows[0][6].ToString());
                    float zcfl2 = float.Parse(dataTable1.Rows[0][7].ToString());
                    float zcfl = float.Parse(dataTable1.Rows[0][8].ToString());

                    this.textBox1.Text = zcpl1.ToString();
                    this.textBox2.Text = zcpl2.ToString();
                    this.textBox3.Text = zcwd1.ToString();
                    this.textBox4.Text = zcwd2.ToString();
                    this.textBox5.Text = zcfy1.ToString();
                    this.textBox6.Text = zcfy2.ToString();
                    this.textBox7.Text = zcfl1.ToString();
                    this.textBox8.Text = zcfl2.ToString();
                    this.textBox10.Text = zcfl.ToString();
                }
                else
                { }
            }
            catch
            {
            }
        }

        //准备表数据
        private double max1, min1;

        private double max2, min2;
        private double max3, min3;
        private double max4, min4;
        private double max5, min5;
        private double max6, min6;
        private double max7, min7;
        private double max8, min8;
        private double max9, min9;
        private double max10, min10;

        private List<double> Num1 = new List<double>();
        private List<double> Num2 = new List<double>();
        private List<double> Num3 = new List<double>();
        private List<double> Num4 = new List<double>();
        private List<double> Num5 = new List<double>();
        private List<double> Num6 = new List<double>();
        private List<double> Num7 = new List<double>();
        private List<double> Num8 = new List<double>();
        private List<double> Num9 = new List<double>();
        private List<double> Num10 = new List<double>();

        /// <summary>
        /// 曲线数据查询
        /// </summary>
        public void shishiquxian()
        {
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

            try
            {
                int sjd = 0;
                try
                {
                    if (comboBox1.InvokeRequired)
                    {
                        Action xx = () => { sjd = int.Parse(comboBox1.Text); };
                        this.comboBox1.Invoke(xx);
                    }
                    else
                    {
                        sjd = int.Parse(comboBox1.Text);
                    }
                }
                catch (Exception e)
                {
                    var xx = e.InnerException.Message;
                }
                int shujugeshu = sjd * 60;
                string sql1 = "select TIMESTAMP,T_1M_PRE_BELT_W_1H_1_3S,T_1M_NEX_WATER_AVG_3S,T_1M_NEX_BELT_W_1H2_1_3S,T_2M_NEX_WATER_AVG_3S,M_PLC_2M_A_W_3S,M_PLC_2M_A_B_W_3S,T_COLD_AO_FT_3S,T_BED_MAT_AO_FT_3S,T_PROD_DELT1_FQ_3S from C_PLC_3S where TIMESTAMP between '" + DateTime.Now.AddHours(-sjd) + "' and '" + DateTime.Now + "' order by TIMESTAMP";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    for (int a = 0; a < dataTable1.Rows.Count; a++)
                    {
                        string sj = dataTable1.Rows[a][0].ToString();
                        time.Add(sj);
                        DataPoint line1 = new DataPoint();
                        if (dataTable1.Rows[a][1].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable1.Rows[a][1].ToString());
                            C1H1.Add(zcpl_1);
                            line1 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][1]));
                            Num1.Add(Convert.ToDouble(dataTable1.Rows[a][1]));
                        }
                        else
                        {
                            C1H1.Add(double.NaN);
                            line1 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][1]));
                        Line1.Add(line1);
                        //Num1.Add(Convert.ToDouble(dataTable1.Rows[a][1]));
                        DataPoint line2 = new DataPoint();
                        if (dataTable1.Rows[a][2].ToString() != "")
                        {
                            double zcpl_2 = double.Parse(dataTable1.Rows[a][2].ToString());
                            S1H3.Add(zcpl_2);
                            line2 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][2]));
                            Num2.Add(Convert.ToDouble(dataTable1.Rows[a][2]));
                        }
                        else
                        {
                            S1H3.Add(double.NaN);
                            line2 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][2]));
                        Line2.Add(line2);

                        DataPoint line3 = new DataPoint();
                        if (dataTable1.Rows[a][3].ToString() != "")
                        {
                            double zcwd_1 = double.Parse(dataTable1.Rows[a][3].ToString());
                            C2H1.Add(zcwd_1);
                            line3 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][3]));
                            Num3.Add(Convert.ToDouble(dataTable1.Rows[a][3]));
                        }
                        else
                        {
                            C2H1.Add(double.NaN);
                            line3 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line3 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][3]));
                        Line3.Add(line3);

                        DataPoint line4 = new DataPoint();
                        if (dataTable1.Rows[a][4].ToString() != "")
                        {
                            double zcwd_2 = double.Parse(dataTable1.Rows[a][4].ToString());
                            SZ41.Add(zcwd_2);
                            line4 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][4]));
                            Num4.Add(Convert.ToDouble(dataTable1.Rows[a][4]));
                        }
                        else
                        {
                            SZ41.Add(double.NaN);
                            line4 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line4 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][4]));
                        Line4.Add(line4);

                        DataPoint line5 = new DataPoint();
                        if (dataTable1.Rows[a][5].ToString() != "")
                        {
                            double zcfy_1 = double.Parse(dataTable1.Rows[a][5].ToString());
                            CZ41.Add(zcfy_1);
                            line5 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][5]));
                            Num5.Add(Convert.ToDouble(dataTable1.Rows[a][5]));
                        }
                        else
                        {
                            CZ41.Add(double.NaN);
                            line5 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line5 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][5]));
                        Line5.Add(line5);

                        DataPoint line6 = new DataPoint();
                        if (dataTable1.Rows[a][6].ToString() != "")
                        {
                            double zcfy_2 = double.Parse(dataTable1.Rows[a][6].ToString());
                            CS1.Add(zcfy_2);
                            line6 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][6]));
                            Num6.Add(Convert.ToDouble(dataTable1.Rows[a][6]));
                        }
                        else
                        {
                            CS1.Add(double.NaN);
                            line6 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line6 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][6]));
                        Line6.Add(line6);

                        DataPoint line7 = new DataPoint();
                        if (dataTable1.Rows[a][7].ToString() != "")
                        {
                            double zcfl_1 = double.Parse(dataTable1.Rows[a][7].ToString());
                            LP5.Add(zcfl_1);
                            line7 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][7]));
                            Num7.Add(Convert.ToDouble(dataTable1.Rows[a][7]));
                        }
                        else
                        {
                            LP5.Add(double.NaN);
                            line7 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line7 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][7]));
                        Line7.Add(line7);

                        DataPoint line8 = new DataPoint();
                        if (dataTable1.Rows[a][8].ToString() != "")
                        {
                            double zcfl_2 = double.Parse(dataTable1.Rows[a][8].ToString());
                            LP9.Add(zcfl_2);
                            line8 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][8]));
                            Num8.Add(Convert.ToDouble(dataTable1.Rows[a][8]));
                        }
                        else
                        {
                            LP9.Add(double.NaN);
                            line8 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line8 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][8]));
                        Line8.Add(line8);
                    }
                    max1 = (Num1.Count == 0) ? 0 : (int)Num1.Max() + 1;
                    min1 = (Num1.Count == 0) ? 0 : (int)Num1.Min() - 1;
                    max2 = (Num2.Count == 0) ? 0 : (int)Num2.Max() + 1;
                    min2 = (Num2.Count == 0) ? 0 : (int)Num2.Min() - 1;
                    max3 = (Num3.Count == 0) ? 0 : (int)Num3.Max() + 1;
                    min3 = (Num3.Count == 0) ? 0 : (int)Num3.Min() - 1;
                    max4 = (Num4.Count == 0) ? 0 : (int)Num4.Max() + 1;
                    min4 = (Num4.Count == 0) ? 0 : (int)Num4.Min() - 1;
                    max5 = (Num5.Count == 0) ? 0 : (int)Num5.Max() + 1;
                    min5 = (Num5.Count == 0) ? 0 : (int)Num5.Min() - 1;
                    max6 = (Num6.Count == 0) ? 0 : (int)Num6.Max() + 1;
                    min6 = (Num6.Count == 0) ? 0 : (int)Num6.Min() - 1;
                    max7 = (Num7.Count == 0) ? 0 : (int)Num7.Max() + 1;
                    min7 = (Num7.Count == 0) ? 0 : (int)Num7.Min() - 1;
                    max8 = (Num8.Count == 0) ? 0 : (int)Num8.Max() + 1;
                    min8 = (Num8.Count == 0) ? 0 : (int)Num8.Min() - 1;
                }
                else
                {
                    string sj = DateTime.Now.ToString();
                    time.Add(sj);
                    C1H1.Add(0);
                    S1H3.Add(0);
                    C2H1.Add(0);
                    SZ41.Add(0);
                    CZ41.Add(0);
                    CS1.Add(0);
                    LP5.Add(0);
                    LP9.Add(0);
                }
            }
            catch
            { }
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

        /// <summary>
        /// 曲线及坐标轴赋值
        /// </summary>
        public void quxian()
        {
            _myPlotModel = new PlotModel()
            {
                Background = OxyColors.White,
                PlotAreaBorderColor = OxyColors.White,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            //X轴
            _dateAxis1 = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.Dot,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 100,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                AxisTickToLabelDistance = 0,
                FontSize = 5.0,
                IsAxisVisible = false,
            };
            _myPlotModel.Axes.Add(_dateAxis1);
            _valueAxis1 = new LinearAxis()
            {
                Key = "1H1(配-1)秤值",
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
                Maximum = max1,
                Minimum = min1 - 1,
            };
            if (min1 == max1 && min1 == 0)
            {
            }
            else
            {
                if (min1 == 0)
                {
                    _valueAxis1.MajorStep = max1;
                }
                else
                {
                    _valueAxis1.MajorStep = (max1 - min1) / 2;
                }
            }
            _myPlotModel.Axes.Add(_valueAxis1);
            checkBox1_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Red,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "1H1(配-1)秤值",
                ItemsSource = Line1,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}1H1(配-1)秤值:{4}",
            };
            _myPlotModel.Series.Add(checkBox1_1);
            plotView1.Model = _myPlotModel;
            //1H3(混-1)水分
            _myPlotMode2 = new PlotModel()
            {
                Background = OxyColors.White,
                PlotAreaBorderColor = OxyColors.White,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis2 = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.Dot,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 100,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                AxisTickToLabelDistance = 0,
                FontSize = 5.0,
                IsAxisVisible = false,
            };
            _myPlotMode2.Axes.Add(_dateAxis2);
            _valueAxis2 = new LinearAxis()
            {
                Key = "1H3(混-1)水分",
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
                PositionTier = 2,
            };
            _myPlotMode2.Axes.Add(_valueAxis2);
            checkBox2_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Purple,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "1H3(混-1)水分",
                ItemsSource = Line2,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}1H3(混-1)水分:{4}",
            };
            _myPlotMode2.Series.Add(checkBox2_1);
            plotView2.Model = _myPlotMode2;
            //2H1(混-2)秤值
            _myPlotMode3 = new PlotModel()
            {
                Background = OxyColors.White,
                PlotAreaBorderColor = OxyColors.White,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis3 = new DateTimeAxis()
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
            _myPlotMode3.Axes.Add(_dateAxis3);
            _valueAxis3 = new LinearAxis()
            {
                Key = "2H1(混-2)秤值",
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
                Maximum = max3,
                Minimum = min3 - 10,
            };
            if (min3 == max3 && min3 == 0)
            {
            }
            else
            {
                if (min3 == 0)
                {
                    _valueAxis3.MajorStep = max3;
                }
                else
                {
                    _valueAxis3.MajorStep = (max3 - min3) / 2;
                }
            }
            _myPlotMode3.Axes.Add(_valueAxis3);
            checkBox3_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Green,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "2H1(混-2)秤值",
                ItemsSource = Line3,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}2H1(混-2)秤值:{4}",
            };
            _myPlotMode3.Series.Add(checkBox3_1);
            plotView3.Model = _myPlotMode3;
            //2#主轴温度
            _myPlotMode4 = new PlotModel()
            {
                Background = OxyColors.White,
                PlotAreaBorderColor = OxyColors.White,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis4 = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.Dot,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 100,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                AxisTickToLabelDistance = 0,
                FontSize = 5.0,
                IsAxisVisible = false,
            };
            _myPlotMode4.Axes.Add(_dateAxis4);
            _valueAxis4 = new LinearAxis()
            {
                Key = "Z41(混-4)水分",
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
                PositionTier = 2,
            };
            _myPlotMode4.Axes.Add(_valueAxis4);
            checkBox4_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Blue,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "Z41(混-4)水分",
                ItemsSource = Line4,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}Z41(混-4)水分:{4}%",
            };
            _myPlotMode4.Series.Add(checkBox4_1);
            plotView4.Model = _myPlotMode4;
            //1#主轴负压
            _myPlotMode5 = new PlotModel()
            {
                Background = OxyColors.White,
                PlotAreaBorderColor = OxyColors.White,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis5 = new DateTimeAxis()
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
            _myPlotMode5.Axes.Add(_dateAxis5);
            _valueAxis5 = new LinearAxis()
            {
                Key = "Z41(混-4)秤值",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Gold,
                MinorTicklineColor = OxyColors.Gold,
                TicklineColor = OxyColors.Gold,
                TextColor = OxyColors.Gold,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                Maximum = max5,
                Minimum = min5 - 1,
                //StartPosition = 0.5,
            };
            //_valueAxis5.Maximum = getMax((int)max3,(int)min3);
            if (min5 == max5 && min5 == 0)
            {
            }
            else
            {
                if (min5 == 0)
                {
                    _valueAxis5.MajorStep = max5;
                }
                else
                {
                    _valueAxis5.MajorStep = (max5 - min5) / 2;
                }
            }

            _myPlotMode5.Axes.Add(_valueAxis5);
            checkBox5_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Gold,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "Z41(混-4)秤值",
                ItemsSource = Line5,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}Z41(混-4)秤值:{4}",
            };
            _myPlotMode5.Series.Add(checkBox5_1);
            plotView5.Model = _myPlotMode5;

            _myPlotMode6 = new PlotModel()
            {
                Background = OxyColors.White,
                PlotAreaBorderColor = OxyColors.White,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis6 = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.Dot,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 100,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                AxisTickToLabelDistance = 0,
                FontSize = 5.0,
                IsAxisVisible = false,
            };
            _myPlotMode6.Axes.Add(_dateAxis6);
            _valueAxis6 = new LinearAxis()
            {
                Key = "S1(混-5)秤值",
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
                PositionTier = 2,
            };
            _myPlotMode6.Axes.Add(_valueAxis6);
            checkBox6_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Black,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "S1(混-5)秤值",
                ItemsSource = Line6,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}S1(混-5)秤值:{4}",
            };
            _myPlotMode6.Series.Add(checkBox6_1);
            plotView6.Model = _myPlotMode6;
            //1#主轴风量
            _myPlotMode7 = new PlotModel()
            {
                Background = OxyColors.White,
                PlotAreaBorderColor = OxyColors.White,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis7 = new DateTimeAxis()
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
            _myPlotMode7.Axes.Add(_dateAxis7);
            _valueAxis7 = new LinearAxis()
            {
                Key = "P5(返-3)流量",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Orange,
                MinorTicklineColor = OxyColors.Orange,
                TicklineColor = OxyColors.Orange,
                TextColor = OxyColors.Orange,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                Maximum = max7,
                Minimum = min7 - 1,
                //StartPosition = 0.2,
            };
            if (min7 == max7 && min7 == 0)
            {
            }
            else
            {
                if (min7 == 0)
                {
                    _valueAxis7.MajorStep = max7;
                }
                else
                {
                    _valueAxis7.MajorStep = (max7 - min7) / 2;
                }
            }
            _myPlotMode7.Axes.Add(_valueAxis7);
            checkBox7_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Orange,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "P5(返-3)流量",
                ItemsSource = Line7,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}P5(返-3)流量:{4}",
            };
            _myPlotMode7.Series.Add(checkBox7_1);
            plotView7.Model = _myPlotMode7;
            //2#主轴风量
            _myPlotMode8 = new PlotModel()
            {
                Background = OxyColors.White,
                PlotAreaBorderColor = OxyColors.White,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis8 = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.Dot,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 100,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                AxisTickToLabelDistance = 0,
                FontSize = 5.0,
                IsAxisVisible = false,
            };
            _myPlotMode8.Axes.Add(_dateAxis8);
            _valueAxis8 = new LinearAxis()
            {
                Key = "P9(铺-3)流量",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Fuchsia,
                MinorTicklineColor = OxyColors.Fuchsia,
                TicklineColor = OxyColors.Fuchsia,
                TextColor = OxyColors.Fuchsia,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                PositionTier = 2,
            };
            _myPlotMode8.Axes.Add(_valueAxis8);
            checkBox8_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Fuchsia,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "P9(铺-3)流量",
                ItemsSource = Line8,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}P9(铺-3)流量:{4}",
            };
            _myPlotMode8.Series.Add(checkBox8_1);
            plotView8.Model = _myPlotMode8;
        }
    }
}