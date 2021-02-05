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
using System.Configuration;
using LY_SINTER.PAGE.Parameter;
using LY_SINTER.PAGE.Quality;
using DevExpress.XtraNavBar;
using LY_SINTER.PAGE.Analysis;
using NBSJ_MAIN_UC;
using LY_SINTER.PAGE.Course;
using LY_SINTER.PAGE.HIS;

namespace LY_SINTER
{
    public partial class Form_Main : Form
    {
        Message_Logging logTable = new Message_Logging();//主框架通用方法
        MIX_PAGE_MODEL mIX_PAGE = new MIX_PAGE_MODEL();//配料页面方法
        /// <summary>
        /// 最大可以显示多少页面
        /// </summary>
        int PAGE_COUNT = 3;
        MIX_Intelligent _PAGE = new MIX_Intelligent();//配料页面
        #region 定时器声明
        /// <summary>
        /// 原料未处理弹出框
        /// </summary>
        public System.Timers.Timer _Timer1 { get; set; }
        /// <summary>
        /// 启停信号变化
        /// </summary>
        public System.Timers.Timer _Timer2 { get; set; }
        /// <summary>
        /// 水分or再用成分发生变化
        /// </summary>
        public System.Timers.Timer _Timer3 { get; set; }
        /// <summary>
        /// 消息通知
        /// </summary>
        public System.Timers.Timer _Timer4 { get; set; }
        #endregion
        public vLog _vLog { get; set; }
        public static Ingredient_auto _Auto;//未处理新原料弹出框
        /// <summary>
        /// 选项卡数量
        /// </summary>
        int index;
        int grade = User_Level.Authority;
        string user_name = User_Level.User_name;
        DBSQL dBSQL = new DBSQL(DataBase.ConstParameters.strCon);
        public Form_Main()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Main\\");
            Timer_Init();//定时器声明
            Show_INIT();//添加智能配料
            Navigation_INIT();//导航栏
            tableLayoutPanel2.RowStyles[4].Height = 0;//底部消息通知框隐藏
            simpleButton1.Text = "|||";//底部消息通知框隐藏
        }
        /// <summary>
        /// 定时器声明
        /// </summary>
        public void Timer_Init()
        {
            _Timer1 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { Timer1_Tick_1(); };//响应事件
            _Timer1.Enabled = true;
            _Timer1.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）


            _Timer2 = new System.Timers.Timer(5000);//初始化颜色变化定时器响应事件
            _Timer2.Elapsed += (x, y) => { Timer1_Tick_2(); };//响应事件
            _Timer2.Enabled = true;
            _Timer2.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer3 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer3.Elapsed += (x, y) => { Timer1_Tick_3(); };//响应事件
            _Timer3.Enabled = true;
            _Timer3.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer4 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer4.Elapsed += (x, y) => { Timer1_Tick_4(); };//响应事件
            _Timer4.Enabled = false;
            _Timer4.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
        }

        #region 定时器响应事件
        /// <summary>
        /// 三级传入新成分弹出框
        /// </summary>
        private void Timer1_Tick_1()
        {
            Action invokeAction = new Action(Timer1_Tick_1);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                Pop_show(1);
            }
        }
        /// <summary>
        /// 启停信号变化弹出框
        /// </summary>
        private void Timer1_Tick_2()
        {
            Action invokeAction = new Action(Timer1_Tick_2);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                Start_Stop_FLAG();
            }
        }
        /// <summary>
        /// 智能配料水分或者成分发生变化
        /// </summary>
        private void Timer1_Tick_3()
        {
            Action invokeAction = new Action(Timer1_Tick_3);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                INGRED_WATER_CHANGE();
            }
        }
        /// <summary>
        /// 消息通知消息
        /// </summary>
        private void Timer1_Tick_4()
        {
            Action invokeAction = new Action(Timer1_Tick_4);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                #region 消息检索弹出框
                string sql_MC_MESSAGE_INTERFACE = "select TIMESTAMP,MES_WEEK,MES_CONCENT,MES_PERSON from MC_MESSAGE_INTERFACE where MES_FLAG = '0'";
                DataTable data_MC_MESSAGE_INTERFACE = dBSQL.GetCommand(sql_MC_MESSAGE_INTERFACE);
                if (data_MC_MESSAGE_INTERFACE.Rows.Count > 0 && data_MC_MESSAGE_INTERFACE != null)
                {
                    dataGridView1.DataSource = data_MC_MESSAGE_INTERFACE;
                    tableLayoutPanel2.RowStyles[4].Height = 10;
                    simpleButton1.Text = "||||";
                }
                #endregion
            }
        }
        #endregion
        /// <summary>
        /// 智能配料启停状态变化判断
        /// </summary>
        public void Start_Stop_FLAG()
        {
            try
            {
                Tuple<bool, int> _tuple = mIX_PAGE._Signat_flag();
                if (_tuple.Item1)
                {
                    _PAGE.BUTTON_PBTZ();
                    _PAGE.BUTTON_PBQR();
                    mIX_PAGE.OVER_Storage(5, mIX_PAGE.Initialize_CAL_MODE());
                    _PAGE.latest_time(2);//最新下发时间
                    string name = "启停信号发生变化，重新计算设定下料量";
                    logTable.Operation_Log(name, "智能配料页面", "智能配料模型");
                    //切换页面
                    for (int i = 0; i < this.tabControl1.TabCount; i++)
                    {
                        if (this.tabControl1.TabPages[i].Text == "智能配料计算模型")
                            this.tabControl1.SelectedIndex = i;
                    }
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("Start_Stop_FLAG方法失败" + ee.ToString(), -1);
            }
        }
        /// <summary>
        /// 智能配料水分或者成分发生变化
        /// </summary>
        public void INGRED_WATER_CHANGE()
        {
            try
            {
                // //查询M_MATERIAL_BINS表，判断标志位ELEMENT_FLAG是否为1，若为一则执行配比调整及确认，优先级大于水分的计算设定下料
                Tuple<bool, int> _tuple = mIX_PAGE.Get_INIT_WATER();
                if (_tuple.Item1)
                {
                    if (_tuple.Item2 == 1)//成分发生变化
                    {
                        _PAGE.BUTTON_PBTZ();
                        _PAGE.BUTTON_PBQR();
                        mIX_PAGE.OVER_Storage(6, mIX_PAGE.Initialize_CAL_MODE());
                        _PAGE.latest_time(2);//最新下发时间
                        string name = "在用仓成分发生变化，计算设定下料量";
                        logTable.Operation_Log(name, "智能配料页面", "智能配料模型");
                    }
                    else if (_tuple.Item2 == 2)//水分发生变化
                    {
                        _PAGE.BUTTON_PBQR();
                        mIX_PAGE.OVER_Storage(6, mIX_PAGE.Initialize_CAL_MODE());
                        _PAGE.latest_time(2);//最新下发时间
                        string name = "在用仓水分发生变化，计算设定下料量";
                        logTable.Operation_Log(name, "智能配料页面", "智能配料模型");
                    }
                    string sql_flag = "update M_MATERIAL_BINS set Water_Flag1 = 0,ELEMENT_FLAG = 0";
                    int _coumt = dBSQL.CommandExecuteNonQuery(sql_flag);
                    if (_coumt <= 0)
                        _vLog.writelog("INGRED_WATER_CHANGE方法更新标志位错误" + sql_flag, -1);
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("INGRED_WATER_CHANGE方法调用失败" + ee.ToString(), -1);
            }

        }


        /// <summary>
        /// 周期查询原料新成分弹出框
        /// _flag =1 :通过判断M_MATERIAL_ANALYSIS表数据查询新成分
        /// _flag = 2：通过B_MATERIAL_CODE报文解析规则判断
        /// </summary>
        public void Pop_show(int _flag)
        {
            try
            {
                if (_flag == 1)
                {
                    #region 原料保护弹出框
                    string sql_M_MATERIAL_ANALYSIS_FLAG = "select L3_CODE from M_MATERIAL_ANALYSIS where FLAG = 1";
                    DataTable data_M_MATERIAL_ANALYSIS_FLAG = dBSQL.GetCommand(sql_M_MATERIAL_ANALYSIS_FLAG);
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
                    #endregion
                }
                else if (_flag == 2)
                {
                    //B_MATERIAL_CODE
                    //ProcessType	varchar(1)	数据处理类型	（i：插入；u：更新；d：删除）	

                    //删除物料规则功能
                    var sql_del = "select * from B_MATERIAL_CODE where ProcessType = 'D' AND FLAG = 1";
                    DataTable _DEL = dBSQL.GetCommand(sql_del);
                    if (_DEL != null && _DEL.Rows.Count > 0)
                    {
                        for (int x = 0; x < _DEL.Rows.Count; x++)
                        {
                            var DEL = "DELETE FROM M_MATERIAL_COOD WHERE L3_CODE = '" + _DEL.Rows[x]["MaterialCode"].ToString() + "'";
                            int COUNT = dBSQL.CommandExecuteNonQuery(DEL);
                            if (COUNT == 1)
                            {
                                _vLog.writelog("三级传入" + _DEL.Rows[x]["MaterialCode"].ToString() + "编码，M_MATERIAL_COOD表对应规则删除成功", 0);
                            }
                            else
                            {
                                _vLog.writelog("三级传入" + _DEL.Rows[x]["MaterialCode"].ToString() + "编码，M_MATERIAL_COOD表对应规则删除失败，已有的规则表中不存在当前品名", 0);
                            }
                        }
                    }
                    //更新物料规则功能
                    var sql_UPDATE = "select * from B_MATERIAL_CODE where ProcessType = 'U' AND FLAG = 1";
                    DataTable _UPDATE = dBSQL.GetCommand(sql_UPDATE);
                    if (_UPDATE != null && _UPDATE.Rows.Count > 0)
                    {
                        for (int x = 0; x < _UPDATE.Rows.Count; x++)
                        {
                            var DEL = "update  M_MATERIAL_COOD  set   L3_CODE = '" + _DEL.Rows[x]["MaterialName"].ToString() + "' where MAT_DESC = '" + _DEL.Rows[x]["MaterialName"].ToString() + "'";
                            int COUNT = dBSQL.CommandExecuteNonQuery(DEL);
                            if (COUNT == 1)
                            {
                                _vLog.writelog("三级传入" + _DEL.Rows[x]["MaterialCode"].ToString() + "编码，M_MATERIAL_COOD表对应规则删除成功", 0);
                            }
                            else
                            {
                                _vLog.writelog("三级传入" + _DEL.Rows[x]["MaterialCode"].ToString() + "编码，M_MATERIAL_COOD表对应规则删除失败，已有的规则表中不存在当前品名", 0);
                            }
                        }
                    }

                }
            }
            catch (Exception ee)
            {
                var mistake = "Pop_show方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }

        }
        /// <summary>
        /// 初始化添加智能配料&主页面
        /// </summary>
        public void Show_INIT()
        {
            TabPage tpg = new TabPage("智能配料计算模型");

            tpg.Controls.Add(_PAGE);
            tabControl1.TabPages.Add(tpg);
            _PAGE.BorderStyle = BorderStyle.None;
            _PAGE.Dock = DockStyle.Fill;
            _PAGE.Show();
            tabControl1.SelectedTab = tpg;
            tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
        }
        /// <summary>
        /// 导航栏
        /// </summary>
        public void Navigation_INIT()
        {
            try
            {
                string sql = "  select parentnode,childnode,producttext,isstop from [CODE_UDT_FORM_NODE_TB] where parentnode = '0'";
                DataTable dataTable = dBSQL.GetCommand(sql);
                for (int x = 0; x < dataTable.Rows.Count; x++)
                {
                    string name = dataTable.Rows[x]["producttext"].ToString();
                    string signal = dataTable.Rows[x]["childnode"].ToString();
                    int authority = int.Parse(dataTable.Rows[x]["isstop"].ToString());
                    if (grade >= authority)
                    {
                        NavBarGroup navBarControl = new NavBarGroup();
                        navBarControl.Name = x.ToString();
                        navBarControl.Caption = name;
                        navBarControl1.Groups.Add(navBarControl);
                        string sql_1 = "select producttext,classname,isstop from CODE_UDT_FORM_NODE_TB where parentnode = '" + signal + "' order by sequence asc";
                        DataTable dataTable_1 = dBSQL.GetCommand(sql_1);
                        for (int y = 0; y < dataTable_1.Rows.Count; y++)
                        {
                            int authority_1 = int.Parse(dataTable_1.Rows[y]["isstop"].ToString());
                            if (grade >= authority_1)
                            {
                                string name_1 = dataTable_1.Rows[y]["producttext"].ToString();
                                NavBarItem nbItem1 = new NavBarItem();
                                nbItem1.Name = y.ToString();
                                nbItem1.Caption = name_1;
                                navBarControl1.Items.Add(nbItem1);
                                navBarControl.ItemLinks.AddRange(new NavBarItemLink[] { new NavBarItemLink(nbItem1) });
                            }


                        }
                    }

                    // NavBarGroup nbGroup1 = new NavBarGroup();

                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("Navigation_INIT方法失败" + ee.ToString(), -1);
            }


        }
        /// <summary>
        /// 导航栏点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void navBarControl1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Add_TabPage(e.Link.Caption);//添加页面
        }
        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="str"></param>
        public void Add_TabPage(string str)
        {
            if (tabControlCheckHave(this.tabControl1, str))//判重页面
            {
                return;
            }
            else
            {


                #region 状态监控
                if (str == "料流定位监控页面")
                {
                    TabPage tpg = new TabPage(str);
                    MainUserControl mainUser = new MainUserControl();

                    tpg.Controls.Add(mainUser);
                    tabControl1.TabPages.Add(tpg);
                    mainUser.BorderStyle = BorderStyle.None;
                    mainUser.Dock = DockStyle.Fill;
                    mainUser.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";

                }
                else if (str == "生产实时曲线页面")
                {

                    TabPage tpg = new TabPage(str);
                    shishiquxianchaxun _PAGE = new shishiquxianchaxun();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                #endregion

                #region 质量优化
                else if (str == "智能配料计算模型")
                {

                    //TabPage tpg = new TabPage(str);
                    //zhinengpeiliao _PAGE = new zhinengpeiliao();
                    //tpg.Controls.Add(_PAGE);
                    //tabControl1.TabPages.Add(tpg);
                    //_PAGE.BorderStyle = BorderStyle.None;
                    //_PAGE.Dock = DockStyle.Fill;
                    //_PAGE.Show();
                    //tabControl1.SelectedTab = tpg;
                    //tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "质量自动控制模型")
                {

                    TabPage tpg = new TabPage(str);
                    Quality_automatic _shaojiezhongdian = new Quality_automatic();
                    tpg.Controls.Add(_shaojiezhongdian);
                    tabControl1.TabPages.Add(tpg);
                    _shaojiezhongdian.BorderStyle = BorderStyle.None;
                    _shaojiezhongdian.Dock = DockStyle.Fill;
                    _shaojiezhongdian.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "结矿成分预测模型")
                {

                    TabPage tpg = new TabPage(str);
                    Mine_Forecast _shaojiezhongdian = new Mine_Forecast();
                    tpg.Controls.Add(_shaojiezhongdian);
                    tabControl1.TabPages.Add(tpg);
                    _shaojiezhongdian.BorderStyle = BorderStyle.None;
                    _shaojiezhongdian.Dock = DockStyle.Fill;
                    _shaojiezhongdian.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "原料入仓追踪模型")
                {

                    //TabPage tpg = new TabPage(str);
                    //yuanliaorucang1 _PAGE = new yuanliaorucang1();
                    //tpg.Controls.Add(_PAGE);
                    //tabControl1.TabPages.Add(tpg);
                    //_PAGE.BorderStyle = BorderStyle.None;
                    //_PAGE.Dock = DockStyle.Fill;
                    //_PAGE.Show();
                    //tabControl1.SelectedTab = tpg;
                    //tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "返矿平衡控制模型")
                {

                    TabPage tpg = new TabPage(str);
                    Balance_Mine _shaojiezhongdian = new Balance_Mine();
                    tpg.Controls.Add(_shaojiezhongdian);
                    tabControl1.TabPages.Add(tpg);
                    _shaojiezhongdian.BorderStyle = BorderStyle.None;
                    _shaojiezhongdian.Dock = DockStyle.Fill;
                    _shaojiezhongdian.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "生产组织计划模型")
                {

                    TabPage tpg = new TabPage(str);
                    shengchanzuzhi _PAGE = new shengchanzuzhi();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "生产原料消耗模型")
                {

                    TabPage tpg = new TabPage(str);
                    shengchanyuanliaoxiaohao _PAGE = new shengchanyuanliaoxiaohao();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "铁矿粉基础性能")
                {

                    TabPage tpg = new TabPage(str);
                    tiekuangfenjichuxingneng _PAGE = new tiekuangfenjichuxingneng();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "烧结其他原料基础性能")
                {

                    TabPage tpg = new TabPage(str);
                    shaojieqitayuanliaojcxn _PAGE = new shaojieqitayuanliaojcxn();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "高炉入炉原料基础性能")
                {

                    TabPage tpg = new TabPage(str);
                    gaoluruluyuanliao _PAGE = new gaoluruluyuanliao();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "优化配矿计算模型")
                {

                    TabPage tpg = new TabPage(str);
                    youhuapeikuangjisuan _PAGE = new youhuapeikuangjisuan();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                #endregion

                #region 过程优化
                else if (str == "加水优化控制模型")
                {
                    TabPage tpg = new TabPage(str);
                    Add_Water _shaojiezhongdian = new Add_Water();
                    tpg.Controls.Add(_shaojiezhongdian);
                    tabControl1.TabPages.Add(tpg);
                    _shaojiezhongdian.BorderStyle = BorderStyle.None;
                    _shaojiezhongdian.Dock = DockStyle.Fill;
                    _shaojiezhongdian.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "矿槽料位控制模型")
                {

                    //TabPage tpg = new TabPage(str);
                    //hunheliaokuangcaoliaowei _PAGE = new hunheliaokuangcaoliaowei();
                    //tpg.Controls.Add(_PAGE);
                    //tabControl1.TabPages.Add(tpg);
                    //_PAGE.BorderStyle = BorderStyle.None;
                    //_PAGE.Dock = DockStyle.Fill;
                    //_PAGE.Show();
                    //tabControl1.SelectedTab = tpg;
                    //tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "烧结终点预测模型")
                {

                    TabPage tpg = new TabPage(str);
                    BTP _shaojiezhongdian = new BTP();
                    tpg.Controls.Add(_shaojiezhongdian);
                    tabControl1.TabPages.Add(tpg);
                    _shaojiezhongdian.BorderStyle = BorderStyle.None;
                    _shaojiezhongdian.Dock = DockStyle.Fill;
                    _shaojiezhongdian.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "终点偏差指导模型")
                {

                    TabPage tpg = new TabPage(str);
                    Deviation_guide _shaojiezhongdian = new Deviation_guide();
                    tpg.Controls.Add(_shaojiezhongdian);
                    tabControl1.TabPages.Add(tpg);
                    _shaojiezhongdian.BorderStyle = BorderStyle.None;
                    _shaojiezhongdian.Dock = DockStyle.Fill;
                    _shaojiezhongdian.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "风箱自动启停模型")
                {

                    //TabPage tpg = new TabPage(str);
                    //fengxiang _PAGE = new fengxiang();
                    //tpg.Controls.Add(_PAGE);
                    //tabControl1.TabPages.Add(tpg);
                    //_PAGE.BorderStyle = BorderStyle.None;
                    //_PAGE.Dock = DockStyle.Fill;
                    //_PAGE.Show();
                    //tabControl1.SelectedTab = tpg;
                    //tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "料层透气分析模型")
                {

                    TabPage tpg = new TabPage(str);
                    Bed_Permeability _shaojiezhongdian = new Bed_Permeability();
                    tpg.Controls.Add(_shaojiezhongdian);
                    tabControl1.TabPages.Add(tpg);
                    _shaojiezhongdian.BorderStyle = BorderStyle.None;
                    _shaojiezhongdian.Dock = DockStyle.Fill;
                    _shaojiezhongdian.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                #endregion

                #region 数据分析
                else if (str == "原料质量分析模型")
                {

                    TabPage tpg = new TabPage(str);
                    Raw_analysis _PAGE = new Raw_analysis();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "设备管理维护模型")
                {

                    //TabPage tpg = new TabPage(str);
                    //shebeiguanliweihuyemian _PAGE = new shebeiguanliweihuyemian();
                    //tpg.Controls.Add(_PAGE);
                    //tabControl1.TabPages.Add(tpg);
                    //_PAGE.BorderStyle = BorderStyle.None;
                    //_PAGE.Dock = DockStyle.Fill;
                    //_PAGE.Show();
                    //tabControl1.SelectedTab = tpg;
                    //tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "成品质量分析模型")
                {

                    TabPage tpg = new TabPage(str);
                    Ripe_analysis _PAGE = new Ripe_analysis();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "生产参数分析模型")
                {

                    TabPage tpg = new TabPage(str);
                    Production_state _PAGE = new Production_state();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }

                #endregion

                #region 历史数据
                else if (str == "生产历史趋势曲线")
                {

                    //TabPage tpg = new TabPage(str);
                    //lishiqushiquxianchaxun1 _PAGE = new lishiqushiquxianchaxun1();

                    //tpg.Controls.Add(_PAGE);
                    //tabControl1.TabPages.Add(tpg);
                    //_PAGE.BorderStyle = BorderStyle.None;
                    //_PAGE.Dock = DockStyle.Fill;
                    //_PAGE.Show();
                    //tabControl1.SelectedTab = tpg;
                    //tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "生产历史数据查询")
                {

                    //TabPage tpg = new TabPage(str);
                    //shengchanshujuxinxichaxun _PAGE = new shengchanshujuxinxichaxun();
                    //tpg.Controls.Add(_PAGE);
                    //tabControl1.TabPages.Add(tpg);
                    //_PAGE.BorderStyle = BorderStyle.None;
                    //_PAGE.Dock = DockStyle.Fill;
                    //_PAGE.Show();
                    //tabControl1.SelectedTab = tpg;
                    //tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "模型调整记录查询")
                {

                    TabPage tpg = new TabPage(str);
                    Model_records _PAGE = new Model_records();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "生产过程匹配追踪")
                {

                    //TabPage tpg = new TabPage(str);
                    //production_CSPP _PAGE = new production_CSPP();
                    //tpg.Controls.Add(_PAGE);
                    //tabControl1.TabPages.Add(tpg);
                    //_PAGE.BorderStyle = BorderStyle.None;
                    //_PAGE.Dock = DockStyle.Fill;
                    //_PAGE.Show();
                    //tabControl1.SelectedTab = tpg;
                    //tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "模型使用时间查询")
                {

                    //TabPage tpg = new TabPage(str);
                    //Model_Monitoring _PAGE = new Model_Monitoring();
                    //tpg.Controls.Add(_PAGE);
                    //tabControl1.TabPages.Add(tpg);
                    //_PAGE.BorderStyle = BorderStyle.None;
                    //_PAGE.Dock = DockStyle.Fill;
                    //_PAGE.Show();
                    //tabControl1.SelectedTab = tpg;
                    //tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                #endregion

                #region 参数维护
                else if (str == "原料成分维护页面")
                {

                    TabPage tpg = new TabPage(str);
                    Ingredient _PAGE = new Ingredient();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "原料成分保护页面")
                {

                    TabPage tpg = new TabPage(str);
                    Ingredient_Protect _PAGE = new Ingredient_Protect();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }

                else if (str == "通知消息录入页面")
                {

                    //TabPage tpg = new TabPage(str);
                    //tongzhixiaoxiyemian _PAGE = new tongzhixiaoxiyemian();
                    //tpg.Controls.Add(_PAGE);
                    //tabControl1.TabPages.Add(tpg);
                    //_PAGE.BorderStyle = BorderStyle.None;
                    //_PAGE.Dock = DockStyle.Fill;
                    //_PAGE.Show();
                    //tabControl1.SelectedTab = tpg;
                    //tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                else if (str == "排班规则维护页面")
                {

                    TabPage tpg = new TabPage(str);
                    Class_Plan _PAGE = new Class_Plan();
                    tpg.Controls.Add(_PAGE);
                    tabControl1.TabPages.Add(tpg);
                    _PAGE.BorderStyle = BorderStyle.None;
                    _PAGE.Dock = DockStyle.Fill;
                    _PAGE.Show();
                    tabControl1.SelectedTab = tpg;
                    tabControl1.SelectedTab.ToolTipText = "双击关闭页签";
                }
                #endregion


            }
        }
        /// <summary>
        /// 判重页面
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="_tabName"></param>
        /// <returns></returns>
        public bool tabControlCheckHave(TabControl tab, string _tabName)
        {

            index = tabControl1.TabPages.Count;
            foreach (TabPage p in tabControl1.TabPages)
            {
                if (_tabName.Equals(p.Text))
                {

                    #region 状态监控
                    if (_tabName == "料流定位监控页面")
                    {

                        MainUserControl _mainUser = (MainUserControl)p.Controls[0];
                        _mainUser.Show();
                        tabControl1.SelectedTab = p;
                        _mainUser.Timer_state();
                        return true;

                    }
                    //if (_tabName == "生产实时曲线页面")
                    //{
                    //    shishiquxianchaxun _mainUser = (shishiquxianchaxun)p.Controls[0];
                    //    _mainUser.Show();
                    //    tabControl1.SelectedTab = p;
                    //    //_mainUser.Timer_state();
                    //    return true;


                    #endregion

                    #region 质量优化
                    if (_tabName == "智能配料计算模型")
                    {

                        MIX_Intelligent _MIX_Intelligent = (MIX_Intelligent)p.Controls[0];
                        _MIX_Intelligent.Show();
                        tabControl1.SelectedTab = p;
                        _MIX_Intelligent.Timer_state();
                        return true;

                    }
                    else if (_tabName == "质量自动控制模型")
                    {

                        Quality_automatic _P = (Quality_automatic)p.Controls[0];
                        _P.Show();
                        tabControl1.SelectedTab = p;
                        _P.Timer_state();
                        return true;

                    }
                    else if (_tabName == "结矿成分预测模型")
                    {

                        Mine_Forecast _P = (Mine_Forecast)p.Controls[0];
                        _P.Show();
                        tabControl1.SelectedTab = p;
                        _P.Timer_state();
                        return true;

                    }
                    else if (_tabName == "原料入仓追踪模型")
                    {

                        //yuanliaorucang1 _yuanliaorucang = (yuanliaorucang1)p.Controls[0];

                        //_yuanliaorucang.Show();

                        //tabControl1.SelectedTab = p;
                        //_yuanliaorucang.Timer_state();
                        //return true;


                    }
                    else if (_tabName == "返矿平衡控制模型")
                    {

                        Balance_Mine _P = (Balance_Mine)p.Controls[0];
                        _P.Show();
                        tabControl1.SelectedTab = p;
                        _P.Timer_state();
                        return true;

                    }
                    else if (_tabName == "混匀矿换堆模型")
                    {

                        //hunyunkuanghuanduimoxing _PAGE = (hunyunkuanghuanduimoxing)p.Controls[0];
                        //_PAGE.Show();
                        //tabControl1.SelectedTab = p;
                        //_PAGE.Timer_state();
                        //return true;

                    }
                    else if (_tabName == "生产组织计划模型")
                    {
                        shengchanzuzhi _PAGE = (shengchanzuzhi)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;

                    }
                    else if (_tabName == "生产原料消耗模型")
                    {
                        shengchanyuanliaoxiaohao _PAGE = (shengchanyuanliaoxiaohao)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;

                    }
                    else if (_tabName == "铁矿粉基础性能")
                    {
                        tiekuangfenjichuxingneng _PAGE = (tiekuangfenjichuxingneng)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;

                    }
                    else if (_tabName == "烧结其他原料基础性能")
                    {
                        shaojieqitayuanliaojcxn _PAGE = (shaojieqitayuanliaojcxn)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;

                    }
                    else if (_tabName == "高炉入炉原料基础性能")
                    {
                        gaoluruluyuanliao _PAGE = (gaoluruluyuanliao)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;

                    }
                    else if (_tabName == "优化配矿计算模型")
                    {
                        youhuapeikuangjisuan _PAGE = (youhuapeikuangjisuan)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;
                    }

                    #endregion

                    #region 过程优化
                    else if (_tabName == "加水优化控制模型")
                    {
                        Add_Water _P = (Add_Water)p.Controls[0];
                        _P.Show();
                        tabControl1.SelectedTab = p;
                        _P.Timer_state();
                        return true;
                    }
                    else if (_tabName == "矿槽料位控制模型")
                    {

                        //hunheliaokuangcaoliaowei _hunheliaokuangcaoliaowei = (hunheliaokuangcaoliaowei)p.Controls[0];
                        //_hunheliaokuangcaoliaowei.Show();
                        //tabControl1.SelectedTab = p;
                        //_hunheliaokuangcaoliaowei.Timer_state();
                        //return true;

                    }
                    else if (_tabName == "烧结终点预测模型")
                    {
                        BTP _shaojiezhongdian = (BTP)p.Controls[0];
                        _shaojiezhongdian.Show();
                        tabControl1.SelectedTab = p;
                        _shaojiezhongdian.Timer_state();
                        return true;

                    }
                    else if (_tabName == "终点偏差指导模型")
                    {
                        Deviation_guide _shaojiezhongdian = (Deviation_guide)p.Controls[0];
                        _shaojiezhongdian.Show();
                        tabControl1.SelectedTab = p;
                        _shaojiezhongdian.Timer_state();
                        return true;

                    }
                    else if (_tabName == "风箱自动启停模型")
                    {

                        //fengxiang _fengxiang = (fengxiang)p.Controls[0];
                        //_fengxiang.Show();
                        //tabControl1.SelectedTab = p;
                        //_fengxiang.Timer_state();
                        //return true;
                    }
                    else if (_tabName == "料层透气分析模型")
                    {

                        Bed_Permeability _shaojiezhongdian = (Bed_Permeability)p.Controls[0];
                        _shaojiezhongdian.Show();
                        tabControl1.SelectedTab = p;
                        _shaojiezhongdian.Timer_state();
                        return true;

                    }
                    #endregion

                    #region 数据分析
                    else if (_tabName == "原料质量分析模型")
                    {

                        Raw_analysis _PAGE = (Raw_analysis)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;

                    }
                    else if (_tabName == "设备管理维护模型")
                    {

                        //shebeiguanliweihuyemian _PAGE = (shebeiguanliweihuyemian)p.Controls[0];
                        //_PAGE.Show();
                        //tabControl1.SelectedTab = p;
                        //_PAGE.Timer_state();
                        //return true;

                    }
                    else if (_tabName == "成品质量分析模型")
                    {
                        Ripe_analysis _PAGE = (Ripe_analysis)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;

                    }

                    else if (_tabName == "生产参数分析模型")
                    {
                        Production_state _PAGE = (Production_state)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;
                    }

                    #endregion

                    #region 历史数据
                    else if (_tabName == "生产历史趋势曲线")
                    {

                        //lishiqushiquxianchaxun1 _PAGE = (lishiqushiquxianchaxun1)p.Controls[0];
                        //_PAGE.Show();
                        //tabControl1.SelectedTab = p;
                    }
                    else if (_tabName == "生产历史数据查询")
                    {

                        //shengchanshujuxinxichaxun _PAGE = (shengchanshujuxinxichaxun)p.Controls[0];
                        //_PAGE.Show();
                        //tabControl1.SelectedTab = p;
                    }
                    else if (_tabName == "模型调整记录查询")
                    {

                        Model_records _PAGE = (Model_records)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;

                    }
                    else if (_tabName == "生产过程匹配追踪")
                    {
                        //   production_CSPP production_CSPP = new production_CSPP();
                        //tabControl1.SelectedTab = p;
                        //production_CSPP _PAGE = (production_CSPP)p.Controls[0];
                        //_PAGE.Show();
                        //tabControl1.SelectedTab = p;
                        //_PAGE.Timer_state();
                        //return true;

                    }
                    else if (_tabName == "模型使用时间查询")
                    {
                        ////   production_CSPP production_CSPP = new production_CSPP();
                        //tabControl1.SelectedTab = p;
                        //Model_Monitoring _PAGE = (Model_Monitoring)p.Controls[0];
                        //_PAGE.Show();
                        //tabControl1.SelectedTab = p;

                        //return true;
                    }

                    #endregion

                    #region 参数维护
                    else if (_tabName == "原料成分维护页面")
                    {
                        // Ingredient ingredient = new Ingredient();

                        Ingredient _PAGE = (Ingredient)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;

                    }
                    else if (_tabName == "原料成分保护页面")
                    {
                        Ingredient_Protect _PAGE = (Ingredient_Protect)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;

                    }
                    else if (_tabName == "通知消息录入页面")
                    {


                    }
                    else if (_tabName == "排班规则维护页面")
                    {
                        Class_Plan _PAGE = (Class_Plan)p.Controls[0];
                        _PAGE.Show();
                        tabControl1.SelectedTab = p;
                        _PAGE.Timer_state();
                        return true;

                    }
                    #endregion

                }
            }

            return false;
        }


        /// <summary>
        /// 顶部消息显示条按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (simpleButton3.Text == "||||")
            {
                tableLayoutPanel2.RowStyles[0].Height = 0;
                simpleButton3.Text = "|||";

            }
            else
               if (simpleButton3.Text == "|||")
            {
                tableLayoutPanel2.RowStyles[0].Height = 10;

            }
        }
        /// <summary>
        /// 左侧显示条按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel3.ColumnStyles[0].Width == 0)
            {
                tableLayoutPanel3.ColumnStyles[0].Width = 10;

            }
            else
            if (tableLayoutPanel3.ColumnStyles[0].Width == 10)
            {
                tableLayoutPanel3.ColumnStyles[0].Width = 0;

            }
        }
        /// <summary>
        /// 底部显示条按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (simpleButton1.Text == "||||")
            {
                tableLayoutPanel2.RowStyles[4].Height = 0;
                simpleButton1.Text = "|||";
                //把消息通知框的数据对应的标志位给置成1
                for (int rows = 0; rows < dataGridView1.Rows.Count; rows++)
                {
                    //消息通知对应的时间
                    string TIME = dataGridView1.Rows[rows].Cells["TIME"].Value.ToString();
                    string sql_update_MC_MESSAGE_INTERFACE = "update MC_MESSAGE_INTERFACE set MES_FLAG = '1' where TIMESTAMP = '" + TIME + "'";
                    dBSQL.CommandExecuteNonQuery(sql_update_MC_MESSAGE_INTERFACE);
                }
            }
        }
        /// <summary>
        /// 选项卡发生变化，作为顶级选项卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WhenSelected(object sender, TabControlEventArgs e)
        {
            if (index == 0)
            {
                return;
            }
            else
            {
                TabControl _tabControl1 = (TabControl)sender;
                string _str = _tabControl1.SelectedTab.Text;
                #region 状态监控
                if (_str == "料流定位监控页面")
                {
                    //     MainUserControl mainUser = new MainUserControl();
                    //    MainUserControl _mainUserControl = (MainUserControl)p.Controls[0];
                    //MainUserControl _mainUserControl = (MainUserControl)_tabControl1.SelectedTab.Controls[0];
                    //_mainUserControl.Timer_state();
                    MainUserControl vSelected = (MainUserControl)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();


                }
                if (_str == "生产实时曲线页面")
                {

                    shishiquxianchaxun _mainUserControl = (shishiquxianchaxun)_tabControl1.SelectedTab.Controls[0];
                    // _mainUserControl.Timer_state();
                }
                #endregion

                #region 质量优化
                else if (_str == "智能配料计算模型")
                {
                    //   zhinengpeiliao zhinengpeiliao = new zhinengpeiliao();
                    //zhinengpeiliao vSelected = (zhinengpeiliao)_tabControl1.SelectedTab.Controls[0];
                    //vSelected.Timer_state();


                }
                else if (_str == "质量自动控制模型")
                {
                    Quality_automatic vSelected = (Quality_automatic)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();

                }
                else if (_str == "结矿成分预测模型")
                {
                    Mine_Forecast vSelected = (Mine_Forecast)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();

                }
                else if (_str == "原料入仓追踪模型")
                {
                    // yuanliaorucang1 yuanliaorucang = new yuanliaorucang1();
                    //yuanliaorucang1 vSelected = (yuanliaorucang1)_tabControl1.SelectedTab.Controls[0];
                    //vSelected.Timer_state();

                }
                else if (_str == "返矿平衡控制模型")
                {
                    Balance_Mine vSelected = (Balance_Mine)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();
                    //   fankaungpinghengkongzhi fankaungpinghengkongzhi = new fankaungpinghengkongzhi();
                    //fankaungpinghengkongzhi vSelected = (fankaungpinghengkongzhi)_tabControl1.SelectedTab.Controls[0];
                    //vSelected.Timer_state();

                }
                else if (_str == "生产组织计划模型")
                {
                    shengchanzuzhi vSelected = (shengchanzuzhi)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();


                }
                else if (_str == "生产原料消耗模型")
                {
                    shengchanyuanliaoxiaohao vSelected = (shengchanyuanliaoxiaohao)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();


                }
                else if (_str == "铁矿粉基础性能")
                {
                    tiekuangfenjichuxingneng vSelected = (tiekuangfenjichuxingneng)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();


                }
                else if (_str == "烧结其他原料基础性能")
                {
                    shaojieqitayuanliaojcxn vSelected = (shaojieqitayuanliaojcxn)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();


                }
                else if (_str == "高炉入炉原料基础性能")
                {
                    gaoluruluyuanliao vSelected = (gaoluruluyuanliao)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();


                }
                else if (_str == "优化配矿计算模型")
                {
                    youhuapeikuangjisuan vSelected = (youhuapeikuangjisuan)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();

                }

                #endregion

                #region 过程优化
                else if (_str == "加水优化控制模型")
                {
                    Add_Water vSelected = (Add_Water)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();


                }
                else if (_str == "矿槽料位控制模型")
                {
                    //  hunheliaokuangcaoliaowei hunheliaokuangcaoliaowei = new hunheliaokuangcaoliaowei();
                    //hunheliaokuangcaoliaowei vSelected = (hunheliaokuangcaoliaowei)_tabControl1.SelectedTab.Controls[0];
                    //vSelected.Timer_state();

                }
                else if (_str == "烧结终点预测模型")
                {

                    //   shaojiezhongdian _shaojiezhongdian = (shaojiezhongdian)p.Controls[0];
                    BTP vSelected = (BTP)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();


                }
                else if (_str == "终点偏差指导模型")
                {
                    Deviation_guide vSelected = (Deviation_guide)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();
                }
                else if (_str == "风箱自动启停模型")
                {
                    //   fengxiang fengxiang = new fengxiang();
                    //fengxiang vSelected = (fengxiang)_tabControl1.SelectedTab.Controls[0];
                    //vSelected.Timer_state();

                }
                else if (_str == "料层透气分析模型")
                {
                    Bed_Permeability vSelected = (Bed_Permeability)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();

                }
                #endregion

                #region 数据分析
                else if (_str == "原料质量分析模型")
                {

                    Raw_analysis vSelected = (Raw_analysis)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();

                }
                else if (_str == "设备管理维护模型")
                {
                    //   shebeiguanliweihuyemian shebeiguanliweihuyemian = new shebeiguanliweihuyemian();
                    //shebeiguanliweihuyemian vSelected = (shebeiguanliweihuyemian)_tabControl1.SelectedTab.Controls[0];
                    //vSelected.Timer_state();
                }
                else if (_str == "成品质量分析模型")
                {
                    Ripe_analysis vSelected = (Ripe_analysis)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();

                }
                else if (_str == "生产参数分析模型")
                {
                    Production_state vSelected = (Production_state)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();
                }

                #endregion

                #region 历史数据
                else if (_str == "生产历史趋势曲线")
                {

                    //lishiqushiquxianchaxun1 vSelected = (lishiqushiquxianchaxun1)_tabControl1.SelectedTab.Controls[0];
                    //vSelected.Timer_state();
                }
                else if (_str == "生产历史数据查询")
                {

                    //shengchanshujuxinxichaxun vSelected = (shengchanshujuxinxichaxun)_tabControl1.SelectedTab.Controls[0];
                    //vSelected.Timer_state();
                }
                else if (_str == "模型调整记录查询")
                {

                    Model_records vSelected = (Model_records)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();
                }
                else if (_str == "生产过程匹配追踪")
                {
                    //   production_CSPP production_CSPP = new production_CSPP();
                    //production_CSPP vSelected = (production_CSPP)_tabControl1.SelectedTab.Controls[0];
                    //vSelected.Timer_state();

                }
                #endregion

                #region 参数维护
                else if (_str == "原料成分维护页面")
                {
                    //Ingredient ingredient = new Ingredient();
                    Ingredient vSelected = (Ingredient)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();

                }
                else if (_str == "原料成分保护页面")
                {
                    //   Ingredient_protect ingredient_Protect = new Ingredient_protect();
                    Ingredient_Protect vSelected = (Ingredient_Protect)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();

                }
                else if (_str == "通知消息录入页面")
                {
                    // tongzhixiaoxiyemian tongzhixiaoxiyemian = new tongzhixiaoxiyemian();
                    //tongzhixiaoxiyemian vSelected = (tongzhixiaoxiyemian)_tabControl1.SelectedTab.Controls[0];
                    //vSelected.Timer_state();

                }
                else if (_str == "排班规则维护页面")
                {
                    //  CLASS_RULE cLASS_RULE = new CLASS_RULE();
                    Class_Plan vSelected = (Class_Plan)_tabControl1.SelectedTab.Controls[0];
                    vSelected.Timer_state();

                }
                #endregion
                if (_tabControl1.TabPages.Count > PAGE_COUNT)
                {
                    foreach (TabPage p in _tabControl1.TabPages)
                    {
                        string _strname = p.Text;
                        if (!_str.Equals(_strname))
                        {

                            #region 状态监控
                            if (_strname == "料流定位监控页面")
                            {
                                ////       MainUserControl mainUser = new MainUserControl();
                                //MainUserControl _mainUserControl = (MainUserControl)p.Controls[0];
                                //_mainUserControl.Timer_stop();
                                ////  _mainUserControl.ControlRemoved();
                                //this.tabControl1.Controls.Remove(p);

                                //   _mainUserControl.UC_Close
                                MainUserControl vf1 = (MainUserControl)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;


                            }
                            if (_strname == "生产实时曲线页面")
                            {

                                shishiquxianchaxun vf1 = (shishiquxianchaxun)p.Controls[0];
                                //  vf1.Timer_stop();
                                this.tabControl1.Controls.Remove(p);
                            }
                            #endregion

                            #region 质量优化
                            else if (_strname == "质量自动控制模型")
                            {
                                Quality_automatic vf1 = (Quality_automatic)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;

                            }
                            else if (_strname == "结矿成分预测模型")
                            {
                                Mine_Forecast vf1 = (Mine_Forecast)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;

                            }
                            else if (_strname == "原料入仓追踪模型")
                            {
                                ////    yuanliaorucang1 yuanliaorucang = new yuanliaorucang1();
                                //yuanliaorucang1 vf1 = (yuanliaorucang1)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);

                            }
                            else if (_strname == "返矿平衡控制模型")
                            {
                                Balance_Mine vf1 = (Balance_Mine)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;

                            }
                            else if (_strname == "混匀矿换堆模型")
                            {

                                //hunyunkuanghuanduimoxing vf1 = (hunyunkuanghuanduimoxing)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);

                            }
                            else if (_strname == "生产组织计划模型")
                            {
                                shengchanzuzhi vf1 = (shengchanzuzhi)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;
                            }
                            else if (_strname == "生产原料消耗模型")
                            {
                                shengchanyuanliaoxiaohao vf1 = (shengchanyuanliaoxiaohao)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;


                            }
                            else if (_strname == "铁矿粉基础性能")
                            {
                                tiekuangfenjichuxingneng vf1 = (tiekuangfenjichuxingneng)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;


                            }
                            else if (_strname == "烧结其他原料基础性能")
                            {
                                shaojieqitayuanliaojcxn vf1 = (shaojieqitayuanliaojcxn)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;


                            }
                            else if (_strname == "高炉入炉原料基础性能")
                            {
                                gaoluruluyuanliao vf1 = (gaoluruluyuanliao)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;
                            }
                            else if (_strname == "优化配矿计算模型")
                            {
                                youhuapeikuangjisuan vf1 = (youhuapeikuangjisuan)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;
                            }

                            #endregion

                            #region 过程优化
                            else if (_strname == "加水优化控制模型")
                            {
                                Add_Water vf1 = (Add_Water)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;
                            }
                            else if (_strname == "矿槽料位控制模型")
                            {
                                //  hunheliaokuangcaoliaowei hunheliaokuangcaoliaowei = new hunheliaokuangcaoliaowei();
                                //hunheliaokuangcaoliaowei vf1 = (hunheliaokuangcaoliaowei)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);

                            }
                            else if (_strname == "烧结终点预测模型")
                            {


                                BTP vf1 = (BTP)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;

                            }
                            else if (_strname == "终点偏差指导模型")
                            {
                                Deviation_guide vf1 = (Deviation_guide)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;

                            }
                            else if (_strname == "风箱自动启停模型")
                            {
                                ////  fengxiang fengxiang = new fengxiang();
                                //fengxiang vf1 = (fengxiang)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);
                            }
                            else if (_strname == "料层透气分析模型")
                            {
                                Bed_Permeability vf1 = (Bed_Permeability)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;
                            }
                            #endregion

                            #region 数据分析
                            else if (_strname == "原料质量分析模型")
                            {

                                Raw_analysis vf1 = (Raw_analysis)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;
                            }
                            else if (_strname == "设备管理维护模型")
                            {
                                ////  shebeiguanliweihuyemian shebeiguanliweihuyemian = new shebeiguanliweihuyemian();
                                //shebeiguanliweihuyemian vf1 = (shebeiguanliweihuyemian)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);
                            }

                            else if (_strname == "成品质量分析模型")
                            {
                                Ripe_analysis vf1 = (Ripe_analysis)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;


                            }
                            else if (_strname == "生产参数分析模型")
                            {
                                Production_state vf1 = (Production_state)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;

                            }


                            #endregion

                            #region 历史数据
                            else if (_strname == "生产历史趋势曲线")
                            {

                                //lishiqushiquxianchaxun1 vf1 = (lishiqushiquxianchaxun1)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);
                            }
                            else if (_strname == "生产历史数据查询")
                            {

                                //shengchanshujuxinxichaxun vf1 = (shengchanshujuxinxichaxun)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);
                            }
                            else if (_strname == "模型调整记录查询")
                            {
                                //Ingredient ingredient = new Ingredient();
                                Model_records vf1 = (Model_records)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;

                            }
                            else if (_strname == "生产过程匹配追踪")
                            {
                                ////  production_CSPP production_CSPP = new production_CSPP();
                                //production_CSPP vf1 = (production_CSPP)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);

                            }
                            else if (_strname == "模型使用时间查询")
                            {
                                ////  production_CSPP production_CSPP = new production_CSPP();
                                //Model_Monitoring vf1 = (Model_Monitoring)p.Controls[0];

                                //this.tabControl1.Controls.Remove(p);

                            }

                            #endregion

                            #region 参数维护
                            else if (_strname == "原料成分维护页面")
                            {
                                //Ingredient ingredient = new Ingredient();
                                Ingredient vf1 = (Ingredient)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;

                            }
                            else if (_strname == "原料成分保护页面")
                            {
                                //  Ingredient_protect ingredient_Protect = new Ingredient_protect();
                                Ingredient_Protect vf1 = (Ingredient_Protect)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;

                            }
                            else if (_strname == "通知消息录入页面")
                            {
                                ////  tongzhixiaoxiyemian tongzhixiaoxiyemian = new tongzhixiaoxiyemian();
                                //tongzhixiaoxiyemian vf1 = (tongzhixiaoxiyemian)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);

                            }
                            else if (_strname == "排班规则维护页面")
                            {
                                //  CLASS_RULE cLASS_RULE = new CLASS_RULE();
                                Class_Plan vf1 = (Class_Plan)p.Controls[0];
                                vf1._Clear();
                                this.tabControl1.Controls.Remove(p);
                                return;

                            }
                            #endregion

                        }

                    }
                }
                else
                {
                    foreach (TabPage p in _tabControl1.TabPages)
                    {
                        string _strname = p.Text;
                        if (!_str.Equals(_strname))
                        {

                            #region 状态监控
                            if (_strname == "料流定位监控页面")
                            {
                                ////       MainUserControl mainUser = new MainUserControl();
                                //MainUserControl _mainUserControl = (MainUserControl)p.Controls[0];
                                //_mainUserControl.Timer_stop();
                                ////  _mainUserControl.ControlRemoved();
                                //this.tabControl1.Controls.Remove(p);

                                //   _mainUserControl.UC_Close
                                MainUserControl vf1 = (MainUserControl)p.Controls[0];
                                vf1.Timer_stop();


                            }
                            if (_strname == "生产实时曲线页面")
                            {

                                shishiquxianchaxun vf1 = (shishiquxianchaxun)p.Controls[0];
                                //  vf1.Timer_stop();
                                this.tabControl1.Controls.Remove(p);
                            }
                            #endregion

                            #region 质量优化
                            else if (_strname == "质量自动控制模型")
                            {
                                Quality_automatic vf1 = (Quality_automatic)p.Controls[0];
                                vf1.Timer_stop();

                            }
                            else if (_strname == "结矿成分预测模型")
                            {
                                Mine_Forecast vf1 = (Mine_Forecast)p.Controls[0];
                                vf1.Timer_stop();

                            }
                            else if (_strname == "原料入仓追踪模型")
                            {
                                ////    yuanliaorucang1 yuanliaorucang = new yuanliaorucang1();
                                //yuanliaorucang1 vf1 = (yuanliaorucang1)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);

                            }
                            else if (_strname == "返矿平衡控制模型")
                            {
                                Balance_Mine vf1 = (Balance_Mine)p.Controls[0];
                                vf1.Timer_stop();

                            }
                            else if (_strname == "混匀矿换堆模型")
                            {

                                //hunyunkuanghuanduimoxing vf1 = (hunyunkuanghuanduimoxing)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);

                            }
                            else if (_strname == "生产组织计划模型")
                            {
                                shengchanzuzhi vf1 = (shengchanzuzhi)p.Controls[0];
                                vf1.Timer_stop();
                            }
                            else if (_strname == "生产原料消耗模型")
                            {
                                shengchanyuanliaoxiaohao vf1 = (shengchanyuanliaoxiaohao)p.Controls[0];
                                vf1.Timer_stop();
                            }
                            else if (_strname == "铁矿粉基础性能")
                            {
                                tiekuangfenjichuxingneng vf1 = (tiekuangfenjichuxingneng)p.Controls[0];
                                vf1.Timer_stop();
                            }
                            else if (_strname == "烧结其他原料基础性能")
                            {
                                shaojieqitayuanliaojcxn vf1 = (shaojieqitayuanliaojcxn)p.Controls[0];
                                vf1.Timer_stop();


                            }
                            else if (_strname == "高炉入炉原料基础性能")
                            {
                                gaoluruluyuanliao vf1 = (gaoluruluyuanliao)p.Controls[0];
                                vf1.Timer_stop();
                            }
                            else if (_strname == "优化配矿计算模型")
                            {
                                youhuapeikuangjisuan vf1 = (youhuapeikuangjisuan)p.Controls[0];
                                vf1.Timer_stop();
                            }
                            #endregion

                            #region 过程优化
                            else if (_strname == "加水优化控制模型")
                            {
                                Add_Water vf1 = (Add_Water)p.Controls[0];
                                vf1.Timer_stop();

                            }
                            else if (_strname == "矿槽料位控制模型")
                            {
                                //  hunheliaokuangcaoliaowei hunheliaokuangcaoliaowei = new hunheliaokuangcaoliaowei();
                                //hunheliaokuangcaoliaowei vf1 = (hunheliaokuangcaoliaowei)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);

                            }
                            else if (_strname == "烧结终点预测模型")
                            {
                                BTP vf1 = (BTP)p.Controls[0];
                                vf1.Timer_stop();
                                //shaojiezhongdian vf1 = (shaojiezhongdian)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);

                            }
                            else if (_strname == "终点偏差指导模型")
                            {
                                Deviation_guide vf1 = (Deviation_guide)p.Controls[0];
                                vf1.Timer_stop();
                            }
                            else if (_strname == "风箱自动启停模型")
                            {
                                ////  fengxiang fengxiang = new fengxiang();
                                //fengxiang vf1 = (fengxiang)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);
                            }
                            else if (_strname == "料层透气分析模型")
                            {
                                Bed_Permeability vf1 = (Bed_Permeability)p.Controls[0];
                                vf1.Timer_stop();
                            }
                            #endregion

                            #region 数据分析
                            else if (_strname == "原料质量分析模型")
                            {
                                Raw_analysis vf1 = (Raw_analysis)p.Controls[0];
                                vf1.Timer_stop();
                            }
                            else if (_strname == "设备管理维护模型")
                            {
                                ////  shebeiguanliweihuyemian shebeiguanliweihuyemian = new shebeiguanliweihuyemian();
                                //shebeiguanliweihuyemian vf1 = (shebeiguanliweihuyemian)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);
                            }

                            else if (_strname == "成品质量分析模型")
                            {
                                Ripe_analysis vf1 = (Ripe_analysis)p.Controls[0];
                                vf1.Timer_stop();


                            }
                            else if (_strname == "生产参数分析模型")
                            {
                                Production_state vf1 = (Production_state)p.Controls[0];
                                vf1.Timer_stop();

                            }


                            #endregion

                            #region 历史数据
                            else if (_strname == "生产历史趋势曲线")
                            {

                                //lishiqushiquxianchaxun1 vf1 = (lishiqushiquxianchaxun1)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);
                            }
                            else if (_strname == "生产历史数据查询")
                            {

                                //shengchanshujuxinxichaxun vf1 = (shengchanshujuxinxichaxun)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);
                            }
                            else if (_strname == "模型调整记录查询")
                            {
                                Model_records vf1 = (Model_records)p.Controls[0];
                                vf1.Timer_stop();

                            }
                            else if (_strname == "生产过程匹配追踪")
                            {
                                ////  production_CSPP production_CSPP = new production_CSPP();
                                //production_CSPP vf1 = (production_CSPP)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);

                            }
                            else if (_strname == "模型使用时间查询")
                            {
                                ////  production_CSPP production_CSPP = new production_CSPP();
                                //Model_Monitoring vf1 = (Model_Monitoring)p.Controls[0];

                                //this.tabControl1.Controls.Remove(p);

                            }

                            #endregion

                            #region 参数维护
                            else if (_strname == "原料成分维护页面")
                            {
                                //Ingredient ingredient = new Ingredient();
                                Ingredient vf1 = (Ingredient)p.Controls[0];
                                vf1.Timer_stop();

                            }
                            else if (_strname == "原料成分保护页面")
                            {
                                //  Ingredient_protect ingredient_Protect = new Ingredient_protect();
                                Ingredient_Protect vf1 = (Ingredient_Protect)p.Controls[0];
                                vf1.Timer_stop();


                            }
                            else if (_strname == "通知消息录入页面")
                            {
                                ////  tongzhixiaoxiyemian tongzhixiaoxiyemian = new tongzhixiaoxiyemian();
                                //tongzhixiaoxiyemian vf1 = (tongzhixiaoxiyemian)p.Controls[0];
                                //vf1.Timer_stop();
                                //this.tabControl1.Controls.Remove(p);

                            }
                            else if (_strname == "排班规则维护页面")
                            {
                                //  CLASS_RULE cLASS_RULE = new CLASS_RULE();
                                Class_Plan vf1 = (Class_Plan)p.Controls[0];
                                vf1.Timer_stop();


                            }
                            #endregion

                        }

                    }
                }

            }
        }
        /// <summary>
        /// 双击关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tabControl1.TabPages[tabControl1.SelectedIndex].Text == "智能配料计算模型")
            {
                MessageBox.Show("配料页面不可关闭");
            }
            else
            {
                //tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
                TabControl _tabControl1 = (TabControl)sender;
                string str = _tabControl1.SelectedTab.Text;
                foreach (TabPage p in _tabControl1.TabPages)
                {
                    if (str.Equals(p.Text))
                    {
                        _tabControl1.SelectedTab.Parent = null;

                        #region 状态监控
                        if (str == "料流定位监控页面")
                        {

                            MainUserControl _PAGE = (MainUserControl)p.Controls[0];
                            _PAGE._Clear();

                        }
                        if (str == "生产实时曲线页面")
                        {
                            shishiquxianchaxun _PAGE = (shishiquxianchaxun)p.Controls[0];
                            _PAGE._Clear();

                        }
                        #endregion

                        #region 质量优化
                        else if (str == "质量自动控制模型")
                        {
                            Quality_automatic _PAGE = (Quality_automatic)p.Controls[0];
                            _PAGE._Clear();
                        }
                        else if (str == "结矿成分预测模型")
                        {
                            Mine_Forecast _PAGE = (Mine_Forecast)p.Controls[0];
                            _PAGE._Clear();

                        }
                        else if (str == "原料入仓追踪模型")
                        {
                            ////     yuanliaorucang1 yuanliaorucang = new yuanliaorucang1();
                            //yuanliaorucang1 _yuanliaorucang = (yuanliaorucang1)p.Controls[0];
                            //_yuanliaorucang.Timer_stop();

                        }
                        else if (str == "返矿平衡控制模型")
                        {
                            Balance_Mine _PAGE = (Balance_Mine)p.Controls[0];
                            _PAGE._Clear();

                        }

                        else if (str == "生产组织计划模型")
                        {
                            shengchanzuzhi _PAGE = (shengchanzuzhi)p.Controls[0];
                            _PAGE._Clear();

                        }
                        else if (str == "生产原料消耗模型")
                        {
                            shengchanyuanliaoxiaohao _PAGE = (shengchanyuanliaoxiaohao)p.Controls[0];
                            _PAGE._Clear();

                        }
                        else if (str == "铁矿粉基础性能")
                        {
                            tiekuangfenjichuxingneng _PAGE = (tiekuangfenjichuxingneng)p.Controls[0];
                            _PAGE._Clear();

                        }
                        else if (str == "烧结其他原料基础性能")
                        {
                            shaojieqitayuanliaojcxn _PAGE = (shaojieqitayuanliaojcxn)p.Controls[0];
                            _PAGE._Clear();

                        }
                        else if (str == "高炉入炉原料基础性能")
                        {
                            gaoluruluyuanliao _PAGE = (gaoluruluyuanliao)p.Controls[0];
                            _PAGE._Clear();

                        }
                        else if (str == "优化配矿计算模型")
                        {
                            youhuapeikuangjisuan _PAGE = (youhuapeikuangjisuan)p.Controls[0];
                            _PAGE._Clear();
                        }
                        #endregion

                        #region 过程优化
                        else if (str == "加水优化控制模型")
                        {
                            Add_Water _PAGE = (Add_Water)p.Controls[0];
                            _PAGE._Clear();

                        }
                        else if (str == "矿槽料位控制模型")
                        {
                            ////hunheliaokuangcaoliaowei hunheliaokuangcaoliaowei = new hunheliaokuangcaoliaowei();
                            //hunheliaokuangcaoliaowei _shaojiezhongdian = (hunheliaokuangcaoliaowei)p.Controls[0];
                            //_shaojiezhongdian.Timer_stop();

                        }
                        else if (str == "烧结终点预测模型")
                        {

                            BTP _PAGE = (BTP)p.Controls[0];
                            _PAGE._Clear();

                        }
                        else if (str == "终点偏差指导模型")
                        {
                            Deviation_guide _PAGE = (Deviation_guide)p.Controls[0];
                            _PAGE._Clear();
                        }
                        else if (str == "风箱自动启停模型")
                        {
                            ////    fengxiang fengxiang = new fengxiang();
                            //fengxiang _fengxiang = (fengxiang)p.Controls[0];
                            //_fengxiang.Timer_stop();

                        }
                        else if (str == "料层透气分析模型")
                        {
                            Bed_Permeability _PAGE = (Bed_Permeability)p.Controls[0];
                            _PAGE._Clear();

                        }
                        #endregion

                        #region 数据分析
                        else if (str == "原料质量分析模型")
                        {

                            Raw_analysis _PAGE = (Raw_analysis)p.Controls[0];
                            _PAGE._Clear();

                        }
                        else if (str == "设备管理维护模型")
                        {
                            ////  shebeiguanliweihuyemian shebeiguanliweihuyemian = new shebeiguanliweihuyemian();
                            //shebeiguanliweihuyemian _PAGE = (shebeiguanliweihuyemian)p.Controls[0];
                            //_PAGE.Timer_stop();

                        }
                        else if (str == "成品质量分析模型")
                        {
                            Ripe_analysis _PAGE = (Ripe_analysis)p.Controls[0];
                            _PAGE._Clear();
                        }
                        else if (str == "生产参数分析模型")
                        {
                            Production_state _PAGE = (Production_state)p.Controls[0];
                            _PAGE._Clear();

                        }



                        #endregion

                        #region 历史数据
                        else if (str == "生产历史趋势曲线")
                        {


                        }
                        else if (str == "生产历史数据查询")
                        {
                            ;
                        }
                        else if (str == "模型调整记录查询")
                        {

                            Model_records _PAGE = (Model_records)p.Controls[0];
                            _PAGE._Clear();
                        }
                        else if (str == "生产过程匹配追踪")
                        {



                        }



                        #endregion

                        #region 参数维护
                        else if (str == "原料成分维护页面")
                        {

                            Ingredient _PAGE = (Ingredient)p.Controls[0];
                            _PAGE._Clear();

                        }
                        else if (str == "原料成分保护页面")
                        {

                            Ingredient_Protect _PAGE = (Ingredient_Protect)p.Controls[0];
                            _PAGE._Clear();

                        }
                        else if (str == "通知消息录入页面")
                        {


                        }
                        else if (str == "排班规则维护页面")
                        {
                            //  CLASS_RULE cLASS_RULE = new CLASS_RULE();
                            Class_Plan _PAGE = (Class_Plan)p.Controls[0];
                            _PAGE._Clear();

                        }
                        #endregion

                    }
                }
            }
        }

        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();//关闭线程
        }
    }
}
