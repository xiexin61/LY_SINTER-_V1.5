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
    public partial class PuDiLiaoCaoUC : UserControl
    {
        public PuDiLiaoCaoUC()
        {
            InitializeComponent();


            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;

            shenyuliangValue = 20;
            //numV = 1;
            myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.铺底料槽;
        }

        private double shenyuliangValue = 20;
        protected StringFormat sf;
        Bitmap myBitmap = null;
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(this.BackColor);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            //if (isrun)
            //{
            //    myBitmap = global::WindowsFormsApplication1.Properties.Resources.风机开2;
            //}
            //else
            //{
            //    myBitmap = global::WindowsFormsApplication1.Properties.Resources.风机关;
            //}
            graphics.DrawImage(myBitmap, 0, 0, this.Width, this.Height);

            //graphics.DrawString(shenyuliangValue.ToString("f2")+"t", Font, Brushes.Black, new Rectangle(0, this.Height / 20, this.Width, this.Height / 3), this.sf);
            //graphics.DrawString("铺底料槽", Font, Brushes.Black, new Rectangle(0, this.Height / 3, this.Width, (int)(this.Height / 3*2)), this.sf);
            ////graphics.DrawString("1.2t/h", Font, Brushes.Black, new Rectangle(0, this.Height / 3*2, this.Width, (int)(this.Height / 3)), this.sf);

            graphics.DrawString(shenyuliangValue.ToString("f2") + "t", Font, Brushes.Black, new Rectangle(0, 0, this.Width, this.Height / 2), this.sf);
            graphics.DrawString("铺底料槽", Font, Brushes.Black, new Rectangle(0, this.Height / 3, this.Width, (int)(this.Height * 0.8f)), this.sf);

        }
        /// <summary>
        /// 获取或设置当前的剩余量
        /// </summary>
        [Browsable(true), Description("获取或设置当前的铺底料槽的剩余量。"), DefaultValue(typeof(double), "20"), Category("Appearance")]
        public double SylValue
        {
            get
            {
                return this.shenyuliangValue;
            }
            set
            {
                this.shenyuliangValue = value;
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
