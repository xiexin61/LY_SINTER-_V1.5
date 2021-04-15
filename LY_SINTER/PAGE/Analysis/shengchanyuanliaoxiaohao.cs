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
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using LY_SINTER.Custom;

namespace LY_SINTER.PAGE.Analysis
{
    public partial class shengchanyuanliaoxiaohao : UserControl
    {
        private DBSQL dBSQL = new DBSQL(ConstParameters.strCon);

        public shengchanyuanliaoxiaohao()
        {
            InitializeComponent();
            GetNewTime();
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            DateTime d1 = DateTime.Now.AddMonths(-1);
            DateTime d2 = DateTime.Now;
            getData(d1, d2);
            dkdhfx(d1, d2);
            gtrhfx(d1, d2);
        }

        /// <summary> 开始时间&结束时间赋值 </summary>
        public void dateTimePicker_value()
        {
            try
            {
                //结束时间
                DateTime time_end = DateTime.Now;
                //开始时间
                DateTime time_begin = time_end.AddMonths(-1);

                textBox_begin.Text = time_begin.ToString();
                textBox_end.Text = time_end.ToString();
            }
            catch (Exception ee)
            {
            }
        }

        private string sql = "", row = "", sql1 = "", qx = "";

        public void GetNewTime()
        {
            DBSQL dBSQL = new DBSQL(ConstParameters.strCon);
            string sql = "select top(1) TIMESTAMP from MC_POPCAL_CONSUME order by TIMESTAMP desc;";
            DataTable table = dBSQL.GetCommand(sql);
            if (table.Rows.Count > 0)
            {
                string time = table.Rows[0][0].ToString();
                this.label2.Text = "最新调整时间:" + time;
            }
        }

        /// <summary>
        /// 原料消耗表格查询
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void getData(DateTime start, DateTime end)
        {
            string banci = comboBox1.Text;

            DataTable d1 = new DataTable();
            if (banci == "全部")
            {
                sql = "select TIMESTAMP,POP_CLASS,MAT_DESC,MAT_VALUE from MC_POPCAL_CONSUME where TIMESTAMP between '" + start + "' and '" + end + "' order by timestamp desc";
                row = "select count(distinct(TIMESTAMP)) from MC_POPCAL_CONSUME where TIMESTAMP between '" + start + "' and '" + end + "';";
                sql1 = "select distinct(TIMESTAMP),POP_CLASS from MC_POPCAL_CONSUME where TIMESTAMP between '" + start + "' and '" + end + "'";
                qx = "select MAT_DESC,isnull(UNIT_PRICE,0) from M_MATERIAL_COOD where L2_CODE in(select L2_CODE from MC_POPCAL_CONSUME) and TIMESTAMP between '" + start + "' and '" + end + "'";
            }
            else
            {
                sql = "select TIMESTAMP,POP_CLASS,MAT_DESC,MAT_VALUE from MC_POPCAL_CONSUME where TIMESTAMP between '" + start + "' and '" + end + "' and POP_CLASS='" + banci + "' order by timestamp desc";
                row = "select count(distinct(TIMESTAMP)) from MC_POPCAL_CONSUME where TIMESTAMP between '" + start + "' and '" + end + "' and POP_CLASS='" + banci + "';";
                sql1 = "select distinct(TIMESTAMP),POP_CLASS from MC_POPCAL_CONSUME where TIMESTAMP between '" + start + "' and '" + end + "' and POP_CLASS='" + banci + "'";
                qx = "select MAT_DESC,isnull(UNIT_PRICE,0) from M_MATERIAL_COOD where L2_CODE in(select L2_CODE from MC_POPCAL_CONSUME where POP_CLASS='" + banci + "') and TIMESTAMP between '" + start + "' and '" + end + "'";
            }

            DataTable table = dBSQL.GetCommand(sql);
            //string row = "select count(distinct(TIMESTAMP)) from MC_POPCAL_CONSUME;" ;
            DataTable trow = dBSQL.GetCommand(row);
            int count = int.Parse(trow.Rows[0][0].ToString());
            dataGridView2.RowCount = count + 1;
            //dataGridView2.RowCount = 7;
            if (table.Rows.Count > 0)
            {
                //dataGridView2.Rows[0].Cells[0].Value = "时间";
                //dataGridView2.Rows[0].Cells[1].Value = "时间";
                //dataGridView2.Columns.Add("时间", "时间");
                dataGridView2.Columns[0].HeaderText = "时间";
                dataGridView2.Columns[0].Name = "时间";
                dataGridView2.Columns[0].MinimumWidth = 150;
                //dataGridView2.Columns[0].AutoSizeMode;
                //dataGridView2.Columns[1].HeaderText = "班别";
                //dataGridView2.Columns[1].Name = "班别";
                string bb = "班别";
                if (dataGridView2.Columns.Contains(bb) == false)
                {
                    dataGridView2.Columns.Add(bb, bb);
                }

                //d1.Columns.Add("时间");
                //d1.Columns.Add("班别");
                for (int r = 0; r < table.Rows.Count; r++)
                {
                    //表头添加物料描述
                    string KF_NAME = table.Rows[r]["MAT_DESC"].ToString() == "" ? "0" : table.Rows[r]["MAT_DESC"].ToString() + "(t)";
                    string KF_NAME1 = table.Rows[r]["MAT_DESC"].ToString() == "" ? "0" : table.Rows[r]["MAT_DESC"].ToString();
                    if (dataGridView2.Columns.Contains(KF_NAME1) == false)
                    {
                        dataGridView2.Columns.Add(KF_NAME1, KF_NAME);

                        //d1.Columns.Add(KF_NAME);
                    }
                }
                //string sql1 = "select distinct(TIMESTAMP),POP_CLASS from MC_POPCAL_CONSUME where TIMESTAMP between '" + start + "' and '" + end + "'";
                DataTable tsql1 = dBSQL.GetCommand(sql1);
                for (int i = 0; i < tsql1.Rows.Count; i++)
                {
                    var timestamp = DateTime.Parse(tsql1.Rows[i]["TIMESTAMP"].ToString());
                    dataGridView2.Rows[i + 1].Cells[0].Value = timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                    dataGridView2.Rows[i + 1].Cells["班别"].Value = tsql1.Rows[i]["POP_CLASS"].ToString();
                    string data = "select MAT_DESC,MAT_VALUE from MC_POPCAL_CONSUME where TIMESTAMP between '" + start + "' and '" + end + "' and TIMESTAMP='" + tsql1.Rows[i]["TIMESTAMP"].ToString() + "' and POP_CLASS='" + tsql1.Rows[i]["POP_CLASS"].ToString() + "';";
                    DataTable tdata = dBSQL.GetCommand(data);
                    for (int j = 0; j < tdata.Rows.Count; j++)
                    {
                        string KF_NAME = tdata.Rows[j]["MAT_DESC"].ToString() == "" ? "0" : tdata.Rows[j]["MAT_DESC"].ToString();
                        dataGridView2.Rows[i + 1].Cells[KF_NAME].Value = tdata.Rows[j]["MAT_VALUE"].ToString();
                    }
                }
                dataGridView2.Rows[0].Cells[0].Value = "合计";
                //d1.Rows[0][0] = "合计";
                //double sum = 0.0;
                int k = 0;
                //for (int i = 0; i < d1.Rows.Count-1; i++)
                for (k = 2; k < dataGridView2.Columns.Count; k++)
                {
                    double sum = 0.0;
                    for (int i = 1; i < dataGridView2.Rows.Count; i++)
                    {
                        sum = sum + Convert.ToDouble(dataGridView2.Rows[i].Cells[k].Value);
                    }
                    dataGridView2.Rows[0].Cells[k].Value = sum;
                }
            }
            for (int i = 2; i < dataGridView2.Columns.Count; i++)
            {
                dataGridView2.Columns[i].Width = 150;
            }
        }

        /// <summary>
        /// 原料单耗成本查询
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void dkdhfx(DateTime start, DateTime end)
        {
            List<double> y = new List<double>();
            List<string> x = new List<string>();
            double sjkcl = 0.0;
            string sql2 = "select isnull(sum(CFP_PLC_PROD_DELT1_FQ)/60,0) from C_CFP_PLC_1MIN where TIMESTAMP between '" + start + "' and '" + end + "'";
            System.Data.DataTable table2 = dBSQL.GetCommand(sql2);
            if (table2.Rows.Count > 0)
            {
                sjkcl = Convert.ToDouble(table2.Rows[0][0]);
            }
            string qx = "select MAT_DESC,isnull(UNIT_PRICE,0) from M_MATERIAL_COOD where L2_CODE in(select distinct L2_CODE from MC_POPCAL_CONSUME where TIMESTAMP between '" + start + "' and '" + end + "')";
            System.Data.DataTable table = dBSQL.GetCommand(qx);
            if (table.Rows.Count > 0)
            {
                for (int r = 0; r < table.Rows.Count; r++)
                {
                    string KF_NAME = table.Rows[r]["MAT_DESC"].ToString();
                    x.Add(KF_NAME);
                    //DataPoint line = new DataPoint(Convert.ToDouble(table.Rows[r]["MAT_DESC"]), Convert.ToDouble(table.Rows[r]["UNIT_PRICE"])/ sjkcl);
                    double ylxhhj = Convert.ToDouble(dataGridView2.Rows[0].Cells[KF_NAME].Value);
                    y.Add(double.Parse((ylxhhj * (Convert.ToDouble(table.Rows[r][1]) / sjkcl)).ToString("0.00")));
                }
            }

            PlotModel _myPlotModel = new PlotModel()
            {
                PlotMargins = new OxyThickness(50, 20, 30, 10),
                Background = OxyColors.White,
            };
            //X轴定义categoriesAxi
            CategoryAxis _categoryAxis = new CategoryAxis()
            {
                MajorTickSize = 0,
                IsZoomEnabled = false,
                Position = AxisPosition.Bottom,
                FontSize = 9.0,
                //GapWidth=30,//柱子间隔宽度
            };
            for (int i = 0; i < x.Count; i++)
            {
                _categoryAxis.Labels.Add(x[i]);
            }
            _myPlotModel.Axes.Add(_categoryAxis);
            //Y轴
            LinearAxis _valueAxis = new LinearAxis()
            {
                MinorTickSize = 0,
                Key = "y",
                FontSize = 9.0,
            };
            _myPlotModel.Axes.Add(_valueAxis);
            var _ColumnSeries = new ColumnSeries()
            {//柱状图
                Background = OxyColors.White,
                LabelPlacement = LabelPlacement.Inside,
                //LabelFormatString="{0.00}",
                TextColor = OxyColors.Black,
                //StrokeThickness = 6,
                //TrackerFormatString = "{0}"
            };
            for (int i = 0; i < y.Count; i++)
            {
                _ColumnSeries.Items.Add(new ColumnItem() { Value = y[i] });
            }
            _myPlotModel.Series.Add(_ColumnSeries);
            _ColumnSeries.LabelPlacement = LabelPlacement.Inside;
            plotView1.Model = _myPlotModel;
            var PlotController = new OxyPlot.PlotController();
            PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);

            plotView1.Controller = PlotController;
        }

        public static int getnMax(int max, int min)
        {
            int s = 0;
            if (min != 0)
            {
                if (max % min != 0)
                {
                    s = (max / min + 1) * min;
                }
                else
                {
                    s = max;
                }
            }
            else
            {
                s = max;
            }
            return s;
        }

        private List<DataPoint> Line1 = new List<DataPoint>();
        private List<DataPoint> Line2 = new List<DataPoint>();

        //private List<ScatterPoint> Line2 = new List<ScatterPoint>();
        private List<double> list1 = new List<double>();

        private List<double> list2 = new List<double>();

        /// <summary>
        /// 固体燃耗分析曲线查询
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void gtrhfx(DateTime start, DateTime end)
        {
            Line1.Clear();
            Line2.Clear();
            list1.Clear();
            list2.Clear();
            string sql = "select TIMESTAMP,POPCAL_H_TSC_CON,POPCAL_H_TSC_CON_LL from MC_POPCAL_RESULT_HOUR where TIMESTAMP between '" + start + "' and '" + end + "' order by timestamp";
            System.Data.DataTable table = dBSQL.GetCommand(sql);
            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table.Rows[i]["POPCAL_H_TSC_CON"]));
                    Line1.Add(line1);
                    list1.Add(Convert.ToDouble(table.Rows[i]["POPCAL_H_TSC_CON"]));
                    //ScatterPoint line2 = new ScatterPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table.Rows[i]["POPCAL_H_TSC_CON_LL"]));
                    DataPoint line2 = new DataPoint(DateTimeAxis.ToDouble(table.Rows[i]["TIMESTAMP"]), Convert.ToDouble(table.Rows[i]["POPCAL_H_TSC_CON_LL"]));
                    Line2.Add(line2);
                    list2.Add(Convert.ToDouble(table.Rows[i]["POPCAL_H_TSC_CON_LL"]));
                }
            }
            PlotModel plotModel = new PlotModel()
            {
                PlotMargins = new OxyThickness(50, 20, 30, 10),
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Inside,
                LegendPosition = LegendPosition.TopRight,
                Background = OxyColors.White,
            };
            DateTimeAxis dateTimeAxis = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IntervalLength = 100,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxisTickToLabelDistance = 0,
                FontSize = 9.0,
                StringFormat = "yyyy/MM/dd HH:mm",
            };
            plotModel.Axes.Add(dateTimeAxis);

            #region 曲线修改

            var _valueAxis1_1 = new LinearAxis()
            {
                Key = "理论湿基",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.LightCoral,
                MinorTicklineColor = OxyColors.LightCoral,
                TicklineColor = OxyColors.LightCoral,
                TextColor = OxyColors.LightCoral,
                FontSize = 9.0,
                //IsAxisVisible = true,
                MinorTickSize = 0,
            };
            var _valueAxis1_2 = new LinearAxis()
            {
                Key = "实际湿基",
                MajorGridlineStyle = LineStyle.None,
                MinorGridlineStyle = LineStyle.None,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                AxislineStyle = LineStyle.Solid,
                AxislineColor = OxyColors.Green,
                MinorTicklineColor = OxyColors.Green,
                TicklineColor = OxyColors.Green,
                TextColor = OxyColors.Green,
                FontSize = 9.0,
                //IsAxisVisible = true,
                MinorTickSize = 0,
                PositionTier = 2,
            };

            #endregion 曲线修改

            plotModel.Axes.Add(_valueAxis1_1);
            plotModel.Axes.Add(_valueAxis1_2);
            LineSeries series = new LineSeries()
            {
                YAxisKey = "理论湿基",
                Color = OxyColors.LightCoral,
                StrokeThickness = 1,
                MarkerSize = 3,
                MarkerStroke = OxyColors.LightCoral,
                MarkerType = MarkerType.None,
                ItemsSource = Line1,
                TrackerFormatString = "{0}\n时间:{2:yyyy/MM/dd HH:mm:ss}\n理论湿基:{4}"
            };
            plotModel.Series.Add(series);
            //ScatterSeries scatterSeries = new ScatterSeries()
            LineSeries scatterSeries = new LineSeries()
            {
                YAxisKey = "实际湿基",
                //MarkerType = MarkerType.Circle,
                StrokeThickness = 1,
                MarkerSize = 1,
                MarkerStroke = OxyColors.Green,
                ItemsSource = Line2,
                TrackerFormatString = "{0}\n时间:{2:yyyy/MM/dd HH:mm:ss}\n实际湿基:{4}",
            };

            plotModel.Series.Add(scatterSeries);
            plotView2.Model = plotModel;
            var PlotController = new OxyPlot.PlotController();
            PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);

            plotView2.Controller = PlotController;
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        //查询固体湿基曲线按钮
        private void simpleButton4_click(object sender, EventArgs e)
        {
        }

        //查询按钮
        private void simpleButton2_click(object sender, EventArgs e)
        {
            DateTime d1 = Convert.ToDateTime(textBox_begin.Text);
            DateTime d2 = Convert.ToDateTime(textBox_end.Text);
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();
            getData(d1, d2);
            dkdhfx(d1, d2);
            gtrhfx(d1, d2);
        }

        //查询最新三天按钮
        private void simpleButton3_click(object sender, EventArgs e)
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