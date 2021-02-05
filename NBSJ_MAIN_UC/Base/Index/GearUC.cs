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
    public partial class GearUC : UserControl
    {
        public GearUC()
        {
            InitializeComponent();

            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.timer = new Timer();
            //this.timer.Interval = 50;
            //this.timer.Tick += new EventHandler(this.Timer_Tick);
            //this.timer.Start();
        }

        //Bitmap myBitmap = null;
        //private Timer timer;
        //protected override void OnPaint(PaintEventArgs e)
        //{

        //    Graphics graphics = e.Graphics;
        //    graphics.Clear(this.BackColor);
        //    graphics.SmoothingMode = SmoothingMode.HighQuality;
        //    graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        //    //Bitmap myBitmap = global::WindowsFormsApplication1.Properties.Resources.齿轮;
        //    //graphics.DrawImage(RotateImg(myBitmap, startAngle), 0, 0, this.Width, this.Height);
        //    ////System.Drawing.Bitmap bmp = AddText(@"D:\test\1.png", "176.94,150.48", 12, "写点啥好呢", (int)startAngle);
        //    ////graphics.DrawImage(bmp, 0, 0, this.Width, this.Height);

        //    if (isrun)
        //    {
        //        myBitmap = global::WindowsFormsApplication1.Properties.Resources.齿轮1;
        //    }
        //    else
        //    {
        //        myBitmap = global::WindowsFormsApplication1.Properties.Resources.齿轮;
        //    }
        //    graphics.DrawImage(myBitmap, 0, 0, this.Width, this.Height);

        //}


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
                pictureBox1.Visible = isrun;
            }
        }



        private float startAngle;
        private float moveSpeed = 0.3f;
        private void Timer_Tick(object sender, EventArgs e)
        {
            //if (this.isConveyerActive)
            //{
            //    this.startOffect -= this.moveSpeed;
            //    if ((this.startOffect <= -10f) || (this.startOffect >= 10f))
            //    {
            //        this.startOffect = 0f;
            //    }
            //}
            this.startAngle += (float)((((double)(this.moveSpeed * 180f)) / 3.1415926535897931) / 10.0);
            if (this.startAngle <= -360f)
            {
                this.startAngle += 360f;
            }
            else if (this.startAngle >= 360f)
            {
                this.startAngle -= 360f;
            }
            base.Invalidate();
        }

        /// <summary>
        /// 根据角度旋转图标
        /// </summary>
        /// <param name="img"></param>
        public Image RotateImg(Image img, float angle)
        {
            //通过Png图片设置图片透明，修改旋转图片变黑问题。
            int width = img.Width;
            int height = img.Height;
            //角度
            Matrix mtrx = new Matrix();
            mtrx.RotateAt(angle, new PointF((width / 2), (height / 2)), MatrixOrder.Append);
            //得到旋转后的矩形
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, width, height));
            RectangleF rct = path.GetBounds(mtrx);
            //生成目标位图
            Bitmap devImage = new Bitmap((int)(rct.Width), (int)(rct.Height));
            Graphics g = Graphics.FromImage(devImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //计算偏移量
            Point Offset = new Point((int)(rct.Width - width) / 2, (int)(rct.Height - height) / 2);
            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, (int)width, (int)height);
            Point center = new Point((int)(rect.X + rect.Width / 2), (int)(rect.Y + rect.Height / 2));
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(angle);
            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(img, rect);
            //重至绘图的所有变换
            g.ResetTransform();
            g.Save();
            g.Dispose();
            path.Dispose();
            return devImage;
        }
        /// <summary>
        /// 第二种方法
        /// </summary>
        /// <param name="b"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public Image RotateImg2(Image b, float angle)
        {
            angle = angle % 360;            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);
            //原图的宽和高
            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));
            //目标位图
            Image dsImage = new Bitmap(W, H);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
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
            //dsImage.Save("yuancd.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return dsImage;
        }


        /// <summary>
        /// 图片添加任意角度文字(文字旋转是中心旋转，角度顺时针为正)
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <param name="locationLeftTop">文字左上角定位(x1,y1)</param>
        /// <param name="fontSize">字体大小，单位为像素</param>
        /// <param name="text">文字内容</param>
        /// <param name="angle">文字旋转角度</param>
        /// <param name="fontName">字体名称</param>
        /// <returns>添加文字后的Bitmap对象</returns>
        public Bitmap AddText(string imgPath, string locationLeftTop, int fontSize, string text, int angle = 0, string fontName = "华文行楷")
        {
            //Image img = Image.FromFile(imgPath);
            Bitmap img =global::NBSJ_MAIN_UC.Properties.Resources.齿轮;

            int width = img.Width;
            int height = img.Height;
            Bitmap bmp = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bmp);
            // 画底图
            graphics.DrawImage(img, 0, 0, width, height);

            Font font = new Font(fontName, fontSize, GraphicsUnit.Pixel);
            SizeF sf = graphics.MeasureString(text, font); // 计算出来文字所占矩形区域

            // 左上角定位
            //string[] location = locationLeftTop.Split(',');
            //float x1 = float.Parse(location[0]);
            //float y1 = float.Parse(location[1]);
            float x1 = 0;
            float y1 = 0;

            // 进行文字旋转的角度定位
            if (angle != 0)
            {
                #region 法一：TranslateTransform平移 + RotateTransform旋转

                /* 
                * 注意：
                * Graphics.RotateTransform的旋转是以Graphics对象的左上角为原点，旋转整个画板的。
                * 同时x，y坐标轴也会跟着旋转。即旋转后的x，y轴依然与矩形的边平行
                * 而Graphics.TranslateTransform方法，是沿着x，y轴平移的
                * 因此分三步可以实现中心旋转
                * 1.把画板(Graphics对象)平移到旋转中心
                * 2.旋转画板
                * 3.把画板平移退回相同的距离(此时的x，y轴仍然是与旋转后的矩形平行的)
                */
                // 把画板的原点(默认是左上角)定位移到文字中心
                graphics.TranslateTransform(x1 + sf.Width / 2, y1 + sf.Height / 2);
                // 旋转画板
                graphics.RotateTransform(angle);
                // 回退画板x,y轴移动过的距离
                graphics.TranslateTransform(-(x1 + sf.Width / 2), -(y1 + sf.Height / 2));

                #endregion

                #region 法二：矩阵旋转

                //Matrix matrix = graphics.Transform;
                //matrix.RotateAt(angle, new PointF(x1 + sf.Width / 2, y1 + sf.Height / 2));
                //graphics.Transform = matrix;

                #endregion
            }

            // 写上自定义角度的文字
            graphics.DrawString(text, font, new SolidBrush(Color.Black), x1, y1);

            graphics.Dispose();
            img.Dispose();

            return bmp;
        }


    }
}
