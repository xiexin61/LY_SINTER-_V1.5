using DataBase;
using LiveCharts.Wpf;
using LY_SINTER.Custom;
using NBSJ_PICAL;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
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
    public partial class Frm_Bed_Permeability_Adjust : Form
    {
        private NBSJ_PICAL.Console_lidu console_Lidu = new Console_lidu();
        public vLog _vLog { get; set; }
        public static bool isopen = false;
        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        /// <summary>
        /// 柱形图数据x
        /// </summary>
        private List<string> list_name = new List<string> { "高返配比", "生石灰配比%", "烧返配比%", "混合料碱度", "混合料Mgo", "混合料固定碳", "一混后检测水分", "布料厚度", "点火段温度", "烧结机速度", "主抽废气流量和", "1#风箱温度", "1#风箱负压" };

        /// <summary>
        /// 柱形图数据y
        /// </summary>
        private List<double> list_data = new List<double>();

        public Frm_Bed_Permeability_Adjust()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Course\\Frm_Bed_Permeability_Adjust\\");
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            //设置控件背景颜色

            curve();
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

        /// <summary>
        /// 计算按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                double[] _result = new double[13];
                string sql = "select count(TIMESTAMP) as count from M_PICAL_BREATH_RESULT where TIMESTAMP >= '" + textBox_begin.Text.ToString() + "' and TIMESTAMP <= '" + textBox_end.Text.ToString() + "' ";
                DataTable dataTable = dBSQL.GetCommand(sql);
                if (dataTable.Rows.Count > 0 && dataTable != null)
                {
                    int count = int.Parse(dataTable.Rows[0]["count"].ToString());
                    if (count < 14)
                    {
                        MessageBox.Show("参加计算的数据不足，请重新选择时间");
                        return;
                    }
                    else
                    {
                        List<string> _A = new List<string>();
                        List<Double> _B = new List<double>();
                        Console_lidu console_ = new Console_lidu();
                        _result = console_.BEISHAO_JICHUSHUJU(count);

                        #region 判断勾选状态

                        if (checkBox1.Checked == true)
                        {
                            _A.Add(checkBox1.Text.ToString());
                            _B.Add(Math.Round(_result[0], 2));
                        }
                        if (checkBox2.Checked == true)
                        {
                            _A.Add(checkBox2.Text.ToString());
                            _B.Add(Math.Round(_result[1], 2));
                        }
                        if (checkBox3.Checked == true)
                        {
                            _A.Add(checkBox3.Text.ToString());
                            _B.Add(Math.Round(_result[2], 2));
                        }
                        if (checkBox4.Checked == true)
                        {
                            _A.Add(checkBox4.Text.ToString());
                            _B.Add(Math.Round(_result[3], 2));
                        }
                        if (checkBox5.Checked == true)
                        {
                            _A.Add(checkBox5.Text.ToString());
                            _B.Add(Math.Round(_result[4], 2));
                        }
                        if (checkBox6.Checked == true)
                        {
                            _A.Add(checkBox6.Text.ToString());
                            _B.Add(Math.Round(_result[5], 2));
                        }
                        if (checkBox7.Checked == true)
                        {
                            _A.Add(checkBox7.Text.ToString());
                            _B.Add(Math.Round(_result[6], 2));
                        }
                        if (checkBox8.Checked == true)
                        {
                            _A.Add(checkBox8.Text.ToString());
                            _B.Add(Math.Round(_result[7], 2));
                        }
                        if (checkBox9.Checked == true)
                        {
                            _A.Add(checkBox9.Text.ToString());
                            _B.Add(Math.Round(_result[8], 2));
                        }
                        if (checkBox10.Checked == true)
                        {
                            _A.Add(checkBox10.Text.ToString());
                            _B.Add(Math.Round(_result[9], 2));
                        }
                        if (checkBox11.Checked == true)
                        {
                            _A.Add(checkBox11.Text.ToString());
                            _B.Add(Math.Round(_result[10], 2));
                        }
                        if (checkBox12.Checked == true)
                        {
                            _A.Add(checkBox12.Text.ToString());
                            _B.Add(Math.Round(_result[11], 2));
                        }
                        if (checkBox13.Checked == true)
                        {
                            _A.Add(checkBox13.Text.ToString());
                            _B.Add(Math.Round(_result[12], 2));
                        }

                        #endregion 判断勾选状态

                        PlotModel _myPlotModel = new PlotModel();
                        //X轴定义
                        CategoryAxis _categoryAxis = new CategoryAxis()
                        {
                            MajorTickSize = 0,
                            IsZoomEnabled = false,
                            Position = AxisPosition.Bottom,
                        };
                        for (int i = 0; i < _A.Count(); i++)
                        {
                            _categoryAxis.Labels.Add(_A[i]);     //添加x坐标
                        }
                        _myPlotModel.Axes.Add(_categoryAxis);
                        //Y轴定义
                        LinearAxis _valueAxis = new LinearAxis()
                        {
                            MinorTickSize = 0,
                            Key = "y",
                        };
                        _myPlotModel.Axes.Add(_valueAxis);
                        var _ColumnSeries = new OxyPlot.Series.ColumnSeries();
                        for (int i = 0; i < _B.Count; i++)
                        {
                            _ColumnSeries.Items.Add(new ColumnItem() { Value = _B[i] });
                        }
                        _myPlotModel.Series.Add(_ColumnSeries);
                        curve_his.Model = _myPlotModel;

                        if (list_name.Count < 1)
                        {
                            MessageBox.Show("请选择需要计算的数据");
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("此时间段无数据");
                    return;
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("离线计算失败" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 全选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (simpleButton1.Text == "全选")
            {
                checkBox1.Checked = true;

                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox4.Checked = true;
                checkBox5.Checked = true;
                checkBox6.Checked = true;
                checkBox7.Checked = true;
                checkBox8.Checked = true;
                checkBox9.Checked = true;
                checkBox10.Checked = true;
                checkBox11.Checked = true;
                checkBox12.Checked = true;
                checkBox13.Checked = true;
                simpleButton1.Text = "取消全选";
            }
            else
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;
                checkBox9.Checked = false;
                checkBox10.Checked = false;
                checkBox11.Checked = false;
                checkBox12.Checked = false;
                checkBox13.Checked = false;
                simpleButton1.Text = "全选";
            }
        }

        /// <summary>
        /// 柱形图初始化
        /// </summary>
        public void curve()
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
            }
        }
    }
}