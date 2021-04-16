using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLog;
using LY_SINTER.Popover.Parameter;
using DataBase;
using System.IO;

namespace LY_SINTER.PAGE.Parameter
{
    public partial class Ingredient_Protect : UserControl
    {
        public System.Timers.Timer _Timer1 { get; set; }
        private DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        public static Ingredient_auto _Auto;

        public vLog _vLog { get; set; }

        public Ingredient_Protect()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Parametery\\Ingredient_Protect_Page\\");
            date_inquire();
            classify();
            time();
            Button_color();
            _Timer1 = new System.Timers.Timer(10000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { _Timer1_Tick(); };
            _Timer1.Enabled = true;
            _Timer1.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
        }

        private void _Timer1_Tick()
        {
            Action invokeAction = new Action(_Timer1_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                Button_color();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            protect_add form_display = new protect_add();
            if (protect_add.isopen == false)
            {
                form_display._transfDelegate_YLBH_ADD += date_inquire;
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 初始化显示数据赋值
        /// </summary>
        public void date_inquire()
        {
            try
            {
                string sql = "select ROW_NUMBER() OVER(ORDER BY L2_CODE asc) AS ID, TIMESTAMP, ('修改') as XG, L2_CODE, L3_CODE,MAT_DESC, UNIT_PRICE,PLACE_ORIGIN,C_TFE_UP, C_TFE_LOWER, C_FEO_UP, C_FEO_LOWER, C_CAO_UP, C_CAO_LOWER, C_SIO2_UP, C_SIO2_LOWER, C_AL2O3_UP, C_AL2O3_LOWER, C_MGO_UP, C_MGO_LOWER, C_S_UP, C_S_LOWER, C_P_UP, C_P_LOWER, C_C_UP, C_C_LOWER, C_MN_UP, C_MN_LOWER, C_LOT_UP, C_LOT_LOWER, C_R_UP, C_R_LOWER, C_H2O_UP, C_H2O_LOWER, C_ASH_UP, C_ASH_LOWER, C_VOLATILES_UP, C_VOLATILES_LOWER, C_TIO2_UP, C_TIO2_LOWER, C_K2O_UP, C_K2O_LOWER, C_NA2O_UP, C_NA2O_LOWER, C_PBO_UP, C_PBO_LOWER, C_ZNO_UP, C_ZNO_LOWER, C_AS_UP, C_AS_LOWER, C_CU_UP, C_CU_LOWER, C_PB_UP, C_PB_LOWER, C_ZN_UP, C_ZN_LOWER, C_K_UP, C_K_LOWER, C_NA_UP, C_NA_LOWER, C_CR_UP, C_CR_LOWER, C_NI_UP, C_NI_LOWER, C_MNO_UP, C_MNO_LOWER from M_MATERIAL_COOD order by L2_CODE asc";
                DataTable dataTable = _dBSQL.GetCommand(sql);
                dataGridView1.DataSource = dataTable;
                Button_color();
            }
            catch (Exception ee)
            {
                string mistake = "原料保护页面初始化显示数据失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 物料归属分类
        /// </summary>
        private void classify()
        {
            try
            {
                string sql = "select M_TYPE as value,M_DESC as name,CODE_MIN,CODE_MAX from [M_MATERIAL_COOD_CONFIG] order by M_TYPE asc";
                DataTable dataTable = _dBSQL.GetCommand(sql);
                DataRow dr = dataTable.NewRow();
                dr["Name"] = "所有物料";
                dr["value"] = 9;
                dr["CODE_MIN"] = 101;
                dr["CODE_MAX"] = 800;
                dataTable.Rows.InsertAt(dr, 0);
                //dataTable.Rows.Add(dr);
                this.comboBox1.DataSource = dataTable;
                this.comboBox1.DisplayMember = "name";
                this.comboBox1.ValueMember = "value";
                this.comboBox1.SelectedIndex = 0;
            }
            catch (Exception ee)
            {
                var mistake = "原料保护页面识别物料归属分类识别失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            protect_del form_display = new protect_del();
            if (protect_del.isopen == false)
            {
                form_display._transfDelegate_YLBH_DEL += date_inquire;
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "button" && e.RowIndex >= 0)
                {
                    string _name = dataGridView1.Rows[e.RowIndex].Cells["button"].Value.ToString();
                    if (_name == "保存")
                    {
                        List<float> list = new List<float>();
                        DateTime date = DateTime.Now;
                        int L2_CODE = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString());
                        dataGridView1.Rows[e.RowIndex].Cells["button"].Value = "修改";
                        for (int x = 0; x < dataGridView1.ColumnCount; x++)
                        {
                            if (x > 4)
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[x].ReadOnly = true;
                                dataGridView1.Rows[e.RowIndex].Cells[x].Style.ForeColor = Color.Black; ;
                                // string a = dataGridView1.Rows[e.RowIndex].Cells[x].Value.ToString();
                                if (x > 7)
                                    list.Add(float.Parse(dataGridView1.Rows[e.RowIndex].Cells[x].Value.ToString()));
                            }
                        }
                        //    //物料描述修改
                        //    dataGridView1.Rows[e.RowIndex].Cells[5].ReadOnly = true;
                        //    dataGridView1.Rows[e.RowIndex].Cells[5].Style.ForeColor = Color.Black; ;
                        //dataGridView1.Rows[e.RowIndex].Cells[6].ReadOnly = true;
                        //dataGridView1.Rows[e.RowIndex].Cells[6].Style.ForeColor = Color.Black; ;
                        //dataGridView1.Rows[e.RowIndex].Cells[7].ReadOnly = true;
                        //dataGridView1.Rows[e.RowIndex].Cells[7].Style.ForeColor = Color.Black; ;
                        string WLMS = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                        string CD = dataGridView1.Rows[e.RowIndex].Cells["Column68"].Value.ToString();//产地
                        string DJ = dataGridView1.Rows[e.RowIndex].Cells["Column67"].Value.ToString() == "" ? "null" : dataGridView1.Rows[e.RowIndex].Cells["Column67"].Value.ToString();//单价

                        string sql_update = "update M_MATERIAL_COOD set C_TFE_UP = " + list[0] + ",C_TFE_LOWER = " + list[1] + "," +
                                "C_FEO_UP =" + list[2] + ",C_FEO_LOWER  =" + list[3] + ",C_CAO_UP =" + list[4] + "," +
                                "C_CAO_LOWER =" + list[5] + ",C_SIO2_UP =" + list[6] + ",C_SIO2_LOWER  =" + list[7] + "," +
                                "C_AL2O3_UP =" + list[8] + ",C_AL2O3_LOWER =" + list[9] + ",C_MGO_UP =" + list[10] + ",C_MGO_LOWER =" + list[11] + "," +
                                "C_S_UP =" + list[12] + ",C_S_LOWER = " + list[13] + ",C_P_UP = " + list[14] + "," +
                                "C_P_LOWER =" + list[15] + ",C_C_UP =" + list[16] + ",C_C_LOWER =" + list[17] + ",C_MN_UP = " + list[18] + "," +
                                "C_MN_LOWER = " + list[19] + ",C_LOT_UP = " + list[20] + ",C_LOT_LOWER = " + list[21] + ",C_R_UP = " + list[22] + "," +
                                "C_R_LOWER = " + list[23] + ",C_H2O_UP = " + list[24] + ",C_H2O_LOWER = " + list[25] + ",C_ASH_UP = " + list[26] + "," +
                                "C_ASH_LOWER = " + list[27] + ",C_VOLATILES_UP= " + list[28] + ",C_VOLATILES_LOWER= " + list[29] + ",C_TIO2_UP= " + list[30] + "," +
                                "C_TIO2_LOWER= " + list[31] + "," +
                                "C_K2O_UP= " + list[32] + ",C_K2O_LOWER= " + list[33] + ",C_NA2O_UP= " + list[34] + ",C_NA2O_LOWER= " + list[35] + "," +
                                "C_PBO_UP= " + list[36] + ",C_PBO_LOWER= " + list[37] + ",C_ZNO_UP= " + list[38] + ",C_ZNO_LOWER= " + list[39] + "," +
                                "C_AS_UP= " + list[40] + ",C_AS_LOWER= " + list[41] + "," +
                                "C_CU_UP= " + list[42] + ",C_CU_LOWER= " + list[43] + ",C_PB_UP= " + list[44] + "," +
                                "C_PB_LOWER= " + list[45] + ",C_ZN_UP= " + list[46] + ",C_ZN_LOWER= " + list[47] + "," +
                                "C_K_UP= " + list[48] + ",C_K_LOWER= " + list[49] + "," +
                                "C_NA_UP= " + list[50] + ",C_NA_LOWER= " + list[51] + ",C_CR_UP= " + list[52] + "," +
                                "C_CR_LOWER= " + list[53] + ",C_NI_UP= " + list[54] + ",C_NI_LOWER= " + list[55] + "," +
                                "C_MNO_UP= " + list[56] + ",C_MNO_LOWER= " + list[57] + " ,TIMESTAMP = getdate()    where L2_CODE = " + L2_CODE + "";
                        string SQL_UPDATE_WLMS = "UPDATE M_MATERIAL_COOD set MAT_DESC = '" + WLMS + "',UNIT_PRICE = " + DJ + ",PLACE_ORIGIN = '" + CD + "' where L2_CODE = " + L2_CODE + "";

                        //20201110 添加修改M_IRON_MATERIAL_CLASS表物料描述
                        var sql = "update M_IRON_MATERIAL_CLASS set MAT_DESC = '" + WLMS + "'  where L2_CODE = " + L2_CODE + "";
                        //20201117 添加修改MC_NUMCAL_INTERFACE_1表物料描述
                        var sq2 = "update MC_NUMCAL_INTERFACE_1 set MAT_DESC = '" + WLMS + "'  where L2_CODE = " + L2_CODE + "";
                        //20201117 添加修改MC_NUMCAL_INTERFACE_1_PRE表物料描述
                        var sq3 = "update MC_NUMCAL_INTERFACE_1_PRE set MAT_DESC = '" + WLMS + "'  where L2_CODE = " + L2_CODE + "";
                        //20201117 添加修改MC_NUMCAL_INTERFACE_2表物料描述
                        var sq4 = "update MC_NUMCAL_INTERFACE_2 set MAT_DESC = '" + WLMS + "'  where L2_CODE = " + L2_CODE + "";
                        //20201117 添加修改MC_NUMCAL_INTERFACE_4表物料描述
                        var sq5 = "update MC_NUMCAL_INTERFACE_4 set MAT_DESC = '" + WLMS + "'  where L2_CODE = " + L2_CODE + "";
                        try
                        {
                            _dBSQL.CommandExecuteNonQuery(sql_update);
                            _dBSQL.CommandExecuteNonQuery(SQL_UPDATE_WLMS);
                            //_dBSQL.CommandExecuteNonQuery(sql);
                            //_dBSQL.CommandExecuteNonQuery(sq2);
                            //_dBSQL.CommandExecuteNonQuery(sq3);
                            //_dBSQL.CommandExecuteNonQuery(sq4);
                            //_dBSQL.CommandExecuteNonQuery(sq5);
                            MessageBox.Show("操作成功");
                        }
                        catch
                        {
                        }
                    }
                    else if (_name == "修改")
                    {
                        dataGridView1.Rows[e.RowIndex].Cells["button"].Value = "保存";
                        for (int x = 0; x < dataGridView1.ColumnCount; x++)
                        {
                            if (x > 4)
                            {
                                dataGridView1.Rows[e.RowIndex].Cells[x].ReadOnly = false;
                                dataGridView1.Rows[e.RowIndex].Cells[x].Style.ForeColor = Color.Red; ;
                            }
                        }
                        //dataGridView1.Rows[e.RowIndex].Cells[5].ReadOnly = false;
                        //dataGridView1.Rows[e.RowIndex].Cells[5].Style.ForeColor = Color.Red; ;
                    }
                }
            }
            catch (Exception ee)
            {
                var mistake = "原料保护页面修改按钮失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                string WLGS_NAME = comboBox1.Text;
                if (WLGS_NAME == "所有物料")
                {
                    date_inquire();
                }
                else
                {
                    string sql_1 = "select M_TYPE as value,M_DESC as name,CODE_MIN,CODE_MAX from [M_MATERIAL_COOD_CONFIG] where M_DESC = '" + WLGS_NAME + "' order by M_TYPE asc";
                    DataTable dataTable = _dBSQL.GetCommand(sql_1);
                    //原料最小值
                    int raw_min = int.Parse(dataTable.Rows[0]["CODE_MIN"].ToString());
                    //原料最大值
                    int raw_max = int.Parse(dataTable.Rows[0]["CODE_MAX"].ToString());
                    string sql_2 = "select ROW_NUMBER() OVER(ORDER BY L2_CODE asc) AS ID, TIMESTAMP, ('修改') as XG, L2_CODE, L3_CODE,MAT_DESC, UNIT_PRICE,PLACE_ORIGIN,C_TFE_UP, C_TFE_LOWER, C_FEO_UP, C_FEO_LOWER, C_CAO_UP, C_CAO_LOWER, C_SIO2_UP, C_SIO2_LOWER, C_AL2O3_UP, C_AL2O3_LOWER, C_MGO_UP, C_MGO_LOWER, C_S_UP, C_S_LOWER, C_P_UP, C_P_LOWER, C_C_UP, C_C_LOWER, C_MN_UP, C_MN_LOWER, C_LOT_UP, C_LOT_LOWER, C_R_UP, C_R_LOWER, C_H2O_UP, C_H2O_LOWER, C_ASH_UP, C_ASH_LOWER, C_VOLATILES_UP, C_VOLATILES_LOWER, C_TIO2_UP, C_TIO2_LOWER, C_K2O_UP, C_K2O_LOWER, C_NA2O_UP, C_NA2O_LOWER, C_PBO_UP, C_PBO_LOWER, C_ZNO_UP, C_ZNO_LOWER, C_AS_UP, C_AS_LOWER, C_CU_UP, C_CU_LOWER, C_PB_UP, C_PB_LOWER, C_ZN_UP, C_ZN_LOWER, C_K_UP, C_K_LOWER, C_NA_UP, C_NA_LOWER, C_CR_UP, C_CR_LOWER, C_NI_UP, C_NI_LOWER, C_MNO_UP, C_MNO_LOWER from M_MATERIAL_COOD where L2_CODE <=" + raw_max + " and L2_CODE >= " + raw_min + " order by L2_CODE asc";
                    DataTable dataTable_2 = _dBSQL.GetCommand(sql_2);
                    // dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dataTable_2;
                }
            }
            catch (Exception ee)
            {
                var mistake = "原料保护页面查询历史失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
        }

        private void simpleButton6_Click(object sender, EventArgs e)
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
        /// 最新调整时间
        /// </summary>
        public void time()
        {
            try
            {
                string sql_time = "select TIMESTAMP from M_MATERIAL_COOD where TIMESTAMP = (select max(TIMESTAMP) from M_MATERIAL_COOD)";
                DataTable data_time = _dBSQL.GetCommand(sql_time);
                this.label2.Text = "最新调整时间:" + data_time.Rows[0][0].ToString();
            }
            catch (Exception ee)
            {
                var mistake = "原料保护页面最新调整时间查询失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 未处理新料弹出框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Button_untreated form_display = new Button_untreated();

            if (Button_untreated.isopen == false)
            {
                form_display._transfDelegate_YLBH_WCL_BUTTON += date_inquire;
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 周期查询原料新成分弹出框
        /// _flag =1 :通过判断M_MATERIAL_ANALYSIS表数据查询新成分
        /// _flag = 2：通过
        /// </summary>
        public void Pop_show(int _flag)
        {
            try
            {
                if (_flag == 1)
                {
                    #region 原料保护弹出框

                    string sql_M_MATERIAL_ANALYSIS_FLAG = "select REOPTTIME,L3_CODE from M_MATERIAL_ANALYSIS where FLAG = 1";
                    DataTable data_M_MATERIAL_ANALYSIS_FLAG = _dBSQL.GetCommand(sql_M_MATERIAL_ANALYSIS_FLAG);
                    if (data_M_MATERIAL_ANALYSIS_FLAG.Rows.Count > 0)
                    {
                        if (_Auto == null || _Auto.IsDisposed)
                        {
                            _Auto = new Ingredient_auto();
                            _Auto.ShowDialog();
                        }
                        else
                        {
                            _Auto.Activate();
                        }
                    }

                    #endregion 原料保护弹出框
                }
                else if (_flag == 2)
                {
                }
            }
            catch (Exception ee)
            {
                var mistake = "Pop_show方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }

        /// <summary>
        /// 定时刷新，未处理新原料按钮颜色
        /// </summary>
        public void Button_color()
        {
            try
            {
                string sql = "select  * from M_MATERIAL_COOD_NEW_RECORD WHERE flag = 1";
                DataTable dataTable = _dBSQL.GetCommand(sql);
                if (dataTable.Rows.Count > 0 && dataTable != null)
                {
                    this.simpleButton7.Appearance.BackColor = Color.Red;
                }
                else
                {
                    this.simpleButton7.Appearance.BackColor = Color.Green;
                }
            }
            catch (Exception EE)
            {
                var MSITAKE = "Button_color方法失败" + EE.ToString();
                _vLog.writelog(MSITAKE, -1);
            }
        }

        /// <summary>
        /// 定时器启用
        /// </summary>
        public void Timer_state()
        {
            _Timer1.Enabled = true;
        }

        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {
            _Timer1.Enabled = false;
        }

        /// <summary>
        /// 控件关闭
        /// </summary>
        public void _Clear()
        {
            _Timer1.Close();//释放定时器资源
            this.Dispose();//释放资源
            GC.Collect();//调用GC
            bool x = this.IsDisposed;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("输入正确的格式");
            return;
        }
    }
}