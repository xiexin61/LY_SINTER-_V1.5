using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseHelper;
using DataReader;
using LiveCharts;
using LiveCharts.Wpf;
using WindowsFormsApp2.page;
using OxyPlot.Axes;
using OxyPlot;
using LY_SINTER.Custom;

namespace LY_SINTER.PAGE.HIS
{
    public partial class lishiqushiquxianchaxun1 : UserControl
    {
        public static List<string> aaa1 = new List<string>();
        public static List<string> bbb1 = new List<string>();
        public static List<string> aaa1_pre = new List<string>();
        public static List<string> bbb1_pre = new List<string>();
        public int isFind = 0;

        #region 曲线数据收集定义

        private DateTimeAxis _dateAxis;//X轴
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
        private LinearAxis _valueAxis19;//Y轴
        private LinearAxis _valueAxis20;//Y轴
        private LinearAxis _valueAxis21;//Y轴

        /// <summary>
        /// 曲线上下限
        /// </summary>
        private List<List<double>> List_text = new List<List<double>>();

        /// <summary>
        /// 曲线数据容器
        /// </summary>
        private List<List<OxyPlot.DataPoint>> list_sum = new List<List<OxyPlot.DataPoint>>();

        #endregion 曲线数据收集定义

        #region 曲线定义

        private PlotModel _myPlotModel;
        public DateTimePicker _dateTimePicker1;
        public DateTimePicker _dateTimePicker2;

        #region 曲线定义

        private OxyPlot.Series.LineSeries series1;
        private OxyPlot.Series.LineSeries series2;
        private OxyPlot.Series.LineSeries series3;
        private OxyPlot.Series.LineSeries series4;
        private OxyPlot.Series.LineSeries series5;
        private OxyPlot.Series.LineSeries series6;
        private OxyPlot.Series.LineSeries series7;
        private OxyPlot.Series.LineSeries series8;
        private OxyPlot.Series.LineSeries series9;
        private OxyPlot.Series.LineSeries series10;
        private OxyPlot.Series.LineSeries series11;
        private OxyPlot.Series.LineSeries series12;
        private OxyPlot.Series.LineSeries series13;
        private OxyPlot.Series.LineSeries series14;
        private OxyPlot.Series.LineSeries series15;
        private OxyPlot.Series.LineSeries series16;
        private OxyPlot.Series.LineSeries series17;
        private OxyPlot.Series.LineSeries series18;
        private OxyPlot.Series.LineSeries series19;
        private OxyPlot.Series.LineSeries series20;
        private OxyPlot.Series.LineSeries series21;

        #endregion 曲线定义

        private string[] curve_name = { "A_1", "A_2", "A_3", "A_4", "A_5", "A_6", "A_7", "A_8", "A_9", "A_10", "A_11", "A_12", "A_13", "A_14", "A_15", "A_16", "A_17", "A_18", "A_19", "A_20", "A_21", };

        #endregion 曲线定义

        public class ValueList : List<double>
        {
            public ValueList()
            { }
        }

        public ValueList[] ValueListArr = new ValueList[21];
        private List<double> b1 = new List<double>();
        private List<double> b2 = new List<double>();
        private List<double> b3 = new List<double>();
        private List<double> b4 = new List<double>();
        private List<double> b5 = new List<double>();
        private List<double> b6 = new List<double>();
        private List<double> b7 = new List<double>();
        private List<double> b8 = new List<double>();
        private List<double> b9 = new List<double>();
        private List<double> b10 = new List<double>();
        private List<double> b11 = new List<double>();
        private List<double> b12 = new List<double>();
        private List<double> b13 = new List<double>();
        private List<double> b14 = new List<double>();
        private List<double> b15 = new List<double>();
        private List<double> b16 = new List<double>();
        private List<double> b17 = new List<double>();
        private List<double> b18 = new List<double>();
        private List<double> b19 = new List<double>();

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
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
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private List<double> b20 = new List<double>();
        private List<double> b21 = new List<double>();

        public lishiqushiquxianchaxun1()
        {
            InitializeComponent();
            dateTimePicker_value1();
            checkbox_yincang();
            DateTimeChoser.AddTo(textBox1);
            DateTimeChoser.AddTo(textBox2);
        }

        /// <summary>
        /// 开始时间&结束时间赋值
        /// </summary>
        public void dateTimePicker_value1()
        {
            //结束时间
            DateTime time_end = DateTime.Now;
            //开始时间
            DateTime time_begin = time_end.AddDays(-7);

            textBox1.Text = time_begin.ToString();
            textBox2.Text = time_end.ToString();
        }

        //数据选择
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            lishi_shujuxuanze form_display = new lishi_shujuxuanze();
            if (lishi_shujuxuanze.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                label4.Text = "最新调整时间:" + DateTime.Now.ToString();
                List<string> ziduanming1 = new List<string>();
                List<string> zhongwenmiaoshu1 = new List<string>();
                ziduanming1 = aaa1;//字段名
                zhongwenmiaoshu1 = bbb1;//字段描述
                checkbox_yincang();//隐藏多选框
                if (!ziduanming1.Count.Equals(0))
                {
                    //checkbox的显示与赋值
                    if (zhongwenmiaoshu1.Count == 1)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox1.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 2)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 3)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 4)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 5)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 6)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 7)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 8)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 9)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 10)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 11)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox11.Text = zhongwenmiaoshu1[10];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                        checkBox11.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 12)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox11.Text = zhongwenmiaoshu1[10];
                        checkBox12.Text = zhongwenmiaoshu1[11];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                        checkBox11.Visible = true;
                        checkBox12.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 13)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox11.Text = zhongwenmiaoshu1[10];
                        checkBox12.Text = zhongwenmiaoshu1[11];
                        checkBox13.Text = zhongwenmiaoshu1[12];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                        checkBox11.Visible = true;
                        checkBox12.Visible = true;
                        checkBox13.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 14)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox11.Text = zhongwenmiaoshu1[10];
                        checkBox12.Text = zhongwenmiaoshu1[11];
                        checkBox13.Text = zhongwenmiaoshu1[12];
                        checkBox14.Text = zhongwenmiaoshu1[13];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                        checkBox11.Visible = true;
                        checkBox12.Visible = true;
                        checkBox13.Visible = true;
                        checkBox14.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 15)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox11.Text = zhongwenmiaoshu1[10];
                        checkBox12.Text = zhongwenmiaoshu1[11];
                        checkBox13.Text = zhongwenmiaoshu1[12];
                        checkBox14.Text = zhongwenmiaoshu1[13];
                        checkBox15.Text = zhongwenmiaoshu1[14];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                        checkBox11.Visible = true;
                        checkBox12.Visible = true;
                        checkBox13.Visible = true;
                        checkBox14.Visible = true;
                        checkBox15.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 16)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox11.Text = zhongwenmiaoshu1[10];
                        checkBox12.Text = zhongwenmiaoshu1[11];
                        checkBox13.Text = zhongwenmiaoshu1[12];
                        checkBox14.Text = zhongwenmiaoshu1[13];
                        checkBox15.Text = zhongwenmiaoshu1[14];
                        checkBox16.Text = zhongwenmiaoshu1[15];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                        checkBox11.Visible = true;
                        checkBox12.Visible = true;
                        checkBox13.Visible = true;
                        checkBox14.Visible = true;
                        checkBox15.Visible = true;
                        checkBox16.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 17)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox11.Text = zhongwenmiaoshu1[10];
                        checkBox12.Text = zhongwenmiaoshu1[11];
                        checkBox13.Text = zhongwenmiaoshu1[12];
                        checkBox14.Text = zhongwenmiaoshu1[13];
                        checkBox15.Text = zhongwenmiaoshu1[14];
                        checkBox16.Text = zhongwenmiaoshu1[15];
                        checkBox17.Text = zhongwenmiaoshu1[16];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                        checkBox11.Visible = true;
                        checkBox12.Visible = true;
                        checkBox13.Visible = true;
                        checkBox14.Visible = true;
                        checkBox15.Visible = true;
                        checkBox16.Visible = true;
                        checkBox17.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 18)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox11.Text = zhongwenmiaoshu1[10];
                        checkBox12.Text = zhongwenmiaoshu1[11];
                        checkBox13.Text = zhongwenmiaoshu1[12];
                        checkBox14.Text = zhongwenmiaoshu1[13];
                        checkBox15.Text = zhongwenmiaoshu1[14];
                        checkBox16.Text = zhongwenmiaoshu1[15];
                        checkBox17.Text = zhongwenmiaoshu1[16];
                        checkBox18.Text = zhongwenmiaoshu1[17];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                        checkBox11.Visible = true;
                        checkBox12.Visible = true;
                        checkBox13.Visible = true;
                        checkBox14.Visible = true;
                        checkBox15.Visible = true;
                        checkBox16.Visible = true;
                        checkBox17.Visible = true;
                        checkBox18.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 19)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox11.Text = zhongwenmiaoshu1[10];
                        checkBox12.Text = zhongwenmiaoshu1[11];
                        checkBox13.Text = zhongwenmiaoshu1[12];
                        checkBox14.Text = zhongwenmiaoshu1[13];
                        checkBox15.Text = zhongwenmiaoshu1[14];
                        checkBox16.Text = zhongwenmiaoshu1[15];
                        checkBox17.Text = zhongwenmiaoshu1[16];
                        checkBox18.Text = zhongwenmiaoshu1[17];
                        checkBox19.Text = zhongwenmiaoshu1[18];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                        checkBox11.Visible = true;
                        checkBox12.Visible = true;
                        checkBox13.Visible = true;
                        checkBox14.Visible = true;
                        checkBox15.Visible = true;
                        checkBox16.Visible = true;
                        checkBox17.Visible = true;
                        checkBox18.Visible = true;
                        checkBox19.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 20)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox11.Text = zhongwenmiaoshu1[10];
                        checkBox12.Text = zhongwenmiaoshu1[11];
                        checkBox13.Text = zhongwenmiaoshu1[12];
                        checkBox14.Text = zhongwenmiaoshu1[13];
                        checkBox15.Text = zhongwenmiaoshu1[14];
                        checkBox16.Text = zhongwenmiaoshu1[15];
                        checkBox17.Text = zhongwenmiaoshu1[16];
                        checkBox18.Text = zhongwenmiaoshu1[17];
                        checkBox19.Text = zhongwenmiaoshu1[18];
                        checkBox20.Text = zhongwenmiaoshu1[19];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                        checkBox11.Visible = true;
                        checkBox12.Visible = true;
                        checkBox13.Visible = true;
                        checkBox14.Visible = true;
                        checkBox15.Visible = true;
                        checkBox16.Visible = true;
                        checkBox17.Visible = true;
                        checkBox18.Visible = true;
                        checkBox19.Visible = true;
                        checkBox20.Visible = true;
                    }
                    else if (zhongwenmiaoshu1.Count == 21)
                    {
                        checkBox1.Text = zhongwenmiaoshu1[0];
                        checkBox2.Text = zhongwenmiaoshu1[1];
                        checkBox3.Text = zhongwenmiaoshu1[2];
                        checkBox4.Text = zhongwenmiaoshu1[3];
                        checkBox5.Text = zhongwenmiaoshu1[4];
                        checkBox6.Text = zhongwenmiaoshu1[5];
                        checkBox7.Text = zhongwenmiaoshu1[6];
                        checkBox8.Text = zhongwenmiaoshu1[7];
                        checkBox9.Text = zhongwenmiaoshu1[8];
                        checkBox10.Text = zhongwenmiaoshu1[9];
                        checkBox11.Text = zhongwenmiaoshu1[10];
                        checkBox12.Text = zhongwenmiaoshu1[11];
                        checkBox13.Text = zhongwenmiaoshu1[12];
                        checkBox14.Text = zhongwenmiaoshu1[13];
                        checkBox15.Text = zhongwenmiaoshu1[14];
                        checkBox16.Text = zhongwenmiaoshu1[15];
                        checkBox17.Text = zhongwenmiaoshu1[16];
                        checkBox18.Text = zhongwenmiaoshu1[17];
                        checkBox19.Text = zhongwenmiaoshu1[18];
                        checkBox20.Text = zhongwenmiaoshu1[19];
                        checkBox21.Text = zhongwenmiaoshu1[20];
                        checkBox1.Visible = true;
                        checkBox2.Visible = true;
                        checkBox3.Visible = true;
                        checkBox4.Visible = true;
                        checkBox5.Visible = true;
                        checkBox6.Visible = true;
                        checkBox7.Visible = true;
                        checkBox8.Visible = true;
                        checkBox9.Visible = true;
                        checkBox10.Visible = true;
                        checkBox11.Visible = true;
                        checkBox12.Visible = true;
                        checkBox13.Visible = true;
                        checkBox14.Visible = true;
                        checkBox15.Visible = true;
                        checkBox16.Visible = true;
                        checkBox17.Visible = true;
                        checkBox18.Visible = true;
                        checkBox19.Visible = true;
                        checkBox20.Visible = true;
                        checkBox21.Visible = true;
                    }

                    fuzhi();//曲线赋值
                }
                else MessageBox.Show("请选择数据！");
            }
            catch
            { }
        }

        /// <summary>
        /// 初始化时将checkbox隐藏
        /// </summary>
        private void checkbox_yincang()
        {
            checkBox1.Visible = false;
            checkBox2.Visible = false;
            checkBox3.Visible = false;
            checkBox4.Visible = false;
            checkBox5.Visible = false;
            checkBox6.Visible = false;
            checkBox7.Visible = false;
            checkBox8.Visible = false;
            checkBox9.Visible = false;
            checkBox10.Visible = false;
            checkBox11.Visible = false;
            checkBox12.Visible = false;
            checkBox13.Visible = false;
            checkBox14.Visible = false;
            checkBox15.Visible = false;
            checkBox16.Visible = false;
            checkBox17.Visible = false;
            checkBox18.Visible = false;
            checkBox19.Visible = false;
            checkBox20.Visible = false;
            checkBox21.Visible = false;
        }

        /// <summary>
        /// 曲线赋值
        /// </summary>
        private void fuzhi()
        {
            try
            {
                list_sum.Clear();
                List<string> ziduanming1 = new List<string>();
                List<string> zhongwenmiaoshu1 = new List<string>();
                ziduanming1 = aaa1;//字段名
                zhongwenmiaoshu1 = bbb1;//字段描述
                //时间截取
                DateTime d1 = Convert.ToDateTime(textBox1.Text);
                int dty = d1.Year;
                int dtM = d1.Month;
                int dtd = d1.Day;
                int dth = d1.Hour;
                int dtmm = d1.Minute;
                int dts = d1.Second;
                DateTime d2 = Convert.ToDateTime(textBox2.Text);
                int dt2y = d2.Year;
                int dt2M = d2.Month;
                int dt2d = d2.Day;
                int dt2h = d2.Hour;
                int dt2mm = d2.Minute;
                int dt2s = d2.Second;

                //mongoDB传数查值
                Logger.Init(AppDomain.CurrentDomain.FriendlyName);
                HistoryDataReader _HistoryDataReader = new HistoryDataReader();
                //要查找的数据标签定义
                List<string> _List = new List<string>();
                for (int b = 0; b < ziduanming1.Count; b++)
                {
                    _List.Add(ziduanming1[b]);
                }
                DateTime db = new DateTime(dty, dtM, dtd, dth, dtmm, dts);//(2020, 3, 25, 13, 0, 0);//读取数据开始时间
                DateTime db_end = new DateTime(dt2y, dt2M, dt2d, dt2h, dt2mm, dt2s);//(2020, 3, 25, 13, 30, 0);//读取数据结束时间
                var hisDatas = _HistoryDataReader.GetHisDataByTagNames(_List, db, db_end, 60000);//mgdb获取数据

                //重新按照发送字段排列顺序
                List<TagHisData> Re_hisDatas = new List<TagHisData>();
                foreach (var para in _List)
                {
                    foreach (var item in hisDatas)
                    {
                        if (item != null)
                        {
                            if (para == item.Name)
                            {
                                Re_hisDatas.Add(item);
                                break;
                            }
                        }
                    }
                }
                if (_List.Count == Re_hisDatas.Count)
                {
                    list_sum.Clear();
                    List_text.Clear();
                    int _count = Re_hisDatas.Count;
                    for (int x1 = 0; x1 < Re_hisDatas.Count; x1++)
                    {
                        list_sum.Add(new List<OxyPlot.DataPoint>());
                        List_text.Add(new List<double>());
                        if (Re_hisDatas.Count != 0 && Re_hisDatas[x1].BsonDatas.Count > 0)
                        {
                            for (int z = 0; z < Re_hisDatas[x1].BsonDatas.Count; z++)
                            {
                                DateTime date_1 = Re_hisDatas[x1].BsonDatas[z].Timestamp;
                                double _value = 0;
                                if (Re_hisDatas[x1].BsonDatas[z].Value != null)
                                {
                                    if (Re_hisDatas[x1].Type == "BOOLEAN")
                                    {
                                        _value = (Re_hisDatas[x1].BsonDatas[z].Value.ToString()) == "true" ? 1 : 0;
                                    }
                                    else
                                    {
                                        _value = Math.Round(double.Parse(Re_hisDatas[x1].BsonDatas[z].Value.ToString() == "" ? "0" : Re_hisDatas[x1].BsonDatas[z].Value.ToString()), 3);
                                    }
                                }
                                else
                                {
                                    _value = double.NaN;
                                }
                                OxyPlot.DataPoint line1 = new OxyPlot.DataPoint(DateTimeAxis.ToDouble(date_1), _value);
                                List_text[x1].Add(_value);
                                list_sum[x1].Add(line1);
                            }
                        }
                    }
                    curve_binddate(_count);
                }
                else
                {
                    MessageBox.Show("数据异常，请重新选择");
                    return;
                }

                #region 注释

                //    // dingyi1(Re_hisDatas[0].BsonDatas.Count);//曲线定义
                //    //赋值(时间)
                //    if (Re_hisDatas.Count != 0 && Re_hisDatas[0].BsonDatas.Count > 0)
                //{
                //    for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //    {
                //        string sj = Re_hisDatas[0].BsonDatas[z].Timestamp.ToString();
                //    //    a.Add(sj);
                //    }

                //    if (Re_hisDatas.Count == 1)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = false;
                //        this.checkBox3.Checked = false;
                //        this.checkBox4.Checked = false;
                //        this.checkBox5.Checked = false;
                //        this.checkBox6.Checked = false;
                //        this.checkBox7.Checked = false;
                //        this.checkBox8.Checked = false;
                //        this.checkBox9.Checked = false;
                //        this.checkBox10.Checked = false;
                //        this.checkBox11.Checked = false;
                //        this.checkBox12.Checked = false;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;

                //        ////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 2)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = false;
                //        this.checkBox4.Checked = false;
                //        this.checkBox5.Checked = false;
                //        this.checkBox6.Checked = false;
                //        this.checkBox7.Checked = false;
                //        this.checkBox8.Checked = false;
                //        this.checkBox9.Checked = false;
                //        this.checkBox10.Checked = false;
                //        this.checkBox11.Checked = false;
                //        this.checkBox12.Checked = false;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;

                //        /////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 3)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = false;
                //        this.checkBox5.Checked = false;
                //        this.checkBox6.Checked = false;
                //        this.checkBox7.Checked = false;
                //        this.checkBox8.Checked = false;
                //        this.checkBox9.Checked = false;
                //        this.checkBox10.Checked = false;
                //        this.checkBox11.Checked = false;
                //        this.checkBox12.Checked = false;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        /////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 4)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = false;
                //        this.checkBox6.Checked = false;
                //        this.checkBox7.Checked = false;
                //        this.checkBox8.Checked = false;
                //        this.checkBox9.Checked = false;
                //        this.checkBox10.Checked = false;
                //        this.checkBox11.Checked = false;
                //        this.checkBox12.Checked = false;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        //////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 5)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = false;
                //        this.checkBox7.Checked = false;
                //        this.checkBox8.Checked = false;
                //        this.checkBox9.Checked = false;
                //        this.checkBox10.Checked = false;
                //        this.checkBox11.Checked = false;
                //        this.checkBox12.Checked = false;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        ///////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 6)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox7.Checked = false;
                //        this.checkBox8.Checked = false;
                //        this.checkBox9.Checked = false;
                //        this.checkBox10.Checked = false;
                //        this.checkBox11.Checked = false;
                //        this.checkBox12.Checked = false;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        //////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 7)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox7.Checked = true;
                //        this.checkBox8.Checked = false;
                //        this.checkBox9.Checked = false;
                //        this.checkBox10.Checked = false;
                //        this.checkBox11.Checked = false;
                //        this.checkBox12.Checked = false;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        /////////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 8)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = false;
                //        this.checkBox10.Checked = false;
                //        this.checkBox11.Checked = false;
                //        this.checkBox12.Checked = false;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;

                //        /////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 9)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = false;
                //        this.checkBox11.Checked = false;
                //        this.checkBox12.Checked = false;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        ///////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 10)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = true;
                //        this.checkBox11.Checked = false;
                //        this.checkBox12.Checked = false;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        /////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 11)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = true;
                //        this.checkBox11.Checked = true;
                //        this.checkBox12.Checked = false;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;

                //        //////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 12)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = true;
                //        this.checkBox11.Checked = true;
                //        this.checkBox12.Checked = true;
                //        this.checkBox13.Checked = false;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        ////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 13)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = true;
                //        this.checkBox11.Checked = true;
                //        this.checkBox12.Checked = true;
                //        this.checkBox13.Checked = true;
                //        this.checkBox14.Checked = false;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;

                //        /////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 14)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = true;
                //        this.checkBox11.Checked = true;
                //        this.checkBox12.Checked = true;
                //        this.checkBox13.Checked = true;
                //        this.checkBox14.Checked = true;
                //        this.checkBox15.Checked = false;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        /////////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 15)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = true;
                //        this.checkBox11.Checked = true;
                //        this.checkBox12.Checked = true;
                //        this.checkBox13.Checked = true;
                //        this.checkBox14.Checked = true;
                //        this.checkBox15.Checked = true;
                //        this.checkBox16.Checked = false;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        ///////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 16)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = true;
                //        this.checkBox11.Checked = true;
                //        this.checkBox12.Checked = true;
                //        this.checkBox13.Checked = true;
                //        this.checkBox14.Checked = true;
                //        this.checkBox15.Checked = true;
                //        this.checkBox16.Checked = true;
                //        this.checkBox17.Checked = false;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        ////////////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 17)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = true;
                //        this.checkBox11.Checked = true;
                //        this.checkBox12.Checked = true;
                //        this.checkBox13.Checked = true;
                //        this.checkBox14.Checked = true;
                //        this.checkBox15.Checked = true;
                //        this.checkBox16.Checked = true;
                //        this.checkBox17.Checked = true;
                //        this.checkBox18.Checked = false;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        //////////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 18)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = true;
                //        this.checkBox11.Checked = true;
                //        this.checkBox12.Checked = true;
                //        this.checkBox13.Checked = true;
                //        this.checkBox14.Checked = true;
                //        this.checkBox15.Checked = true;
                //        this.checkBox16.Checked = true;
                //        this.checkBox17.Checked = true;
                //        this.checkBox18.Checked = true;
                //        this.checkBox19.Checked = false;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        /////////////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 19)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = true;
                //        this.checkBox11.Checked = true;
                //        this.checkBox12.Checked = true;
                //        this.checkBox13.Checked = true;
                //        this.checkBox14.Checked = true;
                //        this.checkBox15.Checked = true;
                //        this.checkBox16.Checked = true;
                //        this.checkBox17.Checked = true;
                //        this.checkBox18.Checked = true;
                //        this.checkBox19.Checked = true;
                //        this.checkBox20.Checked = false;
                //        this.checkBox21.Checked = false;
                //        ///////////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 20)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //        this.checkBox1.Checked = true;
                //        this.checkBox2.Checked = true;
                //        this.checkBox3.Checked = true;
                //        this.checkBox4.Checked = true;
                //        this.checkBox5.Checked = true;
                //        this.checkBox6.Checked = true;
                //        this.checkBox8.Checked = true;
                //        this.checkBox9.Checked = true;
                //        this.checkBox10.Checked = true;
                //        this.checkBox11.Checked = true;
                //        this.checkBox12.Checked = true;
                //        this.checkBox13.Checked = true;
                //        this.checkBox14.Checked = true;
                //        this.checkBox15.Checked = true;
                //        this.checkBox16.Checked = true;
                //        this.checkBox17.Checked = true;
                //        this.checkBox18.Checked = true;
                //        this.checkBox19.Checked = true;
                //        this.checkBox20.Checked = true;
                //        this.checkBox21.Checked = false;
                //        ////////////////////////////////////////////////结束
                //    }
                //    else if (Re_hisDatas.Count == 21)
                //    {
                //        for (int z = 0; z < Re_hisDatas[0].BsonDatas.Count; z++)
                //        {
                //            DataConverter(Re_hisDatas, z, Re_hisDatas.Count);
                //        }
                //        SetB();
                //        BindData();
                //    }
                // //   List<Axis> vaxesY = lChartPlus120.LChart.AxisY.ToList();
                // //   CheckBoxchange(vaxesY);
                //}
                //else
                //{
                //    DataConverter(Re_hisDatas,0, Re_hisDatas.Count);
                //    SetB();
                //    BindData();
                //    MessageBox.Show("此时间段没有数据！");

                //}

                #endregion 注释
            }
            catch (Exception ee)
            {
                string a = ee.Message;
            }
        }

        public void Timer_stop()
        {
            this.Dispose();
        }

        public void Timer_state()
        {
        }

        /// <summary>
        /// 曲线赋值绑定
        /// _text曲线个数
        /// </summary>
        public void curve_binddate(int _text)
        {
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
                MinorIntervalType = DateTimeIntervalType.Days,
                IntervalType = DateTimeIntervalType.Days,
                StringFormat = "yyyy/MM/dd HH:mm",
            };
            _myPlotModel.Axes.Add(_dateAxis);

            if (_text >= 1)
            {
                int x = 1;
                if ((int)((List_text[0].Max() - List_text[0].Min()) / 5) > 0)
                {
                    x = (int)((List_text[0].Max() - List_text[0].Min()) / 5);
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
                    Maximum = (int)(List_text[0].Max() + 1),
                    Minimum = (int)(List_text[0].Min() - 1),
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
                    ItemsSource = list_sum[0]
                };
                _valueAxis1.IsAxisVisible = true;
                _myPlotModel.Series.Add(series1);
            }

            if (_text >= 2)
            {
                int x = 1;
                if ((int)((List_text[1].Max() - List_text[1].Min()) / 5) > 0)
                {
                    x = (int)((List_text[1].Max() - List_text[1].Min()) / 5);
                }
                _valueAxis2 = new LinearAxis()
                {
                    Key = curve_name[1],
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[1].Max() + 1),
                    Minimum = (int)(List_text[1].Min() - 1),
                    PositionTier = 2,
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Purple,
                    MinorTicklineColor = OxyColors.Purple,
                    TicklineColor = OxyColors.Purple,
                    TextColor = OxyColors.Purple,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis2);
                //添加曲线
                series2 = new OxyPlot.Series.LineSeries()
                {
                    Color = OxyColors.Purple,
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Purple,
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[1],
                    ItemsSource = list_sum[1]
                };
                _valueAxis2.IsAxisVisible = true;
                _myPlotModel.Series.Add(series2);
            }

            if (_text >= 3)
            {
                int x = 1;
                if ((int)((List_text[2].Max() - List_text[2].Min()) / 5) > 0)
                {
                    x = (int)((List_text[2].Max() - List_text[2].Min()) / 5);
                }
                _valueAxis3 = new LinearAxis()//数据声明
                {
                    Key = curve_name[2],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[2].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[2].Min() - 1),//刻度最小值
                    PositionTier = 3,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Green,//颜色
                    MinorTicklineColor = OxyColors.Green,
                    TicklineColor = OxyColors.Green,
                    TextColor = OxyColors.Green,
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis3);//添加声明
                //添加曲线
                series3 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.Green,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Green,
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[2],//绑定识别代码
                    ItemsSource = list_sum[2]//数据源
                };
                _valueAxis3.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series3);//添加曲线
            }

            if (_text >= 4)
            {
                int x = 1;
                if ((int)((List_text[3].Max() - List_text[3].Min()) / 5) > 0)
                {
                    x = (int)((List_text[3].Max() - List_text[3].Min()) / 5);
                }
                _valueAxis4 = new LinearAxis()//数据声明
                {
                    Key = curve_name[3],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[3].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[3].Min() - 1),//刻度最小值
                    PositionTier = 4,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Blue,//颜色
                    MinorTicklineColor = OxyColors.Blue,//颜色
                    TicklineColor = OxyColors.Blue,//颜色
                    TextColor = OxyColors.Blue,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis4);//添加声明
                series4 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.Blue,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Blue,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[3],//绑定识别代码
                    ItemsSource = list_sum[3]//数据源
                };
                _valueAxis4.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series4);//添加曲线
            }

            if (_text >= 5)
            {
                int x = 1;
                if ((int)((List_text[4].Max() - List_text[4].Min()) / 5) > 0)
                {
                    x = (int)((List_text[4].Max() - List_text[4].Min()) / 5);
                }
                _valueAxis5 = new LinearAxis()//数据声明
                {
                    Key = curve_name[4],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[4].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[4].Min() - 1),//刻度最小值
                    PositionTier = 5,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Tan,//颜色
                    MinorTicklineColor = OxyColors.Tan,//颜色
                    TicklineColor = OxyColors.Tan,//颜色
                    TextColor = OxyColors.Tan,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis5);//添加声明
                series5 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.Tan,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Tan,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[4],//绑定识别代码
                    ItemsSource = list_sum[4]//数据源
                };
                _valueAxis5.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series5);//添加曲线
            }

            if (_text >= 6)
            {
                int x = 1;
                if ((int)((List_text[5].Max() - List_text[5].Min()) / 5) > 0)
                {
                    x = (int)((List_text[5].Max() - List_text[5].Min()) / 5);
                }
                _valueAxis6 = new LinearAxis()//数据声明
                {
                    Key = curve_name[5],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[5].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[5].Min() - 1),//刻度最小值
                    PositionTier = 6,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Black,//颜色
                    MinorTicklineColor = OxyColors.Black,//颜色
                    TicklineColor = OxyColors.Black,//颜色
                    TextColor = OxyColors.Black,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis6);//添加声明
                series6 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.Black,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Black,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[5],//绑定识别代码
                    ItemsSource = list_sum[5]//数据源
                };
                _valueAxis6.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series6);//添加曲线
            }

            if (_text >= 7)
            {
                int x = 1;
                if ((int)((List_text[6].Max() - List_text[6].Min()) / 5) > 0)
                {
                    x = (int)((List_text[6].Max() - List_text[6].Min()) / 5);
                }
                _valueAxis7 = new LinearAxis()//数据声明
                {
                    Key = curve_name[6],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[6].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[6].Min() - 1),//刻度最小值
                    PositionTier = 7,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Orange,//颜色
                    MinorTicklineColor = OxyColors.Orange,//颜色
                    TicklineColor = OxyColors.Orange,//颜色
                    TextColor = OxyColors.Orange,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis7);//添加声明
                series7 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.Orange,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Orange,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[6],//绑定识别代码
                    ItemsSource = list_sum[6]//数据源
                };
                _valueAxis7.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series7);//添加曲线
            }

            if (_text >= 8)
            {
                int x = 1;
                if ((int)((List_text[7].Max() - List_text[7].Min()) / 5) > 0)
                {
                    x = (int)((List_text[7].Max() - List_text[7].Min()) / 5);
                }
                _valueAxis8 = new LinearAxis()//数据声明
                {
                    Key = curve_name[7],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[7].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[7].Min() - 1),//刻度最小值
                    PositionTier = 8,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Fuchsia,//颜色
                    MinorTicklineColor = OxyColors.Fuchsia,//颜色
                    TicklineColor = OxyColors.Fuchsia,//颜色
                    TextColor = OxyColors.Fuchsia,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis8);//添加声明
                series8 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.Fuchsia,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Fuchsia,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[7],//绑定识别代码
                    ItemsSource = list_sum[7]//数据源
                };
                _valueAxis8.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series8);//添加曲线
            }

            if (_text >= 9)
            {
                int x = 1;
                if ((int)((List_text[8].Max() - List_text[8].Min()) / 5) > 0)
                {
                    x = (int)((List_text[8].Max() - List_text[8].Min()) / 5);
                }
                _valueAxis9 = new LinearAxis()//数据声明
                {
                    Key = curve_name[8],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[8].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[8].Min() - 1),//刻度最小值
                    PositionTier = 9,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Olive,//颜色
                    MinorTicklineColor = OxyColors.Olive,//颜色
                    TicklineColor = OxyColors.Olive,//颜色
                    TextColor = OxyColors.Olive,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis9);//添加声明
                series9 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.Olive,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Olive,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[8],//绑定识别代码
                    ItemsSource = list_sum[8]//数据源
                };
                _valueAxis9.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series9);//添加曲线
            }

            if (_text >= 10)
            {
                int x = 1;
                if ((int)((List_text[9].Max() - List_text[9].Min()) / 5) > 0)
                {
                    x = (int)((List_text[9].Max() - List_text[9].Min()) / 5);
                }
                _valueAxis10 = new LinearAxis()//数据声明
                {
                    Key = curve_name[9],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[9].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[9].Min() - 1),//刻度最小值
                    PositionTier = 10,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.SlateGray,//颜色
                    MinorTicklineColor = OxyColors.SlateGray,//颜色
                    TicklineColor = OxyColors.SlateGray,//颜色
                    TextColor = OxyColors.SlateGray,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis10);//添加声明
                series10 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.SlateGray,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.SlateGray,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[9],//绑定识别代码
                    ItemsSource = list_sum[9]//数据源
                };
                _valueAxis10.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series10);//添加曲线
            }

            if (_text >= 11)
            {
                int x = 1;
                if ((int)((List_text[10].Max() - List_text[10].Min()) / 5) > 0)
                {
                    x = (int)((List_text[10].Max() - List_text[10].Min()) / 5);
                }
                _valueAxis11 = new LinearAxis()//数据声明
                {
                    Key = curve_name[10],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[10].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[10].Min() - 1),//刻度最小值
                    PositionTier = 11,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Aqua,//颜色
                    MinorTicklineColor = OxyColors.Aqua,//颜色
                    TicklineColor = OxyColors.Aqua,//颜色
                    TextColor = OxyColors.Aqua,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis11);//添加声明
                series11 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.Aqua,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Aqua,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[10],//绑定识别代码
                    ItemsSource = list_sum[10]//数据源
                };
                _valueAxis11.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series11);//添加曲线
            }

            if (_text >= 12)
            {
                int x = 1;
                if ((int)((List_text[11].Max() - List_text[11].Min()) / 5) > 0)
                {
                    x = (int)((List_text[11].Max() - List_text[11].Min()) / 5);
                }
                _valueAxis12 = new LinearAxis()//数据声明
                {
                    Key = curve_name[11],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[11].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[11].Min() - 1),//刻度最小值
                    PositionTier = 12,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.SaddleBrown,//颜色
                    MinorTicklineColor = OxyColors.SaddleBrown,//颜色
                    TicklineColor = OxyColors.SaddleBrown,//颜色
                    TextColor = OxyColors.SaddleBrown,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis12);//添加声明
                series12 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.SaddleBrown,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.SaddleBrown,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[11],//绑定识别代码
                    ItemsSource = list_sum[11]//数据源
                };
                _valueAxis12.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series12);//添加曲线
            }

            if (_text >= 13)
            {
                int x = 1;
                if ((int)((List_text[12].Max() - List_text[12].Min()) / 5) > 0)
                {
                    x = (int)((List_text[12].Max() - List_text[12].Min()) / 5);
                }
                _valueAxis13 = new LinearAxis()//数据声明
                {
                    Key = curve_name[12],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[12].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[12].Min() - 1),//刻度最小值
                    PositionTier = 13,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.OrangeRed,//颜色
                    MinorTicklineColor = OxyColors.OrangeRed,//颜色
                    TicklineColor = OxyColors.OrangeRed,//颜色
                    TextColor = OxyColors.OrangeRed,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis13);//添加声明
                series13 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.OrangeRed,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.OrangeRed,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[12],//绑定识别代码
                    ItemsSource = list_sum[12]//数据源
                };
                _valueAxis13.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series13);//添加曲线
            }

            if (_text >= 14)
            {
                int x = 1;
                if ((int)((List_text[13].Max() - List_text[13].Min()) / 5) > 0)
                {
                    x = (int)((List_text[13].Max() - List_text[13].Min()) / 5);
                }
                _valueAxis14 = new LinearAxis()//数据声明
                {
                    Key = curve_name[13],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[13].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[13].Min() - 1),//刻度最小值
                    PositionTier = 14,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.RoyalBlue,//颜色
                    MinorTicklineColor = OxyColors.RoyalBlue,//颜色
                    TicklineColor = OxyColors.RoyalBlue,//颜色
                    TextColor = OxyColors.RoyalBlue,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis14);//添加声明
                series14 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.RoyalBlue,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.RoyalBlue,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[13],//绑定识别代码
                    ItemsSource = list_sum[13]//数据源
                };
                _valueAxis14.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series14);//添加曲线
            }

            if (_text >= 15)
            {
                int x = 1;
                if ((int)((List_text[14].Max() - List_text[14].Min()) / 5) > 0)
                {
                    x = (int)((List_text[14].Max() - List_text[14].Min()) / 5);
                }
                _valueAxis15 = new LinearAxis()//数据声明
                {
                    Key = curve_name[14],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[14].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[14].Min() - 1),//刻度最小值
                    PositionTier = 15,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Tomato,//颜色
                    MinorTicklineColor = OxyColors.Tomato,//颜色
                    TicklineColor = OxyColors.Tomato,//颜色
                    TextColor = OxyColors.Tomato,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis15);//添加声明
                series15 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.Tomato,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Tomato,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[14],//绑定识别代码
                    ItemsSource = list_sum[14]//数据源
                };
                _valueAxis15.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series15);//添加曲线
            }

            if (_text >= 16)
            {
                int x = 1;
                if ((int)((List_text[15].Max() - List_text[15].Min()) / 5) > 0)
                {
                    x = (int)((List_text[15].Max() - List_text[15].Min()) / 5);
                }
                _valueAxis16 = new LinearAxis()//数据声明
                {
                    Key = curve_name[15],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[15].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[15].Min() - 1),//刻度最小值
                    PositionTier = 16,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.YellowGreen,//颜色
                    MinorTicklineColor = OxyColors.YellowGreen,//颜色
                    TicklineColor = OxyColors.YellowGreen,//颜色
                    TextColor = OxyColors.YellowGreen,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis16);//添加声明
                series16 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.YellowGreen,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.YellowGreen,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[15],//绑定识别代码
                    ItemsSource = list_sum[15]//数据源
                };
                _valueAxis16.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series16);//添加曲线
            }

            if (_text >= 17)
            {
                int x = 1;
                if ((int)((List_text[16].Max() - List_text[16].Min()) / 5) > 0)
                {
                    x = (int)((List_text[16].Max() - List_text[16].Min()) / 5);
                }
                _valueAxis17 = new LinearAxis()//数据声明
                {
                    Key = curve_name[16],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[16].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[16].Min() - 1),//刻度最小值
                    PositionTier = 17,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.DarkTurquoise,//颜色
                    MinorTicklineColor = OxyColors.DarkTurquoise,//颜色
                    TicklineColor = OxyColors.DarkTurquoise,//颜色
                    TextColor = OxyColors.DarkTurquoise,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis17);//添加声明
                series17 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.DarkTurquoise,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkTurquoise,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[16],//绑定识别代码
                    ItemsSource = list_sum[16]//数据源
                };
                _valueAxis17.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series17);//添加曲线
            }

            if (_text >= 18)
            {
                int x = 1;
                if ((int)((List_text[17].Max() - List_text[17].Min()) / 5) > 0)
                {
                    x = (int)((List_text[17].Max() - List_text[17].Min()) / 5);
                }
                _valueAxis18 = new LinearAxis()//数据声明
                {
                    Key = curve_name[17],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[17].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[17].Min() - 1),//刻度最小值
                    PositionTier = 18,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.DarkViolet,//颜色
                    MinorTicklineColor = OxyColors.DarkViolet,//颜色
                    TicklineColor = OxyColors.DarkViolet,//颜色
                    TextColor = OxyColors.DarkViolet,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis18);//添加声明
                series18 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.DarkViolet,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.DarkViolet,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[17],//绑定识别代码
                    ItemsSource = list_sum[17]//数据源
                };
                _valueAxis18.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series18);//添加曲线
            }

            if (_text >= 19)
            {
                int x = 1;
                if ((int)((List_text[18].Max() - List_text[18].Min()) / 5) > 0)
                {
                    x = (int)((List_text[18].Max() - List_text[18].Min()) / 5);
                }
                _valueAxis19 = new LinearAxis()//数据声明
                {
                    Key = curve_name[18],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[18].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[18].Min() - 1),//刻度最小值
                    PositionTier = 19,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.ForestGreen,//颜色
                    MinorTicklineColor = OxyColors.ForestGreen,//颜色
                    TicklineColor = OxyColors.ForestGreen,//颜色
                    TextColor = OxyColors.ForestGreen,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis19);//添加声明
                series19 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.ForestGreen,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.ForestGreen,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[18],//绑定识别代码
                    ItemsSource = list_sum[18]//数据源
                };
                _valueAxis19.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series19);//添加曲线
            }

            if (_text >= 20)
            {
                int x = 1;
                if ((int)((List_text[19].Max() - List_text[19].Min()) / 5) > 0)
                {
                    x = (int)((List_text[19].Max() - List_text[19].Min()) / 5);
                }
                _valueAxis20 = new LinearAxis()//数据声明
                {
                    Key = curve_name[19],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[19].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[19].Min() - 1),//刻度最小值
                    PositionTier = 20,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Gray,//颜色
                    MinorTicklineColor = OxyColors.Gray,//颜色
                    TicklineColor = OxyColors.Gray,//颜色
                    TextColor = OxyColors.Gray,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis20);//添加声明
                series20 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.Gray,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Gray,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[19],//绑定识别代码
                    ItemsSource = list_sum[19]//数据源
                };
                _valueAxis20.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series20);//添加曲线
            }

            if (_text >= 21)
            {
                int x = 1;
                if ((int)((List_text[20].Max() - List_text[20].Min()) / 5) > 0)
                {
                    x = (int)((List_text[20].Max() - List_text[20].Min()) / 5);
                }
                _valueAxis21 = new LinearAxis()//数据声明
                {
                    Key = curve_name[20],//识别名称
                    MajorGridlineStyle = LineStyle.None,
                    MinorGridlineStyle = LineStyle.None,
                    IntervalLength = 80,
                    Angle = 60,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    Maximum = (int)(List_text[20].Max() + 1),//刻度最大值
                    Minimum = (int)(List_text[20].Min() - 1),//刻度最小值
                    PositionTier = 21,//y轴地址
                    AxislineStyle = LineStyle.Solid,
                    AxislineColor = OxyColors.Indigo,//颜色
                    MinorTicklineColor = OxyColors.Indigo,//颜色
                    TicklineColor = OxyColors.Indigo,//颜色
                    TextColor = OxyColors.Indigo,//颜色
                    FontSize = 9.0,
                    IsAxisVisible = false,
                    MajorStep = x,
                    //  MajorStep = 50,
                    MinorTickSize = 0,
                };
                _myPlotModel.Axes.Add(_valueAxis20);//添加声明
                series21 = new OxyPlot.Series.LineSeries()//声明曲线
                {
                    Color = OxyColors.Indigo,//颜色
                    StrokeThickness = 1,
                    MarkerSize = 3,
                    MarkerStroke = OxyColors.Indigo,//颜色
                    MarkerType = MarkerType.None,
                    YAxisKey = curve_name[20],//绑定识别代码
                    ItemsSource = list_sum[20]//数据源
                };
                _valueAxis21.IsAxisVisible = true;//声明是否可见
                _myPlotModel.Series.Add(series21);//添加曲线
            }

            curver_ls.Model = _myPlotModel;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
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
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
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
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
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
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
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
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
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
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox7.Checked == true)
                {
                    _valueAxis7.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series7);
                }
                if (checkBox7.Checked == false)
                {
                    _valueAxis7.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series7);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox8.Checked == true)
                {
                    _valueAxis8.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series8);
                }
                if (checkBox8.Checked == false)
                {
                    _valueAxis8.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series8);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox9.Checked == true)
                {
                    _valueAxis9.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series9);
                }
                if (checkBox9.Checked == false)
                {
                    _valueAxis9.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series9);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox10_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox10.Checked == true)
                {
                    _valueAxis10.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series10);
                }
                if (checkBox10.Checked == false)
                {
                    _valueAxis10.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series10);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox11_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox11.Checked == true)
                {
                    _valueAxis11.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series11);
                }
                if (checkBox11.Checked == false)
                {
                    _valueAxis11.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series11);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox12_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox12.Checked == true)
                {
                    _valueAxis12.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series12);
                }
                if (checkBox12.Checked == false)
                {
                    _valueAxis12.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series12);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox13.Checked == true)
                {
                    _valueAxis13.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series13);
                }
                if (checkBox13.Checked == false)
                {
                    _valueAxis13.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series13);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox14_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox14.Checked == true)
                {
                    _valueAxis14.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series14);
                }
                if (checkBox14.Checked == false)
                {
                    _valueAxis14.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series14);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox15_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox15.Checked == true)
                {
                    _valueAxis15.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series15);
                }
                if (checkBox15.Checked == false)
                {
                    _valueAxis15.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series15);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox16_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox16.Checked == true)
                {
                    _valueAxis16.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series16);
                }
                if (checkBox16.Checked == false)
                {
                    _valueAxis16.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series16);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox17_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox17.Checked == true)
                {
                    _valueAxis17.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series17);
                }
                if (checkBox17.Checked == false)
                {
                    _valueAxis17.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series17);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox18.Checked == true)
                {
                    _valueAxis18.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series18);
                }
                if (checkBox18.Checked == false)
                {
                    _valueAxis18.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series18);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox19_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox19.Checked == true)
                {
                    _valueAxis19.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series19);
                }
                if (checkBox19.Checked == false)
                {
                    _valueAxis19.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series19);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox20_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox20.Checked == true)
                {
                    _valueAxis20.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series20);
                }
                if (checkBox20.Checked == false)
                {
                    _valueAxis20.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series20);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkBox21_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                curver_ls.Model = null;
                if (checkBox21.Checked == true)
                {
                    _valueAxis21.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series21);
                }
                if (checkBox21.Checked == false)
                {
                    _valueAxis21.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series21);
                }
                curver_ls.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}