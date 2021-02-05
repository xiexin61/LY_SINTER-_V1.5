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
using LY_SINTER.Custom;
using LY_SINTER.Model;
using System.IO;
using LY_SINTER.Popover.Parameter;

namespace LY_SINTER.PAGE.Parameter
{
    public partial class production_Records : UserControl
    {
        public vLog _vLog { get; set; }
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public production_Records()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Parameter\\production_Records\\");
            time_begin_end();//开始&结束时间赋值
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            show_d1(1);//初始化加载烧结停机记录
        }
        /// <summary>
        /// 开始时间-结束时间赋值
        /// </summary>
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddMonths(-1);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();

            textBox_begin_1.Text = time_begin.ToString();
            textBox_end_1.Text = time_end.ToString();

        }

        /// <summary>
        /// 停机记录选中行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void d1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    Quality_Model.FLAG_1 = int.Parse(this.d1.Rows[e.RowIndex].Cells["Column1"].Value.ToString());
                }
            }
            catch
            {

            }
           
        }
        /// <summary>
        /// 烧结事件选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    Quality_Model.FLAG_2 = int.Parse(this.d2.Rows[e.RowIndex].Cells["Column2"].Value.ToString());
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 停机记录导出按钮
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
                    for (int i = 0; i < d1.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            columnTitle += "\t";
                        }
                        columnTitle += d1.Columns[i].HeaderText;
                    }
                    sw.WriteLine(columnTitle);

                    //写入列内容    
                    for (int j = 0; j < d1.Rows.Count; j++)
                    {
                        string columnValue = "";
                        for (int k = 0; k < d1.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                columnValue += "\t";
                            }
                            if (d1.Rows[j].Cells[k].Value == null)
                                columnValue += "";
                            else
                                columnValue += d1.Rows[j].Cells[k].Value.ToString().Trim();
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
        /// 烧结事件导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton8_Click(object sender, EventArgs e)
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
                    for (int i = 0; i < d2.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            columnTitle += "\t";
                        }
                        columnTitle += d2.Columns[i].HeaderText;
                    }
                    sw.WriteLine(columnTitle);

                    //写入列内容    
                    for (int j = 0; j < d2.Rows.Count; j++)
                    {
                        string columnValue = "";
                        for (int k = 0; k < d2.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                columnValue += "\t";
                            }
                            if (d2.Rows[j].Cells[k].Value == null)
                                columnValue += "";
                            else
                                columnValue += d2.Rows[j].Cells[k].Value.ToString().Trim();
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
        /// 停机记录删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (Quality_Model.FLAG_1 == -1)
                {
                    MessageBox.Show("请选择需要删除的数据");
                }
                else
                {
                    var name_flag = "是否删除选中的记录";
                    DialogResult resule = MessageBox.Show(name_flag, "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    string a = resule.ToString();
                    if (a == "OK")
                    {
                        var sql_del = "delete from where ";
                        int count = dBSQL.CommandExecuteNonQuery(sql_del);
                        if (count == 1 )
                        {
                            _vLog.writelog("simpleButton3_Click事件用户删除成功，sql:" + sql_del, 0);
                            MessageBox.Show("删除成功");
                        }
                        else
                        {
                            MessageBox.Show("删除成功，请重新选择");
                            _vLog.writelog("simpleButton3_Click事件用户删除失败，sql:" + sql_del,-1);
                        }
                    }
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 烧结事件删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton10_Click(object sender, EventArgs e)
        {
            try
            {
                if (Quality_Model.FLAG_2 == -1)
                {
                    MessageBox.Show("请选择需要删除的数据");
                }
                else
                {
                    var name_flag = "是否删除选中的记录";
                    DialogResult resule = MessageBox.Show(name_flag, "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    string a = resule.ToString();
                    if (a == "OK")
                    {
                        var sql_del = "delete from where ";
                        int count = dBSQL.CommandExecuteNonQuery(sql_del);
                        if (count == 1)
                        {
                            _vLog.writelog("simpleButton10_Click事件用户删除成功，sql:" + sql_del, 0);
                            MessageBox.Show("删除成功");
                        }
                        else
                        {
                            MessageBox.Show("删除成功，请重新选择");
                            _vLog.writelog("simpleButton10_Click事件用户删除失败，sql:" + sql_del, -1);
                        }
                    }
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 烧结停机记录查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 烧结事件信息记录查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton9_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 烧结停机记录添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Frm_production_Records_1 form_display = new Frm_production_Records_1();
            if (Frm_production_Records_1.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }
        /// <summary>
        /// 烧结停机记录修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Frm_production_Records_2 form_display = new Frm_production_Records_2();
            if (Frm_production_Records_2.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }
        /// <summary>
        /// 烧结事件信息记录修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            Frm_production_Records_4 form_display = new Frm_production_Records_4();
            if (Frm_production_Records_4.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }
        /// <summary>
        /// 烧结事件信息记录添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Frm_production_Records_3 form_display = new Frm_production_Records_3();
            if (Frm_production_Records_3.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }
        public void Timer_state()
        {
           // _Timer1.Enabled = true;
        }
        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {
         //   _Timer1.Enabled = false;
        }
        /// <summary>
        /// 控件关闭
        /// </summary>
        public void _Clear()
        {
           // _Timer1.Close();//释放定时器资源
            this.Dispose();//释放资源
            GC.Collect();//调用GC
        }
        /// <summary>
        /// 烧结停机记录数据
        /// flag = 1 初始化
        /// flag = 2 查询
        /// </summary>
        /// <param name="flag"></param>
        public void show_d1(int flag)
        {
            try
            {
                if (flag == 1)
                {
                    var sql = "select top(15) ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) AS ID,TIMESTAMP,WORK_SHIFT,WORK_TEAM,REMARK_DESC,STOP_BEGINTIME,STOP_ENDTIME,INTERVAL_TIME,FLAG_1,SORT_BIG,SORT_LITTLE from M_SIN_RUN_STOP order by TIMESTAMP desc";
                    DataTable data_1 = dBSQL.GetCommand(sql);
                    if (data_1 != null && data_1.Rows.Count > 0 )
                    {
                        d1.DataSource = data_1;
                    }
           
                }
                else
                {

                }
            }
            catch(Exception ee)
            {
                _vLog.writelog("" + ee.ToString(), -1);
            }
        }
    }
}
