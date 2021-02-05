using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataBase;

namespace WindowsFormsApp2.page.analyze
{
    public partial class Frm_FKFXN_csxg2 : Form
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public static bool isopen = false;
        public Frm_FKFXN_csxg2()
        {
            InitializeComponent();
            GetData();
        }
        //保存按钮
        private void simpleButton2_click(object sender, EventArgs e)
        {
            string sql = "update MC_ORECAL_ORE_BP_PAR set TIMESTAMP = '"+ DateTime.Now + "'";
            for(int i = 1; i < d1.Rows.Count; i++)
            {
                string name = d1.Rows[i].Cells["name"].Value.ToString();
                string data = d1.Rows[i].Cells["data"].Value.ToString();
                sql = sql + ","+ name + "=" + data;
            }
            sql = sql + " where TIMESTAMP='" + d1.Rows[0].Cells["data"].Value.ToString() + "'";
            int k = dBSQL.CommandExecuteNonQuery(sql);
            if (k > 0)
            {
                MessageBox.Show("更新成功");
            }
            else
            {
                MessageBox.Show("更新失败");
            }
            GetData();
        }
        private void GetData()
        {
            while (d1.Rows.Count < 16)
            {
                d1.Rows.Add();
            }
            string sql = "select top(1) * from MC_ORECAL_ORE_BP_PAR order by TIMESTAMP";
            DataTable table = dBSQL.GetCommand(sql);
            for(int i = 0; i < d1.Rows.Count; i++)
            {
                d1.Rows[i].Cells["id"].Value = i + 1;
                d1.Rows[i].Cells["data"].Value = table.Rows[0][i].ToString();
                d1.Rows[i].Cells["name"].Value = table.Columns[i].ColumnName.ToString();
                d1.Rows[i].Cells["data"].Style.ForeColor = Color.Red;
            }
            d1.Rows[0].Cells["cname"].Value = "记录时间";
            d1.Rows[1].Cells["cname"].Value = "TFe品位等级下限";
            d1.Rows[2].Cells["cname"].Value = "SiO2品位等级下限";
            d1.Rows[3].Cells["cname"].Value = "Al2O3品位等级下限";
            d1.Rows[4].Cells["cname"].Value = "P品位等级下限";
            d1.Rows[5].Cells["cname"].Value = "S品位等级下限";
            d1.Rows[6].Cells["cname"].Value = "TFe品位等级上限";
            d1.Rows[7].Cells["cname"].Value = "SiO2品位等级上限";
            d1.Rows[8].Cells["cname"].Value = "Al2O3品位等级上限";
            d1.Rows[9].Cells["cname"].Value = "P品位等级上限";
            d1.Rows[10].Cells["cname"].Value = "S品位等级上限";
            d1.Rows[11].Cells["cname"].Value = "TFe/FeO等级下限";
            d1.Rows[12].Cells["cname"].Value = "TFe/FeO等级上限";
            d1.Rows[13].Cells["cname"].Value = "TFe/LOI等级下限";
            d1.Rows[14].Cells["cname"].Value = "铁矿粉-0.5mm含量等级下限";
            d1.Rows[15].Cells["cname"].Value = "铁矿粉-0.5mm含量等级上限";
            d1.Columns["data"].ReadOnly = false;
        }
    }
}
