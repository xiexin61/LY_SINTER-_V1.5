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
    public partial class Frm_MIX_Element : Form
    {
        public static bool isopen = false;
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public Frm_MIX_Element()
        {

            InitializeComponent();
            TIME_VALUES();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            combox_date();
            tableLayoutPanel_1();
        }
        /// <summary>
        /// 开始时间-结束时间赋值
        /// </summary>
        public void TIME_VALUES()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddDays(-7);
            textBox_begin.Text = time_begin.ToString(); ;
            textBox_end.Text = time_end.ToString(); ;

        }
        /// <summary>
        /// 下拉框
        /// </summary>
        public void combox_date()
        {
            DataTable data_combox = new DataTable();
            data_combox.Columns.Add("name");
            data_combox.Columns.Add("Value");

            DataRow data_1 = data_combox.NewRow();
            data_1["name"] = "成分设定值";
            data_1["Value"] = "1";
            data_combox.Rows.Add(data_1);

            DataRow data_2 = data_combox.NewRow();
            data_2["name"] = "成分实际值";
            data_2["Value"] = "2";
            data_combox.Rows.Add(data_2);

            DataRow data_3 = data_combox.NewRow();
            data_3["name"] = "成分检测值";
            data_3["Value"] = "3";
            data_combox.Rows.Add(data_3);

            this.comboBox1.DataSource = data_combox;
            this.comboBox1.DisplayMember = "name";
            this.comboBox1.ValueMember = "Value";

            this.comboBox1.SelectedIndex = 0;




        }
        private void tableLayoutPanel_1()
        {


            int fieldName = comboBox1.SelectedIndex;
            DataTable data = new DataTable();
            //成分设定值
            if (fieldName == 0)
            {
                string sql = "select top (10) ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID, TIMESTAMP,SINCAL_SIN_SP_TFE AS TFE,SINCAL_SIN_SP_FEO AS FEO,SINCAL_SIN_SP_CAO AS CAO,SINCAL_SIN_SP_SIO2 AS SIO2,SINCAL_SIN_SP_AL2O3 AS AL2O3 ,SINCAL_SIN_SP_MGO AS MGO ,SINCAL_MIX_SP_C as C,SINCAL_SIN_SP_R AS R,SINCAL_SIN_SP_S AS S,SINCAL_SIN_SP_P AS P ,SINCAL_SIN_SP_MN AS MN,SINCAL_SIN_SP_K2O AS K2O,SINCAL_SIN_SP_NA2O AS NA2O ,SINCAL_SIN_SP_AS AS a_s ,SINCAL_SIN_SP_CU AS CU,SINCAL_SIN_SP_PB AS PB,SINCAL_SIN_SP_ZN AS ZN,SINCAL_SIN_SP_K AS K,SINCAL_SIN_SP_TIO2 as TIO2  from MC_MIXCAL_RESULT_1MIN order by TIMESTAMP desc ";
                data = dBSQL.GetCommand(sql);
            }
            //成分实际值
            else if (fieldName == 1)
            {
                string sql = "select top (10) ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID, TIMESTAMP, SINCAL_SIN_PV_TFE AS TFE,SINCAL_SIN_PV_FEO AS FEO,SINCAL_SIN_PV_CAO AS CAO,SINCAL_SIN_PV_SIO2 AS SIO2,SINCAL_SIN_PV_AL2O3 AS AL2O3 ,SINCAL_SIN_PV_MGO AS MGO ,SINCAL_MIX_PV_C as C,SINCAL_SIN_PV_R AS R,SINCAL_SIN_PV_S AS S,SINCAL_SIN_PV_P AS P ,SINCAL_SIN_PV_MN AS MN,SINCAL_SIN_PV_K2O AS K2O,SINCAL_SIN_PV_NA2O AS NA2O ,SINCAL_SIN_PV_AS AS a_s ,SINCAL_SIN_PV_CU AS CU,SINCAL_SIN_PV_PB AS PB,SINCAL_SIN_PV_ZN AS ZN,SINCAL_SIN_PV_K AS K,SINCAL_SIN_PV_TIO2 as TIO2  from MC_MIXCAL_RESULT_1MIN order by TIMESTAMP desc";
                data = dBSQL.GetCommand(sql);
            }
            //成分检测值
            else if (fieldName == 2)
            {
                string sql = "select top (10) ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID, TIMESTAMP, C_TFE AS TFE,C_FEO AS FEO,C_CAO AS CAO, C_SIO2 as SIO2, C_AL2O3 as AL2O3,C_MGO as MGO, NULL AS C,C_R as R,C_S as S,C_P2O5 AS P,C_MNO AS MN,C_K2O AS K2O,C_NA2O AS NA2O,C_AS AS a_s ,C_CU AS CU ,C_PB AS PB , C_ZN AS ZN, C_K AS K,C_TIO2 AS TIO2  from M_SINTER_ANALYSIS order by TIMESTAMP desc";
                data = dBSQL.GetCommand(sql);
            }
            else
            {
                return;
            }

            if (data.Rows.Count > 0)
            {

                this.dataGridView1.DataSource = data;
            }




        }

        /// <summary>
        ///查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            //开始时间
            DateTime begin = Convert.ToDateTime(textBox_begin.Text);
            //结束时间
            DateTime end = Convert.ToDateTime(textBox_end.Text);
            //下拉框索引号
            int fieldName = comboBox1.SelectedIndex;
            //使用的date
            DataTable data = new DataTable();
            //成分设定值
            if (fieldName == 0)
            {
                string sql = "select ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID, TIMESTAMP,  SINCAL_SIN_SP_TFE AS TFE,SINCAL_SIN_SP_FEO AS FEO,SINCAL_SIN_SP_CAO AS CAO,SINCAL_SIN_SP_SIO2 AS SIO2,SINCAL_SIN_SP_AL2O3 AS AL2O3 ,SINCAL_SIN_SP_MGO AS MGO ,SINCAL_MIX_SP_C as C,SINCAL_SIN_SP_R AS R,SINCAL_SIN_SP_S AS S,SINCAL_SIN_SP_P AS P ,SINCAL_SIN_SP_MN AS MN,SINCAL_SIN_SP_K2O AS K2O,SINCAL_SIN_SP_NA2O AS NA2O ,SINCAL_SIN_SP_AS AS a_s ,SINCAL_SIN_SP_CU AS CU,SINCAL_SIN_SP_PB AS PB,SINCAL_SIN_SP_ZN AS ZN,SINCAL_SIN_SP_K AS K,SINCAL_SIN_SP_TIO2 as TIO2  from MC_MIXCAL_RESULT_1MIN where TIMESTAMP >='" + begin + "' and TIMESTAMP <= '" + end + "' order by TIMESTAMP desc ";
                data = dBSQL.GetCommand(sql);
            }
            //成分实际值
            else if (fieldName == 1)
            {
                string sql = "select ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID, TIMESTAMP,  SINCAL_SIN_PV_TFE AS TFE,SINCAL_SIN_PV_FEO AS FEO,SINCAL_SIN_PV_CAO AS CAO,SINCAL_SIN_PV_SIO2 AS SIO2,SINCAL_SIN_PV_AL2O3 AS AL2O3 ,SINCAL_SIN_PV_MGO AS MGO ,SINCAL_MIX_PV_C as C,SINCAL_SIN_PV_R AS R,SINCAL_SIN_PV_S AS S,SINCAL_SIN_PV_P AS P ,SINCAL_SIN_PV_MN AS MN,SINCAL_SIN_PV_K2O AS K2O,SINCAL_SIN_PV_NA2O AS NA2O ,SINCAL_SIN_PV_AS AS a_s ,SINCAL_SIN_PV_CU AS CU,SINCAL_SIN_PV_PB AS PB,SINCAL_SIN_PV_ZN AS ZN,SINCAL_SIN_PV_K AS K,SINCAL_SIN_PV_TIO2 as TIO2  from MC_MIXCAL_RESULT_1MIN where TIMESTAMP >='" + begin + "' and TIMESTAMP <= '" + end + "' order by TIMESTAMP desc";
                data = dBSQL.GetCommand(sql);
            }
            //成分检测值
            else if (fieldName == 2)
            {
                string sql = "select ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID, TIMESTAMP,  C_TFE AS TFE,C_FEO AS FEO,C_CAO AS CAO, C_SIO2 as SIO2, C_AL2O3 as AL2O3,C_MGO as MGO, NULL AS C,C_R as R,C_S as S,C_P2O5 AS P,C_MNO AS MN,C_K2O AS K2O,C_NA2O AS NA2O,C_AS AS a_s ,C_CU AS CU ,C_PB AS PB , C_ZN AS ZN, C_K AS K,C_TIO2 AS TIO2  from M_SINTER_ANALYSIS where TIMESTAMP >='" + begin + "' and TIMESTAMP <= '" + end + "' order by TIMESTAMP desc";
                data = dBSQL.GetCommand(sql);
            }
            else
            {
                return;
            }

            if (data.Rows.Count > 0)
            {
                //DataTable d2 = RowsToCol(data);
                this.dataGridView1.DataSource = data;
            }
            else
            {
               // MessageBox.Show("此时间段无数据");
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            TIME_VALUES();
            tableLayoutPanel_1();
        }
        /// <summary>
        /// 导出
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

    }
}
