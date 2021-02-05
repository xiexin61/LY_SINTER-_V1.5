using DataBase;
using LiveCharts.Wpf;
using LY_SINTER.Custom;
using NBSJ_PICAL;
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
        public vLog _vLog { get; set; }
        /// <summary>
        /// 曲线控件添加标志位
        /// </summary>
        bool flag_curve = true;
        public static bool isopen = false;
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        ColumnSeries chart_zxt_lxjs { get; set; }
        /// <summary>
        /// 柱形图数据x
        /// </summary>
        List<string> list_name = new List<string> { "高返配比", "生石灰配比%", "烧返配比%", "混合料碱度", "混合料Mgo", "混合料固定碳", "一混后检测水分", "布料厚度", "点火段温度", "烧结机速度", "主抽废气流量和", "1#风箱温度", "1#风箱负压" };
        /// <summary>
        /// 柱形图数据y
        /// </summary>
        List<double> list_data = new List<double>();
        public Frm_Bed_Permeability_Adjust()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Course\\Frm_Bed_Permeability_Adjust\\");
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            //设置控件背景颜色
            this.chart_lxjs.LChart.BackColor = Color.White;
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
                string time_begin = textBox_begin.Text.ToString();
                string time_end = textBox_end.Text.ToString();
                 NBSJ_PICAL.Console_lidu console_Lidu = new Console_lidu();
                double[] _result = new double[13];
                string sql = "select count(TIMESTAMP) as count from M_PICAL_BREATH_RESULT where TIMESTAMP >= '" + time_begin + "' and TIMESTAMP <= '" + time_end + "' ";
                DataTable dataTable = dBSQL.GetCommand(sql);
                if (dataTable.Rows.Count > 0 && dataTable != null)
                {
                    int count = int.Parse(dataTable.Rows[0]["count"].ToString());
                    // _result = console_Lidu.BEISHAO_JICHUSHUJU(count);
                    if (count < 14)
                    {
                        MessageBox.Show("参加计算的数据不足，请重新选择时间");
                        return;
                    }
                    else
                    {
                        chart_lxjs.LChart.Series.Clear();
                        chart_lxjs.LChart.AxisX.Clear();
                        chart_lxjs.LChart.AxisY.Clear();

                        chart_zxt_lxjs = chart_lxjs.MakeCol(0, 0, "chart_zxt_lxjs");
                        chart_lxjs.LChart.Series.Add(chart_zxt_lxjs);

                        list_name.Clear();
                        list_data.Clear();
                        _result = console_Lidu.BEISHAO_JICHUSHUJU(count);

                        #region 判断勾选状态
                        if (checkBox1.Checked == true)
                        {
                            list_name.Add(checkBox1.Text.ToString());
                            list_data.Add(Math.Round(_result[0], 2));
                        }
                        if (checkBox2.Checked == true)
                        {
                            list_name.Add(checkBox2.Text.ToString());
                            list_data.Add(Math.Round(_result[1], 2));
                        }
                        if (checkBox3.Checked == true)
                        {
                            list_name.Add(checkBox3.Text.ToString());
                            list_data.Add(Math.Round(_result[2], 2));
                        }
                        if (checkBox4.Checked == true)
                        {
                            list_name.Add(checkBox4.Text.ToString());
                            list_data.Add(Math.Round(_result[3], 2));
                        }
                        if (checkBox5.Checked == true)
                        {
                            list_name.Add(checkBox5.Text.ToString());
                            list_data.Add(Math.Round(_result[4], 2));
                        }
                        if (checkBox6.Checked == true)
                        {
                            list_name.Add(checkBox6.Text.ToString());
                            list_data.Add(Math.Round(_result[5], 2));
                        }
                        if (checkBox7.Checked == true)
                        {
                            list_name.Add(checkBox7.Text.ToString());
                            list_data.Add(Math.Round(_result[6], 2));
                        }
                        if (checkBox8.Checked == true)
                        {
                            list_name.Add(checkBox8.Text.ToString());
                            list_data.Add(Math.Round(_result[7], 2));
                        }
                        if (checkBox9.Checked == true)
                        {
                            list_name.Add(checkBox9.Text.ToString());
                            list_data.Add(Math.Round(_result[8], 2));
                        }
                        if (checkBox10.Checked == true)
                        {
                            list_name.Add(checkBox10.Text.ToString());
                            list_data.Add(Math.Round(_result[9], 2));
                        }
                        if (checkBox11.Checked == true)
                        {
                            list_name.Add(checkBox11.Text.ToString());
                            list_data.Add(Math.Round(_result[10], 2));
                        }
                        if (checkBox12.Checked == true)
                        {
                            list_name.Add(checkBox12.Text.ToString());
                            list_data.Add(Math.Round(_result[11], 2));
                            if (checkBox13.Checked == true)
                            {
                                list_name.Add(checkBox13.Text.ToString());
                                list_data.Add(Math.Round(_result[12], 2));
                            }
                        }
                        #endregion
                        if (list_name.Count < 1)
                        {
                            MessageBox.Show("请选择需要计算的数据");
                            return;
                        }
                        else
                        {
                            //查询需要计算参与的数据个数
                            int count_zxt = list_name.Count;
                            chart_lxjs.LPageSize = count_zxt;
                            chart_lxjs.LBindDataC<string, double>("chart_zxt_lxjs", list_name, list_data, System.Windows.Media.Brushes.Green);
                            this.chart_lxjs.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示



                        }

                    }
                }
                else
                {
                    MessageBox.Show("此时间段无数据");
                    return;

                }
            }
            catch(Exception ee)
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
            //for (int col = 0; col < list_name.Count; col++)
            //{
            //    list_data.Add(1);
            //}

            //     chart_lxjs.LPageSize = 13;
            //  chart_lxjs.LBindDataC<string, double>("chart_zxt_lxjs", list_name, list_data, System.Windows.Media.Brushes.Green, "", "", 2);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {

            }
        }
    }
}
