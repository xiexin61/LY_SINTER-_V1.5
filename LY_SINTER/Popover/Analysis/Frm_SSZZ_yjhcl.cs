using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataBase;
using LY_SINTER.Custom;

namespace LY_SINTER.Popover.Analysis
{
    public partial class Frm_SSZZ_yjhcl : Form
    {
        public static bool isopen = false;

        public Frm_SSZZ_yjhcl()
        {
            InitializeComponent();
            dateTimePicker_value();
            //DateTimeChoser.AddTo(textBox_begin);
            //DateTimeChoser.AddTo(textBox_end);
            DateTime start = DateTime.Now.AddYears(-1);
            DateTime end = DateTime.Now;
            shuju(start,end);
        }

        /// <summary> 开始时间&结束时间赋值 </summary>
        public void dateTimePicker_value()
        {
            try
            {
                textBox_begin.Text = DateTime.Now.AddYears(-1).ToString();
                textBox_end.Text = DateTime.Now.ToString();
                /*Validate();
                Update();
                Refresh();*/
                /*//结束时间
                DateTime time_end = DateTime.Now;
                //开始时间
                DateTime time_begin = time_end.AddMonths(-1);

                textBox_begin.Text = time_begin.ToString();
                textBox_end.Text = time_end.ToString();
                dateTimePicker1.Text = time_end.ToString();*/
            }
            catch (Exception ee)
            {
            }
        }

        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        private void simpleButton2_click(object sender, EventArgs e)
        {
            DateTime d1 = Convert.ToDateTime(textBox_begin.Text);
            DateTime d2 = Convert.ToDateTime(textBox_end.Text);
            shuju(d1, d2);
        }

        //添加按钮
        private void simpleButton3_click(object sender, EventArgs e)
        {
            if (yjhcl.Text == "")
            {
                MessageBox.Show("请输入月计划产量");
            }
            else
            {
                float tmp;
                if (!float.TryParse(yjhcl.Text, out tmp))//如果转换失败（为false）时输出括号内容
                {
                    MessageBox.Show("请正确输入数字");
                }
                else
                {
                    double yjhcl1 = Convert.ToDouble(yjhcl.Text);
                    DateTime date_now = dateTimePicker1.Value;
                    string scrq = date_now.ToString("yyyyMM");
                    string sql1 = "select * from MC_POPCAL_MON_PL where POPCAL_MON='" + scrq + "'";
                    DataTable table = dBSQL.GetCommand(sql1);
                    if (table.Rows.Count != 0)
                    {
                        MessageBox.Show("该月份产量已存在");
                    }
                    else
                    {
                        string sql2 = "insert into MC_POPCAL_MON_PL(TIMESTAMP,POPCAL_MON,POPCAL_MON_PL,FLAG_1) values ('" + DateTime.Now + "','" + scrq + "','" + yjhcl1 + "',2)";
                        dBSQL.CommandExecuteNonQuery(sql2);
                        MessageBox.Show("插入成功！");
                        shuju(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(dateTimePicker1.Text));
                    }
                }
            }
        }

        public void shuju(DateTime d1, DateTime d2)
        {
            string start = d1.Year.ToString() + d1.Month.ToString("00");
            string end = d2.Year.ToString() + d2.Month.ToString("00");
            DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            string sql = "select POPCAL_MON,POPCAL_MON_PL,TIMESTAMP,RE_TIME from MC_POPCAL_MON_PL where POPCAL_MON between '" + start + "' and '" + end + "' order by POPCAL_MON desc;";
            DataTable table = dBSQL.GetCommand(sql);
            if (table.Rows.Count > 0)
            {
                this.dataGridView1.DataSource = table;
            }
            dataGridView1.Columns["TIMESTAMP"].DisplayIndex = 2;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Column7" && e.RowIndex >= 0)
                {
                    DataGridViewButtonCell bc = dataGridView1.Rows[e.RowIndex].Cells["Column7"] as DataGridViewButtonCell;
                    if (bc.Tag == null)
                    {
                        bc.Value = "删除";
                        bc.Tag = true;
                    }
                    if (bc.Value == "删除")
                    {
                        if (MessageBox.Show("确实要删除该行吗?", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            string time = dataGridView1.Rows[e.RowIndex].Cells["TIMESTAMP"].Value.ToString();
                            string sql = "delete from MC_POPCAL_MON_PL where TIMESTAMP='" + time + "'";
                            dBSQL.CommandExecuteNonQuery(sql);
                            MessageBox.Show("删除成功！");
                            //刷新数据
                            shuju(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));
                            //**********操作存入日志表
                            try
                            {
                                DateTime dateTime = DateTime.Now;
                                string name = "删除";
                                string incident = "用户点击删除纪录按钮";
                                string modelname = "生产组织计划";

                                /*LogTable_TEXT logTable = new LogTable_TEXT();
                                logTable.operation_log(dateTime, modelname, name, incident);*/
                            }
                            catch (Exception ee) { }
                        }
                    }
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "Column6" && e.RowIndex >= 0)
                {
                    DataGridViewButtonCell bc = dataGridView1.Rows[e.RowIndex].Cells["Column6"] as DataGridViewButtonCell;
                    if (bc.Tag == null)
                    {
                        bc.Value = "修改";
                        bc.Tag = true;
                        //return;//Lee@20210315:不知当初为什么要判断bc.Tag
                    }
                    if (bc.Value == "保存")
                    {
                        string month = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                        string cl = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                        bc.Value = "修改";
                        /*dataGridView1.Rows[e.RowIndex].Cells[1].ReadOnly = true;
                        dataGridView1.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.Black;*/
                        dataGridView1.Rows[e.RowIndex].Cells[3].ReadOnly = true;
                        dataGridView1.Rows[e.RowIndex].Cells[3].Style.ForeColor = Color.Black;
                        string sql1 = "update MC_POPCAL_MON_PL set POPCAL_MON_PL='" + cl + "',RE_TIME='" + DateTime.Now + "',FLAG_1=1 where POPCAL_MON='" + month + "'";
                        dBSQL.CommandExecuteNonQuery(sql1);
                        MessageBox.Show("修改成功！");
                        //刷新数据
                        shuju(Convert.ToDateTime(textBox_begin.Value), Convert.ToDateTime(textBox_end.Value));
                        try
                        {
                            DateTime dateTime = DateTime.Now;
                            string name = "修改";
                            string incident = "用户点击修改/保存按钮";
                            string modelname = "生产组织计划";

                            /*LogTable_TEXT logTable = new LogTable_TEXT();
                            logTable.operation_log(dateTime, modelname, name, incident);*/
                        }
                        catch (Exception ee)
                        {
                        }
                    }
                    else if (bc.Value == "修改")
                    {
                        bc.Value = "保存";
                        dataGridView1.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                        dataGridView1.Rows[e.RowIndex].Cells[3].Style.ForeColor = Color.Red;
                    }
                }
            }
            catch
            {
            }
        }
    }
}