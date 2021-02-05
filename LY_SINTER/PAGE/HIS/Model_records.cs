using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataBase;
using LY_SINTER.Custom;
using System.IO;

namespace LY_SINTER.PAGE.HIS
{
    public partial class Model_records : UserControl
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public Model_records()
        {
            InitializeComponent();
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            dataGridView1_CellContentClick();//dataGridView不显示行标题列
            time();//最新调整时间
            shuju();
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
        //最新调整时间
        private void time()
        {
            try
            {
                string sql1 = "select top 1 timestamp from  LogTable order by timestamp desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    string zxsj = dataTable1.Rows[0][0].ToString();
                    label5.Text = "最新调整时间:" + zxsj;
                }
                else
                {
                    label5.Text = "";
                }
            }
            catch
            {
            }
        }
        private void shuju()
        {
            try
            {
                comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                try
                {
                    dataGridView1.Columns[5].Visible = true;
                    if (comboBox1.Text != "")
                    {
                        if (comboBox1.Text == "模型调整")
                        {
                            string sql1 = "select count(timestamp) from  LogTable where FLAG=1 ";
                            DataTable dataTable1 = dBSQL.GetCommand(sql1);
                            int mxtzgs = int.Parse(dataTable1.Rows[0][0].ToString());
                            if (mxtzgs != 0)
                            {
                                if (mxtzgs < 40)
                                {
                                    dataGridView1.RowCount = mxtzgs;
                                    dataGridView1.ColumnCount = 6;
                                    //序号
                                    for (int a = 0; a < mxtzgs; a++)
                                    {
                                        dataGridView1.Rows[a].Cells[0].Value = Convert.ToString(a + 1);
                                    }

                                    string sql2 = "select timestamp,modelname,funcname,info from  LogTable where FLAG=1 order by timestamp desc";
                                    DataTable dataTable2 = dBSQL.GetCommand(sql2);
                                    for (int b = 0; b < mxtzgs; b++)
                                    {
                                        for (int c = 0; c < 4; c++)
                                        {
                                            dataGridView1.Rows[b].Cells[c + 1].Value = Convert.ToString(dataTable2.Rows[b][c]);
                                        }
                                    }
                                    DataGridViewComboBoxColumn dcon = new DataGridViewComboBoxColumn();
                                    DataTable dt1 = new DataTable();
                                    string sql3 = "select appraise from  LogTable where FLAG=1 order by timestamp desc";
                                    DataTable dataTable3 = dBSQL.GetCommand(sql3);
                                    for (int d = 0; d < mxtzgs; d++)
                                    {
                                        dt1.Rows.Add();
                                        dt1.Columns.Add();
                                        dt1.Rows[d][0] = Convert.ToString(dataTable3.Rows[d][0]);
                                    }
                                    for (int d = 0; d < mxtzgs; d++)
                                    {
                                        if (dt1.Rows[d][0].Equals("0"))
                                        {
                                            ((DataGridViewComboBoxCell)dataGridView1.Rows[d].Cells[5]).Value = "正确";
                                        }
                                        if (dt1.Rows[d][0].Equals("1"))
                                        {
                                            ((DataGridViewComboBoxCell)dataGridView1.Rows[d].Cells[5]).Value = "错误";
                                        }
                                    }
                                }
                                else if (mxtzgs > 40)
                                {
                                    dataGridView1.RowCount = 40;
                                    dataGridView1.ColumnCount = 6;
                                    //序号
                                    for (int a = 0; a < 40; a++)
                                    {
                                        dataGridView1.Rows[a].Cells[0].Value = Convert.ToString(a + 1);
                                    }

                                    string sql2 = "select timestamp,modelname,funcname,info from  LogTable where FLAG=1 order by timestamp desc";
                                    DataTable dataTable2 = dBSQL.GetCommand(sql2);
                                    for (int b = 0; b < 40; b++)
                                    {
                                        for (int c = 0; c < 4; c++)
                                        {
                                            dataGridView1.Rows[b].Cells[c + 1].Value = Convert.ToString(dataTable2.Rows[b][c]);
                                        }
                                    }
                                    DataGridViewComboBoxColumn dcon = new DataGridViewComboBoxColumn();
                                    DataTable dt1 = new DataTable();
                                    string sql3 = "select appraise from  LogTable where FLAG=1 order by timestamp desc";
                                    DataTable dataTable3 = dBSQL.GetCommand(sql3);
                                    for (int d = 0; d < 40; d++)
                                    {
                                        dt1.Rows.Add();
                                        dt1.Columns.Add();
                                        dt1.Rows[d][0] = Convert.ToString(dataTable3.Rows[d][0]);
                                    }

                                    for (int d = 0; d < 40; d++)
                                    {
                                        if (dt1.Rows[d][0].Equals("0"))
                                        {
                                            ((DataGridViewComboBoxCell)dataGridView1.Rows[d].Cells[5]).Value = "正确";
                                        }
                                        else if (dt1.Rows[d][0].Equals("1"))
                                        {
                                            ((DataGridViewComboBoxCell)dataGridView1.Rows[d].Cells[5]).Value = "错误";
                                        }
                                    }
                                }
                            }
                            else MessageBox.Show("没有数据！");
                        }
                        //else if (comboBox1.Text == "系统日志")
                        //{
                        //    dataGridView1.Columns[5].Visible = false;
                        //    string sql1 = "select count(timestamp) from  LogTable where FLAG=2 ";
                        //    DataTable dataTable1 = dBSQL.GetCommand(sql1);
                        //    int mxtzgs = int.Parse(dataTable1.Rows[0][0].ToString());
                        //    if (mxtzgs != 0)
                        //    {
                        //        if (mxtzgs < 40)
                        //        {
                        //            dataGridView1.RowCount = mxtzgs;
                        //            dataGridView1.ColumnCount = 6;
                        //            //序号
                        //            for (int a = 0; a < mxtzgs; a++)
                        //            {
                        //                dataGridView1.Rows[a].Cells[0].Value = Convert.ToString(a + 1);
                        //            }

                        //            string sql2 = "select timestamp,modelname,funcname,info from  LogTable where FLAG=2 order by timestamp desc";
                        //            DataTable dataTable2 = dBSQL.GetCommand(sql2);
                        //            for (int b = 0; b < mxtzgs; b++)
                        //            {
                        //                for (int c = 0; c < 4; c++)
                        //                {
                        //                    dataGridView1.Rows[b].Cells[c + 1].Value = Convert.ToString(dataTable2.Rows[b][c]);
                        //                }
                        //            }
                        //        }
                        //        else if (mxtzgs > 40)
                        //        {
                        //            dataGridView1.RowCount = 40;
                        //            dataGridView1.ColumnCount = 5;
                        //            //序号
                        //            for (int a = 0; a < 40; a++)
                        //            {
                        //                dataGridView1.Rows[a].Cells[0].Value = Convert.ToString(a + 1);
                        //            }

                        //            string sql2 = "select timestamp,modelname,funcname,info from  LogTable where FLAG=2 order by timestamp desc";
                        //            DataTable dataTable2 = dBSQL.GetCommand(sql2);
                        //            for (int b = 0; b < 40; b++)
                        //            {
                        //                for (int c = 0; c < 4; c++)
                        //                {
                        //                    dataGridView1.Rows[b].Cells[c + 1].Value = Convert.ToString(dataTable2.Rows[b][c]);
                        //                }
                        //            }
                        //        }
                        //    }
                        //    else MessageBox.Show("没有数据！");

                        //}
                    }
                    else MessageBox.Show("请选择参数！");

                }
                catch
                { }
            }
            catch
            { }
        }

        //按时间查询
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string d1 = textBox_begin.Text.ToString();
            string d2 = textBox_end.Text.ToString();

            try
            {
                if (comboBox1.Text == "模型调整")
                {
                    dataGridView1.Columns[5].Visible = true;
                    string sql1 = "select count(timestamp) from  LogTable where FLAG=1 and timestamp between '" + d1 + "' and '" + d2 + "' ";
                    DataTable dataTable1 = dBSQL.GetCommand(sql1);
                    int mxtzgs = int.Parse(dataTable1.Rows[0][0].ToString());
                    if (mxtzgs != 0)
                    {
                        dataGridView1.RowCount = mxtzgs;
                        dataGridView1.ColumnCount = 6;
                        //序号
                        for (int a = 0; a < mxtzgs; a++)
                        {
                            dataGridView1.Rows[a].Cells[0].Value = Convert.ToString(a + 1);
                        }
                        string sql2 = "select timestamp,modelname,funcname,info from  LogTable where FLAG=1 and timestamp between '" + d1 + "' and '" + d2 + "' order by timestamp desc";
                        DataTable dataTable2 = dBSQL.GetCommand(sql2);
                        for (int b = 0; b < mxtzgs; b++)
                        {
                            for (int c = 0; c < 4; c++)
                            {
                                dataGridView1.Rows[b].Cells[c + 1].Value = Convert.ToString(dataTable2.Rows[b][c]);
                            }
                        }

                        ////下拉框
                        DataGridViewComboBoxColumn dcon1 = new DataGridViewComboBoxColumn();
                        DataTable dt5 = new DataTable();
                        string sql3 = "select appraise from  LogTable where FLAG=1 and timestamp between '" + d1 + "' and '" + d2 + "' order by timestamp desc";
                        DataTable dataTable3 = dBSQL.GetCommand(sql3);
                        for (int d = 0; d < mxtzgs; d++)
                        {
                            dt5.Rows.Add();
                            dt5.Columns.Add();
                            dt5.Rows[d][0] = Convert.ToString(dataTable3.Rows[d][0]);
                        }

                        for (int d = 0; d < mxtzgs; d++)
                        {
                            if (dt5.Rows[d][0].Equals("0"))
                            {
                                ((DataGridViewComboBoxCell)dataGridView1.Rows[d].Cells[5]).Value = "正确";
                            }
                            if (dt5.Rows[d][0].Equals("1"))
                            {
                                ((DataGridViewComboBoxCell)dataGridView1.Rows[d].Cells[5]).Value = "错误";
                            }
                        }
                    }
                    else MessageBox.Show("没有数据！");
                }
                //else if (comboBox1.Text == "系统日志")
                //{
                //    dataGridView1.Columns[5].Visible = false;
                //    string sql1 = "select count(timestamp) from  LogTable where FLAG=2 and timestamp between '" + d1 + "' and '" + d2 + "'";
                //    DataTable dataTable1 = dBSQL.GetCommand(sql1);
                //    int mxtzgs = int.Parse(dataTable1.Rows[0][0].ToString());
                //    if (mxtzgs != 0)
                //    {
                //        dataGridView1.RowCount = mxtzgs;
                //        //序号
                //        for (int a = 0; a < mxtzgs; a++)
                //        {
                //            dataGridView1.Rows[a].Cells[0].Value = Convert.ToString(a + 1);
                //        }
                //        string sql2 = "select timestamp,modelname,funcname,info from  LogTable where FLAG=2 and timestamp between '" + d1 + "' and '" + d2 + "' order by timestamp desc";
                //        DataTable dataTable2 = dBSQL.GetCommand(sql2);
                //        for (int b = 0; b < mxtzgs; b++)
                //        {
                //            for (int c = 0; c < 4; c++)
                //            {
                //                dataGridView1.Rows[b].Cells[c + 1].Value = Convert.ToString(dataTable2.Rows[b][c]);
                //            }
                //        }
                //    }
                //    else MessageBox.Show("没有数据！");
                //}
            }
            catch
            { }
        }

        //修改
        /// <summary>
        /// 首先给这个DataGridView加上EditingControlShowing事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridView dataGridView1 = sender as DataGridView;

            //判断相应的列
            if (dataGridView1.CurrentCell.GetType().Name == "DataGridViewComboBoxCell" && dataGridView1.CurrentCell.RowIndex != -1)
            {

                //给这个DataGridViewComboBoxCell加上下拉事件
                (e.Control as ComboBox).SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);

            }
        }
        /// <summary>
        /// 组合框事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combox = sender as ComboBox;

            //这里比较重要
            combox.Leave += new EventHandler(combox_Leave);
            try
            {
                //在这里就可以做值是否改变判断
                if (combox.SelectedItem != null)
                {
                    if (((ComboBox)sender).Text == "正确")
                    {
                        int b = dataGridView1.CurrentCell.RowIndex;
                        int a = 0;
                        string sql1 = "update  LogTable set appraise='" + a + "' where timestamp='" + dataGridView1.Rows[b].Cells[1].Value + "'";
                        dBSQL.CommandExecuteNonQuery(sql1);
                        MessageBox.Show("修改完成！");
                        time();
                    }
                    else if (((ComboBox)sender).Text == "错误")
                    {
                        int b = dataGridView1.CurrentCell.RowIndex;
                        int a = 1;
                        string sql1 = "update  LogTable set appraise='" + a + "' where timestamp='" + dataGridView1.Rows[b].Cells[1].Value + "'";
                        dBSQL.CommandExecuteNonQuery(sql1);
                        MessageBox.Show("修改完成！");
                        time();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 离开combox时，把事件删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void combox_Leave(object sender, EventArgs e)
        {
            ComboBox combox = sender as ComboBox;
            //做完处理，须撤销动态事件
            combox.SelectedIndexChanged -= new EventHandler(ComboBox_SelectedIndexChanged);
        }
        //dataGridView不显示行标题列
        private void dataGridView1_CellContentClick()
        {
            dataGridView1.RowHeadersVisible = false;
        }
        //下拉框
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            shuju();
        }

        //导出为表格
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
        //实时按钮
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            shuju();
        }
        public void Timer_stop()
        {
          //  _Timer1.Enabled = false;
        //    _Timer2.Enabled = false;

        }
        public void Timer_state()
        {

         //   _Timer1.Enabled = true;
          // _Timer2.Enabled = true;
        }
        public void _Clear()
        {
          //  _Timer1.Close();
          //  _Timer2.Close();
            this.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
