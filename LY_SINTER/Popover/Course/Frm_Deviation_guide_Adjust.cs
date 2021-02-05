using DataBase;
using LY_SINTER.Custom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLog;

namespace LY_SINTER.Popover.Course
{
    public partial class Frm_Deviation_guide_Adjust : Form
    {
        public vLog _vLog { get; set; }
        public static bool isopen = false;
        String biaoming3 = "";
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        List<string> yw = new List<string>();
        List<string> zw = new List<string>();
        public Frm_Deviation_guide_Adjust()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Quality\\Frm_Deviation_guide_Adjust\\");
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            shujubiao();

            show(1);
        }
        public void shujubiao()
        {

            biaoming3 = "MC_UNIFORMCAL_result";

        }
        private void zhongwen()
        {
            try
            {
                for (int i = 0; i < yw.Count; i++)
                {
                    if (yw[i] == "TIMESTAMP")
                    {
                        zw.Add("插入时间");
                    }
                    else if (yw[i] == "UNCAL_flag")
                    {
                        zw.Add("模型下发标志");
                    }
                    else if (yw[i] == "UNCAL_SG_FLAG_1")
                    {
                        zw.Add("1#小闸门一级投入开关");
                    }
                    else if (yw[i] == "UNCAL_SG_FLAG_2")
                    {
                        zw.Add("2#小闸门一级投入开关");
                    }
                    else if (yw[i] == "UNCAL_SG_FLAG_3")
                    {
                        zw.Add("3#小闸门一级投入开关");
                    }
                    else if (yw[i] == "UNCAL_SG_FLAG_4")
                    {
                        zw.Add("4#小闸门一级投入开关");
                    }
                    else if (yw[i] == "UNCAL_SG_FLAG_5")
                    {
                        zw.Add("5#小闸门一级投入开关");
                    }
                    else if (yw[i] == "UNCAL_SG_FLAG_6")
                    {
                        zw.Add("6#小闸门一级投入开关");
                    }
                    else if (yw[i] == "UNCAL_SG_INPUT_BUTTON")
                    {
                        zw.Add("均匀一致性模型投入按钮");
                    }
                    else if (yw[i] == "UNCAL_V_SP")
                    {
                        zw.Add("烧结机设定机速");
                    }
                    else if (yw[i] == "UNCAL_V_PV")
                    {
                        zw.Add("烧结机实际机速");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_1")
                    {
                        zw.Add("1#小闸门设定布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_2")
                    {
                        zw.Add("2#小闸门设定布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_3")
                    {
                        zw.Add("3#小闸门设定布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_4")
                    {
                        zw.Add("4#小闸门设定布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_5")
                    {
                        zw.Add("5#小闸门设定布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_6")
                    {
                        zw.Add("6#小闸门设定布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_1")
                    {
                        zw.Add("1#小闸门实际布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_2")
                    {
                        zw.Add("2#小闸门实际布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_3")
                    {
                        zw.Add("3#小闸门实际布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_4")
                    {
                        zw.Add("4#小闸门实际布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_5")
                    {
                        zw.Add("5#小闸门实际布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_6")
                    {
                        zw.Add("6#小闸门实际布料厚度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_SP_1")
                    {
                        zw.Add("1#小闸门设定开度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_SP_2")
                    {
                        zw.Add("2#小闸门设定开度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_SP_3")
                    {
                        zw.Add("3#小闸门设定开度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_SP_4")
                    {
                        zw.Add("4#小闸门设定开度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_SP_5")
                    {
                        zw.Add("5#小闸门设定开度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_SP_6")
                    {
                        zw.Add("6#小闸门设定开度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_PV_1")
                    {
                        zw.Add("1#小闸门反馈开度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_PV_2")
                    {
                        zw.Add("2#小闸门反馈开度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_PV_3")
                    {
                        zw.Add("3#小闸门反馈开度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_PV_4")
                    {
                        zw.Add("4#小闸门反馈开度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_PV_5")
                    {
                        zw.Add("5#小闸门反馈开度");
                    }
                    else if (yw[i] == "UNCAL_SG_O_PV_6")
                    {
                        zw.Add("6#小闸门反馈开度");
                    }
                    else if (yw[i] == "UNCAL_THICK_PV")
                    {
                        zw.Add("布料厚度反馈值");
                    }
                    else if (yw[i] == "UNCAL_THICK_SP")
                    {
                        zw.Add("布料厚度设定值");
                    }
                    else if (yw[i] == "UNCAL_X_BTP_1")
                    {
                        zw.Add("倒推时间内，1#小闸门烧结终点位置");
                    }
                    else if (yw[i] == "UNCAL_X_BTP_2")
                    {
                        zw.Add("倒推时间内，2#小闸门烧结终点位置");
                    }
                    else if (yw[i] == "UNCAL_X_BTP_3")
                    {
                        zw.Add("倒推时间内，3#小闸门烧结终点位置");
                    }
                    else if (yw[i] == "UNCAL_X_BTP_4")
                    {
                        zw.Add("倒推时间内，4#小闸门烧结终点位置");
                    }
                    else if (yw[i] == "UNCAL_X_BTP_5")
                    {
                        zw.Add("倒推时间内，5#小闸门烧结终点位置");
                    }
                    else if (yw[i] == "UNCAL_X_BTP_6")
                    {
                        zw.Add("倒推时间内，6#小闸门烧结终点位置");
                    }
                    else if (yw[i] == "UNCAL_X_BTP_AV")
                    {
                        zw.Add("倒推时间内，1-6#小闸门烧结终点平均位置");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_AV_1")
                    {
                        zw.Add("倒推时间内，1#小闸门实际布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_AV_2")
                    {
                        zw.Add("倒推时间内，2#小闸门实际布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_AV_3")
                    {
                        zw.Add("倒推时间内，3#小闸门实际布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_AV_4")
                    {
                        zw.Add("倒推时间内，4#小闸门实际布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_AV_5")
                    {
                        zw.Add("倒推时间内，5#小闸门实际布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_PV_AV_6")
                    {
                        zw.Add("倒推时间内，6#小闸门实际布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_AV_1")
                    {
                        zw.Add("倒推时间内，1#小闸门设定布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_AV_2")
                    {
                        zw.Add("倒推时间内，2#小闸门设定布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_AV_3")
                    {
                        zw.Add("倒推时间内，3#小闸门设定布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_AV_4")
                    {
                        zw.Add("倒推时间内，4#小闸门设定布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_AV_5")
                    {
                        zw.Add("倒推时间内，5#小闸门设定布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_SP_AV_6")
                    {
                        zw.Add("倒推时间内，6#小闸门设定布料厚度平均值");
                    }
                    else if (yw[i] == "UNCAL_T_N")
                    {
                        zw.Add("倒推时间");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_A_1")
                    {
                        zw.Add("1#小闸门计算应调料厚");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_A_2")
                    {
                        zw.Add("2#小闸门计算应调料厚");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_A_3")
                    {
                        zw.Add("3#小闸门计算应调料厚");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_A_4")
                    {
                        zw.Add("4#小闸门计算应调料厚");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_A_5")
                    {
                        zw.Add("5#小闸门计算应调料厚");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_A_6")
                    {
                        zw.Add("6#小闸门计算应调料厚");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_AC_1")
                    {
                        zw.Add("1#小闸门计算应调料厚修正值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_AC_2")
                    {
                        zw.Add("2#小闸门计算应调料厚修正值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_AC_3")
                    {
                        zw.Add("3#小闸门计算应调料厚修正值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_AC_4")
                    {
                        zw.Add("4#小闸门计算应调料厚修正值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_AC_5")
                    {
                        zw.Add("5#小闸门计算应调料厚修正值");
                    }
                    else if (yw[i] == "UNCAL_SG_TH_AC_6")
                    {
                        zw.Add("6#小闸门计算应调料厚修正值");
                    }
                    else if (yw[i] == "UNCAL_X_BTP_AVG")
                    {
                        zw.Add("实时综合烧结终点平均位置");
                    }
                    else if (yw[i] == "UNCAL_T1_TE_BTP_1")
                    {
                        zw.Add("1#烧结终点位置处温度");
                    }
                    else if (yw[i] == "UNCAL_T1_TE_BTP_2")
                    {
                        zw.Add("2#烧结终点位置处温度");
                    }
                    else if (yw[i] == "UNCAL_T1_TE_BTP_3")
                    {
                        zw.Add("3#烧结终点位置处温度");
                    }
                    else if (yw[i] == "UNCAL_T1_TE_BTP_4")
                    {
                        zw.Add("4#烧结终点位置处温度");
                    }
                    else if (yw[i] == "UNCAL_T1_TE_BTP_5")
                    {
                        zw.Add("5#烧结终点位置处温度");
                    }
                    else if (yw[i] == "UNCAL_T1_TE_BTP_6")
                    {
                        zw.Add("6#烧结终点位置处温度");
                    }
                    else if (yw[i] == "UNCAL_T1_TE_BTP_AVG")
                    {
                        zw.Add("实时综合烧结终点平均位置处温度");
                    }
                    else if (yw[i] == "UNCAL_SG_D_1")
                    {
                        zw.Add("1#小闸门实时布料厚度偏差值");
                    }
                    else if (yw[i] == "UNCAL_SG_D_2")
                    {
                        zw.Add("2#小闸门实时布料厚度偏差值");
                    }
                    else if (yw[i] == "UNCAL_SG_D_3")
                    {
                        zw.Add("3#小闸门实时布料厚度偏差值");
                    }
                    else if (yw[i] == "UNCAL_SG_D_4")
                    {
                        zw.Add("4#小闸门实时布料厚度偏差值");
                    }
                    else if (yw[i] == "UNCAL_SG_D_5")
                    {
                        zw.Add("5#小闸门实时布料厚度偏差值");
                    }
                    else if (yw[i] == "UNCAL_SG_D_6")
                    {
                        zw.Add("6#小闸门实时布料厚度偏差值");
                    }
                    else if (yw[i] == "UNCAL_E_1")
                    {
                        zw.Add("1#小闸门均匀一致性指数");
                    }
                    else if (yw[i] == "UNCAL_E_2")
                    {
                        zw.Add("2#小闸门均匀一致性指数");
                    }
                    else if (yw[i] == "UNCAL_E_3")
                    {
                        zw.Add("3#小闸门均匀一致性指数");
                    }
                    else if (yw[i] == "UNCAL_E_4")
                    {
                        zw.Add("4#小闸门均匀一致性指数");
                    }
                    else if (yw[i] == "UNCAL_E_5")
                    {
                        zw.Add("5#小闸门均匀一致性指数");
                    }
                    else if (yw[i] == "UNCAL_E_6")
                    {
                        zw.Add("6#小闸门均匀一致性指数");
                    }
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 查询数据
        /// _FALG = 1 实时 _FALG = 2 历史
        /// </summary>
        public void show(int _FALG)
        {
            try
            {
                if (_FALG == 1)
                {
                    var sql = "select top (20)  " +
                        "TIMESTAMP," +
                        "(case UNCAL_flag when '2' then '只插库不下发' when '1' then '既插库又下发' else '异常' end)as UNCAL_flag," +
                        "(case UNCAL_SG_FLAG_1 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_1," +
                        "(case UNCAL_SG_FLAG_2 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_2," +
                        "(case UNCAL_SG_FLAG_3 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_3," +
                        "(case UNCAL_SG_FLAG_4 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_4," +
                        "(case UNCAL_SG_FLAG_5 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_5," +
                        "(case UNCAL_SG_FLAG_6 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_6," +
                        // "(case UNCAL_SG_INPUT_BUTTON when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_INPUT_BUTTON," +
                        "UNCAL_X_BTP_1,UNCAL_X_BTP_2,UNCAL_X_BTP_3,UNCAL_X_BTP_4," +
                        "UNCAL_X_BTP_5,UNCAL_X_BTP_6,UNCAL_X_BTP_AV,UNCAL_SG_TH_PV_AV_1,UNCAL_SG_TH_PV_AV_2," +
                        "UNCAL_SG_TH_PV_AV_3,UNCAL_SG_TH_PV_AV_4,UNCAL_SG_TH_PV_AV_5,UNCAL_SG_TH_PV_AV_6," +
                        "UNCAL_SG_TH_SP_AV_1,UNCAL_SG_TH_SP_AV_2,UNCAL_SG_TH_SP_AV_3,UNCAL_SG_TH_SP_AV_4," +
                        "UNCAL_SG_TH_SP_AV_5,UNCAL_SG_TH_SP_AV_6,UNCAL_T_N,UNCAL_SG_TH_A_1,UNCAL_SG_TH_A_2," +
                        "UNCAL_SG_TH_A_3,UNCAL_SG_TH_A_4,UNCAL_SG_TH_A_5,UNCAL_SG_TH_A_6,UNCAL_SG_TH_AC_1," +
                        "UNCAL_SG_TH_AC_2,UNCAL_SG_TH_AC_3,UNCAL_SG_TH_AC_4,UNCAL_SG_TH_AC_5,UNCAL_SG_TH_AC_6" +
                        " from MC_UNIFORMCAL_result order by TIMESTAMP desc";
                    DataTable _data = dBSQL.GetCommand(sql);
                    if (_data != null)
                    {
                        dataGridView1.DataSource = _data;
                    }
                }
                else
                {
                    var sql = "select  " +
                       "TIMESTAMP," +
                       "(case UNCAL_flag when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_flag," +
                       "(case UNCAL_SG_FLAG_1 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_1," +
                       "(case UNCAL_SG_FLAG_2 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_2," +
                       "(case UNCAL_SG_FLAG_3 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_3," +
                       "(case UNCAL_SG_FLAG_4 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_4," +
                       "(case UNCAL_SG_FLAG_5 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_5," +
                       "(case UNCAL_SG_FLAG_6 when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_FLAG_6," +
                       // "(case UNCAL_SG_INPUT_BUTTON when '0' then '退出' when '1' then '投入' else '异常' end)as UNCAL_SG_INPUT_BUTTON," +
                       "UNCAL_X_BTP_1,UNCAL_X_BTP_2,UNCAL_X_BTP_3,UNCAL_X_BTP_4," +
                       "UNCAL_X_BTP_5,UNCAL_X_BTP_6,UNCAL_X_BTP_AV,UNCAL_SG_TH_PV_AV_1,UNCAL_SG_TH_PV_AV_2," +
                       "UNCAL_SG_TH_PV_AV_3,UNCAL_SG_TH_PV_AV_4,UNCAL_SG_TH_PV_AV_5,UNCAL_SG_TH_PV_AV_6," +
                       "UNCAL_SG_TH_SP_AV_1,UNCAL_SG_TH_SP_AV_2,UNCAL_SG_TH_SP_AV_3,UNCAL_SG_TH_SP_AV_4," +
                       "UNCAL_SG_TH_SP_AV_5,UNCAL_SG_TH_SP_AV_6,UNCAL_T_N,UNCAL_SG_TH_A_1,UNCAL_SG_TH_A_2," +
                       "UNCAL_SG_TH_A_3,UNCAL_SG_TH_A_4,UNCAL_SG_TH_A_5,UNCAL_SG_TH_A_6,UNCAL_SG_TH_AC_1," +
                       "UNCAL_SG_TH_AC_2,UNCAL_SG_TH_AC_3,UNCAL_SG_TH_AC_4,UNCAL_SG_TH_AC_5,UNCAL_SG_TH_AC_6" +
                       " from MC_UNIFORMCAL_result where TIMESTAMP >= '" + textBox_begin.Text.ToString() + "' and TIMESTAMP <= '" + textBox_end.Text.ToString() + "'  order by TIMESTAMP desc";
                    DataTable _data = dBSQL.GetCommand(sql);
                    if (_data != null)
                    {
                        dataGridView1.DataSource = _data;
                    }
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("show方法错误" + ee.ToString(),-1);
            }

        }
        /// <summary>
        /// 开始时间&结束时间赋值
        /// </summary>
        public void dateTimePicker_value()
        {
            //结束时间
            DateTime time_end = DateTime.Now;
            //开始时间
            DateTime time_begin = time_end.AddDays(-1);

            textBox_begin.Text = time_begin.ToString();
            textBox_end.Text = time_end.ToString();
        }



        //实时按钮
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            dateTimePicker_value();
            show(1);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {

                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "Execl files (*.xls)|*.xls";
                dlg.FilterIndex = 0;
                dlg.RestoreDirectory = true;
                dlg.CreatePrompt = true;
                dlg.Title = "保存为Excel文件";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream;
                    myStream = dlg.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, System.Text.Encoding.GetEncoding(-0));
                    string columnTitle = "";
                    try
                    {
                        //写入列标题    
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            if (i > 0)
                            {
                                columnTitle += "\t";
                            }
                            columnTitle += dataGridView1.Columns[i].HeaderText;
                        }
                        sw.WriteLine(columnTitle);

                        //写入列内容    
                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                        {
                            string columnValue = "";
                            for (int k = 0; k < dataGridView1.Columns.Count; k++)
                            {
                                if (k > 0)
                                {
                                    columnValue += "\t";
                                }
                                if (dataGridView1.Rows[j].Cells[k].Value == null)
                                    columnValue += "";
                                else
                                    columnValue += dataGridView1.Rows[j].Cells[k].Value.ToString().Trim();
                            }
                            sw.WriteLine(columnValue);
                        }
                        sw.Close();
                        myStream.Close();
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show(e.ToString());
                    }
                    finally
                    {
                        sw.Close();
                        myStream.Close();
                    }
                }


                else MessageBox.Show("请选择参数！");
            }
            catch
            { }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            show(2);
        }
    }
}
