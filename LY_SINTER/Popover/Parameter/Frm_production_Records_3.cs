using DataBase;
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
    public partial class Frm_production_Records_3 : Form
    {
        public static bool isopen = false;
        public vLog _vLog { get; set; }
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public Frm_production_Records_3()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 下拉框赋值
        /// </summary>
        public void Checkbox_value()
        {
            try
            {
                #region 班次
                DataTable data_1 = new DataTable();
                data_1.Columns.Add("NAME");
                data_1.Columns.Add("VALUES");
                DataRow row1_1 = data_1.NewRow();
                row1_1["NAME"] = "夜";
                row1_1["VALUES"] = 1;
                data_1.Rows.Add(row1_1);
                DataRow row1_2 = data_1.NewRow();
                row1_2["NAME"] = "白";
                row1_2["VALUES"] = 2;
                data_1.Rows.Add(row1_2);
                comboBox1.DataSource = data_1;
                comboBox1.DisplayMember = "NAME";
                comboBox1.ValueMember = "VALUES";
                #endregion

                #region 班别
                DataTable data_2 = new DataTable();
                data_2.Columns.Add("NAME");
                data_2.Columns.Add("VALUES");
                DataRow row2_1 = data_2.NewRow();
                row2_1["NAME"] = "甲";
                row2_1["VALUES"] = 1;
                data_2.Rows.Add(row2_1);
                DataRow row2_2 = data_2.NewRow();
                row2_2["NAME"] = "乙";
                row2_2["VALUES"] = 2;
                data_2.Rows.Add(row2_2);
                DataRow row2_3 = data_2.NewRow();
                row2_3["NAME"] = "丙";
                row2_3["VALUES"] = 3;
                data_2.Rows.Add(row2_3);
                DataRow row2_4 = data_2.NewRow();
                row2_4["NAME"] = "丁";
                row2_4["VALUES"] = 3;
                data_2.Rows.Add(row2_4);
                comboBox2.DataSource = data_2;
                comboBox2.DisplayMember = "NAME";
                comboBox2.ValueMember = "VALUES";
                #endregion

        
            }
            catch (Exception ee)
            {
                _vLog.writelog("Checkbox_value方法错误" + ee.ToString(), -1);
            }
        }
    }
}
