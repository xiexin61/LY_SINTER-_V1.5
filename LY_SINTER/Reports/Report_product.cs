using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataBase;

namespace LY_SINTER.Reports
{
    public partial class Report_product : UserControl
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public Report_product()
        {
            InitializeComponent();
            this.rowMergeView1.AddSpanHeader(1, 1, "台车速度");
            this.rowMergeView1.AddSpanHeader(2, 1, "布料厚度");
            this.rowMergeView1.AddSpanHeader(3, 1, "点火温度");
            this.rowMergeView1.AddSpanHeader(4, 2, "主管负压Kpa");
            this.rowMergeView1.AddSpanHeader(6, 2, "主管废气温度℃");
            this.rowMergeView1.AddSpanHeader(8, 1, "漏风率");
            this.rowMergeView1.AddSpanHeader(9, 1, "煤气压力");
            this.rowMergeView1.AddSpanHeader(10, 1, "煤气流量");
            this.rowMergeView1.AddSpanHeader(11, 1, "余热产气");
            this.rowMergeView1.AddSpanHeader(12, 7, "1#主管风箱温度℃");
            this.rowMergeView1.AddSpanHeader(19, 7, "2#主管风箱温度℃");
            this.rowMergeView1.AddSpanHeader(26, 1, "混合料水分");
            this.rowMergeView1.AddSpanHeader(27, 1, "混合料温");
            this.rowMergeView1.AddSpanHeader(28, 1, "热水水温");
            this.rowMergeView1.AddSpanHeader(29, 1, "混合料粒度");
            this.rowMergeView1.AddSpanHeader(30, 9, "混合料水分");


        }
        public void getData()
        {
            string sql = "select * from ";

        }
    }
}
