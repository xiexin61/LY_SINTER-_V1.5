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
    public partial class BuLiaoQiUC : UserControl
    {
        public BuLiaoQiUC()
        {
            InitializeComponent();

            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;


            myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.布料器1;

        }

        Bitmap myBitmap = null;
        protected StringFormat sf;
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
            graphics.DrawString("布料器", Font, Brushes.Black, new Rectangle(0, 0, this.Width, this.Height / 2), this.sf);

            graphics.DrawString("1BL皮带", Font, Brushes.Black, new Rectangle(0, this.Height / 3, this.Width, (int)(this.Height / 12f * 7f)), this.sf);

        }
    }
}
