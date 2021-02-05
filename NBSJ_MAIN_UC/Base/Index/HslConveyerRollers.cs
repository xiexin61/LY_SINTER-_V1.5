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
    public partial class HConveyerRollers : UserControl
    {
        public HConveyerRollers()
        {
            this.sf = null;
            this.moveSpeed = 0.3f;
            this.startAngle = 0f;
            //this.startOffect = 0f;
            this.timer = null;
            this.circularRadius = 20f;
            this.density = 1f;
            this.upPercent = 0f;
            this.rightPercent = 0f;
            this.conveyerStyle = HConveyerStyle.Horizontal;
            //this.components = null;
            this.InitializeComponent();
            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.ForeColor = Color.FromArgb(0x8e, 0xc4, 0xd8);
            this.timer = new Timer();
            this.timer.Interval = 50;
            this.timer.Tick += new EventHandler(this.Timer_Tick);
            this.timer.Start();


        }

        // Fields
        private float circularRadius;
        //private IContainer components;
        private HConveyerStyle conveyerStyle;
        private float density;
        private float moveSpeed;
        private float rightPercent;
        private StringFormat sf;
        private float startAngle;
        //private float startOffect;
        private Timer timer;
        private float upPercent;

        ////// Methods
        ////protected override void Dispose(bool disposing);
        ////private void InitializeComponent();
        //protected override void OnPaint(PaintEventArgs e);
        protected override void OnPaint(PaintEventArgs e)
        {
            //if (Authorization.CheckAuthorization())
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                int num = 5;
                int num2 = 5;
                if (this.conveyerStyle == HConveyerStyle.Horizontal)
                {
                    this.PaintMain(e.Graphics, (float)base.Width, (float)base.Height, (float)num, (float)num2);
                }
                else if (this.conveyerStyle == HConveyerStyle.Upslope)
                {
                    PointF tf = new PointF((num + num2) + this.circularRadius, (((base.Height - 1) - num) - num2) - this.circularRadius);
                    PointF tf2 = new PointF((((base.Width - num) - num2) - 1) - this.circularRadius, (num + num2) + this.circularRadius);
                    float num3 = (float)Math.Sqrt(Math.Pow((double)(tf2.Y - tf.Y), 2.0) + Math.Pow((double)(tf2.X - tf.X), 2.0));
                    float num4 = (float)((Math.Acos((double)((tf2.X - tf.X) / num3)) * 180.0) / 3.1415926535897931);
                    e.Graphics.TranslateTransform(tf.X, tf.Y);
                    e.Graphics.RotateTransform(-num4);
                    e.Graphics.TranslateTransform((-num - num2) - this.circularRadius, (-num - num2) - this.circularRadius);
                    this.PaintMain(e.Graphics, ((num3 + (2 * num)) + (2 * num2)) + (2f * this.circularRadius), ((2 * num) + (2 * num2)) + (2f * this.circularRadius), (float)num, (float)num2);
                    e.Graphics.ResetTransform();
                }
                else
                {
                    PointF tf3 = new PointF((num + num2) + this.circularRadius, (num + num2) + this.circularRadius);
                    PointF tf4 = new PointF((((base.Width - num) - num2) - 1) - this.circularRadius, (((base.Height - 1) - num) - num2) - this.circularRadius);
                    float num5 = (float)Math.Sqrt(Math.Pow((double)(tf4.Y - tf3.Y), 2.0) + Math.Pow((double)(tf4.X - tf3.X), 2.0));
                    float angle = (float)((Math.Acos((double)((tf4.X - tf3.X) / num5)) * 180.0) / 3.1415926535897931);
                    e.Graphics.TranslateTransform(tf3.X, tf3.Y);
                    e.Graphics.RotateTransform(angle);
                    e.Graphics.TranslateTransform((-num - num2) - this.circularRadius, (-num - num2) - this.circularRadius);
                    this.PaintMain(e.Graphics, ((num5 + (2 * num)) + (2 * num2)) + (2f * this.circularRadius), ((2 * num) + (2 * num2)) + (2f * this.circularRadius), (float)num, (float)num2);
                    e.Graphics.ResetTransform();
                }
                base.OnPaint(e);
            }
        }




        //private void PaintMain(Graphics g, float width, float height, float margin, float offect);
        private void PaintMain(Graphics g, float width, float height, float margin, float offect)
        {
            g.TranslateTransform(0f, height * this.upPercent);
            float num = height * this.upPercent;
            float num2 = width * this.rightPercent;
            height *= 1f - this.upPercent;
            width *= 1f - this.rightPercent;
            float num3 = ((height - (margin * 2f)) - (offect * 2f)) / 2f;
            Pen pen = new Pen(this.ForeColor, 1f);
            Pen pen2 = new Pen(this.ForeColor, (num3 > 100f) ? ((float)7) : ((num3 > 20f) ? ((float)5) : ((num3 > 6f) ? ((float)3) : ((float)1))));
            pen2.StartCap = LineCap.Round;
            pen2.EndCap = LineCap.Round;
            Brush brush = new SolidBrush(this.ForeColor);
            GraphicsPath path = new GraphicsPath();
            path.AddArc(margin, margin, (num3 * 2f) + (offect * 2f), (num3 * 2f) + (offect * 2f), 90f, 180f);
            path.AddLine((margin + offect) + num3, margin, (((width - margin) - offect) - num3) - 1f, margin);
            path.AddArc(((width - margin) - (num3 * 2f)) - (offect * 2f), margin, (num3 * 2f) + (offect * 2f), (num3 * 2f) + (offect * 2f), -90f, 180f);
            path.CloseFigure();
            g.DrawPath(pen, path);
            g.DrawEllipse(pen, (float)(margin + offect), (float)(margin + offect), (float)(num3 * 2f), (float)(num3 * 2f));
            g.DrawEllipse(pen, (float)(((width - margin) - (num3 * 2f)) - offect), (float)(margin + offect), (float)(num3 * 2f), (float)(num3 * 2f));
            g.TranslateTransform((margin + offect) + num3, (margin + offect) + num3);
            g.RotateTransform(this.startAngle);
            g.DrawLine(pen2, (float)(-num3 / 2f), (float)0f, (float)(num3 / 2f), (float)0f);
            g.DrawLine(pen2, (float)0f, (float)(-num3 / 2f), (float)0f, (float)(num3 / 2f));
            g.RotateTransform(-this.startAngle);
            g.TranslateTransform((-margin - offect) - num3, (-margin - offect) - num3);
            g.TranslateTransform(((width - margin) - num3) - offect, (margin + offect) + num3);
            g.RotateTransform(this.startAngle);
            g.DrawLine(pen2, (float)(-num3 / 2f), (float)0f, (float)(num3 / 2f), (float)0f);
            g.DrawLine(pen2, (float)0f, (float)(-num3 / 2f), (float)0f, (float)(num3 / 2f));
            g.RotateTransform(-this.startAngle);
            g.TranslateTransform(((-width + margin) + num3) + offect, (-margin - offect) - num3);
            float num4 = ((width - (margin * 2f)) - (num3 * 4f)) - (offect * 2f);
            int num5 = (int)(((num4 * this.density) / num3) / 2f);
            float num6 = num4 / ((float)num5);
            for (int i = 0; i < num5; i++)
            {
                g.TranslateTransform((((margin + offect) + (num3 * 2f)) + (num6 * i)) + (num6 / 2f), (margin + offect) + num3);
                g.RotateTransform(this.startAngle);
                g.DrawLine(pen2, (float)(-num3 / 2f), (float)0f, (float)(num3 / 2f), (float)0f);
                g.DrawLine(pen2, (float)0f, (float)(-num3 / 2f), (float)0f, (float)(num3 / 2f));
                g.RotateTransform(-this.startAngle);
                g.DrawEllipse(pen, -num3, -num3, num3 * 2f, num3 * 2f);
                g.TranslateTransform((((-margin - offect) - (num3 * 2f)) - (num6 * i)) - (num6 / 2f), (-margin - offect) - num3);
            }
            if ((num > 0f) && (num2 > 0f))
            {
                PointF[] points = new PointF[] { new PointF((margin + offect) + num3, margin), new PointF(((margin + offect) + num3) + num2, -num + margin), new PointF((((width - margin) - num3) - offect) + num2, -num + margin), new PointF(((width - margin) - num3) - offect, margin) };
                g.DrawLines(pen, points);
                g.DrawLine(pen, (float)(((width - margin) - num3) - offect), (float)(height - margin), (float)((((width - margin) - num3) - offect) + num2), (float)((height - margin) - num));
                g.DrawArc(pen, (float)((((((width - margin) - num3) - offect) + num2) - num3) - offect), (float)(-num + margin), (float)((num3 * 2f) + (offect * 2f)), (float)((num3 * 2f) + (offect * 2f)), (float)-90f, (float)180f);
            }
            path.Dispose();
            pen.Dispose();
            pen2.Dispose();
            brush.Dispose();
            g.TranslateTransform(0f, -height * this.upPercent);
        }




        //private void Timer_Tick(object sender, EventArgs e);
        private void Timer_Tick(object sender, EventArgs e)
        {
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
        [Browsable(true), Description("获取或设置传送带的样式，水平，还是上坡，还是下坡"), Category("Appearance"), DefaultValue(1)]
        public HConveyerStyle ConveyerStyle
        {
            get
            {
                return this.conveyerStyle;
            }
            set
            {
                this.conveyerStyle = value;
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置传送带的滚筒密度，默认是1，完全分布，大于1则表示更密，小于1表示更稀疏"), Category("Appearance"), DefaultValue((float)1f)]
        public float DistributeDensity
        {
            get
            {
                return this.density;
            }
            set
            {
                this.density = value;
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
        [Browsable(true), Description("获取或设置右边的偏移量信息，用于3D的传送带显示"), Category("Appearance"), DefaultValue((float)0f)]
        public float RightPercent
        {
            get
            {
                return this.rightPercent;
            }
            set
            {
                this.rightPercent = value;
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
        [Browsable(true), Description("获取或设置上边的偏移量信息，用于3D的传送带显示"), Category("Appearance"), DefaultValue((float)0f)]
        public float UpPercent
        {
            get
            {
                return this.upPercent;
            }
            set
            {
                this.upPercent = value;
                base.Invalidate();
            }
        }




    }

    public enum HConveyerStyle
    {
        Downslope = 2,
        Horizontal = 1,
        Upslope = 3
    }



}
