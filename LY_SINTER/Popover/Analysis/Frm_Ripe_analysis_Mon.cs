using DataBase;
using LY_SINTER.Custom;
using LY_SINTER.Model;
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

namespace LY_SINTER.Popover.Analysis
{
    public partial class Frm_Ripe_analysis_Mon : Form
    {
        public vLog _vLog { get; set; }
        public static bool isopen = false;
        private ANALYSIS_MODEL _MODEL = new ANALYSIS_MODEL();
        private DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        public System.Timers.Timer _Timer1 { get; set; }
        //int

        public Frm_Ripe_analysis_Mon()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Analysis\\Frm_Ripe_analysis_Mon\\");
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
            //   Class_text(2, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
            //  Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
            Time_now();
            _Timer1 = new System.Timers.Timer(1000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { Timer1_Tick_1(); };//响应事件
            _Timer1.Enabled = true;
            _Timer1.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
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
                //   this.dataGridView1.Rows[7].Frozen = true;
                //for (int s = 0; s < 8; s++)
                //{
                //    this.dataGridView1.Rows[s].DefaultCellStyle.BackColor = Color.AliceBlue;
                //}
            }
        }

        private string[] _COL = {
"TIMESTAMP",
"C_TFE",
"C_FEO",
"C_CAO",
"C_SIO2",
"C_AL2O3",
"C_MGO",
"C_S",
"C_P2O5",
"C_R",
"C_K2O",
"C_NA2O",
"C_PB",
"C_ZN",
"C_MNO",
"C_TIO2",
"C_AS",
"C_CU",
"C_K",
"C_V2O5",
"C_AL2O3_SIO2",
"C_MGO_AL2O3",
"C_TI",
"RI",
"RDI_0_5",
"RDI_3_15",
"RDI_6_3",
"SCRENING",
"GRIT_5",
"GRIT_5_10",
"GRIT_10_16",
"GRIT_16_25",
"GRIT_25_40",
"GRIT_40",
"PATCL_AV",
"GRIT_10",
"GRIT_10_40",
"R_005_CONTROL",
"R_008_CONTROL",
"FEO_1_CONTROL",
"MGO_015_CONTROL",
"GRIT_10_CONTROL",
"TFE_STANDARD",
"SIO2_STANDARD",
"CAO_STANDARD",
"R_STANDARD",
"FEO_STANDARD",
"MGO_STANDARD"
};

        /// <summary>
        /// 开始时间&结束时间赋值
        /// </summary>
        public void dateTimePicker_value()
        {
            //结束时间
            DateTime time_end = DateTime.Now;
            //开始时间
            DateTime time_begin = time_end.AddDays(-7);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }

        /// <summary>
        /// 月数据查询
        /// _flag :1 一号烧结机 2：二号烧结机 3：平均烧结机
        /// time_begin:开始时间
        /// time_end结束时间
        /// </summary>
        /// <param name="_flag"></param>
        public void Class_text(int _flag, string time_begin, string time_end)
        {
            try
            {
                DataTable _data = new DataTable();
                for (int x = 0; x < _COL.Count(); x++)
                {
                    _data.Columns.Add(_COL[x]);
                }
                string _NAME_CLASS = "";
                if (_flag == 1)
                {
                    _NAME_CLASS = "1#烧结机";
                }
                else if (_flag == 2)
                {
                    _NAME_CLASS = "2#烧结机";
                }
                else if (_flag == 3)
                {
                    _NAME_CLASS = "3#烧结机";
                }
                else
                {
                    _NAME_CLASS = "1、2#烧结机平均";
                }
                var _SQL = "select * from MC_NUMCAL_INTERFACE_6_MONTH where DL_FLAG=" + _flag + " and RECORD_TIME between '" + time_begin + "' and '" + time_end + "' order by RECORD_TIME desc ";
                DataTable _table = _dBSQL.GetCommand(_SQL);
                if (_table != null && _table.Rows.Count > 0)
                {
                    if (_flag == 1)
                    {
                        this.dataGridView1.DataSource = _table;
                    }
                    if (_flag == 2)
                    {
                        this.dataGridView2.DataSource = _table;
                    }
                    else if (_flag == 3)
                    {
                        #region 最优数据

                        //历史最优
                        {
                            var sql = "select * from  MC_NUMCAL_INTERFACE_6_M_OPTIMAL where class_flag = 1";
                            DataTable _data1 = _dBSQL.GetCommand(sql);
                            if (_data1.Rows.Count > 0 && _data1 != null)
                            {
                                DataRow row_1 = _data.NewRow();
                                row_1["TIMESTAMP"] = "历史最优月(" + DateTime.Parse(_data1.Rows[0]["timestamp"].ToString() == "" ? DateTime.Now.ToString() : _data1.Rows[0]["timestamp"].ToString()).Year + "年" + DateTime.Parse(_data1.Rows[0]["timestamp"].ToString() == "" ? DateTime.Now.ToString() : _data1.Rows[0]["timestamp"].ToString()).Month + "月）";
                                for (int x = 1; x < _COL.Count(); x++)
                                {
                                    row_1[_COL[x]] = _data1.Rows[0][_COL[x]].ToString();
                                }
                                _data.Rows.Add(row_1);
                            }
                        }
                        //今年最优
                        {
                            var sq2 = "select * from  MC_NUMCAL_INTERFACE_6_M_OPTIMAL where class_flag = 2";
                            DataTable _data2 = _dBSQL.GetCommand(sq2);
                            if (_data2.Rows.Count > 0 && _data2 != null)
                            {
                                DataRow row_1 = _data.NewRow();
                                row_1["TIMESTAMP"] = "今年最优月(" + DateTime.Parse(_data2.Rows[0]["timestamp"].ToString() == "" ? DateTime.Now.ToString() : _data2.Rows[0]["timestamp"].ToString()).Month + "月）";
                                for (int x = 1; x < _COL.Count(); x++)
                                {
                                    row_1[_COL[x]] = _data2.Rows[0][_COL[x]].ToString();
                                }
                                _data.Rows.Add(row_1);
                            }
                        }
                        //今年平均月
                        {
                            var sq3 = "select * from  MC_NUMCAL_INTERFACE_6_M_OPTIMAL where class_flag = 3";
                            DataTable _data3 = _dBSQL.GetCommand(sq3);
                            if (_data3.Rows.Count > 0 && _data3 != null)
                            {
                                DataRow row_1 = _data.NewRow();
                                row_1["TIMESTAMP"] = "今年平均月";
                                for (int x = 1; x < _COL.Count(); x++)
                                {
                                    row_1[_COL[x]] = _data3.Rows[0][_COL[x]].ToString();
                                }
                                _data.Rows.Add(row_1);
                            }
                        }

                        #endregion 最优数据

                        #region 基础数据

                        for (int x = 0; x < _table.Rows.Count; x++)
                        {
                            DataRow row_1 = _data.NewRow();
                            row_1["TIMESTAMP"] = "" + DateTime.Parse(_table.Rows[0]["RECORD_TIME"].ToString() == "" ? DateTime.Now.ToString() : _table.Rows[0]["RECORD_TIME"].ToString()).Year + "年" + DateTime.Parse(_table.Rows[0]["RECORD_TIME"].ToString() == "" ? DateTime.Now.ToString() : _table.Rows[0]["RECORD_TIME"].ToString()).Month + "月";
                            for (int x1 = 1; x1 < _COL.Count(); x1++)
                            {
                                row_1[_COL[x1]] = _table.Rows[x][_COL[x1]].ToString();
                            }
                            _data.Rows.Add(row_1);
                        }
                        this.dataGridView1.DataSource = _data;

                        #endregion 基础数据
                    }
                    else
                    {
                        this.dataGridView3.DataSource = _table;
                    }
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("Class_text方法失败" + ee.ToString(), -1);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
            // Class_text(2, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
            //   Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            dateTimePicker_value();
            Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
            //   Class_text(2, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
            //  Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
        }

        /// <summary>
        /// 最新调整时间
        /// </summary>
        public void Time_now()
        {
            try
            {
                string sql1 = "select top 1 RECORD_TIME from MC_NUMCAL_INTERFACE_6_MONTH order by RECORD_TIME desc";
                DataTable dataTable1 = _dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    label3.Text = "最新调整时间:" + dataTable1.Rows[0][0].ToString();
                }
                else
                {
                    label3.Text = "";
                }
            }
            catch
            { }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
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
            }
            catch
            {
            }
        }
    }
}