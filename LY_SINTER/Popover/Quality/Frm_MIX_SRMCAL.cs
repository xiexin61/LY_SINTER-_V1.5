using DataBase;
using LY_SINTER.Model;
using LY_SINTER.PAGE.Quality;
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

namespace LY_SINTER.Popover.Quality
{
    //声明委托和事件
    public delegate void Trans_Frm_MIX_SRMCAL(float PB, int PBID);
    public partial class Frm_MIX_SRMCAL : Form
    {
        public vLog _vLog { get; set; }
        public static bool isopen = false;
        DBSQL dBSQL = new DBSQL(DataBase.ConstParameters.strCon);
        public System.Timers.Timer _Timer1 { get; set; }
        MIX_PAGE_MODEL _PAGE_MODEL = new MIX_PAGE_MODEL();
        /// <summary>
        /// 烧返配比
        /// </summary>
        public float PB;
        public Frm_MIX_SRMCAL()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Quality\\Frm_MIX_SRMCAL\\");
            Initialize_assignment();
            _Timer1 = new System.Timers.Timer(1000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { _Timer1_Tick(); };
            _Timer1.Enabled = true;
            _Timer1.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
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
                string SQL = "update MC_SRMCAL_RESULT  set SRMCAL_A_FLAG = '4' where TIMESTAMP = ( select MAX(b.TIMESTAMP) from MC_SRMCAL_RESULT b ) and SRMCAL_A_FLAG = '1'";
                int _count = dBSQL.CommandExecuteNonQuery(SQL);
                if (_count == 1)
                {
                    var mistake = "*****调整烧返配比弹出框，更改MC_SRMCAL_RESULT表SRMCAL_A_FLAG标志位为4";
                    _vLog.writelog(mistake, 0);
                }
                else
                {
                    var mistake = "*****调整烧返配比弹出框：人工未操作，更改MC_SRMCAL_RESULT表SRMCAL_A_FLAG标志位为4";
                    _vLog.writelog(mistake, 0);
                }

                string[] _A = { "_PAGE_MODEL"};
                List<float> _B = _PAGE_MODEL.Get_MIX_PAR(_A, "MC_MIXCAL_PAR");
                int _FLAG = int.Parse(_B[0].ToString());
                if (_FLAG == 1)
                {
                    //查询烧返配比对应的配比id
                    string sql_1 = "select top 1 peinimingcheng from CFG_MAT_L2_PBSD_INTERFACE where category = '3'";
                    DataTable dataTable = dBSQL.GetCommand(sql_1);
                    int pbid = int.Parse(dataTable.Rows[0][0].ToString());
                    _transfDelegate(PB, pbid);
                    this.Dispose();
                }
                else
                {
                    this.Dispose();
                }
                
            }
        }
        /// <summary>
        /// //声明委托到配料页面中
        /// </summary>
        public event Trans_Frm_MIX_SRMCAL _transfDelegate;
        /// <summary>
        /// 窗体初始化赋值判断
        /// </summary>
        public void Initialize_assignment()
        {
            try
            {
                string sql = "select top (1) " +
               "isnull(SRMCAL_W_AIM,0) as SRMCAL_W_AIM," +
               "isnull(SRMCAL_E,0) as SRMCAL_E," +
               "isnull(SRMCAL_BILL_SP_OLD,0) as SRMCAL_BILL_SP_OLD," +
               "isnull(SRMCAL_W,0) as SRMCAL_W," +
               "isnull(SRMCAL_EC,0) as SRMCAL_EC," +
               "isnull(SRMCAL_BILL_SP_NEW,0) as SRMCAL_BILL_SP_NEW " +
               "from MC_SRMCAL_RESULT where TIMESTAMP = ( select MAX(TIMESTAMP) from MC_SRMCAL_RESULT  ) and SRMCAL_A_FLAG = '1'";
                DataTable dataTable = dBSQL.GetCommand(sql);
                if (dataTable.Rows.Count > 0 && dataTable != null)
                {
                    //烧返目标仓位
                    float SF_1 = float.Parse(dataTable.Rows[0]["SRMCAL_W_AIM"].ToString());
                    //周期内仓位偏差
                    float SF_2 = float.Parse(dataTable.Rows[0]["SRMCAL_E"].ToString());
                    //调整前烧返配比值
                    float SF_3 = float.Parse(dataTable.Rows[0]["SRMCAL_BILL_SP_OLD"].ToString());
                    //烧返仓有效仓位
                    float SF_4 = float.Parse(dataTable.Rows[0]["SRMCAL_W"].ToString());
                    //近期仓位变化趋势
                    float SF_5 = float.Parse(dataTable.Rows[0]["SRMCAL_EC"].ToString());
                    //调整后烧返配比值
                    float SF_6 = float.Parse(dataTable.Rows[0]["SRMCAL_BILL_SP_NEW"].ToString());
                    PB = float.Parse(dataTable.Rows[0]["SRMCAL_BILL_SP_NEW"].ToString());
                    this.textBox1.Text = SF_1.ToString();
                    this.textBox2.Text = SF_2.ToString();
                    this.textBox3.Text = SF_3.ToString();
                    this.textBox4.Text = SF_4.ToString();
                    this.textBox5.Text = SF_5.ToString();
                    this.textBox6.Text = SF_6.ToString();
                }
                else
                {
                    var mistake = "*****调整烧返配比弹出框：Initialize_assignment（）方式失败,无数据，sql:" + sql;
                    _vLog.writelog(mistake, -1);
                }
            }
            catch (Exception ee)
            {
                var mistake = "*****调整烧返配比弹出框：Initialize_assignment（）方式失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "update MC_SRMCAL_RESULT  set SRMCAL_A_FLAG = 2 where TIMESTAMP = ( select MAX(b.TIMESTAMP) from MC_SRMCAL_RESULT b ) and SRMCAL_A_FLAG = '1'";
                int _count = dBSQL.CommandExecuteNonQuery(SQL);
                if (_count == 1)
                {
                    var mistake = "*****调整烧返配比弹出框：点击确认按钮，更改MC_SRMCAL_RESULT表SRMCAL_A_FLAG标志位为2";
                    _vLog.writelog(mistake, 0);
                }
                else
                {
                    var mistake = "*****调整烧返配比弹出框：点击确认按钮，更换SRMCAL_A_FLAG标志位为2失败。sql:" + SQL;
                    _vLog.writelog(mistake, -1);
                }
                //查询烧返配比对应的配比id
                string sql_1 = "select top 1 peinimingcheng from CFG_MAT_L2_PBSD_INTERFACE where category = '3'";
                DataTable dataTable = dBSQL.GetCommand(sql_1);
                int pbid = int.Parse(dataTable.Rows[0][0].ToString());
                _transfDelegate(PB, pbid);
                this.Close();
            }
            catch (Exception ee)
            {
                var mistake = "*****调整烧返配比弹出框：点击确认按钮失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
            
        }
        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "update MC_SRMCAL_RESULT  set SRMCAL_A_FLAG = '3' where TIMESTAMP = ( select MAX(b.TIMESTAMP) from MC_SRMCAL_RESULT b ) and SRMCAL_A_FLAG = '1'";
               int _count =  dBSQL.CommandExecuteNonQuery(SQL);
                if (_count == 1)
                {
                    var mistake = "*****调整烧返配比弹出框：点击取消按钮，更新MC_SRMCAL_RESULT表标志位SRMCAL_A_FLAG为3";
                    _vLog.writelog(mistake, 0);
                }
                else
                {
                    var mistake = "*****调整烧返配比弹出框：点击取消按钮，更新MC_SRMCAL_RESULT表标志位SRMCAL_A_FLAG为3失败，sql:" + SQL;
                    _vLog.writelog(mistake, -1);
                }
                this.Close();
            }
            catch (Exception ee)
            {
                var mistake = "*****调整烧返配比弹出框：点击取消按钮失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
           
        }

        private void Frm_MIX_SRMCAL_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
            MIX_Intelligent._Auto = null;
        }
    }
}
