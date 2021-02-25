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
using LY_SINTER.Model;

namespace LY_SINTER.Popover.Analysis
{
    public partial class Frm_JHPK_update : Form
    {
        //声明委托和事件
        public delegate void TransfDelegate_YHPK(DataTable dataTable);
        //声明委托和事件
        public event TransfDelegate_YHPK _TransfDelegate_YHPK;
        public vLog vLog { get; set; }
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public static bool isopen = false;
        /// <summary>
        /// 仓号
        /// </summary>
        public int CH = Ingredient_Model.Data;
        public Frm_JHPK_update(string MAT_NAME)
        {
            string num = MAT_NAME;
            InitializeComponent();
            combox();
            getData();
            category_2_2(num);
        }
        public void getData()
        {
            try
            {
                int WLBM = 0;
                string name = comboBox2.SelectedValue.ToString();

                string sql_name = "select L2_CODE,MAT_DESC from M_MATERIAL_COOD where MAT_DESC = '" + name + "'";

                DataTable dataTable_name = dBSQL.GetCommand(sql_name);
                if (dataTable_name.Rows.Count > 0)
                {
                    WLBM = int.Parse(dataTable_name.Rows[0]["L2_CODE"].ToString());
                }

                string time1 = dateTimePicker1.Value.ToString();
                string time2 = dateTimePicker2.Value.ToString();
                string sql = "select top 10 TIMESTAMP,BATCH_NUM,SAMPLETIME,REOPTTIME,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C,C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES,C_TIO2,C_K2O,C_NA2O," +
                    "C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN,C_K,C_NA,C_CR,C_NI,C_MNO  from M_ORE_MATERIAL_ANALYSIS " +
                    "where L2_CODE = " + WLBM + " and TIMESTAMP <= '" + time2 + "' and TIMESTAMP >='" + time1 + "' order by TIMESTAMP desc";
                DataTable dataTable = dBSQL.GetCommand(sql);
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ee)
            {
                string mistake = "第一部分，显示原料表最新10条记录失败" + ee.ToString();
                vLog.writelog(mistake, -1);
            }
            
        }
        public void category_2_2(string name)
        {
            //string name = comboBox2.SelectedValue.ToString();
            
            string sql2 = "select  MAT_NAME,MAT_CLASS,UNIT_PRICE,BILL_UPPER,BILL_LOWER,C_TFE,C_FEO,C_SIO2,C_CAO,C_MGO,C_AL2O3," +
                "C_S,C_P,C_LOT,C_H2O,C_AS,C_PB,C_ZN,C_CU,C_K2O,C_NA2O from MC_ORECAL_MAT_ANA_RECORD where MAT_NAME = '" + name + "'";
            DataTable table = dBSQL.GetCommand(sql2);
            string WLMC = table.Rows[0]["MAT_NAME"].ToString();
            string sql = "select L2_CODE from M_MATERIAL_COOD where MAT_DESC = '" + WLMC + "'";
            DataTable table1 = dBSQL.GetCommand(sql);
            string code = table1.Rows[0]["L2_CODE"].ToString();
            this.comboBox2.SelectedValue = code;
            //comboBox2.Text = table.Rows[0]["MAT_NAME"].ToString();
            //单价
            textBox2.Text = table.Rows[0]["UNIT_PRICE"].ToString();
            //配比上限
            textBox3.Text = table.Rows[0]["BILL_UPPER"].ToString();
            //配比下限
            textBox1.Text = table.Rows[0]["BILL_LOWER"].ToString();
            dataGridView2.DataSource = table;
        }
        public class Info { 
            public int value { get; set; }
            public string name { get; set; }
        }
        public void combox()
        {
            //***仓号、物料名称、二级编码、成分维护状态、原料追踪状态、加权平均批数
            string sql_1 = " SELECT b.MAT_DESC,a.L2_CODE FROM M_MATERIAL_BINS a,M_MATERIAL_COOD b where a.L2_CODE = b.L2_CODE ";
            DataTable dataTable_1 = dBSQL.GetCommand(sql_1);
            if (dataTable_1.Rows.Count > 0)
            {

            }
            //查询修改仓号对应的物料编码
            int WLBM = int.Parse(dataTable_1.Rows[0]["L2_CODE"].ToString());
            //查询修改仓号对应的物料名称
            string WLMC = dataTable_1.Rows[0]["MAT_DESC"].ToString();


            //查询成分归属类下拉框选项
            string sql = " select M_DESC as name,M_TYPE as value from M_MATERIAL_COOD_CONFIG where M_TYPE between 2 and 6";
            DataTable dataTable = dBSQL.GetCommand(sql);
            string sql_2 = "select M_TYPE,M_DESC,CODE_MIN,CODE_MAX from M_MATERIAL_COOD_CONFIG order by M_TYPE asc";
            DataTable dataTable_2 = dBSQL.GetCommand(sql_2);
            int max = 0;
            int min = 0;
            //*****物料归属下拉框赋值
            this.comboBox1.DataSource = dataTable;
            this.comboBox1.DisplayMember = "name";
            this.comboBox1.ValueMember = "value";
            //原料归属下拉框默认显示
            //this.comboBox1.Text = WLGSL_NAME;

            string name = comboBox1.SelectedValue.ToString();
            string sql2 = "select M_TYPE,M_DESC,CODE_MIN,CODE_MAX from M_MATERIAL_COOD_CONFIG where M_TYPE='" + name + "'";
            DataTable table2 = dBSQL.GetCommand(sql2);
            max = int.Parse(table2.Rows[0]["CODE_MAX"].ToString());
            min = int.Parse(table2.Rows[0]["CODE_MIN"].ToString());
            //查询所有物料名称和二级编码
            string sql_3 = "select L2_CODE,MAT_DESC from M_MATERIAL_COOD where L2_CODE between " + min + " and " + max + "";
            DataTable dataTable_3 = dBSQL.GetCommand(sql_3);
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




            /*//*****原料下拉框选项赋值
            List<Info> list = new List<Info>();
            for (int x = 0; x < dataTable_3.Rows.Count; x++)
            {
                int Code = int.Parse(dataTable_3.Rows[x]["L2_CODE"].ToString());
                if (Code >= min && Code <= max)
                {
                    Info info = new Info() { name = dataTable_3.Rows[x]["MAT_DESC"].ToString(), value = int.Parse(dataTable_3.Rows[x]["L2_CODE"].ToString()) };
                    list.Add(info);
                }
            }
            this.comboBox2.DataSource = list;
            this.comboBox2.DisplayMember = "name";
            this.comboBox2.ValueMember = "name";*/


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string name = comboBox1.SelectedValue.ToString();
                //索引号从0开始，+1转换为类别号
                string sql = "  select M_TYPE,M_DESC,CODE_MIN,CODE_MAX from M_MATERIAL_COOD_CONFIG where M_TYPE = " + name + "";
                DataTable dataTable = dBSQL.GetCommand(sql);
                int min = int.Parse(dataTable.Rows[0]["CODE_MIN"].ToString());
                int max = int.Parse(dataTable.Rows[0]["CODE_MAX"].ToString());
                string sql_1 = " select L2_CODE, MAT_DESC from M_MATERIAL_COOD order by L2_CODE asc";
                DataTable dataTable1 = dBSQL.GetCommand(sql_1);
                List<Info> list = new List<Info>();
                for (int x = 0; x < dataTable1.Rows.Count; x++)
                {
                    int Code = int.Parse(dataTable1.Rows[x]["L2_CODE"].ToString());
                    if (Code >= min && Code <= max)
                    {
                        Info info = new Info() { name = dataTable1.Rows[x]["MAT_DESC"].ToString(), value = int.Parse(dataTable1.Rows[x]["L2_CODE"].ToString()) };
                        list.Add(info);
                    }
                }
                this.comboBox2.DataSource = null;
                this.comboBox2.DataSource = list;
                this.comboBox2.DisplayMember = "name";
                this.comboBox2.ValueMember = "value";
            }
            catch (Exception ee)
            {

            }
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                string name = comboBox2.Text;

                string sql = "select UNIT_PRICE from M_ORE_MATERIAL_ANALYSIS where L2_CODE in (select L2_CODE from M_MATERIAL_COOD where MAT_DESC='" + comboBox2.Text + "')";
                DataTable table = dBSQL.GetCommand(sql);
                if (table.Rows.Count > 0)
                {
                    textBox2.Text = table.Rows[0][0].ToString();
                }
            }
            catch (Exception ee)
            {

            }
        }
        //查询按钮
        private void simpleButton5_Click_1(object sender, EventArgs e)
        {
            try
            {
                string name = comboBox2.Text;

                string sql_name = "select L2_CODE,MAT_DESC from M_MATERIAL_COOD where MAT_DESC = '" + name + "'";

                DataTable dataTable_name = dBSQL.GetCommand(sql_name);
                int WLBM = int.Parse(dataTable_name.Rows[0]["L2_CODE"].ToString());
                string time1 = dateTimePicker1.Value.ToString();
                string time2 = dateTimePicker2.Value.ToString();
                string sql = "select TIMESTAMP,BATCH_NUM,SAMPLETIME,REOPTTIME,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C,C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES,C_TIO2,C_K2O,C_NA2O," +
                    "C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN,C_K,C_NA,C_CR,C_NI,C_MNO  from M_ORE_MATERIAL_ANALYSIS " +
                    "where L2_CODE = " + WLBM + " and TIMESTAMP <= '" + time2 + "' and TIMESTAMP >='" + time1 + "' order by TIMESTAMP desc";
                DataTable dataTable = dBSQL.GetCommand(sql);
                dataGridView1.DataSource = dataTable;
                string sql1 = "select  MAT_NAME,MAT_CLASS,UNIT_PRICE,BILL_UPPER,BILL_LOWER,C_TFE,C_FEO,C_SIO2,C_CAO,C_MGO,C_AL2O3,C_S,C_P,C_LOT,C_H2O,C_AS,C_PB,C_ZN,C_CU,C_K2O," +
                    "C_NA2O from MC_ORECAL_MAT_ANA_RECORD where MAT_NAME = '" + name + "' and TIMESTAMP <= '" + time2 + "' and TIMESTAMP >='" + time1 + "' order by TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                dataGridView3.DataSource = dataTable1;

            }
            catch (Exception ee)
            {

            }

        }
        //确认使用按钮，更新主页面d1数据
        private void simpleButton3_Click_1(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable = GetDgvToTable(dataGridView3);
            dataTable.Columns.Add("RowNum").SetOrdinal(0);
            dataTable.Columns["BILL_UPPER"].SetOrdinal(4);
            dataTable.Columns["BILL_LOWER"].SetOrdinal(5);
            _TransfDelegate_YHPK(dataTable);
            this.Dispose();
        }
        public DataTable GetDgvToTable(DataGridView dgv)
        {
            DataTable dt = new DataTable();
            for (int count = 0; count < dgv.Columns.Count; count++)
            {
                DataColumn dc = new DataColumn(dgv.Columns[count].Name.ToString());
                dt.Columns.Add(dc);
            }
            for (int count = 0; count < dgv.Rows.Count; count++)
            {
                DataRow dr = dt.NewRow();
                for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
                {
                    dr[countsub] = Convert.ToString(dgv.Rows[count].Cells[countsub].Value);
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        /// <summary>
        ///  第四部分   默认显示“第二部分”中最新一条记录成分（插入时间倒叙） 原料检化验表
        /// </summary>
        /// <summary>
        /// 通过二级编码判断上下限判断条件
        /// retuen 0：错误 ；1：混匀矿； 2：矿粉；3：燃料；4：溶剂；5：除尘灰；6：返矿；7：烧结矿；8：高炉炉渣
        /// </summary>
        /// 
        /// <param name="_L2_CODE"></param>
        public int L2_code_Judeg(int _L2_CODE)
        {
            string sql_l2code = "select M_TYPE from M_MATERIAL_COOD_CONFIG where CODE_MIN<=" + _L2_CODE + " and CODE_MAX >= " + _L2_CODE + "";
            DataTable data = dBSQL.GetCommand(sql_l2code);
            if (data.Rows.Count > 0)
            {
                int flag = int.Parse(data.Rows[0][0].ToString());
                return flag;
            }
            else
            {
                return 0;
            }

        }
        public void category_4_3()
        {
            try
            {


                string name = comboBox2.Text;
                string sql_name = "select L2_CODE,MAT_DESC from M_MATERIAL_COOD where MAT_DESC = '" + name + "'";

                DataTable dataTable_name = dBSQL.GetCommand(sql_name);
                //获取修改后的物料成名编码
                int WLBM = int.Parse(dataTable_name.Rows[0]["L2_CODE"].ToString());
                int Flag = L2_code_Judeg(WLBM);

                //混匀矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                if (Flag == 1)
                {
                    //获取上下限
                    var sql_M_MATERIAL_COOD = "select top(1) " +
                        "C_TFE_UP,C_TFE_LOWER," +
                        "C_CAO_UP,C_CAO_LOWER," +
                        "C_SIO2_UP,C_SIO2_LOWER," +
                        "C_MGO_UP,C_MGO_LOWER," +
                        "C_AL2O3_UP,C_AL2O3_LOWER ," +
                        "C_P_UP,C_P_LOWER," +
                        "C_S_UP,C_S_LOWER," +
                        "C_MNO_UP,C_MNO_LOWER  " +
                        "from M_MATERIAL_COOD where  L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                    DataTable data = dBSQL.GetCommand(sql_M_MATERIAL_COOD);
                    if (data.Rows.Count > 0)
                    {
                        List<double> _list = new List<double>();
                        //获取指定的上下限
                        for (int x = 0; x < data.Columns.Count; x++)
                        {
                            _list.Add(double.Parse(data.Rows[0][x].ToString()));
                        }
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
                             "  from M_ORE_MATERIAL_ANALYSIS" +
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
                        DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);

                        if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                        {
                            /*this.dataGridView3.AutoGenerateColumns = false;
                            this.dataGridView3.DataSource = dataTable_Count;
                            //20200913添加逻辑，使用再用水分及烧损
                            string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                            DataTable dataTable = dBSQL.GetCommand(sql_bins);
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                            }
                            else
                            {
                                string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                                vLog.writelog(mistake, -1);
                            }*/
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败（特殊成分：再用水分烧损）" + sql_Count;
                            vLog.writelog(mistake, -1);
                        }



                    }
                    else
                    {
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,查询二级编码：" + WLBM.ToString() + "上下限制失败" + sql_M_MATERIAL_COOD;
                        vLog.writelog(mistake, -1);
                        return;
                    }


                }
                //矿粉
                else if (Flag == 2)
                {
                    MessageBox.Show("选择更新的成分为矿粉，规则中不存在矿粉判断");
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为矿粉，规则不包含";
                    vLog.writelog(mistake, -1);
                }
                //燃料 **特殊查询** s、C、灰分、挥发灰、每天可能报一条     
                //cao、sio2、mgo、al2o3、p、k2o、nao2为特殊成分，每周可能报一条
                else if (Flag == 3)
                {
                    //获取上下限
                    var sql_M_MATERIAL_COOD = "select top(1) " +
                        "C_S_UP,C_S_LOWER," +
                        "C_C_UP,C_C_LOWER," +
                        "C_ASH_UP,C_ASH_LOWER," +
                        "C_VOLATILES_UP,C_VOLATILES_LOWER," +

                        //特殊成分上下限
                        "C_CAO_UP,C_CAO_LOWER," +
                        "C_SIO2_UP,C_SIO2_LOWER," +
                        "C_MGO_UP,C_MGO_LOWER," +
                        "C_AL2O3_UP,C_AL2O3_LOWER ," +
                        "C_P_UP,C_P_LOWER," +
                        "C_K2O_UP,C_K2O_LOWER,  " +
                        "C_NA2O_UP,C_NA2O_LOWER  " +
                        "from M_MATERIAL_COOD where  L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                    DataTable data = dBSQL.GetCommand(sql_M_MATERIAL_COOD);
                    if (data.Rows.Count > 0)
                    {
                        List<double> _list = new List<double>();
                        //获取指定的上下限
                        for (int x = 0; x < data.Columns.Count; x++)
                        {
                            _list.Add(double.Parse(data.Rows[0][x].ToString()));
                        }
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

                        DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);

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
                        DataTable dataTable_Count1 = dBSQL.GetCommand(sql_Count1);

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
                                vLog.writelog(mistake, -1);
                            }

                            //20200913添加逻辑，使用再用水分及烧损
                            string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                            DataTable dataTable = dBSQL.GetCommand(sql_bins);
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                            }
                            else
                            {
                                string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_Count;
                                vLog.writelog(mistake, -1);
                            }
                        }
                        else
                        {
                            string text = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                            vLog.writelog(text, -1);
                        }

                        //

                    }
                    else
                    {
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,查询二级编码：" + WLBM.ToString() + "上下限制失败" + sql_M_MATERIAL_COOD;
                        vLog.writelog(mistake, -1);
                        return;
                    }

                }
                //熔剂 CAO SIO2 MGO AL2O3 P S 
                else if (Flag == 4)
                {
                    //获取上下限
                    var sql_M_MATERIAL_COOD = "select top(1) " +
                        "C_CAO_UP,C_CAO_LOWER," +
                        "C_SIO2_UP,C_SIO2_LOWER," +
                        "C_MGO_UP,C_MGO_LOWER," +
                        "C_AL2O3_UP,C_AL2O3_LOWER ," +
                        "C_P_UP,C_P_LOWER," +
                        "C_S_UP,C_S_LOWER  " +
                        "from M_MATERIAL_COOD where  L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                    DataTable data = dBSQL.GetCommand(sql_M_MATERIAL_COOD);
                    if (data.Rows.Count > 0)
                    {
                        List<double> _list = new List<double>();
                        //获取指定的上下限
                        for (int x = 0; x < data.Columns.Count; x++)
                        {
                            _list.Add(double.Parse(data.Rows[0][x].ToString()));
                        }

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
                             "  from M_ORE_MATERIAL_ANALYSIS" +
                             "  where L2_CODE = " + WLBM + " " +
                             " and C_CAO <=" + _list[0] + " and C_CAO >= " + _list[1] + "" +
                             " and C_SIO2 <=" + _list[2] + " and C_SIO2 >= " + _list[3] + "" +
                             " and C_MGO <=" + _list[4] + " and C_MGO >= " + _list[5] + "" +
                             " and C_AL2O3 <=" + _list[6] + " and C_AL2O3 >= " + _list[7] + "" +
                             " and C_P <=" + _list[8] + " and C_P >= " + _list[9] + "" +
                             " and C_S <=" + _list[10] + " and C_S >= " + _list[11] + "" +
                             " order by TIMESTAMP desc";
                        DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);

                        if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                        {
                            this.dataGridView3.AutoGenerateColumns = false;
                            this.dataGridView3.DataSource = dataTable_Count;


                            //20200913添加逻辑，使用再用水分及烧损
                            /*string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                            DataTable dataTable = dBSQL.GetCommand(sql_bins);
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                            }
                            else
                            {
                                string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                vLog.writelog(mistake, -1);
                            }*/
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                            vLog.writelog(mistake, -1);
                        }

                    }
                    else
                    {
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,查询二级编码：" + WLBM.ToString() + "上下限制失败" + sql_M_MATERIAL_COOD;
                        vLog.writelog(mistake, -1);
                        return;
                    }
                }
                //除尘灰tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                else if (Flag == 5)
                {
                    //获取上下限
                    var sql_M_MATERIAL_COOD = "select top(1) " +
                        "C_TFE_UP,C_TFE_LOWER," +
                        "C_CAO_UP,C_CAO_LOWER," +
                        "C_SIO2_UP,C_SIO2_LOWER," +
                        "C_MGO_UP,C_MGO_LOWER," +
                        "C_AL2O3_UP,C_AL2O3_LOWER ," +
                        "C_P_UP,C_P_LOWER," +
                        "C_S_UP,C_S_LOWER," +
                        "C_MNO_UP,C_MNO_LOWER  " +
                        "from M_MATERIAL_COOD where  L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                    DataTable data = dBSQL.GetCommand(sql_M_MATERIAL_COOD);
                    if (data.Rows.Count > 0)
                    {
                        List<double> _list = new List<double>();
                        //获取指定的上下限
                        for (int x = 0; x < data.Columns.Count; x++)
                        {
                            _list.Add(double.Parse(data.Rows[0][x].ToString()));
                        }

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
                             "C_K,C_NA,C_CR,C_NI,C_MNO  from M_ORE_MATERIAL_ANALYSIS" +
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
                        DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);
                        if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                        {
                            this.dataGridView3.AutoGenerateColumns = false;
                            this.dataGridView3.DataSource = dataTable_Count;


                            //20200913添加逻辑，使用再用水分及烧损
                            /*string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                            DataTable dataTable = dBSQL.GetCommand(sql_bins);
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                            }
                            else
                            {
                                string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                vLog.writelog(mistake, -1);
                            }*/
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                            vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,查询二级编码：" + WLBM.ToString() + "上下限制失败" + sql_M_MATERIAL_COOD;
                        vLog.writelog(mistake, -1);
                        return;
                    }
                }
                //返矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                else if (Flag == 6)
                {
                    //获取上下限
                    var sql_M_MATERIAL_COOD = "select top(1) " +
                        "C_TFE_UP,C_TFE_LOWER," +
                        "C_CAO_UP,C_CAO_LOWER," +
                        "C_SIO2_UP,C_SIO2_LOWER," +
                        "C_MGO_UP,C_MGO_LOWER," +
                        "C_AL2O3_UP,C_AL2O3_LOWER ," +
                        "C_P_UP,C_P_LOWER," +
                        "C_S_UP,C_S_LOWER," +
                        "C_MNO_UP,C_MNO_LOWER  " +
                        "from M_MATERIAL_COOD where  L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                    DataTable data = dBSQL.GetCommand(sql_M_MATERIAL_COOD);
                    if (data.Rows.Count > 0)
                    {
                        List<double> _list = new List<double>();
                        //获取指定的上下限
                        for (int x = 0; x < data.Columns.Count; x++)
                        {
                            _list.Add(double.Parse(data.Rows[0][x].ToString()));
                        }

                        string sql_Count = "select top (1) TIMESTAMP,C_TFE," +
                             "C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C," +
                             "C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES,C_TIO2," +
                             "C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN," +
                             "C_K,C_NA,C_CR,C_NI,C_MNO  from M_ORE_MATERIAL_ANALYSIS" +
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
                        DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);
                        if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                        {
                            this.dataGridView3.AutoGenerateColumns = false;
                            this.dataGridView3.DataSource = dataTable_Count;


                            //20200913添加逻辑，使用再用水分及烧损
                            /*string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                            DataTable dataTable = dBSQL.GetCommand(sql_bins);
                            if (dataTable != null && dataTable.Rows.Count > 0)
                            {
                                dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                            }
                            else
                            {
                                string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                vLog.writelog(mistake, -1);
                            }*/
                        }
                        else
                        {
                            string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                            vLog.writelog(mistake, -1);
                        }
                    }
                    else
                    {
                        string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,查询二级编码：" + WLBM.ToString() + "上下限制失败" + sql_M_MATERIAL_COOD;
                        vLog.writelog(mistake, -1);
                        return;
                    }
                }
                //烧结矿
                else if (Flag == 7)
                {
                    MessageBox.Show("选择更新的成分为矿粉，规则中不存在烧结矿");
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为烧结矿，规则不包含";
                    vLog.writelog(mistake, -1);
                }
                //高炉炉渣
                else if (Flag == 8)
                {
                    MessageBox.Show("选择更新的成分为矿粉，规则中不存在高炉炉渣");
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为高炉炉渣，规则不包含";
                    vLog.writelog(mistake, -1);
                }

                else if (Flag == 0)
                {
                    string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类区分上下限失败";
                    vLog.writelog(mistake, -1);
                }

            }
            catch (Exception ee)
            {
                string mistake = "第四部分,默认显示“第二部分”中最新一条记录成分失败" + ee.ToString();
                vLog.writelog(mistake, -1);
            }

        }
        /// <summary>
        ///  第四部分 默认显示“第二部分”中最新“加权批数”条的加权平均成分（插入时间倒叙） 原料检化验表
        /// </summary>
        public void category_4_4()
        {
            try
            {


                string name = comboBox2.Text;
                string sql_name = "select L2_CODE,MAT_DESC from M_MATERIAL_COOD where MAT_DESC = '" + name + "'";

                DataTable dataTable_name = dBSQL.GetCommand(sql_name);
                if (dataTable_name.Rows.Count > 0)
                {
                    //获取修改后的物料成名编码
                    int WLBM = int.Parse(dataTable_name.Rows[0]["L2_CODE"].ToString());

                    if (textBox1.Text == "")
                    {
                        MessageBox.Show("请输入正确的加权批数");
                        return;
                    }
                    else
                    {
                        int Flag = L2_code_Judeg(WLBM);
                        //混匀矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                        if (Flag == 1)
                        {
                            //获取上下限
                            var sql_M_MATERIAL_COOD = "select top(1) " +
                                "C_TFE_UP,C_TFE_LOWER," +
                                "C_CAO_UP,C_CAO_LOWER," +
                                "C_SIO2_UP,C_SIO2_LOWER," +
                                "C_MGO_UP,C_MGO_LOWER," +
                                "C_AL2O3_UP,C_AL2O3_LOWER ," +
                                "C_P_UP,C_P_LOWER," +
                                "C_S_UP,C_S_LOWER," +
                                "C_MNO_UP,C_MNO_LOWER  " +
                                "from M_MATERIAL_COOD where  L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                            DataTable data = dBSQL.GetCommand(sql_M_MATERIAL_COOD);
                            if (data.Rows.Count > 0)
                            {
                                List<double> _list = new List<double>();
                                //获取指定的上下限
                                for (int x = 0; x < data.Columns.Count; x++)
                                {
                                    _list.Add(double.Parse(data.Rows[0][x].ToString()));
                                }

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
                                DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);
                                if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                                {
                                    this.dataGridView3.AutoGenerateColumns = false;
                                    this.dataGridView3.DataSource = dataTable_Count;
                                    //20200913添加逻辑，使用再用水分及烧损
                                    string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                                    DataTable dataTable = dBSQL.GetCommand(sql_bins);
                                    if (dataTable != null && dataTable.Rows.Count > 0)
                                    {
                                        dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                        dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                                    }
                                    else
                                    {
                                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                        vLog.writelog(mistake, -1);
                                    }
                                }
                                else
                                {
                                    string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                                    vLog.writelog(mistake, -1);
                                }





                            }
                            else
                            {
                                string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,查询二级编码：" + WLBM.ToString() + "上下限制失败" + sql_M_MATERIAL_COOD;
                                vLog.writelog(mistake, -1);
                                return;
                            }


                        }
                        //矿粉
                        else if (Flag == 2)
                        {
                            MessageBox.Show("选择更新的成分为矿粉，规则中不存在矿粉判断");
                            string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为矿粉，规则不包含";
                            vLog.writelog(mistake, -1);
                        }
                        //燃料 特殊查询 s、C、灰分、挥发灰、为正常查询，每天可能报一条      
                        //cao、sio2、mgo、al2o3、p、k2o、nao2为特殊成分，每周报一条上下限查询条件
                        else if (Flag == 3)
                        {
                            //获取上下限
                            var sql_M_MATERIAL_COOD = "select top(1) " +
                                "C_S_UP,C_S_LOWER," +
                                "C_C_UP,C_C_LOWER," +
                                "C_ASH_UP,C_ASH_LOWER," +
                                "C_VOLATILES_UP,C_VOLATILES_LOWER," +
                                //cao、sio2、mgo、al2o3、p、k2o、nao2


                                "C_CAO_UP,C_CAO_LOWER," +
                                "C_SIO2_UP,C_SIO2_LOWER," +
                                "C_MGO_UP,C_MGO_LOWER," +
                                "C_AL2O3_UP,C_AL2O3_LOWER ," +
                                "C_P_UP,C_P_LOWER," +
                                "C_K2O_UP,C_K2O_LOWER,  " +
                                "C_NA2O_UP,C_NA2O_LOWER  " +
                                "from M_MATERIAL_COOD where  L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                            DataTable data = dBSQL.GetCommand(sql_M_MATERIAL_COOD);
                            if (data.Rows.Count > 0)
                            {
                                List<double> _list = new List<double>();
                                //获取指定的上下限
                                for (int x = 0; x < data.Columns.Count; x++)
                                {
                                    _list.Add(double.Parse(data.Rows[0][x].ToString()));
                                }
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
                                DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);
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
                                DataTable dataTable_Count1 = dBSQL.GetCommand(sql_Count1);

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
                                        vLog.writelog(mistake, -1);
                                    }
                                    //20200913添加逻辑，使用再用水分及烧损
                                    string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                                    DataTable dataTable = dBSQL.GetCommand(sql_bins);
                                    if (dataTable != null && dataTable.Rows.Count > 0)
                                    {
                                        dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                        dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                                    }
                                    else
                                    {
                                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                        vLog.writelog(mistake, -1);
                                    }
                                }
                                else
                                {
                                    string text = "预计使用成分数据加权数据查询失败";
                                    vLog.writelog(text, 0);
                                }


                            }
                            else
                            {
                                string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,查询二级编码：" + WLBM.ToString() + "上下限制失败" + sql_M_MATERIAL_COOD;
                                vLog.writelog(mistake, -1);
                                return;
                            }

                        }
                        //熔剂 CAO SIO2 MGO AL2O3 P S 
                        else if (Flag == 4)
                        {
                            //获取上下限
                            var sql_M_MATERIAL_COOD = "select top(1) " +

                                "C_CAO_UP,C_CAO_LOWER," +
                                "C_SIO2_UP,C_SIO2_LOWER," +
                                "C_MGO_UP,C_MGO_LOWER," +
                                "C_AL2O3_UP,C_AL2O3_LOWER ," +
                                "C_P_UP,C_P_LOWER," +
                                "C_S_UP,C_S_LOWER  " +

                                "from M_MATERIAL_COOD where  L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                            DataTable data = dBSQL.GetCommand(sql_M_MATERIAL_COOD);
                            if (data.Rows.Count > 0)
                            {
                                List<double> _list = new List<double>();
                                //获取指定的上下限
                                for (int x = 0; x < data.Columns.Count; x++)
                                {
                                    _list.Add(double.Parse(data.Rows[0][x].ToString()));
                                }
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
                                DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);
                                if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                                {
                                    this.dataGridView3.AutoGenerateColumns = false;
                                    this.dataGridView3.DataSource = dataTable_Count;
                                    //20200913添加逻辑，使用再用水分及烧损
                                    string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                                    DataTable dataTable = dBSQL.GetCommand(sql_bins);
                                    if (dataTable != null && dataTable.Rows.Count > 0)
                                    {
                                        dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                        dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                                    }
                                    else
                                    {
                                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                        vLog.writelog(mistake, -1);
                                    }
                                }
                                else
                                {
                                    string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                                    vLog.writelog(mistake, -1);
                                }
                            }
                            else
                            {
                                string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,查询二级编码：" + WLBM.ToString() + "上下限制失败" + sql_M_MATERIAL_COOD;
                                vLog.writelog(mistake, -1);
                                return;
                            }
                        }
                        //除尘灰tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                        else if (Flag == 5)
                        {
                            //获取上下限
                            var sql_M_MATERIAL_COOD = "select top(1) " +
                                "C_TFE_UP,C_TFE_LOWER," +
                                "C_CAO_UP,C_CAO_LOWER," +
                                "C_SIO2_UP,C_SIO2_LOWER," +
                                "C_MGO_UP,C_MGO_LOWER," +
                                "C_AL2O3_UP,C_AL2O3_LOWER ," +
                                "C_P_UP,C_P_LOWER," +
                                "C_S_UP,C_S_LOWER," +
                                "C_MNO_UP,C_MNO_LOWER  " +
                                "from M_MATERIAL_COOD where  L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                            DataTable data = dBSQL.GetCommand(sql_M_MATERIAL_COOD);
                            if (data.Rows.Count > 0)
                            {
                                List<double> _list = new List<double>();
                                //获取指定的上下限
                                for (int x = 0; x < data.Columns.Count; x++)
                                {
                                    _list.Add(double.Parse(data.Rows[0][x].ToString()));
                                }
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
                                DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);
                                if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                                {
                                    this.dataGridView3.AutoGenerateColumns = false;
                                    this.dataGridView3.DataSource = dataTable_Count;
                                    //20200913添加逻辑，使用再用水分及烧损
                                    string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                                    DataTable dataTable = dBSQL.GetCommand(sql_bins);
                                    if (dataTable != null && dataTable.Rows.Count > 0)
                                    {
                                        dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                        dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                                    }
                                    else
                                    {
                                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                        vLog.writelog(mistake, -1);
                                    }
                                }
                                else
                                {
                                    string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                                    vLog.writelog(mistake, -1);
                                }
                            }
                            else
                            {
                                string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,查询二级编码：" + WLBM.ToString() + "上下限制失败" + sql_M_MATERIAL_COOD;
                                vLog.writelog(mistake, -1);
                                return;
                            }
                        }
                        //返矿 tfe、cao、sio2、mgo、al2o3、p、s、mno上下限查询条件
                        else if (Flag == 6)
                        {
                            //获取上下限
                            var sql_M_MATERIAL_COOD = "select top(1) " +
                                "C_TFE_UP,C_TFE_LOWER," +
                                "C_CAO_UP,C_CAO_LOWER," +
                                "C_SIO2_UP,C_SIO2_LOWER," +
                                "C_MGO_UP,C_MGO_LOWER," +
                                "C_AL2O3_UP,C_AL2O3_LOWER ," +
                                "C_P_UP,C_P_LOWER," +
                                "C_S_UP,C_S_LOWER," +
                                "C_MNO_UP,C_MNO_LOWER  " +
                                "from M_MATERIAL_COOD where  L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                            DataTable data = dBSQL.GetCommand(sql_M_MATERIAL_COOD);
                            if (data.Rows.Count > 0)
                            {
                                List<double> _list = new List<double>();
                                //获取指定的上下限
                                for (int x = 0; x < data.Columns.Count; x++)
                                {
                                    _list.Add(double.Parse(data.Rows[0][x].ToString()));
                                }
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
                                DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);
                                if (dataTable_Count != null && dataTable_Count.Rows.Count > 0)
                                {
                                    this.dataGridView3.AutoGenerateColumns = false;
                                    this.dataGridView3.DataSource = dataTable_Count;
                                    //20200913添加逻辑，使用再用水分及烧损
                                    string sql_bins = "select isnull(C_H2O,0) as C_H2O ,isnull(C_LOT,0) as C_LOT from M_MATERIAL_BINS where BIN_NUM_SHOW = " + CH + "";
                                    DataTable dataTable = dBSQL.GetCommand(sql_bins);
                                    if (dataTable != null && dataTable.Rows.Count > 0)
                                    {
                                        dataGridView3.Rows[0].Cells["C_H2O"].Value = dataTable.Rows[0]["C_H2O"].ToString();
                                        dataGridView3.Rows[0].Cells["C_LOT"].Value = dataTable.Rows[0]["C_LOT"].ToString();
                                    }
                                    else
                                    {
                                        string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败(特殊成分，再用水份烧损)" + sql_bins;
                                        vLog.writelog(mistake, -1);
                                    }
                                }
                                else
                                {
                                    string mistake = "点击计算成分按钮，预计使用成分数据加权数据查询失败" + sql_Count;
                                    vLog.writelog(mistake, -1);
                                }
                            }
                            else
                            {
                                string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,查询二级编码：" + WLBM.ToString() + "上下限制失败" + sql_M_MATERIAL_COOD;
                                vLog.writelog(mistake, -1);
                                return;
                            }
                        }
                        //烧结矿
                        else if (Flag == 7)
                        {
                            MessageBox.Show("选择更新的成分为矿粉，规则中不存在烧结矿");
                            string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为烧结矿，规则不包含";
                            vLog.writelog(mistake, -1);
                        }
                        //高炉炉渣
                        else if (Flag == 8)
                        {
                            MessageBox.Show("选择更新的成分为矿粉，规则中不存在高炉炉渣");
                            string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类为高炉炉渣，规则不包含";
                            vLog.writelog(mistake, -1);
                        }

                        else if (Flag == 0)
                        {
                            string mistake = "第四部分 默认显示模型结果表最新一条记录成分失败,判断二级编码种类区分上下限失败";
                            vLog.writelog(mistake, -1);
                        }





                    }
                }
                #region 屏蔽
                ////获取修改后的物料成名编码
                //int WLBM = int.Parse(dataTable_name.Rows[0]["L2_CODE"].ToString());
                //int JQPJ = int.Parse(textBox1.Text);
                //#region 查询物料的上下限
                //string sql_RANGE = "SELECT TOP (1) " +
                //    "[C_TFE_UP] ," +
                //    "[C_TFE_LOWER] ," +
                //    "[C_FEO_UP] ,[C_FEO_LOWER] ," +
                //    "[C_CAO_UP] ,[C_CAO_LOWER] ," +
                //    "[C_SIO2_UP] ,[C_SIO2_LOWER]," +
                //    "[C_AL2O3_UP] ,[C_AL2O3_LOWER]," +
                //    "[C_MGO_UP] ,[C_MGO_LOWER] ," +
                //    "[C_S_UP] ,[C_S_LOWER] ," +
                //    "[C_P_UP] ,[C_P_LOWER] ," +
                //    "[C_C_UP] ,[C_C_LOWER] ," +
                //    "[C_MN_UP] ,[C_MN_LOWER]," +
                //    "[C_LOT_UP] ,[C_LOT_LOWER]," +
                //    "[C_R_UP]  ,[C_R_LOWER] ," +
                //    "[C_H2O_UP] ,[C_H2O_LOWER] ," +
                //    "[C_ASH_UP] ,[C_ASH_LOWER] ," +
                //    "[C_VOLATILES_UP] ,[C_VOLATILES_LOWER] ," +
                //    "[C_TIO2_UP] ,[C_TIO2_LOWER] ," +
                //    "[C_K2O_UP] ,[C_K2O_LOWER] ," +
                //    "[C_NA2O_UP] ,[C_NA2O_LOWER] ," +
                //    "[C_PBO_UP]  ,[C_PBO_LOWER] ," +
                //    "[C_ZNO_UP] ,[C_ZNO_LOWER] ," +
                //    "[C_TI_UP] ,[C_TI_LOWER] ," +
                //    "[C_AS_UP] ,[C_AS_LOWER] ," +
                //    "[C_CU_UP] ,[C_CU_LOWER] ," +
                //    "[C_PB_UP] ,[C_PB_LOWER]," +
                //    "[C_ZN_UP] ,[C_ZN_LOWER] ," +
                //    "[C_K_UP]  ,[C_K_LOWER] ," +
                //    "[C_NA_UP] ,[C_NA_LOWER] ," +
                //    "[C_CR_UP] ,[C_CR_LOWER] ," +
                //    "[C_NI_UP] ,[C_NI_LOWER] ," +
                //    "[C_MNO_UP] ,[C_MNO_LOWER] ," +
                //    "[MY_TYPE] FROM [NBSJ].[dbo].[M_MATERIAL_COOD] WHERE L2_CODE = " + WLBM + "";
                //DataTable dataTable_RANGE = dBSQL.GetCommand(sql_RANGE);
                //if (dataTable_RANGE.Rows.Count > 0 )
                //{
                //    ///   key 成分名称 item1 上限 item2 下限
                //    Dictionary<string, Tuple<float, float>> _Dic = new Dictionary<string, Tuple<float, float>>();
                //    _Dic.Add("C_TFE", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_TFE_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_TFE_LOWER"].ToString())));
                //    _Dic.Add("C_FEO", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_FEO_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_FEO_LOWER"].ToString())));
                //    _Dic.Add("C_CAO", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_CAO_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_CAO_LOWER"].ToString())));
                //    _Dic.Add("C_SIO2", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_SIO2_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_SIO2_LOWER"].ToString())));
                //    _Dic.Add("C_AL2O3", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_AL2O3_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_AL2O3_LOWER"].ToString())));
                //    _Dic.Add("C_MGO", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_MGO_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_MGO_LOWER"].ToString())));
                //    _Dic.Add("C_S", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_S_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_S_LOWER"].ToString())));
                //    _Dic.Add("C_P", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_P_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_P_LOWER"].ToString())));
                //    _Dic.Add("C_C", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_C_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_C_LOWER"].ToString())));
                //    _Dic.Add("C_MN", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_MN_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_MN_LOWER"].ToString())));
                //    _Dic.Add("C_LOT", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_LOT_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_LOT_LOWER"].ToString())));
                //    _Dic.Add("C_R", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_R_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_R_LOWER"].ToString())));
                //    _Dic.Add("C_H2O", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_H2O_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_H2O_LOWER"].ToString())));
                //    _Dic.Add("C_ASH", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_ASH_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_ASH_LOWER"].ToString())));
                //    _Dic.Add("C_VOLATILES", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_VOLATILES_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_VOLATILES_LOWER"].ToString())));
                //    _Dic.Add("C_TIO2", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_TIO2_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_TIO2_LOWER"].ToString())));
                //    _Dic.Add("C_K2O", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_K2O_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_K2O_LOWER"].ToString())));
                //    _Dic.Add("C_NA2O", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_NA2O_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_NA2O_LOWER"].ToString())));
                //    _Dic.Add("C_PBO", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_PBO_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_PBO_LOWER"].ToString())));
                //    _Dic.Add("C_ZNO", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_ZNO_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_ZNO_LOWER"].ToString())));
                //    _Dic.Add("C_F", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_TI_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_TI_LOWER"].ToString())));
                //    _Dic.Add("C_AS", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_AS_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_AS_LOWER"].ToString())));
                //    _Dic.Add("C_CU", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_CU_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_CU_LOWER"].ToString())));
                //    _Dic.Add("C_PB", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_PB_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_PB_LOWER"].ToString())));
                //    _Dic.Add("C_ZN", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_ZN_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_ZN_LOWER"].ToString())));
                //    _Dic.Add("C_K", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_K_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_K_LOWER"].ToString())));
                //    _Dic.Add("C_NA", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_NA_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_NA_LOWER"].ToString())));
                //    _Dic.Add("C_CR", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_CR_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_CR_LOWER"].ToString())));
                //    _Dic.Add("C_NI", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_NI_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_NI_LOWER"].ToString())));
                //    _Dic.Add("C_MNO", new Tuple<float, float>(float.Parse(dataTable_RANGE.Rows[0]["C_MNO_UP"].ToString()), float.Parse(dataTable_RANGE.Rows[0]["C_MNO_LOWER"].ToString())));

                //    #endregion
                //    string sql_Count = "select top " + JQPJ + "  " +
                //        "            isnull(C_TFE,0) AS C_TFE ," +
                //                    "isnull(C_FEO,0) AS C_FEO," +
                //                    "isnull(C_CAO,0) AS C_CAO," +
                //                    "isnull(C_SIO2,0) AS C_SIO2," +
                //                    "isnull(C_AL2O3,0) AS C_AL2O3 ," +
                //                    "isnull(C_MGO,0) AS C_MGO," +
                //                    "isnull(C_S,0) AS C_S," +
                //                    "isnull(C_P,0) AS C_P," +
                //                    "isnull(C_C,0) AS C_C," +
                //                    "isnull(C_MN,0) AS C_MN," +
                //                    "isnull(C_LOT,0) AS C_LOT," +
                //                    "isnull(C_R,0) AS C_R ," +
                //                    "isnull(C_H2O,0) AS C_H2O," +
                //                    "isnull(C_ASH,0) AS C_ASH," +
                //                    "isnull(C_VOLATILES,0) AS C_VOLATILES," +
                //                    "isnull(C_TIO2,0) AS C_TIO2," +
                //                    "isnull(C_K2O,0) AS C_K2O," +
                //                    "isnull(C_NA2O,0) AS C_NA2O," +
                //                    "isnull(C_PBO,0) AS C_PBO," +
                //                    "isnull(C_ZNO,0) AS C_ZNO," +
                //                    "isnull(C_F,0) AS C_F," +
                //                    "isnull(C_AS,0) AS C_AS," +
                //                    "isnull(C_CU,0) AS C_CU," +
                //                    "isnull(C_PB,0) AS C_PB," +
                //                    "isnull(C_ZN,0) AS C_ZN ," +
                //                    "isnull(C_K,0) AS C_K," +
                //                    "isnull(C_NA,0) AS C_NA," +
                //                    "isnull(C_CR,0) AS C_CR," +
                //                    "isnull(C_NI,0) AS C_NI," +
                //                    "isnull(C_MNO,0) AS C_MNO" +
                //                    "  from M_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + " order by SAMPLETIME desc";
                //    DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);
                //    //判断M_MATERIAL_ANALYSIS表查询出来的行数是否和用户输入的加权批数相同
                //    if (dataTable_Count.Rows.Count == JQPJ)
                //    {
                //        List<float> element_sum = new List<float>();
                //        for (int col = 0; col < dataTable_Count.Columns.Count; col++)
                //        {
                //            //********begin 0200825修改，把计算后的加权批数计算放到加和中，在未计算之前首先判断每一个数据是否存在超限
                //            float A = 0;
                //            for (int x = 0; x < dataTable_Count.Rows.Count; x++)
                //            {

                //                A += float.Parse(dataTable_Count.Rows[x][col].ToString());
                //            }
                //            element_sum.Add(A / JQPJ);
                //            //******20200825 end 

                //            ////**************** 
                //            ////成分名称
                //            //string element_name = dataTable_Count.Columns[col].ColumnName.ToString();
                //            ////成分名称上限
                //            //float up = _Dic[element_name].Item1;
                //            ////成分名称下限
                //            //float lower = _Dic[element_name].Item2;
                //            //float A = 0;
                //            //for (int x = 0; x < dataTable_Count.Rows.Count; x++)
                //            //{
                //            //    //参与计算的成分
                //            //    float _a = float.Parse(dataTable_Count.Rows[x][col].ToString());
                //            //    if (_a > lower & _a< up)
                //            //    {
                //            //        A += _a;
                //            //    }
                //            //    else
                //            //    {

                //            //    }
                //            //    //A += float.Parse(dataTable_Count.Rows[x][col].ToString());
                //            //}
                //            element_sum.Add(A / JQPJ);

                //            //*****************

                //        }
                //        dataTable_Count.Clear();
                //        DataRow row = dataTable_Count.NewRow();
                //        for (int col = 0; col < dataTable_Count.Columns.Count; col++)
                //        {
                //            row[col] = element_sum[col];
                //        }
                //        //加权计算后的数据
                //        dataTable_Count.Rows.Add(row);
                //        List<int> color_singer = new List<int>();
                //        //判断上下限

                //        for (int y = 0; y < dataTable_Count.Rows.Count; y++)
                //        {
                //            DataRow dataRow = dataTable_Count.Rows[y];
                //            for (int x = 0; x < dataTable_Count.Columns.Count; x++)
                //            {
                //                //颜色变化标志位
                //                int singel = 0;
                //                //成分名称
                //                string element_name = dataTable_Count.Columns[x].ColumnName.ToString();
                //                //计算值
                //                float count = float.Parse(dataTable_Count.Rows[0][element_name].ToString());
                //                //上限
                //                float up = _Dic[element_name].Item1;
                //                //下限
                //                float lower = _Dic[element_name].Item2;
                //                if (count > up)
                //                {
                //                    singel = 1;
                //                    dataRow.BeginEdit();
                //                    dataRow[element_name] = up;
                //                    dataRow.EndEdit();
                //                    color_singer.Add(singel);
                //                }
                //                else
                //                if (count < lower)
                //                {
                //                    singel = 2;
                //                    dataRow.BeginEdit();
                //                    dataRow[element_name] = lower;
                //                    dataRow.EndEdit();
                //                    color_singer.Add(singel);
                //                }
                //                else
                //                {
                //                    singel = 0;
                //                    color_singer.Add(singel);

                //                }

                //            }
                //            dataTable_Count.AcceptChanges();//保存修改的结果。

                //        }
                //        this.dataGridView3.AutoGenerateColumns = false;
                //        //    this.dataGridView3.DataSource = null;
                //        this.dataGridView3.DataSource = dataTable_Count;

                //        //改变页面背景颜色
                //        for (int i = 0; i < color_singer.Count; i++)
                //        {
                //            int ii = color_singer[i];
                //            if (ii == 1)
                //            {
                //                this.dataGridView3.Rows[0].Cells[i].Style.BackColor = System.Drawing.Color.Red;
                //            }
                //            else if (ii == 2)
                //            {
                //                this.dataGridView3.Rows[0].Cells[i].Style.BackColor = System.Drawing.Color.Green;
                //            }

                //            this.label12.Visible = true;
                //            this.label12.BackColor = Color.Green;
                //            this.label13.Visible = true;
                //            this.label13.BackColor = Color.Red;
                //        }
                //    }
                //    else
                //    {
                //        MessageBox.Show("用户输入的加权批数不正确，建议使用"+ JQPJ + "加权批数");
                //    }

                //  }
                #endregion
            }
            catch (Exception ee)
            {
                string mistake = " 第四部分，默认显示“第二部分”中最新“加权批数”条的加权平均成分错误" + ee.ToString();
                vLog.writelog(mistake, -1);
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
                DataTable dataTable_Count = dBSQL.GetCommand(sql_Count);
                this.dataGridView3.AutoGenerateColumns = false;
                this.dataGridView3.DataSource = dataTable_Count;
            }
            catch (Exception ee)
            {
                string mistake = "第四部分,默认显示在用成分失败" + ee.ToString();
                vLog.writelog(mistake, -1);

            }
        }
        //计算成分按钮
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string name = comboBox2.Text;
            string sql_name = "select L2_CODE,MAT_DESC from M_MATERIAL_COOD where MAT_DESC = '" + name + "'";
            DataTable dataTable_name = dBSQL.GetCommand(sql_name);
            int WLBM = int.Parse(dataTable_name.Rows[0]["L2_CODE"].ToString());
            if (textBox2.Text != String.Empty)
            {
                double price = Convert.ToDouble(textBox2.Text);
            }
            //double price = Convert.ToDouble(textBox2.Text);
            double ratioUp = Convert.ToDouble(textBox3.Text);
            double ratioDown = Convert.ToDouble(textBox1.Text);
            if (ratioUp < ratioDown)
            {
                MessageBox.Show("配比上限必须大于配比下限!");
            }
            else
            {
                int weight = Convert.ToInt32(textBox4.Text);
                string sql = "select  TOP(" + weight + ") " +
                             "AVG(isnull(C_TFE,0)) AS C_TFE ," +
                             "AVG(isnull(C_FEO,0)) AS C_FEO," +
                             "AVG(isnull(C_CAO,0)) AS C_CAO," +
                             "AVG(isnull(C_SIO2,0)) AS C_SIO2," +
                             "AVG(isnull(C_AL2O3,0)) AS C_AL2O3 ," +
                             "AVG(isnull(C_MGO,0)) AS C_MGO," +
                             "AVG(isnull(C_S,0)) AS C_S," +
                             "AVG(isnull(C_P,0)) AS C_P," +
                             "AVG(isnull(C_C,0)) AS C_C," +
                             //"AVG(isnull(C_MN,0)) AS C_MN," +
                             "AVG(isnull(C_LOT,0)) AS C_LOT," +
                             "AVG(isnull(C_R,0)) AS C_R ," +
                             "AVG(isnull(C_H2O,0)) AS C_H2O," +
                             "AVG(isnull(C_ASH,0)) AS C_ASH," +
                             "AVG(isnull(C_VOLATILES,0)) AS C_VOLATILES," +
                             "AVG(isnull(C_TIO2,0)) AS C_TIO2," +
                             "AVG(isnull(C_K2O,0)) AS C_K2O," +
                             "AVG(isnull(C_NA2O,0)) AS C_NA2O," +
                             //"AVG(isnull(C_PBO,0)) AS C_PBO," +
                             //"AVG(isnull(C_ZNO,0)) AS C_ZNO," +
                             //"AVG(isnull(C_F,0)) AS C_F," +
                             "AVG(isnull(C_AS,0)) AS C_AS," +
                             "AVG(isnull(C_CU,0)) AS C_CU," +
                             "AVG(isnull(C_PB,0)) AS C_PB," +
                             "AVG(isnull(C_ZN,0)) AS C_ZN ," +
                             //"AVG(isnull(C_K,0)) AS C_K," +
                             //"AVG(isnull(C_NA,0)) AS C_NA," +
                             //"AVG(isnull(C_CR,0)) AS C_CR," +
                             //"AVG(isnull(C_NI,0)) AS C_NI," +
                             "AVG(isnull(C_MNO,0)) AS C_MNO" +
                             " FROM M_ORE_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + "  group by TIMESTAMP order by TIMESTAMP desc";
                DataTable dataTable = dBSQL.GetCommand(sql);
                dataTable.Columns.Add("MAT_NAME").SetOrdinal(0);
                dataTable.Columns.Add("BILL_UPPER").SetOrdinal(3);
                dataTable.Columns.Add("BILL_LOWER").SetOrdinal(4);
                dataTable.Rows[0]["MAT_NAME"] = dataTable_name.Rows[0]["MAT_DESC"].ToString();
                dataTable.Rows[0]["BILL_UPPER"] = ratioUp;
                dataTable.Rows[0]["BILL_LOWER"] = ratioDown;
                string sql2 = "select top(1) ORE_CLASS,UNIT_PRICE from M_ORE_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                DataTable dataTable2 = dBSQL.GetCommand(sql2);
                dataTable.Columns.Add("MAT_CLASS").SetOrdinal(1);
                dataTable.Columns.Add("UNIT_PRICE").SetOrdinal(2);
                dataTable.Rows[0]["MAT_CLASS"] = dataTable2.Rows[0]["ORE_CLASS"].ToString();
                dataTable.Rows[0]["UNIT_PRICE"] = dataTable2.Rows[0]["UNIT_PRICE"].ToString();
                dataGridView3.DataSource = dataTable;
            }
            /*//加权平均数
            int JQPJ = int.Parse(textBox4.Text);
            //加权平均数等于1
            if (JQPJ == 1)
            {
                getData();
                category_4_3();
                //提示用户进行修改
                this.label4.Text = "预计使用成分-(成分可修改)";
                this.label4.BackColor = Color.Yellow;
                this.dataGridView3.ReadOnly = false;
            }
            else
            //加权平均数等于0
            if (JQPJ == 0)
            {
                getData();
                category_4_5();
                //提示用户进行修改
                this.label4.Text = "预计使用成分-(成分可修改)";
                this.label4.BackColor = Color.Yellow;
                this.dataGridView3.ReadOnly = false;

            }
            else
            //加权平均数大于1
            if (JQPJ > 1)

            {
                getData();
                category_4_4();
                //提示用户进行修改
                this.label4.Text = "预计使用成分-(成分可修改)";
                this.label4.BackColor = Color.Yellow;
                this.dataGridView3.ReadOnly = false;
            }
            else
            {
                MessageBox.Show("维护状态为手动维护，原料追踪状态为禁用，加权平均数异常");
            }*/
        }
    }
}
