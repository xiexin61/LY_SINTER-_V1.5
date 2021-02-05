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
    public partial class HunHeLiaoCaoYuanGunUC : UserControl
    {
        public HunHeLiaoCaoYuanGunUC()
        {
            InitializeComponent();

            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;

            shenyuliangValue = 110;
            myBitmap =global::NBSJ_MAIN_UC.Properties.Resources.混合料槽和圆辊;
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

            this.radioButton1.Location = new System.Drawing.Point((int)(this.Width * 0.06f), (int)(this.Height * 0.2f));

            this.sf.Alignment = StringAlignment.Center;
            graphics.DrawString(shenyuliangValue.ToString("f2") + "t", Font, Brushes.Black, new Rectangle(0, (int)(this.Height * 0.1f), this.Width / 2, this.Height / 3), this.sf);

            graphics.DrawString("混合料槽", Font, Brushes.Black, new Rectangle(0, (int)(this.Height * 0.28f), this.Width / 2, (int)(this.Height / 3)), this.sf);

            this.sf.Alignment = StringAlignment.Near;

            ////graphics.DrawString(" SP:" + setZhuanSu+ "Hz", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.53f), (int)(this.Height * 0.1f), this.Width, 20), this.sf);
            ////graphics.DrawString(" PV:" + readZhuanSu + "Hz", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.53f), (int)(this.Height * 0.3f), this.Width, 20), this.sf);
            ////graphics.DrawString(" PV:" + readZhuanSu + "Hz", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.53f), (int)(this.Height * 0.2f), this.Width, 20), this.sf);
            //graphics.DrawString(" 圆辊频率", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.55f), (int)(this.Height * 0.1f), this.Width, 20), this.sf);
            //graphics.DrawString(" " + readZhuanSu + "Hz", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.55f), (int)(this.Height * 0.3f), this.Width, 20), this.sf);
            graphics.DrawString("混合料温度" + hunHeLiaoWenDu + "℃", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.55f), (int)(this.Height * 0.1f), this.Width, 20), this.sf);
            graphics.DrawString("圆辊频率" + readZhuanSu + "Hz", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.55f), (int)(this.Height * 0.3f), this.Width, 20), this.sf);
            //graphics.DrawString("九辊频率" + setZhuanSu + "Hz", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.55f), (int)(this.Height * 0.3f), this.Width, 20), this.sf);


            graphics.DrawString("点火炉", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.58f), (int)(this.Height * 0.58f), this.Width / 2, 20), this.sf);

            graphics.DrawString("保温炉", Font, Brushes.Black, new Rectangle((int)(this.Width * 0.8f), (int)(this.Height * 0.58f), this.Width / 2, 20), this.sf);

        }

        double shenyuliangValue = 110;
        /// <summary>
        /// 获取或设置当前的剩余量
        /// </summary>
        [Browsable(true), Description("获取或设置当前的混合料槽的剩余量。"), DefaultValue(typeof(double), "110"), Category("Appearance")]
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

        string setZhuanSu = "";//设定转速
        string readZhuanSu = "";//实际转速
        string hunHeLiaoWenDu = "";//混合料温度
        [Browsable(true), Description("获取或设置当前的圆辊设定转速。"), DefaultValue(typeof(string), "22.45"), Category("Appearance")]
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

        [Browsable(true), Description("获取或设置当前的圆辊实际转速。"), DefaultValue(typeof(string), "22.45"), Category("Appearance")]
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
        public string HunHeLiaoWenDu
        {
            get
            {
                return this.hunHeLiaoWenDu;
            }
            set
            {
                this.hunHeLiaoWenDu = value;
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

        private void HunHeLiaoCaoYuanGunUC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var quyu = new Rectangle(0, 0, (int)(this.Width * 0.4f), (int)(this.Height * 0.5f));
            //取样点区域
            if (e.X > quyu.X && e.X < (quyu.X + quyu.Width) && e.Y > quyu.Y && e.Y < (quyu.Y + quyu.Height))
            {
                using (FormDaoTui frm = new FormDaoTui())
                {
                    
                    frm.Init(3);
                    frm.ShowDialog();
                }
            }
        }

        DateTime dtimeRbtnDouble = DateTime.Now;
        private void rbtnDouble_Click(object sender, EventArgs e)
        {
            if (dtimeRbtnDouble.AddMilliseconds(1000) > DateTime.Now)
            {
                using (FormDaoTui frm = new FormDaoTui())
                {
                    frm.Init(3);
                    frm.ShowDialog();
                }
            }
            dtimeRbtnDouble = DateTime.Now;
        }

    }
}
