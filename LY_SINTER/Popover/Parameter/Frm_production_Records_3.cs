using DataBase;
using LY_SINTER.Model;
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
        //声明委托和事件
        public delegate void production_Records_3();
        //声明委托和事件
        public event production_Records_3 _production_Records_3;
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        Parameter_Model _Model = new Parameter_Model();
        public Frm_production_Records_3()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Parameter\\Frm_production_Records_3\\");
            Checkbox_value();
        }
        /// <summary>
        /// 下拉框赋值
        /// </summary>
        public void Checkbox_value()
        {
            try
            {
                #region 班次
             
                comboBox1.DataSource = _Model._GetClass(1);
                comboBox1.DisplayMember = "NAME";
                comboBox1.ValueMember = "VALUES";
                #endregion

                #region 班别
        
                comboBox2.DataSource = _Model._GetClass(2);
                comboBox2.DisplayMember = "NAME";
                comboBox2.ValueMember = "VALUES";
                #endregion

                Tuple<bool, string, string> _geta = _Model._Get_Class_Plan(DateTime.Now);
                if (_geta.Item1)
                {
                    comboBox1.Text = _geta.Item2;
                    comboBox2.Text = _geta.Item3;
                }
        
            }
            catch (Exception ee)
            {
                _vLog.writelog("Checkbox_value方法错误" + ee.ToString(), -1);
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        public void _update()
        {
            try
            {
                int x = 1;
                string sql_id = "select max(FLAG) from M_EVENT_INFOR";
                DataTable table_1 = dBSQL.GetCommand(sql_id);
                if (table_1.Rows.Count > 0 && table_1 != null)
                {
                    x = int.Parse(table_1.Rows[0][0].ToString() == "" ? "0" : table_1.Rows[0][0].ToString()) + 1;
                }
                var sql = "insert into M_EVENT_INFOR(TIMESTAMP,WORK_SHIFT,WORK_TEAM,REMARK_DESC,flag) values(getdate(),'"+ comboBox1 .Text.ToString()+ "','" + comboBox2.Text.ToString() + "','"+ textBox2 .Text.ToString()+ "',"+x+")";
                int count = dBSQL.CommandExecuteNonQuery(sql);
                if (count != 1)
                {
                    _vLog.writelog("_update方法失败" + sql,-1);
                    MessageBox.Show("操作失败，检查数据库连接");
                }
                else
                {
                    _production_Records_3();
                    this.Dispose();
                }

            }
            catch(Exception ee)
            {
                _vLog.writelog("_update方法调用失败" + ee.ToString(), -1);
            }
        }
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            _update();
        }
    }
}
