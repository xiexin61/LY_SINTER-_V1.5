using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts.Wpf;
using DataBase;
using LY_SINTER.Custom;
using System.IO;
using LY_SINTER.Popover.Course;
using VLog;
using OxyPlot;
using OxyPlot.Axes;

namespace LY_SINTER.PAGE.Course
{
    public partial class Bed_Permeability : UserControl
    {
        public vLog _vLog { get; set; }
        public System.Timers.Timer _Timer1 { get; set; }
        public System.Timers.Timer _Timer2 { get; set; }
        /// <summary>
        /// 1min
        /// </summary>

        #region 料层透气性趋势曲线定义
        private PlotModel _myPlotModel;
        private DateTimeAxis _dateAxis;//X轴
        private LinearAxis _valueAxis1;//Y轴
        List<DataPoint> Line1 = new List<DataPoint>();
        private OxyPlot.Series.LineSeries series1;//曲线
        #endregion

        #region  透气性相关性曲线定义

        /// <summary>
        /// 透气性相关性曲线实时数据 x
        /// </summary>
        List<string> parameter_x = new List<string> { "高返配比", "生石灰配比", "烧返配比", "混合料碱度", "混合料MgO", "混合料固定碳含量", "混合料水分", "布料厚度", "点火温度", "台车速度", "风箱温度", "主抽风量", "风箱负压" };
        /// <summary>
        /// 透气性相关性曲线实时数据 y
        /// </summary>
        List<double> parameter_y = new List<double>();
        /// <summary>
        /// 定义相关性柱形图
        /// </summary>
        ColumnSeries chart_xgx_zxt { get; set; }

        #endregion

        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        /// <summary>
        /// 表单实时显示多少条数据
        /// </summary>
        int rows_table = 10;
        public Bed_Permeability()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Course\\Bed_Permeability_Page\\");
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);


            _Timer1 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { Timer1_Tick_1(); };//响应事件
            _Timer1.Enabled = true;
            _Timer1.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）


            _Timer2 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer2.Elapsed += (x, y) => { Timer2_Tick_1(); };//响应事件
            _Timer2.Enabled = true;
            _Timer2.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）



          this.d1.AddSpanHeader(3, 16, "透气性影响参数");
          // this.d1.AddSpanHeader(19, 13, "相关性系数");
            //相关性曲线控件背景颜色
            chart_xgx.LChart.BackColor = Color.White;

            time_new();
            //  curve_initial();
            // curve_text();
            //2021413
            tendency_curve_HIS(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));
            date_now();
            real_time_xgx();
            curve_initial1();
        }

        private void d1_Scroll(object sender, ScrollEventArgs e)
        {
            d1.ReDrawHead();
        }
        private void Timer2_Tick_1()
        {
            Action invokeAction = new Action(Timer2_Tick_1);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                #region 趋势曲线
                DateTime _end = DateTime.Now;
                DateTime _begin = _end.AddDays(-1);
                tendency_curve_HIS(_begin, _end);
                #endregion
                //实时表单
                date_now();
                time_new();
                timer_1min_xgx();
            }
        }
        private void Timer1_Tick_1()
        {
            Action invokeAction = new Action(Timer1_Tick_1);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                curve_tq_date_timer();
            }
        }

        /// <summary>
        ///  参数查询修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Frm_Bed_Permeability_PAR frm_Tq_Cs = new Frm_Bed_Permeability_PAR();
            if (Frm_Bed_Permeability_PAR.isopen == false)
            {
                frm_Tq_Cs.ShowDialog();
            }
            else
            {
                frm_Tq_Cs.Activate();
            }


        }
        /// <summary>
        ///最新调整时间
        /// </summary>
        private void time_new()
        {
            try
            {
            string sql = "select top 1 TIMESTAMP from M_PICAL_BREATH_RESULT order by TIMESTAMP desc";
            DataTable dataTable = dBSQL.GetCommand(sql);
            if (dataTable.Rows.Count > 0)
            {
                string time_new = dataTable.Rows[0]["TIMESTAMP"].ToString();
                label2.Text = "最新计算时间:" + time_new;
            }
            }
            catch (Exception ee)
            {
                _vLog.writelog("time_new方法失败" + ee.ToString(), -1);
            }


        }
        /// <summary>
        /// 曲线初始化 相关性曲线
        /// </summary>
        public void curve_initial1()
        {
            try
            {
                #region 透气性相关曲线
                //   this.chart_qs_ss.LChart.Zoom = LiveCharts.ZoomingOptions.None;
                chart_xgx_zxt = chart_xgx.MakeCol(0, 0, "chart_xgx_zxt");
                chart_xgx.LChart.Series.Add(chart_xgx_zxt);
                chart_xgx.LPageSize = 13;
                //    chart_xgx.LBindDataC<string, double>("chart_xgx_zxt", parameter_x, parameter_y,System.Windows.Media.Brushes.Green,"","",1,-1,1,2);
                chart_xgx.LBindDataC<string, double>("chart_xgx_zxt", parameter_x, parameter_y, System.Windows.Media.Brushes.Green);
                this.chart_xgx.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示
                #endregion
            }
            catch
            {

            }
           
        }
        /// <summary>
        /// 历史查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            _Timer2.Enabled = false;
            //  curve_history();
            tendency_curve_HIS(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));
            data_history();
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click_1(object sender, EventArgs e)
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
                    for (int i = 0; i < d1.ColumnCount; i++)
                    {
                        if (i > 0)
                        {
                            columnTitle += "\t";
                        }
                        columnTitle += d1.Columns[i].HeaderText;
                    }
                    sw.WriteLine(columnTitle);

                    //写入列内容    
                    for (int j = 0; j < d1.Rows.Count; j++)
                    {
                        string columnValue = "";
                        for (int k = 0; k < d1.Columns.Count; k++)
                        {
                            if (k > 0)
                            {
                                columnValue += "\t";
                            }
                            if (d1.Rows[j].Cells[k].Value == null)
                                columnValue += "";
                            else
                                columnValue += d1.Rows[j].Cells[k].Value.ToString().Trim();
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
        }

        /// <summary>
        /// 实时按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {

            //更新料层透气性趋势&表单数据
            //  curve_tq_now();
            DateTime _end = DateTime.Now;
            DateTime _begin = _end.AddDays(-1);
            tendency_curve_HIS(_begin, _end);
            //实时表单
            date_now();
            time_new();
            timer_1min_xgx();
            _Timer2.Enabled = true;

        }
        /// <summary>
        /// 实时表单数据
        /// </summary>
        public void date_now()
        {
            try
            {

          
            //string sql_1 = "select top " + rows_table + " ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID," +
            //"TIMESTAMP,PICAL_JPU,PICAL_BREATH_BFO_BILL,PICAL_BREATH_LIME_BILL," +
            //"PICAL_BREATH_SRM_BILL,PICAL_BREATH_R,PICAL_BREATH_MGO,PICAL_BREATH_C," +
            //"PICAL_BREATH_1M_FT_PV,PICAL_BREATH_1M_NEX_WAT,PICAL_BREATH_2M_FT_PV," +
            //"PICAL_BREATH_2M_NEX_WAT,PICAL_BREATH_H,PICAL_BREATH_IG_01_TE," +
            //"PICAL_BREATH_SPARE13,PICAL_BREATH_Q,PICAL_BREATH_SPARE14,PICAL_BREATH_P," +
            //"PICAL_RELAT_BR_BL_BF,PICAL_RELAT_BR_BL_FLUX,PICAL_RELAT_BR_BL_SIN," +
            //"PICAL_RELAT_BR_R,PICAL_RELAT_BR_BL_MGO,PICAL_RELAT_BR_COKE_GRI," +
            //"PICAL_RELAT_BR_WAT,PICAL_RELAT_BR_THICK,PICAL_RELAT_BR_BL_IG_TE," +
            //"PICAL_BREATH_SPARE11,PICAL_RELAT_BR_MA_TE,PICAL_RELAT_BR_BL_BLEND," +
            //"PICAL_RELAT_BR_FLUE_PT " +
            //"from M_PICAL_BREATH_RESULT  order by TIMESTAMP desc";
                string sql_1 = "select top " + rows_table + " ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID," +
    "TIMESTAMP,PICAL_JPU,PICAL_BREATH_BFO_BILL,PICAL_BREATH_LIME_BILL," +
    "PICAL_BREATH_SRM_BILL,PICAL_BREATH_R,PICAL_BREATH_MGO,PICAL_BREATH_C," +
    "PICAL_BREATH_1M_FT_PV,PICAL_BREATH_1M_NEX_WAT,PICAL_BREATH_2M_FT_PV," +
    "PICAL_BREATH_2M_NEX_WAT,PICAL_BREATH_H,PICAL_BREATH_IG_01_TE," +
    "PICAL_BREATH_SPARE13,PICAL_BREATH_Q,PICAL_BREATH_SPARE14,PICAL_BREATH_P " +
    "from M_PICAL_BREATH_RESULT  order by TIMESTAMP desc";
                DataTable dataTable = dBSQL.GetCommand(sql_1);
            if (dataTable.Rows.Count > 0)
            {
                this.d1.DataSource = dataTable;
            }
            }
            catch (Exception ee)
            {
                _vLog.writelog("date_now方法失败" + ee.ToString(), -1);
            }

        }
        /// <summary>
        /// 历史表单数据
        /// </summary>
        public void data_history()
        {
            try
            {

            
            string time_begin = textBox_begin.Text.ToString();
            string time_end = textBox_end.Text.ToString();
            //string sql_1 = "select  ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID," +
            //    "TIMESTAMP,PICAL_JPU,PICAL_BREATH_BFO_BILL,PICAL_BREATH_LIME_BILL," +
            //    "PICAL_BREATH_SRM_BILL,PICAL_BREATH_R,PICAL_BREATH_MGO,PICAL_BREATH_C," +
            //    "PICAL_BREATH_1M_FT_PV,PICAL_BREATH_1M_NEX_WAT,PICAL_BREATH_2M_FT_PV," +
            //    "PICAL_BREATH_2M_NEX_WAT,PICAL_BREATH_H,PICAL_BREATH_IG_01_TE," +
            //    "PICAL_BREATH_SPARE13,PICAL_BREATH_Q,PICAL_BREATH_SPARE14,PICAL_BREATH_P," +
            //    "PICAL_RELAT_BR_BL_BF,PICAL_RELAT_BR_BL_FLUX,PICAL_RELAT_BR_BL_SIN," +
            //    "PICAL_RELAT_BR_R,PICAL_RELAT_BR_BL_MGO,PICAL_RELAT_BR_COKE_GRI," +
            //    "PICAL_RELAT_BR_WAT,PICAL_RELAT_BR_THICK,PICAL_RELAT_BR_BL_IG_TE," +
            //    "PICAL_BREATH_SPARE11,PICAL_RELAT_BR_MA_TE,PICAL_RELAT_BR_BL_BLEND," +
            //    "PICAL_RELAT_BR_FLUE_PT " +
            //    "from M_PICAL_BREATH_RESULT where TIMESTAMP >= '" + time_begin + "' and TIMESTAMP <= '" + time_end + "'  order by TIMESTAMP desc";


                string sql_1 = "select  ROW_NUMBER() OVER (ORDER BY TIMESTAMP desc) as ID," +
             "TIMESTAMP,PICAL_JPU,PICAL_BREATH_BFO_BILL,PICAL_BREATH_LIME_BILL," +
             "PICAL_BREATH_SRM_BILL,PICAL_BREATH_R,PICAL_BREATH_MGO,PICAL_BREATH_C," +
             "PICAL_BREATH_1M_FT_PV,PICAL_BREATH_1M_NEX_WAT,PICAL_BREATH_2M_FT_PV," +
             "PICAL_BREATH_2M_NEX_WAT,PICAL_BREATH_H,PICAL_BREATH_IG_01_TE," +
             "PICAL_BREATH_SPARE13,PICAL_BREATH_Q,PICAL_BREATH_SPARE14,PICAL_BREATH_P   " +
          
             "from M_PICAL_BREATH_RESULT where TIMESTAMP >= '" + time_begin + "' and TIMESTAMP <= '" + time_end + "'  order by TIMESTAMP desc";
                DataTable dataTable = dBSQL.GetCommand(sql_1);
            if (dataTable.Rows.Count < 1)
            {

            }
            else
            {
                this.d1.DataSource = dataTable;
            }
            }
            catch (Exception ee)
            {
                _vLog.writelog("data_history方法失败" + ee.ToString(), -1);
            }


        }
        /// <summary>
        /// 实时柱形图相关性数据
        /// </summary>
        public void real_time_xgx()
        {
            try
            {

           
            parameter_y.Clear();
            string sql_M_PICAL_BREATH_RESULT_xgx = "select top (1) " +
             "isnull(PICAL_RELAT_BR_BL_BF,0)," +
              "isnull(PICAL_RELAT_BR_BL_FLUX,0)," +
              "isnull(PICAL_RELAT_BR_BL_SIN,0)," +
              "isnull(PICAL_RELAT_BR_R,0)," +
              "isnull(PICAL_RELAT_BR_BL_MGO,0)," +
              "isnull(PICAL_RELAT_BR_COKE_GRI,0)," +
              "isnull(PICAL_RELAT_BR_WAT,0)," +
              "isnull(PICAL_RELAT_BR_THICK,0)," +
              "isnull(PICAL_RELAT_BR_BL_IG_TE,0)," +
              "isnull(PICAL_BREATH_SPARE11,0)," +
              "isnull(PICAL_RELAT_BR_MA_TE,0)," +
              "isnull(PICAL_RELAT_BR_BL_BLEND,0)," +
              "isnull(PICAL_RELAT_BR_FLUE_PT,0) " +
              "from M_PICAL_BREATH_RESULT order by TIMESTAMP desc";
            DataTable dataTable_M_PICAL_BREATH_RESULT_xgx = dBSQL.GetCommand(sql_M_PICAL_BREATH_RESULT_xgx);
            if (dataTable_M_PICAL_BREATH_RESULT_xgx.Rows.Count > 0)
            {
                for (int col = 0; col < dataTable_M_PICAL_BREATH_RESULT_xgx.Columns.Count; col++)
                {
                    double text = double.Parse(dataTable_M_PICAL_BREATH_RESULT_xgx.Rows[0][col].ToString());
                    parameter_y.Add(text);
                }
            }
            else
            {
                for (int col = 0; col < dataTable_M_PICAL_BREATH_RESULT_xgx.Columns.Count; col++)
                {
                    //double text = double.Parse(dataTable_M_PICAL_BREATH_RESULT_xgx.Rows[0][col].ToString());
                    parameter_y.Add(double.NaN);
                }
            }
            }
            catch (Exception ee)
            {
                _vLog.writelog("real_time_xgx方法失败" + ee.ToString(), -1);
            }
        }

       
        /// <summary>
        /// 离线计算按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {

            Frm_Bed_Permeability_Adjust frm_Tq_Cs = new Frm_Bed_Permeability_Adjust();
            if (Frm_Bed_Permeability_Adjust.isopen == false)
            {
                frm_Tq_Cs.ShowDialog();
            }
            else
            {
                frm_Tq_Cs.Activate();
            }
        }
        /// <summary>
        /// 料层透气性曲线&表单数据实时刷新
        /// </summary>
        private void curve_tq_date_timer()
        {
                //实时表单
                date_now();

        }

        /// <summary>
        /// 实时相关性曲线数据刷新 1min
        /// </summary>
        public void timer_1min_xgx()
        {
            try
            {

            //  real_time_xgx();
            string sql_M_PICAL_BREATH_RESULT_xgx = "select top (1) " +
           "isnull(PICAL_RELAT_BR_BL_BF,0)," +
            "isnull(PICAL_RELAT_BR_BL_FLUX,0)," +
            "isnull(PICAL_RELAT_BR_BL_SIN,0)," +
            "isnull(PICAL_RELAT_BR_R,0)," +
            "isnull(PICAL_RELAT_BR_BL_MGO,0)," +
            "isnull(PICAL_RELAT_BR_COKE_GRI,0)," +
            "isnull(PICAL_RELAT_BR_WAT,0)," +
            "isnull(PICAL_RELAT_BR_THICK,0)," +
            "isnull(PICAL_RELAT_BR_BL_IG_TE,0)," +
            "isnull(PICAL_BREATH_SPARE11,0)," +
            "isnull(PICAL_RELAT_BR_MA_TE,0)," +
            "isnull(PICAL_RELAT_BR_BL_BLEND,0)," +
            "isnull(PICAL_RELAT_BR_FLUE_PT,0) " +
            "from M_PICAL_BREATH_RESULT order by TIMESTAMP desc";
            DataTable dataTable_M_PICAL_BREATH_RESULT_xgx = dBSQL.GetCommand(sql_M_PICAL_BREATH_RESULT_xgx);
            if (dataTable_M_PICAL_BREATH_RESULT_xgx.Rows.Count > 0)
            {
                //新添加的数据和之前的数据进行对比，判断是否需要重新更新柱形图
                bool flag = false;
                for (int colum = 0; colum < dataTable_M_PICAL_BREATH_RESULT_xgx.Columns.Count; colum++)
                {
                    if (double.Parse(dataTable_M_PICAL_BREATH_RESULT_xgx.Rows[0][colum].ToString()) != parameter_y[colum])
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    parameter_y.Clear();
                    for (int col = 0; col < dataTable_M_PICAL_BREATH_RESULT_xgx.Columns.Count; col++)
                    {
                        double text = double.Parse(dataTable_M_PICAL_BREATH_RESULT_xgx.Rows[0][col].ToString());
                        parameter_y.Add(text);
                    }
                    // chart_xgx.LBindDataC<string, double>("chart_xgx_zxt", parameter_x, parameter_y, System.Windows.Media.Brushes.Green, "", "", 1, 2);
                    chart_xgx.LBindDataC<string, double>("chart_xgx_zxt", parameter_x, parameter_y, System.Windows.Media.Brushes.Green);
                    this.chart_xgx.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示
                }

            }
            else
            {
                parameter_y.Clear();
                for (int col = 0; col < dataTable_M_PICAL_BREATH_RESULT_xgx.Columns.Count; col++)
                {
                    //double text = double.Parse(dataTable_M_PICAL_BREATH_RESULT_xgx.Rows[0][col].ToString());
                    parameter_y.Add(double.NaN);
                }
                //    chart_xgx.LBindDataC<string, double>("chart_xgx_zxt", parameter_x, parameter_y, System.Windows.Media.Brushes.Green, "", "", 1, 2);
                chart_xgx.LBindDataC<string, double>("chart_xgx_zxt", parameter_x, parameter_y, System.Windows.Media.Brushes.Green);

                this.chart_xgx.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示
            }

            }
            catch (Exception ee)
            {
                _vLog.writelog("timer_1min_xgx方法失败" + ee.ToString(), -1);
            }


        }

        public void Timer_stop()
        {
            _Timer1.Enabled = false;
            _Timer2.Enabled = false;
           
        }
        public void Timer_state()
        {
           
            _Timer1.Enabled = true;
            _Timer2.Enabled = true;
        }
        public void _Clear()
        {
            _Timer1.Close();
            _Timer2.Close();
            this.Dispose();
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 开始时间&结束时间赋值历史
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

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
        /// <summary>
        /// 趋势曲线历史
        /// </summary>
        public void tendency_curve_HIS(DateTime _d1, DateTime _d2)
        {
            Line1.Clear();

            try
            {
                string sql_M_PICAL_BREATH_RESULT_HIS = "select TIMESTAMP,PICAL_JPU from M_PICAL_BREATH_RESULT where TIMESTAMP >='" + _d1 + "' and TIMESTAMP <= '" + _d2 + "' order by TIMESTAMP asc";
                DataTable data_curve_ls = dBSQL.GetCommand(sql_M_PICAL_BREATH_RESULT_HIS);
                if (data_curve_ls.Rows.Count > 0)
                {
                    
                    List<double> Mun1 = new List<double>();
                    //定义model
                    _myPlotModel = new PlotModel()
                    {
                        Background = OxyColors.White,
                        //LegendMargin = 100,
                    };
                    //X轴
                    _dateAxis = new DateTimeAxis()
                    {
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dot,
                        IntervalLength = 100,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        AxisTickToLabelDistance = 0,
                        FontSize = 9.0,
                        StringFormat = "yyyy/MM/dd HH:mm",
                    };
                    _myPlotModel.Axes.Add(_dateAxis);
                    for (int i = 0; i < data_curve_ls.Rows.Count; i++)
                    {
                        DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i][1]));
                        Line1.Add(line1);
                        Mun1.Add(Convert.ToDouble(data_curve_ls.Rows[i][1]));
                    }

                    int x = 1;
                    if ((int)((Mun1.Max() - Mun1.Min()) / 5) > 0)
                    {
                        x = (int)((Mun1.Max() - Mun1.Min()) / 5);
                    }
                    _valueAxis1 = new LinearAxis()
                    {
                        Key = "A",
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        //Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun1.Max() + 5),
                        Minimum = (int)(Mun1.Min() - 1),
                        PositionTier = 1,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Red,
                        MinorTicklineColor = OxyColors.Red,
                        TicklineColor = OxyColors.Red,
                        TextColor = OxyColors.Red,
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x,
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis1);
                    //添加曲线
                    series1 = new OxyPlot.Series.LineSeries()
                    {
                        Color = OxyColors.Red,
                        StrokeThickness = 1,
                        MarkerSize = 1,
                        MarkerStroke = OxyColors.Red,
                        MarkerType = MarkerType.Circle,
                        //MarkerStrokeThickness=1,
                        YAxisKey = "A",
                        ItemsSource = Line1,
                        //TrackerFormatString = "{0}\n时间:{2:yyyy-MM-dd HH:mm}透气指数:{4}",
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}透气指数:{4}",
                    };
                        _valueAxis1.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series1);
            
                    curve_his.Model = _myPlotModel;
                    var PlotController = new OxyPlot.PlotController();
                    PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
                    curve_his.Controller = PlotController;
                }

            }
            catch (Exception ee)
            {
                string mistake = "tendency_curve_HIS方法错误" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
    }
}
