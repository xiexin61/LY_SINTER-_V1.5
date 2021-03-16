using DataBase;
using LY_SINTER.Model;
using LY_SINTER.Popover.Parameter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLog;

namespace LY_SINTER.PAGE.Parameter
{
    public partial class Ingredient_auto : Form
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public static bool isopen = false;
        public vLog _vLog { get; set; }
        /// <summary>
        /// 判断模式
        /// 1：读取检化验表数据
        /// 2：读取三级直接传送数据
        /// </summary>
        int flag = 2;
        public Ingredient_auto()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Parametery\\Ingredient_Protect_Pop_auto\\");
            Date();
        }
        public void Date()
        {
            try
            {
                if (flag == 1)
                {
                    var _sql = "select distinct L3_CODE from M_MATERIAL_ANALYSIS where FLAG = 1 order by L3_CODE asc";
                    DataTable _data = dBSQL.GetCommand(_sql);
                    if (_data != null && _data.Rows.Count > 0)
                    {
                        DataTable table = new DataTable();
                        table.Columns.Add("ID");
                        table.Columns.Add("REOPTTIME");
                        table.Columns.Add("L3_CODE");
                        for (int x = 0; x < _data.Rows.Count; x++)
                        {
                            var _sql1 = "select top (1) REOPTTIME from M_MATERIAL_ANALYSIS where L3_CODE = '" + _data.Rows[x][0].ToString() + "' order by REOPTTIME desc";
                            DataTable table1 = dBSQL.GetCommand(_sql1);
                            if (table1.Rows.Count > 0 && table1 != null)
                            {
                                DataRow _row = table.NewRow();
                                _row["ID"] = x + 1;
                                _row["REOPTTIME"] = table1.Rows[0]["REOPTTIME"].ToString();
                                _row["L3_CODE"] = _data.Rows[x]["L3_CODE"].ToString();
                                table.Rows.Add(_row);
                            }
                        }
                        dataGridView1.DataSource = table;
                    }
                    else
                    {
                        this.Close();
                        Form_Main._Auto = null;
                    }

                }
                else
                {
                    var _sql = "select * from B_MATERIAL_CODE where ProcessType = 'I' AND FLAG = 1";
                    DataTable _data = dBSQL.GetCommand(_sql);
                    if (_data != null && _data.Rows.Count > 0)
                    {
                        DataTable table = new DataTable();
                        table.Columns.Add("ID");
                        table.Columns.Add("REOPTTIME");
                        table.Columns.Add("L3_CODE");
                        for (int x = 0; x < _data.Rows.Count; x++)
                        {
                           
                                DataRow _row = table.NewRow();
                                _row["ID"] = x + 1;
                                _row["REOPTTIME"] = _data.Rows[x]["MaterialCode"].ToString();
                                _row["L3_CODE"] = _data.Rows[x]["TIMESTAMP"].ToString();
                                table.Rows.Add(_row);
                            
                        }
                        dataGridView1.DataSource = table;
                    }
                }
            
              
            }
            catch(Exception ee)
            {
                var mistake = "Date方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "BL" && e.RowIndex >= 0)
                {
                    string L3_CODE = dataGridView1.Rows[e.RowIndex].Cells["L3_CODE"].Value.ToString();
                    Raw_Popup.L3_CODE_BL = L3_CODE;
                    Ingredient_supplement form_display = new Ingredient_supplement();
                    if (Ingredient_supplement.isopen == false)
                    {
                        form_display.ShowDialog();
                    }
                    else
                    {
                        form_display.Activate();
                    }
                    this.Dispose();
                    Form_Main._Auto = null;
                }
                else if (dataGridView1.Columns[e.ColumnIndex].Name == "ZC" && e.RowIndex >= 0)
                {
                    string L3_CODE = dataGridView1.Rows[e.RowIndex].Cells["L3_CODE"].Value.ToString();
                    string datatime = dataGridView1.Rows[e.RowIndex].Cells["REOPTTIME"].Value.ToString();
                    //插入到M_MATERIAL_COOD_NEW_RECORD表中
                    string sql = "insert into M_MATERIAL_COOD_NEW_RECORD (TIMESTAMP,L3_CODE,FLAG) VALUES ('" + datatime + "','" + L3_CODE + "','1')";
                    dBSQL.CommandExecuteNonQuery(sql);
                    //重置标志位M_MATERIAL_ANALYSIS
                    //string sql_M_MATERIAL_ANALYSIS = "update M_MATERIAL_ANALYSIS set FLAG = 2 where L3_CODE = '" + L3_CODE + "' and REOPTTIME = '" + datatime + "'";
                    string sql_M_MATERIAL_ANALYSIS = "update M_MATERIAL_ANALYSIS set FLAG = 2 where L3_CODE = '" + L3_CODE + "' ";
                    dBSQL.CommandExecuteNonQuery(sql_M_MATERIAL_ANALYSIS);
                    _vLog.writelog("三级代码:" + L3_CODE + "暂存成功", 0);
                    Date();
                }
                else
                {
                    return;
                }
            }
            catch(Exception EE)
            {
                var MISTAKE = "点击按钮失败"+EE.ToString();
                _vLog.writelog(MISTAKE,-1);
            }
            
        }
        private void Ingredient_auto_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            this.Close();
            Form_Main._Auto = null;
            
        }
    }
}
