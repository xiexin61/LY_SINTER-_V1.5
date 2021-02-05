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
    public partial class BottleUC : UserControl
    {

        public BottleUC()
        {
            //InitializeComponent();   
            this.value = 70.0;
            this.isOpen = false;
            this.bottleTag = "";
            this.headTag = "矿料";
            this.themeStyle = HslThemeStyle.Light;
            this.dockHeight = 30f;
            //this.components = null;
            this.InitializeComponent();
            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.themeStyle = HslThemeStyle.Default;
        }
        private string bottleTag;
        //private IContainer components;
        private float dockHeight;
        private string headTag;
        private bool isOpen;
        private StringFormat sf;
        private HslThemeStyle themeStyle;
        private double value;

        //protected override void Dispose(bool disposing);

        ////private void InitializeComponent();
        //protected override void OnPaint(PaintEventArgs e);


        protected override void OnPaint(PaintEventArgs e)
        {

            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            if ((base.Width >= 15) && (base.Height >= 15))
            {
                float x = ((float)base.Width) / 4f;
                float y = (base.Height - this.dockHeight) - ((((base.Height - this.dockHeight) - 20f) * Convert.ToSingle(this.value)) / 100f);
                int num3 = (base.Width / 50) + 3;
                GraphicsPath path = new GraphicsPath();
                PointF[] points = new PointF[] { new PointF(0f, 20f), new PointF(0f, base.Height - this.dockHeight), new PointF(x, (float)(base.Height - 1)), new PointF(x * 2, base.Height - this.dockHeight), new PointF(x * 3, (float)(base.Height - 1)), new PointF((float)(base.Width - 1), base.Height - this.dockHeight), new PointF((float)(base.Width - 1), 20f), new PointF(0f, 20f) };
                path.AddPolygon(points);
                LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 20), new Point(base.Width - 1, 20), Color.FromArgb(0x8e, 0xc4, 0xd8), Color.FromArgb(240, 240, 240));
                ColorBlend blend = new ColorBlend();
                float[] singleArray1 = new float[3];
                singleArray1[1] = 0.5f;
                singleArray1[2] = 1f;
                blend.Positions = singleArray1;
                if (this.themeStyle == HslThemeStyle.Light)
                {
                    blend.Colors = new Color[] { Color.FromArgb(0x8e, 0xc4, 0xd8), Color.FromArgb(240, 240, 240), Color.FromArgb(0x8e, 0xc4, 0xd8) };
                }
                else if (this.themeStyle == HslThemeStyle.Default)
                {
                    blend.Colors = new Color[] { Color.FromArgb(0xc0, 0xc0, 0xc0), Color.FromArgb(240, 240, 240), Color.FromArgb(0xc0, 0xc0, 0xc0) };
                }
                else
                {
                    //blend.Colors = new Color[] { Color.FromArgb(0x36, 100, 100), Color.FromArgb(110, 0xa1, 0xa3), Color.FromArgb(0x36, 100, 100) };
                    blend.Colors = new Color[] { Color.FromArgb(0xc0, 0xc0, 0xc0), Color.FromArgb(240, 240, 240), Color.FromArgb(0xc0, 0xc0, 0xc0) };
                }
                brush.InterpolationColors = blend;
                graphics.FillPath(brush, path);//料仓区域
                if (this.themeStyle == HslThemeStyle.Light)//背景
                {
                    using (Brush brush2 = new SolidBrush(Color.FromArgb(0x97, 0xe8, 0xf4)))
                    {
                        graphics.FillEllipse(brush2, 1, 20 - num3, base.Width - 2, num3 * 2);
                    }
                }
                else if (this.themeStyle == HslThemeStyle.Default)
                {
                    using (Brush brush2 = new SolidBrush(Color.FromArgb(0xc0, 0xc0, 0xc0)))
                    {
                        graphics.FillEllipse(brush2, 1, 20 - num3, base.Width - 2, num3 * 2);
                    }
                }
                else
                {
                    using (Brush brush2 = new SolidBrush(Color.FromArgb(0xc0, 0xc0, 0xc0)))
                    {
                        graphics.FillEllipse(brush2, 1, 20 - num3, base.Width - 2, num3 * 2);
                    }
                    //using (Brush brush3 = new SolidBrush(Color.FromArgb(0x36, 100, 100)))
                    //{
                    //    graphics.FillEllipse(brush3, 1, 20 - num3, base.Width - 2, num3 * 2);
                    //}
                }
                path.Reset();
                PointF[] tfArray2 = new PointF[] { new PointF(0f, y), new PointF(0f, base.Height - this.dockHeight), new PointF(x, (float)(base.Height - 1)), new PointF(x * 2, base.Height - this.dockHeight), new PointF(x * 3, (float)(base.Height - 1)), new PointF((float)(base.Width - 1), base.Height - this.dockHeight), new PointF((float)(base.Width - 1), y), new PointF(0f, y) };
                path.AddPolygon(tfArray2);
                if (this.themeStyle == HslThemeStyle.Light)
                {
                    blend.Colors = new Color[] { Color.FromArgb(0xc2, 190, 0x4d), Color.FromArgb(0xe2, 0xdd, 0x62), Color.FromArgb(0xc2, 190, 0x4d) };
                }
                //else if (this.themeStyle == HslThemeStyle.Default)
                //{
                //    blend.Colors = new Color[] { Color.FromArgb(0x28, 0x27, 0x26), Color.FromArgb(240, 240, 240), Color.FromArgb(0x28, 0x27, 0x26) };
                //}
                else
                {
                    blend.Colors = new Color[] { Color.FromArgb(0xc2, 190, 0x4d), Color.FromArgb(0xe2, 0xdd, 0x62), Color.FromArgb(0xc2, 190, 0x4d) };
                }
                brush.InterpolationColors = blend;
                graphics.FillPath(brush, path);//有料区域
                brush.Dispose();
                using (Brush brush4 = new SolidBrush(Color.FromArgb(243, 245, 139)))//黄色
                {
                    graphics.FillEllipse(brush4, 1f, y - num3, (float)(base.Width - 2), (float)(num3 * 2));//有料上部圆形区域
                }
                path.Reset();
                PointF[] tfArray3 = new PointF[] { new PointF(0f, base.Height - this.dockHeight), new PointF(x, (float)(base.Height - 1)), new PointF(x * 2, base.Height - this.dockHeight), new PointF(x * 3, (float)(base.Height - 1)), new PointF((float)(base.Width - 1), base.Height - this.dockHeight) };
                path.AddPolygon(tfArray3);
                // path.AddArc(0f, (base.Height - this.dockHeight) - num3, (float)base.Width, (float)(num3 * 2), 0f, 180f);
                using (Brush brush5 = new SolidBrush(Color.FromArgb(0xb8, 180, 0x43)))
                {
                    graphics.FillPath(brush5, path);//有料下部界限
                }
                path.Reset();
                PointF[] tfArray4 = new PointF[] { new PointF(((float)base.Width) / 8f, base.Height - (this.dockHeight / 2f)), new PointF(((float)base.Width) / 8f, (float)(base.Height - 1)), new PointF((base.Width * 3f) / 8f, (float)(base.Height - 1)), new PointF((base.Width * 3f) / 8f, base.Height - (this.dockHeight / 2f)) };
                path.AddLines(tfArray4);
                path.AddArc(((float)base.Width) / 8f, ((base.Height - (this.dockHeight / 2f)) - num3) - 1f, ((float)base.Width) / 4f, (float)(num3 * 2), 0f, 180f);
                graphics.FillPath(Brushes.DimGray, path);//左边黑色部分

                path.Reset();
                PointF[] tfArray5 = new PointF[] { new PointF((base.Width * 5f) / 8f, base.Height - (this.dockHeight / 2f)), new PointF((base.Width * 5f) / 8f, (float)(base.Height - 1)), new PointF((base.Width * 7f) / 8f, (float)(base.Height - 1)), new PointF((base.Width * 7f) / 8f, base.Height - (this.dockHeight / 2f)) };
                path.AddLines(tfArray5);
                path.AddArc((base.Width * 5f) / 8f, ((base.Height - (this.dockHeight / 2f)) - num3) - 1f, ((float)base.Width) / 4f, (float)(num3 * 2), 0f, 180f);
                graphics.FillPath(Brushes.Green, path);//右黑色部分




                if (!string.IsNullOrEmpty(this.bottleTag))
                {
                    graphics.FillRectangle(Brushes.White, new Rectangle(-10, 0x1a, base.Width + 20, 20));
                    graphics.DrawString(this.bottleTag, this.Font, Brushes.Black, new Rectangle(-10, 0x1a, base.Width + 20, 20), this.sf);
                }
                if (!string.IsNullOrEmpty(this.headTag))
                {
                    graphics.DrawString(this.headTag, this.Font, Brushes.DimGray, new Rectangle(-10, 0, base.Width + 20, 20), this.sf);
                }
                path.Dispose();
                points = null;
                blend = null;
                singleArray1 = null;
                tfArray2 = null;
                tfArray3 = null;
                tfArray4 = null;
                tfArray5 = null;
                base.OnPaint(e);
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
        [Browsable(true), Description("获取或设置瓶子的标签信息，用于绘制在瓶子上的信息。"), DefaultValue(typeof(string), ""), Category("Appearance")]
        public string BottleTag
        {
            get
            {
                return this.bottleTag;
            }
            set
            {
                this.bottleTag = value;
                base.Invalidate();
            }
        }
        [Browsable(true), DefaultValue((float)30f), Description("获取或设置控件底座的高度"), Category("Appearance")]
        public float DockHeight
        {
            get
            {
                return this.dockHeight;
            }
            set
            {
                this.dockHeight = value;
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置瓶子的备注信息，用于绘制在瓶子顶部的信息。"), DefaultValue(typeof(string), "矿料1"), Category("Appearance")]
        public string HeadTag
        {
            get
            {
                return this.headTag;
            }
            set
            {
                this.headTag = value;
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置瓶子是否处于打开的状态。"), DefaultValue(typeof(bool), "false"), Category("Appearance")]
        public bool IsOpen
        {
            get
            {
                return this.isOpen;
            }
            set
            {
                this.isOpen = value;
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置当前控件的主题色"), DefaultValue(typeof(HslThemeStyle), "Light"), Category("Appearance")]
        public HslThemeStyle ThemeStyle
        {
            get
            {
                return this.themeStyle;
            }
            set
            {
                this.themeStyle = value;
                base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置瓶子的液位值。"), DefaultValue(typeof(double), "60"), Category("Appearance")]
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if ((value >= 0.0) && (value <= 100.0))
                {
                    this.value = value;
                    base.Invalidate();
                }
            }
        }
    }
}
