using DataBase;
using LY_SINTER.Model;
using LY_SINTER.PAGE.Parameter;
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

namespace LY_SINTER.Popover.Parameter
{
    public partial class Button_untreated : Form
    {
        //声明委托和事件
        public delegate void TransfDelegate_YLBH_WCL_BUTTON();
        public static bool isopen = false;
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public vLog _vLog { get; set; }
        public Button_untreated()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Parametery\\Ingredient_Protect_Pop_Perfect\\");
            Date();
        }
        //声明委托和事件
        public event TransfDelegate_YLBH_WCL_BUTTON _transfDelegate_YLBH_WCL_BUTTON;
        /// <summary>
        /// 初始化
        /// </summary>
        private void Date()
        {

            try
            {
                var _sql = "select  L3_CODE from M_MATERIAL_COOD_NEW_RECORD where FLAG = 1 order by L3_CODE asc";
                DataTable _data = dBSQL.GetCommand(_sql);
                if (_data != null && _data.Rows.Count > 0)
                {
                    DataTable table = new DataTable();
                    table.Columns.Add("ID");
                    table.Columns.Add("TIMESTAMP");
                    table.Columns.Add("L3_CODE");
                    for (int x = 0; x < _data.Rows.Count; x++)
                    {
                        var _sql1 = "select top (1) TIMESTAMP from M_MATERIAL_COOD_NEW_RECORD where L3_CODE = '" + _data.Rows[x][0].ToString() + "' order by TIMESTAMP desc";
                        DataTable table1 = dBSQL.GetCommand(_sql1);
                        if (table1.Rows.Count > 0 && table1 != null)
                        {
                            DataRow _row = table.NewRow();
                            _row["ID"] = x + 1;
                            _row["TIMESTAMP"] = table1.Rows[0]["TIMESTAMP"].ToString();
                            _row["L3_CODE"] = _data.Rows[x]["L3_CODE"].ToString();
                            table.Rows.Add(_row);
                        }
                    }
                    dataGridView1.DataSource = table;
                }
                else
                {
                    dataGridView1.DataSource = null;
                }
            }
            catch (Exception ee)
            {
                var mistake = "Date方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "button" && e.RowIndex >= 0)
                {
                    Raw_Popup.UNTREATED_BL = dataGridView1.Rows[e.RowIndex].Cells["L3_CODE"].Value.ToString();
                    Untreated_popup form_display = new Untreated_popup();
                    if (Untreated_popup.isopen == false)
                    {
                        form_display._transfDelegate_untreated_popup += Date;
                        form_display.ShowDialog();
                    }
                    else
                    {
                        form_display.Activate();
                    }

                }

                else if (dataGridView1.Columns[e.ColumnIndex].Name == "button_del" && e.RowIndex >= 0)
                {
                    string L3_CODE = dataGridView1.Rows[e.RowIndex].Cells["L3_CODE"].Value.ToString();
                    string SQL = "UPDATE M_MATERIAL_COOD_NEW_RECORD SET FLAG = 3 WHERE L3_CODE = '" + L3_CODE + "'";
                  int a =   dBSQL.CommandExecuteNonQuery(SQL);
                    if (a > 0 )
                    {
                        Date();
                        var text = "点击删除按钮，删除暂存物料三级编码为:" + L3_CODE + "成功";
                        _vLog.writelog(text,0);
                       
                    }
                    else
                    {
                        var text = "点击删除按钮，删除暂存物料三级编码为:" + L3_CODE + "失败，sql:" + SQL;
                        _vLog.writelog(text, -1);
                    }
                  
                }
                else
                {
                    return;
                }
            }
            catch(Exception ee)
            {
                var mistake = "点击表单按钮失败"+ee.ToString();
                _vLog.writelog(mistake, -1);
            }
           
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            _transfDelegate_YLBH_WCL_BUTTON();
            this.Dispose();
        }
    }
}
