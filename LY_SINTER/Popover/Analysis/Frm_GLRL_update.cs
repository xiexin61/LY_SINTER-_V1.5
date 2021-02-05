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

namespace LY_SINTER.Popover.Analysis
{
    public partial class Frm_GLRL_update : Form
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        string BATCH_NUM = "", MAT_DESC = "";
        public static bool isopen = false;
        public Frm_GLRL_update(string num, string name)
        {
            InitializeComponent();
            BATCH_NUM = num;
            MAT_DESC = name;
            getData(num, name);
        }
        public void getData(string num, string name)
        {
            while (d1.Rows.Count < 34)
            {
                d1.Rows.Add();
            }
            string sql = "select BATCH_NUM,b.MAT_DESC,a.PLACE_ORIGIN,a.UNIT_PRICE,C_TFE,C_FEO,C_SIO2,C_CAO,C_MGO,C_AL2O3,C_S,C_P,C_C,C_LOT,C_MN,C_R,C_H2O,C_ASH," +
                    "C_VOLATILES,C_TIO2,C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN,C_K,C_NA,C_CR,C_NI,C_MNO " +
                "from M_BF_MATERIAL_ANALYSIS a, M_MATERIAL_COOD b where a.L2_CODE = b.L2_CODE and b.MAT_DESC = '" + name + "' and BATCH_NUM = " + num + "; ";
            DataTable table = dBSQL.GetCommand(sql);
            for (int i = 0; i < d1.Rows.Count; i++)
            {
                d1.Rows[i].Cells["id"].Value = i + 1;
                d1.Rows[i].Cells["before"].Value = table.Rows[0][i].ToString();
                d1.Rows[i].Cells["after"].Value = table.Rows[0][i].ToString();
            }
            d1.Rows[0].Cells["function"].Value = "批号";
            d1.Rows[1].Cells["function"].Value = "物料名";
            d1.Rows[2].Cells["function"].Value = "产地";
            d1.Rows[3].Cells["function"].Value = "单价（元/吨）";
            d1.Rows[4].Cells["function"].Value = "TFe";
            d1.Rows[5].Cells["function"].Value = "FeO";
            d1.Rows[6].Cells["function"].Value = "CaO";
            d1.Rows[7].Cells["function"].Value = "SiO2";
            d1.Rows[8].Cells["function"].Value = "Al2O3";
            d1.Rows[9].Cells["function"].Value = "MgO";
            d1.Rows[10].Cells["function"].Value = "S";
            d1.Rows[11].Cells["function"].Value = "P";
            d1.Rows[12].Cells["function"].Value = "C";
            d1.Rows[13].Cells["function"].Value = "MN";
            d1.Rows[14].Cells["function"].Value = "LOI";
            d1.Rows[15].Cells["function"].Value = "R";
            d1.Rows[16].Cells["function"].Value = "H2O";
            d1.Rows[17].Cells["function"].Value = "灰分";
            d1.Rows[18].Cells["function"].Value = "挥发分";
            d1.Rows[19].Cells["function"].Value = "TiO2";
            d1.Rows[20].Cells["function"].Value = "K2O";
            d1.Rows[21].Cells["function"].Value = "Na2O";
            d1.Rows[22].Cells["function"].Value = "PBO";
            d1.Rows[23].Cells["function"].Value = "ZNO";
            d1.Rows[24].Cells["function"].Value = "F";
            d1.Rows[25].Cells["function"].Value = "As";
            d1.Rows[26].Cells["function"].Value = "Cu";
            d1.Rows[27].Cells["function"].Value = "Pb";
            d1.Rows[28].Cells["function"].Value = "Zn";
            d1.Rows[29].Cells["function"].Value = "K";
            d1.Rows[30].Cells["function"].Value = "Na";
            d1.Rows[31].Cells["function"].Value = "Cr";
            d1.Rows[32].Cells["function"].Value = "Ni";
            d1.Rows[33].Cells["function"].Value = "MNO";
            d1.Rows[0].Cells["after"].ReadOnly = true;
            d1.Rows[1].Cells["after"].ReadOnly = true;
            d1.Rows[2].Cells["after"].ReadOnly = true;
            for (int i = 3; i < d1.Rows.Count; i++)
            {
                d1.Rows[i].Cells["after"].Style.ForeColor = Color.Red;
            }
        }
        //保存按钮，更新数据库
        private void simpleButton2_click(object sender, EventArgs e)
        {
            string sql = "update M_BF_MATERIAL_ANALYSIS set ";

            if (d1.Rows[4].Cells["after"].Value.ToString() != "")
            {
                sql += "C_TFE=" + d1.Rows[4].Cells["after"].Value + ",";
            }
            if (d1.Rows[5].Cells["after"].Value.ToString() != "")
            {
                sql += "C_FEO=" + d1.Rows[5].Cells["after"].Value + ",";
            }
            if (d1.Rows[6].Cells["after"].Value.ToString() != "")
            {
                sql += "C_CAO=" + d1.Rows[6].Cells["after"].Value + ",";
            }
            if (d1.Rows[7].Cells["after"].Value.ToString() != "")
            {
                sql += "C_SIO2=" + d1.Rows[7].Cells["after"].Value + ",";
            }
            if (d1.Rows[8].Cells["after"].Value.ToString() != "")
            {
                sql += "C_AL2O3=" + d1.Rows[8].Cells["after"].Value + ",";
            }
            if (d1.Rows[9].Cells["after"].Value.ToString() != "")
            {
                sql += "C_MGO=" + d1.Rows[9].Cells["after"].Value + ",";
            }
            if (d1.Rows[10].Cells["after"].Value.ToString() != "")
            {
                sql += "C_S=" + d1.Rows[10].Cells["after"].Value + ",";
            }
            if (d1.Rows[11].Cells["after"].Value.ToString() != "")
            {
                sql += "C_P=" + d1.Rows[11].Cells["after"].Value + ",";
            }
            if (d1.Rows[12].Cells["after"].Value.ToString() != "")
            {
                sql += "C_C=" + d1.Rows[11].Cells["after"].Value + ",";
            }
            if (d1.Rows[13].Cells["after"].Value.ToString() != "")
            {
                sql += "C_MN=" + d1.Rows[11].Cells["after"].Value + ",";
            }
            if (d1.Rows[14].Cells["after"].Value.ToString() != "")
            {
                sql += "C_LOT=" + d1.Rows[12].Cells["after"].Value + ",";
            }
            if (d1.Rows[15].Cells["after"].Value.ToString() != "")
            {
                sql += "C_R=" + d1.Rows[11].Cells["after"].Value + ",";
            }
            if (d1.Rows[16].Cells["after"].Value.ToString() != "")
            {
                sql += "C_H2O=" + d1.Rows[13].Cells["after"].Value + ",";
            }
            if (d1.Rows[17].Cells["after"].Value.ToString() != "")
            {
                sql += "C_ASH=" + d1.Rows[13].Cells["after"].Value + ",";
            }
            if (d1.Rows[18].Cells["after"].Value.ToString() != "")
            {
                sql += "C_VOLATILES=" + d1.Rows[13].Cells["after"].Value + ",";
            }
            if (d1.Rows[19].Cells["after"].Value.ToString() != "")
            {
                sql += "C_TIO2=" + d1.Rows[20].Cells["after"].Value + ",";
            }
            if (d1.Rows[20].Cells["after"].Value.ToString() != "")
            {
                sql += "C_K2O=" + d1.Rows[18].Cells["after"].Value + ",";
            }
            if (d1.Rows[21].Cells["after"].Value.ToString() != "")
            {
                sql += "C_NA2O=" + d1.Rows[19].Cells["after"].Value + ",";
            }
            if (d1.Rows[22].Cells["after"].Value.ToString() != "")
            {
                sql += "C_PBO=" + d1.Rows[18].Cells["after"].Value + ",";
            }
            if (d1.Rows[23].Cells["after"].Value.ToString() != "")
            {
                sql += "C_ZNO=" + d1.Rows[19].Cells["after"].Value + ",";
            }
            if (d1.Rows[24].Cells["after"].Value.ToString() != "")
            {
                sql += "C_F=" + d1.Rows[19].Cells["after"].Value + ",";
            }
            if (d1.Rows[25].Cells["after"].Value.ToString() != "")
            {
                sql += "C_AS=" + d1.Rows[14].Cells["after"].Value + ",";
            }
            if (d1.Rows[26].Cells["after"].Value.ToString() != "")
            {
                sql += "C_CU=" + d1.Rows[19].Cells["after"].Value + ",";
            }
            if (d1.Rows[27].Cells["after"].Value.ToString() != "")
            {
                sql += "C_PB=" + d1.Rows[15].Cells["after"].Value + ",";
            }
            if (d1.Rows[28].Cells["after"].Value.ToString() != "")
            {
                sql += "C_ZN=" + d1.Rows[16].Cells["after"].Value + ",";
            }
            if (d1.Rows[29].Cells["after"].Value.ToString() != "")
            {
                sql += "C_K=" + d1.Rows[17].Cells["after"].Value + ",";
            }
            if (d1.Rows[30].Cells["after"].Value.ToString() != "")
            {
                sql += "C_NA=" + d1.Rows[15].Cells["after"].Value + ",";
            }
            if (d1.Rows[31].Cells["after"].Value.ToString() != "")
            {
                sql += "C_CR=" + d1.Rows[16].Cells["after"].Value + ",";
            }
            if (d1.Rows[32].Cells["after"].Value.ToString() != "")
            {
                sql += "C_NI=" + d1.Rows[17].Cells["after"].Value + ",";
            }
            if (d1.Rows[33].Cells["after"].Value.ToString() != "")
            {
                sql += "MNO=" + d1.Rows[17].Cells["after"].Value + ",";
            }
            

            sql += "FLAG=2 where BATCH_NUM = '" + d1.Rows[0].Cells["after"].Value + "'and L2_CODE in(select L2_CODE from M_MATERIAL_COOD where MAT_DESC='" + d1.Rows[1].Cells["after"].Value + "')";
            int k = dBSQL.CommandExecuteNonQuery(sql);
            if (k > 0)
            {
                MessageBox.Show("更新成功");
            }
            else
            {
                MessageBox.Show("更新失败");
            }
        }
    }
}
