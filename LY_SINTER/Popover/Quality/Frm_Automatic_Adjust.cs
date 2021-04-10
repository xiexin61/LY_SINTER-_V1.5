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
    public partial class Frm_Automatic_Adjust : Form
    {

        public System.Timers.Timer _Timer1 { get; set; }
        public static bool isopen = false;
        String biaoming3 = "";
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public Frm_Automatic_Adjust()
        {
            InitializeComponent();
            time_begin_end();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            comboBox1_SelectedIndexChanged();
          
            _Timer1 = new System.Timers.Timer(1000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { _Timer1_Tick(); };
            _Timer1.Enabled = true;
            _Timer1.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
        }
        /// <summary>
        /// 初始化颜色变化定时器响应事件
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
                show(1);
            }
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
        private void comboBox1_SelectedIndexChanged() //object sender, EventArgs e
        {
            shujubiao();

        }
        public void shujubiao()
        {
            if (comboBox1.Text.ToString() == "C调整数据")
            {
                biaoming3 = "MC_SINCAL_C_result";
            }
            else if (comboBox1.Text.ToString() == "R调整数据")
            {
                biaoming3 = "MC_SINCAL_R_result";
            }
            else if (comboBox1.Text.ToString() == "MG调整数据")
            {
                biaoming3 = "MC_SINCAL_MG_result";
            }

        }


        /// <summary>
        /// 查询历史： flag =1 实时 flag = 2
        /// </summary>
        public void show(int _flag)
        {
            if (_flag == 1)
            {
                if (comboBox1.Text.ToString() == "C调整数据")
                {
                    var _sql = "SELECT top(20) " +
                        "ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID," +
                        "TIMESTAMP," +
                        "SINCAL_C_SAMPLE_CODE AS 烧结矿试样号," +
                        "SINCAL_C_COM_RE_ADJ AS 综合C调整量," +
                        "SINCAL_C_BEFORE_Modify AS 调整前C调整值," +
                        "SINCAL_C_AFTER_Modify AS 调整后C调整值," +
                        "SINCAL_C_BEFORE_SV_C AS 调整前参与配料C," +
                        "SINCAL_C_SV_C  AS 调整后参与配料C," +
                        "SINCAL_C_A AS 混合料目标含碳," +
                        "SINCAL_FEO_AIM AS 烧结矿目标FeO," +
                        "SINCAL_C_RET AS 配料混合料含碳," +
                        "SINCAL_C_FEO_TEST AS 烧结矿化验FeO," +
                        "SINCAL_C_CUR AS 当前混合料含碳," +
                        "SINCAL_FeO_TEST_DEV AS 检测亚铁偏差," +
                        "SINCAL_C_DEV AS 亚铁偏差对C影响量," +
                        "SINCAL_C_FeO_RE_ADJ AS 烧结矿FeO变化C调整量," +
                        "SINCAL_C_RM_CHANGE AS 烧返配比变化量," +
                        "SINCAL_C_BILL_SIN_RM_RE_ADJ AS 烧返配比变化C调整量," +
                        "SINCAL_C_BFES_ORE_CHANGE AS 高返配比变化量," +
                        "SINCAL_C_BILL_BFES_ORE_RE_ADJ AS 高返配比变化C调整量," +
                        "SINCAL_C_MIX_SP_LOT_OLD  AS 调整前综合烧损," +
                        "SINCAL_C_MIX_SP_LOT_NEW AS 调整后综合烧损," +
                        "SINCAL_C_LOT_RE_ADJ AS 综合烧损变化C调整量," +
                        "SINCAL_C_NON_FUEL_SP_C_OLD AS 变化前非燃料含碳," +
                        "SINCAL_C_NON_FUEL_SP_C_NEW AS 变化后非燃料含碳," +
                        "SINCAL_C_NONFUEL_RE_ADJ AS 非燃料含碳变化C调整量," +
                        "SINCAL_C_MIX_SP_FeO_OLD  AS 变化前混合料FeO含量," +
                        "SINCAL_C_MIX_SP_FeO_NEW AS 变化后混合料FeO含量," +
                        "SINCAL_C_FeO_MA_RE_ADJ AS 原料FeO变化C调整量," +
                        "SINCAL_C_HOST_ADJ AS 主机参数变化C调整量," +
                        "(CASE SINCAL_C_FEO_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 烧结矿FeO触发," +
                        "(CASE SINCAL_C_BILL_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 烧返配比变化触发," +
                        "(CASE SINCAL_C_BILL_BFES_ORE_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 高返配比变化触发," +
                        "(CASE SINCAL_C_LOT_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 综合烧损变化触发," +
                        "(CASE SINCAL_C_NONFUEL_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 非燃料含碳变化触发," +
                        "(CASE SINCAL_C_FEO_MA_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 原料FeO变化触发," +
                        "(CASE SINCAL_C_HOST_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 主机参数变化触发," +
                        "(CASE SINCAL_C_MODEL_FLAG when '0' THEN 'C自动退出' when '1' then 'C自动投入' else '未知' end)   AS C自动投用状态, " +
                        "(CASE SINCAL_FLAG when '1' THEN '调整完成'  else '禁止调整' end) AS C调整确认状态   " +
                        "from MC_SINCAL_C_result order by TIMESTAMP desc";
                    DataTable _dataTable = dBSQL.GetCommand(_sql);
                    if (_dataTable != null && _dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = _dataTable;
                    }
                }
                else if (comboBox1.Text.ToString() == "R调整数据")
                {
                    var _sql = "select TOP(20) " +
                        "ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID," +
                        " TIMESTAMP ," +
                        "SINCAL_R_SAMPLE_CODE AS 烧结矿试样号," +
                        "SINCAL_R_RE_ADJ AS 碱度调整量," +
                        "SINCAL_R_BEFORE_Modify AS 调整前碱度调整值," +
                        "SINCAL_R_AFTER_Modify AS 调整后碱度调整值," +
                        "SINCAL_R_SV_R_BE AS 调整前参与配料碱度," +
                        "SINCAL_R_SV_R AS 调整后参与配料碱度," +
                        "SINCAL_R_AIM AS 目标碱度," +
                        "SINCAL_R_TEST AS R检测值," +
                        "SINCAL_R_PF AS 执行R偏差," +
                        "SINCAL_R_RE AS 配料碱度," +
                        "SINCAL_R_PRE_AVG AS 当前碱度," +
                        "(case SINCAL_R_MODEL_FLAG when '1' then 'R自动投入' when '0' then 'R自动退出' else '未知' end) as R自动投用状态," +
                        "(case SINCAL_FLAG when '1' then '调整完成'  else '禁止调整' end) as R调整确认状态" +
                       "   from MC_SINCAL_R_result order by TIMESTAMP desc";
                    DataTable _dataTable = dBSQL.GetCommand(_sql);
                    if (_dataTable != null && _dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = _dataTable;
                    }
                }
                else if (comboBox1.Text.ToString() == "MG调整数据")
                {
                    var _sql = "select TOP(20) " +
                       "ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID," +
                       " TIMESTAMP ," +
                       "SINCAL_MG_SAMPLE_CODE AS 烧结矿试样号," +
                       "SINCAL_MG_RE_ADJ AS Mg调整量," +
                       "SINCAL_MG_BEFORE_Modify AS 调整前Mg调整值," +
                        "SINCAL_MG_AFTER_Modify AS 调整后Mg调整值," +
                         "SINCAL_MG_SV_R_BE AS 调整前参与配料Mg," +
                          "SINCAL_MG_SV_R AS 调整后参与配料Mg," +
                          "SINCAL_MG_AIM AS 目标Mg," +
                           "SINCAL_MG_TEST AS Mg检测值," +
                           "SINCAL_MG_PF AS 执行Mg偏差," +
                           "SINCAL_MG_RE AS 配料Mg," +
                            "SINCAL_MG_PRE_AVG AS 当前Mg," +
                             "( CASE SINCAL_MG_MODEL_FLAG WHEN '1' THEN 'MG自动投入' WHEN '0' THEN 'MG自动退出' END) AS Mg自动投用状态," +
                             "( CASE SINCAL_FLAG WHEN '1' THEN '调整完成' else '禁止调整' END) AS Mg调整确认状态 " +
                      " from MC_SINCAL_MG_result order by TIMESTAMP desc";
                    DataTable _dataTable = dBSQL.GetCommand(_sql);
                    if (_dataTable != null && _dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = _dataTable;
                    }
                }

            }
            else if (_flag == 2)
            {
                if (comboBox1.Text.ToString() == "C调整数据")
                {
                    var _sql = "SELECT  " +
                        "ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID," +
                        "TIMESTAMP," +
                        "SINCAL_C_SAMPLE_CODE AS 烧结矿试样号," +
                        "SINCAL_C_COM_RE_ADJ AS 综合C调整量," +
                        "SINCAL_C_BEFORE_Modify AS 调整前C调整值," +
                        "SINCAL_C_AFTER_Modify AS 调整后C调整值," +
                        "SINCAL_C_BEFORE_SV_C AS 调整前参与配料C," +
                        "SINCAL_C_SV_C  AS 调整后参与配料C," +
                        "SINCAL_C_A AS 混合料目标含碳," +
                        "SINCAL_FEO_AIM AS 烧结矿目标FeO," +
                        "SINCAL_C_RET AS 配料混合料含碳," +
                        "SINCAL_C_FEO_TEST AS 烧结矿化验FeO," +
                        "SINCAL_C_CUR AS 当前混合料含碳," +
                        "SINCAL_FeO_TEST_DEV AS 检测亚铁偏差," +
                        "SINCAL_C_DEV AS 亚铁偏差对C影响量," +
                        "SINCAL_C_FeO_RE_ADJ AS 烧结矿FeO变化C调整量," +
                        "SINCAL_C_RM_CHANGE AS 烧返配比变化量," +
                        "SINCAL_C_BILL_SIN_RM_RE_ADJ AS 烧返配比变化C调整量," +
                        "SINCAL_C_BFES_ORE_CHANGE AS 高返配比变化量," +
                        "SINCAL_C_BILL_BFES_ORE_RE_ADJ AS 高返配比变化C调整量," +
                        "SINCAL_C_MIX_SP_LOT_OLD  AS 调整前综合烧损," +
                        "SINCAL_C_MIX_SP_LOT_NEW AS 调整后综合烧损," +
                        "SINCAL_C_LOT_RE_ADJ AS 综合烧损变化C调整量," +
                        "SINCAL_C_NON_FUEL_SP_C_OLD AS 变化前非燃料含碳," +
                        "SINCAL_C_NON_FUEL_SP_C_NEW AS 变化后非燃料含碳," +
                        "SINCAL_C_NONFUEL_RE_ADJ AS 非燃料含碳变化C调整量," +
                        "SINCAL_C_MIX_SP_FeO_OLD  AS 变化前混合料FeO含量," +
                        "SINCAL_C_MIX_SP_FeO_NEW AS 变化后混合料FeO含量," +
                        "SINCAL_C_FeO_MA_RE_ADJ AS 原料FeO变化C调整量," +
                        "SINCAL_C_HOST_ADJ AS 主机参数变化C调整量," +
                        "(CASE SINCAL_C_FEO_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 烧结矿FeO触发," +
                        "(CASE SINCAL_C_BILL_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 烧返配比变化触发," +
                        "(CASE SINCAL_C_BILL_BFES_ORE_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 高返配比变化触发," +
                        "(CASE SINCAL_C_LOT_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 综合烧损变化触发," +
                        "(CASE SINCAL_C_NONFUEL_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 非燃料含碳变化触发," +
                        "(CASE SINCAL_C_FEO_MA_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 原料FeO变化触发," +
                        "(CASE SINCAL_C_HOST_FLAG when '0' THEN '未触发' when '1' then '触发' else '未知' end) AS 主机参数变化触发," +
                        "(CASE SINCAL_C_MODEL_FLAG when '0' THEN 'C自动退出' when '1' then 'C自动投入' else '未知' end)   AS C自动投用状态, " +
                        "(CASE SINCAL_FLAG when '1' THEN '调整完成'  else '禁止调整' end) AS C调整确认状态   " +
                        " from MC_SINCAL_C_result where TIMESTAMP >='" + textBox_begin.Text.ToString() + "' and TIMESTAMP <= '" + textBox_end.Text.ToString() + "' order by TIMESTAMP desc";
                    DataTable _dataTable = dBSQL.GetCommand(_sql);
                    if (_dataTable != null && _dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = _dataTable;
                    }
                }
                else if (comboBox1.Text.ToString() == "R调整数据")
                {
                    var _sql = "SELECT  " +
                        "ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID," +
                        " TIMESTAMP ," +
                        "SINCAL_R_SAMPLE_CODE AS 烧结矿试样号," +
                        "SINCAL_R_RE_ADJ AS 碱度调整量," +
                        "SINCAL_R_BEFORE_Modify AS 调整前碱度调整值," +
                        "SINCAL_R_AFTER_Modify AS 调整后碱度调整值," +
                        "SINCAL_R_SV_R_BE AS 调整前参与配料碱度," +
                        "SINCAL_R_SV_R AS 调整后参与配料碱度," +
                        "SINCAL_R_AIM AS 目标碱度," +
                        "SINCAL_R_TEST AS R检测值," +
                        "SINCAL_R_PF AS 执行R偏差," +
                        "SINCAL_R_RE AS 配料碱度," +
                        "SINCAL_R_PRE_AVG AS 当前碱度," +
                        "(case SINCAL_R_MODEL_FLAG when '1' then 'R自动投入' when '0' then 'R自动退出' else '未知' end) as R自动投用状态," +
                        "(case SINCAL_FLAG when '1' then '调整完成'  else '禁止调整' end) as R调整确认状态" +
                       " from MC_SINCAL_R_result where TIMESTAMP >='" + textBox_begin.Text.ToString() + "' and TIMESTAMP <= '" + textBox_end.Text.ToString() + "' order by TIMESTAMP desc";
                    DataTable _dataTable = dBSQL.GetCommand(_sql);
                    if (_dataTable != null && _dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = _dataTable;
                    }
                }
                else if (comboBox1.Text.ToString() == "MG调整数据")
                {
                    var _sql = "select  " +
                       "ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID," +
                       " TIMESTAMP ," +
                       "SINCAL_MG_SAMPLE_CODE AS 烧结矿试样号," +
                       "SINCAL_MG_RE_ADJ AS Mg调整量," +
                       "SINCAL_MG_BEFORE_Modify AS 调整前Mg调整值," +
                        "SINCAL_MG_AFTER_Modify AS 调整后Mg调整值," +
                         "SINCAL_MG_SV_R_BE AS 调整前参与配料Mg," +
                          "SINCAL_MG_SV_R AS 调整后参与配料Mg," +
                          "SINCAL_MG_AIM AS 目标Mg," +
                           "SINCAL_MG_TEST AS Mg检测值," +
                           "SINCAL_MG_PF AS 执行Mg偏差," +
                           "SINCAL_MG_RE AS 配料Mg," +
                            "SINCAL_MG_PRE_AVG AS 当前Mg," +
                             "( CASE SINCAL_MG_MODEL_FLAG WHEN '1' THEN 'MG自动投入' WHEN '0' THEN 'MG自动退出' END) AS Mg自动投用状态," +
                             "( CASE SINCAL_FLAG WHEN '1' THEN '调整完成' else '禁止调整' END) AS Mg调整确认状态 " +
                      " from MC_SINCAL_MG_result where TIMESTAMP >='" + textBox_begin.Text.ToString() + "' and TIMESTAMP <= '" + textBox_end.Text.ToString() + "' order by TIMESTAMP desc";
                    DataTable _dataTable = dBSQL.GetCommand(_sql);
                    if (_dataTable != null && _dataTable.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = _dataTable;
                    }
                }
            }
            else
            {
                return;
            }
            _class();

        }

        //实时按钮
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            show(1);
        }
        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            show(2);
        }

        //下拉框事件
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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
    }
}
