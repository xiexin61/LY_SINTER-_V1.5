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
    public partial class BottleAllUC2 : UserControl
    {
        public BottleAllUC2()
        {
            InitializeComponent();
        }

        private void Init()
        {
            int width = base.Width;
            int height = base.Height / 2;

            //this.PaintMain(e.Graphics, (float)base.Width, (float)base.Height, (float)num, (float)num2);
            int curCount = 15;

            for (int num = 0; num < curCount; num++)
            {

            }

        }


        protected override void OnPaint(PaintEventArgs e)
        {
            int curCount = tagValue.Count;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;


            int width = base.Width / curCount;
            int height = base.Height / 2;

            for (int num = 0; num < tagValue.Count; num++)
            {

            }
        }

        List<AbstractBottle> tagValue = new List<AbstractBottle>();
        [Browsable(true), Description("料仓列表。"), Category("Appearance")]
        public List<AbstractBottle> BottleList
        {
            get
            {
                return tagValue;
            }
            set
            {
                tagValue = value;
            }
        }


    }
}
