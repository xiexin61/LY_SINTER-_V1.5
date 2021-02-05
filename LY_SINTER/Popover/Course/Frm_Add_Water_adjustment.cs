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
    public partial class Frm_Add_Water_adjustment : Form
    {
        public vLog vLog { get; set; }
        public static bool isopen = false;
        String biaoming3 = "";
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        List<string> yw = new List<string>();
        List<string> zw = new List<string>();
        public Frm_Add_Water_adjustment()
        {
            InitializeComponent();
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            
            if (vLog == null)
                vLog = new vLog(".\\log_Page\\Course\\Frm_Add_Water_adjustment\\");

            //  dateTimePicker_value();
            text_real();
        }

        private void comboBox1_SelectedIndexChanged() //object sender, EventArgs e
        {
            shujubiao();
            selectC_R();
        }
        ////下拉框选择对应的表名
        public void shujubiao()
        {
            try
            {
                if (comboBox1.Text.ToString() == "参数")
                {
                    biaoming3 = "MC_WATERCAL_BUTTON";
                }
                if (comboBox1.Text.ToString() == "参数1")
                {
                    biaoming3 = "MC_WATERCAL_RESULT";
                }
                if (comboBox1.Text.ToString() == "参数2")
                {
                    biaoming3 = "MC_WATERCAL_RESULT_1MIN";
                }
            }
            catch
            { }
        }

        private void zhongwen()
        {
            try
            {
                for (int i = 0; i < yw.Count; i++)
                {
                    if (yw[i] == "TIMESTAMP")
                    {
                        zw.Add("插入时间");
                    }
                    else if (yw[i] == "WATCAL_START_UP_BUTTON")
                    {
                        zw.Add("二级界面“自动加水开始计算开关”");
                    }
                    else if (yw[i] == "WATCAL_FLAG1")
                    {
                        zw.Add("一级界面“加水二级投入开关”投入/退出标志");
                    }
                    else if (yw[i] == "WATCAL_FLAG2")
                    {
                        zw.Add("后馈补偿加水量数值正负标志");
                    }
                    else if (yw[i] == "WATCAL_FLAG3")
                    {
                        zw.Add("模型插库、下发结果标志");
                    }
                    else if (yw[i] == "WATCAL_FLAG4")
                    {
                        zw.Add("后馈目标与检测水分偏差补偿投入/退出状态");
                    }
                    else if (yw[i] == "WATCAL_FLAG5")
                    {
                        zw.Add("重新计算“原始水分纠偏系数”触发标志");
                    }
                    else if (yw[i] == "WATCAL_FLAG6")
                    {
                        zw.Add("停止加水计算标志");
                    }
                    else if (yw[i] == "WATCAL_FLAG7")
                    {
                        zw.Add("二级界面“自动加水开始计算开关”投入/退出标志");
                    }
                    else if (yw[i] == "WATCAL_FLAG8")
                    {
                        zw.Add("模型调整触发条件");
                    }
                    else if (yw[i] == "WATCAL_1M_B_B_SIGNAL")
                    {
                        zw.Add("一混前皮带启停信号状态");
                    }
                    else if (yw[i] == "WATCAL_1M_B_W")
                    {
                        zw.Add("一混前皮带秤值");
                    }
                    else if (yw[i] == "WATCAL_SPEED_SP")
                    {
                        zw.Add("烧结机设定机速");
                    }
                    else if (yw[i] == "WATCAL_SPEED_PV")
                    {
                        zw.Add("烧结机反馈机速");
                    }
                    else if (yw[i] == "WATCAL_1M_H2O_AIM")
                    {
                        zw.Add("一混目标水分值");
                    }
                    else if (yw[i] == "WATCAL_1M_H2O_TEST")
                    {
                        zw.Add("一混后检测水分");
                    }
                    else if (yw[i] == "WATCAL_1M_FT_SP")
                    {
                        zw.Add("一混加水流量设定值");
                    }
                    else if (yw[i] == "WATCAL_1M_FT_PV")
                    {
                        zw.Add("一混加水流量反馈值");
                    }
                    else if (yw[i] == "WATCAL_T_SP_W")
                    {
                        zw.Add("当前设定总料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_1")
                    {
                        zw.Add("1#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_2")
                    {
                        zw.Add("2#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_3")
                    {
                        zw.Add("3#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_4")
                    {
                        zw.Add("4#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_5")
                    {
                        zw.Add("5#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_6")
                    {
                        zw.Add("6#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_7")
                    {
                        zw.Add("7#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_8")
                    {
                        zw.Add("8#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_9")
                    {
                        zw.Add("9#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_10")
                    {
                        zw.Add("10#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_11")
                    {
                        zw.Add("11#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_12")
                    {
                        zw.Add("12#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_13")
                    {
                        zw.Add("13#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_14")
                    {
                        zw.Add("14#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_15")
                    {
                        zw.Add("15#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_16")
                    {
                        zw.Add("16#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_PV_W_17")
                    {
                        zw.Add("17#仓实际下料量");
                    }
                    else if (yw[i] == "WATCAL_H2O_1")
                    {
                        zw.Add("1#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_2")
                    {
                        zw.Add("2#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_3")
                    {
                        zw.Add("3#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_4")
                    {
                        zw.Add("4#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_5")
                    {
                        zw.Add("5#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_6")
                    {
                        zw.Add("6#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_7")
                    {
                        zw.Add("7#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_8")
                    {
                        zw.Add("8#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_9")
                    {
                        zw.Add("9#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_10")
                    {
                        zw.Add("10#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_11")
                    {
                        zw.Add("11#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_12")
                    {
                        zw.Add("12#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_13")
                    {
                        zw.Add("13#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_14")
                    {
                        zw.Add("14#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_15")
                    {
                        zw.Add("15#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_16")
                    {
                        zw.Add("16#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_H2O_17")
                    {
                        zw.Add("17#仓在用水分值");
                    }
                    else if (yw[i] == "WATCAL_CODE_1")
                    {
                        zw.Add("1#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_2")
                    {
                        zw.Add("2#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_3")
                    {
                        zw.Add("3#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_4")
                    {
                        zw.Add("4#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_5")
                    {
                        zw.Add("5#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_6")
                    {
                        zw.Add("6#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_7")
                    {
                        zw.Add("7#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_8")
                    {
                        zw.Add("8#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_9")
                    {
                        zw.Add("9#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_10")
                    {
                        zw.Add("10#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_11")
                    {
                        zw.Add("11#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_12")
                    {
                        zw.Add("12#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_13")
                    {
                        zw.Add("13#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_14")
                    {
                        zw.Add("14#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_15")
                    {
                        zw.Add("15#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_16")
                    {
                        zw.Add("16#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_CODE_17")
                    {
                        zw.Add("17#仓物料编码");
                    }
                    else if (yw[i] == "WATCAL_RAW_H2O")
                    {
                        zw.Add("一混前混合料含水量计算值");
                    }
                    else if (yw[i] == "WATCAL_LIME_SP_W")
                    {
                        zw.Add("所有生石灰仓的设定下料量和");
                    }
                    else if (yw[i] == "WATCAL_1M_H2O_AIM_RE")
                    {
                        zw.Add("计算目标水分值");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_X")
                    {
                        zw.Add("计算一混加水流量设定值X");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_LIME_X2")
                    {
                        zw.Add("计算生石灰消化加水量X2");
                    }
                    else if (yw[i] == "WATCAL_RAW_H2O_B")
                    {
                        zw.Add("原始水分纠偏系数b");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_H2O_AIM_X1")
                    {
                        zw.Add("根据目标水分计算加水量X1");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_LIME_X3")
                    {
                        zw.Add("生石灰下料波动前馈补偿消化加水量X3");
                    }
                    else if (yw[i] == "WATCAL_1M_X3_E_S")
                    {
                        zw.Add("上周期内生石灰仓下料波动量e_s");
                    }
                    else if (yw[i] == "WATCAL_1M_E_Z1")
                    {
                        zw.Add("上周期内1-7#仓下料波动量");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_Z1")
                    {
                        zw.Add("1-7#仓下料波动前馈补偿加水量Z1");
                    }
                    else if (yw[i] == "WATCAL_1M_E_Z2")
                    {
                        zw.Add("上周期内8-12#仓下料波动量");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_Z2")
                    {
                        zw.Add("8-12#仓下料波动前馈补偿加水量Z2");
                    }
                    else if (yw[i] == "WATCAL_1M_E_Z3")
                    {
                        zw.Add("上周期内13-17#仓下料波动量");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_Z3")
                    {
                        zw.Add("13-17#仓下料波动前馈补偿加水量Z3");
                    }
                    else if (yw[i] == "WATCAL_C_TEST_AV_NEW")
                    {
                        zw.Add("当前周期T_S时间内，一混后水分检测值平均值");
                    }
                    else if (yw[i] == "WATCAL_F_TEST_AV_NEW")
                    {
                        zw.Add("当前周期T_S时间内，一混加水流量反馈值平均值");
                    }
                    else if (yw[i] == "WATCAL_C_TEST_AV_OLD")
                    {
                        zw.Add("上一周期T_S时间内，一混后水分检测值平均值");
                    }
                    else if (yw[i] == "WATCAL_F_TEST_AV_OLD")
                    {
                        zw.Add("上一周期T_S时间内，一混加水流量反馈值平均值");
                    }
                    else if (yw[i] == "WATCAL_F_TREND_COM")
                    {
                        zw.Add("检测水分趋势变化补偿加水量");
                    }
                    else if (yw[i] == "WATCAL_F_DEV_COM")
                    {
                        zw.Add("检测水分和目标水分偏差计算的补偿加水量");
                    }
                    else if (yw[i] == "WATCAL_K_NUMBER")
                    {
                        zw.Add("连续同向加水次数");
                    }
                    else if (yw[i] == "WATCAL_F_COM_ORI")
                    {
                        zw.Add("总后馈补偿加水量（原始值）");
                    }
                    else if (yw[i] == "WATCAL_F_COM_PRO")
                    {
                        zw.Add("总后馈补偿加水量（中间修正值）");
                    }
                    else if (yw[i] == "WATCAL_K_ANA")
                    {
                        zw.Add("补偿加水量有效性分析系数");
                    }
                    else if (yw[i] == "WATCAL_F_COM_FINAL")
                    {
                        zw.Add("每周期计算最终后馈补偿加水量");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_H")
                    {
                        zw.Add("后馈补偿加水量（累积值）");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_X_MAX")
                    {
                        zw.Add("计算设定加水量上限");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_X_MIN")
                    {
                        zw.Add("计算设定加水量下限");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_X_OUT")
                    {
                        zw.Add("上下限修订后计算设定加水流量");
                    }
                    else if (yw[i] == "WATCAL_1M_B_B_SIGNAL")
                    {
                        zw.Add("一混前皮带启停信号状态");
                    }
                    else if (yw[i] == "WATCAL_1M_B_W")
                    {
                        zw.Add("一混前皮带秤值");
                    }
                    else if (yw[i] == "WATCAL_SPEED_SP")
                    {
                        zw.Add("烧结机设定机速");
                    }
                    else if (yw[i] == "WATCAL_SPEED_PV")
                    {
                        zw.Add("烧结机反馈机速");
                    }
                    else if (yw[i] == "WATCAL_1M_H2O_AIM")
                    {
                        zw.Add("一混目标水分值（一级界面）");
                    }
                    else if (yw[i] == "WATCAL_1M_H2O_AIM_RE")
                    {
                        zw.Add("计算目标水分值（将水分仪检测值赋值给目标水分值）");
                    }
                    else if (yw[i] == "WATCAL_1M_H2O_TEST")
                    {
                        zw.Add("一混后检测水分（一混后测水仪检测水分）");
                    }
                    else if (yw[i] == "WATCAL_T_SP_W")
                    {
                        zw.Add("当前设定总料量");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_X_OUT")
                    {
                        zw.Add("计算一混加水流量设定值(模型下发值)");
                    }
                    else if (yw[i] == "WATCAL_1M_PT")
                    {
                        zw.Add("一混水管压力");
                    }
                    else if (yw[i] == "WATCAL_1M_VAL_B_SP")
                    {
                        zw.Add("一混加水大阀阀位设定值");
                    }
                    else if (yw[i] == "WATCAL_1M_VAL_B_PV")
                    {
                        zw.Add("一混加水大阀阀位反馈值");
                    }
                    else if (yw[i] == "WATCAL_1M_VAL_L_SP")
                    {
                        zw.Add("一混加水小阀阀位设定值");
                    }
                    else if (yw[i] == "WATCAL_1M_VAL_L_PV")
                    {
                        zw.Add("一混加水小阀阀位反馈值");
                    }
                    else if (yw[i] == "WATCAL_2M_FT_SP")
                    {
                        zw.Add("二混加水流量设定值");
                    }
                    else if (yw[i] == "WATCAL_2M_FT_PV")
                    {
                        zw.Add("二混加水流量反馈值");
                    }
                    else if (yw[i] == "WATCAL_2M_VAL_SP")
                    {
                        zw.Add("二混加水阀位反馈值");
                    }
                    else if (yw[i] == "WATCAL_2M_VAL_PV")
                    {
                        zw.Add("二混加水阀位设定值");
                    }
                    else if (yw[i] == "WATCAL_BLEND_LEVEL")
                    {
                        zw.Add("小矿槽料位");
                    }
                    else if (yw[i] == "WATCAL_THICK_SP")
                    {
                        zw.Add("料层厚度");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_X_MAX")
                    {
                        zw.Add("一混加水流量自动调整下限值");
                    }
                    else if (yw[i] == "WATCAL_1M_CAL_X_MIN")
                    {
                        zw.Add("一混加水流量自动调整上限值");
                    }
                }
            }
            catch
            { }
        }

        private void selectC_R()
        {
            try
            {
                yw.Clear();
                zw.Clear();
                if (comboBox1.Text != "")
                {
                    //查询出字段个数
                    string sql1 = "select count(name) from syscolumns where id=(select id from sysobjects where xtype='u' and name='" + biaoming3 + "')";
                    DataTable RowCount = dBSQL.GetCommand(sql1);
                    int rc = int.Parse(RowCount.Rows[0][0].ToString());
                    //查询出数据个数
                    string sql2 = "select count(TIMESTAMP) from " + biaoming3 + "";
                    DataTable RowCount1 = dBSQL.GetCommand(sql2);
                    int rc1 = int.Parse(RowCount1.Rows[0][0].ToString());
                    if (rc1 != 0)
                    {
                        //需要显示的列数
                        dataGridView1.ColumnCount = rc + 1;
                        //需要显示的行数
                        dataGridView1.RowCount = rc1;
                        //第一列标题
                        dataGridView1.Columns[0].HeaderText = "序号";
                        dataGridView1.Columns[0].Width = 65;
                        //第二列标题
                        dataGridView1.Columns[1].HeaderText = "时间";
                        dataGridView1.Columns[1].Width = 120;
                        //查询字段名称
                        string sql3 = "select COLUMN_NAME from information_schema.COLUMNS where table_name = '" + biaoming3 + "';";
                        DataTable RowCount2 = dBSQL.GetCommand(sql3);
                        for (int aa = 0; aa < RowCount2.Rows.Count; aa++)
                        {
                            yw.Add(Convert.ToString(RowCount2.Rows[aa][0]));
                        }
                        // zhongwen();//转换中文
                        //循环显示标题
                        for (int i = 0; i < dataGridView1.ColumnCount - 2; i++)
                        {
                            dataGridView1.Columns[i + 2].HeaderText = zw[i + 1];
                            dataGridView1.Columns[i + 2].Width = 150;
                        }
                        //序号
                        for (int j = 0; j < dataGridView1.RowCount; j++)
                        {
                            dataGridView1.Rows[j].Cells[0].Value = Convert.ToString(j + 1);
                        }
                        //表格填充数据
                        string sql4 = "select * from " + biaoming3 + " order by TIMESTAMP desc";
                        DataTable dataTable3 = dBSQL.GetCommand(sql4);

                        for (int a = 0; a < dataGridView1.RowCount; a++)
                        {
                            for (int b = 0; b < dataGridView1.ColumnCount - 1; b++)
                            {
                                dataGridView1.Rows[a].Cells[b + 1].Value = Convert.ToString(dataTable3.Rows[a][b]);
                                this.dataGridView1.Rows[a].Cells[b].ReadOnly = true;
                            }
                        }
                    }
                    else MessageBox.Show("没有数据！");
                }
                else MessageBox.Show("请选择参数！");
            }
            catch
            { }
        }

        //查询按钮
        private void button1_Click(object sender, EventArgs e)
        {
            //  text_his(dateTimePicker1.Value, dateTimePicker2.Value);
            //if (comboBox1.Text != "")
            //{
            //    DateTime d1 = dateTimePicker1.Value;
            //    DateTime d2 = dateTimePicker2.Value;
            //    shujubiao();

            //    //查询出字段个数
            //    string sql1 = "select count(name) from syscolumns where id=(select id from sysobjects where xtype='u' and name='" + biaoming3 + "')";
            //    DataTable RowCount = dBSQL.GetCommand(sql1);
            //    int rc = int.Parse(RowCount.Rows[0][0].ToString());
            //    //按时间查询出数据个数
            //    string sql2 = "select count(TIMESTAMP) from " + biaoming3 + " where TIMESTAMP between '" + d1 + "' and '" + d2 + "'";
            //    DataTable RowCount1 = dBSQL.GetCommand(sql2);
            //    int rc1 = int.Parse(RowCount1.Rows[0][0].ToString());
            //    if (rc1 != 0)
            //    {
            //        //需要显示的列数
            //        dataGridView1.ColumnCount = rc + 1;
            //        //需要显示的行数
            //        dataGridView1.RowCount = rc1;
            //        //第一列标题
            //        dataGridView1.Columns[0].HeaderText = "序号";
            //        dataGridView1.Columns[0].Width = 65;
            //        //第二列标题
            //        dataGridView1.Columns[1].HeaderText = "时间";
            //        dataGridView1.Columns[1].Width = 120;
            //        //查询字段名称
            //        string sql3 = "select COLUMN_NAME from information_schema.COLUMNS where table_name = '" + biaoming3 + "';";
            //        DataTable RowCount2 = dBSQL.GetCommand(sql3);
            //        //循环显示标题
            //        for (int i = 0; i < dataGridView1.ColumnCount - 2; i++)
            //        {
            //            dataGridView1.Columns[i + 2].HeaderText = Convert.ToString(RowCount2.Rows[i + 1][0]);
            //            dataGridView1.Columns[i + 2].Width = 150;
            //        }
            //        //序号
            //        for (int j = 0; j < dataGridView1.RowCount; j++)
            //        {
            //            dataGridView1.Rows[j].Cells[0].Value = Convert.ToString(j + 1);
            //        }
            //        //表格填充数据
            //        string sql4 = "select * from " + biaoming3 + " where TIMESTAMP between '" + d1 + "' and '" + d2 + "' order by TIMESTAMP desc";
            //        DataTable dataTable3 = dBSQL.GetCommand(sql4);

            //        for (int a = 0; a < dataGridView1.RowCount; a++)
            //        {
            //            for (int b = 0; b < dataGridView1.ColumnCount - 1; b++)
            //            {
            //                dataGridView1.Rows[a].Cells[b + 1].Value = Convert.ToString(dataTable3.Rows[a][b]);
            //            }
            //        }
            //    }
            //    else MessageBox.Show("此时间段没有数据！");
            //}
            //else MessageBox.Show("请选择参数！");
        }


        //实时按钮
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            text_real();
        }
        //导出
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
            text_his(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));
            //try
            //{
            //    yw.Clear();
            //    zw.Clear();
            //    if (comboBox1.Text != "")
            //    {
            //        DateTime d1 = dateTimePicker1.Value;
            //        DateTime d2 = dateTimePicker2.Value;
            //        shujubiao();

            //        //查询出字段个数
            //        string sql1 = "select count(name) from syscolumns where id=(select id from sysobjects where xtype='u' and name='" + biaoming3 + "')";
            //        DataTable RowCount = dBSQL.GetCommand(sql1);
            //        int rc = int.Parse(RowCount.Rows[0][0].ToString());
            //        //按时间查询出数据个数
            //        string sql2 = "select count(TIMESTAMP) from " + biaoming3 + " where TIMESTAMP between '" + d1 + "' and '" + d2 + "'";
            //        DataTable RowCount1 = dBSQL.GetCommand(sql2);
            //        int rc1 = int.Parse(RowCount1.Rows[0][0].ToString());
            //        if (rc1 != 0)
            //        {
            //            //需要显示的列数
            //            dataGridView1.ColumnCount = rc + 1;
            //            //需要显示的行数
            //            dataGridView1.RowCount = rc1;
            //            //第一列标题
            //            dataGridView1.Columns[0].HeaderText = "序号";
            //            dataGridView1.Columns[0].Width = 65;
            //            //第二列标题
            //            dataGridView1.Columns[1].HeaderText = "时间";
            //            dataGridView1.Columns[1].Width = 120;
            //            //查询字段名称
            //            string sql3 = "select COLUMN_NAME from information_schema.COLUMNS where table_name = '" + biaoming3 + "';";
            //            DataTable RowCount2 = dBSQL.GetCommand(sql3);
            //            for (int aa = 0; aa < RowCount2.Rows.Count; aa++)
            //            {
            //                yw.Add(Convert.ToString(RowCount2.Rows[aa][0]));
            //            }
            //            zhongwen();//转换中文
            //            //循环显示标题
            //            for (int i = 0; i < dataGridView1.ColumnCount - 2; i++)
            //            {
            //                dataGridView1.Columns[i + 2].HeaderText = zw[i + 1];
            //                dataGridView1.Columns[i + 2].Width = 150;
            //            }
            //            //序号
            //            for (int j = 0; j < dataGridView1.RowCount; j++)
            //            {
            //                dataGridView1.Rows[j].Cells[0].Value = Convert.ToString(j + 1);
            //            }
            //            //表格填充数据
            //            string sql4 = "select * from " + biaoming3 + " where TIMESTAMP between '" + d1 + "' and '" + d2 + "' order by TIMESTAMP desc";
            //            DataTable dataTable3 = dBSQL.GetCommand(sql4);

            //            for (int a = 0; a < dataGridView1.RowCount; a++)
            //            {
            //                for (int b = 0; b < dataGridView1.ColumnCount - 1; b++)
            //                {
            //                    dataGridView1.Rows[a].Cells[b + 1].Value = Convert.ToString(dataTable3.Rows[a][b]);
            //                }
            //            }
            //        }
            //        else MessageBox.Show("此时间段没有数据！");
            //    }
            //    else MessageBox.Show("请选择参数！");
            //}
            //catch
            //{ }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

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
        public void text_real()
        {
            try
            {
                var sql = "select top (20) " +
               "TIMESTAMP  ," +
               "(case when WATCAL_FLAG1 = 1 then '投入' when WATCAL_FLAG1 = 0 then '退出' else '异常' end) as WATCAL_FLAG1," +
               "(case when WATCAL_FLAG8 = 1 then '周期' when WATCAL_FLAG8 = 2 then '目标水分变化' when WATCAL_FLAG8 = 3 then '原始水分变化' when WATCAL_FLAG8 = 4 then '" +
               "总料量变化'  when WATCAL_FLAG8 = 5 then '原始水分纠偏' else ' ' end) as WATCAL_FLAG8," +
               "(case when WATCAL_FLAG6 = 1 then '带料停机' when WATCAL_FLAG6 = 2 then '空料停机' when WATCAL_FLAG6 = 3 then '空料停机开始位置'else ' ' end) as WATCAL_FLAG6," +
               "WATCAL_1M_CAL_X_OUT,WATCAL_RAW_H2O,WATCAL_RAW_H2O_B,WATCAL_1M_H2O_AIM,WATCAL_1M_H2O_AIM_RE,WATCAL_1M_H2O_TEST,X_DUST_FT_PV,WATCAL_1M_CAL_LIME_X3," +
               "WATCAL_1M_CAL_Z1,WATCAL_1M_CAL_Z2,WATCAL_1M_CAL_Z3,WATCAL_1M_CAL_H," +
               "(case when WATCAL_1M_B_B_SIGNAL = 1 then '启动' when WATCAL_1M_B_B_SIGNAL = 0 then '停止' else '异常' end) as WATCAL_1M_B_B_SIGNAL," +
               "WATCAL_1M_B_W,WATCAL__SPEED_SP,WATCAL__SPEED_PV,WATCAL_T_SP_W," +
               "WATCAL_1M_CAL_X_MAX,WATCAL_1M_CAL_X_MIN," +
               "(case when WATCAL_FLAG4 = 1 then '投入' when WATCAL_FLAG4 = 2 then '退出' else '异常' end) as WATCAL_FLAG4," +
               "(case when WATCAL_FLAG3 = 1 then '下发完成' when WATCAL_FLAG3 = 2 then '未下发' else '异常' end) as WATCAL_FLAG3" +
               " from MC_WATERCAL_RESULT order by TIMESTAMP desc";
                DataTable table = dBSQL.GetCommand(sql);
                if (table.Rows.Count > 0 && table != null)
                {
                    dataGridView1.DataSource = table;
                }
                else
                {
                    string mistake = "加水页面调整记录查询失败" + sql;
                    vLog.writelog(mistake, -1);
                }
            }
            catch (Exception ee)
            {
                string mistake = "text_real：加水页面调整数据弹出框查询失败" + ee.ToString();
                vLog.writelog(mistake, -1);
            }

        }
        public void text_his(DateTime _begin_time, DateTime _end_time)
        {
            try
            {
                var sql = "select  " +
               "TIMESTAMP  ," +
               "(case when WATCAL_FLAG1 = 1 then '投入' when WATCAL_FLAG1 = 0 then '退出' else '异常' end) as WATCAL_FLAG1," +
               "(case when WATCAL_FLAG8 = 1 then '周期' when WATCAL_FLAG8 = 2 then '目标水分变化' when WATCAL_FLAG8 = 3 then '原始水分变化' when WATCAL_FLAG8 = 4 then '" +
               "总料量变化'  when WATCAL_FLAG8 = 5 then '原始水分纠偏' else ' ' end) as WATCAL_FLAG8," +
               "(case when WATCAL_FLAG6 = 1 then '带料停机' when WATCAL_FLAG6 = 2 then '空料停机' when WATCAL_FLAG6 = 3 then '空料停机开始位置'else ' ' end) as WATCAL_FLAG6," +
               "WATCAL_1M_CAL_X_OUT,WATCAL_RAW_H2O,WATCAL_RAW_H2O_B,WATCAL_1M_H2O_AIM,WATCAL_1M_H2O_AIM_RE,WATCAL_1M_H2O_TEST,X_DUST_FT_PV,WATCAL_1M_CAL_LIME_X3," +
               "WATCAL_1M_CAL_Z1,WATCAL_1M_CAL_Z2,WATCAL_1M_CAL_Z3,WATCAL_1M_CAL_H," +
               "(case when WATCAL_1M_B_B_SIGNAL = 1 then '启动' when WATCAL_1M_B_B_SIGNAL = 0 then '停止' else '异常' end) as WATCAL_1M_B_B_SIGNAL," +
               "WATCAL_1M_B_W,WATCAL__SPEED_SP,WATCAL__SPEED_PV,WATCAL_T_SP_W," +
               "WATCAL_1M_CAL_X_MAX,WATCAL_1M_CAL_X_MIN," +
               "(case when WATCAL_FLAG4 = 1 then '投入' when WATCAL_FLAG4 = 2 then '退出' else '异常' end) as WATCAL_FLAG4," +
               "(case when WATCAL_FLAG3 = 1 then '下发完成' when WATCAL_FLAG3 = 2 then '未下发' else '异常' end) as WATCAL_FLAG3" +
               " from MC_WATERCAL_RESULT where TIMESTAMP <= '" + _end_time + "' and TIMESTAMP >='" + _begin_time + "' order by TIMESTAMP desc";
                DataTable table = dBSQL.GetCommand(sql);
                if (table.Rows.Count > 0 && table != null)
                {
                    dataGridView1.DataSource = table;
                }
                else
                {
                    string mistake = "加水页面调整记录查询失败" + sql;
                    vLog.writelog(mistake, -1);
                }
            }
            catch (Exception ee)
            {
                string mistake = "text_real：加水页面调整数据弹出框查询失败" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
        }
    }
}
