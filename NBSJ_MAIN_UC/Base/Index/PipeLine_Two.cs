using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserControlIndex
{
    public partial class PipeLine_Two : UserControl
    {
        public PipeLine_Two()
        {
            InitializeComponent();

            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;
            pipeLine1.MoveSpeed = -2.5f;
            pipeLine1.Height = 15;
            pipeLine2.MoveSpeed = -2.5f;
            pipeLine2.Height = 15;
            pipeLine3.MoveSpeed = -2.5f;
            pipeLine3.Height = 15;
            /*pipeLine4.MoveSpeed = -2.5f;
            pipeLine4.Height = 15;*/

            //MinimumSize = new Size(10, 10);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            pipeLine1.PipeLineName = "Z41(混-4)";
            pipeLine2.PipeLineName = "S1(混-5)";
            pipeLine3.PipeLineName = "SBL(梭式布料器皮带)";
            //pipeLine4.PipeLineName = "1S-1皮带";



            chengzhi = "秤值";
            shuifen = "水分值";
        }
        private StringFormat sf;

        public void PipeLineActive(bool isPipeLineActive)
        {
            pipeLine1.PipeLineActive = isPipeLineActive;
            pipeLine2.PipeLineActive = isPipeLineActive;
            pipeLine3.PipeLineActive = isPipeLineActive;
            //pipeLine4.PipeLineActive = isPipeLineActive;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }


            Graphics graphics = e.Graphics;

            /*this.pipeLine1.Width = this.Width / 3;
            this.pipeLine1.Location = new System.Drawing.Point(this.Width /4 * 3, (int)(this.Height / 4f));

            this.pipeLine2.Width = this.Width / 3;
            this.pipeLine2.Location = new System.Drawing.Point(this.Width / 4 * 2 , (int)(this.Height / 4f *2));
            this.pipeLine3.Width = this.Width /3;
            this.pipeLine3.Location = new System.Drawing.Point(this.Width /4 * 1, (int)(this.Height / 4f *3));*/
            this.pipeLine1.Width = (this.Width-50) / 3;
            this.pipeLine1.Height = 15;
            this.pipeLine1.Location = new System.Drawing.Point(pipeLine3.Width * 2 + 50, (int)(this.Height / 2.5f - 30));

            this.pipeLine2.Width = (this.Width-50) / 3;
            this.pipeLine2.Height = 15;
            this.pipeLine2.Location = new System.Drawing.Point(pipeLine3.Width + 50, (int)(this.Height / 2.5f - 15));

            this.pipeLine3.Width = (this.Width-50) / 3;
            this.pipeLine3.Height = 15;
            this.pipeLine3.Location = new System.Drawing.Point(50, (int)(this.Height / 2.5f));

            /*this.pipeLine4.Width = this.Width/4;
            this.pipeLine4.Location = new System.Drawing.Point(this.Width /5 , (int)(this.Height / 5f *4));*/

            graphics.DrawString("水分:" + shuifen + "%", Font, Brushes.Black, new Rectangle(this.Width / 3*2, (int)(this.Height /4-34 ), this.pipeLine1.Width, 30), this.sf);
            graphics.DrawString("Z41(混-4)称值:" + chengzhi + "t/h", Font, Brushes.Black, new Rectangle(this.Width / 3 * 2+5, (int)(this.Height / 4 ), this.pipeLine1.Width, 30), this.sf);
            graphics.DrawString("S1(混-5)称值:" + chengzhi2 + "t/h", Font, Brushes.Black, new Rectangle(this.Width / 3 * 2 - 80, (int)(this.Height / 4-27), this.pipeLine1.Width, 30), this.sf);

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
                //pipeLine2.MoveSpeed = -2.5f;
            }
            else
            {
                pipeLine1.MoveSpeed = 0f;
                //pipeLine2.MoveSpeed = 0f;
            }
        }

        /// <summary>
        /// 设置运行和停止
        /// </summary>
        /// <param name="isRun"></param>
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
        /*public void SetIsRunStop4(bool isRun)
        {
            if (isRun)//2.5f
            {
                pipeLine4.MoveSpeed = -2.5f;
            }
            else
            {
                pipeLine4.MoveSpeed = 0f;
            }
        }*/
        string chengzhi;
        string chengzhi2;
        string shuifen;
        //Z41混-4称值
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
        //S1混-5称值
        [Browsable(true), Description("秤值。"), DefaultValue(typeof(string), "秤值"), Category("Appearance")]
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
