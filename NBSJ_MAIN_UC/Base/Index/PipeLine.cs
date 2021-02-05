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
    public partial class PipeLine : UserControl
    {
        public PipeLine()
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
            this.colorPipeLineCenter = Color.FromArgb(0x00, 0x78, 0x50); //Color.DimGray;//Color.FromArgb(0x54, 0xab, 0x11); //DimGray;//Color.DodgerBlue;
            this.moveSpeed = 2.5f;
            this.startOffect = 0f;
            this.chengzhi = "";

            //this.timer = null;
            this.components = null;
            this.InitializeComponent();
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.timer = new Timer();
            //this.timer.Interval = 600;
            //this.timer.Tick += new EventHandler(this.Timer_Tick);
            //this.timer.Start();

            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);

            //worker.WorkerSupportsCancellation = true;
            //if (!worker.IsBusy)
            //{
            //    worker.RunWorkerAsync();
            //}
            this.Disposed += PipeLine_Disposed;


            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;//.Center;
            this.sf.LineAlignment = StringAlignment.Center;

            pipeLineName = "";
        }

        BackgroundWorker worker = new BackgroundWorker();
        //public delegate void DoMeasureDelegate();
        delegate void aaa();


        // Fields
        private Color centerColor;
        private Color colorPipeLineCenter;
        //private IContainer components;
        private Color edgeColor;
        private Pen edgePen;
        private PipeLineStyle hslPipeLineStyle;
        private PipeTurnDirection hslPipeTurnLeft;
        private PipeTurnDirection hslPipeTurnRight;
        private bool isPipeLineActive;
        private float moveSpeed;
        private int pipeLineWidth;
        private float startOffect;
        //private Timer timer;

        ColorBlend colorBlend;
        GraphicsPath path = null;
        LinearGradientBrush brush = null;
        LinearGradientBrush brush2 = null;


        private StringFormat sf;
        Brush shuziColor = Brushes.White;//.Black;
        string chengzhi;
        // Methods
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!this.DesignMode)
            {
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
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
                    //LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, base.Height - 1), this.edgeColor, this.centerColor);
                    brush = new LinearGradientBrush(new Point(0, 0), new Point(0, base.Height - 1), this.edgeColor, this.centerColor);
                    brush.InterpolationColors = colorBlend;
                    if (this.hslPipeTurnLeft == PipeTurnDirection.Up)
                    {
                        this.PaintEllipse(e.Graphics, colorBlend, new Rectangle(0, -base.Height - 1, 2 * base.Height, 2 * base.Height), 90f, 90f);
                    }
                    else if (this.hslPipeTurnLeft == PipeTurnDirection.Down)
                    {
                        this.PaintEllipse(e.Graphics, colorBlend, new Rectangle(0, 0, 2 * base.Height, 2 * base.Height), 180f, 90f);
                    }
                    else
                    {
                        this.PaintRectangleBorderUpDown(e.Graphics, brush, this.edgePen, new Rectangle(0, 0, base.Height, base.Height));
                    }
                    if (this.hslPipeTurnRight == PipeTurnDirection.Up)
                    {
                        this.PaintEllipse(e.Graphics, colorBlend, new Rectangle((base.Width - 1) - (base.Height * 2), -base.Height - 1, 2 * base.Height, 2 * base.Height), 0f, 90f);
                    }
                    else if (this.hslPipeTurnRight == PipeTurnDirection.Down)
                    {
                        this.PaintEllipse(e.Graphics, colorBlend, new Rectangle((base.Width - 1) - (base.Height * 2), 0, 2 * base.Height, 2 * base.Height), 270f, 90f);
                    }
                    else
                    {
                        this.PaintRectangleBorderUpDown(e.Graphics, brush, this.edgePen, new Rectangle(base.Width - base.Height, 0, base.Height, base.Height));
                    }
                    if ((base.Width - (base.Height * 2)) >= 0)
                    {
                        this.PaintRectangleBorderUpDown(e.Graphics, brush, this.edgePen, new Rectangle(base.Height - 1, 0, (base.Width - (2 * base.Height)) + 2, base.Height));
                    }
                    brush.Dispose();
                    if (this.isPipeLineActive)
                    {
                        if (base.Width < base.Height)
                        {
                            path.AddLine(0, base.Height / 2, base.Height, base.Height / 2);
                        }
                        else
                        {
                            if (this.hslPipeTurnLeft == PipeTurnDirection.Up)
                            {
                                path.AddArc(new Rectangle(base.Height / 2, (-base.Height / 2) - 1, base.Height, base.Height), 180f, -90f);
                            }
                            else if (this.hslPipeTurnLeft == PipeTurnDirection.Down)
                            {
                                path.AddArc(new Rectangle(base.Height / 2, base.Height / 2, base.Height, base.Height), 180f, 90f);
                            }
                            else
                            {
                                path.AddLine(0, base.Height / 2, base.Height, base.Height / 2);
                            }
                            if ((base.Width - (base.Height * 2)) >= 0)
                            {
                                path.AddLine(base.Height, base.Height / 2, (base.Width - base.Height) - 1, base.Height / 2);
                            }
                            if (this.hslPipeTurnRight == PipeTurnDirection.Up)
                            {
                                path.AddArc(new Rectangle((base.Width - 1) - ((base.Height * 3) / 2), (-base.Height / 2) - 1, base.Height, base.Height), 90f, -90f);
                            }
                            else if (this.hslPipeTurnRight == PipeTurnDirection.Down)
                            {
                                path.AddArc(new Rectangle((base.Width - 1) - ((base.Height * 3) / 2), base.Height / 2, base.Height, base.Height), 270f, 90f);
                            }
                            else
                            {
                                path.AddLine((int)(base.Width - base.Height), (int)(base.Height / 2), (int)(base.Width - 1), (int)(base.Height / 2));
                            }
                        }
                    }
                }
                else
                {
                    //LinearGradientBrush brush2 = new LinearGradientBrush(new Point(0, 0), new Point(base.Width - 1, 0), this.edgeColor, this.centerColor);
                    brush2 = new LinearGradientBrush(new Point(0, 0), new Point(base.Width - 1, 0), this.edgeColor, this.centerColor);
                    brush2.InterpolationColors = colorBlend;
                    if (this.hslPipeTurnLeft == PipeTurnDirection.Left)
                    {
                        this.PaintEllipse(e.Graphics, colorBlend, new Rectangle(-base.Width - 1, 0, 2 * base.Width, 2 * base.Width), 270f, 90f);
                    }
                    else if (this.hslPipeTurnLeft == PipeTurnDirection.Right)
                    {
                        this.PaintEllipse(e.Graphics, colorBlend, new Rectangle(0, 0, 2 * base.Width, 2 * base.Width), 180f, 90f);
                    }
                    else
                    {
                        this.PaintRectangleBorderLeftRight(e.Graphics, brush2, this.edgePen, new Rectangle(0, 0, base.Width, base.Width));
                    }
                    if (this.hslPipeTurnRight == PipeTurnDirection.Left)
                    {
                        this.PaintEllipse(e.Graphics, colorBlend, new Rectangle(-base.Width - 1, base.Height - (base.Width * 2), 2 * base.Width, 2 * base.Width), 0f, 90f);
                    }
                    else if (this.hslPipeTurnRight == PipeTurnDirection.Right)
                    {
                        this.PaintEllipse(e.Graphics, colorBlend, new Rectangle(0, base.Height - (base.Width * 2), 2 * base.Width, 2 * base.Width), 90f, 90f);
                    }
                    else
                    {
                        this.PaintRectangleBorderLeftRight(e.Graphics, brush2, this.edgePen, new Rectangle(0, base.Height - base.Width, base.Width, base.Width));
                    }
                    if ((base.Height - (base.Width * 2)) >= 0)
                    {
                        this.PaintRectangleBorderLeftRight(e.Graphics, brush2, this.edgePen, new Rectangle(0, base.Width - 1, base.Width, (base.Height - (base.Width * 2)) + 2));
                    }
                    brush2.Dispose();
                    if (this.isPipeLineActive)
                    {
                        if (base.Width > base.Height)
                        {
                            path.AddLine(0, base.Height / 2, base.Height, base.Height / 2);
                        }
                        else
                        {
                            if (this.hslPipeTurnLeft == PipeTurnDirection.Left)
                            {
                                path.AddArc(new Rectangle(-base.Width / 2, (base.Width / 2) - 1, base.Width, base.Width), 270f, 90f);
                            }
                            else if (this.hslPipeTurnLeft == PipeTurnDirection.Right)
                            {
                                path.AddArc(new Rectangle(base.Width / 2, (base.Width / 2) - 1, base.Width, base.Width), 270f, -90f);
                            }
                            else
                            {
                                path.AddLine(base.Width / 2, 0, base.Width / 2, base.Width);
                            }
                            if ((base.Height - (base.Width * 2)) >= 0)
                            {
                                path.AddLine(base.Width / 2, base.Width, base.Width / 2, (base.Height - base.Width) - 1);
                            }
                            if (this.hslPipeTurnRight == PipeTurnDirection.Left)
                            {
                                path.AddArc(new Rectangle(-base.Width / 2, (base.Height - 1) - ((base.Width * 3) / 2), base.Width, base.Width), 0f, 90f);
                            }
                            else if (this.hslPipeTurnRight == PipeTurnDirection.Right)
                            {
                                path.AddArc(new Rectangle(base.Width / 2, (base.Height - 1) - ((base.Width * 3) / 2), base.Width, base.Width), -180f, -90f);
                            }
                            else
                            {
                                path.AddLine((int)(base.Width / 2), (int)(base.Height - base.Width), (int)(base.Width / 2), (int)(base.Height - 1));
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
                    e.Graphics.DrawPath(pen, path);
                }
                base.OnPaint(e);
                GC.Collect();
            }


            Rectangle 矩形2 = new Rectangle((int)0, (int)0, this.Width, this.Height);
            e.Graphics.DrawString(pipeLineName, Font, shuziColor, 矩形2, sf);
        }
        //protected override void OnLoad(EventArgs e)
        //{
        //    if(this.mo)
        //    base.OnLoad(e);
        //}
        private void PipeLine_Disposed(object sender, EventArgs e)
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

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                //this.timer = new Timer();
                //this.timer.Interval = 600;
                //this.timer.Tick += new EventHandler(this.Timer_Tick);
                //this.timer.Start();

                //worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                //worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.WorkerSupportsCancellation = true;
                if (!worker.IsBusy)
                {
                    worker.RunWorkerAsync();
                }

            }
            base.OnLoad(e);
        }

        //GraphicsPath gpath = null;
        //PathGradientBrush pgbrush = null;
        //private void PaintEllipse(Graphics g, ColorBlend paracolorBlend, Rectangle rect, float startAngle, float sweepAngle);
        private void PaintEllipse(Graphics g, ColorBlend paracolorBlend, Rectangle rect, float startAngle, float sweepAngle)
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
            gpath = null;
            pgbrush = null;
            GC.Collect();
        }



        //private void PaintRectangleBorder(Graphics g, Brush brush, Pen pen, Rectangle rectangle);
        private void PaintRectangleBorder(Graphics g, Brush brush, Pen pen, Rectangle rectangle)
        {
            this.PaintRectangleBorder(g, brush, pen, rectangle, true, true, true, true);
        }
        //private void PaintRectangleBorder(Graphics g, Brush brush, Pen pen, Rectangle rectangle, bool left, bool right, bool up, bool down);
        private void PaintRectangleBorder(Graphics g, Brush brush, Pen pen, Rectangle rectangle, bool left, bool right, bool up, bool down)
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
        private void PaintRectangleBorderLeftRight(Graphics g, Brush brush, Pen pen, Rectangle rectangle)
        {
            this.PaintRectangleBorder(g, brush, pen, rectangle, true, true, false, false);
        }
        //private void PaintRectangleBorderUpDown(Graphics g, Brush brush, Pen pen, Rectangle rectangle);
        private void PaintRectangleBorderUpDown(Graphics g, Brush brush, Pen pen, Rectangle rectangle)
        {
            this.PaintRectangleBorder(g, brush, pen, rectangle, false, false, true, true);
        }

        //private void Timer_Tick(object sender, EventArgs e);
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.isPipeLineActive)
            {
                this.startOffect -= this.moveSpeed;
                if ((this.startOffect <= -10f) || (this.startOffect >= 10f))
                {
                    this.startOffect = 0f;
                }
                base.Invalidate();
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
                        base.Invalidate();
                    }
                    System.Threading.Thread.Sleep(1500);
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

        [Browsable(true), Description("称值。"), DefaultValue(typeof(string), "称值"), Category("Appearance")]
        public string ChengZhi
        {
            get
            {
                return this.chengzhi;
            }
            set
            {
                this.chengzhi = value;
                //base.Invalidate();
            }
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
        [Browsable(true), Description("获取或设置管道控件的边缘颜色"), Category("Appearance"), DefaultValue(typeof(Color), "DimGray")]
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
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置管道控件的中心颜色"), Category("Appearance"), DefaultValue(typeof(Color), "LightGray")]
        public Color LineCenterColor
        {
            get
            {
                return this.centerColor;
            }
            set
            {
                this.centerColor = value;
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置管道线液体流动的速度，0为静止，正数为正向流动，负数为反向流动"), Category("Appearance"), DefaultValue((float)0.3f)]
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
        [Browsable(true), Description("获取或设置管道线是否激活液体显示"), Category("Appearance"), DefaultValue(false)]
        public bool PipeLineActive
        {
            get
            {
                return this.isPipeLineActive;
            }
            set
            {
                this.isPipeLineActive = value;
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置管道控件是否是横向的还是纵向的"), Category("Appearance"), DefaultValue(typeof(PipeLineStyle), "Horizontal")]
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
        [Browsable(true), Description("获取或设置管道控件左侧或是上方的端点朝向"), Category("Appearance"), DefaultValue(typeof(PipeTurnDirection), "None")]
        public PipeTurnDirection PipeTurnLeft
        {
            get
            {
                return this.hslPipeTurnLeft;
            }
            set
            {
                this.hslPipeTurnLeft = value;
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置管道控件左侧或是上方的端点朝向"), Category("Appearance"), DefaultValue(typeof(PipeTurnDirection), "None")]
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


        /// <summary>
        /// 皮带的名称
        /// </summary>
        string pipeLineName;

        [Browsable(true), Description("皮带的名称。"), DefaultValue(typeof(string), ""), Category("Appearance")]
        public string PipeLineName
        {
            get
            {
                return this.pipeLineName;
            }
            set
            {
                this.pipeLineName = value;
                //base.Invalidate();
            }
        }

        /// <summary>
        /// 使控件的整个图面无效并导致重绘控件。
        /// </summary>
        public void InvalidateNew()
        {
            base.Invalidate();
        }
    }

    //public enum PipeTurnDirection
    //{
    //    Down = 2,
    //    Left = 3,
    //    None = 5,
    //    Right = 4,
    //    Up = 1
    //}
    //public enum PipeLineStyle
    //{
    //    Horizontal = 1,
    //    Vertical = 2
    //}



}
