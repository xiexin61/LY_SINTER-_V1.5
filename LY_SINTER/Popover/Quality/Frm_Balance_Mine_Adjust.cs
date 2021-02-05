using DataBase;
using LY_SINTER.Custom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LY_SINTER.Popover.Quality
{
    public partial class Frm_Balance_Mine_Adjust : Form
    {
        public static bool isopen = false;
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public Frm_Balance_Mine_Adjust()
        {
            InitializeComponent();
            time_begin_end();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            Text_NOW();
        }
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddDays(-1);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();

        }



        //查询按钮
        private void button1_Click(object sender, EventArgs e)
        {
            Text_TIME(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));

        }


        //实时按钮
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Text_NOW();

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Execl files (*.xls)|*.xls";
                dlg.FilterIndex = 0;
                dlg.RestoreDirectory = true;
                dlg.CreatePrompt = true;
                dlg.Title = "保存为Excel文件";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream;
                    myStream = dlg.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                    string columnTitle = "";
                    try
                    {
                        //写入列标题    
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            if (i > 0)
                            {
                                columnTitle += "\t";
                            }
                            columnTitle += dataGridView1.Columns[i].HeaderText;
                        }
                        sw.WriteLine(columnTitle);

                        //写入列内容    
                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                        {
                            string columnValue = "";
                            for (int k = 0; k < dataGridView1.Columns.Count; k++)
                            {
                                if (k > 0)
                                {
                                    columnValue += "\t";
                                }
                                if (dataGridView1.Rows[j].Cells[k].Value == null)
                                    columnValue += "";
                                else
                                    columnValue += dataGridView1.Rows[j].Cells[k].Value.ToString().Trim();
                            }
                            sw.WriteLine(columnValue);
                        }
                        sw.Close();
                        myStream.Close();
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(e.ToString());
                    }
                    finally
                    {
                        sw.Close();
                        myStream.Close();
                    }
                }

            }
            else MessageBox.Show("请选择参数！");
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 实时按钮
        /// </summary>
        public void Text_NOW()
        {
            if (comboBox1.Text == "配比调整信息")
            {
                string sql_text = "select top (20) " +
             "TIMESTAMP as 时间," +
             //"SRMCAL_SL_FLAG as 烧返配比调整模型状态 ," +
             //"SRMCAL_A_FLAG as 模型调整状态," +
             //"SRMCAL_SIG_15 as '15#启停状态'," +
             //"SRMCAL_SIG_16 as '16#启停状态'," +
             //"SRMCAL_SIG as 烧返仓在用状态," +
             " (case SRMCAL_FLAG when '1' then '人工干预' when '2' then '启停变化' when '3' then '目标仓位变化' when '4' then '仓位超限' when '5' then '周期调用' end) as 调用条件," +
             "(case SRMCAL_RS_FLAG when '1' then '投入' when '2' then '退出' end) as 模型存储下发状态," +
             "SRMCAL_W_15 as '15#实时仓位'," +
             "SRMCAL_W_16 as '16#实时仓位'," +
             "SRMCAL_W as 有效仓位," +
             "SRMCAL_W_AIM as 目标仓位," +
             "SRMCAL_W_LAST as 倒推时刻有效仓位," +
             "SRMCAL_E as 有效仓位与目标偏差," +
             "SRMCAL_EC as 周期内仓位偏差," +
             "SRMCAL_BILL_SP_A as 烧返配比调整量," +
             "SRMCAL_BILL_SP_NEW as 调整后烧返配比," +
             "SRMCAL_BILL_SP_OLD as 调整前烧返配比 " +
             "from MC_SRMCAL_RESULT order by  TIMESTAMP desc";
                DataTable table = dBSQL.GetCommand(sql_text);
                if (table.Rows.Count > 0)
                {
                    dataGridView1.DataSource = table;
                }
                else
                {
                    return;
                }
            }
            else if (comboBox1.Text == "分仓系数调整信息")
            {
                string sql_text = "select top(20) " +
                    "TIMESTAMP AS 时间," +
                    "(case SRMCAL_SIG_15 when '0' then '禁用' when '1' then '启用' end) as '15号仓启停状态'," +
                    "(case SRMCAL_SIG_16 when '0' then '禁用' when '1' then '启用' end) as '16号仓启停状态'," +
                    "SRMCAL_W_15 as'15号烧返仓实时仓位',SRMCAL_W_16 as '16号烧返仓实时仓位'," +
                    "SRMCAL_S_15 as '15号烧返仓分仓系数',SRMCAL_S_16 as '16号烧返仓分仓系数'," +
                    " (CASE FLAG WHEN '0' then'偏差正常' when '1' then '偏差异常' when '2' then '偏差异常且已弹框' when '3' then '偏差正常并已经使用' end) as 烧返仓仓位偏差 " +
                    " from MC_SRMCAL_RESULT_DIST_T  order by  TIMESTAMP desc";
                DataTable table = dBSQL.GetCommand(sql_text);
                if (table.Rows.Count > 0)
                {
                    dataGridView1.DataSource = table;
                }
                else
                {
                    return;
                }
            }

        }

        public void Text_TIME(DateTime _Begin_time, DateTime _End_time)
        {
            if (comboBox1.Text == "配比调整信息")
            {
                string sql_text = "select " +
               "TIMESTAMP as 时间," +
             //"SRMCAL_SL_FLAG as 烧返配比调整模型状态 ," +
             //"SRMCAL_A_FLAG as 模型调整状态," +
             //"SRMCAL_SIG_15 as '15#启停状态'," +
             //"SRMCAL_SIG_16 as '16#启停状态'," +
             //"SRMCAL_SIG as 烧返仓在用状态," +
             " (case SRMCAL_FLAG when '1' then '人工干预' when '2' then '启停变化' when '3' then '目标仓位变化' when '4' then '仓位超限' when '5' then '周期调用' end) as 调用条件," +
             "(case SRMCAL_RS_FLAG when '1' then '投入' when '2' then '退出' end) as 模型存储下发状态," +
             "SRMCAL_W_15 as '15#实时仓位'," +
             "SRMCAL_W_16 as '16#实时仓位'," +
             "SRMCAL_W as 有效仓位," +
             "SRMCAL_W_AIM as 目标仓位," +
             "SRMCAL_W_LAST as 倒推时刻有效仓位," +
             "SRMCAL_E as 有效仓位与目标偏差," +
             "SRMCAL_EC as 周期内仓位偏差," +
             "SRMCAL_BILL_SP_A as 烧返配比调整量," +
             "SRMCAL_BILL_SP_NEW as 调整后烧返配比," +
             "SRMCAL_BILL_SP_OLD as 调整前烧返配比 " +
               "from MC_SRMCAL_RESULT where TIMESTAMP >='" + _Begin_time + "' and TIMESTAMP <='" + _End_time + "'";
                DataTable table = dBSQL.GetCommand(sql_text);
                if (table.Rows.Count > 0)
                {
                    dataGridView1.DataSource = table;
                }
                else
                {
                    return;
                }
            }
            else if (comboBox1.Text == "分仓系数调整信息")
            {
                string sql_text = "select  " +
                    "TIMESTAMP AS 时间," +
                    "(case SRMCAL_SIG_15 when '0' then '禁用' when '1' then '启用' end) as '15号仓启停状态'," +
                    "(case SRMCAL_SIG_16 when '0' then '禁用' when '1' then '启用' end) as '16号仓启停状态'," +
                    "SRMCAL_W_15 as'15号烧返仓实时仓位',SRMCAL_W_16 as '16号烧返仓实时仓位'," +
                    "SRMCAL_S_15 as '15号烧返仓分仓系数',SRMCAL_S_16 as '16号烧返仓分仓系数'," +
                    " (CASE FLAG WHEN '0' then'偏差正常' when '1' then '偏差异常' when '2' then '偏差异常且已弹框'" +
                    " when '3' then '偏差正常并已经使用' end) as 烧返仓仓位偏差 " +
                    " from MC_SRMCAL_RESULT_DIST_T where TIMESTAMP >='" + _Begin_time + "' and TIMESTAMP <='" + _End_time + "'";
                DataTable table = dBSQL.GetCommand(sql_text);
                if (table.Rows.Count > 0)
                {
                    dataGridView1.DataSource = table;
                }
                else
                {
                    return;
                }
            }

        }
    }
}
