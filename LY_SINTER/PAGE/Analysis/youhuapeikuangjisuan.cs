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
using System.IO;
using LY_SINTER.Popover.Analysis;

namespace LY_SINTER.PAGE.Analysis
{
    public partial class youhuapeikuangjisuan : UserControl
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public youhuapeikuangjisuan()
        {
            InitializeComponent();
            comboBoxData();
            getData();
        }
        //表单编码
        public void comboBoxData()
        {
            string sql = "select BATCH_NUM from MC_ORECAL_SIN_ANA_RESULT";
            DataTable table = dBSQL.GetCommand(sql);
            this.comboBox1.DataSource = table;
            this.comboBox1.DisplayMember = "BATCH_NUM";
            this.comboBox1.ValueMember = "BATCH_NUM";
        }
        //界面表格初始化查询
        public void getData()
        {
            string name = comboBox1.SelectedValue.ToString();
            string sql1 = "select  ROW_NUMBER() over(order by TIMESTAMP) as RowNum,MAT_NAME,MAT_CLASS,UNIT_PRICE,BILL_UPPER,BILL_LOWER,C_TFE,C_FEO,C_SIO2,C_CAO,C_MGO,C_AL2O3," +
                "C_S,C_P,C_LOT,C_H2O,C_AS,C_PB,C_ZN,C_CU,C_K2O,C_NA2O from MC_ORECAL_MAT_ANA_RECORD where BATCH_NUM = "+ name + "";
            DataTable table = dBSQL.GetCommand(sql1);
            d1.DataSource = table;
            string sql2 = "select  ROW_NUMBER() over(order by TIMESTAMP) as RowNum,MAT_NAME,MAT_BILL_DRY,MAT_W_DRY,ORE_MAT_BILL_DRY,P_H2O,MAT_BILL_WET,MAT_W_WET,ORE_MAT_BILL_WET,UNIT_PRICE,UNIT_CON_PRICE,MAT_WET_ACT from MC_ORECAL_ORE_PROJECT_RESULT where BATCH_NUM = " + name + ";";
            DataTable table2 = dBSQL.GetCommand(sql2);
            d2.DataSource = table2;
            string sql3 = "select top(1) C_TFE_UPPER,C_TFE_LOWER,C_FEO_UPPER,C_FEO_LOWER,C_CAO_UPPER,C_CAO_LOWER,C_SIO2_UPPER,C_SIO2_LOWER,C_AL2O3_UPPER,C_AL2O3_LOWER,C_MGO_UPPER,C_MGO_LOWER," +
                "C_S_UPPER,C_S_LOWER,C_P_UPPER,C_P_LOWER,C_R_UPPER,C_R_LOWER,C_AS_UPPER,C_AS_LOWER,C_CU_UPPER,C_CU_LOWER,C_PB_UPPER,C_PB_LOWER,C_ZN_UPPER,C_ZN_LOWER from MC_ORECAL_SIN_ANA_RECORD where BATCH_NUM = " + name + "";
            DataTable table3 = dBSQL.GetCommand(sql3);
            while (d3.Rows.Count < 13)
            {
                d3.Rows.Add();
            }
            d3.Rows[0].Cells["MAT"].Value = "TFe";
            d3.Rows[1].Cells["MAT"].Value = "FeO";
            d3.Rows[2].Cells["MAT"].Value = "SiO2";
            d3.Rows[3].Cells["MAT"].Value = "CaO";
            d3.Rows[4].Cells["MAT"].Value = "MgO";
            d3.Rows[5].Cells["MAT"].Value = "Al2O3";
            d3.Rows[6].Cells["MAT"].Value = "S";
            d3.Rows[7].Cells["MAT"].Value = "P";
            d3.Rows[8].Cells["MAT"].Value = "R";
            d3.Rows[9].Cells["MAT"].Value = "As";
            d3.Rows[10].Cells["MAT"].Value = "Pb";
            d3.Rows[11].Cells["MAT"].Value = "Zn";
            d3.Rows[12].Cells["MAT"].Value = "Cu";
            d3.Rows[0].Cells["UPPER"].Value = table3.Rows[0]["C_TFE_UPPER"];
            d3.Rows[0].Cells["LOWER"].Value = table3.Rows[0]["C_TFE_LOWER"];
            d3.Rows[1].Cells["UPPER"].Value = table3.Rows[0]["C_FEO_UPPER"];
            d3.Rows[1].Cells["LOWER"].Value = table3.Rows[0]["C_FEO_LOWER"];
            d3.Rows[2].Cells["UPPER"].Value = table3.Rows[0]["C_CAO_UPPER"];
            d3.Rows[2].Cells["LOWER"].Value = table3.Rows[0]["C_CAO_LOWER"];
            d3.Rows[3].Cells["UPPER"].Value = table3.Rows[0]["C_SIO2_UPPER"];
            d3.Rows[3].Cells["LOWER"].Value = table3.Rows[0]["C_SIO2_UPPER"];
            d3.Rows[4].Cells["UPPER"].Value = table3.Rows[0]["C_AL2O3_UPPER"];
            d3.Rows[4].Cells["LOWER"].Value = table3.Rows[0]["C_AL2O3_LOWER"];
            d3.Rows[5].Cells["UPPER"].Value = table3.Rows[0]["C_MGO_UPPER"];
            d3.Rows[5].Cells["LOWER"].Value = table3.Rows[0]["C_MGO_LOWER"];
            d3.Rows[6].Cells["UPPER"].Value = table3.Rows[0]["C_S_UPPER"];
            d3.Rows[6].Cells["LOWER"].Value = table3.Rows[0]["C_S_LOWER"];
            d3.Rows[7].Cells["UPPER"].Value = table3.Rows[0]["C_P_UPPER"];
            d3.Rows[7].Cells["LOWER"].Value = table3.Rows[0]["C_P_LOWER"];
            d3.Rows[8].Cells["UPPER"].Value = table3.Rows[0]["C_R_UPPER"];
            d3.Rows[8].Cells["LOWER"].Value = table3.Rows[0]["C_R_LOWER"];
            d3.Rows[9].Cells["UPPER"].Value = table3.Rows[0]["C_AS_UPPER"];
            d3.Rows[9].Cells["LOWER"].Value = table3.Rows[0]["C_AS_LOWER"];
            d3.Rows[10].Cells["UPPER"].Value = table3.Rows[0]["C_PB_UPPER"];
            d3.Rows[10].Cells["LOWER"].Value = table3.Rows[0]["C_PB_LOWER"];
            d3.Rows[11].Cells["UPPER"].Value = table3.Rows[0]["C_ZN_UPPER"];
            d3.Rows[11].Cells["LOWER"].Value = table3.Rows[0]["C_ZN_LOWER"];
            d3.Rows[12].Cells["UPPER"].Value = table3.Rows[0]["C_CU_UPPER"];
            d3.Rows[12].Cells["LOWER"].Value = table3.Rows[0]["C_CU_LOWER"];
            string sql4 = "select top(2) C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_R,C_AS,C_CU,C_PB,C_ZN,C_K2O from MC_ORECAL_SIN_ANA_RESULT  where BATCH_NUM = " + name + "";
            DataTable table4 = dBSQL.GetCommand(sql4);
            d4.DataSource = table4;
            d4.Rows[0].Cells["NUM"].Value = "上批";
            d4.Rows[1].Cells["NUM"].Value = "本批";
            string sql5 = "select top(1) SINTER_PRICE,TOTAL_MAT_WET from MC_ORECAL_SIN_ANA_RESULT  where BATCH_NUM = " + name + "";
            DataTable table5 = dBSQL.GetCommand(sql5);
            //textBox.Text = table5.Rows[0]["TOTAL_MAT_WET"].ToString();
            //textBox1.Text = table5.Rows[0]["SINTER_PRICE"].ToString();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                for(int i = 0; i < d3.Rows.Count; i++)
                {
                    d3.Rows[i].Cells["select"].Value = true;
                }
            }
        }
        //添加
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Frm_JHPK_insert form_display = new Frm_JHPK_insert();
            if (Frm_JHPK_insert.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }
        //修改
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            Frm_JHPK_update form_display = new Frm_JHPK_update();
            if (Frm_JHPK_update.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }  
        }
        //删除
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("确定删除吗？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int a = d1.CurrentRow.Index;
                    string data = d1.Rows[a].Cells["BATCH_NUM"].Value.ToString();
                    string name = d1.Rows[a].Cells["MAT_DESC"].Value.ToString();
                    string delSql = "delete from M_BF_MATERIAL_ANALYSIS where BATCH_NUM = '" + data + "' and L2_CODE in(select L2_CODE from M_MATERIAL_COOD where MAT_DESC='" + name + "')";
                    int k = dBSQL.CommandExecuteNonQuery(delSql);
                    if (k > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("删除失败");
                    }
                }
                else { return; }
            }
            catch (Exception ee)
            {

            }
        }
        //导出
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
        //刷新
        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
                for (int i = 0; i < d3.Rows.Count; i++)
                {
                    d3.Rows[i].Cells["select"].Value = false;
                }
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            getData();
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
