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

namespace WindowsFormsApp2.page.analyze
{
    public partial class Frm_JHPK_insert : Form
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public static bool isopen = false;
        public Frm_JHPK_insert()
        {
            InitializeComponent();
            combox();
            getData();
        }
        public void getData()
        {
            int WLBM = 0;
            string name = comboBox2.Text;

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
                string sql1 = "select  MAT_NAME,MAT_CLASS,UNIT_PRICE,BILL_UPPER,BILL_LOWER,C_TFE,C_FEO,C_SIO2,C_CAO,C_MGO,C_AL2O3,C_S,C_P,C_LOT,C_H2O,C_AS,C_PB,C_ZN,C_CU,C_K2O," +
                    "C_NA2O from MC_ORECAL_MAT_ANA_RECORD where MAT_NAME = '" + name + "' and TIMESTAMP <= '" + time2 + "' and TIMESTAMP >='" + time1 + "' order by TIMESTAMP desc";
                DataTable dataTable1 = dBSQL.GetCommand(sql1);
                dataGridView3.DataSource = dataTable1;

            }
            catch (Exception ee)
            {

            }

        }
        //确认使用按钮,更新数据
        private void simpleButton3_Click(object sender, EventArgs e)
        {

            string sql = "";

        }
    }
}
