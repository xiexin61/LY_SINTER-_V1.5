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

namespace LY_SINTER.Popover.Analysis
{
    public partial class Frm_Production_state_Mon : Form
    {
        public static bool isopen = false;
        public vLog _vLog { get; set; }
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public Frm_Production_state_Mon()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Analysis\\Frm_Production_state_Mon\\");
            time_begin_end();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            show();
            d1_col();
        }
        public void d1_col()
        {
            //添加列说明
            this.d2.AddSpanHeader(17, 7, "BTP温度");
            this.d2.AddSpanHeader(24, 3, "大烟道温度");
            this.d2.AddSpanHeader(27, 3, "BTP位置");
            this.d2.AddSpanHeader(30, 3, "大烟道负压");
            this.d2.AddSpanHeader(37, 3, "主排频率");
            this.d2.AddSpanHeader(40, 3, "主排电流");
        }
        public void show()
        {
            string sql = "select top (20) " +
                "TIMESTAMP" +
                ",P_CAL,HHL_W,SF_BALA_PB," +
                "GF_BALA_PB,GF_BALA_W,M_Y,M_P," +
                "MOI_1,MOI_2,WTR_Q_HOUR,PH_STP_NUM," +
                "BED_THICK_AD_NUM,BED_THICK,M_SPEED," +
                "M_C_SPEED,IG_T,BTP_D,BTP_X,BTP_AV," +
                "TEMP_ZONE,BTP_ZONE_RATE,ZONE_RATE_300," +
                "ZONE_RATE_450,MA_T_D,MA_T_X,MA_T_AD," +
                "BTP_POS_D,BTP_POS_X,BTP_POS_AV,MA_P_D," +
                "MA_P_X,MA_P_AV,V_BED_SPEED,SINTER_END_T," +
                "COLD_END_T,COLD_FAN_NUM,MA_HZ_D,MA_HZ_X," +
                "MA_HZ_AV,MA_CURT_D,MA_CURT_X,MA_CURT_AV," +
                "M_FAN_CNSP_E,FUEL_CNSP_PER,GAS_CNSP_PER," +
                "M_ST_NUM,M_ST_TIME " +
                "from MC_NUMCAL_INTERFACE_10_MONTH  order by TIMESTAMP desc";
            DataTable data_MC_NUMCAL_INTERFACE_10_MONTH = dBSQL.GetCommand(sql);
            d2.DataSource = data_MC_NUMCAL_INTERFACE_10_MONTH;
            //修改时间说明

        }
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddDays(-7);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }
        /// <summary>
        /// 实时按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            show();
        }
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {

            //开始时间
            string time_begin = textBox_begin.Text.ToString();
            //结束时间
            string time_end = textBox_end.Text.ToString();
            string sql_MC_NUMCAL_INTERFACE_10_MONTH = "select  " +
                "TIMESTAMP," +
                "P_CAL,HHL_W,SF_BALA_PB," +
                "GF_BALA_PB,GF_BALA_W,M_Y,M_P," +
                "MOI_1,MOI_2,WTR_Q_HOUR,PH_STP_NUM," +
                "BED_THICK_AD_NUM,BED_THICK,M_SPEED," +
                "M_C_SPEED,IG_T,BTP_D,BTP_X,BTP_AV," +
                "TEMP_ZONE,BTP_ZONE_RATE,ZONE_RATE_300," +
                "ZONE_RATE_450,MA_T_D,MA_T_X,MA_T_AD," +
                "BTP_POS_D,BTP_POS_X,BTP_POS_AV,MA_P_D," +
                "MA_P_X,MA_P_AV,V_BED_SPEED,SINTER_END_T," +
                "COLD_END_T,COLD_FAN_NUM,MA_HZ_D,MA_HZ_X," +
                "MA_HZ_AV,MA_CURT_D,MA_CURT_X,MA_CURT_AV," +
                "M_FAN_CNSP_E,FUEL_CNSP_PER,GAS_CNSP_PER," +
                "M_ST_NUM,M_ST_TIME from MC_NUMCAL_INTERFACE_10_MONTH  where TIMESTAMP >= '" + time_begin + "' and TIMESTAMP <='" + time_end + "'" +
                " order by TIMESTAMP desc";
            DataTable data_MC_NUMCAL_INTERFACE_10_MONTH = dBSQL.GetCommand(sql_MC_NUMCAL_INTERFACE_10_MONTH);
            d2.DataSource = data_MC_NUMCAL_INTERFACE_10_MONTH;

        }
        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton7_Click(object sender, EventArgs e)
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

        private void d2_Scroll(object sender, ScrollEventArgs e)
        {
            d2.ReDrawHead();
        }
    }
}
