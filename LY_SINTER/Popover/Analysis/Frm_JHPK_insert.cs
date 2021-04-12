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

namespace LY_SINTER.Popover.Analysis
{
    public partial class Frm_JHPK_insert : Form
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        //声明委托和事件
        public delegate void TransfDelegate_YHPK(DataTable dataTable);
        //声明委托和事件
        public event TransfDelegate_YHPK _TransfDelegate_YHPK;
        public static bool isopen = false;
        public Frm_JHPK_insert()
        {
            InitializeComponent();
            combox();
            getData();
        }
        public void getData()
        {
            string name = comboBox2.SelectedValue.ToString();
            /*string sql_name = "select L2_CODE,MAT_DESC from M_MATERIAL_COOD where L2_CODE = '" + name + "'";

            DataTable dataTable_name = dBSQL.GetCommand(sql_name);
            int WLBM = int.Parse(dataTable_name.Rows[0]["L2_CODE"].ToString());*/
            string sql = "select top(10) ROW_NUMBER() over(order by TIMESTAMP desc) as ID,TIMESTAMP,BATCH_NUM,SAMPLETIME,REOPTTIME,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C,C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES,C_TIO2,C_K2O,C_NA2O," +
                "C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN,C_K,C_NA,C_CR,C_NI,C_MNO  from M_ORE_MATERIAL_ANALYSIS " +
                "where L2_CODE = " + name + " order by TIMESTAMP desc";
            DataTable dataTable = dBSQL.GetCommand(sql);
            dataGridView1.DataSource = dataTable;
        }
        public class Info
        {
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

            //*****原料下拉框选项赋值
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
            this.comboBox2.ValueMember = "value";


        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
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
        private void simpleButton5_Click(object sender, EventArgs e)
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
                /*string sql1 = "select  MAT_NAME,MAT_CLASS,UNIT_PRICE,BILL_UPPER,BILL_LOWER,C_TFE,C_FEO,C_SIO2,C_CAO,C_MGO,C_AL2O3,C_S,C_P,C_LOT,C_H2O,C_AS,C_PB,C_ZN,C_CU,C_K2O," +
                    "C_NA2O from MC_ORECAL_MAT_ANA_RECORD where MAT_NAME = '" + name + "' and TIMESTAMP <= '" + time2 + "' and TIMESTAMP >='" + time1 + "' order by TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                dataGridView3.DataSource = dataTable1;*/

            }
            catch (Exception ee)
            {

            }

        }
        //确认使用按钮,更新主页面数据，不存库
        private void simpleButton3_Click(object sender, EventArgs e)
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
        public void selectdw1(int n)
        {
            try
            {
                string name = comboBox2.Text;

                string sql_name = "select L2_CODE,MAT_DESC from M_MATERIAL_COOD where MAT_DESC = '" + name + "'";

                DataTable dataTable_name = dBSQL.GetCommand(sql_name);
                int WLBM = int.Parse(dataTable_name.Rows[0]["L2_CODE"].ToString());
                string time1 = dateTimePicker1.Value.ToString();
                string time2 = dateTimePicker2.Value.ToString();
                string sql = "select top("+n+") TIMESTAMP,BATCH_NUM,SAMPLETIME,REOPTTIME,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C,C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES,C_TIO2,C_K2O,C_NA2O," +
                    "C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN,C_K,C_NA,C_CR,C_NI,C_MNO  from M_ORE_MATERIAL_ANALYSIS " +
                    "where L2_CODE = " + WLBM + " order by TIMESTAMP desc";
                DataTable dataTable = dBSQL.GetCommand(sql);
                dataGridView1.DataSource = dataTable;
                /*string sql1 = "select  MAT_NAME,MAT_CLASS,UNIT_PRICE,BILL_UPPER,BILL_LOWER,C_TFE,C_FEO,C_SIO2,C_CAO,C_MGO,C_AL2O3,C_S,C_P,C_LOT,C_H2O,C_AS,C_PB,C_ZN,C_CU,C_K2O," +
                    "C_NA2O from MC_ORECAL_MAT_ANA_RECORD where MAT_NAME = '" + name + "' and TIMESTAMP <= '" + time2 + "' and TIMESTAMP >='" + time1 + "' order by TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                dataGridView3.DataSource = dataTable1;*/

            }
            catch (Exception ee)
            {

            }

        }
        //计算成分按钮，更新dataGridView3数据
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            int weight = Convert.ToInt32(textBox4.Text);
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
            if(ratioUp < ratioDown)
            {
                MessageBox.Show("配比上限必须大于配比下限!");
            }
            else
            {
                string sql = "select   "+
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
                             " FROM " +
                             "(select TOP(" + weight + ") * from M_ORE_MATERIAL_ANALYSIS where L2_CODE = " + WLBM + " order by TIMESTAMP desc )as NET";
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
            selectdw1(weight);
        }
    }
}
