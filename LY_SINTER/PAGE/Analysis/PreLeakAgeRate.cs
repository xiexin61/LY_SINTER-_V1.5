using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LY_SINTER.Custom;
using DataBase;

namespace LY_SINTER.PAGE.Analysis
{
    public partial class PreLeakAgeRate : UserControl
    {
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        public PreLeakAgeRate()
        {
            InitializeComponent();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            time_begin_end();
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
        //查询按钮
        private void simpleButton3_Click(object sender, EventArgs e)
        {

        }
        //实时按钮
        private void simpleButton4_Click(object sender, EventArgs e)
        {

        }
        //历史数据
        private void simpleButton1_Click(object sender, EventArgs e)
        {

        }
        //参数维护
        private void simpleButton2_Click(object sender, EventArgs e)
        {

        }
        public void Timer_state()
        {

        }
        public void _Clear()
        {
            this.Dispose();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {

        }
    }
}
