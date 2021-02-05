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
using DataBase;

namespace LY_SINTER.Custom
{
    public partial class Alarm : UserControl
    {
        public vLog vLog { get; set; }
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        public System.Timers.Timer _Timer1 { get; set; }
        /// <summary>
        /// 刷新显示数据
        /// </summary>
        public System.Timers.Timer _Timer2 { get; set; }

        public System.Timers.Timer _Timer3 { get; set; }
        public Alarm()
        {
            InitializeComponent();
            if (vLog == null)
                vLog = new vLog(".\\log_Page\\Prompt_Message_Page\\");

            _Timer1 = new System.Timers.Timer(20000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { Timer1_Tick_1(); };
            _Timer1.Enabled = true;
            _Timer1.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer2 = new System.Timers.Timer(20000);//初始化颜色变化定时器响应事件
            _Timer2.Elapsed += (x, y) => { Timer1_Tick_2(); };
            _Timer2.Enabled = true;
            _Timer2.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer3 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer3.Elapsed += (x, y) => { Timer1_Tick_3(); };
            _Timer3.Enabled = false;
            _Timer3.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            data();
        }
        private void data()
        {
            try
            {
                string sql = " Select top (10) timestamp, modelname, funcname, info, (case appraise when '0' then '正确' when '1' then '错误' else '未知' end) as appraise,isnull(FLAG_alarm,0) as FLAG_alarm from LogTable  order by timestamp desc";
                DataTable dataTable = dBSQL.GetCommand(sql);
                if (dataTable.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dataTable;
                }
                for (int x = 0; x < dataGridView1.Rows.Count; x++)
                {
                    //评价状态
                    string PJZT = dataGridView1.Rows[x].Cells["Column5"].Value.ToString();
                    if (PJZT == "正确")
                    {
                        dataGridView1.Rows[x].DefaultCellStyle.ForeColor = Color.Green;
                    }
                    else if (PJZT == "错误")
                    {
                        dataGridView1.Rows[x].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ee)
            {
                string mistake = "数据查询失败" + ee.ToString();
                vLog.writelog(mistake, -1);
            }

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
                if (check.Checked == true)
                {
                    try
                    {
                        //周期查询判断条件为错误
                        for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                        {
                            //标志状态
                            string PJZT = dataGridView1.Rows[rows].Cells["Column6"].Value.ToString();
                            if (PJZT == "0")
                            {
                                pictureBox2.Image = Properties.Resources.red;
                                string sql_update_LogTable = "update LogTable set FLAG_alarm = '1' where FLAG_alarm = '0'";
                                int count = dBSQL.CommandExecuteNonQuery(sql_update_LogTable);
                                if (count <= 0)
                                {
                                    string mistake = "定时去更新LogTable表标志位失败" + sql_update_LogTable;
                                    vLog.writelog(mistake, -1);
                                }
                                break;
                            }
                            else
                            {
                                pictureBox2.Image = Properties.Resources.green;
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        string mistake = "警报器切换失败" + ee.ToString();
                        vLog.writelog(mistake, -1);
                    }
                }
                else
                {
                    pictureBox2.Image = Properties.Resources.green;
                }

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
                data();//刷新数据
            }
        }
        private void Timer1_Tick_3()
        {
            Action invokeAction = new Action(Timer1_Tick_3);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                //try
                //{
                //    //定时器定时去更新LogTable表的标志位
                //    string sql_update_LogTable = "update LogTable set FLAG_alarm = '1' where FLAG_alarm = '0'";
                //    int count =   dBSQL.CommandExecuteNonQuery(sql_update_LogTable);
                //    if (count <=0)
                //    {
                //        string mistake = "定时去更新LogTable表标志位失败" + sql_update_LogTable;
                //        vLog.writelog(mistake, -1);
                //    }
                //}
                //catch (Exception ee)
                //{
                //    string mistake = "定时去更新LogTable表标志位失败" + ee.ToString();
                //    vLog.writelog(mistake, -1);
                //}

            }
        }
        private void check_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //    if (check.Checked == true)
                //    {
                //        this._Timer1.Enabled = true;
                //        //周期查询判断条件为错误
                //        for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                //        {
                //            //评价状态
                //            string PJZT = dataGridView1.Rows[rows].Cells["Column6"].Value.ToString();
                //            if (PJZT == "0")
                //            {
                //                _Timer3.Enabled = true;
                //                pictureBox2.Image = Properties.Resources.red;
                //                return;
                //            }
                //            else
                //            {
                //                pictureBox2.Image = Properties.Resources.green;
                //            }
                //        }
                //        //  pictureBox2.Image = WindowsFormsApp2.Properties.Resources.red;
                //    }
                //    else
                //    if (check.Checked == false)
                //    {
                //        this._Timer1.Enabled = false;
                //        _Timer3.Enabled = false;
                //        pictureBox2.Image = Properties.Resources.green;

                //    }
                //    else
                //    {
                //        return;
                //    }
            }
            catch (Exception ee)
            {
                string mistake = "警报器判断错误" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }
    }
}
