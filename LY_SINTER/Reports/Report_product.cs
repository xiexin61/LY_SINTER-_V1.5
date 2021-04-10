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
using System.Threading;
using System.IO;

namespace LY_SINTER.Reports
{
    public partial class Report_product : UserControl
    {
        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        //private int i = 0;

        public Report_product()
        {
            InitializeComponent();
            this.rowMergeView1.AddSpanHeader(1, 1, "台车速度");
            this.rowMergeView1.AddSpanHeader(2, 1, "布料厚度");
            this.rowMergeView1.AddSpanHeader(3, 1, "点火温度");
            this.rowMergeView1.AddSpanHeader(4, 2, "主管负压Kpa");
            this.rowMergeView1.AddSpanHeader(6, 2, "主管废气温度℃");
            this.rowMergeView1.AddSpanHeader(8, 1, "漏风率");
            this.rowMergeView1.AddSpanHeader(9, 1, "煤气压力");
            this.rowMergeView1.AddSpanHeader(10, 1, "煤气流量");
            this.rowMergeView1.AddSpanHeader(11, 1, "余热产气");
            this.rowMergeView1.AddSpanHeader(12, 7, "1#主管风箱温度℃");
            this.rowMergeView1.AddSpanHeader(19, 7, "2#主管风箱温度℃");
            this.rowMergeView1.AddSpanHeader(26, 1, "混合料水分");
            this.rowMergeView1.AddSpanHeader(27, 1, "混合料温");
            this.rowMergeView1.AddSpanHeader(28, 1, "热水水温");
            this.rowMergeView1.AddSpanHeader(29, 1, "混合料粒度");
            this.rowMergeView1.AddSpanHeader(30, 9, "混合料水分");
            getData();
            getRowMergeView2Data();
            getRowMergeView3Data();
            getRowMergeView4Data();
        }

        /// <summary>
        /// 获取报表数据
        /// </summary>
        public void getData()
        {
            Dictionary<int, string> time = new Dictionary<int, string>();
            time.Add(0, "8");
            time.Add(1, "9");
            time.Add(2, "10");
            time.Add(3, "11");
            time.Add(4, "12");
            time.Add(5, "13");
            time.Add(6, "14");
            time.Add(7, "15");
            time.Add(8, "16");
            time.Add(9, "17");
            time.Add(10, "18");
            time.Add(11, "19");
            time.Add(12, "班平均");
            time.Add(13, "20");
            time.Add(14, "21");
            time.Add(15, "22");
            time.Add(16, "23");
            time.Add(17, "24");
            time.Add(18, "1");
            time.Add(19, "2");
            time.Add(20, "3");
            time.Add(21, "4");
            time.Add(22, "5");
            time.Add(23, "6");
            time.Add(24, "7");
            time.Add(25, "班平均");
            time.Add(26, "日平均");
            for (int i = 0; i < rowMergeView1.Columns.Count; i++)
            {
                rowMergeView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            for (int i = 0; i < 27; i++)
            {
                DataGridViewRow row11 = new DataGridViewRow();
                rowMergeView1.Rows.Add(row11);
            }

            #region 时间列赋值

            foreach (var a in time)
            {
                this.rowMergeView1.Rows[a.Key].Cells["timestamp"].Value = a.Value;
            }

            #endregion 时间列赋值

            string sql = "", sql1 = "";
            for (int i = 0; i < rowMergeView1.Rows.Count; i++)
            {
                /*if (getTime(hour) > DateTime.Now)
                {
                    break;
                }*/
                if (i == 12)//班平均0-11行平均
                {
                    List<double> list12 = new List<double>();
                    double num = 0;
                    for (int k = 0; k < 12; k++)
                    {
                        num = num + Convert.ToDouble(this.rowMergeView1.Rows[k].Cells["F_PLC_SIN_SPEED_PV"].Value.ToString() == "" ? 0 : this.rowMergeView1.Rows[k].Cells["F_PLC_SIN_SPEED_PV"].Value);
                    }
                    this.rowMergeView1.Rows[i].Cells["F_PLC_SIN_SPEED_PV"].Value = num / 12;
                }
                else if (i == 25)//班平均13-24行平均
                {
                }
                else if (i == 26)//0-11和13-24行平均
                {
                }
                else if (i == 17)//24点是0点
                {
                }
                else
                {
                    string h = this.rowMergeView1.Rows[i].Cells["timestamp"].Value.ToString();
                    int hour = int.Parse(h);
                    DateTime date = getTime(hour);
                    sql = " select top(1) F_PLC_SIN_SPEED_PV,F_PLC_THICK_PV,I_PLC_IG_01_TE,I_PLC_IG_GAS_PT,I_PLC_IG_GAS_PV,I_PLC_IG_01_TE from C_MFI_PLC_1MIN where timestamp >'" + date + "' order by timestamp";
                    sql1 = "select top(1) SIN_PLC_MA_SB_1_FLUE_PT,SIN_PLC_MA_SB_2_FLUE_PT,SIN_PLC_MA_SB_1_FLUE_TE,SIN_PLC_MA_SB_2_FLUE_TE,SIN_PLC_B01_TE_L_1," +
                "SIN_PLC_B02_TE_L_1,SIN_PLC_B03_TE_L_1,SIN_PLC_B19_TE_L_1,SIN_PLC_B20_TE_L_1,SIN_PLC_B21_TE_L_1,SIN_PLC_B22_TE_L_1," +
                "SIN_PLC_B01_TE_L_2,SIN_PLC_B02_TE_L_2,SIN_PLC_B03_TE_L_2,SIN_PLC_B19_TE_L_4,SIN_PLC_B20_TE_L_4,SIN_PLC_B21_TE_L_4,SIN_PLC_B22_TE_L_4 from C_SIN_PLC_1MIN where timestamp >'" + date + "' order by timestamp";
                    DataTable table = dBSQL.GetCommand(sql);
                    this.rowMergeView1.Rows[i].Cells["F_PLC_SIN_SPEED_PV"].Value = (Convert.ToDouble(table.Rows[0]["F_PLC_SIN_SPEED_PV"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["F_PLC_THICK_PV"].Value = (Convert.ToDouble(table.Rows[0]["F_PLC_THICK_PV"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["I_PLC_IG_GAS_PT"].Value = (Convert.ToDouble(table.Rows[0]["I_PLC_IG_GAS_PT"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["I_PLC_IG_GAS_PV"].Value = (Convert.ToDouble(table.Rows[0]["I_PLC_IG_GAS_PV"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["I_PLC_IG_01_TE"].Value = (Convert.ToDouble(table.Rows[0]["I_PLC_IG_01_TE"])).ToString("0.00");
                    DataTable table1 = dBSQL.GetCommand(sql1);
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_MA_SB_1_FLUE_PT"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_MA_SB_1_FLUE_PT"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_MA_SB_2_FLUE_PT"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_MA_SB_2_FLUE_PT"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_MA_SB_1_FLUE_TE"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_MA_SB_1_FLUE_TE"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_MA_SB_2_FLUE_TE"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_MA_SB_2_FLUE_TE"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B01_TE_L_1"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B01_TE_L_1"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B02_TE_L_1"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B02_TE_L_1"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B03_TE_L_1"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B03_TE_L_1"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B19_TE_L_1"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B19_TE_L_1"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B20_TE_L_1"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B20_TE_L_1"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B21_TE_L_1"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B21_TE_L_1"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B22_TE_L_1"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B22_TE_L_1"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B01_TE_L_2"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B01_TE_L_2"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B02_TE_L_2"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B02_TE_L_2"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B03_TE_L_2"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B03_TE_L_2"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B19_TE_L_4"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B19_TE_L_4"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B20_TE_L_4"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B20_TE_L_4"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B21_TE_L_4"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B21_TE_L_4"])).ToString("0.00");
                    this.rowMergeView1.Rows[i].Cells["SIN_PLC_B22_TE_L_4"].Value = (Convert.ToDouble(table1.Rows[0]["SIN_PLC_B22_TE_L_4"])).ToString("0.00");
                }
            }
        }

        /// <summary>
        /// 获取rowMergeView2数据
        /// </summary>
        public void getRowMergeView2Data()
        {
            for (int i = 0; i < 22; i++)
            {
                DataGridViewRow row11 = new DataGridViewRow();
                rowMergeView2.Rows.Add(row11);
            }
            this.rowMergeView2.Rows[0].Cells["xm"].Value = "烧结产量";
            this.rowMergeView2.Rows[1].Cells["xm"].Value = "供高炉量";
            this.rowMergeView2.Rows[2].Cells["xm"].Value = "高炉返矿";
            this.rowMergeView2.Rows[3].Cells["xm"].Value = "高炉矿比";
            this.rowMergeView2.Rows[4].Cells["xm"].Value = "铁料耗量";
            this.rowMergeView2.Rows[5].Cells["xm"].Value = "铁料消耗";
            this.rowMergeView2.Rows[6].Cells["xm"].Value = "燃料耗量";
            this.rowMergeView2.Rows[7].Cells["xm"].Value = "燃料消耗";
            this.rowMergeView2.Rows[8].Cells["xm"].Value = "石灰石耗量";
            this.rowMergeView2.Rows[9].Cells["xm"].Value = "石灰石消耗";
            this.rowMergeView2.Rows[10].Cells["xm"].Value = "白云石耗量";
            this.rowMergeView2.Rows[11].Cells["xm"].Value = "白云石消耗";
            this.rowMergeView2.Rows[12].Cells["xm"].Value = "溶剂消耗";
            this.rowMergeView2.Rows[13].Cells["xm"].Value = "生石灰耗量";
            this.rowMergeView2.Rows[14].Cells["xm"].Value = "生石灰消耗";
            this.rowMergeView2.Rows[15].Cells["xm"].Value = "脱硫白灰耗量";
            this.rowMergeView2.Rows[16].Cells["xm"].Value = "脱硫白灰消耗";
            this.rowMergeView2.Rows[17].Cells["xm"].Value = "脱硫废灰量";
            this.rowMergeView2.Rows[18].Cells["xm"].Value = "脱硫废灰消耗";
            this.rowMergeView2.Rows[19].Cells["xm"].Value = "机头灰排量";
            this.rowMergeView2.Rows[20].Cells["xm"].Value = "机头灰消耗";
            this.rowMergeView2.Rows[21].Cells["xm"].Value = "外排烧结矿";
            this.rowMergeView2.Rows[0].Cells["xm2"].Value = "空压风用量";
            this.rowMergeView2.Rows[1].Cells["xm2"].Value = "空压风消耗";
            this.rowMergeView2.Rows[2].Cells["xm2"].Value = "新水用量";
            this.rowMergeView2.Rows[3].Cells["xm2"].Value = "新水消耗";
            this.rowMergeView2.Rows[4].Cells["xm2"].Value = "浓盐水用量";
            this.rowMergeView2.Rows[5].Cells["xm2"].Value = "浓盐水消耗";
            this.rowMergeView2.Rows[6].Cells["xm2"].Value = "生活水用量";
            this.rowMergeView2.Rows[7].Cells["xm2"].Value = "生活水消耗";
            this.rowMergeView2.Rows[8].Cells["xm2"].Value = "电量";
            this.rowMergeView2.Rows[9].Cells["xm2"].Value = "电耗";
            this.rowMergeView2.Rows[10].Cells["xm2"].Value = "蒸汽量";
            this.rowMergeView2.Rows[11].Cells["xm2"].Value = "蒸汽消耗";
            this.rowMergeView2.Rows[12].Cells["xm2"].Value = "氮气量";
            this.rowMergeView2.Rows[13].Cells["xm2"].Value = "氮气消耗";
            this.rowMergeView2.Rows[14].Cells["xm2"].Value = "高炉煤气耗量";
            this.rowMergeView2.Rows[15].Cells["xm2"].Value = "高炉煤气消耗";
            this.rowMergeView2.Rows[16].Cells["xm2"].Value = "焦炉煤气耗量";
            this.rowMergeView2.Rows[17].Cells["xm2"].Value = "焦炉煤气消耗";
            this.rowMergeView2.Rows[18].Cells["xm2"].Value = "氨水耗量";
            this.rowMergeView2.Rows[19].Cells["xm2"].Value = "氨水消耗";
            this.rowMergeView2.Rows[20].Cells["xm2"].Value = "余热产汽量";
            this.rowMergeView2.Rows[21].Cells["xm2"].Value = "余热产汽消耗";
        }

        /// <summary>
        /// 获取rowMergeView3数据
        /// </summary>
        public void getRowMergeView3Data()
        {
            for (int i = 0; i < rowMergeView3.Columns.Count; i++)
            {
                rowMergeView3.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            /*for (int i = 0; i < 9; i++)
            {
                DataGridViewRow row11 = new DataGridViewRow();
                rowMergeView3.Rows.Add(row11);
            }*/

            string sql = "select top(6) BATCH_NUM,SAMPLETIME,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_R,C_TIO2,C_TI,GRIT_10_16+GRIT_16_25 as GRIT_25_10,GRIT_5_10,GRIT_5 from M_SINTER_ANALYSIS  where BATCH_NUM not like 'L4%' order by TIMESTAMP desc";
            DataTable table = dBSQL.GetCommand(sql);
            table.Columns.Add("ClassName");
            table.Rows[0]["ClassName"] = "白班";
            table.Rows[1]["ClassName"] = "白班";
            table.Rows[2]["ClassName"] = "白班";
            table.Rows[3]["ClassName"] = "夜班";
            table.Rows[4]["ClassName"] = "夜班";
            table.Rows[5]["ClassName"] = "夜班";
            rowMergeView3.DataSource = table;
            table.Rows.Add();
            table.Rows[6]["ClassName"] = "平均值";
            table.Rows.Add();
            table.Rows[7]["ClassName"] = "最大值";
            table.Rows.Add();
            table.Rows[8]["ClassName"] = "最小值";
            for (int i = 2; i < table.Columns.Count - 1; i++)
            {
                List<double> list = new List<double>();
                double sum = 0;
                for (int j = 0; j < 6; j++)
                {
                    string data = table.Rows[j][i].ToString() == "" ? "0" : table.Rows[j][i].ToString();
                    sum = sum + Convert.ToDouble(data);
                    list.Add(Convert.ToDouble(data));
                }
                table.Rows[6][i] = (sum / 6).ToString("0.00");
                table.Rows[7][i] = (list.Max()).ToString("0.00");
                table.Rows[8][i] = (list.Min()).ToString("0.00");
            }
        }

        /// <summary>
        /// 获取rowMergeView4数据
        /// </summary>
        public void getRowMergeView4Data()
        {
            for (int i = 0; i < rowMergeView4.Columns.Count; i++)
            {
                rowMergeView4.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            for (int i = 0; i < 24; i++)
            {
                DataGridViewRow row11 = new DataGridViewRow();
                rowMergeView4.Rows.Add(row11);
            }
            this.rowMergeView4.Rows[0].Cells["name"].Value = "直供料";
            this.rowMergeView4.Rows[1].Cells["name"].Value = "直供料";
            this.rowMergeView4.Rows[2].Cells["name"].Value = "直供料";
            this.rowMergeView4.Rows[3].Cells["name"].Value = "焦粉";
            this.rowMergeView4.Rows[4].Cells["name"].Value = "焦粉";
            this.rowMergeView4.Rows[5].Cells["name"].Value = "焦粉";
            this.rowMergeView4.Rows[6].Cells["name"].Value = "煤粉";
            this.rowMergeView4.Rows[7].Cells["name"].Value = "煤粉";
            this.rowMergeView4.Rows[8].Cells["name"].Value = "煤粉";
            this.rowMergeView4.Rows[9].Cells["name"].Value = "石灰石";
            this.rowMergeView4.Rows[10].Cells["name"].Value = "石灰石";
            this.rowMergeView4.Rows[11].Cells["name"].Value = "石灰石";
            this.rowMergeView4.Rows[12].Cells["name"].Value = "白云石";
            this.rowMergeView4.Rows[13].Cells["name"].Value = "白云石";
            this.rowMergeView4.Rows[14].Cells["name"].Value = "白云石";
            this.rowMergeView4.Rows[15].Cells["name"].Value = "灰尘";
            this.rowMergeView4.Rows[16].Cells["name"].Value = "灰尘";
            this.rowMergeView4.Rows[17].Cells["name"].Value = "灰尘";
            this.rowMergeView4.Rows[18].Cells["name"].Value = "外购灰";
            this.rowMergeView4.Rows[19].Cells["name"].Value = "外购灰";
            this.rowMergeView4.Rows[20].Cells["name"].Value = "外购灰";
            this.rowMergeView4.Rows[21].Cells["name"].Value = "厂内灰";
            this.rowMergeView4.Rows[22].Cells["name"].Value = "厂内灰";
            this.rowMergeView4.Rows[23].Cells["name"].Value = "厂内灰";
        }

        /// <summary>
        /// 根据表格第一列的值获取查询的时间
        /// </summary>
        public DateTime getTime(int hour)
        {
            DateTime dateTime = new DateTime();
            //当天的时间
            if (hour <= 24)
            {
                DateTime time = DateTime.Now.AddDays(-1);
                dateTime = new DateTime(time.Year, time.Month, time.Day, hour, 0, 0);
            }
            else//下一天的时间
            {
                dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, 0, 0);
            }
            return dateTime;
        }

        /// <summary>
        /// 报表导出按钮
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
                    for (int i = 0; i < rowMergeView1.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            columnTitle += "\t";
                        }
                        columnTitle += rowMergeView1.Columns[i].HeaderText;
                    }
                    sw.WriteLine(columnTitle);

                    //写入列内容
                    for (int j = 0; j < rowMergeView1.Rows.Count; j++)
                    {
                        string columnValue = "";
                        for (int k = 0; k < rowMergeView1.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                columnValue += "\t";
                            }
                            if (rowMergeView1.Rows[j].Cells[k].Value == null)
                                columnValue += "";
                            else
                                columnValue += rowMergeView1.Rows[j].Cells[k].Value.ToString().Trim();
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

        public void Timer_state()
        {
        }

        public void _Clear()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {
        }
    }
}