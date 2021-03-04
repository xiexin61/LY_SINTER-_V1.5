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
using LY_SINTER.Popover.Analysis;

namespace LY_SINTER.PAGE.Analysis
{
    public partial class tiekuangfenjichuxingneng : UserControl
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public tiekuangfenjichuxingneng()
        {
            InitializeComponent();
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            this.rowMergeView1.AddSpanHeader(4, 8, "粒度组成%");
            getNewTime();
            getName();
        }
        /// <summary>
        /// 开始时间&结束时间赋值
        /// </summary>
        public void dateTimePicker_value()
        {
            try
            {
                //结束时间
                DateTime time_end = DateTime.Now;
                //开始时间
                DateTime time_begin = time_end.AddMonths(-1);

                textBox_begin.Text = time_begin.ToString();
                textBox_end.Text = time_end.ToString();
            }
            catch (Exception ee)
            {

            }
        }
        //获取物料名信息
        public void getName()
        {
            string sql = "select MAT_DESC from M_MATERIAL_COOD where L2_CODE between 120 and 299";
            DataTable table = dBSQL.GetCommand(sql);
            DataRow row = table.NewRow();
            row["MAT_DESC"] = "全部";
            table.Rows.InsertAt(row, 0);
            comboBox1.DataSource = table;
            comboBox1.DisplayMember = "MAT_DESC";
            comboBox1.ValueMember = "MAT_DESC";
        }
        //最新调整时间
        public void getNewTime()
        {
            string sql = "select top(1) TIMESTAMP from M_ORE_MATERIAL_ANALYSIS order by TIMESTAMP desc;";
            DataTable table = dBSQL.GetCommand(sql);
            if (table.Rows.Count > 0)
            {
                string time = table.Rows[0][0].ToString();
                this.label8.Text = "最新调整时间:" + time;
            }
            
        }
        //部分1表格数据查询
        public void table1GetData(DateTime start, DateTime end)
        {
            DataTable table = new DataTable();
            table.Rows.Clear();
            string sql = "";
            if (comboBox1.Text == "全部")
            {
                sql = "select BATCH_NUM,b.MAT_DESC,ORE_CLASS,a.PLACE_ORIGIN,a.UNIT_PRICE,C_TFE,C_FEO,C_SIO2,C_CAO,C_MGO,C_AL2O3,C_S,C_P,C_LOT,C_H2O,C_ASH,C_PBO,C_ZN," +
                "C_CU,C_K2O,C_NA2O,C_TIO2 from M_ORE_MATERIAL_ANALYSIS a, M_MATERIAL_COOD b where a.L2_CODE = b.L2_CODE " +
                "and a.TIMESTAMP between '" + start + "' and '" + end + "'";
            }
            else
            {
                sql = "select BATCH_NUM, b.MAT_DESC,ORE_CLASS,a.PLACE_ORIGIN,a.UNIT_PRICE,C_TFE,C_FEO,C_SIO2,C_CAO,C_MGO,C_AL2O3,C_S,C_P,C_LOT,C_H2O,C_ASH,C_PBO,C_ZN,C_CU," +
                    "C_K2O,C_NA2O,C_TIO2 from M_ORE_MATERIAL_ANALYSIS a, M_MATERIAL_COOD b where a.L2_CODE = b.L2_CODE and b.MAT_DESC = '" + comboBox1.Text + "' " +
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
        //部分2表格数据查询
        public void table2GetData(DateTime start, DateTime end)
        {
            string sql = "";
            if (comboBox1.Text == "全部")
            {
                sql = "select BATCH_NUM,b.MAT_DESC,ORE_CLASS,GRIT_8,GRIT_5_8,GRIT_3_5,GRIT_1_3,GRIT_05_1,GRIT__025_05," +
                "GRIT_025,GRIT_AVG,W_CAP_05,W_MOL_05,DEN_B,DEN_T,POROSITY from M_ORE_MATERIAL_ANALYSIS a, M_MATERIAL_COOD b where a.L2_CODE = b.L2_CODE " +
                "and a.TIMESTAMP between '" + start + "' and '" + end + "'";
            }
            else
            {
                sql = "select BATCH_NUM,b.MAT_DESC,ORE_CLASS,GRIT_8,GRIT_5_8,GRIT_3_5,GRIT_1_3,GRIT_05_1,GRIT__025_05,GRIT_025,GRIT_AVG,W_CAP_05,W_MOL_05,DEN_B,DEN_T," +
                    "POROSITY from M_ORE_MATERIAL_ANALYSIS a, M_MATERIAL_COOD b where a.L2_CODE = b.L2_CODE and b.MAT_DESC = '" + comboBox1.Text + "' " +
                    "and a.TIMESTAMP between '" + start + "' and '" + end + "'";
            }
            DataTable table = dBSQL.GetCommand(sql);
            if (table.Rows.Count > 0)
            {
                rowMergeView1.DataSource = table;
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                rowMergeView1.Rows[i].Cells["number"].Value = i + 1;
            }
        }
        private void mouse_click(object sender, MouseEventArgs e)
        {


        }
        //参数修改按钮
        private void simpleButton4_click(object sender, EventArgs e)
        {
            Frm_TKFXN_CSXG frm_TKFXN_CSXG = new Frm_TKFXN_CSXG();
            if (Frm_TKFXN_CSXG.isopen == false)
            {
                frm_TKFXN_CSXG.ShowDialog();
            }
            else
            {
                frm_TKFXN_CSXG.Activate();
            }
        }
        //查询按钮
        private void simpleButton2_click(object sender, EventArgs e)
        {
            DateTime time1 = Convert.ToDateTime(textBox_begin.Text);
            DateTime time2 = Convert.ToDateTime(textBox_end.Text);
            table1GetData(time1, time2);
            table2GetData(time1, time2);
        }
        //删除按钮
        private void simpleButton1_click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("确定删除吗？", "删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                int a = d1.CurrentRow.Index;
                string data = d1.Rows[a].Cells["BATCH_NUM"].Value.ToString();
                string name = d1.Rows[a].Cells["MAT_DESC"].Value.ToString();
                string delSql = "delete from M_ORE_MATERIAL_ANALYSIS where BATCH_NUM = '" + data + "' and L2_CODE in(select L2_CODE from M_MATERIAL_COOD where MAT_DESC='" + name + "')";
                int k = dBSQL.CommandExecuteNonQuery(delSql);
                if (k > 0)
                {
                    MessageBox.Show("删除成功");
                    DateTime d1 = Convert.ToDateTime(textBox_begin.Text);
                    DateTime d2 = Convert.ToDateTime(textBox_end.Text);
                    table1GetData(d1, d2);
                    table2GetData(d1, d2);
                }
                else
                {
                    MessageBox.Show("删除失败");
                }
            }
            else { return; }

        }

        //修改按钮
        private void simpleButton3_click(object sender, EventArgs e)
        {
            string num = d1.CurrentRow.Cells["BATCH_NUM"].Value.ToString();
            string name = d1.CurrentRow.Cells["MAT_DESC"].Value.ToString();
            Frm_TKFXN_update form_display = new Frm_TKFXN_update(num, name);
            if (Frm_TKFXN_update.isopen == false)
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
            table2GetData(start, end);
        }


        private void d1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;

            if (dgv != null)
            {
                var idx = dgv.CurrentRow.Index;

                if (rowMergeView1.Rows.Count > 0)
                {

                    for (int i = 0; i < rowMergeView1.Rows.Count; i++)
                    {
                        rowMergeView1.Rows[i].Selected = false;
                    }
                    rowMergeView1.Rows[idx].Selected = true;

                }

            }
        }

        private void rowMergeView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgv = sender as DataGridView;

            if (dgv != null)
            {
                var idx = dgv.CurrentRow.Index;

                if (d1.Rows.Count > 0)
                {

                    for (int i = 0; i < d1.Rows.Count; i++)
                    {
                        d1.Rows[i].Selected = false;
                    }
                    d1.Rows[idx].Selected = true;

                }
            }
        }
        public void Timer_state()
        {

        }
        public void _Clear()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {

        }
        //烧结其他原料基础性能按钮
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            Frm_shaojieqitayuanliaojcxn form_display = new Frm_shaojieqitayuanliaojcxn();
            if (Frm_shaojieqitayuanliaojcxn.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }
        //高炉入炉原料基础性能按钮
        private void simpleButton7_Click(object sender, EventArgs e)
        {
            Frm_gaoluruluyuanliao form_display = new Frm_gaoluruluyuanliao();
            if (Frm_gaoluruluyuanliao.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }
    }
}
