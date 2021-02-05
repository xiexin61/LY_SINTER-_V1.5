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
    
    public partial class Frm_BTP_Adjust : Form
    {
        public static bool isopen = false;
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public vLog _vLog { get; set; }
        public Frm_BTP_Adjust()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Course\\Frm_BTP_Adjust\\");
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            _Show(1);
        }
        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
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


                else MessageBox.Show("请选择参数！");
            }
            catch
            { }
        }
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            _Show(2);
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
        /// 查询数据
        /// _FALG = 1 实时 _FALG = 2 历史
        /// </summary>
        public void _Show(int _FALG)
        {
            try
            {
                if (_FALG == 1)
                {
                    var sql = "SELECT top(20) TIMESTAMP,( case BTPCAL_SPARE_FLAG2 when '0' then '正常' when '1' then '超前' when '2' then '滞后' else '未知' end) as BTPCAL_SPARE_FLAG2,(case BTPCAL_SPARE_FLAG3 when '1' then'投入' when '2' then '退出' else '异常' end) as BTPCAL_SPARE_FLAG3,BTPCAL_aim_BTP,BTPCAL_BTP_L_NUM,BTPCAL_BTP_R_NUM,BTPCAL_BTP_NUM,BTP_AVG_L,BTP_AVG_R,BTP_AVG,BTPCAL_SMALL_SG_PV_AVG,BTPCAL_SB_1_FLUE_TE,BTPCAL_SB_2_FLUE_TE,BTPCAL_SB_FLUE_TE_AVG,BTPCAL_SB_1_FLUE_PT,BTPCAL_SB_2_FLUE_PT,BTPCAL_BRP_AVG,BTPCAL_BTP_AVG,BTPCAL_SIN_MS_SP,BTPCAL_SIN_MS_AD_K,BTPCAL_SIN_MS_AD,BTPCAL_DOWN_FLAG FROM MC_BTPCAL_result order by TIMESTAMP desc";

                    DataTable _data = dBSQL.GetCommand(sql);
                    if (_data != null)
                    {
                        dataGridView1.DataSource = _data;
                    }
                }
                else
                {
                    var sql = "SELECT TIMESTAMP,( case BTPCAL_SPARE_FLAG2 when '0' then '正常' when '1' then '超前' when '2' then '滞后' else '未知' end) as BTPCAL_SPARE_FLAG2,(case BTPCAL_SPARE_FLAG3 when '1' then'投入' when '2' then '退出' else '异常' end) as BTPCAL_SPARE_FLAG3,BTPCAL_aim_BTP,BTPCAL_BTP_L_NUM,BTPCAL_BTP_R_NUM,BTPCAL_BTP_NUM,BTP_AVG_L,BTP_AVG_R,BTP_AVG,BTPCAL_SMALL_SG_PV_AVG,BTPCAL_SB_1_FLUE_TE,BTPCAL_SB_2_FLUE_TE,BTPCAL_SB_FLUE_TE_AVG,BTPCAL_SB_1_FLUE_PT,BTPCAL_SB_2_FLUE_PT,BTPCAL_BRP_AVG,BTPCAL_BTP_AVG,BTPCAL_SIN_MS_SP,BTPCAL_SIN_MS_AD_K,BTPCAL_SIN_MS_AD,BTPCAL_DOWN_FLAG FROM MC_BTPCAL_result where TIMESTAMP >='"+ textBox_begin.Text.ToString() + "' and TIMESTAMP <='"+ textBox_end.Text.ToString() + "'  order by TIMESTAMP desc";

                    DataTable _data = dBSQL.GetCommand(sql);
                    if (_data != null)
                    {
                        dataGridView1.DataSource = _data;
                    }
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("_Show方法错误",-1);
            }

        }
    }
}
