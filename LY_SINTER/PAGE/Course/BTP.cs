using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LY_SINTER.Model;
using OxyPlot.Axes;
using OxyPlot;
using DataBase;
using VLog;
using LY_SINTER.Custom;
using Newtonsoft.Json;
using LiveCharts.Wpf;
using LY_SINTER.Popover.Course;

namespace LY_SINTER.PAGE.Course
{
    public partial class BTP : UserControl
    {
        Course_MODEL _MODEL = new Course_MODEL();
        public vLog _vLog { get; set; }
        DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
        Course_MODEL course_MODEL = new Course_MODEL();
        #region 历史曲线
        private PlotModel _myPlotModel;
        private DateTimeAxis _dateAxis;//X轴
        private LinearAxis _valueAxis1;//Y轴
        private LinearAxis _valueAxis2;//Y轴
        private LinearAxis _valueAxis3;//Y轴
        private LinearAxis _valueAxis4;//Y轴
        private LinearAxis _valueAxis5;//Y轴
        private LinearAxis _valueAxis6;//Y轴

        List<DataPoint> Line1 = new List<DataPoint>();
        List<DataPoint> Line2 = new List<DataPoint>();
        List<DataPoint> Line3 = new List<DataPoint>();
        List<DataPoint> Line4 = new List<DataPoint>();
        List<DataPoint> Line5 = new List<DataPoint>();
        List<DataPoint> Line6 = new List<DataPoint>();

        private OxyPlot.Series.LineSeries series1;//曲线
        private OxyPlot.Series.LineSeries series2;//曲线
        private OxyPlot.Series.LineSeries series3;//曲线
        private OxyPlot.Series.LineSeries series4;//曲线
        private OxyPlot.Series.LineSeries series5;//曲线
        private OxyPlot.Series.LineSeries series6;//曲线
        string[] curve_name = { "A_1", "A_2", "A_3", "A_4", "A_5", "A_6" };//曲线标志位
        #endregion
        #region 定时器声明
        /// <summary>
        /// 1min定时器
        /// </summary>
        public System.Timers.Timer _Timer1 { get; set; }
        /// <summary>
        /// 3D曲线定时器
        /// </summary>
        public System.Timers.Timer _Timer2 { get; set; }
        /// <summary>
        /// 3D曲线初始化
        /// </summary>
        public System.Timers.Timer _Timer3 { get; set; }
        #endregion
        #region 高次曲线
        /// <summary>
        /// 高次x坐标
        /// </summary>
        List<string> list_x = new List<string>();
        /// <summary>
        /// 高次曲线y轴数据（点线图数据）
        /// </summary>
        List<double> list_Y_GCQX = new List<double>();
        /// <summary>
        /// 高次曲线y轴数据（柱状图数据）
        /// </summary>
        List<double> list_y_ZZT = new List<double>();
        /// <summary>
        /// 高次曲线柱形图
        /// </summary>
        ColumnSeries bar_chart { get; set; }
        /// <summary>
        /// 拟合次数
        /// </summary>
        int degree = 0;
        #endregion
        #region 实时趋势曲线声明
        private PlotModel _PlotModel;
        private DateTimeAxis X_Axis;//X轴
        private LinearAxis Y_Axis1;//Y轴
        private LinearAxis Y_Axis2;//Y轴
        private LinearAxis Y_Axis3;//Y轴
        private LinearAxis Y_Axis4;//Y轴
        private LinearAxis Y_Axis5;//Y轴

        List<DataPoint> Line_1 = new List<DataPoint>();//数据源
        List<DataPoint> Line_2 = new List<DataPoint>();//数据源
        List<DataPoint> Line_3 = new List<DataPoint>();//数据源
        List<DataPoint> Line_4 = new List<DataPoint>();//数据源
        List<DataPoint> Line_5 = new List<DataPoint>();//数据源


        private OxyPlot.Series.LineSeries series_1;//曲线
        private OxyPlot.Series.LineSeries series_2;//曲线
        private OxyPlot.Series.LineSeries series_3;//曲线
        private OxyPlot.Series.LineSeries series_4;//曲线
        private OxyPlot.Series.LineSeries series_5;//曲线
        private OxyPlot.Series.LineSeries series_6;//曲线
        string[] curve_name_1 = { "A_1", "A_2", "A_3", "A_4", "A_5"};//曲线标志位

        #endregion
        /// <summary>
        /// 小数位数
        /// </summary>
        int DIS_1 = 2;


        public BTP()
        {
            InitializeComponent();
            if (_vLog == null)
                _vLog = new vLog(".\\log_Page\\Course\\BTP_PAGE\\");
            dateTimePicker_value();//开始时间&结束时间赋值
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            Preparations_Date();//准备数据
            curve_initial();//曲线声明
            TIMER_Statement();//声明定时器
            chart_GCFC.LChart.BackColor = Color.White;
            lUserControl1.FunOne = LGetBindData;//3D绑定数据源
            lUserControl1.LInvokeScript(0);
            Check_text_His();//历史曲线勾选框数据
            Check_text_Real();//趋势曲线勾选框
            Real_time();//生产实时数据
            tendency_curve_HIS(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));
            Higher_Order_Curve();//高次曲线数据绑定
            tendency_curve_Real();//趋势实时曲线

        }
        /// <summary>
        /// 趋势曲线勾选框
        /// </summary>
        public void Check_text_Real()
        {
            #region 趋势曲线
            try
            {
                //BTP实际、BTP预测、BRP实际、BRP预测、TRP实际
                string sql_MC_BTPCAL_PREDICT = "select isnull(BTP_NOW,0) as BTP_NOW ,isnull(BTP_PREDICT,0) as BTP_PREDICT,isnull(BRP_NOW,0) as BRP_NOW,isnull(BRP_PREDICT,0) as BRP_PREDICT,isnull(TRP_NOW,0) as TRP_NOW from MC_BTPCAL_PREDICT where TIMESTAMP = (select max(TIMESTAMP) from MC_BTPCAL_PREDICT)";
                DataTable data_MC_BTPCAL_PREDICT = dBSQL.GetCommand(sql_MC_BTPCAL_PREDICT);
                if (data_MC_BTPCAL_PREDICT.Rows.Count > 0)
                {
                    this.checkEdit1.Text = "BTP实际(m):" + data_MC_BTPCAL_PREDICT.Rows[0]["BTP_NOW"].ToString();
                    this.checkEdit2.Text = "BTP预测(m):" + data_MC_BTPCAL_PREDICT.Rows[0]["BTP_PREDICT"].ToString();
                    this.checkEdit3.Text = "BRP实际(m):" + data_MC_BTPCAL_PREDICT.Rows[0]["BRP_NOW"].ToString();
                    this.checkEdit4.Text = "BRP预测(m):" + data_MC_BTPCAL_PREDICT.Rows[0]["BRP_PREDICT"].ToString();
                    this.checkEdit5.Text = "TRP实际(m):" + data_MC_BTPCAL_PREDICT.Rows[0]["TRP_NOW"].ToString();
                }
                else
                {
                    this.checkEdit1.Text = "BTP实际(m):";
                    this.checkEdit2.Text = "BTP预测(m):";
                    this.checkEdit3.Text = "BRP实际(m):";
                    this.checkEdit4.Text = "BRP预测(m):";
                    this.checkEdit5.Text = "TRP实际(m):";
                }
            }
            catch (Exception ee)
            {
                string mistake = "趋势曲线勾选框数据报错" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
            #endregion
        }
        /// <summary>
        /// 定时器声明
        /// </summary>
        public void TIMER_Statement()
        {
            _Timer1 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { _Timer1_Tick(); };
            _Timer1.Enabled = true;
            _Timer1.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer2 = new System.Timers.Timer(60000);//初始化颜色变化定时器响应事件
            _Timer2.Elapsed += (x, y) => { _Timer2_Tick(); };
            _Timer2.Enabled = false;
            _Timer2.AutoReset = true;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

            _Timer3 = new System.Timers.Timer(3000);//初始化颜色变化定时器响应事件
            _Timer3.Elapsed += (x, y) => { _Timer3_Tick(); };
            _Timer3.Enabled = true;
            _Timer3.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）

        }
        /// <summary>
        /// 准备数据
        /// </summary>
        public void Preparations_Date()
        {
            try
            {
                for (int x = 1; x < 23; x++)//添加高次曲线x轴数据点
                {
                    list_x.Add(x.ToString() + "#");
                }
                string sql_1 = "select  BTPCAL_POWER from MC_BTPCAL_LINE_DISTANCE where TIMESTAMP =(select max(TIMESTAMP) from MC_BTPCAL_LINE_DISTANCE) ";
                DataTable dataTable_1 = dBSQL.GetCommand(sql_1);
                degree = int.Parse(dataTable_1.Rows[0]["BTPCAL_POWER"].ToString());
            }
            catch(Exception ee)
            {
                _vLog.writelog("Preparations_Date方法出错" + ee.ToString(),-1);
            }
        }
        /// <summary>
        /// 曲线定义
        /// </summary>
        public void curve_initial()
        {
            try
            {
                #region 高次曲线
                this.chart_GCFC.LChart.Zoom = LiveCharts.ZoomingOptions.X;
                bar_chart = chart_GCFC.MakeCol(0, 0, "curve_bar");
                chart_GCFC.LChart.Series.Add(bar_chart);
                chart_GCFC.LPageSize = 23;//20200521
                var GCQX = chart_GCFC.MakeLine(0, 0, "GCQX");
                chart_GCFC.LChart.Series.Add(GCQX);
                #endregion
            }
            catch (Exception ee)
            {
                string mistake = "曲线声明报错" + ee.ToString();
            }
            

            

        }
        /// <summary>
        /// 高次曲线赋值
        /// </summary>
        public void Higher_Order_Curve()
        {
            try
            {
                list_Y_GCQX.Clear();
                list_y_ZZT.Clear();
                list_Y_GCQX = _MODEL.Higher_Order_Date_2(degree, 2);//高次曲线
                list_y_ZZT = _MODEL.Higher_Order_Date_1();//柱形图
                if (list_Y_GCQX != null && list_y_ZZT != null)
                {
                    chart_GCFC.LBindData<string, double>("GCQX", list_x, list_Y_GCQX, System.Windows.Media.Brushes.Red, " ", " ");
                    chart_GCFC.LBindDataC<string, double>("curve_bar", list_x, list_y_ZZT, System.Windows.Media.Brushes.CornflowerBlue, " ", " ");
                    this.chart_GCFC.LChart.AxisX[0].Separator = new Separator() { Step = 1 };//x轴数据全部展示
                }
                else
                {
                    _vLog.writelog("Course_MODEL.Higher_Order_Date_2模型调用错误", -1);
                    return;
                }
            }
            catch(Exception ee)
            {
                _vLog.writelog("Higher_Order_Curve方法调用错误  "  + ee.ToString(),-1);
            }
        }
        #region 定时器响应事件
        private void _Timer1_Tick()
        {
            Action invokeAction = new Action(_Timer1_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                Check_text_His();//历史曲线勾选框数据
                Check_text_Real();//趋势曲线勾选框
                Real_time();//生产实时数据
                tendency_curve_Real();//实时趋势曲线
                Higher_Order_Curve();//高次曲线
            }
        }
        private void _Timer2_Tick()//刷新3d控件
        {
            Action invokeAction = new Action(_Timer2_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                lUserControl1.LInvokeScript(1);
            }
        }
        private void _Timer3_Tick()//首次刷新3d控件
        {
            Action invokeAction = new Action(_Timer3_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                _Timer2.Enabled = true;///唤醒周期刷新3D控件
                lUserControl1.LInvokeScript(1);
            }
        }
        #endregion
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
        /// <summary>
        /// 3D曲线
        /// </summary>
        /// <returns></returns>
        public string LGetBindData()
        {
            try
            {
                //数据准备******
                //开始闸门边界
                double StartZloc;
                //结束闸门边界
                double EndZloc;
                //风箱开始位置
                double StartFXlocation;
                //风箱结束位置
                double EndFXlocation;
                //6个闸门位置
                double[] XLocation = new double[6];
                //温度轴
                double[] YLocation = new double[] { 0, 100, 200, 300, 400, 500, 600 };
                //风箱热电偶坐标
                double[] ZLocation = new double[22];
                //风箱热电偶坐标值差
                 double[] RDOSize = new double[21];
                //风箱热电偶位置方向扩点（M）
              //  double Step = 0.25;
                double Step = 0.1;
                //闸门方向扩点（）
                int D_value = 2;
                //闸门个数
                int initCount = 6;
                //********

                //三维图的x轴 热电偶距离位置
                // string sql_x = "select top 1 BTPCAL_BTC01M0101,BTPCAL_BTC01M0201,BTPCAL_BTC01M0301,BTPCAL_BTC01M0401,BTPCAL_BTC01M0501,BTPCAL_BTC01M0601,BTPCAL_BTC01M0701,BTPCAL_BTC01M0801,BTPCAL_BTC01M0901,BTPCAL_BTC01M1001,BTPCAL_BTC01M1101,BTPCAL_BTC01M1201,BTPCAL_BTC01M1301,BTPCAL_BTC01M1401,BTPCAL_BTC01M1501,BTPCAL_BTC01M1601,BTPCAL_BTC01M1701,BTPCAL_BTC01M1801,BTPCAL_BTC01M1901,BTPCAL_BTC01M2001,BTPCAL_BTC01M2101,BTPCAL_BTC01M2201,BTPCAL_BTC01M2301 from MC_BTPCAL_LINE_DISTANCE order by TIMESTAMP desc";
                string sql_x = "select top 1 BTPCAL_BTC01M0101,BTPCAL_BTC01M0201,BTPCAL_BTC01M0301,BTPCAL_BTC01M0401,BTPCAL_BTC01M0501,BTPCAL_BTC01M0601,BTPCAL_BTC01M0701,BTPCAL_BTC01M0801,BTPCAL_BTC01M0901,BTPCAL_BTC01M1001,BTPCAL_BTC01M1101,BTPCAL_BTC01M1201,BTPCAL_BTC01M1301,BTPCAL_BTC01M1401,BTPCAL_BTC01M1501,BTPCAL_BTC01M1601,BTPCAL_BTC01M1701,BTPCAL_BTC01M1801,BTPCAL_BTC01M1901,BTPCAL_BTC01M2001,BTPCAL_BTC01M2101,BTPCAL_BTC01M2201 from MC_BTPCAL_LINE_DISTANCE order by TIMESTAMP desc";
                DataTable dataTable_x = dBSQL.GetCommand(sql_x);
                ///三维图的1-6闸门距离
                string sql_y = "select top 1 PAR_INTERFACE_Y_1,PAR_INTERFACE_Y_2,PAR_INTERFACE_Y_3,PAR_INTERFACE_Y_4,PAR_INTERFACE_Y_5,PAR_INTERFACE_Y_6 from MC_BTPCAL_PAR";
                DataTable dataTable_y = dBSQL.GetCommand(sql_y);

                ///闸门边界位置
                string sql_boundary = "select PAR_INTERFACE_Y_0,PAR_INTERFACE_Y_7 from MC_BTPCAL_PAR ";
                DataTable dataTable_boundary = dBSQL.GetCommand(sql_boundary);

                //1-23#基础数据
                string sql_1 = "select top 1 SIN_CAL_B01_TE_L_1,SIN_CAL_B02_TE_L_1,SIN_CAL_B03_TE_L_1,SIN_CAL_B04_TE_L_1,SIN_CAL_B05_TE_L_1 ,SIN_CAL_B06_TE_L_1 ,SIN_CAL_B07_TE_L_1,SIN_CAL_B08_TE_L_1 ,SIN_CAL_B09_TE_L_1,SIN_CAL_B10_TE_L_1,SIN_CAL_B11_TE_L_1,SIN_CAL_B12_TE_L_1,SIN_CAL_B13_TE_L_1,SIN_CAL_B14_TE_L_1,SIN_CAL_B15_TE_L_1,SIN_CAL_B16_TE_L_1,SIN_CAL_B17_TE_L_1,SIN_CAL_B18_TE_L_1,SIN_CAL_B19_TE_L_1,SIN_CAL_B20_TE_L_1,SIN_CAL_B21_TE_L_1,SIN_CAL_B22_TE_L_1 from MC_BTPCAL_LINE_CAL_TEMP where TIMESTAMP=(select max(TIMESTAMP) from MC_BTPCAL_LINE_CAL_TEMP) " +
                    "union all select top 1 SIN_CAL_B01_TE_L_2,SIN_CAL_B02_TE_L_2,SIN_CAL_B03_TE_L_2,SIN_CAL_B04_TE_L_2,SIN_CAL_B05_TE_L_2 ,SIN_CAL_B06_TE_L_2 ,SIN_CAL_B07_TE_L_2,SIN_CAL_B08_TE_L_2 ,SIN_CAL_B09_TE_L_2,SIN_CAL_B10_TE_L_2,SIN_CAL_B11_TE_L_2,SIN_CAL_B12_TE_L_2,SIN_CAL_B13_TE_L_2,SIN_CAL_B14_TE_L_2,SIN_CAL_B15_TE_L_2,SIN_CAL_B16_TE_L_2,SIN_CAL_B17_TE_L_2,SIN_CAL_B18_TE_L_2,SIN_CAL_B19_TE_L_2,SIN_CAL_B20_TE_L_2,SIN_CAL_B21_TE_L_2,SIN_CAL_B22_TE_L_2 from MC_BTPCAL_LINE_CAL_TEMP where TIMESTAMP=(select max(TIMESTAMP) from MC_BTPCAL_LINE_CAL_TEMP) " +
                    "union all select top 1 SIN_CAL_B01_TE_L_3,SIN_CAL_B02_TE_L_3,SIN_CAL_B03_TE_L_3,SIN_CAL_B04_TE_L_3,SIN_CAL_B05_TE_L_3 ,SIN_CAL_B06_TE_L_3 ,SIN_CAL_B07_TE_L_3,SIN_CAL_B08_TE_L_3 ,SIN_CAL_B09_TE_L_3,SIN_CAL_B10_TE_L_3,SIN_CAL_B11_TE_L_3,SIN_CAL_B12_TE_L_3,SIN_CAL_B13_TE_L_3,SIN_CAL_B14_TE_L_3,SIN_CAL_B15_TE_L_3,SIN_CAL_B16_TE_L_3,SIN_CAL_B17_TE_L_3,SIN_CAL_B18_TE_L_3,SIN_CAL_B19_TE_L_3,SIN_CAL_B20_TE_L_3,SIN_CAL_B21_TE_L_3,SIN_CAL_B22_TE_L_3 from MC_BTPCAL_LINE_CAL_TEMP where TIMESTAMP=(select max(TIMESTAMP) from MC_BTPCAL_LINE_CAL_TEMP) " +
                    "union all select top 1 SIN_CAL_B01_TE_L_4,SIN_CAL_B02_TE_L_4,SIN_CAL_B03_TE_L_4,SIN_CAL_B04_TE_L_4,SIN_CAL_B05_TE_L_4 ,SIN_CAL_B06_TE_L_4 ,SIN_CAL_B07_TE_L_4,SIN_CAL_B08_TE_L_4 ,SIN_CAL_B09_TE_L_4,SIN_CAL_B10_TE_L_4,SIN_CAL_B11_TE_L_4,SIN_CAL_B12_TE_L_4,SIN_CAL_B13_TE_L_4,SIN_CAL_B14_TE_L_4,SIN_CAL_B15_TE_L_4,SIN_CAL_B16_TE_L_4,SIN_CAL_B17_TE_L_4,SIN_CAL_B18_TE_L_4,SIN_CAL_B19_TE_L_4,SIN_CAL_B20_TE_L_4,SIN_CAL_B21_TE_L_4,SIN_CAL_B22_TE_L_4 from MC_BTPCAL_LINE_CAL_TEMP where TIMESTAMP=(select max(TIMESTAMP) from MC_BTPCAL_LINE_CAL_TEMP) " +
                    "union all select top 1 SIN_CAL_B01_TE_L_5,SIN_CAL_B02_TE_L_5,SIN_CAL_B03_TE_L_5,SIN_CAL_B04_TE_L_5,SIN_CAL_B05_TE_L_5 ,SIN_CAL_B06_TE_L_5 ,SIN_CAL_B07_TE_L_5,SIN_CAL_B08_TE_L_5 ,SIN_CAL_B09_TE_L_5,SIN_CAL_B10_TE_L_5,SIN_CAL_B11_TE_L_5,SIN_CAL_B12_TE_L_5,SIN_CAL_B13_TE_L_5,SIN_CAL_B14_TE_L_5,SIN_CAL_B15_TE_L_5,SIN_CAL_B16_TE_L_5,SIN_CAL_B17_TE_L_5,SIN_CAL_B18_TE_L_5,SIN_CAL_B19_TE_L_5,SIN_CAL_B20_TE_L_5,SIN_CAL_B21_TE_L_5,SIN_CAL_B22_TE_L_5 from MC_BTPCAL_LINE_CAL_TEMP where TIMESTAMP=(select max(TIMESTAMP) from MC_BTPCAL_LINE_CAL_TEMP) " +
                    "union all select top 1 SIN_CAL_B01_TE_L_6,SIN_CAL_B02_TE_L_6,SIN_CAL_B03_TE_L_6,SIN_CAL_B04_TE_L_6,SIN_CAL_B05_TE_L_6 ,SIN_CAL_B06_TE_L_6 ,SIN_CAL_B07_TE_L_6,SIN_CAL_B08_TE_L_6 ,SIN_CAL_B09_TE_L_6,SIN_CAL_B10_TE_L_6,SIN_CAL_B11_TE_L_6,SIN_CAL_B12_TE_L_6,SIN_CAL_B13_TE_L_6,SIN_CAL_B14_TE_L_6,SIN_CAL_B15_TE_L_6,SIN_CAL_B16_TE_L_6,SIN_CAL_B17_TE_L_6,SIN_CAL_B18_TE_L_6,SIN_CAL_B19_TE_L_6,SIN_CAL_B20_TE_L_6,SIN_CAL_B21_TE_L_6,SIN_CAL_B22_TE_L_6 from MC_BTPCAL_LINE_CAL_TEMP where TIMESTAMP=(select max(TIMESTAMP) from MC_BTPCAL_LINE_CAL_TEMP) ";
                //基础数据温度点
                DataTable dataTable_1 = dBSQL.GetCommand(sql_1);
                //  三维图大小长和宽，加1必须要有
                Object[] objArrayData = new Object[9030 * 20 + 1];
                int p = 0;
                //边界位置
                StartZloc = double.Parse(dataTable_boundary.Rows[0]["PAR_INTERFACE_Y_0"].ToString());
                EndZloc = double.Parse(dataTable_boundary.Rows[0]["PAR_INTERFACE_Y_7"].ToString());

                //1#风箱开始位置23#风箱结束位置
                string sql_x_BE = "select PAR_SUC_1_STA_LOC,PAR_SUC_22_END_LOC from MC_BTPCAL_PAR ";
                DataTable dataTable_x_be = dBSQL.GetCommand(sql_x_BE);
                //1#风箱开始位置
                StartFXlocation = double.Parse(dataTable_x_be.Rows[0]["PAR_SUC_1_STA_LOC"].ToString());
                //23风箱结束位置
                EndFXlocation = double.Parse(dataTable_x_be.Rows[0]["PAR_SUC_22_END_LOC"].ToString());

                for (int y = 0; y < dataTable_y.Columns.Count; y++)
                {
                    XLocation[y] = double.Parse(dataTable_y.Rows[0][y].ToString());
                }

                for (int x = 0; x < dataTable_x.Columns.Count; x++)
                {
                    ZLocation[x] = double.Parse(dataTable_x.Rows[0][x].ToString());
                }

                for (int x = 0; x < dataTable_x.Columns.Count - 1; x++)
                {
                    RDOSize[x] = double.Parse(dataTable_x.Rows[0][x + 1].ToString()) - double.Parse(dataTable_x.Rows[0][x].ToString());
                }



                List<Point3D> _result = course_MODEL.Refresh_Data(StartZloc, EndZloc, StartFXlocation, EndFXlocation, XLocation, YLocation, ZLocation, RDOSize, dataTable_1, Step, initCount, D_value);
                for (int x = 0; x < _result.Count; x++)
                {
                    Point3D point3 = new Point3D();
                    point3.x = _result[x].x;
                    point3.y = _result[x].y;
                    point3.z = _result[x].z;
                    objArrayData[p] = point3;
                    p++;

                }


                //有多少个数据点
                objArrayData[objArrayData.Length - 1] = p;

                //把每一个xyz对应的点转换尾一个字符串
                string ds = JsonConvert.SerializeObject(objArrayData);

                return ds;
            }
            catch (Exception ee)
            {
                string mistake = "LGetBindData方法保存" + ee.ToString();
                _vLog.writelog(mistake, -1);
                return "-1";
            }

        }
        /// <summary>
        /// 曲线查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            tendency_curve_HIS(Convert.ToDateTime(textBox_begin.Text), Convert.ToDateTime(textBox_end.Text));
        }
        /// <summary>
        /// 趋势曲线历史
        /// </summary>
        public void tendency_curve_HIS(DateTime _d1, DateTime _d2)
        {
            try
            {
                string sql_curve_ls = "select " +
               "a.TIMESTAMP ," +
              "a.BTPCAL_SB_FLUE_TE_AVG ," +
              "a.BTPCAL_V ," +
              "a.BTPCAL_OUT_TOTAL_AVG_X_TRP ," +
              "a.BTPCAL_OUT_TOTAL_AVG_X_BRP ," +
              "a.BTPCAL_OUT_TOTAL_AVG_X_BTP , " +
              "b.PICAL_JPU " +
              "from MC_BTPCAL_result_1min a,M_PICAL_BREATH_RESULT_T2 b WHERE convert(varchar(16),a.TIMESTAMP,121) = convert(varchar(16),b.TIMESTAMP,121) and a.TIMESTAMP between '" + _d1 + "' and '" + _d2 + "' order by a.TIMESTAMP asc ";
                DataTable data_curve_ls = dBSQL.GetCommand(sql_curve_ls);
                if (data_curve_ls.Rows.Count > 0)
                {
                    Line1.Clear();
                    Line2.Clear();
                    Line3.Clear();
                    Line4.Clear();
                    Line5.Clear();
                    Line6.Clear();
                    List<double> Mun1 = new List<double>();
                    List<double> Mun2 = new List<double>();
                    List<double> Mun3 = new List<double>();
                    List<double> Mun4 = new List<double>();
                    List<double> Mun5 = new List<double>();
                    List<double> Mun6 = new List<double>();
                    //定义model
                    _myPlotModel = new PlotModel()
                    {
                        Background = OxyColors.White,
                        Title = "历史",
                        TitleFontSize = 7,
                        TitleColor = OxyColors.White,
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
                    };
                    _myPlotModel.Axes.Add(_dateAxis);
                    for (int i = 0; i < data_curve_ls.Rows.Count; i++)
                    {
                        //******主抽温度******
                        DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["BTPCAL_SB_FLUE_TE_AVG"]));
                        Line1.Add(line1);
                        Mun1.Add(Convert.ToDouble(data_curve_ls.Rows[i]["BTPCAL_SB_FLUE_TE_AVG"]));
                        //*****透气指数*****
                        DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["PICAL_JPU"]));
                        Line2.Add(line2);
                        Mun2.Add(Convert.ToDouble(data_curve_ls.Rows[i]["PICAL_JPU"]));
                        //*****垂直烧结极速*****
                        DataPoint line3 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["BTPCAL_V"]));
                        Line3.Add(line3);
                        Mun3.Add(Convert.ToDouble(data_curve_ls.Rows[i]["BTPCAL_V"]));
                        //*****trp位置*****
                        DataPoint line4 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["BTPCAL_OUT_TOTAL_AVG_X_TRP"]));
                        Line4.Add(line4);
                        Mun4.Add(Convert.ToDouble(data_curve_ls.Rows[i]["BTPCAL_OUT_TOTAL_AVG_X_TRP"]));
                        //*****brp位置*****
                        DataPoint line5 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["BTPCAL_OUT_TOTAL_AVG_X_BRP"]));
                        Line5.Add(line5);
                        Mun5.Add(Convert.ToDouble(data_curve_ls.Rows[i]["BTPCAL_OUT_TOTAL_AVG_X_BRP"]));
                        //*****btp位置*****
                        DataPoint line6 = new DataPoint(DateTimeAxis.ToDouble(data_curve_ls.Rows[i]["TIMESTAMP"]), Convert.ToDouble(data_curve_ls.Rows[i]["BTPCAL_OUT_TOTAL_AVG_X_BTP"]));
                        Line6.Add(line6);
                        Mun6.Add(Convert.ToDouble(data_curve_ls.Rows[i]["BTPCAL_OUT_TOTAL_AVG_X_BTP"]));
                    }

                    int x = 1;
                    if ((int)((Mun1.Max() - Mun1.Min()) / 5) > 0)
                    {
                        x = (int)((Mun1.Max() - Mun1.Min()) / 3);
                    }
                    _valueAxis1 = new LinearAxis()
                    {
                        Key = curve_name[0],
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun1.Max() + 1),
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
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Red,
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[0],
                        ItemsSource = Line1,
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n主抽温度:{4}",

                    };
                    if (checkEdit6.Checked == true)
                    {
                        _valueAxis1.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series1);
                    }

                    int x_1 = 1;//判断增长数据
                    if ((int)((Mun2.Max() - Mun2.Min()) / 5) > 0)
                    {
                        x_1 = (int)((Mun2.Max() - Mun2.Min()) / 5);
                    }
                    _valueAxis2 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[1],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun2.Max() + 1),//极值
                        Minimum = (int)(Mun2.Min() - 1),
                        PositionTier = 2,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Purple,//颜色
                        MinorTicklineColor = OxyColors.Purple,//颜色
                        TicklineColor = OxyColors.Purple,//颜色
                        TextColor = OxyColors.Purple,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_1,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis2);//添加曲线

                    series2 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Purple,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Purple,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[1],//识别符
                        ItemsSource = Line2,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n透气指数:{4}",

                    };
                    if (checkEdit7.Checked == true)
                    {
                        _valueAxis2.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series2);
                    }

                    int x_2 = 1;//判断增长数据
                    if ((int)((Mun3.Max() - Mun3.Min()) / 5) > 0)
                    {
                        x_2 = (int)((Mun3.Max() - Mun3.Min()) / 5);
                    }
                    _valueAxis3 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[2],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun3.Max() + 1),//极值
                        Minimum = (int)(Mun3.Min() - 1),
                        PositionTier = 3,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Green,//颜色
                        MinorTicklineColor = OxyColors.Green,//颜色
                        TicklineColor = OxyColors.Green,//颜色
                        TextColor = OxyColors.Green,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_2,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis3);//添加曲线

                    series3 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Green,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Green,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[2],//识别符
                        ItemsSource = Line3,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\n垂直烧结机速:{4}",

                    };
                    if (checkEdit8.Checked == true)
                    {
                        _valueAxis3.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series3);
                    }
                    int x_3 = 1;//判断增长数据
                    if ((int)((Mun4.Max() - Mun4.Min()) / 5) > 0)
                    {
                        x_3 = (int)((Mun4.Max() - Mun4.Min()) / 5);
                    }
                    _valueAxis4 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[3],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun4.Max() + 1),//极值
                        Minimum = (int)(Mun4.Min() - 1),
                        PositionTier = 4,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Blue,//颜色
                        MinorTicklineColor = OxyColors.Blue,//颜色
                        TicklineColor = OxyColors.Blue,//颜色
                        TextColor = OxyColors.Blue,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_3,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis4);//添加曲线

                    series4 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Blue,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Blue,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[3],//识别符
                        ItemsSource = Line4,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nTRP位置:{4}",

                    };
                    if (checkEdit9.Checked == true)
                    {
                        _valueAxis4.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series4);
                    }


                    int x_4 = 1;//判断增长数据
                    if ((int)((Mun5.Max() - Mun5.Min()) / 5) > 0)
                    {
                        x_4 = (int)((Mun5.Max() - Mun5.Min()) / 5);
                    }
                    _valueAxis5 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[4],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun5.Max() + 1),//极值
                        Minimum = (int)(Mun5.Min() - 1),
                        PositionTier = 5,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Olive,//颜色
                        MinorTicklineColor = OxyColors.Olive,//颜色
                        TicklineColor = OxyColors.Olive,//颜色
                        TextColor = OxyColors.Olive,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_4,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis5);//添加曲线

                    series5 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Olive,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Olive,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[4],//识别符
                        ItemsSource = Line5,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}\nBRP位置:{4}",

                    };
                    if (checkEdit10.Checked == true)
                    {
                        _valueAxis5.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series5);
                    }


                    int x_5 = 1;//判断增长数据
                    if ((int)((Mun6.Max() - Mun6.Min()) / 5) > 0)
                    {
                        x_5 = (int)((Mun6.Max() - Mun6.Min()) / 5);
                    }
                    _valueAxis6 = new LinearAxis()//声明曲线
                    {
                        Key = curve_name[5],//识别码
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun6.Max() + 1),//极值
                        Minimum = (int)(Mun6.Min() - 1),
                        PositionTier = 6,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Black,//颜色
                        MinorTicklineColor = OxyColors.Black,//颜色
                        TicklineColor = OxyColors.Black,//颜色
                        TextColor = OxyColors.Black,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_5,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis6);//添加曲线

                    series6 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Black,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Black,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name[5],//识别符
                        ItemsSource = Line6,//绑定数据
                        TrackerFormatString = "{0}时间:{2: MM月dd日 HH:mm:ss} BTP位置:{4}",

                    };
                    if (checkEdit11.Checked == true)
                    {
                        _valueAxis6.IsAxisVisible = true;
                        _myPlotModel.Series.Add(series6);
                    }
                    curve_his.Model = _myPlotModel;
                }

            }
            catch (Exception ee)
            {
                string mistake = "tendency_curve_HIS方法趋势曲线历史报错" + ee.ToString();
                _vLog.writelog(mistake, -1);
            }
        }
        /// <summary>
        /// 历史曲线勾选框数据
        /// </summary>
        public void Check_text_His()
        {
            try
            {
                //主抽温度、垂直烧结速度、TRP位置、BRP位置、BTP位置
                string sql_MC_BTPCAL_result_1min = "select " +
                    "isnull(BTPCAL_SB_FLUE_TE_AVG,0) as BTPCAL_SB_FLUE_TE_AVG" +
                    ",isnull(BTPCAL_V,0) as BTPCAL_V," +
                    "isnull(BTPCAL_OUT_TOTAL_AVG_X_TRP,0) as BTPCAL_OUT_TOTAL_AVG_X_TRP," +
                    "isnull(BTPCAL_OUT_TOTAL_AVG_X_BRP,0) as BTPCAL_OUT_TOTAL_AVG_X_BRP," +
                    "isnull(BTPCAL_OUT_TOTAL_AVG_X_BTP,0) as BTPCAL_OUT_TOTAL_AVG_X_BTP " +
                    "from MC_BTPCAL_result_1min where TIMESTAMP = (select max(TIMESTAMP) from MC_BTPCAL_result_1min) ";
                DataTable data_MC_BTPCAL_result_1min = dBSQL.GetCommand(sql_MC_BTPCAL_result_1min);
                if (data_MC_BTPCAL_result_1min.Rows.Count > 0)
                {
                    this.checkEdit6.Text = "主抽温度(℃):" + data_MC_BTPCAL_result_1min.Rows[0]["BTPCAL_SB_FLUE_TE_AVG"].ToString();
                    this.checkEdit8.Text = "垂直烧结速度(mm/min):" + data_MC_BTPCAL_result_1min.Rows[0]["BTPCAL_V"].ToString();
                    this.checkEdit9.Text = "TRP位置(m):" + data_MC_BTPCAL_result_1min.Rows[0]["BTPCAL_OUT_TOTAL_AVG_X_TRP"].ToString();
                    this.checkEdit10.Text = "BRP位置(m):" + data_MC_BTPCAL_result_1min.Rows[0]["BTPCAL_OUT_TOTAL_AVG_X_BRP"].ToString();
                    this.checkEdit11.Text = "BTP位置(m):" + data_MC_BTPCAL_result_1min.Rows[0]["BTPCAL_OUT_TOTAL_AVG_X_BTP"].ToString();
                }
                else
                {
                    this.checkEdit6.Text = "主抽温度(℃):";
                    this.checkEdit8.Text = "垂直烧结速度(mm/min):";
                    this.checkEdit9.Text = "TRP位置(m):";
                    this.checkEdit10.Text = "BRP位置(m):";
                    this.checkEdit11.Text = "BTP位置(m):";
                }
                //透气性指数
                string sql_M_PICAL_BREATH_RESULT_T2 = "select isnull(PICAL_JPU,0) as PICAL_JPU from M_PICAL_BREATH_RESULT_T2 where TIMESTAMP = (select max(TIMESTAMP) from M_PICAL_BREATH_RESULT_T2)";
                DataTable data_M_PICAL_BREATH_RESULT_T2 = dBSQL.GetCommand(sql_M_PICAL_BREATH_RESULT_T2);
                if (data_M_PICAL_BREATH_RESULT_T2.Rows.Count > 0)
                {
                    this.checkEdit7.Text = "透气性指数:" + data_M_PICAL_BREATH_RESULT_T2.Rows[0]["PICAL_JPU"].ToString();
                }
                else
                {
                    this.checkEdit7.Text = "透气性指数:";
                }

            }
            catch (Exception ee)
            {
                string mistake_1 = "Check_text_His方法报错" + ee.ToString();
                _vLog.writelog(mistake_1, -1);
            }
        }
        /// <summary>
        /// 生产实时数据
        /// </summary>
        public void Real_time()
        {

            #region TRP目标位置、BRP目标位置、BTP目标位置
            try
            {
                string sql_MC_BTPCAL_PAR = "select " +
                   "isnull(PAR_AIM_TRP,0) as PAR_AIM_TRP ," +
                   "isnull(PAR_AIM_BRP,0) as PAR_AIM_BRP," +
                   "isnull(PAR_AIM_BTP,0) as PAR_AIM_BTP " +
                   "from MC_BTPCAL_PAR where TIMESTAMP = (select max(TIMESTAMP) from MC_BTPCAL_PAR)";
                DataTable data_MC_BTPCAL_PAR = dBSQL.GetCommand(sql_MC_BTPCAL_PAR);
                if (data_MC_BTPCAL_PAR.Rows.Count > 0)
                {
                    //TRP目标位置
                    this.TEXTBOX_TRP_MB.Text = Math.Round(double.Parse(data_MC_BTPCAL_PAR.Rows[0]["PAR_AIM_TRP"].ToString()), 2).ToString() + " m";
                    //BRP目标位置
                    this.TEXTBOX_BRP_MB.Text = Math.Round(double.Parse(data_MC_BTPCAL_PAR.Rows[0]["PAR_AIM_BRP"].ToString()), 2).ToString() + " m";
                    //BTP目标位置
                    this.TEXTBOX_BTP_MB.Text = Math.Round(double.Parse(data_MC_BTPCAL_PAR.Rows[0]["PAR_AIM_BTP"].ToString()), 2).ToString() + " m";
                }
            }
            catch (Exception ee)
            {
                string mistake_1 = "real_time方法MC_BTPCAL_PAR表查询报错" + ee.ToString();
                _vLog.writelog(mistake_1, -1);
            }


            #endregion

            #region TRP实际位置、BRP实际位置、BTP实际位置、TRP温度、BRP温度、BTP温度、料层厚度、垂直烧结速度、烧结机机速、1#主抽温度、2#主抽温度、1#风机频率、2#风机频率、TRP风箱号、BRP风箱号、BTP风箱号

            try
            {
                string sql_MC_BTPCAL_result_1min = "select " +
                        "isnull(BTPCAL_OUT_TOTAL_AVG_X_TRP,0), " +
                        "isnull(BTPCAL_OUT_TOTAL_AVG_X_BRP,0)," +
                        "isnull(BTPCAL_OUT_TOTAL_AVG_X_BTP,0)," +
                        "isnull(BTPCAL_OUT_TOTAL_AVG_TE_TRP,0)," +
                        "isnull(BTPCAL_OUT_TOTAL_AVG_TE_BRP,0)," +
                        "isnull(BTPCAL_OUT_TOTAL_AVG_TE_BTP,0)," +
                        "isnull(BTPCAL_SMALL_SG_PV_AVG,0)," +
                        "isnull(BTPCAL_V,0)," +
                        "isnull(BTPCAL_SIN_MS_PV,0)," +
                        "isnull(BTPCAL_SB_1_FLUE_TE,0)," +
                        "isnull(BTPCAL_SB_2_FLUE_TE,0)," +
                        "isnull(BTPCAL_FAN_1_SP_FRE,0)," +
                        "isnull(BTPCAL_FAN_2_SP_FRE,0)," +
                        "isnull(BTPCAL_TRP_TOTAL_NUM,0)," +
                        "isnull(BTPCAL_BRP_TOTAL_NUM,0)," +
                        "isnull(BTPCAL_BTP_TOTAL_NUM,0)" +
                        " from MC_BTPCAL_result_1min where TIMESTAMP = (select max(TIMESTAMP) from MC_BTPCAL_result_1min)";
                DataTable data_MC_BTPCAL_result_1min = dBSQL.GetCommand(sql_MC_BTPCAL_result_1min);
                if (data_MC_BTPCAL_result_1min.Rows.Count > 0)
                {
                    List<double> _List = new List<double>();
                    for (int x = 0; x < data_MC_BTPCAL_result_1min.Columns.Count; x++)
                    {
                        _List.Add(Math.Round(double.Parse(data_MC_BTPCAL_result_1min.Rows[0][x].ToString()), 2));
                    }

                    //TRP实际位置
                    this.TEXTBOX_TRP_SJ.Text = _List[0].ToString() + " m";
                    //BRP实际位置
                    this.TEXTBOX_BRP_SJ.Text = _List[1].ToString() + " m";
                    //BTP实际位置
                    this.TEXTBOX_BTP_SJ.Text = _List[2].ToString() + " m";
                    //TRP温度
                    this.TEXTBOX_TRP_WD.Text = _List[3].ToString() + " ℃";
                    //BRP温度
                    this.TEXTBOX_BRP_WD.Text = _List[4].ToString() + " ℃";
                    //BTP温度
                    this.TEXTBOX_BTP_WD.Text = _List[5].ToString() + " ℃";
                    //料层厚度
                    this.TEXTBOX_LCHD.Text = _List[6].ToString() + " mm";
                    //垂直烧结速度
                    this.TEXTBOX_CZSJSD.Text = _List[7].ToString() + " mm/min";
                    //1#主抽温度
                    this.TEXTBOX_ZCWD_1.Text = _List[9].ToString() + " ℃";
                    //2#主抽温度
                    this.TEXTBOX_ZCWD_2.Text = _List[10].ToString() + " ℃";
                    //1#风机频率
                    this.TEXTBOX_FJPL_1.Text = _List[11].ToString() + " HZ";
                    //2#风机频率
                    this.TEXTBOX_FJPL_2.Text = _List[12].ToString() + " HZ";
                    //TRP风箱号
                    this.TEXTBOX_TRP_FXH.Text = _List[13].ToString();
                    //BRP风箱号
                    this.TEXTBOX_BRP_FXH.Text = _List[14].ToString();
                    //BTP风箱号
                    this.TEXTBOX_BTP_FXH.Text = _List[15].ToString();
                }
                else
                {

                }
            }
            catch (Exception ee)
            {
                string mistake_2 = "生产实时数据MC_BTPCAL_result_1min表查询报错" + ee.ToString();
                _vLog.writelog(mistake_2, -1);
            }
            #endregion

            #region 烧结机频率SP、烧结机频率PV   20210128更改数据源 

            try
            {
                var sql_C_MFI_PLC_1MIN = "select " +
                        "isnull(F_PLC_SIN_SPEED_SP,0) as F_PLC_SIN_SPEED_SP ," +
                        "isnull(F_PLC_SIN_SPEED_PV,0) as F_PLC_SIN_SPEED_PV " +
                        "from C_MFI_PLC_1MIN where  TIMESTAMP = (select max(TIMESTAMP) from C_MFI_PLC_1MIN)";
                DataTable data_C_MFI_PLC_1MIN = dBSQL.GetCommand(sql_C_MFI_PLC_1MIN);
                if (data_C_MFI_PLC_1MIN.Rows.Count > 0)
                {
                    //烧结机频率SP
                    this.TEXTBOX_JS_SP.Text = Math.Round(double.Parse(data_C_MFI_PLC_1MIN.Rows[0]["F_PLC_SIN_SPEED_SP"].ToString()), 2).ToString();
                    //烧结机频率PV
                    this.TEXTBOX_JS_PV.Text = Math.Round(double.Parse(data_C_MFI_PLC_1MIN.Rows[0]["F_PLC_SIN_SPEED_PV"].ToString()), 2).ToString();
                }
            }
            catch (Exception ee)
            {
                string mistake_1 = "生产实时数据C_MFI_PLC_1MIN表查询报错" + ee.ToString();
                _vLog.writelog(mistake_1, -1);
            }
            #endregion
            #region 透气性指数
            try
            {
                string sql_M_PICAL_BREATH_RESULT_T2 = "select PICAL_JPU from M_PICAL_BREATH_RESULT_T2 where TIMESTAMP = (select max(TIMESTAMP) from M_PICAL_BREATH_RESULT_T2)";
                DataTable data_M_PICAL_BREATH_RESULT_T2 = dBSQL.GetCommand(sql_M_PICAL_BREATH_RESULT_T2);
                if (data_M_PICAL_BREATH_RESULT_T2.Rows.Count > 0)
                {

                    this.TEXTBOX_TQXZS.Text = Math.Round(double.Parse(data_M_PICAL_BREATH_RESULT_T2.Rows[0]["PICAL_JPU"].ToString()), 2).ToString();
                }
                else
                {
                    this.TEXTBOX_TQXZS.Text = "0";
                }
            }
            catch (Exception ee)
            {
                string mistake_1 = "生产实时数据M_PICAL_BREATH_RESULT_T2表查询报错" + ee.ToString();
                _vLog.writelog(mistake_1, -1);
            }
            #endregion

        }
        /// <summary>
        /// 调整数据查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Frm_BTP_Adjust form_display = new Frm_BTP_Adjust();
            if (Frm_BTP_Adjust.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }
        /// <summary>
        /// 趋势曲线实时
        /// </summary>
        public void tendency_curve_Real()
        {
            try
            {
                 DateTime _d2 = DateTime.Now;
               // DateTime _d2 = DateTime.Parse("2021/1/11 06:33:26");
                DateTime _d1 = _d2.AddMinutes(-60);
              //  DateTime _d1 = DateTime.Parse("2021/1/11 05:33:26");
                Line_1.Clear();
                Line_2.Clear();
                Line_3.Clear();
                Line_4.Clear();
                Line_5.Clear();
                List<double> Mun1 = new List<double>();
                List<double> Mun2 = new List<double>();
                List<double> Mun3 = new List<double>();
                List<double> Mun4 = new List<double>();
                List<double> Mun5 = new List<double>();
                //定义model
                _PlotModel = new PlotModel()
                {
                    Background = OxyColors.White,
                    Title = "实时",
                    TitleFontSize = 7,
                    TitleColor = OxyColors.White,
                    //LegendMargin = 100,
                };
                //X轴
                X_Axis = new DateTimeAxis()
                {
                    MajorGridlineStyle = LineStyle.Solid,
                    MinorGridlineStyle = LineStyle.Dot,
                    IntervalLength = 100,
                    IsZoomEnabled = true,
                    IsPanEnabled = false,
                    AxisTickToLabelDistance = 0,
                    FontSize = 9.0,
                };
                _PlotModel.Axes.Add(X_Axis);

                Tuple<int, Dictionary<string, List<Tuple<DateTime, double>>>> _Date = _MODEL._Trend_Curve( _d1,  _d2);
                if (_Date != null)
                {
                    #region
                    for (int x = 0; x < _Date.Item1;x++)
                    {
                        // BTP实际
                        DataPoint _point_1 = new DataPoint(DateTimeAxis.ToDouble(_Date.Item2["BTP"][x].Item1), Math.Round( _Date.Item2["BTP"][x].Item2, DIS_1));
                        Line_1.Add(_point_1);
                        Mun1.Add(_Date.Item2["BTP"][x].Item2);
                        // BTP预测
                        DataPoint _point_2 = new DataPoint(DateTimeAxis.ToDouble(_Date.Item2["BTP_PREDICT"][x].Item1), Math.Round(_Date.Item2["BTP_PREDICT"][x].Item2, DIS_1));
                        Line_2.Add(_point_2);
                        Mun2.Add(_Date.Item2["BTP_PREDICT"][x].Item2);
                        // BRP实际
                        DataPoint _point_3 = new DataPoint(DateTimeAxis.ToDouble(_Date.Item2["BRP"][x].Item1), Math.Round(_Date.Item2["BRP"][x].Item2, DIS_1));
                        Line_3.Add(_point_3);
                        Mun3.Add(_Date.Item2["BRP"][x].Item2);
                        // BRP预测
                        DataPoint _point_4 = new DataPoint(DateTimeAxis.ToDouble(_Date.Item2["BRP_PREDICT"][x].Item1), Math.Round(_Date.Item2["BRP_PREDICT"][x].Item2, DIS_1));
                        Line_4.Add(_point_4);
                        Mun4.Add(_Date.Item2["BRP_PREDICT"][x].Item2);
                        // TRP实际
                        DataPoint _point_5 = new DataPoint(DateTimeAxis.ToDouble(_Date.Item2["TRP"][x].Item1), Math.Round(_Date.Item2["TRP"][x].Item2, DIS_1));
                        Line_5.Add(_point_5);
                        Mun5.Add(_Date.Item2["TRP"][x].Item2);

                    }
                    #endregion
                    #region 绑定数据
                    #region BTP实际
                    int x_1 = 10;
                    if ((int)((Mun1.Max() - Mun1.Min()) / 5) > 0)
                    {
                        x_1 = (int)((Mun1.Max() - Mun1.Min()) / 5);
                    }
                    Y_Axis1 = new LinearAxis()
                    {
                        Key = curve_name_1[0],
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun1.Max() + 1),
                      //  Minimum = (int)(Mun1.Min() - 1),
                        Minimum = 40,
                        PositionTier = 1,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Red,
                        MinorTicklineColor = OxyColors.Red,
                        TicklineColor = OxyColors.Red,
                        TextColor = OxyColors.Red,
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_1,
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _PlotModel.Axes.Add(Y_Axis1);
                    //添加曲线
                    series_1 = new OxyPlot.Series.LineSeries()
                    {
                        Color = OxyColors.Red,
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Red,
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name_1[0],
                        ItemsSource = Line_1,
                        TrackerFormatString = "{0}\n时间:{2: MM月dd日 HH:mm:ss} BTP实际:{4}",
                    };
                    if (checkEdit1.Checked == true)
                    {
                        Y_Axis1.IsAxisVisible = true;
                        _PlotModel.Series.Add(series_1);
                    }
                    #endregion
                    #region BTP预测
                    //int x_2 = 1;
                    //if ((int)((Mun2.Max() - Mun2.Min()) / 5) > 0)
                    //{
                    //    x_2 = (int)((Mun2.Max() - Mun2.Min()) / 5);
                    //}
                    Y_Axis2 = new LinearAxis()
                    {
                        Key = curve_name_1[1],
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun1.Max() + 1),
                        // Minimum = (int)(Mun1.Min() - 1),
                        Minimum = 40,
                        PositionTier = 1,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Purple,
                        MinorTicklineColor = OxyColors.Purple,
                        TicklineColor = OxyColors.Purple,
                        TextColor = OxyColors.Purple,
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_1,
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _PlotModel.Axes.Add(Y_Axis2);
                    //添加曲线
                    series_2 = new OxyPlot.Series.LineSeries()
                    {
                        Color = OxyColors.Purple,
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Purple,
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name_1[1],
                        ItemsSource = Line_2,
                        TrackerFormatString = "{0}\n时间:{2: MM月dd日 HH:mm:ss} BTP预测:{4}",
                    };
                    if (checkEdit2.Checked == true)
                    {
                        Y_Axis2.IsAxisVisible = true;
                        _PlotModel.Series.Add(series_2);
                    }
                    #endregion
                    #region BRP实际
                    //int x_3 = 1;
                    //if ((int)((Mun3.Max() - Mun3.Min()) / 5) > 0)
                    //{
                    //    x_3 = (int)((Mun3.Max() - Mun3.Min()) / 5);
                    //}
                    Y_Axis3 = new LinearAxis()
                    {
                        Key = curve_name_1[2],
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun1.Max() + 1),
                        //  Minimum = (int)(Mun1.Min() - 1),
                        Minimum = 40,
                        PositionTier = 1,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Green,
                        MinorTicklineColor = OxyColors.Green,
                        TicklineColor = OxyColors.Green,
                        TextColor = OxyColors.Green,
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_1,
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _PlotModel.Axes.Add(Y_Axis3);
                    //添加曲线
                    series_3 = new OxyPlot.Series.LineSeries()
                    {
                        Color = OxyColors.Green,
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Green,
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name_1[2],
                        ItemsSource = Line_3,
                        TrackerFormatString = "{0}\n时间:{2: MM月dd日 HH:mm:ss}BRP实际:{4}",
                    };
                    if (checkEdit3.Checked == true)
                    {
                        Y_Axis3.IsAxisVisible = true;
                        _PlotModel.Series.Add(series_3);
                    }
                    #endregion
                    #region BRP预测
                    //int x_4 = 1;
                    //if ((int)((Mun4.Max() - Mun4.Min()) / 5) > 0)
                    //{
                    //    x_4 = (int)((Mun4.Max() - Mun4.Min()) / 5);
                    //}
                    Y_Axis4 = new LinearAxis()
                    {
                        Key = curve_name_1[3],
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun1.Max() + 1),
                        //Minimum = (int)(Mun1.Min() - 1),
                        Minimum = 40,
                        PositionTier = 1,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Blue,
                        MinorTicklineColor = OxyColors.Blue,
                        TicklineColor = OxyColors.Blue,
                        TextColor = OxyColors.Blue,
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_1,
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _PlotModel.Axes.Add(Y_Axis4);
                    //添加曲线
                    series_4 = new OxyPlot.Series.LineSeries()
                    {
                        Color = OxyColors.Blue,
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Blue,
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name_1[3],
                        ItemsSource = Line_4,
                        TrackerFormatString = "{0}\n时间:{2: MM月dd日 HH:mm:ss}BRP实际:{4}",
                    };
                    if (checkEdit4.Checked == true)
                    {
                        Y_Axis4.IsAxisVisible = true;
                        _PlotModel.Series.Add(series_4);
                    }
                    #endregion
                    #region TRP实际
                    //int x_5 = 1;
                    //if ((int)((Mun5.Max() - Mun5.Min()) / 5) > 0)
                    //{
                    //    x_5 = (int)((Mun5.Max() - Mun5.Min()) / 5);
                    //}
                    Y_Axis5 = new LinearAxis()
                    {
                        Key = curve_name_1[4],
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun1.Max() + 1),
                        // Minimum = (int)(Mun1.Min() - 1),
                        Minimum = 40,
                        PositionTier = 1,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Black,
                        MinorTicklineColor = OxyColors.Black,
                        TicklineColor = OxyColors.Black,
                        TextColor = OxyColors.Black,
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_1,
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _PlotModel.Axes.Add(Y_Axis5);
                    //添加曲线
                    series_5 = new OxyPlot.Series.LineSeries()
                    {
                        Color = OxyColors.Black,
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Black,
                        MarkerType = MarkerType.None,
                        YAxisKey = curve_name_1[4],
                        ItemsSource = Line_5,
                        TrackerFormatString = "{0}\n时间:{2: MM月dd日 HH:mm:ss}TRP实际:{4}",
                    };
                    if (checkEdit5.Checked == true)
                    {
                        Y_Axis5.IsAxisVisible = true;
                        _PlotModel.Series.Add(series_5);
                    }
                    #endregion
                    Curve_Real.Model = _PlotModel;
                    #endregion

                }
                else
                {
                   _vLog.writelog("tendency_curve_Real方法失败,接收_Trend_Curve方法为空", -1);
                }
            }
            catch(Exception ee)
            {
                _vLog.writelog("tendency_curve_Real方法失败" + ee.ToString(), -1);
            }
        }

        public void Timer_state()
        {
            _Timer1.Enabled = true;
            _Timer2.Enabled = true;
            _Timer3.Enabled = true;
        }
        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {
            _Timer1.Enabled = false;
            _Timer2.Enabled = false;
            _Timer3.Enabled = false;
        }
        /// <summary>
        /// 控件关闭
        /// </summary>
        public void _Clear()
        {
            _Timer1.Enabled = false;
            _Timer1.Close();//释放定时器资源
            _Timer2.Enabled = false;
            _Timer2.Close();//释放定时器资源
            _Timer3.Enabled = false;
            _Timer3.Close();//释放定时器资源
            this.Dispose();//释放资源
            GC.Collect();//调用GC
            bool x = this.IsDisposed;
        }

        private void checkEdit1_Click(object sender, EventArgs e)
        {
            try
            {
                Curve_Real.Model = null;
                if (checkEdit1.Checked == true)
                {
                    Y_Axis1.IsAxisVisible = true;
                    _PlotModel.Series.Add(series_1);
                }
                if (checkEdit1.Checked == false)
                {
                    Y_Axis1.IsAxisVisible = false;
                    _PlotModel.Series.Remove(series_1);
                }
                Curve_Real.Model = _PlotModel;
            }
            catch
            { }
        }

        private void checkEdit2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Curve_Real.Model = null;
                if (checkEdit2.Checked == true)
                {
                    Y_Axis2.IsAxisVisible = true;
                    _PlotModel.Series.Add(series_2);
                }
                if (checkEdit2.Checked == false)
                {
                    Y_Axis2.IsAxisVisible = false;
                    _PlotModel.Series.Remove(series_2);
                }
                Curve_Real.Model = _PlotModel;
            }
            catch
            { }
        }

        private void checkEdit3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Curve_Real.Model = null;
                if (checkEdit3.Checked == true)
                {
                    Y_Axis3.IsAxisVisible = true;
                    _PlotModel.Series.Add(series_3);
                }
                if (checkEdit3.Checked == false)
                {
                    Y_Axis3.IsAxisVisible = false;
                    _PlotModel.Series.Remove(series_3);
                }
                Curve_Real.Model = _PlotModel;
            }
            catch
            { }
        }

        private void checkEdit4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Curve_Real.Model = null;
                if (checkEdit4.Checked == true)
                {
                    Y_Axis4.IsAxisVisible = true;
                    _PlotModel.Series.Add(series_4);
                }
                if (checkEdit4.Checked == false)
                {
                    Y_Axis4.IsAxisVisible = false;
                    _PlotModel.Series.Remove(series_4);
                }
                Curve_Real.Model = _PlotModel;
            }
            catch
            { }
        }

        private void checkEdit5_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                Curve_Real.Model = null;
                if (checkEdit5.Checked == true)
                {
                    Y_Axis5.IsAxisVisible = true;
                    _PlotModel.Series.Add(series_5);
                }
                if (checkEdit5.Checked == false)
                {
                    Y_Axis5.IsAxisVisible = false;
                    _PlotModel.Series.Remove(series_5);
                }
                Curve_Real.Model = _PlotModel;
            }
            catch
            { }
        }
        /// <summary>
        /// 参数修改弹出框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Frm_BTP_Par form_display = new Frm_BTP_Par();
            if (Frm_BTP_Par.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            Frm_BTP_Guide form_display = new Frm_BTP_Guide();
            if (Frm_BTP_Guide.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        private void checkEdit6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkEdit6_Click(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkEdit6.Checked == true)
                {
                    _valueAxis1.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series1);
                }
                if (checkEdit6.Checked == false)
                {
                    _valueAxis1.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series1);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkEdit7_Click(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkEdit7.Checked == true)
                {
                    _valueAxis2.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series2);
                }
                if (checkEdit7.Checked == false)
                {
                    _valueAxis2.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series2);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkEdit8_Click(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkEdit8.Checked == true)
                {
                    _valueAxis3.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series3);
                }
                if (checkEdit8.Checked == false)
                {
                    _valueAxis3.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series3);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkEdit9_Click(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkEdit9.Checked == true)
                {
                    _valueAxis4.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series4);
                }
                if (checkEdit9.Checked == false)
                {
                    _valueAxis4.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series4);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkEdit10_Click(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkEdit10.Checked == true)
                {
                    _valueAxis5.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series5);
                }
                if (checkEdit10.Checked == false)
                {
                    _valueAxis5.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series5);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }

        private void checkEdit11_Click(object sender, EventArgs e)
        {
            try
            {
                curve_his.Model = null;
                if (checkEdit11.Checked == true)
                {
                    _valueAxis6.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series6);
                }
                if (checkEdit11.Checked == false)
                {
                    _valueAxis6.IsAxisVisible = false;
                    _myPlotModel.Series.Remove(series6);
                }
                curve_his.Model = _myPlotModel;
            }
            catch
            { }
        }
    }
}
