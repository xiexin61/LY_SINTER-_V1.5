using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UserControlIndex;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using NBSJ_MAIN_UC.Model;
using SqlSugar;
using System.Timers;



namespace NBSJ_MAIN_UC
{
    public partial class MainUserControl : UserControl
    {
        private  System.Timers.Timer MongoQMtimer1;//自定义一个定时器
        SqlSugarClient db_sugar = GetInstance();
        public int RefreshTime = 0;
        public static SqlSugarClient GetInstance()
        {
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig() { ConnectionString = ADODB.ConnectionString, DbType = SqlSugar.DbType.SqlServer, IsAutoCloseConnection = true });
            return db;
        }
        public MainUserControl()
        {
          
            InitializeComponent();
            this.picYantong = new System.Windows.Forms.PictureBox();
            this.picYantong.Image =global::NBSJ_MAIN_UC.Properties.Resources.烟筒1;
            this.picYantong.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picYantong.Size = new System.Drawing.Size(140, 170);
            this.Controls.Add(this.picYantong);
            this.TextQuYangTime.Text = "";
            this.labQuyangTime.Visible = true;
            this.TextQuYangTime.BackColor = Color.FromArgb(0xff, 0xff, 0xff);
            // comboBox1.Enabled = false;
            this.labZ1_1.Visible = false;
            this.labSF_2.Visible = false;
            labPuDiLiaoCaoFlow.Text = "";
            #region 初始化的
            /*pipeLine11 = new PipeLine();
            this.Controls.Add(this.pipeLine11);*/
            pipeLine12 = new PipeLine();
            this.Controls.Add(this.pipeLine12);
            /*pipeLine13 = new PipeLine();
            this.Controls.Add(this.pipeLine13);*/
            pipeLine14 = new PipeLine();
            this.Controls.Add(this.pipeLine14);
            pipeLine15 = new PipeLine();
            this.Controls.Add(this.pipeLine15);
            pipeLine16 = new PipeLine();
            this.Controls.Add(this.pipeLine16);

            //pipeLine11.PipeLineActive = true;
            pipeLine12.PipeLineActive = true;
            //pipeLine13.PipeLineActive = true;
            pipeLine14.PipeLineActive = true;
            pipeLine15.PipeLineActive = true;
            pipeLine16.PipeLineActive = true;

            //pipeLine11.MoveSpeed = -2.5f;
            pipeLine12.MoveSpeed = -2.5f;
            //pipeLine13.MoveSpeed = -2.5f;
            pipeLine14.MoveSpeed = -2.5f;
            pipeLine15.MoveSpeed = -2.5f;
            pipeLine16.MoveSpeed = -2.5f;

            //pipeLine11.PipeLineName = "Z5-1皮带";
            pipeLine12.PipeLineName = "CP1(成-5)";
            //pipeLine13.PipeLineName = "板式给矿机";
            pipeLine14.PipeLineName = "Z61(铺-1)";
            pipeLine15.PipeLineName = "Z51皮带";
            pipeLine16.PipeLineName = "SLG皮带";

            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.DoubleBuffered = true;
            this.BackColor = Color.FromArgb(0xee, 0xee, 0xe0);//cbe3f8
                                                              //
           
            btnHuanXinLiao.Location = new System.Drawing.Point((int)(this.Width * 0.22f), 1);

            myBitmap1 =global::NBSJ_MAIN_UC.Properties.Resources.皮带点;
            myBitmap2 =global::NBSJ_MAIN_UC.Properties.Resources.皮带点;
            myBitmap3 = global::NBSJ_MAIN_UC.Properties.Resources.余热发电;
            #endregion 初始化的
            UC_Load();
             db_sugar.Dispose();
            
        }
        PictureBox picYantong;

        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0xB;

        //禁止pnl重绘
        //SendMessage(SelfInfo_pnlContact1.Handle, WM_SETREDRAW, 0, IntPtr.Zero);

        //允许重绘pnl
        //SendMessage(SelfInfo_pnlContact1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);



        MC_MICAL_PAR modelMC_MICAL_PAR = new MC_MICAL_PAR();// iDataBase.Queryable<MC_MICAL_PAR>().FirstOrDefault();
        
        /// <summary>
        ///  获取物料编码
        /// </summary>
        /// <param name="codestr"></param>
        /// <returns></returns>
        private string Getwlbm_Code(int num)
        {
          string str = "select top(1) isnull(MAT_DESC,'')  from M_MATERIAL_COOD where L2_CODE=( select top(1) isnull(L2_CODE,0)  from M_MATERIAL_BINS where BIN_NUM_SHOW="+ num+")";
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


        /// <summary>
        ///  获取仓位上限
        /// </summary>
        /// <param name="codestr"></param>
        /// <returns></returns>
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


        string quyangTime = "";

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(this.BackColor);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;


            RefreshFun(graphics);
            //graphics.Dispose();
            //base.OnPaint(e);
        }

        #region 继承
        /// <summary>
        ///     显示UC
        /// </summary>
        public  void UC_Load()
        {
            pipeLine_Right1.PipeLineActive(true);
            pipeLine_Right1.SetIsRunStop(false);
            pipeLine_Text1.PipeLineActive(true);
            pipeLine_Two1.PipeLineActive(true);
           


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

           // RefreshTime = 5;
            //worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            //worker.DoWork += new DoWorkEventHandler(worker_DoWork);

            //worker.WorkerSupportsCancellation = true;
            //if (!worker.IsBusy)
            //{
            //    worker.RunWorkerAsync();
            //}

            //InitTimer();
            //BeginTimer();

            // UC_Load();
        }

        /// <summary>
        ///     每次显示UC
        /// </summary>
        public  void UC_Show()
        {
           UC_Show();
        }
        /// <summary>
        ///     关闭UC
        /// </summary>
        public  void UC_Close()
        {
            //StopTimer();
            if (worker.IsBusy && !worker.CancellationPending)
            {
                //System.Threading.Thread.Sleep(500);
                worker.CancelAsync();
            }
           // AutoDispose();
            UC_Close();
        }

        /// <summary>
        ///     释放UC
        /// </summary>
        public  void UC_Dispose()
        {
            //StopTimer();
            if (worker.IsBusy && !worker.CancellationPending)
            {
                //System.Threading.Thread.Sleep(500);
                worker.CancelAsync();
            }
           // DestroyIList(wlbmModel);
            UC_Dispose();
        }

        DataTable dataTableMC_BTPCAL_result_1min = new DataTable();
        /// <summary>
        /// BTP位置
        /// </summary>
        string MICAL_BU_C_LOCAT_BTP = "";
        /// <summary>
        /// BTP温度
        /// </summary>
        string MICAL_BU_C_BTP_TE = "";
        /// <summary>
        /// 定时器刷新方法
        /// </summary>
        public  void TimerElapsed()
        {
            try
            {
             
                 string strSQL = "select top(1) *  from C_PLC_3S ORDER BY TIMESTAMP DESC";
                //string Temp = iDataBase.GetString(strSQL);
                try
                {
                    modelT_PLC_3S = db_sugar.SqlQueryable<C_PLC_3S>(strSQL).ToList().FirstOrDefault();
                }
                catch (Exception EE)
                {
                    LogHelper.LogError(EE.Message);
                }

                string sqlcol = "timestamp,BTPCAL_OUT_8_X_AVG_BTP,BTPCAL_OUT_8_TE_AVG_BTP,BTPCAL_QE4_8_X_AVG_BRP,BTPCAL_QE4_8_TE_AVG_BRP";
                    string sqlstr = string.Format("select {0} from {1} where timestamp=(select max(timestamp) from {1})", sqlcol, "MC_BTPCAL_result_1min");
                try
                {
                    dataTableMC_BTPCAL_result_1min = db_sugar.Ado.GetDataTable(sqlstr);

                }
                catch (Exception ee)
                {
                    LogHelper.LogError(ee.Message);
                }
              if (dataTableMC_BTPCAL_result_1min != null && dataTableMC_BTPCAL_result_1min.Rows.Count > 0)
                    {
                        var row = dataTableMC_BTPCAL_result_1min.Rows[0];
                        if (row != null)
                        {
                            if (row["BTPCAL_OUT_8_X_AVG_BTP"] != null && row["BTPCAL_OUT_8_X_AVG_BTP"].ToString() != "")
                            {
                                MICAL_BU_C_LOCAT_BTP = row["BTPCAL_OUT_8_X_AVG_BTP"].ToString();
                            }
                            if (row["BTPCAL_OUT_8_TE_AVG_BTP"] != null && row["BTPCAL_OUT_8_TE_AVG_BTP"].ToString() != "")
                            {
                                MICAL_BU_C_BTP_TE = row["BTPCAL_OUT_8_TE_AVG_BTP"].ToString();
                            }
                        }
                    }


                    if (modelT_PLC_3S != null)
                    {
                        windowformRefresh();
                    }
                    db_sugar.Dispose();
                
                //mc_GUIDE_SAMPLING.START_TIME+
                //if(mc_GUIDE_SAMPLING.START_TIME.Value.addd)
                //InsertUpdate_MC_GUIDE_SAMPLING(false);


            }
            catch (Exception ex)
            { }

            TimerElapsed();
        }
        #endregion 继承

        #region 界面上的变量
        C_PLC_3S modelT_PLC_3S = null;

        Bitmap myBitmap1 = null;
        Bitmap myBitmap2 = null;
        Bitmap myBitmap3 = null;
        protected LinearGradientBrush brush;
        protected ColorBlend blend = new ColorBlend();
        protected StringFormat sf;

        //private PipeLine pipeLine11;
        private PipeLine pipeLine12;
        //private PipeLine pipeLine13;
        private PipeLine pipeLine14;
        private PipeLine pipeLine15;
        private PipeLine pipeLine16;
        Point point1 = new Point(0, 0);
        Point point2 = new Point(0, 0);
        /// <summary>
        /// 烟筒
        /// </summary>
        Bitmap myBitmapYt = null;
        /// <summary>
        /// 脱硫脱硝桶
        /// </summary>
        Bitmap myBitmapTlTx = null;


        BackgroundWorker worker = new BackgroundWorker();
        delegate void aaa();
        #endregion 界面上的变量


        #region 界面的方法

        /// <summary>
        /// 漏斗控件高度的比例
        /// </summary>
        float bottle_Height_K = 0.2f;
        /// <summary>
        /// 边缘宽度系数
        /// </summary>
        float edgeWidthK = 0.005f;
        float xStart = 0;
        float yStart = 0;
        private void InitControl()
        {
            this.bottleAllUC1.Dock = System.Windows.Forms.DockStyle.None;
            //料仓
            
            for (int i = 0; i < 11; i++)
            {
                bottleAllUC1.BottomItems.Add(new BottleItem { BottleType = BottleType.BottleSingle });
                bottleAllUC1.BottomItems[i].BottleObj.Value = (i+1) * 10;
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

            bottleAllUC1.PipeLineActive(true);
            bottleAllUC1.SetIsRunStop(false);
            this.hostConveyerUC1.ForeColor = Color.FromArgb(0x00, 0x78, 0x50); //Color.Green;// Color.FromArgb(0xf0, 0x1b, 0x2d); //System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));

            panel1.Dock = System.Windows.Forms.DockStyle.None;

            myBitmapYt =global::NBSJ_MAIN_UC.Properties.Resources.烟筒;
            //myBitmapYt =global::NBSJ_MAIN_UC.Properties.Resources.烟筒1;
            //image烟筒 = (Image)NBSJ_MAIN_UC.Properties.Resources.烟筒1;

            myBitmapTlTx =global::NBSJ_MAIN_UC.Properties.Resources.脱硫脱硝桶;

            RefreshFun(this.CreateGraphics());

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
        //Image image烟筒;

        private void RefreshFun(Graphics graphics)
        {
            //shaiZiUC1.Visible = false;
            //漏斗
            xStart = this.Width * edgeWidthK;
            yStart = this.Height * edgeWidthK + 25;
            this.bottleAllUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);
            this.bottleAllUC1.Size = new System.Drawing.Size((int)(this.Width * (1 - edgeWidthK * 2)), (int)(this.Height * bottle_Height_K));

            //滚动条和混匀机
            yStart = yStart + bottleAllUC1.Height;
            panel1.Location = new System.Drawing.Point((int)xStart, (int)yStart);
            panel1.Size = new System.Drawing.Size((int)(this.Width * (1 - edgeWidthK * 2)), (int)(this.Height * 0.15f));


            pipeLine_Right1.Width = (int)(this.Width * 0.12f);
            blendingUC1.Width = (int)(this.Width * 0.19f);
            pipeLine_Text1.Width = (int)(this.Width * 0.12f);
            blendingUC2.Width = (int)(this.Width * 0.19f);
            pipeLine_Two1.Width = (int)(this.Width * 0.2f);

            this.pipeLine_Two1.Location = new System.Drawing.Point((int)(this.Width * 0.185f), (int)(this.Height * 0.025f));


            //布料器
            buLiaoQiUC1.Width = (int)(this.Width * 0.065f);
            buLiaoQiUC1.Height = (int)(this.Height * 0.05f);
            xStart = this.Width * 0.196f;// - buLiaoQiUC1.Width / 2;
            yStart = yStart + panel1.Height;
            this.buLiaoQiUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);

            //混合料槽和圆棍
            hunHeLiaoCaoYuanGunUC1.Width = (int)(this.Width * 0.15f);
            hunHeLiaoCaoYuanGunUC1.Height = (int)(this.Height * 0.11f);
            //xStart = this.Width * 0.2536231884f;
            yStart = yStart + buLiaoQiUC1.Height;
            this.hunHeLiaoCaoYuanGunUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);

            //铺底料槽
            puDiLiaoCaoUC1.Width = (int)(this.Width * 0.041f);
            puDiLiaoCaoUC1.Height = (int)(this.Height * 0.07f);
            xStart = xStart - puDiLiaoCaoUC1.Width;
            yStart = yStart + hunHeLiaoCaoYuanGunUC1.Height - puDiLiaoCaoUC1.Height;
            this.puDiLiaoCaoUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);

         
            this.labPuDiLiaoCaoFlow.Location = new System.Drawing.Point(this.puDiLiaoCaoUC1.Location.X - 50, (int)(this.puDiLiaoCaoUC1.Location.Y + this.puDiLiaoCaoUC1.Height * 0.7f));

            //温度和流量器的值的显示
            xStart = xStart + puDiLiaoCaoUC1.Width + hunHeLiaoCaoYuanGunUC1.Width;
            yStart = yStart + puDiLiaoCaoUC1.Height - tempFlowUC1.Height;
            this.tempFlowUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);



            //烟筒
            xStart = this.Width * edgeWidthK * 3;
            yStart = yStart + tempFlowUC1.Height - hunHeLiaoCaoYuanGunUC1.Height - buLiaoQiUC1.Height;
            xStart = this.Width * edgeWidthK * 2;
            this.picYantong.Location = new System.Drawing.Point((int)xStart, (int)(yStart + 5));

            //传送带
            xStart = this.Width * 0.07f;
            yStart = yStart + buLiaoQiUC1.Height + hunHeLiaoCaoYuanGunUC1.Height;
            hostConveyerUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);
            hostConveyerUC1.Size = new System.Drawing.Size((int)(this.Width * 0.8f), (int)(this.Height * 0.1f));



            xStart = this.Width * 0.87f;
            //右边的箭头
            int widthJt = (int)(this.Height * 0.02f);
            yStart = yStart + hostConveyerUC1.Height / 2 - widthJt / 2;
            GraphicsPath path = new GraphicsPath();

            //图像绘制的坐标点 齿轮旁边的箭头
            PointF[] points = new PointF[8];
            points[0] = new PointF(xStart, yStart);
            points[1] = new PointF(xStart, yStart + widthJt);
            points[2] = new PointF(xStart + this.Width * 0.08f, yStart + widthJt);
            points[3] = new PointF(xStart + this.Width * 0.08f, yStart + widthJt + this.Height * 0.08f);
            points[4] = new PointF(xStart + this.Width * 0.08f + widthJt / 2, yStart + widthJt + this.Height * 0.08f + this.Height * 0.02f);
            points[5] = new PointF(xStart + this.Width * 0.08f + widthJt, yStart + widthJt + this.Height * 0.08f);
            points[6] = new PointF(xStart + this.Width * 0.08f + widthJt, yStart);
            points[7] = new PointF(xStart, yStart);
            path.AddPolygon(points);
           
            using (Brush brush2 = new SolidBrush(Color.FromArgb(0xcb, 0xcb, 0xcb)))
            {
                graphics.FillPath(brush2, path);
            }

            //齿轮
            yStart = hostConveyerUC1.Location.Y + hostConveyerUC1.Height / 2 + widthJt;
            gearUC1.Size = new System.Drawing.Size((int)(this.Height * 0.1f), (int)(this.Height * 0.1f));
            gearUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);

            //除尘器
            xStart = this.Width * 0.07f + hostConveyerUC1.Width / 16;
            yStart = hostConveyerUC1.Location.Y + hostConveyerUC1.Height;
            removeDustUC1.Size = new System.Drawing.Size((int)(this.Width * 0.7f), (int)(this.Height * 0.1f));
            removeDustUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);

            //脱硫脱硝桶
            xStart = this.Width * edgeWidthK * 3 + (float)myBitmapYt.Width / 2;
            yStart = removeDustUC1.Location.Y + removeDustUC1.Height - (int)(this.Height * 0.02f);
            graphics.DrawImage(myBitmapTlTx, xStart, yStart, myBitmapTlTx.Width, myBitmapTlTx.Height);



            shaiZiUC1.Width = (int)(this.Height * 0.18f);
            shaiZiUC1.Height = (int)(this.Height * 0.18f);
            xStart = this.Width / 2 - shaiZiUC1.Width / 2;
            yStart = removeDustUC1.Location.Y + removeDustUC1.Height - (int)(removeDustUC1.Height * 0.23f);
            shaiZiUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);
           
            this.rbtnQuYangDian1.Location = new System.Drawing.Point((int)(xStart - 35), (int)(yStart + shaiZiUC1.Height * 0.6f));
            //环冷机
            huanLengJiUC1.Width = (int)(this.Width * 0.29f);
            huanLengJiUC1.Height = (int)(this.Height * 0.19f);
            xStart = shaiZiUC1.Location.X + shaiZiUC1.Width + this.Width * 0.18f;
            yStart = removeDustUC1.Location.Y + removeDustUC1.Height;
            huanLengJiUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);


            //绘制箭头 左边的筛子的
            AdjustableArrowCap aac = new AdjustableArrowCap(5, 2);
            Pen pen = new Pen(Color.Black);
            pen.CustomEndCap = aac;

            yStart = shaiZiUC1.Location.Y + (int)(shaiZiUC1.Height * 0.9f);

            //铺底料皮带
            this.pipeLine14.Location = new System.Drawing.Point(10, (int)(yStart) - 5);
            this.pipeLine14.Size = new System.Drawing.Size((int)(this.Width * 0.45f), 15);



            point1 = new Point((int)(this.Width * 0.45f), (int)(yStart));
            point2 = new Point((int)(this.Width * 0.25f), (int)(yStart));
           // graphics.DrawLine(pen, point1, point2);
            var point_2 = new Point((int)(this.Width * 0.45f), (int)(yStart-40));
            graphics.DrawLines(pen, new Point[] { point2, point1, point_2 });

            //SF_2， Z1_1，Z2_2皮带名称位置
            labSF_2.Location = new System.Drawing.Point(this.Width / 3, (int)yStart - labSF_2.Height);
            labZ1_1.Location = new System.Drawing.Point(this.Width / 8, (int)yStart - labZ1_1.Height);
            //labZ2_2.Location = new System.Drawing.Point(0, (int)yStart - labZ2_2.Height * 3);


            point1 = new Point(3, (int)(yStart));
            point2 = new Point(3, (int)(panel1.Location.Y + panel1.Height));
            graphics.DrawLine(pen, point1, point2); //铺底料烟筒左边箭头


            point1 = new Point(3, (int)(panel1.Location.Y + panel1.Height + 3));
            point2 = new Point((int)(this.Width * 0.176f - 3), (int)(panel1.Location.Y + panel1.Height + 3));
            //graphics.DrawLine(pen, point1, point2);
            var point3ttt000 = new Point((int)(this.Width * 0.176f - 3), (int)(panel1.Location.Y + panel1.Height + 3 + this.Height * 0.06));
            graphics.DrawLines(pen, new Point[] { point1, point2, point3ttt000 });//铺底料槽上方箭头

            //S_2皮带位置
            labS_2.Location = new System.Drawing.Point((int)(this.Width * 0.11f / 2 - labS_2.Width / 2), (int)(panel1.Location.Y + panel1.Height - labS_2.Height));


            //右上筛子开始的箭头
            xStart = shaiZiUC1.Location.X + shaiZiUC1.Width + 3;
            Pen pen2 = new Pen(Color.Black);
            //开始的线
            pen2.CustomStartCap = aac;
            point1 = new Point((int)(xStart), (int)(yStart - this.Height * 0.1));
            point2 = new Point((int)(huanLengJiUC1.Location.X - 50), (int)(yStart - this.Height * 0.1));
            var point3ttt = new Point((int)(huanLengJiUC1.Location.X - 50), (int)(huanLengJiUC1.Location.Y + huanLengJiUC1.Height + 10));//环冷机旁边的线
            graphics.DrawLines(pen2, new Point[] { point1, point2, point3ttt });//ok 返矿皮带
            point1 = point2;
            point2 = point3ttt;

            //SF-1皮带
            var point3t1 = new Point((int)(huanLengJiUC1.Location.X + huanLengJiUC1.Width * 0.7f), (int)(huanLengJiUC1.Location.Y + huanLengJiUC1.Height + 5));
            /*this.pipeLine13.Location = point3t1;
            this.pipeLine13.Size = new System.Drawing.Size((int)(huanLengJiUC1.Width * 0.2f), 15);*/

            //Z4-1皮带
            var point3t2 = new Point((int)(huanLengJiUC1.Location.X + huanLengJiUC1.Width * 0.35f), (int)(huanLengJiUC1.Location.Y + huanLengJiUC1.Height + 10));
            this.pipeLine15.Location = point3t2;
            this.pipeLine15.Size = new System.Drawing.Size((int)(huanLengJiUC1.Width * 0.7f), 15);

     
            graphics.DrawImage(myBitmap3, (int)(huanLengJiUC1.Location.X -40), (int)(huanLengJiUC1.Location.Y+25),50,60);
           
            graphics.DrawString("余热发电", Font, Brushes.Black, new Rectangle((int)(huanLengJiUC1.Location.X - 38), (int)(huanLengJiUC1.Location.Y + 40), 30, 30), this.sf);

            //LS1-1皮带
            var point3t3 = new Point((int)(huanLengJiUC1.Location.X - 50), (int)(huanLengJiUC1.Location.Y + huanLengJiUC1.Height + 25));
            this.pipeLine16.Location = point3t3;
            this.pipeLine16.Size = new System.Drawing.Size((int)(huanLengJiUC1.Width * 0.46f), 15);


            //成品皮带
            /*this.pipeLine11.Location = new System.Drawing.Point((int)(this.Width * 0.25f), (int)(this.shaiZiUC1.Height + shaiZiUC1.Location.Y));
            this.pipeLine11.Size = new System.Drawing.Size((int)(this.Width * 0.25f + this.shaiZiUC1.Width / 2), 15);
*/
            //成品皮带
            this.pipeLine12.Location = new System.Drawing.Point(10, (int)(this.shaiZiUC1.Height+10 + shaiZiUC1.Location.Y + 18));
            this.pipeLine12.Size = new System.Drawing.Size((int)(this.Width * 0.5f), 15);


            labSJK1.Location = new System.Drawing.Point(10 + this.pipeLine12.Width / 3, (int)(this.shaiZiUC1.Height + shaiZiUC1.Location.Y));

            //右下筛子开始的箭头
            xStart = shaiZiUC1.Location.X + shaiZiUC1.Width;

            point1 = new Point((int)(xStart), (int)(yStart-10));
            point2 = new Point((int)(xStart + this.Width * 0.06), (int)(yStart-10));
            graphics.DrawLine(pen, point1, point2);//冷返皮带

            //graphics.DrawImage(myBitmap2, (int)(xStart), yStart - myBitmap2.Height / 2, myBitmap2.Width, myBitmap2.Height);
            graphics.DrawString("冷返矿皮带", Font, Brushes.Black, new Rectangle((int)(xStart), (int)(yStart - myBitmap2.Height-10), 80, 15), this.sf);

            //由 Brushes.Gray改为Brushes.Black

            point1 = point2;
            //point2 = new Point((int)(xStart + this.Width * 0.06), (int)(this.Height * 0.98));
            point2 = new Point((int)(xStart + this.Width * 0.06), (int)((int)(huanLengJiUC1.Location.Y + huanLengJiUC1.Height + 5) + 15 * 1.5f));
            graphics.DrawLine(pen, point1, point2);

            point1 = new Point(point2.X, point2.Y + 5);
            point2 = new Point((int)(this.Width - 3), point2.Y + 5);
            graphics.DrawLine(pen, point1, point2);

            //SF_3,Z1_2皮带名称位置
            //labSF_3.Location = new System.Drawing.Point((int)(xStart + this.Width * 0.065), (int)(point2.Y - labSF_3.Height+15));
            //labZ1_2.Location = new System.Drawing.Point(this.Width - labZ1_2.Width, (int)(this.Height / 2));

            point1 = new Point((int)(this.Width - 5), point2.Y);
            point2 = new Point((int)(this.Width - 5), (int)(3));
            graphics.DrawLine(pen, point1, point2);

            point1 = new Point((int)(this.Width - 5), (int)(5));
            point2 = new Point((int)(this.Width * 0.8f), (int)(5));
            point3ttt000 = new Point((int)(this.Width * 0.8f), 30);
            graphics.DrawLines(pen, new Point[] { point1, point2, point3ttt000 });//Z10-1皮带终点


            //Z2_3皮带名称位置
            //labZ2_3.Location = new System.Drawing.Point((int)(this.Width / 8 * 7), 1);



            //取样点
            //由 Brushes.Gray改为Brushes.Black
            //graphics.DrawString("取样点", Font, Brushes.Black, new Rectangle(this.pipeLine11.Location.X + pipeLine11.Width / 5, this.pipeLine12.Location.Y, 60, 15), this.sf);
            //20210130修改
            this.rbtnQuYangDian.Location = new System.Drawing.Point(this.pipeLine12.Location.X- pipeLine12.Width / 10, this.pipeLine12.Location.Y+18);

            if (this.pipeLine12.ChengZhi != "")
                graphics.DrawString("称值" + this.pipeLine12.ChengZhi+"t/h", Font, Brushes.Black, new Rectangle(this.pipeLine12.Location.X + pipeLine12.Width / 3, this.pipeLine12.Location.Y, (int)(this.Width * 0.2), this.Height), sf);


            //if (this.Height > (this.pipeLine12.Location.Y + this.pipeLine12.Height + 20))
            {
                //quyangTime = "取样时间：2019.01.15 11:01";
                ////graphics.DrawString("取样时间：2019.01.15 11:01", Font, Brushes.Gray, new Rectangle(this.pipeLine11.Location.X + 15, this.pipeLine12.Location.Y + 15, 180, 15), this.sf);
                //graphics.DrawString(quyangTime, Font, Brushes.Gray, new Rectangle(this.pipeLine11.Location.X + 15, this.Height - 25, 180, 15), this.sf);

                //labQuyangTime.Location
                labQuyangTime1.Location = new System.Drawing.Point(this.pipeLine12.Location.X + pipeLine12.Width / 2, this.pipeLine12.Location.Y - 40);
                
                labQuyangTime.Location = new System.Drawing.Point(this.pipeLine12.Location.X+8 , this.pipeLine12.Location.Y+22);
                this.TextQuYangTime.Location = new System.Drawing.Point(this.pipeLine12.Location.X + pipeLine12.Width /3, this.pipeLine12.Location.Y + 20);

            }
             //point1 = point2;
            //point2 = new Point((int)(huanLengJiUC1.Location.X + huanLengJiUC1.Width), (int)(huanLengJiUC1.Location.Y + huanLengJiUC1.Height + 10));
            //graphics.DrawLine(pen2, point1, point2);



            //float xxx = (float)myBitmapYt.Width * 3 / 4;
            float xxx = this.Width * edgeWidthK * 3 + (float)myBitmapYt.Width / 2;
            ////烟筒和脱硫脱销的直线
            point1 = new Point((int)(this.Width * edgeWidthK * 3 + myBitmapYt.Width / 3), panel1.Location.Y + panel1.Height + myBitmapYt.Height);
            point2 = new Point((int)(this.Width * edgeWidthK * 3 + myBitmapYt.Width / 3), removeDustUC1.Location.Y + removeDustUC1.Height + myBitmapTlTx.Height - 20 - (int)(this.Height * 0.02f));
            var point3 = new Point((int)(xxx), removeDustUC1.Location.Y + removeDustUC1.Height + myBitmapTlTx.Height - 20 - (int)(this.Height * 0.02f));

            graphics.DrawLines(pen2, new Point[] { point1, point2, point3 });
            point1 = new Point((int)(xxx + myBitmapTlTx.Width + 3), removeDustUC1.Location.Y + removeDustUC1.Height + myBitmapTlTx.Height - 20 - (int)(this.Height * 0.02f));

            point2 = new Point((int)(xxx + myBitmapTlTx.Width + 3), (int)(removeDustUC1.Location.Y + removeDustUC1.Height * 0.22f));
            point3 = new Point((int)(removeDustUC1.Location.X), (int)(removeDustUC1.Location.Y + removeDustUC1.Height * 0.22f));

            graphics.DrawLines(pen2, new Point[] { point1, point2, point3 });


            point1 = new Point((int)(xxx + myBitmapTlTx.Width + 3), removeDustUC1.Location.Y + removeDustUC1.Height + myBitmapTlTx.Height - 6 - (int)(this.Height * 0.02f));

            point2 = new Point((int)(removeDustUC1.Location.X - 2), removeDustUC1.Location.Y + removeDustUC1.Height + myBitmapTlTx.Height - 6 - (int)(this.Height * 0.02f));
            point3 = new Point((int)(removeDustUC1.Location.X - 2), (int)(removeDustUC1.Location.Y + removeDustUC1.Height * 0.7f));

            graphics.DrawLines(pen2, new Point[] { point1, point2, point3 });

            //shaiZiUC1.Visible = true;
            //labelCCH.Location = new System.Drawing.Point((int)(this.Width*0.9f), 8);

        }

        #endregion 界面的方法

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
                InsertUpdate_MC_GUIDE_SAMPLING(false);
                Refresh_MC_GUIDE_SAMPLING_Bind_comboBox1();
                db_sugar.Dispose();
            }
            catch (Exception ex)
            {
               
            }
        }

    
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        DateTime dateTimeWinRef = DateTime.Now;
        /// <summary>
        /// 铺底料槽流量值
        /// </summary>
        static double puDiLiaoCaoFlow = 0;
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

            #region 漏斗信息 17个仓
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

            bottleAllUC1.BottomItems[2].BottleObj.HeadTag = Getwlbm_Code( 3);
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

            bottleAllUC1.BottomItems[4].BottleObj.HeadTag = Getwlbm_Code( 5);
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

            bottleAllUC1.BottomItems[14].BottleObj.HeadTag = Getwlbm_Code( 15);
            bottleAllUC1.BottomItems[14].BottleObj.BottleTag = modelT_PLC_3S.T_W_15_3S.ToString("f2");
            bottleAllUC1.BottomItems[14].BottleObj.Value = getbottleValue(modelT_PLC_3S.T_W_15_3S) / GetByShangXian_Code(15) * 100;//500 * 100;
            bottleAllUC1.BottomItems[14].BottleObj.SetValue = modelT_PLC_3S.T_SP_W_15_3S.ToString();
            bottleAllUC1.BottomItems[14].BottleObj.CurrentValue = modelT_PLC_3S.T_ACTUAL_W_15_3S.ToString();
            bottleAllUC1.BottomItems[14].BottleObj.SetT_SL_Left = modelT_PLC_3S.T_SL_15_3S == 1 ? Brushes.Green : Brushes.DimGray;

            bottleAllUC1.BottomItems[15].BottleObj.HeadTag = Getwlbm_Code( 16);
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

            //1H1-1皮带 配料总皮带启停信号
            bottleAllUC1.SetIsRunStop(modelT_PLC_3S.T_BELT_SL_P_7_3S == 1 ? true : false);
            bottleAllUC1.InvalidateNew();
            ////bottleAllUC1.Refresh();

            //右边的皮带 皮带1H-1 一混前皮带启停信号
            pipeLine_Right1.SetIsRunStop(modelT_PLC_3S.T_1M_PRE_BELT_B_E_3S == 1 ? true : false);
            pipeLine_Right1.ChengZhi = getbottleValue(modelT_PLC_3S.T_1M_PRE_BELT_W_1H_1_3S).ToString("f2");

            //一混电机设备
            blendingUC1.IsRun = modelT_PLC_3S.T_1M_SL_3S ==1 ? true : false ;
            blendingUC1.ShuiFen = getbottleValue(modelT_PLC_3S.T_PLC_1M_WATER_SP_3S).ToString();//一混目标水分
            blendingUC1.ZhuanSu = getbottleValue(modelT_PLC_3S.T_1M_MIXER_RATE_3S).ToString();///转速
            blendingUC1.SetJsl = getbottleValue(modelT_PLC_3S.T_1M_FT_SP_3S).ToString();//加水设定值
            blendingUC1.ReadJsl = getbottleValue(modelT_PLC_3S.T_1M_FT_PV_3S).ToString();//加水反馈值
            blendingUC1.TitleIndex = 0;
            blendingUC1.InvalidateNew();

            //一混后皮带 1H3
            pipeLine_Text1.SetIsRunStop(modelT_PLC_3S.T_1M_NEX_BELT_B_E_Z2_1_3S == 1?true:false);//设备启停
            pipeLine_Text1.ShuiFen = getbottleValue(modelT_PLC_3S.T_1M_NEX_WATER_AVG_3S).ToString();//一混后混合料水分检测
            pipeLine_Text1.ChengZhi = getbottleValue(modelT_PLC_3S.T_1M_NEX_BELT_W_1H2_1_3S).ToString("f2");//后皮带称值

            pipeLine_Text1.InvalidateNew();
           
            //一混后皮带 2H1
            pipeLine_Text1.SetIsRunStop2(modelT_PLC_3S.T_1M_NEX_BELT_W_1H2_1_3S == 1 ? true : false);//设备启停
            pipeLine_Text1.ChengZhi2 = getbottleValue(modelT_PLC_3S.T_1M_NEX_BELT_W_1H2_1_3S).ToString("f2");//后皮带称值
            pipeLine_Text1.InvalidateNew();

            //20210130修改 2H2
            pipeLine_Text1.SetIsRunStop3(modelT_PLC_3S.T_1M_NEX_BELT_W_1H2_1_3S == 1 ? true : false);//设备启停
            pipeLine_Text1.ChengZhi2 = getbottleValue(modelT_PLC_3S.T_1M_NEX_BELT_W_1H2_1_3S).ToString("f2");//后皮带称值
            pipeLine_Text1.InvalidateNew();

            //二混电机
            blendingUC2.IsRun = modelT_PLC_3S.T_2M_SL_3S == 1 ? true : false;
           // blendingUC2.ShuiFen = getbottleValue(modelT_PLC_3S.M_PLC_2M_WATER_SP).ToString();
            blendingUC2.ZhuanSu = getbottleValue(modelT_PLC_3S.T_2M_MIXER_RATE_3S).ToString();
            blendingUC2.SetJsl = getbottleValue(modelT_PLC_3S.T_2M_FLOW_SP_3S).ToString();
            blendingUC2.ReadJsl = getbottleValue(modelT_PLC_3S.T_2M_FLOW_PV_3S).ToString();
            blendingUC2.ChengZhi= getbottleValue(modelT_PLC_3S.T_2M_BELT_VALUE_3S).ToString();
            blendingUC2.TitleIndex = 1;
            blendingUC2.InvalidateNew();

            //二混后皮带  "Z41(混-4)  S1(混-5)  SBL(梭式布料器皮带)";
            pipeLine_Two1.SetIsRunStop(modelT_PLC_3S.T_2M_NEX_BELT_1H2_2_S_3S == 1 ? true : false);
            pipeLine_Two1.SetIsRunStop2(modelT_PLC_3S.T_2M_NEX_BELT_Z3_1_S_3S == 1 ? true : false);
            pipeLine_Two1.SetIsRunStop3(modelT_PLC_3S.T_2M_NEX_BELT_Z15_1_S_3S == 1 ? true : false);
            //pipeLine_Two1.SetIsRunStop4(modelT_PLC_3S.T_IN_SK_S_3S == 1 ? true : false);

            pipeLine_Two1.ShuiFen = getbottleValue(modelT_PLC_3S.T_2M_NEX_WATER_AVG_3S).ToString();
            pipeLine_Two1.ChengZhi = getbottleValue(modelT_PLC_3S.T_2M_BELT_VALUE_3S).ToString("f2");
            pipeLine_Two1.InvalidateNew();

       


            //混合料槽和圆棍
            //hunHeLiaoCaoYuanGunUC1.SetZhuanSu = getbottleValue(modelT_PLC_3S.C_N_STICK_A_PV_3S).ToString("f2"); //getbottleValue(modelT_PLC_3S.T_STICK_SP_3S).ToString();
            hunHeLiaoCaoYuanGunUC1.ReadZhuanSu = getbottleValue(modelT_PLC_3S.T_STICK_PV_3S).ToString("f2");
            hunHeLiaoCaoYuanGunUC1.SylValue = modelT_PLC_3S.T_BLEND_LEVEL_3S;//混合料仓仓位
            hunHeLiaoCaoYuanGunUC1.HunHeLiaoWenDu = getbottleValue(modelT_PLC_3S.F_PLC_BLEND_TE).ToString("f2");//混合料温度检测值
            hunHeLiaoCaoYuanGunUC1.InvalidateNew();

            //铺底料槽
            puDiLiaoCaoUC1.SylValue = (modelT_PLC_3S.T_BED_MAT_W_3S);//铺底料仓称重
            puDiLiaoCaoUC1.InvalidateNew();

            ////温度和流量器的值的显示
            //tempFlowUC1.TagTemp = getbottleValue(modelT_PLC_3S.T_AIM_TE_3S).ToString();
            //tempFlowUC1.FireTemp = getbottleValue((modelT_PLC_3S.T_IG_02_TE_3S+ modelT_PLC_3S.T_IG_03_TE_3S)/2).ToString();
            tempFlowUC1.TagTemp = getbottleValue(modelT_PLC_3S.T_IG_02_TE_3S).ToString();
            tempFlowUC1.FireTemp = getbottleValue(modelT_PLC_3S.T_IG_03_TE_3S).ToString();
            tempFlowUC1.CoalGasFlow = getbottleValue(modelT_PLC_3S.T_IG_GAS_PV_3S).ToString();
            tempFlowUC1.AirFlow = getbottleValue(modelT_PLC_3S.T_IG_AIR_PV_3S).ToString();
           // tempFlowUC1.T_IG_NATURAL_PV_3S = getbottleValue(modelT_PLC_3S.T_IG_NATURAL_PV_3S).ToString();
            tempFlowUC1.InvalidateNew();


            ////布料器和点火炉
            //buLiaoAndLiaoCaoUC1.SetZhuanSu = getbottleValue(modelT_PLC_3S.T_STICK_SP_3S).ToString();
            //buLiaoAndLiaoCaoUC1.ReadZhuanSu = getbottleValue(modelT_PLC_3S.T_STICK_PV_3S).ToString();
            //buLiaoAndLiaoCaoUC1.SetLc1 = getbottleValue(modelT_PLC_3S.T_BED_MATERAL_W_3S).ToString();//铺底料仓称重
            //buLiaoAndLiaoCaoUC1.SetLc2 = getbottleValue(modelT_PLC_3S.T_BLEND_LEVEL_3S).ToString();//混合料仓仓位

            //buLiaoAndLiaoCaoUC1.TagTemp = getbottleValue(modelT_PLC_3S.T_AIM_TE_3S).ToString();
            //buLiaoAndLiaoCaoUC1.FireTemp = getbottleValue(modelT_PLC_3S.T_IG_01_TE_3S).ToString();
            //buLiaoAndLiaoCaoUC1.CoalGasFlow = getbottleValue(modelT_PLC_3S.T_IG_GAS_PV_3S).ToString();
            //buLiaoAndLiaoCaoUC1.AirFlow = getbottleValue(modelT_PLC_3S.T_IG_AIR_PV_3S).ToString();
            //buLiaoAndLiaoCaoUC1.InvalidateNew();

            //烧结机启停信号
            if (modelT_PLC_3S.T_SIN_SL_3S == 1 ? true : false)
            {
                hostConveyerUC1.IsRun();
            }
            else
            {
                hostConveyerUC1.IsStop();
            }
            if (modelT_PLC_3S.T_SIN_MS_PV_3S < 0.5f)
            {
                hostConveyerUC1.IsStop();
            }
           hostConveyerUC1.HText = string.Format("{0}布料厚度:{1}mm;       实际机速:{2}m/min;       BTP位置:{3}m;    BTP温度:{4}℃;", "", modelT_PLC_3S.C_THICK_PV_3S, modelT_PLC_3S.T_SIN_MS_PV_3S, MICAL_BU_C_LOCAT_BTP, MICAL_BU_C_BTP_TE);
         

            //主抽1风机
            removeDustUC1.IsRun = modelT_PLC_3S.T_FAN_1_SL_3S == 1 ? true : false;
            removeDustUC1.IsRun2 = modelT_PLC_3S.T_FAN_2_SL_3S == 1 ? true : false;
            removeDustUC1.FlowValue = string.Format("流量{0}m³/h", getbottleValue(modelT_PLC_3S.T_MA_SB_1_FLUE_FT_3S));
            removeDustUC1.TempValue = string.Format("温度{0}℃", getbottleValue(modelT_PLC_3S.T_MA_SB_1_FLUE_TE_3S));
            removeDustUC1.KPaValue = string.Format("负压{0}KPa", getbottleValue(modelT_PLC_3S.T_MA_SB_1_FLUE_PT_3S));

            removeDustUC1.FlowValue2 = string.Format("流量{0}m³/h", getbottleValue(modelT_PLC_3S.T_MA_SB_2_FLUE_FT_3S));
            removeDustUC1.TempValue2 = string.Format("温度{0}℃", getbottleValue(modelT_PLC_3S.T_MA_SB_2_FLUE_TE_3S));
            removeDustUC1.KPaValue2 = string.Format("负压{0}KPa", getbottleValue(modelT_PLC_3S.T_MA_SB_2_FLUE_PT_3S));
            removeDustUC1.InvalidateNew();

            //单辊破碎机启停信号
            gearUC1.IsRun = modelT_PLC_3S.T_BM_SL_3S == 1 ? true : false;

            //环冷机
            huanLengJiUC1.IsRunHlj = modelT_PLC_3S.T_RC_SL_3S == 1 ? true : false;
            huanLengJiUC1.RkTemp = getbottleValue(modelT_PLC_3S.T_RC_IN_TE_3S).ToString("f0");
            huanLengJiUC1.CkTemp = getbottleValue(modelT_PLC_3S.T_RC_OUT_TE_3S).ToString("f1");

            huanLengJiUC1.Hlj_Set_SPEED = getbottleValue(modelT_PLC_3S.T_RC_SPEED_SP_3S).ToString();
            huanLengJiUC1.Hlj_Read_SPEED = getbottleValue(modelT_PLC_3S.T_RC_SPEED_PV_3S).ToString();
            huanLengJiUC1.BsGkj_Set_SPEED = getbottleValue(modelT_PLC_3S.T_PF_SPEED_SP_3S).ToString("f2");
            huanLengJiUC1.BsGkj_Read_SPEED = getbottleValue(modelT_PLC_3S.T_PF_SPEED_PV_3S).ToString("f2");

            huanLengJiUC1.IsRunBsGkj = modelT_PLC_3S.T_PF_SL_3S == 1 ? true : false;
            huanLengJiUC1.SetFengJi(modelT_PLC_3S.T_RC_B_S_1_3S == 1 ? true : false, modelT_PLC_3S.T_RC_B_S_2_3S == 1 ? true : false, modelT_PLC_3S.T_RC_B_S_3_3S == 1 ? true : false, modelT_PLC_3S.T_RC_B_S_4_3S == 1 ? true : false, modelT_PLC_3S.T_RC_B_S_5_3S == 1 ? true : false);
            huanLengJiUC1.BsGkj_Cw = modelT_PLC_3S.T_PF_LEVEL_3S.ToString();
            huanLengJiUC1.RefreshInvalidate();
            //板式给矿机启停
            /*if (modelT_PLC_3S.T_PF_SL_3S == 1 ? true : false)//2.5f
            {
                pipeLine13.MoveSpeed = -2.5f;
            }
            else
            {
                pipeLine13.MoveSpeed = 0f;
            }*/
            //进料筛皮带启停信号（Z51（成-1））
            if (modelT_PLC_3S.T_MS_IN_SF_1_SL_3S == 1 ? true : false)//2.5f
            {
                pipeLine15.MoveSpeed = -2.5f;
            }
            else
            {
                pipeLine15.MoveSpeed = 0f;
            }

            //LSG启停信号
            if (modelT_PLC_3S.T_PF_SL_3S == 1 ? true : false)//2.5f
            {
                pipeLine16.MoveSpeed = -2.5f;
            }
            else
            {
                pipeLine16.MoveSpeed = 0f;
            }


            //成品矿皮带启停信号（Z5-1皮带）
            /*if (modelT_PLC_3S.T_FP_BELT1_SL_3S == 1)
            {
                pipeLine11.MoveSpeed = -2.5f;

            }
            else
            {
                pipeLine11.MoveSpeed = 0f;
            }
            pipeLine11.ChengZhi = getbottleValue(modelT_PLC_3S.T_PROD_DELT1_FQ_3S).ToString("f0");*/


            //3#成品皮带启停信号（CP1（成-5））
            if (modelT_PLC_3S.T_FP_BELT3_SL_3S == 1)
            {
                pipeLine12.MoveSpeed = -2.5f;
            }
            else
            {
                pipeLine12.MoveSpeed = 0f;
            }

            //铺底料皮带
            if (modelT_PLC_3S.T_BED_MAT_1_SL_3S == 1)
            {
                pipeLine14.MoveSpeed = -2.5f;
            }
            else
            {
                pipeLine14.MoveSpeed = 0f;
            }


            //冷返矿皮带启停信号
           /* if (modelT_PLC_3S.T_COLD_AO_SF1_SL_3S==1)
            {
                labSF_3.ForeColor = Color.Green;
            }
            else
            {
                labSF_3.ForeColor = Color.Black;
            }*/

            
            
            if (modelT_PLC_3S.T_SCREEN_SL_1_3S == 1 ? true : false)
            {
                shaiZiUC1.IsRun = modelT_PLC_3S.T_SCREEN_SL_1_3S == 1 ? true : false;//筛一启停信号
                shaiZiUC1.IsRun2 = modelT_PLC_3S.T_SCREEN_SL_2_3S == 1 ? true : false;
                shaiZiUC1.IsRun3 = modelT_PLC_3S.T_SCREEN_SL_3_3S == 1 ? true : false;
            }
            else
            {
                shaiZiUC1.IsRun = modelT_PLC_3S.T_SCREEN_SL_1_B_3S == 1 ? true : false;//北筛一启停信号
                shaiZiUC1.IsRun2 = modelT_PLC_3S.T_SCREEN_SL_2_B_3S == 1 ? true : false;
                shaiZiUC1.IsRun3 = modelT_PLC_3S.T_SCREEN_SL_3_B_3S == 1 ? true : false;
            }
            shaiZiUC1.InvalidateNew();


            //puDiLiaoCaoFlow = modelT_PLC_3S.M_BED_MATERIAL_FT;

            //labPuDiLiaoCaoFlow.Text = puDiLiaoCaoFlow.ToString("f1") + "t/h";
            ////铺底料槽流量值
            //graphics.DrawString(puDiLiaoCaoFlow.ToString("f1") + "t/h", Font, Brushes.Black, new Rectangle(this.puDiLiaoCaoUC1.Location.X - 50, (int)(this.puDiLiaoCaoUC1.Location.Y + this.puDiLiaoCaoUC1.Height * 0.7f), 200, (int)(this.puDiLiaoCaoUC1.Height * 0.5f)), this.sf);

            //this.Invalidate();

            //abSJK1.Text = string.Format("秤值:{0}t/h", modelT_PLC_3S.T_FP_BELT_FT_3S);
            #endregion 漏斗信息

            //this.labelCCH.Text = "除尘灰加水量："+ GetCCHJSL()+"t/h";
        }
        //修改20200726
        double GetCCHJSL()
        {
            double va = 0;
            string str = "select  top(1) M_PLC_DUST_FT_PV from C_MFI_PLC_1MIN order by timestamp desc";
            va = db_sugar.Ado.GetDouble(str);
            return va;
        }
        /// <summary>
        /// 获取漏斗的值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        double getbottleValue(object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Math.Round(Convert.ToDouble(obj),2);
            }
        }
        private void btnHuanXinLiao_Click(object sender, EventArgs e)
        {
            //if (!ValidatingPrivilegeByGuidID("MAINUSERCONTROL_BTNHUANXINLIAO"))
            //{ return; }
            InsertUpdate_MC_GUIDE_SAMPLING(true);
        }
        private void Refresh_MC_GUIDE_SAMPLING_Bind_comboBox1()
        {
            

                string strSQL0 = "select top(1) * from MC_MICAL_GUIDE_SAMPLE order by start_time desc";
            try
            {
                mc_GUIDE_SAMPLING = db_sugar.SqlQueryable<MC_MICAL_GUIDE_SAMPLE>(strSQL0).ToList().FirstOrDefault();

            }
            catch (Exception ee)
            {
                LogHelper.LogError(ee.Message);
            }
                if (mc_GUIDE_SAMPLING == null)
                {
                    db_sugar.Dispose();
                    return;
                }
                if (mc_GUIDE_SAMPLING.START_TIME == null || mc_GUIDE_SAMPLING.SAMPLE_TIME_1 == null || mc_GUIDE_SAMPLING.SAMPLE_TIME_0 == null)
                {
                    db_sugar.Dispose();
                    return;
                }
                aaa ShowInfo = delegate ()
                {
                    //Bind_comboBox1(mc_GUIDE_SAMPLING);
                    
                    labQuyangTime.Text = "取样2时间：" + mc_GUIDE_SAMPLING.SAMPLE_TIME_1.ToString();
                    labQuyangTime1.Text = "取样1时间：" + mc_GUIDE_SAMPLING.SAMPLE_TIME_0.ToString();
                    if (mc_GUIDE_SAMPLING.SAMPLE_TIME_1 <= DateTime.Now)
                    {
                        labQuyangTime.Text = "";
                    }
                    if (mc_GUIDE_SAMPLING.SAMPLE_TIME_0 <= DateTime.Now)
                    {
                        labQuyangTime1.Text = "";
                    }
                };
                this.Invoke(ShowInfo);
                db_sugar.Dispose();
            
        }
        int jiaozhunQyz = 0;
        /// <summary>
        /// 取样点时间
        /// </summary>
        MC_MICAL_GUIDE_SAMPLE mc_GUIDE_SAMPLING = null;
        private void InsertUpdate_MC_GUIDE_SAMPLING(bool IsInsert)
        {
            if (IsInsert)
            {
                bool flag = false;
                aaa ShowInfo = delegate ()
                {
                    btnHuanXinLiao.Enabled = false;
                    DialogResult dr = MessageBox.Show("确认换新料吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        //点确定的代码
                        flag = true;
                    }
                    else
                    {
                        //点取消的代码 
                        btnHuanXinLiao.Enabled = true;
                    }
                };
                this.Invoke(ShowInfo);

                if (!flag)
                {
                    return;
                }
            }
           
                if (!IsInsert)
                {
                    if (mc_GUIDE_SAMPLING == null)
                    {
                    string strSQL0 = "select top(1) *  from MC_MICAL_GUIDE_SAMPLE order by start_time desc";
                    try
                    {
                        mc_GUIDE_SAMPLING = db_sugar.SqlQueryable<MC_MICAL_GUIDE_SAMPLE>(strSQL0).ToList().FirstOrDefault();
                    }
                    catch (Exception ee)
                    {
                        LogHelper.LogError(ee.Message);
                    }

                    }
                    if (mc_GUIDE_SAMPLING == null)
                    {
                        return;
                    }
                    if (mc_GUIDE_SAMPLING.START_TIME == null || mc_GUIDE_SAMPLING.SAMPLE_TIME_1 == null)
                    {
                        return;
                    }
                }

            modelMC_MICAL_PAR = db_sugar.Queryable<MC_MICAL_PAR>().ToList().FirstOrDefault();
            if (modelMC_MICAL_PAR == null)
            {
                return;
            }
             float MICAL_SAM_MAT_TIME = 0;
            float MICAL_SAM_SCR_TIME = 0;
           string strSQL = "select  avg(case when MICAL_SAM_MAT_TIME<>0 then MICAL_SAM_MAT_TIME else null end) as MICAL_SAM_MAT_TIME,  avg(case when MICAL_SAM_SCR_TIME<>0 then MICAL_SAM_SCR_TIME else null end) as MICAL_SAM_SCR_TIME  from MC_MICAL_RESULT where DATANUM =14 and timestamp>dateadd(minute,-" + modelMC_MICAL_PAR.PAR_T2+ ",getdate())";
            try
            {
                var dt=db_sugar.Ado.GetDataTable(strSQL);
                if (dt != null)
                {
                    float.TryParse(dt.Rows[0]["MICAL_SAM_MAT_TIME"].ToString(),out MICAL_SAM_MAT_TIME);
                    float.TryParse(dt.Rows[0]["MICAL_SAM_SCR_TIME"].ToString(),out MICAL_SAM_SCR_TIME);
                }
               
            }
            catch (Exception ee)
            {
                LogHelper.LogError(ee.Message);
            }
          
                  
                 
                    if (IsInsert)
                    {
                        mc_GUIDE_SAMPLING = new MC_MICAL_GUIDE_SAMPLE();
                        mc_GUIDE_SAMPLING.START_TIME = DateTime.Now;


                        mc_GUIDE_SAMPLING.SAMPLE_TIME_1 = (mc_GUIDE_SAMPLING.START_TIME.Value).AddMinutes((double)MICAL_SAM_MAT_TIME);
                        mc_GUIDE_SAMPLING.SAMPLE_TIME_0 = (mc_GUIDE_SAMPLING.START_TIME.Value).AddMinutes((double)MICAL_SAM_MAT_TIME).AddMinutes(-(double)MICAL_SAM_SCR_TIME);


                    jiaozhunQyz = 1;
                    //insert
                     string str = "insert into MC_MICAL_GUIDE_SAMPLE(START_TIME,SAMPLE_TIME_1,SAMPLE_TIME_0) values('" + mc_GUIDE_SAMPLING.START_TIME + "','" + mc_GUIDE_SAMPLING.SAMPLE_TIME_1 + "','" + mc_GUIDE_SAMPLING.SAMPLE_TIME_0 + "')";

                      db_sugar.Ado.ExecuteCommand(str);


                        aaa ShowInfo = delegate ()
                        {
                            //Bind_comboBox1(mc_GUIDE_SAMPLING);

                            //quyangTime = "取样时间：" + mc_GUIDE_SAMPLING.SAMPLE_TIME_1.ToString();
                            //labQuyangTime.Text = quyangTime;
                            labQuyangTime.Text = "取样2时间：" + mc_GUIDE_SAMPLING.SAMPLE_TIME_1.ToString();
                            labQuyangTime1.Text = "取样1时间：" + mc_GUIDE_SAMPLING.SAMPLE_TIME_0.ToString();
                            MessageBox.Show("换新料成功.");
                            btnHuanXinLiao.Enabled = true;
                        };
                        this.Invoke(ShowInfo);

                    }
                    else
                    {
                        aaa ShowInfo = delegate ()
                        {
                           // Bind_comboBox1(mc_GUIDE_SAMPLING);
                            labQuyangTime.Text = "取样2时间：" + mc_GUIDE_SAMPLING.SAMPLE_TIME_1.ToString();
                            labQuyangTime1.Text = "取样1时间：" + mc_GUIDE_SAMPLING.SAMPLE_TIME_0.ToString();

                        };
                        this.Invoke(ShowInfo);

                     ////当前时间+指导采样校准周期 《= 取样时间  进行更新
                     if (DateTime.Now.AddMinutes(modelMC_MICAL_PAR.PAR_T2) <= mc_GUIDE_SAMPLING.SAMPLE_TIME_0.Value)
                    {
                      //判断时间
                     //当前时间+指导采样校准周期 《= 取样时间  进行更新

                      mc_GUIDE_SAMPLING.SAMPLE_TIME_0 = (mc_GUIDE_SAMPLING.START_TIME.Value).AddMinutes((double)MICAL_SAM_MAT_TIME).AddMinutes(-(double)MICAL_SAM_SCR_TIME);

                     //执行更新操作
                    db_sugar.Updateable<MC_MICAL_GUIDE_SAMPLE>().UpdateColumns(it => mc_GUIDE_SAMPLING.SAMPLE_TIME_0).ExecuteCommand();
                     }


            }

                if (!initQyz)
                {
                    initQyz = true;
                    aaa ShowInfo = delegate ()
                    {
                        //Bind_comboBox1(mc_GUIDE_SAMPLING);
                        labQuyangTime.Text = "取样2时间：" + mc_GUIDE_SAMPLING.SAMPLE_TIME_1.ToString();
                        labQuyangTime1.Text = "取样1时间：" + mc_GUIDE_SAMPLING.SAMPLE_TIME_0.ToString();
                    };
                    this.Invoke(ShowInfo);
                }
            


                db_sugar.Dispose();
            

        }
        bool initQyz = false;



        DateTime[] ArrDateTimes = new DateTime[6];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        private void Bind_comboBox1(MC_MICAL_GUIDE_SAMPLE model)
        {
            this.labQuyangTime.Visible = true;
            this.TextQuYangTime.Text = "";
            if (model != null)
            {
                ArrDateTimes[0] = mc_GUIDE_SAMPLING.SAMPLE_TIME_1.Value;
               
                int num = 0;
                foreach (var item in ArrDateTimes)
                {
                    //this.comboBox1.Items.Add(item.ToString());
                    if (item < DateTime.Now)
                    {
                        num++;
                    }
                }
                if (num < 6)
                {
                   
                    this.TextQuYangTime.Text = ArrDateTimes[0].ToString();
                   
                }
                else
                {
                    //this.comboBox1.Items.Add(ArrDateTimes[5].ToString());
                    ////this.comboBox1.SelectedIndex = 5;
                    //this.comboBox1.SelectedIndex = 0;
                }
            }


          

        }


        private void blendingUC1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //一混
            using (FormDaoTui frm = new FormDaoTui())
            {
                frm.Init(1);
                frm.ShowDialog();
            }

        }

        private void blendingUC2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //二混
            using (FormDaoTui frm = new FormDaoTui())
            {
                frm.Init(2);
                frm.ShowDialog();
            }
        }


        private void hostConveyerUC1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            
            ////烧结机
            //using (FormDaoTui frm = new FormDaoTui())
            //{
            //    frm.Init(9);
            //    frm.ShowDialog();
            //}
        }

        private void shaiZiUC1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //筛分
            using (FormDaoTui frm = new FormDaoTui())
            {
                frm.Init(13);
                frm.ShowDialog();
            }
        }

        private void MainUserControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var quyu = new Rectangle(this.pipeLine12.Location.X + pipeLine12.Width / 5, this.pipeLine12.Location.Y, 60, 15);
            //取样点区域
            if (e.X > quyu.X && e.X < (quyu.X + 69) && e.Y > quyu.Y && e.Y < (quyu.Y + 15))
            {
                //using (FormDaoTui frm = new FormDaoTui())
                //{
                //    frm.Init(14);
                //    frm.ShowDialog();
                //}
            }
        }

        DateTime dtimeRbtnQyd = DateTime.Now;
        private void rbtnQuYangDian_Click(object sender, EventArgs e)
        {
            if (dtimeRbtnQyd.AddMilliseconds(1000) > DateTime.Now)
            {
                using (FormDaoTui frm = new FormDaoTui())
                {
                    frm.Init(14);
                    frm.ShowDialog();
                }
            }
            dtimeRbtnQyd = DateTime.Now;

        }
        DateTime dtimeRbtnDouble = DateTime.Now;
        private void rbtnQuYangDian_Click1(object sender, EventArgs e)
        {
            if (dtimeRbtnDouble.AddMilliseconds(1000) > DateTime.Now)
            {
                using (UserControlIndex.FormDaoTui frm = new UserControlIndex.FormDaoTui())
                {
                    frm.Init(13);
                    frm.ShowDialog();
                }
            }
            dtimeRbtnDouble = DateTime.Now;

        }
        public void Timer_state()
        {
            MongoQMtimer1.Enabled = true;
        }
        public void Timer_stop()
        {
            MongoQMtimer1.Enabled = false;
           
        }

        public void _Clear()
        {
            MongoQMtimer1.Enabled = false;
            MongoQMtimer1.Close();
        }

        private void labelCCH_Click(object sender, EventArgs e)
        {

        }
    }
}

