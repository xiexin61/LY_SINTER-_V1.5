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
using OxyPlot;
using DataBase;
using OxyPlot.Axes;
using LY_SINTER.Popover.Course;

namespace LY_SINTER.PAGE.Course
{
    public partial class Add_Water : UserControl
    {
        public vLog vLog { get; set; }

        public System.Timers.Timer _Timer1 { get; set; }
        public System.Timers.Timer _Timer2 { get; set; }

        private PlotModel _myPlotModel;
        private DateTimeAxis _dateAxis;//X轴
        private LinearAxis _valueAxis1;//Y轴
        private LinearAxis _valueAxis2;//Y轴
        private LinearAxis _valueAxis3;//Y轴
        private LinearAxis _valueAxis4;//Y轴
        private LinearAxis _valueAxis5;//Y轴
        private LinearAxis _valueAxis6;//Y轴

        List<DataPoint> Line1 = new List<DataPoint>();
        List<DataPoint> Line2 = new List<DataPoint>();
        List<DataPoint> Line3 = new List<DataPoint>();
        List<DataPoint> Line4 = new List<DataPoint>();
        List<DataPoint> Line5 = new List<DataPoint>();
        List<DataPoint> Line6 = new List<DataPoint>();

        private OxyPlot.Series.LineSeries series1;//曲线
        private OxyPlot.Series.LineSeries series2;//曲线
        private OxyPlot.Series.LineSeries series3;//曲线
        private OxyPlot.Series.LineSeries series4;//曲线
        private OxyPlot.Series.LineSeries series5;//曲线
        private OxyPlot.Series.LineSeries series6;//曲线
        string[] curve_name = { "A_1", "A_2", "A_3", "A_4", "A_5", "A_6" };

        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        public Add_Water()
        {
            InitializeComponent();
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            if (vLog == null)
                vLog = new vLog(".\\log_Page\\Quality\\Add_Water\\");


            _Timer1 = new System.Timers.Timer(10000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { Timer1_Tick_1(); };//响应事件
            _Timer1.Enabled = true;
            _Timer1.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer2 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer2.Elapsed += (x, y) => { Timer1_Tick_2(); };//响应事件
            _Timer2.Enabled = true;
            _Timer2.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）


            Text_Par_Show();
            Img_Par_Text();
            Button_Show();
            Check_text();
            Time_now();
            tendency_curve_HIS(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));
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
                Text_Par_Show();
                Img_Par_Text();
            }
        }
        private void Timer1_Tick_2()
        {
            Action invokeAction = new Action(Timer1_Tick_2);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                Button_Show();
                Check_text();
                Time_now();
            }
        }

        /// <summary>
        /// 最新调整时间
        /// </summary>
        private void Time_now()
        {
            try
            {
                string sql1 = "select top 1 TIMESTAMP FROM MC_WATERCAL_RESULT ORDER BY TIMESTAMP DESC";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    string zxtzsj = dataTable1.Rows[0][0].ToString();
                    label1.Text = "最新调整时间:" + zxtzsj;
                }
                else
                {
                    label1.Text = "";
                }
            }
            catch (Exception ee)
            {
                string mistake = "最新调整时间查询失败" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 加水优化控制模型状态 10s
        /// </summary>
        private void Text_Par_Show()
        {
            try
            {
                //加水优化控制模型状态
                //李涛修改，问题，查询功能没做处理，显示异常
                string sql1 = "select top 1 " +
                    "isnull(WATCAL_FLAG1,8) as WATCAL_FLAG1," +//标志位只能为0和1
                    "isnull(WATCAL_FLAG8,8) as WATCAL_FLAG8," +//若为空或0显示空
                    "isnull(WATCAL_FLAG5,8) as WATCAL_FLAG5," +//若为空或0显示空
                    "isnull(WATCAL_FLAG6,8) as WATCAL_FLAG6 " +//若为空或0显示空
                    "from MC_WATERCAL_RESULT order by TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    int mxtyzt = int.Parse(dataTable1.Rows[0][0].ToString());
                    int mxcftj = int.Parse(dataTable1.Rows[0][1].ToString());
                    int cxjstj = int.Parse(dataTable1.Rows[0][2].ToString());
                    int tzjszt = int.Parse(dataTable1.Rows[0][3].ToString());
                    //模型投用状态
                    if (mxtyzt == 1)
                    {
                        this.textBox1.Text = "一级投入";
                    }
                    else if (mxtyzt == 0)
                    {
                        this.textBox1.Text = "一级退出";
                    }
                    else
                    {
                        this.textBox1.Text = "  ";
                    }
                    //模型触发条件
                    if (mxcftj == 1)
                    {
                        this.textBox2.Text = "周期计算";
                    }
                    else if (mxcftj == 2)
                    {
                        this.textBox2.Text = "目标水分变化";
                    }
                    else if (mxcftj == 3)
                    {
                        this.textBox2.Text = "原始水分变化";
                    }
                    else if (mxcftj == 4)
                    {
                        this.textBox2.Text = "总料量变化";
                    }
                    else
                    {
                        this.textBox2.Text = "";
                    }
                    //重新计算条件
                    if (cxjstj == 1)
                    {
                        this.textBox3.Text = "加水模型程序重启";
                    }
                    else if (cxjstj == 2)
                    {
                        this.textBox3.Text = "一级加水自动开关投入";
                    }
                    else if (cxjstj == 3)
                    {
                        this.textBox3.Text = "二级自动加水开始计算开关投入";
                    }
                    else if (cxjstj == 4)
                    {
                        this.textBox3.Text = "人工修改参数表";
                    }
                    else
                    {
                        this.textBox3.Text = "";
                    }
                    //停止加水状态
                    if (tzjszt == 1)
                    {
                        this.textBox4.Text = "带料停机";
                    }
                    else if (tzjszt == 2)
                    {
                        this.textBox4.Text = "空料停机";
                    }
                    else
                    {
                        this.textBox4.Text = "";
                    }
                }
                //李涛修改，数据异常情况加处理
                //加水优化控制模型参数
                string sql2 = "select top 1 " +
                    "isnull(WATCAL_T_SP_W,0)," +
                    "(isnull(WATCAL_RAW_H2O,0)*isnull(WATCAL_RAW_H2O_B,0))," +
                    "isnull(WATCAL_RAW_H2O_B,0)," +
                    "isnull(WATCAL_1M_H2O_AIM,0)," +
                    "isnull(WATCAL_1M_H2O_TEST,0)," +
                    "isnull(WATCAL_1M_FT_SP,0)," +
                    "isnull(WATCAL_1M_FT_PV,0)," +
                    "isnull(WATCAL_1M_CAL_H2O_AIM_X1,0)," +
                    "isnull(WATCAL_1M_CAL_LIME_X2,0)," +
                    "isnull(WATCAL_1M_CAL_LIME_X3,0)," +
                    "isnull(WATCAL_1M_CAL_Z1,0)," +
                    "isnull(WATCAL_1M_CAL_Z2,0)," +
                    "isnull(WATCAL_1M_CAL_Z3,0)," +
                    "isnull(WATCAL_K_NUMBER,0)," +
                    "isnull(WATCAL_1M_CAL_H,0)," +
                    "isnull(WATCAL_1M_CAL_X_OUT,0)," +
                    "isnull(WATCAL_1M_CAL_X_MAX,0)," +
                    "isnull(WATCAL_1M_CAL_X_MIN,0) " +
                    "from MC_WATERCAL_RESULT order by TIMESTAMP desc";
                DataTable dataTable2 = dBSQL.GetCommand(sql2);
                if (dataTable2.Rows.Count > 0)
                {

                    //总料量SP
                    this.textBox5.Text = Math.Round(double.Parse(dataTable2.Rows[0][0].ToString()), 2).ToString() + " t/h";
                    //原始含水量
                    this.textBox7.Text = Math.Round(double.Parse(dataTable2.Rows[0][1].ToString()), 3).ToString() + " %";
                    //纠偏系数
                    this.textBox8.Text = Math.Round(double.Parse(dataTable2.Rows[0][2].ToString()), 3).ToString();
                    //目标水份
                    this.textBox9.Text = Math.Round(double.Parse(dataTable2.Rows[0][3].ToString()), 3).ToString() + " %";
                    //检测水分
                    this.textBox10.Text = Math.Round(double.Parse(dataTable2.Rows[0][4].ToString()), 3).ToString() + " %";
                    //加水量 SP 
                    this.textBox11.Text = Math.Round(double.Parse(dataTable2.Rows[0][5].ToString()), 2).ToString() + " t/h";
                    //加水量 PV 
                    this.textBox12.Text = Math.Round(double.Parse(dataTable2.Rows[0][6].ToString()), 2).ToString() + " t/h";
                    //目标计算水
                    this.textBox13.Text = Math.Round(double.Parse(dataTable2.Rows[0][7].ToString()), 3).ToString() + " t/h";
                    //生石灰消化水
                    this.textBox14.Text = Math.Round(double.Parse(dataTable2.Rows[0][8].ToString()), 2).ToString() + " t/h";
                    //生石灰补偿消化水
                    this.textBox15.Text = Math.Round(double.Parse(dataTable2.Rows[0][9].ToString()), 2).ToString() + " t/h";
                    //1-7#仓补偿水
                    this.textBox16.Text = Math.Round(double.Parse(dataTable2.Rows[0][10].ToString()), 2).ToString() + " t/h";
                    //8-12#仓补偿水
                    this.textBox17.Text = Math.Round(double.Parse(dataTable2.Rows[0][11].ToString()), 2).ToString() + " t/h";
                    //13-17#仓补偿水
                    this.textBox18.Text = Math.Round(double.Parse(dataTable2.Rows[0][12].ToString()), 2).ToString() + " t/h";
                    //后馈同向调整次数
                    this.textBox19.Text = Math.Round(double.Parse(dataTable2.Rows[0][13].ToString()), 0).ToString();
                    //后馈补偿水
                    this.textBox20.Text = Math.Round(double.Parse(dataTable2.Rows[0][14].ToString()), 2).ToString() + " t/h";
                    //计算设定加水量
                    this.textBox21.Text = Math.Round(double.Parse(dataTable2.Rows[0][15].ToString()), 2).ToString() + " t/h";
                    //加水量上限值
                    this.textBox22.Text = Math.Round(double.Parse(dataTable2.Rows[0][16].ToString()), 2).ToString() + " t/h";
                    //加水量下限值
                    this.textBox23.Text = Math.Round(double.Parse(dataTable2.Rows[0][17].ToString()), 2).ToString() + " t/h";
                }
                else
                {

                }
                //李涛修改 ，数据异常问题0609
                string sql3 = "select top 1 isnull(T_TOTAL_PV_W_3S,0) from C_PLC_3S order by TIMESTAMP desc";
                DataTable dataTable3 = dBSQL.GetCommand(sql3);
                if (dataTable3.Rows.Count > 0)
                {
                    //总料量PV

                    this.textBox6.Text = Math.Round(double.Parse(dataTable3.Rows[0][0].ToString()), 2).ToString() + " t/h";
                }
                else
                {

                }

                string sql4 = "select top 1 isnull(PAR_T_N,0) from MC_WATERCAL_PAR order by TIMESTAMP desc";
                DataTable dataTable4 = dBSQL.GetCommand(sql4);
                if (dataTable4.Rows.Count > 0)
                {
                    //加水调整周期
                    this.textBox24.Text = Math.Round(double.Parse(dataTable4.Rows[0][0].ToString()), 0).ToString() + " S";
                }
                else
                {

                }

            }
            catch (Exception ee)
            {
                string mistake = "Text_Par_Show方法错误" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 混合机参数显示
        /// </summary>
        private void Img_Par_Text()
        {
            try
            {
                //李涛添加 数据处理异常问题 0609
                string sql1 = "select top 1 " +
                    "isnull(T_1M_PRE_BELT_W_1H_1_3S,0)," +
                    "isnull(T_1M_FT_SP_3S,0)," +
                    "isnull(T_1M_FT_PV_3S,0)," +
                    "isnull(T_PLC_1M_VAL_B_SP_3S,0)," +
                    "isnull(T_PLC_1M_VAL_B_PV_3S,0)," +
                    "isnull(T_1M_NEX_WATER_AVG_3S,0)," +
                    "isnull(T_2M_FLOW_SP_3S,0)," +
                    "isnull(T_2M_FLOW_PV_3S,0)," +
                    "isnull(T_PLC_2M_VAL_SP_3S,0)," +
                    "isnull(T_PLC_2M_VAL_PV_3S,0)  " +
                    "from C_PLC_3S order by TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {

                    this.label29.Text = "料量:" + dataTable1.Rows[0][0].ToString() + "t/h";
                    this.label31.Text = "流量SP:" + dataTable1.Rows[0][1].ToString() + "t/h";
                    this.label32.Text = "流量PV:" + dataTable1.Rows[0][2].ToString() + "t/h";
                    this.label33.Text = "开度SP:" + dataTable1.Rows[0][3].ToString() + "%";
                    this.label34.Text = "开度PV:" + dataTable1.Rows[0][4].ToString() + "%";
                    this.label39.Text = "一混后水分:" + dataTable1.Rows[0][5].ToString() + "%";
                    this.label35.Text = "流量SP:" + dataTable1.Rows[0][6].ToString() + "t/h";
                    this.label36.Text = "流量PV:" + dataTable1.Rows[0][7].ToString() + "t/h";
                    this.label37.Text = "开度SP:" + dataTable1.Rows[0][8].ToString() + "%";
                    this.label38.Text = "开度PV:" + dataTable1.Rows[0][9].ToString() + "%";
                }


                string sql2 = "select top (1) WATCAL_RAW_H2O,WATCAL_RAW_H2O_B from MC_WATERCAL_RESULT order by TIMESTAMP desc";
                ///李涛修改 0609 数据异常显示问题，参与计算的字段若为空值则为空
                DataTable dataTable2 = dBSQL.GetCommand(sql2);
                if (dataTable2.Rows.Count > 0)
                {
                    if (dataTable2.Rows[0]["WATCAL_RAW_H2O"].ToString() == "" || dataTable2.Rows[0]["WATCAL_RAW_H2O_B"].ToString() == "")
                    {
                        this.label30.Text = "水分: %";
                    }
                    else
                    {
                        float sf = float.Parse(dataTable2.Rows[0]["WATCAL_RAW_H2O"].ToString()) * float.Parse(dataTable2.Rows[0]["WATCAL_RAW_H2O_B"].ToString());
                        this.label30.Text = "水分:" + sf.ToString() + "%";
                    }
                }
                else
                {
                    this.label30.Text = "水分: %";
                }
            }
            catch (Exception ee)
            {
                string mistake = "Img_Par_Text方法错误" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        //查询button按钮状态
        /// <summary>
        /// 按钮状态
        /// </summary>
        private void Button_Show()
        {

        }


        /// <summary>
        ///曲线勾选框显示数据
        /// </summary>
        private void Check_text()
        {
            try
            {
                string sql1 = "select top 1 " +
                    "ISNULL(M_PLC_1M_WATER_SP,0)," +
                    "ISNULL(M_PLC_1M_A_WATER_PV,0)," +
                    "ISNULL(M_PLC_1M_FT_SP,0)," +
                    "isnull(M_PLC_1M_FT_PV,0)  " +
                    "from C_MFI_PLC_1MIN order by TIMESTAMP desc ";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    float mbsf = float.Parse(dataTable1.Rows[0][0].ToString());
                    float sjsf = float.Parse(dataTable1.Rows[0][1].ToString());
                    float jsllsp = float.Parse(dataTable1.Rows[0][2].ToString());
                    float jsllpv = float.Parse(dataTable1.Rows[0][3].ToString());
                    this.checkBox1.Text = "目标水分(%): " + Math.Round(mbsf, 2).ToString();
                    this.checkBox2.Text = "实际水分(%): " + Math.Round(sjsf, 2).ToString();
                    this.checkBox5.Text = "加水流量SP(t/h): " + Math.Round(jsllsp, 2).ToString();
                    this.checkBox6.Text = "加水流量PV(t/h): " + Math.Round(jsllpv, 2).ToString();
                }
                else
                { }
                string sql2 = "select top 1 " +
                    "isnull(MAT_PLC_T_SP_W,0)," +
                    "isnull(MAT_PLC_T_PV_W,0)" +
                    " from C_MAT_PLC_1MIN order by TIMESTAMP desc ";
                DataTable dataTable2 = dBSQL.GetCommand(sql2);
                if (dataTable2.Rows.Count > 0)
                {
                    float zllsp = float.Parse(dataTable2.Rows[0][0].ToString());
                    float zllpv = float.Parse(dataTable2.Rows[0][1].ToString());
                    this.checkBox3.Text = "总料量SP(t/h): " + Math.Round(zllsp, 2).ToString();
                    this.checkBox4.Text = "总料量PV(t/h): " + Math.Round(zllpv, 2).ToString();
                }
                else
                { }
            }
            catch (Exception ee)
            {
                string mistake = "Check_text方法错误" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 参数查询弹出框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Frm_Add_Water_PAR form_display = new Frm_Add_Water_PAR();
            if (Frm_Add_Water_PAR.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 调整数据查询弹出框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Frm_Add_Water_adjustment form_display = new Frm_Add_Water_adjustment();
            if (Frm_Add_Water_adjustment.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        //按时间查询曲线
        public void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox4.Checked = true;
                checkBox5.Checked = true;
                checkBox6.Checked = true;
                tendency_curve_HIS(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));

            }
            catch (Exception ee)
            {
                string mistake = "按时间查询曲线失败" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 趋势曲线历史
        /// </summary>
        public void tendency_curve_HIS(DateTime _d1, DateTime _d2)
        {

            try
            {

                string sql3 = "select a.TIMESTAMP,a.M_PLC_1M_WATER_SP,a.M_PLC_1M_A_WATER_PV,b.MAT_PLC_T_SP_W,b.MAT_PLC_T_PV_W,a.M_PLC_1M_FT_SP,a.M_PLC_1M_FT_PV from  C_MFI_PLC_1MIN a,C_MAT_PLC_1MIN b where convert(varchar(16),a.TIMESTAMP,121)=convert(varchar(16),b.TIMESTAMP,121) and a.TIMESTAMP between '" + _d1 + "' and '" + _d2 + "' order by a.TIMESTAMP";
                DataTable data_curve_ls = dBSQL.GetCommand(sql3);
                if (data_curve_ls.Rows.Count > 0)
                {
                    Line1.Clear();
                    Line2.Clear();
                    Line3.Clear();
                    Line4.Clear();
                    Line5.Clear();
                    Line6.Clear();
                    List<double> Mun1 = new List<double>();
                    List<double> Mun2 = new List<double>();
                    List<double> Mun3 = new List<double>();
                    List<double> Mun4 = new List<double>();
                    List<double> Mun5 = new List<double>();
                    List<double> Mun6 = new List<double>();
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
                        //******目标水分******
                        DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["M_PLC_1M_WATER_SP"]));
                        Line1.Add(line1);
                        Mun1.Add(Convert.ToDouble(data_curve_ls.Rows[i]["M_PLC_1M_WATER_SP"]));
                        //*****实际水分*****
                        DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["M_PLC_1M_A_WATER_PV"]));
                        Line2.Add(line2);
                        Mun2.Add(Convert.ToDouble(data_curve_ls.Rows[i]["M_PLC_1M_A_WATER_PV"]));
                        //*****总料量sp*****
                        DataPoint line3 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["MAT_PLC_T_SP_W"]));
                        Line3.Add(line3);
                        Mun3.Add(Convert.ToDouble(data_curve_ls.Rows[i]["MAT_PLC_T_SP_W"]));
                        //*****总料量pv*****
                        DataPoint line4 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["MAT_PLC_T_PV_W"]));
                        Line4.Add(line4);
                        Mun4.Add(Convert.ToDouble(data_curve_ls.Rows[i]["MAT_PLC_T_PV_W"]));
                        //*****加水流量sp*****
                        DataPoint line5 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["M_PLC_1M_FT_SP"]));
                        Line5.Add(line5);
                        Mun5.Add(Convert.ToDouble(data_curve_ls.Rows[i]["M_PLC_1M_FT_SP"]));
                        //*****加水流量pv*****
                        DataPoint line6 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["M_PLC_1M_FT_PV"]));
                        Line6.Add(line6);
                        Mun6.Add(Convert.ToDouble(data_curve_ls.Rows[i]["M_PLC_1M_FT_PV"]));
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
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n目标水分:{4}",

                    };
                    if (checkBox1.Checked == true)
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
                        Maximum = (int)(Mun1.Max() + 1),//极值
                        Minimum = (int)(Mun1.Min() - 1),
                        PositionTier = 1,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Purple,//颜色
                        MinorTicklineColor = OxyColors.Purple,//颜色
                        TicklineColor = OxyColors.Purple,//颜色
                        TextColor = OxyColors.Purple,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x,//增长数据
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
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n实际水分:{4}",

                    };
                    if (checkBox2.Checked == true)
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
                        PositionTier = 2,
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
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n总料量SP:{4}",

                    };
                    if (checkBox3.Checked == true)
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
                        Maximum = (int)(Mun3.Max() + 1),//极值
                        Minimum = (int)(Mun3.Min() - 1),
                        PositionTier = 2,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Blue,//颜色
                        MinorTicklineColor = OxyColors.Blue,//颜色
                        TicklineColor = OxyColors.Blue,//颜色
                        TextColor = OxyColors.Blue,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_2,//增长数据
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
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n总料量PV:{4}",

                    };
                    if (checkBox4.Checked == true)
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
                        PositionTier = 3,
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
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n加水流量SP:{4}",

                    };
                    if (checkBox5.Checked == true)
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
                        Maximum = (int)(Mun5.Max() + 1),//极值
                        Minimum = (int)(Mun5.Min() - 1),
                        // PositionTier = 6,
                        PositionTier = 3,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Black,//颜色
                        MinorTicklineColor = OxyColors.Black,//颜色
                        TicklineColor = OxyColors.Black,//颜色
                        TextColor = OxyColors.Black,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_4,//增长数据
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
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n加水流量PV:{4}",

                    };
                    if (checkBox6.Checked == true)
                    {
                        _valueAxis6.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series6);
                    }
                    curve_his.Model = _myPlotModel;
                }

            }
            catch (Exception ee)
            {
                string mistake = "tendency_curve_HIS方法错误" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 开始时间&结束时间赋值历史
        /// </summary>
        public void dateTimePicker_value()
        {
            //结束时间
            DateTime time_end = DateTime.Now;
            //开始时间
            DateTime time_begin = time_end.AddDays(-3);

            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
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
                    _valueAxis2.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series2);
                }
                if (checkBox2.Checked == false)
                {
                    _valueAxis2.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series2);
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
                    _valueAxis3.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series3);
                }
                if (checkBox3.Checked == false)
                {
                    _valueAxis3.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series3);
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
                    _valueAxis4.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series4);
                }
                if (checkBox4.Checked == false)
                {
                    _valueAxis4.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series4);
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
                    _valueAxis5.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series5);
                }
                if (checkBox5.Checked == false)
                {
                    _valueAxis5.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series5);
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
                    _valueAxis6.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series6);
                }
                if (checkBox6.Checked == false)
                {
                    _valueAxis6.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series6);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }
        public void Timer_stop()
        {
            _Timer1.Stop();
            _Timer2.Stop();
        }
        public void Timer_state()
        {
            _Timer1.Start();
            _Timer2.Start();
           
        }
        public void _Clear()
        {
            _Timer1.Close();
            _Timer2.Close();
            this.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
