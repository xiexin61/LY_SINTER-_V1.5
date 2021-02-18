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
using WindowsFormsApp2.page.analyze;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using UserControlIndex;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using NBSJ_MAIN_UC.Model;
using SqlSugar;
using NBSJ_MAIN_UC;
using System.Timers;
using LY_SINTER.Custom;
using LY_SINTER.Popover.Analysis;

namespace LY_SINTER.PAGE.Analysis
{
    public partial class shengchanzuzhi : UserControl
    {
        private System.Timers.Timer MongoQMtimer1;//自定义一个定时器
        SqlSugarClient db_sugar = GetInstance();
        C_PLC_3S modelT_PLC_3S = null;
        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = DataBase.ConstParameters.strCon, DbType = SqlSugar.DbType.SqlServer, IsAutoCloseConnection = true });
            return db;
        }
        MC_MICAL_PAR modelMC_MICAL_PAR = new MC_MICAL_PAR();
        public System.Web.UI.Timer Timer_1 { get; set; }
        public System.Web.UI.Timer Timer_2 { get; set; }
        public shengchanzuzhi()
        {
            InitializeComponent();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            GetNewTime();
            llpizhong();
            pizhong();
            dateTimePicker_value();
            //默认显示一个月数据
            DateTime d1 = DateTime.Now.AddMonths(-1);
            DateTime d2 = DateTime.Now;
            quxian(d1, d2);
            Timer_1 = new System.Web.UI.Timer();
            Timer_1.Tick += Timer_1_Tick;
            Timer_1.Interval = 60000;
            Timer_1.Enabled = true;
            this.rowMergeView1.AddSpanHeader(1, 7, "白班(t)");
            this.rowMergeView1.AddSpanHeader(8, 7, "夜班(t)");
            Timer_2 = new System.Web.UI.Timer();
            Timer_2.Tick += Timer_2_Tick;
            Timer_2.Interval = 300000;
            Timer_2.Enabled = true;
            UC_Load();
            sszzjh();
            Check_text();
            //MongoQMtimer1_Elapsed();
            //windowformRefresh();
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
        public void UC_Load()
        {
            InitControl();
            //windowformRefresh();
            string strSQL = "select top(1) * from C_PLC_3S order by timestamp desc";
            string Temp = "";
            try
            {

                modelT_PLC_3S = db_sugar.SqlQueryable<C_PLC_3S>(strSQL).ToList().FirstOrDefault();

            }
            catch (Exception ee)
            {
                LogHelper.LogError(ee.Message);
            }
            if (modelT_PLC_3S != null)
            {
                modelMC_MICAL_PAR = db_sugar.Queryable<MC_MICAL_PAR>().ToList().FirstOrDefault();
                windowformRefresh();
            }
            MongoQMtimer1 = new System.Timers.Timer();
            MongoQMtimer1.Elapsed += new ElapsedEventHandler(MongoQMtimer1_Elapsed);//1s调用一次
            MongoQMtimer1.Interval = 1 * 5000;
            MongoQMtimer1.AutoReset = true;//获取该定时器自动执行
            MongoQMtimer1.Enabled = true;
            MongoQMtimer1.Start();

        }

        private void InitControl()
        {
            this.bottleAllUC1.Dock = System.Windows.Forms.DockStyle.Fill;
            //料仓
            for (int i = 0; i < 10; i++)
            {
                bottleAllUC1.BottomItems.Add(new BottleItem { BottleType = BottleType.BottleSingle });
                bottleAllUC1.BottomItems[i].BottleObj.Value = (i + 1) * 10;
                bottleAllUC1.BottomItems[i].BottleObj.CangHao = i + 1;

                bottleAllUC1.BottomItems[i].BottleObj.Lc1BackColorTop = Color.FromArgb(0xb3, 0xaf, 0xaf);
                bottleAllUC1.BottomItems[i].BottleObj.Lc2BackColors = new Color[] { Color.FromArgb(0x8c, 0x8c, 0x77), Color.FromArgb(0x8c, 0x8c, 0x77), Color.FromArgb(0x8c, 0x8c, 0x77) };//无料时的背景填充色

                if (i < 8)
                {
                    bottleAllUC1.BottomItems[i].BottleObj.Lc3BackColor = Color.FromArgb(0xab, 0x46, 0x38);

                    bottleAllUC1.BottomItems[i].BottleObj.Lc4BackColorTop = Color.FromArgb(0xa8, 0x74, 0x56);
                    bottleAllUC1.BottomItems[i].BottleObj.Lc5BackColors = new Color[] { Color.FromArgb(0xab, 0x46, 0x38), Color.FromArgb(0xab, 0x46, 0x38), Color.FromArgb(0xab, 0x46, 0x38) };//中部的填充颜色737370

                }
                else
                {
                    bottleAllUC1.BottomItems[i].BottleObj.Lc3BackColor = Color.FromArgb(0x1d, 0x20, 0x25);
                    bottleAllUC1.BottomItems[i].BottleObj.Lc4BackColorTop = Color.FromArgb(0x3b, 0x3e, 0x47);
                    bottleAllUC1.BottomItems[i].BottleObj.Lc5BackColors = new Color[] { Color.FromArgb(0x1d, 0x20, 0x25), Color.FromArgb(0x1d, 0x20, 0x25), Color.FromArgb(0x1d, 0x20, 0x25) };//中部的填充颜色737370

                }

            }
            for (int i = 10; i < 12; i++)
            {
                bottleAllUC1.BottomItems.Add(new BottleItem { BottleType = BottleType.BottleSingle });
                bottleAllUC1.BottomItems[i].BottleObj.Value = (i + 1) * 10;
                bottleAllUC1.BottomItems[i].BottleObj.CangHao = i + 1;

                bottleAllUC1.BottomItems[i].BottleObj.Lc1BackColorTop = Color.FromArgb(0xb3, 0xaf, 0xaf);
                bottleAllUC1.BottomItems[i].BottleObj.Lc2BackColors = new Color[] { Color.FromArgb(0x8c, 0x8c, 0x77), Color.FromArgb(0x8c, 0x8c, 0x77), Color.FromArgb(0x8c, 0x8c, 0x77) };//无料时的背景填充色
                bottleAllUC1.BottomItems[i].BottleObj.Lc3BackColor = Color.FromArgb(0x10, 0x4e, 0x8b);
                bottleAllUC1.BottomItems[i].BottleObj.Lc4BackColorTop = Color.FromArgb(0x36, 0x64, 0x8b);
                bottleAllUC1.BottomItems[i].BottleObj.Lc5BackColors = new Color[] { Color.FromArgb(0x10, 0x4e, 0x8b), Color.FromArgb(0x10, 0x4e, 0x8b), Color.FromArgb(0x10, 0x4e, 0x8b) };



            }
            for (int i = 12; i < 20; i++)
            {
                bottleAllUC1.BottomItems.Add(new BottleItem { BottleType = BottleType.BottleSingle });
                bottleAllUC1.BottomItems[i].BottleObj.Value = (i + 1) * 10;
                bottleAllUC1.BottomItems[i].BottleObj.CangHao = i + 1;


                bottleAllUC1.BottomItems[i].BottleObj.Lc1BackColorTop = Color.FromArgb(0xb3, 0xaf, 0xaf);
                bottleAllUC1.BottomItems[i].BottleObj.Lc2BackColors = new Color[] { Color.FromArgb(0x8c, 0x8c, 0x77), Color.FromArgb(0x8c, 0x8c, 0x77), Color.FromArgb(0x8c, 0x8c, 0x77) };//无料时的背景填充色
                bottleAllUC1.BottomItems[i].BottleObj.Lc3BackColor = Color.FromArgb(0x73, 0x73, 0x70);
                if ((i + 1) == 13 || (i + 1) == 14)
                {
                    bottleAllUC1.BottomItems[i].BottleObj.Lc3BackColor = Color.FromArgb(0x98, 0xfb, 0x98);
                    bottleAllUC1.BottomItems[i].BottleObj.Lc4BackColorTop = Color.FromArgb(0x7f, 0xff, 0xaa);
                    bottleAllUC1.BottomItems[i].BottleObj.Lc5BackColors = new Color[] { Color.FromArgb(0x98, 0xfb, 0x98), Color.FromArgb(0x98, 0xfb, 0x98), Color.FromArgb(0x98, 0xfb, 0x98) };//中部的填充颜色737370
                }
                else if ((i + 1) == 15 || (i + 1) == 16)
                {
                    bottleAllUC1.BottomItems[i].BottleObj.Lc3BackColor = Color.FromArgb(0x8b, 0xac, 0xa1);
                    bottleAllUC1.BottomItems[i].BottleObj.Lc4BackColorTop = Color.FromArgb(0xad, 0xb9, 0xbe);
                    bottleAllUC1.BottomItems[i].BottleObj.Lc5BackColors = new Color[] { Color.FromArgb(0x8b, 0xac, 0xa1), Color.FromArgb(0x8b, 0xac, 0xa1), Color.FromArgb(0x8b, 0xac, 0xa1) };//中部的填充颜色737370

                }
                else if ((i + 1) == 17 || (i + 1) == 18 || (i + 1) == 19)
                {
                    bottleAllUC1.BottomItems[i].BottleObj.Lc3BackColor = Color.FromArgb(0x10, 0x4e, 0x8b);
                    bottleAllUC1.BottomItems[i].BottleObj.Lc4BackColorTop = Color.FromArgb(0x36, 0x64, 0x8b);
                    bottleAllUC1.BottomItems[i].BottleObj.Lc5BackColors = new Color[] { Color.FromArgb(0x10, 0x4e, 0x8b), Color.FromArgb(0x10, 0x4e, 0x8b), Color.FromArgb(0x10, 0x4e, 0x8b) };//中部的填充颜色737370

                }
                else
                {
                    bottleAllUC1.BottomItems[i].BottleObj.Lc3BackColor = Color.FromArgb(0x10, 0x4e, 0x8b);
                    bottleAllUC1.BottomItems[i].BottleObj.Lc4BackColorTop = Color.FromArgb(0x36, 0x64, 0x8b);
                    bottleAllUC1.BottomItems[i].BottleObj.Lc5BackColors = new Color[] { Color.FromArgb(0x10, 0x4e, 0x8b), Color.FromArgb(0x10, 0x4e, 0x8b), Color.FromArgb(0x10, 0x4e, 0x8b) };//中部的填充颜色737370


                }
            }
            InitBottleName();
        }
        private void InitBottleName()
        {
            if (this.bottleAllUC1.BottomItems.Count == 20)
            {
                //20个仓
                this.bottleAllUC1.BottomItems[0].BottleDesc = "混匀矿";
                this.bottleAllUC1.BottomItems[1].BottleDesc = "混匀矿";
                this.bottleAllUC1.BottomItems[2].BottleDesc = "混匀矿";
                this.bottleAllUC1.BottomItems[3].BottleDesc = "混匀矿";
                this.bottleAllUC1.BottomItems[4].BottleDesc = "混匀矿";
                this.bottleAllUC1.BottomItems[5].BottleDesc = "混匀矿";
                this.bottleAllUC1.BottomItems[6].BottleDesc = "混匀矿";
                this.bottleAllUC1.BottomItems[7].BottleDesc = "混匀矿";
                this.bottleAllUC1.BottomItems[8].BottleDesc = "燃料";
                this.bottleAllUC1.BottomItems[9].BottleDesc = "燃料";
                this.bottleAllUC1.BottomItems[10].BottleDesc = "石灰石";
                this.bottleAllUC1.BottomItems[11].BottleDesc = "白云石";
                this.bottleAllUC1.BottomItems[12].BottleDesc = "除尘灰";
                this.bottleAllUC1.BottomItems[13].BottleDesc = "除尘灰";
                this.bottleAllUC1.BottomItems[14].BottleDesc = "烧返";
                this.bottleAllUC1.BottomItems[15].BottleDesc = "烧返";
                this.bottleAllUC1.BottomItems[16].BottleDesc = "生石灰";
                this.bottleAllUC1.BottomItems[17].BottleDesc = "生石灰";
                this.bottleAllUC1.BottomItems[18].BottleDesc = "生石灰";
                this.bottleAllUC1.BottomItems[19].BottleDesc = "生石灰";
            }

        }
        delegate void aaa();
        DataTable dataTableMC_BTPCAL_result_1min = new DataTable();
        string MICAL_BU_C_LOCAT_BTP = "";
        string MICAL_BU_C_BTP_TE = "";
        void MongoQMtimer1_Elapsed(object sender, ElapsedEventArgs e)
        {

            try
            {
                string strSQL = "select top(1) *  from C_PLC_3S order by timestamp desc";
                modelT_PLC_3S = db_sugar.SqlQueryable<C_PLC_3S>(strSQL).ToList().FirstOrDefault();
                string sqlcol = "timestamp,BTPCAL_OUT_TOTAL_AVG_X_BTP,BTPCAL_OUT_TOTAL_AVG_TE_BTP";
                string sqlstr = string.Format("select {0} from {1} where timestamp=(select max(timestamp) from {1})", sqlcol, "MC_BTPCAL_result_1min");
                dataTableMC_BTPCAL_result_1min = db_sugar.Ado.GetDataTable(sqlstr);
                if (dataTableMC_BTPCAL_result_1min != null && dataTableMC_BTPCAL_result_1min.Rows.Count > 0)
                {
                    var row = dataTableMC_BTPCAL_result_1min.Rows[0];
                    if (row != null)
                    {
                        if (row["BTPCAL_OUT_TOTAL_AVG_X_BTP"] != null && row["BTPCAL_OUT_TOTAL_AVG_X_BTP"].ToString() != "")
                        {
                            MICAL_BU_C_LOCAT_BTP = row["BTPCAL_OUT_TOTAL_AVG_X_BTP"].ToString();
                        }
                        if (row["BTPCAL_OUT_TOTAL_AVG_TE_BTP"] != null && row["BTPCAL_OUT_TOTAL_AVG_TE_BTP"].ToString() != "")
                        {
                            MICAL_BU_C_BTP_TE = row["BTPCAL_OUT_TOTAL_AVG_TE_BTP"].ToString();
                        }
                    }
                }
                if (modelT_PLC_3S != null)
                {
                    modelMC_MICAL_PAR = db_sugar.Queryable<MC_MICAL_PAR>().ToList().FirstOrDefault();
                    aaa ShowInfo = delegate ()
                    {
                        ////禁止pnl重绘
                        //SendMessage(this.Handle, WM_SETREDRAW, 0, IntPtr.Zero);

                        windowformRefresh();

                        ////允许重绘pnl
                        //SendMessage(this.Handle, WM_SETREDRAW, 1, IntPtr.Zero);

                    };
                    this.Invoke(ShowInfo);

                }
                db_sugar.Dispose();
            }
            catch (Exception ex)
            {

            }
        }
        
        void windowformRefresh()
        {
            //总料量SP PV

            /*if (modelT_PLC_3S.T_TOTAL_SP_W_3S != 0)
            {
                bottleAllUC1.t_total_sp_w_3s = modelT_PLC_3S.T_TOTAL_SP_W_3S.Value.ToString("f0");
            }*/
            if (modelT_PLC_3S.T_TOTAL_PV_W_3S != 0)
            {
                bottleAllUC1.T_TOTAL_PV_W_3S = modelT_PLC_3S.T_TOTAL_PV_W_3S.Value.ToString("F2");
            }

            //#region 漏斗信息 20个仓
            bottleAllUC1.BottomItems[0].BottleObj.CangHao = 1;
            bottleAllUC1.BottomItems[1].BottleObj.CangHao = 2;
            bottleAllUC1.BottomItems[2].BottleObj.CangHao = 3;
            bottleAllUC1.BottomItems[3].BottleObj.CangHao = 4;
            bottleAllUC1.BottomItems[4].BottleObj.CangHao = 5;
            bottleAllUC1.BottomItems[5].BottleObj.CangHao = 6;
            bottleAllUC1.BottomItems[6].BottleObj.CangHao = 7;
            bottleAllUC1.BottomItems[7].BottleObj.CangHao = 8;
            bottleAllUC1.BottomItems[8].BottleObj.CangHao = 9;
            bottleAllUC1.BottomItems[9].BottleObj.CangHao = 10;
            bottleAllUC1.BottomItems[10].BottleObj.CangHao = 11;
            bottleAllUC1.BottomItems[11].BottleObj.CangHao = 12;
            bottleAllUC1.BottomItems[12].BottleObj.CangHao = 13;
            bottleAllUC1.BottomItems[13].BottleObj.CangHao = 14;
            bottleAllUC1.BottomItems[14].BottleObj.CangHao = 15;
            bottleAllUC1.BottomItems[15].BottleObj.CangHao = 16;
            bottleAllUC1.BottomItems[16].BottleObj.CangHao = 17;
            bottleAllUC1.BottomItems[17].BottleObj.CangHao = 18;
            bottleAllUC1.BottomItems[18].BottleObj.CangHao = 19;
            bottleAllUC1.BottomItems[19].BottleObj.CangHao = 20;
            //1-8 体积：1000  9-13体积：500  14-15体积：700？？
            //获取物料编码程序
            bottleAllUC1.BottomItems[0].BottleObj.HeadTag = Getwlbm_Code(1);

            bottleAllUC1.BottomItems[0].BottleObj.BottleTag = modelT_PLC_3S.T_W_1_3S.ToString("f2"); //1号配料仓仓位
            bottleAllUC1.BottomItems[0].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_1_3S) / GetByShangXian_Code(1) * 100;// 1000 * 100;
            bottleAllUC1.BottomItems[0].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_1_3S.ToString();//设定下料量
            bottleAllUC1.BottomItems[0].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_1_3S.ToString();//实际下料量
            bottleAllUC1.BottomItems[0].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_1_3S == 1 ? Brushes.Green : Brushes.DimGray;//下料口启停信号



            bottleAllUC1.BottomItems[1].BottleObj.HeadTag = Getwlbm_Code(2);//通过仓号获取
            bottleAllUC1.BottomItems[1].BottleObj.BottleTag = modelT_PLC_3S.T_W_2_3S.ToString("f2");
            bottleAllUC1.BottomItems[1].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_2_3S) / GetByShangXian_Code(2) * 100;//1000 * 100;
            bottleAllUC1.BottomItems[1].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_2_3S.ToString();
            bottleAllUC1.BottomItems[1].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_2_3S.ToString();
            bottleAllUC1.BottomItems[1].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_2_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[2].BottleObj.HeadTag = Getwlbm_Code(3);
            bottleAllUC1.BottomItems[2].BottleObj.BottleTag = modelT_PLC_3S.T_W_3_3S.ToString("f2");
            bottleAllUC1.BottomItems[2].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_3_3S) / GetByShangXian_Code(3) * 100;//1000 * 100;
            bottleAllUC1.BottomItems[2].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_3_3S.ToString();
            bottleAllUC1.BottomItems[2].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_3_3S.ToString();
            bottleAllUC1.BottomItems[2].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_3_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[3].BottleObj.HeadTag = Getwlbm_Code(4);
            bottleAllUC1.BottomItems[3].BottleObj.BottleTag = modelT_PLC_3S.T_W_4_3S.ToString("f2");
            bottleAllUC1.BottomItems[3].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_4_3S) / GetByShangXian_Code(4) * 100;//1000 * 100;
            bottleAllUC1.BottomItems[3].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_4_3S.ToString();
            bottleAllUC1.BottomItems[3].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_4_3S.ToString();
            bottleAllUC1.BottomItems[3].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_4_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[4].BottleObj.HeadTag = Getwlbm_Code(5);
            bottleAllUC1.BottomItems[4].BottleObj.BottleTag = modelT_PLC_3S.T_W_5_3S.ToString("f2");
            bottleAllUC1.BottomItems[4].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_5_3S) / GetByShangXian_Code(5) * 100;//1000 * 100;
            bottleAllUC1.BottomItems[4].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_5_3S.ToString();
            bottleAllUC1.BottomItems[4].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_5_3S.ToString();
            bottleAllUC1.BottomItems[4].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_5_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[5].BottleObj.HeadTag = Getwlbm_Code(6);
            bottleAllUC1.BottomItems[5].BottleObj.BottleTag = modelT_PLC_3S.T_W_6_3S.ToString("f2");
            bottleAllUC1.BottomItems[5].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_6_3S) / GetByShangXian_Code(6) * 100;//1000 * 100;
            bottleAllUC1.BottomItems[5].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_6_3S.ToString();
            bottleAllUC1.BottomItems[5].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_6_3S.ToString();
            bottleAllUC1.BottomItems[5].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_6_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[6].BottleObj.HeadTag = Getwlbm_Code(7);
            bottleAllUC1.BottomItems[6].BottleObj.BottleTag = modelT_PLC_3S.T_W_7_3S.ToString("f2");
            bottleAllUC1.BottomItems[6].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_7_3S) / GetByShangXian_Code(7) * 100;//1000 * 100;
            bottleAllUC1.BottomItems[6].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_7_3S.ToString();
            bottleAllUC1.BottomItems[6].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_7_3S.ToString();
            bottleAllUC1.BottomItems[6].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_7_3S == 1 ? Brushes.Green : Brushes.DimGray;


            bottleAllUC1.BottomItems[7].BottleObj.HeadTag = Getwlbm_Code(8);
            bottleAllUC1.BottomItems[7].BottleObj.BottleTag = modelT_PLC_3S.T_W_8_3S.ToString("f2");
            bottleAllUC1.BottomItems[7].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_8_3S) / GetByShangXian_Code(8) * 100;//500 * 100;
            bottleAllUC1.BottomItems[7].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_8_3S.ToString();
            bottleAllUC1.BottomItems[7].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_8_3S.ToString();
            bottleAllUC1.BottomItems[7].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_8_3S == 1 ? Brushes.Green : Brushes.DimGray;


            bottleAllUC1.BottomItems[8].BottleObj.HeadTag = Getwlbm_Code(9);
            bottleAllUC1.BottomItems[8].BottleObj.BottleTag = modelT_PLC_3S.T_W_9_3S.ToString("f2");
            bottleAllUC1.BottomItems[8].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_9_3S) / GetByShangXian_Code(9) * 100;//500 * 100;
            bottleAllUC1.BottomItems[8].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_9_3S.ToString();
            bottleAllUC1.BottomItems[8].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_9_3S.ToString();
            bottleAllUC1.BottomItems[8].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_9_3S == 1 ? Brushes.Green : Brushes.DimGray;


            bottleAllUC1.BottomItems[9].BottleObj.HeadTag = Getwlbm_Code(10);
            bottleAllUC1.BottomItems[9].BottleObj.BottleTag = modelT_PLC_3S.T_W_10_3S.ToString("f2");
            bottleAllUC1.BottomItems[9].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_10_3S) / GetByShangXian_Code(10) * 100;//500 * 100;
            bottleAllUC1.BottomItems[9].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_10_3S.ToString();
            bottleAllUC1.BottomItems[9].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_10_3S.ToString();
            bottleAllUC1.BottomItems[9].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_10_3S == 1 ? Brushes.Green : Brushes.DimGray;


            bottleAllUC1.BottomItems[10].BottleObj.HeadTag = Getwlbm_Code(11);
            bottleAllUC1.BottomItems[10].BottleObj.BottleTag = modelT_PLC_3S.T_W_11_3S.ToString("f2");
            bottleAllUC1.BottomItems[10].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_11_3S) / GetByShangXian_Code(11) * 100;//500 * 100;
            bottleAllUC1.BottomItems[10].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_11_3S.ToString();
            bottleAllUC1.BottomItems[10].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_11_3S.ToString();
            bottleAllUC1.BottomItems[10].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_11_3S == 1 ? Brushes.Green : Brushes.DimGray;


            bottleAllUC1.BottomItems[11].BottleObj.HeadTag = Getwlbm_Code(12);
            bottleAllUC1.BottomItems[11].BottleObj.BottleTag = modelT_PLC_3S.T_W_12_3S.ToString("f2");
            bottleAllUC1.BottomItems[11].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_12_3S) / GetByShangXian_Code(12) * 100;//500 * 100;
            bottleAllUC1.BottomItems[11].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_12_3S.ToString();
            bottleAllUC1.BottomItems[11].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_12_3S.ToString();
            bottleAllUC1.BottomItems[11].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_12_3S == 1 ? Brushes.Green : Brushes.DimGray;
            /*bottleAllUC1.BottomItems[11].BottleObj.SetValue2 = modelT_PLC_3S.T_SP_W_14_3S.ToString();
            bottleAllUC1.BottomItems[11].BottleObj.CurrentValue2 = modelT_PLC_3S.T_ACTUAL_W_14_3S.ToString();
            bottleAllUC1.BottomItems[11].BottleObj.SetT_SL_Right = modelT_PLC_3S.T_SL_14_3S == 1 ? Brushes.Green : Brushes.DimGray;*/


            bottleAllUC1.BottomItems[12].BottleObj.HeadTag = Getwlbm_Code(13);
            bottleAllUC1.BottomItems[12].BottleObj.BottleTag = modelT_PLC_3S.T_W_13_3S.ToString("f2");
            bottleAllUC1.BottomItems[12].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_13_3S) / GetByShangXian_Code(13) * 100;//500 * 100;
            bottleAllUC1.BottomItems[12].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_13_3S.ToString();
            bottleAllUC1.BottomItems[12].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_13_3S.ToString();
            bottleAllUC1.BottomItems[12].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_13_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[13].BottleObj.HeadTag = Getwlbm_Code(14);
            bottleAllUC1.BottomItems[13].BottleObj.BottleTag = modelT_PLC_3S.T_W_14_3S.ToString("f2");
            bottleAllUC1.BottomItems[13].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_14_3S) / GetByShangXian_Code(14) * 100;//500 * 100;
            bottleAllUC1.BottomItems[13].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_14_3S.ToString();
            bottleAllUC1.BottomItems[13].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_14_3S.ToString();
            bottleAllUC1.BottomItems[13].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_14_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[14].BottleObj.HeadTag = Getwlbm_Code(15);
            bottleAllUC1.BottomItems[14].BottleObj.BottleTag = modelT_PLC_3S.T_W_15_3S.ToString("f2");
            bottleAllUC1.BottomItems[14].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_15_3S) / GetByShangXian_Code(15) * 100;//500 * 100;
            bottleAllUC1.BottomItems[14].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_15_3S.ToString();
            bottleAllUC1.BottomItems[14].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_15_3S.ToString();
            bottleAllUC1.BottomItems[14].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_15_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[15].BottleObj.HeadTag = Getwlbm_Code(16);
            bottleAllUC1.BottomItems[15].BottleObj.BottleTag = modelT_PLC_3S.T_W_16_3S.ToString("f2");
            bottleAllUC1.BottomItems[15].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_16_3S) / GetByShangXian_Code(16) * 100;//500 * 100;
            bottleAllUC1.BottomItems[15].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_16_3S.ToString();
            bottleAllUC1.BottomItems[15].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_16_3S.ToString();
            bottleAllUC1.BottomItems[15].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_16_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[16].BottleObj.HeadTag = Getwlbm_Code(17);
            bottleAllUC1.BottomItems[16].BottleObj.BottleTag = modelT_PLC_3S.T_W_17_3S.ToString("f2");
            bottleAllUC1.BottomItems[16].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_17_3S) / GetByShangXian_Code(17) * 100;//500 * 100;
            bottleAllUC1.BottomItems[16].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_17_3S.ToString();
            bottleAllUC1.BottomItems[16].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_17_3S.ToString();
            bottleAllUC1.BottomItems[16].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_17_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[17].BottleObj.HeadTag = Getwlbm_Code(18);
            bottleAllUC1.BottomItems[17].BottleObj.BottleTag = modelT_PLC_3S.T_W_18_3S.ToString("f2");
            bottleAllUC1.BottomItems[17].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_18_3S) / GetByShangXian_Code(18) * 100;//500 * 100;
            bottleAllUC1.BottomItems[17].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_18_3S.ToString();
            bottleAllUC1.BottomItems[17].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_18_3S.ToString();
            bottleAllUC1.BottomItems[17].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_18_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[18].BottleObj.HeadTag = Getwlbm_Code(19);
            bottleAllUC1.BottomItems[18].BottleObj.BottleTag = modelT_PLC_3S.T_W_19_3S.ToString("f2");
            bottleAllUC1.BottomItems[18].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_19_3S) / GetByShangXian_Code(19) * 100;//500 * 100;
            bottleAllUC1.BottomItems[18].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_19_3S.ToString();
            bottleAllUC1.BottomItems[18].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_19_3S.ToString();
            bottleAllUC1.BottomItems[18].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_19_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[19].BottleObj.HeadTag = Getwlbm_Code(20);
            bottleAllUC1.BottomItems[19].BottleObj.BottleTag = modelT_PLC_3S.T_W_20_3S.ToString("f2");
            bottleAllUC1.BottomItems[19].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_20_3S) / GetByShangXian_Code(20) * 100;//500 * 100;
            bottleAllUC1.BottomItems[19].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_20_3S.ToString();
            bottleAllUC1.BottomItems[19].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_20_3S.ToString();
            bottleAllUC1.BottomItems[19].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_20_3S == 1 ? Brushes.Green : Brushes.DimGray;

        }
        double getbottleValue(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Math.Round(Convert.ToDouble(obj), 2);
            }
        }
        private double GetByShangXian_Code(int num)
        {
            string str = "select top(1) isnull(L2_CODE,0)  from M_MATERIAL_BINS where BIN_NUM_SHOW=" + num;
            double codestr = db_sugar.Ado.GetDouble(str);

            if (modelMC_MICAL_PAR != null)
            {
                if (codestr >= 101 && codestr <= 299 || (codestr >= 600 && codestr <= 699))
                {
                    if (modelMC_MICAL_PAR.PAR_IRON_BUNK_UP != 0)
                    {
                        return modelMC_MICAL_PAR.PAR_IRON_BUNK_UP;
                    }
                }
                else if (codestr >= 300 && codestr <= 399)
                {
                    if (modelMC_MICAL_PAR.PAR_FUEL_BUNK_UP != 0)
                    {
                        return modelMC_MICAL_PAR.PAR_FUEL_BUNK_UP;
                    }
                }
                else if ((codestr >= 400 && codestr <= 499))
                {
                    if (modelMC_MICAL_PAR.PAR_SOL_BUNK_UP != 0)
                    {
                        return modelMC_MICAL_PAR.PAR_SOL_BUNK_UP;
                    }
                }
                else if (codestr >= 500 && codestr <= 599)
                {
                    if (modelMC_MICAL_PAR.PAR_DUST_BUNK_UP != 0)
                    {
                        return modelMC_MICAL_PAR.PAR_DUST_BUNK_UP;
                    }
                }
            }
            return 1000;

        }

        private string Getwlbm_Code(int num)
        {
            string str = "select top(1) isnull(MAT_DESC,'')  from M_MATERIAL_COOD where L2_CODE=( select top(1) isnull(L2_CODE,0)  from M_MATERIAL_BINS where BIN_NUM_SHOW=" + num + ")";
            string L2_NAME = "";
            try
            {
                L2_NAME = Convert.ToString(db_sugar.Ado.GetString(str));
            }
            catch (Exception ee)
            {
                LogHelper.LogError(ee.Message);
            }

            return num + ":" + L2_NAME;


        }
        private void Timer_1_Tick(object sender, EventArgs e)
        {
            DateTime d1 = DateTime.Now.AddMonths(-1);
            DateTime d2 = DateTime.Now;
            quxian(d1, d2);
            pizhong();//每分钟调用一次
        }
        private void Timer_2_Tick(object sender, EventArgs e)
        {
            sszzjh();
        }
        //最新调整时间
        public void GetNewTime()
        {
            DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            string sql = "select top(1) TIMESTAMP from MC_POPCAL_RESULT order by TIMESTAMP desc;";
            DataTable table = dBSQL.GetCommand(sql);
            string time = table.Rows[0][0].ToString();
            this.label6.Text = "最新调整时间:" + time;
        }
        public void pizhong()
        {
            DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            string sql = "select top(1) MAT_PLC_T_PV_W from C_MAT_PLC_1MIN order by TIMESTAMP desc;";
            DataTable SSpizhong = dBSQL.GetCommand(sql);
            if (SSpizhong.Rows.Count > 0)
            {
                this.label8.Text = "实际批重:" + SSpizhong.Rows[0][0].ToString()+"t/h";
            }  
        }
        //获取查询的时间
        public DateTime GetStartTime(int time)
        {
            DateTime Time = new DateTime();
            if (time == 24)
            {
                time = 0;
                Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, time, 0, 0);
            }
            else
            {
                Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, time, 0, 0);
            }       
            return Time;
        }
        public DateTime GetMTime(int time)
        {
            DateTime Time = new DateTime();
            Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(1).Day, time, 0, 0);
            return Time;
        }
        public DateTime GetZTime(int time)
        {
            DateTime Time = new DateTime();
            Time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.AddDays(-1).Day, time, 0, 0);
            return Time;
        }
        //生产组织计划表格查询
        public void sszzjh()
        {
            DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            string sql = "select top(1) TIMESTAMP,POPCAL_N_OUT_PL,POPCAL_D_OUT_PL,POPCAL_A_OUT_PL from MC_POPCAL_RESULT order by TIMESTAMP desc;";
            DataTable table = dBSQL.GetCommand(sql);
            if (table.Rows.Count > 0)
            {
                DataGridViewRow row11 = new DataGridViewRow();
                rowMergeView1.Rows.Add(row11);
                //计划产量
                this.rowMergeView1.Rows[0].Cells["time"].Value = "计划产量";
                this.rowMergeView1.Rows[0].Cells["Column21"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_D_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["Column22"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_D_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["Column23"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_D_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["Column24"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_D_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["Column25"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_D_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["Column26"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_D_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["白班合计"].Value = Convert.ToDouble(table.Rows[0]["POPCAL_D_OUT_PL"]).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["Column27"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_N_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["Column28"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_N_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["Column29"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_N_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["Column30"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_N_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["Column31"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_N_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["Column32"].Value = (Convert.ToDouble(table.Rows[0]["POPCAL_N_OUT_PL"]) / 60).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["夜班合计"].Value = Convert.ToDouble(table.Rows[0]["POPCAL_N_OUT_PL"]).ToString("0.00");
                this.rowMergeView1.Rows[0].Cells["合计"].Value = Convert.ToDouble(table.Rows[0]["POPCAL_A_OUT_PL"]).ToString("0.00");
                //int index = this.dataGridView1.Rows.Add();
                /*DataGridViewRow row = new DataGridViewRow();
                rowMergeView1.Rows.Add(row);
                //this.rowMergeView1.CurrentRow
                this.rowMergeView1.Rows[1].Cells["time"].Value = "计划产量";
                this.rowMergeView1.Rows[1].Cells["Column21"].Value = Convert.ToDouble(table.Rows[0]["POPCAL_D_OUT_PL"]).ToString("0.00");
                this.rowMergeView1.Rows[1].Cells["Column21"].Value = Convert.ToDouble(table.Rows[0]["POPCAL_N_OUT_PL"]).ToString("0.00");
                this.rowMergeView1.Rows[1].Cells["Column21"].Value = Convert.ToDouble(table.Rows[0]["POPCAL_A_OUT_PL"]).ToString("0.00");
                this.rowMergeView1.Rows[1].Cells["合计"].Value = Convert.ToDouble(table.Rows[0]["POPCAL_A_OUT_PL"]).ToString("0.00");*/
            }
            //llcl中存放的是理论产量、sj中存放的是实际产量
            List<double> llcl = new List<double>();
            List<double> sj = new List<double>();
            int j = 10, t = 2;
            for (int i = 0; i < 13; i++)
            {
                if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 24)//8-24获取当天，0-8获取明天
                {
                    if (j >= 8 && j <= 24)
                    {
                        DateTime Start = GetStartTime(j - 2);
                        DateTime End = GetStartTime(j);
                        string theory = "select isnull(sum(SINCAL_OUTPUT_PV),0) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP between '" + Start + "' and '" + End + "'";
                        System.Data.DataTable Tllcl = dBSQL.GetCommand(theory);
                        if (Tllcl.Rows.Count > 0)
                        {
                            llcl.Add(Convert.ToDouble(Tllcl.Rows[0][0]));
                        }
                        else
                        {
                            llcl.Add(Double.NaN);
                        }
                        string sjcl = "select isnull(sum(CFP_PLC_PROD_DELT1_FQ)/60,0) from C_CFP_PLC_1MIN where TIMESTAMP between '" + Start + "' and '" + End + "'";
                        System.Data.DataTable Tsjcl = dBSQL.GetCommand(sjcl);
                        if (Tsjcl.Rows.Count > 0)
                        {
                            sj.Add(Convert.ToDouble(Tsjcl.Rows[0][0]));
                        }
                    }
                    if (j > 24)
                    {
                        DateTime Start = GetMTime(t - 2);
                        DateTime End = GetMTime(t);
                        string theory = "select isnull(sum(SINCAL_OUTPUT_PV),0) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP between '" + Start + "' and '" + End + "'";
                        System.Data.DataTable Tllcl = dBSQL.GetCommand(theory);
                        if (Tllcl.Rows.Count > 0)
                        {
                            llcl.Add(Convert.ToDouble(Tllcl.Rows[0][0]));
                        }
                        else
                        {
                            llcl.Add(Double.NaN);
                        }
                        string sjcl = "select isnull(sum(CFP_PLC_PROD_DELT1_FQ)/60,0) from C_CFP_PLC_1MIN where TIMESTAMP between '" + Start + "' and '" + End + "'";
                        System.Data.DataTable Tsjcl = dBSQL.GetCommand(sjcl);
                        if (Tsjcl.Rows.Count > 0)
                        {
                            sj.Add(Convert.ToDouble(Tsjcl.Rows[0][0]));
                        }
                    }
                    j = j + 2;
                }
                else//8-24获取前一天，0-8获取今天
                {
                    if (j >= 8 && j <= 24)//8-24点
                    {
                        DateTime Start = GetZTime(j - 2);
                        DateTime End = GetZTime(j);
                        string theory = "select isnull(sum(SINCAL_OUTPUT_PV),0) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP between '" + Start + "' and '" + End + "'";
                        System.Data.DataTable Tllcl = dBSQL.GetCommand(theory);
                        if (Tllcl.Rows.Count > 0)
                        {
                            llcl.Add(Convert.ToDouble(Tllcl.Rows[0][0]));
                        }
                        else
                        {
                            llcl.Add(Double.NaN);
                        }
                        string sjcl = "select isnull(sum(CFP_PLC_PROD_DELT1_FQ)/60,0) from C_CFP_PLC_1MIN where TIMESTAMP between '" + Start + "' and '" + End + "'";
                        System.Data.DataTable Tsjcl = dBSQL.GetCommand(sjcl);
                        if (Tsjcl.Rows.Count > 0)
                        {
                            sj.Add(Convert.ToDouble(Tsjcl.Rows[0][0]));
                        }
                    }
                    if (j > 24)
                    {
                        DateTime Start = GetStartTime(t - 2);
                        DateTime End = GetStartTime(t);
                        string theory = "select isnull(sum(SINCAL_OUTPUT_PV)) from MC_MIXCAL_RESULT_1MIN where TIMESTAMP between '" + Start + "' and '" + End + "'";
                        System.Data.DataTable Tllcl = dBSQL.GetCommand(theory);
                        if (Tllcl.Rows.Count > 0)
                        {
                            llcl.Add(Convert.ToDouble(Tllcl.Rows[0][0]));
                        }
                        else
                        {
                            llcl.Add(Double.NaN);
                        }
                        string sjcl = "select isnull(sum(CFP_PLC_PROD_DELT1_FQ)/60) from C_CFP_PLC_1MIN where TIMESTAMP between '" + Start + "' and '" + End + "'";
                        System.Data.DataTable Tsjcl = dBSQL.GetCommand(sjcl);
                        if (Tsjcl.Rows.Count > 0)
                        {
                            sj.Add(Convert.ToDouble(Tsjcl.Rows[0][0]));
                        }
                    }
                    j = j + 2;
                }
            }
            //DataTable = new DataTable();
            //list中数据展示到页面
            DataGridViewRow row1 = new DataGridViewRow();
            rowMergeView1.Rows.Add(row1);
            this.rowMergeView1.Rows[1].Cells["time"].Value = "理论产量";
            this.rowMergeView1.Rows[1].Cells["Column21"].Value = Convert.ToDouble(llcl[0]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["Column22"].Value = Convert.ToDouble(llcl[1]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["Column23"].Value = Convert.ToDouble(llcl[2]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["Column24"].Value = Convert.ToDouble(llcl[3]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["Column25"].Value = Convert.ToDouble(llcl[4]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["Column26"].Value = Convert.ToDouble(llcl[5]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["白班合计"].Value = Convert.ToDouble(llcl[0] + llcl[1] + llcl[2] + llcl[3] + llcl[4] + llcl[5]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["Column27"].Value = Convert.ToDouble(llcl[6]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["Column28"].Value = Convert.ToDouble(llcl[7]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["Column29"].Value = Convert.ToDouble(llcl[8]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["Column30"].Value = Convert.ToDouble(llcl[9]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["Column31"].Value = Convert.ToDouble(llcl[10]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["Column32"].Value = Convert.ToDouble(llcl[11]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["夜班合计"].Value = Convert.ToDouble(llcl[6] + llcl[7] + llcl[8] + llcl[9] + llcl[10] + llcl[11]).ToString("0.00");
            this.rowMergeView1.Rows[1].Cells["合计"].Value = Convert.ToDouble(llcl[0] + llcl[1] + llcl[2] + llcl[3] + llcl[4] + llcl[5] + llcl[6] + llcl[7] + llcl[8] + llcl[9] + llcl[10] + llcl[11]).ToString("0.00");
            /*DataGridViewRow row2 = new DataGridViewRow();
            rowMergeView1.Rows.Add(row2);
            this.rowMergeView1.Rows[3].Cells["time"].Value = "理论产量";
            this.rowMergeView1.Rows[3].Cells["Column21"].Value = Convert.ToDouble(llcl[0]+ llcl[1] + llcl[2] + llcl[3] + llcl[4] + llcl[5]).ToString("0.00");
            this.rowMergeView1.Rows[3].Cells["Column27"].Value = Convert.ToDouble(llcl[6] + llcl[7] + llcl[8] + llcl[9] + llcl[10] + llcl[11]).ToString("0.00");
            this.rowMergeView1.Rows[3].Cells["合计"].Value = Convert.ToDouble(llcl[0] + llcl[1] + llcl[2] + llcl[3] + llcl[4] + llcl[5]+llcl[6] + llcl[7] + llcl[8] + llcl[9] + llcl[10] + llcl[11]).ToString("0.00");*/
            DataGridViewRow row3 = new DataGridViewRow();
            rowMergeView1.Rows.Add(row3);
            this.rowMergeView1.Rows[2].Cells["time"].Value = "实际产量";
            this.rowMergeView1.Rows[2].Cells["Column21"].Value = Convert.ToDouble(sj[0]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["Column22"].Value = Convert.ToDouble(sj[1]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["Column23"].Value = Convert.ToDouble(sj[2]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["Column24"].Value = Convert.ToDouble(sj[3]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["Column25"].Value = Convert.ToDouble(sj[4]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["Column26"].Value = Convert.ToDouble(sj[5]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["白班合计"].Value = Convert.ToDouble(sj[0] + sj[1] + sj[2] + sj[3] + sj[4] + sj[5]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["Column27"].Value = Convert.ToDouble(sj[6]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["Column28"].Value = Convert.ToDouble(sj[7]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["Column29"].Value = Convert.ToDouble(sj[8]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["Column30"].Value = Convert.ToDouble(sj[9]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["Column31"].Value = Convert.ToDouble(sj[10]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["Column32"].Value = Convert.ToDouble(sj[11]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["夜班合计"].Value = Convert.ToDouble(sj[6] + sj[7] + sj[8] + sj[9] + sj[10] + sj[11]).ToString("0.00");
            this.rowMergeView1.Rows[2].Cells["合计"].Value = Convert.ToDouble(sj[0] + sj[1] + sj[2] + sj[3] + sj[4] + sj[5] + sj[6] + sj[7] + sj[8] + sj[9] + sj[10] + sj[11]).ToString("0.00");
            /*DataGridViewRow row4 = new DataGridViewRow();
            rowMergeView1.Rows.Add(row4);
            this.rowMergeView1.Rows[5].Cells["time"].Value = "实际产量";
            this.rowMergeView1.Rows[5].Cells["Column21"].Value = Convert.ToDouble(sj[0] + sj[1] + sj[2] + sj[3] + sj[4] + sj[5]).ToString("0.00");
            this.rowMergeView1.Rows[5].Cells["Column27"].Value = Convert.ToDouble(sj[6] + sj[7] + sj[8] + sj[9] + sj[10] + sj[11]).ToString("0.00");
            this.rowMergeView1.Rows[5].Cells["合计"].Value = Convert.ToDouble(sj[0] + sj[1] + sj[2] + sj[3] + sj[4] + sj[5]+ sj[6] + sj[7] + sj[8] + sj[9] + sj[10] + sj[11]).ToString("0.00");
            this.rowMergeView1.MergeColumnNames.Add("time");
            this.rowMergeView1.MergeColumnNames.Add("合计");*/
        }
        private void Check_text()
        {
            DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            string sql = "select top(1) TIMESTAMP, P_AL_OUTPUT, T_AL_OUTPUT, A_AL_OUTPUT from MC_POPCAL_OUT order by TIMESTAMP";
            DataTable table = dBSQL.GetCommand(sql);
            if (table.Rows.Count > 0)
            {
                this.checkBox1.Text = "计划产量:" + table.Rows[0][1].ToString() + "t";
                this.checkBox2.Text = "理论产量:" + table.Rows[0][2].ToString() + "t";
                this.checkBox3.Text = "实际产量:" + table.Rows[0][3].ToString() + "t";
            }
        }
        public void llpizhong()
        {
            DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            string sql = "select top(1) POPCAL_BW_THR from MC_POPCAL_RESULT order by TIMESTAMP desc;";
            DataTable LLpizhong = dBSQL.GetCommand(sql);
            if (LLpizhong.Rows.Count > 0)
            {
                this.label7.Text = "理论批重:"+LLpizhong.Rows[0][0].ToString()+"t/h";
            }
        }
        public static int getnMax(int max, int min)
        {
            int s = 0;
            if (min != 0)
            {
                if (max % min != 0)
                {
                    s = (max / min + 1) * min;
                }
                else
                {
                    s = max;
                }

            }
            else
            {
                s = max;
            }
            return s;
        }
        //用于存放坐标点
        List<DataPoint> Line1 = new List<DataPoint>();
        List<DataPoint> Line2 = new List<DataPoint>();
        List<DataPoint> Line3 = new List<DataPoint>();
        PlotModel _myPlotModel_1;
        LinearAxis _valueAxis1_1;
        LinearAxis _valueAxis1_2;
        LinearAxis _valueAxis1_3;
        OxyPlot.Series.LineSeries series1_1;
        OxyPlot.Series.LineSeries series1_2;
        OxyPlot.Series.LineSeries series1_3;
        List<double> list1 = new List<double>();
        List<double> list2 = new List<double>();
        List<double> list3 = new List<double>();
        public void quxian(DateTime time_BIGIN, DateTime time_END)
        {
            Line1.Clear();
            Line2.Clear();
            Line3.Clear();
            list1.Clear();
            list2.Clear();
            list3.Clear();
            //数据采集
            DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            string sql = "select TIMESTAMP,P_AL_OUTPUT,T_AL_OUTPUT,A_AL_OUTPUT from MC_POPCAL_OUT where TIMESTAMP >= '" + time_BIGIN + "' and TIMESTAMP <= '" + time_END + "' order by TIMESTAMP";
            DataTable table = dBSQL.GetCommand(sql);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i]["P_AL_OUTPUT"] == null)
                {

                }
                DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table.Rows[i]["P_AL_OUTPUT"]));
                Line1.Add(line1);
                list1.Add(Convert.ToDouble(table.Rows[i]["P_AL_OUTPUT"]));
                DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table.Rows[i]["T_AL_OUTPUT"]));
                Line2.Add(line2);
                list2.Add(Convert.ToDouble(table.Rows[i]["T_AL_OUTPUT"]));
                DataPoint line3 = new DataPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table.Rows[i]["A_AL_OUTPUT"]));
                Line3.Add(line3);
                list3.Add(Convert.ToDouble(table.Rows[i]["A_AL_OUTPUT"]));
            }
            //曲线准备
            _myPlotModel_1 = new PlotModel()
            {
                Background = OxyColors.White,
            };
            var _dateAxis = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IntervalLength = 100,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                AxisTickToLabelDistance = 0,
                FontSize = 9.0,
                MinorIntervalType = DateTimeIntervalType.Days,
                IntervalType = DateTimeIntervalType.Days,
                StringFormat = "yyyy/MM/dd HH:mm",
            };
            _myPlotModel_1.Axes.Add(_dateAxis);
            _valueAxis1_1 = new LinearAxis()
            {
                Key = "计划产量",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                PositionTier = 1,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Red,
                MinorTicklineColor = OxyColors.Red,
                TicklineColor = OxyColors.Red,
                TextColor = OxyColors.Red,
                FontSize = 9.0,
                IsAxisVisible = false,
                MinorTickSize = 0,
                Maximum = getnMax((int)list1.Max() + 1,(int)list1.Min() - 1),
                Minimum = (int)list1.Min() - 1,
                MajorStep= (int)list1.Min() - 1,
            };
            _myPlotModel_1.Axes.Add(_valueAxis1_1);
            series1_1 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Red,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "计划产量",
                ItemsSource = Line1,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n计划产量:{4}",

            };
            if (checkBox1.Checked == true)
            {
                _valueAxis1_1.IsAxisVisible = true;
                _myPlotModel_1.Series.Add(series1_1);
            }
            //曲线2
            _valueAxis1_2 = new LinearAxis()
            {
                Key = "理论产量",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                PositionTier = 2,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Purple,
                MinorTicklineColor = OxyColors.Purple,
                TicklineColor = OxyColors.Purple,
                TextColor = OxyColors.Purple,
                FontSize = 9.0,
                IsAxisVisible = false,
                MinorTickSize = 0,
                Maximum = getnMax((int)list2.Max() + 1,(int)list2.Min() - 1),
                Minimum = (int)list2.Min() - 1,
                MajorStep= (int)list2.Min() - 1,
            };
            
            _myPlotModel_1.Axes.Add(_valueAxis1_2);
            series1_2 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Purple,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "理论产量",
                ItemsSource = Line2,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n理论产量:{4}",
            };
            if (checkBox2.Checked == true)
            {
                _valueAxis1_2.IsAxisVisible = true;
                _myPlotModel_1.Series.Add(series1_2);
            }
            
            //曲线3
            _valueAxis1_3 = new LinearAxis()
            {
                Key = "实际产量",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IsZoomEnabled = true,
                IsPanEnabled = false,
                PositionTier = 3,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Green,
                MinorTicklineColor = OxyColors.Green,
                TicklineColor = OxyColors.Green,
                TextColor = OxyColors.Green,
                FontSize = 9.0,
                IsAxisVisible = false,
                MinorTickSize = 0,
                Maximum = getnMax((int)list3.Max() + 1,(int)list3.Min() - 1),
                Minimum = (int)list3.Min() - 1,
                MajorStep= (int)list3.Min() - 1,
            };
            _myPlotModel_1.Axes.Add(_valueAxis1_3);
            series1_3 = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Green,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.None,
                YAxisKey = "实际产量",
                ItemsSource = Line3,
                TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n实际产量:{4}",
            };
            if (checkBox3.Checked == true)
            {
                _valueAxis1_3.IsAxisVisible = true;
                _myPlotModel_1.Series.Add(series1_3);
            }
            plotView2.Model = _myPlotModel_1;
            var PlotController = new OxyPlot.PlotController();
            PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);

            plotView2.Controller = PlotController;

            //绑定数据
        }
        //实时
        private void simpleButton3_click(object sender, EventArgs e)
        {
            DateTime d1 = DateTime.Now.AddMonths(-1);
            DateTime d2 = DateTime.Now;
            quxian(d1, d2);
        }
        //查询曲线功能
        private void simpleButton2_click(object sender, EventArgs e)
        {
            DateTime d1= Convert.ToDateTime(textBox_begin.Text);
            DateTime d2 = Convert.ToDateTime(textBox_end.Text);
            quxian(d1,d2);
        }
        private void simpleButton1_click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        //修改数据产量
        private void simpleButton4_click(object sender, EventArgs e)
        {
            Frm_SSZZ_yjhcl form_display = new Frm_SSZZ_yjhcl();
            if (Frm_SSZZ_yjhcl.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
            sszzjh();
        }
        //产量数据查询按钮
        private void simpleButton5_click(object sender, EventArgs e)
        {

            Frm_SSZZ_clsjcx form_display = new Frm_SSZZ_clsjcx();
            if (Frm_SSZZ_clsjcx.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        private void check_event_1(object sender, EventArgs e)
        {
            try
            {
                plotView2.Model = null;
                if (checkBox1.Checked == true)
                {
                    _valueAxis1_1.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series1_1);
                }
                if (checkBox1.Checked == false)
                {
                    _valueAxis1_1.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series1_1);
                }
                plotView2.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        private void check_event_2(object sender, EventArgs e)
        {
            try
            {
                plotView2.Model = null;
                if (checkBox2.Checked == true)
                {
                    _valueAxis1_2.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series1_2);
                }
                if (checkBox2.Checked == false)
                {
                    _valueAxis1_2.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series1_2);
                }
                plotView2.Model = _myPlotModel_1;
            }
            catch
            { }
        }

        private void check_event_3(object sender, EventArgs e)
        {
            try
            {
                plotView2.Model = null;
                if (checkBox3.Checked == true)
                {
                    _valueAxis1_3.IsAxisVisible = true;
                    _myPlotModel_1.Series.Add(series1_3);
                }
                if (checkBox3.Checked == false)
                {
                    _valueAxis1_3.IsAxisVisible = false;
                    _myPlotModel_1.Series.Remove(series1_3);
                }
                plotView2.Model = _myPlotModel_1;
            }
            catch
            { }
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
    }
}
