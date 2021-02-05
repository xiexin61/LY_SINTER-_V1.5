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
    public partial class HuanLengJiUC : UserControl
    {
        public HuanLengJiUC()
        {
            InitializeComponent();
            base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            this.sf = new StringFormat();
            this.sf.Alignment = StringAlignment.Near;//.Center;
            this.sf.LineAlignment = StringAlignment.Center;

            shuziSf.Alignment = StringAlignment.Center; //居中
            shuziSf.LineAlignment = StringAlignment.Center;
            //DrawRectangle(panel2.CreateGraphics(), 0, 0, panel2.Width, panel2.Height);
            //panel2.Refresh();



            myBitmap1 =global::NBSJ_MAIN_UC.Properties.Resources.板式给矿机入口;
            myBitmap2 =global::NBSJ_MAIN_UC.Properties.Resources.板式给矿机2;
            myBitmap3 = global::NBSJ_MAIN_UC.Properties.Resources.环冷机烟筒;
        }
        double fontWidth = 0.3f;
        Bitmap myBitmap1 = null;
        Bitmap myBitmap2 = null;
        Bitmap myBitmap3 = null;

        float xishuK = (float)93 / (float)110;
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            //e.Graphics.DrawString("设定机速：2.8m/min", Font, Brushes.Gray, new Rectangle(-10 + loaclPoint.X, height - 40, Width + 20, 20), this.sf);
            Rectangle 矩形1 = new Rectangle((int)0, (int)0, (int)this.Width, (int)(this.Height * 0.3f));
            Rectangle 矩形2 = new Rectangle((int)0, (int)13, (int)this.Width, (int)(this.Height * 0.3f));
            //graphics.DrawString("设定机速：2.8m/min", Font, shuziColor, 矩形1, sf);
            //graphics.DrawString("实际机速：2.8m/min", Font, shuziColor, 矩形2, sf);
            //graphics.DrawString("机速SP:"+ hlj_Set_SPEED + "m/min", Font, shuziColor, 矩形1, sf);
            graphics.DrawString("实际机速:" + hlj_Read_SPEED + "m/min", Font, shuziColor, 矩形2, sf);


            graphics.DrawImage(myBitmap1, (int)(this.Width * 0.65f), 0, (int)(this.Width * 0.2f), (int)(this.Height * 0.3f));
            this.radioButton1.Location = new System.Drawing.Point((int)(this.Width * 0.65f - this.radioButton1.Width), (int)(this.Height * 0.15f));

            Rectangle 矩形3 = new Rectangle((int)(this.Width * 0.80f), (int)0, (int)(this.Width * fontWidth*0.5), (int)(this.Height * 0.2f));
            //graphics.DrawString("入口:", Font, shuziColor, 矩形3, sf);//"入口温度"

            矩形3 = new Rectangle((int)(this.Width * 0.8f), (int)0, (int)(60), (int)(30));
           graphics.DrawString("入口:" + rkTemp + "℃", Font, shuziColor, 矩形3, sf);//"入口温度"
            graphics.DrawImage(myBitmap3, (int)(this.Width * 0.25f), (int)-28, (int)(this.Width * 0.07f), (int)(this.Height * 0.5f));
            graphics.DrawImage(myBitmap3, (int)(this.Width * 0.45f), (int)-28, (int)(this.Width * 0.07f), (int)(this.Height * 0.5f));
            

            DrawRectangle(graphics, 0, (int)(this.Height * 0.3f), (int)(this.Width * 0.85f), (int)(this.Height * 0.55f));

          

           
            int y3 = (int)(this.Height * 0.55f);
            float h3 = (float)this.Height * 0.45f;
            //float w3 = h3 * xishuK;
            //this.fanUC1.Location = new System.Drawing.Point(1, y3);
            //this.fanUC1.Size = new System.Drawing.Size((int)w3, (int)h3);

            //this.fanUC2.Location = new System.Drawing.Point((int)w3 + 2, y3);
            //this.fanUC2.Size = new System.Drawing.Size((int)w3, (int)h3);

            //this.fanUC3.Location = new System.Drawing.Point((int)(w3 * 2 + 3), y3);
            //this.fanUC3.Size = new System.Drawing.Size((int)w3, (int)h3);

            //this.fanUC4.Location = new System.Drawing.Point((int)(w3 * 3 + 4), y3);
            //this.fanUC4.Size = new System.Drawing.Size((int)w3, (int)h3);

            //this.fanUC5.Location = new System.Drawing.Point((int)(w3 * 4 + 5), y3);
            //this.fanUC5.Size = new System.Drawing.Size((int)w3, (int)h3);

            float w3 = this.Width * 0.65f / 4 - 4;
            this.fanUC1.Location = new System.Drawing.Point(1, y3);
            this.fanUC1.Size = new System.Drawing.Size((int)w3-20, (int)h3);

            this.fanUC2.Location = new System.Drawing.Point((int)w3 + 2, y3);
            this.fanUC2.Size = new System.Drawing.Size((int)w3-20, (int)h3);

            this.fanUC3.Location = new System.Drawing.Point((int)(w3 * 2 + 3), y3);
            this.fanUC3.Size = new System.Drawing.Size((int)w3-20, (int)h3);

            this.fanUC4.Location = new System.Drawing.Point((int)(w3 * 3 + 4), y3);
            this.fanUC4.Size = new System.Drawing.Size((int)w3-20, (int)h3);

            /*this.fanUC5.Location = new System.Drawing.Point((int)(w3 * 4 + 5), y3);
            this.fanUC5.Size = new System.Drawing.Size((int)w3, (int)h3);*/



            graphics.DrawImage(myBitmap2, (int)(this.Width * 0.65f), y3, (int)(this.Width * 0.2f), (int)h3);
            //this.radioButton2.Location = new System.Drawing.Point((int)(this.Width * 0.65f - this.radioButton2.Width), (int)(y3+ h3 * 0.05f));
            this.radioButton2.Location = new System.Drawing.Point((int)(this.Width * 0.675f), (int)(y3 + h3 * 0.05f));
            //Rectangle 矩形4 = new Rectangle((int)(this.Width * 0.65f), y3+ (int)(h3/3)+2, (int)(this.Width * 0.2f), (int)(h3 * 2 /3));
            Rectangle 矩形4 = new Rectangle((int)(this.Width * 0.65f), y3 + (int)(h3 / 5), (int)(this.Width * 0.2f), (int)(h3 * 2 / 3));

            //if (isrunHlj)
            //{
            //    graphics.DrawString(bsgkj_Cw + "t", Font, Brushes.Green, 矩形4, shuziSf);
            //}
            //else
            //{
            //    graphics.DrawString(bsgkj_Cw + "t", Font, Brushes.Black, 矩形4, shuziSf);
            //}

            //矩形4 = new Rectangle((int)(this.Width * 0.65f), y3 + (int)(h3 / 2), (int)(this.Width * 0.2f), (int)(h3 * 2 / 3));
            矩形4 = new Rectangle((int)(this.Width * 0.65f), y3 + (int)(h3 / 3.5f), (int)(this.Width * 0.2f), (int)(h3 * 2 / 3));
            if (isrunHlj)
            {
                //graphics.DrawString("板式给矿机", shuziFont, Brushes.Green, 矩形4, shuziSf);
                graphics.DrawString("板式给矿机", Font, Brushes.Green, 矩形4, shuziSf);
            }
            else
            {
                ////g.DrawString("环冷机", shuziFont, Brushes.Green, 矩形22, shuziSf);
                //graphics.DrawString("板式给矿机", shuziFont, Brushes.Black, 矩形4, shuziSf);
                graphics.DrawString("板式给矿机", Font, Brushes.Black, 矩形4, shuziSf);
            }

            矩形4 = new Rectangle((int)(this.Width * 0.8f), y3, 60, 30);
            graphics.DrawString("出口:" + ckTemp + "℃", Font, shuziColor, 矩形4, sf);//"出口温度"
            ////Rectangle 矩形5 = new Rectangle((int)(this.Width * 0.85f), y3 + (int)(h3 * 0.4f), (int)(this.Width * fontWidth), 30);
            Rectangle 矩形5 = new Rectangle((int)(this.Width * 0.82f), this.Height - 32, 60, 30);
            ////graphics.DrawString("设定机速", Font, shuziColor, 矩形5, sf);
            //graphics.DrawString(bsgkj_Set_SPEED + "Hz", Font, shuziColor, 矩形5, sf);
            graphics.DrawString("实际频率:", Font, shuziColor, 矩形5, sf);
            //Rectangle 矩形6 = new Rectangle((int)(this.Width * 0.85f), (int)(this.Height * 0.86f), (int)(this.Width * fontWidth), 30);
            Rectangle 矩形6 = new Rectangle((int)(this.Width * 0.82f), this.Height - 12, 60, 15);
            //graphics.DrawString("实际机速", Font, shuziColor, 矩形6, sf);
            graphics.DrawString(bsgkj_Read_SPEED + "Hz", Font, shuziColor, 矩形6, sf);

        }

        private StringFormat sf;
        Color rectangleColor = Color.FromArgb(0x5e, 0x88, 0xb8);//5e88b8
        Color rectangleColorG = Color.FromArgb(0x00, 0x99, 0x00);//5e88b8

        Font shuziFont = new Font("宋体", 14F);
        Brush shuziColor = Brushes.Black;
        StringFormat shuziSf = new StringFormat();
        /// <summary>
        /// 绘制矩形
        /// </summary>
        /// <param name="g"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        private void DrawRectangle(Graphics g, float x1, float y1, float x2, float y2)
        {

            float recWidth = (x2 - x1 - 21) / 22;
            float recHeight = y2 - y1;

            float xV = 0;
            for (int num = 0; num < 22; num++)
            {
                xV = x1 + recWidth * num + num * 1;
                //g.FillRectangle(new SolidBrush(Color.FromArgb(150, rectangleColor)), xV, y1, recWidth, recHeight);
                ////g.DrawString((num + 1).ToString(), Font, new SolidBrush(Color.Black), xV + recWidth / 2, y1 + recHeight/2);
                Rectangle 矩形 = new Rectangle((int)xV, (int)y1, (int)recWidth, (int)recHeight);
                if (isrunHlj)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(200, rectangleColorG)), 矩形);
                }
                else
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(100, rectangleColor)), 矩形);
                }
            }
            Rectangle 矩形22 = new Rectangle((int)x1, (int)y1, (int)(x2 - x1), (int)(y2 - y1));
           
              
               
                g.DrawString("环冷机", shuziFont, Brushes.Black, 矩形22, shuziSf);
                   }

        public void SetFengJi(bool IsRun1, bool IsRun2, bool IsRun3, bool IsRun4, bool IsRun5)
        {

            this.fanUC1.IsRun = IsRun1;

            this.fanUC2.IsRun = IsRun2;

            this.fanUC3.IsRun = IsRun3;

            this.fanUC4.IsRun = IsRun4;

            /*this.fanUC5.IsRun = IsRun5;*/

        }


        /// <summary>
        /// 重绘控件
        /// </summary>
        public void RefreshInvalidate()
        {
            base.Invalidate();
        }
        bool isrunHlj = false;
        [Browsable(true), Description("环冷机的启动和停止。"), DefaultValue(typeof(bool), "false"), Category("Appearance")]
        public bool IsRunHlj
        {
            get
            {
                return this.isrunHlj;
            }
            set
            {
                this.isrunHlj = value;
            }
        }
        bool isrunBsGkj = false;
        [Browsable(true), Description("板式给矿机的启动和停止。"), DefaultValue(typeof(bool), "false"), Category("Appearance")]
        public bool IsRunBsGkj
        {
            get
            {
                return this.isrunBsGkj;
            }
            set
            {
                this.isrunBsGkj = value;
            }
        }
        string rkTemp = "";
        string ckTemp = "";
        [Browsable(true), Description("入口温度。"), DefaultValue(typeof(string), "入口温度值"), Category("Appearance")]
        public string RkTemp
        {
            get
            {
                return this.rkTemp;
            }
            set
            {
                this.rkTemp = value;
            }
        }
        [Browsable(true), Description("出口温度。"), DefaultValue(typeof(string), "出口温度值"), Category("Appearance")]
        public string CkTemp
        {
            get
            {
                return this.ckTemp;
            }
            set
            {
                this.ckTemp = value;
            }
        }

        string hlj_Set_SPEED = "";
        string hlj_Read_SPEED = "";
        string bsgkj_Set_SPEED = "";
        string bsgkj_Read_SPEED = "";
        [Browsable(true), Description("环冷机设定机速。"), DefaultValue(typeof(string), "环冷机设定机速值"), Category("Appearance")]
        public string Hlj_Set_SPEED
        {
            get
            {
                return this.hlj_Set_SPEED;
            }
            set
            {
                this.hlj_Set_SPEED = value;
            }
        }
        [Browsable(true), Description("环冷机机速反馈值。"), DefaultValue(typeof(string), "环冷机机速反馈值"), Category("Appearance")]
        public string Hlj_Read_SPEED
        {
            get
            {
                return this.hlj_Read_SPEED;
            }
            set
            {
                this.hlj_Read_SPEED = value;
            }
        }

        [Browsable(true), Description("板式给矿机设定机速。"), DefaultValue(typeof(string), "板式给矿机设定机速值"), Category("Appearance")]
        public string BsGkj_Set_SPEED
        {
            get
            {
                return this.bsgkj_Set_SPEED;
            }
            set
            {
                this.bsgkj_Set_SPEED = value;
            }
        }
        [Browsable(true), Description("板式给矿机机速反馈值。"), DefaultValue(typeof(string), "板式给矿机机速反馈值"), Category("Appearance")]
        public string BsGkj_Read_SPEED
        {
            get
            {
                return this.bsgkj_Read_SPEED;
            }
            set
            {
                this.bsgkj_Read_SPEED = value;
            }
        }

        string bsgkj_Cw = "0";
        /// <summary>
        /// 板式给矿机机仓位
        /// </summary>
        [Browsable(true), Description("板式给矿机机仓位。"), DefaultValue(typeof(string), "板式给矿机机仓位"), Category("Appearance")]
        public string BsGkj_Cw
        {
            get
            {
                return this.bsgkj_Cw;
            }
            set
            {
                this.bsgkj_Cw = value;
            }
        }

        private void HuanLengJiUC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //int xL = (int)(this.Width * 0.65f);
            //int yL = (int)(this.Height * 0.55f);
            int y3 = (int)(this.Height * 0.55f);
            float h3 = (float)this.Height * 0.45f;
            var quyu = new Rectangle((int)(this.Width * 0.65f), y3, (int)(this.Width * 0.2f), (int)h3);
            //取样点区域
            if (e.X > quyu.X && e.X < (quyu.X + quyu.Width) && e.Y > quyu.Y && e.Y < (quyu.Y + quyu.Height * 0.4f))
            {
                using (UserControlIndex.FormDaoTui frm = new UserControlIndex.FormDaoTui())
                {
                    frm.Init(12);
                    frm.ShowDialog();
                }
            }
            else
            {
                quyu = new Rectangle((int)(this.Width * 0.65f), 0, (int)(this.Width * 0.2f), (int)(this.Height * 0.3f));
                //取样点区域
                if (e.X > quyu.X && e.X < (quyu.X + quyu.Width) && e.Y > quyu.Y && e.Y < (quyu.Y + quyu.Height))
                {
                    using (UserControlIndex.FormDaoTui frm = new UserControlIndex.FormDaoTui())
                    {
                        frm.Init(11);
                        frm.ShowDialog();
                    }
                }
            }

        }


        DateTime dtimeRbtnDouble = DateTime.Now;
        private void rbtnDouble_Click(object sender, EventArgs e)
        {
            if (dtimeRbtnDouble.AddMilliseconds(1000) > DateTime.Now)
            {
                System.Windows.Forms.RadioButton rbtnS = (System.Windows.Forms.RadioButton)sender;
                using (UserControlIndex.FormDaoTui frm = new UserControlIndex.FormDaoTui())
                {
                    if (rbtnS.Name == "radioButton1")
                    {
                        frm.Init(11);
                        frm.ShowDialog();
                    }
                    else if (rbtnS.Name == "radioButton2")
                    {
                        frm.Init(12);
                        frm.ShowDialog();
                    }
                }
            }
            dtimeRbtnDouble = DateTime.Now;
        }

    }
}
