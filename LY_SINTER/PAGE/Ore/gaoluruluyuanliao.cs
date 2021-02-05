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
using LY_SINTER.Custom;

namespace WindowsFormsApp2.page.analyze
{
    public partial class gaoluruluyuanliao : UserControl
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public gaoluruluyuanliao()
        {
            InitializeComponent();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            combox();
        }
        public void table1GetData(DateTime start, DateTime end)
        {
            try
            {
                DataTable table = new DataTable();
                table.Rows.Clear();
                string sql = "";
                if (comboBox1.Text == "全部")
                {
                    sql = "select BATCH_NUM,b.MAT_DESC,ORE_CLASS,PLACE_ORIGIN,a.UNIT_PRICE,C_TFE,C_FEO,C_SIO2,C_CAO,C_MGO,C_AL2O3,C_S,C_P,C_LOT,C_H2O,C_ASH,C_PBO,C_ZN," +
                    "C_CU,C_K2O,C_NA2O,C_TIO2 from M_BF_MATERIAL_ANALYSIS a, M_MATERIAL_COOD b where a.L2_CODE = b.L2_CODE " +
                    "and a.TIMESTAMP between '" + start + "' and '" + end + "'";
                }
                else
                {
                    sql = "select BATCH_NUM, b.MAT_DESC,ORE_CLASS,PLACE_ORIGIN,a.UNIT_PRICE,C_TFE,C_FEO,C_SIO2,C_CAO,C_MGO,C_AL2O3,C_S,C_P,C_C,C_LOT,C_MN,C_R,C_H2O,C_ASH," +
                        "C_VOLATILES,C_TIO2,C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN,C_K,C_NA,C_CR,C_NI,C_MNO" +
                        " from M_BF_MATERIAL_ANALYSIS a, M_MATERIAL_COOD b where a.L2_CODE = b.L2_CODE and b.MAT_DESC = '" + comboBox2.Text + "' " +
                        "and a.TIMESTAMP between '" + start + "' and '" + end + "'";
                }
                table = dBSQL.GetCommand(sql);
                if (table.Rows.Count > 0)
                {
                    d1.DataSource = table;
                }
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    d1.Rows[i].Cells["id"].Value = i + 1;
                }
            }
            catch
            {

            }
        }
        //查询按钮
        private void simpleButton2_click(object sender, EventArgs e)
        {
            DateTime time1 = Convert.ToDateTime(textBox_begin.Text);
            DateTime time2 = Convert.ToDateTime(textBox_end.Text);
            table1GetData(time1, time2);
        }
        //删除按钮
        private void simpleButton1_click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("确定删除吗？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int a = d1.CurrentRow.Index;
                    string data = d1.Rows[a].Cells["BATCH_NUM"].Value.ToString();
                    string name = d1.Rows[a].Cells["MAT_DESC"].Value.ToString();
                    string delSql = "delete from M_BF_MATERIAL_ANALYSIS where BATCH_NUM = '" + data + "' and L2_CODE in(select L2_CODE from M_MATERIAL_COOD where MAT_DESC='" + name + "')";
                    int k = dBSQL.CommandExecuteNonQuery(delSql);
                    if (k > 0)
                    {
                        MessageBox.Show("删除成功");
                        DateTime d1 = Convert.ToDateTime(textBox_begin.Text);
                        DateTime d2 = Convert.ToDateTime(textBox_end.Text);
                        table1GetData(d1, d2);
                    }
                    else
                    {
                        MessageBox.Show("删除失败");
                    }
                }
                else { return; }
            }
            catch (Exception ee)
            {

            }
            
        }

        //修改按钮
        private void simpleButton3_click(object sender, EventArgs e)
        {
            try
            {
                string num = d1.CurrentRow.Cells["BATCH_NUM"].Value.ToString();
                string name = d1.CurrentRow.Cells["MAT_DESC"].Value.ToString();
                Frm_GLRL_update form_display = new Frm_GLRL_update(num, name);
                if (Frm_GLRL_update.isopen == false)
                {
                    form_display.ShowDialog();
                }
                else
                {
                    form_display.Activate();
                }
                DateTime start = Convert.ToDateTime(textBox_begin.Text);
                DateTime end = Convert.ToDateTime(textBox_end.Text);
                table1GetData(start, end);
            }
            catch
            {

            } 
        }
        public class Info
        {
            public int value { get; set; }
            public string name { get; set; }
        }
        private void simpleButton4_click(object sender, EventArgs e)
        {

        }
        public void combox()
        {
            try
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
                string sql = " select M_DESC as name,M_TYPE as value from M_MATERIAL_COOD_CONFIG where M_TYPE between 1 and 8";
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
                //物料下拉框默认显示
                //this.comboBox2.Text = WLMC;
            }
            catch
            {

            }
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
    }
}
