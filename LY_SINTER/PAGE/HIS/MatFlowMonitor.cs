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
using System.Threading;

namespace LY_SINTER.PAGE.HIS
{
    public partial class MatFlowMonitor : UserControl
    {
        //1H1(配-1)秤值
        private List<double> C1H1 = new List<double>();

        //2H1(混-2)秤值
        private List<double> C2H1 = new List<double>();

        //Z41(混-4)秤值
        private List<double> CZ41 = new List<double>();

        //S1(混-5)秤值
        private List<double> CS1 = new List<double>();

        //Z51(成-1)秤值
        private List<double> Z51 = new List<double>();

        //LS1(成-2)秤值
        private List<double> LS1 = new List<double>();

        //LS2(成-2)秤值
        private List<double> LS2 = new List<double>();

        //Z71(成-3)秤值
        private List<double> Z71 = new List<double>();

        //Z72(成-4)秤值
        private List<double> Z72 = new List<double>();

        //CP1(成-5)秤值
        private List<double> CP1 = new List<double>();

        //CP2(成-6)秤值
        private List<double> CP2 = new List<double>();

        //NS1(成-7)秤值
        private List<double> NS1 = new List<double>();

        //Z62(返-1)秤值
        private List<double> Z62 = new List<double>();

        //P4(返-2)流量
        private List<double> P4 = new List<double>();

        //P5(返-3)流量
        private List<double> P5 = new List<double>();

        //Z61(铺-1)秤值
        private List<double> Z61 = new List<double>();

        //P9(铺-3)流量
        private List<double> P9 = new List<double>();

        //时间
        private List<string> time = new List<string>();

        #region 曲线定义

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
        public OxyPlot.Series.LineSeries checkBox11_1;
        public OxyPlot.Series.LineSeries checkBox12_1;
        public OxyPlot.Series.LineSeries checkBox13_1;
        public OxyPlot.Series.LineSeries checkBox14_1;
        public OxyPlot.Series.LineSeries checkBox15_1;
        public OxyPlot.Series.LineSeries checkBox16_1;
        public OxyPlot.Series.LineSeries checkBox17_1;

        #endregion 曲线定义

        #region 曲线视图定义

        private PlotModel _myPlotModel;
        private PlotModel _myPlotMode2;
        private PlotModel _myPlotMode3;
        private PlotModel _myPlotMode4;
        private PlotModel _myPlotMode5;
        private PlotModel _myPlotMode6;
        private PlotModel _myPlotMode8;
        private PlotModel _myPlotMode9;
        private PlotModel _myPlotModel0;
        private PlotModel _myPlotMode11;
        private PlotModel _myPlotMode12;
        private PlotModel _myPlotMode13;
        private PlotModel _myPlotMode14;

        #endregion 曲线视图定义

        #region 曲线Y轴坐标定义

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
        private LinearAxis _valueAxis11;//Y轴
        private LinearAxis _valueAxis12;//Y轴
        private LinearAxis _valueAxis13;//Y轴
        private LinearAxis _valueAxis14;//Y轴

        #endregion 曲线Y轴坐标定义

        #region 曲线X轴坐标定义

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
        private DateTimeAxis _dateAxis11;//X轴
        private DateTimeAxis _dateAxis12;//X轴
        private DateTimeAxis _dateAxis13;//X轴
        private DateTimeAxis _dateAxis14;//X轴

        #endregion 曲线X轴坐标定义

        #region 曲线绑定数据定义

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

        #endregion 曲线绑定数据定义

        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        public MatFlowMonitor()
        {
            InitializeComponent();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    NewTime();
                    shuju();
                    shishiquxian();
                    quxian();
                    Thread.Sleep(60000);
                }
            });
        }

        /// <summary>
        /// 获取最新调整时间
        /// </summary>
        private void NewTime()
        {
            try
            {
                string zxsj = DateTime.Now.ToString();
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
                string sql1 = "select TOP(1) M_PLC_1M_B_W,M_PLC_1M_A_W,M_PLC_2M_A_B_W,M_PLC_2M_A_W from C_MFI_PLC_1MIN ORDER BY TIMESTAMP DESC";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);

                if (dataTable1.Rows.Count > 0)
                {
                    float NUM1 = float.Parse(dataTable1.Rows[0][0].ToString());
                    float NUM2 = float.Parse(dataTable1.Rows[0][1].ToString());
                    float NUM3 = float.Parse(dataTable1.Rows[0][2].ToString());
                    float NUM4 = float.Parse(dataTable1.Rows[0][3].ToString());

                    this.textBox1.Text = NUM1.ToString();
                    this.textBox2.Text = NUM2.ToString();
                    this.textBox3.Text = NUM3.ToString();
                    this.textBox4.Text = NUM4.ToString();
                }
                else
                { }
                string sql2 = "select TOP(1) CFP_PLC_C_Z51,CFP_PLC_C_LS1,CFP_PLC_C_LS2,CFP_PLC_PROD_DELT1_FQ,CFP_PLC_C_Z72,CFP_PLC_C_CP1,CFP_PLC_C_CP2,CFP_PLC_C_NS1,CFP_PLC_F_Z62,CFP_PLC_F_P4,CFP_PLC_COLD_AO_FT,CFP_PLC_P_Z61,CFP_PLC_BED_MAT_AO_FT from C_CFP_PLC_1MIN  ORDER BY TIMESTAMP DESC";
                DataTable dataTable2 = dBSQL.GetCommand(sql2);
                if (dataTable2.Rows.Count > 0)
                {
                    float NUM5 = float.Parse(dataTable2.Rows[0][0].ToString());
                    float NUM6 = float.Parse(dataTable2.Rows[0][1].ToString());
                    float NUM7 = float.Parse(dataTable2.Rows[0][2].ToString());
                    float NUM8 = float.Parse(dataTable2.Rows[0][3].ToString());
                    float NUM9 = float.Parse(dataTable2.Rows[0][4].ToString());
                    float NUM10 = float.Parse(dataTable2.Rows[0][5].ToString());
                    float NUM11 = float.Parse(dataTable2.Rows[0][6].ToString());
                    float NUM12 = float.Parse(dataTable2.Rows[0][7].ToString());
                    float NUM13 = float.Parse(dataTable2.Rows[0][8].ToString());
                    float NUM14 = float.Parse(dataTable2.Rows[0][9].ToString());
                    float NUM15 = float.Parse(dataTable2.Rows[0][10].ToString());
                    float NUM16 = float.Parse(dataTable2.Rows[0][11].ToString());
                    float NUM17 = float.Parse(dataTable2.Rows[0][12].ToString());

                    this.textBox5.Text = NUM5.ToString();
                    this.textBox6.Text = NUM6.ToString();
                    this.textBox7.Text = NUM7.ToString();
                    this.textBox8.Text = NUM8.ToString();
                    this.textBox9.Text = NUM9.ToString();
                    this.textBox10.Text = NUM10.ToString();
                    this.textBox11.Text = NUM11.ToString();
                    this.textBox12.Text = NUM12.ToString();
                    this.textBox13.Text = NUM13.ToString();
                    this.textBox14.Text = NUM14.ToString();
                    this.textBox15.Text = NUM15.ToString();
                    this.textBox16.Text = NUM16.ToString();
                    this.textBox17.Text = NUM17.ToString();
                }
                else
                {
                }
            }
            catch
            {
            }
        }

        #region 数据变量

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
        private double max11, min11;
        private double max12, min12;
        private double max13, min13;
        private double max14, min14;
        private double max15, min15;
        private double max16, min16;
        private double max17, min17;

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
        private List<double> Num11 = new List<double>();
        private List<double> Num12 = new List<double>();
        private List<double> Num13 = new List<double>();
        private List<double> Num14 = new List<double>();
        private List<double> Num15 = new List<double>();
        private List<double> Num16 = new List<double>();
        private List<double> Num17 = new List<double>();

        #endregion 数据变量

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
            Line11.Clear();
            Line12.Clear();
            Line13.Clear();
            Line14.Clear();
            Line15.Clear();
            Line16.Clear();
            Line17.Clear();

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
                string sql1 = "select TIMESTAMP,M_PLC_1M_B_W,M_PLC_1M_A_W,M_PLC_2M_A_B_W,M_PLC_2M_A_W from C_MFI_PLC_1MIN where TIMESTAMP between '" + DateTime.Now.AddHours(-sjd) + "' and '" + DateTime.Now + "' order by TIMESTAMP";
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
                        Line1.Add(line1);

                        DataPoint line2 = new DataPoint();
                        if (dataTable1.Rows[a][2].ToString() != "")
                        {
                            double zcwd_1 = double.Parse(dataTable1.Rows[a][2].ToString());
                            C2H1.Add(zcwd_1);
                            line2 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][2]));
                            Num2.Add(Convert.ToDouble(dataTable1.Rows[a][2]));
                        }
                        else
                        {
                            C2H1.Add(double.NaN);
                            line2 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        Line2.Add(line2);

                        DataPoint line3 = new DataPoint();
                        if (dataTable1.Rows[a][3].ToString() != "")
                        {
                            double zcfy_1 = double.Parse(dataTable1.Rows[a][3].ToString());
                            CZ41.Add(zcfy_1);
                            line3 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][3]));
                            Num3.Add(Convert.ToDouble(dataTable1.Rows[a][3]));
                        }
                        else
                        {
                            CZ41.Add(double.NaN);
                            line3 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        Line3.Add(line3);

                        DataPoint line4 = new DataPoint();
                        if (dataTable1.Rows[a][4].ToString() != "")
                        {
                            double zcfy_2 = double.Parse(dataTable1.Rows[a][4].ToString());
                            CS1.Add(zcfy_2);
                            line4 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][4]));
                            Num4.Add(Convert.ToDouble(dataTable1.Rows[a][4]));
                        }
                        else
                        {
                            CS1.Add(double.NaN);
                            line4 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        Line4.Add(line4);
                    }
                    max1 = (Num1.Count == 0) ? 0 : (int)Num1.Max() + 1;
                    min1 = (Num1.Count == 0) ? 0 : (int)Num1.Min() - 1;
                    max2 = (Num2.Count == 0) ? 0 : (int)Num2.Max() + 1;
                    min2 = (Num2.Count == 0) ? 0 : (int)Num2.Min() - 1;
                    max3 = (Num3.Count == 0) ? 0 : (int)Num3.Max() + 1;
                    min3 = (Num3.Count == 0) ? 0 : (int)Num3.Min() - 1;
                    max4 = (Num4.Count == 0) ? 0 : (int)Num4.Max() + 1;
                    min4 = (Num4.Count == 0) ? 0 : (int)Num4.Min() - 1;
                }
                else
                {
                    string sj = DateTime.Now.ToString();
                    time.Add(sj);
                    C1H1.Add(0);
                    C2H1.Add(0);
                    CZ41.Add(0);
                    CS1.Add(0);
                }
                string sql2 = "select TIMESTAMP,CFP_PLC_C_Z51,CFP_PLC_C_LS1,CFP_PLC_C_LS2,CFP_PLC_PROD_DELT1_FQ,CFP_PLC_C_Z72,CFP_PLC_C_CP1,CFP_PLC_C_CP2,CFP_PLC_C_NS1,CFP_PLC_F_Z62,CFP_PLC_F_P4,CFP_PLC_COLD_AO_FT,CFP_PLC_P_Z61,CFP_PLC_BED_MAT_AO_FT from C_CFP_PLC_1MIN where TIMESTAMP between '" + DateTime.Now.AddHours(-sjd) + "' and '" + DateTime.Now + "' order by TIMESTAMP";
                DataTable dataTable2 = dBSQL.GetCommand(sql2);
                if (dataTable2.Rows.Count > 0)
                {
                    for (int a = 0; a < dataTable2.Rows.Count; a++)
                    {
                        DataPoint line5 = new DataPoint();
                        if (dataTable2.Rows[a][1].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][1].ToString());
                            Z51.Add(zcpl_1);
                            line5 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][1]));
                            Num5.Add(Convert.ToDouble(dataTable2.Rows[a][1]));
                        }
                        else
                        {
                            Z51.Add(double.NaN);
                            line5 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line5.Add(line5);

                        DataPoint line6 = new DataPoint();
                        if (dataTable2.Rows[a][2].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][2].ToString());
                            LS1.Add(zcpl_1);
                            line6 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][2]));
                            Num6.Add(Convert.ToDouble(dataTable2.Rows[a][2]));
                        }
                        else
                        {
                            LS1.Add(double.NaN);
                            line6 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line6.Add(line6);

                        DataPoint line7 = new DataPoint();
                        if (dataTable2.Rows[a][3].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][3].ToString());
                            LS2.Add(zcpl_1);
                            line7 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][3]));
                            Num7.Add(Convert.ToDouble(dataTable2.Rows[a][3]));
                        }
                        else
                        {
                            LS2.Add(double.NaN);
                            line7 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line7.Add(line7);

                        DataPoint line8 = new DataPoint();
                        if (dataTable2.Rows[a][4].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][4].ToString());
                            Z71.Add(zcpl_1);
                            line8 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][4]));
                            Num8.Add(Convert.ToDouble(dataTable2.Rows[a][4]));
                        }
                        else
                        {
                            Z71.Add(double.NaN);
                            line8 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line8.Add(line8);

                        DataPoint line9 = new DataPoint();
                        if (dataTable2.Rows[a][5].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][5].ToString());
                            Z72.Add(zcpl_1);
                            line9 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][5]));
                            Num9.Add(Convert.ToDouble(dataTable2.Rows[a][5]));
                        }
                        else
                        {
                            Z72.Add(double.NaN);
                            line9 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line9.Add(line9);

                        DataPoint line10 = new DataPoint();
                        if (dataTable2.Rows[a][6].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][6].ToString());
                            CP1.Add(zcpl_1);
                            line10 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][6]));
                            Num10.Add(Convert.ToDouble(dataTable2.Rows[a][6]));
                        }
                        else
                        {
                            CP1.Add(double.NaN);
                            line10 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line10.Add(line10);

                        DataPoint line11 = new DataPoint();
                        if (dataTable2.Rows[a][7].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][7].ToString());
                            CP2.Add(zcpl_1);
                            line11 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][7]));
                            Num11.Add(Convert.ToDouble(dataTable2.Rows[a][7]));
                        }
                        else
                        {
                            CP2.Add(double.NaN);
                            line11 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line11.Add(line11);

                        DataPoint line12 = new DataPoint();
                        if (dataTable2.Rows[a][8].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][8].ToString());
                            NS1.Add(zcpl_1);
                            line12 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][8]));
                            Num12.Add(Convert.ToDouble(dataTable2.Rows[a][8]));
                        }
                        else
                        {
                            NS1.Add(double.NaN);
                            line12 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line12.Add(line12);

                        DataPoint line13 = new DataPoint();
                        if (dataTable2.Rows[a][9].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][9].ToString());
                            Z62.Add(zcpl_1);
                            line13 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][9]));
                            Num13.Add(Convert.ToDouble(dataTable2.Rows[a][9]));
                        }
                        else
                        {
                            Z62.Add(double.NaN);
                            line13 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line13.Add(line13);

                        DataPoint line14 = new DataPoint();
                        if (dataTable2.Rows[a][10].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][10].ToString());
                            P4.Add(zcpl_1);
                            line14 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][10]));
                            Num14.Add(Convert.ToDouble(dataTable2.Rows[a][10]));
                        }
                        else
                        {
                            P4.Add(double.NaN);
                            line14 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line14.Add(line14);

                        DataPoint line15 = new DataPoint();
                        if (dataTable2.Rows[a][11].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][11].ToString());
                            P5.Add(zcpl_1);
                            line15 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][11]));
                            Num15.Add(Convert.ToDouble(dataTable2.Rows[a][11]));
                        }
                        else
                        {
                            P5.Add(double.NaN);
                            line15 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line15.Add(line15);

                        DataPoint line16 = new DataPoint();
                        if (dataTable2.Rows[a][12].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][12].ToString());
                            Z61.Add(zcpl_1);
                            line16 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][12]));
                            Num16.Add(Convert.ToDouble(dataTable2.Rows[a][12]));
                        }
                        else
                        {
                            Z61.Add(double.NaN);
                            line16 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line16.Add(line16);

                        DataPoint line17 = new DataPoint();
                        if (dataTable2.Rows[a][13].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable2.Rows[a][13].ToString());
                            P9.Add(zcpl_1);
                            line17 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), Convert.ToDouble(dataTable2.Rows[a][13]));
                            Num17.Add(Convert.ToDouble(dataTable2.Rows[a][13]));
                        }
                        else
                        {
                            P9.Add(double.NaN);
                            line17 = new DataPoint(DateTimeAxis.ToDouble(dataTable2.Rows[a][0]), double.NaN);
                        }
                        Line17.Add(line17);
                    }
                    max5 = (Num5.Count == 0) ? 0 : (int)Num5.Max() + 1;
                    min5 = (Num5.Count == 0) ? 0 : (int)Num5.Min() - 1;
                    max6 = (Num6.Count == 0) ? 0 : (int)Num6.Max() + 1;
                    min6 = (Num6.Count == 0) ? 0 : (int)Num6.Min() - 1;
                    max7 = (Num7.Count == 0) ? 0 : (int)Num7.Max() + 1;
                    min7 = (Num7.Count == 0) ? 0 : (int)Num7.Min() - 1;
                    max8 = (Num8.Count == 0) ? 0 : (int)Num8.Max() + 1;
                    min8 = (Num8.Count == 0) ? 0 : (int)Num8.Min() - 1;
                    max9 = (Num9.Count == 0) ? 0 : (int)Num9.Max() + 1;
                    min9 = (Num9.Count == 0) ? 0 : (int)Num9.Min() - 1;
                    max10 = (Num10.Count == 0) ? 0 : (int)Num10.Max() + 1;
                    min10 = (Num10.Count == 0) ? 0 : (int)Num10.Min() - 1;
                    max11 = (Num11.Count == 0) ? 0 : (int)Num11.Max() + 1;
                    min11 = (Num11.Count == 0) ? 0 : (int)Num11.Min() - 1;
                    max12 = (Num12.Count == 0) ? 0 : (int)Num12.Max() + 1;
                    min12 = (Num12.Count == 0) ? 0 : (int)Num12.Min() - 1;
                    max13 = (Num13.Count == 0) ? 0 : (int)Num13.Max() + 1;
                    min13 = (Num13.Count == 0) ? 0 : (int)Num13.Min() - 1;
                    max14 = (Num14.Count == 0) ? 0 : (int)Num14.Max() + 1;
                    min14 = (Num14.Count == 0) ? 0 : (int)Num14.Min() - 1;
                    max15 = (Num15.Count == 0) ? 0 : (int)Num15.Max() + 1;
                    min15 = (Num15.Count == 0) ? 0 : (int)Num15.Min() - 1;
                    max16 = (Num16.Count == 0) ? 0 : (int)Num16.Max() + 1;
                    min16 = (Num16.Count == 0) ? 0 : (int)Num16.Min() - 1;
                    max17 = (Num17.Count == 0) ? 0 : (int)Num17.Max() + 1;
                    min17 = (Num17.Count == 0) ? 0 : (int)Num17.Min() - 1;
                }
                else
                {
                    Z51.Add(0);
                    LS1.Add(0);
                    LS2.Add(0);
                    Z71.Add(0);
                    Z72.Add(0);
                    CP1.Add(0);
                    CP2.Add(0);
                    NS1.Add(0);
                    Z62.Add(0);
                    P4.Add(0);
                    P5.Add(0);
                    Z61.Add(0);
                    P9.Add(0);
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
            #region 1H1(配-1)秤值

            _myPlotModel = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
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
                AxislineColor = OxyColors.Green,
                MinorTicklineColor = OxyColors.Green,
                TicklineColor = OxyColors.Green,
                TextColor = OxyColors.Green,
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
                Color = OxyColors.Green,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "1H1(配-1)秤值",
                ItemsSource = Line1,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}1H1(配-1)秤值:{4}",
            };
            _myPlotModel.Series.Add(checkBox1_1);
            plotView1.Model = _myPlotModel;

            #endregion 1H1(配-1)秤值

            #region 2H1(混-2)秤值

            _myPlotMode2 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
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
                Key = "2H1(混-2)秤值",
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
                Maximum = max2,
                Minimum = min2 - 1,
            };
            if (min2 == max2 && min2 == 0)
            {
            }
            else
            {
                if (min2 == 0)
                {
                    _valueAxis2.MajorStep = max2;
                }
                else
                {
                    _valueAxis2.MajorStep = (max2 - min2) / 2;
                }
            }
            _myPlotMode2.Axes.Add(_valueAxis2);
            checkBox2_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Blue,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "2H1(混-2)秤值",
                ItemsSource = Line2,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}2H1(混-2)秤值:{4}",
            };
            _myPlotMode2.Series.Add(checkBox2_1);
            plotView2.Model = _myPlotMode2;

            #endregion 2H1(混-2)秤值

            #region Z41(混-4)秤值

            _myPlotMode3 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
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
                Color = OxyColors.Gold,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "Z41(混-4)秤值",
                ItemsSource = Line3,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}Z41(混-4)秤值:{4}",
            };
            _myPlotMode3.Series.Add(checkBox3_1);
            plotView3.Model = _myPlotMode3;

            #endregion Z41(混-4)秤值

            #region S1(混-5)秤值

            _myPlotMode4 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
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
                Maximum = max4,
                Minimum = min4 - 1,
            };
            if (min4 == max4 && min4 == 0)
            {
            }
            else
            {
                if (min4 == 0)
                {
                    _valueAxis4.MajorStep = max4;
                }
                else
                {
                    _valueAxis4.MajorStep = (max4 - min4) / 2;
                }
            }
            _myPlotMode4.Axes.Add(_valueAxis4);
            checkBox4_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Black,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "S1(混-5)秤值",
                ItemsSource = Line4,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}S1(混-5)秤值:{4}",
            };
            _myPlotMode4.Series.Add(checkBox4_1);
            plotView4.Model = _myPlotMode4;

            #endregion S1(混-5)秤值

            #region Z51(成-1)秤值

            _myPlotMode5 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
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
                Key = "Z51(成-1)秤值",
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
                Color = OxyColors.Orange,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "Z51(成-1)秤值",
                ItemsSource = Line5,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}Z51(成-1)秤值:{4}",
            };
            _myPlotMode5.Series.Add(checkBox5_1);
            plotView5.Model = _myPlotMode5;

            #endregion Z51(成-1)秤值

            #region LS(成-2)秤值

            _myPlotMode6 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
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
                Key = "LS(成-2)秤值",
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
                Maximum = max6 > max7 ? max6 : max7,
                Minimum = min6 > min7 ? min7 - 10 : min6 - 10,
            };
            if (_valueAxis6.Maximum == max6)
            {
                if (min6 == max6 && min6 == 0)
                {
                }
                else
                {
                    if (min6 == 0)
                    {
                        _valueAxis6.MajorStep = max6;
                    }
                    else
                    {
                        _valueAxis6.MajorStep = (max6 - min6) / 2;
                    }
                }
            }
            else
            {
                if (min7 == max7 && min7 == 0)
                {
                }
                else
                {
                    if (min7 == 0)
                    {
                        _valueAxis6.MajorStep = max7;
                    }
                    else
                    {
                        _valueAxis6.MajorStep = (max7 - min7) / 2;
                    }
                }
            }

            _myPlotMode6.Axes.Add(_valueAxis6);
            checkBox6_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Fuchsia,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "LS(成-2)秤值",
                ItemsSource = Line6,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}LS-1(成-2)秤值:{4}",
            };
            _myPlotMode6.Series.Add(checkBox6_1);
            //plotView6.Model = _myPlotMode6;
            //LS-2(成-2)秤值
            /*_myPlotMode7 = new PlotModel()
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
            _myPlotMode6.Axes.Add(_dateAxis7);
            _valueAxis7 = new LinearAxis()
            {
                Key = "LS-2(成-2)秤值",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Olive,
                MinorTicklineColor = OxyColors.Olive,
                TicklineColor = OxyColors.Olive,
                TextColor = OxyColors.Olive,
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
            _myPlotMode6.Axes.Add(_valueAxis7);*/
            checkBox7_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Olive,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "LS(成-2)秤值",
                ItemsSource = Line7,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}LS-2(成-2)秤值:{4}",
            };
            _myPlotMode6.Series.Add(checkBox7_1);
            plotView6.Model = _myPlotMode6;

            #endregion LS(成-2)秤值

            #region Z71(成-3)秤值

            _myPlotMode8 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
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
                Key = "Z71(成-3)秤值",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.SlateGray,
                MinorTicklineColor = OxyColors.SlateGray,
                TicklineColor = OxyColors.SlateGray,
                TextColor = OxyColors.SlateGray,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                PositionTier = 2,
                Maximum = max8,
                Minimum = min8 - 1,
            };
            if (min8 == max8 && min8 == 0)
            {
            }
            else
            {
                if (min8 == 0)
                {
                    _valueAxis8.MajorStep = max8;
                }
                else
                {
                    _valueAxis8.MajorStep = (max8 - min8) / 2;
                }
            }
            _myPlotMode8.Axes.Add(_valueAxis8);
            checkBox8_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.SlateGray,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "Z71(成-3)秤值",
                ItemsSource = Line8,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}Z71(成-3)秤值:{4}",
            };
            _myPlotMode8.Series.Add(checkBox8_1);
            plotView7.Model = _myPlotMode8;

            #endregion Z71(成-3)秤值

            #region Z72(成-4)秤值

            _myPlotMode9 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis9 = new DateTimeAxis()
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
            _myPlotMode9.Axes.Add(_dateAxis9);
            _valueAxis9 = new LinearAxis()
            {
                Key = "Z72(成-4)秤值",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.DeepSkyBlue,
                MinorTicklineColor = OxyColors.DeepSkyBlue,
                TicklineColor = OxyColors.DeepSkyBlue,
                TextColor = OxyColors.DeepSkyBlue,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                PositionTier = 2,
                Maximum = max9,
                Minimum = min9 - 1,
            };
            if (min9 == max9 && min9 == 0)
            {
            }
            else
            {
                if (min9 == 0)
                {
                    _valueAxis9.MajorStep = max9;
                }
                else
                {
                    _valueAxis9.MajorStep = (max9 - min9) / 2;
                }
            }
            _myPlotMode9.Axes.Add(_valueAxis9);
            checkBox9_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.DeepSkyBlue,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "Z72(成-4)秤值",
                ItemsSource = Line9,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}Z72(成-4)秤值:{4}",
            };
            _myPlotMode9.Series.Add(checkBox9_1);
            plotView8.Model = _myPlotMode9;

            #endregion Z72(成-4)秤值

            #region CP1(成-5)秤值

            _myPlotModel0 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis10 = new DateTimeAxis()
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
            _myPlotModel0.Axes.Add(_dateAxis10);
            _valueAxis10 = new LinearAxis()
            {
                Key = "CP1(成-5)秤值",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.SaddleBrown,
                MinorTicklineColor = OxyColors.SaddleBrown,
                TicklineColor = OxyColors.SaddleBrown,
                TextColor = OxyColors.SaddleBrown,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                PositionTier = 2,
                Maximum = max10,
                Minimum = min10 - 1,
            };
            if (min10 == max10 && min10 == 0)
            {
            }
            else
            {
                if (min10 == 0)
                {
                    _valueAxis10.MajorStep = max10;
                }
                else
                {
                    _valueAxis10.MajorStep = (max10 - min10) / 2;
                }
            }
            _myPlotModel0.Axes.Add(_valueAxis10);
            checkBox10_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.SaddleBrown,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "CP1(成-5)秤值",
                ItemsSource = Line10,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}CP1(成-5)秤值:{4}",
            };
            _myPlotModel0.Series.Add(checkBox10_1);
            plotView9.Model = _myPlotModel0;

            #endregion CP1(成-5)秤值

            #region CP2(成-6)秤值

            _myPlotMode11 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis11 = new DateTimeAxis()
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
            _myPlotMode11.Axes.Add(_dateAxis11);
            _valueAxis11 = new LinearAxis()
            {
                Key = "CP2(成-6)秤值",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.OrangeRed,
                MinorTicklineColor = OxyColors.OrangeRed,
                TicklineColor = OxyColors.OrangeRed,
                TextColor = OxyColors.OrangeRed,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                PositionTier = 2,
                Maximum = max11,
                Minimum = min11 - 1,
            };
            if (min11 == max11 && min11 == 0)
            {
            }
            else
            {
                if (min11 == 0)
                {
                    _valueAxis11.MajorStep = max11;
                }
                else
                {
                    _valueAxis11.MajorStep = (max11 - min11) / 2;
                }
            }
            _myPlotMode11.Axes.Add(_valueAxis11);
            checkBox11_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.OrangeRed,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "CP2(成-6)秤值",
                ItemsSource = Line11,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}CP2(成-6)秤值:{4}",
            };
            _myPlotMode11.Series.Add(checkBox11_1);
            plotView10.Model = _myPlotMode11;

            #endregion CP2(成-6)秤值

            #region NS1(成-7)秤值

            _myPlotMode12 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis12 = new DateTimeAxis()
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
            _myPlotMode12.Axes.Add(_dateAxis12);
            _valueAxis12 = new LinearAxis()
            {
                Key = "NS1(成-7)秤值",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.RoyalBlue,
                MinorTicklineColor = OxyColors.RoyalBlue,
                TicklineColor = OxyColors.RoyalBlue,
                TextColor = OxyColors.RoyalBlue,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                PositionTier = 2,
                Maximum = max12,
                Minimum = min12 - 10,
            };
            if (min12 == max12 && min12 == 0)
            {
            }
            else
            {
                if (min12 == 0)
                {
                    _valueAxis12.MajorStep = max12;
                }
                else
                {
                    _valueAxis12.MajorStep = (max12 - min12) / 2;
                }
            }
            _myPlotMode12.Axes.Add(_valueAxis12);
            checkBox12_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.RoyalBlue,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "NS1(成-7)秤值",
                ItemsSource = Line12,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}NS1(成-7)秤值:{4}",
            };
            _myPlotMode12.Series.Add(checkBox12_1);
            plotView11.Model = _myPlotMode12;

            #endregion NS1(成-7)秤值

            #region 返矿皮带秤值

            _myPlotMode13 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis13 = new DateTimeAxis()
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
            _myPlotMode13.Axes.Add(_dateAxis13);
            _valueAxis13 = new LinearAxis()
            {
                Key = "返矿皮带秤值",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Tomato,
                MinorTicklineColor = OxyColors.Tomato,
                TicklineColor = OxyColors.Tomato,
                TextColor = OxyColors.Tomato,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                PositionTier = 2,
                Maximum = 150,
                Minimum = 0,
                MajorStep = 50,
                //Maximum = max13 > max14 ? max13 : max14,
                //Maximum = Math.Max(max13, max14) > Math.Max(max14, max15) ? Math.Max(max13, max14) : Math.Max(max14, max15),
                //Minimum = Math.Min(max13, max14) > Math.Min(max14, max15) ? Math.Min(max14, max15) : Math.Min(max13, max14),
            };
            //_valueAxis13.MajorStep = (_valueAxis13.Maximum - _valueAxis13.Minimum) / 2;
            /*if (_valueAxis13.Maximum == max13)
            {
                if (min13 == max13 && min13 == 0)
                {
                }
                else
                {
                    if (min13 == 0)
                    {
                        _valueAxis13.MajorStep = max13;
                    }
                    else
                    {
                        _valueAxis13.MajorStep = (max13 - min13) / 2;
                    }
                }
            }*/

            _myPlotMode13.Axes.Add(_valueAxis13);
            checkBox13_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Tomato,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "返矿皮带秤值",
                ItemsSource = Line13,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm} Z62(返-1)秤值:{4}",
            };
            _myPlotMode13.Series.Add(checkBox13_1);

            checkBox14_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.YellowGreen,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "返矿皮带秤值",
                ItemsSource = Line14,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm} P4(返-2)秤值:{4}",
            };
            _myPlotMode13.Series.Add(checkBox14_1);

            checkBox15_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.DarkTurquoise,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "返矿皮带秤值",
                ItemsSource = Line15,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm} P5(返-3)秤值:{4}",
            };
            _myPlotMode13.Series.Add(checkBox15_1);
            plotView12.Model = _myPlotMode13;

            #endregion 返矿皮带秤值

            #region 铺底料皮带秤值

            _myPlotMode14 = new PlotModel()
            {
                Background = OxyColors.AliceBlue,
                PlotAreaBorderColor = OxyColors.AliceBlue,
                PlotMargins = new OxyThickness(40, 10, 5, 0),
            };
            _dateAxis14 = new DateTimeAxis()
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
            _myPlotMode14.Axes.Add(_dateAxis14);
            _valueAxis14 = new LinearAxis()
            {
                Key = "铺底料皮带秤值",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.DarkViolet,
                MinorTicklineColor = OxyColors.DarkViolet,
                TicklineColor = OxyColors.DarkViolet,
                TextColor = OxyColors.DarkViolet,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                PositionTier = 2,
                Maximum = max16 > max17 ? max16 : max17,
                Minimum = min16 > min17 ? min17 - 10 : min16 - 10,
            };
            if (_valueAxis14.Maximum == max16)
            {
                if (min16 == max16 && min16 == 0)
                {
                }
                else
                {
                    if (min16 == 0)
                    {
                        _valueAxis14.MajorStep = max16;
                    }
                    else
                    {
                        _valueAxis14.MajorStep = (max16 - min16) / 2;
                    }
                }
            }
            else
            {
                if (min17 == max17 && min17 == 0)
                {
                }
                else
                {
                    if (min17 == 0)
                    {
                        _valueAxis14.MajorStep = max17;
                    }
                    else
                    {
                        _valueAxis14.MajorStep = (max17 - min17) / 2;
                    }
                }
            }

            _myPlotMode14.Axes.Add(_valueAxis14);
            checkBox16_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.DarkViolet,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "铺底料皮带秤值",
                ItemsSource = Line16,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm} Z61(铺-1)秤值:{4}",
            };
            _myPlotMode14.Series.Add(checkBox16_1);
            checkBox17_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.SlateBlue,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "铺底料皮带秤值",
                ItemsSource = Line17,
                TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm} P9(铺-3)秤值:{4}",
            };
            _myPlotMode14.Series.Add(checkBox17_1);
            plotView13.Model = _myPlotMode14;

            #endregion 铺底料皮带秤值
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView1.Model = null;
                if (checkBox1.Checked == true)
                {
                    _myPlotModel.Series.Add(checkBox1_1);
                }
                if (checkBox1.Checked == false)
                {
                    _myPlotModel.Series.Remove(checkBox1_1);
                }
                plotView1.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView2.Model = null;
                if (checkBox2.Checked == true)
                {
                    _myPlotMode2.Series.Add(checkBox2_1);
                }
                if (checkBox2.Checked == false)
                {
                    _myPlotMode2.Series.Remove(checkBox2_1);
                }
                plotView2.Model = _myPlotMode2;
            }
            catch
            { }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView3.Model = null;
                if (checkBox3.Checked == true)
                {
                    _myPlotMode3.Series.Add(checkBox3_1);
                }
                if (checkBox3.Checked == false)
                {
                    _myPlotMode3.Series.Remove(checkBox3_1);
                }
                plotView3.Model = _myPlotMode3;
            }
            catch
            { }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView4.Model = null;
                if (checkBox4.Checked == true)
                {
                    _myPlotMode4.Series.Add(checkBox4_1);
                }
                if (checkBox4.Checked == false)
                {
                    _myPlotMode4.Series.Remove(checkBox4_1);
                }
                plotView4.Model = _myPlotMode4;
            }
            catch
            { }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox5.Checked == true)
                {
                    _myPlotMode5.Series.Add(checkBox5_1);
                }
                if (checkBox5.Checked == false)
                {
                    _myPlotMode5.Series.Remove(checkBox5_1);
                }
                plotView5.Model = _myPlotMode5;
            }
            catch
            { }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView6.Model = null;
                if (checkBox6.Checked == true)
                {
                    _myPlotMode6.Series.Add(checkBox6_1);
                }
                if (checkBox6.Checked == false)
                {
                    _myPlotMode6.Series.Remove(checkBox6_1);
                }
                plotView6.Model = _myPlotMode6;
            }
            catch
            { }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView6.Model = null;
                if (checkBox7.Checked == true)
                {
                    _myPlotMode6.Series.Add(checkBox7_1);
                }
                if (checkBox7.Checked == false)
                {
                    _myPlotMode6.Series.Remove(checkBox7_1);
                }
                plotView6.Model = _myPlotMode6;
            }
            catch
            { }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView7.Model = null;
                if (checkBox8.Checked == true)
                {
                    _myPlotMode8.Series.Add(checkBox8_1);
                }
                if (checkBox8.Checked == false)
                {
                    _myPlotMode8.Series.Remove(checkBox8_1);
                }
                plotView7.Model = _myPlotMode8;
            }
            catch
            { }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView8.Model = null;
                if (checkBox9.Checked == true)
                {
                    _myPlotMode9.Series.Add(checkBox9_1);
                }
                if (checkBox9.Checked == false)
                {
                    _myPlotMode9.Series.Remove(checkBox9_1);
                }
                plotView8.Model = _myPlotMode9;
            }
            catch
            { }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView9.Model = null;
                if (checkBox10.Checked == true)
                {
                    _myPlotModel0.Series.Add(checkBox10_1);
                }
                if (checkBox10.Checked == false)
                {
                    _myPlotModel0.Series.Remove(checkBox10_1);
                }
                plotView9.Model = _myPlotModel0;
            }
            catch
            { }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView10.Model = null;
                if (checkBox11.Checked == true)
                {
                    _myPlotMode11.Series.Add(checkBox11_1);
                }
                if (checkBox11.Checked == false)
                {
                    _myPlotMode11.Series.Remove(checkBox11_1);
                }
                plotView10.Model = _myPlotMode11;
            }
            catch
            { }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView11.Model = null;
                if (checkBox12.Checked == true)
                {
                    _myPlotMode12.Series.Add(checkBox12_1);
                }
                if (checkBox12.Checked == false)
                {
                    _myPlotMode12.Series.Remove(checkBox12_1);
                }
                plotView11.Model = _myPlotMode12;
            }
            catch
            { }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView12.Model = null;
                if (checkBox13.Checked == true)
                {
                    _myPlotMode13.Series.Add(checkBox13_1);
                }
                if (checkBox13.Checked == false)
                {
                    _myPlotMode13.Series.Remove(checkBox13_1);
                }
                plotView12.Model = _myPlotMode13;
            }
            catch
            { }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    plotView12.Model = null;
                    if (checkBox14.Checked == true)
                    {
                        _myPlotMode13.Series.Add(checkBox14_1);
                    }
                    if (checkBox14.Checked == false)
                    {
                        _myPlotMode13.Series.Remove(checkBox14_1);
                    }
                    plotView12.Model = _myPlotMode13;
                }
                catch
                { }
            }
            catch
            { }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView12.Model = null;
                if (checkBox15.Checked == true)
                {
                    _myPlotMode13.Series.Add(checkBox15_1);
                }
                if (checkBox15.Checked == false)
                {
                    _myPlotMode13.Series.Remove(checkBox15_1);
                }
                plotView12.Model = _myPlotMode13;
            }
            catch
            { }
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView13.Model = null;
                if (checkBox16.Checked == true)
                {
                    _myPlotMode14.Series.Add(checkBox16_1);
                }
                if (checkBox16.Checked == false)
                {
                    _myPlotMode14.Series.Remove(checkBox16_1);
                }
                plotView13.Model = _myPlotMode14;
            }
            catch
            { }
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView13.Model = null;
                if (checkBox17.Checked == true)
                {
                    _myPlotMode14.Series.Add(checkBox17_1);
                }
                if (checkBox17.Checked == false)
                {
                    _myPlotMode14.Series.Remove(checkBox17_1);
                }
                plotView13.Model = _myPlotMode14;
            }
            catch
            { }
        }

        /// <summary>
        /// 显示全部曲线按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.checkBox1.Checked = true;
                this.checkBox2.Checked = true;
                this.checkBox3.Checked = true;
                this.checkBox4.Checked = true;
                this.checkBox5.Checked = true;
                this.checkBox6.Checked = true;
                this.checkBox7.Checked = true;
                this.checkBox8.Checked = true;
                this.checkBox9.Checked = true;
                this.checkBox10.Checked = true;
                this.checkBox11.Checked = true;
                this.checkBox12.Checked = true;
                this.checkBox13.Checked = true;
                this.checkBox14.Checked = true;
                this.checkBox15.Checked = true;
                this.checkBox16.Checked = true;
                this.checkBox17.Checked = true;
            }
            catch
            { }
        }

        /// <summary>
        /// 隐藏全部曲线按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                this.checkBox1.Checked = false;
                this.checkBox2.Checked = false;
                this.checkBox3.Checked = false;
                this.checkBox4.Checked = false;
                this.checkBox5.Checked = false;
                this.checkBox6.Checked = false;
                this.checkBox7.Checked = false;
                this.checkBox8.Checked = false;
                this.checkBox9.Checked = false;
                this.checkBox10.Checked = false;
                this.checkBox11.Checked = false;
                this.checkBox12.Checked = false;
                this.checkBox13.Checked = false;
                this.checkBox14.Checked = false;
                this.checkBox15.Checked = false;
                this.checkBox16.Checked = false;
                this.checkBox17.Checked = false;
            }
            catch
            { }
        }

        /// <summary>
        /// 下拉框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            shishiquxian();
            quxian();
        }

        /// <summary>
        /// 控件关闭
        /// </summary>
        public void _Clear()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}