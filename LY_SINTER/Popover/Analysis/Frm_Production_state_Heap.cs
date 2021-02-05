using DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLog;

namespace LY_SINTER.Popover.Analysis
{
    public partial class Frm_Production_state_Heap : Form
    {
        public static bool isopen = false;
        DBSQL dBSQL = new DBSQL(DataBase.ConstParameters.strCon);
        public vLog _vLog { get; set; }
        public Frm_Production_state_Heap()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Analysis\\Frm_Production_state_Heap\\");
            time_begin_end();
        }
        /// <summary>
        /// 页面二级列明
        /// </summary>
        public void d1_col()
        {
            //添加列说明
            this.d2.AddSpanHeader(3, 3, "配矿比例");
            this.d2.AddSpanHeader(22, 7, "BTP温度");
            this.d2.AddSpanHeader(29, 3, "大烟道温度");
            this.d2.AddSpanHeader(32, 3, "BTP位置");
            this.d2.AddSpanHeader(35, 3, "大烟道负压");
            this.d2.AddSpanHeader(42, 3, "主排频率");
            this.d2.AddSpanHeader(45, 3, "主排电流");
        }
        /// <summary>
        /// 开始时间-结束时间赋值
        /// </summary>
        public void time_begin_end()
        {
            DateTime time_end = DateTime.Now;
            DateTime time_begin = time_end.AddDays(-7);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }
    }
}
