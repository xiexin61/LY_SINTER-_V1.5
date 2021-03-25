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
    public partial class PipeLine_Text : UserControl
    {
        public PipeLine_Text()
        {
            if (this.DesignMode)
            {
                return;
            }
            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;

            InitializeComponent();

            pipeLine1.MoveSpeed = -2.5f;
            pipeLine1.Height = 15;
            pipeLine2.MoveSpeed = -2.5f;
            pipeLine2.Height = 15;
            pipeLine3.MoveSpeed = -2.5f;
            pipeLine3.Height = 15;
            //MinimumSize = new Size(10, 10);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);


            chengzhi = "";
            shuifen = "";
            chengzhi2 = "";

            pipeLine1.PipeLineName = "1H3(混-1)";
            pipeLine2.PipeLineName = "2H1(混-2)";
            pipeLine3.PipeLineName = "2H2(混-3)";
        }
        private StringFormat sf;

        public void PipeLineActive(bool isPipeLineActive)
        {
            pipeLine1.PipeLineActive = isPipeLineActive;
            pipeLine2.PipeLineActive = isPipeLineActive;
            pipeLine3.PipeLineActive = isPipeLineActive;
        }
        /// <summary>
        /// 字体宽度
        /// </summary>
        int foinWidth = 80;
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            Graphics graphics = e.Graphics;

           
            this.pipeLine1.Width = this.Width/3;
            this.pipeLine1.Height = 15;
            this.pipeLine1.Location = new System.Drawing.Point(this.Width / 3*2, (int)(this.Height / 2.5f - 30));

            this.pipeLine2.Width = this.Width / 3;
            this.pipeLine2.Height = 15;
            this.pipeLine2.Location = new System.Drawing.Point(this.Width / 3, (int)(this.Height / 2.5f-15));

            this.pipeLine3.Width = this.Width / 3;
            this.pipeLine3.Height = 15;
            this.pipeLine3.Location = new System.Drawing.Point(0, (int)(this.Height / 2.5f));

            //this.label1.Text = "1H-1秤值";
            //this.label1.Location = new System.Drawing.Point(Width / 2, Height / 2 - 30);

            //由 Brushes.Gray改为Brushes.Black
            //graphics.DrawString("秤值:" + chengzhi + "t/h", Font, Brushes.Black, new Rectangle(Width / 2 - foinWidth / 2, (int)(Height / 2.5f - 32), foinWidth, 20), this.sf);

            //graphics.DrawString("水分:" + shuifen + "%", Font, Brushes.Black, new Rectangle(Width / 2 - foinWidth / 2, (int)(Height / 2.5f), foinWidth, 13), this.sf);


            graphics.DrawString("1H3水分:" + shuifen + "%", Font, Brushes.Black, new Rectangle(Width /5+20, (int)(Height / 2.5f - 45), Width, 20), this.sf);
           graphics.DrawString("2H1秤值:" + chengzhi + "t/h", Font, Brushes.Black, new Rectangle(20, (int)(Height / 2.5f), Width, 20), this.sf);
           // graphics.DrawString("2H2秤值:" + chengzhi2 + "t/h", Font, Brushes.Black, new Rectangle(0 - 40, (int)(Height / 2.5f + 20), Width, 20), this.sf);

            //graphics.DrawString("水分:" + shuifen + "%", Font, Brushes.Black, new Rectangle(0, (int)(Height / 2.5f-22), Width, 13), this.sf);

            //base.OnPaint(e);
        }


        /// <summary>
        /// 设置运行和停止
        /// </summary>
        /// <param name="isRun"></param>
        public void SetIsRunStop(bool isRun)
        {
            if (isRun)//2.5f
            {
                pipeLine1.MoveSpeed = -2.5f;
               
            }
            else
            {
                pipeLine1.MoveSpeed = 0f;
                
            }
        }
        public void SetIsRunStop2(bool isRun)
        {
            if (isRun)//2.5f
            {
                
                pipeLine2.MoveSpeed = -2.5f;
            }
            else
            {
               
                pipeLine2.MoveSpeed = 0f;
            }
        }
        public void SetIsRunStop3(bool isRun)
        {
            if (isRun)//2.5f
            {

                pipeLine3.MoveSpeed = -2.5f;
            }
            else
            {

                pipeLine3.MoveSpeed = 0f;
            }
        }

        string chengzhi;
        string chengzhi2;
        string shuifen;


        [Browsable(true), Description("秤值。"), DefaultValue(typeof(string), "秤值"), Category("Appearance")]
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
        public string ChengZhi2
        {
            get
            {
                return this.chengzhi2;
            }
            set
            {
                this.chengzhi2 = value;
                //base.Invalidate();
            }
        }
        [Browsable(true), Description("水分值。"), DefaultValue(typeof(string), "水分值"), Category("Appearance")]
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

        /// <summary>
        /// 使控件的整个图面无效并导致重绘控件。
        /// </summary>
        public void InvalidateNew()
        {
            base.Invalidate();
        }



    }
}
