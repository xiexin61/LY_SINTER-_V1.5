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
using VLog;

namespace LY_SINTER.PAGE.Quality
{
    public partial class Mine_Forecast : UserControl
    {
        /// <summary>
        /// PAR使用数据标志位
        /// </summary>
        int _FLAG_PAR = 1;
        /// <summary>
        /// 实时曲线查询时间段
        /// </summary>
        int HOUR_CURVE = 12;
        /// <summary>
        /// 判断实时曲线展示数据勾选框是否发生变化
        /// </summary>
        bool CURVE_Real_Flag = false;

        #region 烧结矿成分趋势历史曲线定义
        /// <summary>
        /// 检测曲线
        /// </summary>
        List<double> _List_His_JC = new List<double>();
        /// <summary>
        /// 预测曲线
        /// </summary>
        List<double> _List_His_YC = new List<double>();
        /// <summary>
        /// 历史时间
        /// </summary>
        List<string> _List_His_TIME = new List<string>();
        #endregion

        #region 烧结矿成分趋势实时曲线定义
        /// <summary>
        /// 检测曲线
        /// </summary>
        List<double> _List_Real_JC = new List<double>();
        /// <summary>
        /// 预测曲线
        /// </summary>
        List<double> _List_Real_YC = new List<double>();
        /// <summary>
        /// 实时时间
        /// </summary>
        List<string> _List_Real_TIME = new List<string>();


        #endregion

        #region 页面查询涉及表
        /// <summary>
        /// 烧结矿成分趋势 预测数据表
        /// </summary>
        string CURVE_NAME_YC = "MC_MIXCAL_RESULT_1MIN";
        /// <summary>
        /// 烧结矿成分趋势 预测数据表使用字段
        /// </summary>
        string CURVE_NAME_YC_FIELD = "SINCAL_SIN_PV_TFE";
        /// <summary>
        /// 烧结矿成分趋势 检测数据表
        /// </summary>
        string CURVE_NAME_JC = "M_SINTER_ANALYSIS";
        /// <summary>
        /// 烧结矿成分趋势 检测数据表使用字段
        /// </summary>
        string CURVE_NAME_JC_FIELD = "C_TFE";
        #endregion

        #region 定时器定义
        public System.Timers.Timer _Timer1 { get; set; }
        public System.Timers.Timer _Timer2 { get; set; }

        #endregion

        int flag = 0;
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public vLog vLog { get; set; }
        public Mine_Forecast()
        {
            InitializeComponent();
            if (vLog == null)
                vLog = new vLog(".\\log_Page\\Quality\\Mine_Forecast\\");
            dateTimePicker_value();
            dateTimePicker_value1();
            DateTimeChoser.AddTo(textBox_begin_1);
            DateTimeChoser.AddTo(textBox_end_1);
            DateTimeChoser.AddTo(textBox_begin_2);
            DateTimeChoser.AddTo(textBox_end_2);

            MC_PRE_SINTER_ANA_PAR();
            //曲线控件背景颜色
            lChartPlus100.LChart.BackColor = Color.White;
            timer();
            Data_Real();
            Time_now();
            ycqx();
            HIS_CURVE(Convert.ToDateTime(textBox_begin_1.Text), Convert.ToDateTime(textBox_end_1.Text));
         
        }
        /// <summary>
        /// 定时器定义
        /// </summary>
        public void timer()
        {
            _Timer1 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { Timer1_Tick_1(); };//响应事件
            _Timer1.Enabled = true;
            _Timer1.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
            _Timer2 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer2.Elapsed += (x, y) => { Timer1_Tick_2(); };//响应事件
            _Timer2.Enabled = true;
            _Timer2.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

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
                Data_Real();
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
                Time_now();
                MC_PRE_SINTER_ANA_PAR();
            }
        }

        /// <summary>
        /// 最新调整时间
        /// </summary>
        public void Time_now()
        {
            if (_FLAG_PAR == 1)
            {
                string sql2 = "select top 1 TIMESTAMP from MC_MIXCAL_RESULT_1MIN order by TIMESTAMP desc";
                DataTable dataTable2 = dBSQL.GetCommand(sql2);
                if (dataTable2.Rows.Count > 0)
                {
                    label2.Text = "最新调整时间:" + dataTable2.Rows[0][0].ToString();
                }
                else
                {
                    //  label2.Text = "最新调整时间:" + zxsj;
                    label2.Text = "最新调整时间:";
                }

            }
            else if (_FLAG_PAR == 2)
            {

                string sql2 = "select top 1 TIMESTAMP from MC_PRE_SINTER_ANA_RESULT order by TIMESTAMP desc";
                DataTable dataTable2 = dBSQL.GetCommand(sql2);
                if (dataTable2.Rows.Count > 0)
                {
                    label2.Text = "最新调整时间:" + dataTable2.Rows[0][0].ToString();
                }
                else
                {
                    //  label2.Text = "最新调整时间:" + zxsj;
                    label2.Text = "最新调整时间:";
                }
            }
            else
            {
                label2.Text = "最新调整时间:";
            }
        }

        //烧结矿预测成分历史查询按钮
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            _Timer1.Enabled = false;
            Data_HIS();

        }
        /// <summary>
        /// 历史查询烧结矿成分
        /// </summary>
        public void Data_HIS()
        {
            try
            {
                DateTime d1 = Convert.ToDateTime(textBox_begin_2.Text);
                DateTime d2 = Convert.ToDateTime(textBox_end_2.Text);

                if (_FLAG_PAR == 1)
                {
                    string sql3 = "select  " +
                    "ROW_NUMBER() OVER(ORDER BY TIMESTAMP desc) AS ID," +
                    "TIMESTAMP," +
                    "cast(SINCAL_SIN_PV_TFE as decimal(18,2)) AS TFE," +
                    "cast(SINCAL_SIN_PV_CAO as decimal(18,2)) AS CAO," +
                    "cast(SINCAL_SIN_PV_SIO2 as decimal(18,2)) AS SIO2," +
                    "cast(SINCAL_SIN_PV_AL2O3 as decimal(18,2)) AS AL2O3," +
                    "cast(SINCAL_SIN_PV_MGO as decimal(18,2)) AS MGO," +
                    "cast(SINCAL_SIN_PV_S as decimal(18,2)) AS S," +
                    "cast(SINCAL_SIN_PV_P as decimal(18,2)) AS P," +
                    "cast(SINCAL_SIN_PV_R as decimal(18,2)) AS R," +
                    "cast(SINCAL_SIN_PV_TIO2 as decimal(18,2)) AS TIO2," +
                    "cast(SINCAL_SIN_PV_K2O as decimal(18,2)) AS K2O," +
                    "cast(SINCAL_SIN_PV_NA2O as decimal(18,2)) AS NA2O," +
                    "cast(SINCAL_SIN_PV_CU as decimal(18,2)) AS CU," +
                    "cast(SINCAL_SIN_PV_PB as decimal(18,2)) AS PB," +
                    "cast(SINCAL_SIN_PV_ZN as decimal(18,2)) AS ZN," +
                    "cast(SINCAL_SIN_PV_K as decimal(18,2)) AS K," +
                    "cast(SINCAL_SIN_PV_MNO as decimal(18,2)) AS MNO    " +
                    "from MC_MIXCAL_RESULT_1MIN where TIMESTAMP between '" + d1 + "' and '" + d2 + "' order by TIMESTAMP desc";
                    DataTable dataTable3 = dBSQL.GetCommand(sql3);
                    if (dataTable3.Rows.Count > 0)
                    {
                        this.dataGridView1.DataSource = dataTable3;
                    }
                }
                else if (_FLAG_PAR == 2)
                {

                    string sql3 = "select " +
                       "ROW_NUMBER() OVER(ORDER BY TIMESTAMP desc) AS ID" +
                       "TIMESTAMP," +
                       "cast(C_TFE as decimal(18,2)) AS TFE," +
                       "cast(C_CAO as decimal(18,2)) AS CAO," +
                       "cast(C_SIO2 as decimal(18,2)) AS SIO2," +
                       "cast(C_AL2O3 as decimal(18,2)) AS AL2O3," +
                       "cast(C_MGO as decimal(18,2)) AS MGO," +
                       "cast(C_S as decimal(18,2)) AS S," +
                       "cast(C_P2O5 as decimal(18,2)) AS P," +
                       "cast(C_R as decimal(18,2)) AS R," +
                       "cast(C_TIO2 as decimal(18,2)) AS TIO2," +
                       "cast(C_K2O as decimal(18,2)) AS K2O," +
                       "cast(C_NA2O as decimal(18,2)) AS NA2O," +
                       "cast(C_CU as decimal(18,2)) AS CU," +
                       "cast(C_PB as decimal(18,2)) AS PB," +
                       "cast(C_ZN as decimal(18,2)) AS ZN," +
                       "cast(C_K as decimal(18,2)) AS K," +
                       "cast(C_MNO as decimal(18,2)) AS MNO    " +
                       "from MC_PRE_SINTER_ANA_RESULT where TIMESTAMP between '" + d1 + "' and '" + d2 + "' order by TIMESTAMP desc";
                    DataTable dataTable3 = dBSQL.GetCommand(sql3);
                    if (dataTable3.Rows.Count > 0)
                    {
                        this.dataGridView1.DataSource = dataTable3;
                    }

                }
            }
            catch(Exception ee)
            {
                vLog.writelog("Data_HIS方法失败" + ee.ToString(),-1);
            }
        }

        //烧结矿预测成分实时按钮
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            dateTimePicker_value1();
            Data_Real();
            _Timer1.Enabled = true;

        }
        /// <summary>
        /// 烧结矿预测成分实时数据(1min)
        /// </summary>
        private void Data_Real()
        {
            try
            {
                if (_FLAG_PAR == 1)
                {
                    string sql3 = "select top(20) " +
                    "ROW_NUMBER() OVER(ORDER BY TIMESTAMP desc) AS ID," +
                    "TIMESTAMP," +
                    "cast(SINCAL_SIN_PV_TFE as decimal(18,2)) AS TFE," +
                    "cast(SINCAL_SIN_PV_CAO as decimal(18,2)) AS CAO," +
                    "cast(SINCAL_SIN_PV_SIO2 as decimal(18,2)) AS SIO2," +
                    "cast(SINCAL_SIN_PV_AL2O3 as decimal(18,2)) AS AL2O3," +
                    "cast(SINCAL_SIN_PV_MGO as decimal(18,2)) AS MGO," +
                    "cast(SINCAL_SIN_PV_S as decimal(18,2)) AS S," +
                    "cast(SINCAL_SIN_PV_P as decimal(18,2)) AS P," +
                    "cast(SINCAL_SIN_PV_R as decimal(18,2)) AS R," +
                    "cast(SINCAL_SIN_PV_TIO2 as decimal(18,2)) AS TIO2," +
                    "cast(SINCAL_SIN_PV_K2O as decimal(18,2)) AS K2O," +
                    "cast(SINCAL_SIN_PV_NA2O as decimal(18,2)) AS NA2O," +
                    "cast(SINCAL_SIN_PV_CU as decimal(18,2)) AS CU," +
                    "cast(SINCAL_SIN_PV_PB as decimal(18,2)) AS PB," +
                    "cast(SINCAL_SIN_PV_ZN as decimal(18,2)) AS ZN," +
                    "cast(SINCAL_SIN_PV_K as decimal(18,2)) AS K," +
                    "cast(SINCAL_SIN_PV_MNO as decimal(18,2)) AS MNO    " +
                    "from MC_MIXCAL_RESULT_1MIN  order by TIMESTAMP desc";
                    DataTable dataTable3 = dBSQL.GetCommand(sql3);
                    if (dataTable3.Rows.Count > 0)
                    {
                        this.dataGridView1.DataSource = dataTable3;
                    }
                }
                else if (_FLAG_PAR == 2)
                {

                    string sql3 = "select top(20) " +
                       "ROW_NUMBER() OVER(ORDER BY TIMESTAMP desc) AS ID" +
                       "TIMESTAMP," +
                       "cast(C_TFE as decimal(18,2)) AS TFE," +
                       "cast(C_CAO as decimal(18,2)) AS CAO," +
                       "cast(C_SIO2 as decimal(18,2)) AS SIO2," +
                       "cast(C_AL2O3 as decimal(18,2)) AS AL2O3," +
                       "cast(C_MGO as decimal(18,2)) AS MGO," +
                       "cast(C_S as decimal(18,2)) AS S," +
                       "cast(C_P2O5 as decimal(18,2)) AS P," +
                       "cast(C_R as decimal(18,2)) AS R," +
                       "cast(C_TIO2 as decimal(18,2)) AS TIO2," +
                       "cast(C_K2O as decimal(18,2)) AS K2O," +
                       "cast(C_NA2O as decimal(18,2)) AS NA2O," +
                       "cast(C_CU as decimal(18,2)) AS CU," +
                       "cast(C_PB as decimal(18,2)) AS PB," +
                       "cast(C_ZN as decimal(18,2)) AS ZN," +
                       "cast(C_K as decimal(18,2)) AS K," +
                       "cast(C_MNO as decimal(18,2)) AS MNO    " +
                       "from MC_PRE_SINTER_ANA_RESULT  order by TIMESTAMP desc";
                    DataTable dataTable3 = dBSQL.GetCommand(sql3);
                    if (dataTable3.Rows.Count > 0)
                    {
                        this.dataGridView1.DataSource = dataTable3;
                    }

                }
                else
                {
                    return;
                }

            }
            catch(Exception ee)
            {
                vLog.writelog("Data_Real方法失败" + ee.ToString(), -1);
            }
        }

        //导出按钮
        private void simpleButton5_Click(object sender, EventArgs e)
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

        //初始化调用单击事件，隐藏曲线
        private void ycqx()
        {
            try
            {
                this.checkBox2.Checked = false;
                this.checkBox3.Checked = false;
                this.checkBox4.Checked = false;
                this.checkBox5.Checked = false;
                this.checkBox6.Checked = false;
                this.checkBox7.Checked = false;
                this.checkBox8.Checked = false;
            }
            catch
            { }
        }
        //曲线实时按钮
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            dateTimePicker_value();
            HIS_CURVE(Convert.ToDateTime(textBox_begin_1.Text), Convert.ToDateTime(textBox_end_1.Text));
        }

        //曲线按时间查询按钮
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            HIS_CURVE(Convert.ToDateTime(textBox_begin_1.Text), Convert.ToDateTime(textBox_end_1.Text));

        }


        /// <summary>
        /// 查询PAR数据表字段
        /// </summary>
        public void MC_PRE_SINTER_ANA_PAR()
        {
            try
            {
                string sql = "select top 1 PAR_CONFIG from MC_PRE_SINTER_ANA_PAR order by TIMESTAMP desc";
                DataTable dataTable = dBSQL.GetCommand(sql);
                if (dataTable.Rows.Count > 0)
                {
                    _FLAG_PAR = int.Parse(dataTable.Rows[0]["PAR_CONFIG"].ToString());
                }
            }
            catch(Exception ee)
            {
                vLog.writelog("MC_PRE_SINTER_ANA_PAR方法失败" + ee.ToString(), -1);
            }
        }

        #region 勾选框响应事件

        /// <summary>
        /// TFE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_Click(object sender, EventArgs e)
        {
            CURVE_Real_Flag = true;

            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            if (_FLAG_PAR == 1)
            {
                //预测
                CURVE_NAME_YC = "MC_MIXCAL_RESULT_1MIN";
                CURVE_NAME_YC_FIELD = "SINCAL_SIN_PV_TFE";

            }
            else if (_FLAG_PAR == 2)
            {
                //预测
                CURVE_NAME_YC = "MC_PRE_SINTER_ANA_RESULT";
                CURVE_NAME_YC_FIELD = "C_TFE";

            }
            //检测
            CURVE_NAME_JC = "M_SINTER_ANALYSIS";
            CURVE_NAME_JC_FIELD = "C_TFE";

        }
        /// <summary>
        /// SIO2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_Click(object sender, EventArgs e)
        {
            CURVE_Real_Flag = true;

            checkBox1.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            if (_FLAG_PAR == 1)
            {
                //预测
                CURVE_NAME_YC = "MC_MIXCAL_RESULT_1MIN";
                CURVE_NAME_YC_FIELD = "SINCAL_SIN_PV_SIO2";

            }
            else if (_FLAG_PAR == 2)
            {
                //预测
                CURVE_NAME_YC = "MC_PRE_SINTER_ANA_RESULT";
                CURVE_NAME_YC_FIELD = "C_SIO2";

            }
            //检测
            CURVE_NAME_JC = "M_SINTER_ANALYSIS";
            CURVE_NAME_JC_FIELD = "C_SIO2";
        }

        /// <summary>
        /// CAO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox3_Click(object sender, EventArgs e)
        {
            CURVE_Real_Flag = true;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            if (_FLAG_PAR == 1)
            {
                //预测
                CURVE_NAME_YC = "MC_MIXCAL_RESULT_1MIN";
                CURVE_NAME_YC_FIELD = "SINCAL_SIN_PV_CAO";

            }
            else if (_FLAG_PAR == 2)
            {
                //预测
                CURVE_NAME_YC = "MC_PRE_SINTER_ANA_RESULT";
                CURVE_NAME_YC_FIELD = "C_CAO";

            }
            //检测
            CURVE_NAME_JC = "M_SINTER_ANALYSIS";
            CURVE_NAME_JC_FIELD = "C_CAO";
        }

        /// <summary>
        /// AL2O3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox4_Click(object sender, EventArgs e)
        {
            CURVE_Real_Flag = true;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            if (_FLAG_PAR == 1)
            {
                //预测
                CURVE_NAME_YC = "MC_MIXCAL_RESULT_1MIN";
                CURVE_NAME_YC_FIELD = "SINCAL_SIN_PV_AL2O3";

            }
            else if (_FLAG_PAR == 2)
            {
                //预测
                CURVE_NAME_YC = "MC_PRE_SINTER_ANA_RESULT";
                CURVE_NAME_YC_FIELD = "C_AL2O3";

            }
            //检测
            CURVE_NAME_JC = "M_SINTER_ANALYSIS";
            CURVE_NAME_JC_FIELD = "C_AL2O3";
        }

        /// <summary>
        /// MGO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox5_Click(object sender, EventArgs e)
        {
            CURVE_Real_Flag = true;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            if (_FLAG_PAR == 1)
            {
                //预测
                CURVE_NAME_YC = "MC_MIXCAL_RESULT_1MIN";
                CURVE_NAME_YC_FIELD = "SINCAL_SIN_PV_MGO";

            }
            else if (_FLAG_PAR == 2)
            {
                //预测
                CURVE_NAME_YC = "MC_PRE_SINTER_ANA_RESULT";
                CURVE_NAME_YC_FIELD = "C_MGO";

            }
            //检测
            CURVE_NAME_JC = "M_SINTER_ANALYSIS";
            CURVE_NAME_JC_FIELD = "C_MGO";
        }

        /// <summary>
        /// R
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox6_Click(object sender, EventArgs e)
        {
            CURVE_Real_Flag = true;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            if (_FLAG_PAR == 1)
            {
                //预测
                CURVE_NAME_YC = "MC_MIXCAL_RESULT_1MIN";
                CURVE_NAME_YC_FIELD = "SINCAL_SIN_PV_R";

            }
            else if (_FLAG_PAR == 2)
            {
                //预测
                CURVE_NAME_YC = "MC_PRE_SINTER_ANA_RESULT";
                CURVE_NAME_YC_FIELD = "C_R";

            }
            //检测
            CURVE_NAME_JC = "M_SINTER_ANALYSIS";
            CURVE_NAME_JC_FIELD = "C_R";
        }

        /// <summary>
        /// S
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox7_Click(object sender, EventArgs e)
        {
            CURVE_Real_Flag = true;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox8.Checked = false;
            if (_FLAG_PAR == 1)
            {
                //预测
                CURVE_NAME_YC = "MC_MIXCAL_RESULT_1MIN";
                CURVE_NAME_YC_FIELD = "SINCAL_SIN_PV_S";

            }
            else if (_FLAG_PAR == 2)
            {
                //预测
                CURVE_NAME_YC = "MC_PRE_SINTER_ANA_RESULT";
                CURVE_NAME_YC_FIELD = "C_S";

            }
            //检测
            CURVE_NAME_JC = "M_SINTER_ANALYSIS";
            CURVE_NAME_JC_FIELD = "C_S";
        }
        /// <summary>
        /// P
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox8_Click(object sender, EventArgs e)
        {
            CURVE_Real_Flag = true;

            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            if (_FLAG_PAR == 1)
            {
                //预测
                CURVE_NAME_YC = "MC_MIXCAL_RESULT_1MIN";
                CURVE_NAME_YC_FIELD = "SINCAL_SIN_PV_P";

            }
            else if (_FLAG_PAR == 2)
            {
                //预测
                CURVE_NAME_YC = "MC_PRE_SINTER_ANA_RESULT";
                CURVE_NAME_YC_FIELD = "C_P2O5";

            }
            //检测
            CURVE_NAME_JC = "M_SINTER_ANALYSIS";
            CURVE_NAME_JC_FIELD = "C_P2O5";
        }
        #endregion

        /// <summary>
        /// 历史数据查询
        /// </summary>
        public void HIS_CURVE(DateTime _begin, DateTime _end)
        {

            try
            {
                string sql_curve_his = "SELECT a.TIMESTAMP, " + CURVE_NAME_YC_FIELD + " AS YCZ," + CURVE_NAME_JC_FIELD + " AS JCZ  FROM " + CURVE_NAME_YC + " a ," + CURVE_NAME_JC + " b where convert(varchar(16),a.TIMESTAMP,121)=convert(varchar(16),b.TIMESTAMP,121) and a.TIMESTAMP between '" + _begin + "' and '" + _end + "' order by a.TIMESTAMP asc";
                DataTable TABLE_curve_his = dBSQL.GetCommand(sql_curve_his);
                if (TABLE_curve_his.Rows.Count > 0 && TABLE_curve_his !=null)
                {
                    lChartPlus100.LChart.Series.Clear();
                    lChartPlus100.LChart.AxisX.Clear();
                    lChartPlus100.LChart.AxisY.Clear();

                    _List_His_TIME.Clear();
                    _List_His_JC.Clear();
                    _List_His_YC.Clear();

                    //  滚轮放大缩小，x轴
                    lChartPlus100.LChart.Zoom = LiveCharts.ZoomingOptions.X;

                    //***声明曲线***

                    //预测值
                    var YCZ = lChartPlus100.MakeLine(0, 0, "YCZ");
                    //检测值
                    var JCZ = lChartPlus100.MakeLine(0, 1, "JCZ");

                    //***end***

                    //***曲线控件绑定曲线***
                    lChartPlus100.LChart.Series.Add(YCZ);
                    lChartPlus100.LChart.Series.Add(JCZ);

                    //***end***

                    ///***线条粗细***
                    YCZ.StrokeThickness = 1;
                    JCZ.StrokeThickness = 1;
                    ///***end

                    //***去除圆点***
                    //  YCZ.PointGeometry = null;
                    // JCZ.PointGeometry = null;

                    //***end***



                    for (int rows = 0; rows < TABLE_curve_his.Rows.Count; rows++)
                    {
                        //时间
                        _List_His_TIME.Add(TABLE_curve_his.Rows[rows]["TIMESTAMP"].ToString());
                        //检测值
                        if (TABLE_curve_his.Rows[rows]["JCZ"].ToString() == "")
                        {
                            _List_His_JC.Add(Double.NaN);
                        }
                        else
                        {
                            double text = Double.Parse(TABLE_curve_his.Rows[rows]["JCZ"].ToString());
                            if (text == 0)
                            {
                                _List_His_JC.Add(Double.NaN);
                            }
                            else
                            {
                                _List_His_JC.Add(Math.Round(Double.Parse(TABLE_curve_his.Rows[rows]["JCZ"].ToString()), 2));
                            }

                        }

                        //预测值
                        if (TABLE_curve_his.Rows[rows]["YCZ"].ToString() == "")
                        {
                            _List_His_YC.Add(Double.NaN);
                        }
                        else
                        {
                            double text = Double.Parse(TABLE_curve_his.Rows[rows]["YCZ"].ToString());
                            if (text == 0)
                            {
                                _List_His_YC.Add(Double.NaN);
                            }
                            else
                            {
                                _List_His_YC.Add(Math.Round(Double.Parse(TABLE_curve_his.Rows[rows]["YCZ"].ToString()), 2));
                            }

                        }

                    }

                    lChartPlus100.LBindData<string, double>("YCZ", _List_His_TIME, _List_His_YC, System.Windows.Media.Brushes.Green, "时间", "");
                    lChartPlus100.LBindData<string, double>("JCZ", _List_His_TIME, _List_His_JC, System.Windows.Media.Brushes.Red, "时间", " ");


                }
                else
                {


                    return;
                }
            }
            catch(Exception ee)
            {
                vLog.writelog("HIS_CURVE方法失败" + ee.ToString(), -1);
            }
        }

        public void Timer_stop()
        {
            _Timer1.Enabled = false;
            _Timer2.Enabled = false;
        }
        public void Timer_state()
        {
            _Timer2.Enabled = true;
            _Timer1.Enabled = true;
        }
        public void _Clear()
        {
            _Timer1.Close();
            _Timer2.Close();
            this.Dispose();
            GC.SuppressFinalize(this);
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

            textBox_begin_1.Text = time_begin.ToString();
            textBox_end_1.Text = time_end.ToString();
        }
        /// <summary>
        /// 开始时间&结束时间赋值
        /// </summary>
        public void dateTimePicker_value1()
        {
            //结束时间
            DateTime time_end = DateTime.Now;
            //开始时间
            DateTime time_begin = time_end.AddDays(-1);

            textBox_begin_2.Text = time_begin.ToString();
            textBox_end_2.Text = time_end.ToString();
        }
    }
}
