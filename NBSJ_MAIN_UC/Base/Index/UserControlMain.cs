using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using NBSJ_MAIN_UC.Model;


namespace UserControlIndex
{
    public partial class UserControlMain : UserControl
    {
        public UserControlMain()
        {
            InitializeComponent();


            //pipeLine11 = new PipeLine();
            //this.pipeLine11.Location = new System.Drawing.Point(206, 575);
            //this.pipeLine11.MoveSpeed = 0F;
            //this.pipeLine11.Name = "pipeLine1";
            //this.pipeLine11.Size = new System.Drawing.Size(335, 15);
            //this.pipeLine11.TabIndex = 114;
            pipeLine11 = new PipeLine();
            this.Controls.Add(this.pipeLine11);
            pipeLine12 = new PipeLine();
            this.Controls.Add(this.pipeLine12);
            pipeLine13 = new PipeLine();
            this.Controls.Add(this.pipeLine13);

            pipeLine11.PipeLineActive = true;
            pipeLine12.PipeLineActive = true;
            pipeLine13.PipeLineActive = true;

            pipeLine11.MoveSpeed = -2.5f;
            pipeLine12.MoveSpeed = -2.5f;
            pipeLine13.MoveSpeed = -2.5f;

            pipeLine11.PipeLineName = "P-8皮带";
            pipeLine12.PipeLineName = "SJK-1皮带";
            pipeLine13.PipeLineName = "SF-1皮带";



            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);


            this.BackColor = Color.FromArgb(0xcb, 0xe3, 0xf8);//cbe3f8


            btnHuanXinLiao.Location = new System.Drawing.Point(100, 1);

            myBitmap1 =global::NBSJ_MAIN_UC.Properties.Resources.皮带点;
            myBitmap2 =global::NBSJ_MAIN_UC.Properties.Resources.皮带点;
        }

        C_PLC_3S modelT_PLC_3S = null;
        //IT_PLC_3SBll getT_PLC_3S = null;

        Bitmap myBitmap1 = null;
        Bitmap myBitmap2 = null;
        private void UserControlMain_Load(object sender, EventArgs e)
        {

            pipeLine_Right1.PipeLineActive(true);
            pipeLine_Right1.SetIsRunStop(false);
            pipeLine_Text1.PipeLineActive(true);
            pipeLine_Two1.PipeLineActive(true);


            //getT_PLC_3S = new T_PLC_3SDal();
            //modelT_PLC_3S = getT_PLC_3S.GetT_PLC_3SData("select  * from T_PLC_3S order by TIMESTAMP desc");

            InitControl();
            //windowformRefresh();

            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);

            //worker.WorkerSupportsCancellation = true;
            //if (!worker.IsBusy)
            //{
            //    worker.RunWorkerAsync();
            //}
        }
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

            for (int i = 0; i < 7; i++)
            {
                bottleAllUC1.BottomItems.Add(new BottleItem { BottleType = BottleType.BottleSingle });
                bottleAllUC1.BottomItems[i].BottleObj.Value = (i) * 10;
            }
            for (int i = 7; i < 11; i++)
            {
                bottleAllUC1.BottomItems.Add(new BottleItem { BottleType = BottleType.BootleDouble });
                bottleAllUC1.BottomItems[i].BottleObj.Value = (i + 1) * 10;
            }
            for (int i = 11; i < 15; i++)
            {
                bottleAllUC1.BottomItems.Add(new BottleItem { BottleType = BottleType.BottleSingle });
                bottleAllUC1.BottomItems[i].BottleObj.Value = (i + 1) * 10;
            }
            bottleAllUC1.PipeLineActive(false);

            panel1.Dock = System.Windows.Forms.DockStyle.None;

            myBitmapYt =global::NBSJ_MAIN_UC.Properties.Resources.烟筒;
            myBitmapTlTx =global::NBSJ_MAIN_UC.Properties.Resources.脱硫脱硝桶;

            RefreshFun(this.CreateGraphics());

        }

        protected LinearGradientBrush brush;
        protected ColorBlend blend = new ColorBlend();
        private void RefreshFun(Graphics graphics)
        {

            //漏斗
            xStart = this.Width * edgeWidthK;
            yStart = this.Height * edgeWidthK + 25;
            this.bottleAllUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);
            this.bottleAllUC1.Size = new System.Drawing.Size((int)(this.Width * (1 - edgeWidthK * 2)), (int)(this.Height * bottle_Height_K));

            //滚动条和混匀机
            yStart = yStart + bottleAllUC1.Height;
            panel1.Location = new System.Drawing.Point((int)xStart, (int)yStart);
            panel1.Size = new System.Drawing.Size((int)(this.Width * (1 - edgeWidthK * 2)), (int)(this.Height * 0.16f));


            pipeLine_Right1.Width = (int)(this.Width * 0.15f);
            blendingUC1.Width = (int)(this.Width * 0.15f);
            pipeLine_Text1.Width = (int)(this.Width * 0.15f);
            blendingUC2.Width = (int)(this.Width * 0.15f);
            pipeLine_Two1.Width = (int)(this.Width * 0.25f);


            //布料和料仓
            buLiaoAndLiaoCaoUC1.Width = (int)(this.Width * 0.22f);
            buLiaoAndLiaoCaoUC1.Height = (int)(this.Height * 0.14f);
            xStart = this.Width * 0.24f - buLiaoAndLiaoCaoUC1.Width / 2;
            yStart = yStart + panel1.Height;
            this.buLiaoAndLiaoCaoUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);


            //烟筒
            xStart = this.Width * edgeWidthK * 3;
            //yStart = this.Height * edgeWidthK + bottleAllUC1.Height + panel1.Height;
            graphics.DrawImage(myBitmapYt, xStart, yStart, myBitmapYt.Width, myBitmapYt.Height);


            //传送带
            xStart = this.Width * 0.07f;
            yStart = yStart + buLiaoAndLiaoCaoUC1.Height;
            hostConveyerUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);
            hostConveyerUC1.Size = new System.Drawing.Size((int)(this.Width * 0.8f), (int)(this.Height * 0.1f));



            xStart = this.Width * 0.87f;
            //右边的箭头
            int widthJt = (int)(this.Height * 0.02f);
            yStart = yStart + hostConveyerUC1.Height / 2 - widthJt / 2;
            GraphicsPath path = new GraphicsPath();

            //图像绘制的坐标点
            PointF[] points = new PointF[8];
            points[0] = new PointF(xStart, yStart);
            points[1] = new PointF(xStart, yStart + widthJt);
            points[2] = new PointF(xStart + this.Width * 0.08f, yStart + widthJt);
            points[3] = new PointF(xStart + this.Width * 0.08f, yStart + widthJt + this.Height * 0.08f);
            points[4] = new PointF(xStart + this.Width * 0.08f + widthJt / 2, yStart + widthJt + this.Height * 0.08f + this.Height * 0.02f);
            points[5] = new PointF(xStart + this.Width * 0.08f + widthJt, yStart + widthJt + this.Height * 0.08f);
            points[6] = new PointF(xStart + this.Width * 0.08f + widthJt, yStart);
            points[7] = new PointF(xStart, yStart);
            //PointF[] points = new PointF[] { new PointF(xStart, yStart), new PointF(xStart, yStart + widthJt / 2), new PointF(xStart + this.Width * 0.1f, yStart + widthJt / 2), new PointF(xStart + this.Width * 0.1f, Height - this.dockHeight), new PointF((float)(loaclPoint.X + Width - 1), 20f), new PointF(loaclPoint.X + 0f, 20f) };
            path.AddPolygon(points);
            //brush = new LinearGradientBrush(new Point(0, 20), new Point(Width - 1, 20), Color.FromArgb(0x8e, 0xc4, 0xd8), Color.FromArgb(240, 240, 240));
            //blend.Colors = new Color[] { Color.FromArgb(0xc0, 0xc0, 0xc0), Color.FromArgb(240, 240, 240), Color.FromArgb(0xc0, 0xc0, 0xc0) };
            //brush.InterpolationColors = blend;

            using (Brush brush2 = new SolidBrush(Color.FromArgb(0xcb, 0xcb, 0xcb)))
            {
                graphics.FillPath(brush2, path);
            }

            //齿轮
            yStart = hostConveyerUC1.Location.Y + hostConveyerUC1.Height / 2 + widthJt;
            //yStart = this.Height * edgeWidthK + bottleAllUC1.Height + panel1.Height + buLiaoAndLiaoCaoUC1.Height + hostConveyerUC1.Height;
            gearUC1.Size = new System.Drawing.Size((int)(this.Height * 0.1f), (int)(this.Height * 0.1f));
            gearUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);

            //除尘器
            xStart = this.Width * 0.07f + hostConveyerUC1.Width / 16;
            yStart = hostConveyerUC1.Location.Y + hostConveyerUC1.Height;
            removeDustUC1.Size = new System.Drawing.Size((int)(this.Width * 0.7f), (int)(this.Height * 0.1f));
            removeDustUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);

            //脱硫脱硝桶
            //xStart = (float)myBitmapYt.Width * 3 / 4;
            xStart = this.Width * edgeWidthK * 3 + (float)myBitmapYt.Width / 2;
            yStart = removeDustUC1.Location.Y + removeDustUC1.Height;
            //yStart = this.Height * edgeWidthK + bottleAllUC1.Height + panel1.Height;
            graphics.DrawImage(myBitmapTlTx, xStart, yStart, myBitmapTlTx.Width, myBitmapTlTx.Height);



            //筛子
            //shaiZiUC1.Width = this.Width / 8;
            //shaiZiUC1.Height = this.Width / 8;
            xStart = this.Width / 2 - shaiZiUC1.Width / 2;
            yStart = removeDustUC1.Location.Y + removeDustUC1.Height;
            shaiZiUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);


            //环冷机
            //huanLengJiUC1.Width = (int)(this.Width / 4.5f);
            //huanLengJiUC1.Height = this.Width / 10;
            //xStart = this.Width / 120 * 90;
            huanLengJiUC1.Width = (int)(this.Width * 0.3f);
            huanLengJiUC1.Height = (int)(this.Height * 0.2f);
            xStart = shaiZiUC1.Location.X + shaiZiUC1.Width + this.Width * 0.1f;
            yStart = removeDustUC1.Location.Y + removeDustUC1.Height;
            huanLengJiUC1.Location = new System.Drawing.Point((int)xStart, (int)yStart);


            //绘制箭头 左边的筛子的
            AdjustableArrowCap aac = new AdjustableArrowCap(5, 2);
            Pen pen = new Pen(Color.Black);
            pen.CustomEndCap = aac;
            //pen.CustomStartCap = aac;
            //pen.BringToFront();//将控件放置所有控件最顶层 
            //pen.SendToBack();//将控件放置所有控件最底层
            yStart = shaiZiUC1.Location.Y + shaiZiUC1.Height / 9 * 8;
            point1 = new Point(this.Width / 2, (int)(yStart));
            point2 = new Point(1, (int)(yStart));
            graphics.DrawLine(pen, point1, point2);


            //SF_2， Z1_1，Z2_2皮带名称位置
            labSF_2.Location = new System.Drawing.Point(this.Width / 3, (int)yStart - labSF_2.Height);
            labZ1_1.Location = new System.Drawing.Point(this.Width / 8, (int)yStart - labZ1_1.Height);
            labZ2_2.Location = new System.Drawing.Point(0, (int)yStart - labZ2_2.Height * 3);
            //labZ2_2.BackColor = Color.Transparent;

            graphics.DrawImage(myBitmap1, (int)((labSF_2.Location.X + labZ1_1.Location.X) / 2 - myBitmap1.Width / 2), yStart - myBitmap1.Height / 2, myBitmap1.Width, myBitmap1.Height);

            graphics.DrawString("铺底料皮带", Font, Brushes.Gray, new Rectangle((int)((labSF_2.Location.X + labZ1_1.Location.X) / 2 - myBitmap1.Width / 2), (int)(yStart - myBitmap1.Height), 80, 15), this.sf);


            point1 = new Point(3, (int)(yStart));
            point2 = new Point(3, (int)(panel1.Location.Y + panel1.Height));
            graphics.DrawLine(pen, point1, point2);


            point1 = new Point(3, (int)(panel1.Location.Y + panel1.Height + 3));
            point2 = new Point((int)(this.Width * 0.13f - 3), (int)(panel1.Location.Y + panel1.Height + 3));
            graphics.DrawLine(pen, point1, point2);

            //S_2皮带名称位置
            labS_2.Location = new System.Drawing.Point((int)(this.Width * 0.11f / 2 - labS_2.Width / 2), (int)(panel1.Location.Y + panel1.Height - labS_2.Height));

            point1 = new Point((int)(this.Width * 0.13f - 3), (int)(panel1.Location.Y + panel1.Height + 3));
            point2 = new Point((int)(this.Width * 0.13f - 3), (int)(panel1.Location.Y + panel1.Height + this.Height * 0.05));
            graphics.DrawLine(pen, point1, point2);


            //右下筛子开始的箭头
            xStart = shaiZiUC1.Location.X + shaiZiUC1.Width + 3;

            point1 = new Point((int)(xStart), (int)(yStart));
            point2 = new Point((int)(xStart + this.Width * 0.06), (int)(yStart));
            graphics.DrawLine(pen, point1, point2);

            graphics.DrawImage(myBitmap2, (int)(xStart), yStart - myBitmap2.Height / 2, myBitmap2.Width, myBitmap2.Height);
            graphics.DrawString("冷返矿皮带", Font, Brushes.Gray, new Rectangle((int)(xStart), (int)(yStart - myBitmap2.Height), 80, 15), this.sf);



            point1 = point2;
            point2 = new Point((int)(xStart + this.Width * 0.06), (int)(this.Height * 0.98));
            graphics.DrawLine(pen, point1, point2);

            point1 = point2;
            point2 = new Point((int)(this.Width - 3), (int)(this.Height * 0.98));
            graphics.DrawLine(pen, point1, point2);

            //SF_3,Z1_2皮带名称位置
            labSF_3.Location = new System.Drawing.Point((int)(xStart + this.Width * 0.06), (int)(this.Height * 0.98f + labSF_3.Height / 4));
            labZ1_2.Location = new System.Drawing.Point(this.Width - labZ1_2.Width, (int)(this.Height / 2));

            point1 = new Point((int)(this.Width - 5), (int)(this.Height * 0.98));
            point2 = new Point((int)(this.Width - 5), (int)(3));
            graphics.DrawLine(pen, point1, point2);

            point1 = new Point((int)(this.Width - 5), (int)(5));
            point2 = new Point((int)(this.Width / 2), (int)(5));
            graphics.DrawLine(pen, point1, point2);


            //Z2_3皮带名称位置
            labZ2_3.Location = new System.Drawing.Point((int)(this.Width / 4 * 3), 1);

            //右上筛子开始的箭头
            Pen pen2 = new Pen(Color.Black);
            //开始的线
            pen2.CustomStartCap = aac;
            //pen2.CustomEndCap = aac;
            point1 = new Point((int)(xStart), (int)(yStart - this.Height * 0.1));
            point2 = new Point((int)(huanLengJiUC1.Location.X - 5), (int)(yStart - this.Height * 0.1));
            graphics.DrawLine(pen2, point1, point2);

            point1 = point2;
            point2 = new Point((int)(huanLengJiUC1.Location.X - 5), (int)(huanLengJiUC1.Location.Y + huanLengJiUC1.Height + 10));
            graphics.DrawLine(pen2, point1, point2);


            this.pipeLine13.Location = point2;
            this.pipeLine13.Size = new System.Drawing.Size((int)(huanLengJiUC1.Width * 0.85f), 15);

            this.pipeLine11.Location = new System.Drawing.Point((int)(this.Width * 0.25f), (int)(this.shaiZiUC1.Height + shaiZiUC1.Location.Y));
            this.pipeLine11.Size = new System.Drawing.Size((int)(this.Width * 0.25f + this.shaiZiUC1.Width / 2), 15);

            this.pipeLine12.Location = new System.Drawing.Point(10, (int)(pipeLine11.Location.Y + 18));
            this.pipeLine12.Size = new System.Drawing.Size((int)(this.Width * 0.25f), 15);

            graphics.DrawString("取样点", Font, Brushes.Gray, new Rectangle(this.pipeLine11.Location.X + pipeLine11.Width / 5, this.pipeLine12.Location.Y, 60, 15), this.sf);

            graphics.DrawString("成品皮带秤", Font, Brushes.Gray, new Rectangle(this.pipeLine11.Location.X + pipeLine11.Width / 3 * 2, this.pipeLine12.Location.Y, 80, 15), this.sf);

            if (this.Height > (this.pipeLine12.Location.Y + this.pipeLine12.Height + 20))
            {
                //graphics.DrawString("取样时间：2019.01.15 11:01", Font, Brushes.Gray, new Rectangle(this.pipeLine11.Location.X + 15, this.pipeLine12.Location.Y + 15, 180, 15), this.sf);
                graphics.DrawString("取样时间：2019.01.15 11:01", Font, Brushes.Gray, new Rectangle(this.pipeLine11.Location.X + 15, this.Height - 25, 180, 15), this.sf);
            }

            //point1 = point2;
            //point2 = new Point((int)(huanLengJiUC1.Location.X + huanLengJiUC1.Width), (int)(huanLengJiUC1.Location.Y + huanLengJiUC1.Height + 10));
            //graphics.DrawLine(pen2, point1, point2);



            //float xxx = (float)myBitmapYt.Width * 3 / 4;
            float xxx = this.Width * edgeWidthK * 3 + (float)myBitmapYt.Width / 2;
            ////烟筒和脱硫脱销的直线
            point1 = new Point((int)(this.Width * edgeWidthK * 3 + myBitmapYt.Width / 3), panel1.Location.Y + panel1.Height + myBitmapYt.Height);
            point2 = new Point((int)(this.Width * edgeWidthK * 3 + myBitmapYt.Width / 3), removeDustUC1.Location.Y + removeDustUC1.Height + myBitmapTlTx.Height - 20);
            var point3 = new Point((int)(xxx), removeDustUC1.Location.Y + removeDustUC1.Height + myBitmapTlTx.Height - 20);

            graphics.DrawLines(pen2, new Point[] { point1, point2, point3 });
            point1 = new Point((int)(xxx + myBitmapTlTx.Width + 3), removeDustUC1.Location.Y + removeDustUC1.Height + myBitmapTlTx.Height - 20);

            point2 = new Point((int)(xxx + myBitmapTlTx.Width + 3), (int)(removeDustUC1.Location.Y + removeDustUC1.Height * 0.22f));
            point3 = new Point((int)(removeDustUC1.Location.X), (int)(removeDustUC1.Location.Y + removeDustUC1.Height * 0.22f));

            graphics.DrawLines(pen2, new Point[] { point1, point2, point3 });


            point1 = new Point((int)(xxx + myBitmapTlTx.Width + 3), removeDustUC1.Location.Y + removeDustUC1.Height + myBitmapTlTx.Height - 6);

            point2 = new Point((int)(removeDustUC1.Location.X - 2), removeDustUC1.Location.Y + removeDustUC1.Height + myBitmapTlTx.Height - 6);
            point3 = new Point((int)(removeDustUC1.Location.X - 2), (int)(removeDustUC1.Location.Y + removeDustUC1.Height * 0.7f));

            graphics.DrawLines(pen2, new Point[] { point1, point2, point3 });


        }
        protected StringFormat sf;

        private PipeLine pipeLine11;
        private PipeLine pipeLine12;
        private PipeLine pipeLine13;

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
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(this.BackColor);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;


            RefreshFun(graphics);


        }



        BackgroundWorker worker = new BackgroundWorker();
        delegate void aaa();

        private void btnHuanXinLiao_Click(object sender, EventArgs e)
        {
            hostConveyerUC1.IsRun();
        }



        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Func<bool> cancelFun = () =>
            {
                if (this.worker.CancellationPending)
                {
                    e.Cancel = true;
                    return true;
                }
                else
                {
                    return false;
                }
            };
            e.Result = false;
            int winRefreshTime = 6;//6秒刷新一次界面上面的显示值
            try
            {
                do
                {
                    //modelT_PLC_3S = getT_PLC_3S.GetT_PLC_3SData("select top 1 * from T_PLC_3S order by TIMESTAMP desc");
                    System.Threading.Thread.Sleep(500);
                    if (cancelFun())
                    {
                        break;
                    }

                } while (!cancelFun());
                e.Result = true;
            }
            catch (Exception ex)
            {
                e.Result = false;
                aaa ShowInfo = delegate ()
                {
                    MessageBox.Show("测量时发生错误: " + ex.Message.ToString());
                    //DisplayInfo("测量时发生错误: " + ex.Message.ToString(), 1);
                };
                this.Invoke(ShowInfo);
            }
            finally
            {
                DateTime mEndTime = System.DateTime.Now;

            }
        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        void windowformRefresh()
        {
            bottleAllUC1.T_TOTAL_SP_W_3S = modelT_PLC_3S.T_TOTAL_SP_W_3S.ToString();
            bottleAllUC1.T_TOTAL_PV_W_3S = modelT_PLC_3S.T_TOTAL_PV_W_3S.ToString();

            //1-8 体积：1000  9-13体积：500  14-15体积：700
          //  bottleAllUC1.BottomItems[0].BottleObj.HeadTag = modelT_PLC_3S.T_CODE_1.ToString();
            bottleAllUC1.BottomItems[0].BottleObj.BottleTag = modelT_PLC_3S.T_W_1_3S.ToString();


            //bottleAllUC1.BottomItems[1].BottleObj.HeadTag = modelT_PLC_3S.T_CODE_2.ToString();
            bottleAllUC1.BottomItems[1].BottleObj.BottleTag = modelT_PLC_3S.T_W_2_3S.ToString();


        }

        private void blendingUC1_Load(object sender, EventArgs e)
        {

        }

        private void blendingUC2_Load(object sender, EventArgs e)
        {

        }

        private void buLiaoAndLiaoCaoUC1_Load(object sender, EventArgs e)
        {

        }

        private void hostConveyerUC1_Load(object sender, EventArgs e)
        {

        }
    }
}
