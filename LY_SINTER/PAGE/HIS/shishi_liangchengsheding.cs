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

namespace LY_SINTER.PAGE.HIS
{
    public partial class shishi_liangchengsheding : Form
    {
        public static bool isopen = false;
        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        public shishi_liangchengsheding()
        {
            InitializeComponent();
            dataGridView1_CellContentClick();//dataGridView不显示行标题列
            shuju();
            /*dataGridView1.Rows[1].ReadOnly = true;
            dataGridView1.Rows[3].ReadOnly = true;
            dataGridView1.Rows[5].ReadOnly = true;
            dataGridView1.Rows[7].ReadOnly = true;*/
        }

        //数据
        private void shuju()
        {
            try
            {
                dataGridView1.RowCount = 18;
                /*dataGridView1.Rows[0].Cells[0].Value = "1#主抽频率";
                dataGridView1.Rows[1].Cells[0].Value = "2#主抽频率";*/
                dataGridView1.Rows[0].Cells[0].Value = "1#主抽温度";
                dataGridView1.Rows[1].Cells[0].Value = "2#主抽温度";
                dataGridView1.Rows[2].Cells[0].Value = "1#主抽负压";
                dataGridView1.Rows[3].Cells[0].Value = "2#主抽负压";
                dataGridView1.Rows[4].Cells[0].Value = "1#主抽风量";
                dataGridView1.Rows[5].Cells[0].Value = "2#主抽风量";
                dataGridView1.Rows[6].Cells[0].Value = "终点位置";
                dataGridView1.Rows[7].Cells[0].Value = "布料厚度";
                dataGridView1.Rows[8].Cells[0].Value = "点火温度";
                dataGridView1.Rows[9].Cells[0].Value = "总料量";
                dataGridView1.Rows[10].Cells[0].Value = "一混加水量";
                dataGridView1.Rows[11].Cells[0].Value = "二混加水量";
                dataGridView1.Rows[12].Cells[0].Value = "混合料仓位";
                dataGridView1.Rows[13].Cells[0].Value = "圆辊转速";
                dataGridView1.Rows[14].Cells[0].Value = "烧结机机速";
                dataGridView1.Rows[15].Cells[0].Value = "环冷机机速";

                try
                {
                    //上线和下限
                    //ISNULL(PAR_MA_FAN_SP_MAX, 0),ISNULL(PAR_MA_FAN_SP_MIN, 0),
                    string sql4 = "select ISNULL(PAR_MA_SB_FLUE_TE_MAX,0),ISNULL(PAR_MA_SB_FLUE_TE_MIN,0),ISNULL(PAR_MA_SB_FLUE_TE_MAX_2,0),ISNULL(PAR_MA_SB_FLUE_TE_MIN_2,0),ISNULL(PAR_MA_SB_FLUE_PT_MAX,0),ISNULL(PAR_MA_SB_FLUE_PT_MIN,0)," +
                                  "ISNULL(PAR_MA_SB_FLUE_PT_MAX_2,0),ISNULL(PAR_MA_SB_FLUE_PT_MIN_2,0),ISNULL(PAR_MA_SB_FLUE_FT_MAX,0),ISNULL(PAR_MA_SB_FLUE_FT_MIN,0),ISNULL(PAR_MA_SB_FLUE_FT_MAX_2,0),ISNULL(PAR_MA_SB_FLUE_FT_MIN_2,0),ISNULL(PAR_X_BTP_MAX,0),ISNULL(PAR_X_BTP_MIN,0),ISNULL(PAR_THICK_PV_MAX,0),ISNULL(PAR_THICK_PV_MIN,0),ISNULL(PAR_IG_TE_MAX,0)," +
                                  "ISNULL(PAR_IG_TE_MIN,0),ISNULL(PAR_TOTAL_SP_W_MAX,0),ISNULL(PAR_TOTAL_SP_W_MIN,0),ISNULL(PAR_1M_FT_SP_MAX,0),ISNULL(PAR_1M_FT_SP_MIN,0),ISNULL(PAR_2M_FLOW_SP_MAX,0),ISNULL(PAR_2M_FLOW_SP_MIN,0)," +
                                  "ISNULL(PAR_BLEND_LEVEL_MAX,0),ISNULL(PAR_BLEND_LEVEL_MIN,0),ISNULL(PAR_STICK_SP_MAX,0),ISNULL(PAR_STICK_SP_MIN,0),ISNULL(PAR_SIN_MS_SP_MAX,0),ISNULL(PAR_SIN_MS_SP_MIN,0),ISNULL(PAR_RC_SPEED_SP_MAX,0)," +
                                  "ISNULL(PAR_RC_SPEED_SP_MIN,0) from CFG_R_T_CURVE_INTERFACE_PAR";
                    DataTable dataTable4 = dBSQL.GetCommand(sql4);
                    if (dataTable4.Rows.Count > 0)
                    {
                        /*float zcplsx = float.Parse(dataTable4.Rows[0][0].ToString());
                        float zcplxx = float.Parse(dataTable4.Rows[0][1].ToString());*/
                        float zcwdsx = float.Parse(dataTable4.Rows[0][0].ToString());
                        float zcwdxx = float.Parse(dataTable4.Rows[0][1].ToString());
                        float zcwdsx1 = float.Parse(dataTable4.Rows[0][2].ToString());
                        float zcwdxx1 = float.Parse(dataTable4.Rows[0][3].ToString());
                        float zcfysx = float.Parse(dataTable4.Rows[0][4].ToString());
                        float zcfyxx = float.Parse(dataTable4.Rows[0][5].ToString());
                        float zcfysx1 = float.Parse(dataTable4.Rows[0][6].ToString());
                        float zcfyxx1 = float.Parse(dataTable4.Rows[0][7].ToString());
                        float zcflsx = float.Parse(dataTable4.Rows[0][8].ToString());
                        float zcflxx = float.Parse(dataTable4.Rows[0][9].ToString());
                        float zcflsx1 = float.Parse(dataTable4.Rows[0][10].ToString());
                        float zcflxx1 = float.Parse(dataTable4.Rows[0][11].ToString());
                        float zdwzsx = float.Parse(dataTable4.Rows[0][12].ToString());
                        float zdwzxx = float.Parse(dataTable4.Rows[0][13].ToString());
                        float blhdsx = float.Parse(dataTable4.Rows[0][14].ToString());
                        float blhdxx = float.Parse(dataTable4.Rows[0][15].ToString());
                        float dhwdsx = float.Parse(dataTable4.Rows[0][16].ToString());
                        float dhwdxx = float.Parse(dataTable4.Rows[0][17].ToString());
                        float zllsx = float.Parse(dataTable4.Rows[0][18].ToString());
                        float zllxx = float.Parse(dataTable4.Rows[0][19].ToString());
                        float yhjslsx = float.Parse(dataTable4.Rows[0][20].ToString());
                        float yhjslxx = float.Parse(dataTable4.Rows[0][21].ToString());
                        float ehjslsx = float.Parse(dataTable4.Rows[0][22].ToString());
                        float ehjslxx = float.Parse(dataTable4.Rows[0][23].ToString());
                        float hhlcwsx = float.Parse(dataTable4.Rows[0][24].ToString());
                        float hhlcwxx = float.Parse(dataTable4.Rows[0][25].ToString());
                        float ygzssx = float.Parse(dataTable4.Rows[0][26].ToString());
                        float ygzsxx = float.Parse(dataTable4.Rows[0][27].ToString());
                        float sjjjssx = float.Parse(dataTable4.Rows[0][28].ToString());
                        float sjjjsxx = float.Parse(dataTable4.Rows[0][29].ToString());
                        float hljjssx = float.Parse(dataTable4.Rows[0][30].ToString());
                        float hljjsxx = float.Parse(dataTable4.Rows[0][31].ToString());

                        /*//1#主抽频率
                        dataGridView1.Rows[0].Cells[1].Value = zcplxx.ToString();
                        dataGridView1.Rows[0].Cells[2].Value = zcplsx.ToString();
                        //2#主抽频率
                        dataGridView1.Rows[1].Cells[1].Value = zcplxx.ToString();
                        dataGridView1.Rows[1].Cells[2].Value = zcplsx.ToString();*/
                        //1#主抽温度
                        dataGridView1.Rows[0].Cells[1].Value = zcwdxx.ToString();
                        dataGridView1.Rows[0].Cells[2].Value = zcwdsx.ToString();
                        //2#主抽温度
                        dataGridView1.Rows[1].Cells[1].Value = zcwdxx1.ToString();
                        dataGridView1.Rows[1].Cells[2].Value = zcwdsx1.ToString();
                        //1#主抽负压
                        dataGridView1.Rows[2].Cells[1].Value = zcfyxx.ToString();
                        dataGridView1.Rows[2].Cells[2].Value = zcfysx.ToString();
                        //2#主抽负压
                        dataGridView1.Rows[3].Cells[1].Value = zcfyxx1.ToString();
                        dataGridView1.Rows[3].Cells[2].Value = zcfysx1.ToString();
                        //1#主抽风量
                        dataGridView1.Rows[4].Cells[1].Value = zcflxx.ToString();
                        dataGridView1.Rows[4].Cells[2].Value = zcflsx.ToString();
                        //2#主抽风量
                        dataGridView1.Rows[5].Cells[1].Value = zcflxx1.ToString();
                        dataGridView1.Rows[5].Cells[2].Value = zcflsx1.ToString();
                        //终点位置
                        dataGridView1.Rows[6].Cells[1].Value = zdwzxx.ToString();
                        dataGridView1.Rows[6].Cells[2].Value = zdwzsx.ToString();
                        //布料厚度
                        dataGridView1.Rows[7].Cells[1].Value = blhdxx.ToString();
                        dataGridView1.Rows[7].Cells[2].Value = blhdsx.ToString();
                        //点火温度
                        dataGridView1.Rows[8].Cells[1].Value = dhwdxx.ToString();
                        dataGridView1.Rows[8].Cells[2].Value = dhwdsx.ToString();
                        //总料量
                        dataGridView1.Rows[9].Cells[1].Value = zllxx.ToString();
                        dataGridView1.Rows[9].Cells[2].Value = zllsx.ToString();
                        //一混加水量
                        dataGridView1.Rows[10].Cells[1].Value = yhjslxx.ToString();
                        dataGridView1.Rows[10].Cells[2].Value = yhjslsx.ToString();
                        //二混加水量
                        dataGridView1.Rows[11].Cells[1].Value = ehjslxx.ToString();
                        dataGridView1.Rows[11].Cells[2].Value = ehjslsx.ToString();
                        //混合料仓位
                        dataGridView1.Rows[12].Cells[1].Value = hhlcwxx.ToString();
                        dataGridView1.Rows[12].Cells[2].Value = hhlcwsx.ToString();
                        //圆辊转速
                        dataGridView1.Rows[13].Cells[1].Value = ygzsxx.ToString();
                        dataGridView1.Rows[13].Cells[2].Value = ygzssx.ToString();
                        //烧结机机速
                        dataGridView1.Rows[14].Cells[1].Value = sjjjsxx.ToString();
                        dataGridView1.Rows[14].Cells[2].Value = sjjjssx.ToString();
                        //环冷机机速
                        dataGridView1.Rows[15].Cells[1].Value = hljjsxx.ToString();
                        dataGridView1.Rows[15].Cells[2].Value = hljjssx.ToString();
                    }
                    else
                    { }
                }
                catch
                {
                }
            }
            catch
            { }
        }

        //dataGridView不显示行标题列
        private void dataGridView1_CellContentClick()
        {
            dataGridView1.RowHeadersVisible = false;
        }

        //确定按钮
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string sql1 = "update CFG_R_T_CURVE_INTERFACE_PAR " +
                    "set TIMESTAMP=GETDATE()," +
                    /*"PAR_MA_FAN_SP_MAX='" + dataGridView1.Rows[0].Cells[2].Value + "'," +
                    "PAR_MA_FAN_SP_MIN='" + dataGridView1.Rows[0].Cells[1].Value + "'," +*/
                    "PAR_MA_SB_FLUE_TE_MAX='" + dataGridView1.Rows[0].Cells[2].Value + "'," +
                    "PAR_MA_SB_FLUE_TE_MIN='" + dataGridView1.Rows[0].Cells[1].Value + "'," +
                    "PAR_MA_SB_FLUE_TE_MAX_2='" + dataGridView1.Rows[1].Cells[2].Value + "'," +
                    "PAR_MA_SB_FLUE_TE_MIN_2='" + dataGridView1.Rows[1].Cells[1].Value + "'," +
                    "PAR_MA_SB_FLUE_PT_MAX='" + dataGridView1.Rows[2].Cells[2].Value + "'," +
                    "PAR_MA_SB_FLUE_PT_MIN='" + dataGridView1.Rows[2].Cells[1].Value + "'," +
                    "PAR_MA_SB_FLUE_PT_MAX_2='" + dataGridView1.Rows[3].Cells[2].Value + "'," +
                    "PAR_MA_SB_FLUE_PT_MIN_2='" + dataGridView1.Rows[3].Cells[1].Value + "'," +
                    "PAR_MA_SB_FLUE_FT_MAX='" + dataGridView1.Rows[4].Cells[2].Value + "'," +
                    "PAR_MA_SB_FLUE_FT_MIN='" + dataGridView1.Rows[4].Cells[1].Value + "'," +
                    "PAR_MA_SB_FLUE_FT_MAX_2='" + dataGridView1.Rows[5].Cells[2].Value + "'," +
                    "PAR_MA_SB_FLUE_FT_MIN_2='" + dataGridView1.Rows[5].Cells[1].Value + "'," +
                    "PAR_X_BTP_MAX='" + dataGridView1.Rows[6].Cells[2].Value + "'," +
                    "PAR_X_BTP_MIN='" + dataGridView1.Rows[6].Cells[1].Value + "'," +
                    "PAR_THICK_PV_MAX='" + dataGridView1.Rows[7].Cells[2].Value + "'," +
                    "PAR_THICK_PV_MIN='" + dataGridView1.Rows[7].Cells[1].Value + "'," +
                    "PAR_IG_TE_MAX='" + dataGridView1.Rows[8].Cells[2].Value + "'," +
                    "PAR_IG_TE_MIN='" + dataGridView1.Rows[8].Cells[1].Value + "'," +
                    "PAR_TOTAL_SP_W_MAX='" + dataGridView1.Rows[9].Cells[2].Value + "'," +
                    "PAR_TOTAL_SP_W_MIN='" + dataGridView1.Rows[9].Cells[1].Value + "'," +
                    "PAR_1M_FT_SP_MAX='" + dataGridView1.Rows[10].Cells[2].Value + "'," +
                    "PAR_1M_FT_SP_MIN='" + dataGridView1.Rows[10].Cells[1].Value + "'," +
                    "PAR_2M_FLOW_SP_MAX='" + dataGridView1.Rows[11].Cells[2].Value + "'," +
                    "PAR_2M_FLOW_SP_MIN='" + dataGridView1.Rows[11].Cells[1].Value + "'," +
                    "PAR_BLEND_LEVEL_MAX='" + dataGridView1.Rows[12].Cells[2].Value + "'," +
                    "PAR_BLEND_LEVEL_MIN='" + dataGridView1.Rows[12].Cells[1].Value + "'," +
                    "PAR_STICK_SP_MAX='" + dataGridView1.Rows[13].Cells[2].Value + "'," +
                    "PAR_STICK_SP_MIN='" + dataGridView1.Rows[13].Cells[1].Value + "'," +
                    "PAR_SIN_MS_SP_MAX='" + dataGridView1.Rows[14].Cells[2].Value + "'," +
                    "PAR_SIN_MS_SP_MIN='" + dataGridView1.Rows[14].Cells[1].Value + "'," +
                    "PAR_RC_SPEED_SP_MAX='" + dataGridView1.Rows[15].Cells[2].Value + "'," +
                    "PAR_RC_SPEED_SP_MIN='" + dataGridView1.Rows[15].Cells[1].Value + "'";
                dBSQL.CommandExecuteNonQuery(sql1);
                MessageBox.Show("修改完成！");
            }
            catch
            {
                MessageBox.Show("请填写正确的数据！");
            }
        }

        //取消按钮
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            shuju();
        }
    }
}