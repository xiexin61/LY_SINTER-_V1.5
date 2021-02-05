using DataBase;
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
    public delegate void TransfDelegate_1(bool flag, float C, float R, float MG);
    public partial class Frm_MIX_Ingredient : Form
    {
        public System.Timers.Timer _Timer1 { get; set; }
        public vLog _vLog { get; set; }
        DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        /// <summary>
        /// C使用时间
        /// </summary>
        DateTime TIME_C;
        /// <summary>
        /// R使用时间
        /// </summary>
        DateTime TIME_R;
        /// <summary>
        /// MG使用时间
        /// </summary>
        DateTime TIME_MG;
        /// <summary>
        /// C自动调整模型计算结果状态
        /// </summary>
        public int C_FLAG_1;
        /// <summary>
        /// C自动调整模型状态
        /// </summary>
        public int C_FLAG_2;
        /// <summary>
        /// R自动调整模型计算结果状态
        /// </summary>
        public int R_FLAG_1;
        /// <summary>
        /// R自动调整模型状态
        /// </summary>
        public int R_FLAG_2;
        /// <summary>
        /// MG自动调整模型计算结果状态
        /// </summary>
        public int MG_FLAG_1;
        /// <summary>
        /// MG自动调整模型状态
        /// </summary>
        public int MG_FLAG_2;
        public Frm_MIX_Ingredient()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Quality\\Frm_MIX_Ingredient\\");
            Initialize_assignment();
            _Timer1 = new System.Timers.Timer(30000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { Timer1_Tick_1(); };
            _Timer1.Enabled = true;
            _Timer1.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
        }
        //声明委托和事件
        public event TransfDelegate_1 transfDelegate;
        private void Timer1_Tick_1()
        {
            Action invokeAction = new Action(Timer1_Tick_1);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                _Method();
                this.Dispose();
                MIX_Intelligent._Auto_1 = null;
            }
        }
        /// <summary>
        /// 窗体初始化赋值判断
        /// </summary>
        private void Initialize_assignment()
        {

            try
            {

                #region C自动调整信息赋值判断
                //检索MC_SINCAL_C_result标志位
                string sql_c = "select top (1) " +
                    "isnull(SINCAL_C_FLAG,0) as SINCAL_C_FLAG," +
                    "isnull(SINCAL_C_MODEL_FLAG,0) as SINCAL_C_MODEL_FLAG," +
                    "isnull(SINCAL_C_FEO_FLAG,0) as SINCAL_C_FEO_FLAG," +
                    "isnull(SINCAL_C_BILL_FLAG,0) as SINCAL_C_BILL_FLAG," +
                    "isnull(SINCAL_C_BILL_BFES_ORE_FLAG,0) as SINCAL_C_BILL_BFES_ORE_FLAG," +
                    "isnull(SINCAL_C_LOT_FLAG,0) as SINCAL_C_LOT_FLAG," +
                    "isnull(SINCAL_C_NONFUEL_FLAG,0) as SINCAL_C_NONFUEL_FLAG," +
                    "isnull(SINCAL_C_FEO_MA_FLAG,0) as SINCAL_C_FEO_MA_FLAG," +
                    "isnull(SINCAL_C_HOST_FLAG,0) as SINCAL_C_HOST_FLAG" +
                    "  from MC_SINCAL_C_result order by TIMESTAMP desc ";
                DataTable dataTable_c = _dBSQL.GetCommand(sql_c);
                int flag_C_calculate = 0;
                //C自动开关状态
                int flag_C_switch = 0;
                //烧结矿FeO变化调整条件触发标志
                int flag_C_1 = 0;
                //烧结返矿配比变化调整条件触发标志
                int flag_C_2 = 0;
                //高返配比变化调整条件触发标志
                int flag_C_3 = 0;
                //综合烧损变化调整条件触发标志
                int flag_C_4 = 0;
                //非燃料含碳变化调整条件触发标志
                int flag_C_5 = 0;
                //原料FeO变化调整条件触发标志
                int flag_C_6 = 0;
                //主机参数变化调整条件触发标志
                int flag_C_7 = 0;
                if (dataTable_c.Rows.Count > 0)
                {
                    //C自动调整模型计算结果状态
                    flag_C_calculate = int.Parse(dataTable_c.Rows[0]["SINCAL_C_FLAG"].ToString());
                    //按钮判断标志位
                    C_FLAG_1 = int.Parse(dataTable_c.Rows[0]["SINCAL_C_FLAG"].ToString());
                    //C自动开关状态
                    flag_C_switch = int.Parse(dataTable_c.Rows[0]["SINCAL_C_MODEL_FLAG"].ToString());
                    //按钮判断标志位
                    C_FLAG_2 = int.Parse(dataTable_c.Rows[0]["SINCAL_C_MODEL_FLAG"].ToString());
                    //烧结矿FeO变化调整条件触发标志
                    flag_C_1 = int.Parse(dataTable_c.Rows[0]["SINCAL_C_FEO_FLAG"].ToString());
                    //烧结返矿配比变化调整条件触发标志
                    flag_C_2 = int.Parse(dataTable_c.Rows[0]["SINCAL_C_BILL_FLAG"].ToString());
                    //高返配比变化调整条件触发标志
                    flag_C_3 = int.Parse(dataTable_c.Rows[0]["SINCAL_C_BILL_BFES_ORE_FLAG"].ToString());
                    //综合烧损变化调整条件触发标志
                    flag_C_4 = int.Parse(dataTable_c.Rows[0]["SINCAL_C_LOT_FLAG"].ToString());
                    //非燃料含碳变化调整条件触发标志
                    flag_C_5 = int.Parse(dataTable_c.Rows[0]["SINCAL_C_NONFUEL_FLAG"].ToString());
                    //原料FeO变化调整条件触发标志
                    flag_C_6 = int.Parse(dataTable_c.Rows[0]["SINCAL_C_FEO_MA_FLAG"].ToString());
                    //主机参数变化调整条件触发标志
                    flag_C_7 = int.Parse(dataTable_c.Rows[0]["SINCAL_C_HOST_FLAG"].ToString());
                }
                else
                {
                    return;
                }

                //SINCAL_C_FLAG，只要是1的话显示弹出窗对应表的数据展示,则不显示
                if (flag_C_calculate == 1)
                {
                    #region 查询数据
                    //烧结矿FeO偏差配碳调整信息
                    string sql_C_1 = "select top (1) TIMESTAMP," +
                        "isnull(SINCAL_C_A,0) as SINCAL_C_A," +
                        "isnull(SINCAL_C_FeO_ADJ,0) as SINCAL_C_FeO_ADJ," +
                        "isnull(SINCAL_C_FeO_TEST,0) as SINCAL_C_FeO_TEST," +
                        "isnull(SINCAL_C_FeO_RE_ADJ,0) as SINCAL_C_FeO_RE_ADJ," +
                         //烧返配比变化配碳调整信息
                         "isnull(SINCAL_C_BILL_RM_OLD,0) as SINCAL_C_BILL_RM_OLD," +
                        "isnull(SINCAL_C_BILL_SIN_RM_ADJ,0) as SINCAL_C_BILL_SIN_RM_ADJ," +
                        "isnull(SINCAL_C_BILL_RM_NEW,0) as SINCAL_C_BILL_RM_NEW," +
                        "isnull(SINCAL_C_BILL_SIN_RM_RE_ADJ,0) as SINCAL_C_BILL_SIN_RM_RE_ADJ," +
                         //高返配比变化配碳调整信息
                         "isnull(SINCAL_C_BILL_BFES_ORE_OLD,0) as SINCAL_C_BILL_BFES_ORE_OLD," +
                        "isnull(SINCAL_C_BILL_BFES_ORE_ADJ,0) as SINCAL_C_BILL_BFES_ORE_ADJ," +
                        "isnull(SINCAL_C_BILL_BFES_ORE_NEW,0) as SINCAL_C_BILL_BFES_ORE_NEW," +
                        "isnull(SINCAL_C_BILL_BFES_ORE_RE_ADJ,0) as SINCAL_C_BILL_BFES_ORE_RE_ADJ ," +
                         //混合料综合烧损变化配碳调整信息
                         "isnull(SINCAL_C_MIX_SP_LOT_OLD,0) as SINCAL_C_MIX_SP_LOT_OLD," +
                        "isnull(SINCAL_C_LOT_ADJ,0) as SINCAL_C_LOT_ADJ," +
                        "isnull(SINCAL_C_MIX_SP_LOT_NEW,0) as SINCAL_C_MIX_SP_LOT_NEW," +
                        "isnull(SINCAL_C_LOT_RE_ADJ,0) as SINCAL_C_LOT_RE_ADJ, " +
                          //非燃料含碳变化配碳调整信息
                          "isnull(SINCAL_C_NON_FUEL_SP_C_OLD,0) as SINCAL_C_NON_FUEL_SP_C_OLD," +
                        "isnull(SINCAL_C_NONFUEL_ADJ,0) as SINCAL_C_NONFUEL_ADJ," +
                        "isnull(SINCAL_C_NON_FUEL_SP_C_NEW,0) as SINCAL_C_NON_FUEL_SP_C_NEW, " +
                        "isnull(SINCAL_C_NONFUEL_RE_ADJ,0) as SINCAL_C_NONFUEL_RE_ADJ ," +
                           //混合料FeO变化配碳调整信息
                           "isnull(SINCAL_C_MIX_SP_FeO_OLD,0) as SINCAL_C_MIX_SP_FeO_OLD," +
                        "isnull(SINCAL_C_FeO_MA_ADJ,0) as SINCAL_C_FeO_MA_ADJ," +
                        "isnull(SINCAL_C_MIX_SP_FeO_NEW,0) as SINCAL_C_MIX_SP_FeO_NEW," +
                        "isnull(SINCAL_C_FeO_MA_RE_ADJ,0) as SINCAL_C_FeO_MA_RE_ADJ, " +
                           //主机参数变化配碳调整信息
                           "isnull(SINCAL_C_HOST_ADJ,0) as SINCAL_C_HOST_ADJ," +
                        "isnull(SINCAL_C_NONFUEL_ADJ,0) as SINCAL_C_NONFUEL_ADJ," +
                        //综合配碳调整信息
                        "isnull(SINCAL_C_BEFORE_Modify,0) as SINCAL_C_BEFORE_Modify," +
                        "isnull(SINCAL_C_BEFORE_SV_C,0) as SINCAL_C_BEFORE_SV_C," +
                        "isnull(SINCAL_C_AFTER_Modify,0) as SINCAL_C_AFTER_Modify," +
                        "isnull(SINCAL_C_SV_C,0) as SINCAL_C_SV_C " +
                        " from MC_SINCAL_C_result where TIMESTAMP = (select max(TIMESTAMP) from MC_SINCAL_C_result)";


                    DataTable dataTable_C_1 = _dBSQL.GetCommand(sql_C_1);

                    #endregion
                    #region 烧结矿FeO偏差配碳调整信息赋值
                    //****烧结矿FeO偏差配碳调整信息****
                    //目标C
                    if (dataTable_C_1.Rows.Count > 0)
                    {
                        //时间
                        TIME_C = DateTime.Parse(dataTable_C_1.Rows[0]["TIMESTAMP"].ToString());
                        // float C_1_1 = float.Parse(dataTable_C_1.Rows[0]["SINCAL_C_A"].ToString());
                        //计算C调整量
                        float C_1_2 = float.Parse(dataTable_C_1.Rows[0]["SINCAL_C_FeO_ADJ"].ToString());

                        //**赋值
                        //20200805
                        if (C_1_2 == 0)
                        {
                            this.textBox7.Text = "0";//目标c
                        }
                        else
                        {

                            this.textBox7.Text = dataTable_C_1.Rows[0]["SINCAL_C_A"].ToString();//目标c
                        }
                        //20200805
                        this.textBox8.Text = dataTable_C_1.Rows[0]["SINCAL_C_FeO_ADJ"].ToString();//计算C调整量
                        this.textBox9.Text = dataTable_C_1.Rows[0]["SINCAL_C_FeO_TEST"].ToString();//检测Feo
                        this.textBox10.Text = dataTable_C_1.Rows[0]["SINCAL_C_FeO_RE_ADJ"].ToString();//修正C调整量


                        #endregion
                        #region 烧返配比变化配碳调整信息


                        //**赋值
                        this.textBox11.Text = dataTable_C_1.Rows[0]["SINCAL_C_BILL_RM_OLD"].ToString();//调整前烧返配比
                        this.textBox12.Text = dataTable_C_1.Rows[0]["SINCAL_C_BILL_SIN_RM_ADJ"].ToString();//计算C调整量
                        this.textBox13.Text = dataTable_C_1.Rows[0]["SINCAL_C_BILL_RM_NEW"].ToString();//调整后烧返配比
                        this.textBox14.Text = dataTable_C_1.Rows[0]["SINCAL_C_BILL_SIN_RM_RE_ADJ"].ToString();//修正C调整量


                        #endregion
                        #region 高返配比变化配碳调整信息



                        //**赋值
                        this.textBox33.Text = dataTable_C_1.Rows[0]["SINCAL_C_BILL_BFES_ORE_OLD"].ToString();//调整前高返配比
                        this.textBox34.Text = dataTable_C_1.Rows[0]["SINCAL_C_BILL_BFES_ORE_ADJ"].ToString();//计算C调整量
                        this.textBox35.Text = dataTable_C_1.Rows[0]["SINCAL_C_BILL_BFES_ORE_NEW"].ToString(); //调整后高返配比
                        this.textBox36.Text = dataTable_C_1.Rows[0]["SINCAL_C_BILL_BFES_ORE_RE_ADJ"].ToString();//修正C调整量


                        #endregion
                        #region 混合料综合烧损变化配碳调整信息



                        //**赋值
                        this.textBox15.Text = dataTable_C_1.Rows[0]["SINCAL_C_MIX_SP_LOT_OLD"].ToString(); //变化前综合烧损
                        this.textBox16.Text = dataTable_C_1.Rows[0]["SINCAL_C_LOT_ADJ"].ToString();//计算C调整量
                        this.textBox17.Text = dataTable_C_1.Rows[0]["SINCAL_C_MIX_SP_LOT_NEW"].ToString();//变化后综合烧损
                        this.textBox18.Text = dataTable_C_1.Rows[0]["SINCAL_C_LOT_RE_ADJ"].ToString(); //修正C调整量


                        #endregion
                        #region 非燃料含碳变化配碳调整信息



                        //**赋值
                        this.textBox19.Text = dataTable_C_1.Rows[0]["SINCAL_C_NON_FUEL_SP_C_OLD"].ToString();//变化前非燃料含碳
                        this.textBox20.Text = dataTable_C_1.Rows[0]["SINCAL_C_NONFUEL_ADJ"].ToString();//计算C调整量
                        this.textBox21.Text = dataTable_C_1.Rows[0]["SINCAL_C_NON_FUEL_SP_C_NEW"].ToString();//变化后非燃料含碳
                        this.textBox22.Text = dataTable_C_1.Rows[0]["SINCAL_C_NONFUEL_RE_ADJ"].ToString();//修正C调整量


                        #endregion
                        #region 混合料FeO变化配碳调整信息



                        //**赋值
                        this.textBox23.Text = dataTable_C_1.Rows[0]["SINCAL_C_MIX_SP_FeO_OLD"].ToString();//变化前混合料FeO含量
                        this.textBox24.Text = dataTable_C_1.Rows[0]["SINCAL_C_FeO_MA_ADJ"].ToString();//计算C调整量
                        this.textBox25.Text = dataTable_C_1.Rows[0]["SINCAL_C_MIX_SP_FeO_NEW"].ToString();//变化后混合料FeO含量
                        this.textBox26.Text = dataTable_C_1.Rows[0]["SINCAL_C_FeO_MA_RE_ADJ"].ToString();//修正C调整量


                        #endregion
                        #region 主机参数变化配碳调整信息

                        //主抽温度持续状态
                        float C_6_1 = float.Parse(dataTable_C_1.Rows[0]["SINCAL_C_HOST_ADJ"].ToString());
                        //计算C调整量
                        //float C_6_2 = float.Parse(dataTable_C_1.Rows[0]["SINCAL_C_NONFUEL_ADJ"].ToString());

                        //**赋值
                        if (C_6_1 < 0)
                        {
                            this.textBox27.Text = "主抽温度持续偏高";
                        }
                        //20200805
                        else if (C_6_1 == 0)
                        {
                            this.textBox27.Text = "主抽温度正常";
                        }
                        else
                        {
                            this.textBox27.Text = "主抽温度持续偏低";
                        }

                        this.textBox28.Text = dataTable_C_1.Rows[0]["SINCAL_C_NONFUEL_ADJ"].ToString();//计算C调整量


                        #endregion
                        #region 综合配碳调整信息
                        this.textBox29.Text = dataTable_C_1.Rows[0]["SINCAL_C_BEFORE_Modify"].ToString();//修正前C调整值
                        this.textBox30.Text = dataTable_C_1.Rows[0]["SINCAL_C_BEFORE_SV_C"].ToString();//修正前混合料配料C
                        this.textBox31.Text = dataTable_C_1.Rows[0]["SINCAL_C_AFTER_Modify"].ToString();//修正后C调整值
                        this.textBox32.Text = dataTable_C_1.Rows[0]["SINCAL_C_SV_C"].ToString(); //修正后混合料配料C

                    }
                    #endregion

                    #region 颜色判断
                    if (flag_C_switch == 1)
                    {
                        label41.BackColor = Color.Coral;
                        textBox31.ForeColor = Color.Red;
                    }
                    else
                    {
                        this.tableLayoutPanel12.BackColor = Color.LightGray;
                    }
                    #endregion

                }
                #endregion

                #region Mg自动调整信息赋值判断
                string sql_mg = "select  TIMESTAMP," +
                    "isnull(SINCAL_MG_FLAG,0) as SINCAL_MG_FLAG," +
                    "isnull(SINCAL_MG_MODEL_FLAG,0) as SINCAL_MG_MODEL_FLAG," +
                    "isnull(SINCAL_MG_AIM,0) as SINCAL_MG_AIM," +
                    "isnull(SINCAL_MG_BEFORE_Modify,0) as SINCAL_MG_BEFORE_Modify," +
                    "isnull(SINCAL_MG_SV_R_BE,0) as SINCAL_MG_SV_R_BE," +
                    "isnull(SINCAL_MG_TEST,0) as SINCAL_MG_TEST," +
                    "isnull(SINCAL_MG_AFTER_Modify,0) as SINCAL_MG_AFTER_Modify," +
                    "isnull(SINCAL_MG_SV_R,0) as SINCAL_MG_SV_R from MC_SINCAL_MG_result where TIMESTAMP = (select max(TIMESTAMP) from MC_SINCAL_MG_result)";
                DataTable DataTable_mg = _dBSQL.GetCommand(sql_mg);
                //时间
                TIME_MG = DateTime.Parse(DataTable_mg.Rows[0]["TIMESTAMP"].ToString());
                //mg自动调整模型计算结果状态
                int flag_MG_calculate = int.Parse(DataTable_mg.Rows[0]["SINCAL_MG_FLAG"].ToString());
                //按钮标志位
                MG_FLAG_1 = int.Parse(DataTable_mg.Rows[0]["SINCAL_MG_FLAG"].ToString());
                //mg自动开关状态
                int flag_MG_switch = int.Parse(DataTable_mg.Rows[0]["SINCAL_MG_MODEL_FLAG"].ToString());
                //按钮标志位
                MG_FLAG_2 = int.Parse(DataTable_mg.Rows[0]["SINCAL_MG_MODEL_FLAG"].ToString());
                //SINCAL_MG_FLAG，只要是1的话显示弹出窗对应表的数据展示,则不显示
                if (flag_MG_calculate == 1)
                {
                    //烧结矿目标Mg
                    this.textBox42.Text = DataTable_mg.Rows[0]["SINCAL_MG_AIM"].ToString();
                    //修正前Mg调整值
                    this.textBox41.Text = DataTable_mg.Rows[0]["SINCAL_MG_BEFORE_Modify"].ToString();
                    //修正前配料Mg
                    this.textBox40.Text = DataTable_mg.Rows[0]["SINCAL_MG_SV_R_BE"].ToString();
                    //烧结矿检测Mg
                    this.textBox39.Text = DataTable_mg.Rows[0]["SINCAL_MG_TEST"].ToString();
                    //修正后Mg调整值
                    this.textBox38.Text = DataTable_mg.Rows[0]["SINCAL_MG_AFTER_Modify"].ToString();
                    //修正后配料Mg
                    this.textBox37.Text = DataTable_mg.Rows[0]["SINCAL_MG_SV_R"].ToString();

                    //颜色判断
                    if (flag_MG_switch == 1)
                    {
                        label49.BackColor = Color.Coral;
                        textBox38.ForeColor = Color.Red;
                        // this.tableLayoutPanel24.BackColor = Color.Coral;
                    }

                }

                #endregion

                #region R自动调整信息赋值判断
                try
                {
                    string sql_R = "select TIMESTAMP ," +
                        "SINCAL_R_FLAG," +
                        "SINCAL_R_MODEL_FLAG," +
                        "SINCAL_R_AIM," +
                        "SINCAL_R_TEST," +
                        "SINCAL_R_BEFORE_Modify," +
                        "SINCAL_R_AFTER_Modify," +
                        "SINCAL_R_SV_R_BE," +
                        "SINCAL_R_SV_R " +
                        "from MC_SINCAL_R_result where TIMESTAMP = (select max(TIMESTAMP) from MC_SINCAL_R_result)";
                    DataTable dataTable_r = _dBSQL.GetCommand(sql_R);
                    //时间
                    TIME_R = DateTime.Parse(dataTable_r.Rows[0]["TIMESTAMP"].ToString());
                    //r自动调整模型计算结果状态
                    int flag_R_calculate = int.Parse(dataTable_r.Rows[0]["SINCAL_R_FLAG"].ToString());
                    //按钮标志位
                    R_FLAG_1 = int.Parse(dataTable_r.Rows[0]["SINCAL_R_FLAG"].ToString());
                    //r自动开关状态
                    int flag_R_switch = int.Parse(dataTable_r.Rows[0]["SINCAL_R_MODEL_FLAG"].ToString());
                    //按钮标志位
                    R_FLAG_2 = int.Parse(dataTable_r.Rows[0]["SINCAL_R_MODEL_FLAG"].ToString());
                    //烧结矿目标R
                    float R_1 = float.Parse(dataTable_r.Rows[0]["SINCAL" + "_R_AIM"].ToString());

                    if (flag_R_calculate == 1)
                    {
                        this.textBox1.Text = R_1.ToString();
                        this.textBox2.Text = dataTable_r.Rows[0]["SINCAL_R_BEFORE_Modify"].ToString();//修正前R调整值
                        this.textBox3.Text = dataTable_r.Rows[0]["SINCAL_R_SV_R_BE"].ToString();//修正前配料R
                        this.textBox4.Text = dataTable_r.Rows[0]["SINCAL_R_TEST"].ToString();//烧结矿检测R
                        this.textBox5.Text = dataTable_r.Rows[0]["SINCAL_R_AFTER_Modify"].ToString();//修正后R调整值
                        this.textBox6.Text = dataTable_r.Rows[0]["SINCAL_R_SV_R"].ToString();//修正后配料R

                        if (flag_R_switch == 1)
                        {
                            label10.BackColor = Color.Coral;
                            textBox5.ForeColor = Color.Red;
                        }
                    }
                }
                catch
                {

                }
                #endregion

            }
            catch (Exception ee)
            {
                var mistake = "碱度配碳弹出框窗体初始化赋值判断错误" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }

        private void Frm_MIX_Ingredient_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            MIX_Intelligent._Auto_1 = null;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            _Method();
            this.Dispose();
            MIX_Intelligent._Auto_1 = null;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            MIX_Intelligent._Auto_1 = null;
        }
        /// <summary>
        /// 调用方法
        /// </summary>
        public void _Method()
        {
            try
            {
                var text1 = "成分调整弹出框用户点击确认按钮";
                _vLog.writelog(text1, 0);
                //修正后C调整值
                float C_adjustment = 10000;
                //修正后R调整值
                float R_adjustment = 10000;
                //修正后MG调整值
                float MG_adjustment = 10000;
                //调用条件标志
                bool FLAG = false;
                #region C
                int c = 0;
                if (C_FLAG_1 == 1 && C_FLAG_2 == 1)
                {
                    c = 1;
                    C_adjustment = float.Parse(textBox31.Text.ToString());
                }
                else if (C_FLAG_1 == 1 && C_FLAG_2 == 0)
                {

                    c = 3;
                }
                else if (C_FLAG_1 != 1 && C_FLAG_2 == 0)
                {

                    c = 4;
                }
                else if (C_FLAG_1 != 1 && C_FLAG_2 == 1)
                {
                    c = 5;
                }
                string sql_C_1 = "update MC_SINCAL_C_result  set SINCAL_FLAG = " + c + ",SINCAL_C_FLAG ='2' where  TIMESTAMP = '" + TIME_C + "' and SINCAL_C_FLAG = '1'";
                int c_count = _dBSQL.CommandExecuteNonQuery(sql_C_1);
                if (c_count > 0)
                {
                    //var text = "*****************碱度配碳弹出框点击确认按钮，更新数据库MC_SINCAL_C_result标志 SINCAL_FLAG = " + c + "SINCAL_C_FLAG = 2成功";
                    //_vLog.writelog(text, 0);
                }
                else
                {
                    var text = "**************更新数据库MC_SINCAL_C_result标志 SINCAL_FLAG = " + c + "SINCAL_C_FLAG = 2失败";
                    _vLog.writelog(text, -1);
                }
                #endregion


                int r = 0;
                #region R
                if (R_FLAG_1 == 1 && R_FLAG_2 == 1)
                {

                    r = 1;
                    R_adjustment = float.Parse(textBox5.Text.ToString());
                }
                else if (R_FLAG_1 == 1 && R_FLAG_2 == 0)
                {

                    r = 3;

                }
                else if (R_FLAG_1 != 1 && R_FLAG_2 == 0)
                {

                    r = 4;
                }
                else if (R_FLAG_1 != 1 && R_FLAG_2 == 1)
                {

                    r = 5;
                }
                string sql_R_1 = "update MC_SINCAL_R_result  set SINCAL_FLAG = " + r + ",SINCAL_R_FLAG ='2' where  TIMESTAMP = '" + TIME_R + "' and SINCAL_R_FLAG = '1'";
                int count_r = _dBSQL.CommandExecuteNonQuery(sql_R_1);
                if (count_r > 0)
                {
                    //var text = "*****************碱度配碳弹出框点击确认按钮，更新数据库MC_SINCAL_R_result标志 SINCAL_FLAG = " + r + "SINCAL_r_FLAG = 2成功";
                    //_vLog.writelog(text, 0);
                }
                else
                {
                    var text = "**************碱度配碳弹出框点击确认按钮，更新数据库MC_SINCAL_R_result标志 SINCAL_FLAG = " + r + "SINCAL_r_FLAG = 2失败";
                    _vLog.writelog(text, -1);
                }
                #endregion

                int mg = 0;
                #region MG
                if (MG_FLAG_1 == 1 && MG_FLAG_2 == 1)
                {

                    mg = 1;
                    MG_adjustment = float.Parse(textBox38.Text.ToString());
                }
                else if (MG_FLAG_1 == 1 && MG_FLAG_2 == 0)
                {
                    mg = 3;

                }
                else if (MG_FLAG_1 != 1 && MG_FLAG_2 == 0)
                {
                    mg = 4;

                }
                else if (MG_FLAG_1 != 1 && MG_FLAG_2 == 1)
                {
                    mg = 5;

                }
                string sql_MG_1 = "update MC_SINCAL_MG_result  set SINCAL_FLAG = " + mg + ",SINCAL_MG_FLAG ='2' where TIMESTAMP = '" + TIME_MG + "' and SINCAL_MG_FLAG = '1'";
                int mg_count = _dBSQL.CommandExecuteNonQuery(sql_MG_1);

                if (mg_count > 0)
                {
                    //var text = "*****************碱度配碳弹出框点击确认按钮，更新数据库MC_SINCAL_MG_result标志 SINCAL_FLAG = " + mg + "SINCAL_MG_FLAG = 2成功";
                    //_vLog.writelog(text, 0);
                }
                else
                {
                    var text = "**************碱度配碳弹出框点击确认按钮，更新数据库MC_SINCAL_MG_result标志 SINCAL_FLAG = " + mg + "SINCAL_MG_FLAG = 2失败";
                    _vLog.writelog(text, -1);
                }
                #endregion

                //判断是否符合调用条件
                if ((C_FLAG_1 == 1 && C_FLAG_2 == 1) || (R_FLAG_1 == 1 && R_FLAG_2 == 1) || (MG_FLAG_1 == 1 && MG_FLAG_2 == 1))
                {
                    FLAG = true;
                }
                //调用方法
                transfDelegate(FLAG, C_adjustment, R_adjustment, MG_adjustment);
                this.Dispose();
                MIX_Intelligent._Auto_1 = null;
            }
            catch (Exception ee)
            {
                var mistake = "*****碱度配碳弹出框报错，点击确认按钮" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
    }
}
