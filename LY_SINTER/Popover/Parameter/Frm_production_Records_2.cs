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
        //声明委托和事件
        public delegate void production_Records_2();
        //声明委托和事件
        public event production_Records_2 _production_Records_2;
        Tuple<bool, Dictionary<string, string>> _Rule_Big;
        Tuple<bool, Dictionary<string, string>> _Rule_Loser;
        Tuple<bool, Dictionary<string, string>> _Rule_Big_1;
        Tuple<bool, Dictionary<string, string>> _Rule_Loser_1;
        public Frm_production_Records_2()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Parameter\\Frm_production_Records_2\\");
            time_begin_end();//开始&结束时间赋值
            Checkbox_value();//下拉框赋值
            _Rule_Big = _Model._GetRule(1);//原因大类对应关系规则
            _Rule_Loser = _Model._GetRule(2);//原因大类对应关系规则
            _Rule_Big_1 = _Model._GetRule_1(1);//原因大类对应关系规则
            _Rule_Loser_1 = _Model._GetRule_1(2);//原因大类对应关系规则
            _Show();
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        public void _Show()
        {
            try
            {
                var _sql = "select * from M_SIN_RUN_STOP where FLAG_1 = "+ FLAG + "";
                DataTable table = dBSQL.GetCommand(_sql);
                if (table.Rows.Count > 0 && table != null)
                {
                    //班次
                    comboBox1.Text = table.Rows[0]["WORK_SHIFT"].ToString();
                    //班别
                    comboBox2.Text = table.Rows[0]["WORK_TEAM"].ToString();
                    //原因大类
                    if(table.Rows[0]["SORT_BIG"].ToString() != "")
                      comboBox3.Text = _Rule_Big_1.Item2[table.Rows[0]["SORT_BIG"].ToString()];
                    //原因小类
                    if (table.Rows[0]["SORT_LITTLE"].ToString() != "")
                        comboBox4.Text = _Rule_Loser_1.Item2[table.Rows[0]["SORT_LITTLE"].ToString()];
                    //开始时间
                    textBox_begin.Value = DateTime.Parse( table.Rows[0]["STOP_BEGINTIME"].ToString());
                    //结束时间
                    if(table.Rows[0]["STOP_ENDTIME"].ToString() != "")
                       textBox_end.Value = DateTime.Parse(table.Rows[0]["STOP_ENDTIME"].ToString());
                   
                    //停机时间
                    textBox1.Text = table.Rows[0]["INTERVAL_TIME"].ToString();
                    //原因描述
                    textBox2.Text = table.Rows[0]["REMARK_DESC"].ToString();
                }
                else
                {
                    _vLog.writelog("_Show方法失败,sql" + _sql, -1);
                }
            }
            catch(Exception EE)
            {
                _vLog.writelog("_Show方法失败" + EE.ToString(),-1);
            }
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
            _update();
          
        }
        /// <summary>
        /// 采集记录
        /// </summary>
        public void _update()
        {
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
                        var sql = "update M_SIN_RUN_STOP set TIMESTAMP = getdate(),WORK_SHIFT = '" + comboBox1.Text.ToString() + "',WORK_TEAM = '" + comboBox2.Text.ToString() + "',REMARK_DESC = '" + textBox2.Text.ToString() + "',STOP_BEGINTIME = '" + textBox_begin.Value.ToString() + "',STOP_ENDTIME = '" + textBox_end.Value.ToString() + "',INTERVAL_TIME = '" + textBox1.Text.ToString() + "',SORT_BIG = '" + _A + "',SORT_LITTLE = '" + _B + "' where FLAG_1 = " + FLAG + "";
                        int _count = dBSQL.CommandExecuteNonQuery(sql);
                        if (_count != 1)
                        {
                            _vLog.writelog("_update方法调用失败，sql" + sql, -1);
                        }
                        else
                        {
                            _production_Records_2();
                            this.Dispose();
                        }
                     
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
    }
}
