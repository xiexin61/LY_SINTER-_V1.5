using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLog;
using LY_SINTER.Model;
using DataBase;
using System.IO;
using LY_SINTER.Custom;
using OxyPlot;
using OxyPlot.Axes;

namespace LY_SINTER.PAGE.Analysis
{
    public partial class Raw_analysis : UserControl
    {
        /// <summary>
        /// 小数位数
        /// </summary>
        int Digit = 2;
        DBSQL _dBSQL = new DBSQL(ConstParameters.strCon);
        /// <summary>
        ///存放物料种类规则
        ///key:物料描述
        /// values:item1:二级编码最小值、item2:二级编码最大值;、item3:物料归属标志位
        /// </summary>
        Dictionary<string, List<Tuple<int, int, int>>> GetDic_L2_code_CONFIG = new Dictionary<string, List<Tuple<int, int, int>>>();
        /// <summary>
        /// 物料及二级编码对应关系
        /// key:物料描述
        /// values：二级编码
        /// </summary>
        Dictionary<String, int> _dic_L2_CODE_CLASS = new Dictionary<string, int>();
        MIX_PAGE_MODEL mIX_PAGE = new MIX_PAGE_MODEL();//配料页面方法
        ANALYSIS_MODEL aNALYSIS_MODEL = new ANALYSIS_MODEL();//数据分析页面模型
        /// <summary>
        /// 数据库字段
        /// </summary>
        string[] Get_data_name = {"ID","REOPTTIME",
                                    "BATCH_NUM",
                                    "L2_CODE",
                                    "C_TFE",
                                    "C_FEO",
                                    "C_CAO",
                                    "C_SIO2",
                                    "C_AL2O3",
                                    "C_MGO",
                                    "C_S",
                                    "C_P",
                                    "C_C",
                                    "C_MN",
                                    "C_LOT",
                                    "C_R",
                                    "C_H2O",
                                    "C_ASH",
                                    "C_VOLATILES",
                                    "C_TIO2",
                                    "C_K2O",
                                    "C_NA2O",
                                    "C_PBO",
                                    "C_ZNO",
                                    "C_F",
                                    "C_AS",
                                    "C_CU",
                                    "C_PB",
                                    "C_ZN",
                                    "C_K",
                                    "C_NA",
                                    "C_CR",
                                    "C_NI",
                                    "C_MNO"
                                     };
        /// <summary>
        /// 曲线下拉框选项赋值
        /// </summary>
        string[] _get_combox_values =
        {
 "TFe",
"FeO",
"CaO",
"SiO2",
"Al2O3",
"MgO",
"S",
"P",
"C",
"Mn",
"烧损",
"碱度",
"H2O",
"灰分",
"挥发分",
"TiO2",
"K2O",
"Na2O",
"PbO",
"ZnO",
"F",
"As",
"Cu",
"Pb",
"Zn",
"K",
"Na",
"Cr",
"Ni",
"MNO"
};
        #region 曲线声明
        private PlotModel _myPlotModel;//曲线容器声明
        private DateTimeAxis _dateAxis;//X轴 若存在多x轴则声明多个
        private LinearAxis _valueAxis1;//Y轴 若存在多y轴则声明多个
        List<DataPoint> Line1 = new List<DataPoint>();//曲线数据容器
        private OxyPlot.Series.LineSeries series1;//声明曲线对象
        /// <summary>
        /// 曲线数据
        /// key：下拉框名称
        /// values item1：数据 item2：时间
        /// </summary>
        Dictionary<string, List<Tuple<double, DateTime>>> _Dic_curve = new Dictionary<string, List<Tuple<double, DateTime>>>();
        #endregion
        public vLog _vLog { get; set; }
        public Raw_analysis()
        {
            InitializeComponent();
            if (_vLog == null)//声明日志
                _vLog = new vLog(".\\log_Page\\Analysis\\Raw_analysis_Page\\");
            dateTimePicker_value();
            DateTimeChoser.AddTo(textBox_begin);
            DateTimeChoser.AddTo(textBox_end);
            Combox_1_date();//物料类别下拉框绑定
           // combox_3_values();
            data_text("混匀矿", textBox_begin.Text.ToString(), textBox_end.Text.ToString());
            _combox2_values();
            Curve_text(comboBox2.Text.ToString());
        }
        /// <summary>
        /// 开始时间&结束时间赋值
        /// </summary>
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
                _vLog.writelog("dateTimePicker_value方法失败"+ee.ToString(),-1);
            }
        }
        /// <summary>
        /// 物料类别下拉框绑定
        /// </summary>
        public void Combox_1_date()
        {
            try
            {
                GetDic_L2_code_CONFIG = mIX_PAGE._DIC_L2_CODE_CONFIG_1();
                DataTable _data = new DataTable();
                _data.Columns.Add("Name");
                _data.Columns.Add("Values");
                int X = 0;
                foreach(var x in GetDic_L2_code_CONFIG)
                {
                    DataRow row_now = _data.NewRow();
                    row_now["Name"] = x.Key;
                    row_now["Values"] = X;
                    _data.Rows.Add(row_now);
                    X = X + 1;
                }
                this.comboBox1.DataSource = _data;
                this.comboBox1.DisplayMember = "Name";
                this.comboBox1.ValueMember = "Values";
                this.comboBox1.SelectedIndex = 2;
            }
            catch(Exception ee)
            {
                _vLog.writelog("Combox_1_date方法失败" + ee.ToString(),-1);
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            combox_3_values();
        }
        /// <summary>
        /// 料名数据绑定事件
        /// </summary>
        public void combox_3_values()
        {
            try
            {
                string _NAME = comboBox1.Text.ToString();//物料种类归属名称
                if (GetDic_L2_code_CONFIG.ContainsKey(_NAME))
                {
                    var _sql = "select L2_CODE, MAT_DESC  from M_MATERIAL_COOD where ";//拼接字符串
                    List<Tuple<int, int, int>> _list = GetDic_L2_code_CONFIG[_NAME];
                    for (int x = 0; x < _list.Count; x++)
                    {
                        if (x == 0)
                        {
                            _sql += "L2_CODE between " + _list[x].Item1 + " and " + _list[x].Item2 + "";
                        }
                        else
                        {
                            _sql += " or L2_CODE between " + _list[x].Item1 + " and " + _list[x].Item2 + "";
                        }

                    }
                    _sql += " order by  L2_CODE asc";
                    DataTable data_Value = new DataTable();
                    data_Value.Columns.Add("Name");
                    data_Value.Columns.Add("Value");
                    DataTable _data = _dBSQL.GetCommand(_sql);
                    if (_data.Rows.Count > 0 && _data != null)
                    {
                        _dic_L2_CODE_CLASS.Clear();
                        for (int x = 0; x < _data.Rows.Count; x++)
                        {
                            //物料描述
                            var _name = _data.Rows[x]["MAT_DESC"].ToString();
                            var _l2_code = int.Parse(_data.Rows[x]["L2_CODE"].ToString());
                            //二级编码
                            if (!_dic_L2_CODE_CLASS.ContainsKey(_name))
                                _dic_L2_CODE_CLASS.Add(_name, _l2_code);
                            DataRow row_now = data_Value.NewRow();
                            row_now["Name"] = _name;
                            row_now["Value"] = x;
                            data_Value.Rows.Add(row_now);
                        }
                        this.comboBox3.DataSource = data_Value;
                        this.comboBox3.DisplayMember = "Name";
                        this.comboBox3.ValueMember = "Value";
                        this.comboBox3.SelectedIndex = 0;
                        string _n1 = comboBox3.Text.ToString();
                    }
                }

            }
            catch (Exception ee)
            {
                _vLog.writelog("物料种类下拉框赋值响应时间失败" + ee.ToString(), -1);
            }
        }

        /// <summary>
        /// 数据查询
        /// _name :物料名称
        /// _time_begin 开始时间
        /// _time_end 结束时间
        /// </summary>
        public void data_text(string _name,string _time_begin,string _time_end)
        {
            try
            {
               var _sql = "select TIMESTAMP,REOPTTIME,BATCH_NUM,L2_CODE,C_TFE,C_FEO,C_CAO,C_SIO2,C_AL2O3,C_MGO,C_S,C_P,C_C,C_MN,C_LOT,C_R,C_H2O,C_ASH,C_VOLATILES,C_TIO2,C_K2O,C_NA2O,C_PBO,C_ZNO,C_F,C_AS,C_CU,C_PB,C_ZN,C_K,C_NA,C_CR,C_NI,C_MNO from M_MATERIAL_ANALYSIS WHERE L2_CODE='" + _dic_L2_CODE_CLASS[_name] + "' and TIMESTAMP between '" + _time_begin + "' and '" + _time_end + "' order by TIMESTAMP desc";
                DataTable table_1 = _dBSQL.GetCommand(_sql);//参与计算数据
                if (table_1 != null && table_1.Rows.Count > 0 )
                {
                    DataTable _table = new DataTable();//表单数据源
                    for (int x = 0; x < Get_data_name.Count(); x++)
                    {
                        _table.Columns.Add(Get_data_name[x]);
                    }
                    //  平均
                    DataRow row_1 = _table.NewRow();
                    row_1[Get_data_name[0]] = "平均";//序号
                    row_1[Get_data_name[1]] = "-";//报样时间
                    row_1[Get_data_name[2]] = "-";//物料名
                    row_1[Get_data_name[3]] = "-";//试样号
                    //标准偏差
                    DataRow row_2 = _table.NewRow();
                    row_2[Get_data_name[0]] = "标准偏差";//序号
                    row_2[Get_data_name[1]] = "-";//报样时间
                    row_2[Get_data_name[2]] = "-";//物料名
                    row_2[Get_data_name[3]] = "-";//试样号
                    //最大值
                    DataRow row_3 = _table.NewRow();
                    row_3[Get_data_name[0]] = "最大值";//序号
                    row_3[Get_data_name[1]] = "-";//报样时间
                    row_3[Get_data_name[2]] = "-";//物料名
                    row_3[Get_data_name[3]] = "-";//试样号
                     //最小值
                    DataRow row_4 = _table.NewRow();
                    row_4[Get_data_name[0]] = "最小值";//序号
                    row_4[Get_data_name[1]] = "-";//报样时间
                    row_4[Get_data_name[2]] = "-";//物料名
                    row_4[Get_data_name[3]] = "-";//试样号
                    //极值
                    DataRow row_5 = _table.NewRow();
                    row_5[Get_data_name[0]] = "极值";//序号
                    row_5[Get_data_name[1]] = "-";//报样时间
                    row_5[Get_data_name[2]] = "-";//物料名
                    row_5[Get_data_name[3]] = "-";//试样号
                    //在用成分
                    DataRow row_6 = _table.NewRow();
                    row_6[Get_data_name[0]] = "在用成分";//序号
                    row_6[Get_data_name[1]] = "-";//报样时间
                    row_6[Get_data_name[2]] = "-";//物料名
                    row_6[Get_data_name[3]] = "-";//试样号
                    for (int x = 4;x < Get_data_name.Count();x++)
                    {
                        List<float> _list = new List<float>();
                        for (int y = 0; y < table_1.Rows.Count;y++)
                        {
                            _list.Add(float.Parse(table_1.Rows[y][Get_data_name[x]].ToString() == "" ? "0": table_1.Rows[y][Get_data_name[x]].ToString()));
                        }
                        row_1[Get_data_name[x]] = aNALYSIS_MODEL.Computer_MODEL(_list, 1, 2).ToString();//成分
                        row_2[Get_data_name[x]] = aNALYSIS_MODEL.Computer_MODEL(_list, 2, 2).ToString();//标准偏差
                        row_3[Get_data_name[x]] = aNALYSIS_MODEL.Computer_MODEL(_list, 3, 2).ToString();//最大值
                        row_4[Get_data_name[x]] = aNALYSIS_MODEL.Computer_MODEL(_list, 4, 2).ToString();//最小值
                        row_5[Get_data_name[x]] = aNALYSIS_MODEL.Computer_MODEL(_list, 5, 2).ToString();//极值
                    }
                    _table.Rows.Add(row_1);
                    _table.Rows.Add(row_2);
                    _table.Rows.Add(row_3);
                    _table.Rows.Add(row_4);
                    _table.Rows.Add(row_5);
                    //拼接基础数据
                    _Dic_curve.Clear();
                    for (int x = 0; x < table_1.Rows.Count;x++)
                    {
                        DataRow row_now = _table.NewRow();
                        row_now["ID"] = x + 1;
                        for(int y = 1; y < Get_data_name.Count(); y++)
                        {
                            ///数据列
                            if (y > 3)
                            {
                                if (table_1.Rows[x][Get_data_name[y]].ToString() == "")//为空
                                {
                                    row_now[Get_data_name[y]] = table_1.Rows[x][Get_data_name[y]].ToString();
                                }
                                else
                                {
                                    row_now[Get_data_name[y]] = Math.Round(double.Parse( table_1.Rows[x][Get_data_name[y]].ToString()), Digit);
                                }
                                if (_Dic_curve.ContainsKey(Get_data_name[y]))
                                {
                                    _Dic_curve[Get_data_name[y]].Add(new Tuple<double, DateTime>(Math.Round( float.Parse(table_1.Rows[x][Get_data_name[y]].ToString() == "" ? "0" : table_1.Rows[x][Get_data_name[y]].ToString()), Digit), DateTime.Parse(table_1.Rows[x]["TIMESTAMP"].ToString())));
                                }
                                else
                                {
                                    List<Tuple<double, DateTime>> _list = new List<Tuple<double, DateTime>>();
                                    _list.Add(new Tuple<double, DateTime>(Math.Round( float.Parse(table_1.Rows[x][Get_data_name[y]].ToString() == "" ? "0" : table_1.Rows[x][Get_data_name[y]].ToString()), Digit), DateTime.Parse(table_1.Rows[x]["TIMESTAMP"].ToString())));
                                    _Dic_curve.Add(Get_data_name[y], _list);
                                }
                            }
                            ///非数据列
                            else
                            {
                                row_now[Get_data_name[y]] = table_1.Rows[x][Get_data_name[y]].ToString();
                            }
                            
                        }
                        _table.Rows.Add(row_now);
                    }
                    dataGridView1.DataSource = _table;
                }
            }
            catch(Exception ee)
            {
                _vLog.writelog("data_text方法失败" + ee.ToString(),-1);
            }
        }
        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
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
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            data_text(comboBox3.Text.ToString(), textBox_begin.Text.ToString(), textBox_end.Text.ToString());
            Curve_text(comboBox2.Text.ToString());//曲线赋值
        }
        /// <summary>
        /// 成分下拉框绑定
        /// </summary>
        public void _combox2_values()
        {
            DataTable _data = new DataTable();
            _data.Columns.Add("Name");
            _data.Columns.Add("Values");
            for(int x = 0; x < _get_combox_values.Count(); x++)
            {
                DataRow row_now = _data.NewRow();
                row_now["Name"] = _get_combox_values[x];
                row_now["Values"] = x+1;
                _data.Rows.Add(row_now);
            }
            this.comboBox2.DataSource = _data;
            this.comboBox2.DisplayMember = "Name";
            this.comboBox2.ValueMember = "Values";
            this.comboBox2.SelectedIndex = 1;
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
                #region 字符转换
                string _name_1 = "";
                if (_name == "TFe")
                    _name_1 = "C_TFE";
                else if(_name == "FeO")
                    _name_1 = "C_FEO";
                else if (_name == "CaO")
                    _name_1 = "C_CAO";
                else if (_name == "SiO2")
                    _name_1 = "C_SIO2";
                else if (_name == "Al2O3")
                    _name_1 = "C_AL2O3";
                else if (_name == "MgO")
                    _name_1 = "C_MGO";
                else if (_name == "S")
                    _name_1 = "C_S";
                else if (_name == "P")
                    _name_1 = "C_P";
                else if (_name == "C")
                    _name_1 = "C_C";
                else if (_name == "Mn")
                    _name_1 = "C_MN";
                else if (_name == "烧损")
                    _name_1 = "C_LOT";
                else if (_name == "碱度")
                    _name_1 = "C_R";
                else if (_name == "H2O")
                    _name_1 = "C_H2O";
                else if (_name == "灰分")
                    _name_1 = "C_ASH";
                else if (_name == "挥发分")
                    _name_1 = "C_VOLATILES";
                else if (_name == "TiO2")
                    _name_1 = "C_TIO2";
                else if (_name == "K2O")
                    _name_1 = "C_K2O";
                else if (_name == "Na2O")
                    _name_1 = "C_NA2O";
                else if (_name == "PbO")
                    _name_1 = "C_PBO";
                else if (_name == "ZnO")
                    _name_1 = "C_ZNO";
                else if (_name == "F")
                    _name_1 = "C_F";
                else if (_name == "As")
                    _name_1 = "C_AS";
                else if (_name == "Cu")
                    _name_1 = "C_CU";
                else if (_name == "Pb")
                    _name_1 = "C_PB";
                else if (_name == "Zn")
                    _name_1 = "C_ZN";
                else if (_name == "K")
                    _name_1 = "C_K";
                else if (_name == "Na")
                    _name_1 = "C_NA";
                else if (_name == "Cr")
                    _name_1 = "C_CR";
                else if (_name == "Ni")
                    _name_1 = "C_NI";
                else if (_name == "MNO")
                    _name_1 = "C_MNO";
                #endregion

                if (_Dic_curve.ContainsKey(_name_1))//判断是否存在key值
                {
                    List<Tuple<double, DateTime>> _list = _Dic_curve[_name_1];//获取数据
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
                        TrackerFormatString = "{0}时间:{2: MM月dd日 HH:mm:ss} " + _name + ":{4}",
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

        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Curve_text(comboBox2.Text.ToString());
        }
        /// <summary>
        /// 定时器启用
        /// </summary>
        public void Timer_state()
        {
           // _Timer1.Enabled = true;
        }
        /// <summary>
        /// 定时器停用
        /// </summary>
        public void Timer_stop()
        {
           // _Timer1.Enabled = false;
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
    }
}
