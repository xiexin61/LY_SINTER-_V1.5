using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing;
using System.Windows.Forms;

namespace UserControlIndex
{

    public class BottleItem
    {

        private AbstractBottle bottleobj;
        public AbstractBottle BottleObj
        {

            get
            {
                if (bottleobj == null)
                {
                    switch (bottleType)
                    {
                        case BottleType.BottleSingle:
                            bottleobj = new BottleOne();
                            break;
                        case BottleType.BootleDouble:
                            bottleobj = new BottleTwo();
                            break;
                    }
                }
                return bottleobj;
            }
            set
            {

                bottleobj = value;
            }

        }
        private BottleType bottleType = BottleType.BottleSingle;
        public BottleType BottleType
        {

            get
            {
                return bottleType;
            }
            set
            {
                bottleType = value;
                if (BottleObj != null)
                {
                    BottleObj.Dispose();
                    BottleObj = null;
                }
                switch (value)
                {
                    case BottleType.BottleSingle:
                        BottleObj = new BottleOne();
                        break;
                    case BottleType.BootleDouble:
                        BottleObj = new BottleTwo();
                        break;
                }
            }
        }
       
        public string BottleDesc
        {

            get;
            set;
        }
        public BottleItem()
        {

        }
        public BottleItem(BottleType bottletype)
        {
            BottleType = bottletype;
        }

    }

    public enum BottleType
    {
        BottleSingle,
        BootleDouble
    }

    public enum HslThemeStyle
    {
        Light = 1,
        Dark = 2,
        Default = 0
    }

    public abstract class AbstractBottle : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected int headHeight;
        protected string bottleTag;
        protected float dockHeight;
        protected string headTag;
        protected StringFormat sf;
        protected HslThemeStyle themeStyle;
        protected double value;
        protected LinearGradientBrush brush;
        protected ColorBlend blend = new ColorBlend();
        public AbstractBottle()
        {
            headHeight = 5;//20
            setValue = "123";
            currentValue = "456";

            this.value = 100;
            this.themeStyle = HslThemeStyle.Light;
            this.dockHeight = 30f;
            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;

            float[] singleArray1 = new float[3];
            singleArray1[1] = 0.5f;
            singleArray1[2] = 1f;
            blend.Positions = singleArray1;
            blend.Colors = new Color[] { Color.FromArgb(0xc0, 0xc0, 0xc0), Color.FromArgb(240, 240, 240), Color.FromArgb(0xc0, 0xc0, 0xc0) };

            setT_SL_Left = Brushes.DimGray;
            setT_SL_Right = Brushes.DimGray;

            if (CangHao <= 7)//无料时的背景填充色
            {
                Lc1BackColorTop = Color.FromArgb(184, 90, 154);
                Lc2BackColors = new Color[] { Color.FromArgb(122, 2, 60), Color.FromArgb(122, 2, 60), Color.FromArgb(122, 2, 60) };//无料时的背景填充色

                Lc3BackColor = Color.FromArgb(0x88, 0x88, 0x88);
                Lc4BackColorTop = Color.FromArgb(0xb5, 0xb1, 0xb1);
                Lc5BackColors = new Color[] { Color.FromArgb(0x73, 0x73, 0x70), Color.FromArgb(0xb3, 0xaf, 0xaf), Color.FromArgb(0x73, 0x73, 0x70) };//中部的填充颜色737370

            }
            else
            {
                Lc1BackColorTop = Color.FromArgb(0x33, 0xcc, 0x66);
                Lc2BackColors = new Color[] { Color.FromArgb(0x33, 0x99, 0x33), Color.FromArgb(0x33, 0x99, 0x33), Color.FromArgb(0x33, 0x99, 0x33) };//无料时的背景填充色

                Lc3BackColor = Color.FromArgb(0x88, 0x88, 0x88);
                Lc4BackColorTop = Color.FromArgb(0xb5, 0xb1, 0xb1);
                Lc5BackColors = new Color[] { Color.FromArgb(0x73, 0x73, 0x70), Color.FromArgb(0xb3, 0xaf, 0xaf), Color.FromArgb(0x73, 0x73, 0x70) };//中部的填充颜色737370

            }


        }
        /// <summary>
        /// 设定值
        /// </summary>
        protected string setValue;
        /// <summary>
        /// 当前值
        /// </summary>
        protected string currentValue;


        /// <summary>
        /// 料仓 顶部椭圆的颜色
        /// </summary>
        public Color Lc1BackColorTop = Color.FromArgb(184, 90, 154);//Color.FromArgb(0x33, 0xcc, 0x66)))//c0c0c0 椭圆
        /// <summary>
        /// 料仓 无料时的背景填充色
        /// </summary>
        public Color[] Lc2BackColors = new Color[] { Color.FromArgb(122, 2, 60), Color.FromArgb(122, 2, 60), Color.FromArgb(122, 2, 60) };//无料时的背景填充色

        /// <summary>
        /// 底部三角的填充颜色
        /// </summary>
        public Color Lc3BackColor = Color.FromArgb(0x88, 0x88, 0x88);

        /// <summary>
        /// 中间的椭圆的填充颜色
        /// </summary>
        public Color Lc4BackColorTop = Color.FromArgb(0xb5, 0xb1, 0xb1);
        /// <summary>
        /// 中部的填充颜色
        /// </summary>
        public Color[] Lc5BackColors = new Color[] { Color.FromArgb(0x73, 0x73, 0x70), Color.FromArgb(0xb3, 0xaf, 0xaf), Color.FromArgb(0x73, 0x73, 0x70) };//中部的填充颜色737370


        /// <summary>
        /// 设定值
        /// </summary>
        public string SetValue
        {
            get
            {
                return this.setValue;
            }
            set
            {
                this.setValue = value;

            }
        }
        /// <summary>
        /// 当前值
        /// </summary>
        public string CurrentValue
        {
            get
            {
                return this.currentValue;
            }
            set
            {
                this.currentValue = value;

            }
        }

        /// <summary>
        /// 设定值
        /// </summary>
        protected string setValue2;
        /// <summary>
        /// 当前值
        /// </summary>
        protected string currentValue2;
        /// <summary>
        /// 设定值
        /// </summary>
        public string SetValue2
        {
            get
            {
                return this.setValue2;
            }
            set
            {
                this.setValue2 = value;

            }
        }
        /// <summary>
        /// 当前值
        /// </summary>
        public string CurrentValue2
        {
            get
            {
                return this.currentValue2;
            }
            set
            {
                this.currentValue2 = value;

            }
        }

        /// <summary>
        /// 下料口启停信号
        /// </summary>
        protected Brush setT_SL_Left;
        /// <summary>
        /// 下料口启停信号
        /// </summary>
        protected Brush setT_SL_Right;

        /// <summary>
        /// 下料口启停信号
        /// </summary>
        public Brush SetT_SL_Left
        {
            get
            {
                return this.setT_SL_Left;
            }
            set
            {
                this.setT_SL_Left = value;

            }
        }

        /// <summary>
        /// 下料口启停信号
        /// </summary>
        public Brush SetT_SL_Right
        {
            get
            {
                return this.setT_SL_Right;
            }
            set
            {
                this.setT_SL_Right = value;

            }
        }


        public abstract void OnPaint(Graphics graphics, int Width, int Height, Point loaclPoint,string desc);
        public abstract void Dispose();
        public string BottleTag
        {
            get
            {
                return this.bottleTag;
            }
            set
            {
                this.bottleTag = value;
                
            }
        }
        public float DockHeight
        {
            get
            {
                return this.dockHeight;
            }
            set
            {
                this.dockHeight = value;
            }
        }
        public string HeadTag
        {
            get
            {
                return this.headTag;
            }
            set
            {
                this.headTag = value;
            }
        }
        int canghao = 0;
        /// <summary>
        /// 仓号
        /// </summary>
        public int CangHao
        {
            get
            {
                return this.canghao;
            }
            set
            {
                this.canghao = value;
            }
        }
        public HslThemeStyle ThemeStyle
        {
            get
            {
                return this.themeStyle;
            }
            set
            {
                this.themeStyle = value;
            }
        }
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                if (value <= 0.0)
                {
                    this.value = 0;
                }
                else if (value >= 100.0)
                {
                    this.value = 100;
                }
                else
                {
                    this.value = value;
                }

            }
        }


        private Font font = new Font(FontFamily.GenericSansSerif, 9F);
        public Font Font
        {
            get
            {
                return font;
            }

            set
            {
                font = value;
            }
        }
        private int width = 0;
        public int Width
        {
            get { return width; }
            protected set
            {
                if (value >= 15)
                {
                    width = value;
                }
                else
                {
                    width = 15;
                }
            }

        }
        private int height = 0;
        public int Height
        {
            get
            {
                return height;
            }
            protected set
            {
                if (value >= 15)
                {
                    height = value;
                }
                else
                {
                    height = 15;
                }
            }

        }

        protected bool IsChangeSize { get; set; }

        private Dictionary<string, GraphicsPath> graphicsPathDic = new Dictionary<string, GraphicsPath>();
        public Dictionary<string, GraphicsPath> GraphicsPathDic
        {
            get
            {
                return graphicsPathDic;
            }
        }

        /// <summary>
        /// Y坐标开始的位置
        /// </summary>
        protected double YStartPosition = 1f;//20f
    }

    public class BottleOne : AbstractBottle
    {
        public BottleOne() : base()
        { }
        public override void Dispose()
        {

        }

        public override void OnPaint(Graphics graphics, int width, int height, Point loaclPoint,string desc)
        {
            //headHeight = 5;//20
            if (graphics == null) return;
            IsChangeSize = Width != width || Height != height || GraphicsPathDic.Count == 0 ? true : false;
            Width = width;
            Height = height-46;
            float x = ((float)Width) / 2f;
            float y = (Height - this.dockHeight) - ((((Height - this.dockHeight) - headHeight) * Convert.ToSingle(this.value)) / 100f);
            int num3 = (Width / 50) + 3;
            //if (true)
            {

                foreach (GraphicsPath item in GraphicsPathDic.Values)
                {
                    item.Dispose();
                }
                GraphicsPathDic.Clear();
                GraphicsPath path = new GraphicsPath();

                //图像绘制的坐标点
                PointF[] points = new PointF[] { new PointF(loaclPoint.X + 0f, headHeight), new PointF(loaclPoint.X + 0f, Height - this.dockHeight), new PointF(loaclPoint.X + x, (float)(Height - 1)), new PointF((float)(loaclPoint.X + Width - 1), Height - this.dockHeight), new PointF((float)(loaclPoint.X + Width - 1), headHeight), new PointF(loaclPoint.X + 0f, headHeight) };
                path.AddPolygon(points);
                brush = new LinearGradientBrush(new Point(0, headHeight), new Point(Width - 1, headHeight), Color.FromArgb(0x8e, 0xc4, 0xd8), Color.FromArgb(240, 240, 240));
                //blend.Colors = new Color[] { Color.FromArgb(0x33, 0x99, 0x33), Color.FromArgb(0x33, 0x99, 0x33), Color.FromArgb(0x33, 0x99, 0x33) };//无料时的背景填充色
                blend.Colors = Lc2BackColors;//无料时的背景填充色

                brush.InterpolationColors = blend;
                graphics.FillPath(brush, path);
                GraphicsPathDic.Add("ID0", path);
                using (Brush brush2 = new SolidBrush(Lc1BackColorTop))//c0c0c0 椭圆  顶部的椭圆的填充颜色
                {
                    graphics.FillEllipse(brush2, loaclPoint.X + 1, headHeight - num3, Width - 2, num3 * 2);
                   
                }
                points = null;

                path = new GraphicsPath();
                points = new PointF[] { new PointF(loaclPoint.X + 0f, y), new PointF(loaclPoint.X + 0f, Height - this.dockHeight), new PointF(loaclPoint.X + x, (float)(Height - 1)), new PointF((float)(loaclPoint.X + Width - 1), Height - this.dockHeight), new PointF((float)(loaclPoint.X + Width - 1), y), new PointF(loaclPoint.X + 0f, y) };
                path.AddPolygon(points);
                //blend.Colors = new Color[] { Color.FromArgb(0xc2, 190, 0x4d), Color.FromArgb(0xe2, 0xdd, 0x62), Color.FromArgb(0xc2, 190, 0x4d) };//中部的填充颜色
                //blend.Colors = new Color[] { Color.FromArgb(0x73, 0x73, 0x70), Color.FromArgb(0xb3, 0xaf, 0xaf), Color.FromArgb(0x73, 0x73, 0x70) };//中部的填充颜色737370
                blend.Colors = Lc5BackColors;//中部的填充颜色
                brush.InterpolationColors = blend;
                graphics.FillPath(brush, path);
                GraphicsPathDic.Add("ID1", path);
                //using (Brush brush4 = new SolidBrush(Color.FromArgb(0xf3, 0xf5, 0x8b)))//f3f58b 黄色椭圆  中间的椭圆的填充颜色
                //using (Brush brush4 = new SolidBrush(Color.FromArgb(0xb5, 0xb1, 0xb1)))//b1b1b1 黄色椭圆  中间的椭圆的填充颜色
                using (Brush brush4 = new SolidBrush(Lc4BackColorTop))//b1b1b1 黄色椭圆  中间的椭圆的填充颜色
                {
                    graphics.FillEllipse(brush4, loaclPoint.X + 1f, y - num3, (float)(Width - 2), (float)(num3 * 2));
                }
                points = null;

                path = new GraphicsPath();
                points = new PointF[] { new PointF(loaclPoint.X + 0f, Height - this.dockHeight), new PointF(loaclPoint.X + x, (float)(Height - 1)), new PointF((float)(loaclPoint.X + Width - 1), Height - this.dockHeight) };
                path.AddPolygon(points);
                //添加底部椭圆
                path.AddArc(loaclPoint.X + 0f, (Height - this.dockHeight) - num3, (float)Width-2, (float)(num3 * 2), 0f, 180f);
                //using (Brush brush5 = new SolidBrush(Color.FromArgb(0xb8, 180, 0x43)))//b8b443 底部三角的填充颜色
                //using (Brush brush5 = new SolidBrush(Color.FromArgb(0x88, 0x88, 0x88)))//b8b443 底部三角的填充颜色
                using (Brush brush5 = new SolidBrush(Lc3BackColor))//b8b443 底部三角的填充颜色
                {
                    graphics.FillPath(brush5, path);
                }
                GraphicsPathDic.Add("ID2", path);
                points = null;

                path = new GraphicsPath();
                points = new PointF[] { new PointF(((float)Width) / 3f + loaclPoint.X, Height - (this.dockHeight / 2.5f)), new PointF(((float)Width) / 3f + loaclPoint.X, (float)(Height - 1)), new PointF((Width * 2f / 3f) + loaclPoint.X, (float)(Height - 1)), new PointF((Width * 2f / 3f) + loaclPoint.X, Height - (this.dockHeight / 2.5f)) };
                path.AddLines(points);
                path.AddArc(((float)Width) / 3f + loaclPoint.X, ((Height - (this.dockHeight / 2f)) - num3) - 1f, ((float)Width) / 3f , (float)(num3 * 3), 0f, 180f);//给下面黑色部分加弧度
                //graphics.FillPath(Brushes.DimGray, path); //左边黑色部分
                graphics.FillPath(setT_SL_Left, path);
                GraphicsPathDic.Add("ID3", path);
                points = null;

                path.Dispose();
                //blend = null;
                GC.Collect();
            }
            //else
            //{
            //    graphics.FillPath(brush, GraphicsPathDic["ID0"]);
            //    using (Brush brush2 = new SolidBrush(Color.FromArgb(0xc0, 0xc0, 0xc0)))
            //    {
            //        graphics.FillEllipse(brush2, 1, 20 - num3, Width - 2, num3 * 2);
            //    }

            //    graphics.FillPath(brush, GraphicsPathDic["ID1"]);
            //    using (Brush brush4 = new SolidBrush(Color.FromArgb(0xf3, 0xf5, 0x8b)))
            //    {
            //       graphics.FillEllipse(brush4, 1f, y - num3, (float)(Width - 2), (float)(num3 * 2));
            //    }
            //    using (Brush brush5 = new SolidBrush(Color.FromArgb(0xb8, 180, 0x43)))
            //    {
            //        graphics.FillPath(brush5, GraphicsPathDic["ID2"]);
            //    }
            //  graphics.FillPath(Brushes.DimGray, GraphicsPathDic["ID3"]); //左边黑色部分
            //}

            //由 Brushes.Gray改为Brushes.Black
            if (!string.IsNullOrEmpty(this.bottleTag))
            {
                //Brush shuziColor = Brushes.White;//.Black;
                graphics.DrawString(this.bottleTag, Font, Brushes.White, new Rectangle(-10 + loaclPoint.X, 0, Width + 20, 20), this.sf);
                //由 Brushes.Gray改为Brushes.Black
                //graphics.DrawString(this.bottleTag, Font, Brushes.Black, new Rectangle(-10 + loaclPoint.X, 0x1a, Width + 20, 20), this.sf);
            }
            if (!string.IsNullOrEmpty(this.headTag))
            {
                graphics.DrawString(this.headTag, Font, Brushes.White, new Rectangle(loaclPoint.X, 0x1a, Width, 30), this.sf);
                ////由 Brushes.DimGray改为Brushes.Black
                //graphics.DrawString(this.headTag, Font, Brushes.Black, new Rectangle(loaclPoint.X, 0, Width, 25), this.sf);
            }

            
            //graphics.DrawString("下料量", Font, Brushes.Gray, new Rectangle(-10 + loaclPoint.X, height - 70, Width + 20, 20), this.sf);

            graphics.DrawString(setValue, Font, Brushes.Black, new Rectangle(-10 + loaclPoint.X, height - 30, Width + 20, 15), this.sf);
            graphics.DrawString(currentValue, Font, Brushes.Black, new Rectangle(-10 + loaclPoint.X, height - 15, Width + 20, 15), this.sf);
           // graphics.DrawString(desc, Font, Brushes.Red, new Rectangle(loaclPoint.X + Width /10, 2, Width, 15), this.sf);

        }
    }
    public class BottleTwo : AbstractBottle
    {
        public override void Dispose()
        {

        }
        public override void OnPaint(Graphics graphics, int width, int height, Point loaclPoint,string desc)
        {
            //headHeight = 5;//20
            if (graphics == null) return;
            IsChangeSize = Width != width || Height != height || GraphicsPathDic.Count == 0 ? true : false;
            Width = width;
            Height = height-46;
            float x = ((float)Width) / 4f;
            float y = (Height - this.dockHeight) - ((((Height - this.dockHeight) - headHeight) * Convert.ToSingle(this.value)) / 100f);
            int num3 = (Width / 50) + 3;
            //if (true)
            {

                foreach (GraphicsPath item in GraphicsPathDic.Values)
                {
                    item.Dispose();
                }
                GraphicsPathDic.Clear();
                GraphicsPath path = new GraphicsPath();

                //图像绘制的坐标点
                PointF[] points = new PointF[] { new PointF(loaclPoint.X+ 0f, headHeight), new PointF(loaclPoint.X + 0f, Height - this.dockHeight), new PointF(loaclPoint.X + x, (float)(Height - 1)), new PointF(loaclPoint.X + x * 2, Height - this.dockHeight), new PointF(loaclPoint.X + x * 3, (float)(Height - 1)), new PointF((float)(loaclPoint.X + Width - 1), Height - this.dockHeight), new PointF((float)(loaclPoint.X + Width - 1), headHeight), new PointF(loaclPoint.X + 0f, headHeight) };
                path.AddPolygon(points);
                brush = new LinearGradientBrush(new Point(0, headHeight), new Point(Width - 1, headHeight), Color.FromArgb(0x8e, 0xc4, 0xd8), Color.FromArgb(240, 240, 240));
                //blend.Colors = new Color[] { Color.FromArgb(0x33, 0x99, 0x33), Color.FromArgb(0x33, 0x99, 0x33), Color.FromArgb(0x33, 0x99, 0x33) };
                blend.Colors = Lc2BackColors;//无料时的背景填充色
                brush.InterpolationColors = blend;
                graphics.FillPath(brush, path);
                GraphicsPathDic.Add("ID0", path);
                //using (Brush brush2 = new SolidBrush(Color.FromArgb(0x33, 0xcc, 0x66)))
                using (Brush brush2 = new SolidBrush(Lc1BackColorTop))////c0c0c0 椭圆  顶部的椭圆的填充颜色
                {
                    graphics.FillEllipse(brush2, loaclPoint.X + 1, headHeight - num3, Width - 2, num3 * 2);
                }
                points = null;

                path = new GraphicsPath();
                points = new PointF[] { new PointF(loaclPoint.X + 0f, y), new PointF(loaclPoint.X + 0f, Height - this.dockHeight), new PointF(loaclPoint.X + x, (float)(Height - 1)), new PointF(loaclPoint.X + x * 2, Height - this.dockHeight), new PointF(loaclPoint.X + x * 3, (float)(Height - 1)), new PointF((float)(loaclPoint.X + Width - 1), Height - this.dockHeight), new PointF((float)(loaclPoint.X + Width - 1), y), new PointF(loaclPoint.X + 0f, y) };
                path.AddPolygon(points);
                //blend.Colors = new Color[] { Color.FromArgb(0xc2, 190, 0x4d), Color.FromArgb(0xe2, 0xdd, 0x62), Color.FromArgb(0xc2, 190, 0x4d) };中部的填充颜色
                //blend.Colors = new Color[] { Color.FromArgb(0x73, 0x73, 0x70), Color.FromArgb(0xb3, 0xaf, 0xaf), Color.FromArgb(0x73, 0x73, 0x70) };//中部的填充颜色737370
                blend.Colors = Lc5BackColors;//中部的填充颜色
                brush.InterpolationColors = blend;
                graphics.FillPath(brush, path);
                GraphicsPathDic.Add("ID1", path);
                points = null;

                path = new GraphicsPath();
                points = new PointF[] { new PointF(loaclPoint.X + 0f, Height - this.dockHeight), new PointF(loaclPoint.X + x, (float)(Height - 1)), new PointF(loaclPoint.X + x * 2, Height - this.dockHeight), new PointF(loaclPoint.X + x * 3, (float)(Height - 1)), new PointF((float)(loaclPoint.X + Width - 1), Height - this.dockHeight), new PointF(loaclPoint.X + 0f, Height - this.dockHeight) };
                path.AddPolygon(points);
                //添加底部椭圆
                path.AddArc(loaclPoint.X + 0f, (Height - this.dockHeight) - num3, (float)Width / 2, (float)(num3 * 2), 0f, 180f);
                path.AddArc(loaclPoint.X + 0f + (float)Width / 2, (Height - this.dockHeight) - num3, (float)Width / 2-1, (float)(num3 * 2), 0f, 180f);
                //using (Brush brush5 = new SolidBrush(Color.FromArgb(0xb8, 180, 0x43)))//底部三角的填充颜色
                //using (Brush brush5 = new SolidBrush(Color.FromArgb(0x88, 0x88, 0x88)))//b8b443 底部三角的填充颜色
                using (Brush brush5 = new SolidBrush(Lc3BackColor))//b8b443 底部三角的填充颜色
                {
                    graphics.FillPath(brush5, path);
                }
                GraphicsPathDic.Add("ID2", path);
                points = null;

                //using (Brush brush4 = new SolidBrush(Color.FromArgb(0xf3, 0xf5, 0x8b)))//黄色椭圆  中间的椭圆的填充颜色
                //using (Brush brush4 = new SolidBrush(Color.FromArgb(0xb5, 0xb1, 0xb1)))//b1b1b1 黄色椭圆  中间的椭圆的填充颜色
                using (Brush brush4 = new SolidBrush(Lc4BackColorTop))//b1b1b1 黄色椭圆  中间的椭圆的填充颜色
                {
                    graphics.FillEllipse(brush4, loaclPoint.X + 1f, y - num3, (float)(Width - 2), (float)(num3 * 2));
                    
                }

                path = new GraphicsPath();
                points = new PointF[] { new PointF(Width * 3f / 16f + loaclPoint.X, Height - (this.dockHeight / 2.5f)), new PointF(Width * 3f / 16f + loaclPoint.X, (float)(Height - 1)), new PointF(((Width) * 5f) / 16f + loaclPoint.X, (float)(Height - 1)), new PointF(((Width) * 5f) / 16f + loaclPoint.X, Height - (this.dockHeight / 2.5f)) };
                path.AddLines(points);
                path.AddArc(((float)Width * 3f) / 16f + loaclPoint.X, ((Height - (this.dockHeight / 2f)) - num3) - 1f, ((float)Width) / 8f, (float)(num3 * 3), 0f, 180f);
                //graphics.FillPath(Brushes.DimGray, path); //左边黑色部分
                graphics.FillPath(setT_SL_Left, path);
                GraphicsPathDic.Add("ID3", path);
                points = null;
                path = new GraphicsPath();
                points = new PointF[] { new PointF(((float)Width * 11f) / 16f + loaclPoint.X, Height - (this.dockHeight / 2.5f)), new PointF(((float)Width * 11f) / 16f + loaclPoint.X, (float)(Height - 1)), new PointF(((Width) * 13f) / 16f + loaclPoint.X, (float)(Height - 1)), new PointF(((Width) * 13f) / 16f + loaclPoint.X, Height - (this.dockHeight / 2.5f)) };
                path.AddLines(points);
                path.AddArc(((float)Width * 11f) / 16f + loaclPoint.X, ((Height - (this.dockHeight / 2f)) - num3) - 1f, ((float)Width) / 8f, (float)(num3 * 3), 0f, 180f);
                //graphics.FillPath(Brushes.DimGray, path); //右边黑色部分
                graphics.FillPath(setT_SL_Right, path);
                //graphics.FillPath(Brushes.Green, path);//右绿色部分
                GraphicsPathDic.Add("ID4", path);
                points = null;

                path.Dispose();
                //blend = null;
                GC.Collect();
            }
            //else
            //{
            //    graphics.FillPath(brush, GraphicsPathDic["ID0"]);
            //    using (Brush brush2 = new SolidBrush(Color.FromArgb(0xc0, 0xc0, 0xc0)))
            //    {
            //        graphics.FillEllipse(brush2, 1, 20 - num3, Width - 2, num3 * 2);
            //    }

            //    graphics.FillPath(brush, GraphicsPathDic["ID1"]);
            //    using (Brush brush4 = new SolidBrush(Color.FromArgb(0xf3, 0xf5, 0x8b)))
            //    {
            //        graphics.FillEllipse(brush4, 1f, y - num3, (float)(Width - 2), (float)(num3 * 2));
            //    }
            //    using (Brush brush5 = new SolidBrush(Color.FromArgb(0xb8, 180, 0x43)))
            //    {
            //        graphics.FillPath(brush5, GraphicsPathDic["ID2"]);
            //    }
            //    graphics.FillPath(Brushes.DimGray, GraphicsPathDic["ID3"]); //左边黑色部分
            //    graphics.FillPath(Brushes.DimGray, GraphicsPathDic["ID4"]); //右边黑色部分
            //}
            if (!string.IsNullOrEmpty(this.bottleTag))
            {
                //Brush shuziColor = Brushes.White;//.Black;
                //由 Brushes.Gray改为Brushes.Black
                graphics.DrawString(this.bottleTag, Font, Brushes.White, new Rectangle(-10 + loaclPoint.X, 0, Width + 20, 20), this.sf);
            }
            if (!string.IsNullOrEmpty(this.headTag))
            {
                //由 Brushes.DimGray改为Brushes.Black
                graphics.DrawString(this.headTag, Font, Brushes.White, new Rectangle(loaclPoint.X, 0x1a, Width, 30), this.sf);
            }


            ////graphics.DrawString("下料量", Font, Brushes.Gray, new Rectangle(-10 + loaclPoint.X, height - 70, Width + 20, 20), this.sf);
            //graphics.DrawString("设定值", Font, Brushes.Gray, new Rectangle(-10 + loaclPoint.X, height - 40, Width + 20, 20), this.sf);
            //graphics.DrawString("实际值", Font, Brushes.Gray, new Rectangle(-10 + loaclPoint.X, height - 20, Width + 20, 20), this.sf);

            //由 Brushes.Gray改为Brushes.Black
            graphics.DrawString(setValue, Font, Brushes.Black, new Rectangle(loaclPoint.X, height - 30, Width/2, 15), this.sf);
            graphics.DrawString(currentValue, Font, Brushes.Black, new Rectangle(loaclPoint.X, height - 15, Width/2, 15), this.sf);
           // graphics.DrawString(desc, Font, Brushes.Red, new Rectangle(loaclPoint.X + Width /10, 5, Width, 15), this.sf);

            graphics.DrawString(setValue2, Font, Brushes.Black, new Rectangle(loaclPoint.X + Width / 2, height - 30, Width / 2, 15), this.sf);
            graphics.DrawString(currentValue2, Font, Brushes.Black, new Rectangle(loaclPoint.X + Width / 2, height - 15, Width / 2, 15), this.sf);

           
        }

    }
}
