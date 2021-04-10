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
using LY_SINTER.Model;
using VLog;
using LY_SINTER.Custom;
using System.IO;
using LY_SINTER.Popover.Parameter;

namespace LY_SINTER.PAGE.Parameter
{
    public partial class Ingredient : UserControl
    {
        
        DBSQL _dBSQL = new DBSQL(DataBase.ConstParameters.strCon);//连接数据库
        Message_Logging logTable = new Message_Logging();//主框架通用方法
        public vLog _vLog { get; set; }
        /// <summary>
        /// 初始化颜色变化定时器
        /// </summary>
        public System.Timers.Timer _Timer1 { get; set; }
        public Ingredient()
        {
            InitializeComponent();
            time_begin_end();//开始&结束时间赋值
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Parametery\\Ingredient_Page\\");
            Combox_value();//下拉框赋值
            Show_d1();//再用成分赋值
            SHOW_D2();//历史成分赋值
            Time_now();//最新修改时间
            TIMER_Statement();//定时器声明
        }
     
        /// <summary>
        /// 再用成分赋值
        /// </summary>
        public void Show_d1()
        {
            try
            {
                string sql = "select a.BIN_NUM_SHOW ,b.MAT_DESC, (CASe when a.STATE = 1 then '自动维护' when a.STATE = 0 then '手动维护' when a.STATE = 2 then '自动维护2' end) as STATE," +
                    "a.NUMBER_FLAG,(CASe when a.P_T_FLAG = 1 then '启用' when a.P_T_FLAG = 0 then '禁用' end) as P_T_FLAG ," +
               "a.C_TFE,a.C_FEO,a.C_CAO,a.C_SIO2,a.C_AL2O3,a.C_MGO,a.C_S,a.C_P,a.C_C,a.C_MN,a.C_LOT,a.C_R,a.C_H2O," +
               "a.C_ASH,a.C_VOLATILES,a.C_TIO2,a.C_K2O,a.C_NA2O,a.C_PBO," +
               "a.C_ZNO,a.C_F,a.C_AS,a.C_CU,a.C_PB,a.C_ZN,a.C_K,a.C_NA,a.C_CR,a.C_NI,a.C_MNO from M_MATERIAL_BINS a," +
               " M_MATERIAL_COOD b where a.L2_CODE = b.L2_CODE ORDER BY BIN_NUM_SHOW asc";
                DataTable dataTable = _dBSQL.GetCommand(sql);
                dataGridView1.DataSource = dataTable;
            }
            catch(Exception ee)
            {
                var mistake = "Show_d1()方法初始失败" + ee.ToString();
                _vLog.writelog(mistake,-1);
            }
           
        }
        /// <summary>
            /// 下拉框赋值
            /// </summary>
        public void Combox_value()
        {
            DataTable table_1 = new DataTable();
            table_1.Columns.Add("NAME");
            table_1.Columns.Add("VALUE");
            for (int x = 0; x <=20;x++)
            {
                DataRow dr = table_1.NewRow();
                string _name = "";
                if (x == 0)
                {
                    _name = "所有仓";
                }
                else
                {
                    _name = x + "#仓";
                }
                dr[0] = _name;
                dr[1] = x;
                table_1.Rows.Add(dr);
            }
            this.comboBox1.DataSource = table_1;
            this.comboBox1.DisplayMember = "NAME";
            this.comboBox1.ValueMember = "VALUE";
            comboBox1.Text = "所有仓";
        }
        /// <summary>
        /// 开始时间-结束时间赋值
        /// </summary>
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddMonths(-1);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();

        }
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            SHOW_D2();//历史成分赋值
        }
        /// <summary>
        /// 历史成分赋值
        /// </summary>
        public void SHOW_D2()
        {
            try
            {
                string time_begin = textBox_begin.Text.ToString();
                string time_end = textBox_end.Text.ToString();
                string CH_NAME = comboBox1.Text;
                int CH = 0;
                string sql = "";
                if (CH_NAME == "所有仓")
                {
                    sql = "select ROW_NUMBER() OVER (ORDER BY a.TIMESTAMP desc) AS ID," +
                        "A.TIMESTAMP, a.BIN_NUM_SHOW ,b.MAT_DESC, (CASe when a.STATE = 1" +
                        " then '自动维护' when a.STATE = 0 then '手动维护' end) as STATE," +
                        "a.NUMBER_FLAG,(CASe when a.P_T_FLAG = 1 then '启用' when a.P_T_FLAG = 0 then '禁用' end) as P_T_FLAG " +
                        ",a.C_TFE,a.C_FEO,a.C_CAO,a.C_SIO2,a.C_AL2O3,a.C_MGO,a.C_S,a.C_P,a.C_C,a.C_MN,a.C_LOT,a.C_R,a.C_H2O,a.C_ASH," +
                        "a.C_VOLATILES,a.C_TIO2,a.C_K2O,a.C_NA2O,a.C_PBO,a.C_ZNO,a.C_F,a.C_AS,a.C_CU,a.C_PB,a.C_ZN,a.C_K,a.C_NA,a.C_CR," +
                        "a.C_NI,a.C_MNO from M_MATERIAL_BINS_CHANGE a, M_MATERIAL_COOD b " +
                        "where a.L2_CODE = b.L2_CODE AND A.TIMESTAMP >= '" + time_begin + "' AND A.TIMESTAMP < = '" + time_end + "'  ORDER BY A.TIMESTAMP DESC";
                }
                else
                {
                    CH = int.Parse(comboBox1.SelectedIndex.ToString());
                    sql = "select ROW_NUMBER() OVER (ORDER BY a.TIMESTAMP desc) AS ID," +
                       "A.TIMESTAMP, a.BIN_NUM_SHOW ,b.MAT_DESC, (CASe when a.STATE = 1" +
                       " then '自动维护' when a.STATE = 0 then '手动维护' end) as STATE," +
                       "a.NUMBER_FLAG,(CASe when a.P_T_FLAG = 1 then '启用' when a.P_T_FLAG = 0 then '禁用' end) as P_T_FLAG " +
                       ",a.C_TFE,a.C_FEO,a.C_CAO,a.C_SIO2,a.C_AL2O3,a.C_MGO,a.C_S,a.C_P,a.C_C,a.C_MN,a.C_LOT,a.C_R,a.C_H2O,a.C_ASH," +
                       "a.C_VOLATILES,a.C_TIO2,a.C_K2O,a.C_NA2O,a.C_PBO,a.C_ZNO,a.C_F,a.C_AS,a.C_CU,a.C_PB,a.C_ZN,a.C_K,a.C_NA,a.C_CR," +
                       "a.C_NI,a.C_MNO from M_MATERIAL_BINS_CHANGE a, M_MATERIAL_COOD b " +
                       "where a.L2_CODE = b.L2_CODE AND A.TIMESTAMP >= '" + time_begin + "' AND A.TIMESTAMP < = '" + time_end + "' AND A.BIN_NUM_SHOW = " + CH + "  ORDER BY A.TIMESTAMP DESC";
                }
                DataTable dataTable = _dBSQL.GetCommand(sql);
                dataGridView2.DataSource = dataTable;
            }
            catch (Exception ee)
            {
                var mistake = "查询按钮失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            time_begin_end();//开始&结束时间赋值
            Combox_value();//下拉框赋值
            SHOW_D2();//历史成分赋值
        }
        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    for (int i = 0; i < dataGridView2.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            columnTitle += "\t";
                        }
                        columnTitle += dataGridView2.Columns[i].HeaderText;
                    }
                    sw.WriteLine(columnTitle);

                    //写入列内容    
                    for (int j = 0; j < dataGridView2.Rows.Count; j++)
                    {
                        string columnValue = "";
                        for (int k = 0; k < dataGridView2.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                columnValue += "\t";
                            }
                            if (dataGridView2.Rows[j].Cells[k].Value == null)
                                columnValue += "";
                            else
                                columnValue += dataGridView2.Rows[j].Cells[k].Value.ToString().Trim();
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
        /// 定时器声明
        /// </summary>
        public void TIMER_Statement()
        {

            _Timer1 = new System.Timers.Timer(1000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { _Timer1_Tick(); };
            _Timer1.Enabled = true;
            _Timer1.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
        }
        /// <summary>
        /// 最新时间、仓号赋值
        /// </summary>
        public void Time_now()
        {
            try
            {
                string sql_time = "select TOP (1)  TIMESTAMP,BIN_NUM_SHOW from M_MATERIAL_BINS ORDER BY TIMESTAMP DESC";
                DataTable dataTable_time = _dBSQL.GetCommand(sql_time);
                int CH = int.Parse(dataTable_time.Rows[0][1].ToString());
                string time = dataTable_time.Rows[0][0].ToString();
                label2.Text = "最新修改仓号：" + CH + "  最新时间 ：" + time;
            }
            catch (Exception ee)
            {
                var mistake = "" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
           
        }
        /// <summary>
        /// d1点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "button" && e.RowIndex >= 0)
                {
                    int CH = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                    Ingredient_Model.Data = CH;
                    Ingredient_Popup form_display = new Ingredient_Popup();
                    if (Ingredient_Popup.isopen == false)
                    {
                        form_display._TransfDelegate_YLWH += _TransfDelegate;
                        form_display.ShowDialog();
                    }
                    else
                    {
                        form_display.Activate();
                        //}
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ee)
            {
                var mistake = "点击成分修改按钮" + ee.ToString();
                _vLog.writelog(mistake,-1);

            }
        }
        /// <summary>
        /// 颜色变化
        /// </summary>
        public void Color_Change()
        {
            for (int x = 0; x < dataGridView1.Rows.Count; x++)
            {
                if (dataGridView1.Rows[x].Cells[3].Value.ToString() == "手动维护")
                {
                    dataGridView1.Rows[x].Cells[3].Style.ForeColor = Color.Red;
                }
                else
                {
                    dataGridView1.Rows[x].Cells[3].Style.ForeColor = Color.Green;
                }
            }
           
        }
        private void _Timer1_Tick()
        {
            Color_Change();
        }
        /// <summary>
        /// 弹出框关闭响应事件
        /// </summary>
        public void _TransfDelegate()
        {
            time_begin_end();//开始&结束时间赋值
            Show_d1();//再用成分赋值
            SHOW_D2();//历史成分赋值
            Time_now();//最新修改时间
            Color_Change();//颜色变化
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
        }
    }
}
