using DataBase;
using LY_SINTER.Custom;
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
    public partial class Frm_production_Records_1 : Form
    {
        public static bool isopen = false;
        //声明委托和事件
        public delegate void production_Records_1();
        //声明委托和事件
        public event production_Records_1 _production_Records_1;
        Parameter_Model _Model = new Parameter_Model();//页面modle
        public vLog _vLog { get; set; }
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        Tuple<bool, Dictionary<string, string>> _Rule_Big;
        Tuple<bool, Dictionary<string, string>> _Rule_Loser;
        public Frm_production_Records_1()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Parameter\\Frm_production_Records_1\\");
            time_begin_end();//开始&结束时间赋值
            Checkbox_value();//下拉框赋值
            Change_Min();//分钟间隔计算
            _Rule_Big = _Model._GetRule(1);//原因大类对应关系规则
           _Rule_Loser = _Model._GetRule(2);//原因大类对应关系规则
        }
        /// <summary>
        /// 开始时间-结束时间赋值
        /// </summary>
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddHours(-12);
            textBox_begin.Value = time_begin;
            textBox_end.Value = time_end;

            //计算时间间隔
            DateTime time_1 = textBox_begin.Value;
            DateTime time_2 = textBox_end.Value;
            TimeSpan time_chang = time_end - time_begin;
            double count = time_chang.TotalMinutes;
            if (count > 0)
            {
                textBox1.Text = Math.Round(count, 2).ToString();
            }

        }
        /// <summary>
        /// 下拉框赋值
        /// </summary>
        public void Checkbox_value()
        {
            try
            {
                #region 班次
           
                comboBox1.DataSource = _Model._GetClass(1);
                comboBox1.DisplayMember = "NAME";
                comboBox1.ValueMember = "VALUES";
                #endregion

                #region 班别
             
                comboBox2.DataSource = _Model._GetClass(2);
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
                    if (count > 0 )
                    {
                        textBox1.Text = Math.Round(count, 2).ToString();
                    }
                    else
                    {
                        textBox1.Text ="0";
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
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            _update();
            
        }
        /// <summary>
        /// 采集记录
        /// </summary>
        /// 
        public void _update()
        {
            int x = 1;
            string sql_id = "select max(FLAG_1) from M_SIN_RUN_STOP";
            DataTable table_1 = dBSQL.GetCommand(sql_id);
            if (table_1.Rows.Count > 0 && table_1 != null)
            {
                x = int.Parse(table_1.Rows[0][0].ToString() == "" ? "0" : table_1.Rows[0][0].ToString()) +1;
            }
            //获取原因大类编码
            if (_Rule_Big.Item1)
            {
                if (_Rule_Loser.Item1)
                {
                    Tuple<bool, string, string> _Judge = _Model.Judge_Class_Time(textBox_begin.Value, textBox_end.Value);
                    if (_Judge.Item1)
                    {
                        //原因大类代码
                        string _A = _Rule_Big.Item2[comboBox3.Text.ToString()];
                        //原因小类代码
                        string _B = _Rule_Loser.Item2[comboBox4.Text.ToString()];
                        var sql = "insert into M_SIN_RUN_STOP (TIMESTAMP,WORK_SHIFT,WORK_TEAM,REMARK_DESC,STOP_BEGINTIME,STOP_ENDTIME,INTERVAL_TIME,FLAG,FLAG_1,SORT_BIG,SORT_LITTLE) " +
                        "values (getdate(),'" + comboBox1.Text.ToString() + "','" + comboBox2.Text.ToString() + "','" + textBox2.Text.ToString() + "','" + textBox_begin.Value.ToString() + "','" + textBox_end.Value.ToString() + "','" + textBox1.Text.ToString() + "',2," + x + ",'" + _A + "','" + _B + "')";
                        int _count = dBSQL.CommandExecuteNonQuery(sql);
                        _production_Records_1();
                        this.Dispose();
                    }
                    else
                    {
                        MessageBox.Show("操作失败，选择的开始时间及结束时间已经跨班");
                    }
                }
                else
                {
                    MessageBox.Show("操作失败，检查原因小类数据库对应关系");
                }
            }
            else
            {
                MessageBox.Show("操作失败，检查原因大类数据库对应关系");
            }

           
        }
        private void textBox_end_CloseUp(object sender, EventArgs e)
        {
            Change_Min();//分钟间隔计算
        }
    }
}
