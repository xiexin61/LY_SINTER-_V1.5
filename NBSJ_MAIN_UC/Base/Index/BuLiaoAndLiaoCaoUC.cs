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
    public partial class BuLiaoAndLiaoCaoUC : UserControl
    {
        public BuLiaoAndLiaoCaoUC()
        {
            InitializeComponent();


            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;

            MinimumSize = new Size(10, 10);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.布料器和料槽;
        }


        protected StringFormat sf;
        Bitmap myBitmap = null;
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(this.BackColor);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            graphics.DrawImage(myBitmap, 0, 0, this.Width, this.Height);

            //由 Brushes.Gray改为Brushes.Black
            this.sf.Alignment = StringAlignment.Center;
            graphics.DrawString("温度SP:" + tagTemp, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.6f), 1, (int)(this.Width * 0.4f), 15), this.sf);
            graphics.DrawString("温度PV:" + fireTemp, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.6f), 16, (int)(this.Width * 0.4f), 15), this.sf);
            graphics.DrawString("煤气流量:" + coalGasFlow, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.6f), 31, (int)(this.Width * 0.4f), 15), this.sf);
            graphics.DrawString("空气流量:" + airFlow, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.6f), 46, (int)(this.Width * 0.4f), 15), this.sf);


            this.sf.Alignment = StringAlignment.Near;
            graphics.DrawString("转速SP:" + setZhuanSu, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.22f), this.Height - 25, (int)(this.Width * 0.4f), 15), this.sf);
            graphics.DrawString("转速PV:" + readZhuanSu, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.22f), this.Height - 15, (int)(this.Width * 0.4f), 15), this.sf);


            graphics.DrawString(setLc2 + "t", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.4f), (int)(this.Height * 0.36f), (int)(this.Width * 0.4f), 20), this.sf);
            graphics.DrawString(setLc1 + "t", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.07f), (int)(this.Height * 0.58f), (int)(this.Width * 0.4f), 20), this.sf);

        }

        string tagTemp = "";//目标点火温度
        string fireTemp = "";//点火段温度
        string coalGasFlow = "";//煤气流量
        string airFlow = "";//空气流量
        string setZhuanSu = "";//设定转速
        string readZhuanSu = "";//实际转速

        string setLc1 = "";//料仓1
        string setLc2 = "";//料仓2

        public string SetZhuanSu
        {
            get
            {
                return this.setZhuanSu;
            }
            set
            {
                this.setZhuanSu = value;
                //base.Invalidate();
            }
        }
        public string ReadZhuanSu
        {
            get
            {
                return this.readZhuanSu;
            }
            set
            {
                this.readZhuanSu = value;
                //base.Invalidate();
            }
        }
        public string SetLc1
        {
            get
            {
                return this.setLc1;
            }
            set
            {
                this.setLc1 = value;
                //base.Invalidate();
            }
        }
        public string SetLc2
        {
            get
            {
                return this.setLc2;
            }
            set
            {
                this.setLc2 = value;
                //base.Invalidate();
            }
        }

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

        /// <summary>
        /// 使控件的整个图面无效并导致重绘控件。
        /// </summary>
        public void InvalidateNew()
        {
            base.Invalidate();
        }
    }
}
