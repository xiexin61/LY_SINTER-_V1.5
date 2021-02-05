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
    public partial class PipeLine_Right : UserControl
    {
        public PipeLine_Right()
        {
            if (this.DesignMode)
            {
                return;
            }
            InitializeComponent();


            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;

            pipeLine1.MoveSpeed = -2.5f;
            pipeLine2.MoveSpeed = 2.5f;


            pipeLine1.Height = 15;
            pipeLine2.Width = 15;


            MinimumSize = new Size(10, 10);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            chengzhi = "秤值";

            pipeLine1.PipeLineName = "1H-1皮带";
        }


        private StringFormat sf;

        /// <summary>
        /// 设置运行和停止
        /// </summary>
        /// <param name="isRun"></param>
        public void SetIsRunStop(bool isRun)
        {
            if (isRun)//2.5f
            {
                pipeLine1.MoveSpeed = -2.5f;
                pipeLine2.MoveSpeed = 2.5f;
            }
            else
            {
                pipeLine1.MoveSpeed = 0f;
                pipeLine2.MoveSpeed = 0f;
            }
        }
        public void PipeLineActive(bool isPipeLineActive)
        {
            pipeLine1.PipeLineActive = isPipeLineActive;
            pipeLine2.PipeLineActive = isPipeLineActive;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this.DesignMode)
            {
                return;
            }
            Graphics graphics = e.Graphics;

            //graphics.DrawString("总料量SP", Font, Brushes.Gray, new Rectangle(3, 20, 60, 20), this.sf);
            //graphics.DrawString("200kg/s", Font, Brushes.Gray, new Rectangle(3, 40, 60, 20), this.sf);
            //graphics.DrawString("总料量PV", Font, Brushes.Gray, new Rectangle(3, 60, 60, 20), this.sf);
            //graphics.DrawString("200kg/s", Font, Brushes.Gray, new Rectangle(3, 80, 60, 20), this.sf);

            //graphics.DrawString("下料量", Font, Brushes.Gray, new Rectangle(3, Height - 65, 60, 20), this.sf);

            //graphics.DrawString("设定kg/s", Font, Brushes.Gray, new Rectangle(3, Height - 40, 60, 20), this.sf);
            //graphics.DrawString(setValue, Font, Brushes.Gray, new Rectangle(-10 + loaclPoint.X, height - 40, Width + 20, 20), this.sf);
            //graphics.DrawString(currentValue, Font, Brushes.Gray, new Rectangle(-10 + loaclPoint.X, height - 20, Width + 20, 20), this.sf);
            this.pipeLine2.Width = 15;
            this.pipeLine2.Height = (int)(this.Height / 2.5f);
            this.pipeLine2.Location = new System.Drawing.Point(this.Width - 15, 0);

            this.pipeLine1.Width = this.Width - 15;
            this.pipeLine1.Height = 15;
            this.pipeLine1.Location = new System.Drawing.Point(0, (int)(this.Height / 2.5f - 15));


            //由 Brushes.Gray改为Brushes.Black
            //this.label1.Text = "1H-1秤值";
            //this.label1.Location = new System.Drawing.Point(Width / 2, Height / 2 - 30);
            //graphics.DrawString("秤值:" + chengzhi+"t/h", Font, Brushes.Black, new Rectangle(Width / 2 - 30, (int)(Height / 2.5f - 32), 90, 20), this.sf);
            graphics.DrawString("秤值:" + chengzhi + "t/h", Font, Brushes.Black, new Rectangle(0, (int)(Height / 2.5f - 32), Width, 20), this.sf);

            //base.OnPaint(e);
        }

        string chengzhi;


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
                base.Invalidate();
            }
        }


    }
}
