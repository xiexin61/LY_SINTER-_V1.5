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
    public partial class Frm_TKFXN_CSXG : Form
    {
        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public static bool isopen = false;

        public Frm_TKFXN_CSXG()
        {
            InitializeComponent();
            d1GetData();
            d2GetData();
            d3GetData();
        }

        private void simpleButton3_click(object sender, EventArgs e)
        {
            Frm_FKFXN_csxg2 form_display = new Frm_FKFXN_csxg2();
            if (Frm_FKFXN_csxg2.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
            d1GetData();
            d2GetData();
            d3GetData();
        }

        /// <summary>
        /// 获取d1表格数据
        /// </summary>
        private void d1GetData()
        {
            string sql = "select top(1) PAR_TFE_RANK_LOWER,PAR_TFE_RANK_UPPER,PAR_SIO2_RANK_LOWER,PAR_SIO2_RANK_UPPER,PAR_AL2O3_RANK_LOWER,PAR_AL2O3_RANK_UPPER," +
                "PAR_S_RANK_LOWER,PAR_S_RANK_UPPER,PAR_P_RANK_LOWER,PAR_P_RANK_UPPER from MC_ORECAL_ORE_BP_PAR order by TIMESTAMP desc";
            DataTable table = dBSQL.GetCommand(sql);
            while (d1.Rows.Count < 6)
            {
                d1.Rows.Add();
            }
            d1.Rows[0].Cells["id"].Value = "TFe";
            d1.Rows[0].Cells["low"].Value = "< " + table.Rows[0][0].ToString();
            d1.Rows[0].Cells["middle"].Value = table.Rows[0][0].ToString() + "～" + table.Rows[0][1].ToString();
            d1.Rows[0].Cells["high"].Value = "> " + table.Rows[0][1].ToString();

            d1.Rows[1].Cells["id"].Value = "SiO2";
            d1.Rows[1].Cells["low"].Value = "< " + table.Rows[0][2];
            d1.Rows[1].Cells["middle"].Value = table.Rows[0][2] + "～" + table.Rows[0][3];
            d1.Rows[1].Cells["high"].Value = "> " + table.Rows[0][3];

            d1.Rows[2].Cells["id"].Value = "Al2O3";
            d1.Rows[2].Cells["low"].Value = "< " + table.Rows[0][4];
            d1.Rows[2].Cells["middle"].Value = table.Rows[0][4] + "～" + table.Rows[0][5];
            d1.Rows[2].Cells["high"].Value = "> " + table.Rows[0][5];

            d1.Rows[3].Cells["id"].Value = "S";
            d1.Rows[3].Cells["low"].Value = "< " + table.Rows[0][6];
            d1.Rows[3].Cells["high"].Value = "> " + table.Rows[0][7];

            d1.Rows[4].Cells["id"].Value = "P";
            d1.Rows[4].Cells["low"].Value = "< " + table.Rows[0][8];
            d1.Rows[4].Cells["high"].Value = "> " + table.Rows[0][9];
        }

        /// <summary>
        /// 获取d2表格数据
        /// </summary>
        private void d2GetData()
        {
            string sql = "select top(1) PAR_TFE_FEO_LOWER,PAR_TFE_FEO_UPPER,PAR_TFE_LOI_RANK from MC_ORECAL_ORE_BP_PAR order by TIMESTAMP desc";
            DataTable table = dBSQL.GetCommand(sql);
            while (d2.Rows.Count < 5)
            {
                d2.Rows.Add();
            }
            d2.Rows[0].Cells["type"].Value = "磁铁矿";
            d2.Rows[0].Cells["TFeFeO"].Value = "< " + table.Rows[0][0];
            d2.Rows[0].Cells["TFeLOI"].Value = "> " + table.Rows[0][2];
            d2.Rows[1].Cells["type"].Value = "混合矿(磁铁矿+赤铁矿)";
            d2.Rows[1].Cells["TFeFeO"].Value = table.Rows[0][0] + "～" + table.Rows[0][1];
            d2.Rows[1].Cells["TFeLOI"].Value = "> " + table.Rows[0][2];
            d2.Rows[2].Cells["type"].Value = "赤铁矿";
            d2.Rows[2].Cells["TFeFeO"].Value = "> " + table.Rows[0][1];
            d2.Rows[2].Cells["TFeLOI"].Value = "> " + table.Rows[0][2];
            d2.Rows[3].Cells["type"].Value = "褐铁矿";
            d2.Rows[3].Cells["TFeFeO"].Value = "> " + table.Rows[0][1];
            d2.Rows[3].Cells["TFeLOI"].Value = "< " + table.Rows[0][2];
        }

        /// <summary>
        /// 获取d3表格数据
        /// </summary>
        private void d3GetData()
        {
            d3.Rows.Add();
            string sql = "select top(1) PAR_0_5_MM_LOWER,PAR_0_5_MM_UPPER from MC_ORECAL_ORE_BP_PAR order by TIMESTAMP desc";
            DataTable table = dBSQL.GetCommand(sql);
            d3.Rows[0].Cells["Clomum1"].Value = "比例";
            d3.Rows[0].Cells["Column2"].Value = "> " + table.Rows[0][1].ToString();
            d3.Rows[0].Cells["Column3"].Value = table.Rows[0][0].ToString() + "～" + table.Rows[0][1].ToString();
            d3.Rows[0].Cells["Column4"].Value = "< " + table.Rows[0][0].ToString();
        }
    }
}