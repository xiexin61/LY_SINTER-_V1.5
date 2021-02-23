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
    public partial class Frm_production_Records_4 : Form
    {
        public static bool isopen = false;
        public vLog _vLog { get; set; }
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        Parameter_Model _Model = new Parameter_Model();
        //声明委托和事件
        public delegate void production_Records_4();
        //声明委托和事件
        public event production_Records_4 _production_Records_4;
        int FLAG = Quality_Model.FLAG_2;//接收主页面选中的标志位
        public Frm_production_Records_4()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Parameter\\Frm_production_Records_4\\");
            Checkbox_value();//下拉框赋值
            _Show();//初始化数据
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            _update();
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
        /// 初始化数据
        /// </summary>
        public void _Show()
        {
            try
            {
                var _sql = "select * from M_EVENT_INFOR where FLAG = " + FLAG + "";
                DataTable table = dBSQL.GetCommand(_sql);
                if (table.Rows.Count > 0 && table != null)
                {
                    //班次
                    comboBox1.Text = table.Rows[0]["WORK_SHIFT"].ToString();
                    //班别
                    comboBox2.Text = table.Rows[0]["WORK_TEAM"].ToString();
                    //原因描述
                    textBox2.Text = table.Rows[0]["REMARK_DESC"].ToString();
                }
                else
                {
                    _vLog.writelog("_Show方法失败,sql" + _sql, -1);
                }
            }
            catch (Exception EE)
            {
                _vLog.writelog("_Show方法失败" + EE.ToString(), -1);
            }
        }
        public void _update()
        {
            var sql = "update M_EVENT_INFOR set TIMESTAMP = getdate(),WORK_SHIFT = '" + comboBox1.Text.ToString() + "',WORK_TEAM = '" + comboBox2.Text.ToString() + "',REMARK_DESC = '" + textBox2.Text.ToString() + "' where FLAG = " + FLAG + "";
            int _count = dBSQL.CommandExecuteNonQuery(sql);
            if (_count != 1)
            {
                _vLog.writelog("_update方法调用失败，sql" + sql, -1);
            }
            else
            {
                _production_Records_4();
                this.Dispose();
            }
           
        }
    }
}
