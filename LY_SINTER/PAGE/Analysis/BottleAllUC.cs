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
    public partial class BottleAllUC : UserControl
    {
        private List<BottleItem> bottomitems;
        [Browsable(false)]
        public List<BottleItem> BottomItems
        {
            get
            {
                if (bottomitems == null)
                {
                    bottomitems = new List<BottleItem>();
                }
                return bottomitems;
            }
        }
        public BottleAllUC()
        {
            if (this.DesignMode)
            {
                return;
            }
            this.t_total_sp_w_3s = "0";
            this.t_total_pv_w_3s = "0";
            InitializeComponent();
            MinimumSize = new Size(10, 10);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);


            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Center;
            this.sf.LineAlignment = StringAlignment.Center;


           

            pipeLine3.PipeLineName = "1H-1皮带";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// 设置运行和停止
        /// </summary>
        /// <param name="isRun"></param>
        public void SetIsRunStop(bool isRun)
        {
            if (isRun)//2.5f
            {
                pipeLine3.MoveSpeed = 2.5f;
                pipeLine4.MoveSpeed = 2.5f;
            }
            else
            {
                pipeLine3.MoveSpeed = 0f;
                pipeLine4.MoveSpeed = 0f;
            }
        }

        public void PipeLineActive(bool isPipeLineActive)
        {
            pipeLine3.PipeLineActive = isPipeLineActive;
            pipeLine4.PipeLineActive = isPipeLineActive;
        }

        //private AbstractPipeLine pipeLine2;
        //private AbstractPipeLine pipeLine1;
        protected override void OnPaint(PaintEventArgs e)
        {

            if (this.DesignMode)
            {
                return;
            }
            Graphics graphics = e.Graphics;
            graphics.Clear(this.BackColor);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            
            int LocationXstart = 60;//开始绘图的X方向的位置
            int width = Width - LocationXstart;

            //w1*1.2=w2
            //11*w1+4*w2=width
           // double w1 = width / 15.8f;
            //double w2 = w1 * 1.2f;
            double w1 = width / 20.0f;
            double w2 = w1 * 1.0f;
            if (BottomItems.Count > 0 && width > 10 && Height > 10)
            {
                Point point = new Point(LocationXstart, 0);
                foreach (var item in BottomItems)
                {
                    if (item.BottleType == BottleType.BottleSingle)
                    {
                        item.BottleObj.OnPaint(graphics, (int)w1, Height, point,item.BottleDesc);
                        point.X += (int)w1;
                    }
                    else
                    {
                        item.BottleObj.OnPaint(graphics, (int)(w2), Height, point, item.BottleDesc);
                        point.X += (int)(w2);
                    }
                    
                }
            }
           
            //Brushes.Black  由 Brushes.Gray改为Brushes.Black
            graphics.DrawString("总料量SP", Font, Brushes.Black, new Rectangle(0, 5, 60, 15), this.sf);
            graphics.DrawString(t_total_sp_w_3s + "t/h", Font, Brushes.Black, new Rectangle(0, 18, 60, 15), this.sf);
            graphics.DrawString("总料量PV", Font, Brushes.Black, new Rectangle(0, 35, 60, 15), this.sf);
            graphics.DrawString(t_total_pv_w_3s + "t/h", Font, Brushes.Black, new Rectangle(0, 49, 60, 15), this.sf);

           


            graphics.DrawString("下料量", Font, Brushes.Black, new Rectangle(3, Height - 50, 60, 20), this.sf);

            graphics.DrawString("设定t/h", Font, Brushes.Black, new Rectangle(3, Height - 30, 60, 15), this.sf);
            graphics.DrawString("实际t/h", Font, Brushes.Black, new Rectangle(3, Height - 15, 60, 15), this.sf);

            this.pipeLine3.Width = Width - 75;
            this.pipeLine3.Location = new System.Drawing.Point(75, Height - 45);
            this.pipeLine4.Height = 100;
            //this.pipeLine4.Location = new System.Drawing.Point(Width - this.pipeLine4.Width-125, Height - this.pipeLine4.Height + 1);
            this.pipeLine4.Location = new System.Drawing.Point(this.pipeLine3.Location.X+ this.pipeLine3.Width-15, this.pipeLine3.Location.Y+15);

            //this.pipeLine4.Height = 52;
            base.OnPaint(e);
        }
        private StringFormat sf;

        public string t_total_sp_w_3s;
        public string t_total_pv_w_3s;

        [Browsable(true), Description("获取或设置 总料量SP，用于绘制在控件上的信息。"), DefaultValue(typeof(string), "总料量SP"), Category("Appearance")]
        public string T_TOTAL_SP_W_3S
        {
            get
            {
                return this.t_total_sp_w_3s;
            }
            set
            {
                this.t_total_sp_w_3s = value;
                //base.Invalidate();
            }
        }
        [Browsable(true), Description("获取或设置 总料量PV，用于绘制在控件上的信息。"), DefaultValue(typeof(string), "总料量PV"), Category("Appearance")]
        public string T_TOTAL_PV_W_3S
        {
            get
            {
                return this.t_total_pv_w_3s;
            }
            set
            {
                this.t_total_pv_w_3s = value;
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
        public override void Refresh()
        {
            base.Refresh();
        }
    }
}
