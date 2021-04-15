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
    public partial class BlendingUC2 : UserControl
    {
        public BlendingUC2()
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


            shuifen = "";
            zhuansu = "转速";
            setJsl = "SP";
            readJsl = "PV";
            chengzhi = "称值";
            ArrTitle[0] = "一次混合";
            ArrTitle[1] = "二次混合";
        }

        string[] ArrTitle = new string[2];

        protected StringFormat sf;

        Font shuziFont = new Font("宋体", 14F, FontStyle.Bold);
        Bitmap myBitmap = null;
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            graphics.Clear(this.BackColor);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;


            if (isrun)
            {
                myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.混匀机2;
            }
            else
            {
                myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.混匀机停止;
            }
            //Point point = new Point(0, 0);
            //graphics.DrawImage(myBitmap, 0, 0, myBitmap.Size.Width, myBitmap.Size.Height);
            //graphics.DrawImage(myBitmap, 0, 0, this.Width, this.Height - 45);
            graphics.DrawImage(myBitmap, 0, 0, this.Width, (int)(this.Height * 0.7f));

            this.radioButton1.Location = new System.Drawing.Point((int)(this.Width * 0.1f), (int)(this.Height * 0.2f));


            if (isrun)
            {
                //graphics.DrawString("一次混合", shuziFont, Brushes.Green, new Rectangle(0, (int)(Height * 0.2f), this.Width, 15), this.sf);
                graphics.DrawString(ArrTitle[titleIndex], shuziFont, Brushes.White, new Rectangle(0, (int)(Height * 0.2f), this.Width, 30), this.sf);
            }
            else
            {
                graphics.DrawString(ArrTitle[titleIndex], shuziFont, Brushes.White, new Rectangle(0, (int)(Height * 0.2f), this.Width, 30), this.sf);
            }

            ////由 Brushes.Gray改为Brushes.Black
            ////shuifen = "一二混目标水分";
            ////zhuansu = "转速";
            ////setJsl = "设定加水量";
            ////readJsl = "实际加水量";
            ////var b = shuifen == '月' ? Brushes.Red : Brushes.Black;
            //graphics.DrawString("目标水分值:"+shuifen + "%", Font, Brushes.Black, new Rectangle(0, Height - 45, this.Width, 15), this.sf);
            //// graphics.DrawString("转速:"+zhuansu+ "称值:" + chengzhi, Font, Brushes.Black, new Rectangle(0, Height - 45, this.Width, 15), this.sf);
            // graphics.DrawString("称值:" + chengzhi, Font, Brushes.Black, new Rectangle(0, Height - 30, this.Width, 15), this.sf);
            ////graphics.DrawString("目标水分:" + shuifen + "%", Font, Brushes.Black, new Rectangle(0, Height - 60, this.Width, 15), this.sf);
            //graphics.DrawString("目标水分:" + shuifen + "%", Font, Brushes.Black, new Rectangle(0, Height - 45, this.Width, 15), this.sf);

            ////graphics.DrawString("加水量SP:" + setJsl + "m³/h", Font, Brushes.Black, new Rectangle(0, Height - 30, this.Width, 15), this.sf);
            //////graphics.DrawString("实际加水量:" + readJsl, Font, Brushes.Gray, new Rectangle(this.Width / 2 - 40, Height - 15, 80, 15), this.sf);
            ////graphics.DrawString("加水量PV:" + readJsl + "m³/h", Font, Brushes.Black, new Rectangle(0, Height - 15, this.Width, 15), this.sf);
            ///graphics.DrawString("加水量SP:" + setJsl + "t/h", Font, Brushes.Black, new Rectangle(0, Height - 30, this.Width, 15), this.sf);
            ////graphics.DrawString("加水量PV:" + readJsl + "t/h", Font, Brushes.Black, new Rectangle(0, Height - 15, this.Width, 15), this.sf);
            graphics.DrawString("加水设定值:" + setJsl + "t/h", Font, Brushes.Black, new Rectangle(0, Height - 45, this.Width, 15), this.sf);
            graphics.DrawString("加水反馈值:" + readJsl + "t/h", Font, Brushes.Black, new Rectangle(0, Height-30, this.Width, 15), this.sf);
           // graphics.DrawString("实际加水量:" + readJsl + "t/h", Font, Brushes.Black, new Rectangle(0, Height - 40, this.Width, 15), this.sf);


            //var g = e.Graphics;
            //var s = "测试文字";
            //for (var i = 0; i < s.Length; i++)
            //{
            //    int size = 52;
            //    using (var p = new GraphicsPath())
            //    {
            //        var b = Brushes.Black;
            //        p.AddString(s[i].ToString(), new FontFamily("隶书"), 1, size, new Point(i * size, 20), new StringFormat());
            //        g.FillPath(b, p);
            //    }
            //} 

        }

        int titleIndex = 0;
        [Browsable(true), Description("标题。"), DefaultValue(typeof(int), "标题"), Category("Appearance")]
        public int TitleIndex
        {
            get
            {
                return this.titleIndex;
            }
            set
            {
                this.titleIndex = value;
            }
        }
        bool isrun = false;
        [Browsable(true), Description("混匀机的启动和停止。"), DefaultValue(typeof(bool), "false"), Category("Appearance")]
        public bool IsRun
        {
            get
            {
                return this.isrun;
            }
            set
            {
                this.isrun = value;
                //base.Invalidate();
            }
        }

        string shuifen;
        string zhuansu;
        string setJsl;
        string readJsl;
        string chengzhi;
        [Browsable(true), Description("一二混目标水分。"), DefaultValue(typeof(string), "水分值"), Category("Appearance")]
        public string ShuiFen
        {
            get
            {
                return this.shuifen;
            }
            set
            {
                this.shuifen = value;
                //base.Invalidate();
            }
        }


        [Browsable(true), Description("转速值。"), DefaultValue(typeof(string), "转速值"), Category("Appearance")]
        public string ZhuanSu
        {
            get
            {
                return this.zhuansu;
            }
            set
            {
                this.zhuansu = value;
                //base.Invalidate();
            }
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

        [Browsable(true), Description("设置的加水量。"), DefaultValue(typeof(string), "设置的加水量"), Category("Appearance")]
        public string SetJsl
        {
            get
            {
                return this.setJsl;
            }
            set
            {
                this.setJsl = value;
                //base.Invalidate();
            }
        }

        [Browsable(true), Description("实际加水量。"), DefaultValue(typeof(string), "实际加水量"), Category("Appearance")]
        public string ReadJsl
        {
            get
            {
                return this.readJsl;
            }
            set
            {
                this.readJsl = value;
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


        DateTime dtimeRbtnDouble = DateTime.Now;
        private void rbtnDouble_Click(object sender, EventArgs e)
        {
            if (dtimeRbtnDouble.AddMilliseconds(1000) > DateTime.Now)
            {
                using (UserControlIndex.FormDaoTui frm = new UserControlIndex.FormDaoTui())
                {
                    frm.Init(titleIndex + 1);
                    frm.ShowDialog();
                }
            }
            dtimeRbtnDouble = DateTime.Now;
        }
    }
}
