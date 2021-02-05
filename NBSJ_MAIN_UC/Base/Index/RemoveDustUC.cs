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
    public partial class RemoveDustUC : UserControl
    {
        public RemoveDustUC()
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

            liuliang = "流量 8651 m3/min";
            wendu = "温度 125 ℃";
            fuya = "负压 -12.5 KPa";


            liuliang2 = "流量 8231 m3/min";
            wendu2 = "温度 130 ℃";
            fuya2 = "负压 -11.5 KPa";
        }


        protected StringFormat sf;

        int x1, y1;
        int x2, y2;
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(this.BackColor);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;


            Bitmap myBitmap = null;


            if (isrun && isrun2)
            {
                myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.除尘22;
            }
            else if (isrun && !isrun2)
            {
                myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.除尘20;
            }
            else if (!isrun && isrun2)
            {
                myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.除尘21;
            }
            else
            {
                myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.除尘;
            }
            graphics.DrawImage(myBitmap, 0, 0, this.Width, this.Height);

            x1 = (int)(this.Width * 0.2f);
            y1 = (int)(this.Height * 0.16f);
            x2 = (int)(this.Width * 0.2f);
            y2 = (int)(this.Height * 0.59f);


            graphics.DrawString("左-1", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.16f), y1, 50, 15), this.sf);

            //由 Brushes.Gray改为Brushes.Black
            graphics.DrawString(liuliang, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.3f), y1, 110, 15), this.sf);
            graphics.DrawString(wendu, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.45f), y1, 80, 15), this.sf);
            graphics.DrawString(fuya, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.6f), y1, 110, 15), this.sf);


            graphics.DrawString("右-2", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.16f), y2, 50, 15), this.sf);


            graphics.DrawString(liuliang2, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.3f), y2, 110, 15), this.sf);
            graphics.DrawString(wendu2, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.45f), y2, 80, 15), this.sf);
            graphics.DrawString(fuya2, Font, Brushes.Black, new Rectangle((int)(this.Width * 0.6f), y2, 110, 15), this.sf);

        }


        string liuliang;
        string wendu;
        string fuya;

        string liuliang2;
        string wendu2;
        string fuya2;

        [Browsable(true), Description("流量。"), DefaultValue(typeof(string), "流量值"), Category("Appearance")]
        public string FlowValue
        {
            get
            {
                return this.liuliang;
            }
            set
            {
                this.liuliang = value;
                //base.Invalidate();
            }
        }

        [Browsable(true), Description("温度。"), DefaultValue(typeof(string), "温度值"), Category("Appearance")]
        public string TempValue
        {
            get
            {
                return this.wendu;
            }
            set
            {
                this.wendu = value;
                //base.Invalidate();
            }
        }
        [Browsable(true), Description("负压。"), DefaultValue(typeof(string), "负压值"), Category("Appearance")]
        public string KPaValue
        {
            get
            {
                return this.fuya;
            }
            set
            {
                this.fuya = value;
                //base.Invalidate();
            }
        }


        [Browsable(true), Description("流量。"), DefaultValue(typeof(string), "流量值"), Category("Appearance")]
        public string FlowValue2
        {
            get
            {
                return this.liuliang2;
            }
            set
            {
                this.liuliang2 = value;
                //base.Invalidate();
            }
        }

        [Browsable(true), Description("温度。"), DefaultValue(typeof(string), "温度值"), Category("Appearance")]
        public string TempValue2
        {
            get
            {
                return this.wendu2;
            }
            set
            {
                this.wendu2 = value;
                //base.Invalidate();
            }
        }
        [Browsable(true), Description("负压。"), DefaultValue(typeof(string), "负压值"), Category("Appearance")]
        public string KPaValue2
        {
            get
            {
                return this.fuya2;
            }
            set
            {
                this.fuya2 = value;
                //base.Invalidate();
            }
        }



        bool isrun = false;
        bool isrun2 = false;
        [Browsable(true), Description("除尘机的启动和停止。"), DefaultValue(typeof(bool), "false"), Category("Appearance")]
        public bool IsRun
        {
            get
            {
                return this.isrun;
            }
            set
            {
                this.isrun = value;
            }
        }
        [Browsable(true), Description("除尘机的启动和停止。"), DefaultValue(typeof(bool), "false"), Category("Appearance")]
        public bool IsRun2
        {
            get
            {
                return this.isrun2;
            }
            set
            {
                this.isrun2 = value;
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
