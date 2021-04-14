using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using VLog;
using DataBase;
using LY_SINTER.Model;
using System.IO;
using LY_SINTER.Popover.Analysis;
using LY_SINTER.Custom;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace LY_SINTER.PAGE.Analysis
{
    public partial class Ripe_analysis : UserControl
    {
        #region 曲线声明

        private PlotModel _myPlotModel;//曲线容器声明
        private DateTimeAxis _dateAxis;//X轴 若存在多x轴则声明多个
        private LinearAxis _valueAxis1;//Y轴 若存在多y轴则声明多个
        private List<DataPoint> Line1 = new List<DataPoint>();//曲线数据容器
        private OxyPlot.Series.LineSeries series1;//声明曲线对象

        /// <summary>
        /// 初始化颜色变化定时器
        /// </summary>
        public System.Timers.Timer _Timer1 { get; set; }

        /// <summary>
        /// 曲线数据
        /// key：下拉框名称
        /// values item1：数据 item2：时间
        /// </summary>
        private Dictionary<string, List<Tuple<double, DateTime>>> _Dic_curve = new Dictionary<string, List<Tuple<double, DateTime>>>();

        #endregion 曲线声明

        #region 柱形图声明

        //DateTime time_Begin = DateTime.Now.AddMonths(-1);//开始时间
        //DateTime time_End = DateTime.Now;//结束时间
        private string[] _Bar_1 = { "R±0.05%", "R±0.08%", "FeO±1%", "MgO±0.15%" };               //烧结矿成分稳定率曲线

        private string[] _Bar_2 = { "σTFe", "σFeO", "σCaO", "σSiO2", "σMgO", "σR", "σAl2O3" };//标准偏差曲线

        private string[] _Bar_3 = { "<5mm", "5-10mm", "10-16mm", "16-25mm", "25-40mm", ">40mm" };//粒度组成

        #endregion 柱形图声明

        private ANALYSIS_MODEL aNALYSIS_MODEL = new ANALYSIS_MODEL();
        public vLog _vLog { get; set; }
        private DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);

        private string[] _datatable_name =  {"ID","BATCH_NUM"  ,
"MIXEDTIME"   ,
"SAMPLETIME"  ,
"TIMESTAMP"   ,
"SHIFT_FLAG"  ,
"C_TFE"   ,
"C_FEO"   ,
"C_CAO"   ,
"C_SIO2"  ,
"C_AL2O3" ,
"C_MGO"   ,
"C_S" ,
"C_P2O5"  ,
"C_R" ,
"C_K2O"   ,
"C_NA2O"  ,
"C_PB"    ,
"C_ZN"    ,
"C_MNO"   ,
"C_TIO2"  ,
"C_AS"    ,
"C_CU"    ,
"C_K" ,
"C_V2O5"  ,
"C_AL2O3_SIO2"    ,
"C_MGO_AL2O3" ,
"C_TI"    ,
"RI"  ,
"RDI_0_5" ,
"RDI_3_15"    ,
"RDI_6_3" ,
"SCRENING"    ,
"GRIT_5"  ,
"GRIT_5_10"   ,
"GRIT_10_16"  ,
"GRIT_16_25"  ,
"GRIT_25_40"  ,
"GRIT_40" ,
"PATCL_AV"    ,
"GRIT_10" ,
"GRIT_10_40"  ,
"FLAG_R_003"  ,
"FLAG_R_005"  ,
"FLAG_R_008"  ,
"FLAG_FEO_06" ,
"FLAG_FEO_1"  ,
"FLAG_MGO_015"    ,
"FLAG_GRIT_10"    ,
"R_AIM"   ,
"FeO_AIM" ,
"MgO_AIM" ,
"GRIT_10_AIM" ,
 };

        private string[] _combox_name = {"C_TFE",
"C_FEO",
"C_CAO",
"C_SIO2",
"C_AI2O3",
"C_MGO",
"C_S",
"C_P2O5",
"C_R",
"C_K2O",
"C_NA2O",
"C_PB",
"C_ZN",
"C_MNO",
"C_TIO2",
"C_AS",
"C_CU",
"C_K",
"C_V205"
 };

        private Dictionary<string, string> _v = new Dictionary<string, string>();

        public Ripe_analysis()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Analysis\\Ripe_analysis_Page\\");
            comboBox1_data();//查询种类下拉框赋值
            dateTimePicker_value();//时间赋值
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            Comboc_2_Value();//成分下拉框赋值
            Time_Now();//调整时间
            data_show("3#烧结机", textBox_begin.Text.ToString(), textBox_end.Text.ToString());//表单查询
                                                                                           //   comboBox_values();//成分下拉框赋值

            Curve_Bar();//烧结矿成分稳定率柱形图
            Curve_Bar_1();//标准偏差柱形图
            Curve_Bar_2();//粒度
            _Timer1 = new System.Timers.Timer(500);//初始化颜色变化定时器响应事件
            _Timer1.Elapsed += (x, y) => { _Timer1_Tick(); };
            _Timer1.Enabled = true;
            _Timer1.AutoReset = false;////每到指定时间Elapsed事件是触发一次（false），还是一直触发（true）
        }

        /// <summary>
        /// 最新调整时间
        /// </summary>
        public void Time_Now()
        {
            try
            {
                string sql1 = "select top (1) TIMESTAMP from MC_NUMCAL_INTERFACE_6 order by TIMESTAMP desc";
                DataTable dataTable1 = _dBSQL.GetCommand(sql1);
                if (dataTable1.Rows.Count > 0)
                {
                    label2.Text = "最新调整时间:" + dataTable1.Rows[0][0].ToString();
                }
                else
                {
                    label2.Text = "";
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("Time_Now方法失败" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 查询下拉框赋值
        /// </summary>
        public void comboBox1_data()
        {
            DataTable _data = aNALYSIS_MODEL._get_Data();
            this.comboBox1.DataSource = _data;
            this.comboBox1.DisplayMember = "Name";
            this.comboBox1.ValueMember = "Values";
            this.comboBox1.SelectedIndex = 0;
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
        /// 成分下拉框声明
        /// </summary>
        public void Comboc_2_Value()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Name");
            table.Columns.Add("Value");
            List<string> list = new List<string> {
            "TFe",
            "FeO",
            "CaO",
            "SiO2",
            "Al2O3",
            "MgO",
            "S",
            "P",
            "碱度(R)",
            "K2O",
            "Na2O",
            "PbO",
            "Zn",
            "TIO2",
            "As",
            "Cu",
            "K",
            "MNO" };
            List<string> list1 = new List<string>{"C_TFE",
"C_FEO",
"C_CAO",
"C_SIO2",
"C_AI2O3",
"C_MGO",
"C_S",
"C_P2O5",
"C_R",
"C_K2O",
"C_NA2O",
"C_PB",
"C_ZN",
"C_TIO2",
"C_AS",
"C_CU",
"C_K",
"C_MNO",
 };
            for (int x = 0; x < list.Count; x++)
            {
                _v.Add(list[x], list1[x]);
            }
            for (int x = 0; x < list.Count; x++)
            {
                DataRow row = table.NewRow();
                row["Name"] = list[x];
                row["Value"] = x;
                table.Rows.Add(row);
            }
            this.comboBox2.DataSource = table;
            this.comboBox2.DisplayMember = "Name";
            this.comboBox2.ValueMember = "Value";
            this.comboBox2.SelectedIndex = 2;
        }

        /// <summary>
        /// 表单数据显示
        /// _name:烧结机号
        /// _time_begin：开始时间
        /// _time_end：结束时间
        /// </summary>
        public void data_show(string _name, string _time_begin, string _time_end)
        {
            try
            {
                //var _sql = "select  (CASE WHEN DL_FLAG = '1' THEN '1#烧结机' WHEN DL_FLAG = '2' THEN '2#烧结机' WHEN DL_FLAG = '3' THEN '3#烧结机' else '未知' end) AS DL_FLAG,MAT_NAME,MIXEDTIME,SAMPLETIME,REOPTTIME,BATCH_NUM,HEAP_NUM,SAMPLE_TEST_NUM,SHIFT_FLAG,C_TFE as TFE,C_FEO as FEO,C_CAO as CAO,C_SIO2 as SIO2,C_AL2O3 as AL2O3,C_MGO as MGO,C_S as S,C_P2O5 as P2O5,C_R as R,C_K2O as K2O,C_NA2O as NA2O,C_PB as PB,C_ZN as ZN,C_MNO as MNO,C_TIO2 as TIO2,C_AS as 'AS',C_CU as CU,C_K as K,C_V2O5 as V2O5,C_AL2O3_SIO2,C_MGO_AL2O3,C_TOTAL,C_TI,RI,RDI_0_5,RDI_3_15,RDI_6_3,SCRENING,GRIT_5,GRIT_5_10,GRIT_10_16,GRIT_16_25,GRIT_25_40,GRIT_40,PATCL_AV,GRIT_10,GRIT_10_40,FLAG_R_005,FLAG_R_008,FLAG_FEO_06,FLAG_FEO_1,FLAG_MGO_015,FLAG_GRIT_10,FLAG_TOL_CON,R_AIM,FeO_AIM,MgO_AIM,GRIT_10_AIM,TOL_CON_AIM,TIMESTAMP from MC_NUMCAL_INTERFACE_6";\
                var _sql = "select  BATCH_NUM	,MIXEDTIME	,SAMPLETIME	,TIMESTAMP	,SHIFT_FLAG	,C_TFE	,C_FEO	,C_CAO	,C_SIO2	,C_AL2O3	,C_MGO	,C_S	,C_P2O5	,C_R	,C_K2O	,C_NA2O	,C_PB	,C_ZN	,C_MNO	,C_TIO2,C_AS,C_CU	,C_K	,C_V2O5	,C_AL2O3_SIO2	,C_MGO_AL2O3	,C_TI	,RI	,RDI_0_5	,RDI_3_15	,RDI_6_3	,SCRENING	,GRIT_5	,GRIT_5_10	,GRIT_10_16,GRIT_16_25,GRIT_25_40,GRIT_40,PATCL_AV,GRIT_10	,GRIT_10_40	,FLAG_R_003	,FLAG_R_005	,FLAG_R_008	,FLAG_FEO_06	,FLAG_FEO_1	,FLAG_MGO_015	,FLAG_GRIT_10,R_AIM,FeO_AIM,MgO_AIM,GRIT_10_AIM from MC_NUMCAL_INTERFACE_6 ";

                if (_name == "1#烧结机")
                {
                    _sql += " where DL_FLAG = 1 and TIMESTAMP >= '" + _time_begin + "' and TIMESTAMP <= '" + _time_end + "' order by TIMESTAMP asc";
                }
                else if (_name == "2#烧结机")
                {
                    _sql += " where DL_FLAG = 2 and TIMESTAMP >= '" + _time_begin + "' and TIMESTAMP <= '" + _time_end + "' order by TIMESTAMP asc";
                }
                else if (_name == "3#烧结机")
                {
                    _sql += " where DL_FLAG = 3 and TIMESTAMP >= '" + _time_begin + "' and TIMESTAMP <= '" + _time_end + "' order by TIMESTAMP asc";
                }
                else
                {
                    _sql += " where  TIMESTAMP >= '" + _time_begin + "' and TIMESTAMP <= '" + _time_end + "' order by TIMESTAMP asc";
                }
                DataTable _table = _dBSQL.GetCommand(_sql);//基础计算数据
                if (_table != null && _table.Rows.Count > 0)
                {
                    DataTable _data = new DataTable();//表单绑定数据
                    for (int x = 0; x < _datatable_name.Count(); x++)
                    {
                        _data.Columns.Add(_datatable_name[x]);
                    }

                    #region 计算值

                    DataRow row_1 = _data.NewRow();//平均
                    DataRow row_2 = _data.NewRow();//标准偏差
                    DataRow row_3 = _data.NewRow();//最大值
                    DataRow row_4 = _data.NewRow();//最小值
                    DataRow row_5 = _data.NewRow();//极差值
                    row_1[0] = "平均";
                    row_2[0] = "标准偏差";
                    row_3[0] = "最大值";
                    row_4[0] = "最小值";
                    row_5[0] = "极差";
                    for (int x = 1; x < 6; x++)
                    {
                        row_1[_datatable_name[x]] = "-";
                        row_2[_datatable_name[x]] = "-";
                        row_3[_datatable_name[x]] = "-";
                        row_4[_datatable_name[x]] = "-";
                        row_5[_datatable_name[x]] = "-";
                    }
                    for (int x = 6; x < _data.Columns.Count; x++)
                    {
                        List<float> _list = new List<float>();
                        for (int y = 0; y < _table.Rows.Count; y++)
                        {
                            // double xx  = double.Parse(_table.Rows[y][_datatable_name[x]].ToString());
                            if (_table.Rows[y][_datatable_name[x]].ToString() != "" && double.Parse(_table.Rows[y][_datatable_name[x]].ToString()) != 0)
                                _list.Add(float.Parse(_table.Rows[y][_datatable_name[x]].ToString() == "" ? "0" : _table.Rows[y][_datatable_name[x]].ToString()));
                        }
                        row_1[_datatable_name[x]] = aNALYSIS_MODEL.Computer_MODEL(_list, 1, 2).ToString();//平均
                        row_2[_datatable_name[x]] = aNALYSIS_MODEL.Computer_MODEL(_list, 2, 2).ToString();//标准偏差
                        row_3[_datatable_name[x]] = aNALYSIS_MODEL.Computer_MODEL(_list, 3, 2).ToString();//最大值
                        row_4[_datatable_name[x]] = aNALYSIS_MODEL.Computer_MODEL(_list, 4, 2).ToString();//最小值
                        row_5[_datatable_name[x]] = aNALYSIS_MODEL.Computer_MODEL(_list, 5, 2).ToString();//极差值
                    }
                    _data.Rows.Add(row_1);
                    _data.Rows.Add(row_2);
                    _data.Rows.Add(row_3);
                    _data.Rows.Add(row_4);
                    _data.Rows.Add(row_5);

                    #endregion 计算值

                    #region 绑定基础数据

                    _Dic_curve.Clear();
                    for (int x = 0; x < _table.Rows.Count; x++)
                    {
                        DataRow row_now = _data.NewRow();
                        row_now["ID"] = (x + 1).ToString();
                        for (int y = 1; y < _datatable_name.Count(); y++)
                        {
                            row_now[_datatable_name[y]] = _table.Rows[x][_datatable_name[y]].ToString();
                            if (_combox_name.Contains(_datatable_name[y]))
                            {
                                if (_Dic_curve.ContainsKey(_datatable_name[y]))
                                {
                                    _Dic_curve[_datatable_name[y]].Add(new Tuple<double, DateTime>(float.Parse(_table.Rows[x][_datatable_name[y]].ToString() == "" ? "0" : _table.Rows[x][_datatable_name[y]].ToString()), DateTime.Parse(_table.Rows[x]["TIMESTAMP"].ToString())));
                                }
                                else
                                {
                                    List<Tuple<double, DateTime>> _list = new List<Tuple<double, DateTime>>();
                                    _list.Add(new Tuple<double, DateTime>(float.Parse(_table.Rows[x][_datatable_name[y]].ToString() == "" ? "0" : _table.Rows[x][_datatable_name[y]].ToString()), DateTime.Parse(_table.Rows[x]["TIMESTAMP"].ToString())));
                                    _Dic_curve.Add(_datatable_name[y], _list);
                                }
                            }
                        }
                        _data.Rows.Add(row_now);
                    }
                    this.dataGridView1.DataSource = _data;//绑定数据源

                    #endregion 绑定基础数据
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("data_show方法失败" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 初始化颜色变化定时器响应事件
        /// </summary>
        private void _Timer1_Tick()
        {
            Action invokeAction = new Action(_Timer1_Tick);
            if (this.InvokeRequired)
            {
                this.Invoke(invokeAction);
            }
            else
            {
                Curve_text(_v[comboBox2.Text.ToString()]);//曲线绑定数据源
                this.dataGridView1.Rows[4].Frozen = true;
            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            data_show(comboBox1.Text.ToString(), textBox_begin.Text.ToString(), textBox_end.Text.ToString());//表单数据查询
            Curve_text(comboBox2.Text.ToString());//曲线赋值
            Curve_Bar();//烧结矿成分稳定率柱形图
            Curve_Bar_1();//标准偏差柱形图
            Curve_Bar_2();
            _Timer1.Enabled = true;
        }

        /// <summary>
        /// 实时按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            dateTimePicker_value();//时间还原
            data_show(comboBox1.Text.ToString(), textBox_begin.Text.ToString(), textBox_end.Text.ToString());//表单数据查询
            _Timer1.Enabled = true;
        }

        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton7_Click(object sender, EventArgs e)
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
                    MessageBox.Show(ee.ToString());
                }
                finally
                {
                    sw.Close();
                    myStream.Close();
                }
            }
        }

        /// <summary>
        /// 参数查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Frm_Ripe_analysis_PAR form_display = new Frm_Ripe_analysis_PAR();
            if (Frm_Ripe_analysis_PAR.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 班烧结炉弹出框按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Frm_Ripe_analysis_CLASS form_display = new Frm_Ripe_analysis_CLASS();
            if (Frm_Ripe_analysis_CLASS.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 堆烧结炉弹出框按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            Frm_Ripe_analysis_HEAP form_display = new Frm_Ripe_analysis_HEAP();
            if (Frm_Ripe_analysis_HEAP.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            Frm_Ripe_analysis_Mon form_display = new Frm_Ripe_analysis_Mon();
            if (Frm_Ripe_analysis_Mon.isopen == false)
            {
                form_display.ShowDialog();
            }
            else
            {
                form_display.Activate();
            }
        }

        /// <summary>
        /// 定时器启用
        /// </summary>
        public void Timer_state()
        {
            //  _Timer1.Enabled = true;
        }

        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {
            //  _Timer1.Enabled = false;
        }

        /// <summary>
        /// 控件关闭
        /// </summary>
        public void _Clear()
        {
            //  _Timer1.Close();//释放定时器资源
            this.Dispose();//释放资源
            GC.Collect();//调用GC
        }

        /// <summary>
        /// 成分下拉框赋值
        /// </summary>
        public void comboBox_values()
        {
            DataTable _data = new DataTable();
            _data.Columns.Add("Name");
            _data.Columns.Add("Values");
            for (int x = 0; x < _combox_name.Count(); x++)
            {
                DataRow row_now = _data.NewRow();
                row_now["Name"] = _combox_name[x];
                row_now["Values"] = x + 1;
                _data.Rows.Add(row_now);
            }
            comboBox2.DataSource = _data;
            this.comboBox2.DisplayMember = "Name";
            this.comboBox2.ValueMember = "Values";
        }

        /// <summary>
        /// 曲线数据绑定
        /// _name成分名称
        /// </summary>
        /// <param name="_name"></param>
        public void Curve_text(string _name)
        {
            try
            {
                if (_Dic_curve.ContainsKey(_name))//判断是否存在key值
                {
                    List<Tuple<double, DateTime>> _list = _Dic_curve[_name];//获取数据
                    Line1.Clear();//清除曲线数据
                    List<double> Mun1 = new List<double>();//判断极值

                    _myPlotModel = new PlotModel() //定义model
                    {
                        Background = OxyColors.White,//背景颜色
                        Title = "历史",
                        TitleFontSize = 7,
                        TitleColor = OxyColors.White,
                        //LegendMargin = 100,
                    };
                    _dateAxis = new DateTimeAxis()//X轴
                    {
                        MajorGridlineStyle = LineStyle.Solid,
                        MinorGridlineStyle = LineStyle.Dot,
                        IntervalLength = 100,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        AxisTickToLabelDistance = 0,
                        FontSize = 9.0,
                    };
                    _myPlotModel.Axes.Add(_dateAxis);//添加x轴

                    for (int x = 0; x < _list.Count; x++)//数据赋值
                    {
                        // DataPoint（x:时间  y：数据）
                        DataPoint line1 = new DataPoint(DateTimeAxis.ToDouble(_list[x].Item2), Math.Round(_list[x].Item1, 2));
                        Line1.Add(line1);
                        Mun1.Add(Convert.ToDouble(_list[x].Item1));
                    }
                    int x_1 = 1;
                    if ((int)((Mun1.Max() - Mun1.Min()) / 5) > 0)
                        x_1 = (int)((Mun1.Max() - Mun1.Min()) / 5);

                    _valueAxis1 = new LinearAxis()//声明曲线
                    {
                        Key = "X",//y轴标识符，多个y轴需要标识多个标识符，唯一性
                        MajorGridlineStyle = LineStyle.None,
                        MinorGridlineStyle = LineStyle.None,
                        IntervalLength = 80,
                        Angle = 60,
                        IsZoomEnabled = true,
                        IsPanEnabled = false,
                        Maximum = (int)(Mun1.Max() + 1),//极值
                        Minimum = (int)(Mun1.Min() - 1),//极值
                        PositionTier = 1,
                        AxislineStyle = LineStyle.Solid,
                        AxislineColor = OxyColors.Red,//颜色
                        MinorTicklineColor = OxyColors.Red,//颜色
                        TicklineColor = OxyColors.Red,//颜色
                        TextColor = OxyColors.Red,//颜色
                        FontSize = 9.0,
                        IsAxisVisible = false,
                        MajorStep = x_1,//增长数据
                        //  MajorStep = 50,
                        MinorTickSize = 0,
                    };
                    _myPlotModel.Axes.Add(_valueAxis1);//添加曲线

                    series1 = new OxyPlot.Series.LineSeries()//添加曲线
                    {
                        Color = OxyColors.Purple,//颜色
                        StrokeThickness = 1,
                        MarkerSize = 3,
                        MarkerStroke = OxyColors.Purple,//颜色
                        MarkerType = MarkerType.None,
                        YAxisKey = "X",//y轴标识符，多个y轴需要标识多个标识符，唯一性
                        ItemsSource = Line1,//绑定数据
                        TrackerFormatString = "{0}\n时间:{2:HH:mm:ss}    " + _name + ":{4}",
                    };
                    _valueAxis1.IsAxisVisible = true;
                    _myPlotModel.Series.Add(series1);
                    plotView1.Model = _myPlotModel;
                    var PlotController = new OxyPlot.PlotController();
                    PlotController.BindMouseEnter(PlotCommands.HoverPointsOnlyTrack);
                    plotView1.Controller = PlotController;
                }
            }
            catch (Exception ee)
            {
                _vLog.writelog("Curve_text方法" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 烧结矿稳定率柱形图
        /// </summary>
        public void Curve_Bar()
        {
            try
            {
                PlotModel _myPlotModel = new PlotModel();
                //X轴定义
                CategoryAxis _categoryAxis = new CategoryAxis()
                {
                    MajorTickSize = 0,
                    IsZoomEnabled = false,
                    Position = AxisPosition.Bottom,
                    GapWidth = 3
                };
                for (int i = 0; i < _Bar_1.Count(); i++)
                {
                    _categoryAxis.Labels.Add(_Bar_1[i]);     //添加x坐标
                }
                _myPlotModel.Axes.Add(_categoryAxis);
                //Y轴定义
                LinearAxis _valueAxis = new LinearAxis()
                {
                    MinorTickSize = 0,
                    Key = "y",
                    Maximum = 100
                };
                _myPlotModel.Axes.Add(_valueAxis);
                var _ColumnSeries = new ColumnSeries();
                List<double> _Value_Bar_1 = new List<double>();
                var _sql = "select count(TIMESTAMP) as _Count, sum(FLAG_R_005) as FLAG_R_005,sum(FLAG_R_008) as FLAG_R_008,sum(FLAG_FEO_1) as FLAG_FEO_1,sum(FLAG_MGO_015) as FLAG_MGO_015 from MC_NUMCAL_INTERFACE_6 where DL_FLAG=3 and FLAG_R_005 is not null and TIMESTAMP between '" + textBox_begin.Text.ToString() + "' and '" + textBox_end.Text.ToString() + "'";
                DataTable _data = _dBSQL.GetCommand(_sql);
                if (_data.Rows.Count > 0 && _data != null)
                {
                    for (int x = 1; x < _data.Columns.Count; x++)
                    {
                        _Value_Bar_1.Add(Math.Round(double.Parse(_data.Rows[0][x].ToString()) / double.Parse(_data.Rows[0][0].ToString()) * 100, 2));
                    }
                }
                for (int i = 0; i < _Value_Bar_1.Count; i++)
                {
                    _ColumnSeries.Items.Add(new ColumnItem() { Value = _Value_Bar_1[i] });
                }
                _myPlotModel.Series.Add(_ColumnSeries);
                plotView4.Model = _myPlotModel;
            }
            catch (Exception ee)
            {
                _vLog.writelog("Curve_Bar方法调用失败" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 粒度组成
        /// </summary>
        public void Curve_Bar_2()
        {
            try
            {
                PlotModel _myPlotMode2 = new PlotModel();
                //X轴定义
                CategoryAxis _categoryAxis = new CategoryAxis()
                {
                    MajorTickSize = 0,
                    IsZoomEnabled = false,
                    Position = AxisPosition.Bottom,
                    GapWidth = 3
                };
                for (int i = 0; i < _Bar_3.Count(); i++)
                {
                    _categoryAxis.Labels.Add(_Bar_3[i]);     //添加x坐标
                }
                _myPlotMode2.Axes.Add(_categoryAxis);
                //Y轴定义
                LinearAxis _valueAxis = new LinearAxis()
                {
                    MinorTickSize = 0,
                    Key = "y",
                    Minimum = 0,
                    Maximum = 1
                };
                _myPlotMode2.Axes.Add(_valueAxis);
                var _ColumnSeries = new ColumnSeries();
                List<double> _Value_Bar_1 = new List<double>();

                if (this.dataGridView1.Rows.Count > 5)
                {
                    // string[] _Bar_2 = { "σTFe", "σFeO", "σCaO", "σSiO2", "σMgO", "σR", "σAl2O3" };//标准偏差曲线
                    //string a = this.dataGridView1.Rows[1].Cells["Column9"].Value.ToString();
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[0].Cells["Column37"].Value.ToString()), 2));//TFe
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[0].Cells["Column38"].Value.ToString()), 2));//FeO
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[0].Cells["Column39"].Value.ToString()), 2));//CaO
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[0].Cells["Column40"].Value.ToString()), 2));//SiO2
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[0].Cells["Column41"].Value.ToString()), 2));//MgO
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[0].Cells["Column42"].Value.ToString()), 2));//R
                }
                else
                {
                    _Value_Bar_1.Add(0);
                    _Value_Bar_1.Add(0);
                    _Value_Bar_1.Add(0);
                    _Value_Bar_1.Add(0);
                    _Value_Bar_1.Add(0);
                    _Value_Bar_1.Add(0);
                    // _Value_Bar_1.Add(0);
                }
                for (int i = 0; i < _Value_Bar_1.Count; i++)
                {
                    _ColumnSeries.Items.Add(new ColumnItem() { Value = _Value_Bar_1[i] });
                }
                _myPlotMode2.Series.Add(_ColumnSeries);
                plotView3.Model = _myPlotMode2;
            }
            catch (Exception ee)
            {
                _vLog.writelog("Curve_Bar_1方法失败" + ee.ToString(), -1);
            }
        }

        public void Curve_Bar_1()
        {
            try
            {
                PlotModel _myPlotModel = new PlotModel();
                //X轴定义
                CategoryAxis _categoryAxis = new CategoryAxis()
                {
                    MajorTickSize = 0,
                    IsZoomEnabled = false,
                    Position = AxisPosition.Bottom,
                    GapWidth = 3
                };
                for (int i = 0; i < _Bar_2.Count(); i++)
                {
                    _categoryAxis.Labels.Add(_Bar_2[i]);     //添加x坐标
                }
                _myPlotModel.Axes.Add(_categoryAxis);
                //Y轴定义
                LinearAxis _valueAxis = new LinearAxis()
                {
                    MinorTickSize = 0,
                    Key = "y",
                    Minimum = 0,
                    Maximum = 1
                };
                _myPlotModel.Axes.Add(_valueAxis);
                var _ColumnSeries = new ColumnSeries();
                List<double> _Value_Bar_1 = new List<double>();

                if (this.dataGridView1.Rows.Count > 5)
                {
                    // string[] _Bar_2 = { "σTFe", "σFeO", "σCaO", "σSiO2", "σMgO", "σR", "σAl2O3" };//标准偏差曲线
                    string a = this.dataGridView1.Rows[1].Cells["Column9"].Value.ToString();
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[1].Cells["Column9"].Value.ToString()), 2));//TFe
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[1].Cells["Column10"].Value.ToString()), 2));//FeO
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[1].Cells["Column11"].Value.ToString()), 2));//CaO
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[1].Cells["Column12"].Value.ToString()), 2));//SiO2
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[1].Cells["Column14"].Value.ToString()), 2));//MgO
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[1].Cells["Column6"].Value.ToString()), 2));//R
                    _Value_Bar_1.Add(Math.Round(double.Parse(this.dataGridView1.Rows[1].Cells["Column13"].Value.ToString()), 2));//Al2O3
                }
                else
                {
                    _Value_Bar_1.Add(0);
                    _Value_Bar_1.Add(0);
                    _Value_Bar_1.Add(0);
                    _Value_Bar_1.Add(0);
                    _Value_Bar_1.Add(0);
                    _Value_Bar_1.Add(0);
                    _Value_Bar_1.Add(0);
                }
                for (int i = 0; i < _Value_Bar_1.Count; i++)
                {
                    _ColumnSeries.Items.Add(new ColumnItem() { Value = _Value_Bar_1[i] });
                }
                _myPlotModel.Series.Add(_ColumnSeries);
                plotView2.Model = _myPlotModel;
            }
            catch (Exception ee)
            {
                _vLog.writelog("Curve_Bar_1方法失败" + ee.ToString(), -1);
            }
        }

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            _Timer1.Enabled = true;
        }
    }
}