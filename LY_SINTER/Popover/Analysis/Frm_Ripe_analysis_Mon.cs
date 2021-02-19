using DataBase;
using LY_SINTER.Custom;
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
    public partial class Frm_Ripe_analysis_Mon : Form
    {
        public vLog _vLog { get; set; }
        public static bool isopen = false;
        DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        public Frm_Ripe_analysis_Mon()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Analysis\\Frm_Ripe_analysis_Mon\\");
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
         //   Class_text(2, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
          //  Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
            Time_now();
        }
        /// <summary>
        /// 开始时间&结束时间赋值
        /// </summary>
        public void dateTimePicker_value()
        {
            //结束时间
            DateTime time_end = DateTime.Now;
            //开始时间
            DateTime time_begin = time_end.AddDays(-7);
            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }
        /// <summary>
        /// 堆数据查询
        /// _flag :1 一号烧结机 2：二号烧结机 3：平均烧结机
        /// time_begin:开始时间
        /// time_end结束时间
        /// </summary>
        /// <param name="_flag"></param>
        public void Class_text(int _flag, string time_begin, string time_end)
        {
            try
            {
                string _NAME_CLASS = "";
                if (_flag == 1)
                {
                    _NAME_CLASS = "1#烧结机";
                }
                else if (_flag == 2)
                {
                    _NAME_CLASS = "2#烧结机";
                }
                else if(_flag == 3)
                {
                    _NAME_CLASS = "3#烧结机";
                }
                else
                {
                    _NAME_CLASS = "1、2#烧结机平均";
                }
                var _SQL = "select " + _NAME_CLASS + " AS ID, R_003_CONTROL,R_005_CONTROL,R_008_CONTROL,FEO_06_CONTROL,FEO_1_CONTROL,MGO_015_CONTROL,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P2O5,C_R,C_K2O,C_NA2O,C_PB,C_ZN,C_AL2O3_SIO2,C_MGO_AL2O3,C_TOTAL,C_TI,RI,RDI_3_15,GRIT_5,GRIT_5_10,GRIT_10_16,GRIT_16_25,GRIT_25_40,GRIT_40,PATCL_AV,GRIT_10,GRIT_10_40,TFE_STANDARD,SIO2_STANDARD,CAO_STANDARD,R_STANDARD,FEO_STANDARD,MGO_STANDARD " +
                    " from MC_NUMCAL_INTERFACE_6_MONTH where DL_FLAG=" + _flag + " and RECORD_TIME between '" + time_begin + "' and '" + time_end + "' order by RECORD_TIME desc";
                DataTable _table = _dBSQL.GetCommand(_SQL);
                if (_table != null && _table.Rows.Count > 0)
                {
                    if (_flag == 1)
                    {
                        this.dataGridView1.DataSource = _table;
                    }
                    if (_flag == 2)
                    {
                        this.dataGridView2.DataSource = _table;
                    }
                    else  if (_flag == 3)
                    {
                        this.dataGridView1.DataSource = _table;
                    }
                    else
                    {
                        this.dataGridView3.DataSource = _table;
                    }

                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("Class_text方法失败" + ee.ToString(), -1);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
           // Class_text(2, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
         //   Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            dateTimePicker_value();
            Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
         //   Class_text(2, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
          //  Class_text(3, textBox_begin.Text.ToString(), textBox_end.Text.ToString());
        }

        /// <summary>
        /// 最新调整时间
        /// </summary>
        public void Time_now()
        {
            try
            {
                string sql1 = "select top 1 RECORD_TIME from MC_NUMCAL_INTERFACE_6_MONTH order by RECORD_TIME desc";
                DataTable dataTable1 = _dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    label3.Text = "最新调整时间:" + dataTable1.Rows[0][0].ToString();
                }
                else
                {
                    label3.Text = "";
                }
            }
            catch
            { }
        }
    }
}
