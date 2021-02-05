using DataBase;
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
using LY_SINTER.Custom;

namespace LY_SINTER.Popover.Quality
{
    public partial class Frm_PB_MESSAGE : Form
    {
        public System.Timers.Timer _Timer1 { get; set; }
        public static bool isopen = false;
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public Frm_PB_MESSAGE()
        {
            InitializeComponent();
            time_begin_end();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            combox_date();
            show();
            time_text();
            _Timer1 = new System.Timers.Timer(1000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { _Timer1_Tick(); };
            _Timer1.Enabled = true;
            _Timer1.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
        }

        /// <summary>
        /// 初始化班级定时器响应事件
        /// </summary>
        private void _Timer1_Tick()
        {
            Action invokeAction = new Action(_Timer1_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                _class();
            }
        }
        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
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
        /// 下拉框
        /// </summary>
        public void combox_date()
        {

            DataTable data_combox = new DataTable();
            data_combox.Columns.Add("name");
            data_combox.Columns.Add("Value");

            DataRow data_0 = data_combox.NewRow();
            data_0["name"] = "全部";
            data_0["Value"] = "0";
            data_combox.Rows.Add(data_0);

            DataRow data_1 = data_combox.NewRow();
            data_1["name"] = "人工";
            data_1["Value"] = "1";
            data_combox.Rows.Add(data_1);


            DataRow data_3 = data_combox.NewRow();
            data_3["name"] = "自动(调整烧返配比)";
            data_3["Value"] = "2";
            data_combox.Rows.Add(data_3);

            DataRow data_4 = data_combox.NewRow();
            data_4["name"] = "自动(质量控制模型)";
            data_4["Value"] = "3";
            data_combox.Rows.Add(data_4);

            DataRow data_5 = data_combox.NewRow();
            data_5["name"] = "自动(调整烧返分仓系数)";
            data_5["Value"] = "4";
            data_combox.Rows.Add(data_5);

            DataRow data_8 = data_combox.NewRow();
            data_8["name"] = "自动(启停信号变化)";
            data_8["Value"] = "5";
            data_combox.Rows.Add(data_8);

            DataRow data_9 = data_combox.NewRow();
            data_9["name"] = "自动(在用仓成分变化)";
            data_9["Value"] = "6";
            data_combox.Rows.Add(data_9);



            this.comboBox1.DataSource = data_combox;
            this.comboBox1.DisplayMember = "name";
            this.comboBox1.ValueMember = "Value";

            this.comboBox1.SelectedIndex = 0;




        }
        /// <summary>
        /// 班级
        /// </summary>
        public void _class()
        {
            for (int x = 0; x < dataGridView1.Rows.Count; x++)
            {
                //获取时间列
                string _time = dataGridView1.Rows[x].Cells["Column2"].Value.ToString();
                var sql1 = "select top(1) SHIFT_FLAG from M_CLASS_PLAN where START_TIME <= '" + _time + "' and END_TIME >= '" + _time + "'";
                DataTable _data = dBSQL.GetCommand(sql1);
                if (_data != null && _data.Rows.Count > 0)
                {
                    this.dataGridView1.Rows[x].Cells["Column3"].Value = _data.Rows[0][0].ToString();
                }
            }
        }
        /// <summary>
        /// 表单页面初始化
        /// </summary>
        public void show()
        {
           string sql_MC_MIXCAL_BILL_INFO_RESULT = "select top (20)  " +
                "ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID , " +
                "TIMESTAMP,	(MAT_L2_MB_C + MAT_L2_TZ_C) AS C_SUM," +
                "(case when MAT_L2_FLAG = 1 then '人工'" +
                " when MAT_L2_FLAG = 2 then '烧返调整配比'" +
                "  when MAT_L2_FLAG = 3 then '自动(质量自动模型)' " +
                "when MAT_L2_FLAG = 4 then '烧返调整分仓系数' " +
                "when MAT_L2_FLAG = 5 then '启停信号变化' " +
                "when MAT_L2_FLAG = 6 then '自动(原料成分变化)' " +
                "when MAT_L2_FLAG = 7 then '自动——返矿模型调整(分仓系数)' " +
                "when MAT_L2_FLAG = 8 then '自动(原料成分变化)' " +
                  "when MAT_L2_FLAG = 9 then '自动(换堆模型-混匀矿成分变化)' " +
                " else '异常'  end) as MAT_L2_FLAG," +
                "MAT_L2_PBBFB_1,MAT_L2_PBBFB_2,MAT_L2_PBBFB_3,MAT_L2_PBBFB_4,MAT_L2_PBBFB_5,MAT_L2_PBBFB_6,MAT_L2_PBBFB_7,MAT_L2_PBBFB_8,MAT_L2_DQBFB_1,MAT_L2_DQBFB_2,MAT_L2_DQBFB_3,MAT_L2_DQBFB_4,MAT_L2_DQBFB_5,MAT_L2_DQBFB_6,MAT_L2_DQBFB_7,MAT_L2_DQBFB_8,MAT_L2_ZLL_SP,MAT_L2_ZLL_PV," +
                "MAT_L2_MB_C,MAT_L2_TZ_C,(MAT_L2_MB_R + MAT_L2_TZ_R) AS R_SUM,(MAT_L2_MB_MG + MAT_L2_TZ_MG) AS MGO_SUM,MAT_L2_MB_R,MAT_L2_TZ_R,MAT_L2_MB_MG,MAT_L2_TZ_MG,MAT_L2_DQSF_1,MAT_L2_DQSF_2,MAT_L2_DQSF_3,MAT_L2_DQSF_4,MAT_L2_DQSF_5,MAT_L2_DQSF_6,MAT_L2_DQSF_7,MAT_L2_DQSF_8,MAT_L2_DQSF_9,MAT_L2_DQSF_10,MAT_L2_DQSF_11,MAT_L2_DQSF_12,MAT_L2_DQSF_13,MAT_L2_DQSF_14,MAT_L2_DQSF_15,MAT_L2_DQSF_16,MAT_L2_DQSF_17,MAT_L2_DQSF_18,MAT_L2_DQSF_19,MAT_L2_DQSF_20,MAT_L2_SIGNAL_R,MAT_L2_SIGNAL_C,MAT_L2_SIGNAL_MG,MAT_L2_SIGNAL_FK" +
                " from MC_MIXCAL_BILL_INFO_RESULT  order by  TIMESTAMP desc";
            DataTable data_MC_MIXCAL_BILL_INFO_RESULT = dBSQL.GetCommand(sql_MC_MIXCAL_BILL_INFO_RESULT);
            if (data_MC_MIXCAL_BILL_INFO_RESULT.Rows.Count > 0)
            {
                dataGridView1.DataSource = data_MC_MIXCAL_BILL_INFO_RESULT;

            }

        }
        /// <summary>
        /// 开始时间-结束时间赋值
        /// </summary>
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddDays(-7);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();

        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            text_his(comboBox1.Text.ToString());
            _class();
        }
        /// <summary>
        /// 历史数据查询
        /// </summary>
        public void text_his(string _name)
        {
            if (_name == "全部")
            {
                //开始时间
                DateTime time_begin = Convert.ToDateTime(textBox_begin.Text);
                //结束时间
                DateTime time_end = Convert.ToDateTime(textBox_end.Text);
                string sql_MC_MIXCAL_BILL_INFO_RESULT = "select   " +
                  "ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID , " +
                  "TIMESTAMP,	" +
                 "(case when MAT_L2_FLAG = 1 then '人工'" +
                " when MAT_L2_FLAG = 2 then '烧返调整配比'" +
                "  when MAT_L2_FLAG = 3 then '自动(质量自动模型)' " +
                "when MAT_L2_FLAG = 4 then '烧返调整分仓系数' " +
                "when MAT_L2_FLAG = 5 then '启停信号变化' " +
                "when MAT_L2_FLAG = 6 then '自动(原料成分变化)' " +
                "when MAT_L2_FLAG = 7 then '自动——返矿模型调整(分仓系数)' " +
                "when MAT_L2_FLAG = 8 then '自动(原料成分变化)' " +
                  "when MAT_L2_FLAG = 9 then '自动(换堆模型-混匀矿成分变化)' " +
                " else '异常'  end) as MAT_L2_FLAG," +
                 "MAT_L2_PBBFB_1,MAT_L2_PBBFB_2,MAT_L2_PBBFB_3,MAT_L2_PBBFB_4,MAT_L2_PBBFB_5,MAT_L2_PBBFB_6,MAT_L2_PBBFB_7,MAT_L2_PBBFB_8,MAT_L2_DQBFB_1,MAT_L2_DQBFB_2,MAT_L2_DQBFB_3,MAT_L2_DQBFB_4,MAT_L2_DQBFB_5,MAT_L2_DQBFB_6,MAT_L2_DQBFB_7,MAT_L2_DQBFB_8,MAT_L2_ZLL_SP,MAT_L2_ZLL_PV," +
                 "(MAT_L2_MB_R + MAT_L2_TZ_R) AS R_SUM,(MAT_L2_MB_MG + MAT_L2_TZ_MG) AS MGO_SUM,(MAT_L2_MB_C + MAT_L2_TZ_C) AS C_SUM,MAT_L2_MB_C,MAT_L2_TZ_C,MAT_L2_MB_R,MAT_L2_TZ_R,MAT_L2_MB_MG,MAT_L2_TZ_MG,MAT_L2_DQSF_1,MAT_L2_DQSF_2,MAT_L2_DQSF_3,MAT_L2_DQSF_4,MAT_L2_DQSF_5,MAT_L2_DQSF_6,MAT_L2_DQSF_7,MAT_L2_DQSF_8,MAT_L2_DQSF_9,MAT_L2_DQSF_10,MAT_L2_DQSF_11,MAT_L2_DQSF_12,MAT_L2_DQSF_13,MAT_L2_DQSF_14,MAT_L2_DQSF_15,MAT_L2_DQSF_16,MAT_L2_DQSF_17,MAT_L2_SIGNAL_R,MAT_L2_SIGNAL_C,MAT_L2_SIGNAL_MG,MAT_L2_SIGNAL_FK" +
                  " from MC_MIXCAL_BILL_INFO_RESULT  where  TIMESTAMP >= '" + time_begin + "' and TIMESTAMP <= '" + time_end + "' order by TIMESTAMP desc";
                DataTable data_MC_MIXCAL_BILL_INFO_RESULT = dBSQL.GetCommand(sql_MC_MIXCAL_BILL_INFO_RESULT);
                if (data_MC_MIXCAL_BILL_INFO_RESULT.Rows.Count > 0)
                {
                    dataGridView1.DataSource = data_MC_MIXCAL_BILL_INFO_RESULT;
                }
                else
                {

                }
            }
            else
            {

                int _flag = 0;
                if (_name == "人工")
                {
                    _flag = 1;
                }
                else if (_name == "自动(调整烧返配比)")
                {
                    _flag = 2;
                }
                else if (_name == "自动(质量控制模型)")
                {
                    _flag = 3;
                }
                else if (_name == "自动(调整烧返分仓系数)")
                {
                    _flag = 4;
                }
                else if (_name == "自动(启停信号变化)")
                {
                    _flag = 5;
                }
                else if (_name == "自动(在用仓成分变化)")
                {
                    _flag = 6;
                }
                

                //开始时间
                DateTime time_begin = Convert.ToDateTime(textBox_begin.Text);
                //结束时间
                DateTime time_end = Convert.ToDateTime(textBox_end.Text);
                string sql_MC_MIXCAL_BILL_INFO_RESULT = "select   " +
                  "ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID , " +
                  "TIMESTAMP,	" +
                 "(case when MAT_L2_FLAG = 1 then '人工'" +
                " when MAT_L2_FLAG = 2 then '烧返调整配比'" +
                "  when MAT_L2_FLAG = 3 then '自动(质量自动模型)' " +
                "when MAT_L2_FLAG = 4 then '烧返调整分仓系数' " +
                "when MAT_L2_FLAG = 5 then '启停信号变化' " +
                "when MAT_L2_FLAG = 6 then '自动(原料成分变化)' " +
                "when MAT_L2_FLAG = 7 then '自动——返矿模型调整(分仓系数)' " +
                "when MAT_L2_FLAG = 8 then '自动(原料成分变化)' " +
                  "when MAT_L2_FLAG = 9 then '自动(换堆模型-混匀矿成分变化)' " +
                " else '异常'  end) as MAT_L2_FLAG," +
                  "MAT_L2_PBBFB_1,MAT_L2_PBBFB_2,MAT_L2_PBBFB_3,MAT_L2_PBBFB_4,MAT_L2_PBBFB_5,MAT_L2_PBBFB_6,MAT_L2_PBBFB_7,MAT_L2_PBBFB_8,MAT_L2_DQBFB_1,MAT_L2_DQBFB_2,MAT_L2_DQBFB_3,MAT_L2_DQBFB_4,MAT_L2_DQBFB_5,MAT_L2_DQBFB_6,MAT_L2_DQBFB_7,MAT_L2_DQBFB_8,MAT_L2_ZLL_SP,MAT_L2_ZLL_PV," +
                  "(MAT_L2_MB_R + MAT_L2_TZ_R) AS R_SUM,(MAT_L2_MB_MG + MAT_L2_TZ_MG) AS MGO_SUM,(MAT_L2_MB_C + MAT_L2_TZ_C) AS C_SUM,MAT_L2_MB_C,MAT_L2_TZ_C,MAT_L2_MB_R,MAT_L2_TZ_R,MAT_L2_MB_MG,MAT_L2_TZ_MG,MAT_L2_DQSF_1,MAT_L2_DQSF_2,MAT_L2_DQSF_3,MAT_L2_DQSF_4,MAT_L2_DQSF_5,MAT_L2_DQSF_6,MAT_L2_DQSF_7,MAT_L2_DQSF_8,MAT_L2_DQSF_9,MAT_L2_DQSF_10,MAT_L2_DQSF_11,MAT_L2_DQSF_12,MAT_L2_DQSF_13,MAT_L2_DQSF_14,MAT_L2_DQSF_15,MAT_L2_DQSF_16,MAT_L2_DQSF_17,MAT_L2_SIGNAL_R,MAT_L2_SIGNAL_C,MAT_L2_SIGNAL_MG,MAT_L2_SIGNAL_FK" +
                  "  from MC_MIXCAL_BILL_INFO_RESULT  where  MAT_L2_FLAG = " + _flag + " and  TIMESTAMP >= '" + time_begin + "' and TIMESTAMP <= '" + time_end + "' order by TIMESTAMP desc";
                DataTable data_MC_MIXCAL_BILL_INFO_RESULT = dBSQL.GetCommand(sql_MC_MIXCAL_BILL_INFO_RESULT);
                if (data_MC_MIXCAL_BILL_INFO_RESULT.Rows.Count > 0)
                {
                    dataGridView1.DataSource = data_MC_MIXCAL_BILL_INFO_RESULT;
                }
                else
                {

                }
            }

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            show();
            time_text();
            _class();
        }
        /// <summary>
        /// 最新调整时间
        /// </summary>
        public void time_text()
        {
            string sql_time = "select top (1) TIMESTAMP from MC_MIXCAL_BILL_INFO_RESULT where TIMESTAMP = (select max(TIMESTAMP) from MC_MIXCAL_BILL_INFO_RESULT)";
            DataTable data_time = dBSQL.GetCommand(sql_time);
            if (data_time.Rows.Count > 0)
            {
                label2.Text = "最新调整时间:" + data_time.Rows[0]["TIMESTAMP"].ToString();
            }
            else
            {
                label2.Text = "最新调整时间:";
            }
        }
    }
}
