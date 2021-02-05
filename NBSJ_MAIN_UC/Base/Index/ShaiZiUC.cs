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

namespace UserControlIndex
{
    public partial class ShaiZiUC : UserControl
    {
        public ShaiZiUC()
        {
            InitializeComponent();

            //Graphics g = pictureBox1.CreateGraphics();
            //g.DrawRectangle(new Pen(Color.Red), 100, 50, 100, 50);
            //g.TranslateTransform(100, 50);
            //g.RotateTransform(30);
            //g.TranslateTransform(-100, -50);
            //g.DrawRectangle(new Pen(Color.Blue), 100, 50, 100, 50);

        }

        Bitmap myBitmap = null;
        Bitmap myBitmap2 = null;
        //Bitmap myBitmap3 = null;
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(this.BackColor);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (isrun)
            {
                myBitmap = Rotate(NBSJ_MAIN_UC.Properties.Resources.筛子开, 330);

            }
            else
            {
                myBitmap = Rotate(NBSJ_MAIN_UC.Properties.Resources.筛子关, 330);
            }

            if (isrun2)
            {
                myBitmap2 = Rotate(NBSJ_MAIN_UC.Properties.Resources.筛子开, 30);

            }
            else
            {
                myBitmap2 = Rotate(NBSJ_MAIN_UC.Properties.Resources.筛子关, 30);
            }

            /*if (isrun3)
            {
                myBitmap3 = Rotate(NBSJ_MAIN_UC.Properties.Resources.筛子开, 30);

            }
            else
            {
                myBitmap3 = Rotate(NBSJ_MAIN_UC.Properties.Resources.筛子关, 30);
            }*/

            
            graphics.DrawImage(myBitmap, (int)(this.Width * 0.25f), 0, (int)(this.Width * 0.65), (int)(this.Height * 0.65));
            graphics.DrawImage(myBitmap2, (int)(this.Width * 0.25f), (int)(this.Height * 0.35f), (int)(this.Width * 0.65), (int)(this.Height * 0.65));
            //graphics.DrawImage(myBitmap3, 0, (int)(this.Height * 0.2), (int)(this.Width * 0.65), (int)(this.Height * 0.65));

            graphics.DrawString("1", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.55f), (int)(this.Height * 0.25f), 15, 15));
            //graphics.DrawString("2", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.30f), (int)(this.Width * 0.50), 15, 15));

            graphics.DrawString("2", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.62f), (int)(this.Height * 0.62f), 15, 15));
           
            GC.Collect();

        }
        bool isrun = false;
        [Browsable(true), Description("启动和停止。"), DefaultValue(typeof(bool), "false"), Category("Appearance")]
        public bool IsRun
        {
            get
            {
                return this.isrun;
            }
            set
            {
                this.isrun = value;
                //base.Invalidate();
            }
        }


        bool isrun2 = false;
        [Browsable(true), Description("启动和停止。"), DefaultValue(typeof(bool), "false"), Category("Appearance")]
        public bool IsRun2
        {
            get
            {
                return this.isrun2;
            }
            set
            {
                this.isrun2 = value;
                //base.Invalidate();
            }
        }

        bool isrun3 = false;
        [Browsable(true), Description("启动和停止。"), DefaultValue(typeof(bool), "false"), Category("Appearance")]
        public bool IsRun3
        {
            get
            {
                return this.isrun3;
            }
            set
            {
                this.isrun3 = value;
                //base.Invalidate();
            }
        }

        #region 图片旋转函数
        /// <summary>
        /// 以逆时针为方向对图像进行旋转
        /// </summary>
        /// <param name="b">位图流</param>
        /// <param name="angle">旋转角度[0,360](前台给的)</param>
        /// <returns></returns>
        public Bitmap Rotate(Bitmap b, int angle)
        {
            angle = angle % 360;

            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            //原图的宽和高
            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));

            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            Graphics g = Graphics.FromImage(dsImage);

            g.InterpolationMode = InterpolationMode.Bilinear;

            g.SmoothingMode = SmoothingMode.HighQuality;

            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);

            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);

            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);

            //重至绘图的所有变换
            g.ResetTransform();

            g.Save();
            g.Dispose();
            return dsImage;
        }
        #endregion 图片旋转函数


        /// <summary>
        /// 使控件的整个图面无效并导致重绘控件。
        /// </summary>
        public void InvalidateNew()
        {
            base.Invalidate();
        }


        DateTime dtimeRbtnDouble = DateTime.Now;
        private void rbtnDouble_Click(object sender, EventArgs e)
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
    }
}
