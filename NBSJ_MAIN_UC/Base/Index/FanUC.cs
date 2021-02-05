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
    public partial class FanUC : UserControl
    {
        public FanUC()
        {
            InitializeComponent();
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;

            numV = 1;
        }

        protected StringFormat sf;
        Bitmap myBitmap = null;
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(this.BackColor);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            if (isrun)
            {
                myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.风机开;
            }
            else
            {
                myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.风机关;
            }
            graphics.DrawImage(myBitmap, 0, 0, this.Width, this.Height);

            graphics.DrawString(numV.ToString(), Font, Brushes.Gray, new Rectangle(this.Width / 5, 0, this.Width, this.Height / 2), this.sf);

        }

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
                base.Invalidate();
            }
        }

        int numV;

        [Browsable(true), Description("编号。"), DefaultValue(typeof(int), "1"), Category("Appearance")]
        public int NumValue
        {
            get
            {
                return this.numV;
            }
            set
            {
                this.numV = value;
                base.Invalidate();
            }
        }



    }
}
