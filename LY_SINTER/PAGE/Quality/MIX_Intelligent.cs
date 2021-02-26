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
using VLog;
using LY_SINTER.Model;
using MIXHMICAL;
using LY_SINTER.Popover.Quality;
using System.Threading;
using LRpc;

namespace LY_SINTER.PAGE.Quality
{
    public partial class MIX_Intelligent : UserControl
    {
        /// <summary>
        /// 下发间隔
        /// </summary>
        int _time = 2000;
        #region 用户输入项设定上下限制
        //R调整
        float R_TZ_MIN = 0;
        float R_TZ_MAX = 100;
        //目标R
        float R_MB_MIN = 0;
        float R_MB_MAX = 100;
        //C调整
        float C_TZ_MIN = 0;
        float C_TZ_MAX = 100;
        //目标C
        float C_MB_MIN = 0;
        float C_MB_MAX = 100;
        //MG调整
        float MG_TZ_MIN = 0;
        float MG_TZ_MAX = 100;
        //目标MG
        float MG_MB_MIN = 0;
        float MG_MB_MAX = 100;
        //总料量sp
        float ZLL_MIN = 0;
        float ZLL_MAX = 1200;
        //燃料
        float RL_MAX = 0;
        float RL_MIN = 0;
        //溶剂
        float RJ_MAX = 0;
        float RJ_MIN = 0;
        //白云石
        float BYS_MAX = 0;
        float BYS_MIN = 0;

        #endregion
        #region 页面参数声明
        /// <summary>
        /// 设定下料量闪烁功能开始
        /// </summary>
        int COLOR_BEGIN = 0;
        /// <summary>
        /// 设定下料量闪烁功能结束
        /// </summary>
        int COLOR_END = 30;
        /// <summary>
        /// 人工输入配比上下限
        /// </summary>
        Dictionary<int, float> Rule_Dic = new Dictionary<int, float>();
        /// <summary>
        /// 熔剂燃料白云石计算模型
        ///  1：计算熔剂燃料 ；2：计算熔剂燃料白云石
        /// </summary>
        int CAL_MODE = 0;
        /// <summary>
        /// 开关标志r
        /// </summary>
        int R_MODE;
        /// <summary>
        /// 开关标志c
        /// </summary>
        int C_MODE;
        /// <summary>
        /// 开关标志FK
        /// </summary>
        int FK_MODE;
        /// <summary>
        /// 开关标志MG
        /// </summary>
        int MG_MODE;
        /// <summary>
        /// C调整值自动点击添加or减少数值
        /// </summary>
        float C_ADD;
        /// <summary>
        /// R调整值自动点击添加or减少数值
        /// </summary>
        float R_ADD;
        /// <summary>
        /// MG调整值自动点击添加or减少数值
        /// </summary>
        float MG_ADD;
        /// <summary>
        /// 总料量SP调整值自动点击添加or减少数值
        /// </summary>
        float SP_ADD;
        /// <summary>
        /// 设定配比小数位数
        /// </summary>
        int Digit_1;
        /// <summary>
        /// 设定配比%小数位数
        /// </summary>
        int Digit_2;
        /// <summary>
        /// 烧返分仓系数小数位数
        /// </summary>
        int Digit_3;
        /// <summary>
        /// 仓位小数位数
        /// </summary>
        int Digit_4;
        /// <summary>
        /// 实际下料量小数位数
        /// </summary>
        int Digit_5;
        /// <summary>
        /// 偏差小数位数
        /// </summary>
        int Digit_6;
        /// <summary>
        /// 设备转速小数位数
        /// </summary>
        int Digit_7;
        /// <summary>
        /// 湿配比小数位数
        /// </summary>
        int Digit_8;
        /// <summary>
        /// 累计值小数位数
        /// </summary>
        int Digit_9;
        /// <summary>
        /// 开关c开启
        /// </summary>
        string switch_1_open;
        /// <summary>
        /// 开关c关闭
        /// </summary>
        string switch_1_close;
        /// <summary>
        /// 开关r开启
        /// </summary>
        string switch_2_open;
        /// <summary>
        /// 开关r关闭
        /// </summary>
        string switch_2_close;
        /// <summary>
        /// 开关mg开启
        /// </summary>
        string switch_3_open;
        /// <summary>
        /// 开关mg关闭
        /// </summary>
        string switch_3_close;
        /// <summary>
        /// 开关返矿开启
        /// </summary>
        string switch_4_open;
        /// <summary>
        /// 开关返矿关闭
        /// </summary>
        string switch_4_close;
        /// <summary>
        /// D2表单数据加载响应事件
        /// </summary>
        bool FLAG_1 = false;

        #endregion
        #region 初始化触发参数声明
        /// <summary>
        /// 初始化勾选框是否触发
        /// </summary>
        bool check_signal = false;
        /// <summary>
        /// 特殊分仓系数对应下料口号
        /// </summary>
        List<int> _list_XLK = new List<int>() {15,16 };//烧返仓下料口
        bool FLAG_OUT = true;//是否允许下发
        #endregion
        /// <summary>
        /// 中控权限
        /// </summary>
        bool FALG_Oper;
        #region 定时器声明
        /// <summary>
        /// 初始化颜色变化定时器
        /// </summary>
        public System.Timers.Timer _Timer1 { get; set; }
        /// <summary>
        /// 返矿弹出框
        /// </summary>
        public System.Timers.Timer _Timer2 { get; set; }
        /// <summary>
        /// 调整值弹出框
        /// </summary>
        public System.Timers.Timer _Timer3 { get; set; }
        /// <summary>
        /// 修改烧返仓分仓系数
        /// </summary>
        public System.Timers.Timer _Timer4 { get; set; }
        /// <summary>
        /// 周期刷新预测值数据
        /// </summary>
        public System.Timers.Timer _Timer5 { get; set; }
        /// <summary>
        /// 周期刷新PLC数据
        /// </summary>
        public System.Timers.Timer _Timer6 { get; set; }
        /// <summary>
        /// 设定下料量闪烁功能
        /// </summary>
        public System.Timers.Timer _Timer7 { get; set; }
        #endregion

        #region 弹出框交互标志位
        /// <summary>
        /// 烧返弹出框
        /// </summary>
        public static Frm_MIX_SRMCAL _Auto;
        /// <summary>
        /// 成分调整弹出框
        /// </summary>
        public static Frm_MIX_Ingredient _Auto_1;
        #endregion
        DBSQL _dBSQL = new DBSQL(DataBase.ConstParameters.strCon);//连接数据库
        Message_Logging logTable = new Message_Logging();//主框架通用方法
        LGSinter HMICAL = new LGSinter();//配料模型方法
        MIX_PAGE_MODEL mIX_PAGE = new MIX_PAGE_MODEL();//配料页面方法
        public vLog _vLog { get; set; }
        public MIX_Intelligent()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Quality\\MIXHMICAL_Page\\");
            MIX_PAGE_Digit();//配置参数
            Initialize_Plan();//添加列标题
            CAL_MODE = mIX_PAGE.Initialize_CAL_MODE();//获取特殊设定配比调整方式
            MAX_MIN_VLAUES();//上下限赋值
            Button_state(CAL_MODE);//开关状态
            PBTZ_GRTDATA(0);// 初始化加载d2数据
            Button_Show();//权限控制
            latest_time(2);//最新下发时间
            import_R_C_MG(CAL_MODE);
            Counter_OutPut();//总干料量&理论产量赋值
            INIT_SP_PV();//总料量SP、PV赋值
            Ingredient();//预测成分加载数据
            Accumulated_INIT();//初始化累计天数
            TIMER_Statement();//声明定时器
            TEXT_CHANGE_PAGE();//获取调整值每次调整数据
            FLAG_1 = true;//激活表单响应事件

            this.d2.MergeColumnNames.Add("Column6");//需要合并的列名称
            this.d2.MergeColumnNames.Add("Column7");//需要合并的列名称
            this.d2.MergeColumnNames.Add("Column8");//需要合并的列名称
            this.d2.MergeColumnNames.Add("Column9");//需要合并的列名称
            this.d2.MergeColumnNames.Add("Column17");//需要合并的列名称
            this.d2.MergeByColumnName = "Column20";//合并标志位
        }
        /// <summary>
        /// 列标题添加
        /// </summary>
        public void Initialize_Plan()
        {
            this.d2.AddSpanHeader(5, 6, "烧结配比 （干）");
            this.d2.AddSpanHeader(11, 2, "水分(%)");
            this.d2.AddSpanHeader(13, 4, "下料(t/h 湿)");
        }
        /// <summary>
        /// 开关状态
        /// </summary>
        public void Button_state(int _CAL_MODE)
        {
            try
            {
                if (_CAL_MODE == 1)//计算熔剂燃料
                {
                    string sql_button_state = "select top (1) " +
                       " MAT_L2_but_c, " +
                       "MAT_L2_but_r," +
                       "MAT_L2_but_fk  " +
                       "from CFG_MAT_L2_Butsig_INTERFACE " +
                       "where TIMESTAMP = (select max(TIMESTAMP) from CFG_MAT_L2_Butsig_INTERFACE) ";
                    DataTable dataTable = _dBSQL.GetCommand(sql_button_state);
                    if (dataTable.Rows.Count > 0)
                    {
                        C_MODE = int.Parse(dataTable.Rows[0]["MAT_L2_but_c"].ToString());//C
                        R_MODE = int.Parse(dataTable.Rows[0]["MAT_L2_but_r"].ToString());//R
                        FK_MODE = int.Parse(dataTable.Rows[0]["MAT_L2_but_fk"].ToString());//返矿
                        Button_flag(1);
                    }
                }
                else if (_CAL_MODE == 2)//计算熔剂燃料白云石
                {
                    string sql_button_state = "select top (1) " +
                      "isnull(MAT_L2_but_c,0) as MAT_L2_but_c, " +
                      "isnull(MAT_L2_but_r,0) as MAT_L2_but_r," +
                      "isnull(MAT_L2_but_fk,0) as MAT_L2_but_fk ," +
                      "isnull(MAT_L2_but_mg,0) as MAT_L2_but_mg " +
                      "from CFG_MAT_L2_Butsig_INTERFACE where TIMESTAMP = (select max(TIMESTAMP) from CFG_MAT_L2_Butsig_INTERFACE)  order by TIMESTAMP desc";

                    DataTable dataTable = _dBSQL.GetCommand(sql_button_state);
                    if (dataTable.Rows.Count > 0)
                    {
                        C_MODE = int.Parse(dataTable.Rows[0]["MAT_L2_but_c"].ToString());//C
                        R_MODE = int.Parse(dataTable.Rows[0]["MAT_L2_but_r"].ToString());//R
                        FK_MODE = int.Parse(dataTable.Rows[0]["MAT_L2_but_fk"].ToString());//返矿
                        MG_MODE = int.Parse(dataTable.Rows[0]["MAT_L2_but_mg"].ToString());//换堆
                        Button_flag(2);
                    }
                }
                else
                {
                    var mistake = "Button_state()方法开关状态失败,CAL_MODE标志位未能识别";
                    _vLog.writelog(mistake, -1);
                }
            }
            catch (Exception ee)
            {
                var mistake = "Button_state()方法开关状态失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 判断开关颜色
        ///_flag =1 计算熔剂燃料，_flag = 2计算熔剂燃料白云石
        /// </summary>
        /// <param name="_flag"></param>
        public void Button_flag(int _flag)
        {
            try
            {
                #region C
                if (C_MODE == 1)
                {

                    this.Check_C.Text = switch_1_open;
                    this.Check_C.ForeColor = Color.Green;
                    check_signal = false;
                    this.Check_C.Checked = true;
                    check_signal = true;
                }
                else if (C_MODE == 0)
                {
                    this.Check_C.Text = switch_1_close;
                    this.Check_C.ForeColor = Color.Red;
                    check_signal = false;
                    this.Check_C.Checked = false;
                    check_signal = true;


                }
                else
                {
                    String mistake = "Button_flag方法判断C自动开关标志位不正确";
                    _vLog.writelog(mistake, -1);
                }
                #endregion
                #region r
                if (R_MODE == 1)
                {
                    this.Check_R.Text = switch_2_open;
                    this.Check_R.ForeColor = Color.Green;
                    check_signal = false;
                    this.Check_R.Checked = true;
                    check_signal = true;
                }
                else if (R_MODE == 0)
                {
                    this.Check_R.Text = switch_2_close;
                    this.Check_R.ForeColor = Color.Red;
                    check_signal = false;
                    this.Check_R.Checked = false;
                    check_signal = true;
                }
                else
                {
                    String mistake = "Button_flag方法判断R自动开关标志位不正确";
                    _vLog.writelog(mistake, -1);
                }
                #endregion
                #region FK
                if (FK_MODE == 1)
                {
                    this.Check_FK.Text = switch_4_open;
                    this.Check_FK.ForeColor = Color.Green;
                    check_signal = false;
                    this.Check_FK.Checked = true;
                    check_signal = true;
                }
                else
                if (FK_MODE == 0)
                {
                    this.Check_FK.Text = switch_4_close;
                    this.Check_FK.ForeColor = Color.Red;
                    check_signal = false;
                    this.Check_FK.Checked = false;
                    check_signal = true;
                }
                else
                {
                    String mistake = "Button_flag方法判断返矿自动开关标志位不正确";
                    _vLog.writelog(mistake, -1);
                }
                #endregion
                //判断调整模式
                if (_flag == 1)
                {
                    //隐藏mgo=开关
                    Check_MG.Visible = false;
                    //判断配比确认按钮是否隐藏
                    if (C_MODE == 0 && R_MODE == 0)
                    {
                        this.button2.Visible = true;
                    }
                    else
                    {
                        this.button2.Visible = false;
                    }
                }
                else
                {
                    //显示mgo=开关
                    Check_MG.Visible = true;
                    #region MGO
                    if (MG_MODE == 1)
                    {
                        this.Check_MG.Text = switch_3_open;
                        this.Check_MG.ForeColor = Color.Green;
                        check_signal = false;
                        this.Check_MG.Checked = true;
                        check_signal = true;

                    }
                    else if (MG_MODE == 0)
                    {
                        this.Check_MG.Text = switch_3_close;
                        this.Check_MG.ForeColor = Color.Red;
                        check_signal = false;
                        this.Check_MG.Checked = false;
                        check_signal = true;
                    }
                    else
                    {
                        String mistake = "Button_flag方法判断MG自动开关标志位不正确";
                        _vLog.writelog(mistake, -1);
                    }
                    #endregion
                    if (MG_MODE == 0 && C_MODE == 0 && R_MODE == 0)
                    {
                        this.button2.Visible = true;
                    }
                    else
                    {
                        this.button2.Visible = false;
                    }
                }

            }
            catch (Exception ee)
            {
                string mistake = "Button_flag方法判断按钮颜色标志位失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// R开关点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit3_Click(object sender, EventArgs e)
        {
            try
            {
                if (check_signal == true)
                {
                    string name_flag = "";
                    if (R_MODE == 0)
                    {
                        name_flag = "R开关变为自动";
                    }
                    else
                    {
                        name_flag = "R开关变为手动";
                    }
                    DialogResult resule = MessageBox.Show(name_flag, "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    string a = resule.ToString();
                    if (a == "OK")
                    {
                        string flage_R = Check_R.Text.ToString();
                        if (CAL_MODE == 1)
                        {
                            if (flage_R == switch_2_close)
                            {
                                R_MODE = 1;
                            }
                            else if (flage_R == switch_2_open)
                            {
                                R_MODE = 0;
                            }
                            var sql = "insert INTO CFG_MAT_L2_Butsig_INTERFACE (MAT_L2_but_c,MAT_L2_but_r,MAT_L2_but_fk ,TIMESTAMP) VALUES (" + C_MODE + "," + R_MODE + "," + FK_MODE + ",GETDATE())";
                            int COUNT = _dBSQL.CommandExecuteNonQuery(sql);
                            if (COUNT != 1)
                            {
                                var mistake = "r开关变化记录失败，存库失败";
                                _vLog.writelog(mistake, -1);
                                MessageBox.Show("开关切换失败");
                                return;
                            }
                            else
                            {
                                Button_flag(CAL_MODE);
                            }
                        }
                        else
                        {
                            if (flage_R == switch_2_close)
                            {
                                R_MODE = 1;
                            }
                            else if (flage_R == switch_2_open)
                            {
                                R_MODE = 0;
                            }
                            var sql = "insert INTO CFG_MAT_L2_Butsig_INTERFACE (MAT_L2_but_c,MAT_L2_but_r,MAT_L2_but_fk ,MAT_L2_but_mg,TIMESTAMP) VALUES (" + C_MODE + "," + R_MODE + "," + FK_MODE + "," + MG_MODE + ",GETDATE());";
                            int COUNT = _dBSQL.CommandExecuteNonQuery(sql);
                            if (COUNT != 1)
                            {
                                var mistake = "r开关变化记录失败，存库失败";
                                _vLog.writelog(mistake, -1);
                                MessageBox.Show("开关切换失败");
                                return;
                            }
                            else
                            {
                                Button_flag(CAL_MODE);
                            }
                        }
                     
                        string name = "改变R开关为" + Check_R.Text.ToString();
                        logTable.Operation_Log( name, "智能配料页面", "智能配料模型");
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ee)
            {
                string mistake = "r开关修改异常" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// C开关点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkEdit4_Click(object sender, EventArgs e)
        {
            try
            {
                if (check_signal == true)
                {
                    string name_flag = "";
                    if (C_MODE == 0)
                    {
                        name_flag = "C开关变为自动";
                    }
                    else
                    {
                        name_flag = "C开关变为手动";
                    }
                    DialogResult resule = MessageBox.Show(name_flag, "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    string a = resule.ToString();
                    if (a == "OK")
                    {
                        string flage_C = Check_C.Text.ToString();
                        if (CAL_MODE == 1)
                        {
                            if (flage_C == switch_1_close)
                            {
                                C_MODE = 1;
                            }
                            else if (flage_C == switch_1_open)
                            {
                                C_MODE = 0;
                            }
                            var sql = "insert INTO CFG_MAT_L2_Butsig_INTERFACE (MAT_L2_but_c,MAT_L2_but_r,MAT_L2_but_fk ,TIMESTAMP) VALUES (" + C_MODE + "," + R_MODE + "," + FK_MODE + ",GETDATE())";
                            int COUNT = _dBSQL.CommandExecuteNonQuery(sql);
                            if (COUNT != 1)
                            {
                                var mistake = "C开关变化记录失败，存库失败";
                                _vLog.writelog(mistake, -1);
                                MessageBox.Show("开关切换失败");
                                return;
                            }
                            else
                            {
                                Button_flag(CAL_MODE);
                            }
                        }
                        else
                        {
                            if (flage_C == switch_1_close)
                            {
                                C_MODE = 1;
                            }
                            else if (flage_C == switch_1_open)
                            {
                                C_MODE = 0;
                            }
                            var sql = "insert INTO CFG_MAT_L2_Butsig_INTERFACE (MAT_L2_but_c,MAT_L2_but_r,MAT_L2_but_fk ,MAT_L2_but_mg,TIMESTAMP) VALUES (" + C_MODE + "," + R_MODE + "," + FK_MODE + "," + MG_MODE + ",GETDATE());";
                            int COUNT = _dBSQL.CommandExecuteNonQuery(sql);
                            if (COUNT != 1)
                            {
                                var mistake = "C开关变化记录失败，存库失败";
                                _vLog.writelog(mistake, -1);
                                MessageBox.Show("开关切换失败");
                                return;
                            }
                            else
                            {
                                Button_flag(CAL_MODE);
                            }
                        }
                        string name = "改变C开关为" + Check_C.Text.ToString();
                        logTable.Operation_Log(name, "智能配料页面", "智能配料模型");
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ee)
            {
                string mistake = "C开关修改异常" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// MG开关点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_MG_Click(object sender, EventArgs e)
        {
            try
            {
                if (check_signal == true)
                {
                    string name_flag = "";
                    if (MG_MODE == 0)
                    {
                        name_flag = "MG开关变为自动";
                    }
                    else
                    {
                        name_flag = "MG开关变为手动";
                    }
                    DialogResult resule = MessageBox.Show(name_flag, "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    string a = resule.ToString();
                    if (a == "OK")
                    {
                        string flage_MG = Check_MG.Text.ToString();
                        if (CAL_MODE == 1)
                        {
                            if (flage_MG == switch_3_close)
                            {
                                MG_MODE = 1;
                            }
                            else if (flage_MG == switch_3_open)
                            {
                                MG_MODE = 0;
                            }
                            var sql = "insert INTO CFG_MAT_L2_Butsig_INTERFACE (MAT_L2_but_c,MAT_L2_but_r,MAT_L2_but_fk ,TIMESTAMP) VALUES (" + C_MODE + "," + R_MODE + "," + FK_MODE + ",GETDATE())";
                            int COUNT = _dBSQL.CommandExecuteNonQuery(sql);
                            if (COUNT != 1)
                            {
                                var mistake = "MG开关变化记录失败，存库失败";
                                _vLog.writelog(mistake, -1);
                                MessageBox.Show("开关切换失败");
                                return;
                            }
                            else
                            {
                                Button_flag(CAL_MODE);
                            }
                        }
                        else
                        {
                            if (flage_MG == switch_3_close)
                            {
                                MG_MODE = 1;
                            }
                            else if (flage_MG == switch_3_open)
                            {
                                MG_MODE = 0;
                            }
                            var sql = "insert INTO CFG_MAT_L2_Butsig_INTERFACE (MAT_L2_but_c,MAT_L2_but_r,MAT_L2_but_fk ,MAT_L2_but_mg,TIMESTAMP) VALUES (" + C_MODE + "," + R_MODE + "," + FK_MODE + "," + MG_MODE + ",GETDATE());";
                            int COUNT = _dBSQL.CommandExecuteNonQuery(sql);
                            if (COUNT != 1)
                            {
                                var mistake = "MG开关变化记录失败，存库失败";
                                _vLog.writelog(mistake, -1);
                                MessageBox.Show("开关切换失败");
                                return;
                            }
                            else
                            {
                                Button_flag(CAL_MODE);
                            }
                        }
                        string name = "改变MG开关为" + Check_MG.Text.ToString();
                        logTable.Operation_Log(name, "智能配料页面", "智能配料模型");
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ee)
            {
                string mistake = "r开关修改异常" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 返矿开关点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Check_FK_Click(object sender, EventArgs e)
        {
            try
            {
                if (check_signal == true)
                {
                    string name_flag = "";
                    if (FK_MODE == 0)
                    {
                        name_flag = "返矿开关变为自动";
                    }
                    else
                    {
                        name_flag = "返矿开关变为手动";
                    }
                    DialogResult resule = MessageBox.Show(name_flag, "提示信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    string a = resule.ToString();
                    if (a == "OK")
                    {
                        string flage_FK = Check_FK.Text.ToString();
                        if (CAL_MODE == 1)
                        {
                            if (flage_FK == switch_4_close)
                            {
                                FK_MODE = 1;
                            }
                            else if (flage_FK == switch_4_open)
                            {
                                FK_MODE = 0;
                            }
                            var sql = "insert INTO CFG_MAT_L2_Butsig_INTERFACE (MAT_L2_but_c,MAT_L2_but_r,MAT_L2_but_fk ,TIMESTAMP) VALUES (" + C_MODE + "," + R_MODE + "," + FK_MODE + ",GETDATE())";
                            int COUNT = _dBSQL.CommandExecuteNonQuery(sql);
                            if (COUNT != 1)
                            {
                                var mistake = "返矿开关变化记录失败，存库失败";
                                _vLog.writelog(mistake, -1);
                                MessageBox.Show("开关切换失败");
                                return;
                            }
                            else
                            {
                                Button_flag(CAL_MODE);
                            }
                        }
                        else
                        {
                            if (flage_FK == switch_4_close)
                            {
                                FK_MODE = 1;
                            }
                            else if (flage_FK == switch_4_open)
                            {
                                FK_MODE = 0;
                            }
                            var sql = "insert INTO CFG_MAT_L2_Butsig_INTERFACE (MAT_L2_but_c,MAT_L2_but_r,MAT_L2_but_fk ,MAT_L2_but_mg,TIMESTAMP) VALUES (" + C_MODE + "," + R_MODE + "," + FK_MODE + "," + MG_MODE + ",GETDATE());";
                            int COUNT = _dBSQL.CommandExecuteNonQuery(sql);
                            if (COUNT != 1)
                            {
                                var mistake = "返矿开关变化记录失败，存库失败";
                                _vLog.writelog(mistake, -1);
                                MessageBox.Show("开关切换失败");
                                return;
                            }
                            else
                            {
                                Button_flag(CAL_MODE);
                            }
                        }

                        string name = "改变返矿开关为" + Check_FK.Text.ToString();
                        logTable.Operation_Log(name, "智能配料页面", "智能配料模型");
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ee)
            {
                string mistake = "r开关修改异常" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 配比调整按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            BUTTON_PBTZ();
            if(button2.Enabled == false)
            {
                BUTTON_PBQR();
                mIX_PAGE.OVER_Storage(1, CAL_MODE);
                latest_time(2);//最新下发时间
                string name = "人工点击配比确认按钮";
                logTable.Operation_Log(name, "智能配料页面", "智能配料模型");
            }
          //  COLOR_CHANE(8);
            button1.Enabled = true;
        }
        /// <summary>
        /// 配比调整按钮调用事件
        /// </summary>
        public void BUTTON_PBTZ()
        {
            #region 判断输入项的有效性
            //C调整
            bool _f1 = logTable.JudgeOk(C_TZ_MAX, C_TZ_MIN, this.textBox_TZ_C.Text.ToString());
            if (_f1)
            {
                MessageBox.Show("C调整超限", "警号");
                string messbox = "C调整超限" + this.textBox_TZ_C.Text.ToString();
                _vLog.writelog(messbox, -1);
                return;
            }
            //目标C
            bool _f2 = logTable.JudgeOk(C_MB_MAX, C_MB_MIN, this.textBox_MB_C.Text.ToString());
            if (_f2)
            {
                MessageBox.Show("目标C超限", "警号");
                string messbox = "目标C超限" + this.textBox_MB_C.Text.ToString();
                _vLog.writelog(messbox, -1);
                return;
            }
            //目标R
            bool _f3 = logTable.JudgeOk(R_MB_MAX, R_MB_MIN, this.textBox_MB_R.Text.ToString());
            if (_f3)
            {
                MessageBox.Show("目标R超限", "警号");
                string messbox = "目标R超限" + this.textBox_MB_R.Text.ToString();
                _vLog.writelog(messbox, -1);
                return;
            }
            //R调整
            bool _f4 = logTable.JudgeOk(R_TZ_MAX, R_TZ_MIN, this.textBox_TZ_R.Text.ToString());
            if (_f4)
            {
                MessageBox.Show("R调整超限", "警号");
                string messbox = "R调整超限" + this.textBox_TZ_R.Text.ToString();
                _vLog.writelog(messbox, -1);
                return;
            }
            //mg调整
            bool _f5 = logTable.JudgeOk(MG_TZ_MAX, MG_TZ_MIN, this.textBox_TZ_MG.Text.ToString());
            if (_f5)
            {
                MessageBox.Show("MG调整超限", "警号");
                string messbox = "MG调整超限" + this.textBox_TZ_MG.Text.ToString();
                _vLog.writelog(messbox, -1);
                return;
            }
            //目标MG
            bool _f6 = logTable.JudgeOk(MG_MB_MAX, MG_MB_MIN, this.textBox_MB_MG.Text.ToString());
            if (_f6)
            {
                MessageBox.Show("目标MG超限", "警号");
                string messbox = "目标MG超限" + this.textBox_MB_MG.Text.ToString();
                _vLog.writelog(messbox, -1);
                return;
            }
            #endregion
            bool f_1 = Mix_fcxs_differ();//判断分仓系数修改更新标志位并存库
            if (f_1)
            {
                bool f_2 = Assignment_inquire();//更新设定配比
                if (f_2)
                {
                    bool f_3 = CptSolfuel_3();//调整值存库
                    if (f_3)
                    {
                        bool f_4 = mIX_PAGE.CptSolfuel_1(CAL_MODE,  RJ_MAX,  RJ_MIN,  RL_MAX,  RL_MIN,  BYS_MAX,  BYS_MIN);//判断特殊成分配比
                        HMICAL.CalPB(1);//计算当前配比%
                        PBTZ_GRTDATA(1);//更新设定配比&设定配比%数据
                    }
                }
            }
           
         
        }
        /// <summary>
        /// 配比确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.button2.Enabled = false;
            BUTTON_PBQR();
            mIX_PAGE.OVER_Storage(1, CAL_MODE);
            latest_time(2);//最新下发时间
            string name = "人工点击配比确认按钮";
            logTable.Operation_Log(name, "智能配料页面", "智能配料模型");
            this.button2.Enabled = true;
        }
        /// <summary>
        /// 下发一级
        /// Pattern = 1 直接下发设定下料量
        /// Pattern = 2 比较现场数据后下发设定下料量
        /// </summary>
        public void Issue_SDXLL(int Pattern = 1)
        {
            try
            {
                if (FLAG_OUT)
                {


                    if (true)//判断一级是否投入
                    {
                        //正式版
                        string text = "一级智能配料模型投入";
                        _vLog.writelog(text, 0);
                        List<int> list_mid = mIX_PAGE._Get_Mid();//mid************屏蔽总料量
                                                                
                        Tuple<bool, List<float>> list1 = mIX_PAGE._Get_Mid_Date();//数据************屏蔽总料量
                        Tuple<bool, List<float>> list2 = mIX_PAGE._Get_Values(4);//获取现场设定下料量
                        if (list1.Item1)
                        {
                            if (Pattern == 1 )
                            {
                                LDataSet lds = new LDataSet();
                                lds.Ip = ConstParameters.strCon_ID;//数据库地址
                                lds.Port = ConstParameters.PORT;//端口号
                                for (int count_1 = 0; count_1 < ConstParameters._COUNT_1; count_1++)//21ci 
                                {
                                    LDataUnits ldus = new LDataUnits();
                                    for (int count = 0; count < ConstParameters._COUNT; count++)//下发个数
                                    {
                                        ldus.Data[count] = new DataUnit();
                                    }

                                    for (int count = 0; count < ConstParameters._COUNT; count++)
                                    {
                                        lds.initData(ldus.Data[count], list_mid[count_1], list1.Item2[count_1]);
                                    }
                                    ldus.Count = ConstParameters._COUNT;
                                    lds.TimeOuter = _time;//下发间隔
                                    lds.SetData(ldus);

                                    int _flag = lds.Flags;
                                    if (_flag == -1)//下发失败
                                    {
                                        MessageBox.Show("下发失败");
                                    }
                                    else if (_flag == -2)//重新下发
                                    {
                                        MessageBox.Show("重新下发");
                                    }
                                    else if (_flag == 1)//下发成功
                                    {
                                        label3.BackColor = Color.Red;
                                        string messbox = "设定下料下发成功";
                                        //  _vLog.writelog(messbox, 0);
                                    }
                                }
                            }
                            else if(Pattern == 2)
                            {
                                if (list2.Item1)
                                {
                                        LDataSet lds = new LDataSet();
                                    lds.TimeOuter = 3000;//设置平台响应时间（初始2s）
                                        lds.Ip = ConstParameters.strCon_ID;//数据库地址
                                        lds.Port = ConstParameters.PORT;//端口号
                                        for (int count_1 = 0; count_1 < ConstParameters._COUNT_1; count_1++)//21ci 
                                        {
                                            if (list2.Item2[count_1] != list1.Item2[count_1])
                                            {
                                                LDataUnits ldus = new LDataUnits();
                                                for (int count = 0; count < ConstParameters._COUNT; count++)//下发个数
                                                {
                                                    ldus.Data[count] = new DataUnit();
                                                }
                                                for (int count = 0; count < ConstParameters._COUNT; count++)
                                                {
                                                    lds.initData(ldus.Data[count], list_mid[count_1], list1.Item2[count_1]);
                                                }
                                                ldus.Count = ConstParameters._COUNT;
                                                lds.TimeOuter = _time;//下发间隔
                                                lds.SetData(ldus);

                                                int _flag = lds.Flags;
                                                if (_flag == -1)//下发失败
                                                {
                                                    MessageBox.Show("下发失败");
                                                }
                                                else if (_flag == -2)//重新下发
                                                {
                                                    MessageBox.Show("重新下发");
                                                }
                                                else if (_flag == 1)//下发成功
                                                {
                                                    label3.BackColor = Color.Red;
                                                    string messbox = "设定下料下发成功";
                                                    //  _vLog.writelog(messbox, 0);
                                                }
                                            }
                                            else
                                            {
                                            string messbox = "设定下料量与现场下料量使用一致，未下发，"+ count_1.ToString();
                                            _vLog.writelog(messbox, 0);
                                        }
                                    }
     
                                }
                                else
                                {
                                    string messbox = "Issue_SDXLL方法调用_Get_Values接收数据异常，请检查数据库连接";
                                    _vLog.writelog(messbox, -1);
                                    MessageBox.Show("设定下料量下发失败", "警告");
                                }
                            }
                            
                        }
                        else
                        {
                            string messbox = "Issue_SDXLL方法调用_Get_Mid_Date接收数据异常，请检查数据库连接";
                            _vLog.writelog(messbox, -1);
                            MessageBox.Show("设定下料量下发失败","警告");
                        }
                        #region 测试
                        //List<int> list_mid1 = new List<int>();
                        //list_mid1.Add(7);
                        ////准备数据
                        //List<float> list11 = new List<float>();
                        //list11.Add(200);
                        //LDataSet lds_1 = new LDataSet();
                        //lds_1.Ip = ConstParameters.strCon_ID;//数据库地址
                        //lds_1.Port = ConstParameters.PORT;//端口号
                        //LDataUnits ldus1 = new LDataUnits();
                        //for (int count = 0; count < 1; count++)//下发个数
                        //{
                        //    ldus1.Data[count] = new DataUnit();
                        //}

                        //for (int count = 0; count < 1; count++)
                        //{
                        //    lds_1.initData(ldus1.Data[count], list_mid1[count], list11[count]);
                        //}
                        //ldus1.Count = 1;


                        //lds_1.TimeOuter = 1000;
                        //lds_1.SetData(ldus1);

                        //int _flag1 = lds_1.Flags;
                        //if (_flag1 == -1)//下发失败
                        //{
                        //    MessageBox.Show("下发失败");
                        //}
                        //else if (_flag1 == -2)//重新下发
                        //{
                        //    MessageBox.Show("重新下发");
                        //}
                        //else if (_flag1 == 1)//下发成功
                        //{
                        //    label3.BackColor = Color.Red;
                        //    string messbox = "设定下料下发成功";
                        //    _vLog.writelog(messbox, 0);
                        //}
                        //#endregion

                        //for (int x = 0; x < 20; x++)
                        //{
                        //    #region
                        //    List<int> list_mid2 = new List<int>();
                        //    list_mid2.Add(x + 3);
                        //    //准备数据
                        //    List<float> list111 = new List<float>();
                        //    list111.Add(100 + x);
                        //    LDataSet lds_2 = new LDataSet();
                        //    lds_2.Ip = ConstParameters.strCon_ID;//数据库地址
                        //    lds_2.Port = ConstParameters.PORT;//端口号
                        //    LDataUnits ldus2 = new LDataUnits();
                        //    for (int count = 0; count < 1; count++)//下发个数
                        //    {
                        //        ldus2.Data[count] = new DataUnit();
                        //    }

                        //    for (int count = 0; count < 1; count++)
                        //    {
                        //        lds_2.initData(ldus2.Data[count], list_mid2[count], list111[count]);
                        //    }
                        //    ldus2.Count = 1;


                        //    lds_2.TimeOuter = 1000;
                        //    lds_2.SetData(ldus2);

                        //    int _flag2 = lds_2.Flags;
                        //    if (_flag2 == -1)//下发失败
                        //    {
                        //        //   MessageBox.Show("下发失败");
                        //    }
                        //    else if (_flag2 == -2)//重新下发
                        //    {
                        //        //   MessageBox.Show("重新下发");
                        //    }
                        //    else if (_flag2 == 1)//下发成功
                        //    {
                        //        label3.BackColor = Color.Red;
                        //        string messbox = "设定下料下发成功";
                        //        _vLog.writelog(messbox, 0);
                        //    }
                        //}
                        // #endregion
                    }

                    //#region 测试Soccomm
                    //if (true)//判断一级是否投入
                    //{
                    //    string text = "一级智能配料模型投入";
                    //    _vLog.writelog(text, 0);
                    //    List<int> list_mid = mIX_PAGE._Get_Mid();//mid
                    //                                             //准备数据
                    //    Tuple<bool, List<float>> list1 = mIX_PAGE._Get_Mid_Date();//数据
                    //    float a = 1;

                    //    #region 测试
                    //    for (int x = 0; x < 10000; x++)
                    //    {
                    //        for (int y = 0; y < list1.Item2.Count; y++)
                    //        {
                    //            list1.Item2[y] = list1.Item2[y] + a;
                    //        }
                    //        if (list1.Item1)
                    //        {
                    //            LDataSet lds = new LDataSet();
                    //            lds.Ip = ConstParameters.strCon_ID;//数据库地址
                    //            lds.Port = ConstParameters.PORT;//端口号
                    //            LDataUnits ldus = new LDataUnits();
                    //            for (int count = 0; count < ConstParameters._COUNT; count++)//下发个数
                    //            {
                    //                ldus.Data[count] = new DataUnit();
                    //            }

                    //            for (int count = 0; count < ConstParameters._COUNT; count++)
                    //            {
                    //                lds.initData(ldus.Data[count], list_mid[count], list1.Item2[count]);
                    //            }
                    //            ldus.Count = ConstParameters._COUNT;


                    //            lds.TimeOuter = 3000;
                    //            lds.SetData(ldus);

                    //            int _flag = lds.Flags;
                    //            if (_flag == -1)//下发失败
                    //            {
                    //                MessageBox.Show("下发失败");
                    //            }
                    //            else if (_flag == -2)//重新下发
                    //            {
                    //                MessageBox.Show("重新下发");
                    //            }
                    //            else if (_flag == 1)//下发成功
                    //            {
                    //                label3.BackColor = Color.Red;
                    //                string messbox = "设定下料下发成功";
                    //                _vLog.writelog(messbox, 0);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            string messbox = "Issue_SDXLL方法接收数据异常，请检查数据库连接";
                    //            _vLog.writelog(messbox, -1);
                    //        }
                    //    }
                    //}
                    //    #endregion
                    //    //        if (list1.Item1)
                    //    //    {
                    //    //        LDataSet lds = new LDataSet();
                    //    //        lds.Ip = ConstParameters.strCon_ID;//数据库地址
                    //    //        lds.Port = ConstParameters.PORT;//端口号
                    //    //        LDataUnits ldus = new LDataUnits();
                    //    //        for (int count = 0; count < ConstParameters._COUNT; count++)//下发个数
                    //    //        {
                    //    //            ldus.Data[count] = new DataUnit();
                    //    //        }

                    //    //        for (int count = 0; count < ConstParameters._COUNT; count++)
                    //    //        {
                    //    //            lds.initData(ldus.Data[count], list_mid[count], list1.Item2[count]);
                    //    //        }
                    //    //        ldus.Count = ConstParameters._COUNT;


                    //    //        lds.TimeOuter = 2000;
                    //    //        lds.SetData(ldus);

                    //    //        int _flag = lds.Flags;
                    //    //        if (_flag == -1)//下发失败
                    //    //        {
                    //    //            MessageBox.Show("下发失败");
                    //    //        }
                    //    //        else if (_flag == -2)//重新下发
                    //    //        {
                    //    //            MessageBox.Show("重新下发");
                    //    //        }
                    //    //        else if (_flag == 1)//下发成功
                    //    //        {
                    //    //            label3.BackColor = Color.Red;
                    //    //            string messbox = "设定下料下发成功";
                    //    //            _vLog.writelog(messbox, 0);
                    //    //        }
                    //    //    }
                    //    //    else
                    //    //    {
                    //    //        string messbox = "Issue_SDXLL方法接收数据异常，请检查数据库连接";
                    //    //        _vLog.writelog(messbox, -1);
                    //    //    }













                    //    //}
                    //    #endregion


                    //try
                    //{
                    //    //判断一级开关是否为智能投入状态
                    //    var flag_sql = "select MAT_PLC_BUTTON_3S from C_PLC_3S where TIMESTAMP = (select MAX(TIMESTAMP) from C_PLC_3S)";
                    //    DataTable data1 = _dBSQL.GetCommand(flag_sql);
                    //    if (data1 != null && data1.Rows.Count > 0)
                    //    {
                    //        int flag = int.Parse(data1.Rows[0][0].ToString() == "" ? "0" : data1.Rows[0][0].ToString());
                    //        if (flag == 1)
                    //        {
                    //            string text = "一级智能配料模型投入";
                    //            _vLog.writelog(text, 0);
                    //            List<int> list_mid = new List<int>();
                    //            //设定下料量、总料量sp  mid
                    //            int a = 174;
                    //            for (int X = 0; X < 20; X++)
                    //            {
                    //                list_mid.Add(a + X);
                    //            }
                    //            //下料比例、水分、总干料量
                    //            int b = 1266;
                    //            for (int X = 0; X < 39; X++)
                    //            {
                    //                list_mid.Add(b + X);
                    //            }
                    //            //准备数据
                    //            List<float> list1 = new List<float>();
                    //            string sql = "select  MAT_L2_GXLBL, MAT_L2_SFDQ, MAT_L2_SDXL   from CFG_MAT_L2_XLK_INTERFACE a,  CFG_MAT_L2_SJPB_INTERFACE b   where a.MAT_L2_CH = b.MAT_L2_CH  ORDER BY A.MAT_L2_XLK ASC";
                    //            DataTable data = _dBSQL.GetCommand(sql);
                    //            if (data != null && data.Rows.Count > 0)
                    //            {
                    //                //设定下料量
                    //                for (int x = 0; x < data.Rows.Count; x++)
                    //                {
                    //                    list1.Add(float.Parse(data.Rows[x]["MAT_L2_SDXL"].ToString() == "" ? "0" : data.Rows[x]["MAT_L2_SDXL"].ToString()));
                    //                }
                    //                //总料量sp
                    //                if (textBox_SP.Text.ToString() == "")
                    //                {
                    //                    list1.Add(0);

                    //                }
                    //                else
                    //                {
                    //                    list1.Add(float.Parse(textBox_SP.Text.ToString()));

                    //                }
                    //                //下料比例
                    //                for (int x = 0; x < data.Rows.Count; x++)
                    //                {
                    //                    list1.Add(float.Parse(data.Rows[x]["MAT_L2_GXLBL"].ToString() == "" ? "0" : data.Rows[x]["MAT_L2_GXLBL"].ToString()));
                    //                }
                    //                //水分
                    //                for (int x = 0; x < data.Rows.Count; x++)
                    //                {
                    //                    list1.Add(float.Parse(data.Rows[x]["MAT_L2_SFDQ"].ToString() == "" ? "0" : data.Rows[x]["MAT_L2_SFDQ"].ToString()));
                    //                }

                    //                //总干料量
                    //                if (textBox_ZGLL.Text.ToString() == "")
                    //                {
                    //                    list1.Add(0);
                    //                }
                    //                else
                    //                {
                    //                    list1.Add(float.Parse(textBox_ZGLL.Text.ToString()));

                    //                }

                    //                LDataSet lds = new LDataSet();
                    //                lds.Ip = ConstParameters.strCon_ID;//数据库地址
                    //                lds.Port = ConstParameters.PORT;//端口号
                    //                LDataUnits ldus = new LDataUnits();
                    //                for (int count = 0; count < ConstParameters._COUNT; count++)//下发个数
                    //                {
                    //                    ldus.Data[count] = new DataUnit();
                    //                }

                    //                for (int count = 0; count < ConstParameters._COUNT; count++)
                    //                {
                    //                    lds.initData(ldus.Data[count], list_mid[count], list1[count]);
                    //                }
                    //                ldus.Count = ConstParameters._COUNT;


                    //                lds.TimeOuter = 2000;
                    //                lds.SetData(ldus);

                    //                int _flag = lds.Flags;
                    //                if (_flag == -1)//下发失败
                    //                {
                    //                    MessageBox.Show("下发失败");
                    //                }
                    //                else if (_flag == -2)//重新下发
                    //                {
                    //                    MessageBox.Show("重新下发");
                    //                }
                    //                else if (_flag == 1)//下发成功
                    //                {
                    //                    label3.BackColor = Color.Red;
                    //                    string messbox = "设定下料下发成功";
                    //                    _vLog.writelog(messbox, 0);
                    //                }
                    //            }
                    //            else
                    //            {
                    //                string mistake = "下发功能，查询数据失败" + sql;
                    //                _vLog.writelog(mistake, -1);
                    //            }

                    //        }
                    //        else
                    //        {
                    //            string text = "一级智能配料模型退出";
                    //            _vLog.writelog(text, 0);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        var mistake = "判断一级开关是否为智能投入状态查询失败" + flag_sql;
                    //        _vLog.writelog(mistake, -1);
                    //    }
                    //    latest_time(2);
                    //}
                    //catch (Exception ee)
                    //{
                    //    string mistake = "设定下料量计算失败" + ee.ToString();
                    //    _vLog.writelog(mistake, -1);
                    //}
                    #endregion
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("Issue_SDXLL方法失败" + ee.ToString(),-1);
                MessageBox.Show("警告,下发一级数据失败！！！");
            }

           
        }
        /// <summary>
        /// 配比确认按钮调用事件
        /// </summary>
        public void BUTTON_PBQR()
        {
            try
            {
                bool _f7 = logTable.JudgeOk(ZLL_MAX, ZLL_MIN, this.textBox_SP.Text.ToString());//判断总料量sp是否超限
                if (_f7)
                {
                    MessageBox.Show("总料量SP超限", "警号");
                    string messbox = "总料量SP超限" + this.textBox_SP.Text.ToString();
                    _vLog.writelog(messbox, -1);
                    return;
                }
                HMICAL.CalPB(2);
                bool _f8 =  assignment_1();//设定水分总料量存库
                if (_f8)
                {
                    bool _f9 = Warehousing(_list_XLK);//分仓系数存库
                    if (_f9)
                    {
                        bool _f10 = FeedBLCompute_1();//计算下料比例
                        if (_f10)
                        {
                           bool _f11 =  total_holdup();//计算设定下料量
                            if (_f11)
                            {
                                Dictionary<int, float> _A1 = mIX_PAGE.Calculate_SPB();//湿配比计算
                                if (_A1 != null)
                                {
                                    foreach (var x in _A1)
                                    {
                                        //湿配比存库
                                        string sql = "update CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_SPB = " + x.Value + " where MAT_PB_ID = " + x.Key + "";
                                        int _count = _dBSQL.CommandExecuteNonQuery(sql);
                                        if (_count <= 0)
                                            _vLog.writelog("_Timer4_Tick定时器计算湿配比存库有误" + sql, -1);
                                    }
                                    Issue_SDXLL();//下发
                                    PBTZ_GRTDATA(2);//页面刷新

                                    SP_PV();//计算设定值
                                    Ingredient();//更新设定值
                                                 //  COLOR_CHANE(9);//设定下料量背景颜色变化
                                    COLOR_BEGIN = 0;//还原颜色次数
                                    _Timer7.Enabled = true;//设定下料量背景颜色变化
                                    COLOR_CHANE(1);//启停信号颜色变化
                                }
                                    
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                var mistake = "BUTTON_PBQR方法配比确认调用失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 上下限赋值
        /// </summary>
        public void MAX_MIN_VLAUES()
        {
            MIX_PAGE_MODEL mIX_PAGE_ = new MIX_PAGE_MODEL();
            Tuple<bool, List<float>> _tuple = mIX_PAGE_.Judge_Val();
            if (_tuple.Item1)
            {
                //R调整
                 R_TZ_MIN = _tuple.Item2[0];
                 R_TZ_MAX = _tuple.Item2[1];
                //目标R
                R_MB_MIN = _tuple.Item2[2];
                R_MB_MAX = _tuple.Item2[3];
                //C调整
                C_TZ_MIN = _tuple.Item2[4];
                C_TZ_MAX = _tuple.Item2[5];
                //目标C
                C_MB_MIN = _tuple.Item2[6];
                C_MB_MAX = _tuple.Item2[7];
                //MG调整
                MG_TZ_MIN = _tuple.Item2[8];
                MG_TZ_MAX = _tuple.Item2[9];
                //目标MG
                MG_MB_MIN = _tuple.Item2[10];
                MG_MB_MAX = _tuple.Item2[11];
                //总料量sp
                ZLL_MIN = _tuple.Item2[12];
                ZLL_MAX = _tuple.Item2[13];
                //燃料
                RL_MIN = _tuple.Item2[14];
                RL_MAX = _tuple.Item2[15];
                //溶剂
                RJ_MIN = _tuple.Item2[16];
                RJ_MAX = _tuple.Item2[17];
                //白云石
                BYS_MIN = _tuple.Item2[18];
                BYS_MAX = _tuple.Item2[19];
            }
            else
            {
                var mistake = "MAX_MIN_VLAUES()方法上下限赋值返回错误";
                _vLog.writelog(mistake,-1);
            }
        }
        /// <summary>
        /// 设定配比存库
        /// </summary>
        public bool Assignment_inquire()
        {
            try
            {
                for (int x = 0; x < d2.Rows.Count; x++)
                {
                    //设定配比值
                    float PB = float.Parse(d2.Rows[x].Cells["Column6"].Value.ToString());
                    //配比id
                    int PBID = int.Parse(d2.Rows[x].Cells["Column20"].Value.ToString());
                    string sql_PBTZ = "update CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_SDPB = " + PB + " where  MAT_PB_ID = " + PBID + "";
                    int count = _dBSQL.CommandExecuteNonQuery(sql_PBTZ);
                    if (count > 0)
                    {
                        string messbox = "点击配比调整按钮，更新设定配比，配比ID : + '" + PBID + "'," + "配比值为'" + PB + "'";
                        _vLog.writelog(messbox, 0);
                    }
                    else
                    {
                        string messbox = "点击配比调整按钮，更新设定配比失败，配比ID : + '" + PBID + "'," + "配比值为'" + PB + "'";
                        _vLog.writelog(messbox, -1);
                    }
                    string sql_PBTZ_1 = "update CFG_MAT_L2_PBSD_INTERFACE set peibizhi = " + PB + " where  peinimingcheng = " + PBID + "";
                    int count1= _dBSQL.CommandExecuteNonQuery(sql_PBTZ_1);
                    if (count <= 0)
                    {
                        string messbox = "点击配比调整按钮，更新CFG_MAT_L2_PBSD_INTERFACE表设定配比失败，配比ID : + '" + PBID + "'," + "配比值为'" + PB + "'";
                        _vLog.writelog(messbox, -1);
                    }
                }
                return true;
            }
            catch (Exception ee)
            {
                string messbox = "点击配比调整按钮，更新设定配比失败" + ee.ToString();
                _vLog.writelog(messbox, -1);
                return false;
            }
           
        }
        /// <summary>
        /// 判断是否人为修改分仓系数
        /// </summary>
        public bool Mix_fcxs_differ()
        {
            try
            {
                //数据库存储字典 key:配比id item1：下料口 item2：分仓系数
                Dictionary<int, List<Tuple<int, double>>> FCXS_Change = new Dictionary<int, List<Tuple<int, double>>>();
                //获取数据库中的分仓系数
                var sql_date = " select MAT_PB_ID, MAT_L2_FCXS,MAT_L2_XLK  from CFG_MAT_L2_XLK_INTERFACE order by MAT_L2_XLK asc";
                DataTable data_1 = _dBSQL.GetCommand(sql_date);
                if (data_1 != null && data_1.Rows.Count > 0)
                {
                    for (int x = 0; x < data_1.Rows.Count; x++)
                    {
                        List<Tuple<int, double>> list_1 = new List<Tuple<int, double>>();
                        int PBID = int.Parse(data_1.Rows[x]["MAT_PB_ID"].ToString() == "" ? "0" : data_1.Rows[x][0].ToString());
                        double FCXS = Math.Round(double.Parse(data_1.Rows[x]["MAT_L2_FCXS"].ToString() == "" ? "0" : data_1.Rows[x][1].ToString()), 3);
                        int xlk = int.Parse(data_1.Rows[x]["MAT_L2_XLK"].ToString() == "" ? "0" : data_1.Rows[x][0].ToString());
                        if (FCXS_Change.ContainsKey(PBID))
                        {
                            FCXS_Change[PBID].Add(new Tuple<int, double>(xlk, FCXS));
                        }
                        else
                        {
                            Tuple<int, double> tuple1 = new Tuple<int, double>(xlk, FCXS);
                            list_1.Add(tuple1);
                            FCXS_Change.Add(PBID, list_1);
                        }

                    }
                    //页面存储字典
                    Dictionary<int, List<Tuple<int, double>>> FCXS_Change_YM = new Dictionary<int, List<Tuple<int, double>>>();
                    for (int aa = 0; aa < d2.Rows.Count; aa++)
                    {
                        List<Tuple<int, double>> list_1 = new List<Tuple<int, double>>();
                        int PBID = int.Parse(d2.Rows[aa].Cells["Column20"].Value.ToString() == "" ? "0" : d2.Rows[aa].Cells["Column20"].Value.ToString());
                        double FCXS_YM = Math.Round(double.Parse(d2.Rows[aa].Cells["Column10"].Value.ToString()), 3);
                        int xlk = int.Parse(d2.Rows[aa].Cells["Column3"].Value.ToString() == "" ? "0" : d2.Rows[aa].Cells["Column3"].Value.ToString());
                        if (FCXS_Change_YM.ContainsKey(PBID))
                        {
                            FCXS_Change_YM[PBID].Add(new Tuple<int, double>(xlk, FCXS_YM));
                        }
                        else
                        {
                            list_1.Add(new Tuple<int, double>(xlk, FCXS_YM));
                            FCXS_Change_YM.Add(PBID, list_1);
                        }

                    }
                    //分仓系数根据配比id判读是否发生变化 0未发生变化 1发生变化
                    Dictionary<int, int> FCXS_Change_1 = new Dictionary<int, int>();
                    for (int x = 1; x <= FCXS_Change.Count; x++)
                    {
                        List<Tuple<int, double>> list_1 = FCXS_Change[x];
                        List<Tuple<int, double>> list_2 = FCXS_Change_YM[x];
                        int aa = 0;
                        for (int a = 0; a < list_1.Count; a++)
                        {
                            if (list_1[a].Item2 != list_2[a].Item2)
                            {
                                aa = aa + 1;
                            }
                        }
                        if (aa == 0)
                        {
                            var sql_change = "update CFG_MAT_L2_XLK_INTERFACE set MAT_PB_FALG = 0 where MAT_PB_ID = " + x + "";
                            int count = _dBSQL.CommandExecuteNonQuery(sql_change);
                            string text = "配比id：" + x.ToString() + "分仓系数未发生变化";
                            if (count > 0)
                            {
                                text += "CFG_MAT_L2_XLK_INTERFACE表标志位修改成功,影响行数" + count.ToString();
                                _vLog.writelog(text, 0);
                            }
                            else
                            {
                                text += "CFG_MAT_L2_XLK_INTERFACE表标志位修改失败,影响行数" + count.ToString() + "sql:" + sql_change;
                                _vLog.writelog(text, -1);
                            }
                        }
                        else
                        {
                            #region 插入标志位
                            var sql_change = "update CFG_MAT_L2_XLK_INTERFACE set MAT_PB_FALG = 1 where MAT_PB_ID = " + x + "";
                            int count = _dBSQL.CommandExecuteNonQuery(sql_change);
                            string text = "配比id：" + x.ToString() + "分仓系数发生变化";
                            if (count > 0)
                            {
                                text += "CFG_MAT_L2_XLK_INTERFACE表标志位修改成功,影响行数" + count.ToString();
                                _vLog.writelog(text, 0);
                            }
                            else
                            {
                                text += "CFG_MAT_L2_XLK_INTERFACE表标志位修改失败,影响行数" + count.ToString() + "sql:" + sql_change;
                                _vLog.writelog(text, -1);
                            }
                            #endregion
                            #region 更新分仓系数
                            for (int x1 = 0; x1 < list_2.Count; x1++)
                            {
                                //更新修改后的分仓系数
                                var sql1 = "update CFG_MAT_L2_XLK_INTERFACE set MAT_L2_FCXS = " + list_2[x1].Item2 + " where MAT_L2_XLK = " + list_2[x1].Item1 + "";
                                int count1 = _dBSQL.CommandExecuteNonQuery(sql1);
                                if (count1 > 0)
                                {
                                    string text1 = "下料口号：" + list_2[x1].Item1.ToString() + "修改分仓系数成功，分仓系数为:" + list_2[x1].Item2.ToString();
                                    _vLog.writelog(text1, 0);
                                }
                                else
                                {
                                    string text1 = "数据库连接失败，下料口号：" + list_2[x1].Item1.ToString() + "修改分仓系数失败，分仓系数为:" + list_2[x1].Item2.ToString() + "   sql:" + sql1;
                                    _vLog.writelog(text1, -1);
                                }
                            }
                            #endregion
                        }
                    }
                    return true;
                }
                else
                {
                    string mess = "配比调整，判断分仓系数是否为人修改报错：查询数据库失败" + sql_date;
                    _vLog.writelog(mess, -1);
                    return false;
                }
            }
            catch (Exception ee)
            {
                string mistake = "判断是否人为修改分仓系数报错" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return false;
            }
        }
        /// <summary>
        /// 目标碱度、目标含碳、碳调整值、R调整值存库
        /// </summary>
        public bool CptSolfuel_3()
        {

            try
            {
                int ID = 1;
                float MBHT = float.Parse(this.textBox_MB_C.Text.ToString());//目标碳
                float TTZZ = float.Parse(this.textBox_TZ_C.Text.ToString());//碳调整值
                float MBJD = float.Parse(this.textBox_MB_R.Text.ToString());//目标碱度
                float RTZZ = float.Parse(this.textBox_TZ_R.Text.ToString());//碱度调整值
                float MBMG = float.Parse(this.textBox_MB_MG.Text.ToString());//目标MG
                float MGZZ = float.Parse(this.textBox_TZ_MG.Text.ToString());//MG调整值
                string sql = "INSERT INTO CFG_MAT_L2_MACAL_IDT (id ,C_Aim,C_Md,R_Aim,R_Md,MG_Aim, MG_Md,TIMESTAMP) VALUES ('" + ID + "','" + MBHT + "','" + TTZZ + "','" + MBJD + "','" + RTZZ + "','" + MBMG + "','" + MGZZ + "',GETDATE());";
                int count = _dBSQL.CommandExecuteNonQuery(sql);
                if (count > 0)
                {
                    string messbox = "人工输入项存库成功,目标碳:" + MBHT.ToString() + ",碳调整值:" + TTZZ.ToString() + ",目标碱度:" + MBJD.ToString() + ",碱度调整值:" + RTZZ.ToString() + ",目标MG:" + MBMG.ToString() + ",MG调整值:" + MGZZ.ToString();
                    _vLog.writelog(messbox, 0);
                    return true;
                }
                else
                {
                    string messbox = "人工输入项存库失败,目标碳:" + MBHT.ToString() + ",碳调整值:" + TTZZ.ToString() + ",目标碱度:" + MBJD.ToString() + ",碱度调整值:" + RTZZ.ToString() + ",目标MG:" + MBMG.ToString() + ",MG调整值:" + MGZZ.ToString() + "     sql语句" + sql.ToString();
                    _vLog.writelog(messbox, -1);
                    return false;
                }
            }
            catch (Exception ee)
            {
                string mistake = "目标碱度、目标含碳、碳调整值、R调整值存库、目标mg、mg调整值存库失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return false;
            }
        }
        /// <summary>
        /// 更新数据
        /// _flag = 0 初始化加载
        /// _flag = 1 更新设定配比、设定配比%
        /// _flag = 2 点击配比确认按钮更新数据
        /// </summary>
        public void PBTZ_GRTDATA(int _flag)
        {
            try
            {
                var sql = " select " +
                    "b.MAT_L2_CH ," +
                    "d.MAT_DESC as WL ," +
                    "a.MAT_L2_XLK," +
                    "(case when a.MAT_L2_XLKZT = 0 then '禁用' when a.MAT_L2_XLKZT = 1 then '启用' end ) as MAT_L2_XLKZT," +
                    "b.MAT_L2_CW ," +
                    "cast(cast(b.MAT_L2_SDPB as decimal(18,"+ Digit_1 + ")) as VARCHAR(8)) as MAT_L2_SDPB," +
                    "cast(b.MAT_L2_SDBFB as decimal(18," + Digit_2 + ")) as MAT_L2_SDBFB ," +
                    "cast(cast(b.MAT_L2_DQPB as decimal(18," + Digit_1 + ")) as VARCHAR(8)) as MAT_L2_DQPB," +
                    "cast(cast(b.MAT_L2_DQBFB as decimal(18," + Digit_2 + ")) as VARCHAR(8)) as MAT_L2_DQBFB," +
                    "a.MAT_L2_FCXS," +
                    "a.MAT_L2_GXLBL," +
                    "b.MAT_L2_SFSD," +
                    "b.MAT_L2_SFDQ," +
                    "a.MAT_L2_SDXL," +
                    "a.MAT_L2_SJXL," +
                    "a.MAT_L2_PC," +
                    "cast(cast(b.MAT_L2_SPB as decimal(18," + Digit_2 + ")) as VARCHAR(8)) as MAT_L2_SPB," +
                    "a.MAT_L2_ZS ," +
                    "b.MAT_PB_ID," +
                    "e.MAT_L2_WLXH  " +
                    "from " +
                    "CFG_MAT_L2_XLK_INTERFACE a," +
                    " CFG_MAT_L2_SJPB_INTERFACE b," +
                    " M_MATERIAL_BINS c ," +
                    "M_MATERIAL_COOD d ," +
                    "CFG_MAT_L2_LJJS_INTERFACE e " +
                    "where   a.MAT_L2_CH = b.MAT_L2_CH and  c.L2_CODE  = d.L2_CODE and c.BIN_NUM_SHOW = b.MAT_L2_CH and a.MAT_L2_CH = e.MAT_CH ORDER BY A.MAT_L2_XLK ASC";
                DataTable dataTable = _dBSQL.GetCommand(sql);
                if (dataTable.Rows.Count >= 0)
                {
                    FLAG_1 = false;
                    if (_flag == 0)
                    {
                        this.d2.DataSource = dataTable;
                    }
                    else if(_flag == 1)
                    {
                        for (int X = 0; X < d2.Rows.Count; X++)
                        {
                            d2.Rows[X].Cells["Column6"].Value = dataTable.Rows[X]["MAT_L2_SDPB"].ToString();
                            d2.Rows[X].Cells["Column7"].Value = dataTable.Rows[X]["MAT_L2_SDBFB"].ToString();
                        }
                    }
                    else if (_flag == 2)
                    {
                        for (int X = 0; X < d2.Rows.Count; X++)
                        {
                            //料种
                            d2.Rows[X].Cells["Column2"].Value = dataTable.Rows[X]["WL"].ToString();
                            //启停状态
                            d2.Rows[X].Cells["Column4"].Value = dataTable.Rows[X]["MAT_L2_XLKZT"].ToString();
                            //设定配比
                            d2.Rows[X].Cells["Column6"].Value = dataTable.Rows[X]["MAT_L2_SDPB"].ToString();
                            //设定配比%
                            d2.Rows[X].Cells["Column7"].Value = dataTable.Rows[X]["MAT_L2_SDBFB"].ToString();
                            //当前配比
                            d2.Rows[X].Cells["Column8"].Value = dataTable.Rows[X]["MAT_L2_DQPB"].ToString();
                            //当前配比%
                            d2.Rows[X].Cells["Column9"].Value = dataTable.Rows[X]["MAT_L2_DQBFB"].ToString();
                            //分仓系数
                            d2.Rows[X].Cells["Column10"].Value = dataTable.Rows[X]["MAT_L2_FCXS"].ToString();
                            //下料比例
                            d2.Rows[X].Cells["Column11"].Value = dataTable.Rows[X]["MAT_L2_GXLBL"].ToString();
                            //水分设定
                            d2.Rows[X].Cells["Column12"].Value = dataTable.Rows[X]["MAT_L2_SFSD"].ToString();
                            //水分当前
                            d2.Rows[X].Cells["Column13"].Value = dataTable.Rows[X]["MAT_L2_SFDQ"].ToString();
                            //设定下料量
                            d2.Rows[X].Cells["Column14"].Value = dataTable.Rows[X]["MAT_L2_SDXL"].ToString();
                            //湿配比
                            d2.Rows[X].Cells["Column17"].Value = dataTable.Rows[X]["MAT_L2_SPB"].ToString();

                        }
                            
                    }
                    FLAG_1 = true;
                }
                else
                {
                    string mistake = "配比调整更新页面数据有误,sql:" + sql;
                    _vLog.writelog(mistake, -1);
                }
               
            }
            catch (Exception ee)
            {
                string mistake = "配比调整更新页面数据有误" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }

        }
        /// <summary>
        /// 下发时间
        /// </summary>
        /// <param name="flag"></param>
        public void latest_time(int flag)
        {
            try
            {
                if (flag == 1)
                {
                    string sql = "select top (1) timestamp from LogTable where info = '人工点击配比确认按钮'  order by TIMESTAMP desc";
                    DataTable dataTable = _dBSQL.GetCommand(sql);
                    if (dataTable.Rows.Count > 0 && dataTable != null)
                    {
                        string time = dataTable.Rows[0][0].ToString();
                        this.label3.Text = "最新下发时间：" + time;
                    }
                }
                else
                {
                    this.label3.Text = "最新下发时间：" + DateTime.Now;
                }
            }
            catch (Exception ee)
            {
                string mistake = "latest_time方法更新最新调整时间失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 颜色变化设置
        /// _FLAG = 1 启停信号字体颜色变化
        /// _FLAG = 2 特殊数据背景颜色变化
        /// _FLAG = 3 设定下料量字体颜色变化
        /// _FLAG = 4 特殊设定配比字体颜色变化
        /// _FLAG = 5 预测成分值特殊成分颜色字体变化
        /// _FLAG = 6 预测成分值L3背景颜色变化
        /// _FLAG = 7 特殊分仓系数背景颜色变化
        /// _FLAG = 8 特殊配比计算颜色闪烁
        /// _FLAG = 9 设定下料量背景颜色变化
        /// </summary>
        /// <param name="_FLAG"></param>
        public void COLOR_CHANE(int _FLAG)
        {
            try
            {
                if (_FLAG == 1)//启停信号变化
                {
                    for (int i = 0; i < d2.Rows.Count; i++)
                    {
                        string a = d2.Rows[i].Cells["Column4"].Value.ToString();
                        if (a == "禁用")
                        {
                            this.d2.Rows[i].Cells["Column4"].Style.ForeColor = Color.Red;
                        }
                        else
                        {
                            this.d2.Rows[i].Cells["Column4"].Style.ForeColor = Color.Green;
                        }
                    }
                }
                else if (_FLAG == 2)//特殊配比颜色变化
                {
                    //L1采集数据
                    this.d2.Columns["Column4"].DefaultCellStyle.BackColor = Color.Cornsilk;//启停信号背景颜色
                    this.d2.Columns["Column5"].DefaultCellStyle.BackColor = Color.Cornsilk;//仓位背景颜色
                    this.d2.Columns["Column10"].DefaultCellStyle.BackColor = Color.Cornsilk;//分仓系数背景颜色
                    this.d2.Columns["Column15"].DefaultCellStyle.BackColor = Color.Cornsilk;//实际下料量背景颜色
                    this.d2.Columns["Column18"].DefaultCellStyle.BackColor = Color.Cornsilk;//设备转速背景颜色
                    //输入数据
                    this.d2.Columns["Column6"].DefaultCellStyle.BackColor = Color.PaleGreen;//设定配比
                 //   this.d2.Columns["Column6"].DefaultCellStyle.ForeColor = Color.Black;//设定配比
                    this.d2.Columns["Column12"].DefaultCellStyle.BackColor = Color.PaleGreen;//设定水分
                }
                else if (_FLAG == 3) //设定下料量字体颜色变化
                {
                    this.d2.Columns["Column14"].DefaultCellStyle.ForeColor = Color.Red;
                    this.d2.Columns["Column14"].DefaultCellStyle.BackColor = Color.White;
                }
                else if (_FLAG == 4)//特殊设定配比字体颜色变化
                {
                    string sql_1 = "";
                    if (CAL_MODE == 1)
                    {
                        sql_1 = " select distinct category ,canghao from CFG_MAT_L2_PBSD_INTERFACE  where category  =1 or category = 2";
                    }
                    else if (CAL_MODE == 2)
                    {
                        sql_1 = " select distinct category ,canghao from CFG_MAT_L2_PBSD_INTERFACE  where category  =1 or category = 2 or category = 4 ";
                    }
                    DataTable dataTable_1 = _dBSQL.GetCommand(sql_1);
                    List<int> _a = new List<int>();
                    for (int x = 0; x < dataTable_1.Rows.Count; x++)
                    {
                        _a.Add(int.Parse(dataTable_1.Rows[x]["canghao"].ToString()));
                    }
                        for (int xx = 0; xx < d2.Rows.Count; xx++)
                        {
                       
                           
                            if (_a.Contains(int.Parse(d2.Rows[xx].Cells["Column1"].Value.ToString()) ))
                            {
                                this.d2.Rows[xx].Cells["Column6"].Style.ForeColor = Color.Red;
                                this.d2.Rows[xx].Cells["Column6"].Style.BackColor = Color.MediumAquamarine;
                                //  this.d2.Rows[xx].Cells["Column7"].Style.BackColor = Color.Orange;
                            }
                            else
                            {
                                this.d2.Rows[xx].Cells["Column6"].Style.ForeColor = Color.Black;
                                this.d2.Rows[xx].Cells["Column6"].Style.BackColor = Color.PaleGreen;
                            }
                       
                        }
                }
                else if (_FLAG == 5 )
                {
                    //设定 or 实际值 需要给特殊的字体颜色标识
                    for (int x = 0; x < 2; x++)
                    {
                        //MgO
                        this.d1.Rows[x].Cells["Column26"].Style.ForeColor = Color.Red;
                        //C
                        this.d1.Rows[x].Cells["Column27"].Style.ForeColor = Color.Red;
                        //R
                        this.d1.Rows[x].Cells["Column28"].Style.ForeColor = Color.Red;
                    }
                }
                else if (_FLAG == 6)//成分检测值
                {
                    d1.Rows[2].DefaultCellStyle.BackColor = Color.PaleTurquoise;
                }
                else if (_FLAG == 7)//特殊分仓系数颜色变化
                {
                    for (int x = 0; x < d2.Rows.Count; x++)
                    {
                        int _xlk = int.Parse(d2.Rows[x].Cells["Column3"].Value.ToString());
                        for (int y = 0; y < _list_XLK.Count();y++)
                        {
                            if (_xlk == _list_XLK[y])
                            {
                                this.d2.Rows[x].Cells["Column10"].Style.BackColor = Color.MediumAquamarine;
                            }
                        }
                    }
                }
                else if (_FLAG == 8)
                {
                    string sql_1 = "";
                    if (CAL_MODE == 1)
                    {
                         sql_1 = " select distinct category ,canghao from CFG_MAT_L2_PBSD_INTERFACE  where category  =1 or category = 2";
                    }
                    else if (CAL_MODE == 2)
                    {
                         sql_1 = " select distinct category ,canghao from CFG_MAT_L2_PBSD_INTERFACE  where category  =1 or category = 2 or category = 4 ";
                    }
                    DataTable dataTable_1 = _dBSQL.GetCommand(sql_1);
                    for (int x_1 = 0; x_1 < 5; x_1++)
                    {
                        
                        if (x_1 % 2 == 0)
                        {
                            for (int x = 0; x < dataTable_1.Rows.Count; x++)
                            {
                                int CH = int.Parse(dataTable_1.Rows[x]["canghao"].ToString());
                                int category = int.Parse(dataTable_1.Rows[x]["category"].ToString());

                                for (int xx = 0; xx < d2.Rows.Count; xx++)
                                {
                                    if (int.Parse(d2.Rows[xx].Cells["Column1"].Value.ToString()) == CH)
                                    {
                                       // this.d2.Rows[xx].Cells["Column6"].Style.ForeColor = Color.Red;
                                          this.d2.Rows[xx].Cells["Column7"].Style.BackColor = Color.Orange;
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int x = 0; x < dataTable_1.Rows.Count; x++)
                            {
                                int CH = int.Parse(dataTable_1.Rows[x]["canghao"].ToString());
                                int category = int.Parse(dataTable_1.Rows[x]["category"].ToString());

                                for (int xx = 0; xx < d2.Rows.Count; xx++)
                                {
                                    if (int.Parse(d2.Rows[xx].Cells["Column1"].Value.ToString()) == CH)
                                    {
                                       // this.d2.Rows[xx].Cells["Column6"].Style.ForeColor = Color.Red;
                                         this.d2.Rows[xx].Cells["Column7"].Style.BackColor = Color.White;
                                    }
                                }
                            }
                        }
                        Thread.Sleep(3000);
                    }
                }
                else if (_FLAG == 9) //设定下料量字体颜色变化
                {
                    this.d2.Columns["Column14"].DefaultCellStyle.BackColor = Color.Red;
                    this.d2.Columns["Column14"].DefaultCellStyle.ForeColor = Color.White; 
                }
                else if (_FLAG == 10) //设定下料量字体颜色变化
                {
                    this.d2.Columns["Column14"].DefaultCellStyle.BackColor = Color.Red;
                    this.d2.Columns["Column14"].DefaultCellStyle.ForeColor = Color.Yellow;
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 人工输入设定值、调整值赋值
        /// _flag 特殊成分调整模型
        /// </summary>
        public void import_R_C_MG( int _flag)
        {
            try
            {
                string SQL_R = "select top (1) C_Aim,C_Md,R_Aim,R_Md,MG_Aim,MG_Md from  CFG_MAT_L2_MACAL_IDT  order by TIMESTAMP desc ";
                DataTable dataTable_R = _dBSQL.GetCommand(SQL_R);
                if (dataTable_R.Rows.Count > 0 && dataTable_R != null)
                {
                    this.textBox_MB_R.Text = dataTable_R.Rows[0]["R_Aim"].ToString();
                    this.textBox_TZ_R.Text = dataTable_R.Rows[0]["R_Md"].ToString();
                    this.textBox_MB_C.Text = dataTable_R.Rows[0]["C_Aim"].ToString();
                    this.textBox_TZ_C.Text = dataTable_R.Rows[0]["C_Md"].ToString();
                    this.textBox_MB_MG.Text = dataTable_R.Rows[0]["MG_Aim"].ToString();
                    this.textBox_TZ_MG.Text = dataTable_R.Rows[0]["MG_Md"].ToString();
                }
                /// 判断是否隐藏MG输入框
                if (_flag == 1)
                {
                    label6.Visible = false;
                    label7.Visible = false;
                    textBox_MB_MG.Visible = false;
                    textBox_MB_MG.Text = "0";
                    textBox_TZ_MG.Visible = false;
                    textBox_TZ_MG.Text = "0";
                    simpleButton10.Visible = false;
                    simpleButton11.Visible = false;
                }
                else if (_flag == 2)
                {
                    label6.Visible = true;
                    label7.Visible = true;
                    textBox_MB_MG.Visible = true;
                    textBox_TZ_MG.Visible = true;
                    simpleButton10.Visible = true;
                    simpleButton11.Visible = true;
                }
            }
            catch (Exception ee)
            {
                string mistake = "人工输入值赋值查询失败" + ee.ToString();
                _vLog.writelog(mistake, -1);

            }
        }
        /// <summary>
        /// 总干料量、理论产量赋值
        /// </summary>
        public void Counter_OutPut()
        {
            try
            {
                string sql_MC_MIXCAL_RESULT_1MIN = "select  SINCAL_DRY_MIX_SP,ISNULL(SINCAL_OUTPUT_PV,0) AS SINCAL_OUTPUT_PV from MC_MIXCAL_RESULT_1MIN ORDER BY TIMESTAMP DESC ";
                DataTable data_MC_MIXCAL_RESULT_1MIN = _dBSQL.GetCommand(sql_MC_MIXCAL_RESULT_1MIN);
                if (data_MC_MIXCAL_RESULT_1MIN.Rows.Count > 0 && data_MC_MIXCAL_RESULT_1MIN != null)
                {
                    //总干料量
                    this.textBox_ZGLL.Text = data_MC_MIXCAL_RESULT_1MIN.Rows[0]["SINCAL_DRY_MIX_SP"].ToString();
                    //理论产量
                    double LLCL = Math.Round(double.Parse(data_MC_MIXCAL_RESULT_1MIN.Rows[0]["SINCAL_OUTPUT_PV"].ToString()) * 60, 2);
                    this.textBox_LLCL.Text = LLCL.ToString();
                }
            }
            catch (Exception ee)
            {
                var mistake = "Counter_OutPut方法总干料量&理论产量赋值失败" + ee.ToString();
                _vLog.writelog(mistake,-1);
            }
        }
        /// <summary>
        /// 总料量sp、pv赋值
        /// </summary>
        public void INIT_SP_PV()
        {
            try
            {
                string SQL_ZLL = "  SELECT TOP (1) MAT_L2_ZGLL,MAT_PLC_PV, MAT_L2_SP ,MAT_L2_LLCL  FROM CFG_MAT_L2_PLZL_INTERFACE ORDER BY TIMESTAMP DESC";
                DataTable dataTable_ZLL = _dBSQL.GetCommand(SQL_ZLL);
                if (dataTable_ZLL.Rows.Count > 0 && dataTable_ZLL !=null)
                {
                    this.textBox_PV.Text = dataTable_ZLL.Rows[0][1].ToString();//总料量PV
                    this.textBox_SP.Text = dataTable_ZLL.Rows[0][2].ToString();//总料量SP                                                     
                }
            }
            catch(Exception ee)
            {
                var mistake = "INIT_SP_PV方法总料量sp、pv赋值赋值失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
            
        }

        /// <summary>
        /// 预测成分加载数据
        /// </summary>
        public void Ingredient()
        {
            try
            {
                ///MC_MIXCAL_RESULT_1MIN表 烧结矿预测设定成分
                string sql_SDZ = "select top (1) " +
                    "TIMESTAMP AS TIME," +
                    "SINCAL_SIN_SP_TFE AS TFE," +
                    "SINCAL_SIN_SP_FEO AS FEO," +
                    "SINCAL_SIN_SP_CAO AS CAO," +
                    "SINCAL_SIN_SP_SIO2 AS SIO2," +
                    "SINCAL_SIN_SP_AL2O3 AS AL2O3 ," +
                    "SINCAL_SIN_SP_MGO AS MGO ," +
                    "SINCAL_MIX_SP_C as C," +
                    "SINCAL_SIN_SP_R AS R," +
                    "SINCAL_SIN_SP_S AS S," +
                    "SINCAL_SIN_SP_P AS P ," +
                    "SINCAL_SIN_SP_MN AS MN," +
                    "SINCAL_SIN_SP_K2O AS K2O," +
                    "SINCAL_SIN_SP_NA2O AS NA2O ," +
                    "SINCAL_SIN_SP_AS AS a_s ," +
                    "SINCAL_SIN_SP_CU AS CU," +
                    "SINCAL_SIN_SP_PB AS PB," +
                    "SINCAL_SIN_SP_ZN AS ZN," +
                    "SINCAL_SIN_SP_K AS K," +
                    "SINCAL_SIN_SP_TIO2 as TIO2" +
                    "  from MC_MIXCAL_RESULT_1MIN   order by TIMESTAMP desc";
                ///MC_MIXCAL_RESULT_1MIN表 烧结矿预测实际成分
                string sql_SJZ = "select top (1)  " +
                    "TIMESTAMP AS TIME," +
                    "SINCAL_SIN_PV_TFE AS TFE," +
                    "SINCAL_SIN_PV_FEO AS FEO," +
                    "SINCAL_SIN_PV_CAO AS CAO," +
                    "SINCAL_SIN_PV_SIO2 AS SIO2," +
                    "SINCAL_SIN_PV_AL2O3 AS AL2O3 ," +
                    "SINCAL_SIN_PV_MGO AS MGO ," +
                    "SINCAL_MIX_PV_C as C," +
                    "SINCAL_SIN_PV_R AS R," +
                    "SINCAL_SIN_PV_S AS S," +
                    "SINCAL_SIN_PV_P AS P ," +
                    "SINCAL_SIN_PV_MN AS MN," +
                    "SINCAL_SIN_PV_K2O AS K2O," +
                    "SINCAL_SIN_PV_NA2O AS NA2O ," +
                    "SINCAL_SIN_PV_AS AS a_s ," +
                    "SINCAL_SIN_PV_CU AS CU," +
                    "SINCAL_SIN_PV_PB AS PB," +
                    "SINCAL_SIN_PV_ZN AS ZN," +
                    "SINCAL_SIN_PV_K AS K," +
                    "SINCAL_SIN_PV_TIO2 as TIO2" +
                    "  from MC_MIXCAL_RESULT_1MIN  order by TIMESTAMP desc";
                ///M_SINTER_ANALYSIS 烧结矿预测检测成分
                /////20200814 检测值添加限制条件 查询C_TFE、C_FEO、C_CAO、C_SIO2、C_AL2O3、C_MGO 都不为0
                string sql_JCZ = "select top (1) TIMESTAMP AS TIME,C_TFE AS TFE,C_FEO AS FEO,C_CAO AS CAO, C_SIO2 as SIO2, C_AL2O3 as AL2O3,C_MGO as MGO, NULL AS C,C_R as R,C_S as S,C_P2O5 AS P,C_MNO AS MN,C_K2O AS K2O,C_NA2O AS NA2O,C_AS AS a_s ,C_CU AS CU ,C_PB AS PB , C_ZN AS ZN, C_K AS K,C_TIO2 AS TIO2  from M_SINTER_ANALYSIS   where  (C_TFE <> 0 OR C_FEO <> 0 OR C_CAO <> 0 OR C_SIO2 <> 0 OR C_AL2O3 <> 0 OR  C_MGO <> 0) order by SAMPLETIME desc";
                DataTable dt_SDZ = _dBSQL.GetCommand(sql_SDZ);
                if (dt_SDZ.Rows.Count > 0)
                {
                    dt_SDZ.Columns.Add("explain");
                    dt_SDZ.Rows[0]["explain"] = "设定值";
                }
                DataTable dt_SJZ = _dBSQL.GetCommand(sql_SJZ);
                if (dt_SJZ.Rows.Count > 0)
                {
                    dt_SJZ.Columns.Add("explain");
                    dt_SJZ.Rows[0]["explain"] = "实际值";
                }
                DataTable dt_JCZ = _dBSQL.GetCommand(sql_JCZ);
                if (dt_JCZ.Rows.Count > 0)
                {
                    dt_JCZ.Columns.Add("explain");
                    dt_JCZ.Rows[0]["explain"] = "检验值";
                }
                DataTable newDataTable = dt_SDZ.Copy();
                foreach (DataRow dr in dt_SJZ.Rows)
                {
                    newDataTable.ImportRow(dr);
                }
                DataTable newDataTable_1 = newDataTable.Copy();
                foreach (DataRow dr in dt_JCZ.Rows)
                {
                    newDataTable_1.ImportRow(dr);
                }
                if (newDataTable_1.Rows.Count > 0)
                {
                    this.d1.DataSource = newDataTable_1;
                }
                else
                {
                    string mistake = "Ingredient方法加载预测成分数据失败，无数据" ;
                    _vLog.writelog(mistake, -1);
                }
            }
            catch (Exception ee)
            {
                string mistake = "Ingredient方法加载预测成分数据失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 设定水分、总料量sp、总干料量、理论产量、总料量PV存库
        /// </summary>
        public bool assignment_1()
        {
            try
            {
                for (int x = 0;x < d2.Rows.Count;x++)
                {
                    //仓号
                    int _CH = int.Parse(d2.Rows[x].Cells["Column1"].Value.ToString());
                    //设定水分
                    float water_SD = float.Parse(d2.Rows[x].Cells["Column12"].Value.ToString());
                    //配料表更新水分
                    string sql_SET_Water = "update CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_SFSD = " + water_SD + ",MAT_L2_SFDQ = " + water_SD + " where  MAT_L2_CH = " + _CH + "";
                    //20200907bins表更新水分
                    string sql_SET_Water_1 = "update M_MATERIAL_BINS set C_H2O = " + water_SD + " where  BIN_NUM_SHOW = " + _CH + "";
                    int count = _dBSQL.CommandExecuteNonQuery(sql_SET_Water);
                    int count_1 = _dBSQL.CommandExecuteNonQuery(sql_SET_Water_1);
                    if (count <= 0 && count_1 <= 0)
                    {
                        string messbox = "人工修改水分失败 ,仓号:" + _CH + ",设定和当前水分值:" + water_SD + ",SQL1语句：" + sql_SET_Water + "SQL2语句" + sql_SET_Water_1;
                        _vLog.writelog(messbox, -1);
                        MessageBox.Show("计算错误,检查水分值是否正常");
                        return false;
                    }
                }
                if (this.textBox_SP.Text.ToString() != "" && this.textBox_PV.Text.ToString() != "" && this.textBox_ZGLL.Text.ToString() != "" && this.textBox_LLCL.Text.ToString() != "")
                {
                    float ZLL_SP = float.Parse(this.textBox_SP.Text.ToString());//总料量SP
                    float ZLL_PV = float.Parse(this.textBox_PV.Text.ToString());//总料量PV
                    float ZGLL = float.Parse(this.textBox_ZGLL.Text.ToString());//总干料量
                    float LLCL = float.Parse(this.textBox_LLCL.Text.ToString());//理论产量
                    string SQL_ZLL = "INSERT INTO CFG_MAT_L2_PLZL_INTERFACE (MAT_L2_ZGLL, MAT_PLC_PV, MAT_L2_SP, MAT_L2_LLCL, TIMESTAMP) VALUES ('" + ZGLL + "', '" + ZLL_PV + "', '" + ZLL_SP + "', '" + LLCL + "',getdate());";
                    int count = _dBSQL.CommandExecuteNonQuery(SQL_ZLL);
                    if (count <= 0)
                    {
                        string messbox = "人工修改失败 总料量SP：" + this.textBox_SP.Text.ToString() + ",总料量PV:" + textBox_PV.Text.ToString() + ",总干料量:" + textBox_ZGLL.Text.ToString() + ",理论产量:" + textBox_LLCL.Text.ToString() + "sql语句" + SQL_ZLL;
                        _vLog.writelog(messbox, -1);
                        return false;
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("计算错误,检查输入项是否正常");
                    return false;
                }
            }
            catch (Exception ee)
            {
                string mistake = "设定水分、总料量sp、总干料量、理论产量、总料量PV存库失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return false;
            }

        }
        /// <summary>
        ///  分仓系数存库
        ///  _XLK:需要更新的分仓系数对应的下料口号
        /// </summary>
        public bool Warehousing(List<int> _XLK)
        {
            try
            {
                for (int x = 0; x < d2.Rows.Count;x++)
                {
                    int XLK = int.Parse(d2.Rows[x].Cells["Column3"].Value.ToString());//下料口
                    for (int y = 0; y < _XLK.Count();y++)
                    {
                        if (XLK == _XLK[y])
                        {
                            float FCXS = float.Parse(d2.Rows[x].Cells["Column10"].Value.ToString());
                            string sql_17 = "update CFG_MAT_L2_XLK_INTERFACE set MAT_L2_FCXS = " + FCXS + " where MAT_L2_XLK = "+ XLK + "";
                            int _COUNT =   _dBSQL.CommandExecuteNonQuery(sql_17);
                            if (_COUNT != 1)
                            {
                                var mistake = "Warehousing方法更新分仓系数失败，sql:" + sql_17;
                                _vLog.writelog(mistake,-1);
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch(Exception ee)
            {
                var mistake = "Warehousing方法更新分仓系数失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return false;
            }
        }
        /// <summary>
        /// 料口比例 计算 计算出所有的数据
        /// </summary>
        private bool FeedBLCompute_1()
        {
            try
            {
                var sql_liaokou = "select distinct MAT_PB_ID, MAT_L2_DQBFB from CFG_MAT_L2_SJPB_INTERFACE order by MAT_PB_ID asc";
                DataTable dataTable = _dBSQL.GetCommand(sql_liaokou);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    //配比id
                    int PBID = int.Parse(dataTable.Rows[i]["MAT_PB_ID"].ToString());
                    //当前配比%
                    float DQPB = float.Parse(dataTable.Rows[i]["MAT_L2_DQBFB"].ToString());
                    //添加判断启停信号
                    string sql = "select MAT_L2_XLKZT from CFG_MAT_L2_XLK_INTERFACE WHERE MAT_PB_ID =" + PBID + "";
                    DataTable dataTable_1 = _dBSQL.GetCommand(sql);
                    if (dataTable_1.Rows.Count > 0)
                    {
                        int sum = 0;
                        for (int x = 0; x < dataTable_1.Rows.Count; x++)
                        {
                            sum += int.Parse(dataTable_1.Rows[x]["MAT_L2_XLKZT"].ToString());
                        }
                        if (sum == 0)
                        {
                            string messbox = "FeedBLCompute_1方法计算下料比例，配比id：" + PBID + "的下料口状态都为禁用,当前配比：" + DQPB;
                            _vLog.writelog(messbox, 0);
                            HMICAL.FeedBLCompute(PBID, 0);  
                        }
                        else
                        {
                            string messbox = "FeedBLCompute_1方法计算下料比例，配比id：" + PBID + ",当前配比：" + DQPB;
                            _vLog.writelog(messbox, 0);
                            HMICAL.FeedBLCompute(PBID, DQPB);  
                        }                        
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ee)
            {
                string mistake = " 料口比例计算失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return false;
            }
        }
        /// <summary>
        /// 设定下料量计算
        /// </summary>
        private bool total_holdup()
        {
            try
            {
                string sql = "select top (1)  MAT_L2_SP from CFG_MAT_L2_PLZL_INTERFACE order by TIMESTAMP desc";
                DataTable dataTable = _dBSQL.GetCommand(sql);
                if (dataTable.Rows.Count > 0)
                {
                    float ZLL = float.Parse(dataTable.Rows[0]["MAT_L2_SP"].ToString());
                    if (ZLL < 0)
                    {
                        string messbox = "total_holdup方法计算设定下料量，总料量Sp不正常" + ZLL;
                        _vLog.writelog(messbox, -1);
                        return false;
                    }
                    else
                    {
                        string messbox = "total_holdup方法计算设定下料量，总料量Sp：" + ZLL;
                        _vLog.writelog(messbox, 0);
                        HMICAL.FeedLLCompute(ZLL);
                        return true;
                    }
                }
                else
                {
                    string messbox = "total_holdup方法计算设定下料量，数据库获取总料量Sp不正常" + sql;
                    _vLog.writelog(messbox, -1);
                    return false;
                }
            }
            catch (Exception ee)
            {
                string mistake = "总料量计算失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return false;
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

            _Timer2 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer2.Elapsed += (x, y) => { _Timer2_Tick(); };
            _Timer2.Enabled = false;
            _Timer2.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer3 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer3.Elapsed += (x, y) => { _Timer3_Tick(); };
            _Timer3.Enabled = true;
            _Timer3.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer4 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer4.Elapsed += (x, y) => { _Timer4_Tick(); };
            _Timer4.Enabled = true;
            _Timer4.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer5 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer5.Elapsed += (x, y) => { _Timer5_Tick(); };
            _Timer5.Enabled = true;
            _Timer5.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer6 = new System.Timers.Timer(3000);//初始化颜色变化定时器响应事件
            _Timer6.Elapsed += (x, y) => { _Timer6_Tick(); };
            _Timer6.Enabled = true;
            _Timer6.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer7 = new System.Timers.Timer(500);//初始化颜色变化定时器响应事件
            _Timer7.Elapsed += (x, y) => { _Timer7_Tick(); };
            _Timer7.Enabled = false;
            _Timer7.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
        }


        #region 定时器响应事件
        /// <summary>
        /// 初始化颜色变化定时器响应事件
        /// </summary>
        private void _Timer1_Tick()
        {
            Action invokeAction = new Action(_Timer1_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                COLOR_CHANE(1);
                COLOR_CHANE(2);
                COLOR_CHANE(3);
                COLOR_CHANE(4);
                COLOR_CHANE(5);
                COLOR_CHANE(6);
                COLOR_CHANE(7);
            }
        }
        /// <summary>
        /// 返矿调整配比弹出框
        /// </summary>
        private void _Timer2_Tick()
        {
            Action invokeAction = new Action(_Timer2_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                try
                {
                    #region 烧返弹出框检测 修改配比
                    string sql_SF = "select top (1) " +
                        "isnull(SRMCAL_SL_FLAG,0) as SRMCAL_SL_FLAG," +
                        "isnull(SRMCAL_A_FLAG,0) as SRMCAL_A_FLAG," +
                        "isnull(SRMCAL_FLAG,0) as SRMCAL_FLAG    " +
                        "from MC_SRMCAL_RESULT " +
                        "where TIMESTAMP = (select max(TIMESTAMP) from MC_SRMCAL_RESULT) order by TIMESTAMP desc";
                    DataTable dataTable_SF = _dBSQL.GetCommand(sql_SF);
                    if (dataTable_SF.Rows.Count > 0 && dataTable_SF!=null)
                    {
                        ///20200826 添加逻辑，添加判断是否人工修改标志位,龙飞
                        int x1 = int.Parse(dataTable_SF.Rows[0]["SRMCAL_SL_FLAG"].ToString());
                        int x2 = int.Parse(dataTable_SF.Rows[0]["SRMCAL_A_FLAG"].ToString());
                        int x3 = int.Parse(dataTable_SF.Rows[0]["SRMCAL_FLAG"].ToString());
                        if (x1 == 1 && x2 == 1 && x3 != 1)
                        {
                            string text = "****************调整烧返配比弹出框已弹出************************";
                            _vLog.writelog(text, 0);
                            //Frm_MIX_SRMCAL form_display = new Frm_MIX_SRMCAL();
                            //form_display._transfDelegate += SF_POP;
                            //form_display.Show();
                            if (_Auto == null || _Auto.IsDisposed)
                            {
                                _Auto = new Frm_MIX_SRMCAL();
                                _Auto._transfDelegate += SF_POP;
                                _Auto.ShowDialog();
                            }
                            else
                            {
                                _Auto.Activate();
                            }

                        }
                    }
                    #endregion
                }
                catch (Exception ee)
                {
                    var mistake = "***********_Timer2_Tick()方法周期查询返矿调整配比弹出框失败" + ee.ToString();
                    _vLog.writelog(mistake, -1);
                }
            }
        }
        /// <summary>
        /// 调整值弹出框
        /// </summary>
        private void _Timer3_Tick()
        {
            Action invokeAction = new Action(_Timer3_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                try
                {
                    bool _FLAG = false;//开关是否存在自动状态
                   
                    if (CAL_MODE == 1)
                    {
                        if (R_MODE == 1 || C_MODE == 1 )
                        {
                            _FLAG = true;
                        }
                        else
                        {
                            return;
                        }

                    }
                    else if(CAL_MODE == 2)
                    {
                        if (R_MODE == 1 || C_MODE == 1 || MG_MODE == 1)
                        {
                            _FLAG = true;
                        }
                        else
                        {
                            return;
                        }
                    }

                    if (_FLAG)
                    {
                            //判断是否激活弹出框
                            bool FLAG_c = true;
                            bool FLAG_r = true;
                            bool FLAG_mgo = true;
                            string sql_MG = "select top (1) SINCAL_MG_FLAG,SINCAL_MG_RE_ADJ,TIMESTAMP  from MC_SINCAL_MG_result order by TIMESTAMP desc";
                            DataTable dataTable_MG = _dBSQL.GetCommand(sql_MG);
                            string sql_C = "select top (1)  SINCAL_C_FLAG,TIMESTAMP,SINCAL_C_COM_RE_ADJ from MC_SINCAL_C_result order by TIMESTAMP desc ";
                            DataTable dataTable_C = _dBSQL.GetCommand(sql_C);
                            string sql_R = "select top (1)  SINCAL_R_FLAG,SINCAL_R_RE_ADJ,TIMESTAMP from MC_SINCAL_R_result order by TIMESTAMP desc";
                            DataTable dataTable_R = _dBSQL.GetCommand(sql_R);

                            if (dataTable_C.Rows.Count > 0 && dataTable_R.Rows.Count > 0 && dataTable_MG.Rows.Count > 0)
                            {

                                int flag_c = int.Parse(dataTable_C.Rows[0]["SINCAL_C_FLAG"].ToString() == "" ? "0" : dataTable_C.Rows[0]["SINCAL_C_FLAG"].ToString());
                                int flag_r = int.Parse(dataTable_R.Rows[0]["SINCAL_R_FLAG"].ToString() == "" ? "0" : dataTable_R.Rows[0]["SINCAL_R_FLAG"].ToString());
                                int flag_mg = int.Parse(dataTable_MG.Rows[0]["SINCAL_MG_FLAG"].ToString() == "" ? "0" : dataTable_MG.Rows[0]["SINCAL_MG_FLAG"].ToString());

                                float flag_c_ADJ = float.Parse(dataTable_C.Rows[0]["SINCAL_C_COM_RE_ADJ"].ToString() == "" ? "0" : dataTable_C.Rows[0]["SINCAL_C_COM_RE_ADJ"].ToString());
                                float flag_r_ADJ = float.Parse(dataTable_R.Rows[0]["SINCAL_R_RE_ADJ"].ToString() == "" ? "0" : dataTable_R.Rows[0]["SINCAL_R_RE_ADJ"].ToString());
                                float flag_mg_ADJ = float.Parse(dataTable_MG.Rows[0]["SINCAL_MG_RE_ADJ"].ToString() == "" ? "0" : dataTable_MG.Rows[0]["SINCAL_MG_RE_ADJ"].ToString());

                                #region C
                                if (flag_c == 1)
                                {
                                    if (flag_c_ADJ != 0)
                                    {
                                        FLAG_c = true;
                                    }
                                    else
                                    {
                                        FLAG_c = false;
                                    }
                                }
                                else
                                {
                                    FLAG_c = false;
                                }
                                #endregion
                                #region R
                                if (flag_r == 1)
                                {
                                    if (flag_r_ADJ != 0)
                                    {
                                        FLAG_r = true;
                                    }
                                    else
                                    {
                                        FLAG_r = false;
                                    }
                                }
                                else
                                {
                                    FLAG_r = false;
                                }
                                #endregion
                                #region MG
                                if (flag_mg == 1)
                                {
                                    if (flag_mg_ADJ != 0)
                                    {
                                        FLAG_mgo = true;
                                    }
                                    else
                                    {
                                        FLAG_mgo = false;
                                    }
                                }
                                else
                                {
                                    FLAG_mgo = false;
                                }
                            #endregion
                            if (CAL_MODE == 1)
                            {
                                if (FLAG_c || FLAG_r)
                                {
                                    string text = "*********C、R、MG弹出框检测已弹出**************";
                                    _vLog.writelog(text, 0);
                                    if (_Auto_1 == null || _Auto_1.IsDisposed)
                                    {
                                        _Auto_1 = new Frm_MIX_Ingredient();
                                        _Auto_1.transfDelegate += C_R_MG;
                                        _Auto_1.ShowDialog();
                                    }
                                    else
                                    {
                                        _Auto_1.Activate();
                                    }
                                }
                            }
                            else
                            {
                                if (FLAG_c || FLAG_r || FLAG_mgo)
                                {
                                    string text = "*********C、R、MG弹出框检测已弹出**************";
                                    _vLog.writelog(text, 0);
                                    if (_Auto_1 == null || _Auto_1.IsDisposed)
                                    {
                                        _Auto_1 = new Frm_MIX_Ingredient();
                                        _Auto_1.transfDelegate += C_R_MG;
                                        _Auto_1.ShowDialog();
                                    }
                                    else
                                    {
                                        _Auto_1.Activate();
                                    }
                                }
                            }

                        }
                            else
                            {
                                var mistake = "周期查询C、R、MG弹出框检测失败，结果表查询数据为空,sql_mg = " + sql_MG + "sql_c = " + sql_C + " sql_r = " + sql_R;
                                _vLog.writelog(mistake, -1);
                                return;
                            }
                    }
                }
                catch(Exception ee)
                {
                    var mistake = "调整值弹出框周期调用失败" + ee.ToString();
                    _vLog.writelog(mistake, -1);
                }
            }
        }
        /// <summary>
        /// 调整值弹出框
        /// </summary>
        private void _Timer4_Tick()
        {
            Action invokeAction = new Action(_Timer4_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                try
                {
                    if (FK_MODE == 1)
                    {
                        string sql_MC_SRMCAL_RESULT_DIST_T = "select  FLAG, FLAG2,TIMESTAMP from MC_SRMCAL_RESULT_DIST_T where TIMESTAMP = (select max(TIMESTAMP) from MC_SRMCAL_RESULT_DIST_T) ";
                        DataTable data_MC_SRMCAL_RESULT_DIST_T = _dBSQL.GetCommand(sql_MC_SRMCAL_RESULT_DIST_T);
                        if (data_MC_SRMCAL_RESULT_DIST_T.Rows.Count > 0)
                        {
                            //时间
                            string _time = data_MC_SRMCAL_RESULT_DIST_T.Rows[0]["TIMESTAMP"].ToString();
                            //第一个标志位
                            int flag = int.Parse(data_MC_SRMCAL_RESULT_DIST_T.Rows[0]["FLAG"].ToString());
                            //第二个标志位
                            int flag_1 = int.Parse(data_MC_SRMCAL_RESULT_DIST_T.Rows[0]["FLAG2"].ToString());
                            //查询烧返对应的配比id
                            if (flag == 1)
                            {
                                DateTime dateTime = DateTime.Now;
                                string name = "烧返分仓系数调整，偏差异常";
                                string incident = "智能配料页面";
                                string modelname = "烧返模型";
                                logTable.Operation_Log(name, incident, modelname);

                                string sql_MC_SRMCAL_RESULT_DIST_T_FLAG = "update MC_SRMCAL_RESULT_DIST_T set FLAG = '2' where TIMESTAMP = (select max(TIMESTAMP) from MC_SRMCAL_RESULT_DIST_T)";
                                int count = _dBSQL.CommandExecuteNonQuery(sql_MC_SRMCAL_RESULT_DIST_T_FLAG);

                                if (count >= 0)
                                {
                                    _vLog.writelog("烧返仓仓位偏差异常更新标志位MC_SRMCAL_RESULT_DIST_T,FLAG = 2成功", 0);
                                }
                                else
                                {
                                    _vLog.writelog("烧返仓仓位偏差异常更新标志位MC_SRMCAL_RESULT_DIST_T,FLAG = 2失败,sql+" + sql_MC_SRMCAL_RESULT_DIST_T_FLAG, -1);
                                }
                            }
                            else if (flag == 0)
                            {
                              
                                if (flag_1 == 0)//继续调整
                                {
                                    string text = "**********烧返定时刷新分仓系数 修改分仓系数 *************************";
                                    _vLog.writelog(text, 0);
                                    //烧返仓仓位偏差（0：偏差正常；1：偏差异常；2：偏差异常且已弹框；3：偏差正常并已经使用）
                                    //重置标志位 MC_SRMCAL_RESULT_DIST_T表FLAG字段
                                    string sql_MC_SRMCAL_RESULT_DIST_T_FLAG = "update MC_SRMCAL_RESULT_DIST_T set FLAG = 3 where TIMESTAMP = '" + _time + "'";
                                    int count = _dBSQL.CommandExecuteNonQuery(sql_MC_SRMCAL_RESULT_DIST_T_FLAG);
                                    if (count >= 0)
                                    {
                                        _vLog.writelog("烧返仓仓位偏差正常,更新MC_SRMCAL_RESULT_DIST_T表FLAG=3成功", 1);
                                    }
                                    else
                                    {
                                        _vLog.writelog("烧返仓仓位偏差正常,更新MC_SRMCAL_RESULT_DIST_T表FLAG=3失败,sql=" + sql_MC_SRMCAL_RESULT_DIST_T_FLAG, -1);
                                    }
                                    //查询烧返的配比id
                                    string sql_sf_pbid = "select top 1 peinimingcheng from CFG_MAT_L2_PBSD_INTERFACE where category = '3'";
                                    DataTable dataTable_sf_pbid = _dBSQL.GetCommand(sql_sf_pbid);
                                    int pbid = int.Parse(dataTable_sf_pbid.Rows[0][0].ToString());

                                    //调用配比确认按钮
                                  bool A1 =   Mix_fcxs_differ();//判断是否修改了分仓系数
                                    if(A1)
                                    {
                                      bool A2 =   Bhouse_1_1(pbid);//更新分仓系数
                                        if (A2)
                                        {
                                          bool A3 = FeedBLCompute_SF(pbid);//计算下料比例
                                            if (A3)
                                            {
                                              bool A4 = total_holdup();//总料量计算
                                                if (A4)
                                                {
                                                    Dictionary<int, float> _A1 = mIX_PAGE.Calculate_SPB();//湿配比计算
                                                    if (_A1 != null)
                                                    {
                                                        foreach (var x in _A1)
                                                        {
                                                            //湿配比存库
                                                            string sql = "update CFG_MAT_L2_SJPB_INTERFACE set MAT_L2_SPB = " + x.Value + " where MAT_PB_ID = " + x.Key + "";
                                                            int _count = _dBSQL.CommandExecuteNonQuery(sql);
                                                            if (_count <= 0)
                                                                _vLog.writelog("_Timer4_Tick定时器计算湿配比存库有误" + sql, -1);
                                                        }
                                                        Issue_SDXLL();//下发
                                                        PBTZ_GRTDATA(2);//页面刷新
                                                        SP_PV();//计算成分
                                                        mIX_PAGE.OVER_Storage(4, CAL_MODE);//整体存库
                                                        PBTZ_GRTDATA(2);//更新页面数据
                                                        COLOR_BEGIN = 0;//还原颜色次数
                                                        _Timer7.Enabled = true;//设定下料量背景颜色变化
                                                        DateTime dateTime = DateTime.Now;
                                                        string name = "烧返仓调整分仓系数";
                                                        string incident = "智能配料页面";
                                                        string modelname = "烧返模型";
                                                        logTable.Operation_Log(name, incident, modelname);
                                                        latest_time(2);
                                                    }
                                                }
                                            }
                                        }

                                        
                                    }
                                    
                                }
                                else if (flag_1 == 1)
                                {
                                    string text = "**********烧返定时刷新分仓系数 修改分仓系数 *************************";
                                    _vLog.writelog(text, 0);
                                   
                                    string sql_MC_SRMCAL_RESULT_DIST_T_FLAG = "update MC_SRMCAL_RESULT_DIST_T set FLAG2 = 4 where TIMESTAMP  = '" + _time + "'";
                                    int count = _dBSQL.CommandExecuteNonQuery(sql_MC_SRMCAL_RESULT_DIST_T_FLAG);

                                    if (count >= 0)
                                    {
                                        _vLog.writelog("15#仓分仓系数过低,更新MC_SRMCAL_RESULT_DIST_T表FLAG=4成功", 1);
                                    }
                                    else
                                    {
                                        _vLog.writelog("15#仓分仓系数过低,更新MC_SRMCAL_RESULT_DIST_T表FLAG=4失败,sql=" + sql_MC_SRMCAL_RESULT_DIST_T_FLAG, -1);
                                    }

                                }
                                else if (flag_1 == 2)
                                {  
                                    string sql_MC_SRMCAL_RESULT_DIST_T_FLAG = "update MC_SRMCAL_RESULT_DIST_T set FLAG2 = 4 where TIMESTAMP  = '" + _time + "'";
                                    int count = _dBSQL.CommandExecuteNonQuery(sql_MC_SRMCAL_RESULT_DIST_T_FLAG);
                                    if (count >= 0)
                                    {
                                        _vLog.writelog("16#仓分仓系数过低,更新MC_SRMCAL_RESULT_DIST_T表FLAG2=4成功", 1);
                                    }
                                    else
                                    {
                                        _vLog.writelog("16#仓分仓系数过低,更新MC_SRMCAL_RESULT_DIST_T表FLAG2=4失败,sql=" + sql_MC_SRMCAL_RESULT_DIST_T_FLAG, -1);
                                    }
                                }

                            }

                        }
                    }
                }
                catch(Exception ee)
                {
                    var mistake = "_Timer4_Tick定周期检索烧返仓分仓系数失败" + ee.ToString();
                }
            }
        }
        /// <summary>
        /// 定时刷新预测值
        /// </summary>
        private void _Timer5_Tick()
        {
            Action invokeAction = new Action(_Timer5_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                try
                {
                    Ingredient();
                }
                catch (Exception ee)
                {
                    var mistake = "_Timer5_Tick方法失败" + ee.ToString();
                }
            }
        }
        /// <summary>
        /// 定时刷新PLC数据
        /// </summary>
        private void _Timer6_Tick()
        {
            Action invokeAction = new Action(_Timer6_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                try
                {
                    PLC_3s();
                }
                catch (Exception ee)
                {
                    var mistake = "_Timer6_Tick方法失败" + ee.ToString();
                }
            }
        }
        private void _Timer7_Tick()
        {
            Action invokeAction = new Action(_Timer7_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                Color_Big();//设定下料量颜色闪烁
            }
        }
        #endregion


        /// <summary>
        /// 计算烧返的下料比例
        /// </summary>
        /// <param name="_PBID"></param>
        /// <returns></returns>
        public bool FeedBLCompute_SF(int _PBID)
        {
            try
            {
               
                var sql_liaokou = "select MAT_L2_DQBFB from CFG_MAT_L2_SJPB_INTERFACE where MAT_PB_ID = "+ _PBID + "";

                DataTable _dataTable = _dBSQL.GetCommand(sql_liaokou);
                if (_dataTable.Rows.Count > 0 && _dataTable != null)
                {
                    //配比对应启停信号判断
                    string sql = "select MAT_L2_XLKZT from CFG_MAT_L2_XLK_INTERFACE WHERE MAT_PB_ID =" + _PBID + "";
                    DataTable dataTable_1 = _dBSQL.GetCommand(sql);
                    if (dataTable_1.Rows.Count > 0 && dataTable_1 != null)
                    {
                        int sum = 0;
                        for (int x = 0; x < dataTable_1.Rows.Count; x++)
                        {
                            sum += int.Parse(dataTable_1.Rows[x]["MAT_L2_XLKZT"].ToString());
                        }
                        HMICAL.FeedBLCompute(_PBID, float.Parse(_dataTable.Rows[0]["MAT_L2_DQBFB"].ToString()));
                        return true;
                    }
                    else
                    {
                        var mistake = "FeedBLCompute_SF方法失败，配比id对应仓的启停信号都为禁用,sql:" + sql;
                        _vLog.writelog(mistake, -1);
                        return false;
                    }
                       
                }
                else
                {
                    var mistake = "FeedBLCompute_SF方法失败，sql = " + sql_liaokou;
                    _vLog.writelog(mistake, -1);
                    return false;

                }
            }
            catch(Exception ee)
            {
                var mistake = "FeedBLCompute_SF方法失败" + ee.ToString();
                _vLog.writelog(mistake,-1);
                return false;
            }
        }
        /// <summary>
        /// 字符串格式异常响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void d2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                MessageBox.Show("输入数据格式错误", "警告");
            }
            catch { }
            
        }
        /// <summary>
        /// 计算成分sp_pv_L3
        /// </summary>
        public void SP_PV()
        {
            try
            {
                //1、SP
                var _Sp = HMICAL.CalculateSinterBySP();
                if (_Sp.Item1 != 0)
                {
                    _vLog.writelog("设定成分计算错误", -1);
                }
                //2、PV
                var _Pv = HMICAL.CalculateSinterByPV();
                if (_Pv.Item1 != 0)
                {
                    _vLog.writelog("实际成分计算错误", -1);
                }
                //3、新增插入数据  20200322
                var _NewAddData = HMICAL.ModifyData();
                if (!_NewAddData.Item1)
                {
                    _vLog.writelog("新增模型数据获取失败,表M_MACAL_INTERFACE_RESULT", -1);
                }
                //20200911 李涛更新
                //插入数据库
                if (_Sp.Item1 == 0 && _Pv.Item1 == 0 && _NewAddData.Item1)
                {
                    string sql_insert_spv = "insert into MC_MIXCAL_RESULT_1MIN(TIMESTAMP," +
                    "SINCAL_SIN_SP_TFE,SINCAL_SIN_SP_FEO,SINCAL_SIN_SP_CAO,SINCAL_SIN_SP_SIO2," +
                    "SINCAL_SIN_SP_AL2O3,SINCAL_SIN_SP_MGO,SINCAL_SIN_SP_S,SINCAL_SIN_SP_P,SINCAL_SIN_SP_MN," +
                    "SINCAL_SIN_SP_R,SINCAL_SIN_SP_TIO2,SINCAL_SIN_SP_K2O,SINCAL_SIN_SP_NA2O,SINCAL_SIN_SP_PBO," +
                    "SINCAL_SIN_SP_ZNO,SINCAL_SIN_SP_F,SINCAL_SIN_SP_AS,SINCAL_SIN_SP_CU,SINCAL_SIN_SP_PB,SINCAL_SIN_SP_ZN," +
                    "SINCAL_SIN_SP_K,SINCAL_SIN_SP_NA,SINCAL_SIN_SP_CR,SINCAL_SIN_SP_NI,SINCAL_SIN_SP_MNO,SINCAL_SIN_SP_SPARE1," +
                    "SINCAL_SIN_SP_SPARE2,SINCAL_SIN_SP_SPARE3,SINCAL_SIN_SP_SPARE4,SINCAL_SIN_SP_SPARE5,SINCAL_SIN_SP_SPARE6," +
                    "SINCAL_MIX_SP_LOT,SINCAL_MIX_SP_H2O_1,SINCAL_MIX_SP_H2O_2,SINCAL_MIX_SP_FeO,SINCAL_MIX_SP_C," +
                    "SINCAL_NON_FUEL_SP_C,SINCAL_FUEL_SP_C,SINCAL_NON_FE_SP_SIO2,SINCAL_DRY_MIX_SP,SINCAL_OUTPUT_SP," +
                    "SINCAL_FUEL_CON_SP,SINCAL_SIN_PV_TFE,SINCAL_SIN_PV_FEO,SINCAL_SIN_PV_CAO,SINCAL_SIN_PV_SIO2," +
                    "SINCAL_SIN_PV_AL2O3,SINCAL_SIN_PV_MGO,SINCAL_SIN_PV_S,SINCAL_SIN_PV_P,SINCAL_SIN_PV_MN,SINCAL_SIN_PV_R," +
                    "SINCAL_SIN_PV_TIO2,SINCAL_SIN_PV_K2O,SINCAL_SIN_PV_NA2O,SINCAL_SIN_PV_PBO,SINCAL_SIN_PV_ZNO,SINCAL_SIN_PV_F," +
                    "SINCAL_SIN_PV_AS,SINCAL_SIN_PV_CU,SINCAL_SIN_PV_PB,SINCAL_SIN_PV_ZN,SINCAL_SIN_PV_K,SINCAL_SIN_PV_NA," +
                    "SINCAL_SIN_PV_CR,SINCAL_SIN_PV_NI,SINCAL_SIN_PV_MNO,SINCAL_SIN_PV_SPARE1,SINCAL_SIN_PV_SPARE2," +
                    "SINCAL_SIN_PV_SPARE3,SINCAL_SIN_PV_SPARE4,SINCAL_SIN_PV_SPARE5,SINCAL_MIX_PV_LOT," +
                    "SINCAL_MIX_PV_H2O_1,SINCAL_MIX_PV_H2O_2,SINCAL_MIX_PV_FeO,SINCAL_MIX_PV_C,SINCAL_NON_FUEL_PV_C," +
                    "SINCAL_FUEL_PV_C,SINCAL_NON_FE_PV_SIO2,SINCAL_DRY_MIX_PV,SINCAL_OUTPUT_PV,SINCAL_FUEL_CON_PV," +
                    "SINCAL_BL_RATIO_PV," +
                    //从M_MACAL_INTERFACE_RESULT中获取
                    "SINCAL_L2_CODE_1,SINCAL_L2_CODE_2,SINCAL_L2_CODE_3,SINCAL_L2_CODE_4,SINCAL_L2_CODE_5," +
                    "SINCAL_L2_CODE_6,SINCAL_L2_CODE_7,SINCAL_L2_CODE_8,SINCAL_L2_CODE_9,SINCAL_L2_CODE_10," +
                    "SINCAL_L2_CODE_11,SINCAL_L2_CODE_12,SINCAL_L2_CODE_13,SINCAL_L2_CODE_14,SINCAL_L2_CODE_15," +
                    "SINCAL_L2_CODE_16,SINCAL_L2_CODE_17,SINCAL_L2_CODE_18,SINCAL_L2_CODE_19,SINCAL_L2_CODE_20," +
                    "SINCAL_BLEND_ORE_BILL_DRY,SINCAL_BFES_ORE_BILL_DRY,SINCAL_FLUX_STONE_BILL_DRY," +
                    "SINCAL_DOLOMATE_BILL_DRY,SINCAL_FLUX_BILL_DRY,SINCAL_FUEL_BILL_DRY,SINCAL_BRUN_DRY," +
                    "SINCAL_ASH_DUST_BILL_DRY,SINCAL_9_BILL_DRY," +
                    "SINCAL_10_BILL_DRY,SINCAL_11_BILL_DRY,SINCAL_12_BILL_DRY,SINCAL_13_BILL_DRY,SINCAL_14_BILL_DRY,SINCAL_15_16_BILL_DRY," +
                    "SINCAL_17_BILL_DRY,SINCAL_18_BILL_DRY,SINCAL_19_BILL_DRY,SINCAL_20_BILL_DRY," +
                     "SINCAL_C_A,SINCAL_R_A,SINCAL_MG_A,SINCAL_R_C,SINCAL_C_DC,SINCAL_MG_C,SINCAL_SUM_MIX_SP,SINCAL_SUM_MIX_PV," +
                    "SINCAL_BLEND_ORE_BILL_WET,SINCAL_BFES_ORE_BILL_WET,SINCAL_FLUX_STONE_BILL_WET," +
                    "SINCAL_DOLOMATE_BILL_WET,SINCAL_FLUX_BILL_WET,SINCAL_FUEL_BILL_WET," +
                    "SINCAL_BRUN_BILL_WET,SINCAL_ASH_DUST_BILL_WET,SINCAL_9_BILL_WET," +
                     "SINCAL_10_BILL_WET,SINCAL_11_BILL_WET,SINCAL_12_BILL_WET,SINCAL_13_BILL_WET,SINCAL_14_BILL_WET," +
                     "SINCAL_15_16_BILL_WET,SINCAL_17_BILL_WET,SINCAL_18_BILL_WET,SINCAL_19_BILL_WET,SINCAL_20_BILL_WET,SINCAL_MATCH_BFE_W_1," +
                    "SINCAL_MATCH_BFE_W_2,SINCAL_MATCH_RE_W_1,SINCAL_MATCH_RE_W_2" +
                    ")values(";
                    string sql_mid = "'" + DateTime.Now + "',";
                    string sql_end = ")";
                    List<float> _vRs = new List<float>();
                    _vRs.AddRange(_Sp.Item2);
                    _vRs.AddRange(_Pv.Item2);
                    _vRs.AddRange(_NewAddData.Item2);
                    int len = _vRs.Count();
                    for (int i = 0; i < len; i++)
                    {
                        if (i != len - 1)
                            sql_mid += Math.Round(_vRs[i], 4) + ",";
                        else
                            sql_mid += Math.Round(_vRs[i], 4);
                    }
                    string sql_insert = sql_insert_spv + sql_mid + sql_end;
                    int _rs = _dBSQL.CommandExecuteNonQuery(sql_insert);
                    if (_rs <= 0)
                    {
                        _vLog.writelog("SP-PV插入数据库表MC_MIXCAL_RESULT_1MIN失败(" + sql_insert + ")", -1);
                    }
                }
                else
                {
                    if (_Sp.Item1 < 0)
                    {
                        _vLog.writelog("总残存sumRemnant_SP<=0,错误！，需要检查现场设定配比是否正确输入", -1);
                    }
                    if (_Pv.Item1 < 0)
                    {
                        _vLog.writelog("sumMix_Wet_PV总下料量为0错误; 请查看Read_MC_MEASURE_5MIN 是否5分钟表内没有数据", -1);
                    }
                }
            }
            catch (Exception EE)
            {
                var MISTAKE = "调用SP_PV（）方法失败" + EE.ToString();
                _vLog.writelog(MISTAKE, -1);
            }
        }

        #region 按钮调整值赋值
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            float A = float.Parse(textBox_TZ_R.Text.ToString());
            textBox_TZ_R.Text = Math.Round((A + R_ADD),3).ToString();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            float A = float.Parse(textBox_TZ_R.Text.ToString());
            textBox_TZ_R.Text = (Math.Round(A - R_ADD, 3) ).ToString();
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            float A = float.Parse(textBox_TZ_C.Text.ToString());
            textBox_TZ_C.Text = (Math.Round(A + C_ADD, 3) ).ToString();
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            float A = float.Parse(textBox_TZ_C.Text.ToString());
            textBox_TZ_C.Text = (Math.Round(A - C_ADD, 3)).ToString();
        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            float A = float.Parse(textBox_TZ_MG.Text.ToString());
            textBox_TZ_MG.Text = (Math.Round(A + MG_ADD, 3) ).ToString();
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            float A = float.Parse(textBox_TZ_MG.Text.ToString());
            textBox_TZ_MG.Text = (Math.Round(A - MG_ADD, 3) ).ToString();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            float A = float.Parse(textBox_SP.Text.ToString());
            textBox_SP.Text = (A + SP_ADD).ToString();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            float A = float.Parse(textBox_SP.Text.ToString());
            textBox_SP.Text = (A - SP_ADD).ToString();
        }
        #endregion

        /// <summary>
        /// plc更新页面数据
        /// </summary>
        public void PLC_3s()
        {
            try
            {
                string SQL_PLC_3S = "  select  " +
                    //仓位
                   "isnull(T_W_1_3S,0) as T_W_1_3S," +
                   "isnull(T_W_2_3S,0) as T_W_2_3S," +
                   "isnull(T_W_3_3S,0) as T_W_3_3S," +
                   "isnull(T_W_4_3S,0) as T_W_4_3S," +
                   "isnull(T_W_5_3S,0) as T_W_5_3S," +
                   "isnull(T_W_6_3S,0) as T_W_6_3S," +
                   "isnull(T_W_7_3S,0) as T_W_7_3S," +
                   "isnull(T_W_8_3S,0) as T_W_8_3S," +
                   "isnull(T_W_9_3S,0) as T_W_9_3S," +
                   "isnull(T_W_10_3S,0) as T_W_10_3S," +
                   "isnull(T_W_11_3S,0) as T_W_11_3S," +
                   "isnull(T_W_12_3S,0) as T_W_12_3S," +
                   "isnull(T_W_13_3S,0) as T_W_13_3S," +
                   "isnull(T_W_14_3S,0) as T_W_14_3S," +
                   "isnull(T_W_15_3S,0) as T_W_15_3S," +
                   "isnull(T_W_16_3S,0) as T_W_16_3S," +
                   "isnull(T_W_17_3S,0) as T_W_17_3S, " +
                   "isnull(T_W_18_3S,0) as T_W_18_3S," +
                   "isnull(T_W_19_3S,0) as T_W_19_3S," +
                   "isnull(T_W_20_3S,0) as T_W_20_3S, " +
                   //实际下料量
                   "isnull(T_ACTUAL_W_1_3S,0) as T_ACTUAL_W_1_3S," +
                   "isnull(T_ACTUAL_W_2_3S,0) as T_ACTUAL_W_2_3S," +
                   "isnull(T_ACTUAL_W_3_3S,0) as T_ACTUAL_W_3_3S," +
                   "isnull(T_ACTUAL_W_4_3S,0) as T_ACTUAL_W_4_3S," +
                   "isnull(T_ACTUAL_W_5_3S,0) as T_ACTUAL_W_5_3S," +
                   "isnull(T_ACTUAL_W_6_3S,0) as T_ACTUAL_W_6_3S," +
                   "isnull(T_ACTUAL_W_7_3S,0) as T_ACTUAL_W_7_3S," +
                   "isnull(T_ACTUAL_W_8_3S,0) as T_ACTUAL_W_8_3S," +
                   "isnull(T_ACTUAL_W_9_3S,0) as T_ACTUAL_W_9_3S," +
                   "isnull(T_ACTUAL_W_10_3S,0) as T_ACTUAL_W_10_3S," +
                   "isnull(T_ACTUAL_W_11_3S,0) as T_ACTUAL_W_11_3S," +
                   "isnull(T_ACTUAL_W_12_3S,0) as T_ACTUAL_W_12_3S," +
                   "isnull(T_ACTUAL_W_13_3S,0) as T_ACTUAL_W_13_3S," +
                   "isnull(T_ACTUAL_W_14_3S,0) as T_ACTUAL_W_14_3S," +
                   "isnull(T_ACTUAL_W_15_3S,0) as T_ACTUAL_W_15_3S," +
                   "isnull(T_ACTUAL_W_16_3S,0) as T_ACTUAL_W_16_3S," +
                   "isnull(T_ACTUAL_W_17_3S,0) as T_ACTUAL_W_17_3S," +
                   "isnull(T_ACTUAL_W_18_3S,0) as T_ACTUAL_W_18_3S," +
                   "isnull(T_ACTUAL_W_19_3S,0) as T_ACTUAL_W_19_3S ," +
                   "isnull(T_ACTUAL_W_20_3S,0) as T_ACTUAL_W_19_3S ," +
                   //皮带转速
                    "isnull(T_BELT_SPEED_1_3S,0) as T_BELT_SPEED_1_3S," +
                   "isnull(T_BELT_SPEED_2_3S,0) as T_BELT_SPEED_2_3S," +
                   "isnull(T_BELT_SPEED_3_3S,0) as T_BELT_SPEED_3_3S," +
                   "isnull(T_BELT_SPEED_4_3S,0) as T_BELT_SPEED_4_3S," +
                   "isnull(T_BELT_SPEED_5_3S,0) as T_BELT_SPEED_5_3S," +
                   "isnull(T_BELT_SPEED_6_3S,0) as T_BELT_SPEED_6_3S," +
                   "isnull(T_BELT_SPEED_7_3S,0) as T_BELT_SPEED_7_3S," +
                   "isnull(T_BELT_SPEED_8_3S,0) as T_BELT_SPEED_8_3S," +
                   "isnull(T_BELT_SPEED_9_3S,0) as T_BELT_SPEED_9_3S," +
                   "isnull(T_BELT_SPEED_10_3S,0) as T_BELT_SPEED_10_3S," +
                   "isnull(T_BELT_SPEED_11_3S,0) as T_BELT_SPEED_11_3S," +
                   "isnull(T_BELT_SPEED_12_3S,0) as T_BELT_SPEED_12_3S," +
                   "isnull(T_BELT_SPEED_13_3S,0) as T_BELT_SPEED_13_3S," +
                   "isnull(T_BELT_SPEED_14_3S,0) as T_BELT_SPEED_14_3S," +
                   "isnull(T_BELT_SPEED_15_3S,0) as T_BELT_SPEED_15_3S," +
                   "isnull(T_BELT_SPEED_16_3S,0) as T_BELT_SPEED_16_3S," +
                   "isnull(T_BELT_SPEED_17_3S,0) as T_BELT_SPEED_17_3S," +
                   "isnull(T_BELT_SPEED_18_3S,0) as T_BELT_SPEED_18_3S," +
                   "isnull(T_BELT_SPEED_19_3S,0) as T_BELT_SPEED_19_3S ," +
                    "isnull(T_BELT_SPEED_20_3S,0) as T_BELT_SPEED_20_3S ," +

                    //总料量PV
                  "  isnull(T_TOTAL_PV_W_3S, 0) as T_TOTAL_PV_W_3S " +
                   "from C_PLC_3S " +
                   "where TIMESTAMP = (select max(TIMESTAMP) from C_PLC_3S)  ";
                DataTable table_3s = _dBSQL.GetCommand(SQL_PLC_3S);
                if (table_3s != null && table_3s.Rows.Count > 0)
                {
                    FLAG_1 = false;//表单响应事件标志位
                    string dateTime = DateTime.Now.ToShortTimeString().ToString();//累计值时间
                    for (int x = 0; x < 20;x++)
                    {
                        this.d2.Rows[x].Cells["Column5"].Value = Math.Round( float.Parse(table_3s.Rows[0][x].ToString()), Digit_4);//仓位
                        this.d2.Rows[x].Cells["Column15"].Value = Math.Round(float.Parse(table_3s.Rows[0][x+20].ToString()), Digit_5);//实际下料量
                        this.d2.Rows[x].Cells["Column18"].Value = Math.Round(float.Parse(table_3s.Rows[0][x + 20 + 20].ToString()), Digit_7);//设备转速
                        this.d2.Rows[x].Cells["Column16"].Value = Math.Round(float.Parse(this.d2.Rows[x].Cells["Column15"].Value.ToString()) - float.Parse(this.d2.Rows[x].Cells["Column14"].Value.ToString()), Digit_6);//偏差
                                                                                                                                                                                                                         //累计值
                        
                        //判断每天的凌晨，累计清零
                        if (dateTime == "8:00" || dateTime == "20:00")
                        {
                          //累计值
                          this.d2.Rows[x].Cells["Column19"].Value = 0;                            
                        }
                        else
                        {
                           this.d2.Rows[x].Cells["Column19"].Value = Math.Round(float.Parse(this.d2.Rows[x].Cells["Column19"].Value.ToString()) + float.Parse(table_3s.Rows[0][x + 20].ToString())/60, Digit_9);
                        }
                    }
                    FLAG_1 = true;
                    this.textBox_PV.Text = table_3s.Rows[0]["T_TOTAL_PV_W_3S"].ToString();
                }
                else
                {
                    return;
                }
            }
            catch (Exception ee)
            {
                var mistake = "PLC_3s方法plc更新页面失败"+ee.ToString();
                _vLog.writelog(mistake,-1);
            }
        }
        /// <summary>
        /// 参数变化弹出框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>ty
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Frm_MIX_Parameter form_display = new Frm_MIX_Parameter();
            if (Frm_MIX_Parameter.isopen == false)
            {
                form_display._TransfDelegate_PL+= Form_sender;
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
         
        }
        /// <summary>
        /// 参数弹出框响应事件
        /// </summary>
        public void Form_sender()
        {
            COLOR_CHANE(4);//特殊配比值颜色变化
            MIX_PAGE_Digit();//配置参数
            MAX_MIN_VLAUES();//上下限赋值
        }

        /// <summary>
        /// 获取添加or减少调整值数据
        /// </summary>
        public void TEXT_CHANGE_PAGE()
        {
           Tuple<float, float, float, float> _tuple =   mIX_PAGE.TEXT_CHANGE();
            R_ADD = _tuple.Item1;
            C_ADD = _tuple.Item2;
            MG_ADD = _tuple.Item3;
            SP_ADD = _tuple.Item4;
        }
        /// <summary>
        /// 烧返弹出框调用条件
        /// </summary>
        /// <param name="PB"> 烧返对应的配比</param>
        /// <param name="PBID"> 烧返对应的配比id</param>
        public void SF_POP(float PB, int PBID)
        {
            try
            {
                for (int x = 0; x < d2.Rows.Count;x++)
                {
                    if (PBID == int.Parse(d2.Rows[x].Cells["Column20"].Value.ToString()))
                    {
                        FLAG_1 = false;
                        this.d2.Rows[x].Cells["Column6"].Value = Math.Round(PB, Digit_1);
                        FLAG_1 = true;
                    }
                }
                BUTTON_PBTZ();
                BUTTON_PBQR();
                mIX_PAGE.OVER_Storage(2, CAL_MODE);
                string name = "模型自动调整烧返配比";
                logTable.Operation_Log(name, "智能配料页面", "智能配料模型");
            }
            catch(Exception ee)
            {
                var mistake = "SF_POP方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 表单响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void d2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (FLAG_1)
            {
                #region 设定配比联动功能
                if (e.ColumnIndex == this.d2.Columns["Column6"].Index)
                {
                    string _PBID = this.d2.Rows[e.RowIndex].Cells["Column20"].Value.ToString();//配比id
                    float _SDPB = float.Parse(this.d2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());//修改后的配比值
                    for (int x = 0; x < d2.Rows.Count; x++)
                    {
                        string _ID = this.d2.Rows[x].Cells["Column20"].Value.ToString();//配比id
                        if (_ID == _PBID)
                        {
                            FLAG_1 = false;
                            d2.Rows[x].Cells["Column6"].Value = Math.Round(_SDPB, Digit_1);
                            FLAG_1 = true;
                        }
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// 配置参数
        /// </summary>
        public void MIX_PAGE_Digit()
        {
            this.d2.ColumnHeadersHeight = 50; //设置Dgv属性ColumnHeadersHeightSizeMode才会生效
            List<int> _list = mIX_PAGE.MIX_Digit();//小数位数获取
            Digit_1 = _list[0];
            Digit_2 = _list[1];
            Digit_3 = _list[2];
            Digit_4 = _list[3];
            Digit_5 = _list[4];
            Digit_6 = _list[5];
            Digit_7 = _list[6];
            Digit_8 = _list[7];
            Digit_9 = _list[8];
            List<string>_list_1 = mIX_PAGE.Switch_name();//开关名称

            switch_1_open = _list_1[0];
            switch_1_close = _list_1[1];

            switch_2_open = _list_1[2];
            switch_2_close = _list_1[3];

            switch_3_open = _list_1[4];
            switch_3_close = _list_1[5];

            switch_4_open = _list_1[6];
            switch_4_close = _list_1[7];

            FALG_Oper = mIX_PAGE._GetIp_Jurisdiction();//获取现场权限
        }
        /// <summary>
        /// 计算成分查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Frm_MIX_Element form_display = new Frm_MIX_Element();
            if (Frm_MIX_Element.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }
        public void C_R_MG(bool _flag, float _C, float _R, float _MG)
        {
            if (_flag)
            {
                if (_C != 10000)
                {
                    textBox_TZ_C.Text = _C.ToString();
                    var text = "*****碱度配碳弹出框，用户点击确认按钮，修改碳调整值：" + _C;
                    _vLog.writelog(text, 0);
                }
                else
                {
                    var text = "*****碱度配碳弹出框，用户点击确认按钮，修改碳调整值不满足，未修改";
                    _vLog.writelog(text, 0);
                }

                if (_R != 10000)
                {
                    textBox_TZ_R.Text = _R.ToString();
                    var text = "*****碱度配碳弹出框，用户点击确认按钮，修改碱度调整值：" + _R;
                    _vLog.writelog(text, 0);
                }
                else
                {
                    var text = "*****碱度配碳弹出框，用户点击确认按钮，修改碱度调整值不满足";
                    _vLog.writelog(text, 0);
                }

                if (_MG != 10000)
                {
                    textBox_TZ_MG.Text = _MG.ToString();
                    var text = "*****碱度配碳弹出框，用户点击确认按钮，修改mgo调整值：" + _MG;
                    _vLog.writelog(text, 0);
                    
                }
                else
                {
                    var text = "*****碱度配碳弹出框，用户点击确认按钮，修改MGO调整值不满足";
                    _vLog.writelog(text, 0);
                }
                BUTTON_PBTZ();
                BUTTON_PBQR();
                mIX_PAGE.OVER_Storage(3, CAL_MODE);
                latest_time(2);//最新下发时间
                string name = "目标值及调整值发生变化";
                logTable.Operation_Log(name, "智能配料页面", "智能配料模型");
               

            }
            else
            {
                var text = "*****碱度配碳弹出框，用户点击确认按钮，条件不满足，执行退出";
                _vLog.writelog(text, 0);
                return;
            }
        }
        /// <summary>
        /// 分仓系数判断
        /// </summary>
        /// <param name="_PBID"></param>
        public bool Bhouse_1_1(int _PBID)
        {
            try
            {
                
                //判断分仓系数是否修改，若修改则使用页面分仓系数，若无修改则使用MC_SRMCAL_RESULT_DIST_T表
                var _sql = "select top(1) isnull(MAT_PB_FALG, 0) as MAT_PB_FALG from CFG_MAT_L2_XLK_INTERFACE where MAT_PB_ID = " + _PBID + " ";
                DataTable _data = _dBSQL.GetCommand(_sql);
                if(_data != null && _data.Rows.Count > 0)
                {
                    if (_data.Rows[0][0].ToString() != "0")//分仓系数发生了变化
                    {
                        //***更新配比id对应的分仓系数
                        for (int xx = 0; xx < d2.Rows.Count; xx++)
                        {

                            if (_PBID == int.Parse(d2.Rows[xx].Cells["Column20"].Value.ToString()))
                            {
                                float FCXS = float.Parse(d2.Rows[xx].Cells["Column10"].Value.ToString());
                                int XLK = xx + 1;
                                string sql = "update [CFG_MAT_L2_XLK_INTERFACE] set MAT_L2_FCXS = " + FCXS + " WHERE MAT_L2_XLK =" + XLK + "";
                               int _count =  _dBSQL.CommandExecuteNonQuery(sql);
                                if(_count <=0)
                                    _vLog.writelog("Bhouse_1_1方法更新分仓系数失败" + sql, 0);
                            }
                        }
                        var sql11 = "update CFG_MAT_L2_XLK_INTERFACE set MAT_PB_FALG = 0 where MAT_PB_ID = " + _PBID + "";
                        int count = _dBSQL.CommandExecuteNonQuery(sql11);
                        if (count <= 0)
                            _vLog.writelog("Bhouse_1_1方法烧返周期判断分仓系数发生变化，还原标志位失败，影响行数：" + count + " ****sql: " + sql11, -1);
                        
                    }
                    else
                    {

                        //查询MC_SRMCAL_RESULT_DIST_T最新的一条数据的SRMCAL_S_15和SRMCAL_S_16的数据并赋值到配料页面对应的15号仓和16号仓的分仓系数中（雪刚让写死）
                        string sql_sf_1 = "select  top (1) SRMCAL_S_15,SRMCAL_S_16 from MC_SRMCAL_RESULT_DIST_T order by TIMESTAMP desc";
                        DataTable dataTable_SF_1 = _dBSQL.GetCommand(sql_sf_1);
                        if (dataTable_SF_1.Rows.Count > 0)
                        {
                            float FCXS_1 = float.Parse(dataTable_SF_1.Rows[0]["SRMCAL_S_15"].ToString());
                            float FCXS_2 = float.Parse(dataTable_SF_1.Rows[0]["SRMCAL_S_16"].ToString());
                            //页面赋值
                            for (int x_fcxs = 0; x_fcxs < d2.Rows.Count; x_fcxs++)
                            {
                                FLAG_1 = false;
                                if (int.Parse(d2.Rows[x_fcxs].Cells["Column1"].Value.ToString()) == 15)
                                    d2.Rows[x_fcxs].Cells["Column10"].Value = Math.Round( FCXS_1, Digit_3).ToString();
                                if (int.Parse(d2.Rows[x_fcxs].Cells["Column1"].Value.ToString()) == 16)
                                    d2.Rows[x_fcxs].Cells["Column10"].Value = Math.Round(FCXS_2, Digit_3).ToString();
                                FLAG_1 = true;
                            }
                           
                            //更新数据库中的烧返的分仓系数
                            string sql_s15 = "update CFG_MAT_L2_XLK_INTERFACE set MAT_L2_FCXS = " + FCXS_1 + " WHERE MAT_L2_CH =" + 15 + "";
                            int _count_1 = _dBSQL.CommandExecuteNonQuery(sql_s15);
                            if(_count_1 <= 0)
                                _vLog.writelog("Bhouse_1_1方法更新分仓系数失败，sql: " + sql_s15, -1);
                            string sql_s16 = "update [CFG_MAT_L2_XLK_INTERFACE] set MAT_L2_FCXS = " + FCXS_2 + " WHERE MAT_L2_CH =" + 16 + "";
                            int count_2 = _dBSQL.CommandExecuteNonQuery(sql_s16);
                            if(count_2<=0)
                                _vLog.writelog("Bhouse_1_1方法更新分仓系数失败，sql: " + sql_s16, -1);

                        }
                        else
                        {
                            string mistake = "Bhouse_1_1方法MC_SRMCAL_RESULT_DIST_T表数据异常";
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
                    
            }
            catch(Exception ee)
            {
                var mistake = "Bhouse_1_1方法调用失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return false;
            }
           

        }
        /// <summary>
        /// 配比信息查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Frm_PB_MESSAGE form_display = new Frm_PB_MESSAGE();
            if (Frm_PB_MESSAGE.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 初始化累计
        /// </summary>
        public void Accumulated_INIT()
        {
            try
            {
                FLAG_1 = false;
                Tuple<bool, Dictionary<int, double>> _A = mIX_PAGE.Accumulative_account();
                if (_A.Item1)
                {
                    for (int x = 0; x < d2.Rows.Count; x++)
                    {
                        this.d2.Rows[x].Cells["Column19"].Value = _A.Item2[x+1];
                    }
                }
                else
                {

                    for (int x = 0; x < d2.Rows.Count; x++)
                    {
                        this.d2.Rows[x].Cells["Column19"].Value = 0;
                    }
                }
                FLAG_1 = true;
            }
            catch(Exception ee)
            {
                _vLog.writelog("Accumulated_INIT方法失败" + ee.ToString(),-1);
            }
           
        }
        /// <summary>
        /// 设定下料量颜色闪烁
        /// </summary>
        public void Color_Big()
        {
            try
            {
                if (COLOR_BEGIN >= 0 && COLOR_BEGIN < COLOR_END)
                {
                    COLOR_BEGIN += 1;
                    if (COLOR_BEGIN % 2 == 0)
                    {
                        COLOR_CHANE(9);
                    }
                    else
                    {
                        COLOR_CHANE(10);
                    }
                }
                else
                {
                    COLOR_CHANE(3);
                    COLOR_BEGIN = 0;
                    _Timer7.Enabled = false;
                }
            }
            catch (Exception ee)
            {
                var mistake = "_Timer7_Tick方法失败" + ee.ToString();
            }
        }


        /// <summary>
        /// 定时器启用
        /// </summary>
        public void Timer_state()
        {
            _Timer6.Enabled = true;
        }
        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {
            _Timer6.Enabled = false;
        }
        /// <summary>
        /// 控件关闭
        /// </summary>
        public void _Clear()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 控制操作按钮是否允许操作
        /// </summary>
        public void Button_Show()
        {
            if (FALG_Oper)
            {
                simpleButton4.Enabled = true;
                simpleButton5.Enabled = true;
                simpleButton6.Enabled = true;
                simpleButton7.Enabled = true;
                simpleButton8.Enabled = true;
                simpleButton9.Enabled = true;
                simpleButton10.Enabled = true;
                simpleButton11.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                Check_R.Enabled = true;
                Check_C.Enabled = true;
                Check_MG.Enabled = true;
                Check_FK.Enabled = true;
            }
            else
            {
                simpleButton4.Enabled = false;
                simpleButton5.Enabled = false;
                simpleButton6.Enabled = false;
                simpleButton7.Enabled = false;
                simpleButton8.Enabled = false;
                simpleButton9.Enabled = false;
                simpleButton10.Enabled = false;
                simpleButton11.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                Check_R.Enabled = false;
                Check_C.Enabled = false;
                Check_MG.Enabled = false;
                Check_FK.Enabled = false;
            }
        }
        
    }
}
