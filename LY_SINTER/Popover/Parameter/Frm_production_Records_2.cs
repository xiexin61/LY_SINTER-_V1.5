using DataBase;
using LY_SINTER.Model;
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

namespace LY_SINTER.Popover.Parameter
{
    public partial class Frm_production_Records_2 : Form
    {
        int FLAG = Quality_Model.FLAG_1;//接收主页面选中的标志位
        public static bool isopen = false;
        Parameter_Model _Model = new Parameter_Model();
        public vLog _vLog { get; set; }
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public Frm_production_Records_2()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Parameter\\Frm_production_Records_2\\");
            time_begin_end();//开始&结束时间赋值
            Checkbox_value();//下拉框赋值
        }
        /// <summary>
        /// 开始时间-结束时间赋值
        /// </summary>
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddDays(-1);
            textBox_begin.Value = time_begin;
            textBox_end.Value = time_end;

        }
        /// <summary>
        /// 下拉框赋值
        /// </summary>
        public void Checkbox_value()
        {
            try
            {
                #region 班次
                DataTable data_1 = new DataTable();
                data_1.Columns.Add("NAME");
                data_1.Columns.Add("VALUES");
                DataRow row1_1 = data_1.NewRow();
                row1_1["NAME"] = "夜";
                row1_1["VALUES"] = 1;
                data_1.Rows.Add(row1_1);
                DataRow row1_2 = data_1.NewRow();
                row1_2["NAME"] = "白";
                row1_2["VALUES"] = 2;
                data_1.Rows.Add(row1_2);
                comboBox1.DataSource = data_1;
                comboBox1.DisplayMember = "NAME";
                comboBox1.ValueMember = "VALUES";
                #endregion

                #region 班别
                DataTable data_2 = new DataTable();
                data_2.Columns.Add("NAME");
                data_2.Columns.Add("VALUES");
                DataRow row2_1 = data_2.NewRow();
                row2_1["NAME"] = "甲";
                row2_1["VALUES"] = 1;
                data_2.Rows.Add(row2_1);
                DataRow row2_2 = data_2.NewRow();
                row2_2["NAME"] = "乙";
                row2_2["VALUES"] = 2;
                data_2.Rows.Add(row2_2);
                DataRow row2_3 = data_2.NewRow();
                row2_3["NAME"] = "丙";
                row2_3["VALUES"] = 3;
                data_2.Rows.Add(row2_3);
                DataRow row2_4 = data_2.NewRow();
                row2_4["NAME"] = "丁";
                row2_4["VALUES"] = 3;
                data_2.Rows.Add(row2_4);
                comboBox2.DataSource = data_2;
                comboBox2.DisplayMember = "NAME";
                comboBox2.ValueMember = "VALUES";
                #endregion

                #region 原因大类
                Tuple<bool, DataTable> _tuple_1 = _Model._GetTs(1);
                if (_tuple_1.Item1)
                {
                    comboBox3.DataSource = _tuple_1.Item2;
                    comboBox3.DisplayMember = "NAME";
                    comboBox3.ValueMember = "VALUES";
                }
                else
                {
                    _vLog.writelog("Parameter_Model页面模型方法_GetTs（1）调用失败", -1);
                }
                #endregion

                #region 原因小类
                Tuple<bool, DataTable> _tuple_2 = _Model._GetTs(2);
                if (_tuple_2.Item1)
                {
                    comboBox4.DataSource = _tuple_2.Item2;
                    comboBox4.DisplayMember = "NAME";
                    comboBox4.ValueMember = "VALUES";
                }
                else
                {
                    _vLog.writelog("Parameter_Model页面模型方法_GetTs（2）调用失败", -1);
                }
                #endregion
            }
            catch (Exception ee)
            {
                _vLog.writelog("Checkbox_value方法错误" + ee.ToString(), -1);
            }
        }
        /// <summary>
        /// 选择时间发生变化，计算分钟差
        /// </summary>
        public void Change_Min()
        {
            try
            {
                if (textBox_begin.Text.ToString() != "" && textBox_end.Text.ToString() != "")
                {
                    DateTime time_1 = textBox_begin.Value;
                    DateTime time_2 = textBox_end.Value;
                    TimeSpan time_chang = time_2 - time_1;
                    double count = time_chang.TotalMinutes;
                    if (count > 0)
                    {
                        textBox1.Text = Math.Round(count, 2).ToString();
                    }
                    else
                    {
                        textBox1.Text = "0";
                        MessageBox.Show("结束时间需要大于开始时间", "注意");
                    }

                }
                else
                {
                    return;
                }
            }
            catch
            {

            }
        }

        private void textBox_end_ValueChanged(object sender, EventArgs e)
        {
            Change_Min();//计算分钟差
        } 
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
