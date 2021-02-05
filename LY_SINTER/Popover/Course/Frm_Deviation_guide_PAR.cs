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
using VLog;

namespace LY_SINTER.Popover.Course
{
    public partial class Frm_Deviation_guide_PAR : Form
    {
        public vLog _vLog { get; set; }
        public static bool isopen = false;
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        /// <summary>
        /// 参数表
        /// </summary>
        String biaoming = "";
        /// <summary>
        /// 参数说明表
        /// </summary>
        String biaoming1 = "";
        /// <summary>
        /// 参数历史表
        /// </summary>
        String biaoming2 = "";
        public Frm_Deviation_guide_PAR()
        {
            InitializeComponent();
            time_begin_end();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Quality\\Frm_Deviation_guide_PAR\\");
            DATE_NAME();
            comboBox1_SelectedIndexChanged();
        }
        public void DATE_NAME()
        {
            biaoming = "MC_UNIFORMCAL_PAR";
            biaoming1 = "MC_UNIFORMCAL_PAR_DESC";
            biaoming2 = "MC_UNIFORMCAL_PAR_CHANGE";
        }
        /// <summary>
        /// 开始时间-结束时间赋值
        /// </summary>
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddDays(-1);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();

        }
        //主体按下拉框查询
        private void comboBox1_SelectedIndexChanged() //object sender, EventArgs e
        {
            DATE_NAME();
            select1();
            select2();
        }
        //实时数据初始化
        private void select1()
        {
            try
            {

                //查询出数据行数
                string sql2 = "SELECT COUNT(NAME) FROM " + biaoming1 + " where state = '1'";
                DataTable dataTable2 = dBSQL.GetCommand(sql2);
                int count = int.Parse(dataTable2.Rows[0][0].ToString());
                if (count != 0)
                {
                    dataGridView1.RowCount = count;
                    //序号
                    for (int j = 0; j < dataGridView1.RowCount; j++)
                    {
                        dataGridView1.Rows[j].Cells[0].Value = Convert.ToString(j);
                    }
                    //取出字段名
                    string sql = "SELECT NAME FROM " + biaoming1 + " where state = '1' order by ORDER_FLAG asc";
                    DataTable dataTable1 = dBSQL.GetCommand(sql);

                    //左面表格填充数据
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        string sql1 = "select b.column_def,a." + dataTable1.Rows[i][0] + ",b.unit,b.NAME from " + biaoming + " a ," + biaoming1 + " b where b.name='" + dataTable1.Rows[i][0] + "'";
                        DataTable dataTable = dBSQL.GetCommand(sql1);
                        dataGridView1.Rows[i].Cells[1].Value = Convert.ToString(dataTable.Rows[0][0]);
                        dataGridView1.Rows[i].Cells[2].Value = Convert.ToString(dataTable.Rows[0][1]);
                        dataGridView1.Rows[i].Cells[3].Value = Convert.ToString(dataTable.Rows[0][2]);
                        dataGridView1.Rows[i].Cells[4].Value = Convert.ToString(dataTable.Rows[0][3]);
                        this.dataGridView1.Rows[i].Cells[0].ReadOnly = true;
                        this.dataGridView1.Rows[i].Cells[1].ReadOnly = true;
                        this.dataGridView1.Rows[i].Cells[2].ReadOnly = true;
                        this.dataGridView1.Rows[i].Cells[3].ReadOnly = true;
                        this.dataGridView1.Rows[i].Cells[4].ReadOnly = true;

                    }
                }
                else MessageBox.Show("没有数据！");

            }
            catch (Exception ee)
            {
                _vLog.writelog("select1方法报错" + ee.ToString(), -1);
            }
        }
        //历史数据初始化
        private void select2()
        {
            try
            {
                DataTable data_select2 = new DataTable();
                data_select2.Columns.Add("记录号");
                data_select2.Columns.Add("时间");
                //获取所有的列明
                string[] name_ye = new string[dataGridView1.Rows.Count];
                for (int x = 0; x < dataGridView1.Rows.Count; x++)
                {
                    name_ye[x] = dataGridView1.Rows[x].Cells["Column5"].Value.ToString();
                }
                //拼接字符串
                string text = "";
                int row_ye = name_ye.Count() - 1;
                for (int x = 0; x < name_ye.Count(); x++)
                {
                    //判断最后一个字段没有逗号
                    if (x != row_ye)
                    {
                        text += name_ye[x] + ",";
                    }
                    else
                    {
                        text += name_ye[x];
                    }

                }
                string sql = "select top (20) " + text + " from " + biaoming2 + " order by TIMESTAMP desc";
                DataTable data_text = dBSQL.GetCommand(sql);
                for (int col = 1; col < dataGridView1.Rows.Count; col++)
                {
                    data_select2.Columns.Add(col.ToString());
                }

                //    初始化显示20条数据
                for (int x = 0; x < data_text.Rows.Count; x++)
                {
                    DataRow data = data_select2.NewRow();

                    data["记录号"] = x + 1;
                    for (int row = 0; row < data_text.Columns.Count; row++)
                    {
                        data[row + 1] = data_text.Rows[x][row].ToString();
                    }
                    data_select2.Rows.Add(data);
                }



                dataGridView2.DataSource = data_select2;
            }
            catch
            {

            }




        }
        /// <summary>
        /// 更新数据库操作
        /// </summary>
        private void update()
        {
            try
            {
                //查询出行数
                //string sql2 = "SELECT COUNT(NAME) FROM " + biaoming1 + "";
                //DataTable dataTable2 = dBSQL.GetCommand(sql2);
                //int count = int.Parse(dataTable2.Rows[0][0].ToString());
                //dataGridView1.RowCount = count;
                string time_now = DateTime.Now.ToString();
                //插入到历史表中
                string sql4 = "insert into " + biaoming2 + " (TIMESTAMP) VALUES  ('" + time_now + "')";
                dBSQL.CommandExecuteNonQuery(sql4);
                //修改
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {

                    string sql1 = "update " + biaoming + " SET " + dataGridView1.Rows[i].Cells[4].Value + "='" + dataGridView1.Rows[i].Cells[2].Value + "'";
                    dBSQL.CommandExecuteNonQuery(sql1);


                    string sql3 = "update " + biaoming1 + " SET column_def='" + dataGridView1.Rows[i].Cells[1].Value + "',unit='" + dataGridView1.Rows[i].Cells[3].Value + "' where name='" + dataGridView1.Rows[i].Cells[4].Value + "'";
                    dBSQL.CommandExecuteNonQuery(sql3);
                    //根据时间循环修改
                    if (i > 0)
                    {
                        string sql5 = "update " + biaoming2 + " set " + dataGridView1.Rows[i].Cells[4].Value + " = '" + dataGridView1.Rows[i].Cells[2].Value + "' where TIMESTAMP = '" + time_now + "'";
                        dBSQL.CommandExecuteNonQuery(sql5);
                    }
                }
            }
            catch (Exception ee)
            {

            }
        }
        /// <summary>
        /// 实时数据导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
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
        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {

            if (simpleButton4.Text == "修改")
            {
                //设置为只读
                this.dataGridView1.Columns[0].ReadOnly = true;
                //设置为可编辑
                this.dataGridView1.Columns[1].ReadOnly = false;
                //变换背景颜色
                this.dataGridView1.Columns[1].DefaultCellStyle.BackColor = Color.FromArgb(217, 253, 220);
                this.dataGridView1.Columns[2].ReadOnly = false;
                this.dataGridView1.Columns[2].DefaultCellStyle.BackColor = Color.FromArgb(217, 253, 220);
                this.dataGridView1.Columns[3].ReadOnly = false;
                this.dataGridView1.Columns[3].DefaultCellStyle.BackColor = Color.FromArgb(217, 253, 220);
                this.dataGridView1.Columns[4].ReadOnly = true;
                //this.simpleButton4.BackColor = Color.LimeGreen;
                simpleButton4.Text = "保存";
                return;
            }
            if (simpleButton4.Text == "保存")
            {
                update();
                select1();
                this.dataGridView1.Columns[1].ReadOnly = true;
                this.dataGridView1.Columns[1].DefaultCellStyle.BackColor = Color.White;
                this.dataGridView1.Columns[2].ReadOnly = true;
                this.dataGridView1.Columns[2].DefaultCellStyle.BackColor = Color.White;
                this.dataGridView1.Columns[3].ReadOnly = true;
                this.dataGridView1.Columns[3].DefaultCellStyle.BackColor = Color.White;

                this.simpleButton4.BackColor = Color.White;
                simpleButton4.Text = "修改";
                select2();
                MessageBox.Show("修改完成！");

                return;
            }

        }
        /// <summary>
        /// 历史数据查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //开始时间
            DateTime d1 = Convert.ToDateTime(textBox_begin.Text);
            //结束时间
            DateTime d2 = Convert.ToDateTime(textBox_end.Text);
            DataTable data_select2 = new DataTable();
            data_select2.Columns.Add("记录号");
            data_select2.Columns.Add("时间");
            //获取所有的列明
            string[] name_ye = new string[dataGridView1.Rows.Count];
            for (int x = 0; x < dataGridView1.Rows.Count; x++)
            {
                name_ye[x] = dataGridView1.Rows[x].Cells["Column5"].Value.ToString();
            }
            //拼接字符串
            string text = "";
            int row_ye = name_ye.Count() - 1;
            for (int x = 0; x < name_ye.Count(); x++)
            {
                //判断最后一个字段没有逗号
                if (x != row_ye)
                {
                    text += name_ye[x] + ",";
                }
                else
                {
                    text += name_ye[x];
                }

            }
            string sql = "select  " + text + " from " + biaoming2 + " where TIMESTAMP >='" + d1 + "' and TIMESTAMP <='" + d2 + "' order by TIMESTAMP desc";
            DataTable data_text = dBSQL.GetCommand(sql);
            for (int col = 1; col < dataGridView1.Rows.Count; col++)
            {
                data_select2.Columns.Add(col.ToString());
            }


            for (int x = 0; x < data_text.Rows.Count; x++)
            {
                DataRow data = data_select2.NewRow();

                data["记录号"] = x + 1;
                for (int row = 0; row < data_text.Columns.Count; row++)
                {
                    data[row + 1] = data_text.Rows[x][row].ToString();
                }
                data_select2.Rows.Add(data);
            }
            dataGridView2.DataSource = data_select2;

        }
        /// <summary>
        /// 历史数据实时按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click_1(object sender, EventArgs e)
        {
            DATE_NAME();
            select2();
        }
        /// <summary>
        /// 实时数据刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            select1();
            select2();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
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
                    for (int i = 0; i < dataGridView2.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            columnTitle += "\t";
                        }
                        columnTitle += dataGridView2.Columns[i].HeaderText;
                    }
                    sw.WriteLine(columnTitle);

                    //写入列内容    
                    for (int j = 0; j < dataGridView2.Rows.Count; j++)
                    {
                        string columnValue = "";
                        for (int k = 0; k < dataGridView2.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                columnValue += "\t";
                            }
                            if (dataGridView2.Rows[j].Cells[k].Value == null)
                                columnValue += "";
                            else
                                columnValue += dataGridView2.Rows[j].Cells[k].Value.ToString().Trim();
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
    }
}
