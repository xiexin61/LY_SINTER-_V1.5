using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace UserControlIndex
{
    public partial class HostConveyerUC : UserControl
    {

        public HostConveyerUC()
        {
            //Imagess = global::ControlsDemo.Properties.Resources.锯齿;
            this.sf = null;
            this.moveSpeed = 0.3f;
            this.startAngle = 0f;
            this.startOffect = 0f;
            this.timer = null;
            this.isConveyerActive = true;
            this.circularRadius = 20f;
            this.components = null;
            this.InitializeComponent();
            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //base.ForeColor = Color.FromArgb(0xf0, 0x1b, 0x2d);
            //base.ForeColor = Color.FromArgb(0x9a, 0x9a, 0x9a);
            base.ForeColor = Color.FromArgb(0x69, 0x69, 0x69);
            this.timer = new Timer();
            this.timer.Interval = 50;
            this.timer.Tick += new EventHandler(this.Timer_Tick);
            //this.timer.Start();

            //this.pictureBox2.Location = new System.Drawing.Point(515, 270);
            //this.pictureBox2.Name = "pictureBox2";
            //this.pictureBox2.Size = new System.Drawing.Size(89, 93);
            //this.pictureBox2.TabIndex = 5;
            //this.pictureBox2.TabStop = false;
            ////f.ForeColor = Color.Blue; //颜色 


            shuziSf.Alignment = StringAlignment.Center; //居中
            shuziSf.LineAlignment = StringAlignment.Center;
            //格式.Alignment = StringAlignment.Far; //右对齐



        }
        Font font0 = new Font("宋体", 16, FontStyle.Regular);
        //Image Imagess;
        private float circularRadius;
        private bool isConveyerActive;
        private float moveSpeed;
        private StringFormat sf;
        private float startAngle;
        private float startOffect;
        private Timer timer;
        float[] juxingArr = new float[4];

        public void IsRun()
        {
            if (!timer.Enabled)
            {
                this.timer.Start();
            }
        }
        public void IsStop()
        {
            if (timer.Enabled)
            {
                this.timer.Stop();

            }
        }


        Color rectangleColor = Color.FromArgb(0x5e, 0x88, 0xb8);//5e88b8
        // Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            int num = 5;
            int num2 = 5;
            this.PaintMain(e.Graphics, (float)base.Width, (float)base.Height, (float)num, (float)num2);

        }

        /// <summary>
        /// 三角形颜色
        /// </summary>
        Color sanjiaoxingColor = Color.FromArgb(0xd1, 0x81, 0x8f);//5e88b8  d1818f
        PointF[] sanjiaoxPoints = null;

        Font shuziFont = new Font("宋体", 10.5F);
        Brush shuziColor = Brushes.Black;
        StringFormat shuziSf = new StringFormat();
        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void DrawRectangle(Graphics g, float x1, float y1, float x2, float y2)
        {

            float recWidth = (x2 - x1 - 21) / 22;
            float recHeight = y2 - y1;

            float xV = 0;
            for (int num = 0; num < 22; num++)
            {
                xV = x1 + recWidth * num + num * 1;
                //g.FillRectangle(new SolidBrush(Color.FromArgb(150, rectangleColor)), xV, y1, recWidth, recHeight);
                ////g.DrawString((num + 1).ToString(), Font, new SolidBrush(Color.Black), xV + recWidth / 2, y1 + recHeight/2);
                Rectangle 矩形 = new Rectangle((int)xV, (int)y1, (int)recWidth, (int)recHeight);
                g.FillRectangle(new SolidBrush(Color.FromArgb(150, rectangleColor)), 矩形);
                g.DrawString((num + 1).ToString(), shuziFont, shuziColor, 矩形, shuziSf);


            }

            Color color000 = Color.FromArgb(100, sanjiaoxingColor);// System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(54)))), ((int)(((byte)(82)))));
            Brush brushes = new SolidBrush(color000);
            sanjiaoxPoints = new PointF[] { new PointF(x1, y1), new PointF(x2, y1), new PointF(x2, y2 - 5), new PointF(x1, y1) };
            g.FillPolygon(brushes, sanjiaoxPoints);

            brushes.Dispose();
            sanjiaoxPoints = null;
            GC.Collect();
            ////g.CreateGraphics()
            //g.FillRectangle(new SolidBrush(Color.FromArgb(100, rectangleColor)), 0, 0, 100, height - 10);//Color.LightGreen
            ////Color.FromArgb(125,Color.LightGreen)；125即颜色的α（阿尔法）值，α越大颜色的透明度越小，α为零就是全透明的了，阿尔法最大值是255，表示不透明。
            //////图像绘制的坐标点
            ////PointF[] points = new PointF[] { new PointF(loaclPoint.X + 0f, 20f), new PointF(loaclPoint.X + 0f, Height - this.dockHeight), new PointF(loaclPoint.X + x, (float)(Height - 1)), new PointF((float)(loaclPoint.X + Width - 1), Height - this.dockHeight), new PointF((float)(loaclPoint.X + Width - 1), 20f), new PointF(loaclPoint.X + 0f, 20f) };
            ////path.AddPolygon(points);

        }

        //private void Form1_Paint(object sender, PaintEventArgs e)
        //{
        //    Pen pen = new Pen(Color.Red, 1);            
        //    e.Graphics.DrawLine(pen, 10, 10, 50, 10);
        //    e.Graphics.DrawLine(pen, 10, 10, 50, 50);
        //    e.Graphics.DrawLine(pen, 50, 10, 50, 50);

        //    Color color = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(54)))), ((int)(((byte)(82)))));
        //    Brush brushes = new SolidBrush(color);
        //    Point[] point new Point[3];
        //    point[0] = new Point(50,100);
        //    point[1] = new Point(100,50);
        //    point[2] = new Point(100,100);
        //    e.Graphics.FillPolygon(brushes, point);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="margin">边缘</param>
        /// <param name="offect">抵消</param>
        private void PaintMain(Graphics g, float width, float height, float margin, float offect)
        {

            Pen pen0 = new Pen(Color.Black, 0.8f);//中间的分割线

            float num = ((height - (margin * 2f)) - (offect * 2f)) / 2f;//高度中间位置
            Pen pen = new Pen(this.ForeColor, 1f);
            Pen pen2 = new Pen(this.ForeColor, (num > 100f) ? ((float)7) : ((num > 20f) ? ((float)5) : ((num > 5f) ? ((float)3) : ((float)1))));
            pen2.StartCap = LineCap.Round;
            pen2.EndCap = LineCap.Round;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(margin, margin, (num * 2f) + (offect * 2f), (num * 2f) + (offect * 2f), 90f, 180f);
            path.AddLine((margin + offect) + num, margin, (((width - margin) - offect) - num) - 1f, margin);
            path.AddArc(((width - margin) - (num * 2f)) - (offect * 2f), margin, (num * 2f) + (offect * 2f), (num * 2f) + (offect * 2f), -90f, 180f);
            path.CloseFigure();
            g.DrawPath(pen, path);
            g.DrawEllipse(pen, (float)(margin + offect), (float)(margin + offect), (float)(num * 2f), (float)(num * 2f));
            g.DrawEllipse(pen, (float)(((width - margin) - (num * 2f)) - offect), (float)(margin + offect), (float)(num * 2f), (float)(num * 2f));
            float num2 = (float)Math.Sqrt((double)(((2f * offect) * num) + (offect * offect)));
            float num3 = (float)((Math.Asin((double)(num / (num + offect))) * 180.0) / 3.1415926535897931);
            path.Reset();
            path.AddArc(margin, margin, (num * 2f) + (offect * 2f), (num * 2f) + (offect * 2f), -num3, num3 * 2f);
            path.AddLine((float)(((margin + offect) + num) + num2), (float)((height - margin) - offect), (float)(((((width - margin) - offect) - num) - 1f) - num2), (float)((height - margin) - offect));
            path.AddArc(((width - margin) - (num * 2f)) - (offect * 2f), margin, (num * 2f) + (offect * 2f), (num * 2f) + (offect * 2f), 180f - num3, num3 * 2f);
            path.CloseFigure();
            g.DrawPath(pen, path);
            g.TranslateTransform((margin + offect) + num, (margin + offect) + num);
            g.RotateTransform(this.startAngle);
            //前加号
            g.DrawLine(pen2, (float)(-num / 2f), (float)0f, (float)(num / 2f), (float)0f);
            g.DrawLine(pen2, (float)0f, (float)(-num / 2f), (float)0f, (float)(num / 2f));
            //g.DrawImage(Imagess, (float)(-margin * 2f), (float)(-offect * 2f));
            //g.DrawImage(Imagess, (float)0f, (float)(-num / 2f));
            //前加号
            g.RotateTransform(-this.startAngle);
            g.TranslateTransform((-margin - offect) - num, (-margin - offect) - num);
            g.TranslateTransform(((width - margin) - num) - offect, (margin + offect) + num);
            g.RotateTransform(this.startAngle);
            //后加号
            g.DrawLine(pen2, (float)(-num / 2f), (float)0f, (float)(num / 2f), (float)0f);
            g.DrawLine(pen2, (float)0f, (float)(-num / 2f), (float)0f, (float)(num / 2f));
            //后加号
            g.RotateTransform(-this.startAngle);
            g.TranslateTransform(((-width + margin) + num) + offect, (-margin - offect) - num);
            if (this.isConveyerActive)
            {
                using (Pen pen3 = new Pen(this.ForeColor, 5f))
                {
                    pen3.DashStyle = DashStyle.Custom;
                    pen3.DashPattern = new float[] { 5f, 5f };
                    pen3.DashOffset = this.startOffect;
                    path.Reset();
                    path.AddArc((float)(margin + (offect / 2f)), (float)(margin + (offect / 2f)), (float)((num * 2f) + offect), (float)((num * 2f) + offect), 90f, 180f);
                    path.AddLine((float)(((margin + offect) + num) + 1f), (float)(margin + (offect / 2f)), (float)((((width - margin) - offect) - num) - 2f), (float)(margin + (offect / 2f)));
                    path.AddArc((float)((((width - margin) - (num * 2f)) - (offect * 2f)) + (offect / 2f)), (float)(margin + (offect / 2f)), (float)((num * 2f) + offect), (float)((num * 2f) + offect), -90f, 180f);
                    path.CloseFigure();
                    g.DrawPath(pen3, path);
                }
            }
            g.DrawLine(pen0, (float)((margin * 2f) + (num * 2f) + (offect * 2f)), (float)(height / 2f), (float)(width - ((margin * 2f) + (num * 2f) + (offect * 2f))), (float)(height / 2f));

            //绘制矩形
            float x1 = (float)((margin * 2f) + (num * 2f) + (offect * 2f));
            float y1 = (float)(height / 2f);
            float x2 = (float)(width - ((margin * 2f) + (num * 2f) + (offect * 2f)));
            float y2 = (float)(height / 2f - 5);

            x1 = width * 0.25f;
            y1 = 15;
            //float lengh1 = x2 - x1;
            //float h11 = y2 - y1;
            //g.FillRectangle(new SolidBrush(Color.FromArgb(150, rectangleColor)), x1, y1, lengh1, h11);
            juxingArr[0] = x1;
            juxingArr[1] = y1;
            juxingArr[2] = x2;
            juxingArr[3] = y2;

            DrawRectangle(g, x1, y1, x2, y2);

            //Brush brush = new SolidBrush(Color.Black);//new SolidBrush(this.ForeColor);
            ////g.DrawString(this.Text, font0, brush, new RectangleF(0f, (float)(height / 4f), width, height), this.sf);
            //g.DrawString(this.Text, this.Font, brush, new RectangleF(0f, (float)(height / 4f), width, height), this.sf);
          //  g.DrawString(this.Text, this.Font, Brushes.Black, new RectangleF(0f, (float)(height / 4f), width, height), this.sf);
            g.DrawString(this.HText, this.Font, Brushes.Black, new RectangleF(0f, (float)(height / 4f), width, height), this.sf);

            //Rectangle 矩形6 = new Rectangle(0, (int)(height / 4f), (int)width, (int)height);
            //g.DrawString(this.Text, Font, shuziColor, 矩形6, sf);
            path.Dispose();
            pen.Dispose();
            pen2.Dispose();
            pen0.Dispose();
            //brush.Dispose();

            path = null;
            pen = null;
            pen2 = null;
            pen0 = null;
            //brush = null; 

            GC.Collect();
        }

        //private void Timer_Tick(object sender, EventArgs e);
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.isConveyerActive)
            {
                this.startOffect -= this.moveSpeed;
                if ((this.startOffect <= -10f) || (this.startOffect >= 10f))
                {
                    this.startOffect = 0f;
                }
            }
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

        // Properties
        [Browsable(true), Description("获取或设置控件的背景色"), Category("Appearance"), DefaultValue(typeof(Color), "Transparent"), EditorBrowsable(EditorBrowsableState.Always)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;
            }
        }
        [Browsable(true), Description("获取或设置两侧的圆圈的半径，当且仅当ConveyerStyle属性不为Horizontal枚举时成立"), Category("Appearance"), DefaultValue(1)]
        public float CircularRadius
        {
            get
            {
                return this.circularRadius;
            }
            set
            {
                if ((value <= (base.Width / 2)) && (value <= (base.Height / 2)))
                {
                    this.circularRadius = value;
                    base.Invalidate();
                }
            }
        }
        [Browsable(true), Description("获取或设置管道线是否激活液体显示"), Category("Appearance"), DefaultValue(false)]
        public bool ConveyerActive
        {
            get
            {
                return this.isConveyerActive;
            }
            set
            {
                this.isConveyerActive = value;
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置控件的前景色"), Category("Appearance"), DefaultValue(typeof(Color), "[142, 196, 216]"), EditorBrowsable(EditorBrowsableState.Always)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置传送带流动的速度，0为静止，正数为正向流动，负数为反向流动"), Category("Appearance"), DefaultValue((float)0.3f)]
        public float MoveSpeed
        {
            get
            {
                return this.moveSpeed;
            }
            set
            {
                this.moveSpeed = value;
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置当前控件的文本"), Category("Appearance"), EditorBrowsable(EditorBrowsableState.Always), Bindable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                base.Invalidate();
            }
        }
        public string htext;
        public  string  HText
        {
            get
            {
                return htext;
            }
            set
            {
                htext = value;
                base.Invalidate();
            }
        }

        private void HostConveyerUC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            float x1 = juxingArr[0];// = x1;
            float y1 = juxingArr[1];// = y1;
            float x2 = juxingArr[2];// = x2;
            float y2 = juxingArr[3];// = y2;

            float recWidth = (x2 - x1 - 21) / 22;
            float recHeight = y2 - y1;

            float xV = 0;
            for (int num = 0; num < 22; num++)
            {
                xV = x1 + recWidth * num + num * 1;
                Rectangle quyu = new Rectangle((int)xV, (int)y1, (int)recWidth, (int)recHeight);
                //g.FillRectangle(new SolidBrush(Color.FromArgb(150, rectangleColor)), 矩形);
                //g.DrawString((num + 1).ToString(), shuziFont, shuziColor, 矩形, shuziSf);

                //取样点区域
                if (e.X > quyu.X && e.X < (quyu.X + quyu.Width) && e.Y > quyu.Y && e.Y < (quyu.Y + quyu.Height))
                {
                    //MessageBox.Show((num / 3 + 4).ToString());
                    using (UserControlIndex.FormDaoTui frm = new UserControlIndex.FormDaoTui())
                    {
                        if (num < 9)
                        {
                            frm.Init((num / 3 + 4));
                        }
                        else if (num >= 9 && num <= 13)
                        {

                            frm.Init(7);
                        }
                        else if (num >= 14 && num <= 16)
                        {
                            frm.Init(8);
                        }
                        else if (num >= 17&& num <= 19)
                        {
                            frm.Init(9);
                        }
                        else
                        {
                            frm.Init(10);
                        }
                        frm.ShowDialog();
                    }
                }



            }

        }

        private void HostConveyerUC_MouseMove(object sender, MouseEventArgs e)
        {
            float x1 = juxingArr[0];// = x1;
            float y1 = juxingArr[1];// = y1;
            float x2 = juxingArr[2];// = x2;
            float y2 = juxingArr[3];// = y2;

            float recWidth = (x2 - x1);
            float recHeight = y2 - y1;

            Rectangle quyu = new Rectangle((int)x1, (int)y1, (int)recWidth, (int)recHeight);
            //取样点区域
            if (e.X > quyu.X && e.X < (quyu.X + quyu.Width) && e.Y > quyu.Y && e.Y < (quyu.Y + quyu.Height))
            {
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
