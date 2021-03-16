using DataBase;
using OxyPlot;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LY_SINTER.PAGE.HIS
{
    public partial class shishiquxianchaxun : UserControl
    {
        //时间
        private List<string> shijian = new List<string>();

        //1#主抽频率
        private List<double> zcpl1 = new List<double>();

        //2#主抽频率
        private List<double> zcpl2 = new List<double>();

        //1#主抽温度
        private List<double> zcwd1 = new List<double>();

        //2#主抽温度
        private List<double> zcwd2 = new List<double>();

        //1#主抽负压
        private List<double> zcfy1 = new List<double>();

        //2#主抽负压
        private List<double> zcfy2 = new List<double>();

        //1#主抽风量
        private List<double> zcfl1 = new List<double>();

        //2#主抽风量
        private List<double> zcfl2 = new List<double>();

        //时间
        private List<string> shijian1 = new List<string>();

        //终点位置
        private List<double> zdwz = new List<double>();

        //布料厚度
        private List<double> blhd = new List<double>();

        //点火温度
        private List<double> dhwd = new List<double>();

        //总料量
        private List<double> zll = new List<double>();

        //一混加水量
        private List<double> yhjsl = new List<double>();

        //二混加水量
        private List<double> ehjsl = new List<double>();

        //混合料仓位
        private List<double> hhlcw = new List<double>();

        //圆辊转速
        private List<double> ygzs = new List<double>();

        //烧结机机速
        private List<double> sjjs = new List<double>();

        //环冷机机速
        private List<double> hljs = new List<double>();

        //1min一个数
        //时间
        private List<string> shijian_1 = new List<string>();

        //1#主抽频率
        private List<double> zcpl1_1 = new List<double>();

        //2#主抽频率
        private List<double> zcpl2_1 = new List<double>();

        //1#主抽温度
        private List<double> zcwd1_1 = new List<double>();

        //2#主抽温度
        private List<double> zcwd2_1 = new List<double>();

        //1#主抽负压
        private List<double> zcfy1_1 = new List<double>();

        //2#主抽负压
        private List<double> zcfy2_1 = new List<double>();

        //1#主抽风量
        private List<double> zcfl1_1 = new List<double>();

        //2#主抽风量
        private List<double> zcfl2_1 = new List<double>();

        //时间
        private List<string> shijian1_1 = new List<string>();

        //终点位置
        private List<double> zdwz_1 = new List<double>();

        //布料厚度
        private List<double> blhd_1 = new List<double>();

        //点火温度
        private List<double> dhwd_1 = new List<double>();

        //总料量
        private List<double> zll_1 = new List<double>();

        //一混加水量
        private List<double> yhjsl_1 = new List<double>();

        //二混加水量
        private List<double> ehjsl_1 = new List<double>();

        //混合料仓位
        private List<double> hhlcw_1 = new List<double>();

        //圆辊转速
        private List<double> ygzs_1 = new List<double>();

        //烧结机机速
        private List<double> sjjs_1 = new List<double>();

        //环冷机机速
        private List<double> hljs_1 = new List<double>();

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
        public OxyPlot.Series.LineSeries checkBox11_1;
        public OxyPlot.Series.LineSeries checkBox12_1;
        public OxyPlot.Series.LineSeries checkBox13_1;
        public OxyPlot.Series.LineSeries checkBox14_1;
        public OxyPlot.Series.LineSeries checkBox15_1;
        public OxyPlot.Series.LineSeries checkBox16_1;
        public OxyPlot.Series.LineSeries checkBox17_1;
        public OxyPlot.Series.LineSeries checkBox18_1;

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
        private PlotModel _myPlotModel1;
        private PlotModel _myPlotModel2;
        private PlotModel _myPlotModel3;
        private PlotModel _myPlotModel4;
        private PlotModel _myPlotModel5;
        private PlotModel _myPlotModel6;
        private PlotModel _myPlotModel7;
        private PlotModel _myPlotModel8;

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
        private LinearAxis _valueAxis11;//Y轴
        private LinearAxis _valueAxis12;//Y轴
        private LinearAxis _valueAxis13;//Y轴
        private LinearAxis _valueAxis14;//Y轴
        private LinearAxis _valueAxis15;//Y轴
        private LinearAxis _valueAxis16;//Y轴
        private LinearAxis _valueAxis17;//Y轴
        private LinearAxis _valueAxis18;//Y轴

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
        private DateTimeAxis _dateAxis11;//X轴
        private DateTimeAxis _dateAxis12;//X轴
        private DateTimeAxis _dateAxis13;//X轴
        private DateTimeAxis _dateAxis14;//X轴
        private DateTimeAxis _dateAxis15;//X轴
        private DateTimeAxis _dateAxis16;//X轴
        private DateTimeAxis _dateAxis17;//X轴
        private DateTimeAxis _dateAxis18;//X轴

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
        private List<DataPoint> Line11 = new List<DataPoint>();
        private List<DataPoint> Line12 = new List<DataPoint>();
        private List<DataPoint> Line13 = new List<DataPoint>();
        private List<DataPoint> Line14 = new List<DataPoint>();
        private List<DataPoint> Line15 = new List<DataPoint>();
        private List<DataPoint> Line16 = new List<DataPoint>();
        private List<DataPoint> Line17 = new List<DataPoint>();
        private List<DataPoint> Line18 = new List<DataPoint>();

        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        //int sjd = 4;
        public shishiquxianchaxun()
        {
            InitializeComponent();
            //曲线控件背景颜色
            /*lChartPlus40.LChart.BackColor = Color.White;
            lChartPlus41.LChart.BackColor = Color.White;
            lChartPlus42.LChart.BackColor = Color.White;
            lChartPlus43.LChart.BackColor = Color.White;
            lChartPlus44.LChart.BackColor = Color.White;
            lChartPlus45.LChart.BackColor = Color.White;
            lChartPlus46.LChart.BackColor = Color.White;
            lChartPlus47.LChart.BackColor = Color.White;
            lChartPlus48.LChart.BackColor = Color.White;
            lChartPlus49.LChart.BackColor = Color.White;
            lChartPlus49_1.LChart.BackColor = Color.White;
            lChartPlus49_2.LChart.BackColor = Color.White;
            lChartPlus49_3.LChart.BackColor = Color.White;
            lChartPlus49_4.LChart.BackColor = Color.White;*/
            time();//最新调整时间
            shuju();//左数据项
            //quxiandingyi();//曲线定义
            //quxianfuzhi();//曲线赋值
            //zhongdianweizhiquxian();//终点位置曲线赋值
            //int sjd = int.Parse(comboBox1.Text);
            Task.Factory.StartNew(() =>
            {
                /*while (true)
                {
                    HIS_CURVE_SS(DateTime.Now.AddHours(-12), DateTime.Now);
                    HIS_CURVE_SS2(DateTime.Now.AddHours(-12), DateTime.Now);
                    HIS_CURVE_SS4(DateTime.Now.AddHours(-12), DateTime.Now);
                    Thread.Sleep(60000);
                }*/
                //int sjd = int.Parse(comboBox1.Text);
                while (true)
                {
                    shishiquxian();
                    quxian();
                    Thread.Sleep(60000);
                }

                /*for (int i = 1; i < 100; i++)
                {
                    da = da.AddHours(1);
                    end = end.AddHours(1);
                    shishiquxian();
                    quxian();
                    //HIS_CURVE_Test(DateTime.Now.AddDays(-i-1), DateTime.Now.AddDays(-i));
                    Thread.Sleep(3000);
                }*/
            });
            //shishiquxian();
            //quxian();
            timer1.Enabled = true;
            timer2.Enabled = true;
        }

        //定时器1min刷新
        private void timer1_Tick(object sender, EventArgs e)
        {
            //time();
            ///shuju();
            //min1();//一分钟取一个数(前6条曲线)
        }

        //定时器1min刷新
        private void timer2_Tick(object sender, EventArgs e)
        {
            //min2();//一分钟取一个数（第7条曲线后）
        }

        //当前时间(1min)
        private void time()
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

        //左数据项(1min)
        private void shuju()
        {
            try
            {
                string sql1 = "select top 1 ISNULL(T_PLC_MA_FAN_1_SP_3S,0),ISNULL(T_PLC_MA_FAN_2_SP_3S,0),ISNULL(T_MA_SB_1_FLUE_TE_3S,0),ISNULL(T_MA_SB_2_FLUE_TE_3S,0),ISNULL(T_MA_SB_1_FLUE_PT_3S,0),ISNULL(T_MA_SB_2_FLUE_PT_3S,0)," +
                    "ISNULL(T_MA_SB_1_FLUE_FT_3S,0),ISNULL(T_MA_SB_2_FLUE_FT_3S,0),ISNULL(C_THICK_PV_3S,0),ISNULL(T_TOTAL_SP_W_3S,0),ISNULL(T_1M_FT_SP_3S,0),ISNULL(T_2M_FLOW_SP_3S,0),ISNULL(T_BLEND_LEVEL_3S,0)," +
                    "ISNULL(T_STICK_SP_3S,0),ISNULL(T_SIN_MS_SP_3S,0),ISNULL(T_RC_SPEED_SP_3S,0),isnull(T_IG_01_TE_3S,0),isnull(T_IG_02_TE_3S,0),isnull(T_IG_03_TE_3S,0) from C_PLC_3S order by TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    float sjjjs = 0;
                    float hljjs = 0;
                    //       实时曲线：烧结机机速：改为C_MFI_PLC_1MIN表，F_PLC_SIN_SPEED_PV字段；
                    //环冷机机速：改为C_CFP_PLC_1MIN表，CFP_PLC_RC_SPEED_PV字段；（曲线查询能否改为
                    var sql_1 = "select top(1) F_PLC_SIN_SPEED_PV from C_MFI_PLC_1MIN order by TIMESTAMP desc";
                    DataTable table_1 = dBSQL.GetCommand(sql_1);
                    if (table_1.Rows.Count > 0)
                    {
                        sjjjs = float.Parse(table_1.Rows[0][0].ToString());
                    }
                    var sql_2 = "select CFP_PLC_RC_SPEED_PV from C_CFP_PLC_1MIN order by TIMESTAMP desc";
                    DataTable table_2 = dBSQL.GetCommand(sql_2);
                    if (table_2.Rows.Count > 0)
                    {
                        hljjs = float.Parse(table_2.Rows[0][0].ToString());
                    }
                    float zcpl1 = float.Parse(dataTable1.Rows[0][0].ToString());
                    float zcpl2 = float.Parse(dataTable1.Rows[0][1].ToString());
                    float zcwd1 = float.Parse(dataTable1.Rows[0][2].ToString());
                    float zcwd2 = float.Parse(dataTable1.Rows[0][3].ToString());
                    float zcfy1 = float.Parse(dataTable1.Rows[0][4].ToString());
                    float zcfy2 = float.Parse(dataTable1.Rows[0][5].ToString());
                    float zcfl1 = float.Parse(dataTable1.Rows[0][6].ToString());
                    float zcfl2 = float.Parse(dataTable1.Rows[0][7].ToString());
                    //布料厚度
                    float blhd = float.Parse(dataTable1.Rows[0][8].ToString());
                    //总料量
                    float zll = float.Parse(dataTable1.Rows[0][9].ToString());
                    float yhjsl = float.Parse(dataTable1.Rows[0][10].ToString());
                    float ehjsl = float.Parse(dataTable1.Rows[0][11].ToString());
                    float hhlcw = float.Parse(dataTable1.Rows[0][12].ToString());
                    float ygzs = float.Parse(dataTable1.Rows[0][13].ToString());

                    this.textBox1.Text = zcpl1.ToString();
                    this.textBox2.Text = zcpl2.ToString();
                    this.textBox3.Text = zcwd1.ToString();
                    this.textBox4.Text = zcwd2.ToString();
                    this.textBox5.Text = zcfy1.ToString();
                    this.textBox6.Text = zcfy2.ToString();
                    this.textBox7.Text = zcfl1.ToString();
                    this.textBox8.Text = zcfl2.ToString();
                    this.textBox9.Text = blhd.ToString();
                    this.textBox11.Text = zll.ToString();
                    this.textBox12.Text = yhjsl.ToString();
                    this.textBox13.Text = ehjsl.ToString();
                    this.textBox14.Text = hhlcw.ToString();
                    this.textBox15.Text = ygzs.ToString();
                    this.textBox16.Text = sjjjs.ToString();
                    this.textBox17.Text = hljjs.ToString();

                    //点火温度
                    float dhwd1 = float.Parse(dataTable1.Rows[0][16].ToString());
                    float dhwd2 = float.Parse(dataTable1.Rows[0][17].ToString());
                    float dhwd3 = float.Parse(dataTable1.Rows[0][18].ToString());
                    float a = 0;
                    int b = 0;
                    if (dhwd1 > 800 && dhwd1 < 1400)
                    {
                        a = a + dhwd1;
                        b = b + 1;
                    }
                    if (dhwd2 > 800 && dhwd2 < 1400)
                    {
                        a = a + dhwd2;
                        b = b + 1;
                    }
                    if (dhwd3 > 800 && dhwd2 < 1400)
                    {
                        a = a + dhwd3;
                        b = b + 1;
                    }
                    float c = 0;
                    c = float.Parse((a / b).ToString());
                    this.textBox10.Text = c.ToString();
                    //终点位置
                    string sql2 = "select top 1 ISNULL(BTPCAL_OUT_TOTAL_AVG_X_BTP,0) from MC_BTPCAL_result_1min order by TIMESTAMP desc";
                    DataTable dataTable2 = dBSQL.GetCommand(sql2);
                    if (dataTable2.Rows.Count > 0)
                    {
                        float zdwz = float.Parse(dataTable2.Rows[0][0].ToString());
                        this.textBox18.Text = zdwz.ToString();
                        try
                        {
                            //超过上限变红，低于下限变黄
                            string sql4 = "select ISNULL(PAR_MA_FAN_SP_MAX,0),ISNULL(PAR_MA_FAN_SP_MIN,0),ISNULL(PAR_MA_SB_FLUE_TE_MAX,0),ISNULL(PAR_MA_SB_FLUE_TE_MIN,0),ISNULL(PAR_MA_SB_FLUE_PT_MAX,0),ISNULL(PAR_MA_SB_FLUE_PT_MIN,0)," +
                                "ISNULL(PAR_MA_SB_FLUE_FT_MAX,0),ISNULL(PAR_MA_SB_FLUE_FT_MIN,0),ISNULL(PAR_X_BTP_MAX,0),ISNULL(PAR_X_BTP_MIN,0),ISNULL(PAR_THICK_PV_MAX,0),ISNULL(PAR_THICK_PV_MIN,0),ISNULL(PAR_IG_TE_MAX,0)," +
                                "ISNULL(PAR_IG_TE_MIN,0),ISNULL(PAR_TOTAL_SP_W_MAX,0),ISNULL(PAR_TOTAL_SP_W_MIN,0),ISNULL(PAR_1M_FT_SP_MAX,0),ISNULL(PAR_1M_FT_SP_MIN,0),ISNULL(PAR_2M_FLOW_SP_MAX,0),ISNULL(PAR_2M_FLOW_SP_MIN,0)," +
                                "ISNULL(PAR_BLEND_LEVEL_MAX,0),ISNULL(PAR_BLEND_LEVEL_MIN,0),ISNULL(PAR_STICK_SP_MAX,0),ISNULL(PAR_STICK_SP_MIN,0),ISNULL(PAR_SIN_MS_SP_MAX,0),ISNULL(PAR_SIN_MS_SP_MIN,0),ISNULL(PAR_RC_SPEED_SP_MAX,0),ISNULL(PAR_RC_SPEED_SP_MIN,0) from CFG_R_T_CURVE_INTERFACE_PAR";
                            DataTable dataTable4 = dBSQL.GetCommand(sql4);
                            if (dataTable4.Rows.Count > 0)
                            {
                                float zcplsx = float.Parse(dataTable4.Rows[0][0].ToString());
                                float zcplxx = float.Parse(dataTable4.Rows[0][1].ToString());
                                float zcwdsx = float.Parse(dataTable4.Rows[0][2].ToString());
                                float zcwdxx = float.Parse(dataTable4.Rows[0][3].ToString());
                                float zcfysx = float.Parse(dataTable4.Rows[0][4].ToString());
                                float zcfyxx = float.Parse(dataTable4.Rows[0][5].ToString());
                                float zcflsx = float.Parse(dataTable4.Rows[0][6].ToString());
                                float zcflxx = float.Parse(dataTable4.Rows[0][7].ToString());
                                float zdwzsx = float.Parse(dataTable4.Rows[0][8].ToString());
                                float zdwzxx = float.Parse(dataTable4.Rows[0][9].ToString());
                                float blhdsx = float.Parse(dataTable4.Rows[0][10].ToString());
                                float blhdxx = float.Parse(dataTable4.Rows[0][11].ToString());
                                float dhwdsx = float.Parse(dataTable4.Rows[0][12].ToString());
                                float dhwdxx = float.Parse(dataTable4.Rows[0][13].ToString());
                                float zllsx = float.Parse(dataTable4.Rows[0][14].ToString());
                                float zllxx = float.Parse(dataTable4.Rows[0][15].ToString());
                                float yhjslsx = float.Parse(dataTable4.Rows[0][16].ToString());
                                float yhjslxx = float.Parse(dataTable4.Rows[0][17].ToString());
                                float ehjslsx = float.Parse(dataTable4.Rows[0][18].ToString());
                                float ehjslxx = float.Parse(dataTable4.Rows[0][19].ToString());
                                float hhlcwsx = float.Parse(dataTable4.Rows[0][20].ToString());
                                float hhlcwxx = float.Parse(dataTable4.Rows[0][21].ToString());
                                float ygzssx = float.Parse(dataTable4.Rows[0][22].ToString());
                                float ygzsxx = float.Parse(dataTable4.Rows[0][23].ToString());
                                float sjjjssx = float.Parse(dataTable4.Rows[0][24].ToString());
                                float sjjjsxx = float.Parse(dataTable4.Rows[0][25].ToString());
                                float hljjssx = float.Parse(dataTable4.Rows[0][26].ToString());
                                float hljjsxx = float.Parse(dataTable4.Rows[0][27].ToString());

                                //1#主抽频率
                                if (zcpl1 > zcplsx)
                                {
                                    textBox1.BackColor = Color.Red;
                                }
                                else if (zcpl1 < zcplxx)
                                {
                                    textBox1.BackColor = Color.Yellow;
                                }
                                else textBox1.BackColor = Color.White;
                                //2#主抽频率
                                if (zcpl2 > zcplsx)
                                {
                                    textBox2.BackColor = Color.Red;
                                }
                                else if (zcpl2 < zcplxx)
                                {
                                    textBox2.BackColor = Color.Yellow;
                                }
                                else textBox2.BackColor = Color.White;
                                //1#主抽温度
                                if (zcwd1 > zcwdsx)
                                {
                                    textBox3.BackColor = Color.Red;
                                }
                                else if (zcwd1 < zcwdxx)
                                {
                                    textBox3.BackColor = Color.Yellow;
                                }
                                else textBox3.BackColor = Color.White;
                                //2#主抽温度
                                if (zcwd2 > zcwdsx)
                                {
                                    textBox4.BackColor = Color.Red;
                                }
                                else if (zcwd2 < zcwdxx)
                                {
                                    textBox4.BackColor = Color.Yellow;
                                }
                                else textBox4.BackColor = Color.White;
                                //1#主抽负压
                                if (zcfy1 > zcfysx)
                                {
                                    textBox5.BackColor = Color.Red;
                                }
                                else if (zcfy1 < zcfyxx)
                                {
                                    textBox5.BackColor = Color.Yellow;
                                }
                                else textBox5.BackColor = Color.White;
                                //2#主抽负压
                                if (zcfy2 > zcfysx)
                                {
                                    textBox6.BackColor = Color.Red;
                                }
                                else if (zcfy2 < zcfyxx)
                                {
                                    textBox6.BackColor = Color.Yellow;
                                }
                                else textBox6.BackColor = Color.White;
                                //1#主抽风量
                                if (zcfl1 > zcflsx)
                                {
                                    textBox7.BackColor = Color.Red;
                                }
                                else if (zcfl1 < zcflxx)
                                {
                                    textBox7.BackColor = Color.Yellow;
                                }
                                else textBox7.BackColor = Color.White;
                                //2#主抽风量
                                if (zcfl2 > zcflsx)
                                {
                                    textBox8.BackColor = Color.Red;
                                }
                                else if (zcfl2 < zcflxx)
                                {
                                    textBox8.BackColor = Color.Yellow;
                                }
                                else textBox8.BackColor = Color.White;
                                //终点位置
                                if (zdwz > zdwzsx)
                                {
                                    textBox18.BackColor = Color.Red;
                                }
                                else if (zdwz < zdwzxx)
                                {
                                    textBox18.BackColor = Color.Yellow;
                                }
                                else textBox18.BackColor = Color.White;
                                //布料厚度
                                if (blhd > blhdsx)
                                {
                                    textBox9.BackColor = Color.Red;
                                }
                                else if (blhd < blhdxx)
                                {
                                    textBox9.BackColor = Color.Yellow;
                                }
                                else textBox9.BackColor = Color.White;
                                //点火温度
                                if (c > dhwdsx)
                                {
                                    textBox10.BackColor = Color.Red;
                                }
                                else if (c < dhwdxx)
                                {
                                    textBox10.BackColor = Color.Yellow;
                                }
                                else textBox10.BackColor = Color.White;
                                //总料量
                                if (zll > zllsx)
                                {
                                    textBox11.BackColor = Color.Red;
                                }
                                else if (zll < zllxx)
                                {
                                    textBox11.BackColor = Color.Yellow;
                                }
                                else textBox11.BackColor = Color.White;
                                //一混加水量
                                if (yhjsl > yhjslsx)
                                {
                                    textBox12.BackColor = Color.Red;
                                }
                                else if (yhjsl < yhjslxx)
                                {
                                    textBox12.BackColor = Color.Yellow;
                                }
                                else textBox12.BackColor = Color.White;
                                //二混加水量
                                if (ehjsl > ehjslsx)
                                {
                                    textBox13.BackColor = Color.Red;
                                }
                                else if (ehjsl < ehjslxx)
                                {
                                    textBox13.BackColor = Color.Yellow;
                                }
                                else textBox13.BackColor = Color.White;
                                //混合料仓位
                                if (hhlcw > hhlcwsx)
                                {
                                    textBox14.BackColor = Color.Red;
                                }
                                else if (hhlcw < hhlcwxx)
                                {
                                    textBox14.BackColor = Color.Yellow;
                                }
                                else textBox14.BackColor = Color.White;
                                //圆辊转速
                                if (ygzs > ygzssx)
                                {
                                    textBox15.BackColor = Color.Red;
                                }
                                else if (ygzs < ygzsxx)
                                {
                                    textBox15.BackColor = Color.Yellow;
                                }
                                else textBox15.BackColor = Color.White;
                                //烧结机机速
                                if (sjjjs > sjjjssx)
                                {
                                    textBox16.BackColor = Color.Red;
                                }
                                else if (sjjjs < sjjjsxx)
                                {
                                    textBox16.BackColor = Color.Yellow;
                                }
                                else textBox16.BackColor = Color.White;
                                //环冷机机速
                                if (hljjs > hljjssx)
                                {
                                    textBox17.BackColor = Color.Red;
                                }
                                else if (hljjs < hljjsxx)
                                {
                                    textBox17.BackColor = Color.Yellow;
                                }
                                else textBox17.BackColor = Color.White;
                            }
                            else
                            { }
                        }
                        catch
                        {
                        }
                    }
                    else
                    { }
                }
                else
                { }
            }
            catch
            {
            }
        }

        //量程设定按钮
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            shishi_liangchengsheding form_display = new shishi_liangchengsheding();
            if (shishi_liangchengsheding.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
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
        private double max11, min11;
        private double max12, min12;
        private double max13, min13;
        private double max14, min14;
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

        //int sjd = int.Parse(comboBox1.Text);
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
            Line18.Clear();

            try
            {
                /*string a1 =comboBox1.SelectedText.ToString();
                string b =comboBox1.SelectedValue.ToString();
                string c=comboBox1.SelectedText.ToString();
                int sjd = int.Parse(comboBox1.Text.ToString());*/
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
                string sql1 = "select TIMESTAMP, T_PLC_MA_FAN_1_SP_3S,T_PLC_MA_FAN_2_SP_3S,T_MA_SB_1_FLUE_TE_3S,T_MA_SB_2_FLUE_TE_3S,T_MA_SB_1_FLUE_PT_3S,T_MA_SB_2_FLUE_PT_3S," +
                                "T_MA_SB_1_FLUE_FT_3S,T_MA_SB_2_FLUE_FT_3S,C_THICK_PV_3S,T_TOTAL_SP_W_3S,T_1M_FT_SP_3S,T_2M_FLOW_SP_3S,T_BLEND_LEVEL_3S,T_STICK_SP_3S,T_SIN_MS_SP_3S,T_RC_SPEED_SP_3S" +
                                " from C_PLC_3S where TIMESTAMP between '" + DateTime.Now.AddHours(-sjd) + "' and '" + DateTime.Now + "' order by TIMESTAMP";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    for (int a = 0; a < dataTable1.Rows.Count; a++)
                    {
                        string sj = dataTable1.Rows[a][0].ToString();
                        shijian.Add(sj);
                        DataPoint line1 = new DataPoint();
                        if (dataTable1.Rows[a][1].ToString() != "")
                        {
                            double zcpl_1 = double.Parse(dataTable1.Rows[a][1].ToString());
                            zcpl1.Add(zcpl_1);
                            line1 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][1]));
                            Num1.Add(Convert.ToDouble(dataTable1.Rows[a][1]));
                        }
                        else
                        {
                            zcpl1.Add(double.NaN);
                            line1 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][1]));
                        Line1.Add(line1);
                        //Num1.Add(Convert.ToDouble(dataTable1.Rows[a][1]));
                        DataPoint line2 = new DataPoint();
                        if (dataTable1.Rows[a][2].ToString() != "")
                        {
                            double zcpl_2 = double.Parse(dataTable1.Rows[a][2].ToString());
                            zcpl2.Add(zcpl_2);
                            line2 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][2]));
                            Num1.Add(Convert.ToDouble(dataTable1.Rows[a][2]));
                        }
                        else
                        {
                            zcpl2.Add(double.NaN);
                            line2 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][2]));
                        Line2.Add(line2);

                        DataPoint line3 = new DataPoint();
                        if (dataTable1.Rows[a][3].ToString() != "")
                        {
                            double zcwd_1 = double.Parse(dataTable1.Rows[a][3].ToString());
                            zcwd1.Add(zcwd_1);
                            line3 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][3]));
                            Num2.Add(Convert.ToDouble(dataTable1.Rows[a][3]));
                        }
                        else
                        {
                            zcwd1.Add(double.NaN);
                            line3 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line3 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][3]));
                        Line3.Add(line3);

                        DataPoint line4 = new DataPoint();
                        if (dataTable1.Rows[a][4].ToString() != "")
                        {
                            double zcwd_2 = double.Parse(dataTable1.Rows[a][4].ToString());
                            zcwd2.Add(zcwd_2);
                            line4 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][4]));
                            Num2.Add(Convert.ToDouble(dataTable1.Rows[a][4]));
                        }
                        else
                        {
                            zcwd2.Add(double.NaN);
                            line4 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line4 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][4]));
                        Line4.Add(line4);

                        DataPoint line5 = new DataPoint();
                        if (dataTable1.Rows[a][5].ToString() != "")
                        {
                            double zcfy_1 = double.Parse(dataTable1.Rows[a][5].ToString());
                            zcfy1.Add(zcfy_1);
                            line5 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][5]));
                            Num3.Add(Convert.ToDouble(dataTable1.Rows[a][5]));
                        }
                        else
                        {
                            zcfy1.Add(double.NaN);
                            line5 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line5 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][5]));
                        Line5.Add(line5);

                        DataPoint line6 = new DataPoint();
                        if (dataTable1.Rows[a][6].ToString() != "")
                        {
                            double zcfy_2 = double.Parse(dataTable1.Rows[a][6].ToString());
                            zcfy2.Add(zcfy_2);
                            line6 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][6]));
                            Num3.Add(Convert.ToDouble(dataTable1.Rows[a][6]));
                        }
                        else
                        {
                            zcfy2.Add(double.NaN);
                            line6 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line6 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][6]));
                        Line6.Add(line6);

                        DataPoint line7 = new DataPoint();
                        if (dataTable1.Rows[a][7].ToString() != "")
                        {
                            double zcfl_1 = double.Parse(dataTable1.Rows[a][7].ToString());
                            zcfl1.Add(zcfl_1);
                            line7 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][7]));
                            Num4.Add(Convert.ToDouble(dataTable1.Rows[a][7]));
                        }
                        else
                        {
                            zcfl1.Add(double.NaN);
                            line7 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line7 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][7]));
                        Line7.Add(line7);

                        DataPoint line8 = new DataPoint();
                        if (dataTable1.Rows[a][8].ToString() != "")
                        {
                            double zcfl_2 = double.Parse(dataTable1.Rows[a][8].ToString());
                            zcfl2.Add(zcfl_2);
                            line8 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][8]));
                            Num4.Add(Convert.ToDouble(dataTable1.Rows[a][8]));
                        }
                        else
                        {
                            zcfl2.Add(double.NaN);
                            line8 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line8 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][8]));
                        Line8.Add(line8);

                        DataPoint line10 = new DataPoint();
                        if (dataTable1.Rows[a][9].ToString() != "")
                        {
                            double blhd_1 = double.Parse(dataTable1.Rows[a][9].ToString());
                            blhd.Add(blhd_1);
                            line10 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][9]));
                            Num6.Add(Convert.ToDouble(dataTable1.Rows[a][9]));
                        }
                        else
                        {
                            blhd.Add(double.NaN);
                            line10 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line10 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][9]));
                        Line10.Add(line10);

                        DataPoint line12 = new DataPoint();
                        if (dataTable1.Rows[a][10].ToString() != "")
                        {
                            double zll_1 = double.Parse(dataTable1.Rows[a][10].ToString());
                            zll.Add(zll_1);
                            line12 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][10]));
                            Num8.Add(Convert.ToDouble(dataTable1.Rows[a][10]));
                        }
                        else
                        {
                            zll.Add(double.NaN);
                            line12 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line12 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][10]));
                        Line12.Add(line12);

                        DataPoint line13 = new DataPoint();
                        if (dataTable1.Rows[a][11].ToString() != "")
                        {
                            double yhjsl_1 = double.Parse(dataTable1.Rows[a][11].ToString());
                            yhjsl.Add(yhjsl_1);
                            line13 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][11]));
                            Num9.Add(Convert.ToDouble(dataTable1.Rows[a][11]));
                        }
                        else
                        {
                            yhjsl.Add(double.NaN);
                            line13 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line13 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][11]));
                        Line13.Add(line13);

                        DataPoint line14 = new DataPoint();
                        if (dataTable1.Rows[a][12].ToString() != "")
                        {
                            double ehjsl_1 = double.Parse(dataTable1.Rows[a][12].ToString());
                            ehjsl.Add(ehjsl_1);
                            line14 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][12]));
                            Num10.Add(Convert.ToDouble(dataTable1.Rows[a][12]));
                        }
                        else
                        {
                            ehjsl.Add(double.NaN);
                            line14 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line14 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][12]));
                        Line14.Add(line14);

                        DataPoint line15 = new DataPoint();
                        if (dataTable1.Rows[a][13].ToString() != "")
                        {
                            double hhlcw_1 = double.Parse(dataTable1.Rows[a][13].ToString());
                            hhlcw.Add(hhlcw_1);
                            line15 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][13]));
                            Num11.Add(Convert.ToDouble(dataTable1.Rows[a][13]));
                        }
                        else
                        {
                            hhlcw.Add(double.NaN);
                            line15 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line15 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][13]));
                        Line15.Add(line15);

                        max11 = (int)Num11.Max() + 1;
                        min11 = (int)Num11.Min();
                        DataPoint line16 = new DataPoint();
                        if (dataTable1.Rows[a][14].ToString() != "")
                        {
                            double ygzs_1 = double.Parse(dataTable1.Rows[a][14].ToString());
                            ygzs.Add(ygzs_1);
                            line16 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][14]));
                            Num12.Add(Convert.ToDouble(dataTable1.Rows[a][14]));
                        }
                        else
                        {
                            ygzs.Add(double.NaN);
                            line16 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), double.NaN);
                        }
                        //DataPoint line16 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][14]));
                        Line16.Add(line16);
                    }

                    //烧结机机速
                    var sql_2 = "select TIMESTAMP, F_PLC_SIN_SPEED_PV from C_MFI_PLC_1MIN where TIMESTAMP between '" + DateTime.Now.AddHours(-sjd) + "' and '" + DateTime.Now + "' order by TIMESTAMP";
                    DataTable data_1 = dBSQL.GetCommand(sql_2);
                    if (data_1.Rows.Count > 0)
                    {
                        for (int a = 0; a < data_1.Rows.Count; a++)
                        {
                            DataPoint line17 = new DataPoint();
                            if (data_1.Rows[a][1].ToString() != "")
                            {
                                double sjjs_1 = double.Parse(data_1.Rows[a][1].ToString());
                                sjjs.Add(sjjs_1);
                                line17 = new DataPoint(DateTimeAxis.ToDouble(data_1.Rows[a][0]), Convert.ToDouble(data_1.Rows[a][1]));
                                Num13.Add(Convert.ToDouble(data_1.Rows[a][1]));
                            }
                            else
                            {
                                sjjs.Add(double.NaN);
                                line17 = new DataPoint(DateTimeAxis.ToDouble(data_1.Rows[a][0]), double.NaN);
                            }
                            //DataPoint line17 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][15]));
                            Line17.Add(line17);
                        }
                    }
                    //环冷机机速
                    var sql_3 = "select TIMESTAMP, CFP_PLC_RC_SPEED_PV from C_CFP_PLC_1MIN where TIMESTAMP between '" + DateTime.Now.AddHours(-sjd) + "' and '" + DateTime.Now + "' order by TIMESTAMP";
                    DataTable data_2 = dBSQL.GetCommand(sql_3);
                    if (data_2.Rows.Count > 0)
                    {
                        for (int a = 0; a < data_2.Rows.Count; a++)
                        {
                            DataPoint line18 = new DataPoint();
                            if (data_2.Rows[a][1].ToString() != "")
                            {
                                double hljs_1 = double.Parse(data_2.Rows[a][1].ToString());
                                hljs.Add(hljs_1);
                                line18 = new DataPoint(DateTimeAxis.ToDouble(data_2.Rows[a][0]), Convert.ToDouble(data_2.Rows[a][1]));
                                Num14.Add(Convert.ToDouble(data_2.Rows[a][1]));
                            }
                            else
                            {
                                hljs.Add(double.NaN);
                                line18 = new DataPoint(DateTimeAxis.ToDouble(data_2.Rows[a][0]), double.NaN);
                            }
                            //DataPoint line18 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[a][0]), Convert.ToDouble(dataTable1.Rows[a][16]));
                            Line18.Add(line18);
                        }
                    }
                    string sql6 = "select TIMESTAMP,ISNULL(T_IG_01_TE_3S,0),ISNULL(T_IG_02_TE_3S,0),ISNULL(T_IG_03_TE_3S,0)" +
                                      " from C_PLC_3S where TIMESTAMP between '" + DateTime.Now.AddHours(-sjd) + "' and '" + DateTime.Now + "' order by TIMESTAMP";
                    DataTable dataTable6 = dBSQL.GetCommand(sql6);
                    for (int i = 0; i < dataTable6.Rows.Count; i++)
                    {
                        double dhwd1 = double.Parse(dataTable6.Rows[i][1].ToString());
                        double dhwd2 = double.Parse(dataTable6.Rows[i][2].ToString());
                        double dhwd3 = double.Parse(dataTable6.Rows[i][3].ToString());
                        double a = 0;
                        int b = 0;
                        if (dhwd1 > 800 && dhwd1 < 1400)
                        {
                            a = a + dhwd1;
                            b = b + 1;
                        }

                        if (dhwd2 > 800 && dhwd2 < 1400)
                        {
                            a = a + dhwd2;
                            b = b + 1;
                        }
                        if (dhwd3 > 800 && dhwd2 < 1400)
                        {
                            a = a + dhwd3;
                            b = b + 1;
                        }
                        float c = 0;
                        if (b != 0)
                        {
                            c = float.Parse((a / b).ToString());
                        }
                        //c = float.Parse((a / b).ToString());
                        dhwd.Add(c);
                        DataPoint line11 = new DataPoint(DateTimeAxis.ToDouble(dataTable1.Rows[i]["TIMESTAMP"]), Convert.ToDouble(c));
                        Line11.Add(line11);

                        Num7.Add(c);
                    }
                    string sql_sj = "select TIMESTAMP,BTPCAL_OUT_TOTAL_AVG_X_BTP from MC_BTPCAL_result_1min where TIMESTAMP between '" + DateTime.Now.AddHours(-sjd) + "' and '" + DateTime.Now + "' order by TIMESTAMP";
                    DataTable dataTable_sj = dBSQL.GetCommand(sql_sj);
                    if (dataTable_sj.Rows.Count > 0)
                    {
                        for (int a = 0; a < dataTable_sj.Rows.Count; a++)
                        {
                            string sj = dataTable_sj.Rows[a][0].ToString();
                            shijian1.Add(sj);
                            DataPoint line9 = new DataPoint();
                            if (dataTable_sj.Rows[a][1].ToString() != "")
                            {
                                double zdwz_1 = double.Parse(dataTable_sj.Rows[a][1].ToString());
                                zdwz.Add(zdwz_1);
                                line9 = new DataPoint(DateTimeAxis.ToDouble(dataTable_sj.Rows[a][0]), Convert.ToDouble(dataTable_sj.Rows[a][1]));
                            }
                            else
                            {
                                zdwz.Add(double.NaN);
                                line9 = new DataPoint(DateTimeAxis.ToDouble(dataTable_sj.Rows[a][0]), double.NaN);
                            }
                            //DataPoint line9 = new DataPoint(DateTimeAxis.ToDouble(dataTable_sj.Rows[a][0]), Convert.ToDouble(dataTable_sj.Rows[a][1]));
                            Line9.Add(line9);
                            Num5.Add(Convert.ToDouble(dataTable_sj.Rows[a][1]));
                        }
                        max5 = (int)Num5.Max() + 1;
                        min5 = (int)Num5.Min();
                    }
                    max7 = (int)Num7.Max() + 1;
                    min7 = (int)Num7.Min();
                    max1 = (Num1.Count == 0) ? 0 : (int)Num1.Max() + 1;
                    min1 = (Num1.Count == 0) ? 0 : (int)Num1.Min() - 1;
                    max2 = (Num2.Count == 0) ? 0 : (int)Num2.Max() + 1;
                    min2 = (Num2.Count == 0) ? 0 : (int)Num2.Min() - 1;
                    max3 = (Num3.Count == 0) ? 0 : (int)Num3.Max() + 1;
                    min3 = (Num3.Count == 0) ? 0 : (int)Num3.Min() - 1;
                    max4 = (Num4.Count == 0) ? 0 : (int)Num4.Max() + 1;
                    min4 = (Num4.Count == 0) ? 0 : (int)Num4.Min() - 1;
                    max6 = (Num6.Count == 0) ? 0 : (int)Num6.Max() + 1;
                    min6 = (Num6.Count == 0) ? 0 : (int)Num6.Min() - 1;
                    max8 = (Num8.Count == 0) ? 0 : (int)Num8.Max() + 1;
                    min8 = (Num8.Count == 0) ? 0 : (int)Num8.Min() - 1;
                    max9 = (Num9.Count == 0) ? 0 : (int)Num9.Max() + 1;
                    min9 = (Num9.Count == 0) ? 0 : (int)Num9.Min() - 1;
                    max10 = (Num10.Count == 0) ? 0 : (int)Num10.Max() + 1;
                    min10 = (Num10.Count == 0) ? 0 : (int)Num10.Min() - 1;
                    max12 = (Num12.Count == 0) ? 0 : (int)Num12.Max() + 1;
                    min12 = (Num12.Count == 0) ? 0 : (int)Num12.Min() - 1;
                    max13 = (Num13.Count == 0) ? 0 : (int)Num13.Max() + 1;
                    min13 = (Num13.Count == 0) ? 0 : (int)Num13.Min() - 1;
                    max14 = (Num14.Count == 0) ? 0 : (int)Num14.Max() + 1;
                    min14 = (Num14.Count == 0) ? 0 : (int)Num14.Min() - 1;
                }
                else
                {
                    string sj = DateTime.Now.ToString();
                    shijian.Add(sj);
                    zcpl1.Add(0);
                    zcpl2.Add(0);
                    zcwd1.Add(0);
                    zcwd2.Add(0);
                    zcfy1.Add(0);
                    zcfy2.Add(0);
                    zcfl1.Add(0);
                    zcfl2.Add(0);
                    blhd.Add(0);
                    dhwd.Add(0);
                    zll.Add(0);
                    yhjsl.Add(0);
                    ehjsl.Add(0);
                    hhlcw.Add(0);
                    ygzs.Add(0);
                    sjjs.Add(0);
                    hljs.Add(0);
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

        //定义曲线和坐标轴
        public void quxian()
        {
            _myPlotModel = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
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
                Key = "主轴频率",
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
                Minimum = min1,
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
                YAxisKey = "主轴频率",
                ItemsSource = Line1,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n1#主轴频率:{4}",
            };
            _myPlotModel.Series.Add(checkBox1_1);
            //plotView1.Model = _myPlotModel;
            //2#主轴频率
            /*_myPlotMode2 = new PlotModel()
            {
                Background = OxyColors.White,
                *//*Title = "2#主轴频率",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*//*
            };
            _dateAxis2 = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 100,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                AxisTickToLabelDistance = 0,
                FontSize = 5.0,
            };
            _myPlotMode2.Axes.Add(_dateAxis2);
            _valueAxis2 = new LinearAxis()
            {
                Key = "2#主轴频率",
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
            _myPlotModel.Axes.Add(_valueAxis2);*/
            checkBox2_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Purple,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "主轴频率",
                ItemsSource = Line2,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n2#主轴频率:{4}",
            };
            _myPlotModel.Series.Add(checkBox2_1);
            plotView1.Model = _myPlotModel;
            //1#主轴温度
            _myPlotMode3 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "1#主轴温度",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
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
                Key = "主轴温度",
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
                Maximum = max2,
                Minimum = min2,
                //StartPosition = 0.5,
            };
            if (min2 == max2 && min2 == 0)
            {
            }
            else
            {
                if (min2 == 0)
                {
                    _valueAxis3.MajorStep = max2;
                }
                else
                {
                    _valueAxis3.MajorStep = (max2 - min2) / 2;
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
                YAxisKey = "主轴温度",
                ItemsSource = Line3,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n1#主轴温度:{4}℃",
            };
            _myPlotMode3.Series.Add(checkBox3_1);
            /*plotView3.Model = _myPlotMode3;
            //2#主轴温度
            _myPlotMode4 = new PlotModel()
            {
                Background = OxyColors.White,
                *//*Title = "2#主轴温度",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*//*
            };
            _dateAxis4 = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 100,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                AxisTickToLabelDistance = 0,
                FontSize = 5.0,
            };
            _myPlotMode4.Axes.Add(_dateAxis4);
            _valueAxis4 = new LinearAxis()
            {
                Key = "2#主轴温度",
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
            _myPlotMode3.Axes.Add(_valueAxis4);*/
            checkBox4_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Blue,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "主轴温度",
                ItemsSource = Line4,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n2#主轴温度:{4}℃",
            };
            _myPlotMode3.Series.Add(checkBox4_1);
            plotView2.Model = _myPlotMode3;
            //1#主轴负压
            _myPlotMode5 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "1#主轴负压",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
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
                Key = "主轴负压",
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
                Minimum = min3,
                //StartPosition = 0.5,
            };
            //_valueAxis5.Maximum = getMax((int)max3,(int)min3);
            if (min3 == max3 && min3 == 0)
            {
            }
            else
            {
                if (min3 == 0)
                {
                    _valueAxis5.MajorStep = max3;
                }
                else
                {
                    _valueAxis5.MajorStep = (max3 - min3) / 2;
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
                YAxisKey = "主轴负压",
                ItemsSource = Line5,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n1#主轴负压:{4}",
            };
            _myPlotMode5.Series.Add(checkBox5_1);
            //plotView5.Model = _myPlotMode5;
            //2#主轴负压
            /*_myPlotMode6 = new PlotModel()
            {
                Background = OxyColors.White,
                *//*Title = "2#主轴负压",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*//*
            };
            _dateAxis6 = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 100,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                AxisTickToLabelDistance = 0,
                FontSize = 5.0,
            };
            _myPlotMode6.Axes.Add(_dateAxis6);
            _valueAxis6 = new LinearAxis()
            {
                Key = "主轴负压",
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
            _myPlotMode5.Axes.Add(_valueAxis6);*/
            checkBox6_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Black,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "主轴负压",
                ItemsSource = Line6,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n2#主轴负压:{4}",
            };
            _myPlotMode5.Series.Add(checkBox6_1);
            plotView3.Model = _myPlotMode5;
            //1#主轴风量
            _myPlotMode7 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "1#主轴风量",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
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
                Key = "主轴风量",
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
                Maximum = max4,
                Minimum = min4,
                //StartPosition = 0.2,
            };
            if (min4 == max4 && min4 == 0)
            {
            }
            else
            {
                if (min4 == 0)
                {
                    _valueAxis7.MajorStep = max4;
                }
                else
                {
                    _valueAxis7.MajorStep = (max4 - min4) / 2;
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
                YAxisKey = "主轴风量",
                ItemsSource = Line7,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n1#主轴风量:{4}",
            };
            _myPlotMode7.Series.Add(checkBox7_1);
            //plotView7.Model = _myPlotMode7;
            //2#主轴风量
            /*_myPlotMode8 = new PlotModel()
            {
                Background = OxyColors.White,
                *//*Title = "2#主轴风量",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*//*
            };
            _dateAxis8 = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 100,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                AxisTickToLabelDistance = 0,
                FontSize = 5.0,
            };
            _myPlotMode8.Axes.Add(_dateAxis8);
            _valueAxis8 = new LinearAxis()
            {
                Key = "主轴风量",
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
            _myPlotMode7.Axes.Add(_valueAxis8);*/
            checkBox8_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Fuchsia,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "主轴风量",
                ItemsSource = Line8,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n2#主轴风量:{4}",
            };
            _myPlotMode7.Series.Add(checkBox8_1);
            plotView4.Model = _myPlotMode7;
            //终点位置
            _myPlotMode9 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "终点位置",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
            };
            _dateAxis9 = new DateTimeAxis()
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
            _myPlotMode9.Axes.Add(_dateAxis9);
            _valueAxis9 = new LinearAxis()
            {
                Key = "终点位置",
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
                Maximum = max5,
                Minimum = min5,
                //StartPosition = 0.2,
            };
            if (min5 == max5 && min5 == 0)
            {
            }
            else
            {
                if (min5 == 0)
                {
                    _valueAxis9.MajorStep = max5;
                }
                else
                {
                    _valueAxis9.MajorStep = (max5 - min5) / 2;
                }
            }

            _myPlotMode9.Axes.Add(_valueAxis9);
            checkBox9_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Olive,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "终点位置",
                ItemsSource = Line9,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n终点位置:{4}",
            };
            _myPlotMode9.Series.Add(checkBox9_1);
            plotView5.Model = _myPlotMode9;
            //布料厚度
            _myPlotModel0 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "布料厚度",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
            };
            _dateAxis10 = new DateTimeAxis()
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
            _myPlotModel0.Axes.Add(_dateAxis10);
            _valueAxis10 = new LinearAxis()
            {
                Key = "布料厚度",
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
                Maximum = max6,
                Minimum = min6,
                //StartPosition = 0.2,
            };
            if (min6 == max6 && min6 == 0)
            {
            }
            else
            {
                if (min6 == 0)
                {
                    _valueAxis10.MajorStep = max6;
                }
                else
                {
                    _valueAxis10.MajorStep = (max6 - min6) / 2;
                }
            }

            _myPlotModel0.Axes.Add(_valueAxis10);
            checkBox10_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.SlateGray,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "布料厚度",
                ItemsSource = Line10,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n布料厚度:{4}",
            };
            _myPlotModel0.Series.Add(checkBox10_1);
            plotView6.Model = _myPlotModel0;
            //点火温度
            _myPlotModel1 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "点火温度",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
            };
            _dateAxis11 = new DateTimeAxis()
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
            _myPlotModel1.Axes.Add(_dateAxis11);
            _valueAxis11 = new LinearAxis()
            {
                Key = "点火温度",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Aqua,
                MinorTicklineColor = OxyColors.Aqua,
                TicklineColor = OxyColors.Aqua,
                TextColor = OxyColors.Aqua,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                Maximum = max7,
                Minimum = min7,
                //StartPosition = 0.2,
            };
            if (min7 == max7 && min7 == 0)
            {
            }
            else
            {
                if (min7 == 0)
                {
                    _valueAxis11.MajorStep = max7;
                }
                else
                {
                    _valueAxis11.MajorStep = (max7 - min7) / 2;
                }
            }

            _myPlotModel1.Axes.Add(_valueAxis11);
            checkBox11_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Aqua,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "点火温度",
                ItemsSource = Line11,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n点火温度:{4}",
            };
            _myPlotModel1.Series.Add(checkBox11_1);
            plotView7.Model = _myPlotModel1;
            //总料量
            _myPlotModel2 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "总料量",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
            };
            _dateAxis12 = new DateTimeAxis()
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
            _myPlotModel2.Axes.Add(_dateAxis12);
            _valueAxis12 = new LinearAxis()
            {
                Key = "总料量",
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
                Maximum = max8,
                Minimum = min8,
                //StartPosition = 0.2,
            };
            if (min8 == max8 && min8 == 0)
            {
            }
            else
            {
                if (min8 == 0)
                {
                    _valueAxis12.MajorStep = max8;
                }
                else
                {
                    _valueAxis12.MajorStep = (max8 - min8) / 2;
                }
            }

            _myPlotModel2.Axes.Add(_valueAxis12);
            checkBox12_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.SaddleBrown,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "总料量",
                ItemsSource = Line12,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n总料量:{4}",
            };
            _myPlotModel2.Series.Add(checkBox12_1);
            plotView8.Model = _myPlotModel2;
            //一混加水
            _myPlotModel3 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "一混加水",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
            };
            _dateAxis13 = new DateTimeAxis()
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
            _myPlotModel3.Axes.Add(_dateAxis13);
            _valueAxis13 = new LinearAxis()
            {
                Key = "一混加水",
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
                Maximum = max9,
                Minimum = min9,
                //MajorStep=10,
                //StartPosition = 0.2,
            };
            if (min9 == max9 && min9 == 0)
            {
            }
            else
            {
                if (min9 == 0)
                {
                    _valueAxis13.MajorStep = max9;
                }
                else
                {
                    _valueAxis13.MajorStep = (max9 - min9) / 2;
                }
            }
            /*if (min9 == 0)
            {
                _valueAxis13.MajorStep = max9;
            }
            else
            {
                _valueAxis13.MajorStep = max9 - min9;
            }*/
            _myPlotModel3.Axes.Add(_valueAxis13);
            checkBox13_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.OrangeRed,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "一混加水",
                ItemsSource = Line13,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n一混加水:{4}",
            };
            _myPlotModel3.Series.Add(checkBox13_1);
            plotView9.Model = _myPlotModel3;
            //二混加水
            _myPlotModel4 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "一混加水",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
            };
            _dateAxis14 = new DateTimeAxis()
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
            _myPlotModel4.Axes.Add(_dateAxis14);
            _valueAxis14 = new LinearAxis()
            {
                Key = "二混加水",
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
                Maximum = max10,
                Minimum = min10,
                //MajorStep=1,
                //StartPosition = 0.2,
            };
            if (min10 == max10 && min10 == 0)
            {
            }
            else
            {
                if (min10 == 0)
                {
                    _valueAxis14.MajorStep = max10;
                }
                else
                {
                    _valueAxis14.MajorStep = (max10 - min10) / 2;
                }
            }
            /*if (min10 == 0)
            {
                _valueAxis14.MajorStep = max10;
            }
            else
            {
                _valueAxis14.MajorStep = max10 - min10;
            }*/
            _myPlotModel4.Axes.Add(_valueAxis14);
            checkBox14_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.RoyalBlue,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "二混加水",
                ItemsSource = Line14,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n二混加水:{4}",
            };
            _myPlotModel4.Series.Add(checkBox14_1);
            plotView10.Model = _myPlotModel4;
            //混合料仓
            _myPlotModel5 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "混合料仓",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
            };
            _dateAxis15 = new DateTimeAxis()
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
            _myPlotModel5.Axes.Add(_dateAxis15);
            _valueAxis15 = new LinearAxis()
            {
                Key = "混合料仓",
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
                Maximum = max11,
                Minimum = min11,
                //MajorStep=17,
            };
            if (min11 == max11 && min11 == 0)
            {
            }
            else
            {
                if (min11 == 0)
                {
                    _valueAxis15.MajorStep = max11;
                }
                else
                {
                    _valueAxis15.MajorStep = (max11 - min11) / 2;
                }
            }
            /*if (min11 == 0)
            {
                _valueAxis15.MajorStep = max11;
            }
            else
            {
                _valueAxis15.MajorStep = max11 - min11;
            }*/
            _myPlotModel5.Axes.Add(_valueAxis15);
            checkBox15_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Tomato,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "混合料仓",
                ItemsSource = Line15,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n混合料仓:{4}",
            };
            _myPlotModel5.Series.Add(checkBox15_1);
            plotView11.Model = _myPlotModel5;
            //圆辊转速
            _myPlotModel6 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "圆辊转速",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
            };
            _dateAxis16 = new DateTimeAxis()
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
            _myPlotModel6.Axes.Add(_dateAxis16);
            _valueAxis16 = new LinearAxis()
            {
                Key = "圆辊转速",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.YellowGreen,
                MinorTicklineColor = OxyColors.YellowGreen,
                TicklineColor = OxyColors.YellowGreen,
                TextColor = OxyColors.YellowGreen,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                Maximum = max12,
                Minimum = min12,
                //MajorStep=1,
            };
            if (min12 == max12 && min12 == 0)
            {
            }
            else
            {
                if (min12 == 0)
                {
                    _valueAxis16.MajorStep = max12;
                }
                else
                {
                    _valueAxis16.MajorStep = (max12 - min12) / 2;
                }
            }

            _myPlotModel6.Axes.Add(_valueAxis16);
            checkBox16_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.YellowGreen,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "圆辊转速",
                ItemsSource = Line16,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n圆辊转速:{4}",
            };
            _myPlotModel6.Series.Add(checkBox16_1);
            plotView12.Model = _myPlotModel6;
            //烧结机机速
            _myPlotModel7 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 0),
                /*Title = "烧结机机速",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
            };
            _dateAxis17 = new DateTimeAxis()
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
            _myPlotModel7.Axes.Add(_dateAxis17);
            _valueAxis17 = new LinearAxis()
            {
                Key = "烧结机机速",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.DarkTurquoise,
                MinorTicklineColor = OxyColors.DarkTurquoise,
                TicklineColor = OxyColors.DarkTurquoise,
                TextColor = OxyColors.DarkTurquoise,
                FontSize = 9.0,
                IsAxisVisible = true,
                MinorTickSize = 0,
                Maximum = max13,
                Minimum = min13,
                //MajorStep=1,
                //StartPosition = 0.2,
            };
            if (min13 == max13 && min13 == 0)
            {
            }
            else
            {
                if (min13 == 0)
                {
                    _valueAxis17.MajorStep = max13;
                }
                else
                {
                    _valueAxis17.MajorStep = (max13 - min13) / 2;
                }
            }
            /*if (min13 == 0)
            {
                _valueAxis17.MajorStep = max13;
            }
            else
            {
                _valueAxis17.MajorStep = max13 - min13;
            }*/
            _myPlotModel7.Axes.Add(_valueAxis17);
            checkBox17_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.DarkTurquoise,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "烧结机机速",
                ItemsSource = Line17,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n烧结机机速:{4}",
            };
            _myPlotModel7.Series.Add(checkBox17_1);
            plotView13.Model = _myPlotModel7;
            //环冷机机速
            _myPlotModel8 = new PlotModel()
            {
                Background = OxyColors.DarkGray,
                PlotAreaBorderColor = OxyColors.DarkGray,
                PlotMargins = new OxyThickness(40, 0, 5, 10),
                /*Title = "环冷机机速",
                TitleFontSize = 5,
                TitleColor = OxyColors.White,*/
            };
            _dateAxis18 = new DateTimeAxis()
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
            _myPlotModel8.Axes.Add(_dateAxis18);
            _valueAxis18 = new LinearAxis()
            {
                Key = "环冷机机速",
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
                Maximum = max14,
                Minimum = min14,
                //MajorStep=1,
                //StartPosition = 0.2,
            };
            if (min14 == max14 && min14 == 0)
            {
            }
            else
            {
                if (min14 == 0)
                {
                    _valueAxis18.MajorStep = max14;
                }
                else
                {
                    _valueAxis18.MajorStep = (max14 - min14) / 2;
                }
            }
            /*f (min14 == 0)
            {
                _valueAxis18.MajorStep = max14;
            }
            else
            {
                _valueAxis18.MajorStep = max14 - min14;
            }*/
            _myPlotModel8.Axes.Add(_valueAxis18);
            checkBox18_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.DarkViolet,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "环冷机机速",
                ItemsSource = Line18,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n环冷机机速:{4}",
            };
            _myPlotModel8.Series.Add(checkBox18_1);
            plotView14.Model = _myPlotModel8;
        }

        //单击隐藏曲线
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView1.Model = null;
                if (checkBox1.Checked == true)
                {
                    //_valueAxis1.IsAxisVisible = true;
                    _myPlotModel.Series.Add(checkBox1_1);
                }
                if (checkBox1.Checked == false)
                {
                    //_valueAxis1.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(checkBox1_1);
                }
                plotView1.Model = _myPlotModel;
                //tableLayoutPanel23_label();
                //lChartPlus40.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView1.Model = null;
                if (checkBox2.Checked == true)
                {
                    //_valueAxis1.IsAxisVisible = true;
                    _myPlotModel.Series.Add(checkBox2_1);
                }
                if (checkBox2.Checked == false)
                {
                    //_valueAxis2.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(checkBox2_1);
                }
                plotView1.Model = _myPlotModel;
                //tableLayoutPanel23_label();
                //lChartPlus40.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView2.Model = null;
                if (checkBox3.Checked == true)
                {
                    //_valueAxis3.IsAxisVisible = true;
                    _myPlotMode3.Series.Add(checkBox3_1);
                }
                if (checkBox3.Checked == false)
                {
                    //_valueAxis3.IsAxisVisible = false;
                    _myPlotMode3.Series.Remove(checkBox3_1);
                }
                plotView2.Model = _myPlotMode3;
                //lChartPlus41.ToggleCheckBoxY(sender, e, 0);
                //tableLayoutPanel23_label();
            }
            catch
            { }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView2.Model = null;
                if (checkBox4.Checked == true)
                {
                    //_valueAxis3.IsAxisVisible = true;
                    _myPlotMode3.Series.Add(checkBox4_1);
                }
                if (checkBox4.Checked == false)
                {
                    //_valueAxis3.IsAxisVisible = false;
                    _myPlotMode3.Series.Remove(checkBox4_1);
                }
                plotView2.Model = _myPlotMode3;
                //lChartPlus41.ToggleCheckBoxY(sender, e, 0);
                //tableLayoutPanel23_label();
            }
            catch
            { }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView3.Model = null;
                if (checkBox5.Checked == true)
                {
                    //_valueAxis5.IsAxisVisible = true;
                    _myPlotMode5.Series.Add(checkBox5_1);
                }
                if (checkBox5.Checked == false)
                {
                    //_valueAxis5.IsAxisVisible = false;
                    _myPlotMode5.Series.Remove(checkBox5_1);
                }
                plotView3.Model = _myPlotMode5;
                //lChartPlus42.ToggleCheckBoxY(sender, e, 0);
                //tableLayoutPanel23_label();
            }
            catch
            { }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView3.Model = null;
                if (checkBox6.Checked == true)
                {
                    //_valueAxis5.IsAxisVisible = true;
                    _myPlotMode5.Series.Add(checkBox6_1);
                }
                if (checkBox6.Checked == false)
                {
                    //_valueAxis5.IsAxisVisible = false;
                    _myPlotMode5.Series.Remove(checkBox6_1);
                }
                plotView3.Model = _myPlotMode5;
                //tableLayoutPanel23_label();
                //lChartPlus42.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView4.Model = null;
                if (checkBox7.Checked == true)
                {
                    //_valueAxis7.IsAxisVisible = true;
                    _myPlotMode7.Series.Add(checkBox7_1);
                }
                if (checkBox7.Checked == false)
                {
                    //_valueAxis7.IsAxisVisible = false;
                    _myPlotMode7.Series.Remove(checkBox7_1);
                }
                plotView4.Model = _myPlotMode7;
                //tableLayoutPanel23_label();
                //lChartPlus43.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView4.Model = null;
                if (checkBox8.Checked == true)
                {
                    //_valueAxis7.IsAxisVisible = true;
                    _myPlotMode7.Series.Add(checkBox8_1);
                }
                if (checkBox8.Checked == false)
                {
                    //_valueAxis7.IsAxisVisible = false;
                    _myPlotMode7.Series.Remove(checkBox8_1);
                }
                plotView4.Model = _myPlotMode7;
                //tableLayoutPanel23_label();
                //lChartPlus43.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView5.Model = null;
                if (checkBox18.Checked == true)
                {
                    //_valueAxis9.IsAxisVisible = true;
                    _myPlotMode9.Series.Add(checkBox9_1);
                }
                if (checkBox18.Checked == false)
                {
                    //_valueAxis9.IsAxisVisible = false;
                    _myPlotMode9.Series.Remove(checkBox9_1);
                }
                plotView5.Model = _myPlotMode9;
                //tableLayoutPanel23_label();
                //lChartPlus44.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView6.Model = null;
                if (checkBox9.Checked == true)
                {
                    //_valueAxis10.IsAxisVisible = true;
                    _myPlotModel0.Series.Add(checkBox10_1);
                }
                if (checkBox9.Checked == false)
                {
                    //_valueAxis10.IsAxisVisible = false;
                    _myPlotModel0.Series.Remove(checkBox10_1);
                }
                plotView6.Model = _myPlotModel0;
                //tableLayoutPanel23_label();
                // lChartPlus45.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView7.Model = null;
                if (checkBox10.Checked == true)
                {
                    //_valueAxis11.IsAxisVisible = true;
                    _myPlotModel1.Series.Add(checkBox11_1);
                }
                if (checkBox10.Checked == false)
                {
                    //_valueAxis11.IsAxisVisible = false;
                    _myPlotModel1.Series.Remove(checkBox11_1);
                }
                plotView7.Model = _myPlotModel1;
                //tableLayoutPanel23_label();
                //lChartPlus46.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView8.Model = null;
                if (checkBox11.Checked == true)
                {
                    //_valueAxis12.IsAxisVisible = true;
                    _myPlotModel2.Series.Add(checkBox12_1);
                }
                if (checkBox11.Checked == false)
                {
                    //_valueAxis12.IsAxisVisible = false;
                    _myPlotModel2.Series.Remove(checkBox12_1);
                }
                plotView8.Model = _myPlotModel2;
                //tableLayoutPanel23_label();
                //lChartPlus47.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView9.Model = null;
                if (checkBox12.Checked == true)
                {
                    //_valueAxis13.IsAxisVisible = true;
                    _myPlotModel3.Series.Add(checkBox13_1);
                }
                if (checkBox12.Checked == false)
                {
                    //_valueAxis13.IsAxisVisible = false;
                    _myPlotModel3.Series.Remove(checkBox13_1);
                }
                plotView9.Model = _myPlotModel3;
                //tableLayoutPanel23_label();
                //lChartPlus48.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView10.Model = null;
                if (checkBox13.Checked == true)
                {
                    //_valueAxis14.IsAxisVisible = true;
                    _myPlotModel4.Series.Add(checkBox14_1);
                }
                if (checkBox13.Checked == false)
                {
                    //_valueAxis14.IsAxisVisible = false;
                    _myPlotModel4.Series.Remove(checkBox14_1);
                }
                plotView10.Model = _myPlotModel4;
                //tableLayoutPanel23_label();
                //lChartPlus49.ToggleCheckBoxY(sender, e, 0);
            }
            catch
            { }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                plotView11.Model = null;
                if (checkBox14.Checked == true)
                {
                    //_valueAxis15.IsAxisVisible = true;
                    _myPlotModel5.Series.Add(checkBox15_1);
                }
                if (checkBox14.Checked == false)
                {
                    //_valueAxis15.IsAxisVisible = false;
                    _myPlotModel5.Series.Remove(checkBox15_1);
                }
                plotView11.Model = _myPlotModel5;
                //tableLayoutPanel23_label();
                /*lChartPlus49_1.ToggleCheckBoxY(sender, e, 0);
                lChartPlus49_2.ToggleCheckBoxY(sender, e, 0);
                lChartPlus49_3.ToggleCheckBoxY(sender, e, 0);
                lChartPlus49_4.ToggleCheckBoxY(sender, e, 0);*/
            }
            catch
            { }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            plotView12.Model = null;
            if (checkBox15.Checked == true)
            {
                //_valueAxis16.IsAxisVisible = true;
                _myPlotModel6.Series.Add(checkBox16_1);
            }
            if (checkBox15.Checked == false)
            {
                //_valueAxis16.IsAxisVisible = false;
                _myPlotModel6.Series.Remove(checkBox16_1);
            }
            plotView12.Model = _myPlotModel6;
            //tableLayoutPanel23_label();
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            plotView13.Model = null;
            if (checkBox16.Checked == true)
            {
                //_valueAxis17.IsAxisVisible = true;
                _myPlotModel7.Series.Add(checkBox17_1);
            }
            if (checkBox16.Checked == false)
            {
                //_valueAxis17.IsAxisVisible = false;
                _myPlotModel7.Series.Remove(checkBox17_1);
            }
            plotView13.Model = _myPlotModel7;
            //tableLayoutPanel23_label();
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            plotView14.Model = null;
            if (checkBox17.Checked == true)
            {
                //_valueAxis18.IsAxisVisible = true;
                _myPlotModel8.Series.Add(checkBox18_1);
            }
            if (checkBox17.Checked == false)
            {
                //_valueAxis18.IsAxisVisible = false;
                _myPlotModel8.Series.Remove(checkBox18_1);
            }
            plotView14.Model = _myPlotModel8;
            //tableLayoutPanel23_label();
        }

        //显示全部曲线按钮
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
                this.checkBox18.Checked = true;
            }
            catch
            { }
        }

        //隐藏全部曲线按钮
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
                this.checkBox18.Checked = false;
            }
            catch
            { }
        }

        //下拉框事件
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int sj = int.Parse(comboBox1.Text);
            shishiquxian();
            quxian();
            //DateTime dt = new DateTime(2020, 10, 27, 16, 21, 0);
            //shishiquxian(dt);
            //quxianfuzhi();//曲线赋值
            //zhongdianweizhiquxian();//终点位置曲线赋值
        }

        /// <summary>
        /// 自动缩放
        /// </summary>
        public void tableLayoutPanel23_label()
        {
            if (checkBox1.Checked == false && checkBox2.Checked == false)
            {
                tableLayoutPanel23.RowStyles[0].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[0].Height = 8;
            }
            if (checkBox3.Checked == false && checkBox4.Checked == false)
            {
                tableLayoutPanel23.RowStyles[1].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[1].Height = 8;
            }
            if (checkBox5.Checked == false && checkBox6.Checked == false)
            {
                tableLayoutPanel23.RowStyles[2].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[2].Height = 8;
            }
            if (checkBox7.Checked == false && checkBox8.Checked == false)
            {
                tableLayoutPanel23.RowStyles[3].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[3].Height = 8;
            }
            if (checkBox18.Checked == false)
            {
                tableLayoutPanel23.RowStyles[4].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[4].Height = 8;
            }
            if (checkBox9.Checked == false)
            {
                tableLayoutPanel23.RowStyles[5].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[5].Height = 8;
            }
            if (checkBox10.Checked == false)
            {
                tableLayoutPanel23.RowStyles[6].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[6].Height = 8;
            }
            if (checkBox11.Checked == false)
            {
                tableLayoutPanel23.RowStyles[7].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[7].Height = 8;
            }
            if (checkBox12.Checked == false)
            {
                tableLayoutPanel23.RowStyles[8].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[8].Height = 8;
            }
            if (checkBox13.Checked == false)
            {
                tableLayoutPanel23.RowStyles[9].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[9].Height = 8;
            }
            if (checkBox14.Checked == false)
            {
                tableLayoutPanel23.RowStyles[10].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[10].Height = 8;
            }
            if (checkBox15.Checked == false)
            {
                tableLayoutPanel23.RowStyles[11].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[11].Height = 8;
            }
            if (checkBox16.Checked == false)
            {
                tableLayoutPanel23.RowStyles[12].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[12].Height = 8;
            }
            if (checkBox17.Checked == false)
            {
                tableLayoutPanel23.RowStyles[13].Height = 0;
            }
            else
            {
                tableLayoutPanel23.RowStyles[13].Height = 8;
            }
        }

        public void _Clear()
        {
        }
    }
}