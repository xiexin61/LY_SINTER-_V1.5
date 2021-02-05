using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel;

namespace UserControlIndex
{
    public abstract class AbstractPipeLine : IDisposable
    {
        public AbstractPipeLine()
        {
            //InitializeComponent();  
            this.hslPipeLineStyle = PipeLineStyle.Horizontal;
            this.centerColor = Color.LightGray;
            this.edgeColor = Color.DimGray;
            this.edgePen = new Pen(Color.DimGray, 1f);
            this.hslPipeTurnLeft = PipeTurnDirection.None;
            this.hslPipeTurnRight = PipeTurnDirection.None;
            this.isPipeLineActive = false;
            this.pipeLineWidth = 5;
            this.colorPipeLineCenter = Color.DimGray;//Color.DodgerBlue;
            this.moveSpeed = 2.5f;
            this.startOffect = 0f;
            //this.timer = null;
            //this.components = null;
            //this.InitializeComponent();
            //base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            //base.SetStyle(ControlStyles.ResizeRedraw, true);
            //base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.timer = new Timer();
            //this.timer.Interval = 600;
            //this.timer.Tick += new EventHandler(this.Timer_Tick);
            //this.timer.Start();

            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);

            worker.WorkerSupportsCancellation = true;
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
        }

        BackgroundWorker worker = new BackgroundWorker();
        //public delegate void DoMeasureDelegate();
        delegate void aaa();

        public void PipeLine_Disposed()
        {
            //if (timer != null)
            //{
            //    timer.Stop();
            //    timer.Tick -= this.Timer_Tick;
            //}
            if (worker.IsBusy && !worker.CancellationPending)
            {
                worker.CancelAsync();

            }
        }

        public void OnLoad()
        {

            //this.timer = new Timer();
            //this.timer.Interval = 600;
            //this.timer.Tick += new EventHandler(this.Timer_Tick);
            //this.timer.Start();

            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);

            worker.WorkerSupportsCancellation = true;
            if (!worker.IsBusy)
            {
                worker.RunWorkerAsync();
            }
        }


        // Fields
        protected Color centerColor;
        protected Color colorPipeLineCenter;
        //private IContainer components;
        protected Color edgeColor;
        protected Pen edgePen;
        protected PipeLineStyle hslPipeLineStyle;
        protected PipeTurnDirection hslPipeTurnLeft;
        protected PipeTurnDirection hslPipeTurnRight;
        protected bool isPipeLineActive;
        protected float moveSpeed;
        protected int pipeLineWidth;
        protected float startOffect;
        //private Timer timer;

        protected ColorBlend colorBlend;
        protected GraphicsPath path = null;
        protected LinearGradientBrush brush = null;
        protected LinearGradientBrush brush2 = null;
        // Methods

        public abstract void OnPaint(Graphics graphics, int Width, int Height, Point loaclPoint);
        public abstract void Dispose();

        //GraphicsPath gpath = null;
        //PathGradientBrush pgbrush = null;
        //private void PaintEllipse(Graphics g, ColorBlend paracolorBlend, Rectangle rect, float startAngle, float sweepAngle);
        protected void PaintEllipse(Graphics g, ColorBlend paracolorBlend, Rectangle rect, float startAngle, float sweepAngle)
        {
            GraphicsPath gpath = new GraphicsPath();
            gpath.AddEllipse(rect);
            //PathGradientBrush brush = new PathGradientBrush(gpath);
            PathGradientBrush pgbrush = new PathGradientBrush(gpath);
            pgbrush.CenterPoint = (PointF)new Point(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
            pgbrush.InterpolationColors = paracolorBlend;
            g.FillPie(pgbrush, rect, startAngle, sweepAngle);
            g.DrawArc(this.edgePen, rect, startAngle, sweepAngle);
            pgbrush.Dispose();
            gpath.Dispose();
            //gpath = null;
            //pgbrush = null;
            //GC.Collect();
        }



        //private void PaintRectangleBorder(Graphics g, Brush brush, Pen pen, Rectangle rectangle);
        protected void PaintRectangleBorder(Graphics g, Brush brush, Pen pen, Rectangle rectangle)
        {
            this.PaintRectangleBorder(g, brush, pen, rectangle, true, true, true, true);
        }
        //private void PaintRectangleBorder(Graphics g, Brush brush, Pen pen, Rectangle rectangle, bool left, bool right, bool up, bool down);
        protected void PaintRectangleBorder(Graphics g, Brush brush, Pen pen, Rectangle rectangle, bool left, bool right, bool up, bool down)
        {
            g.FillRectangle(brush, rectangle);
            if (left)
            {
                g.DrawLine(pen, rectangle.X, rectangle.Y, rectangle.X, (rectangle.Y + rectangle.Height) - 1);
            }
            if (up)
            {
                g.DrawLine(pen, rectangle.X, rectangle.Y, (rectangle.X + rectangle.Width) - 1, rectangle.Y);
            }
            if (right)
            {
                g.DrawLine(pen, (rectangle.X + rectangle.Width) - 1, rectangle.Y, (rectangle.X + rectangle.Width) - 1, (rectangle.Y + rectangle.Height) - 1);
            }
            if (down)
            {
                g.DrawLine(pen, rectangle.X, (rectangle.Y + rectangle.Height) - 1, (rectangle.X + rectangle.Width) - 1, (rectangle.Y + rectangle.Height) - 1);
            }
        }

        //private void PaintRectangleBorderLeftRight(Graphics g, Brush brush, Pen pen, Rectangle rectangle);
        protected void PaintRectangleBorderLeftRight(Graphics g, Brush brush, Pen pen, Rectangle rectangle)
        {
            this.PaintRectangleBorder(g, brush, pen, rectangle, true, true, false, false);
        }
        //private void PaintRectangleBorderUpDown(Graphics g, Brush brush, Pen pen, Rectangle rectangle);
        protected void PaintRectangleBorderUpDown(Graphics g, Brush brush, Pen pen, Rectangle rectangle)
        {
            this.PaintRectangleBorder(g, brush, pen, rectangle, false, false, true, true);
        }

        //private void Timer_Tick(object sender, EventArgs e);
        protected void Timer_Tick(object sender, EventArgs e)
        {
            if (this.isPipeLineActive)
            {
                this.startOffect -= this.moveSpeed;
                if ((this.startOffect <= -10f) || (this.startOffect >= 10f))
                {
                    this.startOffect = 0f;
                }
                //base.Invalidate();
            }
            System.Threading.Thread.Sleep(100);
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

            try
            {
                do
                {
                    if (this.isPipeLineActive)
                    {
                        this.startOffect -= this.moveSpeed;
                        if ((this.startOffect <= -10f) || (this.startOffect >= 10f))
                        {
                            this.startOffect = 0f;
                        }
                        //base.Invalidate();
                    }
                    System.Threading.Thread.Sleep(500);
                    if (cancelFun())
                    {
                        break;
                    }

                } while (true);


                e.Result = true;
            }
            catch (Exception ex)
            {
                e.Result = false;
                //aaa ShowInfo = delegate()
                //{
                //    MessageBox.Show("测量时发生错误: " + ex.Message.ToString());
                //    //DisplayInfo("测量时发生错误: " + ex.Message.ToString(), 1);
                //};
                //this.Invoke(ShowInfo);
            }
            finally
            {
                DateTime mEndTime = System.DateTime.Now;

            }
        }
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }



        //// Properties
        //[Browsable(true), Description("获取或设置控件的背景色"), Category("Appearance"), DefaultValue(typeof(Color), "Transparent"), EditorBrowsable(EditorBrowsableState.Always)]
        //public override Color BackColor
        //{
        //    get
        //    {
        //        return base.BackColor;
        //    }
        //    set
        //    {
        //        base.BackColor = value;
        //    }
        //}
        //[Browsable(true), Description("获取或设置管道控件的边缘颜色"), Category("Appearance"), DefaultValue(typeof(Color), "DimGray")]
        public Color EdgeColor
        {
            get
            {
                return this.edgeColor;
            }
            set
            {
                this.edgeColor = value;
                this.edgePen = new Pen(value, 1f);
                //base.Invalidate();
            }
        }
        //[Browsable(true), Description("获取或设置管道控件的中心颜色"), Category("Appearance"), DefaultValue(typeof(Color), "LightGray")]
        public Color LineCenterColor
        {
            get
            {
                return this.centerColor;
            }
            set
            {
                this.centerColor = value;
                //base.Invalidate();
            }
        }
        //[Browsable(true), Description("获取或设置管道线液体流动的速度，0为静止，正数为正向流动，负数为反向流动"), Category("Appearance"), DefaultValue((float)0.3f)]
        public float MoveSpeed
        {
            get
            {
                return this.moveSpeed;
            }
            set
            {
                this.moveSpeed = value;
                //base.Invalidate();
            }
        }
        //[Browsable(true), Description("获取或设置管道线是否激活液体显示"), Category("Appearance"), DefaultValue(false)]
        public bool PipeLineActive
        {
            get
            {
                return this.isPipeLineActive;
            }
            set
            {
                this.isPipeLineActive = value;
                //base.Invalidate();
            }
        }
        //[Browsable(true), Description("获取或设置管道控件是否是横向的还是纵向的"), Category("Appearance"), DefaultValue(typeof(PipeLineStyle), "Horizontal")]
        public PipeLineStyle PipeLineStyle
        {
            get
            {
                return this.hslPipeLineStyle;
            }
            set
            {
                this.hslPipeLineStyle = value;
                //base.Invalidate();
            }
        }
        //[Browsable(true), Description("获取或设置管道控件左侧或是上方的端点朝向"), Category("Appearance"), DefaultValue(typeof(PipeTurnDirection), "None")]
        public PipeTurnDirection PipeTurnLeft
        {
            get
            {
                return this.hslPipeTurnLeft;
            }
            set
            {
                this.hslPipeTurnLeft = value;
                //base.Invalidate();
            }
        }
        //[Browsable(true), Description("获取或设置管道控件左侧或是上方的端点朝向"), Category("Appearance"), DefaultValue(typeof(PipeTurnDirection), "None")]
        public PipeTurnDirection PipeTurnRight
        {
            get
            {
                return this.hslPipeTurnRight;
            }
            set
            {
                this.hslPipeTurnRight = value;
                //base.Invalidate();
            }
        }


    }


    public class PipeLineNew : AbstractPipeLine
    {
        public PipeLineNew()
            : base()
        { }
        public override void Dispose()
        {

        }


        public override void OnPaint(Graphics graphics, int width, int height, Point loaclPoint)
        { 
            {
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                //ColorBlend colorBlend = new ColorBlend();
                colorBlend = new ColorBlend();
                float[] singleArray1 = new float[3];
                singleArray1[1] = 0.5f;
                singleArray1[2] = 1f;
                colorBlend.Positions = singleArray1;
                colorBlend.Colors = new Color[] { this.edgeColor, this.centerColor, this.edgeColor };
                //GraphicsPath path = new GraphicsPath(FillMode.Alternate);
                path = new GraphicsPath(FillMode.Alternate);
                if (this.hslPipeLineStyle == PipeLineStyle.Horizontal)
                {
                    //LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, height - 1), this.edgeColor, this.centerColor);
                    brush = new LinearGradientBrush(new Point(loaclPoint.X + 0, loaclPoint.Y + 0), new Point(loaclPoint.X + 0, loaclPoint.Y + height - 1), this.edgeColor, this.centerColor);
                    brush.InterpolationColors = colorBlend;
                    if (this.hslPipeTurnLeft == PipeTurnDirection.Up)
                    {
                        this.PaintEllipse(graphics, colorBlend, new Rectangle(loaclPoint.X + 0, loaclPoint.Y + -height - 1, 2 * height, 2 * height), 90f, 90f);
                    }
                    else if (this.hslPipeTurnLeft == PipeTurnDirection.Down)
                    {
                        this.PaintEllipse(graphics, colorBlend, new Rectangle(loaclPoint.X + 0, loaclPoint.Y + 0, 2 * height, 2 * height), 180f, 90f);
                    }
                    else
                    {
                        this.PaintRectangleBorderUpDown(graphics, brush, this.edgePen, new Rectangle(loaclPoint.X + 0, loaclPoint.Y + 0, height, height));
                    }
                    if (this.hslPipeTurnRight == PipeTurnDirection.Up)
                    {
                        this.PaintEllipse(graphics, colorBlend, new Rectangle(loaclPoint.X + (width - 1) - (height * 2), loaclPoint.Y + -height - 1, 2 * height, 2 * height), 0f, 90f);
                    }
                    else if (this.hslPipeTurnRight == PipeTurnDirection.Down)
                    {
                        this.PaintEllipse(graphics, colorBlend, new Rectangle(loaclPoint.X + (width - 1) - (height * 2), loaclPoint.Y + 0, 2 * height, 2 * height), 270f, 90f);
                    }
                    else
                    {
                        this.PaintRectangleBorderUpDown(graphics, brush, this.edgePen, new Rectangle(loaclPoint.X + width - height, loaclPoint.Y + 0, height, height));
                    }
                    if ((width - (height * 2)) >= 0)
                    {
                        this.PaintRectangleBorderUpDown(graphics, brush, this.edgePen, new Rectangle(loaclPoint.X + height - 1, loaclPoint.Y + 0, (width - (2 * height)) + 2, height));
                    }
                    brush.Dispose();
                    if (this.isPipeLineActive)
                    {
                        if (width < height)
                        {
                            path.AddLine(loaclPoint.X + 0, loaclPoint.Y + height / 2, loaclPoint.X + height, loaclPoint.Y + height / 2);
                        }
                        else
                        {
                            if (this.hslPipeTurnLeft == PipeTurnDirection.Up)
                            {
                                path.AddArc(new Rectangle(loaclPoint.X + height / 2, loaclPoint.Y + (-height / 2) - 1, height, height), 180f, -90f);
                            }
                            else if (this.hslPipeTurnLeft == PipeTurnDirection.Down)
                            {
                                path.AddArc(new Rectangle(loaclPoint.X + height / 2, loaclPoint.Y + height / 2, height, height), 180f, 90f);
                            }
                            else
                            {
                                path.AddLine(loaclPoint.X + 0, loaclPoint.Y + height / 2, loaclPoint.X + height, loaclPoint.Y + height / 2);
                            }
                            if ((width - (height * 2)) >= 0)
                            {
                                path.AddLine(loaclPoint.X + height, loaclPoint.Y + height / 2, loaclPoint.X + (width - height) - 1, loaclPoint.Y + height / 2);
                            }
                            if (this.hslPipeTurnRight == PipeTurnDirection.Up)
                            {
                                path.AddArc(new Rectangle(loaclPoint.X + (width - 1) - ((height * 3) / 2), loaclPoint.Y + (-height / 2) - 1, height, height), 90f, -90f);
                            }
                            else if (this.hslPipeTurnRight == PipeTurnDirection.Down)
                            {
                                path.AddArc(new Rectangle(loaclPoint.X + (width - 1) - ((height * 3) / 2), loaclPoint.Y + height / 2, height, height), 270f, 90f);
                            }
                            else
                            {
                                path.AddLine(loaclPoint.X + (int)(width - height), loaclPoint.Y + (int)(height / 2), loaclPoint.X + (int)(width - 1), loaclPoint.Y + (int)(height / 2));
                            }
                        }
                    }
                }
                else
                {
                    //LinearGradientBrush brush2 = new LinearGradientBrush(new Point(0, 0), new Point(width - 1, 0), this.edgeColor, this.centerColor);
                    brush2 = new LinearGradientBrush(new Point(loaclPoint.X + 0, loaclPoint.Y + 0), new Point(loaclPoint.X + width - 1, loaclPoint.Y + 0), this.edgeColor, this.centerColor);
                    brush2.InterpolationColors = colorBlend;
                    if (this.hslPipeTurnLeft == PipeTurnDirection.Left)
                    {
                        this.PaintEllipse(graphics, colorBlend, new Rectangle(loaclPoint.X + -width - 1, loaclPoint.Y + 0, 2 * width, 2 * width), 270f, 90f);
                    }
                    else if (this.hslPipeTurnLeft == PipeTurnDirection.Right)
                    {
                        this.PaintEllipse(graphics, colorBlend, new Rectangle(loaclPoint.X + 0, loaclPoint.Y + 0, 2 * width, 2 * width), 180f, 90f);
                    }
                    else
                    {
                        this.PaintRectangleBorderLeftRight(graphics, brush2, this.edgePen, new Rectangle(loaclPoint.X + 0, loaclPoint.Y + 0, width, width));
                    }
                    if (this.hslPipeTurnRight == PipeTurnDirection.Left)
                    {
                        this.PaintEllipse(graphics, colorBlend, new Rectangle(loaclPoint.X + -width - 1, loaclPoint.Y + height - (width * 2), 2 * width, 2 * width), 0f, 90f);
                    }
                    else if (this.hslPipeTurnRight == PipeTurnDirection.Right)
                    {
                        this.PaintEllipse(graphics, colorBlend, new Rectangle(loaclPoint.X + 0, loaclPoint.Y + height - (width * 2), 2 * width, 2 * width), 90f, 90f);
                    }
                    else
                    {
                        this.PaintRectangleBorderLeftRight(graphics, brush2, this.edgePen, new Rectangle(loaclPoint.X + 0, loaclPoint.Y + height - width, width, width));
                    }
                    if ((height - (width * 2)) >= 0)
                    {
                        this.PaintRectangleBorderLeftRight(graphics, brush2, this.edgePen, new Rectangle(loaclPoint.X + 0, loaclPoint.Y + width - 1, width, (height - (width * 2)) + 2));
                    }
                    brush2.Dispose();
                    if (this.isPipeLineActive)
                    {
                        if (width > height)
                        {
                            path.AddLine(loaclPoint.X + 0, loaclPoint.Y + height / 2, loaclPoint.X + height, loaclPoint.Y + height / 2);
                        }
                        else
                        {
                            if (this.hslPipeTurnLeft == PipeTurnDirection.Left)
                            {
                                path.AddArc(new Rectangle(loaclPoint.X + (-width / 2), loaclPoint.Y + (width / 2) - 1, width, width), 270f, 90f);
                            }
                            else if (this.hslPipeTurnLeft == PipeTurnDirection.Right)
                            {
                                path.AddArc(new Rectangle(loaclPoint.X + width / 2, loaclPoint.Y + (width / 2) - 1, width, width), 270f, -90f);
                            }
                            else
                            {
                                path.AddLine(loaclPoint.X + width / 2, loaclPoint.Y + 0, loaclPoint.X + width / 2, loaclPoint.Y + width);
                            }
                            if ((height - (width * 2)) >= 0)
                            {
                                path.AddLine(loaclPoint.X + width / 2, loaclPoint.Y + width, loaclPoint.X + width / 2, loaclPoint.Y + (height - width) - 1);
                            }
                            if (this.hslPipeTurnRight == PipeTurnDirection.Left)
                            {
                                path.AddArc(new Rectangle(loaclPoint.X + -width / 2, loaclPoint.Y + (height - 1) - ((width * 3) / 2), width, width), 0f, 90f);
                            }
                            else if (this.hslPipeTurnRight == PipeTurnDirection.Right)
                            {
                                path.AddArc(new Rectangle(loaclPoint.X + width / 2, loaclPoint.Y + (height - 1) - ((width * 3) / 2), width, width), -180f, -90f);
                            }
                            else
                            {
                                path.AddLine(loaclPoint.X + (int)(width / 2), loaclPoint.Y + (int)(height - width), loaclPoint.X + (int)(width / 2), loaclPoint.Y + (int)(height - 1));
                            }
                        }
                    }
                }
                colorBlend = null;
                using (Pen pen = new Pen(this.colorPipeLineCenter, (float)this.pipeLineWidth))
                {
                    pen.DashStyle = DashStyle.Custom;
                    pen.DashPattern = new float[] { 5f, 5f };
                    pen.DashOffset = this.startOffect;
                    graphics.DrawPath(pen, path);
                }
                //base.OnPaint(e);
                //GC.Collect();
            }
        }


    }
    public enum PipeTurnDirection
    {
        Down = 2,
        Left = 3,
        None = 5,
        Right = 4,
        Up = 1
    }
    public enum PipeLineStyle
    {
        Horizontal = 1,
        Vertical = 2
    }

}
