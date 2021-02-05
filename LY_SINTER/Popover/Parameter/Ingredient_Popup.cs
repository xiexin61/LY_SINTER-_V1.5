using DataBase;
using LY_SINTER.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLog;
using LY_SINTER.Custom;

namespace LY_SINTER.Popover.Parameter
{

    public partial class Ingredient_Popup : Form
    {
        /// <summary>
        /// code表对应关系
        /// key:物料名称
        /// value 二级编码
        /// </summary>
        Dictionary<string, int> _Dic = new Dictionary<string, int>();
        /// <summary>
        /// 物料种类及二级编码极值
        /// key：物料归属
        /// item1：二级编码最小值 itme2：二级编码最大值
        /// </summary>
        Dictionary<string, Tuple<int, int>> _keys = new Dictionary<string, Tuple<int, int>>();
        L2_CODE_CALSS _L2_CODE_CALSS = new L2_CODE_CALSS();
        //批号
        string PH = "";
        /// <summary>
        /// 成分计算方式
        /// </summary>
        int SINTER_METHOD = 0;
        /// <summary>
        /// 物料种类是否显示标志位
        /// </summary>
        bool _flag = false;
        /// <summary>
        /// 仓号
        /// </summary>
        public int CH = Ingredient_Model.Data;
        public vLog _vLog { get; set; }
        //声明委托和事件
        public delegate void TransfDelegate_YLWH();
        //声明委托和事件
        public event TransfDelegate_YLWH _TransfDelegate_YLWH;
        public static bool isopen = false;
        DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        public Ingredient_Popup()
        {
            InitializeComponent();
            time_begin_end();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Parametery\\Ingredient_Pop\\");
            L2_NAME_RULE();//收集规则数据
            Init_text();//初始化数据
            _flag = true;//物料种类是否显示标志位
            combox_look();
        }
        /// <summary>
        /// 开始时间-结束时间赋值
        /// </summary>
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddDays(-7);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();

        }
        /// <summary>
        /// 物料归属下拉框赋值
        /// insert _l2_code:二级编码
        /// retuen item1 :最小值 item1 :最大值
        /// </summary>
        /// <param name="_l2_code"></param>
        public Tuple<int, int> Combox_text_1(int _l2_code)
        {
            try
            {
                string sql = " select M_DESC as name ,M_TYPE as value ,CODE_MIN,CODE_MAX from [M_MATERIAL_COOD_CONFIG] order by M_TYPE asc";
                DataTable dataTable = _dBSQL.GetCommand(sql);
                if (dataTable.Rows.Count > 0 && dataTable != null)
                {
                    this.comboBox1.DataSource = dataTable;
                    this.comboBox1.DisplayMember = "name";
                    this.comboBox1.ValueMember = "value";
                    string sql_1 = " select M_DESC ,CODE_MIN,CODE_MAX from [M_MATERIAL_COOD_CONFIG] where  CODE_MIN <= " + _l2_code + "  and  CODE_MAX >= " + _l2_code + " ";
                    DataTable dataTable_1 = _dBSQL.GetCommand(sql_1);
                    if (dataTable_1.Rows.Count > 0 && dataTable_1 != null)
                    {
                        this.comboBox1.Text = dataTable_1.Rows[0][0].ToString();
                        return new Tuple<int, int>(int.Parse(dataTable_1.Rows[0]["CODE_MIN"].ToString()), int.Parse(dataTable_1.Rows[0]["CODE_MAX"].ToString()));
                    }
                    else
                    {
                        return new Tuple<int, int>(0, 0);
                    }
                }
                else
                {
                    return new Tuple<int, int>(0, 0);
                }

            }
            catch (Exception ee)
            {
                var mistake = "弹出框Combox_text_1方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return new Tuple<int, int>(0, 0);
            }

        }
        /// <summary>
        /// 物料种类下拉框赋值
        /// item1 :最小值 item1 :最大值
        /// </summary>
        /// <param name="_tup"></param>
        public void Combox_text_2(Tuple<int, int> _tup)
        {
            try
            {
                // 查询所有物料名称和二级编码
                string sql_3 = "select L2_CODE,MAT_DESC from M_MATERIAL_COOD where L2_CODE >= '" + _tup.Item1 + "' and L2_CODE <= '" + _tup.Item2 + "' order by L2_CODE asc";
                DataTable dataTable_3 = _dBSQL.GetCommand(sql_3);
                if (dataTable_3.Rows.Count > 0 && dataTable_3 != null)
                {
                    DataTable data_1 = new DataTable();
                    data_1.Columns.Add("NAME");
                    data_1.Columns.Add("VALUES");
                    for (int x = 0; x < dataTable_3.Rows.Count; x++)
                    {
                        DataRow row_1 = data_1.NewRow();
                        row_1["NAME"] = dataTable_3.Rows[x]["MAT_DESC"].ToString();
                        row_1["VALUES"] = int.Parse(dataTable_3.Rows[x]["L2_CODE"].ToString());
                        data_1.Rows.Add(row_1);
                    }
                    this.comboBox2.DataSource = data_1;
                    this.comboBox2.DisplayMember = "NAME";
                    this.comboBox2.ValueMember = "VALUES";
                }
            }
            catch (Exception ee)
            {
                var mistake = "弹出框Combox_text_2方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 初始化查询
        /// </summary>
        public void Init_text()
        {
            try
            {
                //****仓号、物料名称、二级编码、成分维护状态、原料追踪状态、加权平均批数
                string sql_1 = " SELECT" +
                    " a.BIN_NUM_SHOW,b.MAT_DESC,a.L2_CODE," +
                    "a.STATE,a.P_T_FLAG,a.NUMBER_FLAG,a.C_TFE,a.C_FEO,a.C_CAO," +
                    "a.C_SIO2,a.C_AL2O3,a.C_MGO,a.C_S,a.C_P,a.C_C,a.C_MN,a.C_LOT," +
                    "a.C_R,a.C_H2O,a.C_ASH,a.C_VOLATILES,a.C_TIO2,a.C_K2O,a.C_NA2O," +
                    "a.C_PBO,a.C_ZNO,a.C_F,a.C_AS,a.C_CU,a.C_PB,a.C_ZN,a.C_K,a.C_NA," +
                    "a.C_CR,a.C_NI,a.C_MNO " +
                    "FROM M_MATERIAL_BINS a,M_MATERIAL_COOD b " +
                    "where a.L2_CODE = b.L2_CODE and BIN_NUM_SHOW = " + CH + "";
                DataTable dataTable_1 = _dBSQL.GetCommand(sql_1);
                if (dataTable_1.Rows.Count > 0 && dataTable_1 != null)
                {
                    //查询修改仓号对应的物料编码
                    int WLBM = int.Parse(dataTable_1.Rows[0]["L2_CODE"].ToString());
                    //查询修改仓号对应的物料名称
                    string WLMC = dataTable_1.Rows[0]["MAT_DESC"].ToString();
                    //查询修改仓号的维护状态
                    int WHZT = int.Parse(dataTable_1.Rows[0]["STATE"].ToString());
                    //查询修改仓号的原料追踪状态
                    int YLZZ = int.Parse(dataTable_1.Rows[0]["P_T_FLAG"].ToString());
                    //查询修改仓号的加权平均批数
                    string JQPS = dataTable_1.Rows[0]["NUMBER_FLAG"].ToString();
                    Tuple<int, int> tup_1 = Combox_text_1(WLBM);//物料归属下拉框赋值
                    Combox_text_2(tup_1);//物料下拉框赋值
                    //物料下拉框默认显示
                    this.comboBox2.Text = WLMC;
                    //****维护状态下拉框     
                    List<INFO_1> list_1 = new List<INFO_1>();
                    INFO_1 iNFO_1 = new INFO_1() { name = "手动维护", value = 0 };
                    INFO_1 iNFO_2 = new INFO_1() { name = "自动维护", value = 1 };
                    list_1.Add(iNFO_1);
                    list_1.Add(iNFO_2);

                    this.comboBox3.DataSource = list_1;
                    this.comboBox3.DisplayMember = "name";
                    this.comboBox3.ValueMember = "value";
                    //维护状态下拉框默认赋值
                    this.comboBox3.SelectedIndex = WHZT;


                    //****原料追踪下拉框
                    List<INFO_2> list_2 = new List<INFO_2>();
                    INFO_2 iNFO_3 = new INFO_2() { name = "禁用", value = 0 };
                    //   INFO_2 iNFO_4 = new INFO_2() { name = "启用", value = 1 };
                    list_2.Add(iNFO_3);
                    //  list_2.Add(iNFO_4);
                    this.comboBox4.DataSource = list_2;
                    this.comboBox4.DisplayMember = "name";
                    this.comboBox4.ValueMember = "value";
                    //原料追踪下拉框默认赋值
                    this.comboBox4.Text = "禁用";

                    //**加权批数赋值
                    this.textBox1.Text = JQPS;

                    //***当前仓使用数据赋值
                    this.dataGridView2.AutoGenerateColumns = false;
                    this.dataGridView2.DataSource = dataTable_1;
                    show_d2(WLBM, WHZT);//展示数据

                    //****窗体四部分功能说明赋值
                    this.label1.Text = "当前用户操作： " + CH + "号仓";
                    this.label2.Text = "原料检验成分";
                    this.label3.Text = "当前在用成分";
                    this.label4.Text = "预计使用成分";
                }
            }
            catch (Exception ee)
            {
                var mistake = "Init_text()成分弹出框初始化失败，" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 展示数据
        /// _l2_code = 二级编码
        /// _WHZT：维护状态
        /// </summary>
        /// <param name="_l2_code"></param>
        public void show_d2(int _l2_code, int _WHZT)
        {
            try
            {
                //**展示数据   前十条
                string sql_show = "select top 10 ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) AS ID,TIMESTAMP,BATCH_NUM," +
                    "SAMPLETIME,REOPTTIME,C_TFE," +
                    "C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C," +
                    "C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES,C_TIO2," +
                    "C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN," +
                    "C_K,C_NA,C_CR,C_NI,C_MNO  from M_MATERIAL_ANALYSIS" +
                    " where L2_CODE = " + _l2_code + " order by timestamp desc";
                DataTable dataTable_show = _dBSQL.GetCommand(sql_show);
                this.dataGridView1.AutoGenerateColumns = false;
                ///初始化判断，若检化验数据为空，则判断使用的物料维护状态是否为自动维护，若为自动维护则在加权批数中默认显示0并且不能修改
                if (dataTable_show.Rows.Count > 0)
                {
                    this.dataGridView1.DataSource = dataTable_show;
                }
                else
                {
                    if (_WHZT == 1)
                    {
                        textBox1.Text = "0";
                        textBox1.ReadOnly = true;
                    }
                }
            }
            catch (Exception ee)
            {
                var mistake = "show_d2方法失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }

        }

        /// <summary>
        /// 成分维护状态  
        ///  name 维护方式
        ///  value 维护标志位
        /// </summary>
        public class INFO_1
        {
            public int value { get; set; }
            public string name { get; set; }
        }
        /// <summary>
        /// 原料追踪状态  
        ///  name 开关
        ///  value 开关标志位
        /// </summary>
        public class INFO_2
        {
            public int value { get; set; }
            public string name { get; set; }
        }
        /// <summary>
        /// 物料归属下拉框响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int index = this.comboBox1.SelectedIndex;
            //索引号从0开始，+1转换为类别号
            index = index + 1;
            string sql = "  select M_TYPE,M_DESC,CODE_MIN,CODE_MAX from M_MATERIAL_COOD_CONFIG where M_TYPE = " + index + "";
            DataTable dataTable = _dBSQL.GetCommand(sql);
            int min = int.Parse(dataTable.Rows[0]["CODE_MIN"].ToString());
            int max = int.Parse(dataTable.Rows[0]["CODE_MAX"].ToString());
            Combox_text_2(new Tuple<int, int>(min, max));
        }
        /// <summary>
        /// 物料名称下拉框响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (_flag)
            {
                //   int L2_CODE = _Dic[comboBox2.Text.ToString()];_GET_L2CODE
                int L2_CODE = _GET_L2CODE(comboBox1.Text.ToString(),comboBox2.Text.ToString());
                string _A1 = comboBox3.Text.ToString();
                if (_A1 == "自动维护")
                {
                    show_d2(L2_CODE, 1);//展示数据
                }
                else
                {
                    show_d2(L2_CODE, 0);//展示数据
                }
            }

        }
        /// <summary>
        /// 计算成分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox5.Text == "系统计算成分")
                {
                    //维护状态
                    string WHZT = comboBox3.Text;
                    //维护状态标志位 0 :手动维护; 1 :自动维护 ;2 :自动维护2
                    int WHZT_SIGNAL = 0;
                    if (WHZT == "手动维护")
                    {
                        WHZT_SIGNAL = 0;
                    }
                    else
                    if (WHZT == "自动维护")
                    {
                        WHZT_SIGNAL = 1;
                    }
                    else
                    if (WHZT == "自动维护2")
                    {
                        WHZT_SIGNAL = 2;
                    }
                    else
                    {
                        MessageBox.Show("维护状态有误");
                        return;
                    }
                    //原料追踪状态
                    string YLZZ = comboBox4.Text;
                    int YLZZ_SIGNAL = 0;
                    if (YLZZ == "启用")
                    {
                        YLZZ_SIGNAL = 1;
                    }
                    else
                    if (YLZZ == "禁用")
                    {
                        YLZZ_SIGNAL = 0;
                    }
                    else
                    {
                        MessageBox.Show("原料追踪状态有误");
                        return;
                    }
                    //加权平均数
                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("请输入正确的加权批数");
                        var mistake = "点击计算按钮，输入的加权批数有误";
                        _vLog.writelog(mistake, -1);
                        return;
                    }
                    else
                    {
                        int JQPJ = int.Parse(textBox1.Text);

                        if (WHZT_SIGNAL == 0)//维护状态为手动维护
                        {
                            //原料追踪状态为禁用
                            if (YLZZ_SIGNAL == 0)
                            {
                                //加权平均数等于1
                                if (JQPJ == 1)
                                {
                                    category_2_2();
                                    category_4_3();
                                    SINTER_METHOD = 9;
                                    //提示用户进行修改
                                    this.label4.Text = "预计使用成分-(成分可修改)";
                                    this.label4.BackColor = Color.Yellow;
                                    this.dataGridView3.ReadOnly = false;
                                }
                                else
                                //加权平均数等于0
                                if (JQPJ == 0)
                                {
                                    category_2_2();
                                    category_4_5();
                                    SINTER_METHOD = 8;
                                    //提示用户进行修改
                                    this.label4.Text = "预计使用成分-(成分可修改)";
                                    this.label4.BackColor = Color.Yellow;
                                    this.dataGridView3.ReadOnly = false;

                                }
                                else
                                //加权平均数大于1
                                if (JQPJ > 1)
                                {
                                    category_2_2();
                                    category_4_4();
                                    SINTER_METHOD = 10;
                                    //提示用户进行修改
                                    this.label4.Text = "预计使用成分-(成分可修改)";
                                    this.label4.BackColor = Color.Yellow;
                                    this.dataGridView3.ReadOnly = false;
                                }
                                else
                                {
                                    MessageBox.Show("维护状态为手动维护，原料追踪状态为禁用，加权平均数异常");
                                }
                            }
                            else
                            //原料追踪状态为启用
                            if (YLZZ_SIGNAL == 1)
                            {
                                //加权平均数等于1
                                if (JQPJ == 1)
                                {
                                    category_2_1();
                                    category_4_1();
                                    SINTER_METHOD = 6;
                                    //提示用户进行修改
                                    this.label4.Text = "当前：" + CH + "仓计算成分   --  (成分可修改)";
                                    this.label4.BackColor = Color.Red;
                                    this.dataGridView3.ReadOnly = false;
                                }
                                else
                                //加权平均数等于0
                                if (JQPJ == 0)
                                {
                                    category_2_1();
                                    category_4_5();
                                    SINTER_METHOD = 5;
                                    //提示用户进行修改
                                    this.label4.Text = "当前：" + CH + "仓计算成分   --  (成分可修改)";
                                    this.label4.BackColor = Color.Red;
                                    this.dataGridView3.ReadOnly = false;
                                }
                                else
                                //加权平均数大于1
                                if (JQPJ > 1)
                                {
                                    category_2_1();
                                    category_4_2();
                                    SINTER_METHOD = 7;
                                    //提示用户进行修改
                                    this.label4.Text = "当前：" + CH + "仓计算成分   --  (成分可修改)";
                                    this.label4.BackColor = Color.Red;
                                    this.dataGridView3.ReadOnly = false;

                                }
                                else
                                {
                                    MessageBox.Show("维护状态为手动维护，原料追踪状态为启用，加权平均数异常");
                                    string mistake = "维护状态为手动维护，原料追踪状态为启用，加权平均数异常";
                                    _vLog.writelog(mistake, -1);
                                }
                            }
                            else
                            {
                                MessageBox.Show("维护状态为手动维护正常，原料追踪状态报错");
                                string mistake = "维护状态为手动维护正常，原料追踪状态报错";
                                _vLog.writelog(mistake, -1);
                            }

                        }
                        else
                        if (WHZT_SIGNAL == 1) //维护状态为自动维护
                        {
                            //原料追踪状态为禁用
                            if (YLZZ_SIGNAL == 0)
                            {
                                //加权平均数等于1
                                if (JQPJ == 1)
                                {
                                  //  int WLBM = _Dic[comboBox2.Text];
                                    int WLBM = _GET_L2CODE(comboBox1.Text.ToString(), comboBox2.Text.ToString());
                                    string sql_PH = "select top 1 BATCH_NUM  from M_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                                    DataTable dataTable = _dBSQL.GetCommand(sql_PH);
                                    PH = (dataTable.Rows[0]["BATCH_NUM"].ToString());
                                    category_2_2();
                                    category_4_3();
                                    SINTER_METHOD = 3;
                                    //提示用户进行修改
                                    this.label4.Text = "预计使用成分";
                                    this.label4.BackColor = Color.LightBlue;
                                    this.dataGridView3.ReadOnly = true;
                                }
                                else
                                //加权平均数大于1
                                if (JQPJ > 1)
                                {
                                    category_2_2();
                                    category_4_4();
                                    SINTER_METHOD = 4;
                                    this.label4.Text = "预计使用成分";
                                    this.label4.BackColor = Color.LightBlue;
                                    this.dataGridView3.ReadOnly = true;
                                }
                                else
                                {
                                    MessageBox.Show("维护状态为自动维护，原料追踪状态为禁用，加权平均数异常");
                                    string mistake = "维护状态为自动维护，原料追踪状态为禁用，加权平均数异常";
                                    _vLog.writelog(mistake, -1);
                                }
                            }
                            else
                            //原料追踪状态为启用
                            if (YLZZ_SIGNAL == 1)
                            {
                                //加权平均数等于1
                                if (JQPJ == 1)
                                {
                                    category_2_1();
                                    category_4_1();
                                    SINTER_METHOD = 1;
                                    this.label4.Text = "预计使用成分";
                                    this.label4.BackColor = Color.LightBlue;
                                    this.dataGridView3.ReadOnly = true;
                                }
                                else
                                //加权平均数大于1
                                if (JQPJ > 1)
                                {
                                    category_2_1();
                                    category_4_2();
                                    SINTER_METHOD = 2;
                                    this.label4.Text = "预计使用成分";
                                    this.label4.BackColor = Color.LightBlue;
                                    this.dataGridView3.ReadOnly = true;
                                }
                                else
                                {
                                    MessageBox.Show("维护状态为自动维护，原料追踪状态为启用，加权平均数异常");
                                    string mistake = "维护状态为自动维护，原料追踪状态为启用，加权平均数异常";
                                    _vLog.writelog(mistake, -1);
                                }
                            }
                            else
                            {
                                MessageBox.Show("维护状态为手动维护正常，原料追踪状态报错");
                                string mistake = "维护状态为手动维护正常，原料追踪状态报错";
                                _vLog.writelog(mistake, -1);
                            }
                        }
                        else
                        if (WHZT_SIGNAL == 2)//维护状态为自动维护2
                        {
                            //原料追踪状态为禁用
                            if (YLZZ_SIGNAL == 0)
                            {
                                category_2_1();
                                category_4_5();
                                this.label4.Text = "预计使用成分";
                                this.label4.BackColor = Color.LightBlue;
                                this.dataGridView3.ReadOnly = true;
                            }
                            else
                            //原料追踪状态为启用
                            if (YLZZ_SIGNAL == 1)
                            {
                                //加权平均数等于1
                                if (JQPJ == 1)
                                {
                                    category_2_1();
                                    category_4_1();
                                    SINTER_METHOD = 1;
                                    this.label4.Text = "预计使用成分";
                                    this.label4.BackColor = Color.LightBlue;
                                    this.dataGridView3.ReadOnly = true;
                                }
                                else
                                //加权平均数大于1
                                if (JQPJ > 1)
                                {
                                    category_2_1();
                                    category_4_2();
                                    SINTER_METHOD = 2;
                                    this.label4.Text = "预计使用成分";
                                    this.label4.BackColor = Color.LightBlue;
                                    this.dataGridView3.ReadOnly = true;
                                }
                                else
                                {
                                    MessageBox.Show("维护状态为自动维护，原料追踪状态为启用，加权平均数异常");
                                    string mistake = "维护状态为自动维护，原料追踪状态为启用，加权平均数异常";
                                    _vLog.writelog(mistake, -1);
                                }
                            }
                            else
                            {
                                MessageBox.Show("维护状态为手动维护正常，原料追踪状态报错");
                                string mistake = "维护状态为手动维护正常，原料追踪状态报错";
                                _vLog.writelog(mistake, -1);
                            }
                        }
                        else
                        {
                            MessageBox.Show("维护状态错误");
                            string mistake = "维护状态错误";
                            _vLog.writelog(mistake, -1);
                        }
                    }

                }
                else
                {
                    int _ch = 0;
                    //获取选择仓的仓#
                    if (comboBox5.Text == "使用2#仓成分")
                    {
                        _ch = 2;
                    }
                    if (comboBox5.Text == "使用3#仓成分")
                    {
                        _ch = 3;
                    }
                    if (comboBox5.Text == "使用4#仓成分")
                    {
                        _ch = 4;
                    }
                    if (comboBox5.Text == "使用5#仓成分")
                    {
                        _ch = 5;
                    }
                    if (comboBox5.Text == "使用6#仓成分")
                    {
                        _ch = 6;
                    }
                    if (comboBox5.Text == "使用7#仓成分")
                    {
                        _ch = 7;
                    }
                    if (comboBox5.Text == "使用8#仓成分")
                    {
                        _ch = 8;
                    }
                    if (comboBox5.Text == "使用9#仓成分")
                    {
                        _ch = 9;
                    }
                    if (comboBox5.Text == "使用10#仓成分")
                    {
                        _ch = 10;
                    }
                    if (comboBox5.Text == "使用11#仓成分")
                    {
                        _ch = 11;
                    }
                    if (comboBox5.Text == "使用13#仓成分")
                    {
                        _ch = 13;
                    }
                    if (comboBox5.Text == "使用14#仓成分")
                    {
                        _ch = 14;
                    }
                    if (comboBox5.Text == "使用15#仓成分")
                    {
                        _ch = 15;
                    }
                    if (comboBox5.Text == "使用16#仓成分")
                    {
                        _ch = 16;
                    }
                    if (comboBox5.Text == "使用17#仓成分")
                    {
                        _ch = 17;
                    }
                    if (comboBox5.Text == "使用18#仓成分")
                    {
                        _ch = 18;
                    }
                    if (comboBox5.Text == "使用19#仓成分")
                    {
                        _ch = 19;
                    }
                    if (comboBox5.Text == "使用20#仓成分")
                    {
                        _ch = 20;
                    }

                    var sql = "select [C_TFE],[C_FEO] ,[C_CAO],[C_SIO2] ,[C_AL2O3] ,[C_MGO] ,[C_S] ,[C_P] ,[C_C] ,[C_MN] ,[C_LOT] ,[C_R],[C_H2O] ,[C_ASH] ,[C_VOLATILES] ,[C_TIO2] ,[C_K2O] ,[C_NA2O] ,[C_PBO] ,[C_ZNO],[C_F] ,[C_AS] ,[C_CU] ,[C_PB] ,[C_ZN],[C_K] ,[C_NA],[C_CR],[C_NI] ,[C_MNO] from  M_MATERIAL_BINS where BIN_NUM_SHOW = " + _ch + "";
                    DataTable data = _dBSQL.GetCommand(sql);
                    if (data.Rows.Count > 0)
                    {
                        this.dataGridView3.AutoGenerateColumns = false;
                        this.dataGridView3.DataSource = data;
                    }
                }
            }
            catch (Exception ee)
            {
                var mistake = "弹出框点击计算按钮失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            d1_text();
        }
        /// <summary>
        /// 查询按钮
        /// </summary>
        public void d1_text()
        {
            try
            {
             //   int WLBM = _Dic[comboBox2.Text];
                int WLBM = _GET_L2CODE(comboBox1.Text.ToString(), comboBox2.Text.ToString());

                string time_begin = textBox_begin.Text.ToString();
                string time_end = textBox_end.Text.ToString();
                string sql = "select ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) AS ID, " +
                    "TIMESTAMP,BATCH_NUM,SAMPLETIME,REOPTTIME,C_TFE,C_FEO,C_CAO,C_SIO2," +
                    "C_AL2O3,C_MGO,C_S,C_P,C_C,C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES," +
                    "C_TIO2,C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN,C_K,C_NA," +
                    "C_CR,C_NI,C_MNO  from M_MATERIAL_ANALYSIS " +
                    " where L2_CODE = " + WLBM + " AND  TIMESTAMP >= '" + time_begin + "' AND TIMESTAMP < = '" + time_end + "'  ORDER BY TIMESTAMP DESC ";
                DataTable dataTable = _dBSQL.GetCommand(sql);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ee)
            {
                string mistake = "弹出框查询按钮失败 " + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 刷新按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                //  int WLBM = _Dic[comboBox2.Text];
                int WLBM = _GET_L2CODE(comboBox1.Text.ToString(), comboBox2.Text.ToString());

                string sql = "select TOP 10 ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) AS ID, " +
                    "TIMESTAMP,BATCH_NUM,SAMPLETIME,REOPTTIME,C_TFE,C_FEO,C_CAO,C_SIO2," +
                    "C_AL2O3,C_MGO,C_S,C_P,C_C,C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES," +
                    "C_TIO2,C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN,C_K,C_NA," +
                    "C_CR,C_NI,C_MNO  from M_MATERIAL_ANALYSIS  where L2_CODE = " + WLBM + "  ORDER BY TIMESTAMP DESC ";
                DataTable dataTable = _dBSQL.GetCommand(sql);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ee)
            {
                string mistake = "弹出框刷新按钮失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
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
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            if (i > 0)
                            {
                                columnTitle += "\t";
                            }
                            columnTitle += dataGridView1.Columns[i].HeaderText;
                        }
                        sw.WriteLine(columnTitle);

                        //写入列内容    
                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                        {
                            string columnValue = "";
                            for (int k = 0; k < dataGridView1.Columns.Count; k++)
                            {
                                if (k > 0)
                                {
                                    columnValue += "\t";
                                }
                                if (dataGridView1.Rows[j].Cells[k].Value == null)
                                    columnValue += "";
                                else
                                    columnValue += dataGridView1.Rows[j].Cells[k].Value.ToString().Trim();
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
            catch (Exception ee)
            {
                string mistake = "导出按钮失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 第二部分 原料表最新10条记录，且标注模型结果表最新一条记录位置
        /// </summary>
        public void category_2_1()
        {
            try
            {
                #region 数据准备
                //    int WLBM = _Dic[comboBox2.Text];
                //   int WLBM = _GET_L2CODE(comboBox1.Text.ToString(), comboBox2.Text.ToString());
                int WLBM = _GET_L2CODE(comboBox1.Text.ToString(), comboBox2.Text.ToString());

                #endregion
                string sql_show = "select top 10 ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) AS ID, TIMESTAMP,BATCH_NUM,SAMPLETIME,REOPTTIME,C_TFE," +
                                       "C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C," +
                                       "C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES,C_TIO2," +
                                       "C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN," +
                                       "C_K,C_NA,C_CR,C_NI,C_MNO  from M_MATERIAL_ANALYSIS" +
                                       " where L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                DataTable dataTable_show = _dBSQL.GetCommand(sql_show);
                this.dataGridView1.AutoGenerateColumns = false;
                //  this.dataGridView1.DataSource = null;
                this.dataGridView1.DataSource = dataTable_show;
                //展示数据判断对应模型表的数据颜色变化
                string sql_model = "select top (1)  SAMPLE_CODE from MC_PARTICLE_TRACKING_RESULT  where L2_CODE = " + WLBM + " and TIMESTAMP = (select max(TIMESTAMP) from MC_PARTICLE_TRACKING_RESULT) ";
                DataTable dataTable_model = _dBSQL.GetCommand(sql_model);
                if (dataTable_model.Rows.Count > 0)
                {
                    string SAMPLE_CODE = dataTable_model.Rows[0]["SAMPLE_CODE"].ToString();
                    for (int x = 0; x < dataGridView1.Rows.Count; x++)
                    {
                        //页批号
                        string time_page = dataGridView1.Rows[x].Cells["Column5"].Value.ToString();
                        if (SAMPLE_CODE == time_page)
                        {
                            dataGridView1.Rows[x].DefaultCellStyle.ForeColor = Color.Red;
                            break;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                string mistake = "原料表最新10条记录，且标注模型结果表最新一条记录位置查询失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }


        }
        /// <summary>
        /// 第二部分 原料表最新10条记录
        /// </summary>
        public void category_2_2()
        {
            try
            {
                //  int WLBM = _Dic[comboBox2.Text];
                int WLBM = _GET_L2CODE(comboBox1.Text.ToString(), comboBox2.Text.ToString());

                string sql_show = "select top 10 ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) AS ID, TIMESTAMP,BATCH_NUM,SAMPLETIME,REOPTTIME,C_TFE," +
                                        "C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C," +
                                        "C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES,C_TIO2," +
                                        "C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN," +
                                        "C_K,C_NA,C_CR,C_NI,C_MNO  from M_MATERIAL_ANALYSIS" +
                                        " where L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                DataTable dataTable_show = _dBSQL.GetCommand(sql_show);
                this.dataGridView1.AutoGenerateColumns = false;
                if (dataTable_show.Rows.Count > 0)
                {
                    this.dataGridView1.DataSource = dataTable_show;
                }
            }
            catch (Exception ee)
            {
                string mistake = "第二部分，显示原料表最新10条记录失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }

        }
        /// <summary>
        /// 第四部分 默认显示模型结果表最新一条记录成分
        /// </summary>
        public void category_4_1()
        {
            try
            {
                // int WLBM = _Dic[comboBox2.Text];
                int WLBM = _GET_L2CODE(comboBox1.Text.ToString(), comboBox2.Text.ToString());

                int Flag = _L2_CODE_CALSS.L2_code_Judeg(WLBM);
                //混匀矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                if (Flag == 1)
                {
                    List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                    string sql_Count = "select top (1) " +
                         "TIMESTAMP," +
                         "C_TFE," +
                         "C_FEO," +
                         "C_CAO," +
                         "C_SIO2," +
                         "C_AL2O3," +
                         "C_MGO," +
                         "C_S," +
                         "C_P," +
                         "C_C," +
                         "C_MN," +
                         "C_R," +
                         "C_ASH," +
                         "C_VOLATILES," +
                         "C_TIO2," +
                         "C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN," +
                         "C_K,C_NA,C_CR,C_NI,C_MNO  from MC_PARTICLE_TRACKING_RESULT" +
                         " where L2_CODE = " + WLBM + " " +
                         " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                         " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                         " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                         " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                         " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                         " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                         " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                         " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                         " order by TIMESTAMP desc";
                    DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                    if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                    {
                        this.dataGridView3.AutoGenerateColumns = false;
                        this.dataGridView3.DataSource = dataTable_Count;
                        //20200913添加逻辑，使用再用水分及烧损
                        string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                        DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                            dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_bins;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败（特殊成分：再用水分烧损）" + sql_Count;
                        _vLog.writelog(mistake, -1);
                    }
                }
                //矿粉
                else if (Flag == 2)
                {
                    MessageBox.Show("选择更新的成分为矿粉，规则中不存在矿粉判断");
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为矿粉，规则不包含";
                    _vLog.writelog(mistake, -1);
                }
                //燃料 s、C、灰分、挥发灰、cao、sio2、mgo、al2o3、p、k2o、nao2上下限查询条件
                else if (Flag == 3)
                {
                    List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                    string sql_Count = "select top (1) TIMESTAMP,C_TFE," +
                         "C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C," +
                         "C_MN,C_R,C_ASH,C_VOLATILES,C_TIO2," +
                         "C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN," +
                         "C_K,C_NA,C_CR,C_NI,C_MNO  from MC_PARTICLE_TRACKING_RESULT" +
                         " where L2_CODE = " + WLBM + " " +
                         " and C_S <=" + _list[0] + " and C_S >= " + _list[1] + " " +
                         " and C_C <=" + _list[2] + " and C_C >= " + _list[3] + "" +
                         " and C_ASH <=" + _list[4] + " and C_ASH >= " + _list[5] + "" +
                         " and C_VOLATILES <=" + _list[6] + " and C_VOLATILES >= " + _list[7] + "" +
                         " and C_CAO <=" + _list[8] + " and C_CAO >= " + _list[9] + "" +
                         " and C_SIO2 <=" + _list[10] + " and C_SIO2 >= " + _list[11] + "" +
                         " and C_MGO <=" + _list[12] + " and C_MGO >= " + _list[13] + "" +
                         " and C_AL2O3 <=" + _list[14] + " and C_AL2O3 >= " + _list[15] + "" +
                         " and C_P <=" + _list[16] + " and C_P >= " + _list[17] + "" +
                         " and C_K2O <=" + _list[18] + " and C_K2O >= " + _list[19] + "" +
                         " and C_NA2O <=" + _list[20] + " and C_NA2O >= " + _list[21] + "" +
                         " order by TIMESTAMP desc";
                    DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                    if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                    {
                        this.dataGridView3.AutoGenerateColumns = false;
                        this.dataGridView3.DataSource = dataTable_Count;
                        //20200913添加逻辑，使用再用水分及烧损
                        string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                        DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                            dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_bins;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败（特殊成分：再用水分烧损）" + sql_Count;
                        _vLog.writelog(mistake, -1);
                    }
                }
                //熔剂 CAO SIO2 MGO AL2O3 P S 
                else if (Flag == 4)
                {
                    List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                    string sql_Count = "select top (1) TIMESTAMP," +
                        "C_TFE," +
                         "C_FEO," +
                         "C_CAO," +
                         "C_SIO2," +
                         "C_AL2O3," +
                         "C_MGO," +
                         "C_S," +
                         "C_P,C_C," +
                         "C_MN," +
                         //  "C_LOT," +
                         "C_R," +
                         // "C_H2O," +
                         "C_ASH,C_VOLATILES,C_TIO2," +
                         "C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN," +
                         "C_K,C_NA,C_CR,C_NI,C_MNO  from MC_PARTICLE_TRACKING_RESULT" +
                         " where L2_CODE = " + WLBM + " " +
                         " and C_CAO <=" + _list[0] + " and C_CAO >= " + _list[1] + "" +
                         " and C_SIO2 <=" + _list[2] + " and C_SIO2 >= " + _list[3] + "" +
                         " and C_MGO <=" + _list[4] + " and C_MGO >= " + _list[5] + "" +
                         " and C_AL2O3 <=" + _list[6] + " and C_AL2O3 >= " + _list[7] + "" +
                         " and C_P <=" + _list[8] + " and C_P >= " + _list[9] + "" +
                         " and C_S <=" + _list[10] + " and C_S >= " + _list[11] + "" +
                         " order by TIMESTAMP desc";
                    DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                    if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                    {
                        this.dataGridView3.AutoGenerateColumns = false;
                        this.dataGridView3.DataSource = dataTable_Count;
                        //20200913添加逻辑，使用再用水分及烧损
                        string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                        DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                            dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_bins;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败（特殊成分：再用水分烧损）" + sql_Count;
                        _vLog.writelog(mistake, -1);
                    }
                }
                //除尘灰tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                else if (Flag == 5)
                {
                    List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                    string sql_Count = "select top (1) TIMESTAMP,C_TFE," +
                         "C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C," +
                         "C_MN,C_R,C_ASH,C_VOLATILES,C_TIO2," +
                         "C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN," +
                         "C_K,C_NA,C_CR,C_NI,C_MNO  from MC_PARTICLE_TRACKING_RESULT" +
                         " where L2_CODE = " + WLBM + " " +
                         " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                         " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                         " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                         " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                         " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                         " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                         " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                         " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                         " order by TIMESTAMP desc";
                    DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                    if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                    {
                        this.dataGridView3.AutoGenerateColumns = false;
                        this.dataGridView3.DataSource = dataTable_Count;
                        //20200913添加逻辑，使用再用水分及烧损
                        string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                        DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                            dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_bins;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败（特殊成分：再用水分烧损）" + sql_Count;
                        _vLog.writelog(mistake, -1);
                    }
                }
                //返矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                else if (Flag == 6)
                {
                    List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                    string sql_Count = "select top (1) TIMESTAMP,C_TFE," +
                         "C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C," +
                         "C_MN,C_R,C_ASH,C_VOLATILES,C_TIO2," +
                         "C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN," +
                         "C_K,C_NA,C_CR,C_NI,C_MNO  from MC_PARTICLE_TRACKING_RESULT" +
                         " where L2_CODE = " + WLBM + " " +
                         " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                         " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                         " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                         " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                         " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                         " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                         " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                         " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                         " order by TIMESTAMP desc";
                    DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                    if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                    {
                        this.dataGridView3.AutoGenerateColumns = false;
                        this.dataGridView3.DataSource = dataTable_Count;
                        //20200913添加逻辑，使用再用水分及烧损
                        string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                        DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                            dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_bins;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败（特殊成分：再用水分烧损）" + sql_Count;
                        _vLog.writelog(mistake, -1);
                    }
                }
                //烧结矿
                else if (Flag == 7)
                {
                    MessageBox.Show("选择更新的成分为矿粉，规则中不存在烧结矿");
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为烧结矿，规则不包含";
                    _vLog.writelog(mistake, -1);
                }
                //高炉炉渣
                else if (Flag == 8)
                {
                    MessageBox.Show("选择更新的成分为矿粉，规则中不存在高炉炉渣");
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为高炉炉渣，规则不包含";
                    _vLog.writelog(mistake, -1);
                }

                else if (Flag == 0)
                {
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类区分上下限失败";
                    _vLog.writelog(mistake, -1);
                }

            }
            catch (Exception ee)
            {
                string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }


        }
        /// <summary>
        /// 第四部分 默认显示模型结果表最新“加权批数”条的加权平均成分（结果表按照插入时间倒叙排序）
        /// </summary>
        public void category_4_2()
        {
            try
            {
                //  int WLBM = _Dic[comboBox2.Text];
                int WLBM = _GET_L2CODE(comboBox1.Text.ToString(), comboBox2.Text.ToString());

                if (textBox1.Text == "")
                {
                    MessageBox.Show("请输入正确的加权批数");
                    return;
                }
                else
                {
                    int Flag = _L2_CODE_CALSS.L2_code_Judeg(WLBM);
                    //混匀矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                    if (Flag == 1)
                    {
                        List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);

                        //页面获取加权批数
                        int JQPJ = int.Parse(textBox1.Text);
                        //**计算数据  默认显示模型结果表最新加权批数条数据（结果表时间倒叙）

                        //****20200913 添加逻辑 水分和烧损不参与计算，使用再用水分及烧损
                        string sql_Count = "select  " +
                            "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                            "AVG(isnull(C_FEO,0)) AS C_FEO," +
                            "AVG(isnull(C_CAO,0)) AS C_CAO," +
                            "AVG(isnull(C_SIO2,0)) AS C_SIO2," +
                            "AVG(isnull(C_AL2O3,0)) AS C_AL2O3 ," +
                            "AVG(isnull(C_MGO,0)) AS C_MGO," +
                            "AVG(isnull(C_S,0)) AS C_S," +
                            "AVG(isnull(C_P,0)) AS C_P," +
                            "AVG(isnull(C_C,0)) AS C_C," +
                            "AVG(isnull(C_MN,0)) AS C_MN," +
                            //"AVG(isnull(C_LOT,0)) AS C_LOT," +
                            "AVG(isnull(C_R,0)) AS C_R ," +
                            // "AVG(isnull(C_H2O,0)) AS C_H2O," +
                            "AVG(isnull(C_ASH,0)) AS C_ASH," +
                            "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                            "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                            "AVG(isnull(C_K2O,0)) AS C_K2O," +
                            "AVG(isnull(C_NA2O,0)) AS C_NA2O," +
                            "AVG(isnull(C_PBO,0)) AS C_PBO," +
                            "AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                            "AVG(isnull(C_F,0)) AS C_F," +
                            "AVG(isnull(C_AS,0)) AS C_AS," +
                            "AVG(isnull(C_CU,0)) AS C_CU," +
                            "AVG(isnull(C_PB,0)) AS C_PB," +
                            "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                            "AVG(isnull(C_K,0)) AS C_K," +
                            "AVG(isnull(C_NA,0)) AS C_NA," +
                            "AVG(isnull(C_CR,0)) AS C_CR," +
                            "AVG(isnull(C_NI,0)) AS C_NI," +
                            "AVG(isnull(C_MNO,0)) AS C_MNO" +
                            " from " +
                            "(SELECT TOP(" + JQPJ + ") * FROM MC_PARTICLE_TRACKING_RESULT where L2_CODE = " + WLBM + "" +
                              " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                             " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                             " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                             " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                             " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                             " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                             " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                             " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                            "   order by TIMESTAMP DESC) AS NET";
                        DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                        if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                        {
                            this.dataGridView3.AutoGenerateColumns = false;
                            //   this.dataGridView3.DataSource = null;
                            this.dataGridView3.DataSource = dataTable_Count;

                            //20200913添加逻辑，使用再用水分及烧损
                            string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                            DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                            }
                            else
                            {
                                string mistake = "成分计算查询再用仓水分及烧损失败" + sql_Count;
                                _vLog.writelog(mistake, -1);
                            }
                        }
                        else
                        {
                            string mistake = "成分计算查询失败" + sql_Count;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    //矿粉
                    else if (Flag == 2)
                    {
                        MessageBox.Show("选择更新的成分为矿粉，规则中不存在矿粉判断");
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为矿粉，规则不包含";
                        _vLog.writelog(mistake, -1);
                    }
                    //燃料 s、C、灰分、挥发灰、cao、sio2、mgo、al2o3、p、k2o、nao2上下限查询条件
                    else if (Flag == 3)
                    {
                        List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                        //页面获取加权批数
                        int JQPJ = int.Parse(textBox1.Text);
                        //**计算数据  默认显示模型结果表最新加权批数条数据（结果表时间倒叙）
                        string sql_Count = "select  " +
                            "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                            "AVG(isnull(C_FEO,0)) AS C_FEO," +
                            "AVG(isnull(C_CAO,0)) AS C_CAO," +
                            "AVG(isnull(C_SIO2,0)) AS C_SIO2," +
                            "AVG(isnull(C_AL2O3,0)) AS C_AL2O3 ," +
                            "AVG(isnull(C_MGO,0)) AS C_MGO," +
                            "AVG(isnull(C_S,0)) AS C_S," +
                            "AVG(isnull(C_P,0)) AS C_P," +
                            "AVG(isnull(C_C,0)) AS C_C," +
                            "AVG(isnull(C_MN,0)) AS C_MN," +
                            // "AVG(isnull(C_LOT,0)) AS C_LOT," +
                            "AVG(isnull(C_R,0)) AS C_R ," +
                            // "AVG(isnull(C_H2O,0)) AS C_H2O," +
                            "AVG(isnull(C_ASH,0)) AS C_ASH," +
                            "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                            "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                            "AVG(isnull(C_K2O,0)) AS C_K2O," +
                            "AVG(isnull(C_NA2O,0)) AS C_NA2O," +
                            "AVG(isnull(C_PBO,0)) AS C_PBO," +
                            "AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                            "AVG(isnull(C_F,0)) AS C_F," +
                            "AVG(isnull(C_AS,0)) AS C_AS," +
                            "AVG(isnull(C_CU,0)) AS C_CU," +
                            "AVG(isnull(C_PB,0)) AS C_PB," +
                            "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                            "AVG(isnull(C_K,0)) AS C_K," +
                            "AVG(isnull(C_NA,0)) AS C_NA," +
                            "AVG(isnull(C_CR,0)) AS C_CR," +
                            "AVG(isnull(C_NI,0)) AS C_NI," +
                            "AVG(isnull(C_MNO,0)) AS C_MNO" +
                            " from " +
                            "(SELECT TOP(" + JQPJ + ") * FROM MC_PARTICLE_TRACKING_RESULT where L2_CODE = " + WLBM + "" +
                            " and C_S <=" + _list[0] + " and C_S >= " + _list[1] + " " +
                             " and C_C <=" + _list[2] + " and C_C >= " + _list[3] + "" +
                             " and C_ASH <=" + _list[4] + " and C_ASH >= " + _list[5] + "" +
                             " and C_VOLATILES <=" + _list[6] + " and C_VOLATILES >= " + _list[7] + "" +
                             " and C_CAO <=" + _list[8] + " and C_CAO >= " + _list[9] + "" +
                             " and C_SIO2 <=" + _list[10] + " and C_SIO2 >= " + _list[11] + "" +
                             " and C_MGO <=" + _list[12] + " and C_MGO >= " + _list[13] + "" +
                             " and C_AL2O3 <=" + _list[14] + " and C_AL2O3 >= " + _list[15] + "" +
                             " and C_P <=" + _list[16] + " and C_P >= " + _list[17] + "" +
                             " and C_K2O <=" + _list[18] + " and C_K2O >= " + _list[19] + "" +
                             " and C_NA2O <=" + _list[20] + " and C_NA2O >= " + _list[21] + "" +
                            "   order by TIMESTAMP DESC) AS NET";
                        DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                        this.dataGridView3.AutoGenerateColumns = false;
                        //   this.dataGridView3.DataSource = null;
                        this.dataGridView3.DataSource = dataTable_Count;

                    }
                    //熔剂 CAO SIO2 MGO AL2O3 P S 
                    else if (Flag == 4)
                    {
                        List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                        //页面获取加权批数
                        int JQPJ = int.Parse(textBox1.Text);
                        //**计算数据  默认显示模型结果表最新加权批数条数据（结果表时间倒叙）
                        string sql_Count = "select  " +
                            "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                            "AVG(isnull(C_FEO,0)) AS C_FEO," +
                            "AVG(isnull(C_CAO,0)) AS C_CAO," +
                            "AVG(isnull(C_SIO2,0)) AS C_SIO2," +
                            "AVG(isnull(C_AL2O3,0)) AS C_AL2O3 ," +
                            "AVG(isnull(C_MGO,0)) AS C_MGO," +
                            "AVG(isnull(C_S,0)) AS C_S," +
                            "AVG(isnull(C_P,0)) AS C_P," +
                            "AVG(isnull(C_C,0)) AS C_C," +
                            "AVG(isnull(C_MN,0)) AS C_MN," +
                            "AVG(isnull(C_LOT,0)) AS C_LOT," +
                            "AVG(isnull(C_R,0)) AS C_R ," +
                            "AVG(isnull(C_H2O,0)) AS C_H2O," +
                            "AVG(isnull(C_ASH,0)) AS C_ASH," +
                            "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                            "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                            "AVG(isnull(C_K2O,0)) AS C_K2O," +
                            "AVG(isnull(C_NA2O,0)) AS C_NA2O," +
                            "AVG(isnull(C_PBO,0)) AS C_PBO," +
                            "AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                            "AVG(isnull(C_F,0)) AS C_F," +
                            "AVG(isnull(C_AS,0)) AS C_AS," +
                            "AVG(isnull(C_CU,0)) AS C_CU," +
                            "AVG(isnull(C_PB,0)) AS C_PB," +
                            "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                            "AVG(isnull(C_K,0)) AS C_K," +
                            "AVG(isnull(C_NA,0)) AS C_NA," +
                            "AVG(isnull(C_CR,0)) AS C_CR," +
                            "AVG(isnull(C_NI,0)) AS C_NI," +
                            "AVG(isnull(C_MNO,0)) AS C_MNO" +
                            " from " +
                            "(SELECT TOP(" + JQPJ + ") * FROM MC_PARTICLE_TRACKING_RESULT where L2_CODE = " + WLBM + "" +
                             " and C_CAO <=" + _list[0] + " and C_CAO >= " + _list[1] + "" +
                             " and C_SIO2 <=" + _list[2] + " and C_SIO2 >= " + _list[3] + "" +
                             " and C_MGO <=" + _list[4] + " and C_MGO >= " + _list[5] + "" +
                             " and C_AL2O3 <=" + _list[6] + " and C_AL2O3 >= " + _list[7] + "" +
                             " and C_P <=" + _list[8] + " and C_P >= " + _list[9] + "" +
                             " and C_S <=" + _list[10] + " and C_S >= " + _list[11] + "" +
                            "   order by TIMESTAMP DESC) AS NET";
                        DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                        this.dataGridView3.AutoGenerateColumns = false;
                        //   this.dataGridView3.DataSource = null;
                        this.dataGridView3.DataSource = dataTable_Count;
                    }
                    //除尘灰tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                    else if (Flag == 5)
                    {
                        List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                        //页面获取加权批数
                        int JQPJ = int.Parse(textBox1.Text);
                        //**计算数据  默认显示模型结果表最新加权批数条数据（结果表时间倒叙）
                        string sql_Count = "select  " +
                            "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                            "AVG(isnull(C_FEO,0)) AS C_FEO," +
                            "AVG(isnull(C_CAO,0)) AS C_CAO," +
                            "AVG(isnull(C_SIO2,0)) AS C_SIO2," +
                            "AVG(isnull(C_AL2O3,0)) AS C_AL2O3 ," +
                            "AVG(isnull(C_MGO,0)) AS C_MGO," +
                            "AVG(isnull(C_S,0)) AS C_S," +
                            "AVG(isnull(C_P,0)) AS C_P," +
                            "AVG(isnull(C_C,0)) AS C_C," +
                            "AVG(isnull(C_MN,0)) AS C_MN," +
                            "AVG(isnull(C_LOT,0)) AS C_LOT," +
                            "AVG(isnull(C_R,0)) AS C_R ," +
                            "AVG(isnull(C_H2O,0)) AS C_H2O," +
                            "AVG(isnull(C_ASH,0)) AS C_ASH," +
                            "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                            "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                            "AVG(isnull(C_K2O,0)) AS C_K2O," +
                            "AVG(isnull(C_NA2O,0)) AS C_NA2O," +
                            "AVG(isnull(C_PBO,0)) AS C_PBO," +
                            "AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                            "AVG(isnull(C_F,0)) AS C_F," +
                            "AVG(isnull(C_AS,0)) AS C_AS," +
                            "AVG(isnull(C_CU,0)) AS C_CU," +
                            "AVG(isnull(C_PB,0)) AS C_PB," +
                            "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                            "AVG(isnull(C_K,0)) AS C_K," +
                            "AVG(isnull(C_NA,0)) AS C_NA," +
                            "AVG(isnull(C_CR,0)) AS C_CR," +
                            "AVG(isnull(C_NI,0)) AS C_NI," +
                            "AVG(isnull(C_MNO,0)) AS C_MNO" +
                            " from " +
                            "(SELECT TOP(" + JQPJ + ") * FROM MC_PARTICLE_TRACKING_RESULT where L2_CODE = " + WLBM + "" +
                              " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                             " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                             " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                             " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                             " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                             " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                             " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                             " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                            "   order by TIMESTAMP DESC) AS NET";

                        DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                        this.dataGridView3.AutoGenerateColumns = false;
                        //   this.dataGridView3.DataSource = null;
                        this.dataGridView3.DataSource = dataTable_Count;
                    }
                    //返矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                    else if (Flag == 6)
                    {
                        List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                        //页面获取加权批数
                        int JQPJ = int.Parse(textBox1.Text);
                        //**计算数据  默认显示模型结果表最新加权批数条数据（结果表时间倒叙）
                        string sql_Count = "select  " +
                            "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                            "AVG(isnull(C_FEO,0)) AS C_FEO," +
                            "AVG(isnull(C_CAO,0)) AS C_CAO," +
                            "AVG(isnull(C_SIO2,0)) AS C_SIO2," +
                            "AVG(isnull(C_AL2O3,0)) AS C_AL2O3 ," +
                            "AVG(isnull(C_MGO,0)) AS C_MGO," +
                            "AVG(isnull(C_S,0)) AS C_S," +
                            "AVG(isnull(C_P,0)) AS C_P," +
                            "AVG(isnull(C_C,0)) AS C_C," +
                            "AVG(isnull(C_MN,0)) AS C_MN," +
                            "AVG(isnull(C_LOT,0)) AS C_LOT," +
                            "AVG(isnull(C_R,0)) AS C_R ," +
                            "AVG(isnull(C_H2O,0)) AS C_H2O," +
                            "AVG(isnull(C_ASH,0)) AS C_ASH," +
                            "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                            "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                            "AVG(isnull(C_K2O,0)) AS C_K2O," +
                            "AVG(isnull(C_NA2O,0)) AS C_NA2O," +
                            "AVG(isnull(C_PBO,0)) AS C_PBO," +
                            "AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                            "AVG(isnull(C_F,0)) AS C_F," +
                            "AVG(isnull(C_AS,0)) AS C_AS," +
                            "AVG(isnull(C_CU,0)) AS C_CU," +
                            "AVG(isnull(C_PB,0)) AS C_PB," +
                            "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                            "AVG(isnull(C_K,0)) AS C_K," +
                            "AVG(isnull(C_NA,0)) AS C_NA," +
                            "AVG(isnull(C_CR,0)) AS C_CR," +
                            "AVG(isnull(C_NI,0)) AS C_NI," +
                            "AVG(isnull(C_MNO,0)) AS C_MNO" +
                            " from " +
                            "(SELECT TOP(" + JQPJ + ") * FROM MC_PARTICLE_TRACKING_RESULT where L2_CODE = " + WLBM + "" +
                              " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                             " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                             " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                             " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                             " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                             " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                             " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                             " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                            "   order by TIMESTAMP DESC) AS NET";
                        DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                        this.dataGridView3.AutoGenerateColumns = false;
                        //   this.dataGridView3.DataSource = null;
                        this.dataGridView3.DataSource = dataTable_Count;
                    }
                    //烧结矿
                    else if (Flag == 7)
                    {
                        MessageBox.Show("选择更新的成分为矿粉，规则中不存在烧结矿");
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为烧结矿，规则不包含";
                        _vLog.writelog(mistake, -1);
                    }
                    //高炉炉渣
                    else if (Flag == 8)
                    {
                        MessageBox.Show("选择更新的成分为矿粉，规则中不存在高炉炉渣");
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为高炉炉渣，规则不包含";
                        _vLog.writelog(mistake, -1);
                    }

                    else if (Flag == 0)
                    {
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类区分上下限失败";
                        _vLog.writelog(mistake, -1);
                    }
                }

            }
            catch (Exception ee)
            {
                string mistake = "第四部分 默认显示模型结果表最新“加权批数”条的加权平均成分失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }


        }
        /// <summary>
        ///  第四部分   默认显示“第二部分”中最新一条记录成分（插入时间倒叙） 原料检化验表
        /// </summary>
        public void category_4_3()
        {
            try
            {
                //    int WLBM = _Dic[comboBox2.Text];
                int WLBM = _GET_L2CODE(comboBox1.Text.ToString(), comboBox2.Text.ToString());

                int Flag = _L2_CODE_CALSS.L2_code_Judeg(WLBM);
                //混匀矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                if (Flag == 1)
                {
                    List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                    //20200913添加逻辑，水分和烧损不参与计算，使用再用水分及烧损
                    string sql_Count = "select top (1) " +
                        "TIMESTAMP," +
                        "C_TFE," +
                         "C_FEO," +
                         "C_CAO," +
                         "C_SIO2," +
                         "C_AL2O3," +
                         "C_MGO," +
                         "C_S," +
                         "C_P," +
                         "C_C," +
                         "C_MN," +
                         //  "C_LOT," +
                         "C_R," +
                         // "C_H2O," +
                         "C_ASH," +
                         "C_VOLATILES," +
                         "C_TIO2," +
                         "C_K2O," +
                         "C_NA2O," +
                         "C_PBO," +
                         "C_ZNO," +
                         "C_F," +
                         "C_AS," +
                         "C_CU," +
                         "C_PB," +
                         "C_ZN," +
                         "C_K," +
                         "C_NA," +
                         "C_CR," +
                         "C_NI," +
                         "C_MNO" +
                         "  from M_MATERIAL_ANALYSIS" +
                         " where L2_CODE = " + WLBM + " " +
                         " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                         " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                         " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                         " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                         " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                         " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                         " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                         " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                         " order by TIMESTAMP desc";
                    DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);

                    if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                    {
                        this.dataGridView3.AutoGenerateColumns = false;
                        this.dataGridView3.DataSource = dataTable_Count;
                        //20200913添加逻辑，使用再用水分及烧损
                        string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                        DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                            dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败（特殊成分：再用水分烧损）" + sql_Count;
                        _vLog.writelog(mistake, -1);
                    }


                }
                //矿粉
                else if (Flag == 2)
                {
                    MessageBox.Show("选择更新的成分为矿粉，规则中不存在矿粉判断");
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为矿粉，规则不包含";
                    _vLog.writelog(mistake, -1);
                }
                //燃料 **特殊查询** s、C、灰分、挥发灰、每天可能报一条     
                //cao、sio2、mgo、al2o3、p、k2o、nao2为特殊成分，每周可能报一条
                else if (Flag == 3)
                {
                    List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM); ;
                    //页面获取加权批数
                    int JQPJ = int.Parse(textBox1.Text);
                    //**计算数据  默认显示模型结果表最新加权批数条数据（插入时间倒叙）
                    //20200913 添加逻辑，去除水分烧损计算，使用再用水分烧损
                    string sql_Count = "select  " +
                        "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                        "AVG(isnull(C_FEO,0)) AS C_FEO," +
                        "AVG(isnull(C_S,0)) AS C_S," +
                        "AVG(isnull(C_C,0)) AS C_C," +
                        "AVG(isnull(C_MN,0)) AS C_MN," +
                        // "AVG(isnull(C_LOT,0)) AS C_LOT," +
                        "AVG(isnull(C_R,0)) AS C_R ," +
                        // "AVG(isnull(C_H2O,0)) AS C_H2O," +
                        "AVG(isnull(C_ASH,0)) AS C_ASH," +
                        "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                        "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                        "AVG(isnull(C_PBO,0)) AS C_PBO," +
                        "AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                        "AVG(isnull(C_F,0)) AS C_F," +
                        "AVG(isnull(C_AS,0)) AS C_AS," +
                        "AVG(isnull(C_CU,0)) AS C_CU," +
                        "AVG(isnull(C_PB,0)) AS C_PB," +
                        "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                        "AVG(isnull(C_K,0)) AS C_K," +
                        "AVG(isnull(C_NA,0)) AS C_NA," +
                        "AVG(isnull(C_CR,0)) AS C_CR," +
                        "AVG(isnull(C_NI,0)) AS C_NI," +
                        "AVG(isnull(C_MNO,0)) AS C_MNO" +
                        " from " +
                        "(SELECT TOP(1) * FROM M_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + "" +
                        " and C_S <=" + _list[0] + " and C_S >= " + _list[1] + " " +
                         " and C_C <=" + _list[2] + " and C_C >= " + _list[3] + "" +
                         " and C_ASH <=" + _list[4] + " and C_ASH >= " + _list[5] + "" +
                         " and C_VOLATILES <=" + _list[6] + " and C_VOLATILES >= " + _list[7] + "  " +
                        "   order by TIMESTAMP DESC) AS NET";

                    DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);

                    string sql_Count1 = "select  " +
                    "isnull(C_CAO,0) AS C_CAO," +
                    "isnull(C_SIO2,0) AS C_SIO2," +
                    "isnull(C_AL2O3,0) AS C_AL2O3 ," +
                    "isnull(C_MGO,0) AS C_MGO," +
                    "isnull(C_P,0) AS C_P," +
                    "isnull(C_K2O,0) AS C_K2O," +
                    "isnull(C_NA2O,0) AS C_NA2O    " +
                     "  from " +
                     "(SELECT TOP(1) * FROM M_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + "" +
                     " and C_CAO <=" + _list[8] + " and C_CAO >= " + _list[9] + "" +
                     " and C_SIO2 <=" + _list[10] + " and C_SIO2 >= " + _list[11] + "" +
                     " and C_MGO <=" + _list[12] + " and C_MGO >= " + _list[13] + "" +
                     " and C_AL2O3 <=" + _list[14] + " and C_AL2O3 >= " + _list[15] + "" +
                     " and C_P <=" + _list[16] + " and C_P >= " + _list[17] + "" +
                     " and C_K2O <=" + _list[18] + " and C_K2O >= " + _list[19] + "" +
                     " and C_NA2O <=" + _list[20] + " and C_NA2O >= " + _list[21] + "" +
                     "   order by TIMESTAMP DESC) AS NET";
                    DataTable dataTable_Count1 = _dBSQL.GetCommand(sql_Count1);

                    if (dataTable_Count.Rows.Count > 0 && dataTable_Count != null)
                    {
                        this.dataGridView3.AutoGenerateColumns = false;
                        this.dataGridView3.DataSource = dataTable_Count;
                        if (dataTable_Count1.Rows.Count > 0)
                        {

                            dataGridView3.Rows[0].Cells["C_CAO"].Value = dataTable_Count1.Rows[0]["C_CAO"].ToString();
                            dataGridView3.Rows[0].Cells["C_SIO2"].Value = dataTable_Count1.Rows[0]["C_SIO2"].ToString();
                            dataGridView3.Rows[0].Cells["C_AL2O3"].Value = dataTable_Count1.Rows[0]["C_AL2O3"].ToString();
                            dataGridView3.Rows[0].Cells["C_MGO"].Value = dataTable_Count1.Rows[0]["C_MGO"].ToString();
                            dataGridView3.Rows[0].Cells["C_P"].Value = dataTable_Count1.Rows[0]["C_P"].ToString();
                            dataGridView3.Rows[0].Cells["C_K2O"].Value = dataTable_Count1.Rows[0]["C_K2O"].ToString();
                            dataGridView3.Rows[0].Cells["C_NA2O"].Value = dataTable_Count1.Rows[0]["C_NA2O"].ToString();
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败（燃料特殊成分）" + sql_Count;
                            _vLog.writelog(mistake, -1);
                        }

                        //20200913添加逻辑，使用再用水分及烧损
                        string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                        DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                            dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_Count;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string text = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                        _vLog.writelog(text, -1);
                    }


                }
                //熔剂 CAO SIO2 MGO AL2O3 P S 
                else if (Flag == 4)
                {
                    List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                    string sql_Count = "select top (1) " +
                            "TIMESTAMP," +
                            "C_TFE," +
                             "C_FEO," +
                             "C_CAO," +
                             "C_SIO2," +
                             "C_AL2O3," +
                             "C_MGO," +
                             "C_S," +
                             "C_P," +
                             "C_C," +
                             "C_MN," +
                             //  "C_LOT," +
                             "C_R," +
                             //  "C_H2O," +
                             "C_ASH," +
                             "C_VOLATILES," +
                             "C_TIO2," +
                             "C_K2O," +
                             "C_NA2O," +
                             "C_PBO," +
                             "C_ZNO," +
                             "C_F," +
                             "C_AS," +
                             "C_CU," +
                             "C_PB," +
                             "C_ZN," +
                             "C_K," +
                             "C_NA," +
                             "C_CR," +
                             "C_NI," +
                             "C_MNO" +
                             "  from M_MATERIAL_ANALYSIS" +
                             "  where L2_CODE = " + WLBM + " " +
                             " and C_CAO <=" + _list[0] + " and C_CAO >= " + _list[1] + "" +
                             " and C_SIO2 <=" + _list[2] + " and C_SIO2 >= " + _list[3] + "" +
                             " and C_MGO <=" + _list[4] + " and C_MGO >= " + _list[5] + "" +
                             " and C_AL2O3 <=" + _list[6] + " and C_AL2O3 >= " + _list[7] + "" +
                             " and C_P <=" + _list[8] + " and C_P >= " + _list[9] + "" +
                             " and C_S <=" + _list[10] + " and C_S >= " + _list[11] + "" +
                             " order by TIMESTAMP desc";
                    DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);

                    if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                    {
                        this.dataGridView3.AutoGenerateColumns = false;
                        this.dataGridView3.DataSource = dataTable_Count;


                        //20200913添加逻辑，使用再用水分及烧损
                        string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                        DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                            dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                        _vLog.writelog(mistake, -1);
                    }
                }
                //除尘灰tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                else if (Flag == 5)
                {
                    List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                    string sql_Count = "select top (1) " +
                            "TIMESTAMP," +
                            "C_TFE," +
                             "C_FEO," +
                             "C_CAO," +
                             "C_SIO2," +
                             "C_AL2O3," +
                             "C_MGO," +
                             "C_S," +
                             "C_P," +
                             "C_C," +
                             "C_MN," +
                             // "C_LOT," +
                             "C_R," +
                             //"C_H2O," +
                             "C_ASH,C_VOLATILES,C_TIO2," +
                             "C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN," +
                             "C_K,C_NA,C_CR,C_NI,C_MNO  from M_MATERIAL_ANALYSIS" +
                             " where L2_CODE = " + WLBM + " " +
                             " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                             " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                             " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                             " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                             " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                             " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                             " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                             " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                             " order by TIMESTAMP desc";
                    DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                    if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                    {
                        this.dataGridView3.AutoGenerateColumns = false;
                        this.dataGridView3.DataSource = dataTable_Count;


                        //20200913添加逻辑，使用再用水分及烧损
                        string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                        DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                            dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                        _vLog.writelog(mistake, -1);
                    }
                }
                //返矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                else if (Flag == 6)
                {
                    List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                    string sql_Count = "select top (1) TIMESTAMP,C_TFE," +
                             "C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C," +
                             "C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES,C_TIO2," +
                             "C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN," +
                             "C_K,C_NA,C_CR,C_NI,C_MNO  from M_MATERIAL_ANALYSIS" +
                             " where L2_CODE = " + WLBM + " " +
                             " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                             " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                             " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                             " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                             " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                             " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                             " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                             " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                             " order by TIMESTAMP desc";
                    DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                    if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                    {
                        this.dataGridView3.AutoGenerateColumns = false;
                        this.dataGridView3.DataSource = dataTable_Count;


                        //20200913添加逻辑，使用再用水分及烧损
                        string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                        DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                        if (dataTable != null && dataTable.Rows.Count > 0)
                        {
                            dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                            dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                        _vLog.writelog(mistake, -1);
                    }
                }
                //烧结矿
                else if (Flag == 7)
                {
                    MessageBox.Show("选择更新的成分为矿粉，规则中不存在烧结矿");
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为烧结矿，规则不包含";
                    _vLog.writelog(mistake, -1);
                }
                //高炉炉渣
                else if (Flag == 8)
                {
                    MessageBox.Show("选择更新的成分为矿粉，规则中不存在高炉炉渣");
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为高炉炉渣，规则不包含";
                    _vLog.writelog(mistake, -1);
                }

                else if (Flag == 0)
                {
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类区分上下限失败";
                    _vLog.writelog(mistake, -1);
                }

            }
            catch (Exception ee)
            {
                string mistake = "第四部分,默认显示“第二部分”中最新一条记录成分失败" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }

        }
        /// <summary>
        ///  第四部分 默认显示“第二部分”中最新“加权批数”条的加权平均成分（插入时间倒叙） 原料检化验表
        /// </summary>
        public void category_4_4()
        {
            try
            {
                //获取修改后的物料成名编码
                //  int WLBM = _Dic[comboBox2.Text];
                int WLBM = _GET_L2CODE(comboBox1.Text.ToString(), comboBox2.Text.ToString());

                if (textBox1.Text == "")
                {
                    MessageBox.Show("请输入正确的加权批数");
                    return;
                }
                else
                {
                    int Flag = _L2_CODE_CALSS.L2_code_Judeg(WLBM);
                    //混匀矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                    if (Flag == 1)
                    {
                        List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                        //页面获取加权批数
                        int JQPJ = int.Parse(textBox1.Text);
                        //**计算数据  默认显示模检化验表最新加权批数条数据（插入时间倒叙）
                        string sql_Count = "select  " +
                            "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                            "AVG(isnull(C_FEO,0)) AS C_FEO," +
                            "AVG(isnull(C_CAO,0)) AS C_CAO," +
                            "AVG(isnull(C_SIO2,0)) AS C_SIO2," +
                            "AVG(isnull(C_AL2O3,0)) AS C_AL2O3 ," +
                            "AVG(isnull(C_MGO,0)) AS C_MGO," +
                            "AVG(isnull(C_S,0)) AS C_S," +
                            "AVG(isnull(C_P,0)) AS C_P," +
                            "AVG(isnull(C_C,0)) AS C_C," +
                            "AVG(isnull(C_MN,0)) AS C_MN," +
                            //  "AVG(isnull(C_LOT,0)) AS C_LOT," +
                            "AVG(isnull(C_R,0)) AS C_R ," +
                            // "AVG(isnull(C_H2O,0)) AS C_H2O," +
                            "AVG(isnull(C_ASH,0)) AS C_ASH," +
                            "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                            "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                            "AVG(isnull(C_K2O,0)) AS C_K2O," +
                            "AVG(isnull(C_NA2O,0)) AS C_NA2O," +
                            "AVG(isnull(C_PBO,0)) AS C_PBO," +
                            "AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                            "AVG(isnull(C_F,0)) AS C_F," +
                            "AVG(isnull(C_AS,0)) AS C_AS," +
                            "AVG(isnull(C_CU,0)) AS C_CU," +
                            "AVG(isnull(C_PB,0)) AS C_PB," +
                            "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                            "AVG(isnull(C_K,0)) AS C_K," +
                            "AVG(isnull(C_NA,0)) AS C_NA," +
                            "AVG(isnull(C_CR,0)) AS C_CR," +
                            "AVG(isnull(C_NI,0)) AS C_NI," +
                            "AVG(isnull(C_MNO,0)) AS C_MNO" +
                            " from " +
                            "(SELECT TOP(" + JQPJ + ") * FROM M_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + "" +
                              " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                             " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                             " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                             " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                             " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                             " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                             " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                             " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                            "   order by TIMESTAMP DESC) AS NET";
                        DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                        if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                        {
                            this.dataGridView3.AutoGenerateColumns = false;
                            this.dataGridView3.DataSource = dataTable_Count;
                            //20200913添加逻辑，使用再用水分及烧损
                            string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                            DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                            }
                            else
                            {
                                string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                _vLog.writelog(mistake, -1);
                            }
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                            _vLog.writelog(mistake, -1);
                        }


                    }
                    //矿粉
                    else if (Flag == 2)
                    {
                        MessageBox.Show("选择更新的成分为矿粉，规则中不存在矿粉判断");
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为矿粉，规则不包含";
                        _vLog.writelog(mistake, -1);
                    }
                    //燃料 特殊查询 s、C、灰分、挥发灰、为正常查询，每天可能报一条      
                    //cao、sio2、mgo、al2o3、p、k2o、nao2为特殊成分，每周报一条上下限查询条件
                    else if (Flag == 3)
                    {
                        List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                        //页面获取加权批数
                        int JQPJ = int.Parse(textBox1.Text);
                        //**计算数据  默认显示原料检化验表最新加权批数条数据（插入时间倒叙）
                        string sql_Count = "select  " +
                            "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                            "AVG(isnull(C_FEO,0)) AS C_FEO," +
                            "AVG(isnull(C_S,0)) AS C_S," +
                            "AVG(isnull(C_C,0)) AS C_C," +
                            "AVG(isnull(C_MN,0)) AS C_MN," +
                            // "AVG(isnull(C_LOT,0)) AS C_LOT," +
                            "AVG(isnull(C_R,0)) AS C_R ," +
                            // "AVG(isnull(C_H2O,0)) AS C_H2O," +
                            "AVG(isnull(C_ASH,0)) AS C_ASH," +
                            "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                            "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                            "AVG(isnull(C_PBO,0)) AS C_PBO," +
                            "AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                            "AVG(isnull(C_F,0)) AS C_F," +
                            "AVG(isnull(C_AS,0)) AS C_AS," +
                            "AVG(isnull(C_CU,0)) AS C_CU," +
                            "AVG(isnull(C_PB,0)) AS C_PB," +
                            "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                            "AVG(isnull(C_K,0)) AS C_K," +
                            "AVG(isnull(C_NA,0)) AS C_NA," +
                            "AVG(isnull(C_CR,0)) AS C_CR," +
                            "AVG(isnull(C_NI,0)) AS C_NI," +
                            "AVG(isnull(C_MNO,0)) AS C_MNO" +
                            " from " +//cao、sio2、mgo、al2o3、p、k2o、nao2
                            "(SELECT TOP(" + JQPJ + ") * FROM M_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + "" +
                            " and C_S <=" + _list[0] + " and C_S >= " + _list[1] + " " +
                             " and C_C <=" + _list[2] + " and C_C >= " + _list[3] + "" +
                             " and C_ASH <=" + _list[4] + " and C_ASH >= " + _list[5] + "" +
                             " and C_VOLATILES <=" + _list[6] + " and C_VOLATILES >= " + _list[7] + "  " +
                            "   order by TIMESTAMP DESC) AS NET";
                        DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                        string sql_Count1 = "select  " +
                          "AVG(isnull(C_CAO,0)) AS C_CAO," +
                           "AVG(isnull(C_SIO2,0)) AS C_SIO2," +
                           "AVG(isnull(C_AL2O3,0)) AS C_AL2O3 ," +
                          "AVG(isnull(C_MGO,0)) AS C_MGO," +
                           "AVG(isnull(C_P,0)) AS C_P," +
                         "AVG(isnull(C_K2O,0)) AS C_K2O," +
                          "AVG(isnull(C_NA2O,0)) AS C_NA2O    " +

                         " from " +//cao、sio2、mgo、al2o3、p、k2o、nao2
                         "(SELECT TOP(" + JQPJ + ") * FROM M_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + "" +

                         " and C_CAO <=" + _list[8] + " and C_CAO >= " + _list[9] + "" +
                         " and C_SIO2 <=" + _list[10] + " and C_SIO2 >= " + _list[11] + "" +
                         " and C_MGO <=" + _list[12] + " and C_MGO >= " + _list[13] + "" +
                         " and C_AL2O3 <=" + _list[14] + " and C_AL2O3 >= " + _list[15] + "" +
                         " and C_P <=" + _list[16] + " and C_P >= " + _list[17] + "" +
                         " and C_K2O <=" + _list[18] + " and C_K2O >= " + _list[19] + "" +
                         " and C_NA2O <=" + _list[20] + " and C_NA2O >= " + _list[21] + "" +
                         "   order by TIMESTAMP DESC) AS NET";
                        DataTable dataTable_Count1 = _dBSQL.GetCommand(sql_Count1);

                        if (dataTable_Count.Rows.Count > 0)
                        {
                            this.dataGridView3.AutoGenerateColumns = false;
                            this.dataGridView3.DataSource = dataTable_Count;
                            if (dataTable_Count1.Rows.Count > 0)
                            {

                                dataGridView3.Rows[0].Cells["C_CAO"].Value = dataTable_Count1.Rows[0]["C_CAO"].ToString();
                                dataGridView3.Rows[0].Cells["C_SIO2"].Value = dataTable_Count1.Rows[0]["C_SIO2"].ToString();
                                dataGridView3.Rows[0].Cells["C_AL2O3"].Value = dataTable_Count1.Rows[0]["C_AL2O3"].ToString();
                                dataGridView3.Rows[0].Cells["C_MGO"].Value = dataTable_Count1.Rows[0]["C_MGO"].ToString();
                                dataGridView3.Rows[0].Cells["C_P"].Value = dataTable_Count1.Rows[0]["C_P"].ToString();
                                dataGridView3.Rows[0].Cells["C_K2O"].Value = dataTable_Count1.Rows[0]["C_K2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_NA2O"].Value = dataTable_Count1.Rows[0]["C_NA2O"].ToString();

                            }
                            else
                            {
                                string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分)" + sql_Count1;
                                _vLog.writelog(mistake, -1);
                            }
                            //20200913添加逻辑，使用再用水分及烧损
                            string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                            DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                            }
                            else
                            {
                                string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                _vLog.writelog(mistake, -1);
                            }
                        }
                        else
                        {
                            string text = "预计使用成分数据加权数据查询失败";
                            _vLog.writelog(text, 0);
                        }

                    }
                    //熔剂 CAO SIO2 MGO AL2O3 P S 
                    else if (Flag == 4)
                    {
                        List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                        //页面获取加权批数
                        int JQPJ = int.Parse(textBox1.Text);
                        //**计算数据  默认显示模型结果表最新加权批数条数据（结果表时间倒叙）
                        string sql_Count = "select  " +
                            "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                            "AVG(isnull(C_FEO,0)) AS C_FEO," +
                            "AVG(isnull(C_CAO,0)) AS C_CAO," +
                            "AVG(isnull(C_SIO2,0)) AS C_SIO2," +
                            "AVG(isnull(C_AL2O3,0)) AS C_AL2O3 ," +
                            "AVG(isnull(C_MGO,0)) AS C_MGO," +
                            "AVG(isnull(C_S,0)) AS C_S," +
                            "AVG(isnull(C_P,0)) AS C_P," +
                            "AVG(isnull(C_C,0)) AS C_C," +
                            "AVG(isnull(C_MN,0)) AS C_MN," +
                            //  "AVG(isnull(C_LOT,0)) AS C_LOT," +
                            "AVG(isnull(C_R,0)) AS C_R ," +
                            //  "AVG(isnull(C_H2O,0)) AS C_H2O," +
                            "AVG(isnull(C_ASH,0)) AS C_ASH," +
                            "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                            "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                            "AVG(isnull(C_K2O,0)) AS C_K2O," +
                            "AVG(isnull(C_NA2O,0)) AS C_NA2O," +
                            "AVG(isnull(C_PBO,0)) AS C_PBO," +
                            "AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                            "AVG(isnull(C_F,0)) AS C_F," +
                            "AVG(isnull(C_AS,0)) AS C_AS," +
                            "AVG(isnull(C_CU,0)) AS C_CU," +
                            "AVG(isnull(C_PB,0)) AS C_PB," +
                            "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                            "AVG(isnull(C_K,0)) AS C_K," +
                            "AVG(isnull(C_NA,0)) AS C_NA," +
                            "AVG(isnull(C_CR,0)) AS C_CR," +
                            "AVG(isnull(C_NI,0)) AS C_NI," +
                            "AVG(isnull(C_MNO,0)) AS C_MNO" +
                            " from " +
                            "(SELECT TOP(" + JQPJ + ") * FROM M_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + "" +
                             " and C_CAO <=" + _list[0] + " and C_CAO >= " + _list[1] + "" +
                             " and C_SIO2 <=" + _list[2] + " and C_SIO2 >= " + _list[3] + "" +
                             " and C_MGO <=" + _list[4] + " and C_MGO >= " + _list[5] + "" +
                             " and C_AL2O3 <=" + _list[6] + " and C_AL2O3 >= " + _list[7] + "" +
                             " and C_P <=" + _list[8] + " and C_P >= " + _list[9] + "" +
                             " and C_S <=" + _list[10] + " and C_S >= " + _list[11] + "" +
                            "   order by TIMESTAMP DESC) AS NET";
                        DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                        if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                        {
                            this.dataGridView3.AutoGenerateColumns = false;
                            this.dataGridView3.DataSource = dataTable_Count;
                            //20200913添加逻辑，使用再用水分及烧损
                            string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                            DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                            }
                            else
                            {
                                string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                _vLog.writelog(mistake, -1);
                            }
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                            _vLog.writelog(mistake, -1);
                        }

                    }
                    //除尘灰tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                    else if (Flag == 5)
                    {
                        List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                        //页面获取加权批数
                        int JQPJ = int.Parse(textBox1.Text);
                        //**计算数据  默认显示模型结果表最新加权批数条数据（结果表时间倒叙）
                        string sql_Count = "select  " +
                            "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                            "AVG(isnull(C_FEO,0)) AS C_FEO," +
                            "AVG(isnull(C_CAO,0)) AS C_CAO," +
                            "AVG(isnull(C_SIO2,0)) AS C_SIO2," +
                            "AVG(isnull(C_AL2O3,0)) AS C_AL2O3 ," +
                            "AVG(isnull(C_MGO,0)) AS C_MGO," +
                            "AVG(isnull(C_S,0)) AS C_S," +
                            "AVG(isnull(C_P,0)) AS C_P," +
                            "AVG(isnull(C_C,0)) AS C_C," +
                            "AVG(isnull(C_MN,0)) AS C_MN," +
                            //  "AVG(isnull(C_LOT,0)) AS C_LOT," +
                            "AVG(isnull(C_R,0)) AS C_R ," +
                            //  "AVG(isnull(C_H2O,0)) AS C_H2O," +
                            "AVG(isnull(C_ASH,0)) AS C_ASH," +
                            "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                            "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                            "AVG(isnull(C_K2O,0)) AS C_K2O," +
                            "AVG(isnull(C_NA2O,0)) AS C_NA2O," +
                            "AVG(isnull(C_PBO,0)) AS C_PBO," +
                            "AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                            "AVG(isnull(C_F,0)) AS C_F," +
                            "AVG(isnull(C_AS,0)) AS C_AS," +
                            "AVG(isnull(C_CU,0)) AS C_CU," +
                            "AVG(isnull(C_PB,0)) AS C_PB," +
                            "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                            "AVG(isnull(C_K,0)) AS C_K," +
                            "AVG(isnull(C_NA,0)) AS C_NA," +
                            "AVG(isnull(C_CR,0)) AS C_CR," +
                            "AVG(isnull(C_NI,0)) AS C_NI," +
                            "AVG(isnull(C_MNO,0)) AS C_MNO" +
                            " from " +
                            "(SELECT TOP(" + JQPJ + ") * FROM M_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + "" +
                              " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                             " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                             " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                             " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                             " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                             " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                             " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                             " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                            "   order by TIMESTAMP DESC) AS NET";
                        DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                        if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                        {
                            this.dataGridView3.AutoGenerateColumns = false;
                            this.dataGridView3.DataSource = dataTable_Count;
                            //20200913添加逻辑，使用再用水分及烧损
                            string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                            DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                            }
                            else
                            {
                                string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                _vLog.writelog(mistake, -1);
                            }
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                            _vLog.writelog(mistake, -1);
                        }
                    }
                    //返矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                    else if (Flag == 6)
                    {
                        List<double> _list = _L2_CODE_CALSS._GetList(Flag, WLBM);
                        //页面获取加权批数
                        int JQPJ = int.Parse(textBox1.Text);
                        //**计算数据  默认显示模型结果表最新加权批数条数据（结果表时间倒叙）
                        string sql_Count = "select  " +
                            "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                            "AVG(isnull(C_FEO,0)) AS C_FEO," +
                            "AVG(isnull(C_CAO,0)) AS C_CAO," +
                            "AVG(isnull(C_SIO2,0)) AS C_SIO2," +
                            "AVG(isnull(C_AL2O3,0)) AS C_AL2O3 ," +
                            "AVG(isnull(C_MGO,0)) AS C_MGO," +
                            "AVG(isnull(C_S,0)) AS C_S," +
                            "AVG(isnull(C_P,0)) AS C_P," +
                            "AVG(isnull(C_C,0)) AS C_C," +
                            "AVG(isnull(C_MN,0)) AS C_MN," +
                            //  "AVG(isnull(C_LOT,0)) AS C_LOT," +
                            "AVG(isnull(C_R,0)) AS C_R ," +
                            //  "AVG(isnull(C_H2O,0)) AS C_H2O," +
                            "AVG(isnull(C_ASH,0)) AS C_ASH," +
                            "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                            "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                            "AVG(isnull(C_K2O,0)) AS C_K2O," +
                            "AVG(isnull(C_NA2O,0)) AS C_NA2O," +
                            "AVG(isnull(C_PBO,0)) AS C_PBO," +
                            "AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                            "AVG(isnull(C_F,0)) AS C_F," +
                            "AVG(isnull(C_AS,0)) AS C_AS," +
                            "AVG(isnull(C_CU,0)) AS C_CU," +
                            "AVG(isnull(C_PB,0)) AS C_PB," +
                            "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                            "AVG(isnull(C_K,0)) AS C_K," +
                            "AVG(isnull(C_NA,0)) AS C_NA," +
                            "AVG(isnull(C_CR,0)) AS C_CR," +
                            "AVG(isnull(C_NI,0)) AS C_NI," +
                            "AVG(isnull(C_MNO,0)) AS C_MNO" +
                            " from " +
                            "(SELECT TOP(" + JQPJ + ") * FROM M_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + "" +
                              " and C_TFE <=" + _list[0] + " and C_TFE >= " + _list[1] + " " +
                             " and C_CAO <=" + _list[2] + " and C_CAO >= " + _list[3] + "" +
                             " and C_SIO2 <=" + _list[4] + " and C_SIO2 >= " + _list[5] + "" +
                             " and C_MGO <=" + _list[6] + " and C_MGO >= " + _list[7] + "" +
                             " and C_AL2O3 <=" + _list[8] + " and C_AL2O3 >= " + _list[9] + "" +
                             " and C_P <=" + _list[10] + " and C_P >= " + _list[11] + "" +
                             " and C_S <=" + _list[12] + " and C_S >= " + _list[13] + "" +
                             " and C_MNO <=" + _list[14] + " and C_MNO >= " + _list[15] + "" +
                            "   order by TIMESTAMP DESC) AS NET";
                        DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                        if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                        {
                            this.dataGridView3.AutoGenerateColumns = false;
                            this.dataGridView3.DataSource = dataTable_Count;
                            //20200913添加逻辑，使用再用水分及烧损
                            string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                            DataTable dataTable = _dBSQL.GetCommand(sql_bins);
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                            }
                            else
                            {
                                string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                _vLog.writelog(mistake, -1);
                            }
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                            _vLog.writelog(mistake, -1);
                        }

                    }
                    //烧结矿
                    else if (Flag == 7)
                    {
                        MessageBox.Show("选择更新的成分为矿粉，规则中不存在烧结矿");
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为烧结矿，规则不包含";
                        _vLog.writelog(mistake, -1);
                    }
                    //高炉炉渣
                    else if (Flag == 8)
                    {
                        MessageBox.Show("选择更新的成分为矿粉，规则中不存在高炉炉渣");
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为高炉炉渣，规则不包含";
                        _vLog.writelog(mistake, -1);
                    }
                    else if (Flag == 0)
                    {
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类区分上下限失败";
                        _vLog.writelog(mistake, -1);
                    }
                }
            }
            catch (Exception ee)
            {
                string mistake = " 第四部分，默认显示“第二部分”中最新“加权批数”条的加权平均成分错误" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }

        }
        /// <summary>
        /// 第四部分  默认显示在用成分
        /// </summary>
        public void category_4_5()
        {
            try
            {
                string sql_Count = " SELECT" +
                                      " a.BIN_NUM_SHOW,b.MAT_DESC,a.L2_CODE," +
                                      "a.STATE,a.P_T_FLAG,a.NUMBER_FLAG,a.C_TFE,a.C_FEO,a.C_CAO," +
                                      "a.C_SIO2,a.C_AL2O3,a.C_MGO,a.C_S,a.C_P,a.C_C,a.C_MN,a.C_LOT," +
                                      "a.C_R,a.C_H2O,a.C_ASH,a.C_VOLATILES,a.C_TIO2,a.C_K2O,a.C_NA2O," +
                                      "a.C_PBO,a.C_ZNO,a.C_F,a.C_AS,a.C_CU,a.C_PB,a.C_ZN,a.C_K,a.C_NA," +
                                      "a.C_CR," +
                                      "a.C_NI,a.C_MNO " +
                                      "FROM M_MATERIAL_BINS a,M_MATERIAL_COOD b " +
                                      "where a.L2_CODE = b.L2_CODE and BIN_NUM_SHOW = " + CH + "";
                DataTable dataTable_Count = _dBSQL.GetCommand(sql_Count);
                this.dataGridView3.AutoGenerateColumns = false;
                this.dataGridView3.DataSource = dataTable_Count;
            }
            catch (Exception ee)
            {
                string mistake = "第四部分,默认显示在用成分失败" + ee.ToString();
                _vLog.writelog(mistake, -1);

            }
        }
        /// <summary>
        /// 确认按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            try
            {
                #region  
                //  int WLBM = _Dic[comboBox2.Text];
                int WLBM = _GET_L2CODE(comboBox1.Text.ToString(), comboBox2.Text.ToString());

                ////收集d3显示的预计使用的成分数据
                List<float> list = new List<float>();
                for (int count = 0; count < dataGridView3.ColumnCount; count++)
                {
                    string element_name = dataGridView3.Columns[count].Name.ToString();
                    list.Add(float.Parse(dataGridView3.Rows[0].Cells[count].Value.ToString()));

                }
                #endregion
                #region 数据准备

                string WHZT = comboBox3.Text;
                int WHZT_SIGNAL = 0;
                if (WHZT == "手动维护")
                {
                    WHZT_SIGNAL = 0;
                }
                else
                if (WHZT == "自动维护")
                {
                    WHZT_SIGNAL = 1;
                }
                else
                if (WHZT == "自动维护2")
                {
                    WHZT_SIGNAL = 2;
                }
                else
                {
                    MessageBox.Show("维护状态有误");
                    string mistake = "预计使用成分失败：维护状态有误";
                    _vLog.writelog(mistake, -1);
                    return;
                }
                //原料追踪状态
                string YLZZ = comboBox4.Text;
                int YLZZ_SIGNAL = 0;
                if (YLZZ == "启用")
                {
                    YLZZ_SIGNAL = 1;
                }
                else
                if (YLZZ == "禁用")
                {
                    YLZZ_SIGNAL = 0;
                }
                else
                {
                    MessageBox.Show("原料追踪状态有误");
                    string mistake = "预计使用成分失败：原料追踪状态有误";
                    _vLog.writelog(mistake, -1);
                    return;
                }
                //加权平均数
                int JQPJ = int.Parse(textBox1.Text);
                #endregion
                //时间
                //  string time = DateTime.Now.ToString();
                //更新
                //20200903水分和烧损为自动更新
                string sql = "update M_MATERIAL_BINS set " +
                    "TIMESTAMP = getdate()" +
                    " ,L2_CODE = '" + WLBM + "'," +
                    " STATE = '" + WHZT_SIGNAL + "' " +
                    ",P_T_FLAG = '" + YLZZ_SIGNAL + "'," +
                    "C_TFE = '" + list[0] + "' " +
                    ",C_FEO = '" + list[1] + "' ," +
                    "C_CAO = '" + list[2] + "'," +
                    "C_SIO2 = '" + list[3] + "'," +
                    "C_AL2O3 = '" + list[4] + "'," +
                    "C_MGO = '" + list[5] + "'," +
                    "C_S = '" + list[6] + "'," +
                    "C_P = '" + list[7] + "'," +
                    "C_C = '" + list[8] + "'," +
                    "C_MN = '" + list[9] + "'," +
                    //    "C_LOT = '" + list[10] + "'," +
                    "C_R = '" + list[11] + "'," +
                    //  "C_H2O = '" + list[12] + "'," +
                    "C_ASH = '" + list[13] + "'," +
                    "C_VOLATILES = '" + list[14] + "'," +
                    "C_TIO2 = '" + list[15] + "'," +
                    "C_K2O = '" + list[16] + "'," +
                    "C_NA2O = '" + list[17] + "'," +
                    "C_PBO = '" + list[18] + "'," +
                    "C_ZNO = '" + list[19] + "'," +
                    "C_F = '" + list[20] + "'," +
                    "C_AS = '" + list[21] + "'," +
                    "C_CU = '" + list[22] + "'," +
                    "C_PB = '" + list[23] + "'," +
                    "C_ZN = '" + list[24] + "'," +
                    "C_K = '" + list[25] + "'," +
                    "C_NA = '" + list[26] + "'," +
                    "C_CR = '" + list[27] + "'," +
                    "C_NI = '" + list[28] + "'," +
                    "C_MNO = '" + list[29] + "'," +
                    "BATCH_NUM = '" + PH + "', " +
                    "NUMBER_FLAG = '" + JQPJ + "' , " +
                    " SINTER_METHOD = '" + SINTER_METHOD + "'" +
                    "where BIN_NUM_SHOW = '" + CH + "' ";
                int flag = _dBSQL.CommandExecuteNonQuery(sql);
                if (flag > 0)
                {
                    MessageBox.Show("预计使用成分操作成功");
                    _TransfDelegate_YLWH();
                    var text = "点击计算按钮，" + CH + "仓成分更新成功，sql：" + sql;
                    _vLog.writelog(text, 0);
                    this.Dispose();
                }
                else
                {
                    string mistake = "预计使用成分操作失败，数据库连接失败";
                    _vLog.writelog(mistake, -1);
                }
            }
            catch (Exception ee)
            {
                string mistake = "预计使用成分错误" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 其他仓使用同样的成分,宁刚专用规则
        /// </summary>
        public void combox_look()
        {
            //if (CH == 1)
            //{
            //    DataTable data = new DataTable();
            //    data.Columns.Add("Name");
            //    data.Columns.Add("Value");
            //    DataRow row_1 = data.NewRow();
            //    row_1["Name"] = "系统计算成分";
            //    row_1["Value"] = 1;
            //    data.Rows.Add(row_1);
            //    this.comboBox5.DataSource = data;
            //    this.comboBox5.DisplayMember = "Name";
            //    this.comboBox5.ValueMember = "Value";
            //}
            //else if (CH >= 2 && CH <= 7)
            //{
            //    DataTable data = new DataTable();
            //    data.Columns.Add("Name");
            //    data.Columns.Add("Value");
            //    DataRow row_1 = data.NewRow();
            //    row_1["Name"] = "系统计算成分";
            //    row_1["Value"] = 1;
            //    data.Rows.Add(row_1);
            //    DataRow row_2 = data.NewRow();
            //    row_2["Name"] = "使用2号仓成分";
            //    row_2["Value"] = 2;
            //    data.Rows.Add(row_2);
            //    DataRow row_3 = data.NewRow();
            //    row_3["Name"] = "使用3号仓成分";
            //    row_3["Value"] = 3;
            //    data.Rows.Add(row_3);
            //    DataRow row_4 = data.NewRow();
            //    row_4["Name"] = "使用4号仓成分";
            //    row_4["Value"] = 4;
            //    data.Rows.Add(row_4);
            //    DataRow row_5 = data.NewRow();
            //    row_5["Name"] = "使用5号仓成分";
            //    row_5["Value"] = 5;
            //    data.Rows.Add(row_5);
            //    DataRow row_6 = data.NewRow();
            //    row_6["Name"] = "使用6号仓成分";
            //    row_6["Value"] = 6;
            //    data.Rows.Add(row_6);
            //    DataRow row_7 = data.NewRow();
            //    row_7["Name"] = "使用7号仓成分";
            //    row_7["Value"] = 7;
            //    data.Rows.Add(row_7);
            //    this.comboBox5.DataSource = data;
            //    this.comboBox5.DisplayMember = "Name";
            //    this.comboBox5.ValueMember = "Value";
            //    //物料下拉框默认显示



            //}
            //else if (CH >= 8 && CH <= 11)
            //{
            //    DataTable data = new DataTable();
            //    data.Columns.Add("Name");
            //    data.Columns.Add("Value");
            //    DataRow row_1 = data.NewRow();
            //    row_1["Name"] = "系统计算成分";
            //    row_1["Value"] = 1;
            //    data.Rows.Add(row_1);
            //    DataRow row_2 = data.NewRow();
            //    row_2["Name"] = "使用8号仓成分";
            //    row_2["Value"] = 8;
            //    data.Rows.Add(row_2);
            //    DataRow row_3 = data.NewRow();
            //    row_3["Name"] = "使用9号仓成分";
            //    row_3["Value"] = 9;
            //    data.Rows.Add(row_3);
            //    DataRow row_4 = data.NewRow();
            //    row_4["Name"] = "使用10号仓成分";
            //    row_4["Value"] = 10;
            //    data.Rows.Add(row_4);
            //    DataRow row_5 = data.NewRow();
            //    row_5["Name"] = "使用11号仓成分";
            //    row_5["Value"] = 11;
            //    data.Rows.Add(row_5);

            //    this.comboBox5.DataSource = data;
            //    this.comboBox5.DisplayMember = "Name";
            //    this.comboBox5.ValueMember = "Value";
            //}
            //else if (CH == 12)
            //{
            //    DataTable data = new DataTable();
            //    data.Columns.Add("Name");
            //    data.Columns.Add("Value");
            //    DataRow row_1 = data.NewRow();
            //    row_1["Name"] = "系统计算成分";
            //    row_1["Value"] = 1;
            //    data.Rows.Add(row_1);


            //    this.comboBox5.DataSource = data;
            //    this.comboBox5.DisplayMember = "Name";
            //    this.comboBox5.ValueMember = "Value";
            //}
            //else if (CH >= 13 && CH <= 14)
            //{
            //    DataTable data = new DataTable();
            //    data.Columns.Add("Name");
            //    data.Columns.Add("Value");
            //    DataRow row_1 = data.NewRow();
            //    row_1["Name"] = "系统计算成分";
            //    row_1["Value"] = 1;
            //    data.Rows.Add(row_1);
            //    DataRow row_2 = data.NewRow();
            //    row_2["Name"] = "使用13号仓成分";
            //    row_2["Value"] = 13;
            //    data.Rows.Add(row_2);
            //    DataRow row_3 = data.NewRow();
            //    row_3["Name"] = "使用14号仓成分";
            //    row_3["Value"] = 14;
            //    data.Rows.Add(row_3);


            //    this.comboBox5.DataSource = data;
            //    this.comboBox5.DisplayMember = "Name";
            //    this.comboBox5.ValueMember = "Value";
            //}
            //else if (CH >= 15 && CH <= 16)
            //{
            //    DataTable data = new DataTable();
            //    data.Columns.Add("Name");
            //    data.Columns.Add("Value");
            //    DataRow row_1 = data.NewRow();
            //    row_1["Name"] = "系统计算成分";
            //    row_1["Value"] = 1;
            //    data.Rows.Add(row_1);
            //    DataRow row_2 = data.NewRow();
            //    row_2["Name"] = "使用15号仓成分";
            //    row_2["Value"] = 15;
            //    data.Rows.Add(row_2);
            //    DataRow row_3 = data.NewRow();
            //    row_3["Name"] = "使用16号仓成分";
            //    row_3["Value"] = 16;
            //    data.Rows.Add(row_3);


            //    this.comboBox5.DataSource = data;
            //    this.comboBox5.DisplayMember = "Name";
            //    this.comboBox5.ValueMember = "Value";
            //}
            //else if (CH == 17)
            //{
            //    DataTable data = new DataTable();
            //    data.Columns.Add("Name");
            //    data.Columns.Add("Value");
            //    DataRow row_1 = data.NewRow();
            //    row_1["Name"] = "系统计算成分";
            //    row_1["Value"] = 1;
            //    data.Rows.Add(row_1);
            //    this.comboBox5.DataSource = data;
            //    this.comboBox5.DisplayMember = "Name";
            //    this.comboBox5.ValueMember = "Value";
            //}

            DataTable data = new DataTable();
            data.Columns.Add("Name");
            data.Columns.Add("Value");
            DataRow row_1 = data.NewRow();
            row_1["Name"] = "系统计算成分";
            row_1["Value"] = 1;
            data.Rows.Add(row_1);
            for (int x = 1; x <= 20; x++)
            {
                DataRow row_2 = data.NewRow();
                row_2["Name"] = "使用" + x + "#仓成分";
                row_2["Value"] = x;
                data.Rows.Add(row_2);
            }
            this.comboBox5.DataSource = data;
            this.comboBox5.DisplayMember = "Name";
            this.comboBox5.ValueMember = "Value";
        }

        /// <summary>
        /// 收集L2、物料名称规则
        /// </summary>
        public void L2_NAME_RULE()
        {
            try
            {
                var SQL_1 = "select L2_CODE,MAT_DESC from M_MATERIAL_COOD";
                DataTable _data = _dBSQL.GetCommand(SQL_1);
                if (_data != null && _data.Rows.Count > 0)
                {
                    for (int x = 0; x < _data.Rows.Count; x++)
                    {
                        string _NAME = _data.Rows[x]["MAT_DESC"].ToString();//物料名称
                        int L2_CODE = int.Parse(_data.Rows[x]["L2_CODE"].ToString());//二级编码
                        if (_Dic.ContainsKey(_NAME))
                        {
                            var mistake = "L2_NAME_RULE方法报错，M_MATERIAL_COOD表物料名称重复";
                            _vLog.writelog(mistake, -1);
                            _Dic.Remove(_NAME);
                            _Dic.Add(_NAME, L2_CODE);
                        }
                        else
                        {
                            _Dic.Add(_NAME, L2_CODE);
                        }

                    }
                }

                var sql_2 = "select * from M_MATERIAL_COOD_CONFIG";
                DataTable table = _dBSQL.GetCommand(sql_2);
                if (table != null && table.Rows.Count > 0)
                {
                    for (int x = 0; x < table.Rows.Count; x++)
                    {
                        string _NAME = table.Rows[x]["M_DESC"].ToString();//物料名称
                        int L2_CODE_MIN = int.Parse(table.Rows[x]["CODE_MIN"].ToString());//二级编码最小值
                        int L2_CODE_MAX = int.Parse(table.Rows[x]["CODE_MAX"].ToString());//二级编码最大值
                        if (_Dic.ContainsKey(_NAME))
                        {
                            var mistake = "L2_NAME_RULE方法报错，M_MATERIAL_COOD表物料名称重复";
                            _vLog.writelog(mistake, -1);
                            _keys.Remove(_NAME);
                            _keys.Add(_NAME, new Tuple<int, int>(L2_CODE_MIN, L2_CODE_MAX));
                        }
                        else
                        {
                            _keys.Add(_NAME, new Tuple<int, int>(L2_CODE_MIN, L2_CODE_MAX));
                        }

                    }
                }
               

            }
            catch (Exception ee)
            {
                var mistake = "L2_NAME_RULE方法报错" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 返回二级编码
        /// _A:物料归属
        /// _b:物料描述
        /// </summary>
        /// <param name="_A"></param>
        /// <param name="_B"></param>
        /// <returns></returns>
        public int _GET_L2CODE(string _A,string _B)
        {
            try
            {
                if (_keys.ContainsKey(_A))
                {
                    var sql = "select L2_CODE from  M_MATERIAL_COOD WHERE L2_CODE >= "+ _keys[_A].Item1 + " and L2_CODE <= " + _keys[_A].Item2 + " and MAT_DESC = '"+ _B+"'";
                    DataTable data = _dBSQL.GetCommand(sql);
                    if (data != null && data.Rows.Count == 1)
                    {
                        return int.Parse(data.Rows[0][0].ToString());
                    }
                    else
                    {
                        _vLog.writelog("_GET_L2CODE方法错误,sql:" + sql, -1);
                        return 0;
                    }
                }
                else
                {
                    _vLog.writelog("_GET_L2CODE方法错误,物料规则描述中不包含" + _A, -1);
                    return 0;
                }
            }
            catch (Exception EE)
            {
                _vLog.writelog("_GET_L2CODE方法错误" + EE.ToString(), -1);
                return 0;
                
            }

        }
    }
}
