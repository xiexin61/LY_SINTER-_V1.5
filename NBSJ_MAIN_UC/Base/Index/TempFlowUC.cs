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
    public partial class TempFlowUC : UserControl
    {
        public TempFlowUC()
        {
            InitializeComponent();
            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Near;//.Center;
            this.sf.LineAlignment = StringAlignment.Center;

            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

        }


        protected StringFormat sf;
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(this.BackColor);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;


            ////由 Brushes.Gray改为Brushes.Black
            //this.sf.Alignment = StringAlignment.Center;
            //graphics.DrawString("温度SP:" + tagTemp, Font, Brushes.Black, new Rectangle(0, 1, (int)(this.Width), 15), this.sf);
            //graphics.DrawString("温度PV:" + fireTemp, Font, Brushes.Black, new Rectangle(0, 16, (int)(this.Width), 15), this.sf);
            //graphics.DrawString("目标温度:" + tagTemp+"℃", Font, Brushes.Black, new Rectangle(0, 1, (int)(this.Width), 15), this.sf);
            //graphics.DrawString("点火温度:" + fireTemp + "℃", Font, Brushes.Black, new Rectangle(0, 16, (int)(this.Width), 15), this.sf);
            graphics.DrawString("点火温度1: " + tagTemp + "℃", Font, Brushes.Black, new Rectangle(0, 21, (int)(this.Width), 15), this.sf);
            graphics.DrawString("点火温度2: " + fireTemp + "℃", Font, Brushes.Black, new Rectangle(0, 36, (int)(this.Width), 15), this.sf);
            graphics.DrawString("煤气流量 : " + coalGasFlow + "m³/h", Font, Brushes.Black, new Rectangle(0, 51, (int)(this.Width), 15), this.sf);
            graphics.DrawString("空气流量 : " + airFlow + "m³/h", Font, Brushes.Black, new Rectangle(0, 66, (int)(this.Width), 15), this.sf);
            //graphics.DrawString("天然气流量: " + t_IG_NATURAL_PV_3S + "m³/h", Font, Brushes.Black, new Rectangle(0, 61, (int)(this.Width), 15), this.sf);

        }

        string tagTemp = "";//目标点火温度
        string fireTemp = "";//点火段温度
        string coalGasFlow = "";//煤气流量
        string airFlow = "";//空气流量

        /// <summary>
        /// 目标点火温度
        /// </summary>
        [Browsable(true), Description("目标点火温度。"), DefaultValue(typeof(string), ""), Category("Appearance")]
        public string TagTemp
        {
            get
            {
                return this.tagTemp;
            }
            set
            {
                this.tagTemp = value;
                //base.Invalidate();
            }
        }

        /// <summary>
        /// 点火段温度
        /// </summary>
        [Browsable(true), Description("点火段温度。"), DefaultValue(typeof(string), ""), Category("Appearance")]
        public string FireTemp
        {
            get
            {
                return this.fireTemp;
            }
            set
            {
                this.fireTemp = value;
                //base.Invalidate();
            }
        }

        /// <summary>
        /// 煤气流量
        /// </summary>
        [Browsable(true), Description("煤气流量。"), DefaultValue(typeof(string), ""), Category("Appearance")]
        public string CoalGasFlow
        {
            get
            {
                return this.coalGasFlow;
            }
            set
            {
                this.coalGasFlow = value;
                //base.Invalidate();
            }
        }
        //string airFlow = "";//空气流量
        /// <summary>
        /// 空气流量
        /// </summary>
        [Browsable(true), Description("空气流量。"), DefaultValue(typeof(string), ""), Category("Appearance")]
        public string AirFlow
        {
            get
            {
                return this.airFlow;
            }
            set
            {
                this.airFlow = value;
                //base.Invalidate();
            }
        }

        string t_IG_NATURAL_PV_3S = "";
        //T_IG_NATURAL_PV_3S 点火段天然气流量反馈值
        /// <summary>
        /// 点火段天然气流量反馈值
        /// </summary>
        [Browsable(true), Description("点火段天然气流量反馈值。"), DefaultValue(typeof(string), ""), Category("Appearance")]
        public string T_IG_NATURAL_PV_3S
        {
            get
            {
                return this.t_IG_NATURAL_PV_3S;
            }
            set
            {
                this.t_IG_NATURAL_PV_3S = value;
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
}
